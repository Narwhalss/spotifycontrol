﻿Imports System.Xml

Public Class MetadataAPI
    Dim TrackXML As New XmlDocument
    Public Sub New(ByVal ArtistName As String, ByVal TrackName As String)
        Try
            Dim TrackUrl As String
            ' TrackUrl has the following syntax http://ws.spotify.com/search/1/track?q=ArtistName&TrackName
            TrackUrl = "http://ws.spotify.com/search/1/track?q=" & ArtistName & " " & TrackName
            Dim request As Net.HttpWebRequest
            Dim response As Net.HttpWebResponse
            request = Net.HttpWebRequest.Create(TrackUrl)
            Threading.Thread.Sleep(1 * 1000)
            request.Method = "GET"
            request.UserAgent = "SpotifyControl"
            request.Timeout = Threading.Timeout.Infinite
            request.KeepAlive = False
            request.ProtocolVersion = Net.HttpVersion.Version10
            request.ContentType = "text/xml"
            response = request.GetResponse
            ' load the downloaded xml into our xml document
            TrackXML.Load(response.GetResponseStream)
        Catch ex As Exception
            'MsgBox(ex.Message)
        End Try
    End Sub
    ' gets the artist spotify URI
    Public Function GetArtistURL() As String
        Try
            Return TrackXML.GetElementsByTagName("artist")(0).Attributes(0).InnerText
        Catch ex As Exception
            Return "Link Not Found"
        End Try
    End Function
    ' gets the album spotify URI
    Public Function GetAlbumURL() As String
        Try
            Return TrackXML.GetElementsByTagName("album")(0).Attributes(0).InnerText
        Catch ex As Exception
            Return "Link Not Found"
        End Try
    End Function
    ' gets the album name and the release year
    Public Function GetAlbumName() As String
        Try
            Dim result As String
            result = TrackXML.GetElementsByTagName("album")(0).ChildNodes(0).InnerText
            result = result & " (" & TrackXML.GetElementsByTagName("album")(0).ChildNodes(1).InnerText & ")"
            Return result
        Catch ex As Exception
            Return "Album Name Not Found"
        End Try
    End Function
    ' gets the track length(in Minutes:Seconds format)
    Public Function GetTrackLength() As String
        Try
            Dim TotalTime, Seconds, Minutes As Integer
            TotalTime = TrackXML.GetElementsByTagName("length")(0).InnerText
            Minutes = Math.Truncate(TotalTime / 60)
            Seconds = TotalTime Mod 60
            Return Minutes & ":" & Seconds
        Catch ex As Exception
            Return "0:00"
        End Try
    End Function
End Class
