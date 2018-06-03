Imports Microsoft.VisualBasic
Imports System.Collections.Generic
Imports System.IO

'----------------------------------------------------------------
'Read BIONICLE.ini
'----------------------------------------------------------------

Public Class INIFile
    Public Sections As Dictionary(Of String, INISection)

    Public Sub New(path As String)
        Sections = New Dictionary(Of String, INISection)

        If Not System.IO.File.Exists(path) Then
            Exit Sub
        End If

        Using reader As New StreamReader(path)
            Dim currentSection As INISection = New INISection("")
            Sections.Add("", currentSection)

            While reader.EndOfStream = False
                Dim line As String = reader.ReadLine().Trim()
                If line.Length = 0 Then
                    Continue While
                End If

                If line.StartsWith("[") Then
                    ' Read the section name
                    Dim sectionName As String = line.Substring(1, line.Length() - 2)
                    currentSection = New INISection(sectionName)
                    Sections.Add(sectionName, currentSection)
                ElseIf line.StartsWith(";") Then
                    Continue While
                Else
                    ' Read key and value
                    Dim parts As String() = line.Split("=")
                    If parts.Length = 2 Then
                        If Not currentSection.Entries.ContainsKey(parts(0).Trim()) Then
                            currentSection.Entries.Add(parts(0).Trim(), parts(1).Trim())
                        Else
                            System.Diagnostics.Debugger.Break()
                        End If
                    Else
                        System.Windows.Forms.MessageBox.Show("Bad key and value")
                    End If
                End If
            End While
        End Using
    End Sub

    Public Sub Write(path As String)
        Using writer As New StreamWriter(path)
            For Each section As INISection In Sections.Values
                If Not section.Name = "" Then
                    writer.WriteLine("[" + section.Name + "]")
                End If

                For Each value As KeyValuePair(Of String, String) In section.Entries
                    writer.WriteLine(value.Key + "=" + value.Value)
                Next
            Next
        End Using
    End Sub

    Public Function GetString(section As String, key As String, Optional defaultValue As String = "") As String
        If Sections.ContainsKey(section) Then
            If Sections(section).Entries.ContainsKey(key) Then
                Return Sections(section).Entries(key)
            Else
                Return defaultValue
            End If
        Else
            Return defaultValue
        End If
    End Function

    Public Sub SetString(section As String, key As String, value As String)
        If Not Sections.ContainsKey(section) Then
            Sections.Add(section, New INISection(section))
        End If
        If Sections(section).Entries.ContainsKey(key) Then
            Sections(section).Entries(key) = value
        Else
            Sections(section).Entries.Add(key, value)
        End If
    End Sub
End Class

Public Class INISection
    Public Name As String
    Public Entries As Dictionary(Of String, String)

    Public Sub New(name As String)
        Me.Name = name
        Entries = New Dictionary(Of String, String)()
    End Sub
End Class