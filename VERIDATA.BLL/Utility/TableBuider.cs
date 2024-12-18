using System.Data;
using System.Text;

namespace VERIDATA.BLL.utility
{
    public class TableBuider
    {
        private readonly StringBuilder builder = new();

        public DataRowCollection BodyData { get; set; }
        public DataColumnCollection HeaderData { get; set; }
        public int BodyRows { get; set; }

        public TableBuider(DataTable dt)
        {
            HeaderData = dt.Columns;
            BodyData = dt.Rows;
        }

        //public TableBuider(int bodyRows, string[] bodyData)
        //{
        //    BodyData = bodyData;
        //    BodyRows = bodyRows;
        //}

        /// <summary>
        /// Since your table headers are static, and your table body
        /// is variable, we don't need to store the headers. Instead
        /// we need to know the number of rows and the information
        /// that goes in those rows.
        /// </summary>
        //public TableBuider(string[] tableInfo, int bodyRows)
        //{
        //    BodyData = tableInfo;
        //    BodyRows = bodyRows;
        //    BuildTable();
        //}

        public string BuildTable()
        {
            BuildTableHead();
            BuildTableBody();
            return builder.ToString();
        }

        private void BuildTableHead()
        {
            _ = builder.Append("<table id=\"keywords\" class=\"report_table\" cellspacing=\"0\" cellpadding=\"0\">");
            _ = builder.Append("<thead>");
            _ = builder.Append("<tr>");
            foreach (DataColumn dc in HeaderData)
            {
                AppendTableHeader(dc.ColumnName);
            }
            _ = builder.Append("</tr>");
            _ = builder.Append("</thead>");
        }

        private void BuildTableBody()
        {
            _ = builder.Append("<tbody>");
            //builder.Append("<tr>");
            // For every row we need added, append a <td>info</td>
            // to the table from the data we have
            foreach (DataRow dr in BodyData)
            {
                _ = builder.Append("<tr>");
                foreach (DataColumn dc in HeaderData)
                {
                    string? cellValue = dr[dc] != null ? dr[dc].ToString() : "";
                    AppendTableDefinition(cellValue);
                }
            }
            _ = builder.Append("</tr>");
            //builder.Append("</tr>");
            _ = builder.Append("</table");
        }

        private void AppendTableHeader(string input)
        {
            AppendSpanTag("th", input);
        }

        private void AppendTableDefinition(string input)
        {
            AppendTag("td", "lalign", input);
        }

        private void AppendTag(string tag, string classdata, string input)
        {
            _ = builder.Append("<" + tag + " class=" + classdata + ">");
            _ = builder.Append(input);
            _ = builder.Append("</" + tag + ">");
        }

        private void AppendSpanTag(string tag, string input)
        {
            _ = builder.Append("<" + tag + "> <span>");
            _ = builder.Append(input);
            _ = builder.Append("</span> </" + tag + ">");
        }
    }
}