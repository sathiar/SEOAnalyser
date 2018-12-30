<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="WebClient.Index" ValidateRequest="false" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>

    <form id="form1" runat="server">
        <table style="width: 100%;">
            <tr>
                <td>
                    <asp:CheckBox ID="checkBoxCalculateWordsOnPage" runat="server" Text="Calculate number of occurrences on the page" Checked="True" />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:CheckBox ID="checkBoxCalculateKeywordsOnPage" runat="server" Text="Calculate number of occurrences on the page of each word listed in meta tags" Checked="True" />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:CheckBox ID="checkBoxCalculateExternalLinks" runat="server" Text="Calculate number of external links in the text" Checked="True" />
                </td>
            </tr>
            <tr>
                <td>
                    <input id="radioAnalyzeUrl" type="radio" name="placement" runat="server" checked="true" onchange="var t=document.getElementById('textUrlOrText');t.rows=1;t.innerText='';" />Web page URL<br />
                    <input id="radioAnalyzeText" type="radio" name="placement" runat="server" onchange="var t=document.getElementById('textUrlOrText');t.rows=20;t.innerText='';" />HTML text<br />
                    <br />
                    <asp:Label ID="labelErrorText" runat="server"></asp:Label>
                    <asp:TextBox ID="textUrlOrText" runat="server" TextMode="MultiLine" Rows="1" Width="100%" BorderColor="">http://www.bbc.com/news/</asp:TextBox>
                </td>
            </tr>
        </table>
        <asp:Button ID="buttonAnalyze" runat="server" Text="Analyze" OnClick="Analyze_Click" />
        <asp:Button ID="toggleEditStopWords" runat="server" Text="Edit Stop Words" OnClick="toggleEdit_Click" />
        <asp:Table ID="tblStopWords" runat="server" style="width: 100%;">
            <asp:TableHeaderRow>
                <asp:TableCell>Stop Words</asp:TableCell>
            </asp:TableHeaderRow>
            <asp:TableRow>
                <asp:TableCell>
                    <asp:TextBox ID="textStopWords" runat="server" TextMode="MultiLine" Width="100%" Rows="3"></asp:TextBox>
                </asp:TableCell>
            </asp:TableRow>
        </asp:Table>
                    
        <br />
        <br />

        <asp:Label ID="labelExternalLinkNumber" runat="server"></asp:Label>
        <table>
            <tr>
                <td valign="top">
                    <asp:GridView ID="gridViewWords" AllowSorting="true" runat="server" OnSorting="gridViewWords_Sorting" AutoGenerateColumns="False" Caption="Words">
                        <Columns>
                            <asp:BoundField DataField="Key" HeaderText="Word" SortExpression="Key" />
                            <asp:BoundField DataField="Value" HeaderText="Occurences" SortExpression="Value" />
                        </Columns>
                    </asp:GridView>
                </td>
                <td valign="top">
                    <asp:GridView ID="gridViewKeywords" AllowSorting="true" runat="server" OnSorting="gridViewKeywords_Sorting" AutoGenerateColumns="False" Caption="Keywords">
                        <Columns>
                            <asp:BoundField DataField="Key" HeaderText="Word" SortExpression="Key" />
                            <asp:BoundField DataField="Value" HeaderText="Occurences" SortExpression="Value" />
                        </Columns>
                    </asp:GridView>
                </td>
            </tr>
        </table>
    </form>
</body>
<script type="text/javascript">
    var t = document.getElementById('textUrlOrText');
    if (document.getElementById('radioAnalyzeUrl').checked)
        t.rows = 1;
    else
        t.rows = 20;
</script>
</html>
