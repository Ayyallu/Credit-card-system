using System;
using System.Collections;
using System.Collections.Specialized;
using System.Configuration;
using System.ComponentModel;
using System.Data;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.IO;
using System.Text;
using System.Security.Policy;
using System.Data.SqlClient;
using System.Net;
using CrystalDecisions.ReportSource;
using CrystalDecisions.Shared;
using CrystalDecisions.CrystalReports.Engine;
using System.Data.Common;
//using Microsoft.Reporting.WebForms;


namespace CC_Ingenio.Common.Utils
{
    /// <summary>
    /// stores one parameter name and value
    /// </summary>
    /// 
    public class ReportParam
    {

        
        private string FParameterName;
        private object FParameterValue;
        

        #region  ---- Constants ----
        
        //private static string const CNST_S5LOGOPATH 

        #endregion 

        public ReportParam(string AParameterName, object AParameterValue)
        {
            FParameterName = AParameterName;
            FParameterValue = AParameterValue;
        }

        public string ParameterName
        {
            get
            {
                return FParameterName;
            }
        }

        public object ParameterValue
        {
            get
            {
                return FParameterValue;
            }

            set
            {
                FParameterValue = value;
            }
        }
    } //end class


    public class ReportParams : System.Collections.CollectionBase
    {
        public void Add(ReportParam AReportParam)
        {
            List.Add(AReportParam);
        }

        public void Remove(int Index)
        {
            if ((Index > Count - 1) || (Index < 0))
            {
                throw (new Exception("Error while removing a parameter"));
            }
            else
            {
                List.RemoveAt(Index);
            } //end else
        }

        public void Remove(string AName)
        {
            int intIndex = IndexOf(AName);
            if (intIndex != -1)
                Remove(intIndex);
        }

        public ReportParam Item(int Index)
        {
            return (ReportParam)List[Index];
        }

        public ReportParam ItemByName(string AName)
        {
            for (int intLoop = 0; intLoop < List.Count; intLoop++)
            {
                if (Item(intLoop).ParameterName == AName)
                    return (Item(intLoop));
            }

            return (null);
        }

        public int IndexOf(string AName)
        {
            for (int intLoop = 0; intLoop < List.Count; intLoop++)
            {
                if (Item(intLoop).ParameterName == AName)
                    return (intLoop);
            }

            return (-1);
        }

        public void ChangeItemValue(string AItemName, object ANewValue)
        {
            ReportParam rpmTemp = ItemByName(AItemName);
            if (rpmTemp != null)
                rpmTemp.ParameterValue = ANewValue;
        }
    }
    /// <summary>
    /// Summary description for ReportGenerator.
    /// </summary>
    public class ReportGenerator
    {
        ReportDocument repDoc;
        ReportDocument subRepDoc;
        Sections crSections;
        ReportObjects crReportObjects;
        SubreportObject crSubreportObject;
        CrystalDecisions.CrystalReports.Engine.Database crDatabase;
        CrystalDecisions.CrystalReports.Engine.Tables crTables;
        ReportParams FReportParams;  //temporary collection to keep the report parameters
        //these get cleared on closing report
        ReportParams FSubReportParams;
        TableLogOnInfo crLogOnInfo;
        ConnectionInfo crConnInfo;
        HttpResponse FResponse;
        string FReportName;
        string FRequestNo;
        int FPrintCount = 1;//counter for current report print
        int FCallslno = 0;
        int FNoofCopies = 0;
        string FPdfName;
        string FName;//Name of the Report (RPTPOL, RPTDEBIT....)
        string FReportDisplayName = "*";//Display Name of the Report (RptPolicy, RptDebit....)		
        //The actual report name which is passed to ocx and subsequently used for downloading.
        //M002
        string FReportNameForPrint;

        public ReportGenerator()
        {
        }

        public string PdfName
        {
            get
            {
                return (FPdfName);
            }
        }

        public int NoofCopies
        {
            get
            {
                return (FNoofCopies);
            }

            set
            {
                FNoofCopies = value;
            }
        }

        public string Name
        {
            get
            {
                return (FName);
            }

            set
            {
                FName = value;
            }
        }

        public ReportDocument CrystalReport
        {
            get
            {
                return (repDoc);
            }
        }


        public string ReportDisplayName
        {
            get
            {
                return (FReportDisplayName);
            }

            set
            {
                FReportDisplayName = value;
            }
        }

