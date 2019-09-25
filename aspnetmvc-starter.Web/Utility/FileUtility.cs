using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.UI.WebControls;
using System.Xml.Serialization;

namespace aspnetmvc_starter.Web.Utility
{
    /// <summary>
    /// Summary description for ImportFromExcel
    /// </summary>
    public class FileUtility
    {
        public FileUtility()
        {
        }

        public static string SaveFile(HttpPostedFileBase fileUpload, out string strErrMsg)
        {
            strErrMsg = "";
            try
            {
                string sDirPath = HttpContext.Current.Server.MapPath(AppConstant.FileUploadPath);
                string sFilePath = "";
                //Path.Combine(HttpContext.Current.Server.MapPath(AppConstant.FileUploadPath), Path.GetFileName(fileUpload.FileName));

                if (!Directory.Exists(sDirPath))
                {
                    Directory.CreateDirectory(sDirPath);
                }

                sFilePath = DateTime.Now.Ticks.ToString() + "__" + fileUpload.FileName;
                sFilePath = Path.Combine(sDirPath, sFilePath);

                fileUpload.SaveAs(sFilePath);

                return sFilePath;
            }
            catch (Exception ex)
            {
                strErrMsg = ex.Message;
            }

            return "";
        }

        public static bool CopyFile(string strFromFilePath, string strToFilePath, out string strErrMsg)
        {
            strErrMsg = "";
            if (!string.IsNullOrEmpty(strFromFilePath) && System.IO.File.Exists(strFromFilePath)
                && !string.IsNullOrEmpty(strToFilePath))
            {
                try
                {
                    bool a = System.IO.Directory.Exists(strToFilePath);
                    System.IO.File.Copy(strFromFilePath, strToFilePath, true);
                    return true;
                }
                catch (Exception ex)
                {
                    strErrMsg = "Could not Copy File! <br />" + ex.Message;
                    return false;
                }
            }
            strErrMsg = "Source File not Found!";
            return false;
        }

        public static bool MoveFile(string strFromFilePath, string strToFilePath, out string strErrMsg)
        {
            strErrMsg = "";
            if (!string.IsNullOrEmpty(strFromFilePath) && System.IO.File.Exists(strFromFilePath)
                && !string.IsNullOrEmpty(strToFilePath))
            {
                try
                {
                    string strToFileDir = Path.GetDirectoryName(strToFilePath);
                    if (!string.IsNullOrEmpty(strToFileDir) && !System.IO.Directory.Exists(strToFileDir))
                        System.IO.Directory.CreateDirectory(strToFileDir);

                    System.IO.File.Move(strFromFilePath, strToFilePath);
                    return true;
                }
                catch (Exception ex)
                {
                    strErrMsg = "Could not Move File! <br />" + ex.Message;
                    return false;
                }
            }
            strErrMsg = "Source File not Found!";
            return false;
        }

        public static bool DeleteFile(string strFilePath, out string strErrMsg)
        {
            strErrMsg = "";
            if (System.IO.File.Exists(strFilePath))
            {
                try
                {
                    System.IO.File.Delete(strFilePath);
                    return true;
                }
                catch (Exception ex)
                {
                    strErrMsg = "Could not Delete File! <br />" + ex.Message;
                    return false;
                }
            }
            strErrMsg = "File not Found!";
            return false;
        }

        private static void DeleteAllFileHelper(string strDirectory)
        {
            if (Directory.Exists(strDirectory))
                Array.ForEach(Directory.GetFiles(strDirectory), File.Delete);
        }

        #region ImageUpload

        /// <summary>
        /// Uploads an Image file, size less than 1MB and Returns the file saved location
        /// </summary>
        /// <param name="fileUpload">File upload control</param>
        /// <param name="image">Optional send an image control to show the image after upload</param>
        /// <param name="editMode">add or edit mode</param>
        /// <param name="existingFileName">Target file path</param>
        /// <param name="strErrMsg">to return error message</param>
        /// <returns></returns>
        public static string SaveImage(FileUpload fileUpload, System.Web.UI.WebControls.Image image, string editMode,
                                       out string existingFileName, out string strErrMsg)
        {
            existingFileName = strErrMsg = "";
            string saveFolderPath = "";
            string saveFilePath = "";
            try
            {
                if (fileUpload.HasFile)
                {
                    saveFolderPath = AppConstant.FileUploadPath;
                    string postedFileName = fileUpload.PostedFile.FileName;
                    long sizeInMB = fileUpload.PostedFile.ContentLength/(1024*1024);

                    if (sizeInMB < 1)
                    {
                        string imgName = "";
                        if (editMode == "add")
                        {
                            Guid guidImg3 = System.Guid.NewGuid();
                            imgName = guidImg3.ToString();
                        }
                        else
                        {
                            if (existingFileName != string.Empty)
                            {
                                imgName = existingFileName.Substring(existingFileName.LastIndexOf('\\'),
                                                                     existingFileName.LastIndexOf('.') -
                                                                     existingFileName.LastIndexOf('\\'));
                            }
                            else
                            {
                                Guid guidImg3 = System.Guid.NewGuid();
                                imgName = guidImg3.ToString();
                            }
                        }

                        string postedFileType = postedFileName.Substring(postedFileName.LastIndexOf('.'));
                        imgName += postedFileType;
                        saveFilePath = HttpContext.Current.Server.MapPath(saveFolderPath) + imgName;
                        try
                        {
                            fileUpload.PostedFile.SaveAs(saveFilePath);
                            System.Drawing.Image original = System.Drawing.Image.FromFile(saveFilePath);

                            using (System.Drawing.Image resized = ResizeImage(original, new Size(256, 192), true))
                            {
                                original.Dispose();
                                original = null;

                                if (File.Exists(saveFilePath))
                                {
                                    File.Delete(saveFilePath);
                                }

                                resized.Save(saveFilePath);
                                if (image != null)
                                    image.ImageUrl = saveFolderPath + imgName;

                                existingFileName = AppConstant.FileUploadPath + imgName;
                            }
                        }
                        catch (Exception ex)
                        {
                            strErrMsg = "Invalid file entered! <BR />" + ex.Message;
                        }
                    }
                    else
                    {
                        strErrMsg = "Please select an image less than 1 MB.";
                    }
                }
                else
                {
                    strErrMsg = "File not found!";
                }
            }
            catch (Exception ex)
            {
                strErrMsg = "File could not be uploaded! <br/>" + ex.Message;
            }

            return saveFilePath;
        }

