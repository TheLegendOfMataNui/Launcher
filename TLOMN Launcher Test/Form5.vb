Public Class Form5
    Private Sub Form2_Shown(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Shown
        Me.Visible = False
        Me.Hide()

        '----------------------------------------------------------------
        'Test for game being installed
        '----------------------------------------------------------------

        If My.Computer.FileSystem.FileExists("..\LEGO Bionicle (Beta)\LEGO Bionicle.exe") Then
            Form6.Show()

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
        MyBase.Close()
    End Sub
End Class