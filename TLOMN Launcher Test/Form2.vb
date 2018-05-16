Public Class Form2
    Private Sub Form2_Shown(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Shown
        Me.Visible = False
        Me.Hide()

        '----------------------------------------------------------------
        'Test for game being installed
        '----------------------------------------------------------------

        If My.Computer.FileSystem.FileExists("..\LEGO Bionicle\LEGO Bionicle.exe") Then
            Form3.Show()

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

                    My.Computer.FileSystem.DeleteFile("..\Data\characters\1hat\Anims\1haB.bhd")
                    My.Computer.FileSystem.DeleteFile("..\Data\characters\1hat\Anims\1haD.bhd")
                    My.Computer.FileSystem.DeleteFile("..\Data\characters\1hat\Anims\1haL.bhd")
                    My.Computer.FileSystem.DeleteFile("..\Data\characters\1hat\Anims\1haR.bhd")
                    My.Computer.FileSystem.DeleteFile("..\Data\characters\1hat\Anims\1haU.bhd")
                    My.Computer.FileSystem.DeleteFile("..\Data\characters\1hat\Anims\1haW.bhd")
                    Form3.Show()
                Catch
                    Me.Close()
                End Try
            End If
        End If
        MyBase.Close()
    End Sub
End Class