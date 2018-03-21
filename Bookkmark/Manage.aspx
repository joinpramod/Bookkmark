<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Manage.aspx.cs" Inherits="Bookmark.Manage" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
     <meta name="viewport" content="width=device-width, initial-scale=1" />
    <title>Booqmarq</title>
    <link id="Link1" runat="server" rel="shortcut icon" href="~/favicon.ico" type="image/x-icon" />
    <link id="Link2" runat="server" rel="icon" href="~/favicon.ico" type="image/ico" />

    <meta http-equiv="Pragma" content="no-cache" />
    <meta http-equiv="expires" content="0" />
    <meta http-equiv="Content-Type" content="text/html; charset=UTF-8" />
    <meta name="Language" content="en-us" />
    <meta name="robots" content="noindex" />
</head>
<body>
    <form id="form1" runat="server">
    <div>
            <table style="width:100%;">
            <tr>
                <td>
                    <asp:TextBox ID="txtSQL" runat="server" Height="120px" TextMode="MultiLine" Width="90%"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:TextBox ID="txtTemp" runat="server" Height="120px" TextMode="MultiLine" Width="90%"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td>
                    <asp:Button ID="Button1" runat="server" OnClick="Button1_Click" Text="Submit" />
                </td>
            </tr>
            <tr>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td>
                    <asp:GridView ID="GridView1" runat="server" PageSize="100" Width="90%">
                    </asp:GridView>
                </td>
            </tr>
        </table>

    </div>


        <div>


            <br />
            <br />
            <asp:TextBox ID="txtEmailId" runat="server" Width="562px"></asp:TextBox>
            <br />
            <br />
            <asp:TextBox ID="txtSubject" runat="server" Width="562px"></asp:TextBox>
            <br />
            <br />
            <br />
            <asp:TextBox ID="txtMessage" runat="server" Height="98px" style="margin-top: 0px" TextMode="MultiLine" Width="709px"></asp:TextBox>
            <br />
            <br />
            <asp:Button ID="btnSend" runat="server" OnClick="btnSend_Click" Text="Send" />
            <br />
            <br />
            <asp:Button ID="btnSendBulkEMails" runat="server" OnClick="btnSendBulkEMails_Click" Text="Send email to all who have bookmarks" />
            <br />
            <br />
            <asp:Button ID="btnSendBulkEMailsWhoDontHaveBookmarks" runat="server" OnClick="btnSendBulkEMailsWhoDontHaveBookmarks_Click" Text="Send email to all who dont have bookmarks" />
            <br />
            <br />


        </div>


    </form>
</body>
</html>
