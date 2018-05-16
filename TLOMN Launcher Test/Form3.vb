Public Class Form3
    Private Sub Form3_Shown(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Shown
        Me.Visible = False
        Me.Hide()

        '----------------------------------------------------------------
        'Check patch version
        '----------------------------------------------------------------

        Try
            My.Computer.Network.DownloadFile(
            "http://biomediaproject.com/bmp/files/gms/tlomn/Launcher/Patch/version.txt",
            "Temp\version.txt", "", "", True, 1000, True)
        Catch
            Me.Close()
            Process.Start("..\LEGO Bionicle\LEGO Bionicle.exe")
            MyBase.Close()
            Form1.Close()
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
        MyBase.Close()
        Form1.Close()
    End Sub
End Class