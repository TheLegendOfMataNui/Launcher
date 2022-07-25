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
    Public Const LOMNRebuiltRepoIdentifier As String = "LOMN-REBUILT"
    Public Const LOMNBetaRepoIdentifier As String = "LOMN-Beta"
    Public Const LOMNAlphaRepoIdentifier As String = "LOMN-Alpha"
    Public Const LauncherRepoIdentifier As String = "Launcher"

    Private Function SendGETRequest(endpoint As String) As String
        Using client As New System.Net.WebClient()
            ' GitHub asks that tools identify themselves somehow in the user-agent
            client.Headers.Set(Net.HttpRequestHeader.UserAgent, "TheLegendOfMataNui/Launcher")

            Using stream As System.IO.Stream = client.OpenRead("https://" & GitHubAPIHost & endpoint)
                GitHubAPI.LogRateLimits(client.ResponseHeaders)
                Using reader As New System.IO.StreamReader(stream)
                    Return reader.ReadToEnd()
                End Using
            End Using
        End Using
    End Function

    Public Sub DownloadFile(url As String, filename As String)
        Using client As New System.Net.WebClient()
            ' GitHub asks that tools identify themselves somehow in the user-agent
            client.Headers.Set(Net.HttpRequestHeader.UserAgent, "TheLegendOfMataNui/Launcher")

            Using stream As System.IO.Stream = client.OpenRead(url)
                GitHubAPI.LogRateLimits(client.ResponseHeaders)
                Using fileStream As New System.IO.FileStream(filename, System.IO.FileMode.Create)
                    stream.CopyTo(fileStream)
                End Using
            End Using
        End Using
    End Sub

    Public Function GetLatestRelease(owner As String, repo As String, Optional includePrerelease As Boolean = False) As GitHubRelease
        ' API information here: https://docs.github.com/en/rest/releases/releases#list-releases
        Dim response As String = SendGETRequest("/repos/" & owner & "/" & repo & "/releases")

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

    Private Sub LogRateLimits(responseHeaders As WebHeaderCollection)
        Dim limit As String = responseHeaders.Get("x-ratelimit-limit")
        Dim remaining As String = responseHeaders.Get("x-ratelimit-remaining")
        Dim used As String = responseHeaders.Get("x-ratelimit-used")
        System.Diagnostics.Debug.WriteLine("GitHubt API: " & vbNewLine & "  Limit: " & limit & vbNewLine & " Remaining: " & remaining & vbNewLine & "  Used: " & used & vbNewLine)
    End Sub
End Module