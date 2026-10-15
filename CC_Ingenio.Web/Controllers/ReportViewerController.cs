using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CC_Ingenio.Common.Utils;
using CC_Ingenio.DataTranfer.UserInfo;
using CC_Ingenio_Resources;
using System.IO;
using System.Data.OleDb;
using System.Data;
using System.Xml.Linq;
using Excel = Microsoft.Office.Interop.Excel;
using System.Data.SqlClient;
using System.Runtime.InteropServices;
using CC_Ingenio.Web.ViewModel.CardBooking;
using CC_Ingenio.Web.Filters;


namespace CC_Ingenio.Web.Controllers
{
    public class ReportViewerController : Controller
    {
        //
        // GET: /ReportViewer/

        public ActionResult Index()
        {
            return View();
        }

        public void ShowReport()
        {
            try
            {
                bool isValid = true;

                string strReportName = System.Web.HttpContext.Current.Session["rptName"].ToString();
                string strempCode = System.Web.HttpContext.Current.Session["empCode"].ToString();
                var rptSource = System.Web.HttpContext.Current.Session["rptSource"];

                if (string.IsNullOrEmpty(strReportName))
                {
                    isValid = false;
                }

                if (isValid)
                {
                    ReportDocument rd = new ReportDocument();
                    string strPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "rpt//" + strReportName;
                    rd.Load(strPath);

                    if (rptSource != null && rptSource.GetType().ToString() != "System.String")
                        rd.SetDataSource(rptSource);
                    if (!string.IsNullOrEmpty(strempCode))
                        rd.SetParameterValue("empCode", strempCode);


                    rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "crReport");

                    // Clear all sessions value
                    Session["rptName"] = null;
                    Session["empCode"] = null;
                    Session["rptSource"] = null;
                }
                else
                {
                    Response.Write("<H2>Nothing Found; No Report name found</H2>");
                }
            }
            catch (Exception ex)
            {
                Response.Write(ex.ToString());
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public void ShowStatementImageReport()
        {
            string ACardNumber = System.Web.HttpContext.Current.Session["CardNumber"].ToString();
            string ABillMonth = System.Web.HttpContext.Current.Session["BillMonth"].ToString();    
            string ABillYear = System.Web.HttpContext.Current.Session["BillYear"].ToString();            
            try
            {
                string rptDoc = string.Empty;
                ViewBag.Month = Utils.GetMonths();
                ViewBag.Year = Utils.GetYearList();
                ReportGenerator objRpt = new ReportGenerator();
                string strReportPath = Server.MapPath("~/Reports/");
                LoginUserDTO loggedInUsername = (LoginUserDTO)System.Web.HttpContext.Current.Session["LOGGED_IN_USER"];
                string stParamValues = ACardNumber + "," + ABillMonth + "," + ABillYear;
                ReportDocument rd = objRpt.CreateReportDocument("StatementGeneration", "@P_CardNumber,@P_BillMonth,@P_BillYear", stParamValues, strReportPath, loggedInUsername.Username);
                rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "crReport");
            }
            catch (Exception ex)
            {
                Response.Write(ex.ToString());
            }         
        }

        #region All Reports

        #region Daily Collection Report

        #region DailyCollectionReport
        
        private void ExcelImport(string Filepath)
        {

            // Object for missing (or optional) arguments. 
            object oMissing = System.Reflection.Missing.Value;

            // Create an instance of Microsoft Excel, make it visible, 
            // and open Book1.xls. 
            Excel.Application oExcel = new Excel.Application();
            Excel.Workbooks oBooks = oExcel.Workbooks;
            Excel._Workbook oBook = null;
            oBook = oBooks.Open(Filepath, oMissing, oMissing,
            oMissing, oMissing, oMissing, oMissing, oMissing, oMissing,
            oMissing, oMissing, oMissing, oMissing, oMissing, oMissing);

            RunMacro(oExcel, new Object[] { "Run_Program" });

        }
        private void RunMacro(object oApp, object[] oRunArgs)
        {
            try
            {
                oApp.GetType().InvokeMember("Run",
                System.Reflection.BindingFlags.Default |
                System.Reflection.BindingFlags.InvokeMethod,
                null, oApp, oRunArgs);
            }
            catch(Exception ex) { Common.Logging.LoggerFactory.CreateLog().LogError("Error" + ex.StackTrace, 100, 2); }
        }
        

