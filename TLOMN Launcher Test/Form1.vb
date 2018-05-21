Imports CefSharp.WinForms
Imports CefSharp
Imports System.Net

Public Class Form1

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

    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e _
    As System.EventArgs) Handles MyBase.Load
        LauncherUpdater.DoUpdateCheck()
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
        '----------------------------------------------------------------
        'Test for game being installed
        '----------------------------------------------------------------

        If My.Computer.FileSystem.FileExists("..\LEGO Bionicle\LEGO Bionicle.exe") Then
            DoPatch()

            '----------------------------------------------------------------
            'If not found, show download prompt
            '----------------------------------------------------------------

        Else
            Dim msgRslt As MsgBoxResult = MsgBox("The alpha base game was not found! Would you like to download it?", MsgBoxStyle.YesNo)
            If msgRslt = MsgBoxResult.Yes Then
                Try
                    My.Computer.Network.DownloadFile("http://biomediaproject.com/bmp/files/gms/tlomn/Launcher/setup.exe",
            "Temp\setup.exe", "", "", True, 1000, True)
                    Process.Start("Temp\setup.exe").WaitForExit()

                    '----------------------------------------------------------------
                    'Cleanup Dancyboi garbage
                    '----------------------------------------------------------------

                    My.Computer.FileSystem.DeleteFile("..\LEGO Bionicle\Data\characters\1hat\Anims\1haB.bhd")
                    My.Computer.FileSystem.DeleteFile("..\LEGO Bionicle\Data\characters\1hat\Anims\1haD.bhd")
                    My.Computer.FileSystem.DeleteFile("..\LEGO Bionicle\Data\characters\1hat\Anims\1haL.bhd")
                    My.Computer.FileSystem.DeleteFile("..\LEGO Bionicle\Data\characters\1hat\Anims\1haR.bhd")
                    My.Computer.FileSystem.DeleteFile("..\LEGO Bionicle\Data\characters\1hat\Anims\1haU.bhd")
                    My.Computer.FileSystem.DeleteFile("..\LEGO Bionicle\Data\characters\1hat\Anims\1haW.bhd")
                    DoPatch()
                Catch
                    Me.Close()
                End Try
            End If
        End If
    End Sub

    '----------------------------------------------------------------
    'Perform game tests & launch (Beta)
    '----------------------------------------------------------------

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        '----------------------------------------------------------------
        'Test for game being installed
        '----------------------------------------------------------------

        If My.Computer.FileSystem.FileExists("..\LEGO Bionicle (Beta)\LEGO Bionicle.exe") Then
            DoPatchBeta()

            '----------------------------------------------------------------
            'If not found, show download prompt
            '----------------------------------------------------------------

        Else
            Dim msgRslt As MsgBoxResult = MsgBox("The beta base game was not found! Would you like to download it?", MsgBoxStyle.YesNo)
            If msgRslt = MsgBoxResult.Yes Then
                Try
                    My.Computer.Network.DownloadFile("http://biomediaproject.com/bmp/files/gms/tlomn/Launcher/setupbeta.exe",
            "Temp\setupbeta.exe", "", "", True, 1000, True)
                    Process.Start("Temp\setupbeta.exe").WaitForExit()
                Catch
                    Me.Close()
                End Try
            End If
        End If
    End Sub

    '----------------------------------------------------------------
    'Launch debug fix
    '----------------------------------------------------------------

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Process.Start("Debug Menu Fix.exe")
    End Sub

    '----------------------------------------------------------------
    'Launch DGVoodoo
    '----------------------------------------------------------------

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Process.Start("dgVoodooCpl.exe")
    End Sub

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

    Public Sub DoPatch()
        '----------------------------------------------------------------
        'Check patch version
        '----------------------------------------------------------------

        Try
            My.Computer.Network.DownloadFile(
            "http://biomediaproject.com/bmp/files/gms/tlomn/Launcher/Patch/version.txt",
            "Temp\version.txt", "", "", True, 1000, True)
        Catch
            Process.Start("..\LEGO Bionicle\LEGO Bionicle.exe")
            Me.Close()
            Exit Sub
        End Try
        Dim text1 As String = My.Computer.FileSystem.ReadAllText("Temp\version.txt")
        Dim text2 As String = My.Computer.FileSystem.ReadAllText("..\LEGO Bionicle\version.txt")
        If text1 <> text2 Then

            '----------------------------------------------------------------
            'If mismatch, show download prompt
            '----------------------------------------------------------------

            Dim msgRslt As MsgBoxResult = MsgBox("A new alpha patch is available! Would you like to download it?", MsgBoxStyle.YesNo)
            If msgRslt = MsgBoxResult.Yes Then
                Try
                    My.Computer.Network.DownloadFile(
                   "http://biomediaproject.com/bmp/files/gms/tlomn/Launcher/Patch/Patch.exe",
                   "..\LEGO Bionicle\Patch.exe", "", "", True, 1000, True)
                    Process.Start("..\LEGO Bionicle\Patch.exe").WaitForExit()
                Catch
                    Me.Close()
                End Try
                Process.Start("..\LEGO Bionicle\LEGO Bionicle.exe")
                My.Computer.FileSystem.DeleteFile("..\LEGO Bionicle\Patch.exe")

                '----------------------------------------------------------------
                'If patch declined, run game anyways
                '----------------------------------------------------------------

            ElseIf msgRslt = MsgBoxResult.No Then
                Process.Start("..\LEGO Bionicle\LEGO Bionicle.exe")
            End If
        Else
            Process.Start("..\LEGO Bionicle\LEGO Bionicle.exe")
        End If
        My.Computer.FileSystem.DeleteDirectory("Temp", FileIO.DeleteDirectoryOption.DeleteAllContents)
        Close()
    End Sub

    Public Sub DoPatchBeta()
        '----------------------------------------------------------------
        'Check patch version
        '----------------------------------------------------------------

        Try
            My.Computer.Network.DownloadFile(
            "http://biomediaproject.com/bmp/files/gms/tlomn/Launcher/Patch/versionbeta.txt",
            "Temp\versionbeta.txt", "", "", True, 1000, True)
        Catch
            Process.Start("..\LEGO Bionicle (Beta)\LEGO Bionicle.exe")
            Exit Sub
            Me.Close()
        End Try
        Dim text1 As String = My.Computer.FileSystem.ReadAllText("Temp\versionbeta.txt")
        Dim text2 As String = My.Computer.FileSystem.ReadAllText("..\LEGO Bionicle (Beta)\versionbeta.txt")
        If text1 <> text2 Then

            '----------------------------------------------------------------
            'If mismatch, show download prompt
            '----------------------------------------------------------------

            Dim msgRslt As MsgBoxResult = MsgBox("A new beta patch is available! Would you like to download it?", MsgBoxStyle.YesNo)
            If msgRslt = MsgBoxResult.Yes Then
                Try
                    My.Computer.Network.DownloadFile(
                   "http://biomediaproject.com/bmp/files/gms/tlomn/Launcher/Patch/PatchB.exe",
                   "..\LEGO Bionicle (Beta)\PatchB.exe", "", "", True, 1000, True)
                    Process.Start("..\LEGO Bionicle (Beta)\PatchB.exe").WaitForExit()
                Catch
                    Me.Close()
                End Try
                Process.Start("..\LEGO Bionicle (Beta)\LEGO Bionicle.exe")
                My.Computer.FileSystem.DeleteFile("..\LEGO Bionicle (Beta)\PatchB.exe")

                '----------------------------------------------------------------
                'If patch declined, run game anyways
                '----------------------------------------------------------------

            ElseIf msgRslt = MsgBoxResult.No Then
                Process.Start("..\LEGO Bionicle (Beta)\LEGO Bionicle.exe")
            End If
        Else
            Process.Start("..\LEGO Bionicle (Beta)\LEGO Bionicle.exe")
        End If
        My.Computer.FileSystem.DeleteDirectory("Temp", FileIO.DeleteDirectoryOption.DeleteAllContents)
        Me.Close()
    End Sub
End Class