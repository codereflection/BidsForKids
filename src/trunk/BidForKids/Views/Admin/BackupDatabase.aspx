<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Backup Database
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Backup Database</h2>
    
    <script type="text/javascript">

        function StartDatabaseBackup() {
            $.ajax({
                url: '<%= Url.Action("StartDatabaseBackup")%>',
                data: null,
                dataType: 'text',
                type: 'POST',
                success: function (data, textStatus, XMLHttpRequest) {
                    alert(data);
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    alert("Error: " + textStatus);
                }
            });
        }
    
    </script>
    
    <input type="button" id="BackupDatabase" onclick="StartDatabaseBackup();" value="Backup Database" />

</asp:Content>