        private string ExportExcel()
        {

            Common.Logging.LoggerFactory.CreateLog().LogError("Error1" + "before intialization ", 100, 2);
            Excel.Application objXLApplication = null;
            Excel._Workbook objXLWorkBook = null;
            Excel._Worksheet objXLSheet = null;
            Excel.Range objXLRange = null;
            Common.Logging.LoggerFactory.CreateLog().LogError("Error2" + "after intialization ", 100, 2);
            string strExcelName = "";
            try
            {
                Common.Logging.LoggerFactory.CreateLog().LogError("Error3" + "before application1 ", 100, 2);
                objXLApplication = new Excel.Application();
                Common.Logging.LoggerFactory.CreateLog().LogError("Error4" + "after application1 ", 100, 2);
                string strExcel = "";                

                strExcel  = Server.MapPath("~/Reports/DailyBalanceReport.xlsx");
                strExcelName = strExcel;

               strExcelName = Server.MapPath("~/Reports/DailyBalanceReport_" + System.DateTime.Now.ToString("dd_MMM_yyy_hh_mm_ss_tt") + ".xlsx");
               // strExcelName =  "c:\\temp\\DailyBalanceReport_" + System.DateTime.Now.ToString("dd_MMM_yyy_hh_mm_ss_tt") + ".xlsx";//"//~/Reports/DailyBalanceReport_" + System.DateTime.Now.ToString("dd_MMM_yyy_hh_mm_ss_tt") + ".xlsx");
            


                if (!(System.IO.File.Exists(strExcelName)))
                    System .IO. File.Copy(strExcel, strExcelName);

               // strExcelName = Server.MapPath("~/Reports/DailyBalanceReport_24_Dec_2014_11_58_57_AM.xlsx");


                Common.Logging.LoggerFactory.CreateLog().LogError("Error5" + "after copy  ", 100, 2);
                objXLWorkBook = (Excel._Workbook)(objXLApplication.Workbooks.Add(strExcelName));
                objXLSheet = (Excel._Worksheet)objXLWorkBook.Sheets.get_Item(1);

                string strSQL = "EXEC PR_DailyCollectionReport ";
               // string conn = Utils.BuildConnectionString();

                string conn =
                "ODBC;Driver=SQL Server;Server=" +
                System.Configuration.ConfigurationManager.AppSettings["server"] + ";uid=" +
                System.Configuration.ConfigurationManager.AppSettings["UserId"] + ";pwd=" +
                 CC_Ingenio.Common.Utils.Utils.Helper.DecryptPassword(System.Configuration.ConfigurationManager.AppSettings["Password"].ToString()) + ";database=" +
                System.Configuration.ConfigurationManager.AppSettings["Database"] + ";";

                Common.Logging.LoggerFactory.CreateLog().LogError("Error6" + "before query table ", 100, 2);
                Excel.QueryTable querytable = null;
                Common.Logging.LoggerFactory.CreateLog().LogError("Error7" + "after query table  ", 100, 2);
                querytable = objXLSheet.QueryTables[1];
                querytable.CommandText = strSQL;
                querytable.Connection = conn;
                Common.Logging.LoggerFactory.CreateLog().LogError("Error8" + "before connection  ", 100, 2);
                objXLWorkBook.RefreshAll();
                Common.Logging.LoggerFactory.CreateLog().LogError("Error9" + "after connection  ", 100, 2);
                objXLApplication.Visible = true;

            //    ExcelImport(strExcelName);
     
                return strExcelName;

            }
            catch (Exception ex)
            {
                   
                Common.Logging.LoggerFactory.CreateLog().LogError("Error filename " + strExcelName, 100, 2);                
                Common.Logging.LoggerFactory.CreateLog().LogError(ex.Message, 100, 2);
               // Common.Logging.LoggerFactory.CreateLog().LogError(ex.InnerException, 100, 2);
             
                return strExcelName;
            }
            finally
            {
                                
                    if (objXLWorkBook != null)
                    {
                        Common.Logging.LoggerFactory.CreateLog().LogError("Error9" + "marshal before  ", 100, 2);
                        if (objXLRange != null)
                            Marshal.ReleaseComObject(objXLRange);
                        if (objXLApplication != null)
                            Marshal.ReleaseComObject(objXLApplication);
                        if (objXLSheet != null)
                            Marshal.ReleaseComObject(objXLSheet);
                        if (objXLWorkBook != null)
                            Marshal.ReleaseComObject(objXLWorkBook);
                        Common.Logging.LoggerFactory.CreateLog().LogError("Error9" + "after marshal   ", 100, 2);

                         
                    }
                }
            
            
        }

