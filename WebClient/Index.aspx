<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="WebClient.Index" ValidateRequest="false" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <asp:Table runat="server" style="width: 100%;">
            <asp:TableHeaderRow Width="100%">
                <asp:TableCell Font-Size="XX-Large" Width="100%" BackColor="LightBlue">Welcome to SEO Analyser</asp:TableCell>
            </asp:TableHeaderRow>
        </asp:Table><br /><br />

        <div class="row">
            <div class="col-md-6">
                <h2>Text Analyzer</h2>
                <p>User submits a text in English, page filters out stop-words (e.g. ‘or’, ‘and’, ‘a’, ‘the’ etc), calculates number of occurrences on the page of each word, number of occurrences on the page of each word listed in meta tags, number of external links in the text.</p>
                <asp:LinkButton ID="btnTextAnalyser" runat="server" Text="Try Text Analyser" OnClick="TextAnalyser_Click"/>
            </div>
            <div class="col-md-6">
                <h2>URL Analyzer</h2>
                <p>User submits a URL, page filters out stop-words (e.g. ‘or’, ‘and’, ‘a’, ‘the’ etc), calculates number of occurrences on the page of each word, number of occurrences on the page of each word listed in meta tags, number of external links in the text.</p>
                <asp:LinkButton ID="btnUrlAnalyser" runat="server" Text="Try URL Analyser" OnClick="UrlAnalyser_Click" />
            </div><br /> <br /> 

            <asp:Label ID="labelErrorText" runat="server"></asp:Label>
            <asp:TextBox ID="txtWords" runat="server" TextMode="MultiLine" Rows="1" Width="100%" BorderColor=""></asp:TextBox>
        </div><br />

        <asp:Button ID="buttonAnalyze" runat="server" Text="Analyze" OnClick="Analyze_Click" />
        <asp:Button ID="toggleEditStopWords" runat="server" Text="View Stop Words" OnClick="toggleEdit_Click" /><br />

        <asp:Table ID="tblStopWords" runat="server" style="width: 100%;">
            <asp:TableHeaderRow>
                <asp:TableCell Font-Size="X-Large">Stop Words</asp:TableCell>
            </asp:TableHeaderRow>
            <asp:TableRow>
                <asp:TableCell>
                    <asp:TextBox ID="textStopWords" runat="server" TextMode="MultiLine" Width="100%" Rows="3"></asp:TextBox>
                </asp:TableCell>
            </asp:TableRow>
        </asp:Table>     
        <br /><br />

        <asp:Label ID="labelExternalLinkNumber" runat="server"></asp:Label><br /><br />
        <table>
            <tr>
                <td valign="top">
                    <asp:GridView ID="gridViewWords" AllowSorting="true" runat="server" OnSorting="gridViewWords_Sorting" AutoGenerateColumns="False" Caption="Words">
                        <Columns>
                            <asp:BoundField DataField="Key" HeaderText="Word" SortExpression="Key" />
                            <asp:BoundField DataField="Value" HeaderText="Frequency" SortExpression="Value" />
                        </Columns>
                    </asp:GridView>
                </td>
                <td valign="top">
                    <asp:GridView ID="gridViewKeywords" AllowSorting="true" runat="server" OnSorting="gridViewKeywords_Sorting" AutoGenerateColumns="False" Caption="Keywords">
                        <Columns>
                            <asp:BoundField DataField="Key" HeaderText="Word" SortExpression="Key" />
                            <asp:BoundField DataField="Value" HeaderText="Frequency" SortExpression="Value" />
                        </Columns>
                    </asp:GridView>
                </td>
            </tr>
        </table>
    </form>
</body>
<%--<script type="text/javascript">
var t = document.getElementById('textUrlOrText');
if (document.getElementById('radioAnalyzeUrl').checked)
 t.rows = 1;
else
t.rows = 20;
</script>--%>
</html>
