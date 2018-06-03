Imports CefSharp.WinForms
Imports CefSharp
Imports System.Net

Public Class Form1

    '----------------------------------------------------------------
    'Startup Checks
    '----------------------------------------------------------------

    Private Const dgVoodooConfigFilename As String = "dgVoodoo.conf"
    Private Const LauncherConfigFilename As String = "TLOMNLauncher.ini"
    Private Const BionicleConfigFilename As String = "Bionicle.ini"

    Private Const AlphaDefaultFilename As String = "..\LEGO Bionicle\LEGO Bionicle.exe"
    Private Const BetaDefaultFilename As String = "..\LEGO Bionicle (Beta)\LEGO Bionicle.exe"

    Public Configuration As INIFile

    Public Shared Function CheckForInternetConnection() As Boolean
        Try
            Using client = New WebClient()
                Using stream = client.OpenRead("http://www.google.com")
                    Return True
                End Using
            End Using
        Catch
            Return False
        End Try
    End Function

    Private Function GCD(a As Integer, b As Integer) As Integer
        If b = 0 Then
            Return a
        Else
            Return GCD(b, a Mod b)
        End If
    End Function

    Private Function CalcAspectRatio(width As Integer, height As Integer) As String
        Dim scale As Integer = GCD(width, height)
        Return (width / scale) & ":" & (height / scale)
    End Function

    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e _
    As System.EventArgs) Handles MyBase.Load
        ' Read the TLOMNLauncher config
        Configuration = New INIFile(LauncherConfigFilename)

        LauncherUpdater.DoUpdateCheck()
        DoDebugFix()

        If Configuration.GetString("Beta", "EXEName", "<none>") IsNot "<none>" Then
            Dim voodooFilename As String = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(Configuration.GetString("Beta", "EXEName", "<none>")), dgVoodooConfigFilename)
            If System.IO.File.Exists(voodooFilename) Then
                ' Check if the conf starts with "DEGET"

                Dim firstFiveLetters As String

                Using reader As New System.IO.BinaryReader(New System.IO.FileStream(voodooFilename, System.IO.FileMode.Open))
                    firstFiveLetters = System.Text.Encoding.ASCII.GetString(reader.ReadBytes(5))
                End Using

                If firstFiveLetters = "DEGET" Then
                    MessageBox.Show("WARNING: You must to dgVoodoo 2.54 or newer to use the resolution picker.")
                Else
                    Dim dgVoodooINI As New INIFile(voodooFilename)
                    Dim resolutionSetting As String = dgVoodooINI.GetString("DirectX", "Resolution", "h:640, v:480")
                    'MessageBox.Show("Your dgVoodoo resolution is " & resolutionSetting)
                    Dim parts() As String = resolutionSetting.Split(" ")
                    Dim horizontal As Integer = parts(0).TrimEnd(",").Substring(2)
                    Dim vertical As Integer = parts(1).Substring(2)
                    Dim selectedRes As String = horizontal & "x" & vertical & " (" & CalcAspectRatio(horizontal, vertical) & ")"
                    ComboBox1.SelectedItem = selectedRes
                End If
            End If
        End If
    End Sub

    '----------------------------------------------------------------
    'Draw browser window
    '----------------------------------------------------------------

    Private WithEvents Browser As ChromiumWebBrowser

    Public Sub New()
        InitializeComponent()

        Dim settings As New CefSettings()
        CefSharp.Cef.Initialize(settings)

        If CheckForInternetConnection() = False Then
            Exit Sub
        End If

        Browser = New ChromiumWebBrowser("http://biomediaproject.com/bmp/files/gms/tlomn/Launcher/Web/PatchNotes.html") With {
            .Dock = DockStyle.Fill
        }
        News.Controls.Add(Browser)
    End Sub

    '----------------------------------------------------------------
    'Perform game tests & launch (Alpha)
    '----------------------------------------------------------------

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click

        Dim gameFilename As String = Configuration.GetString("Alpha", "EXEName", "<none>")
        If gameFilename = "<none>" Then
            If System.IO.File.Exists(AlphaDefaultFilename) Then
                gameFilename = AlphaDefaultFilename
            Else
                Dim choice As DialogResult
                choice = MessageBox.Show("Could not find the game. Is it downloaded?", "The Legend of Mata Nui Alpha", MessageBoxButtons.YesNoCancel)
                If choice = DialogResult.Yes Then
                    ' Browse for the game
                    Dim browser As New OpenFileDialog()
                    browser.Filter = "LEGO Bionicle.exe|LEGO Bionicle.exe"
                    If browser.ShowDialog() = DialogResult.OK Then
                        gameFilename = browser.FileName
                    Else
                        Exit Sub
                    End If
                ElseIf choice = DialogResult.No Then
                    ' Download the game
                    Try
                        My.Computer.Network.DownloadFile("http://biomediaproject.com/bmp/files/gms/tlomn/Launcher/setup.exe",
            "Temp\setup.exe", "", "", True, 1000, True)
                        Process.Start("Temp\setup.exe").WaitForExit()

                        ' Ask user where they installed
                        Dim browser As New OpenFileDialog()
                        browser.Filter = "LEGO Bionicle.exe|LEGO Bionicle.exe"
                        If browser.ShowDialog() = DialogResult.OK Then
                            gameFilename = browser.FileName
                        Else
                            Exit Sub
                        End If

                        '----------------------------------------------------------------
                        'Cleanup Dancyboi garbage
                        '----------------------------------------------------------------

                        My.Computer.FileSystem.DeleteFile("..\LEGO Bionicle\Data\characters\1hat\Anims\1haB.bhd")
                        My.Computer.FileSystem.DeleteFile("..\LEGO Bionicle\Data\characters\1hat\Anims\1haD.bhd")
                        My.Computer.FileSystem.DeleteFile("..\LEGO Bionicle\Data\characters\1hat\Anims\1haL.bhd")
                        My.Computer.FileSystem.DeleteFile("..\LEGO Bionicle\Data\characters\1hat\Anims\1haR.bhd")
                        My.Computer.FileSystem.DeleteFile("..\LEGO Bionicle\Data\characters\1hat\Anims\1haU.bhd")
                        My.Computer.FileSystem.DeleteFile("..\LEGO Bionicle\Data\characters\1hat\Anims\1haW.bhd")
                    Catch
                        Exit Sub
                    End Try
                Else
                    ' User canceled
                    Exit Sub
                End If
            End If
            Configuration.SetString("Alpha", "EXEName", gameFilename)
            Configuration.Write(LauncherConfigFilename)
        End If
        DoPatch()
    End Sub

    '----------------------------------------------------------------
    'Perform game tests & launch (Beta)
    '----------------------------------------------------------------

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        Dim gameFilename As String = Configuration.GetString("Beta", "EXEName", "<none>")
        If gameFilename = "<none>" Then
            If System.IO.File.Exists(BetaDefaultFilename) Then
                gameFilename = BetaDefaultFilename
            Else
                Dim choice As DialogResult
                choice = MessageBox.Show("Could not find the game. Is it downloaded?", "The Legend of Mata Nui Beta", MessageBoxButtons.YesNoCancel)
                If choice = DialogResult.Yes Then
                    ' Browse for the game
                    Dim browser As New OpenFileDialog()
                    browser.Filter = "LEGO Bionicle.exe|LEGO Bionicle.exe"
                    If browser.ShowDialog() = DialogResult.OK Then
                        gameFilename = browser.FileName
                    Else
                        Exit Sub
                    End If
                ElseIf choice = DialogResult.No Then
                    ' Download the game
                    Try
                        My.Computer.Network.DownloadFile("http://biomediaproject.com/bmp/files/gms/tlomn/Launcher/setupbeta.exe",
            "Temp\setupbeta.exe", "", "", True, 1000, True)
                        Process.Start("Temp\setupbeta.exe").WaitForExit()

                        ' Ask user where they installed
                        Dim browser As New OpenFileDialog()
                        browser.Filter = "LEGO Bionicle.exe|LEGO Bionicle.exe"
                        If browser.ShowDialog() = DialogResult.OK Then
                            gameFilename = browser.FileName
                        Else
                            Exit Sub
                        End If

                    Catch
                        Exit Sub
                    End Try
                Else
                    ' User canceled
                    Exit Sub
                End If
            End If
            Configuration.SetString("Beta", "EXEName", gameFilename)
            Configuration.Write(LauncherConfigFilename)
        End If
        DoPatchBeta()
    End Sub

    '----------------------------------------------------------------
    'Check Patch Version (Alpha)
    '----------------------------------------------------------------

    Public Sub DoPatch()

        '----------------------------------------------------------------
        'Check patch version
        '----------------------------------------------------------------

        Try
            My.Computer.Network.DownloadFile(
            "http://biomediaproject.com/bmp/files/gms/tlomn/Launcher/Patch/version.txt",
            "Temp\version.txt", "", "", True, 1000, True)
        Catch
            Process.Start(Configuration.GetString("Alpha", "EXEName", "<none>"))
            Me.Close()
            Exit Sub
        End Try

        Dim gameFolder As String = System.IO.Path.GetDirectoryName(Configuration.GetString("Alpha", "EXEName", "<none>"))
        Dim patchFilename As String = System.IO.Path.Combine(gameFolder, "Patch.exe")

        Dim text1 As String = My.Computer.FileSystem.ReadAllText("Temp\version.txt")
        Dim text2 As String = ""
        If System.IO.File.Exists(System.IO.Path.Combine(gameFolder, "version.txt")) Then
            text2 = My.Computer.FileSystem.ReadAllText(System.IO.Path.Combine(gameFolder, "version.txt"))
        End If
        If text1 <> text2 Then

            '----------------------------------------------------------------
            'If mismatch, show download prompt
            '----------------------------------------------------------------

            Dim msgRslt As MsgBoxResult = MsgBox("A new alpha patch is available! Would you like to download it?", MsgBoxStyle.YesNo)
            If msgRslt = MsgBoxResult.Yes Then
                Try
                    My.Computer.Network.DownloadFile(
                   "http://biomediaproject.com/bmp/files/gms/tlomn/Launcher/Patch/Patch.exe",
                   patchFilename, "", "", True, 1000, True)
                    Process.Start(patchFilename).WaitForExit()
                Catch
                    Me.Close()
                End Try
                Process.Start(Configuration.GetString("Alpha", "EXEName", "<none>"))
                My.Computer.FileSystem.DeleteFile(patchFilename)

                '----------------------------------------------------------------
                'If patch declined, run game anyways
                '----------------------------------------------------------------

            ElseIf msgRslt = MsgBoxResult.No Then
                Process.Start(Configuration.GetString("Alpha", "EXEName", "<none>"))
            End If
        Else
            Process.Start(Configuration.GetString("Alpha", "EXEName", "<none>"))
        End If
        My.Computer.FileSystem.DeleteDirectory("Temp", FileIO.DeleteDirectoryOption.DeleteAllContents)
        Close()
    End Sub

    '----------------------------------------------------------------
    'Check Patch Version (Beta)
    '----------------------------------------------------------------

    Public Sub DoPatchBeta()

        '----------------------------------------------------------------
        'Check patch version
        '----------------------------------------------------------------

        Try
            My.Computer.Network.DownloadFile(
            "http://biomediaproject.com/bmp/files/gms/tlomn/Launcher/Patch/versionbeta.txt",
            "Temp\versionbeta.txt", "", "", True, 1000, True)
        Catch
            Process.Start(Configuration.GetString("Beta", "EXEName", "<none>"))
            Exit Sub
            Me.Close()
        End Try

        Dim gameFolder As String = System.IO.Path.GetDirectoryName(Configuration.GetString("Beta", "EXEName", "<none>"))
        Dim patchFilename As String = System.IO.Path.Combine(gameFolder, "PatchB.exe")

        Dim text1 As String = My.Computer.FileSystem.ReadAllText("Temp\versionbeta.txt")
        Dim text2 As String = ""
        If System.IO.File.Exists(System.IO.Path.Combine(gameFolder, "versionbeta.txt")) Then
            text2 = My.Computer.FileSystem.ReadAllText(System.IO.Path.Combine(gameFolder, "versionbeta.txt"))
        End If
        If text1 <> text2 Then

            '----------------------------------------------------------------
            'If mismatch, show download prompt
            '----------------------------------------------------------------

            Dim msgRslt As MsgBoxResult = MsgBox("A new beta patch is available! Would you like to download it?", MsgBoxStyle.YesNo)
            If msgRslt = MsgBoxResult.Yes Then
                Try
                    My.Computer.Network.DownloadFile(
                   "http://biomediaproject.com/bmp/files/gms/tlomn/Launcher/Patch/PatchB.exe",
                   patchFilename, "", "", True, 1000, True)
                    Process.Start(patchFilename).WaitForExit()
                Catch
                    Me.Close()
                End Try
                Process.Start(Configuration.GetString("Beta", "EXEName", "<none>"))
                My.Computer.FileSystem.DeleteFile(patchFilename)

                '----------------------------------------------------------------
                'If patch declined, run game anyways
                '----------------------------------------------------------------

            ElseIf msgRslt = MsgBoxResult.No Then
                Process.Start(Configuration.GetString("Beta", "EXEName", "<none>"))
            End If
        Else
            Process.Start(Configuration.GetString("Beta", "EXEName", "<none>"))
        End If
        My.Computer.FileSystem.DeleteDirectory("Temp", FileIO.DeleteDirectoryOption.DeleteAllContents)
        Close()
    End Sub

    '----------------------------------------------------------------
    'Launch DGVoodoo
    '----------------------------------------------------------------

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Process.Start("dgVoodooCpl.exe")
    End Sub

    '----------------------------------------------------------------
    'Launch Debug Menu Fix
    '----------------------------------------------------------------

    Public Sub DoDebugFix()
        Process.Start("Debug Menu Fix.exe")
    End Sub

    '----------------------------------------------------------------
    'Sidebar Links
    '----------------------------------------------------------------

    Private Sub PictureBox1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PictureBox1.Click

        Process.Start("http://biomediaproject.com/")

    End Sub

    Private Sub PictureBox2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PictureBox2.Click

        Process.Start("http://bit.ly/Beavercord")

    End Sub

    Private Sub PictureBox3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PictureBox3.Click

        Process.Start("http://youtube.com/vahkiti/")

    End Sub

    Private Sub PictureBox4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PictureBox4.Click

        Process.Start("https://github.com/TheLegendOfMataNui/game-issues/issues")

    End Sub

    Private Sub Form1_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        Configuration.Write(LauncherConfigFilename)
    End Sub

    Private Sub ComboBox1_SelectedValueChanged(sender As Object, e As EventArgs) Handles ComboBox1.SelectedValueChanged
        Dim selectedItem As String = ComboBox1.SelectedItem
        Dim parts() As String = selectedItem.Split(" ")(0).Split("x")
        Dim width As Integer = parts(0)
        Dim height As Integer = parts(1)
        Dim bioHeight As Integer = 480
        Dim bioWidth As Integer = bioHeight * (width / height)

        Dim alphaFilename As String = Configuration.GetString("Alpha", "EXEName", "<none>")
        Dim alphaFolder As String = System.IO.Path.GetDirectoryName(alphaFilename)
        If alphaFilename IsNot "<none>" Then
            Dim BioINI As New INIFile(System.IO.Path.Combine(alphaFolder, BionicleConfigFilename))
            BioINI.SetString("GraphicsOptions", "ResolutionW", bioWidth)
            BioINI.SetString("GraphicsOptions", "ResolutionH", bioHeight)
            BioINI.Write(System.IO.Path.Combine(alphaFolder, BionicleConfigFilename))

            Dim voodooINI As New INIFile(System.IO.Path.Combine(alphaFolder, dgVoodooConfigFilename))
            voodooINI.SetString("DirectX", "Resolution", "h:" & width & ", v:" & height)
            voodooINI.Write(System.IO.Path.Combine(alphaFolder, dgVoodooConfigFilename))
        End If

        Dim betaFilename As String = Configuration.GetString("Beta", "EXEName", "<none>")
        Dim betaFolder As String = System.IO.Path.GetDirectoryName(betaFilename)
        If betaFilename IsNot "<none>" Then
            Dim BioINI As New INIFile(System.IO.Path.Combine(betaFolder, BionicleConfigFilename))
            BioINI.SetString("GraphicsOptions", "ResolutionW", bioWidth)
            BioINI.SetString("GraphicsOptions", "ResolutionH", bioHeight)
            BioINI.Write(System.IO.Path.Combine(betaFolder, BionicleConfigFilename))

            Dim voodooINI As New INIFile(System.IO.Path.Combine(betaFolder, dgVoodooConfigFilename))
            voodooINI.SetString("DirectX", "Resolution", "h:" & width & ", v:" & height)
            voodooINI.Write(System.IO.Path.Combine(betaFolder, dgVoodooConfigFilename))
        End If
    End Sub
End Class