Imports System.Net

Public Class GitHubRelease
    Public ReadOnly Property TagName As String
    Public ReadOnly Property Name As String
    Public ReadOnly Property AssetDownloadURL As String

    Public Sub New(tagName As String, name As String, assetDownloadURL As String)
        Me.TagName = tagName
        Me.Name = name
        Me.AssetDownloadURL = assetDownloadURL
    End Sub

    Public Sub DownloadAsset(destinationFilename As String)
        My.Computer.Network.DownloadFile(Me.AssetDownloadURL, destinationFilename)
    End Sub
End Class

Public Module GitHubAPI
    Private Const GitHubAPIHost As String = "api.github.com"

    Public Const OrganizationIdentifier As String = "TheLegendOfMataNui"
    Public Const LOMNBetaRepoIdentifier As String = "LOMN-Beta"
    Public Const LOMNAlphaRepoIdentifier As String = "LOMN-Alpha"
    Public Const LauncherRepoIdentifier As String = "Launcher"

    Private Const UserAgent As String = "TheLegendOfMataNuiLauncher"

    Private Async Function SendGetRequestAsync(endpoint As String) As Task(Of String)
        Using client As New System.Net.Http.HttpClient()
            ' GitHub asks that tools identify themselves somehow in the user-agent
            client.DefaultRequestHeaders.UserAgent.Clear()
            client.DefaultRequestHeaders.UserAgent.Add(New Http.Headers.ProductInfoHeaderValue(UserAgent, ""))

            Return Await client.GetStringAsync(New Uri("https://" & GitHubAPIHost & endpoint))
        End Using
    End Function

    Public Async Sub DownloadFile(url As String, filename As String)
        Using client As New System.Net.Http.HttpClient()
            ' GitHub asks that tools identify themselves somehow in the user-agent
            client.DefaultRequestHeaders.UserAgent.Clear()
            client.DefaultRequestHeaders.UserAgent.Add(New Http.Headers.ProductInfoHeaderValue(UserAgent, ""))

            Using stream As System.IO.Stream = Await client.GetStreamAsync(url)
                Using fileStream As New System.IO.FileStream(filename, System.IO.FileMode.Create)
                    Await stream.CopyToAsync(fileStream)
                    Form1.LoadingIcon.Visible = False
                End Using
            End Using
        End Using
    End Sub

    Public Async Function GetLatestRelease(owner As String, repo As String, Optional includePrerelease As Boolean = False) As Task(Of GitHubRelease)
        ' API information here: https://docs.github.com/en/rest/releases/releases#list-releases
        Dim response As String = Await SendGetRequestAsync("/repos/" & owner & "/" & repo & "/releases")

        Dim json As System.Text.Json.JsonDocument = System.Text.Json.JsonDocument.Parse(response)

        For Each releaseObject As System.Text.Json.JsonElement In json.RootElement.EnumerateArray()
            ' Skip draft releases
            Dim isDraft As Boolean = releaseObject.GetProperty("draft").GetBoolean()
            If isDraft Then Continue For
            ' Skip prereleases if includePrerelease is False
            Dim isPrerelease As Boolean = releaseObject.GetProperty("prerelease").GetBoolean()
            If isPrerelease AndAlso Not includePrerelease Then Continue For

            ' Get the release info
            Dim tagName As String = releaseObject.GetProperty("tag_name").GetString()
            Dim name As String = releaseObject.GetProperty("name").GetString()

            ' Get the asset info for the first asset, if any
            Dim assetURL As String = ""
            Dim assetArray As System.Text.Json.JsonElement = releaseObject.GetProperty("assets")
            If assetArray.GetArrayLength() > 0 Then
                assetURL = assetArray(0).GetProperty("browser_download_url").GetString()
            End If

            Return New GitHubRelease(tagName, name, assetURL)
        Next

        Return Nothing
    End Function
End Module
