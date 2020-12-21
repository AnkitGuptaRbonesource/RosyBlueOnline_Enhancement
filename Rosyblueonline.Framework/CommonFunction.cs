using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Rosyblueonline.Framework
{
    public class CommonFunction
    {
        public DataSet READExcel(string path)
        {
            //Instance reference for Excel Application
            Microsoft.Office.Interop.Excel.Application objXL = null;
            //Workbook refrence
            Microsoft.Office.Interop.Excel.Workbook objWB = null;
            System.Data.DataSet ds = new System.Data.DataSet();
            try
            {
                //Instancing Excel using COM services
                objXL = new Microsoft.Office.Interop.Excel.Application();
                //Adding WorkBook
                objWB = objXL.Workbooks.Open(path);

                //foreach (Microsoft.Office.Interop.Excel.Worksheet objSHT in objWB.Worksheets)
                Microsoft.Office.Interop.Excel.Worksheet objSHT = objWB.Worksheets[0];
                //{
                int rows = objSHT.UsedRange.Rows.Count;
                int cols = objSHT.UsedRange.Columns.Count;
                int lastUsedRow;
                int lastUsedColumn;
                // Find the last real row
                lastUsedRow = objSHT.Cells.Find("*", System.Reflection.Missing.Value,
                                               System.Reflection.Missing.Value, System.Reflection.Missing.Value,
                                               Microsoft.Office.Interop.Excel.XlSearchOrder.xlByRows, Microsoft.Office.Interop.Excel.XlSearchDirection.xlPrevious,
                                               false, System.Reflection.Missing.Value, System.Reflection.Missing.Value).Row;

                // Find the last real column
                lastUsedColumn = objSHT.Cells.Find("*", System.Reflection.Missing.Value,
                                               System.Reflection.Missing.Value, System.Reflection.Missing.Value,
                                               Microsoft.Office.Interop.Excel.XlSearchOrder.xlByColumns, Microsoft.Office.Interop.Excel.XlSearchDirection.xlPrevious,
                                               false, System.Reflection.Missing.Value, System.Reflection.Missing.Value).Column;

                System.Data.DataTable dt = new System.Data.DataTable();
                int noofrow = 1;

                //If 1st Row Contains unique Headers for datatable include this part else remove it
                //Start
                for (int c = 1; c <= cols; c++)
                {
                    string colname = objSHT.Cells[1, c].Text;
                    dt.Columns.Add(colname);
                    noofrow = 2;
                }
                //END

                for (int r = noofrow; r <= lastUsedRow; r++)
                {
                    DataRow dr = dt.NewRow();
                    for (int c = 1; c <= cols; c++)
                    {
                        dr[c - 1] = objSHT.Cells[r, c].Text;
                    }
                    dt.Rows.Add(dr);
                }
                ds.Tables.Add(dt);

                //}

                //Closing workbook
                objWB.Close();
                //Closing excel application
                objXL.Quit();

            }
            catch (Exception ex)
            {
                ///CreateFile("InvUpload", ex.ToString());
                objWB.Saved = true;
                //Closing work book
                objWB.Close();
                //Closing excel application
                objXL.Quit();
                throw ex;
                //Response.Write("Illegal permission");
            }
            return ds;
        }

        public string[] GetSheetsNameFromExcel(string excelFile)
        {
            OleDbConnection objConn = null;
            System.Data.DataTable dt = null;

            try
            {
                // Connection String. Change the excel file to the file you
                // will search.
                //String connString = "Provider=Microsoft.Jet.OLEDB.4.0;" +
                //    "Data Source=" + excelFile + ";Extended Properties=Excel 8.0;";
                String connString = "Provider=Microsoft.ACE.OLEDB.12.0;" +
                    "Data Source=" + excelFile + ";Extended Properties=Excel 12.0;";

                // Create connection object by using the preceding connection string.
                objConn = new OleDbConnection(connString);
                // Open connection with the database.
                objConn.Open();
                // Get the data table containg the schema guid.
                dt = objConn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);

                if (dt == null)
                {
                    return null;
                }

                String[] excelSheets = new String[dt.Rows.Count];
                int i = 0;

                // Add the sheet name to the string array.
                foreach (DataRow row in dt.Rows)
                {
                    excelSheets[i] = row["TABLE_NAME"].ToString().Replace("'", "").Replace("$", "");
                    i++;
                }

                // Loop through all of the sheets if you want too...
                for (int j = 0; j < excelSheets.Length; j++)
                {
                    // Query each excel sheet.
                }

                return excelSheets;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                // Clean up.
                if (objConn != null)
                {
                    objConn.Close();
                    objConn.Dispose();
                }
                if (dt != null)
                {
                    dt.Dispose();
                }
            }
        }

        public DataTable GetDataFromExcel2(string excelFile, bool HasHeader = true)
        {
            //Instance reference for Excel Application
            Microsoft.Office.Interop.Excel.Application objXL = null;
            //Workbook refrence
            Microsoft.Office.Interop.Excel.Workbook objWB = null;
            //System.Data.DataSet ds = new System.Data.DataSet();
            DataTable dt = new DataTable();
            try
            {
                //Instancing Excel using COM services
                objXL = new Microsoft.Office.Interop.Excel.Application();
                //Adding WorkBook
                objWB = objXL.Workbooks.Open(excelFile);

                foreach (Microsoft.Office.Interop.Excel.Worksheet objSHT in objWB.Worksheets)
                //if (objWB.Worksheets.Count > 0)
                {
                    //Microsoft.Office.Interop.Excel.Worksheet objSHT = objWB.Worksheets[0];
                    int rows = objSHT.UsedRange.Rows.Count;
                    int cols = objSHT.UsedRange.Columns.Count;
                    int lastUsedRow;
                    int lastUsedColumn;
                    // Find the last real row
                    lastUsedRow = objSHT.Cells.Find("*", System.Reflection.Missing.Value,
                                                   System.Reflection.Missing.Value, System.Reflection.Missing.Value,
                                                   Microsoft.Office.Interop.Excel.XlSearchOrder.xlByRows, Microsoft.Office.Interop.Excel.XlSearchDirection.xlPrevious,
                                                   false, System.Reflection.Missing.Value, System.Reflection.Missing.Value).Row;

                    // Find the last real column
                    lastUsedColumn = objSHT.Cells.Find("*", System.Reflection.Missing.Value,
                                                   System.Reflection.Missing.Value, System.Reflection.Missing.Value,
                                                   Microsoft.Office.Interop.Excel.XlSearchOrder.xlByColumns, Microsoft.Office.Interop.Excel.XlSearchDirection.xlPrevious,
                                                   false, System.Reflection.Missing.Value, System.Reflection.Missing.Value).Column;

                    //System.Data.DataTable dt = new System.Data.DataTable();
                    int noofrow = 1;

                    //If 1st Row Contains unique Headers for datatable include this part else remove it
                    //Start
                    for (int c = 1; c <= cols; c++)
                    {
                        string colname = objSHT.Cells[1, c].Text;
                        if (!string.IsNullOrEmpty(colname))
                        {
                            dt.Columns.Add(colname.Trim());
                            noofrow = 2;
                        }
                    }
                    cols = dt.Columns.Count;
                    //END

                    for (int r = noofrow; r <= lastUsedRow; r++)
                    {
                        DataRow dr = dt.NewRow();
                        for (int c = 1; c <= cols; c++)
                        {
                            //if (c == 44)
                            //{
                            //    //below code written to convert text into date format for databasefriendly format (yyyy-mm-dd)
                            //    string[] dts;
                            //    string tmpdts;
                            //    if (objSHT.Cells[r, c].Text != "")
                            //    {
                            //        tmpdts = objSHT.Cells[r, c].Text;
                            //        dts = tmpdts.Split('/');
                            //        dr[c - 1] = dts[2] + "-" + dts[1] + "-" + dts[0];
                            //    }
                            //    else
                            //    {
                            //        dr[c - 1] = objSHT.Cells[r, c].Text;
                            //    }
                            //}
                            //else
                            //{
                            //    dr[c - 1] = objSHT.Cells[r, c].Text;
                            //}
                            dr[c - 1] = objSHT.Cells[r, c].Text.Trim();
                        }
                        dt.Rows.Add(dr);
                    }
                    //ds.Tables.Add(dt);
                    break;
                }


                //Closing workbook
                objWB.Close();
                //Closing excel application
                objXL.Quit();

            }
            catch (Exception ex)
            {
                objWB.Saved = true;
                //Closing work book
                objWB.Close();
                //Closing excel application
                objXL.Quit();
                //Response.Write("Illegal permission");
            }
            return dt;
        }

        public DataTable GetDataFromExcel(string excelFile, bool HasHeader = true)
        {
            OleDbConnection objConn = null;
            System.Data.DataTable dt = null;
            try
            {
                // Connection String. Change the excel file to the file you
                // will search.
                //String connString = "Provider=Microsoft.Jet.OLEDB.4.0;" +
                //    "Data Source=" + excelFile + ";Extended Properties=Excel 8.0;";
                String connString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + excelFile + ";Extended Properties='Excel 12.0;Xml;IMEX=1;HDR=" + (HasHeader ? "Yes" : "No") + "'";
                //bool firstRow = true;

                // Create connection object by using the preceding connection string.
                objConn = new OleDbConnection(connString);
                // Open connection with the database.
                objConn.Open();
                DataTable excelDataTable = new DataTable();
                string[] Sheets = GetSheetsNameFromExcel(excelFile);
                if (Sheets.Length > 0)
                {
                    OleDbDataAdapter objDA = new System.Data.OleDb.OleDbDataAdapter("select * from [" + Sheets[0] + "$]", objConn);
                    objDA.Fill(excelDataTable);
                }
                return excelDataTable;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                // Clean up.
                if (objConn != null)
                {
                    objConn.Close();
                    objConn.Dispose();
                }
                if (dt != null)
                {
                    dt.Dispose();
                }
            }
        }

        public DataTable ParseToString(DataTable oldDT)
        {
            DataTable dt = new DataTable();
            for (int i = 0; i < oldDT.Columns.Count; i++)
            {
                if (i == 1 || i == 2)
                {
                    dt.Columns.Add(new DataColumn(oldDT.Columns[i].ColumnName, typeof(string)));
                }
                else if (i == 43)
                {
                    dt.Columns.Add(new DataColumn(oldDT.Columns[i].ColumnName, typeof(DateTime)));
                }
                else
                {
                    dt.Columns.Add(new DataColumn(oldDT.Columns[i].ColumnName));
                }
            }

            for (int i = 0; i < oldDT.Rows.Count; i++)
            {
                DataRow dr = dt.NewRow();
                for (int j = 0; j < oldDT.Columns.Count; j++)
                {
                    if (j == 1 || j == 2)
                    {
                        dr[j] = Convert.ToString(oldDT.Rows[i][j]);
                    }
                    else if (j == 43)
                    {
                        dr[j] = Convert.ToString(oldDT.Rows[i][j]).Trim() == "" ? "" : Convert.ToDateTime(oldDT.Rows[i][j]).ToString("yyyy-MM-dd");
                    }
                    else
                    {
                        dr[j] = oldDT.Rows[i][j];
                    }
                }
                dt.Rows.Add(dr);
            }
            return dt;
        }

        public void CreateFile(string filename, string content)
        {
            System.IO.StreamWriter fp;
            fp = System.IO.File.CreateText(("CreateFilePath" + (filename + ".txt")));
            fp.WriteLine(content + "-------------DateAndTime-----------" + DateTime.Now.ToString());
            fp.Close();
        }

        public static string ConvertDataTableToHTML(DataTable dt, bool WithHeader = true, bool OnlyRows = false)
        {
            string html = OnlyRows ? "" : "<table>";
            //add header row
            if (WithHeader)
            {
                html += "<tr bgcolor='#365f91' style='color:#e7e7e7; font-size:11px; text-align:center;'>";
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    if (dt.Columns[i].ColumnName != "_blank")
                    {
                        html += "<td style='border-right:solid 1px #ffffff;font: 300 13px sans-serif, Verdana'>" + dt.Columns[i].ColumnName + "</td>";
                    }
                    else
                    {
                        html += "<td style='border-right:solid 1px #ffffff;font: 300 13px sans-serif, Verdana;'></td>";
                    }
                }
                html += "</tr>";
            }
            //add rows
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                html += "<tr>";
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    //if (dt.Columns[j].ColumnName != "_blank")
                    //{
                    if (dt.Rows[i][j].ToString().Contains("HYPERLINK"))
                    {
                        html += "<td align='center' style='border-left: solid 1px #365f91;border-right:solid 1px #365f91;border-top:solid 1px #365f91;'><font face='Verdana' ,'sans-serif' size=2px color='#365f91'><a target = '_blank' href ="+ dt.Rows[i][j].ToString().Replace("=HYPERLINK(","").ToString().Replace("GIA","").ToString()+" >" + dt.Rows[i]["Certificate"].ToString()+" </a></font></td>";
 
                    }
                    else
                    {
                        html += "<td align='center' style='border-left: solid 1px #365f91;border-right:solid 1px #365f91;border-top:solid 1px #365f91;'><font face='Verdana' ,'sans-serif' size=2px color='#365f91'>" + dt.Rows[i][j].ToString() + "</font></td>";
                    }
                    //}
                    //else
                    //{
                    //    html += "<td align='center' style='border-right:solid 1px #365f91;border-top:solid 1px #365f91;padding:4px;background-color:#337ab7 !important;'><font face='Verdana' ,'sans-serif' size=2px color='#365f91'>" + dt.Rows[i][j].ToString() + "</font></td>";
                    //}
                }
                html += "</tr>";
            }
            html += OnlyRows ? "" : "</table>";
            return html;
        }




        public static string GetIpAddress(HttpRequestBase Request)
        {
            // Request.UserHostAddress returns the IP address of the client.
            var userip = Request.UserHostAddress;
            if (Request.UserHostAddress != null)
            {
                return userip;
            }
            //In case the Request.UserHostAddress returns null then we need to read the ip address from the ServerVariables
            else
            {
                //Request.ServerVariables["HTTP_X_FORWARDED_FOR"] will have value if the client machine is using a proxy server 
                userip = Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
                if (string.IsNullOrEmpty(userip))
                {
                    //Request.ServerVariables["REMOTE_ADDR"] contains the ip address of the client.
                    userip = Request.ServerVariables["REMOTE_ADDR"];
                }
            }
            return userip;
        }

    }
}