        public string RequestNo
        {
            get
            {
                return (FRequestNo);
            }

            set
            {
                FRequestNo = value;
            }
        }

        public HttpResponse Response
        {
            get
            {
                return (FResponse);
            }

            set
            {
                FResponse = value;
            }
        }

        public string ReportNameForPrint
        {
            get
            {
                return (FReportNameForPrint);
            }
        }

        public int Callslno
        {
            get
            {
                return FCallslno;
            }
            set
            {
                FCallslno = value;
            }
        }

        private void SetConnectionParams()
        {
            //crConnInfo.UserID = ConfigurationSettings.AppSettings["USERNAME"];
            //crConnInfo.Password = ConfigurationSettings.AppSettings["PWD"];
            crConnInfo.UserID = ConfigurationManager.AppSettings["USERNAME"];
            crConnInfo.Password = ConfigurationManager.AppSettings["PWD"];
        }

        private void ApplyLogonInfo(ReportDocument AReportDoc, int ATableIndex, string ADatabase)
        {
            //	crLogOnInfo = AReportDoc.Database.Tables[ATableIndex].LogOnInfo;
            // Set the connection information for database alone
            //coz all the rest of the connection info has already been set
            //and then assign it to for login information.
            crLogOnInfo = new TableLogOnInfo();
            crConnInfo.DatabaseName = ADatabase;
            //crConnInfo.ServerName = ConfigurationSettings.AppSettings["SERVERNAME"].ToString();
            crConnInfo.ServerName = ConfigurationManager.AppSettings["SERVERNAME"].ToString();
            crLogOnInfo.ConnectionInfo = crConnInfo;
            AReportDoc.Database.Tables[ATableIndex].ApplyLogOnInfo(crLogOnInfo);
        }

        private void SetAllReportConnections(ReportDocument AReportDoc)
        {
            //string strMarineDB = ConfigurationSettings.AppSettings["DATABASE"];
            string strMarineDB = ConfigurationManager.AppSettings["DATABASE"];
            for (int i = 0; i < AReportDoc.Database.Tables.Count; i++)
            {
                string strLocation = AReportDoc.Database.Tables[i].Location;
                ApplyLogonInfo(AReportDoc, i, strMarineDB);
                AReportDoc.Database.Tables[i].Location = strLocation;
            }
        }

        private void SetupReportConnections()
        {
            SetAllReportConnections(repDoc);

            for (int i = 0; i < repDoc.Subreports.Count; i++)
            {
                string x = repDoc.Subreports[i].Name;
                SetAllReportConnections(repDoc.Subreports[i]);
            }
        }
        public void RefreshReport()
        {
            repDoc.Refresh();
            SetupReportConnections();
        }

        /// <summary>
        /// This method is invoked if report has to dirctly 
        /// interact with database
        /// </summary>
        /// <param name="AServer"></param>
        /// <param name="AReportName"></param>
        public ReportGenerator(HttpServerUtility AServer,
             string AReportName)
        {
            repDoc = new ReportDocument();
            subRepDoc = new ReportDocument();
            crConnInfo = new ConnectionInfo();
            SetConnectionParams();
            FReportName = AReportName;
            //FRequestNo = ARequestNo;
            string strRptName_WithPath_WithoutExt = AServer.MapPath("~/Reports/" + FReportName);
            repDoc.Load(strRptName_WithPath_WithoutExt + ".rpt");//End of M002     

            repDoc.ReportOptions.EnableSaveDataWithReport = false;
            crDatabase = repDoc.Database;
            crTables = crDatabase.Tables;
            FReportParams = new ReportParams();
            FSubReportParams = new ReportParams();
            SetupReportConnections();
        }

        public void AddParameter(string AParameterName, object AParameterValue)
        {
            FReportParams.Add(new ReportParam(AParameterName, AParameterValue));
        }

        public void AddSubReportParameter(string AParameterName, object AParameterValue)
        {
            FSubReportParams.Add(new ReportParam(AParameterName, AParameterValue));
        }

        /// <summary>
        /// checks if the passed parameter is available in report
        /// </summary>
        /// <param name="AParameterName"></param>
        /// <returns></returns>
        private bool HasParams(string AParameterName)
        {
            for (int i = 0; i < repDoc.DataDefinition.ParameterFields.Count; i++)
            {
                if (repDoc.DataDefinition.ParameterFields[i].ParameterFieldName.ToUpper() ==
                    AParameterName.ToUpper())
                    return (true); //found!! 
            } //end for
            return (false);
        }

