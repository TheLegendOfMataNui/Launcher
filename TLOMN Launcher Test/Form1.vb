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

    Private Sub Form4_Load(ByVal sender As System.Object, ByVal e _
    As System.EventArgs) Handles MyBase.Load
        Form4.Show()
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
        Form2.Show()
    End Sub

    '----------------------------------------------------------------
    'Perform game tests & launch (Beta)
    '----------------------------------------------------------------

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        Form5.Show()
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

    '----------------------------------------------------------------
    'Sidebar links
    '----------------------------------------------------------------

    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e _
    As System.EventArgs) Handles MyBase.Load
        LinkLabel1.Text = "Beaverhouse Discord"
        LinkLabel1.Links.Add(0, 19, "http://bit.ly/Beavercord")
        LinkLabel2.Text = "The BioMedia Project"
        LinkLabel2.Links.Add(0, 20, "http://biomediaproject.com/bmp")
        LinkLabel3.Text = "Beaverhouse YouTube"
        LinkLabel3.Links.Add(0, 19, "http://youtube.com/vahkiti")
        LinkLabel4.Text = "Project Issue Tracker"
        LinkLabel4.Links.Add(0, 21, "https://github.com/TheLegendOfMataNui/game-issues/issues")
    End Sub

    Private Sub LinkLabel1_LinkClicked(ByVal sender As System.Object, ByVal _
    e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles _
    LinkLabel1.LinkClicked
        System.Diagnostics.Process.Start(e.Link.LinkData.ToString())
    End Sub

    Private Sub LinkLabel2_LinkClicked(ByVal sender As System.Object, ByVal _
    e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles _
    LinkLabel2.LinkClicked
        System.Diagnostics.Process.Start(e.Link.LinkData.ToString())
    End Sub

    Private Sub LinkLabel3_LinkClicked(ByVal sender As System.Object, ByVal _
    e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles _
    LinkLabel3.LinkClicked
        System.Diagnostics.Process.Start(e.Link.LinkData.ToString())
    End Sub

    Private Sub LinkLabel4_LinkClicked(ByVal sender As System.Object, ByVal _
    e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles _
    LinkLabel4.LinkClicked
        System.Diagnostics.Process.Start(e.Link.LinkData.ToString())
    End Sub
End Class