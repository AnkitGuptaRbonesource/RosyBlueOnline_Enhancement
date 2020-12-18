using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using OfficeOpenXml;
using OfficeOpenXml.Drawing;
using OfficeOpenXml.Style;

namespace Rosyblueonline.Framework
{
    public static class ExportToExcel
    {
        public static void SaveExcel(string FilePath, string FileName, string SheetName, DataTable dt)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                using (ExcelPackage pck = new ExcelPackage(ms))
                {
                    ExcelWorksheet ws = pck.Workbook.Worksheets.Add(SheetName);
                    ws.Cells["A1"].LoadFromDataTable(dt, true);
                    pck.Save();
                    FileStream fs = new FileStream(FilePath + "/" + FileName + ".xlsx", FileMode.OpenOrCreate);
                    ms.WriteTo(fs);
                    fs.Close();
                }
                ms.Dispose();
            }
        }

        public static byte[] DownloadExcel(string SheetName, DataTable dt)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                using (ExcelPackage pck = new ExcelPackage(ms))
                {
                    ExcelWorksheet ws = pck.Workbook.Worksheets.Add(SheetName);
                    ws.Cells["A1"].LoadFromDataTable(dt, true);
                    pck.Save();


                    //FileStream fs = new FileStream(FilePath + "/" + FileName + ".xlsx", FileMode.OpenOrCreate);
                    //ms.WriteTo(fs);
                    return ms.ToArray();
                }
            }
        }

        public static byte[] DownloadExcel(string SheetNames, DataSet ds)
        {
            string[] SheetName = SheetNames.Split(',');
            using (MemoryStream ms = new MemoryStream())
            {
                using (ExcelPackage pck = new ExcelPackage(ms))
                {
                    for (int i = 0; i < ds.Tables.Count; i++)
                    {
                        string sname = SheetName.Length > i ? SheetName[i] : "Sheet" + i.ToString();
                        ExcelWorksheet ws = pck.Workbook.Worksheets.Add(sname);
                        ws.Protection.IsProtected = false;
                        ws.Protection.AllowSelectLockedCells = false;
                        DataColumn dc = ds.Tables[i].Columns["RowNum"];
                        ds.Tables[i].Columns.Remove(dc);
                        ws.Cells["A1"].LoadFromDataTable(ds.Tables[i], true);
                        ws = SetURL(ds.Tables[i], ws);
                    }
                    pck.Save();
                    //FileStream fs = new FileStream(FilePath + "/" + FileName + ".xlsx", FileMode.OpenOrCreate);
                    //ms.WriteTo(fs);
                    return ms.ToArray();
                }
            }
        }

        public static ExcelWorksheet SetURL(DataTable dt, ExcelWorksheet ws)
        {
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                if (Convert.ToString(((object[,])ws.Cells.Value)[0, i]) == "Lab")
                {
                    for (int j = 1; j < dt.Rows.Count; j++)
                    {
                        if (((object[,])ws.Cells.Value)[0, i] != null)
                        {
                            var a = ((object[,])ws.Cells.Value)[j, i].ToString().Split(',');
                            ExcelRangeBase objBase = ws.Cells[j + 1, i + 1];
                            objBase.Hyperlink = new Uri(a[0], UriKind.Absolute);
                            objBase.Value = a[1];
                        }
                    }
                }
                if (Convert.ToString(((object[,])ws.Cells.Value)[0, i]) == "v360url")
                {
                    for (int j = 1; j < dt.Rows.Count; j++)
                    {
                        if (((object[,])ws.Cells.Value)[j, i] != null)
                        {
                            var a = ((object[,])ws.Cells.Value)[j, i].ToString();
                            ExcelRangeBase objBase = ws.Cells[j + 1, i + 1];
                            if (a.Contains("http"))
                            {
                                objBase.Hyperlink = new Uri(a, UriKind.Absolute);
                                objBase.Value = "360";
                            }
                        }
                    }
                }
                if (Convert.ToString(((object[,])ws.Cells.Value)[0, i]) == "Video")
                {
                    for (int j = 1; j < dt.Rows.Count; j++)
                    {
                        if (((object[,])ws.Cells.Value)[0, i] != null)
                        {
                            var a = ((object[,])ws.Cells.Value)[j, i].ToString();
                            ExcelRangeBase objBase = ws.Cells[j + 1, i + 1];
                            if (a.Contains("http"))
                            {
                                objBase.Hyperlink = new Uri(a, UriKind.Absolute);
                                objBase.Value = "DNA";
                            }
                        }
                    }
                }
            }
            return ws;

        }



        public static byte[] InventoryExportToExcel(DataTable ds, string imgPath, bool RemoveLocation = false, string Column = "AT", bool RemoveRefdata = true)
        {
            byte[] bytes;
            try
            {
                var dsResult = ds;
                var find = 0;
                dsResult.Columns.Remove("Certificate");
             //   dsResult.Columns.Remove("Stockstatus");
                dsResult.Columns.Remove("Reportdate");
                foreach (DataColumn column in dsResult.Columns)
                {
                    if (column.ColumnName.Contains("Stockstatus"))
                    {
                        find = 1;
                    }
                }
                if (find == 1)
                { dsResult.Columns.Remove("Stockstatus"); }

                if (RemoveLocation)
                {
                    dsResult.Columns.Remove("SalesLocation");
                }
                if (RemoveRefdata)
                {
                    dsResult.Columns.Remove("refdata");
                }
                else
                {
                    dsResult.Columns["refdata"].ColumnName = "Company Name";
                }
                var countColoumns = Convert.ToString(dsResult.Rows.Count + 12);
                using (ExcelPackage pck = new ExcelPackage())
                {
                    ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Inventory Download");
                    int rowIndex = 1;
                    int colIndex = 1;
                    int PixelTop = 50;
                    int PixelLeft = 80;
                    int Height = 160;
                    int Width = 1200;
                    System.Drawing.Image img = System.Drawing.Image.FromFile(imgPath);
                    ExcelPicture pic = ws.Drawings.AddPicture("Sample", img);
                    pic.SetPosition(rowIndex, 0, colIndex, 0);
                    //pic.SetPosition(PixelTop, PixelLeft);  
                    pic.SetSize(Width, Height);
                    //pic.SetSize(40);  
                    ws.Protection.IsProtected = false;
                    ws.Protection.AllowSelectLockedCells = false;

                    #region Filter
                    ws.Cells["A12:AT12"].AutoFilter = true;
                    #endregion

                    //ws.Cells["A12:AG12"].Style.Font.Bold = true;
                    //ws.Cells["A12:AG12"].Style.Font.UnderLine = true;
                    //ws.Cells["A12:AG12"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    //ws.Cells["A12:AG12"].Style.Fill.BackgroundColor.SetColor(Color.Yellow);
                    ws.Cells["A12:" + Column + "12"].Style.Font.Bold = true;
                    ws.Cells["A12:" + Column + "12"].Style.Font.UnderLine = true;
                    ws.Cells["A12:" + Column + "12"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    ws.Cells["A12:" + Column + "12"].Style.Fill.BackgroundColor.SetColor(Color.Yellow);
                    if (dsResult.Columns.Count > 0 && dsResult.Columns[0].ColumnName == "RowNum")
                    {
                        dsResult.Columns.Remove("RowNum");
                    }
                    ws.Cells["A12"].LoadFromDataTable(dsResult, true);
                    //ws.Cells["A11:AG50"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    //ws.Cells["A11:AG50"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    ws.Cells["A11"].Formula = "=ROUND(SUBTOTAL(3,A13:A" + countColoumns + "),2)";//      FOR TOTAL COUNT
                    //ws.Cells["C10"].Value = "Total Carat";
                    ws.Cells["C11"].Formula = "=ROUND(SUBTOTAL(9,C13:C" + countColoumns + "),2)";//             Weight
                    //ws.Cells["G10"].Formula = "=SUM(G12:G" + countColoumns + ")";//                             Length
                    //ws.Cells["H10"].Formula = "=SUM(G12:G" + countColoumns + ")";//                             Width
                    //ws.Cells["P10"].Value = "Avg Rap/ct ($)";
                    ws.Cells["P11"].Formula = "=ROUND(Q11/C11,2)";//?                                           Rapnet_Price
                                                                  // ws.Cells["Q10"].Value = "Total Rap ($)";
                    ws.Cells["Q11"].Formula = "=ROUND(SUBTOTAL(9,Q13:Q" + countColoumns + "),2)";//?              Rap_Amount
                                                                                                 // ws.Cells["R10"].Value = "Avg Rap. Off (%)";
                    ws.Cells["R11"].Formula = "=ROUND(100-(T11/Q11%),2)";// "=IF(P11<> 0,ROUND((P11-S11)/P11*-100,2),'')";// STRING???        Rapnet_Discount_Per
                                                                         // ws.Cells["S10"].Value = "Price/ct $";
                    ws.Cells["S11"].Formula = "=ROUND(T11/C11,2)";//                                            Pricect
                                                                  // ws.Cells["T10"].Value = "Payable Amount ($)";
                    ws.Cells["T11"].Formula = "=ROUND(SUBTOTAL(9,T13:T" + countColoumns + "),2)";//             Amount



                    #region Weight
                    foreach (var cell in ws.Cells["C13:C" + countColoumns + ""])
                    {
                        cell.Value = Convert.ToDecimal(cell.Value);
                    }
                    ws.Cells["C13:C" + countColoumns + ""].Style.Numberformat.Format = "#,##0.00";
                    #endregion
                    #region Length
                    foreach (var cell in ws.Cells["G13:G" + countColoumns + ""])
                    {
                        cell.Value = Convert.ToDecimal(cell.Value);
                    }
                    ws.Cells["G13:G" + countColoumns + ""].Style.Numberformat.Format = "#,##0.00";
                    #endregion
                    #region Width
                    foreach (var cell in ws.Cells["H13:H" + countColoumns + ""])
                    {
                        cell.Value = Convert.ToDecimal(cell.Value);
                    }
                    ws.Cells["H13:H" + countColoumns + ""].Style.Numberformat.Format = "#,##0.00";
                    #endregion
                    #region Depth
                    foreach (var cell in ws.Cells["I13:I" + countColoumns + ""])
                    {
                        cell.Value = Convert.ToDecimal(cell.Value);
                    }
                    ws.Cells["I3:I" + countColoumns + ""].Style.Numberformat.Format = "#,##0.00";
                    #endregion
                    #region Rapnet_Price
                    foreach (var cell in ws.Cells["P13:P" + countColoumns + ""])
                    {
                        cell.Value = Convert.ToDecimal(cell.Value);
                    }
                    ws.Cells["P13:Q" + countColoumns + ""].Style.Numberformat.Format = "#,##0.00";
                    #endregion
                    #region Rap_Amount
                    foreach (var cell in ws.Cells["Q13:Q" + countColoumns + ""])
                    {
                        cell.Value = Convert.ToDecimal(cell.Value);
                    }
                    ws.Cells["Q13:Q" + countColoumns + ""].Style.Numberformat.Format = "#,##0.00";
                    #endregion

                    #region Rapnet_Discount_Per
                    foreach (var cell in ws.Cells["R13:R" + countColoumns + ""])
                    {
                        cell.Value = Convert.ToDecimal(cell.Value);
                    }
                    ws.Cells["R13:R" + countColoumns + ""].Style.Numberformat.Format = "#,##0.00";
                    #endregion

                    #region HyperLink
                    //foreach (var cell in ws.Cells["O13:O" + countColoumns + ""])
                    //{
                    //    if (cell.Value != null)
                    //    {
                    //        var a = cell.Value.ToString().Split(',');
                    //        cell.Hyperlink = new Uri(a[0], UriKind.Absolute);
                    //        cell.Value = a[1];
                    //    }
                    //    //cell.Value = Convert.ToDecimal(cell.Value);
                    //    //cell.Hyperlink = new Uri(cell.Text.ToString());
                    //    //cell.Value = cell.Value;
                    //    //cell.Formula = string.Format("HYPERLINK({0},{1})", cell.Value, cell.Text);
                    //    //cell.Calculate();
                    //}

                    //ws.Cells["R12:R" + countColoumns + ""].Style.Numberformat.Format = "#,##0.00";
                    #endregion

                    #region Pricect
                    foreach (var cell in ws.Cells["S13:S" + countColoumns + ""])
                    {
                        cell.Value = Convert.ToDecimal(cell.Value);
                    }
                    ws.Cells["S13:S" + countColoumns + ""].Style.Numberformat.Format = "#,##0.00";
                    #endregion
                    #region Amount
                    foreach (var cell in ws.Cells["T13:T" + countColoumns + ""])
                    {
                        cell.Value = Convert.ToDecimal(cell.Value);
                    }
                    ws.Cells["T13:T" + countColoumns + ""].Style.Numberformat.Format = "#,##0.00";
                    #endregion
                    #region Depth_per
                    //foreach (var cell in ws.Cells["V13:V" + countColoumns + ""])
                    //{
                    //    cell.Value = Convert.ToDecimal(cell.Value);
                    //}
                    //ws.Cells["V13:V" + countColoumns + ""].Style.Numberformat.Format = "#,##0.00";
                    #endregion
                    #region Table_per
                    //foreach (var cell in ws.Cells["W13:W" + countColoumns + ""])
                    //{
                    //    cell.Value = Convert.ToDecimal(cell.Value);
                    //}
                    //ws.Cells["W13:W" + countColoumns + ""].Style.Numberformat.Format = "#,##0.00";
                    #endregion
                    #region Crown_Height
                    //foreach (var cell in ws.Cells["Y13:Y" + countColoumns + ""])
                    //{
                    //    cell.Value = Convert.ToDecimal(cell.Value);
                    //}
                    //ws.Cells["Y13:Y" + countColoumns + ""].Style.Numberformat.Format = "#,##0.00";
                    #endregion
                    #region Crown_Angle
                    //foreach (var cell in ws.Cells["Z13:Z" + countColoumns + ""])
                    //{
                    //    cell.Value = Convert.ToDecimal(cell.Value);
                    //}
                    //ws.Cells["Z13:Z" + countColoumns + ""].Style.Numberformat.Format = "#,##0.00";
                    #endregion
                    #region Pavilion_Depth
                    //foreach (var cell in ws.Cells["AA13:AA" + countColoumns + ""])
                    //{
                    //    cell.Value = Convert.ToDecimal(cell.Value);
                    //}
                    //ws.Cells["AA13:AA" + countColoumns + ""].Style.Numberformat.Format = "#,##0.00";
                    #endregion
                    #region Pavilion_Angle
                    //foreach (var cell in ws.Cells["AB13:AB" + countColoumns + ""])
                    //{
                    //    cell.Value = Convert.ToDecimal(cell.Value);
                    //}
                    //ws.Cells["AB13:AB" + countColoumns + ""].Style.Numberformat.Format = "#,##0.00";
                    #endregion
                    #region StarLength
                    //foreach (var cell in ws.Cells["AC13:AC" + countColoumns + ""])
                    //{
                    //    cell.Value = Convert.ToDecimal(cell.Value);
                    //}
                    //ws.Cells["AC13:AC" + countColoumns + ""].Style.Numberformat.Format = "#,##0.00";
                    #endregion
                    #region LowerHalf
                    //foreach (var cell in ws.Cells["AD13:AD" + countColoumns + ""])
                    //{
                    //    cell.Value = Convert.ToDecimal(cell.Value);
                    //}
                    //ws.Cells["AD13:AD" + countColoumns + ""].Style.Numberformat.Format = "#,##0.00";
                    #endregion
                    #region Girdle_per
                    //foreach (var cell in ws.Cells["AE13:AE" + countColoumns + ""])
                    //{
                    //    cell.Value = Convert.ToDecimal(cell.Value);
                    //}
                    //ws.Cells["AE13:AE" + countColoumns + ""].Style.Numberformat.Format = "#,##0.00";
                    #endregion

                    foreach (var cell in ws.Cells["O13:O" + countColoumns + ""])
                    {
                        cell.Formula = cell.Value.ToString();
                    }
                    foreach (var cell in ws.Cells["AQ13:AQ" + countColoumns + ""])
                    {
                        if (cell.Value != null && cell.Value.ToString().Contains("=HYPERLINK"))
                        {
                            cell.Formula = cell.Value.ToString();
                        }
                    }
                    foreach (var cell in ws.Cells["AR13:AR" + countColoumns + ""])
                    {
                        if (cell.Value != null && cell.Value.ToString().Contains("=HYPERLINK"))
                        {
                            cell.Formula = cell.Value.ToString();
                        }
                    }
                    foreach (var cell in ws.Cells["AS13:AS" + countColoumns + ""])
                    {
                        if (cell.Value != null && cell.Value.ToString().Contains("=HYPERLINK"))
                        {
                            cell.Formula = cell.Value.ToString();
                        }
                    }
                    foreach (var cell in ws.Cells["AT13:AT" + countColoumns + ""])
                    {
                        if (cell.Value != null && cell.Value.ToString().Contains("=HYPERLINK"))
                        {
                            cell.Formula = cell.Value.ToString();
                        }
                    }
                    bytes = pck.GetAsByteArray();
                }
                return bytes;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
            }
        }

        public static byte[] StoneStatusExportToExcel(DataTable ds, string Column = "AP")
        {
            byte[] bytes;
            try
            {
                var dsResult = ds;
                var countRows = Convert.ToString(dsResult.Rows.Count + 1);
                using (ExcelPackage pck = new ExcelPackage())
                {
                    ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Stone Status");
                    ws.Protection.IsProtected = false;
                    ws.Protection.AllowSelectLockedCells = false;

                    #region Filter
                    ws.Cells["A1:AP1"].AutoFilter = true;
                    #endregion

                    //ws.Cells["A12:AG12"].Style.Font.Bold = true;
                    //ws.Cells["A12:AG12"].Style.Font.UnderLine = true;
                    //ws.Cells["A12:AG12"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    //ws.Cells["A12:AG12"].Style.Fill.BackgroundColor.SetColor(Color.Yellow);
                    ws.Cells["A1:" + Column + "1"].Style.Font.Bold = true;
                    ws.Cells["A1:" + Column + "1"].Style.Font.UnderLine = true;
                    ws.Cells["A1:" + Column + "1"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    ws.Cells["A1:" + Column + "1"].Style.Fill.BackgroundColor.SetColor(Color.Yellow);
                    if (dsResult.Columns.Count > 0 && dsResult.Columns[0].ColumnName == "RowNum")
                    {
                        dsResult.Columns.Remove("RowNum");
                    }
                    ws.Cells["A1"].LoadFromDataTable(dsResult, true);

                    //V360
                    foreach (var cell in ws.Cells["D2:O" + (countRows) + ""])
                    {
                        if (cell.Value != null && cell.Value.ToString().Contains("=HYPERLINK"))
                        {
                            cell.Formula = cell.Value.ToString();
                        }
                    }

                    bytes = pck.GetAsByteArray();
                }
                return bytes;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
            }
        }

        public static string DownloadExcelViaGridView(string SheetName, DataTable dt)
        {

            System.IO.StringWriter tw = new System.IO.StringWriter();
            System.Web.UI.HtmlTextWriter hw = new System.Web.UI.HtmlTextWriter(tw);
            GridView grdResultDetails = new GridView();
            grdResultDetails.DataSource = dt;
            grdResultDetails.DataBind();
            grdResultDetails.HeaderRow.Style.Add("background-color", "#fff");
            for (int i = 0; i <= grdResultDetails.HeaderRow.Cells.Count - 1; i++)
            {
                grdResultDetails.HeaderRow.Cells[i].Style.Add("background-color", "#9a9a9a");
            }
            grdResultDetails.RenderControl(hw);
            return tw.ToString();

        }
    }

    public static class ListtoDataTable
    {
        public static DataTable ToDataTable<T>(List<T> items)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);
            //Get all the properties by using reflection   
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in Props)
            {
                //Setting column names as Property names  
                dataTable.Columns.Add(prop.Name);
            }
            foreach (T item in items)
            {
                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
                {
                    values[i] = Props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }

            return dataTable;
        }
    }
}