        private bool HasSubReportParams(ReportDocument ASubReport, string AParameterName)
        {
            for (int i = 0; i < ASubReport.DataDefinition.ParameterFields.Count; i++)
            {
                if (ASubReport.DataDefinition.ParameterFields[i].ParameterFieldName.ToUpper() ==
                    AParameterName.ToUpper())
                    return (true); //found!! 
            } //end for
            return (false);
        }

        public void LoadReportParams()
        {
            for (int intLoop = 0; intLoop < FReportParams.Count; intLoop++)
            {
                ParameterDiscreteValue dParam = new ParameterDiscreteValue();
                ParameterValues currentValues = new ParameterValues();
                string strParameterName = FReportParams.Item(intLoop).ParameterName;
                object objParameterValue = FReportParams.Item(intLoop).ParameterValue;

                if (!HasParams(strParameterName)) continue;
                repDoc.DataDefinition.ParameterFields[strParameterName].DefaultValues.Clear();
                dParam.Value = objParameterValue;
                currentValues = repDoc.DataDefinition.ParameterFields[strParameterName].CurrentValues;
                currentValues.Add(dParam);
                repDoc.DataDefinition.ParameterFields[strParameterName].ApplyCurrentValues(currentValues);
            }//end for
        }

        public void LoadSubReportParams(string ASubReportName)
        {
            if (FSubReportParams.Count == 0) return;
            crSections = repDoc.ReportDefinition.Sections;

            foreach (Section crSection in crSections)
            {
                crReportObjects = crSection.ReportObjects;
                foreach (ReportObject crReportObject in crReportObjects)
                {
                    if (crReportObject.Kind == ReportObjectKind.SubreportObject)
                    {
                        crSubreportObject = (SubreportObject)crReportObject;
                        if (crSubreportObject.SubreportName == ASubReportName)
                        {
                            subRepDoc = crSubreportObject.OpenSubreport(crSubreportObject.SubreportName);
                            ///found report
                            for (int intLoop = 0; intLoop < FSubReportParams.Count; intLoop++)
                            {
                                ParameterDiscreteValue dParam = new ParameterDiscreteValue();
                                ParameterValues currentValues = new ParameterValues();
                                string strParameterName = FSubReportParams.Item(intLoop).ParameterName;
                                object objParameterValue = FSubReportParams.Item(intLoop).ParameterValue;

                                if (!HasSubReportParams(subRepDoc, strParameterName)) continue;
                                subRepDoc.DataDefinition.ParameterFields[strParameterName].DefaultValues.Clear();
                                dParam.Value = objParameterValue;
                                currentValues = subRepDoc.DataDefinition.ParameterFields[strParameterName].CurrentValues;
                                currentValues.Add(dParam);
                                subRepDoc.DataDefinition.ParameterFields[strParameterName].ApplyCurrentValues(currentValues);
                            }//end for			
                            break;
                        }
                    } ///end if
                }
            }
        }

        public void RemoveParameter(string AParameterName)
        {
            FReportParams.Remove(AParameterName);
        }

        public void RemoveSubReportParameter(string AParameterName)
        {
            FSubReportParams.Remove(AParameterName);
        }


        /// <summary>
        /// This method is invoked if 
        /// data set is send to the report 
        /// </summary>
        /// <param name="ADataSet"></param>
        /// <returns></returns>
        public string SearchReport(DataSet ADataSet, HttpServerUtility AServer)
        {
            if (ADataSet.Tables[0].Rows.Count == 0)
                return "";

            repDoc.SetDataSource(ADataSet);
            LoadReportParams();

            //string strReportNameForPrint = FReportName
            //    + "_" + HttpContext.Current.Session["LOGINID"] + "_" + FPrintCount.ToString() + ".pdf";

            string strReportNameForPrint = FRequestNo.ToString() + ".pdf";


            //string strExportFilePath = ConfigurationManager.AppSettings["REPORTPATH"].ToString() + strReportNameForPrint;
            string strExportFilePath = AServer.MapPath("") + "//Reports//Temp//" + strReportNameForPrint;
            FPdfName = FPdfName != null && FPdfName != "" ? FPdfName + "|" + strExportFilePath : strExportFilePath;
            FPdfName = FPdfName.Replace("\\", "\\\\");
            DiskFileDestinationOptions crDiskFileDestinationOptions = new DiskFileDestinationOptions();
            crDiskFileDestinationOptions.DiskFileName = strExportFilePath;
            FReportNameForPrint = strReportNameForPrint;
            repDoc.ExportOptions.DestinationOptions = crDiskFileDestinationOptions;
            repDoc.ExportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
            repDoc.ExportOptions.ExportFormatType = ExportFormatType.PortableDocFormat;
            if (System.IO.File.Exists(strExportFilePath))
                System.IO.File.Delete(strExportFilePath);
            repDoc.Export();
            FPrintCount++;
            // return strReportNameForPrint;
            return strExportFilePath;
        }

