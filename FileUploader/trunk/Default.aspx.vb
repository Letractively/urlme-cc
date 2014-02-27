Partial Class _Default
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        For Each file As String In My.Computer.FileSystem.GetFiles("c:\temp\")
            Response.Write(file & "<br/>")
        Next
    End Sub
End Class