        public string DataSetToExcel(DataSet dsExport, string path, string tableName, ReportCriteria AReportCriteria)
        {

            string reportType = AReportCriteria.ReportType.ToString();
            if (path == string.Empty)
                return path;
            StreamWriter SWriter = new StreamWriter(path);
            string str = string.Empty;
            Int32 colspan = dsExport.Tables[0].Columns.Count;
            str += "<Table border=2><TR><TD align='center' colspan=" + Convert.ToString(colspan) + ">" + tableName + "</TD></tr>";
            str += "<tr><TD align='center' bgcolor='#D1DAA7' style='font-size:18px' colspan=" + Convert.ToString(colspan) + ">" + tableName;
            if (reportType.Substring(0, 2) == "DB")
               str+= "Daily Balance Report as on " + Utils.BusinessDate.Date.ToShortDateString() + "</TD></tr>";
            else if((reportType.Substring(0, 2) =="SR"))
                str += "Subsidiary Report as on " + Utils.BusinessDate.Date.ToShortDateString() + "</TD></tr>";
            else if (reportType == "MR")
                str += "Merchant Settlement Report as on " + Utils.BusinessDate.Date.ToShortDateString() + "</TD></tr>";
            else if (reportType.Substring(0, 2) == "GL")
            {
               // str += "GL Transaction Report as on " + Utils.BusinessDate.Date.ToShortDateString() + "</TD></tr>";

                if (AReportCriteria.FromDate == AReportCriteria.ToDate)
                  //  str += "<tr><TD align='center' bgcolor='#D1DAA7' style='font-size:18px' colspan=" + Convert.ToString(colspan) + ">" + "GL Transaction " + " Report as on " + AReportCriteria.FromDate.Date.ToShortDateString() + "</TD></tr>";
                    str += "GL Transaction Report as on " +  AReportCriteria.FromDate.Date.ToShortDateString() + "</TD></tr>";
                else
                    //str += "<tr><TD align='center' bgcolor='#D1DAA7' style='font-size:18px' colspan=" + Convert.ToString(colspan) + ">" + "GL Transaction " + " Report  From  " + AReportCriteria.FromDate.Date.ToShortDateString() + " To  " + AReportCriteria.ToDate.Date.ToShortDateString() + "</TD></tr>";
                str += "GL Transaction " + " Report  From  " + AReportCriteria.FromDate.Date.ToShortDateString() + " To  " + AReportCriteria.ToDate.Date.ToShortDateString() + "</TD></tr>";
            }
            else if (reportType.Substring(0, 2) == "CR")
            {
                // str += "GL Transaction Report as on " + Utils.BusinessDate.Date.ToShortDateString() + "</TD></tr>";

                if (AReportCriteria.FromDate == AReportCriteria.ToDate)
                    //  str += "<tr><TD align='center' bgcolor='#D1DAA7' style='font-size:18px' colspan=" + Convert.ToString(colspan) + ">" + "GL Transaction " + " Report as on " + AReportCriteria.FromDate.Date.ToShortDateString() + "</TD></tr>";
                    str += "Control  Report as on " + AReportCriteria.FromDate.Date.ToShortDateString() + "</TD></tr>";
                else
                    //str += "<tr><TD align='center' bgcolor='#D1DAA7' style='font-size:18px' colspan=" + Convert.ToString(colspan) + ">" + "GL Transaction " + " Report  From  " + AReportCriteria.FromDate.Date.ToShortDateString() + " To  " + AReportCriteria.ToDate.Date.ToShortDateString() + "</TD></tr>";
                    str += "Control  " + " Report  From  " + AReportCriteria.FromDate.Date.ToShortDateString() + " To  " + AReportCriteria.ToDate.Date.ToShortDateString() + "</TD></tr>";
            }
            //str += "<tr></tr><tr><TD align='left' bgcolor='#D1DAA7' style='font-size:10px' colspan=" + Convert.ToString(colspan) + ">" + tableName + " Printed On  " + Utils.BusinessDate.Date + "</TD></tr><tr></TR>";
            foreach (DataColumn DBCol in dsExport.Tables[0].Columns)
            {
                str += "<TD bgcolor='LightSteelBlue'>" + DBCol.ColumnName + "</TD>";
            }
            str += "</TR>";

            
            foreach (DataRow DBRow in dsExport.Tables[0].Rows)
            {
                str += "<TR>";
                foreach (DataColumn DBCol in dsExport.Tables[0].Columns)
                {
                    str += "<TD>" + Convert.ToString(DBRow[DBCol.ColumnName]) + "</TD>";
                }
                str += "</TR>";
            }


            if (dsExport.Tables.Count > 1)
            {
                str += "<TR>";
                str += "</TR>";

                foreach (DataRow DBRow in dsExport.Tables[1].Rows)
                {
                    str += "<TR>";
                    foreach (DataColumn DBCol in dsExport.Tables[1].Columns)
                    {
                        str += "<TD>" + Convert.ToString(DBRow[DBCol.ColumnName]) + "</TD>";
                    }
                    str += "</TR>";
                }
            }
            

            str += "</TABLE>";
            SWriter.WriteLine(str);
            SWriter.Flush();
            SWriter.Close();

                        

            if (path.Length > 5)
            {
                FileInfo file = new FileInfo(path);
                Response.Clear();
                Response.AddHeader("Content-Disposition", "attachment; filename=" + file.Name);
                Response.AddHeader("Content-Length", file.Length.ToString());
                Response.ContentType = "application/vnd.ms-excel";

                byte[] BOM = { 0xEF, 0xBB, 0xBF }; // The BOM for UTF-8 encoding.
                Response.BinaryWrite(BOM);
                                
                Response.WriteFile(file.FullName);
                Response.End();
            }
            

            return path;

        }