        public string GenerateReport(string ASubReportName, HttpServerUtility AServer)
        {
            LoadReportParams();
            if (ASubReportName != null)
                LoadSubReportParams(ASubReportName);

            string strReportNameForPrint = FReportName
                + "_" + HttpContext.Current.Session["LOGINNAME"] + "_" + FPrintCount.ToString() + "_" + String.Format("{0:dd-MM-yyyy-hh-mm-ss}", DateTime.Now) + ".pdf";
            string strExportFilePath = AServer.MapPath("../../PDFS").ToString() + '/' + strReportNameForPrint;
            //string strExportFilePath = getserver.MapPath("") + "//Temp//" + strReportNameForPrint;
            FPdfName = FPdfName != null && FPdfName != "" ? FPdfName + "|" + strExportFilePath : strExportFilePath;
            FPdfName = FPdfName.Replace("\\", "\\\\");
            DiskFileDestinationOptions crDiskFileDestinationOptions = new DiskFileDestinationOptions();
            crDiskFileDestinationOptions.DiskFileName = strExportFilePath;
            FReportNameForPrint = strReportNameForPrint;
            repDoc.ExportOptions.DestinationOptions = crDiskFileDestinationOptions;
            repDoc.ExportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
            //repDoc.ExportOptions.ExportFormatType = ExportFormatType.CrystalReport;         
            repDoc.ExportOptions.ExportFormatType = ExportFormatType.PortableDocFormat;
            if (System.IO.File.Exists(strExportFilePath))
                System.IO.File.Delete(strExportFilePath);
            repDoc.Export();
            repDoc.Close();
            FPrintCount++;
            //return ConfigurationManager.AppSettings["PDFPATH"].ToString() + '/' + strReportNameForPrint;
           // return AServer.MapPath("../../PDFS/")  + strReportNameForPrint;
            return  strReportNameForPrint;


        }
	
