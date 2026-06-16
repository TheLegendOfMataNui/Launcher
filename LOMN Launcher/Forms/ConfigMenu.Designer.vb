<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class ConfigMenu
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(ConfigMenu))
        Me.TestMenuCheckbox = New System.Windows.Forms.CheckBox()
        Me.FullscreenCheckbox = New System.Windows.Forms.CheckBox()
        Me.VoodooButton = New System.Windows.Forms.Button()
        Me.DataFolderButton = New System.Windows.Forms.Button()
        Me.ResolutionDropdown = New System.Windows.Forms.ComboBox()
        Me.SelectResolution = New System.Windows.Forms.PictureBox()
        Me.CustomResX = New System.Windows.Forms.TextBox()
        Me.CustomResY = New System.Windows.Forms.TextBox()
        Me.ApplyButton = New System.Windows.Forms.Button()
        Me.CancelButton = New System.Windows.Forms.Button()
        Me.DebugMenuCheckbox = New System.Windows.Forms.CheckBox()
        Me.CheatCheckbox = New System.Windows.Forms.CheckBox()
        CType(Me.SelectResolution, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'TestMenuCheckbox
        '
        Me.TestMenuCheckbox.AutoSize = True
        Me.TestMenuCheckbox.BackColor = System.Drawing.Color.Transparent
        Me.TestMenuCheckbox.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TestMenuCheckbox.Location = New System.Drawing.Point(22, 42)
        Me.TestMenuCheckbox.Name = "TestMenuCheckbox"
        Me.TestMenuCheckbox.Size = New System.Drawing.Size(93, 19)
        Me.TestMenuCheckbox.TabIndex = 19
        Me.TestMenuCheckbox.Text = "Test Menu"
        Me.TestMenuCheckbox.UseVisualStyleBackColor = False
        '
        'FullscreenCheckbox
        '
        Me.FullscreenCheckbox.AutoSize = True
        Me.FullscreenCheckbox.BackColor = System.Drawing.Color.Transparent
        Me.FullscreenCheckbox.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FullscreenCheckbox.Location = New System.Drawing.Point(22, 21)
        Me.FullscreenCheckbox.Name = "FullscreenCheckbox"
        Me.FullscreenCheckbox.Size = New System.Drawing.Size(93, 19)
        Me.FullscreenCheckbox.TabIndex = 20
        Me.FullscreenCheckbox.Text = "Fullscreen"
        Me.FullscreenCheckbox.UseVisualStyleBackColor = False
        '
        'VoodooButton
        '
        Me.VoodooButton.BackColor = System.Drawing.SystemColors.ControlLight
        Me.VoodooButton.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.VoodooButton.Image = Global.LOMN_Launcher.My.Resources.Resources.ButtonDGV
        Me.VoodooButton.Location = New System.Drawing.Point(134, 107)
        Me.VoodooButton.Name = "VoodooButton"
        Me.VoodooButton.Size = New System.Drawing.Size(153, 27)
        Me.VoodooButton.TabIndex = 21
        Me.VoodooButton.UseVisualStyleBackColor = False
        '
        'DataFolderButton
        '
        Me.DataFolderButton.BackColor = System.Drawing.SystemColors.ControlLight
        Me.DataFolderButton.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.DataFolderButton.Image = Global.LOMN_Launcher.My.Resources.Resources.ButtonGD
        Me.DataFolderButton.Location = New System.Drawing.Point(134, 140)
        Me.DataFolderButton.Name = "DataFolderButton"
        Me.DataFolderButton.Size = New System.Drawing.Size(153, 27)
        Me.DataFolderButton.TabIndex = 22
        Me.DataFolderButton.UseVisualStyleBackColor = False
        '
        'ResolutionDropdown
        '
        Me.ResolutionDropdown.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ResolutionDropdown.DropDownWidth = 150
        Me.ResolutionDropdown.FormattingEnabled = True
        Me.ResolutionDropdown.Items.AddRange(New Object() {"Select Resolution...", "640x480 (4:3)", "852x480 (16:9)", "800x600 (4:3)", "960x720 (4:3)", "1024x576 (16:9)", "1024x768 (4:3)", "1152x864 (4:3)", "1176x664 (16:9)", "1280x720 (16:9)", "1280x960 (4:3)", "1280x1024 (4:3)", "1366x768 (16:9)", "1600x900 (16:9)", "1600x1200 (4:3)", "1920x1080 (16:9)", "1920x1440 (4:3)", "2048x1536 (4:3)", "2560x1440 (16:9)", "2880x2160 (4:3)", "3840x2160 (16:9)", "7680x4320 (16:9)"})
        Me.ResolutionDropdown.Location = New System.Drawing.Point(136, 54)
        Me.ResolutionDropdown.Name = "ResolutionDropdown"
        Me.ResolutionDropdown.Size = New System.Drawing.Size(150, 21)
        Me.ResolutionDropdown.TabIndex = 23
        '
        'SelectResolution
        '
        Me.SelectResolution.BackColor = System.Drawing.SystemColors.ControlLight
        Me.SelectResolution.Image = Global.LOMN_Launcher.My.Resources.Resources.TexRes
        Me.SelectResolution.Location = New System.Drawing.Point(134, 22)
        Me.SelectResolution.Name = "SelectResolution"
        Me.SelectResolution.Size = New System.Drawing.Size(153, 27)
        Me.SelectResolution.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage
        Me.SelectResolution.TabIndex = 24
        Me.SelectResolution.TabStop = False
        '
        'CustomResX
        '
        Me.CustomResX.Location = New System.Drawing.Point(213, 81)
        Me.CustomResX.Name = "CustomResX"
        Me.CustomResX.Size = New System.Drawing.Size(73, 20)
        Me.CustomResX.TabIndex = 25
        Me.CustomResX.Text = "Custom Y"
        Me.CustomResX.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'CustomResY
        '
        Me.CustomResY.Location = New System.Drawing.Point(136, 81)
        Me.CustomResY.Name = "CustomResY"
        Me.CustomResY.Size = New System.Drawing.Size(73, 20)
        Me.CustomResY.TabIndex = 26
        Me.CustomResY.Text = "Custom X"
        Me.CustomResY.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'ApplyButton
        '
        Me.ApplyButton.BackColor = System.Drawing.SystemColors.GradientInactiveCaption
        Me.ApplyButton.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ApplyButton.Image = Global.LOMN_Launcher.My.Resources.Resources.ButtonApply
        Me.ApplyButton.Location = New System.Drawing.Point(21, 107)
        Me.ApplyButton.Name = "ApplyButton"
        Me.ApplyButton.Size = New System.Drawing.Size(108, 27)
        Me.ApplyButton.TabIndex = 27
        Me.ApplyButton.UseVisualStyleBackColor = False
        '
        'CancelButton
        '
        Me.CancelButton.BackColor = System.Drawing.SystemColors.Info
        Me.CancelButton.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CancelButton.Image = Global.LOMN_Launcher.My.Resources.Resources.ButtonCancel
        Me.CancelButton.Location = New System.Drawing.Point(21, 140)
        Me.CancelButton.Name = "CancelButton"
        Me.CancelButton.Size = New System.Drawing.Size(107, 27)
        Me.CancelButton.TabIndex = 28
        Me.CancelButton.UseVisualStyleBackColor = False
        '
        'DebugMenuCheckbox
        '
        Me.DebugMenuCheckbox.AutoSize = True
        Me.DebugMenuCheckbox.BackColor = System.Drawing.Color.Transparent
        Me.DebugMenuCheckbox.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.DebugMenuCheckbox.Location = New System.Drawing.Point(22, 64)
        Me.DebugMenuCheckbox.Name = "DebugMenuCheckbox"
        Me.DebugMenuCheckbox.Size = New System.Drawing.Size(108, 19)
        Me.DebugMenuCheckbox.TabIndex = 29
        Me.DebugMenuCheckbox.Text = "Debug Menu"
        Me.DebugMenuCheckbox.UseVisualStyleBackColor = False
        '
        'CheatCheckbox
        '
        Me.CheatCheckbox.AutoSize = True
        Me.CheatCheckbox.BackColor = System.Drawing.Color.Transparent
        Me.CheatCheckbox.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CheatCheckbox.Location = New System.Drawing.Point(22, 85)
        Me.CheatCheckbox.Name = "CheatCheckbox"
        Me.CheatCheckbox.Size = New System.Drawing.Size(103, 19)
        Me.CheatCheckbox.TabIndex = 30
        Me.CheatCheckbox.Text = "Easter Eggs"
        Me.CheatCheckbox.UseVisualStyleBackColor = False
        '
        'ConfigMenu
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackgroundImage = Global.LOMN_Launcher.My.Resources.Resources.po_lev_10xx_Dirt2
        Me.ClientSize = New System.Drawing.Size(308, 187)
        Me.Controls.Add(Me.CheatCheckbox)
        Me.Controls.Add(Me.DebugMenuCheckbox)
        Me.Controls.Add(Me.CancelButton)
        Me.Controls.Add(Me.ApplyButton)
        Me.Controls.Add(Me.CustomResY)
        Me.Controls.Add(Me.CustomResX)
        Me.Controls.Add(Me.SelectResolution)
        Me.Controls.Add(Me.ResolutionDropdown)
        Me.Controls.Add(Me.DataFolderButton)
        Me.Controls.Add(Me.VoodooButton)
        Me.Controls.Add(Me.FullscreenCheckbox)
        Me.Controls.Add(Me.TestMenuCheckbox)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "ConfigMenu"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Game Configuration"
        CType(Me.SelectResolution, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents TestMenuCheckbox As CheckBox
    Friend WithEvents FullscreenCheckbox As CheckBox
    Friend WithEvents VoodooButton As Button
    Friend WithEvents DataFolderButton As Button
    Friend WithEvents ResolutionDropdown As ComboBox
    Friend WithEvents SelectResolution As PictureBox
    Friend WithEvents CustomResX As TextBox
    Friend WithEvents CustomResY As TextBox
    Friend WithEvents ApplyButton As Button
    Friend WithEvents CancelButton As Button
    Friend WithEvents DebugMenuCheckbox As CheckBox
    Friend WithEvents CheatCheckbox As CheckBox
End Class
