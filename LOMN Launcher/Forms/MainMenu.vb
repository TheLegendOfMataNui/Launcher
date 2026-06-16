Imports CefSharp.WinForms
Imports CefSharp
Imports System.Net
Imports System.IO
Imports System.Environment

Public Class MainMenu

    '----------------------------------------------------------------
    'Startup Checks
    '----------------------------------------------------------------

    Private Const LauncherConfigFilename As String = "LOMNLauncher.ini"

    Public Configuration As INIFile

    ' AppData Storage Paths for Launcher State
    Public AppDataLauncherFolder As String = ""
    Private AppDataTempFolder As String = ""

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

    Private Sub MainMenu_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ' Setup our new AppData paths
        Dim roamingPath As String = System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData)
        AppDataLauncherFolder = System.IO.Path.Combine(roamingPath, "Litestone Studios", "LOMN-Launcher")
        AppDataTempFolder = System.IO.Path.Combine(AppDataLauncherFolder, "Temp")

        ' === LAUNCHER MIGRATION LOGIC ===
        Try
            If Not System.IO.Directory.Exists(AppDataTempFolder) Then
                System.IO.Directory.CreateDirectory(AppDataTempFolder)
            End If

            ' 1. Migrate legacy version AND config files if they exist locally but not in AppData yet
            Dim localFilesToMigrate() As String = {"version.txt", "versionL.txt", LauncherConfigFilename}
            For Each fileName As String In localFilesToMigrate
                Dim localFile As String = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName)
                Dim targetFile As String = System.IO.Path.Combine(AppDataLauncherFolder, fileName)

                If System.IO.File.Exists(localFile) Then
                    If Not System.IO.File.Exists(targetFile) Then
                        System.IO.File.Move(localFile, targetFile)
                    Else
                        System.IO.File.Delete(localFile)
                    End If
                End If
            Next

            ' 2. If an old local Temp directory exists, move any stranded files over
            Dim localTempDir As String = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Temp")
            If System.IO.Directory.Exists(localTempDir) Then
                For Each fileInTemp In System.IO.Directory.GetFiles(localTempDir)
                    Dim fileName As String = System.IO.Path.GetFileName(fileInTemp)
                    Dim targetTempFile As String = System.IO.Path.Combine(AppDataTempFolder, fileName)

                    If Not System.IO.File.Exists(targetTempFile) Then
                        System.IO.File.Move(fileInTemp, targetTempFile)
                    Else
                        System.IO.File.Delete(fileInTemp)
                    End If
                Next
                System.IO.Directory.Delete(localTempDir, True)
            End If

            ' === FINAL CLEANUP PASS ===
            For Each fileName As String In localFilesToMigrate
                Dim localFile As String = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName)
                If System.IO.File.Exists(localFile) Then
                    System.IO.File.Delete(localFile)
                End If
            Next

        Catch ex As Exception
            Console.WriteLine("Launcher migration failed: " & ex.Message)
        End Try

        ' Read the LOMNLauncher config from the AppData directory
        Dim appDataConfigPath As String = System.IO.Path.Combine(AppDataLauncherFolder, LauncherConfigFilename)
        Configuration = New INIFile(appDataConfigPath)
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

    Private Sub AlphaButton_Click(sender As Object, e As EventArgs) Handles AlphaButton.Click
        Try
            Dim LatestAlphaRelease As GitHubRelease = GitHubAPI.GetLatestRelease(GitHubAPI.OrganizationIdentifier, GitHubAPI.LOMNAlphaRepoIdentifier, False)
            Dim gameFilename As String = Configuration.GetString("Alpha", "EXEName", "<none>")
            If gameFilename = "none" Or Not My.Computer.FileSystem.FileExists(gameFilename) Then
                Dim choice As DialogResult
                choice = MessageBox.Show("Could not find the game. Is it downloaded?", "The Legend of Mata Nui Alpha", MessageBoxButtons.YesNoCancel)
                If choice = DialogResult.Yes Then
                    Dim browser As New OpenFileDialog()
                    If browser.ShowDialog() = DialogResult.OK Then
                        gameFilename = browser.FileName
                        Configuration.SetString("Alpha", "EXEName", gameFilename)
                    End If
                ElseIf choice = DialogResult.No Then
                    NewsPage.Load("https://810nicleday.com/LOMN/PatchNotesLoading.html")
                    GitHubAPI.DownloadFile(LatestAlphaRelease.AssetDownloadURL, System.IO.Path.Combine(AppDataTempFolder, "Setup.exe"))
                    NewsPage.Load("https://810nicleday.com/LOMN/PatchNotes.html")
                    Process.Start(System.IO.Path.Combine(AppDataTempFolder, "Setup.exe")).WaitForExit()
                    gameFilename = My.Computer.FileSystem.ReadAllText(System.IO.Path.Combine(AppDataTempFolder, "AlphaDir.txt"))
                    Configuration.SetString("Alpha", "EXEName", gameFilename)
                End If
            End If
            DoPatchAlpha()
        Catch
        End Try
    End Sub

    Public Sub DoPatchAlpha()
        Try
            Dim LatestAlphaRelease As GitHubRelease = GitHubAPI.GetLatestRelease(GitHubAPI.OrganizationIdentifier, GitHubAPI.LOMNAlphaRepoIdentifier, False)
            Dim gameFilename As String = Configuration.GetString("Alpha", "EXEName", "<none>")
            Dim GameFolder As String = System.IO.Path.GetDirectoryName(Configuration.GetString("Alpha", "EXEName", "<none>"))
            Dim GitTag As String = LatestAlphaRelease.TagName
            Dim LocalVer As String = ""
            LocalVer = My.Computer.FileSystem.ReadAllText(System.IO.Path.Combine(GameFolder, "version.txt"))
            If GitTag <> LocalVer Then
                Dim choice As MsgBoxResult = MsgBox("A new update is available! Would you like to download it?", MsgBoxStyle.YesNo)
                If choice = MsgBoxResult.Yes Then
                    NewsPage.Load("https://810nicleday.com/LOMN/PatchNotesLoading.html")
                    GitHubAPI.DownloadFile(LatestAlphaRelease.AssetDownloadURL, System.IO.Path.Combine(AppDataTempFolder, "Setup.exe"))
                    NewsPage.Load("https://810nicleday.com/LOMN/PatchNotes.html")
                    Process.Start(System.IO.Path.Combine(AppDataTempFolder, "Setup.exe")).WaitForExit()
                    gameFilename = My.Computer.FileSystem.ReadAllText(System.IO.Path.Combine(AppDataTempFolder, "AlphaDir.txt"))
                    Configuration.SetString("Alpha", "EXEName", gameFilename)
                End If
            End If
        Catch
        End Try
        LaunchAlpha()
    End Sub

    Public Sub LaunchAlpha()
        Dim appDataConfigPath As String = System.IO.Path.Combine(AppDataLauncherFolder, LauncherConfigFilename)
        Configuration.Write(appDataConfigPath)
        Dim directoryName As String = AppDataTempFolder
        For Each deleteFile In Directory.GetFiles(directoryName, "*.*", SearchOption.TopDirectoryOnly)
            File.Delete(deleteFile)
        Next
        Dim gameFilename As String = Configuration.GetString("Alpha", "EXEName", "<none>")
        Process.Start(New ProcessStartInfo(gameFilename) With {.WorkingDirectory = IO.Path.GetDirectoryName(gameFilename)})
        Me.Close()
    End Sub

    '----------------------------------------------------------------
    'Perform game tests & launch (Beta)
    '----------------------------------------------------------------

    Private Sub BetaButton_Click(sender As Object, e As EventArgs) Handles BetaButton.Click
        Try
            Dim LatestBetaRelease As GitHubRelease = GitHubAPI.GetLatestRelease(GitHubAPI.OrganizationIdentifier, GitHubAPI.LOMNBetaRepoIdentifier, False)
            Dim gameFilename As String = Configuration.GetString("Beta", "EXEName", "<none>")
            If gameFilename = "none" Or Not My.Computer.FileSystem.FileExists(gameFilename) Then
                Dim choice As DialogResult
                choice = MessageBox.Show("Could not find the game. Is it downloaded?", "The Legend of Mata Nui Beta", MessageBoxButtons.YesNoCancel)
                If choice = DialogResult.Yes Then
                    Dim browser As New OpenFileDialog()
                    If browser.ShowDialog() = DialogResult.OK Then
                        gameFilename = browser.FileName
                        Configuration.SetString("Beta", "EXEName", gameFilename)
                    End If
                ElseIf choice = DialogResult.No Then
                    NewsPage.Load("https://810nicleday.com/LOMN/PatchNotesLoading.html")
                    GitHubAPI.DownloadFile(LatestBetaRelease.AssetDownloadURL, System.IO.Path.Combine(AppDataTempFolder, "Setup.exe"))
                    NewsPage.Load("https://810nicleday.com/LOMN/PatchNotes.html")
                    Process.Start(System.IO.Path.Combine(AppDataTempFolder, "Setup.exe")).WaitForExit()
                    gameFilename = My.Computer.FileSystem.ReadAllText(System.IO.Path.Combine(AppDataTempFolder, "BetaDir.txt"))
                    Configuration.SetString("Beta", "EXEName", gameFilename)
                End If
            End If
            DoPatchBeta()
        Catch
        End Try
    End Sub

    Public Sub DoPatchBeta()
        Try
            Dim LatestBetaRelease As GitHubRelease = GitHubAPI.GetLatestRelease(GitHubAPI.OrganizationIdentifier, GitHubAPI.LOMNBetaRepoIdentifier, False)
            Dim gameFilename As String = Configuration.GetString("Beta", "EXEName", "<none>")
            Dim GameFolder As String = System.IO.Path.GetDirectoryName(Configuration.GetString("Beta", "EXEName", "<none>"))
            Dim GitTag As String = LatestBetaRelease.TagName
            Dim LocalVer As String = ""
            LocalVer = My.Computer.FileSystem.ReadAllText(System.IO.Path.Combine(GameFolder, "version.txt"))
            If GitTag <> LocalVer Then
                Dim choice As MsgBoxResult = MsgBox("A new update is available! Would you like to download it?", MsgBoxStyle.YesNo)
                If choice = MsgBoxResult.Yes Then
                    NewsPage.Load("https://810nicleday.com/LOMN/PatchNotesLoading.html")
                    GitHubAPI.DownloadFile(LatestBetaRelease.AssetDownloadURL, System.IO.Path.Combine(AppDataTempFolder, "Setup.exe"))
                    NewsPage.Load("https://810nicleday.com/LOMN/PatchNotes.html")
                    Process.Start(System.IO.Path.Combine(AppDataTempFolder, "Setup.exe")).WaitForExit()
                    gameFilename = My.Computer.FileSystem.ReadAllText(System.IO.Path.Combine(AppDataTempFolder, "BetaDir.txt"))
                    Configuration.SetString("Beta", "EXEName", gameFilename)
                End If
            End If
        Catch
        End Try
        LaunchBeta()
    End Sub

    Public Sub LaunchBeta()
        Dim appDataConfigPath As String = System.IO.Path.Combine(AppDataLauncherFolder, LauncherConfigFilename)
        Configuration.Write(appDataConfigPath)
        Dim directoryName As String = AppDataTempFolder
        For Each deleteFile In Directory.GetFiles(directoryName, "*.*", SearchOption.TopDirectoryOnly)
            File.Delete(deleteFile)
        Next
        Dim gameFilename As String = Configuration.GetString("Beta", "EXEName", "<none>")
        Process.Start(New ProcessStartInfo(gameFilename) With {.WorkingDirectory = IO.Path.GetDirectoryName(gameFilename)})
        Me.Close()
    End Sub

    '----------------------------------------------------------------
    'Perform game tests & launch (Rebuilt)
    '----------------------------------------------------------------

    Private Sub RebuiltButton_Click(sender As Object, e As EventArgs) Handles RebuiltButton.Click
        Try
            Dim LatestRebuiltRelease As GitHubRelease = GitHubAPI.GetLatestRelease(GitHubAPI.OrganizationIdentifier, GitHubAPI.LOMNRebuiltRepoIdentifier, False)
            Dim gameFilename As String = Configuration.GetString("Rebuilt", "EXEName", "<none>")
            If gameFilename = "none" Or Not My.Computer.FileSystem.FileExists(gameFilename) Then
                Dim choice As DialogResult
                choice = MessageBox.Show("Could not find the game. Is it downloaded?", "The Legend of Mata Nui Rebuilt", MessageBoxButtons.YesNoCancel)
                If choice = DialogResult.Yes Then
                    Dim browser As New OpenFileDialog()
                    If browser.ShowDialog() = DialogResult.OK Then
                        gameFilename = browser.FileName
                        Configuration.SetString("Rebuilt", "EXEName", gameFilename)
                    End If
                ElseIf choice = DialogResult.No Then
                    NewsPage.Load("https://810nicleday.com/LOMN/PatchNotesLoading.html")
                    GitHubAPI.DownloadFile(LatestRebuiltRelease.AssetDownloadURL, System.IO.Path.Combine(AppDataTempFolder, "Setup.exe"))
                    NewsPage.Load("https://810nicleday.com/LOMN/PatchNotes.html")
                    Process.Start(System.IO.Path.Combine(AppDataTempFolder, "Setup.exe")).WaitForExit()
                    gameFilename = My.Computer.FileSystem.ReadAllText(System.IO.Path.Combine(AppDataTempFolder, "RebuiltDir.txt"))
                    Configuration.SetString("Rebuilt", "EXEName", gameFilename)
                End If
            End If
            DoPatchRebuilt()
        Catch
        End Try
    End Sub

    Public Sub DoPatchRebuilt()
        Try
            Dim LatestRebuiltRelease As GitHubRelease = GitHubAPI.GetLatestRelease(GitHubAPI.OrganizationIdentifier, GitHubAPI.LOMNRebuiltRepoIdentifier, False)
            Dim gameFilename As String = Configuration.GetString("Rebuilt", "EXEName", "<none>")
            Dim GameFolder As String = System.IO.Path.GetDirectoryName(Configuration.GetString("Rebuilt", "EXEName", "<none>"))
            Dim GitTag As String = LatestRebuiltRelease.TagName
            Dim LocalVer As String = ""
            LocalVer = My.Computer.FileSystem.ReadAllText(System.IO.Path.Combine(GameFolder, "version.txt"))
            If GitTag <> LocalVer Then
                Dim choice As MsgBoxResult = MsgBox("A new update is available! Would you like to download it?", MsgBoxStyle.YesNo)
                If choice = MsgBoxResult.Yes Then
                    NewsPage.Load("https://810nicleday.com/LOMN/PatchNotesLoading.html")
                    GitHubAPI.DownloadFile(LatestRebuiltRelease.AssetDownloadURL, System.IO.Path.Combine(AppDataTempFolder, "Setup.exe"))
                    NewsPage.Load("https://810nicleday.com/LOMN/PatchNotes.html")
                    Process.Start(System.IO.Path.Combine(AppDataTempFolder, "Setup.exe")).WaitForExit()
                    gameFilename = My.Computer.FileSystem.ReadAllText(System.IO.Path.Combine(AppDataTempFolder, "RebuiltDir.txt"))
                    Configuration.SetString("Rebuilt", "EXEName", gameFilename)
                End If
            End If
        Catch
        End Try
        LaunchRebuilt()
    End Sub

    Public Sub LaunchRebuilt()
        Dim appDataConfigPath As String = System.IO.Path.Combine(AppDataLauncherFolder, LauncherConfigFilename)
        Configuration.Write(appDataConfigPath)
        Dim directoryName As String = AppDataTempFolder
        For Each deleteFile In Directory.GetFiles(directoryName, "*.*", SearchOption.TopDirectoryOnly)
            File.Delete(deleteFile)
        Next
        Dim gameFilename As String = Configuration.GetString("Rebuilt", "EXEName", "<none>")
        Process.Start(New ProcessStartInfo(gameFilename) With {.WorkingDirectory = IO.Path.GetDirectoryName(gameFilename)})
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
            Dim appDataVersionFile As String = System.IO.Path.Combine(AppDataLauncherFolder, "version.txt")
            Dim appDataVersionLFile As String = System.IO.Path.Combine(AppDataLauncherFolder, "versionL.txt")

            If System.IO.File.Exists(appDataVersionFile) Then
                LocalVer = My.Computer.FileSystem.ReadAllText(appDataVersionFile)
            ElseIf System.IO.File.Exists(appDataVersionLFile) Then
                LocalVer = My.Computer.FileSystem.ReadAllText(appDataVersionLFile)
            End If

            If GitTag <> LocalVer Then
                Dim msgRslt As MsgBoxResult = MsgBox("A new launcher update is available!" & vbCrLf & "Would you like to download it?", MsgBoxStyle.YesNo)
                If msgRslt = MsgBoxResult.Yes Then
                    NewsPage.Load("https://810nicleday.com/LOMN/PatchNotesLoading.html")

                    Dim installerPath As String = System.IO.Path.Combine(AppDataTempFolder, "BIONICLE Launcher Launcher Installer.exe")
                    GitHubAPI.DownloadFile(LatestLauncherRelease.AssetDownloadURL, installerPath)
                    Process.Start(installerPath)

                    Me.Close()
                End If
            End If
        Catch
        End Try
    End Sub

    '----------------------------------------------------------------
    'Launch Configuration Submenu Popup
    '----------------------------------------------------------------

    Private Sub ConfigButton_Click(sender As Object, e As EventArgs) Handles ConfigButton.Click
        Using settingsPopup As New ConfigMenu(Me)
            settingsPopup.ShowDialog(Me)
        End Using
    End Sub

    '----------------------------------------------------------------
    'Sidebar Links
    '----------------------------------------------------------------

    Private Sub PictureBox1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BioMediaLink.Click
        Process.Start("http://biomediaproject.com/")
    End Sub

    Private Sub PictureBox2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DiscordLink.Click
        Process.Start("https://discord.gg/pZwgHwq")
    End Sub

    Private Sub PictureBox3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles YouTubeLink.Click
        Process.Start("http://youtube.com/LitestoneStudios/")
    End Sub

    Private Sub PictureBox4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles GitHubLink.Click
        Process.Start("https://github.com/TheLegendOfMataNui/game-issues/issues")
    End Sub

    '----------------------------------------------------------------
    'Save Config On Exit
    '----------------------------------------------------------------

    Private Sub MainMenu_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        Dim appDataConfigPath As String = System.IO.Path.Combine(AppDataLauncherFolder, LauncherConfigFilename)
        Configuration.Write(appDataConfigPath)
    End Sub

    Private Sub MainMenu_Shown(sender As Object, e As EventArgs) Handles MyBase.Shown
        CheckLauncherUpdates()
    End Sub
End Class