        public ReportDocument GenerateReport(string ASubReportName, string AReportName)
        {
            LoadReportParams();
            if (ASubReportName != null)
                LoadSubReportParams(ASubReportName);

            //string strReportNameForPrint = FReportName
            //    + "_" + HttpContext.Current.Session["LOGINID"] + "_" + FPrintCount.ToString() + ".rpt";
            //string strExportFilePath = ConfigurationSettings.AppSettings["REPORTPATH"].ToString() + '\\' + strReportNameForPrint;
            //FPdfName = FPdfName != null && FPdfName != "" ? FPdfName + "|" + strExportFilePath : strExportFilePath;
            //FPdfName = FPdfName.Replace("\\", "\\\\");
            DiskFileDestinationOptions crDiskFileDestinationOptions = new DiskFileDestinationOptions();
            //crDiskFileDestinationOptions.DiskFileName = ConfigurationSettings.AppSettings["REPORTPATH"].ToString() + '\\' + AReportName + ".rpt";
            crDiskFileDestinationOptions.DiskFileName = ConfigurationManager.AppSettings["REPORTPATH"].ToString() + '\\' + AReportName + ".rpt";
            //FReportNameForPrint = strReportNameForPrint;
            repDoc.ExportOptions.DestinationOptions = crDiskFileDestinationOptions;
            repDoc.ExportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
            repDoc.ExportOptions.ExportFormatType = ExportFormatType.PortableDocFormat;

            //if (System.IO.File.Exists(strExportFilePath))
            //    System.IO.File.Delete(strExportFilePath);
            //repDoc.Export();
            //FPrintCount++;
            return repDoc;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="AstrreportName"></param>
        /// <param name="AstrParameterFields"></param>
        /// <param name="AstrParameterValues"></param>
        /// <param name="path"></param>
        /// <param name="ALoginName"></param>
        /// <returns></returns>
        public ReportDocument CreateReportDocument(string AstrreportName, string AstrParameterFields, string AstrParameterValues,
                                                     string path, string ALoginName)
        {
            ReportDocument repDoc = new ReportDocument();
            CrystalDecisions.CrystalReports.Engine.Database crDatabase;
            CrystalDecisions.CrystalReports.Engine.Tables crTables;
            TableLogOnInfo crLogOnInfo;
            ConnectionInfo crConnInfo = new ConnectionInfo();
            string strRptName_WithPath_WithoutExt = path + AstrreportName;

            repDoc.Load(strRptName_WithPath_WithoutExt + ".rpt");
            repDoc.Refresh();
            crDatabase = repDoc.Database;
            crTables = crDatabase.Tables;

            string ServerName = ConfigurationManager.AppSettings["Server"].ToString();
            crConnInfo.ServerName = ServerName.Replace("[", "");
            crConnInfo.DatabaseName = ConfigurationManager.AppSettings["Database"].ToString();
            crConnInfo.UserID = ConfigurationManager.AppSettings["UserId"].ToString();
            crConnInfo.Password = CC_Ingenio.Common.Utils.Utils.Helper.DecryptPassword(ConfigurationManager.AppSettings["Password"].ToString());

            foreach (CrystalDecisions.CrystalReports.Engine.Table crTable in crTables)
            {
                crLogOnInfo = crTable.LogOnInfo;
                crLogOnInfo.ConnectionInfo = crConnInfo;
                crTable.ApplyLogOnInfo(crLogOnInfo);
                crTable.Location = crTable.Name;
            }

            Sections crSections = repDoc.ReportDefinition.Sections;
            string[] strParamFlds;
            string[] strParamVals;

            strParamFlds = AstrParameterFields.Split(',');
            strParamVals = AstrParameterValues.Split(',');

            string AFileNameFirstPart = "";
            if (AstrParameterFields.Length != 0)
            {
                for (int i = 0; i < strParamFlds.Length; i++)
                {
                    ParameterDiscreteValue dParam = new ParameterDiscreteValue();
                    ParameterValues currentValues = new ParameterValues();
                    repDoc.DataDefinition.ParameterFields[strParamFlds[i]].DefaultValues.Clear();
                    dParam.Value = strParamVals[i];
                    currentValues = repDoc.DataDefinition.ParameterFields[strParamFlds[i]].CurrentValues;
                    currentValues.Add(dParam);
                    repDoc.DataDefinition.ParameterFields[strParamFlds[i]].ApplyCurrentValues(currentValues);
                    AFileNameFirstPart = AFileNameFirstPart + strParamVals[i] + "-";
                }
            }
            return repDoc;
        }

        public string PrintReportWithParamNewWindow(string AstrreportName, string AstrParameterFields, string AstrParameterValues, string path,
      string ALoginName)//, string AstrServerName, string AstrdatabaseName)
        {
           
                // Business.Common FCommon = new Business.Common();
                //string strRptName_WithPath_WithoutExt = Aserver.MapPath("~/Reports/" + AstrreportName);
                //return strRptName_WithPath_WithoutExt;
                ReportDocument repDoc = new ReportDocument();
                ReportDocument subRepDoc = new ReportDocument();
                Sections crSections;
                //ReportObjects crReportObjects;
                //SubreportObject crSubreportObject;
                CrystalDecisions.CrystalReports.Engine.Database crDatabase;
                CrystalDecisions.CrystalReports.Engine.Tables crTables;
                TableLogOnInfo crLogOnInfo;
                ConnectionInfo crConnInfo = new ConnectionInfo();


                string strRptName_WithPath_WithoutExt = System.Web.HttpContext.Current.Server.MapPath("~/Reports/" + AstrreportName);
                //string strRptName_WithPath_WithoutExt = path + AstrreportName;

                repDoc.Load(strRptName_WithPath_WithoutExt + ".rpt");//End of M002             
                repDoc.Refresh();
                crDatabase = repDoc.Database;
                crTables = crDatabase.Tables;




                foreach (CrystalDecisions.CrystalReports.Engine.Table crTable in crTables)
                {
                    string ServerName = ConfigurationManager.AppSettings["Server"].ToString();
                    crConnInfo.ServerName = ServerName.Replace("[", "");
                    crConnInfo.DatabaseName = ConfigurationManager.AppSettings["Database"].ToString();
                    crConnInfo.UserID = ConfigurationManager.AppSettings["UserId"].ToString();//"GEM";
                    crConnInfo.Password = CC_Ingenio.Common.Utils.Utils.Helper.DecryptPassword(ConfigurationManager.AppSettings["Password"].ToString());



                    crLogOnInfo = crTable.LogOnInfo;
                    crLogOnInfo.ConnectionInfo = crConnInfo;
                    crTable.ApplyLogOnInfo(crLogOnInfo);
                    crTable.Location = crTable.Name;
                }

                crSections = repDoc.ReportDefinition.Sections;
                //  return strRptName_WithPath_WithoutExt;

                string[] strParamFlds;
                string[] strParamVals;

                strParamFlds = AstrParameterFields.Split(',');
                strParamVals = AstrParameterValues.Split(',');

                string AFileNameFirstPart = "";

                if (AstrParameterFields.Length != 0)
                {
                    for (int i = 0; i < strParamFlds.Length; i++)
                    {
                        ParameterDiscreteValue dParam = new ParameterDiscreteValue();
                        ParameterValues currentValues = new ParameterValues();
                        repDoc.DataDefinition.ParameterFields[strParamFlds[i]].DefaultValues.Clear();
                        dParam.Value = strParamVals[i];
                        currentValues = repDoc.DataDefinition.ParameterFields[strParamFlds[i]].CurrentValues;
                        currentValues.Add(dParam);
                        repDoc.DataDefinition.ParameterFields[strParamFlds[i]].ApplyCurrentValues(currentValues);
                        AFileNameFirstPart = strParamVals[0] + "_";
                    }
                }

                //AFileNameFirstPart = AFileNameFirstPart.Remove(AFileNameFirstPart.Length - 1);

                string str_date = DateTime.Now.ToString("MMddyyyhhmmss").ToString();


                string strReportNameForPrint = AFileNameFirstPart.ToString() +  ALoginName + ".pdf";
                  // + "_" + String.Format("{0:dd-MM-yyyy}", DateTime.Now) + ".pdf";//gg
                string exportFilePath = path + '\\' + strReportNameForPrint;

              /*   Stream stream = repDoc.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                    stream.Seek(0, SeekOrigin.Begin);
                  string str =    File(stream,"application/pdf", "Sammple.pdf");*/

                DiskFileDestinationOptions crDiskFileDestinationOptions = new DiskFileDestinationOptions();
                crDiskFileDestinationOptions.DiskFileName = exportFilePath;
                ExportOptions crExportOptions = repDoc.ExportOptions;
                crExportOptions.DestinationOptions = crDiskFileDestinationOptions;
                crExportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
                crExportOptions.ExportFormatType = ExportFormatType.PortableDocFormat  ;

                repDoc.Export();
                repDoc.Close();

                //return repDoc ;
                return strReportNameForPrint;
            //}
            //catch (Exception ex)
            //{
            //    throw ex;
            //}
        }

        public  string  PrintReportWithParamNewWindow(string AstrreportName, string AstrParameterFields, string AstrParameterValues, HttpResponse AResponse, HttpServerUtility Aserver, string ALoginName, string AstrServerName, string AstrdatabaseName)
        {
            try
            {
                // Business.Common FCommon = new Business.Common();
                //string strRptName_WithPath_WithoutExt = Aserver.MapPath("~/Reports/" + AstrreportName);
                //return strRptName_WithPath_WithoutExt;
                ReportDocument repDoc = new ReportDocument();
                ReportDocument subRepDoc = new ReportDocument();
                Sections crSections;
                //ReportObjects crReportObjects;
                //SubreportObject crSubreportObject;
                CrystalDecisions.CrystalReports.Engine.Database crDatabase;
                CrystalDecisions.CrystalReports.Engine.Tables crTables;
                TableLogOnInfo crLogOnInfo;
                ConnectionInfo crConnInfo = new ConnectionInfo();


                string strRptName_WithPath_WithoutExt = Aserver.MapPath("~/Reports/" + AstrreportName);
              
                repDoc.Load(strRptName_WithPath_WithoutExt + ".rpt");//End of M002             
                repDoc.Refresh();
                crDatabase = repDoc.Database;
                crTables = crDatabase.Tables;


               
                
                foreach (CrystalDecisions.CrystalReports.Engine.Table crTable in crTables)
                {
                    string ServerName = ConfigurationManager.AppSettings["Server"].ToString();
                    crConnInfo.ServerName = ServerName.Replace("[", "");
                    crConnInfo.DatabaseName = ConfigurationManager.AppSettings["DATABASE"].ToString();
                    crConnInfo.UserID = ConfigurationManager.AppSettings["USERNAME"].ToString();//"GEM";
                    crConnInfo.Password = ConfigurationManager.AppSettings["PWD"].ToString();//"GEM";

                 

                   
                    crLogOnInfo = crTable.LogOnInfo;
                    crLogOnInfo.ConnectionInfo = crConnInfo;
                    crTable.ApplyLogOnInfo(crLogOnInfo);
                    crTable.Location = crTable.Name;
                }

                crSections = repDoc.ReportDefinition.Sections;
              //  return strRptName_WithPath_WithoutExt;

                string[] strParamFlds;
                string[] strParamVals;

                strParamFlds = AstrParameterFields.Split(',');
                strParamVals = AstrParameterValues.Split(',');

                if (AstrParameterFields.Length != 0)
                {
                    for (int i = 0; i < strParamFlds.Length; i++)
                    {
                        ParameterDiscreteValue dParam = new ParameterDiscreteValue();
                        ParameterValues currentValues = new ParameterValues();
                        repDoc.DataDefinition.ParameterFields[strParamFlds[i]].DefaultValues.Clear();
                        dParam.Value = strParamVals[i];
                        currentValues = repDoc.DataDefinition.ParameterFields[strParamFlds[i]].CurrentValues;
                        currentValues.Add(dParam);
                        repDoc.DataDefinition.ParameterFields[strParamFlds[i]].ApplyCurrentValues(currentValues);
                    }
                }

                string str_date = DateTime.Now.ToString("MMddyyyhhmmss").ToString();
                string strReportNameForPrint = AstrreportName
                   + "_" + HttpContext.Current.Session["LOGGED_IN_USER"] + "_" + String.Format("{0:dd-MM-yyyy-hh-mm-ss}", DateTime.Now) + ".pdf";//gg
                string exportFilePath = Aserver.MapPath("../../PDFS").ToString() + '/' + strReportNameForPrint;



                DiskFileDestinationOptions crDiskFileDestinationOptions = new DiskFileDestinationOptions();
                crDiskFileDestinationOptions.DiskFileName = exportFilePath;
                ExportOptions crExportOptions = repDoc.ExportOptions;
                crExportOptions.DestinationOptions = crDiskFileDestinationOptions;
                crExportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
                crExportOptions.ExportFormatType = ExportFormatType.PortableDocFormat;

                repDoc.Export();
                repDoc.Close();
                

                return strReportNameForPrint;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }


        //                crConnInfo.DatabaseName.Tables(1).connectionProperties("data source") = "the server name"
        //report.database.tables(1).connectionProperties(initial catalog") = "the database"
        //report.database.tables(1).connectionProperties("user ID") = "the user ID"
        //report.database.tables(1).connectionProperties("password") = "the password" 
        private static void AddImageRow(DataTable AdtTbl, string AHeaderFile, string ALogoFile)
        {
            FileStream fs;
            BinaryReader br;
            DataRow row = AdtTbl.NewRow();
            row[0] = DBNull.Value;
            row[1] = DBNull.Value;


            if (File.Exists(AHeaderFile))
            {
                fs = new FileStream(AHeaderFile, FileMode.Open);	// create a file stream
                br = new BinaryReader(fs);			// create binary reader
                row[0] = br.ReadBytes((int)br.BaseStream.Length);
                fs.Close();
            }

            if (File.Exists(ALogoFile))
            {
                fs = new FileStream(ALogoFile, FileMode.Open);	// create a file stream
                br = new BinaryReader(fs); // create binary reader
                row[1] = br.ReadBytes((int)br.BaseStream.Length);
                fs.Close();
            }

            // add this row to the table
            AdtTbl.Rows.Add(row);

            // clean up
            br = null;
            fs = null;
        }


        

    }

}