        public string DataTableToExcel(DataTable dsExport, string path, ReportCriteria AReportCriteria ,string type )
        {
            if (path == string.Empty)
                return path;
            StreamWriter SWriter = new StreamWriter(path);
            string str = string.Empty;
            Int32 colspan = dsExport.Columns.Count;
            str += "<Table border=2><TR><TD align='center' colspan=" + Convert.ToString(colspan) + ">" +" " + "</TD></tr>";
            if(AReportCriteria .FromDate == AReportCriteria .ToDate )
                str += "<tr><TD align='center' bgcolor='#D1DAA7' style='font-size:18px' colspan=" + Convert.ToString(colspan) + ">" + type + " Report as on " + AReportCriteria.FromDate.Date.ToShortDateString() + "</TD></tr>";         
            else
                str += "<tr><TD align='center' bgcolor='#D1DAA7' style='font-size:18px' colspan=" + Convert.ToString(colspan) + ">" + type + " Report  From  " + AReportCriteria.FromDate.Date.ToShortDateString() + " To  " + AReportCriteria.ToDate.Date.ToShortDateString() + "</TD></tr>";         
            //str += "<tr></tr><tr><TD align='left' bgcolor='#D1DAA7' style='font-size:10px' colspan=" + Convert.ToString(colspan) + ">" + tableName + " Printed On  " + Utils.BusinessDate.Date + "</TD></tr><tr></TR>";
            foreach (DataColumn DBCol in dsExport.Columns)
            {
                str += "<TD bgcolor='LightSteelBlue'>" + DBCol.ColumnName + "</TD>";
            }
            str += "</TR>";


            foreach (DataRow DBRow in dsExport.Rows)
            {
                str += "<TR>";
                foreach (DataColumn DBCol in dsExport.Columns)
                {
                    str += "<TD>" + Convert.ToString(DBRow[DBCol.ColumnName]) + "</TD>";
                }
                str += "</TR>";
            }


            str += "</TABLE>";
            SWriter.WriteLine(str);
            SWriter.Flush();
            SWriter.Close();

         
   
                                    

            return path;

        }

