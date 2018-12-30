using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Net;
using System.Web.UI.WebControls;

namespace WebClient
{
    public partial class Index : System.Web.UI.Page
    {
        bool isUrlPassed;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                labelErrorText.Text = "";
                txtWords.BorderColor = Color.Empty;
                using (StreamReader stream = new StreamReader(Path.Combine(Server.MapPath("~"),
                        @"util\WordsToIgnore.txt")))
                {
                    textStopWords.Text = stream.ReadToEnd();
                }
                tblStopWords.Visible = false;
            }
        }

        private void createDataGrid(GridView v, Dictionary<string, int> dic, string gridName)
        {
            if (dic == null)
            {
                v.DataSource = null;
                v.DataBind();
                return;
            }

            var dt = new DataTable();
            DataColumn column;
            column = new DataColumn
            {
                DataType = Type.GetType("System.String"),
                ColumnName = "Key",
                ReadOnly = true,
                Unique = false
            };
            dt.Columns.Add(column);

            column = new DataColumn
            {
                DataType = Type.GetType("System.Int32"),
                ColumnName = "Value",
                ReadOnly = true,
                Unique = false
            };
            dt.Columns.Add(column);

            foreach (var word in dic)
            {
                DataRow dr = dt.NewRow();
                dr["Key"] = word.Key;
                dr["Value"] = word.Value;
                dt.Rows.Add(dr);
            }

            Session[gridName] = dt;
            v.DataSource = Session[gridName];
            v.DataBind();
        }

        private delegate Dictionary<string, int> GetDicDelegate();

        private void analyseWords(GridView resultsGrid, GetDicDelegate del_GetDictionary, string gridName)
        {
            createDataGrid(resultsGrid, del_GetDictionary(), gridName);
        }

        protected void toggleEdit_Click(object sender, EventArgs e)
        {
            tblStopWords.Visible = !tblStopWords.Visible;
        }

        protected void Analyze_Click(object sender, EventArgs e)
        {
            try
            {
                isUrlPassed = (txtWords.Rows == 1) ? true : false;
                var counter = new util.Analyser(txtWords.Text, isUrlPassed, textStopWords.Text);
                analyseWords(gridViewWords, counter.GetWordsDictionary, "wordsTable");
                analyseWords(gridViewKeywords, counter.GetKeywordsDictionary, "keywordsTable");

                labelExternalLinkNumber.Text = String.Format("Number of external links: {0}", counter.GetExternalLinksNumber());
            }
            catch (UriFormatException)
            {
                labelErrorText.Text = "Incompatible url format";
                txtWords.BorderColor = Color.Red;
            }
            catch (WebException ex)
            {
                labelErrorText.Text = ex.Message;
                txtWords.BorderColor = Color.Red;
            }
        }

        protected void TextAnalyser_Click(object sender, EventArgs e)
        {
            txtWords.Text = "Input your text here to analyse text";
            txtWords.Rows = 20;
            isUrlPassed = false;
        }

        protected void UrlAnalyser_Click(object sender, EventArgs e)
        {
            txtWords.Text = "http://www.google.com";
            txtWords.Rows = 1;
            isUrlPassed = true;
        }
        private string getSortDirection(string column)
        {
            string sortDirection = "ASC";
            string sortExpression = ViewState["SortExpression"] as string;

            if (sortExpression != null)
            {
                if (sortExpression == column)
                {
                    string lastDirection = ViewState["SortDirection"] as string;
                    if ((lastDirection != null) && (lastDirection == "ASC"))
                    {
                        sortDirection = "DESC";
                    }
                }
            }

            ViewState["SortDirection"] = sortDirection;
            ViewState["SortExpression"] = column;

            return sortDirection;
        }

        private void sortTable(string tableName, string sortExpression, GridView gv)
        {
            var dt = Session[tableName] as DataTable;
            if (dt != null)
            {
                dt.DefaultView.Sort = sortExpression + " " + getSortDirection(sortExpression);
                gv.DataSource = Session[tableName];
                gv.DataBind();
            }
        }

        protected void gridViewWords_Sorting(object sender, GridViewSortEventArgs e)
        {
            sortTable("wordsTable", e.SortExpression, gridViewWords);
        }

        protected void gridViewKeywords_Sorting(object sender, GridViewSortEventArgs e)
        {
            sortTable("keywordsTable", e.SortExpression, gridViewKeywords);
        }

    }
}