        public static System.Drawing.Image ResizeImage(System.Drawing.Image image, Size size, bool preserveAspectRatio)
        {
            int newWidth;
            int newHeight;
            if (preserveAspectRatio)
            {
                int originalWidth = image.Width;
                int originalHeight = image.Height;
                float percentWidth = (float) size.Width/(float) originalWidth;
                float percentHeight = (float) size.Height/(float) originalHeight;
                float percent = percentHeight < percentWidth ? percentHeight : percentWidth;
                newWidth = (int) (originalWidth*percent);
                newHeight = (int) (originalHeight*percent);
            }
            else
            {
                newWidth = size.Width;
                newHeight = size.Height;
            }

            System.Drawing.Image newImage = new Bitmap(newWidth, newHeight);
            using (Graphics graphicsHandle = Graphics.FromImage(newImage))
            {
                graphicsHandle.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphicsHandle.CompositingQuality = CompositingQuality.HighSpeed;
                graphicsHandle.DrawImage(image, 0, 0, newWidth, newHeight);
            }
            return newImage;
        }

        #endregion

        /*
    #region ExcelImportExport

    public static TableHeaderCell GetFormattedTableHeaderCell()
    {
        TableHeaderCell hcell = new TableHeaderCell();
        hcell.BorderStyle = System.Web.UI.WebControls.BorderStyle.Solid;

        return hcell;
    }
    public static TableCell GetFormattedTableCell()
    {
        TableCell cell = new TableCell();
        cell.BorderStyle = System.Web.UI.WebControls.BorderStyle.Solid;

        return cell;
    }

    //Import
    [Obsolete]
    public static DataTable LoadExcelSheet(string strPath, bool status, out string strErrMsg)
    {
        strErrMsg = "";
        //string connectionString = @"provider=Microsoft.JET.OLEDB.4.0; data source=" + strPath + ";" + "Extended Properties=Excel 8.0;";
        string connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0; Data Source=" + strPath + ";" + "Extended Properties=Excel 12.0;";
        string fName = System.IO.Path.GetFileName(strPath);
        string commandString = @"select * from" + " " + fName;
        OleDbConnection conn = new OleDbConnection(connectionString);
        DataTable myTable;
        myTable = new DataTable();
        try
        {
            OleDbDataAdapter da = new OleDbDataAdapter("Select T.* From [Sheet1$] as T ", conn);
            da.Fill(myTable);
            conn.Close();
        }
        catch (Exception ex)
        {
            strErrMsg = "Unable to load excel file <br/>" + ex.Message;
            conn.Close();
        }

        return myTable;
    }
    /// <summary>
    /// This mehtod retrieves data in the given excel sheet name. Ref:Excel.dll, ISharpCode.SharpZipLib.dll
    /// </summary>
    /// <param name="strPath">Excel file name with path</param>
    /// <param name="strSheet">Excel sheet name with $</param>
    /// <param name="strMsg">Error message</param>
    /// <returns></returns>
    public static DataTable LoadExcelSheet(string filePath, string strSheet, out string strMsg)
    {
        strMsg = "";
        DataTable myTable = new DataTable();
        DataSet dsExcelData = null;
        IExcelDataReader excelReader = null;

        if (string.IsNullOrEmpty(filePath))
        {
            strMsg = "File path not given.";
            return myTable;
        }

        strSheet = string.IsNullOrEmpty(strSheet) ? "Sheet1" : strSheet; //by default read Sheet1

        try
        {
            FileInfo file = new FileInfo(filePath);
            if (!file.Exists)
            {
                strMsg = "File not found.";
                return myTable;
            }

            FileStream stream = File.Open(filePath, FileMode.Open, FileAccess.Read);

            if (file.Extension.Equals(".xls")) //Reading from a binary Excel file ('97-2003 format; *.xls)
            {
                excelReader = ExcelReaderFactory.CreateBinaryReader(stream);
            }
            else //Reading from a OpenXml Excel file (2007 format; *.xlsx)
            {
                excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);
            }

            //DataSet - Create column names from first row
            excelReader.IsFirstRowAsColumnNames = true;
            dsExcelData = excelReader.AsDataSet();

            if (dsExcelData != null && dsExcelData.Tables.Count > 0)
            {
                strMsg = "No of WorkSheets found: " + dsExcelData.Tables.Count;
                myTable = dsExcelData.Tables[strSheet];
            }
        }
        catch (Exception ex)
        {
            strMsg = ex.Message;
        }

        return myTable;
    }

    //Export
    public static void WriteTsv<T>(IEnumerable<T> data, TextWriter output)
    {
        System.ComponentModel.PropertyDescriptorCollection props = System.ComponentModel.TypeDescriptor.GetProperties(typeof(T));
        foreach (System.ComponentModel.PropertyDescriptor prop in props)
        {
            output.Write(prop.DisplayName); // header
            output.Write("\t");
        }
        output.WriteLine();
        foreach (T item in data)
        {
            foreach (System.ComponentModel.PropertyDescriptor prop in props)
            {
                output.Write(prop.Converter.ConvertToString(
                     prop.GetValue(item)));
                output.Write("\t");
            }
            output.WriteLine();
        }
    }
    public static void WriteToExcel()
    {
        //DataTable people = (DataTable)Session["people"];
        //// Create excel file.
        //ExcelFile ef = new ExcelFile();
        //ExcelWorksheet ws = ef.Worksheets.Add("DataSheet");
        //ws.InsertDataTable(people, "A1", true);

        //Response.Clear();
        //// Stream file to browser, in required type.
        //switch (this.RadioButtonList1.SelectedValue)
        //{
        //    case "XLS":
        //        Response.ContentType = "application/vnd.ms-excel";
        //        Response.AddHeader("Content-Disposition", "attachment; filename=" +
        //             "Report.xls");
        //        ef.SaveXls(Response.OutputStream);
        //        break;
        //    case "XLSX":
        //        Response.ContentType = "application/vnd.openxmlformats";
        //        Response.AddHeader("Content-Disposition", "attachment; filename=" +
        //             "Report.xlsx");
        //        // With XLSX it is a bit more complicated as MS Packaging API
        //        // can't write directly to Response.OutputStream.
        //        // Therefore we use temporary MemoryStream.
        //        MemoryStream ms = new MemoryStream();
        //        ef.SaveXlsx(ms)
        //        ms.WriteTo(Response.OutputStream);
        //        break;
        //}
        //Response.End();
    }
    //NPOI Export
    public static void ExportToExcel(DataTable dt, string saveAsFileName)
    {
        // Create a new workbook and a sheet 
        var workbook = new HSSFWorkbook();
        var sheet = workbook.CreateSheet("Sheet1");

        var fnt = workbook.CreateFont();
        fnt.Boldweight = (short)FontBoldWeight.BOLD;
        // Add header labels
        var rowIndex = 0;
        var colIndex = 0;
        var row = sheet.CreateRow(rowIndex);
        foreach (DataColumn dc in dt.Columns)
        {
            row.CreateCell(colIndex++).SetCellValue(dc.ColumnName);
        }
        //row.CreateCell(0).SetCellValue("Receive ID");
        //row.CreateCell(1).SetCellValue("Detail ID");
        //row.CreateCell(2).SetCellValue("Item ID");
        //row.CreateCell(3).SetCellValue("Item Code");
        //row.CreateCell(4).SetCellValue("Item Status");
        //row.CreateCell(5).SetCellValue("Serial ID");
        //row.CreateCell(6).SetCellValue("Serial No.");
        //row.CreateCell(7).SetCellValue("Barcode");
        //row.RowStyle.SetFont(fnt);
        rowIndex++;

        // Add data rows
        foreach (DataRow dr in dt.Rows)
        {
            row = sheet.CreateRow(rowIndex);
            colIndex = 0;
            foreach (DataColumn dc in dt.Columns)
            {
                row.CreateCell(colIndex).SetCellValue(dr[colIndex].ToString());
                colIndex++;
            }
            //row.CreateCell(1).SetCellValue(dr[0].ToString());
            //row.CreateCell(2).SetCellValue(dr[1].ToString());
            //row.CreateCell(3).SetCellValue(obj.STRITEMID);
            //row.CreateCell(4).SetCellValue(obj.STRITEMSTATUS);
            //row.CreateCell(5).SetCellValue(obj.NUMID);
            //row.CreateCell(6).SetCellValue(obj.STRSERIALNO);
            //row.CreateCell(7).SetCellValue(obj.STRBARCODE);
            rowIndex++;
        }

        // Auto-size each column
        for (var i = 0; i < sheet.GetRow(0).LastCellNum; i++)
            sheet.AutoSizeColumn(i);

        // Add row indicating date/time report was generated...
        //sheet.CreateRow(rowIndex + 1).CreateCell(0).SetCellValue("Report generated on " + DateTime.Now.ToString());

        // Use the following commented-out code to save the Excel spreadsheet to the web server's filesystem
        //using (var fileData = new FileStream(Server.MapPath(saveAsFileName), FileMode.Create))
        //{
        //    string saveAsFileName = string.Format("MembershipExport-{0:d}.xls", DateTime.Now).Replace("/", "-");
        //    workbook.Write(fileData);
        //}

        // Save the Excel spreadsheet to a MemoryStream and return it to the client
        using (var exportData = new MemoryStream())
        {
            workbook.Write(exportData);

            //string saveAsFileName = string.Format("Export_ItemSerialList-{0:d}.xls", DateTime.Now).Replace("/", "-");
            // string saveAsFileName = "SerialList.xls";

            //HttpContext.Current.Response.ContentType = "application/vnd.xls";
            //HttpContext.Current.Response.AddHeader("Content-Disposition", string.Format("attachment;filename={0}", saveAsFileName));
            //HttpContext.Current.Response.Clear();
            //HttpContext.Current.Response.BinaryWrite(exportData.GetBuffer());
            //HttpContext.Current.Response.End();


            HttpContext.Current.Response.ContentType = "application/vnd.ms-excel";
            HttpContext.Current.Response.AddHeader("Content-Disposition", string.Format("attachment;filename={0}", saveAsFileName));
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.BinaryWrite(exportData.GetBuffer());
            HttpContext.Current.Response.End();
        }
    }

    public static ICellStyle GetHeaderCellStyle(HSSFWorkbook workbook)
    {
        NPOI.SS.UserModel.IFont fnt = workbook.CreateFont();
        fnt.Boldweight = (short)NPOI.SS.UserModel.FontBoldWeight.BOLD;
        fnt.Color = HSSFColor.WHITE.index;

        ICellStyle headerCellStyle = workbook.CreateCellStyle();
        headerCellStyle.SetFont(fnt);
        headerCellStyle.BorderBottom = (NPOI.SS.UserModel.BorderStyle.THIN);
        headerCellStyle.BorderLeft = (NPOI.SS.UserModel.BorderStyle.THIN);
        headerCellStyle.BorderRight = (NPOI.SS.UserModel.BorderStyle.THIN);
        headerCellStyle.BorderTop = (NPOI.SS.UserModel.BorderStyle.THIN);
        headerCellStyle.VerticalAlignment = VerticalAlignment.CENTER;
        HSSFPalette palette = workbook.GetCustomPalette();
        palette.SetColorAtIndex(HSSFColor.GREY_50_PERCENT.index,
            (byte)0,    //RGB red (0-255) 49-79-79 --Dark Slate Gray
            (byte)128,  //RGB green #008080
            (byte)128   //RGB blue
            );
        headerCellStyle.FillForegroundColor = HSSFColor.GREY_50_PERCENT.index;
        headerCellStyle.FillPattern = FillPatternType.SOLID_FOREGROUND;
        headerCellStyle.Alignment = HorizontalAlignment.CENTER;

        return headerCellStyle;
    }

    public static ICellStyle GetDataCellStyleDefault(HSSFWorkbook workbook)
    {
        ICellStyle DataCellStyle = workbook.CreateCellStyle();
        DataCellStyle.BorderBottom = (NPOI.SS.UserModel.BorderStyle.THIN);
        DataCellStyle.BorderLeft = (NPOI.SS.UserModel.BorderStyle.THIN);
        DataCellStyle.BorderRight = (NPOI.SS.UserModel.BorderStyle.THIN);
        DataCellStyle.BorderTop = (NPOI.SS.UserModel.BorderStyle.THIN);
        DataCellStyle.VerticalAlignment = VerticalAlignment.TOP;

        return DataCellStyle;
    }

    public static ICellStyle GetDataCellStyleDate(HSSFWorkbook workbook)
    {
        ICellStyle DataCellStyleDate = GetDataCellStyleDefault(workbook);

        var newDataFormat = workbook.CreateDataFormat();
        DataCellStyleDate.DataFormat = newDataFormat.GetFormat("dd-MMM-yyyy");
        DataCellStyleDate.Alignment = HorizontalAlignment.CENTER;

        return DataCellStyleDate;
    }

    public static ICellStyle GetDataCellStyleNumber(HSSFWorkbook workbook)
    {
        ICellStyle DataCellStyleNumber = GetDataCellStyleDefault(workbook);

        //var newDataFormat = workbook.CreateDataFormat();
        //DataCellStyleNumber.DataFormat = newDataFormat.GetFormat("#,##0");
        DataCellStyleNumber.Alignment = HorizontalAlignment.RIGHT;

        return DataCellStyleNumber;
    }

    //NPOI v1.2.5
    public static void ExportToExcel(DataTable dtColumn, DataTable dt, string saveAsFileName, string strSheetName)
    {
        // Create a new workbook and a sheet 
        var workbook = new HSSFWorkbook();
        var sheet = workbook.CreateSheet(strSheetName);

        // Add header labels
        var rowIndex = 0;
        var colIndex = 0;
        ICellStyle headerCellStyle = GetHeaderCellStyle(workbook);

        var row = sheet.CreateRow(rowIndex);
        row.Height = 700;
        foreach (DataColumn dc in dtColumn.Columns)
        {
            ICell Hcell = row.CreateCell(colIndex++);
            Hcell.CellStyle = (headerCellStyle);
            Hcell.SetCellValue(dc.ColumnName);
        }
        rowIndex++;

        ICell datacell = null;
        //DataCellStyle Default
        ICellStyle DataCellStyle = GetDataCellStyleDefault(workbook);
        //DataCellStyle Date
        ICellStyle DataCellStyleDate = GetDataCellStyleDate(workbook);
        //DataCellStyle Number
        ICellStyle DataCellStyleNumber = GetDataCellStyleNumber(workbook);

        // Add data rows
        foreach (DataRow dr in dt.Rows)
        {
            row = sheet.CreateRow(rowIndex);
            colIndex = 0;
            foreach (DataColumn dc in dt.Columns)
            {

                datacell = row.CreateCell(colIndex);
                if (dc.DataType == typeof(DateTime))
                {
                    if (!string.IsNullOrEmpty(dr[colIndex].ToString()))
                    {
                        datacell.SetCellValue(Convert.ToDateTime(dr[colIndex].ToString()));
                    }
                    datacell.CellStyle = (DataCellStyleDate);
                }
                else if (dc.DataType == typeof(Decimal))
                {
                    if (!string.IsNullOrEmpty(dr[colIndex].ToString()))
                    {
                        datacell.SetCellValue(Convert.ToDouble(dr[colIndex].ToString()));
                    }
                    datacell.CellStyle = (DataCellStyleNumber);
                }
                else
                {
                    DataCellStyle.Alignment = HorizontalAlignment.LEFT;
                    DataCellStyle.WrapText = true;
                    datacell.CellStyle = (DataCellStyle);
                    datacell.SetCellValue(dr[colIndex].ToString());
                }
                colIndex++;
            }
            rowIndex++;
        }

        try
        {
            // Auto-size each column
            int range = sheet.GetRow(0).LastCellNum;
            for (var i = 0; i < range; i++)
                sheet.AutoSizeColumn(i);
        }
        catch (Exception ex)
        {
            //throw;
        }

        // Save the Excel spreadsheet to a MemoryStream and return it to the client
        using (var exportData = new MemoryStream())
        {
            workbook.Write(exportData);
            HttpContext.Current.Response.Buffer = true;
            HttpContext.Current.Response.ContentType = "application/vnd.ms-excel";
            HttpContext.Current.Response.AddHeader("Content-Disposition", string.Format("attachment;filename={0}", saveAsFileName));
            HttpContext.Current.Response.Clear();
            //HttpContext.Current.Response.ClearContent();
            HttpContext.Current.Response.BinaryWrite(exportData.GetBuffer());
            HttpContext.Current.Response.Flush();
            //HttpContext.Current.Response.End();
        }
    }
    //NPOI v1.2.5 with Summary
    public static void ExportToExcel(DataTable dtColumn, DataTable dt, ArrayList SumColIndex, string saveAsFileName, string strSheetName)
    {
        // Create a new workbook and a sheet 
        var workbook = new HSSFWorkbook();
        var sheet = workbook.CreateSheet(strSheetName);

        // Add header labels
        var rowIndex = 0;
        var colIndex = 0;
        ICellStyle headerCellStyle = GetHeaderCellStyle(workbook);

        var row = sheet.CreateRow(rowIndex);
        row.Height = 700;
        foreach (DataColumn dc in dtColumn.Columns)
        {
            ICell Hcell = row.CreateCell(colIndex++);
            Hcell.CellStyle = (headerCellStyle);
            Hcell.SetCellValue(dc.ColumnName);
        }
        rowIndex++;

        ICell datacell = null;
        //DataCellStyle Default
        ICellStyle DataCellStyle = GetDataCellStyleDefault(workbook);
        //DataCellStyle Date
        ICellStyle DataCellStyleDate = GetDataCellStyleDate(workbook);
        //DataCellStyle Number
        ICellStyle DataCellStyleNumber = GetDataCellStyleNumber(workbook);

        // Add data rows
        foreach (DataRow dr in dt.Rows)
        {
            row = sheet.CreateRow(rowIndex);
            colIndex = 0;
            foreach (DataColumn dc in dt.Columns)
            {

                datacell = row.CreateCell(colIndex);
                if (dc.DataType == typeof(DateTime))
                {
                    if (!string.IsNullOrEmpty(dr[colIndex].ToString()))
                    {
                        datacell.SetCellValue(Convert.ToDateTime(dr[colIndex].ToString()));
                    }
                    datacell.CellStyle = (DataCellStyleDate);
                }
                else if (dc.DataType == typeof(Decimal))
                {
                    if (!string.IsNullOrEmpty(dr[colIndex].ToString()))
                    {
                        datacell.SetCellValue(Convert.ToDouble(dr[colIndex].ToString()));
                    }
                    datacell.CellStyle = (DataCellStyleNumber);
                }
                else
                {
                    DataCellStyle.Alignment = HorizontalAlignment.LEFT;
                    DataCellStyle.WrapText = true;
                    datacell.CellStyle = (DataCellStyle);
                    datacell.SetCellValue(dr[colIndex].ToString());
                }
                colIndex++;
            }
            rowIndex++;
        }
        //Sum Cell
        if (SumColIndex.Count > 0)
        {
            try
            {
                var sumRow = sheet.CreateRow(rowIndex);
                colIndex = int.Parse(SumColIndex[0].ToString()) - 1;
                ICell sumcell = sumRow.CreateCell(colIndex);
                sumcell.SetCellValue("Total");
                ICellStyle sumCellStyle = workbook.CreateCellStyle();
                sumCellStyle.CloneStyleFrom(headerCellStyle);
                sumCellStyle.Alignment = HorizontalAlignment.LEFT;
                sumcell.CellStyle = (sumCellStyle);
                for (int i = 0; i < SumColIndex.Count; i++)
                {
                    colIndex = int.Parse(SumColIndex[i].ToString());
                    ICellStyle sumDataCellStyleNumber = workbook.CreateCellStyle();
                    sumDataCellStyleNumber.CloneStyleFrom(sumCellStyle);
                    sumDataCellStyleNumber.Alignment = HorizontalAlignment.RIGHT;
                    sumcell = sumRow.CreateCell(colIndex);
                    int intCellNo = 65 + colIndex;
                    char chCellIndex = Convert.ToChar(intCellNo);
                    String reference = (chCellIndex + "2") + ":" + (chCellIndex + rowIndex.ToString());
                    sumcell.CellFormula = ("SUM(" + reference + ")");
                    sumcell.CellStyle = (sumDataCellStyleNumber);
                }
            }
            catch (Exception)
            {
            }
        }
        try
        {
            // Auto-size each column
            int range = sheet.GetRow(0).LastCellNum;
            for (var i = 0; i < range; i++)
                sheet.AutoSizeColumn(i);
        }
        catch (Exception ex)
        {
            //throw;
        }

        // Save the Excel spreadsheet to a MemoryStream and return it to the client
        using (var exportData = new MemoryStream())
        {
            workbook.Write(exportData);
            HttpContext.Current.Response.Buffer = true;
            HttpContext.Current.Response.ContentType = "application/vnd.ms-excel";
            HttpContext.Current.Response.AddHeader("Content-Disposition", string.Format("attachment;filename={0}", saveAsFileName));
            HttpContext.Current.Response.Clear();
            //HttpContext.Current.Response.ClearContent();
            HttpContext.Current.Response.BinaryWrite(exportData.GetBuffer());
            HttpContext.Current.Response.Flush();
            //HttpContext.Current.Response.End();
        }
    }
    // Group wise separate sheet
    /// <summary>
    /// Exports datatable to excel. If columnName is provided then exports in separate sheet for each column item. Ref: System.Linq
    /// </summary>
    /// <param name="dtColumn"></param>
    /// <param name="dt"></param>
    /// <param name="saveAsFileName"></param>
    /// <param name="strSheetName"></param>
    /// <param name="columnName"></param>
    /// <param name="columnSortExpression"></param>
    public static void ExportToExcelSeparateSheet(DataTable dtColumn, DataTable dt, string saveAsFileName, string strSheetName, string columnName, string columnSortExpression)
    {
        // Create a new workbook and a sheet 
        var workbook = new HSSFWorkbook();
        BuildWorkSheet(workbook, strSheetName, dtColumn, dt);
        string filter = "";

        // Create groupwise sheets in same workbook
        if (columnName.Length > 0)
        {
            var result = from row in dt.AsEnumerable()
                         orderby row.Field<string>(columnName)
                         group row by row.Field<string>(columnName) into grp
                         select new
                         {
                             SheetName = grp.Key,
                             ItemCount = grp.Count()
                         };

            foreach (var t in result)
            {
                filter = columnName + " = '" + t.SheetName + "'";

                using (DataView view = new DataView(dt))
                {
                    if (filter.Length > 0)
                    {
                        view.RowFilter = filter;
                    }
                    if (columnSortExpression.Length > 0)
                    {
                        view.Sort = columnSortExpression;
                    }

                    BuildWorkSheet(workbook, t.SheetName, dtColumn, view.ToTable());
                }
            }
        }

        // Save sheet
        SaveSpreadSheet(workbook, saveAsFileName);
    }
    public static void BuildWorkSheet(HSSFWorkbook workbook, string strSheetName, DataTable dtColumn, DataTable dt)
    {
        var sheet = workbook.CreateSheet(strSheetName);

        // Add header labels
        var rowIndex = 0;
        var colIndex = 0;
        ICellStyle headerCellStyle = GetHeaderCellStyle(workbook);

        var row = sheet.CreateRow(rowIndex);
        row.Height = 700;
        foreach (DataColumn dc in dtColumn.Columns)
        {
            ICell Hcell = row.CreateCell(colIndex++);
            Hcell.CellStyle = (headerCellStyle);
            Hcell.SetCellValue(dc.ColumnName);
        }
        rowIndex++;

        ICell datacell = null;
        //DataCellStyle Default
        ICellStyle DataCellStyle = GetDataCellStyleDefault(workbook);
        //DataCellStyle Date
        ICellStyle DataCellStyleDate = GetDataCellStyleDate(workbook);
        //DataCellStyle Number
        ICellStyle DataCellStyleNumber = GetDataCellStyleNumber(workbook);

        // Add data rows
        foreach (DataRow dr in dt.Rows)
        {
            row = sheet.CreateRow(rowIndex);
            colIndex = 0;
            foreach (DataColumn dc in dt.Columns)
            {

                datacell = row.CreateCell(colIndex);
                if (dc.DataType == typeof(DateTime))
                {
                    if (!string.IsNullOrEmpty(dr[colIndex].ToString()))
                    {
                        datacell.SetCellValue(Convert.ToDateTime(dr[colIndex].ToString()));
                    }
                    datacell.CellStyle = (DataCellStyleDate);
                }
                else if (dc.DataType == typeof(Decimal))
                {
                    if (!string.IsNullOrEmpty(dr[colIndex].ToString()))
                    {
                        datacell.SetCellValue(Convert.ToDouble(dr[colIndex].ToString()));
                    }
                    datacell.CellStyle = (DataCellStyleNumber);
                }
                else
                {
                    DataCellStyle.Alignment = HorizontalAlignment.LEFT;
                    DataCellStyle.WrapText = true;
                    datacell.CellStyle = (DataCellStyle);
                    datacell.SetCellValue(dr[colIndex].ToString());
                }
                colIndex++;
            }
            rowIndex++;
        }

        try
        {
            // Auto-size each column
            int range = sheet.GetRow(0).LastCellNum;
            for (var i = 0; i < range; i++)
                sheet.AutoSizeColumn(i);
        }
        catch (Exception ex)
        {
            //throw;
        }
    }
    public static void SaveSpreadSheet(HSSFWorkbook workbook, string saveAsFileName)
    {
        // Save the Excel spreadsheet to a MemoryStream and return it to the client
        using (var exportData = new MemoryStream())
        {
            workbook.Write(exportData);
            HttpContext.Current.Response.Buffer = true;
            HttpContext.Current.Response.ContentType = "application/vnd.ms-excel";
            HttpContext.Current.Response.AddHeader("Content-Disposition", string.Format("attachment;filename={0}", saveAsFileName));
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.BinaryWrite(exportData.GetBuffer());
            HttpContext.Current.Response.Flush();
        }
    }
    #endregion
*/
    }