        public ActionResult DailyCollectionReport2()
        {

           
           string  path = ExportExcel();
            
            try
            {

                 // path = ExportExcel();

                //ReportGenerator objRpt = new ReportGenerator();
                //string strReportPath = Server.MapPath("~/Reports/");
                //LoginUserDTO loggedInUsername = (LoginUserDTO)System.Web.HttpContext.Current.Session["LOGGED_IN_USER"];
                //string strParamValues = Utils.OrganizationCode + "," + Utils.BusinessDate;
                //ReportDocument rd = objRpt.CreateReportDocument("RenewalCards", "@P_OrganizationCode,@P_BusinessDate", strParamValues, strReportPath, loggedInUsername.Username);
                //rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "crReport");
                FileInfo file = new FileInfo(path);
                Response.Clear();
                Response.AddHeader("Content-Disposition", "attachment; filename=" + file.Name);
                Response.AddHeader("Content-Length", file.Length.ToString());
                Response.ContentType = "application/octet-stream";

                byte[] BOM = { 0xEF, 0xBB, 0xBF }; // The BOM for UTF-8 encoding.
                Response.BinaryWrite(BOM);

                Response.WriteFile(file.FullName);
                Response.End();

            }
            catch
            {
                TempData["Message"] = Resources.MsgErrGenReport;
            }
            return File(path, "application/vnd.ms-excel");
        }

