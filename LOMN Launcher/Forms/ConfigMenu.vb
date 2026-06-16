Imports System.IO

Public Class ConfigMenu
    Private LauncherMain As MainMenu
    Private IgnoreUI As Boolean = True

    Private Const dgVoodooConfigFilename As String = "dgVoodoo.conf"
    Private Const BionicleConfigFilename As String = "Bionicle.ini"

    ' Accept a reference to the MainMenu instance on initialization
    Public Sub New(mainForm As MainMenu)
        InitializeComponent()
        LauncherMain = mainForm
    End Sub

    Private Sub ConfigMenu_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        IgnoreUI = True

        ' 1. Add the "Custom..." option to the dropdown if it isn't already there
        If Not ResolutionDropdown.Items.Contains("Custom...") Then
            ResolutionDropdown.Items.Add("Custom...")
        End If

        ' 2. Load the existing resolution string & fullscreen flag from the active game files
        LoadCurrentGameSettings()

        ' 3. Scan the active Bionicle.ini file to initialize checkbox states
        Dim isTestMenuEnabled As Boolean = False
        Dim isDebugMenuEnabled As Boolean = False
        Dim isEasterEggEnabled As Boolean = False

        Dim alphaFilename As String = LauncherMain.Configuration.GetString("Alpha", "EXEName", "<none>")
        Dim betaFilename As String = LauncherMain.Configuration.GetString("Beta", "EXEName", "<none>")
        Dim rebuiltFilename As String = LauncherMain.Configuration.GetString("Rebuilt", "EXEName", "<none>")

        Dim initialFolder As String = "<none>"
        If betaFilename <> "<none>" Then
            initialFolder = Path.GetDirectoryName(betaFilename)
        ElseIf rebuiltFilename <> "<none>" Then
            initialFolder = Path.GetDirectoryName(rebuiltFilename)
        ElseIf alphaFilename <> "<none>" Then
            initialFolder = Path.GetDirectoryName(alphaFilename)
        End If

        If initialFolder <> "<none>" Then
            Dim bioINIFile As String = Path.Combine(initialFolder, BionicleConfigFilename)
            If File.Exists(bioINIFile) Then
                Dim bioINI As New INIFile(bioINIFile)
                isTestMenuEnabled = (bioINI.GetString("Misc", "Cheatmenu", "<none>") = "LEGOTester")
                isDebugMenuEnabled = (bioINI.GetString("Cheat", "Cheat", "<none>") = "NoGreenCapes")
                isEasterEggEnabled = (bioINI.GetString("Cheat", "Urinate", "<none>") = "Pohatu")
            End If
        End If

        TestMenuCheckbox.Checked = isTestMenuEnabled
        DebugMenuCheckbox.Checked = isDebugMenuEnabled
        CheatCheckbox.Checked = isEasterEggEnabled

        ' 4. Evaluate whether custom boxes should be usable based on the initial selection
        ToggleCustomInputState()

        IgnoreUI = False
    End Sub

    Private Sub LoadCurrentGameSettings()
        Dim activeFolder As String = GetActiveGameFolder()
        If activeFolder IsNot Nothing Then
            Dim voodooFile As String = Path.Combine(activeFolder, dgVoodooConfigFilename)
            If File.Exists(voodooFile) Then
                Dim voodooINI As New INIFile(voodooFile)

                Dim screenMode As String = voodooINI.GetString("General", "FullScreenMode", "false").ToLower()
                FullscreenCheckbox.Checked = (screenMode = "true" OrElse screenMode = "1")

                Dim resolutionSetting As String = voodooINI.GetString("DirectX", "Resolution", "h:640, v:480")
                Try
                    Dim parts() As String = resolutionSetting.Split(" ")
                    Dim horizontal As Integer = parts(0).TrimEnd(",").Substring(2)
                    Dim vertical As Integer = parts(1).Substring(2)

                    ' Reconstruct your preset string format: "1920x1080 (16:9)"
                    Dim scale As Integer = GCD(horizontal, vertical)
                    Dim aspect As String = (horizontal / scale) & ":" & (vertical / scale)
                    Dim targetPreset As String = horizontal & "x" & vertical & " (" & aspect & ")"

                    If ResolutionDropdown.Items.Contains(targetPreset) Then
                        ResolutionDropdown.SelectedItem = targetPreset
                    Else
                        ' If it's a completely custom resolution, populate the fields directly
                        CustomResY.Text = horizontal.ToString() ' Custom X Textbox
                        CustomResX.Text = vertical.ToString()   ' Custom Y Textbox
                        ResolutionDropdown.SelectedItem = "Custom..."
                    End If
                Catch
                    ResolutionDropdown.SelectedIndex = 0
                End Try
            End If
        End If
    End Sub

    ' Toggles Enabled property of Custom Fields to prevent input conflicts
    Private Sub ToggleCustomInputState()
        Dim isCustomActive As Boolean = (ResolutionDropdown.SelectedItem IsNot Nothing AndAlso ResolutionDropdown.SelectedItem.ToString() = "Custom...")

        CustomResY.Enabled = isCustomActive ' Designer layout: CustomResY represents X Input
        CustomResX.Enabled = isCustomActive ' Designer layout: CustomResX represents Y Input

        ' Populate placeholders if switching to custom fresh
        If isCustomActive AndAlso (CustomResY.Text = "Custom X" OrElse CustomResY.Text = "") Then
            CustomResY.Text = "1920"
            CustomResX.Text = "1080"
        End If
    End Sub

    ' Monitor the Dropdown menu changes to shift box usability states
    Private Sub ResolutionDropdown_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ResolutionDropdown.SelectedIndexChanged
        If IgnoreUI Then Exit Sub
        ToggleCustomInputState()
    End Sub

    Private Function GetActiveGameFolder() As String
        If LauncherMain.Configuration.GetString("Rebuilt", "EXEName", "<none>") IsNot "<none>" Then
            Return Path.GetDirectoryName(LauncherMain.Configuration.GetString("Rebuilt", "EXEName", "<none>"))
        ElseIf LauncherMain.Configuration.GetString("Beta", "EXEName", "<none>") IsNot "<none>" Then
            Return Path.GetDirectoryName(LauncherMain.Configuration.GetString("Beta", "EXEName", "<none>"))
        ElseIf LauncherMain.Configuration.GetString("Alpha", "EXEName", "<none>") IsNot "<none>" Then
            Return Path.GetDirectoryName(LauncherMain.Configuration.GetString("Alpha", "EXEName", "<none>"))
        End If
        Return Nothing
    End Function

    Private Function GCD(a As Integer, b As Integer) As Integer
        If b = 0 Then Return a Else Return GCD(b, a Mod b)
    End Function

    Private Sub VoodooButton_Click(sender As Object, e As EventArgs) Handles VoodooButton.Click
        Try
            Process.Start("dgVoodooCpl.exe")
        Catch ex As Exception
            MessageBox.Show("Could not find or execute dgVoodooCpl.exe", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub DataFolderButton_Click(sender As Object, e As EventArgs) Handles DataFolderButton.Click
        Try
            ' Resolve the user's roaming AppData directory path
            Dim roamingPath As String = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)
            Dim targetPath As String = Path.Combine(roamingPath, "Litestone Studios")

            ' If the path exists, invoke Windows Explorer to open it directly
            If Directory.Exists(targetPath) Then
                Process.Start("explorer.exe", targetPath)
            Else
                ' Fallback safety check in case the folder hasn't been initialized yet
                MessageBox.Show("The Litestone Studios data folder has not been created yet. Launch a game version first to initialize it.", "Folder Not Found", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If
        Catch ex As Exception
            MessageBox.Show("Failed to open data folder: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub ApplyButton_Click(sender As Object, e As EventArgs) Handles ApplyButton.Click
        ApplyConfigChanges()
        Me.Close()
    End Sub

    Private Sub CancelButton_Click(sender As Object, e As EventArgs) Handles CancelButton.Click
        Me.Close()
    End Sub

    Private Sub ApplyConfigChanges()
        Dim width As Integer = 640
        Dim height As Integer = 480

        ' 1. Resolve values based on Dropdown Preset selection vs Custom Input Mode
        If ResolutionDropdown.SelectedItem IsNot Nothing Then
            Dim selectedItem As String = ResolutionDropdown.SelectedItem.ToString()

            If selectedItem = "Custom..." Then
                ' Parse raw values straight from the visual text boxes
                Integer.TryParse(CustomResY.Text, width)  ' Layout field labeled "Custom X"
                Integer.TryParse(CustomResX.Text, height) ' Layout field labeled "Custom Y"
            ElseIf selectedItem <> "Select Resolution..." Then
                ' Standard Preset processing
                Dim parts() As String = selectedItem.Split(" ")(0).Split("x")
                width = Integer.Parse(parts(0))
                height = Integer.Parse(parts(1))
            End If
        End If

        ' Absolute fallback guard limits to prevent division by zero
        If height <= 0 Then height = 480
        If width <= 0 Then width = 640

        ' 2. ASPECT RATIO GUARD: Enforce a minimum aspect ratio of 4:3 (1.3333...)
        ' If the user enters a narrower aspect ratio, clamp the width to exactly 4:3 of the height
        Dim currentAspect As Double = width / height
        Dim minAspect As Double = 4.0 / 3.0 ' 1.33333333333333

        If currentAspect < minAspect Then
            ' Automatically upscale the width to satisfy the 4:3 boundary requirement
            width = CInt(Math.Round(height * minAspect))

            ' Optional: Update the textbox values on screen so the user sees the correction
            If ResolutionDropdown.SelectedItem.ToString() = "Custom..." Then
                CustomResY.Text = width.ToString()
            End If

            MessageBox.Show("The entered resolution is narrower than 4:3, which causes graphical errors. The width has been adjusted to " & width & " to maintain compatibility.", "Aspect Ratio Corrected", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End If

        ' 3. MATH LAYER: Calculate exact engine resolution properties dynamically on the fly
        Dim bioHeight As Integer = 480
        Dim bioWidth As Integer = CInt(Math.Round(bioHeight * (width / height)))

        Dim fullScreenFlag As String = If(FullscreenCheckbox.Checked, "true", "false")

        ' Build directory map
        Dim targetPaths As New List(Of String)()
        Dim alphaFilename As String = LauncherMain.Configuration.GetString("Alpha", "EXEName", "<none>")
        Dim betaFilename As String = LauncherMain.Configuration.GetString("Beta", "EXEName", "<none>")
        Dim rebuiltFilename As String = LauncherMain.Configuration.GetString("Rebuilt", "EXEName", "<none>")

        If alphaFilename <> "<none>" Then targetPaths.Add(System.IO.Path.GetDirectoryName(alphaFilename))
        If betaFilename <> "<none>" Then targetPaths.Add(System.IO.Path.GetDirectoryName(betaFilename))
        If rebuiltFilename <> "<none>" Then targetPaths.Add(System.IO.Path.GetDirectoryName(rebuiltFilename))

        ' Flush configuration settings down to individual targeted deployment paths
        For Each folder In targetPaths
            ' Write Engine Graphics settings
            Dim BioINI As New INIFile(System.IO.Path.Combine(folder, BionicleConfigFilename))
            BioINI.SetString("GraphicsOptions", "ResolutionW", bioWidth)
            BioINI.SetString("GraphicsOptions", "ResolutionH", bioHeight)

            ' Write Test Menu setting
            If TestMenuCheckbox.Checked Then
                BioINI.SetString("Misc", "Cheatmenu", "LEGOTester")
            Else
                BioINI.RemoveString("Misc", "Cheatmenu")
            End If

            ' Write Debug Menu Toggle ("Cheat=NoGreenCapes")
            If DebugMenuCheckbox.Checked Then
                BioINI.SetString("Cheat", "Cheat", "NoGreenCapes")
            Else
                BioINI.RemoveString("Cheat", "Cheat")
            End If

            ' Write Easter Eggs Toggle ("Urinate=Pohatu")
            If CheatCheckbox.Checked Then
                BioINI.SetString("Cheat", "Urinate", "Pohatu")
            Else
                BioINI.RemoveString("Cheat", "Urinate")
            End If

            ' Commit changes to file
            BioINI.Write(System.IO.Path.Combine(folder, BionicleConfigFilename))

            ' Write dgVoodoo display configuration settings
            Dim voodooINI As New INIFile(System.IO.Path.Combine(folder, dgVoodooConfigFilename))
            voodooINI.SetString("DirectX", "Resolution", "h:" & width & ", v:" & height)
            voodooINI.SetString("General", "FullScreenMode", fullScreenFlag)
            voodooINI.Write(System.IO.Path.Combine(folder, dgVoodooConfigFilename))
        Next
    End Sub
End Class