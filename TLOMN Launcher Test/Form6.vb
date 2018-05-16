Public Class Form6
    Private Sub Form3_Shown(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Shown
        Me.Visible = False
        Me.Hide()

        '----------------------------------------------------------------
        'Check patch version
        '----------------------------------------------------------------

        Try
            My.Computer.Network.DownloadFile(
            "http://biomediaproject.com/bmp/files/gms/tlomn/Launcher/Patch/versionbeta.txt",
            "Temp\versionbeta.txt", "", "", True, 1000, True)
        Catch
            Me.Close()
            Process.Start("..\LEGO Bionicle (Beta)\LEGO Bionicle.exe")
            MyBase.Close()
            Form1.Close()
            Exit Sub
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
        MyBase.Close()
        Form1.Close()
    End Sub
End Class