        public void DailyCollectionReport(ReportCriteria AReportCriteria)
        {
            string  path= string.Empty;
            var fpath = string.Empty;
            try
            {
                object missing = System.Reflection.Missing.Value;
                string ssqlconnectionstring = Utils.BuildConnectionString();
                SqlConnection con = new SqlConnection(ssqlconnectionstring);
                SqlCommand command = new SqlCommand();
                command.CommandTimeout = 1000000;
                command.CommandText = "PR_TransactionReport";
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@P_Type", AReportCriteria.ReportType);
                command.Parameters.AddWithValue("@P_FromDate", AReportCriteria.FromDate);
                command.Parameters.AddWithValue("@p_todate", AReportCriteria.ToDate);
                command.Connection = con;
                SqlDataAdapter da = new SqlDataAdapter(command);
                DataSet ds = new DataSet();
                da.Fill(ds, "ProductDetails");

                Utils.Purchase = "";
                Utils.Payments = "";
                Utils.CreditNote = "";

                if (Directory.Exists(Server.MapPath("~/Reports/Export/")) == false)
                    Directory.CreateDirectory(Server.MapPath("~/Reports/Export/"));
                
                //fpath = Server.MapPath("~/Reports/Export/DailyBalance" + System.DateTime.Now.ToString("yyyyMMddhhmmsstt") + ".xls");
               
               // if (fpath.Trim() != string.Empty)
              //  {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    if ((AReportCriteria.ReportType.Substring(0,2) == "DB") || (AReportCriteria.ReportType.Substring(0,2) == "SR"))
                    {
                        fpath = AReportCriteria.ReportType.Substring(0, 2) == "DB" ? Server.MapPath("~/Reports/Export/DailyBalance" + System.DateTime.Now.ToString("yyyyMMddhhmmsstt") + ".xls") :
                                Server.MapPath("~/Reports/Export/SubsidiaryReport" + System.DateTime.Now.ToString("yyyyMMddhhmmsstt") + ".xls");
                        if (System.IO.File.Exists(fpath) == false)
                            System.IO.File.Create(fpath).Close();
                        else
                            System.IO.File.Create(fpath).Close();
                        path = DataSetToExcel(ds, fpath, "", AReportCriteria);
                    }
                    else if (AReportCriteria.ReportType == "MR")
                    {

                        fpath = AReportCriteria.ReportType == "MR" ? Server.MapPath("~/Reports/Export/MerchantSettlementReport" + System.DateTime.Now.ToString("yyyyMMddhhmmsstt") + ".xls") :
                               Server.MapPath("~/Reports/Export/MerchantSettlementReport" + System.DateTime.Now.ToString("yyyyMMddhhmmsstt") + ".xls");

                        if (System.IO.File.Exists(fpath) == false)
                            System.IO.File.Create(fpath).Close();
                        else
                            System.IO.File.Create(fpath).Close();
                        path = DataSetToExcel(ds, fpath, "", AReportCriteria);
                    }
                    else if (AReportCriteria.ReportType.Substring(0, 2) == "GL")
                    {

                        fpath = AReportCriteria.ReportType.Substring(0, 2) == "GL" ? Server.MapPath("~/Reports/Export/GLTransactionReport" + System.DateTime.Now.ToString("yyyyMMddhhmmsstt") + ".xls") :
                               Server.MapPath("~/Reports/Export/GLTransactionReport" + System.DateTime.Now.ToString("yyyyMMddhhmmsstt") + ".xls");

                        if (System.IO.File.Exists(fpath) == false)
                            System.IO.File.Create(fpath).Close();
                        else
                            System.IO.File.Create(fpath).Close();
                        path = DataSetToExcel(ds, fpath, "", AReportCriteria);
                    }
                    else if (AReportCriteria.ReportType.Substring(0, 2) == "CR")
                    {

                        fpath = AReportCriteria.ReportType.Substring(0, 2) == "CR" ? Server.MapPath("~/Reports/Export/ControlReport" + System.DateTime.Now.ToString("yyyyMMddhhmmsstt") + ".xls") :
                               Server.MapPath("~/Reports/Export/ControlReport" + System.DateTime.Now.ToString("yyyyMMddhhmmsstt") + ".xls");

                        if (System.IO.File.Exists(fpath) == false)
                            System.IO.File.Create(fpath).Close();
                        else
                            System.IO.File.Create(fpath).Close();
                        path = DataSetToExcel(ds, fpath, "", AReportCriteria);
                    }
                    else
                    {
                        int i = 0;
                        string strTransType = "";
                        string strReport = string.Empty;
                        if (AReportCriteria.ReportType == "AC")
                            strReport = "Acpt";
                        else if (AReportCriteria.ReportType == "RJ")
                            strReport = "Rej";
                        else strReport = "All";

                        foreach (DataTable table in ds.Tables)
                        {
                            if (table.Rows.Count == 0)
                                continue;
                            strTransType = table.Rows[0]["TransactionType"].ToString();
                            if (strTransType == "PPD")
                                strTransType = "Purchases";
                            else if (strTransType == "CNT" || strTransType == "CADJ")
                                strTransType = "CreditNotes";
                            else if (strTransType == "PYC")
                                strTransType = "Payments";
                            else strTransType = "Others";

                            i++;
                            fpath = Server.MapPath("~/Reports/Export/TransactionReport_" + strReport + "_" + strTransType + "_" + System.DateTime.Now.ToString("yyyyMMddhhmmsstt") + ".xls");
                            string strType = strTransType;

                            strTransType = table.Rows[0]["TransactionType"].ToString();
                            if (strTransType == "PPD")
                                Utils.Purchase = fpath;
                            else if (strTransType == "CNT" || strTransType == "CADJ")
                                Utils.CreditNote = fpath;
                            else if (strTransType == "PYC")
                                Utils.Payments = fpath;


                            DataTableToExcel(table, fpath, AReportCriteria, strType);

                            //FileInfo file = new FileInfo(fpath);
                            //Response.Clear();
                            //Response.AddHeader("Content-Disposition", "attachment; filename=" + file.Name);
                            //Response.AddHeader("Content-Length", file.Length.ToString());
                            //Response.ContentType = "application/vnd.ms-excel";
                            //Response.WriteFile(file.FullName);
                            //Response.End();
                            //Response.Flush();

                        }

                        //if (fpath.Length > 5)
                        //{
                        //    FileInfo file = new FileInfo(fpath);
                        //    Response.Clear();
                        //    Response.AddHeader("Content-Disposition", "attachment; filename=" + file.Name);
                        //    Response.AddHeader("Content-Length", file.Length.ToString());
                        //    Response.ContentType = "application/vnd.ms-excel";
                        //    Response.WriteFile(file.FullName);
                        //    Response.End();
                        //}

                    }
                    if (ds.Tables.Count != 0)
                    {
                        string str = Server.MapPath(@"~/Reports/Export");
                        str = str.Replace("\\", @"/");
                        TempData["Message"] = "Please click the corresponding link to view the files.";
                        //FileInfo file = new FileInfo(fpath);
                        //Response.Clear();
                        //Response.AddHeader("Content-Disposition", "attachment; filename=" + file.Name);
                        //Response.AddHeader("Content-Length", file.Length.ToString());
                        //Response.ContentType = "application/vnd.ms-excel";
                        //Response.WriteFile(file.FullName);

                    }
                    else
                    {
                        TempData["Message"] = "No Transaction is posted.";
                        Utils.Purchase = "";
                        Utils.Payments = "";
                        Utils.CreditNote = "";
                    }
                    // }

                }
                else
                {
                    TempData["Message"] = "No Transaction is posted.";
                }
            }
            catch
            {
                TempData["Message"] = Resources.MsgErrGenReport;
            }