    public static class Export
    {
        #region Export GenericList to CSV

        public static void ExportGenericListToCSV<T>(this List<T> list, string filename)
        {
            if (list != null && list.Count > 0)
            {
                string csv = GeCsvFromGenericList(list);
                ExportCSVHelper(csv, filename);
            }
        }

        private static string GeCsvFromGenericList<T>(this List<T> list)
        {
            StringBuilder sb = new StringBuilder();

            //Get the properties for type T for the headers
            PropertyInfo[] propInfos = typeof (T).GetProperties();
            for (int i = 0; i <= propInfos.Length - 1; i++)
            {
                sb.Append(propInfos[i].Name);

                if (i < propInfos.Length - 1)
                {
                    sb.Append(",");
                }
            }

            sb.AppendLine();

            //Loop through the collection, then the properties and add the values
            for (int i = 0; i <= list.Count - 1; i++)
            {
                T item = list[i];
                for (int j = 0; j <= propInfos.Length - 1; j++)
                {
                    object o = item.GetType().GetProperty(propInfos[j].Name).GetValue(item, null);
                    if (o != null)
                    {
                        string value = o.ToString();

                        //Check if the value contans a comma and place it in quotes if so
                        if (value.Contains(","))
                        {
                            value = string.Concat("\"", value, "\"");
                        }

                        //Replace any \r or \n special characters from a new line with a space
                        if (value.Contains("\r"))
                        {
                            value = value.Replace("\r", " ");
                        }
                        if (value.Contains("\n"))
                        {
                            value = value.Replace("\n", " ");
                        }

                        sb.Append(value);
                    }

                    if (j < propInfos.Length - 1)
                    {
                        sb.Append(",");
                    }
                }

                sb.AppendLine();
            }

            return sb.ToString();
        }

