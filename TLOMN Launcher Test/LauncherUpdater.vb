Public Module LauncherUpdater
    Public Sub DoUpdateCheck()
        '----------------------------------------------------------------
        'Old Version Cleanup
        '----------------------------------------------------------------

        Try
            My.Computer.FileSystem.DeleteDirectory("..\LEGO Bionicle\Launcher", FileIO.DeleteDirectoryOption.DeleteAllContents)
            'Form1.Close()
        Catch ex As Exception
            'Form1.Close()
        End Try

        '----------------------------------------------------------------
        'Check Launcher version
        '----------------------------------------------------------------

        Try
            My.Computer.Network.DownloadFile(
            "http://biomediaproject.com/bmp/files/gms/tlomn/Launcher/versionL.txt",
            "Temp\versionL.txt", "", "", True, 1000, True)

        Catch
            Exit Sub
        End Try

        Dim text1 As String = My.Computer.FileSystem.ReadAllText("Temp\versionL.txt")
        Dim text2 As String = My.Computer.FileSystem.ReadAllText("versionL.txt")
        If text1 <> text2 Then

            '----------------------------------------------------------------
            'If mismatch, show download prompt
            '----------------------------------------------------------------

            Dim msgRslt As MsgBoxResult = MsgBox("A new launcher update is available! Would you like to download it?", MsgBoxStyle.YesNo)
            If msgRslt = MsgBoxResult.Yes Then
                Try
                    My.Computer.Network.DownloadFile(
                   "http://biomediaproject.com/bmp/files/gms/tlomn/Launcher/BIONICLE%20Launcher%20Installer.exe",
                   "Temp\BIONICLE Launcher Installer.exe", "", "", True, 1000, True)
                    Process.Start("Temp\BIONICLE Launcher Installer.exe")
                    Form1.Close()
                Catch
                End Try

                '----------------------------------------------------------------
                'If update declined, do nothing
                '----------------------------------------------------------------

            ElseIf msgRslt = MsgBoxResult.No Then
                Exit Sub
            End If
        End If

    End Sub
End Module