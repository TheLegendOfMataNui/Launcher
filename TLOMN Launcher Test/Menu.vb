Imports CefSharp.WinForms
Imports CefSharp
Imports System.Net
Imports System.IO

Public Class Form1

    '----------------------------------------------------------------
    'Startup Checks
    '----------------------------------------------------------------

    Private Const dgVoodooConfigFilename As String = "dgVoodoo.conf"
    Private Const LauncherConfigFilename As String = "TLOMNLauncher.ini"
    Private Const BionicleConfigFilename As String = "Bionicle.ini"

    Private IgnoreUI As Boolean = True

    Public Configuration As INIFile

    '----------------------------------------------------------------
    'Startup Tasks
    '----------------------------------------------------------------

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

        CheckboxBeta()
        CheckboxRebuilt()

        If Configuration.GetString("Beta", "EXEName", "<none>") IsNot "<none>" Then
            Dim voodooFilename As String = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(Configuration.GetString("Beta", "EXEName", "<none>")), dgVoodooConfigFilename)
            If System.IO.File.Exists(voodooFilename) Then
                ' Check if the conf starts with "DEGET"

                Dim firstFiveLetters As String

                Using reader As New System.IO.BinaryReader(New System.IO.FileStream(voodooFilename, System.IO.FileMode.Open))
                    firstFiveLetters = System.Text.Encoding.ASCII.GetString(reader.ReadBytes(5))
                End Using

                If firstFiveLetters = "DEGET" Then
                    MessageBox.Show("WARNING: You must use dgVoodoo 2.54 or newer to use the resolution picker.")
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
        ElseIf Configuration.GetString("Alpha", "EXEName", "<none>") IsNot "<none>" Then

            Dim voodooFilename As String = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(Configuration.GetString("Alpha", "EXEName", "<none>")), dgVoodooConfigFilename)
            If System.IO.File.Exists(voodooFilename) Then
                ' Check if the conf starts with "DEGET"

                Dim firstFiveLetters As String

                Using reader As New System.IO.BinaryReader(New System.IO.FileStream(voodooFilename, System.IO.FileMode.Open))
                    firstFiveLetters = System.Text.Encoding.ASCII.GetString(reader.ReadBytes(5))
                End Using

                If firstFiveLetters = "DEGET" Then
                    MessageBox.Show("WARNING: You must use dgVoodoo 2.54 or newer to use the resolution picker.")
                Else
                    Dim dgVoodooINI As New INIFile(voodooFilename)
                    Dim resolutionSetting As String = dgVoodooINI.GetString("DirectX", "Resolution", "h:640, v:480")
                    'MessageBox.Show("Your dgVoodoo resolution is " & resolutionSetting)
                    Dim parts() As String = resolutionSetting.Split(" ")
                    Dim horizontal As Integer = parts(0).TrimEnd(",").Substring(2)
                    Dim vertical As Integer = parts(1).Substring(2)
                    Dim selectedRes As String = horizontal & "x" & vertical & " (" & CalcAspectRatio(horizontal, vertical) & ")"
                    If Not ComboBox1.Items.Contains(selectedRes) Then
                        ComboBox1.Items.Add(selectedRes)
                    End If
                    ComboBox1.SelectedItem = selectedRes
                End If
            End If

        ElseIf Configuration.GetString("Rebuilt", "EXEName", "<none>") IsNot "<none>" Then

            Dim voodooFilename As String = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(Configuration.GetString("Rebuilt", "EXEName", "<none>")), dgVoodooConfigFilename)
            If System.IO.File.Exists(voodooFilename) Then
                ' Check if the conf starts with "DEGET"

                Dim firstFiveLetters As String

                Using reader As New System.IO.BinaryReader(New System.IO.FileStream(voodooFilename, System.IO.FileMode.Open))
                    firstFiveLetters = System.Text.Encoding.ASCII.GetString(reader.ReadBytes(5))
                End Using

                If firstFiveLetters = "DEGET" Then
                    MessageBox.Show("WARNING: You must use dgVoodoo 2.54 or newer to use the resolution picker.")
                Else
                    Dim dgVoodooINI As New INIFile(voodooFilename)
                    Dim resolutionSetting As String = dgVoodooINI.GetString("DirectX", "Resolution", "h:640, v:480")
                    'MessageBox.Show("Your dgVoodoo resolution is " & resolutionSetting)
                    Dim parts() As String = resolutionSetting.Split(" ")
                    Dim horizontal As Integer = parts(0).TrimEnd(",").Substring(2)
                    Dim vertical As Integer = parts(1).Substring(2)
                    Dim selectedRes As String = horizontal & "x" & vertical & " (" & CalcAspectRatio(horizontal, vertical) & ")"
                    If Not ComboBox1.Items.Contains(selectedRes) Then
                        ComboBox1.Items.Add(selectedRes)
                    End If
                    ComboBox1.SelectedItem = selectedRes
                End If
            End If
        End If


    End Sub

    Public Sub CheckboxBeta()
        Dim betaFilename As String = Configuration.GetString("Beta", "EXEName", "<none>")
        If betaFilename = "<none>" Then
            Exit Sub
        End If

        Dim betaFolder As String = System.IO.Path.GetDirectoryName(betaFilename)
        Dim BioINI As New INIFile(System.IO.Path.Combine(betaFolder, BionicleConfigFilename))
        If BioINI.GetString("Misc", "Cheatmenu", "<none>") = "LEGOTester" Then
            TestMenu.Checked = True
        Else
            TestMenu.Checked = False
        End If
    End Sub

    Public Sub CheckboxRebuilt()
        Dim rebuiltFilename As String = Configuration.GetString("Rebuilt", "EXEName", "<none>")
        If rebuiltFilename = "<none>" Then
            Exit Sub
        End If

        Dim rebuiltFolder As String = System.IO.Path.GetDirectoryName(rebuiltFilename)
        Dim BioINI As New INIFile(System.IO.Path.Combine(rebuiltFolder, BionicleConfigFilename))
        If BioINI.GetString("Misc", "Cheatmenu", "<none>") = "LEGOTester" Then
            TestMenu.Checked = True
        Else
            TestMenu.Checked = False
        End If
    End Sub


    '----------------------------------------------------------------
    'Draw browser window
    '----------------------------------------------------------------

    Private WithEvents NewsPage As ChromiumWebBrowser

    Public Sub New()
        InitializeComponent()

        Dim settings As New CefSettings()
        CefSharp.Cef.Initialize(settings)

        If CheckForInternetConnection() = False Then
            Exit Sub
        End If

        NewsPage = New ChromiumWebBrowser("https://810nicleday.com/LOMN/PatchNotes.html") With {
            .Dock = DockStyle.Fill
        }
        News.Controls.Add(NewsPage)
    End Sub

    '----------------------------------------------------------------
    'Perform game tests & launch (Alpha)
    '----------------------------------------------------------------

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        'Check for game
        Try
            Dim LatestAlphaRelease As GitHubRelease = GitHubAPI.GetLatestRelease(GitHubAPI.OrganizationIdentifier, GitHubAPI.LOMNAlphaRepoIdentifier, False)
            Dim gameFilename As String = Configuration.GetString("Alpha", "EXEName", "<none>")
            If gameFilename = "none" Or Not My.Computer.FileSystem.FileExists(gameFilename) Then
                Dim choice As DialogResult
                choice = MessageBox.Show("Could not find the game. Is it downloaded?", "The Legend of Mata Nui Alpha", MessageBoxButtons.YesNoCancel)
                If choice = DialogResult.Yes Then
                    'Browse for the game
                    Dim browser As New OpenFileDialog()
                    If browser.ShowDialog() = DialogResult.OK Then
                        gameFilename = browser.FileName
                        Configuration.SetString("Alpha", "EXEName", gameFilename)
                    End If
                ElseIf choice = DialogResult.No Then
                    'Download the game
                    NewsPage.Load("https://810nicleday.com/LOMN/PatchNotesLoading.html")
                    GitHubAPI.DownloadFile(LatestAlphaRelease.AssetDownloadURL, "Temp\Setup.exe")
                    NewsPage.Load("https://810nicleday.com/LOMN/PatchNotes.html")
                    Process.Start("Temp\Setup.exe").WaitForExit()
                    gameFilename = My.Computer.FileSystem.ReadAllText("Temp\AlphaDir.txt")
                    Configuration.SetString("Alpha", "EXEName", gameFilename)
                End If
            End If
            DoPatchAlpha()
        Catch
        End Try
    End Sub

    Public Sub DoPatchAlpha()
        Try
            'Check for updates
            Dim LatestAlphaRelease As GitHubRelease = GitHubAPI.GetLatestRelease(GitHubAPI.OrganizationIdentifier, GitHubAPI.LOMNAlphaRepoIdentifier, False)
            Dim gameFilename As String = Configuration.GetString("Alpha", "EXEName", "<none>")
            Dim GameFolder As String = System.IO.Path.GetDirectoryName(Configuration.GetString("Alpha", "EXEName", "<none>"))
            Dim GitTag As String = LatestAlphaRelease.TagName
            Dim LocalVer As String = ""
            LocalVer = My.Computer.FileSystem.ReadAllText(System.IO.Path.Combine(GameFolder, "version.txt"))
            If GitTag <> LocalVer Then
                'If version is mismatched, show download prompt                
                Dim choice As MsgBoxResult = MsgBox("A new update is available! Would you like to download it?", MsgBoxStyle.YesNo)
                If choice = MsgBoxResult.Yes Then
                    NewsPage.Load("https://810nicleday.com/LOMN/PatchNotesLoading.html")
                    GitHubAPI.DownloadFile(LatestAlphaRelease.AssetDownloadURL, "Temp\Setup.exe")
                    NewsPage.Load("https://810nicleday.com/LOMN/PatchNotes.html")
                    Process.Start("Temp\Setup.exe").WaitForExit()
                    gameFilename = My.Computer.FileSystem.ReadAllText("Temp\AlphaDir.txt")
                    Configuration.SetString("Alpha", "EXEName", gameFilename)
                End If
            End If
        Catch
        End Try
        LaunchAlpha()
    End Sub

    Public Sub LaunchAlpha()
        'Launch game, write config, & cleanup
        ApplyGameOptions()
        Configuration.Write(LauncherConfigFilename)
        Dim directoryName As String = "Temp"
        For Each deleteFile In Directory.GetFiles(directoryName, "*.*", SearchOption.TopDirectoryOnly)
            File.Delete(deleteFile)
        Next
        Process.Start(Configuration.GetString("Alpha", "EXEName", "<none>"))
        Me.Close()
    End Sub

    '----------------------------------------------------------------
    'Perform game tests & launch (Beta)
    '----------------------------------------------------------------

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        'Check for game
        Try
            Dim LatestBetaRelease As GitHubRelease = GitHubAPI.GetLatestRelease(GitHubAPI.OrganizationIdentifier, GitHubAPI.LOMNBetaRepoIdentifier, False)
            Dim gameFilename As String = Configuration.GetString("Beta", "EXEName", "<none>")
            If gameFilename = "none" Or Not My.Computer.FileSystem.FileExists(gameFilename) Then
                Dim choice As DialogResult
                choice = MessageBox.Show("Could not find the game. Is it downloaded?", "The Legend of Mata Nui Beta", MessageBoxButtons.YesNoCancel)
                If choice = DialogResult.Yes Then
                    'Browse for the game
                    Dim browser As New OpenFileDialog()
                    If browser.ShowDialog() = DialogResult.OK Then
                        gameFilename = browser.FileName
                        Configuration.SetString("Beta", "EXEName", gameFilename)
                    End If
                ElseIf choice = DialogResult.No Then
                    'Download the game
                    NewsPage.Load("https://810nicleday.com/LOMN/PatchNotesLoading.html")
                    GitHubAPI.DownloadFile(LatestBetaRelease.AssetDownloadURL, "Temp\Setup.exe")
                    NewsPage.Load("https://810nicleday.com/LOMN/PatchNotes.html")
                    Process.Start("Temp\Setup.exe").WaitForExit()
                    gameFilename = My.Computer.FileSystem.ReadAllText("Temp\BetaDir.txt")
                    Configuration.SetString("Beta", "EXEName", gameFilename)
                End If
            End If
            DoPatchBeta()
        Catch
        End Try
    End Sub

    Public Sub DoPatchBeta()
        Try
            'Check for updates
            Dim LatestBetaRelease As GitHubRelease = GitHubAPI.GetLatestRelease(GitHubAPI.OrganizationIdentifier, GitHubAPI.LOMNBetaRepoIdentifier, False)
            Dim gameFilename As String = Configuration.GetString("Beta", "EXEName", "<none>")
            Dim GameFolder As String = System.IO.Path.GetDirectoryName(Configuration.GetString("Beta", "EXEName", "<none>"))
            Dim GitTag As String = LatestBetaRelease.TagName
            Dim LocalVer As String = ""
            LocalVer = My.Computer.FileSystem.ReadAllText(System.IO.Path.Combine(GameFolder, "version.txt"))
            If GitTag <> LocalVer Then
                'If version is mismatched, show download prompt                
                Dim choice As MsgBoxResult = MsgBox("A new update is available! Would you like to download it?", MsgBoxStyle.YesNo)
                If choice = MsgBoxResult.Yes Then
                    NewsPage.Load("https://810nicleday.com/LOMN/PatchNotesLoading.html")
                    GitHubAPI.DownloadFile(LatestBetaRelease.AssetDownloadURL, "Temp\Setup.exe")
                    NewsPage.Load("https://810nicleday.com/LOMN/PatchNotes.html")
                    Process.Start("Temp\Setup.exe").WaitForExit()
                    gameFilename = My.Computer.FileSystem.ReadAllText("Temp\BetaDir.txt")
                    Configuration.SetString("Beta", "EXEName", gameFilename)
                End If
            End If
        Catch
        End Try
        LaunchBeta()
    End Sub

    Public Sub LaunchBeta()
        'Launch game, write config, & cleanup
        ApplyGameOptions()
        Configuration.Write(LauncherConfigFilename)
        Dim directoryName As String = "Temp"
        For Each deleteFile In Directory.GetFiles(directoryName, "*.*", SearchOption.TopDirectoryOnly)
            File.Delete(deleteFile)
        Next
        Process.Start(Configuration.GetString("Beta", "EXEName", "<none>"))
        Me.Close()
    End Sub

    '----------------------------------------------------------------
    'Perform game tests & launch (Rebuilt)
    '----------------------------------------------------------------

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        'Check for game
        Try
            Dim LatestRebuiltRelease As GitHubRelease = GitHubAPI.GetLatestRelease(GitHubAPI.OrganizationIdentifier, GitHubAPI.LOMNRebuiltRepoIdentifier, False)
            Dim gameFilename As String = Configuration.GetString("Rebuilt", "EXEName", "<none>")
            If gameFilename = "none" Or Not My.Computer.FileSystem.FileExists(gameFilename) Then
                Dim choice As DialogResult
                choice = MessageBox.Show("Could not find the game. Is it downloaded?", "The Legend of Mata Nui Rebuilt", MessageBoxButtons.YesNoCancel)
                If choice = DialogResult.Yes Then
                    'Browse for the game
                    Dim browser As New OpenFileDialog()
                    If browser.ShowDialog() = DialogResult.OK Then
                        gameFilename = browser.FileName
                        Configuration.SetString("Rebuilt", "EXEName", gameFilename)
                    End If
                ElseIf choice = DialogResult.No Then
                    'Download the game
                    NewsPage.Load("https://810nicleday.com/LOMN/PatchNotesLoading.html")
                    GitHubAPI.DownloadFile(LatestRebuiltRelease.AssetDownloadURL, "Temp\Setup.exe")
                    NewsPage.Load("https://810nicleday.com/LOMN/PatchNotes.html")
                    Process.Start("Temp\Setup.exe").WaitForExit()
                    gameFilename = My.Computer.FileSystem.ReadAllText("Temp\RebuiltDir.txt")
                    Configuration.SetString("Rebuilt", "EXEName", gameFilename)
                End If
            End If
            DoPatchRebuilt()
        Catch
        End Try
    End Sub

    Public Sub DoPatchRebuilt()
        Try
            'Check for updates
            Dim LatestRebuiltRelease As GitHubRelease = GitHubAPI.GetLatestRelease(GitHubAPI.OrganizationIdentifier, GitHubAPI.LOMNRebuiltRepoIdentifier, False)
            Dim gameFilename As String = Configuration.GetString("Rebuilt", "EXEName", "<none>")
            Dim GameFolder As String = System.IO.Path.GetDirectoryName(Configuration.GetString("Rebuilt", "EXEName", "<none>"))
            Dim GitTag As String = LatestRebuiltRelease.TagName
            Dim LocalVer As String = ""
            LocalVer = My.Computer.FileSystem.ReadAllText(System.IO.Path.Combine(GameFolder, "version.txt"))
            If GitTag <> LocalVer Then
                'If version is mismatched, show download prompt                
                Dim choice As MsgBoxResult = MsgBox("A new update is available! Would you like to download it?", MsgBoxStyle.YesNo)
                If choice = MsgBoxResult.Yes Then
                    NewsPage.Load("https://810nicleday.com/LOMN/PatchNotesLoading.html")
                    GitHubAPI.DownloadFile(LatestRebuiltRelease.AssetDownloadURL, "Temp\Setup.exe")
                    NewsPage.Load("https://810nicleday.com/LOMN/PatchNotes.html")
                    Process.Start("Temp\Setup.exe").WaitForExit()
                    gameFilename = My.Computer.FileSystem.ReadAllText("Temp\RebuiltDir.txt")
                    Configuration.SetString("Rebuilt", "EXEName", gameFilename)
                End If
            End If
        Catch
        End Try
        LaunchRebuilt()
    End Sub

    Public Sub LaunchRebuilt()
        'Launch game, write config, & cleanup
        ApplyGameOptions()
        Configuration.Write(LauncherConfigFilename)
        Dim directoryName As String = "Temp"
        For Each deleteFile In Directory.GetFiles(directoryName, "*.*", SearchOption.TopDirectoryOnly)
            File.Delete(deleteFile)
        Next
        Process.Start(Configuration.GetString("Rebuilt", "EXEName", "<none>"))
        Me.Close()
    End Sub

    '----------------------------------------------------------------
    'Check Launcher version
    '----------------------------------------------------------------

    Public Sub CheckLauncherUpdates()
        Try
            Dim LatestLauncherRelease As GitHubRelease = GitHubAPI.GetLatestRelease(GitHubAPI.OrganizationIdentifier, GitHubAPI.LauncherRepoIdentifier, False)
            Dim GitTag As String = LatestLauncherRelease.TagName
            Dim LocalVer As String = ""
            If System.IO.File.Exists("version.txt") Then
                LocalVer = My.Computer.FileSystem.ReadAllText("version.txt")
            ElseIf System.IO.File.Exists("versionL.txt") Then
                LocalVer = My.Computer.FileSystem.ReadAllText("versionL.txt")
            End If
            If GitTag <> LocalVer Then
                '----------------------------------------------------------------
                'If version is newer, show download prompt
                '----------------------------------------------------------------
                Dim msgRslt As MsgBoxResult = MsgBox("A new launcher update is available! Would you like to download it?", MsgBoxStyle.YesNo)
                If msgRslt = MsgBoxResult.Yes Then
                    NewsPage.Load("https://810nicleday.com/LOMN/PatchNotesLoading.html")
                    GitHubAPI.DownloadFile(LatestLauncherRelease.AssetDownloadURL, "Temp\BIONICLE Launcher Installer.exe")
                    Process.Start("Temp\BIONICLE Launcher Installer.exe")
                    Me.Close()
                End If
            End If
        Catch
        End Try
    End Sub

    '----------------------------------------------------------------
    'Launch DGVoodoo
    '----------------------------------------------------------------

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Process.Start("dgVoodooCpl.exe")
    End Sub

    '----------------------------------------------------------------
    'Sidebar Links
    '----------------------------------------------------------------

    Private Sub PictureBox1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PictureBox1.Click

        Process.Start("http://biomediaproject.com/")

    End Sub

    Private Sub PictureBox2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PictureBox2.Click

        Process.Start("http://bit.ly/LitestoneDiscord")

    End Sub

    Private Sub PictureBox3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PictureBox3.Click

        Process.Start("http://youtube.com/LitestoneStudios/")

    End Sub

    Private Sub PictureBox4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PictureBox4.Click

        Process.Start("https://github.com/TheLegendOfMataNui/game-issues/issues")

    End Sub

    '----------------------------------------------------------------
    'Resolution Selector
    '----------------------------------------------------------------

    Private Sub ComboBox1_SelectedValueChanged(sender As Object, e As EventArgs) Handles ComboBox1.SelectedValueChanged
        If IgnoreUI Then Exit Sub
        ApplyGameOptions()
    End Sub

    Private Sub TestMenu_CheckedChanged(sender As Object, e As EventArgs) Handles TestMenu.CheckedChanged
        If IgnoreUI Then Exit Sub
        ApplyGameOptions()
    End Sub

    Public Sub ApplyGameOptions()
        Dim width As Integer
        Dim height As Integer
        If ComboBox1.SelectedItem IsNot Nothing Then
            Dim selectedItem As String = ComboBox1.SelectedItem
            Dim parts() As String = selectedItem.Split(" ")(0).Split("x")
            width = parts(0)
            height = parts(1)
        Else
            width = 640
            height = 480
        End If
        Dim bioHeight As Integer = 480
        Dim bioWidth As Integer = bioHeight * (width / height)

        Dim alphaFilename As String = Configuration.GetString("Alpha", "EXEName", "<none>")
        If alphaFilename IsNot "<none>" Then
            Dim alphaFolder As String = System.IO.Path.GetDirectoryName(alphaFilename)
            Dim BioINI As New INIFile(System.IO.Path.Combine(alphaFolder, BionicleConfigFilename))
            BioINI.SetString("GraphicsOptions", "ResolutionW", bioWidth)
            BioINI.SetString("GraphicsOptions", "ResolutionH", bioHeight)
            BioINI.Write(System.IO.Path.Combine(alphaFolder, BionicleConfigFilename))

            Dim voodooINI As New INIFile(System.IO.Path.Combine(alphaFolder, dgVoodooConfigFilename))
            voodooINI.SetString("DirectX", "Resolution", "h:" & width & ", v:" & height)
            voodooINI.Write(System.IO.Path.Combine(alphaFolder, dgVoodooConfigFilename))
        End If

        Dim betaFilename As String = Configuration.GetString("Beta", "EXEName", "<none>")
        If betaFilename IsNot "<none>" Then
            Dim betaFolder As String = System.IO.Path.GetDirectoryName(betaFilename)
            Dim BioINI As New INIFile(System.IO.Path.Combine(betaFolder, BionicleConfigFilename))
            BioINI.SetString("GraphicsOptions", "ResolutionW", bioWidth)
            BioINI.SetString("GraphicsOptions", "ResolutionH", bioHeight)
            BioINI.Write(System.IO.Path.Combine(betaFolder, BionicleConfigFilename))

            Dim voodooINI As New INIFile(System.IO.Path.Combine(betaFolder, dgVoodooConfigFilename))
            voodooINI.SetString("DirectX", "Resolution", "h:" & width & ", v:" & height)
            voodooINI.Write(System.IO.Path.Combine(betaFolder, dgVoodooConfigFilename))
        End If

        Dim rebuiltFilename As String = Configuration.GetString("Rebuilt", "EXEName", "<none>")
        If rebuiltFilename IsNot "<none>" Then
            Dim rebuiltFolder As String = System.IO.Path.GetDirectoryName(rebuiltFilename)
            Dim BioINI As New INIFile(System.IO.Path.Combine(rebuiltFolder, BionicleConfigFilename))
            BioINI.SetString("GraphicsOptions", "ResolutionW", bioWidth)
            BioINI.SetString("GraphicsOptions", "ResolutionH", bioHeight)
            BioINI.Write(System.IO.Path.Combine(rebuiltFolder, BionicleConfigFilename))

            Dim voodooINI As New INIFile(System.IO.Path.Combine(rebuiltFolder, dgVoodooConfigFilename))
            voodooINI.SetString("DirectX", "Resolution", "h:" & width & ", v:" & height)
            voodooINI.Write(System.IO.Path.Combine(rebuiltFolder, dgVoodooConfigFilename))
        End If

        '   Dim gameFilename As String = Configuration.GetString("Beta", "EXEName", "<none>")
        '   gameFilename = Configuration.GetString("Rebuilt", "EXEName", "<none>")
        '   If gameFilename = "<none>" Then
        '   Exit Sub
        '   End If

        If TestMenu.Checked And betaFilename IsNot "<none>" Then
            Dim betaFolder As String = System.IO.Path.GetDirectoryName(betaFilename)
            Dim BioINI As New INIFile(System.IO.Path.Combine(betaFolder, BionicleConfigFilename))
            BioINI.SetString("Misc", "Cheatmenu", "LEGOTester")
            BioINI.Write(System.IO.Path.Combine(betaFolder, BionicleConfigFilename))
        Else
            If betaFilename IsNot "<none>" Then
                Dim betaFolder As String = System.IO.Path.GetDirectoryName(betaFilename)
                Dim BioINI As New INIFile(System.IO.Path.Combine(betaFolder, BionicleConfigFilename))
                BioINI.RemoveString("Misc", "Cheatmenu")
                BioINI.Write(System.IO.Path.Combine(betaFolder, BionicleConfigFilename))
            End If
        End If

        If TestMenu.Checked And rebuiltFilename IsNot "<none>" Then
            Dim rebuiltFolder As String = System.IO.Path.GetDirectoryName(rebuiltFilename)
            Dim BioINI As New INIFile(System.IO.Path.Combine(rebuiltFolder, BionicleConfigFilename))
            BioINI.SetString("Misc", "Cheatmenu", "LEGOTester")
            BioINI.Write(System.IO.Path.Combine(rebuiltFolder, BionicleConfigFilename))
        Else
            If rebuiltFilename IsNot "<none>" Then
                Dim rebuiltFolder As String = System.IO.Path.GetDirectoryName(rebuiltFilename)
                Dim BioINI As New INIFile(System.IO.Path.Combine(rebuiltFolder, BionicleConfigFilename))
                BioINI.Write(System.IO.Path.Combine(rebuiltFolder, BionicleConfigFilename))
                BioINI.RemoveString("Misc", "Cheatmenu")
                BioINI.Write(System.IO.Path.Combine(rebuiltFolder, BionicleConfigFilename))
            End If
        End If
    End Sub

    '----------------------------------------------------------------
    'Save Config On Exit
    '----------------------------------------------------------------

    Private Sub Form1_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        Configuration.Write(LauncherConfigFilename)
        ApplyGameOptions()
    End Sub

    Private Sub ComboBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox1.SelectedIndexChanged

    End Sub

    Private Sub Form1_Shown(sender As Object, e As EventArgs) Handles MyBase.Shown
        CheckLauncherUpdates()
    End Sub
End Class