        #endregion

        [Obsolete]
        public static void ExportDataTableToHtmlExcel(DataTable dt, string strFileName)
        {
            if (dt != null && dt.Rows.Count > 0)
            {
                System.IO.StringWriter tw = new System.IO.StringWriter();
                System.Web.UI.HtmlTextWriter hw = new System.Web.UI.HtmlTextWriter(tw);

                //Get the HTML for the control.
                System.Web.UI.WebControls.DataGrid dgGrid = new System.Web.UI.WebControls.DataGrid();
                dgGrid.DataSource = dt;
                dgGrid.DataBind();
                dgGrid.RenderControl(hw);

                //Write the HTML back to the browser.
                //HttpContext.Current.Response.ContentType = "application/vnd.ms-excel";
                //HttpContext.Current.Response.AppendHeader("Content-Disposition", "attachment; filename=" + filename + "");
                //HttpContext.Current.Response.Write(tw.ToString());
                //HttpContext.Current.Response.End();
                ExportCSVHelper(tw.ToString(), strFileName);
            }
        }

        public static void ExportDataTableToCSV(DataTable dt, string strFileName)
        {
            StringBuilder sb = new StringBuilder();

            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataColumn column in dt.Columns)
                {
                    sb.Append(column.ColumnName + ",");
                }
                sb.AppendLine();