           // return File(fpath, "application/vnd.ms-excel");
        }

        public ActionResult OpenReportExcelFromDump(string AType,DateTime AFromDate)
        {
            string fpath = "";
            string fileName = "";
            try
            {

                string ssqlconnectionstring = Utils.BuildConnectionString();
                SqlConnection con = new SqlConnection(ssqlconnectionstring);
                SqlCommand command = new SqlCommand();
                command.CommandTimeout = 1000000;

                command.CommandText = "PR_GetReportFileNameFromDump";
                command.CommandType = CommandType.StoredProcedure;  
                command.Parameters.AddWithValue("@P_Date", AFromDate);
                command.Parameters.AddWithValue("@P_ReportType", AType);
                
                command.Connection = con;
                SqlDataAdapter da = new SqlDataAdapter(command);
                DataSet dsReports = new DataSet();
                da.Fill(dsReports, "Reports");
                con.Close();

                DataTable dtReports = new DataTable();
                if (dsReports != null)
                {
                    if (dsReports.Tables.Count > 0)
                    {
                        dtReports = dsReports.Tables[0];
                    }
                }

                if (dtReports.Rows.Count > 0)
                {
                    fileName = dtReports.Rows[0][0].ToString();
                }

                //fpath = Server.MapPath("~/Reports/Export/" + fileName + ".xls");

                fpath = Server.MapPath("~/Reports/Export/" + fileName );


                if (System.IO.File.Exists(fpath) == true)
                {
                    //FileInfo file = new FileInfo(fpath);
                    //Response.Clear();
                    //Response.AddHeader("Content-Disposition", "attachment; filename=" + file.Name);
                    //Response.AddHeader("Content-Length", file.Length.ToString());
                    //Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

                    //byte[] BOM = { 0xEF, 0xBB, 0xBF }; // The BOM for UTF-8 encoding.
                    //Response.BinaryWrite(BOM);

                    //Response.WriteFile(file.FullName);
                    //Response.End();

                    //FileStream fs1 = new FileStream(fpath, FileMode.Open, FileAccess.Read);

                    using (FileStream fs1 = new FileStream(fpath, FileMode.Open, FileAccess.Read))
                    {
                        byte[] data1 = new byte[fs1.Length];
                        fs1.Read(data1, 0, data1.Length);
                        this.Response.ContentType = "application/xlsx";
                        Response.AddHeader("content-disposition", "attachment; filename=" + fpath + "");
                        Response.BinaryWrite(data1);
                        Response.End(); 
                    }
                     

                }
                else
                {
                    TempData["Message"] = "File not generated.";
                    fpath = "";
                }

            }
            catch { TempData["Message"] = "File not generated."; }
            if (fpath == null || fpath == "")
            {
                var reportTypes = Utils.GetReportTypes();
                ViewBag.ReportTypes = reportTypes;
                ReportCriteria objReportCriteria = new ReportCriteria();
                objReportCriteria.FromDate = Utils.BusinessDate;
                objReportCriteria.ToDate = Utils.BusinessDate;
                return View("DailyCollectionReport", objReportCriteria);
            }
            else
            {
                return File(fpath, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
            }

        }



        [AuthorizeUserAttribute(Entity = "RPT", AccessLevel = new UserActivities[] { UserActivities.View })]
        public ActionResult DailyCollectionReportList()
        {
            var reportTypes = Utils.GetReportTypes();
            ViewBag.ReportTypes = reportTypes;

            ReportCriteria objReportCriteria = new ReportCriteria();
            objReportCriteria.FromDate = Utils.BusinessDate;
            objReportCriteria.ToDate= Utils.BusinessDate;

            return View("DailyCollectionReport", objReportCriteria);
        }

        [AuthorizeUserAttribute(Entity = "RPT", AccessLevel = new UserActivities[] { UserActivities.View })]
        public ActionResult  ShowDailyCollectionReport(ReportCriteria AReportCriteria)
        {
            var reportTypes = Utils.GetReportTypes();
            ViewBag.ReportTypes = reportTypes;
            string str = AReportCriteria.ReportType;

            if (AReportCriteria.ReportType.ToString() == "DBN" || AReportCriteria.ReportType.ToString() == "DBO" || AReportCriteria.ReportType.ToString() == "DBW" ||
                AReportCriteria.ReportType.ToString() == "SRN" || AReportCriteria.ReportType.ToString() == "SRO" || AReportCriteria.ReportType.ToString() == "SRW")
            OpenReportExcelFromDump(AReportCriteria.ReportType,AReportCriteria .FromDate  );
            else DailyCollectionReport(AReportCriteria);

            ReportCriteria objReportCriteria = new ReportCriteria();
            objReportCriteria.FromDate = Utils.BusinessDate;
            objReportCriteria.ToDate = Utils.BusinessDate;
            return View("DailyCollectionReport", objReportCriteria);
           
        }


        public ActionResult OpenExcel(string AType)
        {
            string fpath = "";
            try
            {
                

                if (AType == "PR")
                {
                    fpath = Utils.Purchase;
                }
                else if (AType == "CN")
                {
                    fpath = Utils.CreditNote;
                }
                if (AType == "PY")
                {
                    fpath = Utils.Payments;
                }

                if (fpath.Length > 5)
                {
                    FileInfo file = new FileInfo(fpath);
                    Response.Clear();
                    Response.AddHeader("Content-Disposition", "attachment; filename=" + file.Name);
                    Response.AddHeader("Content-Length", file.Length.ToString());
                    Response.ContentType = "application/vnd.ms-excel";

                    byte[] BOM = { 0xEF, 0xBB, 0xBF }; // The BOM for UTF-8 encoding.
                    Response.BinaryWrite(BOM);

                    Response.WriteFile(file.FullName);
                    Response.End();
                }
              
            }
            catch { TempData["Message"] = "File not generated."; }
            if (fpath == null || fpath =="")
            {
                var reportTypes = Utils.GetReportTypes();
                ViewBag.ReportTypes = reportTypes;
                ReportCriteria objReportCriteria = new ReportCriteria();
                objReportCriteria.FromDate = Utils.BusinessDate;
                objReportCriteria.ToDate = Utils.BusinessDate;
                return View("DailyCollectionReport", objReportCriteria);
            }
            else
            {
                return File(fpath, "application/vnd.ms-excel");
            }
            
        }

        //public void dumbexcel()
        //{
        //    using (ExcelPackage pck = new ExcelPackage())
        //    {
        //        //Create the worksheet
        //        ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Demo");

        //        //Load the datatable into the sheet, starting from cell A1. Print the column names on row 1
        //        ws.Cells["A1"].LoadFromDataTable(tbl, true);

        //        //Format the header for column 1-3
        //        using (ExcelRange rng = ws.Cells["A1:C1"])
        //        {
        //            rng.Style.Font.Bold = true;
        //            rng.Style.Fill.PatternType = ExcelFillStyle.Solid;                      //Set Pattern for the background to Solid
        //            rng.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(79, 129, 189));  //Set color to dark LightSteelBlue
        //            rng.Style.Font.Color.SetColor(Color.White);
        //        }

        //        //Example how to Format Column 1 as numeric 
        //        using (ExcelRange col = ws.Cells[2, 1, 2 + tbl.Rows.Count, 1])
        //        {
        //            col.Style.Numberformat.Format = "#,##0.00";
        //            col.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
        //        }

        //        //Write it back to the client
        //        Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        //        Response.AddHeader("content-disposition", "attachment;  filename=ExcelDemo.xlsx");
        //        Response.BinaryWrite(pck.GetAsByteArray());
        //    }
        //}

        #endregion

        #endregion

        #endregion



    }
}