                foreach (DataRow row in dt.Rows)
                {
                    for (int i = 0; i < dt.Columns.Count; i++)
                    {
                        sb.Append(row[i].ToString().Replace(",", ";") + ",");
                    }
                    sb.AppendLine();
                }

                ExportCSVHelper(sb.ToString(), strFileName);
            }
        }

        public static void ExportDataSetToCSV(DataSet ds, string strFileName)
        {
            StringBuilder sb = new StringBuilder();
            try
            {
                foreach (DataTable table in ds.Tables)
                {
                    foreach (DataColumn column in table.Columns)
                    {
                        sb.Append(column.ColumnName + ",");
                    }

                    sb.AppendLine();

                    foreach (DataRow row in table.Rows)
                    {
                        for (int i = 0; i < table.Columns.Count; i++)
                        {
                            sb.Append(row[i].ToString().Replace(",", ";") + ",");
                        }

                        sb.AppendLine();
                    }

                    sb.AppendLine();
                    sb.AppendLine();
                }

                ExportCSVHelper(sb.ToString(), strFileName);
            }
            catch (Exception ex)
            {
            }
        }

        public static void ExportDataSetToXML(DataSet ds, string strFileName, bool IsWithSchema = false)
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                string header = @"<?xml version=""1.0"" encoding=""utf-8""?>";

                if (IsWithSchema)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        using (TextWriter streamWriter = new StreamWriter(memoryStream))
                        {
                            var xmlSerializer = new XmlSerializer(typeof (DataSet));
                            xmlSerializer.Serialize(streamWriter, ds);

                            //return Encoding.UTF8.GetString(memoryStream.ToArray());
                            var xml = Encoding.UTF8.GetString(memoryStream.ToArray());

                            ExportXMLHelper(xml, strFileName);
                        }
                    }
                }
                else
                {
                    sb.Append(header);
                    sb.Append("<DataSetList>");
                    var docresult = ds.GetXml();
                    sb.Append(docresult);
                    sb.Append("</DataSetList>");

                    ExportXMLHelper(sb.ToString(), strFileName);
                    sb.Clear();
                }
            }
            catch (Exception ex)
            {
            }
        }

        public static void ExportDatasetListToXml(List<DataSet> dsList, string strFileName, bool IsWithSchema = false)
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                string header = @"<?xml version=""1.0"" encoding=""utf-8""?>";
                //string header1 = "<?xml version=\"1.0\" encoding=\"utf-8\"?>";

                if (IsWithSchema)
                {
                    sb.Append(header);
                    sb.Append("<DataSetList>");
                    foreach (DataSet ds in dsList)
                    {
                        using (var memoryStream = new MemoryStream())
                        {
                            using (TextWriter streamWriter = new StreamWriter(memoryStream))
                            {
                                var xmlSerializer = new XmlSerializer(typeof (DataSet));
                                xmlSerializer.Serialize(streamWriter, ds);
                                var docresult = Encoding.UTF8.GetString(memoryStream.ToArray());
                                docresult = docresult.Substring(40, docresult.Length - 40);
                                    //docresult.Replace(header, "");

                                sb.Append(docresult);
                                docresult = null;
                            }
                        }
                    }
                    sb.Append("</DataSetList>");
                    ExportXMLHelper(sb.ToString(), strFileName);
                    sb.Clear();
                }
                else
                {
                    sb.Append(header);
                    sb.Append("<DataSetList>");
                    foreach (DataSet ds in dsList)
                    {
                        var docresult = ds.GetXml();
                        sb.Append(docresult);
                    }
                    sb.Append("</DataSetList>");

                    ExportXMLHelper(sb.ToString(), strFileName);
                    sb.Clear();
                }
            }
            catch (Exception ex)
            {
                //lblMsg.Text = "An error occured processing your request" + ex.Message;
            }
        }

        /// <summary>
        /// Usage call the following methods: FileUtility.DeleteAllFileHelper(strDirectory); ExportDatasetListToFile(dsList, strDirectory, false); ExportZippedHelper("xml", strDirectory, "ReservationInfoListZip");
        /// </summary>
        /// <param name="dsList"></param>
        /// <param name="strDirectory"></param>
        /// <param name="IsWithSchema"></param>
        public static void ExportDatasetListToFile(List<DataSet> dsList, string strDirectory, bool IsWithSchema = false)
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                string header = @"<?xml version=""1.0"" encoding=""utf-8""?>";

                if (IsWithSchema)
                {
                    foreach (DataSet ds in dsList)
                    {
                        using (var memoryStream = new MemoryStream())
                        {
                            using (TextWriter streamWriter = new StreamWriter(memoryStream))
                            {

                                var xmlSerializer = new XmlSerializer(typeof (DataSet));
                                xmlSerializer.Serialize(streamWriter, ds);
                                var docresult = Encoding.UTF8.GetString(memoryStream.ToArray());

                                sb.Append(docresult);
                                docresult = null;

                                string strPath = strDirectory + ds.DataSetName + ".xml";
                                ExportTxtToFileHelper(strPath, sb.ToString());
                            }
                        }
                        sb.Clear();
                    }
                }
                else
                {
                    foreach (DataSet ds in dsList)
                    {
                        sb.Append(header);

                        var docresult = ds.GetXml();
                        sb.Append(docresult);

                        string strPath = strDirectory + ds.DataSetName + ".xml";
                        ExportTxtToFileHelper(strPath, sb.ToString());

                        sb.Clear();
                    }
                }
            }
            catch (Exception ex)
            {
                //lblMsg.Text = "An error occured processing your request" + ex.Message;
            }
        }

        private static void ExportCSVHelper(string strCsv, string strFileName)
        {
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.AddHeader("Pragma", "public");
            HttpContext.Current.Response.AppendHeader("Content-Disposition",
                                                      "attachment; filename=" + strFileName + ".csv");
            HttpContext.Current.Response.ContentType = "text/csv";
            HttpContext.Current.Response.Write(strCsv);
            HttpContext.Current.Response.End();
        }

        private static void ExportXMLHelper(string strXml, string strFileName)
        {
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.AddHeader("Pragma", "public");
            HttpContext.Current.Response.AppendHeader("Content-Disposition",
                                                      "attachment; filename=" + strFileName + ".xml");
            HttpContext.Current.Response.ContentType = "text/xml; charset=utf-8";
            HttpContext.Current.Response.Write(strXml);
            HttpContext.Current.Response.End();
        }

        private static void ExportTxtToFileHelper(string strFileNameWithPath, string strContents)
        {
            if (!Directory.Exists(Path.GetDirectoryName(strFileNameWithPath))) //strDirectory
                Directory.CreateDirectory(Path.GetDirectoryName(strFileNameWithPath));

            File.WriteAllText(strFileNameWithPath, strContents);
        }

        //attach and add reference to Ionic.Zip.Reduced.dll v1.9.1.8
        private static void ExportZippedHelper(string strFileExtension, string strDirectory, string strFileName)
        {
            //HttpContext.Current.Response.Clear();
            //HttpContext.Current.Response.BufferOutput = false;
            //HttpContext.Current.Response.ContentType = "application/zip";
            //HttpContext.Current.Response.AddHeader("content-disposition", "attachment; filename=" + strFileName + ".zip");

            //using (Ionic.Zip.ZipFile zip = new Ionic.Zip.ZipFile())
            //{
            //    zip.CompressionLevel = Ionic.Zlib.CompressionLevel.None;
            //    zip.AddSelectedFiles("*." + strFileExtension, strDirectory, "", false);
            //    zip.Save(Response.OutputStream);
            //}

            //Response.Close();
        }
    }
}