/*All rights reserved.  No part of this program or publication may be reproduced,
transcribed, stored in a retrieval system, or translated into any language or computer
language, or in any form or by any means, electronic, mechanical, magnetic, optical,
chemical, biological or otherwise, without the prior written permission of :


      GEMINI SOFTWARE SOLUTIONS PRIVATE LTD.,
      NILA, TECHNOPARK CAMPUS, TRIVANDRUM, INDIA.

      - - - - - - - - - - - - - - - - - - - - -
      File Name : MasterController.cs
      - - - - - - - - - - - - - - - - - - - - -

      System				:  	CC_Ingenio
      Module				:  	Controller Logic - Currency
      Author				:	Amal    
      Date					:	03.03.2014
      Function				:	Controller methods for adding/editing/searching and deleting in currency master
      Modification History	:   
     

      Label			Date		Name		Change description
      - - - - -	   - - - - 		- - - - - 	- - - - - - - - - - - - - - - - 
 * 
 * 
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using CC_Ingenio.Manager.Contracts;
using CC_Ingenio.DataTranfer.ContactInfo;
using CC_Ingenio.Common.Logging;
using CC_Ingenio.Common.Utils;
using CC_Ingenio.DataTranfer.MasterInfo;
using CC_Ingenio.DataTranfer.LookUpInfo;

using CC_Ingenio.Web.ViewModel.Master;
using System.Web.Script.Serialization;
using CC_Ingenio.Web.Filters;
using CC_Ingenio.DataTranfer.UserInfo;
using System.Diagnostics;
using System.IO;
using CC_Ingenio_Resources;
using System.Globalization;

namespace CC_Ingenio.Web.Controllers
{
    
    public class MasterController : Controller
    {

        #region Variables
        private readonly IMasterManager tMasterManager;
        private readonly ILookUpManager tLookUpManager;
        private CC_Ingenio.Manager.CommonManager tCommonManager;
        #endregion

        #region Constructor
        public MasterController(IMasterManager AMasterManager, ILookUpManager ALookUpManager)
        {
            tMasterManager = AMasterManager;
            tLookUpManager = ALookUpManager;
            tCommonManager = new CC_Ingenio.Manager.CommonManager(System.Web.HttpContext.Current);
        }
        #endregion

        #region Controller methods

        #region Index
        public ActionResult Index()
        {
            try
            {
                var currencies = tMasterManager.FindCurrency(1, Utils.GRD_ROWS_PER_PAGE).AsQueryable();

                return View(currencies);
            }
            catch (Exception ex)
            {

                LoggerFactory.CreateLog().LogError(ex.ToString(), 100, 2);
                return View();
            }

        }
        #endregion

        #region Currency

        #region CurrencyList
        [AuthorizeUserAttribute(Entity = "MT_CURRENCY", AccessLevel = new UserActivities[] { UserActivities.View })]
        public ActionResult CurrencyList(CurrencySearch AVMCurrencySearch, string ClearButton, int? page)
        {
            try
            {

                int iPageNumber = Convert.ToInt32((page == null) ? 0 : page);

                if (ClearButton == Resources.Clear)
                {
                    AVMCurrencySearch.CurrencyCode = null;
                    AVMCurrencySearch.CurrencyName = null;
                    ModelState.Clear();
                }

                if ((Utils.OrganizationCode !="")|| (!(string.IsNullOrEmpty(AVMCurrencySearch.SearchButton)) &&
                     !((string.IsNullOrWhiteSpace(AVMCurrencySearch.CurrencyCode) && 
                     (string.IsNullOrWhiteSpace(AVMCurrencySearch.CurrencyName))))))
                {
                    var entities = tMasterManager.FindAll(iPageNumber, new CurrencyDTO()
                    {
                        CurrencyCode = AVMCurrencySearch.CurrencyCode,
                        CurrencyName = AVMCurrencySearch.CurrencyName,
                        CountryCode = AVMCurrencySearch.CountryCode
                    });
                    AVMCurrencySearch.SearchResults = entities.AsQueryable();
                }
                else
                {
                    var currResult = tMasterManager.FindCurrency(1, Utils.GRD_ROWS_PER_PAGE).AsQueryable();

                    AVMCurrencySearch.SearchResults = currResult.AsQueryable();
                }

                AVMCurrencySearch.SearchResults = AVMCurrencySearch.SearchResults.Where(x => x.ApprovalStatus != "DEL" && x.ApprovalStatus != "DIS");
                ViewBag.Privileges = tCommonManager.GetUserPrivilegesForEntity("MT_CURRENCY");
                return View(AVMCurrencySearch);
            }
            catch (Exception ex)
            {
                LoggerFactory.CreateLog().LogError("Error" + ex.Message, 100, 2);
                ViewBag.ErrorMessage = Resources.MsgErrServerException;
                ViewBag.ErrorType = SessionErrorType.ServerException;
                return View("Error");
            }
        }
        #endregion
        #region CurrencyAdd
        [AuthorizeUserAttribute(Entity = "MT_CURRENCY", AccessLevel = new UserActivities[] { UserActivities.Add, UserActivities.Update, UserActivities.View })]
        public ActionResult CurrencyAdd(string ACurrencyCode ,string AOrganizationCode)
        {
            try
            {
                CurrencyDTO currencyDTO;
                ViewBag.TableName = "CUR";
                var countryCode = tLookUpManager.GetAll<CountryDTO>("", "Country");

                if (String.IsNullOrWhiteSpace(ACurrencyCode))
                {
                    currencyDTO = new CurrencyDTO();
                    currencyDTO.OrganizationCode = Utils.OrganizationCode;
                }
                else
                    currencyDTO = tMasterManager.FindByCurrencyCode(ACurrencyCode, AOrganizationCode);
                if (!String.IsNullOrWhiteSpace(currencyDTO.ApprovalStatus) && currencyDTO.ApprovalStatus.Equals("P"))
                {
                    var reasonCodes = tLookUpManager.GetAll<ApprovalContextDTO>("CUR", "ReasonContext");
                    ViewBag.ReasonCodes = reasonCodes;

                }

                ViewBag.Privileges = tCommonManager.GetUserPrivilegesForEntity("MT_CURRENCY");
                ViewData["FormStatus"] = currencyDTO.ApprovalStatus;
                ViewBag.Countries = countryCode;
                return View(currencyDTO);
            }
            catch(Exception ex) 
            {
                LoggerFactory.CreateLog().LogError("Error" + ex.Message, 100, 2);
                ViewBag.ErrorMessage = Resources.MsgErrServerException;
                ViewBag.ErrorType = SessionErrorType.ServerException;
                return View("Error");
            }
        }
        #endregion
        #region CurrencyView
        [AuthorizeUserAttribute(Entity = "MT_CURRENCY", AccessLevel = new UserActivities[] { UserActivities.View })]
        public ActionResult CurrencyView(string ACurrencyCode, string AOrganizationCode)
        {
            try
            {
                CurrencyDTO currencyDTO;
                ViewBag.TableName = "CUR";
                var countryCode = tLookUpManager.GetAll<CountryDTO>("", "Country");

                ViewBag.Privileges = tCommonManager.GetUserPrivilegesForEntity("MT_CURRENCY");
                ViewBag.Countries = countryCode;

                if (String.IsNullOrWhiteSpace(ACurrencyCode))
                {
                    currencyDTO = new CurrencyDTO();
                    currencyDTO.OrganizationCode = Utils.OrganizationCode;
                    ViewData["FormStatus"] = currencyDTO.ApprovalStatus;
                    return View("CurrencyAdd", currencyDTO);
                }
                else
                {
                    currencyDTO = tMasterManager.FindByCurrencyCode(ACurrencyCode, AOrganizationCode);

                    ViewBag.ReasonCodes = null;
                }

                ViewData["FormStatus"] = currencyDTO.ApprovalStatus;

                return View("CurrencyAdd", currencyDTO);
            }
            catch (Exception ex)
            {
                LoggerFactory.CreateLog().LogError("Error" + ex.Message, 100, 2);
                ViewBag.ErrorMessage = Resources.MsgErrServerException;
                ViewBag.ErrorType = SessionErrorType.ServerException;
                return View("Error");
            }
            
        }
        #endregion
        #region CurrencySave
        [HttpPost]
        [AuthorizeUserAttribute(Entity = "MT_CURRENCY", AccessLevel = new UserActivities[] { UserActivities.Add, UserActivities.Update })]
        public ActionResult CurrencySave(string ACurrencyCode, string AOrganizationCode, string Command, CurrencyDTO ACurrencyDTO)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (Command.Equals(Resources.Submit))
                        ACurrencyDTO.ApprovalStatus = "P";
                    else if (Command.Equals(Resources.Save))
                        ACurrencyDTO.ApprovalStatus = "D";

                    if (Command.Equals(Resources.Submit) || Command.Equals(Resources.Save))
                    {
                        if (string.IsNullOrWhiteSpace(tMasterManager.FindByCurrencyCode(ACurrencyDTO.CurrencyCode,AOrganizationCode  ).CurrencyCode))
                        {
                            ACurrencyDTO.CreatedBy = tCommonManager.GetLoggedUserName();
                            ACurrencyDTO.LastUpdatedBy = tCommonManager.GetLoggedUserName();
                            ACurrencyDTO.OrganizationCode = Utils.OrganizationCode;
                            tMasterManager.SaveCurrencyDetails(ACurrencyDTO);
                            TempData["Message"] = String.Format(Resources.MsgInfoSaveSuccess, String.Concat(new object[] { Resources.Currency, " ", ACurrencyDTO.CurrencyCode })); 
                        }
                        else
                        {
                            if (!string.IsNullOrWhiteSpace(ACurrencyCode))
                            {
                                ACurrencyDTO.LastUpdatedBy = tCommonManager.GetLoggedUserName();
                                tMasterManager.UpdateCurrencyDetails(ACurrencyCode,AOrganizationCode , ACurrencyDTO);
                                TempData["Message"] = String.Format(Resources.MsgInfoSaveSuccess, String.Concat(new object[] { Resources.Currency, " ", ACurrencyDTO.CurrencyCode })); 
                            }
                            else
                            {
                                TempData["Message"] = String.Format(Resources.MsgErrAlreadyExists, String.Concat(new object[] { Resources.Currency, " ", ACurrencyDTO.CurrencyCode })); 
                            }
                        }
                    }
                    else if (Command.Equals(Resources.Delete))
                    {
                        try
                        {
                            ACurrencyDTO.LastUpdatedBy = tCommonManager.GetLoggedUserName();
                            ACurrencyDTO.ApprovalStatus = Utils.GetApprovalStatusCode(Utils.ApprovalStatus.Deleted);
                            tMasterManager.UpdateCurrencyDetails(ACurrencyCode ,AOrganizationCode , ACurrencyDTO);
                            TempData["Message"] = String.Format(Resources.MsgInfoDeleteSuccess, String.Concat(new object[] { Resources.Currency, " ", ACurrencyDTO.CurrencyCode })); 
                        }
                        catch (Exception ex)
                        {
                            LoggerFactory.CreateLog().LogError("Error" + ex.Message, 100, 2);
                            TempData["Message"] = String.Format(Resources.MsgErrDeleteConcurrencyError, String.Concat(new object[] { Resources.Currency, " ", ACurrencyDTO.CurrencyCode })); 
                        }
                    }
                }
            }

            catch (Exception ex)
            {
                LoggerFactory.CreateLog().LogError("Error" + ex.Message, 100, 2);
                ViewBag.ErrorMessage = Resources.MsgErrServerException;
                ViewBag.ErrorType = SessionErrorType.ServerException;
                return View("Error");
            }

            return ((Command.Equals(Resources.Submit) || Command.Equals(Resources.Delete)) ? RedirectToAction("CurrencyAdd", "master") : RedirectToAction("CurrencyAdd", "master", new { ACurrencyCode = ACurrencyCode ,AOrganizationCode = AOrganizationCode  }));

        }
        #endregion
        #region CurrencyDelete
        [HttpPost]
        [AuthorizeUserAttribute(Entity = "MT_CURRENCY", AccessLevel = new UserActivities[] { UserActivities.Delete })]
        public JsonResult CurrencyDelete(string ACurrencyCode, string AOrganizationCode)
        {

            CurrencyDTO ACurrencyDTO = new CurrencyDTO();
            try
            {
                ACurrencyDTO = tMasterManager.FindByCurrencyCode(ACurrencyCode,AOrganizationCode );
                ACurrencyDTO.LastUpdatedBy = tCommonManager.GetLoggedUserName();
                ACurrencyDTO.ApprovalStatus = Utils.GetApprovalStatusCode(Utils.ApprovalStatus.Deleted);
                tMasterManager.UpdateCurrencyDetails(ACurrencyCode,AOrganizationCode  , ACurrencyDTO);

                var dResult = new { result = "success", action = "CurrencyList", controller = "Master" };
                TempData["Message"] = String.Format(Resources.MsgInfoDeleteSuccess, String.Concat(new object[] { Resources.Currency, " ", ACurrencyDTO.CurrencyCode })); 
                return Json(dResult, JsonRequestBehavior.AllowGet);

            }
            catch
            {
                var dResultFail = new { result = "fail", action = "CurrencyList", controller = "Master" };
                TempData["Message"] = String.Format(Resources.MsgErrDeleteConcurrencyError, String.Concat(new object[] { Resources.Currency, " ", ACurrencyDTO.CurrencyCode })); 
                return Json(dResultFail, JsonRequestBehavior.AllowGet);

            }

        }
        #endregion
        #region CurrencyAuthorize
        [AuthorizeUserAttribute(Entity = "MT_CURRENCY", AccessLevel = new UserActivities[] { UserActivities.Authorize })]
        public ActionResult CurrencyAuthorize(string ACurrencyCode, string AOrganizationCode, string AMode, string QId)
        {
            var countryCode = tLookUpManager.GetAll<CountryDTO>("", "Country");

            ViewBag.RequestQueueId = QId;
            ViewBag.TableName = "CUR";
            if (!String.IsNullOrWhiteSpace(ACurrencyCode))
            {
                CurrencyDTO currencyDTO = tMasterManager.FindByCurrencyCode(ACurrencyCode,AOrganizationCode );
                ViewBag.Countries = countryCode;
                if (!String.IsNullOrWhiteSpace(currencyDTO.ApprovalStatus) && currencyDTO.ApprovalStatus.Equals("P"))
                {
                    var reasonCodes = tLookUpManager.GetAll<ApprovalContextDTO>("CUR", "ReasonContext");
                    ViewBag.ReasonCodes = reasonCodes;
                }
                ViewBag.Privileges = tCommonManager.GetUserPrivilegesForEntity("MT_CURRENCY");
                ViewData["FormStatus"] = currencyDTO.ApprovalStatus;
                ViewData["FormCall"] = "ApprovalQueue";
                return View("CurrencyAdd", currencyDTO);

            }
            else
                return RedirectToAction("ApprovalQueueList", "Approval");
        }
        #endregion

        #endregion

        #region Organization

        #region OrganizationView
        [AuthorizeUserAttribute(Entity = "MT_ORGANIZATION", AccessLevel = new UserActivities[] { UserActivities.View })]
        public ActionResult OrganizationView(string Id)
        {
            try
            {
                OrganizationDTO organizationDTO;
                ViewBag.TableName = "ORGSTN";

                ViewBag.Privileges = tCommonManager.GetUserPrivilegesForEntity("MT_ORGANIZATION");

                if (String.IsNullOrWhiteSpace(Id))
                {
                    organizationDTO = new OrganizationDTO();
                    organizationDTO.EffectiveFromDate = Utils.BusinessDate;
                    organizationDTO.EffectiveToDate = Utils.BusinessDate;
                    ViewData["FormStatus"] = organizationDTO.ApprovalStatus;
                    return View("OrganizationAdd", organizationDTO);
                }
                else
                {
                    organizationDTO = tMasterManager.FindByOrganizationCode(Convert.ToString(Id));
                    ViewBag.ReasonCodes = null;
                }

                ViewData["FormStatus"] = organizationDTO.ApprovalStatus;

                return View("OrganizationAdd", organizationDTO);
            }
            catch (Exception ex)
            {
                LoggerFactory.CreateLog().LogError("Error" + ex.Message, 100, 2);
                ViewBag.ErrorMessage = Resources.MsgErrServerException;
                ViewBag.ErrorType = SessionErrorType.ServerException;
                return View("Error");
            }	
        }
        #endregion
        #region OrganizationList
        [AuthorizeUserAttribute(Entity = "MT_ORGANIZATION", AccessLevel = new UserActivities[] { UserActivities.View })]
        public ActionResult OrganizationList(OrganizationSearch AVMOrganizationSearch, string ClearButton, int? page)
        {
            try
            {

                int iPageNumber = Convert.ToInt32((page == null) ? 0 : page);

                if (ClearButton == Resources.Clear)
                {
                    AVMOrganizationSearch.OrganizationCode = null;
                    AVMOrganizationSearch.OrganizationDescription = null;
                    ModelState.Clear();
                }

                if (!(string.IsNullOrEmpty(AVMOrganizationSearch.SearchButton)) &&
                 !((string.IsNullOrWhiteSpace(AVMOrganizationSearch.OrganizationCode) &&
                   (string.IsNullOrWhiteSpace(AVMOrganizationSearch.OrganizationDescription)) &&
                            (AVMOrganizationSearch.EffectiveFromDate == DateTime.MinValue) &&
                            (AVMOrganizationSearch.EffectiveToDate == DateTime.MinValue)
                            )))
                {
                    var entities = tMasterManager.FindAll(iPageNumber, new OrganizationDTO()
                    {
                        OrganizationCode = AVMOrganizationSearch.OrganizationCode,
                        OrganizationDescription = AVMOrganizationSearch.OrganizationDescription,
                        EffectiveFromDate = AVMOrganizationSearch.EffectiveFromDate,
                        EffectiveToDate = (AVMOrganizationSearch.EffectiveToDate == DateTime.MinValue) ? DateTime.MaxValue : AVMOrganizationSearch.EffectiveToDate
                    });
                    AVMOrganizationSearch.SearchResults = entities.AsQueryable();
                    ViewData["SearchMode"] = "Search";
                }
                else
                {
                    var currResult = tMasterManager.FindOrganization(1, Utils.GRD_ROWS_PER_PAGE).AsQueryable();
                    AVMOrganizationSearch.SearchResults = currResult.AsQueryable();
                    ViewData["SearchMode"] = "Default";
                }

                if (AVMOrganizationSearch.EffectiveFromDate == DateTime.MinValue && AVMOrganizationSearch.EffectiveToDate == DateTime.MinValue)
                {
                    ViewData["SearchMode"] = "Default";
                }

                AVMOrganizationSearch.SearchResults = AVMOrganizationSearch.SearchResults.Where(x => x.ApprovalStatus != "DEL" && x.ApprovalStatus != "DIS");
                ViewBag.Privileges = tCommonManager.GetUserPrivilegesForEntity("MT_ORGANIZATION");
                return View(AVMOrganizationSearch);
            }
            catch (Exception ex)
            {
                LoggerFactory.CreateLog().LogError("Error" + ex.Message, 100, 2);
                ViewBag.ErrorMessage = Resources.MsgErrServerException;
                ViewBag.ErrorType = SessionErrorType.ServerException;
                return View("Error");
            }
        }
        #endregion
        #region OrganizationAdd
        [AuthorizeUserAttribute(Entity = "MT_ORGANIZATION", AccessLevel = new UserActivities[] { UserActivities.Add, UserActivities.Update, UserActivities.View })]
        public ActionResult OrganizationAdd(string Id)
        {
            try
            {
                OrganizationDTO organizationDTO;

                if (Id == null)
                {
                    organizationDTO = new OrganizationDTO();
                    organizationDTO.EffectiveFromDate = Utils.BusinessDate;
                    organizationDTO.EffectiveToDate = Utils.BusinessDate;
                }
                else
                {
                    organizationDTO = tMasterManager.FindByOrganizationCode(Convert.ToString(Id));
                    if (organizationDTO.ApprovalStatus.Equals("DEL") || organizationDTO.ApprovalStatus.Equals("DIS"))
                    {
                        organizationDTO = new OrganizationDTO();
                        organizationDTO.EffectiveFromDate = Utils.BusinessDate;
                        organizationDTO.EffectiveToDate = Utils.BusinessDate;
                    }
                }

                if (!String.IsNullOrWhiteSpace(organizationDTO.ApprovalStatus) && organizationDTO.ApprovalStatus.Equals("P"))
                {
                    var reasonCodes = tLookUpManager.GetAll<ApprovalContextDTO>("ORGSTN", "ReasonContext");
                    ViewBag.ReasonCodes = reasonCodes;
                }

                ViewBag.Privileges = tCommonManager.GetUserPrivilegesForEntity("MT_ORGANIZATION");

                ViewData["FormStatus"] = organizationDTO.ApprovalStatus;
                return View(organizationDTO);
            }
            catch (Exception ex)
            {
                LoggerFactory.CreateLog().LogError("Error" + ex.Message, 100, 2);
                ViewBag.ErrorMessage = Resources.MsgErrServerException;
                ViewBag.ErrorType = SessionErrorType.ServerException;
                return View("Error");
            }
            
        }
        #endregion
        #region OrganizationSave
        [HttpPost]
        [AuthorizeUserAttribute(Entity = "MT_ORGANIZATION", AccessLevel = new UserActivities[] { UserActivities.Add, UserActivities.Update })]
        public ActionResult OrganizationSave(string Id, string Command, OrganizationDTO AOrganizationDTO, HttpPostedFileBase file)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    if (Command.Equals(Resources.Submit))
                        AOrganizationDTO.ApprovalStatus = "P";
                    else if (Command.Equals(Resources.Save))
                        AOrganizationDTO.ApprovalStatus = "D";
                                            
                    if (Command.Equals(Resources.Submit) || Command.Equals(Resources.Save))
                    {
                        if (file != null)
                        {
                            string filename = System.IO.Path.GetFileName(file.FileName) ?? "";

                            if (filename != "")
                            {
                                filename = AOrganizationDTO.OrganizationCode + "-" + filename;
                                string directory = Server.MapPath("~/Reports/OrganizationLogo/");
                                if (!Directory.Exists(directory))
                                {
                                    Directory.CreateDirectory(directory);
                                }
                                string path = Path.Combine(directory, filename);

                                if (System.IO.File.Exists(path))
                                {
                                    System.IO.File.Delete(path);
                                }

                                file.SaveAs(path);

                                AOrganizationDTO.OrganizationLogo = @path;
                            }
                        }
                        else
                        {
                            AOrganizationDTO.OrganizationLogo = "";
                        }

                        if (string.IsNullOrWhiteSpace(tMasterManager.FindByOrganizationCode(AOrganizationDTO.OrganizationCode).OrganizationCode))
                        {
                            AOrganizationDTO.CreatedBy = tCommonManager.GetLoggedUserName();
                            AOrganizationDTO.LastUpdatedBy = tCommonManager.GetLoggedUserName();
                            tMasterManager.SaveOrganizationDetails(AOrganizationDTO);
                            TempData["Message"] = String.Format(Resources.MsgInfoSaveSuccess, String.Concat(new object[] { Resources.Organization, " ", AOrganizationDTO.OrganizationCode })); 
                        }
                        else
                        {
                            if (!string.IsNullOrWhiteSpace(Id))
                            {
                                AOrganizationDTO.LastUpdatedBy = tCommonManager.GetLoggedUserName();
                                tMasterManager.UpdateOrganizationDetails(Id, AOrganizationDTO);
                                TempData["Message"] = String.Format(Resources.MsgInfoSaveSuccess, String.Concat(new object[] { Resources.Organization, " ", AOrganizationDTO.OrganizationCode })); 
                            }
                            else
                            {
                                TempData["Message"] = String.Format(Resources.MsgErrAlreadyExists, String.Concat(new object[] { Resources.Organization, " ", AOrganizationDTO.OrganizationCode })); 
                            }
                        }
                    }
                    else if (Command.Equals(Resources.Delete))
                    {
                        try
                        {
                            AOrganizationDTO.LastUpdatedBy = tCommonManager.GetLoggedUserName();
                            AOrganizationDTO.ApprovalStatus = Utils.GetApprovalStatusCode(Utils.ApprovalStatus.Deleted);
                            tMasterManager.UpdateOrganizationDetails(Id, AOrganizationDTO);
                            TempData["Message"] = String.Format(Resources.MsgInfoDeleteSuccess, String.Concat(new object[] { Resources.Organization, " ", AOrganizationDTO.OrganizationCode })); 
                        }
                        catch (Exception ex)
                        {
                            LoggerFactory.CreateLog().LogError("Error" + ex.Message, 100, 2);
                            TempData["Message"] = String.Format(Resources.MsgErrDeleteConcurrencyError, String.Concat(new object[] { Resources.Organization, " ", AOrganizationDTO.OrganizationCode })); 
                            return RedirectToAction("OrganizationAdd", "master", new { Id = Id });
                        }
                    }
                }
            }

            catch (Exception ex)
            {
                LoggerFactory.CreateLog().LogError("Error" + ex.Message, 100, 2);
                ViewBag.ErrorMessage = Resources.MsgErrServerException;
                ViewBag.ErrorType = SessionErrorType.ServerException;
                return View("Error");
            }

            return ((Command.Equals(Resources.Submit) || Command.Equals(Resources.Delete)) ? RedirectToAction("OrganizationAdd", "master") : RedirectToAction("OrganizationAdd", "master", new { Id = Id }));

        }
        #endregion
        #region OrganizationDelete
        [HttpPost]
        [AuthorizeUserAttribute(Entity = "MT_ORGANIZATION", AccessLevel = new UserActivities[] { UserActivities.Delete })]
        public JsonResult OrganizationDelete(string Id)
        {
            OrganizationDTO AOrganizationDTO = new OrganizationDTO();
            try
            {
                AOrganizationDTO = tMasterManager.FindByOrganizationCode(Id);
                AOrganizationDTO.LastUpdatedBy = tCommonManager.GetLoggedUserName();
                AOrganizationDTO.ApprovalStatus = Utils.GetApprovalStatusCode(Utils.ApprovalStatus.Deleted);
                tMasterManager.UpdateOrganizationDetails(Id, AOrganizationDTO);

                var dResult = new { result = "success", action = "OrganizationList", controller = "Master" };
                TempData["Message"] = String.Format(Resources.MsgInfoDeleteSuccess, String.Concat(new object[] { Resources.Organization, " ", AOrganizationDTO.OrganizationCode })); 
                return Json(dResult, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                var dResultFail = new { result = "fail", action = "OrganizationList", controller = "Master" };
                TempData["Message"] = String.Format(Resources.MsgErrDeleteConcurrencyError, String.Concat(new object[] { Resources.Organization, " ", AOrganizationDTO.OrganizationCode })); 
                return Json(dResultFail, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion
        #region OrganizationAuthorize
        [AuthorizeUserAttribute(Entity = "MT_ORGANIZATION", AccessLevel = new UserActivities[] { UserActivities.Authorize })]
        public ActionResult OrganizationAuthorize(string Id, string AMode, string QId)
        {
            ViewBag.RequestQueueId = QId;
            ViewBag.TableName = "ORGSTN";
            if (!String.IsNullOrWhiteSpace(Id))
            {
                OrganizationDTO organizationDTO = tMasterManager.FindByOrganizationCode(Id);

                if (!String.IsNullOrWhiteSpace(organizationDTO.ApprovalStatus) && organizationDTO.ApprovalStatus.Equals("P"))
                {
                    var reasonCodes = tLookUpManager.GetAll<ApprovalContextDTO>("ORGSTN", "ReasonContext");
                    ViewBag.ReasonCodes = reasonCodes;
                }

                ViewBag.Privileges = tCommonManager.GetUserPrivilegesForEntity("MT_ORGANIZATION");
                ViewData["FormCall"] = "ApprovalQueue";
                ViewData["FormStatus"] = organizationDTO.ApprovalStatus;
                return View("OrganizationAdd", organizationDTO);
            }
            else
                return RedirectToAction("ApprovalQueueList", "Approval");
        }
        #endregion
        #region ViewLogo
        public ActionResult ViewLogo(string AFileName, string AType)
        {
            string fileName = AFileName;
            EmptyResult _singleton = new EmptyResult();

            if (!string.IsNullOrEmpty(fileName) && System.IO.File.Exists(fileName))
            {
                FileStream stream = new FileStream(fileName, FileMode.Open, FileAccess.Read);
                FileStreamResult result = new FileStreamResult(stream, "image/png");
                return result;
            }
            else
            {
                return _singleton;
            }
        }
        #endregion

        #endregion

        #region Franchise

        #region FranchiseView
        [AuthorizeUserAttribute(Entity = "MT_FRANCHISE", AccessLevel = new UserActivities[] { UserActivities.View })]
        public ActionResult FranchiseView(string AFranchiseCode, string AOrganizationCode)
        {
            try
            {
                FranchiseDTO franchiseDTO;
                ViewBag.TableName = "FRNCHSE";

                var organizationCode = tLookUpManager.GetAll<OrganizationDTO>("", "Organization");

                ViewBag.Organizations = organizationCode;
                ViewBag.Privileges = tCommonManager.GetUserPrivilegesForEntity("MT_FRANCHISE");

                if (String.IsNullOrWhiteSpace(AFranchiseCode))
                {
                    franchiseDTO = new FranchiseDTO();
                    franchiseDTO.OrganizationCode = Utils.OrganizationCode;
                    franchiseDTO.EffectiveFromDate = Utils.BusinessDate;
                    franchiseDTO.EffectiveToDate = Utils.BusinessDate;
                    ViewData["FormStatus"] = franchiseDTO.ApprovalStatus;
                    return View("FranchiseAdd", franchiseDTO);
                }
                else
                {
                    franchiseDTO = tMasterManager.FindByFranchiseCode(Convert.ToString(AFranchiseCode));
                    ViewBag.ReasonCodes = null;
                }

                ViewData["FormStatus"] = franchiseDTO.ApprovalStatus;

                return View("FranchiseAdd", franchiseDTO);
            }
            catch (Exception ex)
            {
                LoggerFactory.CreateLog().LogError("Error" + ex.Message, 100, 2);
                ViewBag.ErrorMessage = Resources.MsgErrServerException;
                ViewBag.ErrorType = SessionErrorType.ServerException;
                return View("Error");
            }
        }
        #endregion
        #region FranchiseList
        [AuthorizeUserAttribute(Entity = "MT_FRANCHISE", AccessLevel = new UserActivities[] { UserActivities.View })]
        public ActionResult FranchiseList(FranchiseSearch AVMFranchiseSearch, string ClearButton, int? page)
        {
            try
            {

                int iPageNumber = Convert.ToInt32((page == null) ? 0 : page);

                if (ClearButton == Resources.Clear)
                {
                    AVMFranchiseSearch.FranchiseCode = null;
                    AVMFranchiseSearch.FranchiseName = null;
                    ModelState.Clear();
                }

                if ((Utils.OrganizationCode != "") || (!(string.IsNullOrEmpty(AVMFranchiseSearch.SearchButton)) &&
                     !((string.IsNullOrWhiteSpace(AVMFranchiseSearch.FranchiseCode) && (string.IsNullOrWhiteSpace(AVMFranchiseSearch.FranchiseName))))))
                {
                    var entities = tMasterManager.FindAll(iPageNumber, new FranchiseDTO()
                    {
                        FranchiseCode = AVMFranchiseSearch.FranchiseCode,
                        FranchiseName = AVMFranchiseSearch.FranchiseName
                    });
                    AVMFranchiseSearch.SearchResults = entities.AsQueryable();
                }
                else
                {
                    var currResult = tMasterManager.FindFranchise(1, Utils.GRD_ROWS_PER_PAGE).AsQueryable();

                    AVMFranchiseSearch.SearchResults = currResult.AsQueryable();
                }
                ViewBag.Privileges = tCommonManager.GetUserPrivilegesForEntity("MT_FRANCHISE");
                return View(AVMFranchiseSearch);
            }
            catch (Exception ex)
            {
                LoggerFactory.CreateLog().LogError("Error" + ex.Message, 100, 2);
                ViewBag.ErrorMessage = Resources.MsgErrServerException;
                ViewBag.ErrorType = SessionErrorType.ServerException;
                return View("Error");
            }
        }
        #endregion
        #region FranchiseAdd
        [AuthorizeUserAttribute(Entity = "MT_FRANCHISE", AccessLevel = new UserActivities[] { UserActivities.Add, UserActivities.Update, UserActivities.View })]
        public ActionResult FranchiseAdd(string AFranchiseCode, string AOrganizationCode)
        {
            try
            {
                FranchiseDTO franchiseDTO;

                var organizationCode = tLookUpManager.GetAll<OrganizationDTO>("", "Organization");

                if (AFranchiseCode == null)
                {
                    franchiseDTO = new FranchiseDTO();
                    franchiseDTO.OrganizationCode = Utils.OrganizationCode;
                    franchiseDTO.EffectiveFromDate = Utils.BusinessDate;
                    franchiseDTO.EffectiveToDate = Utils.BusinessDate;
                }
                else
                    franchiseDTO = tMasterManager.FindByFranchiseCode(Convert.ToString(AFranchiseCode));

                if (!String.IsNullOrWhiteSpace(franchiseDTO.ApprovalStatus) && franchiseDTO.ApprovalStatus.Equals("P"))
                {
                    var reasonCodes = tLookUpManager.GetAll<ApprovalContextDTO>("FRNCHSE", "ReasonContext");
                    ViewBag.ReasonCodes = reasonCodes;
                }

                ViewBag.Privileges = tCommonManager.GetUserPrivilegesForEntity("MT_FRANCHISE");

                ViewData["FormStatus"] = franchiseDTO.ApprovalStatus;
                ViewBag.Organizations = organizationCode;

                return View(franchiseDTO);
            }
            catch (Exception ex)
            {
                LoggerFactory.CreateLog().LogError("Error" + ex.Message, 100, 2);
                ViewBag.ErrorMessage = Resources.MsgErrServerException;
                ViewBag.ErrorType = SessionErrorType.ServerException;
                return View("Error");
            }
        }
        #endregion
        #region FranchiseSave
        [HttpPost]
        [AuthorizeUserAttribute(Entity = "MT_FRANCHISE", AccessLevel = new UserActivities[] { UserActivities.Add, UserActivities.Update })]
        public ActionResult FranchiseSave(string AFranchiseCode, string AOrganizationCode, string Command, FranchiseDTO AFranchiseDTO)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    if (Command.Equals(Resources.Submit))
                        AFranchiseDTO.ApprovalStatus = "P";
                    else if (Command.Equals(Resources.Save))
                        AFranchiseDTO.ApprovalStatus = "D";

                    if (Command.Equals(Resources.Submit) || Command.Equals(Resources.Save))
                    {
                        if (string.IsNullOrWhiteSpace(tMasterManager.FindByFranchiseCode(AFranchiseDTO.FranchiseCode).FranchiseCode))
                        {
                            AFranchiseDTO.CreatedBy = tCommonManager.GetLoggedUserName();
                            AFranchiseDTO.LastUpdatedBy = tCommonManager.GetLoggedUserName();
                            tMasterManager.SaveFranchiseDetails(AFranchiseDTO);
                            TempData["Message"] = String.Format(Resources.MsgInfoSaveSuccess, String.Concat(new object[] { Resources.Franchise, " ", AFranchiseDTO.FranchiseCode })); 
                        }
                        else
                        {
                            if (!string.IsNullOrWhiteSpace(AFranchiseCode))
                            {
                                AFranchiseDTO.LastUpdatedBy = tCommonManager.GetLoggedUserName();
                                tMasterManager.UpdateFranchiseDetails(AFranchiseCode, AFranchiseDTO);
                                TempData["Message"] = String.Format(Resources.MsgInfoSaveSuccess, String.Concat(new object[] { Resources.Franchise, " ", AFranchiseDTO.FranchiseCode })); 
                            }
                            else
                            {
                                TempData["Message"] = String.Format(Resources.MsgErrAlreadyExists, String.Concat(new object[] { Resources.Franchise, " ", AFranchiseDTO.FranchiseCode })); 
                            }
                        }
                    }
                    else if (Command.Equals(Resources.Delete))
                    {
                        try
                        {
                            AFranchiseDTO.LastUpdatedBy = tCommonManager.GetLoggedUserName();
                            AFranchiseDTO.ApprovalStatus = Utils.GetApprovalStatusCode(Utils.ApprovalStatus.Deleted);
                            tMasterManager.UpdateFranchiseDetails(AFranchiseCode, AFranchiseDTO);
                            TempData["Message"] = String.Format(Resources.MsgInfoDeleteSuccess, String.Concat(new object[] { Resources.Franchise, " ", AFranchiseDTO.FranchiseCode })); 
                        }
                        catch (Exception ex)
                        {
                            LoggerFactory.CreateLog().LogError("Error" + ex.Message, 100, 2);
                            TempData["Message"] = String.Format(Resources.MsgErrDeleteConcurrencyError, String.Concat(new object[] { Resources.Franchise, " ", AFranchiseDTO.FranchiseCode })); 
                            return RedirectToAction("FranchiseAdd", "master", new { AFranchiseCode = AFranchiseCode, AOrganizationCode = AOrganizationCode });
                        }
                    }
                }

            }

            catch (Exception ex)
            {
                LoggerFactory.CreateLog().LogError("Error" + ex.Message, 100, 2);
                ViewBag.ErrorMessage = Resources.MsgErrServerException;
                ViewBag.ErrorType = SessionErrorType.ServerException;
                return View("Error");
            }

            return ((Command.Equals(Resources.Submit) || Command.Equals(Resources.Delete)) ? RedirectToAction("FranchiseAdd", "master") : RedirectToAction("FranchiseAdd", "master", new { AFranchiseCode = AFranchiseCode, AOrganizationCode = AOrganizationCode }));

        }
        #endregion
        #region FranchiseDelete
        [HttpPost]
        [AuthorizeUserAttribute(Entity = "MT_FRANCHISE", AccessLevel = new UserActivities[] { UserActivities.Delete })]
        public JsonResult FranchiseDelete(string Id)
        {
            FranchiseDTO AFranchiseDTO = new FranchiseDTO();
            try
            {
                AFranchiseDTO = tMasterManager.FindByFranchiseCode(Id);
                AFranchiseDTO.LastUpdatedBy = tCommonManager.GetLoggedUserName();
                AFranchiseDTO.ApprovalStatus = Utils.GetApprovalStatusCode(Utils.ApprovalStatus.Deleted);
                tMasterManager.UpdateFranchiseDetails(Id, AFranchiseDTO);

                var dResult = new { result = "success", action = "FranchiseList", controller = "Master" };
                TempData["Message"] = String.Format(Resources.MsgInfoDeleteSuccess, String.Concat(new object[] { Resources.Franchise, " ", AFranchiseDTO.FranchiseCode })); 
                return Json(dResult, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                var dResultFail = new { result = "fail", action = "FranchiseList", controller = "Master" };
                TempData["Message"] = String.Format(Resources.MsgErrDeleteConcurrencyError, String.Concat(new object[] { Resources.Franchise, " ", AFranchiseDTO.FranchiseCode })); 
                return Json(dResultFail, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion
        #region FranchiseAuthorize
        [AuthorizeUserAttribute(Entity = "MT_FRANCHISE", AccessLevel = new UserActivities[] { UserActivities.Authorize })]
        public ActionResult FranchiseAuthorize(string Id, string AMode, string QId)
        {
            ViewBag.RequestQueueId = QId;
            ViewBag.TableName = "FRNCHSE";
            if (!String.IsNullOrWhiteSpace(Id))
            {
                FranchiseDTO franchiseDTO = tMasterManager.FindByFranchiseCode(Id);
                var organizationCode = tLookUpManager.GetAll<OrganizationDTO>("", "Organization");
                if (!String.IsNullOrWhiteSpace(franchiseDTO.ApprovalStatus) && franchiseDTO.ApprovalStatus.Equals("P"))
                {
                    var reasonCodes = tLookUpManager.GetAll<ApprovalContextDTO>("FRNCHSE", "ReasonContext");
                    ViewBag.ReasonCodes = reasonCodes;
                }

                ViewBag.Privileges = tCommonManager.GetUserPrivilegesForEntity("MT_FRANCHISE");
                ViewData["FormCall"] = "ApprovalQueue";
                ViewData["FormStatus"] = franchiseDTO.ApprovalStatus;
                ViewBag.Organizations = organizationCode;
                return View("FranchiseAdd", franchiseDTO);
            }
            else
                return RedirectToAction("ApprovalQueueList", "Approval");
        }
        #endregion

        #endregion

        #region Sourcing
        #region SourcingView
        [AuthorizeUserAttribute(Entity = "MT_SOURCING", AccessLevel = new UserActivities[] { UserActivities.View })]
        public ActionResult SourcingView(string ASourcingChannelCode, string AOrganizationCode)
        {
            try
            {
                SourcingDTO sourcingDTO;
                ViewBag.TableName = "SRC";

                ViewBag.Privileges = tCommonManager.GetUserPrivilegesForEntity("MT_SOURCING");

                if (String.IsNullOrWhiteSpace(ASourcingChannelCode))
                {
                    sourcingDTO = new SourcingDTO();
                    sourcingDTO.OrganizationCode = AOrganizationCode;
                    ViewData["FormStatus"] = sourcingDTO.ApprovalStatus;
                    return View("SourcingAdd", sourcingDTO);
                }
                else
                {
                    sourcingDTO = tMasterManager.FindBySourcingChannelCode(ASourcingChannelCode, AOrganizationCode);
                    ViewBag.ReasonCodes = null;
                }
                ViewData["FormStatus"] = sourcingDTO.ApprovalStatus;

                return View("SourcingAdd", sourcingDTO);
            }
            catch (Exception ex)
            {
                LoggerFactory.CreateLog().LogError("Error" + ex.Message, 100, 2);
                ViewBag.ErrorMessage = Resources.MsgErrServerException;
                ViewBag.ErrorType = SessionErrorType.ServerException;
                return View("Error");
            }
        }
        #endregion
        #region SourcingAdd
        [AuthorizeUserAttribute(Entity = "MT_SOURCING", AccessLevel = new UserActivities[] { UserActivities.Add, UserActivities.Update, UserActivities.View })]
        public ActionResult SourcingAdd(string ASourcingChannelCode, string AOrganizationCode)
        {
            try
            {
                SourcingDTO sourcingDTO;

                if (ASourcingChannelCode == null)
                {
                    sourcingDTO = new SourcingDTO();
                    sourcingDTO.OrganizationCode = Utils.OrganizationCode;
                }
                else
                {
                    sourcingDTO = tMasterManager.FindBySourcingChannelCode(ASourcingChannelCode, AOrganizationCode);
                }
                if (!String.IsNullOrWhiteSpace(sourcingDTO.ApprovalStatus) && sourcingDTO.ApprovalStatus.Equals("P"))
                {
                    var reasonCodes = tLookUpManager.GetAll<ApprovalContextDTO>("SRCING", "ReasonContext");
                    ViewBag.ReasonCodes = reasonCodes;

                }

                ViewBag.Privileges = tCommonManager.GetUserPrivilegesForEntity("MT_SOURCING");

                ViewData["FormStatus"] = sourcingDTO.ApprovalStatus;
                return View(sourcingDTO);
            }
            catch (Exception ex)
            {
                LoggerFactory.CreateLog().LogError("Error" + ex.Message, 100, 2);
                ViewBag.ErrorMessage = Resources.MsgErrServerException;
                ViewBag.ErrorType = SessionErrorType.ServerException;
                return View("Error");
            }
        }
        #endregion
        #region SourcingSave
        [HttpPost]
        [AuthorizeUserAttribute(Entity = "MT_SOURCING", AccessLevel = new UserActivities[] { UserActivities.Add, UserActivities.Update })]
        public ActionResult SourcingSave(string ASourcingChannelCode, string AOrganizationCode, string Command, SourcingDTO ASourcingDTO)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (Command.Equals(Resources.Submit))
                        ASourcingDTO.ApprovalStatus = "P";
                    else if (Command.Equals(Resources.Save))
                        ASourcingDTO.ApprovalStatus = "D";

                    if (Command.Equals(Resources.Submit) || Command.Equals(Resources.Save))
                    {
                        if (string.IsNullOrWhiteSpace(tMasterManager.FindBySourcingChannelCode(ASourcingDTO.SourcingChannelCode, AOrganizationCode).SourcingChannelCode))
                        {
                            ASourcingDTO.CreatedBy = tCommonManager.GetLoggedUserName();
                            ASourcingDTO.LastUpdatedBy = tCommonManager.GetLoggedUserName();
                            ASourcingDTO.OrganizationCode = Utils.OrganizationCode;
                            tMasterManager.SaveSourcingDetails(ASourcingDTO);
                            TempData["Message"] = String.Format(Resources.MsgInfoSaveSuccess, String.Concat(new object[] { Resources.SourcingChannelCode, " ", ASourcingDTO.SourcingChannelCode })); 
                        }
                        else
                        {
                            if (!string.IsNullOrWhiteSpace(ASourcingChannelCode))
                            {
                                ASourcingDTO.LastUpdatedBy = tCommonManager.GetLoggedUserName();
                                tMasterManager.UpdateSourcingDetails(ASourcingChannelCode, AOrganizationCode, ASourcingDTO);
                                TempData["Message"] = String.Format(Resources.MsgInfoSaveSuccess, String.Concat(new object[] { Resources.SourcingChannelCode, " ", ASourcingDTO.SourcingChannelCode })); 
                            }
                            else
                            {
                                TempData["Message"] = String.Format(Resources.MsgErrAlreadyExists, String.Concat(new object[] { Resources.SourcingChannelCode, " ", ASourcingDTO.SourcingChannelCode })); 
                            }
                        }
                    }
                    else if (Command.Equals(Resources.Delete))
                    {
                        try
                        {
                            ASourcingDTO.LastUpdatedBy = tCommonManager.GetLoggedUserName();
                            ASourcingDTO.ApprovalStatus = Utils.GetApprovalStatusCode(Utils.ApprovalStatus.Deleted);
                            tMasterManager.UpdateSourcingDetails(ASourcingChannelCode, AOrganizationCode, ASourcingDTO);

                            TempData["Message"] = String.Format(Resources.MsgInfoDeleteSuccess, String.Concat(new object[] { Resources.SourcingChannelCode, " ", ASourcingDTO.SourcingChannelCode })); 
                        }
                        catch (Exception ex)
                        {
                            LoggerFactory.CreateLog().LogError("Error" + ex.Message, 100, 2);
                            TempData["Message"] = String.Format(Resources.MsgErrDeleteConcurrencyError, String.Concat(new object[] { Resources.SourcingChannelCode, " ", ASourcingDTO.SourcingChannelCode })); 
                            return RedirectToAction("SourcingAdd", "master", new { ASourcingChannelCode = ASourcingChannelCode, AOrganizationCode = AOrganizationCode });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LoggerFactory.CreateLog().LogError("Error" + ex.Message, 100, 2);
                ViewBag.ErrorMessage = Resources.MsgErrServerException;
                ViewBag.ErrorType = SessionErrorType.ServerException;
                return View("Error");
            }

            return ((Command.Equals(Resources.Submit) || Command.Equals(Resources.Delete)) ? RedirectToAction("SourcingAdd", "Master") : RedirectToAction("SourcingAdd", "Master", new { ASourcingChannelCode = ASourcingChannelCode, AOrganizationCode = AOrganizationCode }));

        }
        #endregion
        #region SourcingDelete
        [HttpPost]
        [AuthorizeUserAttribute(Entity = "MT_SOURCING", AccessLevel = new UserActivities[] { UserActivities.Delete })]
        public JsonResult SourcingDelete(string ASourcingChannelCode, string AOrganizationCode)
        {
            SourcingDTO ASourcingDTO = new SourcingDTO();
            try
            {
                ASourcingDTO = tMasterManager.FindBySourcingChannelCode(ASourcingChannelCode, AOrganizationCode);
                ASourcingDTO.LastUpdatedBy = tCommonManager.GetLoggedUserName();
                ASourcingDTO.ApprovalStatus = Utils.GetApprovalStatusCode(Utils.ApprovalStatus.Deleted);
                tMasterManager.UpdateSourcingDetails(ASourcingChannelCode, AOrganizationCode, ASourcingDTO);

                var dResult = new { result = "success", action = "SourcingList", controller = "Master" };
                TempData["Message"] = String.Format(Resources.MsgInfoDeleteSuccess, String.Concat(new object[] { Resources.SourcingChannelCode, " ", ASourcingDTO.SourcingChannelCode })); 
                return Json(dResult, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                var dResultFail = new { result = "fail", action = "SourcingList", controller = "Master" };
                TempData["Message"] = String.Format(Resources.MsgErrDeleteConcurrencyError, String.Concat(new object[] { Resources.SourcingChannelCode, " ", ASourcingDTO.SourcingChannelCode })); 
                return Json(dResultFail, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion
        #region SourcingList
        [AuthorizeUserAttribute(Entity = "MT_SOURCING", AccessLevel = new UserActivities[] { UserActivities.View })]
        public ActionResult SourcingList(SourcingSearch AVMSourcingSearch, string ClearButton, int? page)
        {
            try
            {
                int iPageNumber = Convert.ToInt32((page == null) ? 0 : page);

                if (ClearButton == Resources.Clear)
                {
                    AVMSourcingSearch.SourcingChannelCode = null;
                    AVMSourcingSearch.SourcingChannelName = null;
                    ModelState.Clear();
                }

                if ((Utils.OrganizationCode != "") ||
                (!(string.IsNullOrEmpty(AVMSourcingSearch.SearchButton)) &&
                     !((string.IsNullOrWhiteSpace(AVMSourcingSearch.SourcingChannelCode)
                     && (string.IsNullOrWhiteSpace(AVMSourcingSearch.SourcingChannelName))))))
                {
                    var entities = tMasterManager.FindAll(iPageNumber, new SourcingDTO()
                    {
                        SourcingChannelCode = AVMSourcingSearch.SourcingChannelCode,
                        SourcingChannelName = AVMSourcingSearch.SourcingChannelName
                    });
                    AVMSourcingSearch.SearchResults = entities.AsQueryable();
                }
                else
                {
                    var currResult = tMasterManager.FindSourcing(1, Utils.GRD_ROWS_PER_PAGE).AsQueryable();

                    AVMSourcingSearch.SearchResults = currResult.AsQueryable();
                }

                AVMSourcingSearch.SearchResults = AVMSourcingSearch.SearchResults.Where(x => x.ApprovalStatus != "DEL" && x.ApprovalStatus != "DIS");
                ViewBag.Privileges = tCommonManager.GetUserPrivilegesForEntity("MT_SOURCING");
                return View(AVMSourcingSearch);
            }
            catch (Exception ex)
            {
                LoggerFactory.CreateLog().LogError("Error" + ex.Message, 100, 2);
                ViewBag.ErrorMessage = Resources.MsgErrServerException;
                ViewBag.ErrorType = SessionErrorType.ServerException;
                return View("Error");
            }
        }
        #endregion
        #region SourcingAuthorize
        [AuthorizeUserAttribute(Entity = "MT_SOURCING", AccessLevel = new UserActivities[] { UserActivities.Authorize })]
        public ActionResult SourcingAuthorize(string ASourcingChannelCode, string AOrganizationCode, string AMode, string QId)
        {
            ViewBag.RequestQueueId = QId;
            ViewBag.TableName = "SRC";
            if (!String.IsNullOrWhiteSpace(ASourcingChannelCode))
            {
                SourcingDTO sourcingDTO = tMasterManager.FindBySourcingChannelCode(ASourcingChannelCode, AOrganizationCode);

                if (!String.IsNullOrWhiteSpace(sourcingDTO.ApprovalStatus) && sourcingDTO.ApprovalStatus.Equals("P"))
                {
                    var reasonCodes = tLookUpManager.GetAll<ApprovalContextDTO>("SRC", "ReasonContext");
                    ViewBag.ReasonCodes = reasonCodes;
                }

                ViewBag.Privileges = tCommonManager.GetUserPrivilegesForEntity("MT_SOURCING");
                ViewData["FormCall"] = "ApprovalQueue";
                ViewData["FormStatus"] = sourcingDTO.ApprovalStatus;
                return View("SourcingAdd", sourcingDTO);
            }
            else
                return RedirectToAction("ApprovalQueueList", "Approval");
        }
        #endregion
        #endregion

        #region Company
        #region CompanyView
        [AuthorizeUserAttribute(Entity = "MT_COMPANY", AccessLevel = new UserActivities[] { UserActivities.View })]
        public ActionResult CompanyView(string ACompanyCode, string AOrganizationCode)
        {
            try
            {
                CompanyDTO companyDTO;
                ViewBag.TableName = "COMP";

                var companyCategory = tLookUpManager.GetAll<CompanyCategoryDTO>(string.Empty, "CompanyCategory");
                var productCategoryList = tLookUpManager.GetAll<ProductCategoryDTO>(string.Empty, "ProductCategory");

                ViewBag.CompanyCategory = companyCategory;
                ViewBag.ProductCategoryList = productCategoryList;

                ViewBag.Privileges = tCommonManager.GetUserPrivilegesForEntity("MT_COMPANY");

                if (String.IsNullOrWhiteSpace(ACompanyCode))
                {
                    companyDTO = new CompanyDTO();
                    companyDTO.EffectiveFromDate = Utils.BusinessDate;
                    companyDTO.EffectiveToDate = Utils.BusinessDate;
                    companyDTO.OrganizationCode = Utils.OrganizationCode;
                    ViewData["FormStatus"] = companyDTO.ApprovalStatus;
                    return View("CompanyAdd", companyDTO);
                }
                else
                {
                    companyDTO = tMasterManager.FindByCompanyCode(ACompanyCode, AOrganizationCode);
                    ViewBag.ReasonCodes = null;
                }

                ViewData["FormStatus"] = companyDTO.ApprovalStatus;

                return View("CompanyAdd", companyDTO);
            }
            catch (Exception ex)
            {
                LoggerFactory.CreateLog().LogError("Error" + ex.Message, 100, 2);
                ViewBag.ErrorMessage = Resources.MsgErrServerException;
                ViewBag.ErrorType = SessionErrorType.ServerException;
                return View("Error");
            }
        }
        #endregion
        #region CompanyAdd

        [AuthorizeUserAttribute(Entity = "MT_COMPANY", AccessLevel = new UserActivities[] { UserActivities.Add, UserActivities.Update, UserActivities.View })]
        public ActionResult CompanyAdd(string ACompanyCode, string AOrganizationCode)
        {
            try
            {
                CompanyDTO companyDTO;

                var companyCategory = tLookUpManager.GetAll<CompanyCategoryDTO>(string.Empty, "CompanyCategory");
                var productCategoryList = tLookUpManager.GetAll<ProductCategoryDTO>(string.Empty, "ProductCategory");
                if (ACompanyCode == null)
                {
                    companyDTO = new CompanyDTO();
                    companyDTO.OrganizationCode = Utils.OrganizationCode;
                    companyDTO.EffectiveFromDate = Utils.BusinessDate;
                    companyDTO.EffectiveToDate = Utils.BusinessDate;
                }
                else
                {
                    companyDTO = tMasterManager.FindByCompanyCode(ACompanyCode, AOrganizationCode);
                }
                if (!String.IsNullOrWhiteSpace(companyDTO.ApprovalStatus) && companyDTO.ApprovalStatus.Equals("P"))
                {
                    var reasonCodes = tLookUpManager.GetAll<ApprovalContextDTO>("COMP", "ReasonContext");
                    ViewBag.ReasonCodes = reasonCodes;

                }


                ViewBag.Privileges = tCommonManager.GetUserPrivilegesForEntity("MT_COMPANY");

                ViewData["FormStatus"] = companyDTO.ApprovalStatus;
                ViewBag.CompanyCategory = companyCategory;
                ViewBag.ProductCategoryList = productCategoryList;
                return View(companyDTO);
            }
            catch (Exception ex)
            {
                LoggerFactory.CreateLog().LogError("Error" + ex.Message, 100, 2);
                ViewBag.ErrorMessage = Resources.MsgErrServerException;
                ViewBag.ErrorType = SessionErrorType.ServerException;
                return View("Error");
            }
        }
        #endregion
        #region CompanySave
        [HttpPost]
        [AuthorizeUserAttribute(Entity = "MT_COMPANY", AccessLevel = new UserActivities[] { UserActivities.Add, UserActivities.Update })]
        public ActionResult CompanySave(string ACompanyCode, string AOrganizationCode, string Command, CompanyDTO ACompanyDTO)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (Command.Equals(Resources.Submit))
                        ACompanyDTO.ApprovalStatus = "P";
                    else if (Command.Equals(Resources.Save))
                        ACompanyDTO.ApprovalStatus = "D";

                    if (Command.Equals(Resources.Submit) || Command.Equals(Resources.Save))
                    {
                        if (string.IsNullOrWhiteSpace(tMasterManager.FindByCompanyCode(ACompanyDTO.CompanyCode, AOrganizationCode).CompanyCode))
                        {
                            ACompanyDTO.CreatedBy = tCommonManager.GetLoggedUserName();
                            ACompanyDTO.LastUpdatedBy = tCommonManager.GetLoggedUserName();
                            ACompanyDTO.OrganizationCode = Utils.OrganizationCode;
                            tMasterManager.SaveCompanyDetails(ACompanyDTO);
                            TempData["Message"] = String.Format(Resources.MsgInfoSaveSuccess, String.Concat(new object[] { Resources.Corporate, " ", ACompanyDTO.CompanyCode })); 
                        }
                        else
                        {
                            if (!string.IsNullOrWhiteSpace(ACompanyCode))
                            {
                                ACompanyDTO.LastUpdatedBy = tCommonManager.GetLoggedUserName();
                                tMasterManager.UpdateCompanyDetails(ACompanyCode, AOrganizationCode, ACompanyDTO);
                                TempData["Message"] = String.Format(Resources.MsgInfoSaveSuccess, String.Concat(new object[] { Resources.Corporate, " ", ACompanyDTO.CompanyCode })); 
                            }
                            else
                            {
                                TempData["Message"] = String.Format(Resources.MsgErrAlreadyExists, String.Concat(new object[] { Resources.Corporate, " ", ACompanyDTO.CompanyCode })); 
                            }
                        }
                    }
                    else if (Command.Equals(Resources.Delete))
                    {
                        try
                        {

                            ACompanyDTO.LastUpdatedBy = tCommonManager.GetLoggedUserName();
                            ACompanyDTO.ApprovalStatus = Utils.GetApprovalStatusCode(Utils.ApprovalStatus.Deleted);
                            tMasterManager.UpdateCompanyDetails(ACompanyCode, AOrganizationCode, ACompanyDTO);

                            TempData["Message"] = String.Format(Resources.MsgInfoDeleteSuccess, String.Concat(new object[] { Resources.Corporate, " ", ACompanyDTO.CompanyCode })); 
                        }
                        catch (Exception ex)
                        {
                            LoggerFactory.CreateLog().LogError("Error" + ex.Message, 100, 2);
                            TempData["Message"] = String.Format(Resources.MsgErrDeleteConcurrencyError, String.Concat(new object[] { Resources.Corporate, " ", ACompanyDTO.CompanyCode })); 
                            return RedirectToAction("CompanyAdd", "master", new { ACompanyCode = ACompanyCode, AOrganizationCode = AOrganizationCode });
                        }
                    }
                }
            }

            catch (Exception ex)
            {
                LoggerFactory.CreateLog().LogError("Error" + ex.Message, 100, 2);
                ViewBag.ErrorMessage = Resources.MsgErrServerException;
                ViewBag.ErrorType = SessionErrorType.ServerException;
                return View("Error");
            }

            return ((Command.Equals(Resources.Submit) || (Command.Equals(Resources.Delete)) ? RedirectToAction("CompanyAdd", "Master") : RedirectToAction("CompanyAdd", "Master", new { ACompanyCode = ACompanyCode, AOrganizationCode = AOrganizationCode })));
        }
        #endregion
        #region CompanyDelete

        [HttpPost]
        [AuthorizeUserAttribute(Entity = "MT_COMPANY", AccessLevel = new UserActivities[] { UserActivities.Delete })]
        public JsonResult CompanyDelete(string ACompanyCode, string AOrganizationCode)
        {
            CompanyDTO ACompanyDTO = new CompanyDTO();
            try
            {

                ACompanyDTO = tMasterManager.FindByCompanyCode(ACompanyCode, AOrganizationCode);
                ACompanyDTO.LastUpdatedBy = tCommonManager.GetLoggedUserName();
                ACompanyDTO.ApprovalStatus = Utils.GetApprovalStatusCode(Utils.ApprovalStatus.Deleted);
                tMasterManager.UpdateCompanyDetails(ACompanyCode, AOrganizationCode, ACompanyDTO);

                var dResult = new { result = "success", action = "CompanyList", controller = "Master" };
                TempData["Message"] = String.Format(Resources.MsgInfoDeleteSuccess, String.Concat(new object[] { Resources.Corporate, " ", ACompanyDTO.CompanyCode })); 
                return Json(dResult, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                var dResultFail = new { result = "fail", action = "CompanyList", controller = "Master" };
                TempData["Message"] = String.Format(Resources.MsgErrDeleteConcurrencyError, String.Concat(new object[] { Resources.Corporate, " ", ACompanyDTO.CompanyCode })); 
                return Json(dResultFail, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion
        #region CompanyList
        [AuthorizeUserAttribute(Entity = "MT_COMPANY", AccessLevel = new UserActivities[] { UserActivities.View })]
        public ActionResult CompanyList(CompanySearch AVMCompanySearch, string ClearButton, int? page)
        {
            try
            {
                int iPageNumber = Convert.ToInt32((page == null) ? 0 : page);
                var companyCategoryList = tLookUpManager.GetAll<CompanyCategoryDTO>(string.Empty, "CompanyCategory");
                var productCategoryList = tLookUpManager.GetAll<ProductCategoryDTO>(string.Empty, "ProductCategory");


                if (ClearButton == Resources.Clear)
                {
                    AVMCompanySearch.CompanyCode = null;
                    AVMCompanySearch.CompanyName = null;
                    AVMCompanySearch.CompanyCategoryCode = null;
                    AVMCompanySearch.ProductCategoryCode = null;
                    ModelState.Clear();
                }
                if ((Utils.OrganizationCode != "") ||
              (!(string.IsNullOrEmpty(AVMCompanySearch.SearchButton)) &&
                  !((string.IsNullOrWhiteSpace(AVMCompanySearch.CompanyCode) &&
                    (string.IsNullOrWhiteSpace(AVMCompanySearch.CompanyName)) &&
                    (string.IsNullOrWhiteSpace(AVMCompanySearch.CompanyCategoryCode)) &&
                    (string.IsNullOrWhiteSpace(AVMCompanySearch.ProductCategoryCode))))))
                {
                    var entities = tMasterManager.FindAll(iPageNumber, new CompanyDTO()
                    {
                        CompanyCode = AVMCompanySearch.CompanyCode,
                        CompanyName = AVMCompanySearch.CompanyName,
                        CompanyCategoryCode = AVMCompanySearch.CompanyCategoryCode,
                        ProductCategoryCode = AVMCompanySearch.ProductCategoryCode
                    });
                    AVMCompanySearch.SearchResults = entities.AsQueryable();
                }
                else
                {
                    var currResult = tMasterManager.FindCompany(1, Utils.GRD_ROWS_PER_PAGE).AsQueryable();

                    AVMCompanySearch.SearchResults = currResult.AsQueryable();
                }
                ViewBag.CompanyCategoryList = companyCategoryList;
                ViewBag.ProductCategoryList = productCategoryList;

                AVMCompanySearch.SearchResults = AVMCompanySearch.SearchResults.Where(x => x.ApprovalStatus != "DEL" && x.ApprovalStatus != "DIS");
                ViewBag.Privileges = tCommonManager.GetUserPrivilegesForEntity("MT_COMPANY");
                return View(AVMCompanySearch);
            }
            catch (Exception ex)
            {
                LoggerFactory.CreateLog().LogError("Error" + ex.Message, 100, 2);
                ViewBag.ErrorMessage = Resources.MsgErrServerException;
                ViewBag.ErrorType = SessionErrorType.ServerException;
                return View("Error");
            }
        }
        #endregion
        #region CompanyAuthorize

        [AuthorizeUserAttribute(Entity = "MT_COMPANY", AccessLevel = new UserActivities[] { UserActivities.Authorize })]
        public ActionResult CompanyAuthorize(string ACompanyCode, string AOrganizationCode, string AMode, string QId)
        {
            ViewBag.RequestQueueId = QId;
            ViewBag.TableName = "COMP";
            var companyCategory = tLookUpManager.GetAll<CompanyCategoryDTO>("", "CompanyCategory");
            var productCategoryList = tLookUpManager.GetAll<ProductCategoryDTO>(string.Empty, "ProductCategory");
            if (!String.IsNullOrWhiteSpace(ACompanyCode))
            {
                CompanyDTO companyDTO = tMasterManager.FindByCompanyCode(ACompanyCode, AOrganizationCode);
                if (!String.IsNullOrWhiteSpace(companyDTO.ApprovalStatus) && companyDTO.ApprovalStatus.Equals("P"))
                {
                    var reasonCodes = tLookUpManager.GetAll<ApprovalContextDTO>("COMP", "ReasonContext");
                    ViewBag.ReasonCodes = reasonCodes;
                }

                ViewBag.Privileges = tCommonManager.GetUserPrivilegesForEntity("MT_COMPANY");
                ViewData["FormCall"] = "ApprovalQueue";
                ViewData["FormStatus"] = companyDTO.ApprovalStatus;
                ViewBag.CompanyCategory = companyCategory;
                ViewBag.ProductCategoryList = productCategoryList;
                return View("CompanyAdd", companyDTO);
            }
            else
                return RedirectToAction("ApprovalQueueList", "Approval");


        }
        #endregion
        #endregion

        #region City

        #region CityView
        [AuthorizeUserAttribute(Entity = "MT_CITY", AccessLevel = new UserActivities[] { UserActivities.View })]
        public ActionResult CityView(string ACityCode, string AOrganizationCode)
        {
            try
            {
                CityDTO cityDTO;
                ViewBag.TableName = "CITY";

                var countryCode = tLookUpManager.GetAll<CountryDTO>("", "Country");
                var cityCategoryCode = tLookUpManager.GetAll<CityCategoryDTO>("", "CityCategory");
                var regionCode = tLookUpManager.GetAll<RegionDTO>("", "Region");
                var hubCode = tLookUpManager.GetAll<HubDTO>("", "Hub");
                var spokeCode = tLookUpManager.GetAll<SpokeDTO>("", "Spoke");
                var communaCode = tLookUpManager.GetAll<CommunaDTO>("", "Communa");

                ViewBag.Countries = countryCode;
                ViewBag.CityCategories = cityCategoryCode;
                ViewBag.Privileges = tCommonManager.GetUserPrivilegesForEntity("MT_CITY");
                ViewBag.Regions = regionCode;
                ViewBag.Hubs = hubCode;
                ViewBag.Spokes = spokeCode;
                ViewBag.Communas = communaCode;

                if (String.IsNullOrWhiteSpace(ACityCode))
                {
                    cityDTO = new CityDTO();
                    cityDTO.OrganizationCode = Utils.OrganizationCode;
                    ViewData["FormStatus"] = cityDTO.ApprovalStatus;
                    return View("CityAdd", cityDTO);
                }
                else
                {
                    cityDTO = tMasterManager.FindByCityCode(Convert.ToString(ACityCode), AOrganizationCode);
                    ViewBag.ReasonCodes = null;
                }

                ViewData["FormStatus"] = cityDTO.ApprovalStatus;

                return View("CityAdd", cityDTO);
            }
            catch (Exception ex)
            {
                LoggerFactory.CreateLog().LogError("Error" + ex.Message, 100, 2);
                ViewBag.ErrorMessage = Resources.MsgErrServerException;
                ViewBag.ErrorType = SessionErrorType.ServerException;
                return View("Error");
            }
        }
        #endregion
        #region CityList
        [AuthorizeUserAttribute(Entity = "MT_CITY", AccessLevel = new UserActivities[] { UserActivities.View })]
        public ActionResult CityList(CitySearch AVMCitySearch, string ClearButton, int? page)
        {
            try
            {

                var countryCode = tLookUpManager.GetAll<CountryDTO>("", "Country");
                var cityCategoryCode = tLookUpManager.GetAll<CityCategoryDTO>("", "CityCategory");

                if (ClearButton == Resources.Clear)
                {
                    AVMCitySearch.CityCode = null;
                    AVMCitySearch.CityName = null;
                    AVMCitySearch.CityCategoryCode = null;
                    AVMCitySearch.CountryCode = null;

                    ModelState.Clear();
                }

                int iPageNumber = Convert.ToInt32((page == null) ? 0 : page);

                if ((Utils.OrganizationCode != "") || (!(string.IsNullOrEmpty(AVMCitySearch.SearchButton)) &&
                   !((string.IsNullOrWhiteSpace(AVMCitySearch.CityCode) &&
                   (string.IsNullOrWhiteSpace(AVMCitySearch.CityName)) &&
                   (string.IsNullOrWhiteSpace(AVMCitySearch.CityCategoryCode)) &&
                   (string.IsNullOrWhiteSpace(AVMCitySearch.CountryCode))
                   ))))
                {
                    var entities = tMasterManager.FindAll(iPageNumber, new CityDTO()
                    {
                        CityCode = AVMCitySearch.CityCode,
                        CityName = AVMCitySearch.CityName,
                        CityCategoryCode = AVMCitySearch.CityCategoryCode,
                        CountryCode = AVMCitySearch.CountryCode
                    });
                    AVMCitySearch.SearchResults = entities.AsQueryable();
                }
                else
                {
                    var currResult = tMasterManager.FindCity(1, Utils.GRD_ROWS_PER_PAGE).AsQueryable();
                    AVMCitySearch.SearchResults = currResult.AsQueryable();
                }

                ViewBag.Countries = countryCode;
                ViewBag.CityCategories = cityCategoryCode;

                AVMCitySearch.SearchResults = AVMCitySearch.SearchResults.Where(x => x.ApprovalStatus != "DEL" && x.ApprovalStatus != "DIS");
                ViewBag.Privileges = tCommonManager.GetUserPrivilegesForEntity("MT_CITY");
                return View(AVMCitySearch);
            }
            catch (Exception ex)
            {
                LoggerFactory.CreateLog().LogError("Error" + ex.Message, 100, 2);
                ViewBag.ErrorMessage = Resources.MsgErrServerException;
                ViewBag.ErrorType = SessionErrorType.ServerException;
                return View("Error");
            }
        }
        #endregion
        #region CityAdd
        [AuthorizeUserAttribute(Entity = "MT_CITY", AccessLevel = new UserActivities[] { UserActivities.Add, UserActivities.Update, UserActivities.View })]
        public ActionResult CityAdd(string ACityCode, string AOrganizationCode)
        {
            try
            {
                CityDTO cityDTO;

                var countryCode = tLookUpManager.GetAll<CountryDTO>("", "Country");
                var cityCategoryCode = tLookUpManager.GetAll<CityCategoryDTO>("", "CityCategory");
                var regionCode = tLookUpManager.GetAll<RegionDTO>("", "Region");
                var hubCode = tLookUpManager.GetAll<HubDTO>("", "Hub");
                var spokeCode = tLookUpManager.GetAll<SpokeDTO>("", "Spoke");
                var communaCode = tLookUpManager.GetAll<CommunaDTO>("", "Communa");

                if (ACityCode == null)
                {
                    cityDTO = new CityDTO();
                    cityDTO.OrganizationCode = Utils.OrganizationCode;
                }
                else
                    cityDTO = tMasterManager.FindByCityCode(Convert.ToString(ACityCode), AOrganizationCode);

                if (!String.IsNullOrWhiteSpace(cityDTO.ApprovalStatus) && cityDTO.ApprovalStatus.Equals("P"))
                {
                    var reasonCodes = tLookUpManager.GetAll<ApprovalContextDTO>("CITY", "ReasonContext");
                    ViewBag.ReasonCodes = reasonCodes;
                }

                ViewBag.Privileges = tCommonManager.GetUserPrivilegesForEntity("MT_CITY");

                ViewData["FormStatus"] = cityDTO.ApprovalStatus;
                ViewBag.Countries = countryCode;
                ViewBag.CityCategories = cityCategoryCode;
                ViewBag.Regions = regionCode;
                ViewBag.Hubs = hubCode;
                ViewBag.Spokes = spokeCode;
                ViewBag.Communas = communaCode;

                return View(cityDTO);
            }
            catch (Exception ex)
            {
                LoggerFactory.CreateLog().LogError("Error" + ex.Message, 100, 2);
                ViewBag.ErrorMessage = Resources.MsgErrServerException;
                ViewBag.ErrorType = SessionErrorType.ServerException;
                return View("Error");
            }
        }
        #endregion
        #region CitySave
        [HttpPost]
        [AuthorizeUserAttribute(Entity = "MT_CITY", AccessLevel = new UserActivities[] { UserActivities.Add, UserActivities.Update })]
        public ActionResult CitySave(string ACityCode, string AOrganizationCode, string Command, CityDTO ACityDTO)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    if (Command.Equals(Resources.Submit))
                        ACityDTO.ApprovalStatus = "P";
                    else if (Command.Equals(Resources.Save))
                        ACityDTO.ApprovalStatus = "D";

                    if (Command.Equals(Resources.Submit) || Command.Equals(Resources.Save))
                    {
                        if (string.IsNullOrWhiteSpace(tMasterManager.FindByCityCode(ACityDTO.CityCode, AOrganizationCode).CityCode))
                        {
                            ACityDTO.CreatedBy = tCommonManager.GetLoggedUserName();
                            ACityDTO.LastUpdatedBy = tCommonManager.GetLoggedUserName();
                            ACityDTO.OrganizationCode = Utils.OrganizationCode;
                            tMasterManager.SaveCityDetails(ACityDTO);
                            TempData["Message"] = String.Format(Resources.MsgInfoSaveSuccess, String.Concat(new object[] { Resources.City, " ", ACityDTO.CityCode })); 
                        }
                        else
                        {
                            if (!string.IsNullOrWhiteSpace(ACityCode))
                            {
                                ACityDTO.LastUpdatedBy = tCommonManager.GetLoggedUserName();
                                tMasterManager.UpdateCityDetails(ACityCode, AOrganizationCode, ACityDTO);
                                TempData["Message"] = String.Format(Resources.MsgInfoSaveSuccess, String.Concat(new object[] { Resources.City, " ", ACityDTO.CityCode })); 
                            }
                            else
                            {
                                TempData["Message"] = String.Format(Resources.MsgErrAlreadyExists, String.Concat(new object[] { Resources.City, " ", ACityDTO.CityCode })); 
                            }
                        }
                    }
                    else if (Command.Equals(Resources.Delete))
                    {
                        try
                        {
                            ACityDTO.LastUpdatedBy = tCommonManager.GetLoggedUserName();
                            ACityDTO.ApprovalStatus = Utils.GetApprovalStatusCode(Utils.ApprovalStatus.Deleted);
                            tMasterManager.UpdateCityDetails(ACityCode, AOrganizationCode, ACityDTO);
                            TempData["Message"] = String.Format(Resources.MsgInfoDeleteSuccess, String.Concat(new object[] { Resources.City, " ", ACityCode })); 
                        }
                        catch (Exception ex)
                        {
                            LoggerFactory.CreateLog().LogError("Error" + ex.Message, 100, 2);
                            TempData["Message"] = String.Format(Resources.MsgErrDeleteConcurrencyError, String.Concat(new object[] { Resources.City, " ", ACityCode })); 
                            return RedirectToAction("CityAdd", "master", new { ACityCode = ACityCode, AOrganizationCode = AOrganizationCode });
                        }
                    }
                }
            }

            catch (Exception ex)
            {
                LoggerFactory.CreateLog().LogError("Error" + ex.Message, 100, 2);
                ViewBag.ErrorMessage = Resources.MsgErrServerException;
                ViewBag.ErrorType = SessionErrorType.ServerException;
                return View("Error");
            }

            return ((Command.Equals(Resources.Submit) || Command.Equals(Resources.Delete)) ? RedirectToAction("CityAdd", "master") : RedirectToAction("CityAdd", "master", new { ACityCode = ACityCode, AOrganizationCode = AOrganizationCode }));

        }
        #endregion
        #region CityAuthorize
        [AuthorizeUserAttribute(Entity = "MT_CITY", AccessLevel = new UserActivities[] { UserActivities.Authorize })]
        public ActionResult CityAuthorize(string ACityCode, string AOrganizationCode, string AMode, string QId)
        {
            ViewBag.RequestQueueId = QId;
            ViewBag.TableName = "CITY";
            if (!String.IsNullOrWhiteSpace(ACityCode))
            {
                CityDTO cityDTO = tMasterManager.FindByCityCode(ACityCode, AOrganizationCode);

                var countryCode = tLookUpManager.GetAll<CountryDTO>("", "Country");
                var cityCategoryCode = tLookUpManager.GetAll<CityCategoryDTO>("", "CityCategory");
                var regionCode = tLookUpManager.GetAll<RegionDTO>("", "Region");
                var hubCode = tLookUpManager.GetAll<HubDTO>("", "Hub");
                var spokeCode = tLookUpManager.GetAll<SpokeDTO>("", "Spoke");
                var communaCode = tLookUpManager.GetAll<CommunaDTO>("", "Communa");

                if (!String.IsNullOrWhiteSpace(cityDTO.ApprovalStatus) && cityDTO.ApprovalStatus.Equals("P"))
                {
                    var reasonCodes = tLookUpManager.GetAll<ApprovalContextDTO>("CITY", "ReasonContext");
                    ViewBag.ReasonCodes = reasonCodes;
                }

                ViewBag.Privileges = tCommonManager.GetUserPrivilegesForEntity("MT_CITY");
                ViewData["FormCall"] = "ApprovalQueue";
                ViewData["FormStatus"] = cityDTO.ApprovalStatus;
                ViewBag.Countries = countryCode;
                ViewBag.CityCategories = cityCategoryCode;
                ViewBag.Regions = regionCode;
                ViewBag.Hubs = hubCode;
                ViewBag.Spokes = spokeCode;
                ViewBag.Communas = communaCode;
                return View("CityAdd", cityDTO);
            }
            else
                return RedirectToAction("ApprovalQueueList", "Approval");
        }
        #endregion
        #region CityDelete
        [HttpPost]
        [AuthorizeUserAttribute(Entity = "MT_CITY", AccessLevel = new UserActivities[] { UserActivities.Delete })]
        public JsonResult CityDelete(string ACityCode, string AOrganizationCode)
        {
            CityDTO ACityDTO = new CityDTO();
            try
            {
                ACityDTO = tMasterManager.FindByCityCode(ACityCode, AOrganizationCode);
                ACityDTO.LastUpdatedBy = tCommonManager.GetLoggedUserName();
                ACityDTO.ApprovalStatus = Utils.GetApprovalStatusCode(Utils.ApprovalStatus.Deleted);
                tMasterManager.UpdateCityDetails(ACityCode, AOrganizationCode, ACityDTO);

                var dResult = new { result = "success", action = "CityList", controller = "Master" };
                TempData["Message"] = String.Format(Resources.MsgInfoDeleteSuccess, String.Concat(new object[] { Resources.City, " ", ACityCode })); 
                return Json(dResult, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                var dResultFail = new { result = "fail", action = "CityList", controller = "Master" };
                TempData["Message"] = String.Format(Resources.MsgErrDeleteConcurrencyError, String.Concat(new object[] { Resources.City, " ", ACityCode })); 
                return Json(dResultFail, JsonRequestBehavior.AllowGet);
            }

        }
        #endregion
        #region Cascadind dropdowns
        public JsonResult GetRegions(string Id)
        {
            List<RegionDTO> regionDTO;
            regionDTO = new List<RegionDTO>();
            var regionCodes = tLookUpManager.GetAll<RegionDTO>(Id, "Region");

            return Json(regionCodes, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetHubs(string Id)
        {
            List<HubDTO> hubDTO;
            hubDTO = new List<HubDTO>();
            var hubCodes = tLookUpManager.GetAll<RegionDTO>(Id, "Hub");

            return Json(hubCodes, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetSpokes(string Id)
        {
            List<SpokeDTO> spokeDTO;
            spokeDTO = new List<SpokeDTO>();
            var spokeCodes = tLookUpManager.GetAll<SpokeDTO>(Id, "Spoke");

            return Json(spokeCodes, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetCommunas(string Id)
        {
            List<CommunaDTO> communaDTO;
            communaDTO = new List<CommunaDTO>();
            var communaCodes = tLookUpManager.GetAll<CommunaDTO>(Id, "Communa");

            return Json(communaCodes, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region GetCommunaByCountry
        public JsonResult GetCommunaByCountry(string ACountryCode)
        {
            var communaCodes = tLookUpManager.GetAll<CommunaDTO>(ACountryCode, "CommunaByCountry");
            
            return Json(communaCodes, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region GetCityByCommuna
        public JsonResult GetCityByCommuna(string ACommunaCode)
        {
            var cityCodes = tLookUpManager.GetAll<CityDTO>(ACommunaCode, "CityByCommuna");

            CityNewDTO objCityNewDTO = new CityNewDTO();
            objCityNewDTO  = tMasterManager.GetCityRegionByCommuna(ACommunaCode, "City");

            return Json(new { cityCodes, objCityNewDTO.CityCode }, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region GetRegionByCommuna
        public JsonResult GetRegionByCommuna(string ACommunaCode)
        {
            var regionCodes = tLookUpManager.GetAll<RegionDTO>(ACommunaCode, "RegionByCommuna");

            var regionCode = tMasterManager.GetCityRegionByCommuna(ACommunaCode, "Region");


            CityNewDTO objCityNewDTO = new CityNewDTO();
            objCityNewDTO = tMasterManager.GetCityRegionByCommuna(ACommunaCode, "Region");


            return Json(new { regionCodes, objCityNewDTO.RegionCode }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #endregion

        #region Credit Segment

        #region CreditSegmentView
        [AuthorizeUserAttribute(Entity = "MT_CREDIT_SEGMENT", AccessLevel = new UserActivities[] { UserActivities.View })]
        public ActionResult CreditSegmentView(string ACardSegmentCode, string AOrganizationCode)
        {
            try
            {
                CreditSegmentDTO CreditSegmentDTO;
                ViewBag.TableName = "CRDTSGMT";

                var legalEntity = tLookUpManager.GetAll<LegalEntityDTO>("", "LegalEntity");
                ViewBag.LegalEntity = legalEntity;

                ViewBag.Privileges = tCommonManager.GetUserPrivilegesForEntity("MT_CREDIT_SEGMENT");

                if (String.IsNullOrWhiteSpace(ACardSegmentCode))
                //return View("CreditSegmentList");
                {
                    CreditSegmentDTO = new CreditSegmentDTO();
                    CreditSegmentDTO.EffectiveFromDate = Utils.BusinessDate;
                    CreditSegmentDTO.EffectiveToDate = Utils.BusinessDate;
                    CreditSegmentDTO.OrganizationCode = Utils.OrganizationCode;
                    ViewData["FormStatus"] = CreditSegmentDTO.ApprovalStatus;

                    return View("CreditSegmentAdd", CreditSegmentDTO);
                }
                else
                {
                    CreditSegmentDTO = tMasterManager.FindByCardSegmentCode(ACardSegmentCode, AOrganizationCode);
                    ViewBag.ReasonCodes = null;
                }

                ViewData["FormStatus"] = CreditSegmentDTO.ApprovalStatus;

                return View("CreditSegmentAdd", CreditSegmentDTO);
            }
            catch (Exception ex)
            {
                LoggerFactory.CreateLog().LogError("Error" + ex.Message, 100, 2);
                ViewBag.ErrorMessage = Resources.MsgErrServerException;
                ViewBag.ErrorType = SessionErrorType.ServerException;
                return View("Error");
            }
        }
        #endregion
        #region CreditSegmentAdd
        [AuthorizeUserAttribute(Entity = "MT_CREDIT_SEGMENT", AccessLevel = new UserActivities[] { UserActivities.Add, UserActivities.Update, UserActivities.View })]
        public ActionResult CreditSegmentAdd(string ACardSegmentCode, string AOrganizationCode)
        {
            try
            {
                CreditSegmentDTO creditSegmentDTO;

                var legalEntity = tLookUpManager.GetAll<LegalEntityDTO>("", "LegalEntity");
                if (ACardSegmentCode == null)
                {
                    creditSegmentDTO = new CreditSegmentDTO();
                    creditSegmentDTO.EffectiveFromDate = Utils.BusinessDate;
                    creditSegmentDTO.EffectiveToDate = Utils.BusinessDate;
                    creditSegmentDTO.OrganizationCode = Utils.OrganizationCode;
                }
                else
                {
                    creditSegmentDTO = tMasterManager.FindByCardSegmentCode(ACardSegmentCode, AOrganizationCode);
                }
                if (!String.IsNullOrWhiteSpace(creditSegmentDTO.ApprovalStatus) && creditSegmentDTO.ApprovalStatus.Equals("P"))
                {
                    var reasonCodes = tLookUpManager.GetAll<ApprovalContextDTO>("CRDTSGMT", "ReasonContext");
                    ViewBag.ReasonCodes = reasonCodes;

                }

                ViewBag.Privileges = tCommonManager.GetUserPrivilegesForEntity("MT_CREDIT_SEGMENT");

                ViewData["FormStatus"] = creditSegmentDTO.ApprovalStatus;
                ViewBag.LegalEntity = legalEntity;
                return View(creditSegmentDTO);
            }
            catch (Exception ex)
            {
                LoggerFactory.CreateLog().LogError("Error" + ex.Message, 100, 2);
                ViewBag.ErrorMessage = Resources.MsgErrServerException;
                ViewBag.ErrorType = SessionErrorType.ServerException;
                return View("Error");
            }
        }
        #endregion
        #region CreditSegmentSave
        [HttpPost]
        [AuthorizeUserAttribute(Entity = "MT_CREDIT_SEGMENT", AccessLevel = new UserActivities[] { UserActivities.Add, UserActivities.Update })]
        public ActionResult CreditSegmentSave(string ACardSegmentCode, string AOrganizationCode, string Command, CreditSegmentDTO ACreditSegmentDTO)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (Command.Equals(Resources.Submit))
                        ACreditSegmentDTO.ApprovalStatus = "P";
                    else if (Command.Equals(Resources.Save))
                        ACreditSegmentDTO.ApprovalStatus = "D";

                    if (Command.Equals(Resources.Submit) || Command.Equals(Resources.Save))
                    {
                        if (string.IsNullOrWhiteSpace(tMasterManager.FindByCardSegmentCode(ACreditSegmentDTO.CardSegmentCode, AOrganizationCode).CardSegmentCode))
                        {
                            ACreditSegmentDTO.CreatedBy = tCommonManager.GetLoggedUserName();
                            ACreditSegmentDTO.LastUpdatedBy = tCommonManager.GetLoggedUserName();
                            ACreditSegmentDTO.OrganizationCode = Utils.OrganizationCode;
                            tMasterManager.SaveCreditSegmentDetails(ACreditSegmentDTO);
                            TempData["Message"] = String.Format(Resources.MsgInfoSaveSuccess, String.Concat(new object[] { Resources.CreditSegment, " ", ACreditSegmentDTO.CardSegmentCode })); 
                        }
                        else
                        {
                            if (!string.IsNullOrWhiteSpace(ACardSegmentCode))
                            {
                                ACreditSegmentDTO.LastUpdatedBy = tCommonManager.GetLoggedUserName();
                                tMasterManager.UpdateCreditSegmentDetails(ACardSegmentCode, AOrganizationCode, ACreditSegmentDTO);
                                TempData["Message"] = String.Format(Resources.MsgInfoSaveSuccess, String.Concat(new object[] { Resources.CreditSegment, " ", ACreditSegmentDTO.CardSegmentCode })); 
                            }
                            else
                            {
                                TempData["Message"] = String.Format(Resources.MsgErrAlreadyExists, String.Concat(new object[] { Resources.CreditSegment, " ", ACreditSegmentDTO.CardSegmentCode })); 
                            }
                        }
                    }
                    else if (Command.Equals(Resources.Delete))
                    {
                        try
                        {
                            //tMasterManager.DeleteCreditSegment(Id);

                            ACreditSegmentDTO.LastUpdatedBy = tCommonManager.GetLoggedUserName();
                            ACreditSegmentDTO.ApprovalStatus = Utils.GetApprovalStatusCode(Utils.ApprovalStatus.Deleted);
                            tMasterManager.UpdateCreditSegmentDetails(ACardSegmentCode, AOrganizationCode, ACreditSegmentDTO);

                            TempData["Message"] = String.Format(Resources.MsgInfoDeleteSuccess, String.Concat(new object[] { Resources.CreditSegment, " ", ACreditSegmentDTO.CardSegmentCode })); 
                        }
                        catch (Exception ex)
                        {
                            LoggerFactory.CreateLog().LogError("Error" + ex.Message, 100, 2);
                            TempData["Message"] = String.Format(Resources.MsgErrDeleteConcurrencyError, String.Concat(new object[] { Resources.CreditSegment, " ", ACardSegmentCode })); 
                            return RedirectToAction("CreditSegmentAdd", "master", new { ACardSegmentCode = ACardSegmentCode, AOrganizationCode = AOrganizationCode });
                        }
                    }
                }
            }

            catch (Exception ex)
            {
                LoggerFactory.CreateLog().LogError("Error" + ex.Message, 100, 2);
                ViewBag.ErrorMessage = Resources.MsgErrServerException;
                ViewBag.ErrorType = SessionErrorType.ServerException;
                return View("Error");
            }

            return ((Command.Equals(Resources.Submit) || (Command.Equals(Resources.Delete))) ? RedirectToAction("CreditSegmentAdd", "Master") : RedirectToAction("CreditSegmentAdd", "Master", new { ACardSegmentCode = ACardSegmentCode, AOrganizationCode = AOrganizationCode }));

        }
        #endregion
        #region CreditSegmentDelete
        [HttpPost]
        [AuthorizeUserAttribute(Entity = "MT_CREDIT_SEGMENT", AccessLevel = new UserActivities[] { UserActivities.Delete })]
        public JsonResult CreditSegmentDelete(string ACardSegmentCode, string AOrganizationCode)
        {
            CreditSegmentDTO ACreditSegmentDTO = new CreditSegmentDTO();
            try
            {
                ACreditSegmentDTO = tMasterManager.FindByCardSegmentCode(ACardSegmentCode, AOrganizationCode);
                ACreditSegmentDTO.LastUpdatedBy = tCommonManager.GetLoggedUserName();
                ACreditSegmentDTO.ApprovalStatus = Utils.GetApprovalStatusCode(Utils.ApprovalStatus.Deleted);
                tMasterManager.UpdateCreditSegmentDetails(ACardSegmentCode, ACreditSegmentDTO.OrganizationCode, ACreditSegmentDTO);

                var dResult = new { result = "success", action = "CreditSegmentList", controller = "Master" };
                TempData["Message"] = String.Format(Resources.MsgInfoDeleteSuccess, String.Concat(new object[] { Resources.CreditSegment, " ", ACreditSegmentDTO.CardSegmentCode })); 
                return Json(dResult, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                var dResultFail = new { result = "fail", action = "CreditSegmentList", controller = "Master" };
                TempData["Message"] = String.Format(Resources.MsgErrDeleteConcurrencyError, String.Concat(new object[] { Resources.CreditSegment, " ", ACreditSegmentDTO.CardSegmentCode })); 
                return Json(dResultFail, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion
        #region CreditSegmentList
        [AuthorizeUserAttribute(Entity = "MT_CREDIT_SEGMENT", AccessLevel = new UserActivities[] { UserActivities.View })]
        public ActionResult CreditSegmentList(CreditSegmentSearch AVMCreditSegmentSearch, string ClearButton, int? page)
        {
            try
            {
                ViewBag.Privileges = tCommonManager.GetUserPrivilegesForEntity("MT_CREDIT_SEGMENT");
                int iPageNumber = Convert.ToInt32((page == null) ? 0 : page);


                if (ClearButton == Resources.Clear)
                {
                    AVMCreditSegmentSearch.CardSegmentCode = null;
                    AVMCreditSegmentSearch.CardSegmentName = null;
                    //AVMCreditSegmentSearch.EffectiveFromDate = Utils.BusinessDate.Date;
                    //AVMCreditSegmentSearch.EffectiveToDate = Utils.BusinessDate.Date;
                    ModelState.Clear();
                }

                if ((Utils.OrganizationCode != "") ||
                     !((string.IsNullOrWhiteSpace(AVMCreditSegmentSearch.CardSegmentCode) &&
                            (string.IsNullOrWhiteSpace(AVMCreditSegmentSearch.CardSegmentName)) &&
                            (AVMCreditSegmentSearch.EffectiveFromDate == DateTime.MinValue) &&
                            (AVMCreditSegmentSearch.EffectiveToDate == DateTime.MinValue)
                            )))
                {
                    var entities = tMasterManager.FindAll(iPageNumber, new CreditSegmentDTO()
                    {
                        CardSegmentCode = AVMCreditSegmentSearch.CardSegmentCode,
                        CardSegmentName = AVMCreditSegmentSearch.CardSegmentName,
                        EffectiveFromDate = AVMCreditSegmentSearch.EffectiveFromDate,
                        EffectiveToDate = (AVMCreditSegmentSearch.EffectiveToDate == DateTime.MinValue) ? DateTime.MaxValue : AVMCreditSegmentSearch.EffectiveToDate
                    });
                    AVMCreditSegmentSearch.SearchResults = entities.AsQueryable();
                }
                else
                {
                    var currResult = tMasterManager.FindCreditSegment(1, Utils.GRD_ROWS_PER_PAGE).AsQueryable();
                    AVMCreditSegmentSearch.SearchResults = currResult.AsQueryable();
                    //AVMCreditSegmentSearch.EffectiveFromDate = Utils.BusinessDate;
                    ViewData["SearchMode"] = "Default";
                }

                if (AVMCreditSegmentSearch.EffectiveFromDate == DateTime.MinValue && AVMCreditSegmentSearch.EffectiveToDate == DateTime.MinValue)
                {
                    ViewData["SearchMode"] = "Default";
                }

                //if (!AVMCreditSegmentSearch.SearchDeleted)
                //{
                //    AVMCreditSegmentSearch.SearchResults = AVMCreditSegmentSearch.SearchResults.Where(x => x.ApprovalStatus != "DEL" && x.ApprovalStatus != "DIS");
                //}

                AVMCreditSegmentSearch.SearchResults = AVMCreditSegmentSearch.SearchResults.Where(x => x.ApprovalStatus != "DEL" && x.ApprovalStatus != "DIS");

                return View(AVMCreditSegmentSearch);
            }
            catch (Exception ex)
            {
                LoggerFactory.CreateLog().LogError("Error" + ex.Message, 100, 2);
                ViewBag.ErrorMessage = Resources.MsgErrServerException;
                ViewBag.ErrorType = SessionErrorType.ServerException;
                return View("Error");
            }
        }
        #endregion
        #region CreditSegmentAuthorize
        [AuthorizeUserAttribute(Entity = "MT_CREDIT_SEGMENT", AccessLevel = new UserActivities[] { UserActivities.Authorize })]
        public ActionResult CreditSegmentAuthorize(string ACardSegmentCode, string AOrganizationCode, string AMode, string QId)
        {
            ViewBag.RequestQueueId = QId;
            ViewBag.TableName = "CRDTSGMT";
            if (!String.IsNullOrWhiteSpace(ACardSegmentCode))
            {
                CreditSegmentDTO creditSegmentDTO = tMasterManager.FindByCardSegmentCode(ACardSegmentCode, AOrganizationCode);
                var legalEntity = tLookUpManager.GetAll<LegalEntityDTO>("", "LegalEntity");
                if (!String.IsNullOrWhiteSpace(creditSegmentDTO.ApprovalStatus) && creditSegmentDTO.ApprovalStatus.Equals("P"))
                {
                    var reasonCodes = tLookUpManager.GetAll<ApprovalContextDTO>("CRDTSGMT", "ReasonContext");
                    ViewBag.ReasonCodes = reasonCodes;
                }

                ViewBag.Privileges = tCommonManager.GetUserPrivilegesForEntity("MT_CREDIT_SEGMENT");
                ViewData["FormCall"] = "ApprovalQueue";
                ViewData["FormStatus"] = creditSegmentDTO.ApprovalStatus;
                ViewBag.LegalEntity = legalEntity;
                return View("CreditSegmentAdd", creditSegmentDTO);
            }
            else
                return RedirectToAction("ApprovalQueueList", "Approval");
        }
        #endregion

        #endregion

        #region Credit SubSegment

        #region CreditSubSegmentView
        [AuthorizeUserAttribute(Entity = "MT_CREDIT_SUBSEGMENT", AccessLevel = new UserActivities[] { UserActivities.View })]
        public ActionResult CreditSubSegmentView(string ACreditSegmentCode, int ACreditSubSegmentCode = 0)
        {
            try
            {
                CreditSubSegmentDTO CreditSubSegmentDTO;
                ViewBag.TableName = "CRSBSGMT";

                var CreditSegment = tLookUpManager.GetAll<CreditSegmentDTO>("", "CreditSegment");
                var CreditSubSegment = tLookUpManager.GetAll<CreditSubSegmentDTO>("", "CreditSubSegment");
                ViewBag.CreditSegmentCodeList = CreditSegment;
                ViewBag.CreditSubSegmentCodeList = CreditSubSegment;

                ViewBag.Privileges = tCommonManager.GetUserPrivilegesForEntity("MT_CREDIT_SUBSEGMENT");

                if (String.IsNullOrWhiteSpace(ACreditSegmentCode) && Convert.ToInt32(ACreditSubSegmentCode) == 0)
                {
                    CreditSubSegmentDTO = new CreditSubSegmentDTO();

                    ViewData["FormStatus"] = CreditSubSegmentDTO.ApprovalStatus;
                    return View("CreditSubSegmentAdd", CreditSubSegmentDTO);
                }
                else
                {
                    CreditSubSegmentDTO = tMasterManager.FindByCreditSubSegmentCode(ACreditSegmentCode, ACreditSubSegmentCode);
                    ViewBag.ReasonCodes = null;
                }

                ViewData["FormStatus"] = CreditSubSegmentDTO.ApprovalStatus;

                return View("CreditSubSegmentAdd", CreditSubSegmentDTO);
            }
            catch (Exception ex)
            {
                LoggerFactory.CreateLog().LogError("Error" + ex.Message, 100, 2);
                ViewBag.ErrorMessage = Resources.MsgErrServerException;
                ViewBag.ErrorType = SessionErrorType.ServerException;
                return View("Error");
            }
        }
        #endregion
        #region CreditSubSegmentAdd
        [AuthorizeUserAttribute(Entity = "MT_CREDIT_SUBSEGMENT", AccessLevel = new UserActivities[] { UserActivities.Add, UserActivities.Update, UserActivities.View })]
        public ActionResult CreditSubSegmentAdd(string ACreditSegmentCode, int ACreditSubSegmentCode = 0)
        {
            try
            {
                CreditSubSegmentDTO creditSubSegmentDTO;

                var CreditSegment = tLookUpManager.GetAll<CreditSegmentDTO>("", "CreditSegment");
                var CreditSubSegment = tLookUpManager.GetAll<CreditSubSegmentDTO>("", "CreditSubSegment");
                if (String.IsNullOrWhiteSpace(ACreditSegmentCode) && Convert.ToInt32(ACreditSubSegmentCode) == 0)
                {
                    creditSubSegmentDTO = new CreditSubSegmentDTO();
                }
                else
                {
                    creditSubSegmentDTO = tMasterManager.FindByCreditSubSegmentCode(ACreditSegmentCode, ACreditSubSegmentCode);
                }
                if (!String.IsNullOrWhiteSpace(creditSubSegmentDTO.ApprovalStatus) && creditSubSegmentDTO.ApprovalStatus.Equals("P"))
                {
                    var reasonCodes = tLookUpManager.GetAll<ApprovalContextDTO>("CRSBSGMT", "ReasonContext");
                    ViewBag.ReasonCodes = reasonCodes;
                }

                ViewBag.Privileges = tCommonManager.GetUserPrivilegesForEntity("MT_CREDIT_SUBSEGMENT");

                ViewData["FormStatus"] = creditSubSegmentDTO.ApprovalStatus;
                ViewBag.CreditSegmentCodeList = CreditSegment;
                ViewBag.CreditSubSegmentCodeList = CreditSubSegment;
                return View(creditSubSegmentDTO);
            }
            catch (Exception ex)
            {
                LoggerFactory.CreateLog().LogError("Error" + ex.Message, 100, 2);
                ViewBag.ErrorMessage = Resources.MsgErrServerException;
                ViewBag.ErrorType = SessionErrorType.ServerException;
                return View("Error");
            }
        }
        #endregion
        #region CreditSubSegmentSave
        [HttpPost]
        [AuthorizeUserAttribute(Entity = "MT_CREDIT_SUBSEGMENT", AccessLevel = new UserActivities[] { UserActivities.Add, UserActivities.Update })]
        public ActionResult CreditSubSegmentSave(string ACreditSegmentCode, int ACreditSubSegmentCode, string Command, CreditSubSegmentDTO ACreditSubSegmentDTO)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = tMasterManager.FindByCreditSubSegmentCode(ACreditSubSegmentDTO.CreditSegmentCode, ACreditSubSegmentDTO.CreditSubSegmentCode);

                    if (Command.Equals(Resources.Submit) || Command.Equals(Resources.Save))
                    {
                        if (Command.Equals(Resources.Submit))
                            ACreditSubSegmentDTO.ApprovalStatus = "P";
                        else if (Command.Equals(Resources.Save))
                            ACreditSubSegmentDTO.ApprovalStatus = "D";

                        if (Command.Equals(Resources.Submit) || Command.Equals(Resources.Save))
                        {
                            if (string.IsNullOrWhiteSpace(result.CreditSegmentCode) && Convert.ToInt32(result.CreditSubSegmentCode) == 0)
                            {
                                ACreditSubSegmentDTO.CreatedBy = tCommonManager.GetLoggedUserName();
                                ACreditSubSegmentDTO.LastUpdatedBy = tCommonManager.GetLoggedUserName();
                                tMasterManager.SaveCreditSubSegmentDetails(ACreditSubSegmentDTO);
                                result = tMasterManager.FindByCreditSubSegmentCode(ACreditSubSegmentDTO.CreditSegmentCode, ACreditSubSegmentDTO.CreditSubSegmentCode);
                                TempData["Message"] = String.Format(Resources.MsgInfoSaveSuccess, String.Concat(new object[] { Resources.CreditSubSegment, " ", ACreditSubSegmentDTO.CreditSegmentName })); 
                            }
                            else
                            {
                                if (!(string.IsNullOrWhiteSpace(ACreditSegmentCode) && Convert.ToInt32(ACreditSubSegmentCode) == 0))
                                {
                                    ACreditSubSegmentDTO.LastUpdatedBy = tCommonManager.GetLoggedUserName();
                                    tMasterManager.UpdateCreditSubSegmentDetails(ACreditSubSegmentDTO);
                                    TempData["Message"] = String.Format(Resources.MsgInfoSaveSuccess, String.Concat(new object[] { Resources.CreditSubSegment, " ", ACreditSubSegmentDTO.CreditSegmentName })); 
                                }
                                else
                                {
                                    TempData["Message"] = String.Format(Resources.MsgErrAlreadyExists, String.Concat(new object[] { Resources.CreditSubSegment, " ", ACreditSubSegmentDTO.CreditSegmentName })); 
                                }
                            }
                        }
                    }
                    else if (Command.Equals(Resources.Delete))
                    {
                        try
                        {
                            ACreditSubSegmentDTO.LastUpdatedBy = tCommonManager.GetLoggedUserName();
                            ACreditSubSegmentDTO.ApprovalStatus = Utils.GetApprovalStatusCode(Utils.ApprovalStatus.Deleted);
                            tMasterManager.UpdateCreditSubSegmentDetails(ACreditSubSegmentDTO);
                            TempData["Message"] = String.Format(Resources.MsgInfoDeleteSuccess, String.Concat(new object[] { Resources.CreditSubSegment, " ", ACreditSubSegmentDTO.CreditSegmentName })); 
                        }
                        catch (Exception ex)
                        {
                            LoggerFactory.CreateLog().LogError("Error" + ex.Message, 100, 2);
                            TempData["Message"] = String.Format(Resources.MsgErrDeleteConcurrencyError, String.Concat(new object[] { Resources.CreditSubSegment, " ", ACreditSubSegmentDTO.CreditSegmentName })); 
                            return RedirectToAction("CreditSubSegmentAdd", "master", new { ACreditSegmentCode = ACreditSegmentCode, ACreditSubSegmentCode = ACreditSubSegmentCode });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LoggerFactory.CreateLog().LogError("Error" + ex.Message, 100, 2);
                ViewBag.ErrorMessage = Resources.MsgErrServerException;
                ViewBag.ErrorType = SessionErrorType.ServerException;
                return View("Error");
            }
            return ((Command.Equals(Resources.Submit) || (Command.Equals(Resources.Delete))) ? RedirectToAction("CreditSubSegmentAdd", "Master") : RedirectToAction("CreditSubSegmentAdd", "Master", new { ACreditSegmentCode = ACreditSegmentCode, ACreditSubSegmentCode = ACreditSubSegmentCode }));
        }
        #endregion
        #region CreditSubSegmentDelete
        [HttpPost]
        [AuthorizeUserAttribute(Entity = "MT_CREDIT_SUBSEGMENT", AccessLevel = new UserActivities[] { UserActivities.Delete })]
        public JsonResult CreditSubSegmentDelete(string ACreditSegmentCode, int ACreditSubSegmentCode)
        {
            CreditSubSegmentDTO ACreditSubSegmentDTO = new CreditSubSegmentDTO();
            try
            {
                ACreditSubSegmentDTO = tMasterManager.FindByCreditSubSegmentCode(ACreditSegmentCode, ACreditSubSegmentCode);
                ACreditSubSegmentDTO.LastUpdatedBy = tCommonManager.GetLoggedUserName();
                ACreditSubSegmentDTO.ApprovalStatus = Utils.GetApprovalStatusCode(Utils.ApprovalStatus.Deleted);
                tMasterManager.UpdateCreditSubSegmentDetails(ACreditSubSegmentDTO);
                TempData["Message"] = String.Format(Resources.MsgInfoDeleteSuccess, String.Concat(new object[] { Resources.CreditSubSegment, " ", ACreditSubSegmentDTO.CreditSegmentName })); 

                var dResult = new { result = "success", action = "CreditSubSegmentList", controller = "Master" };

                return Json(dResult, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                var dResultFail = new { result = "fail", action = "CreditSubSegmentList", controller = "Master" };
                TempData["Message"] = String.Format(Resources.MsgErrDeleteConcurrencyError, String.Concat(new object[] { Resources.CreditSubSegment, " ", ACreditSubSegmentDTO.CreditSegmentName })); 
                return Json(dResultFail, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion
        #region CreditSubSegmentList
        [AuthorizeUserAttribute(Entity = "MT_CREDIT_SUBSEGMENT", AccessLevel = new UserActivities[] { UserActivities.View })]
        public ActionResult CreditSubSegmentList(CreditSubSegmentSearch AVMCreditSubSegmentSearch, string ClearButton, int? page)
        {
            try
            {
                int iPageNumber = Convert.ToInt32((page == null) ? 0 : page);

                var CreditSegment = tLookUpManager.GetAll<CreditSegmentDTO>("", "CreditSegment");
                var CreditSubSegment = tLookUpManager.GetAll<CreditSubSegmentDTO>("", "CreditSubSegment");

                if (ClearButton == Resources.Clear)
                {
                    AVMCreditSubSegmentSearch.CreditSegmentCode = null;
                    AVMCreditSubSegmentSearch.CreditSubSegmentCode = 0;
                    ModelState.Clear();
                }

                if (!(string.IsNullOrEmpty(AVMCreditSubSegmentSearch.SearchButton)) &&
                     !((string.IsNullOrWhiteSpace(AVMCreditSubSegmentSearch.CreditSegmentCode) &&
                          (Convert.ToInt32(AVMCreditSubSegmentSearch.CreditSubSegmentCode) == 0 &&
                          (string.IsNullOrWhiteSpace(AVMCreditSubSegmentSearch.Description))))))
                {
                    var entities = tMasterManager.FindAll(iPageNumber, new CreditSubSegmentDTO()
                    {
                        CreditSegmentCode = AVMCreditSubSegmentSearch.CreditSegmentCode,
                        CreditSubSegmentCode = AVMCreditSubSegmentSearch.CreditSubSegmentCode
                    });
                    AVMCreditSubSegmentSearch.SearchResults = entities.AsQueryable();
                }
                else
                {
                    var currResult = tMasterManager.FindCreditSubSegment(1, Utils.GRD_ROWS_PER_PAGE).AsQueryable();
                    AVMCreditSubSegmentSearch.SearchResults = currResult.AsQueryable();

                    ViewData["SearchMode"] = "Default";
                }
                ViewBag.CreditSegmentCodeList = CreditSegment;
                ViewBag.CreditSubSegmentCodeList = CreditSubSegment;

                AVMCreditSubSegmentSearch.SearchResults = AVMCreditSubSegmentSearch.SearchResults.Where(x => x.ApprovalStatus != "DEL" && x.ApprovalStatus != "DIS");

                return View(AVMCreditSubSegmentSearch);
            }
            catch (Exception ex)
            {
                LoggerFactory.CreateLog().LogError("Error" + ex.Message, 100, 2);
                ViewBag.ErrorMessage = Resources.MsgErrServerException;
                ViewBag.ErrorType = SessionErrorType.ServerException;
                return View("Error");
            }
        }
        #endregion
        #region CreditSubSegmentAuthorize
        [AuthorizeUserAttribute(Entity = "MT_CREDIT_SUBSEGMENT", AccessLevel = new UserActivities[] { UserActivities.Authorize })]
        public ActionResult CreditSubSegmentAuthorize(string ACreditSegmentCode, int ACreditSubSegmentCode, string AMode, string QId)
        {
            ViewBag.RequestQueueId = QId;
            ViewBag.TableName = "CRSBSGMT";
            if (!(string.IsNullOrWhiteSpace(ACreditSegmentCode) && Convert.ToInt32(ACreditSubSegmentCode) == 0))
            {
                CreditSubSegmentDTO creditSubSegmentDTO = tMasterManager.FindByCreditSubSegmentCode(ACreditSegmentCode, ACreditSubSegmentCode);
                var CreditSegmentCode = tLookUpManager.GetAll<CreditSegmentDTO>("", "CreditSegment");
                var CreditSubSegmentCode = tLookUpManager.GetAll<CreditSubSegmentDTO>("", "CreditSubSegment");
                if (!String.IsNullOrWhiteSpace(creditSubSegmentDTO.ApprovalStatus) && creditSubSegmentDTO.ApprovalStatus.Equals("P"))
                {
                    var reasonCodes = tLookUpManager.GetAll<ApprovalContextDTO>("CRSBSGMT", "ReasonContext");
                    ViewBag.ReasonCodes = reasonCodes;

                }

                ViewBag.Privileges = tCommonManager.GetUserPrivilegesForEntity("MT_CREDIT_SUBSEGMENT");
                ViewData["FormCall"] = "ApprovalQueue";
                ViewData["FormStatus"] = creditSubSegmentDTO.ApprovalStatus;
                ViewBag.CreditSegmentCodeList = CreditSegmentCode;
                ViewBag.CreditSubSegmentCodeList = CreditSubSegmentCode;
                return View("CreditSubSegmentAdd", creditSubSegmentDTO);
            }
            else
                return RedirectToAction("ApprovalQueueList", "Approval");
        }
        #endregion

        #endregion

        #region Designation

        #region DesignationView
        [AuthorizeUserAttribute(Entity = "MT_DESIGNATION", AccessLevel = new UserActivities[] { UserActivities.View })]
        public ActionResult DesignationView(string ADesignationCode, string AOrganizationCode)
        {
            try
            {
                DesignationDTO designationDTO;
                ViewBag.TableName = "DESIG";
                var Position = tLookUpManager.GetAll<DesignationDTO>(string.Empty, "Position");
                ViewBag.Privileges = tCommonManager.GetUserPrivilegesForEntity("MT_DESIGNATION");

                if (String.IsNullOrWhiteSpace(ADesignationCode))
                //return View("DesignationList");
                {
                    designationDTO = new DesignationDTO();
                    designationDTO.EffectiveFromDate = Utils.BusinessDate;
                    designationDTO.EffectiveToDate = Utils.BusinessDate;
                    designationDTO.OrganizationCode = Utils.OrganizationCode;
                    ViewData["FormStatus"] = designationDTO.ApprovalStatus;
                    //return View("DesignationAdd", designationDTO);
                }
                else
                {
                    designationDTO = tMasterManager.FindByDesignationCode(Convert.ToString(ADesignationCode), AOrganizationCode);
                    ViewBag.ReasonCodes = null;
                }

                ViewData["FormStatus"] = designationDTO.ApprovalStatus;
                ViewBag.Position = Position;
                return View("DesignationAdd", designationDTO);
            }
            catch (Exception ex)
            {
                LoggerFactory.CreateLog().LogError("Error" + ex.Message, 100, 2);
                ViewBag.ErrorMessage = Resources.MsgErrServerException;
                ViewBag.ErrorType = SessionErrorType.ServerException;
                return View("Error");
            }
        }
        #endregion
        #region DesignationList
        [AuthorizeUserAttribute(Entity = "MT_DESIGNATION", AccessLevel = new UserActivities[] { UserActivities.View })]
        public ActionResult DesignationList(DesignationSearch AVMDesignationSearch, string ClearButton, int? page)
        {
            try
            {

                int iPageNumber = Convert.ToInt32((page == null) ? 0 : page);
                var Position = tLookUpManager.GetAll<DesignationDTO>(string.Empty, "Position");

                if (ClearButton == Resources.Clear)
                {
                    AVMDesignationSearch.DesignationCode = null;
                    AVMDesignationSearch.DesignationDescription = null;
                    AVMDesignationSearch.Position = null;

                    AVMDesignationSearch.Rank = null;
                    ModelState.Clear();
                }


                if ((Utils.OrganizationCode != "") || (!(string.IsNullOrEmpty(AVMDesignationSearch.SearchButton)) &&
                     !((string.IsNullOrWhiteSpace(AVMDesignationSearch.DesignationCode)
                     && (string.IsNullOrWhiteSpace(AVMDesignationSearch.DesignationDescription))
                     && (string.IsNullOrWhiteSpace(AVMDesignationSearch.Position))

                     && (string.IsNullOrWhiteSpace(AVMDesignationSearch.Rank))))))
                {
                    var entities = tMasterManager.FindAll(iPageNumber, new DesignationDTO()
                    {
                        DesignationCode = AVMDesignationSearch.DesignationCode,
                        DesignationDescription = AVMDesignationSearch.DesignationDescription,
                        Position = AVMDesignationSearch.Position,

                        Rank = AVMDesignationSearch.Rank
                    });
                    AVMDesignationSearch.SearchResults = entities.AsQueryable();
                }
                else
                {
                    var currResult = tMasterManager.FindDesignation(1, Utils.GRD_ROWS_PER_PAGE).AsQueryable();

                    AVMDesignationSearch.SearchResults = currResult.AsQueryable();
                }
                ViewBag.Position = Position;

                AVMDesignationSearch.SearchResults = AVMDesignationSearch.SearchResults.Where(x => x.ApprovalStatus != "DEL" && x.ApprovalStatus != "DIS");
                ViewBag.Privileges = tCommonManager.GetUserPrivilegesForEntity("MT_DESIGNATION");
                return View(AVMDesignationSearch);
            }
            catch (Exception ex)
            {
                LoggerFactory.CreateLog().LogError("Error" + ex.Message, 100, 2);
                ViewBag.ErrorMessage = Resources.MsgErrServerException;
                ViewBag.ErrorType = SessionErrorType.ServerException;
                return View("Error");
            }
        }
        #endregion
        #region DesignationAdd
        [AuthorizeUserAttribute(Entity = "MT_DESIGNATION", AccessLevel = new UserActivities[] { UserActivities.Add, UserActivities.Update, UserActivities.View })]
        public ActionResult DesignationAdd(string ADesignationCode, string AOrganizationCode)
        {
            try
            {
                DesignationDTO designationDTO;
                var positionList = tLookUpManager.GetAll<DesignationDTO>(string.Empty, "Position");
                if (ADesignationCode == null)
                {
                    designationDTO = new DesignationDTO();
                    designationDTO.OrganizationCode = Utils.OrganizationCode;
                    designationDTO.EffectiveFromDate = Utils.BusinessDate;
                    designationDTO.EffectiveToDate = Utils.BusinessDate;
                }
                else
                    designationDTO = tMasterManager.FindByDesignationCode(Convert.ToString(ADesignationCode), AOrganizationCode);

                if (!String.IsNullOrWhiteSpace(designationDTO.ApprovalStatus) && designationDTO.ApprovalStatus.Equals("P"))
                {
                    var reasonCodes = tLookUpManager.GetAll<ApprovalContextDTO>("DESIG", "ReasonContext");
                    ViewBag.ReasonCodes = reasonCodes;
                }

                ViewBag.Privileges = tCommonManager.GetUserPrivilegesForEntity("MT_DESIGNATION");

                ViewData["FormStatus"] = designationDTO.ApprovalStatus;
                ViewBag.Position = positionList;
                return View(designationDTO);
            }
            catch (Exception ex)
            {
                LoggerFactory.CreateLog().LogError("Error" + ex.Message, 100, 2);
                ViewBag.ErrorMessage = Resources.MsgErrServerException;
                ViewBag.ErrorType = SessionErrorType.ServerException;
                return View("Error");
            }
        }
        #endregion
        #region DesignationSave
        [HttpPost]
        [AuthorizeUserAttribute(Entity = "MT_DESIGNATION", AccessLevel = new UserActivities[] { UserActivities.Add, UserActivities.Update })]
        public ActionResult DesignationSave(string ADesignationCode, string AOrganizationCode, string Command, DesignationDTO ADesignationDTO)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    if (Command.Equals(Resources.Submit))
                        ADesignationDTO.ApprovalStatus = "P";
                    else if (Command.Equals(Resources.Save))
                        ADesignationDTO.ApprovalStatus = "D";

                    if (Command.Equals(Resources.Submit) || Command.Equals(Resources.Save))
                    {
                        if (string.IsNullOrWhiteSpace(tMasterManager.FindByDesignationCode(ADesignationDTO.DesignationCode, AOrganizationCode).DesignationCode))
                        {
                            ADesignationDTO.CreatedBy = tCommonManager.GetLoggedUserName();
                            ADesignationDTO.LastUpdatedBy = tCommonManager.GetLoggedUserName();
                            ADesignationDTO.OrganizationCode = Utils.OrganizationCode;
                            tMasterManager.SaveDesignationDetails(ADesignationDTO);
                            TempData["Message"] = String.Format(Resources.MsgInfoSaveSuccess, String.Concat(new object[] { Resources.Designation, " ", ADesignationDTO.DesignationCode })); 
                        }
                        else
                        {
                            if (!string.IsNullOrWhiteSpace(ADesignationCode))
                            {
                                ADesignationDTO.LastUpdatedBy = tCommonManager.GetLoggedUserName();
                                tMasterManager.UpdateDesignationDetails(ADesignationCode, AOrganizationCode, ADesignationDTO);
                                TempData["Message"] = String.Format(Resources.MsgInfoSaveSuccess, String.Concat(new object[] { Resources.Designation, " ", ADesignationDTO.DesignationCode })); 
                            }
                            else
                            {
                                TempData["Message"] = String.Format(Resources.MsgErrAlreadyExists, String.Concat(new object[] { Resources.Designation, " ", ADesignationDTO.DesignationCode })); 
                            }
                        }
                    }
                    else if (Command.Equals(Resources.Delete))
                    {
                        try
                        {
                            ADesignationDTO.LastUpdatedBy = tCommonManager.GetLoggedUserName();
                            ADesignationDTO.ApprovalStatus = Utils.GetApprovalStatusCode(Utils.ApprovalStatus.Deleted);
                            tMasterManager.UpdateDesignationDetails(ADesignationCode, AOrganizationCode, ADesignationDTO);

                            TempData["Message"] = String.Format(Resources.MsgInfoDeleteSuccess, String.Concat(new object[] { Resources.Designation, " ", ADesignationDTO.DesignationCode })); 
                        }
                        catch (Exception ex)
                        {
                            LoggerFactory.CreateLog().LogError("Error" + ex.Message, 100, 2);
                            TempData["Message"] = String.Format(Resources.MsgErrDeleteConcurrencyError, String.Concat(new object[] { Resources.Designation, " ", ADesignationDTO.DesignationCode })); 
                            return RedirectToAction("DesignationAdd", "master", new { ADesignationCode = ADesignationCode, AOrganizationCode = AOrganizationCode });
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                LoggerFactory.CreateLog().LogError("Error" + ex.Message, 100, 2);
                ViewBag.ErrorMessage = Resources.MsgErrServerException;
                ViewBag.ErrorType = SessionErrorType.ServerException;
                return View("Error");
            }

            return ((Command.Equals(Resources.Submit) || Command.Equals(Resources.Delete)) ? RedirectToAction("DesignationAdd", "master") : RedirectToAction("DesignationAdd", "master", new { ADesignationCode = ADesignationCode, AOrganizationCode = AOrganizationCode }));

        }
        #endregion
        #region DesignationDelete
        [HttpPost]
        [AuthorizeUserAttribute(Entity = "MT_DESIGNATION", AccessLevel = new UserActivities[] { UserActivities.Delete })]
        public JsonResult DesignationDelete(string ADesignationCode, string AOrganizationCode)
        {
            DesignationDTO ADesignationDTO = new DesignationDTO();
            try
            {
                ADesignationDTO = tMasterManager.FindByDesignationCode(ADesignationCode, AOrganizationCode);
                ADesignationDTO.LastUpdatedBy = tCommonManager.GetLoggedUserName();
                ADesignationDTO.ApprovalStatus = Utils.GetApprovalStatusCode(Utils.ApprovalStatus.Deleted);
                tMasterManager.UpdateDesignationDetails(ADesignationCode, AOrganizationCode, ADesignationDTO);

                var dResult = new { result = "success", action = "DesignationList", controller = "Master" };
                TempData["Message"] = String.Format(Resources.MsgInfoDeleteSuccess, String.Concat(new object[] { Resources.Designation, " ", ADesignationDTO.DesignationCode })); 
                return Json(dResult, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                var dResultFail = new { result = "fail", action = "DesignationList", controller = "Master" };
                TempData["Message"] = String.Format(Resources.MsgErrDeleteConcurrencyError, String.Concat(new object[] { Resources.Designation, " ", ADesignationDTO.DesignationCode })); 
                return Json(dResultFail, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion
        #region DesignationAuthorize
        [AuthorizeUserAttribute(Entity = "MT_DESIGNATION", AccessLevel = new UserActivities[] { UserActivities.Authorize })]
        public ActionResult DesignationAuthorize(string ADesignationCode, string AOrganizationCode, string AMode, string QId)
        {
            ViewBag.RequestQueueId = QId;
            ViewBag.TableName = "DESIG";
            var Position = tLookUpManager.GetAll<DesignationDTO>("", "Position");
            if (!String.IsNullOrWhiteSpace(ADesignationCode))
            {
                DesignationDTO designationDTO = tMasterManager.FindByDesignationCode(ADesignationCode, AOrganizationCode);

                if (!String.IsNullOrWhiteSpace(designationDTO.ApprovalStatus) && designationDTO.ApprovalStatus.Equals("P"))
                {
                    var reasonCodes = tLookUpManager.GetAll<ApprovalContextDTO>("DESIG", "ReasonContext");
                    ViewBag.ReasonCodes = reasonCodes;
                }

                ViewBag.Privileges = tCommonManager.GetUserPrivilegesForEntity("MT_DESIGNATION");
                ViewData["FormCall"] = "ApprovalQueue";
                ViewBag.Position = Position;
                ViewData["FormStatus"] = designationDTO.ApprovalStatus;
                return View("DesignationAdd", designationDTO);
            }
            else
                return RedirectToAction("ApprovalQueueList", "Approval");
        }
        #endregion

        #endregion

        #region SystemParameter

        #region SystemParameterView
        [AuthorizeUserAttribute(Entity = "MT_SYSTEM_PARAMETER", AccessLevel = new UserActivities[] { UserActivities.View })]
        public ActionResult SystemParameterView(string Id)
        {
            try
            {
                SystemParameterDTO systemParameterDTO;
                ViewBag.TableName = "SYSPARM";

                ViewBag.Privileges = tCommonManager.GetUserPrivilegesForEntity("MT_SYSTEM_PARAMETER");

                if (String.IsNullOrWhiteSpace(Id))
                //return View("SystemParameterList");
                {
                    systemParameterDTO = new SystemParameterDTO();
                    systemParameterDTO.OrganizationCode = Utils.OrganizationCode;
                    ViewData["FormStatus"] = systemParameterDTO.ApprovalStatus;
                    return View("SystemParameterAdd", systemParameterDTO);
                }
                else
                {
                    systemParameterDTO = tMasterManager.FindByParameterId(Convert.ToInt32(Id));
                    ViewBag.ReasonCodes = null;
                }

                ViewData["FormStatus"] = systemParameterDTO.ApprovalStatus;

                return View("SystemParameterAdd", systemParameterDTO);
            }
            catch (Exception ex)
            {
                LoggerFactory.CreateLog().LogError("Error" + ex.Message, 100, 2);
                ViewBag.ErrorMessage = Resources.MsgErrServerException;
                ViewBag.ErrorType = SessionErrorType.ServerException;
                return View("Error");
            }
        }
        #endregion
        #region SystemParameterAdd
        [AuthorizeUserAttribute(Entity = "MT_SYSTEM_PARAMETER", AccessLevel = new UserActivities[] { UserActivities.Add, UserActivities.Update, UserActivities.View })]
        public ActionResult SystemParameterAdd(string Id)
        {
            try
            {
                SystemParameterDTO systemParameterDTO;

                if (Id == null)
                {
                    systemParameterDTO = new SystemParameterDTO();
                    systemParameterDTO.OrganizationCode = Utils.OrganizationCode;
                }
                else
                {
                    systemParameterDTO = tMasterManager.FindByParameterId(Convert.ToInt32(Id));
                }
                if (!String.IsNullOrWhiteSpace(systemParameterDTO.ApprovalStatus) && systemParameterDTO.ApprovalStatus.Equals("P"))
                {
                    var reasonCodes = tLookUpManager.GetAll<ApprovalContextDTO>("SYSPARM", "ReasonContext");
                    ViewBag.ReasonCodes = reasonCodes;
                }

                ViewBag.Privileges = tCommonManager.GetUserPrivilegesForEntity("MT_SYSTEM_PARAMETER");

                ViewData["FormStatus"] = systemParameterDTO.ApprovalStatus;
                return View(systemParameterDTO);
            }
            catch (Exception ex)
            {
                LoggerFactory.CreateLog().LogError("Error" + ex.Message, 100, 2);
                ViewBag.ErrorMessage = Resources.MsgErrServerException;
                ViewBag.ErrorType = SessionErrorType.ServerException;
                return View("Error");
            }
        }
        #endregion
        #region SystemParameterSave
        [HttpPost]
        [AuthorizeUserAttribute(Entity = "MT_SYSTEM_PARAMETER", AccessLevel = new UserActivities[] { UserActivities.Add, UserActivities.Update })]
        public ActionResult SystemParameterSave(int Id, string Command, SystemParameterDTO ASystemParameterDTO)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    if (Command.Equals(Resources.Submit))
                        ASystemParameterDTO.ApprovalStatus = "P";
                    else if (Command.Equals(Resources.Save))
                        ASystemParameterDTO.ApprovalStatus = "D";

                    if (Command.Equals(Resources.Submit) || Command.Equals(Resources.Save))
                    {
                        if (string.IsNullOrWhiteSpace(tMasterManager.FindByParameterId(Id).ParameterName))
                        {
                            ASystemParameterDTO.CreatedBy = tCommonManager.GetLoggedUserName();
                            ASystemParameterDTO.LastUpdatedBy = tCommonManager.GetLoggedUserName();
                            ASystemParameterDTO.OrganizationCode = Utils.OrganizationCode;
                            tMasterManager.SaveSystemParameterDetails(ASystemParameterDTO);
                            TempData["Message"] = String.Format(Resources.MsgInfoSaveSuccess, String.Concat(new object[] { Resources.SystemParameter, " ", ASystemParameterDTO.ParameterName })); 
                        }
                        else
                        {
                            if (!string.IsNullOrWhiteSpace(Convert.ToString(Id)))
                            {
                                ASystemParameterDTO.LastUpdatedBy = tCommonManager.GetLoggedUserName();
                                ASystemParameterDTO.OrganizationCode = Utils.OrganizationCode;
                                tMasterManager.UpdateSystemParameterDetails(Id, ASystemParameterDTO);
                                TempData["Message"] = String.Format(Resources.MsgInfoSaveSuccess, String.Concat(new object[] { Resources.SystemParameter, " ", ASystemParameterDTO.ParameterName })); 
                            }
                            else
                            {
                                TempData["Message"] = String.Format(Resources.MsgErrAlreadyExists, String.Concat(new object[] { Resources.SystemParameter, " ", ASystemParameterDTO.ParameterName })); 
                            }
                        }
                    }
                    else if (Command.Equals(Resources.Delete))
                    {
                        try
                        {
                            ASystemParameterDTO.LastUpdatedBy = tCommonManager.GetLoggedUserName();
                            ASystemParameterDTO.ApprovalStatus = Utils.GetApprovalStatusCode(Utils.ApprovalStatus.Deleted);
                            ASystemParameterDTO.OrganizationCode = Utils.OrganizationCode;
                            tMasterManager.UpdateSystemParameterDetails(Id, ASystemParameterDTO);

                            TempData["Message"] = String.Format(Resources.MsgInfoDeleteSuccess, String.Concat(new object[] { Resources.SystemParameter, " ", ASystemParameterDTO.ParameterName })); 
                        }
                        catch (Exception ex)
                        {
                            LoggerFactory.CreateLog().LogError("Error" + ex.Message, 100, 2);
                            TempData["Message"] = String.Format(Resources.MsgErrDeleteConcurrencyError, String.Concat(new object[] { Resources.SystemParameter, " ", ASystemParameterDTO.ParameterName })); 
                            return RedirectToAction("SystemParameterAdd", "master", new { Id = Id });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LoggerFactory.CreateLog().LogError("Error" + ex.Message, 100, 2);
                ViewBag.ErrorMessage = Resources.MsgErrServerException;
                ViewBag.ErrorType = SessionErrorType.ServerException;
                return View("Error");
            }

            return ((Command.Equals(Resources.Submit) || (Command.Equals(Resources.Delete))) ? RedirectToAction("SystemParameterAdd", "Master") : RedirectToAction("SystemParameterAdd", "Master", new { Id = Id }));

        }
        #endregion
        #region SystemParameterDelete

        [HttpPost]
        [AuthorizeUserAttribute(Entity = "MT_SYSTEM_PARAMETER", AccessLevel = new UserActivities[] { UserActivities.Delete })]
        public JsonResult SystemParameterDelete(int Id, SystemParameterDTO ASystemParameterDTO)
        {
            try
            {
                ASystemParameterDTO = tMasterManager.FindByParameterId(Id);
                ASystemParameterDTO.LastUpdatedBy = tCommonManager.GetLoggedUserName();
                ASystemParameterDTO.ApprovalStatus = Utils.GetApprovalStatusCode(Utils.ApprovalStatus.Deleted);
                tMasterManager.UpdateSystemParameterDetails(Id, ASystemParameterDTO);

                var dResult = new { result = "success", action = "SystemParameterList", controller = "Master" };
                TempData["Message"] = String.Format(Resources.MsgInfoDeleteSuccess, String.Concat(new object[] { Resources.SystemParameter, " ", ASystemParameterDTO.ParameterName })); 
                return Json(dResult, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                var dResultFail = new { result = "fail", action = "SystemParameterList", controller = "Master" };
                TempData["Message"] = String.Format(Resources.MsgErrDeleteConcurrencyError, String.Concat(new object[] { Resources.SystemParameter, " ", ASystemParameterDTO.ParameterName })); 
                return Json(dResultFail, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion
        #region SystemParameterList
        [AuthorizeUserAttribute(Entity = "MT_SYSTEM_PARAMETER", AccessLevel = new UserActivities[] { UserActivities.View })]
        public ActionResult SystemParameterList(SystemParameterSearch AVMSystemParameterSearch, string ClearButton, int? page)
        {
            try
            {
                int iPageNumber = Convert.ToInt32((page == null) ? 0 : page);

                if (ClearButton == Resources.Clear)
                {
                    AVMSystemParameterSearch.ParameterName = null;
                    ModelState.Clear();
                }

                if ((Utils.OrganizationCode != "") || (!(string.IsNullOrEmpty(AVMSystemParameterSearch.SearchButton)) &&
                     !(string.IsNullOrWhiteSpace(AVMSystemParameterSearch.ParameterName))))
                {
                    var entities = tMasterManager.FindAll(iPageNumber, new SystemParameterDTO() { ParameterName = AVMSystemParameterSearch.ParameterName });
                    AVMSystemParameterSearch.SearchResults = entities.AsQueryable();
                }
                else
                {
                    var currResult = tMasterManager.FindSystemParameter(1, Utils.GRD_ROWS_PER_PAGE).AsQueryable();

                    AVMSystemParameterSearch.SearchResults = currResult.AsQueryable();
                }

                AVMSystemParameterSearch.SearchResults = AVMSystemParameterSearch.SearchResults.Where(x => x.ApprovalStatus != "DEL" && x.ApprovalStatus != "DIS");
                ViewBag.Privileges = tCommonManager.GetUserPrivilegesForEntity("MT_SYSTEM_PARAMETER");
                return View(AVMSystemParameterSearch);
            }
            catch (Exception ex)
            {
                LoggerFactory.CreateLog().LogError("Error" + ex.Message, 100, 2);
                ViewBag.ErrorMessage = Resources.MsgErrServerException;
                ViewBag.ErrorType = SessionErrorType.ServerException;
                return View("Error");
            }
        }
        #endregion
        #region SystemParameterAuthorize
        [AuthorizeUserAttribute(Entity = "MT_SYSTEM_PARAMETER", AccessLevel = new UserActivities[] { UserActivities.Authorize })]
        public ActionResult SystemParameterAuthorize(string Id, string AMode, string QId)
        {
            ViewBag.RequestQueueId = QId;
            ViewBag.TableName = "SYSPARM";
            if (!String.IsNullOrWhiteSpace(Id))
            {
                SystemParameterDTO systemParameterDTO = tMasterManager.FindByParameterId(Convert.ToInt32(Id));

                if (!String.IsNullOrWhiteSpace(systemParameterDTO.ApprovalStatus) && systemParameterDTO.ApprovalStatus.Equals("P"))
                {
                    var reasonCodes = tLookUpManager.GetAll<ApprovalContextDTO>("SYSPARM", "ReasonContext");
                    ViewBag.ReasonCodes = reasonCodes;
                }

                ViewBag.Privileges = tCommonManager.GetUserPrivilegesForEntity("MT_SYSTEM_PARAMETER");
                ViewData["FormCall"] = "ApprovalQueue";
                ViewData["FormStatus"] = systemParameterDTO.ApprovalStatus;
                return View("SystemParameterAdd", systemParameterDTO);
            }
            else
                return RedirectToAction("ApprovalQueueList", "Approval");
        }
        #endregion

        #endregion

        #region Customer Segment
        #region CustomerSegmentView
        [AuthorizeUserAttribute(Entity = "MT_CUSTOMER_SEGMENT", AccessLevel = new UserActivities[] { UserActivities.View })]
        public ActionResult CustomerSegmentView(string ASegmentCode, string AOrganizationCode)
        {
            try
            {
                CustomerSegmentDTO customerSegmentDTO;
                ViewBag.TableName = "CSTSGMT";

                ViewBag.Privileges = tCommonManager.GetUserPrivilegesForEntity("MT_CUSTOMER_SEGMENT");

                if (String.IsNullOrWhiteSpace(ASegmentCode))
                //return View("CustomerSegmentList");
                {
                    customerSegmentDTO = new CustomerSegmentDTO();
                    customerSegmentDTO.OrganizationCode = Utils.OrganizationCode;
                    ViewData["FormStatus"] = customerSegmentDTO.ApprovalStatus;
                    return View("CustomerSegmentAdd", customerSegmentDTO);
                }
                else
                {
                    customerSegmentDTO = tMasterManager.FindBySegmentCode(ASegmentCode, AOrganizationCode);
                    ViewBag.ReasonCodes = null;
                }

                ViewData["FormStatus"] = customerSegmentDTO.ApprovalStatus;

                return View("CustomerSegmentAdd", customerSegmentDTO);
            }
            catch (Exception ex)
            {
                LoggerFactory.CreateLog().LogError("Error" + ex.Message, 100, 2);
                ViewBag.ErrorMessage = Resources.MsgErrServerException;
                ViewBag.ErrorType = SessionErrorType.ServerException;
                return View("Error");
            }
        }
        #endregion
        #region CustomerSegmentAdd
        [AuthorizeUserAttribute(Entity = "MT_CUSTOMER_SEGMENT", AccessLevel = new UserActivities[] { UserActivities.Add, UserActivities.Update, UserActivities.View })]
        public ActionResult CustomerSegmentAdd(string ASegmentCode, string AOrganizationCode)
        {
            try
            {
                CustomerSegmentDTO customerSegmentDTO;

                if (ASegmentCode == null)
                {
                    customerSegmentDTO = new CustomerSegmentDTO();
                    customerSegmentDTO.OrganizationCode = Utils.OrganizationCode;
                    customerSegmentDTO.IsDefault = true;
                }
                else
                {
                    customerSegmentDTO = tMasterManager.FindBySegmentCode(ASegmentCode, AOrganizationCode);
                }
                if (!String.IsNullOrWhiteSpace(customerSegmentDTO.ApprovalStatus) && customerSegmentDTO.ApprovalStatus.Equals("P"))
                {
                    var reasonCodes = tLookUpManager.GetAll<ApprovalContextDTO>("CSTSGMT", "ReasonContext");
                    ViewBag.ReasonCodes = reasonCodes;

                }

                ViewBag.Privileges = tCommonManager.GetUserPrivilegesForEntity("MT_CUSTOMER_SEGMENT");

                ViewData["FormStatus"] = customerSegmentDTO.ApprovalStatus;
                return View(customerSegmentDTO);
            }
            catch (Exception ex)
            {
                LoggerFactory.CreateLog().LogError("Error" + ex.Message, 100, 2);
                ViewBag.ErrorMessage = Resources.MsgErrServerException;
                ViewBag.ErrorType = SessionErrorType.ServerException;
                return View("Error");
            }
        }
        #endregion
        #region CustomerSegmentSave
        [HttpPost]
        [AuthorizeUserAttribute(Entity = "MT_CUSTOMER_SEGMENT", AccessLevel = new UserActivities[] { UserActivities.Add, UserActivities.Update })]
        public ActionResult CustomerSegmentSave(string ASegmentCode, string AOrganizationCode, string Command, CustomerSegmentDTO ACustomerSegmentDTO)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (Command.Equals(Resources.Submit))
                        ACustomerSegmentDTO.ApprovalStatus = "P";
                    else if (Command.Equals(Resources.Save))
                        ACustomerSegmentDTO.ApprovalStatus = "D";

                    if(ACustomerSegmentDTO.IsDefault)
                    {
                        if (CheckIfDefaultCustomerSegmentExists(ACustomerSegmentDTO.SegmentCode))
                        {
                            ModelState.AddModelError("IsDefault", String.Format(Resources.MsgErrDefaultExist, Resources.CustomerSegment));
                            ViewBag.Privileges = tCommonManager.GetUserPrivilegesForEntity("MT_CUSTOMER_SEGMENT");
                            ACustomerSegmentDTO.OrganizationCode = Utils.OrganizationCode;
                            ACustomerSegmentDTO.ApprovalStatus = "";
                            ViewData["FormStatus"] = "";
                            return View("CustomerSegmentAdd", ACustomerSegmentDTO);
                        }
                    }
                                        
                    if (Command.Equals(Resources.Submit) || Command.Equals(Resources.Save))
                    {
                        if (string.IsNullOrWhiteSpace(tMasterManager.FindBySegmentCode(ACustomerSegmentDTO.SegmentCode, AOrganizationCode).SegmentCode))
                        {
                            ACustomerSegmentDTO.CreatedBy = tCommonManager.GetLoggedUserName();
                            ACustomerSegmentDTO.LastUpdatedBy = tCommonManager.GetLoggedUserName();
                            ACustomerSegmentDTO.OrganizationCode = Utils.OrganizationCode;
                            tMasterManager.SaveCustomerSegmentDetails(ACustomerSegmentDTO);
                            TempData["Message"] = String.Format(Resources.MsgInfoSaveSuccess, String.Concat(new object[] { Resources.CustomerSegment, " ", ACustomerSegmentDTO.SegmentCode })); 
                        }
                        else
                        {
                            if (!string.IsNullOrWhiteSpace(Convert.ToString(ASegmentCode)))
                            {
                                ACustomerSegmentDTO.LastUpdatedBy = tCommonManager.GetLoggedUserName();
                                tMasterManager.UpdateCustomerSegmentDetails(ASegmentCode, AOrganizationCode, ACustomerSegmentDTO);
                                TempData["Message"] = String.Format(Resources.MsgInfoSaveSuccess, String.Concat(new object[] { Resources.CustomerSegment, " ", ACustomerSegmentDTO.SegmentCode })); 
                            }
                            else
                            {
                                TempData["Message"] = String.Format(Resources.MsgErrAlreadyExists, String.Concat(new object[] { Resources.CustomerSegment, " ", ACustomerSegmentDTO.SegmentCode })); 
                            }
                        }
                    }
                    else if (Command.Equals(Resources.Delete))
                    {
                        try
                        {
                            ACustomerSegmentDTO.LastUpdatedBy = tCommonManager.GetLoggedUserName();
                            ACustomerSegmentDTO.ApprovalStatus = Utils.GetApprovalStatusCode(Utils.ApprovalStatus.Deleted);
                            tMasterManager.UpdateCustomerSegmentDetails(ASegmentCode, AOrganizationCode, ACustomerSegmentDTO);

                            TempData["Message"] = String.Format(Resources.MsgInfoDeleteSuccess, String.Concat(new object[] { Resources.CustomerSegment, " ", ACustomerSegmentDTO.SegmentCode })); 
                        }
                        catch (Exception ex)
                        {
                            LoggerFactory.CreateLog().LogError("Error" + ex.Message, 100, 2);
                            TempData["Message"] = String.Format(Resources.MsgErrDeleteConcurrencyError, String.Concat(new object[] { Resources.CustomerSegment, " ", ACustomerSegmentDTO.SegmentCode })); 
                            return RedirectToAction("CustomerSegmentAdd", "master", new { ASegmentCode = ASegmentCode, AOrganizationCode = AOrganizationCode });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LoggerFactory.CreateLog().LogError("Error" + ex.Message, 100, 2);
                ViewBag.ErrorMessage = Resources.MsgErrServerException;
                ViewBag.ErrorType = SessionErrorType.ServerException;
                return View("Error");
            }

            return ((Command.Equals(Resources.Submit) || Command.Equals(Resources.Delete)) ? RedirectToAction("CustomerSegmentAdd", "Master") : RedirectToAction("CustomerSegmentAdd", "Master", new { ASegmentCode = ASegmentCode, AOrganizationCode = AOrganizationCode }));

        }
        #endregion
        #region CustmerSegmentDelete
        [HttpPost]
        [AuthorizeUserAttribute(Entity = "MT_CUSTOMER_SEGMENT", AccessLevel = new UserActivities[] { UserActivities.Delete })]
        public JsonResult CustomerSegmentDelete(string ASegmentCode, string AOrganizationCode)
        {
            CustomerSegmentDTO ACustomerSegmentDTO = new CustomerSegmentDTO();
            try
            {
                ACustomerSegmentDTO = tMasterManager.FindBySegmentCode(ASegmentCode, AOrganizationCode);
                ACustomerSegmentDTO.LastUpdatedBy = tCommonManager.GetLoggedUserName();
                ACustomerSegmentDTO.ApprovalStatus = Utils.GetApprovalStatusCode(Utils.ApprovalStatus.Deleted);
                tMasterManager.UpdateCustomerSegmentDetails(ASegmentCode, AOrganizationCode, ACustomerSegmentDTO);

                var dResult = new { result = "success", action = "CustomerSegmentList", controller = "Master" };
                TempData["Message"] = String.Format(Resources.MsgInfoDeleteSuccess, String.Concat(new object[] { Resources.CustomerSegment, " ", ACustomerSegmentDTO.SegmentCode })); 
                return Json(dResult, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                var dResultFail = new { result = "fail", action = "CustomerSegmentList", controller = "Master" };
                TempData["Message"] = String.Format(Resources.MsgErrDeleteConcurrencyError, String.Concat(new object[] { Resources.CustomerSegment, " ", ACustomerSegmentDTO.SegmentCode })); 
                return Json(dResultFail, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion
        #region CustomerSegmentList
        [AuthorizeUserAttribute(Entity = "MT_CUSTOMER_SEGMENT", AccessLevel = new UserActivities[] { UserActivities.View })]
        public ActionResult CustomerSegmentList(CustomerSegementSearch AVMCustomerSegmentSearch, string ClearButton, int? page)
        {
            try
            {
                int iPageNumber = Convert.ToInt32((page == null) ? 0 : page);

                if (ClearButton == Resources.Clear)
                {
                    AVMCustomerSegmentSearch.SegmentCode = null;
                    AVMCustomerSegmentSearch.SegmentName = null;
                    ModelState.Clear();
                }

                if ((Utils.OrganizationCode != "") || (!(string.IsNullOrEmpty(AVMCustomerSegmentSearch.SearchButton)) &&
                     !((string.IsNullOrWhiteSpace(AVMCustomerSegmentSearch.SegmentCode) && (string.IsNullOrWhiteSpace(AVMCustomerSegmentSearch.SegmentName))))))
                {
                    var entities = tMasterManager.FindAll(iPageNumber, new CustomerSegmentDTO()
                    {
                        SegmentCode = AVMCustomerSegmentSearch.SegmentCode,
                        SegmentName = AVMCustomerSegmentSearch.SegmentName
                    });
                    AVMCustomerSegmentSearch.SearchResults = entities.AsQueryable();
                }
                else
                {
                    var currResult = tMasterManager.FindCustomerSegment(1, Utils.GRD_ROWS_PER_PAGE).AsQueryable();

                    AVMCustomerSegmentSearch.SearchResults = currResult.AsQueryable();
                }

                AVMCustomerSegmentSearch.SearchResults = AVMCustomerSegmentSearch.SearchResults.Where(x => x.ApprovalStatus != "DEL" && x.ApprovalStatus != "DIS");
                ViewBag.Privileges = tCommonManager.GetUserPrivilegesForEntity("MT_CUSTOMER_SEGMENT");
                return View(AVMCustomerSegmentSearch);
            }
            catch (Exception ex)
            {
                LoggerFactory.CreateLog().LogError("Error" + ex.Message, 100, 2);
                ViewBag.ErrorMessage = Resources.MsgErrServerException;
                ViewBag.ErrorType = SessionErrorType.ServerException;
                return View("Error");
            }
        }
        #endregion
        #region CustomerSegmentAuthorize
        [AuthorizeUserAttribute(Entity = "MT_CUSTOMER_SEGMENT", AccessLevel = new UserActivities[] { UserActivities.Authorize })]
        public ActionResult CustomerSegmentAuthorize(string ASegmentCode, string AOrganizationCode, string AMode, string QId)
        {
            ViewBag.RequestQueueId = QId;
            ViewBag.TableName = "CSTSGMT";
            if (!String.IsNullOrWhiteSpace(ASegmentCode))
            {
                CustomerSegmentDTO customerSegmentDTO = tMasterManager.FindBySegmentCode(ASegmentCode, AOrganizationCode);

                if (!String.IsNullOrWhiteSpace(customerSegmentDTO.ApprovalStatus) && customerSegmentDTO.ApprovalStatus.Equals("P"))
                {
                    var reasonCodes = tLookUpManager.GetAll<ApprovalContextDTO>("CSTSGMT", "ReasonContext");
                    ViewBag.ReasonCodes = reasonCodes;
                }

                ViewBag.Privileges = tCommonManager.GetUserPrivilegesForEntity("MT_CUSTOMER_SEGMENT");
                ViewData["FormCall"] = "ApprovalQueue";
                ViewData["FormStatus"] = customerSegmentDTO.ApprovalStatus;
                return View("CustomerSegmentAdd", customerSegmentDTO);
            }
            else
                return RedirectToAction("ApprovalQueueList", "Approval");
        }
        #endregion
        #region CheckIfDefaultCustomerSegmentExists
        public bool CheckIfDefaultCustomerSegmentExists(string ACustomerSegmentCode)
        {
            CustomerSegmentDTO ACustomerSegmentDTO = new CustomerSegmentDTO();
            bool isExists = tMasterManager.CheckIfDefaultCustomerSegmentExists(ACustomerSegmentCode);
            string message = string.Empty;
            //if (isExists)
            //{
            //    message = String.Format(Resources.MsgErrDefaultExist, String.Concat(new object[] { Resources.CustomerSegment, " ", ACustomerSegmentDTO.SegmentCode })); 
            //}
            //return Json(new { Message = message }, JsonRequestBehavior.AllowGet);
            return isExists;
        }
        #endregion
        #endregion

        #region Customer SubSegment

        #region CustomerSubSegmentView
        [AuthorizeUserAttribute(Entity = "MT_CUSTOMER_SUB_SEGMENT", AccessLevel = new UserActivities[] { UserActivities.View })]
        public ActionResult CustomerSubSegmentView(string ASubSegmentCode, string AOrganizationCode)
        {
            try
            {
                CustomerSubSegmentDTO customerSubSegmentDTO;
                ViewBag.TableName = "CSTSBSGMT";
                ViewBag.Privileges = tCommonManager.GetUserPrivilegesForEntity("MT_CUSTOMER_SUB_SEGMENT");
                if (String.IsNullOrWhiteSpace(ASubSegmentCode))
                //return View("CustomerSubSegmentList");
                {
                    customerSubSegmentDTO = new CustomerSubSegmentDTO();
                    customerSubSegmentDTO.OrganizationCode = Utils.OrganizationCode;
                    ViewData["FormStatus"] = customerSubSegmentDTO.ApprovalStatus;
                    return View("CustomerSubSegmentAdd", customerSubSegmentDTO);
                }
                else
                {
                    customerSubSegmentDTO = tMasterManager.FindBySubSegmentCode(ASubSegmentCode, AOrganizationCode);
                    ViewBag.ReasonCodes = null;
                }

                ViewData["FormStatus"] = customerSubSegmentDTO.ApprovalStatus;

                return View("CustomerSubSegmentAdd", customerSubSegmentDTO);
            }
            catch (Exception ex)
            {
                LoggerFactory.CreateLog().LogError("Error" + ex.Message, 100, 2);
                ViewBag.ErrorMessage = Resources.MsgErrServerException;
                ViewBag.ErrorType = SessionErrorType.ServerException;
                return View("Error");
            }
        }
        #endregion
        #region CustomerSubSegmentAdd

        [AuthorizeUserAttribute(Entity = "MT_CUSTOMER_SUB_SEGMENT", AccessLevel = new UserActivities[] { UserActivities.Add, UserActivities.Update, UserActivities.View })]
        public ActionResult CustomerSubSegmentAdd(string ASubSegmentCode, string AOrganizationCode)
        {
            try
            {
                CustomerSubSegmentDTO customerSubSegmentDTO;

                if (ASubSegmentCode == null)
                {
                    customerSubSegmentDTO = new CustomerSubSegmentDTO();
                    customerSubSegmentDTO.OrganizationCode = Utils.OrganizationCode;
                    customerSubSegmentDTO.IsDefault = true;
                }
                else
                {
                    customerSubSegmentDTO = tMasterManager.FindBySubSegmentCode(ASubSegmentCode, AOrganizationCode);
                }
                if (!String.IsNullOrWhiteSpace(customerSubSegmentDTO.ApprovalStatus) && customerSubSegmentDTO.ApprovalStatus.Equals("P"))
                {
                    var reasonCodes = tLookUpManager.GetAll<ApprovalContextDTO>("CSTSBSGMT", "ReasonContext");
                    ViewBag.ReasonCodes = reasonCodes;

                }

                ViewBag.Privileges = tCommonManager.GetUserPrivilegesForEntity("MT_CUSTOMER_SUB_SEGMENT");

                ViewData["FormStatus"] = customerSubSegmentDTO.ApprovalStatus;
                return View(customerSubSegmentDTO);
            }
            catch (Exception ex)
            {
                LoggerFactory.CreateLog().LogError("Error" + ex.Message, 100, 2);
                ViewBag.ErrorMessage = Resources.MsgErrServerException;
                ViewBag.ErrorType = SessionErrorType.ServerException;
                return View("Error");
            }
        }
        #endregion
        #region CustomerSubSegmentSave
        [HttpPost]
        [AuthorizeUserAttribute(Entity = "MT_CUSTOMER_SUB_SEGMENT", AccessLevel = new UserActivities[] { UserActivities.Add, UserActivities.Update })]
        public ActionResult CustomerSubSegmentSave(string ASubSegmentCode, string AOrganizationCode, string Command, CustomerSubSegmentDTO ACustomerSubSegmentDTO)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    if (ACustomerSubSegmentDTO.IsDefault)
                    {
                        if (CheckIfDefaultCustomerSubSegmentExists(ACustomerSubSegmentDTO.SubSegmentCode))
                        {
                            ModelState.AddModelError("IsDefault", String.Format(Resources.MsgErrDefaultExist, Resources.CustomerSubSegment));
                            ViewBag.Privileges = tCommonManager.GetUserPrivilegesForEntity("MT_CUSTOMER_SUB_SEGMENT");
                            ACustomerSubSegmentDTO.OrganizationCode = Utils.OrganizationCode;
                            ACustomerSubSegmentDTO.ApprovalStatus = "";
                            ViewData["FormStatus"] = "";
                            return View("CustomerSubSegmentAdd", ACustomerSubSegmentDTO);
                        }
                    }

                    if (Command.Equals(Resources.Submit))
                        ACustomerSubSegmentDTO.ApprovalStatus = "P";
                    else if (Command.Equals(Resources.Save))
                        ACustomerSubSegmentDTO.ApprovalStatus = "D";

                    if (Command.Equals(Resources.Submit) || Command.Equals(Resources.Save))
                    {
                        if (string.IsNullOrWhiteSpace(tMasterManager.FindBySubSegmentCode(ACustomerSubSegmentDTO.SubSegmentCode, AOrganizationCode).SubSegmentCode))
                        {
                            ACustomerSubSegmentDTO.CreatedBy = tCommonManager.GetLoggedUserName();
                            ACustomerSubSegmentDTO.LastUpdatedBy = tCommonManager.GetLoggedUserName();
                            ACustomerSubSegmentDTO.OrganizationCode = Utils.OrganizationCode;
                            tMasterManager.SaveCustomerSubSegmentDetails(ACustomerSubSegmentDTO);
                            TempData["Message"] = String.Format(Resources.MsgInfoSaveSuccess, String.Concat(new object[] { Resources.CustomerSubSegment, " ", ACustomerSubSegmentDTO.SubSegmentCode })); 
                        }
                        else
                        {
                            if (!string.IsNullOrWhiteSpace(Convert.ToString(ASubSegmentCode)))
                            {
                                ACustomerSubSegmentDTO.LastUpdatedBy = tCommonManager.GetLoggedUserName();
                                tMasterManager.UpdateCustomerSubSegmentDetails(ASubSegmentCode, AOrganizationCode, ACustomerSubSegmentDTO);
                                TempData["Message"] = String.Format(Resources.MsgInfoSaveSuccess, String.Concat(new object[] { Resources.CustomerSubSegment, " ", ACustomerSubSegmentDTO.SubSegmentCode })); 
                            }
                            else
                            {
                                TempData["Message"] = String.Format(Resources.MsgErrAlreadyExists, String.Concat(new object[] { Resources.CustomerSubSegment, " ", ACustomerSubSegmentDTO.SubSegmentCode })); 
                            }
                        }
                    }
                    else if (Command.Equals(Resources.Delete))
                    {
                        try
                        {
                            ACustomerSubSegmentDTO.LastUpdatedBy = tCommonManager.GetLoggedUserName();
                            ACustomerSubSegmentDTO.ApprovalStatus = Utils.GetApprovalStatusCode(Utils.ApprovalStatus.Deleted);
                            tMasterManager.UpdateCustomerSubSegmentDetails(ASubSegmentCode, AOrganizationCode, ACustomerSubSegmentDTO);

                            TempData["Message"] = String.Format(Resources.MsgInfoDeleteSuccess, String.Concat(new object[] { Resources.CustomerSubSegment, " ", ACustomerSubSegmentDTO.SubSegmentCode })); 
                        }
                        catch (Exception ex)
                        {
                            LoggerFactory.CreateLog().LogError("Error" + ex.Message, 100, 2);
                            TempData["Message"] = String.Format(Resources.MsgErrDeleteConcurrencyError, String.Concat(new object[] { Resources.CustomerSubSegment, " ", ACustomerSubSegmentDTO.SubSegmentCode })); 
                            return RedirectToAction("CustomerSubSegmentAdd", "master", new { ASubSegmentCode = ASubSegmentCode, AOrganizationCode = AOrganizationCode });
                        }
                    }
                }

            }

            catch (Exception ex)
            {
                LoggerFactory.CreateLog().LogError("Error" + ex.Message, 100, 2);
                ViewBag.ErrorMessage = Resources.MsgErrServerException;
                ViewBag.ErrorType = SessionErrorType.ServerException;
                return View("Error");
            }

            return ((Command.Equals(Resources.Submit) || Command.Equals(Resources.Delete)) ? RedirectToAction("CustomerSubSegmentAdd", "Master") : RedirectToAction("CustomerSubSegmentAdd", "Master", new { ASubSegmentCode = ASubSegmentCode, AOrganizationCode = AOrganizationCode }));

        }
        #endregion
        #region CustomerSubSegmentDelete
        [HttpPost]
        [AuthorizeUserAttribute(Entity = "MT_CUSTOMER_SUB_SEGMENT", AccessLevel = new UserActivities[] { UserActivities.Delete })]
        public ActionResult CustomerSubSegmentDelete(string ASubSegmentCode, string AOrganizationCode, CustomerSubSegmentDTO ACustomerSubSegmentDTO)
        {
            try
            {
                ACustomerSubSegmentDTO = tMasterManager.FindBySubSegmentCode(ASubSegmentCode, AOrganizationCode);
                ACustomerSubSegmentDTO.LastUpdatedBy = tCommonManager.GetLoggedUserName();
                ACustomerSubSegmentDTO.ApprovalStatus = Utils.GetApprovalStatusCode(Utils.ApprovalStatus.Deleted);
                tMasterManager.UpdateCustomerSubSegmentDetails(ASubSegmentCode, AOrganizationCode, ACustomerSubSegmentDTO);

                var dResult = new { result = "success", action = "CustomerSubSegmentList", controller = "Master" };
                TempData["Message"] = String.Format(Resources.MsgInfoDeleteSuccess, String.Concat(new object[] { Resources.CustomerSubSegment, " ", ACustomerSubSegmentDTO.SubSegmentCode })); 
                return Json(dResult, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                var dResultFail = new { result = "fail", action = "CustomerSubSegmentList", controller = "Master" };
                TempData["Message"] = String.Format(Resources.MsgErrDeleteConcurrencyError, String.Concat(new object[] { Resources.CustomerSubSegment, " ", ACustomerSubSegmentDTO.SubSegmentCode })); 
                return Json(dResultFail, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion
        #region CustomerSubSegmentList
        [AuthorizeUserAttribute(Entity = "MT_CUSTOMER_SUB_SEGMENT", AccessLevel = new UserActivities[] { UserActivities.View })]
        public ActionResult CustomerSubSegmentList(CustomerSubSegmentSearch AVMCustomerSubSegmentSearch, string ClearButton, int? page)
        {
            try
            {
                int iPageNumber = Convert.ToInt32((page == null) ? 0 : page);

                if (ClearButton == Resources.Clear)
                {
                    AVMCustomerSubSegmentSearch.SubSegmentCode = null;
                    AVMCustomerSubSegmentSearch.SubSegmentName = null;
                    ModelState.Clear();
                }

                if ((Utils.OrganizationCode != "") || (!(string.IsNullOrEmpty(AVMCustomerSubSegmentSearch.SearchButton)) &&
                     !((string.IsNullOrWhiteSpace(AVMCustomerSubSegmentSearch.SubSegmentCode) && (string.IsNullOrWhiteSpace(AVMCustomerSubSegmentSearch.SubSegmentName))))))
                {
                    var entities = tMasterManager.FindAll(iPageNumber, new CustomerSubSegmentDTO()
                    {
                        SubSegmentCode = AVMCustomerSubSegmentSearch.SubSegmentCode,
                        SubSegmentName = AVMCustomerSubSegmentSearch.SubSegmentName
                    });
                    AVMCustomerSubSegmentSearch.SearchResults = entities.AsQueryable();
                }
                else
                {
                    var currResult = tMasterManager.FindCustomerSubSegment(1, Utils.GRD_ROWS_PER_PAGE).AsQueryable();

                    AVMCustomerSubSegmentSearch.SearchResults = currResult.AsQueryable();
                }

                AVMCustomerSubSegmentSearch.SearchResults = AVMCustomerSubSegmentSearch.SearchResults.Where(x => x.ApprovalStatus != "DEL" && x.ApprovalStatus != "DIS");
                ViewBag.Privileges = tCommonManager.GetUserPrivilegesForEntity("MT_CUSTOMER_SUB_SEGMENT");
                return View(AVMCustomerSubSegmentSearch);
            }
            catch (Exception ex)
            {
                LoggerFactory.CreateLog().LogError("Error" + ex.Message, 100, 2);
                ViewBag.ErrorMessage = Resources.MsgErrServerException;
                ViewBag.ErrorType = SessionErrorType.ServerException;
                return View("Error");
            }
        }
        #endregion
        #region CustomerSubSegmentAuthorize
        [AuthorizeUserAttribute(Entity = "MT_CUSTOMER_SUB_SEGMENT", AccessLevel = new UserActivities[] { UserActivities.Authorize })]
        public ActionResult CustomerSubSegmentAuthorize(string ASubSegmentCode, string AOrganizationCode, string AMode, string QId)
        {
            ViewBag.RequestQueueId = QId;
            ViewBag.TableName = "CSTSBSGMT";
            if (!String.IsNullOrWhiteSpace(ASubSegmentCode))
            {
                CustomerSubSegmentDTO customerSubSegmentDTO = tMasterManager.FindBySubSegmentCode(ASubSegmentCode, AOrganizationCode);

                if (!String.IsNullOrWhiteSpace(customerSubSegmentDTO.ApprovalStatus) && customerSubSegmentDTO.ApprovalStatus.Equals("P"))
                {
                    var reasonCodes = tLookUpManager.GetAll<ApprovalContextDTO>("CSTSBSGMT", "ReasonContext");
                    ViewBag.ReasonCodes = reasonCodes;
                }

                ViewBag.Privileges = tCommonManager.GetUserPrivilegesForEntity("MT_CUSTOMER_SUB_SEGMENT");
                ViewData["FormCall"] = "ApprovalQueue";
                ViewData["FormStatus"] = customerSubSegmentDTO.ApprovalStatus;
                return View("CustomerSubSegmentAdd", customerSubSegmentDTO);
            }
            else
                return RedirectToAction("ApprovalQueueList", "Approval");
        }
        #endregion
        #region CheckIfDefaultCustomerSubSegmentExists
        public bool CheckIfDefaultCustomerSubSegmentExists(string ACustomerSubSegmentCode)
        {
            CustomerSubSegmentDTO ACustomerSubSegmentDTO = new CustomerSubSegmentDTO();
            bool isExists = tMasterManager.CheckIfDefaultCustomerSubSegmentExists(ACustomerSubSegmentCode);
            string message = string.Empty;
            //if (isExists)
            //{
            //    message = String.Format(Resources.MsgErrDefaultExist, String.Concat(new object[] { Resources.CustomerSubSegment, " ", ACustomerSubSegmentDTO.SubSegmentCode }));  
            //}
            //return Json(new { Message = message }, JsonRequestBehavior.AllowGet);
            return isExists;
        }
        #endregion

        #endregion

        #region Card Status

        #region CardStatusView
        [AuthorizeUserAttribute(Entity = "MT_CARDSTATUS", AccessLevel = new UserActivities[] { UserActivities.View })]
        public ActionResult CardStatusView(string Id)
        {
            CardStatusDTO cardStatusDTO;
            ViewBag.TableName = "CRDSTS";

            var decisionList = tLookUpManager.GetAll<DecisionDTO>("", "Decision");
            ViewBag.Privileges = tCommonManager.GetUserPrivilegesForEntity("MT_CARDSTATUS");
            ViewBag.DecisionList = decisionList;

            if (String.IsNullOrWhiteSpace(Id))
            {
                cardStatusDTO = new CardStatusDTO();
                ViewData["FormStatus"] = cardStatusDTO.ApprovalStatus;
                return View("CardStatusAdd", cardStatusDTO);
            }
            else
            {
                cardStatusDTO = tMasterManager.FindByCardStatusCode(Convert.ToString(Id));
                ViewBag.ReasonCodes = null;
            }

            ViewData["FormStatus"] = cardStatusDTO.ApprovalStatus;

            return View("CardStatusAdd", cardStatusDTO);
        }
        #endregion
        #region CardStatusList
        [AuthorizeUserAttribute(Entity = "MT_CARDSTATUS", AccessLevel = new UserActivities[] { UserActivities.View })]
        public ActionResult CardStatusList(CardStatusSearch AVMCardStatusSearch, string ClearButton, int? page)
        {
            try
            {
                var decisionList = tLookUpManager.GetAll<DecisionDTO>("", "Decision");
                int iPageNumber = Convert.ToInt32((page == null) ? 0 : page);

                if (ClearButton == Resources.Clear)
                {
                    AVMCardStatusSearch.CardStatusCode = null;
                    AVMCardStatusSearch.DecisionCode = null;
                    AVMCardStatusSearch.Narration = null;
                    ModelState.Clear();
                }

                if ((Utils.OrganizationCode != string.Empty) || (!(string.IsNullOrEmpty(AVMCardStatusSearch.SearchButton)) &&
                     !((string.IsNullOrWhiteSpace(AVMCardStatusSearch.CardStatusCode)
                     && (string.IsNullOrWhiteSpace(AVMCardStatusSearch.DecisionCode))
                     && (string.IsNullOrWhiteSpace(AVMCardStatusSearch.Narration))
                     ))))
                {
                    var entities = tMasterManager.FindAll(iPageNumber, new CardStatusDTO()
                    {
                        CardStatusCode = AVMCardStatusSearch.CardStatusCode,
                        DecisionCode = AVMCardStatusSearch.DecisionCode,
                        Narration = AVMCardStatusSearch.Narration
                    });
                    AVMCardStatusSearch.SearchResults = entities.AsQueryable();
                }
                else
                {
                    var currResult = tMasterManager.FindCardStatus(1, Utils.GRD_ROWS_PER_PAGE).AsQueryable();
                    AVMCardStatusSearch.SearchResults = currResult.AsQueryable();
                }

                ViewBag.DecisionList = decisionList;

                AVMCardStatusSearch.SearchResults = AVMCardStatusSearch.SearchResults.Where(x => x.ApprovalStatus != "DEL" && x.ApprovalStatus != "DIS");
                ViewBag.Privileges = tCommonManager.GetUserPrivilegesForEntity("MT_CARDSTATUS");
                return View(AVMCardStatusSearch);
            }
            catch (Exception ex)
            {

                LoggerFactory.CreateLog().LogError("Error" + ex.Message, 100, 2);
                return View("CardStatusAdd");
            }
        }
        #endregion
        #region CardStatusAdd
        [AuthorizeUserAttribute(Entity = "MT_CARDSTATUS", AccessLevel = new UserActivities[] { UserActivities.Add, UserActivities.Update, UserActivities.View })]
        public ActionResult CardStatusAdd(string Id)
        {
            CardStatusDTO cardStatusDTO;

            var decisionList = tLookUpManager.GetAll<DecisionDTO>("", "Decision");

            if (Id == null)
                cardStatusDTO = new CardStatusDTO();
            else
                cardStatusDTO = tMasterManager.FindByCardStatusCode(Convert.ToString(Id));

            if (!String.IsNullOrWhiteSpace(cardStatusDTO.ApprovalStatus) && cardStatusDTO.ApprovalStatus.Equals("P"))
            {
                var reasonCodes = tLookUpManager.GetAll<ApprovalContextDTO>("CRDSTS", "ReasonContext");
                ViewBag.ReasonCodes = reasonCodes;
            }

            ViewBag.Privileges = tCommonManager.GetUserPrivilegesForEntity("MT_CARDSTATUS");
            ViewBag.DecisionList = decisionList;

            ViewData["FormStatus"] = cardStatusDTO.ApprovalStatus;

            return View(cardStatusDTO);
        }
        #endregion
        #region CardStatusSave
        [HttpPost]
        [AuthorizeUserAttribute(Entity = "MT_CARDSTATUS", AccessLevel = new UserActivities[] { UserActivities.Add, UserActivities.Update })]
        public ActionResult CardStatusSave(string Id, string Command, CardStatusDTO ACardStatusDTO)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    if (Command.Equals(Resources.Submit))
                        ACardStatusDTO.ApprovalStatus = "P";
                    else if (Command.Equals(Resources.Save))
                        ACardStatusDTO.ApprovalStatus = "D";

                    ACardStatusDTO.OrganizationCode = Utils.OrganizationCode;
                    if (Command.Equals(Resources.Submit) || Command.Equals(Resources.Save))
                    {
                        if (string.IsNullOrWhiteSpace(tMasterManager.FindByCardStatusCode(ACardStatusDTO.CardStatusCode).CardStatusCode))
                        {
                            ACardStatusDTO.CreatedBy = tCommonManager.GetLoggedUserName();
                            ACardStatusDTO.LastUpdatedBy = tCommonManager.GetLoggedUserName();
                            tMasterManager.SaveCardStatusDetails(ACardStatusDTO);
                            TempData["Message"] = String.Format(Resources.MsgInfoSaveSuccess, String.Concat(new object[] { Resources.MT_CARDSTATUS, " ", ACardStatusDTO.CardStatusCode })); 
                        }
                        else
                        {
                            if (!string.IsNullOrWhiteSpace(Convert.ToString(Id)))
                            {
                                ACardStatusDTO.LastUpdatedBy = tCommonManager.GetLoggedUserName();
                                tMasterManager.UpdateCardStatusDetails(Id, ACardStatusDTO);
                                TempData["Message"] = String.Format(Resources.MsgInfoSaveSuccess, String.Concat(new object[] { Resources.MT_CARDSTATUS, " ", ACardStatusDTO.CardStatusCode })); 
                            }
                            else
                            {
                                TempData["Message"] = String.Format(Resources.MsgErrAlreadyExists, String.Concat(new object[] { Resources.MT_CARDSTATUS, " ", ACardStatusDTO.CardStatusCode })); 
                            }
                        }
                    }
                    else if (Command.Equals(Resources.Delete))
                    {
                        try
                        {
                            ACardStatusDTO.LastUpdatedBy = tCommonManager.GetLoggedUserName();
                            ACardStatusDTO.ApprovalStatus = Utils.GetApprovalStatusCode(Utils.ApprovalStatus.Deleted);
                            tMasterManager.UpdateCardStatusDetails(Id, ACardStatusDTO);

                            TempData["Message"] = String.Format(Resources.MsgInfoDeleteSuccess, String.Concat(new object[] { Resources.MT_CARDSTATUS, " ", ACardStatusDTO.CardStatusCode })); 
                        }
                        catch (Exception ex)
                        {
                            LoggerFactory.CreateLog().LogError("Error" + ex.Message, 100, 2);
                            TempData["Message"] = String.Format(Resources.MsgErrDeleteConcurrencyError, String.Concat(new object[] { Resources.MT_CARDSTATUS, " ", ACardStatusDTO.CardStatusCode })); 
                            return RedirectToAction("CardStatusAdd", "master", new { Id = Id });
                        }
                    }
                }
            }

            catch (Exception ex)
            {
                LoggerFactory.CreateLog().LogError("Error" + ex.Message, 100, 2);
            }

            return ((Command.Equals(Resources.Submit) || Command.Equals(Resources.Delete)) ? RedirectToAction("CardStatusAdd", "master") : RedirectToAction("CardStatusAdd", "master", new { Id = Id }));

        }
        #endregion
        #region CardStatusDelete
        [HttpPost]
        [AuthorizeUserAttribute(Entity = "MT_CARDSTATUS", AccessLevel = new UserActivities[] { UserActivities.Delete })]
        public JsonResult CardStatusDelete(string Id)
        {
            CardStatusDTO ACardStatusDTO = new CardStatusDTO();
            try
            {
                ACardStatusDTO = tMasterManager.FindByCardStatusCode(Id);
                ACardStatusDTO.LastUpdatedBy = tCommonManager.GetLoggedUserName();
                ACardStatusDTO.ApprovalStatus = Utils.GetApprovalStatusCode(Utils.ApprovalStatus.Deleted);
                ACardStatusDTO.OrganizationCode = Utils.OrganizationCode;
                tMasterManager.UpdateCardStatusDetails(Id, ACardStatusDTO);

                var dResult = new { result = "success", action = "CardStatusList", controller = "Master" };
                TempData["Message"] = String.Format(Resources.MsgInfoDeleteSuccess, String.Concat(new object[] { Resources.MT_CARDSTATUS, " ", ACardStatusDTO.CardStatusCode })); 
                return Json(dResult, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                var dResultFail = new { result = "fail", action = "CardStatusList", controller = "Master" };
                TempData["Message"] = String.Format(Resources.MsgErrDeleteConcurrencyError, String.Concat(new object[] { Resources.MT_CARDSTATUS, " ", ACardStatusDTO.CardStatusCode })); 
                return Json(dResultFail, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion
        #region CardStatusAuthorize
        [AuthorizeUserAttribute(Entity = "MT_CARDSTATUS", AccessLevel = new UserActivities[] { UserActivities.Authorize })]
        public ActionResult CardStatusAuthorize(string Id, string AMode, string QId)
        {
            ViewBag.RequestQueueId = QId;
            ViewBag.TableName = "CRDSTS";

            var decisionList = tLookUpManager.GetAll<DecisionDTO>("", "Decision");

            if (!String.IsNullOrWhiteSpace(Id))
            {
                CardStatusDTO cardStatusDTO = tMasterManager.FindByCardStatusCode(Id);

                if (!String.IsNullOrWhiteSpace(cardStatusDTO.ApprovalStatus) && cardStatusDTO.ApprovalStatus.Equals("P"))
                {
                    var reasonCodes = tLookUpManager.GetAll<ApprovalContextDTO>("CRDSTS", "ReasonContext");
                    ViewBag.ReasonCodes = reasonCodes;
                }

                ViewBag.DecisionList = decisionList;
                ViewBag.Privileges = tCommonManager.GetUserPrivilegesForEntity("MT_CARDSTATUS");
                ViewData["FormCall"] = "ApprovalQueue";
                ViewData["FormStatus"] = cardStatusDTO.ApprovalStatus;
                return View("CardStatusAdd", cardStatusDTO);
            }
            else
                return RedirectToAction("ApprovalQueueList", "Approval");
        }
        #endregion

        #endregion

        #region General

        #region GeneralView
    //    [AuthorizeUserAttribute(Entity = "MT_GENERAL", AccessLevel = new UserActivities[] { UserActivities.View })]
        public ActionResult GeneralView(string AKeyName, string AOrganizationCode)
        {
            GeneralDTO generalDTO;
            ViewBag.TableName = "GENRL";
            ViewBag.Privileges = tCommonManager.GetUserPrivilegesForEntity("MT_GENERAL");

            if (String.IsNullOrWhiteSpace(AKeyName))
            //return View("GeneralList");
            {
                generalDTO = new GeneralDTO();
                generalDTO.EffectiveFromDate = Utils.BusinessDate;
                generalDTO.EffectiveToDate = Utils.BusinessDate;
                generalDTO.OrganizationCode = Utils.OrganizationCode;
                ViewData["FormStatus"] = generalDTO.ApprovalStatus;
                return View("GeneralAdd", generalDTO);
            }
            else
            {
                generalDTO = tMasterManager.FindByGeneralKeyName(Convert.ToString(AKeyName), AOrganizationCode);
                ViewBag.ReasonCodes = null;
            }

            ViewData["FormStatus"] = generalDTO.ApprovalStatus;

            return View("GeneralAdd", generalDTO);
        }
        #endregion
        #region GeneralList
 //      [AuthorizeUserAttribute(Entity = "MT_GENERAL", AccessLevel = new UserActivities[] { UserActivities.View })]
        public ActionResult GeneralList(GeneralSearch AVMGeneralSearch, string ClearButton, int? page)
        {
            try
            {

                int iPageNumber = Convert.ToInt32((page == null) ? 0 : page);

                if (ClearButton == Resources.Clear)
                {
                    AVMGeneralSearch.KeyName = null;
                    AVMGeneralSearch.Value1 = null;
                    AVMGeneralSearch.Value2 = null;
                    ModelState.Clear();
                }

                if ((Utils.OrganizationCode != "") || (!(string.IsNullOrEmpty(AVMGeneralSearch.SearchButton)) &&
                     !((string.IsNullOrWhiteSpace(AVMGeneralSearch.KeyName)
                     && (string.IsNullOrWhiteSpace(AVMGeneralSearch.Value1))
                     && (string.IsNullOrWhiteSpace(AVMGeneralSearch.Value2))))))
                {
                    var entities = tMasterManager.FindAll(iPageNumber, new GeneralDTO()
                    {
                        KeyName = AVMGeneralSearch.KeyName,
                        Value1 = AVMGeneralSearch.Value1,
                        Value2 = AVMGeneralSearch.Value2
                    });
                    AVMGeneralSearch.SearchResults = entities.AsQueryable();
                }
                else
                {
                    var currResult = tMasterManager.FindGeneral(1, Utils.GRD_ROWS_PER_PAGE).AsQueryable();

                    AVMGeneralSearch.SearchResults = currResult.AsQueryable();
                }

                AVMGeneralSearch.SearchResults = AVMGeneralSearch.SearchResults.Where(x => x.ApprovalStatus != "DEL" && x.ApprovalStatus != "DIS");
                ViewBag.Privileges = tCommonManager.GetUserPrivilegesForEntity("MT_GENERAL");
                return View(AVMGeneralSearch);
            }
            catch (Exception ex)
            {

                LoggerFactory.CreateLog().LogError("Error" + ex.Message, 100, 2);
                return View("GeneralAdd");
            }
        }
        #endregion
        #region GeneralAdd
        [AuthorizeUserAttribute(Entity = "MT_GENERAL", AccessLevel = new UserActivities[] { UserActivities.Add, UserActivities.Update, UserActivities.View })]
        public ActionResult GeneralAdd(string AKeyName, string AOrganizationCode)
        {
            GeneralDTO generalDTO;

            if (AKeyName == null)
            {
                generalDTO = new GeneralDTO();
                generalDTO.OrganizationCode = Utils.OrganizationCode;
                generalDTO.EffectiveFromDate = Utils.BusinessDate;
                generalDTO.EffectiveToDate = Utils.BusinessDate;
            }
            else
                generalDTO = tMasterManager.FindByGeneralKeyName(Convert.ToString(AKeyName), AOrganizationCode);

            if (!String.IsNullOrWhiteSpace(generalDTO.ApprovalStatus) && generalDTO.ApprovalStatus.Equals("P"))
            {
                var reasonCodes = tLookUpManager.GetAll<ApprovalContextDTO>("GENRL", "ReasonContext");
                ViewBag.ReasonCodes = reasonCodes;
            }

            ViewBag.Privileges = tCommonManager.GetUserPrivilegesForEntity("MT_GENERAL");

            ViewData["FormStatus"] = generalDTO.ApprovalStatus;

            return View(generalDTO);
        }
        #endregion
        #region GeneralSave
        [HttpPost]
        [AuthorizeUserAttribute(Entity = "MT_GENERAL", AccessLevel = new UserActivities[] { UserActivities.Add, UserActivities.Update })]
        public ActionResult GeneralSave(string AKeyName, string AOrganizationCode, string Command, GeneralDTO AGeneralDTO)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    if (Command.Equals(Resources.Submit))
                        AGeneralDTO.ApprovalStatus = "P";
                    else if (Command.Equals(Resources.Save))
                        AGeneralDTO.ApprovalStatus = "D";

                    if (Command.Equals(Resources.Submit) || Command.Equals(Resources.Save))
                    {
                        if (string.IsNullOrWhiteSpace(tMasterManager.FindByGeneralKeyName(AGeneralDTO.KeyName, AOrganizationCode).KeyName))
                        {
                            AGeneralDTO.CreatedBy = tCommonManager.GetLoggedUserName();
                            AGeneralDTO.LastUpdatedBy = tCommonManager.GetLoggedUserName();
                            AGeneralDTO.OrganizationCode = Utils.OrganizationCode;
                            tMasterManager.SaveGeneralDetails(AGeneralDTO);
                            TempData["Message"] = String.Format(Resources.MsgInfoSaveSuccess, String.Concat(new object[] { Resources.MT_GENERAL, " ", AGeneralDTO.KeyName })); 
                        }
                        else
                        {
                            if (!string.IsNullOrWhiteSpace(AKeyName))
                            {
                                AGeneralDTO.LastUpdatedBy = tCommonManager.GetLoggedUserName();
                                tMasterManager.UpdateGeneralDetails(AKeyName, AOrganizationCode, AGeneralDTO);
                                TempData["Message"] = String.Format(Resources.MsgInfoSaveSuccess, String.Concat(new object[] { Resources.MT_GENERAL, " ", AGeneralDTO.KeyName })); 
                            }
                            else
                            {
                                TempData["Message"] = String.Format(Resources.MsgErrAlreadyExists, String.Concat(new object[] { Resources.MT_GENERAL, " ", AGeneralDTO.KeyName })); 
                            }
                        }
                    }
                    else if (Command.Equals(Resources.Delete))
                    {
                        try
                        {
                            AGeneralDTO.LastUpdatedBy = tCommonManager.GetLoggedUserName();
                            AGeneralDTO.ApprovalStatus = Utils.GetApprovalStatusCode(Utils.ApprovalStatus.Deleted);
                            tMasterManager.UpdateGeneralDetails(AKeyName, AOrganizationCode, AGeneralDTO);

                            TempData["Message"] = String.Format(Resources.MsgInfoDeleteSuccess, String.Concat(new object[] { Resources.MT_GENERAL, " ", AGeneralDTO.KeyName })); 
                        }
                        catch (Exception ex)
                        {
                            LoggerFactory.CreateLog().LogError("Error" + ex.Message, 100, 2);
                            TempData["Message"] = String.Format(Resources.MsgErrDeleteConcurrencyError, String.Concat(new object[] { Resources.MT_GENERAL, " ", AGeneralDTO.KeyName })); 
                            return RedirectToAction("GeneralAdd", "master", new { AKeyName = AKeyName, AOrganizationCode = AOrganizationCode });
                        }
                    }
                }

            }

            catch (Exception ex)
            {
                LoggerFactory.CreateLog().LogError("Error" + ex.Message, 100, 2);
            }

            return ((Command.Equals(Resources.Submit) || Command.Equals(Resources.Delete)) ? RedirectToAction("GeneralAdd", "master") : RedirectToAction("GeneralAdd", "master", new { AKeyName = AKeyName, AOrganizationCode = AOrganizationCode }));

        }
        #endregion
        #region GeneralDelete
        [HttpPost]
        [AuthorizeUserAttribute(Entity = "MT_GENERAL", AccessLevel = new UserActivities[] { UserActivities.Delete })]
        public JsonResult GeneralDelete(string AKeyName, string AOrganizationCode)
        {
            GeneralDTO AGeneralDTO = new GeneralDTO();
            try
            {
                AGeneralDTO = tMasterManager.FindByGeneralKeyName(AKeyName, AOrganizationCode);
                AGeneralDTO.LastUpdatedBy = tCommonManager.GetLoggedUserName();
                AGeneralDTO.ApprovalStatus = Utils.GetApprovalStatusCode(Utils.ApprovalStatus.Deleted);
                tMasterManager.UpdateGeneralDetails(AKeyName, AOrganizationCode, AGeneralDTO);

                var dResult = new { result = "success", action = "GeneralList", controller = "Master" };
                TempData["Message"] = String.Format(Resources.MsgInfoDeleteSuccess, String.Concat(new object[] { Resources.MT_GENERAL, " ", AGeneralDTO.KeyName })); 
                return Json(dResult, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                var dResultFail = new { result = "fail", action = "GeneralList", controller = "Master" };
                TempData["Message"] = String.Format(Resources.MsgErrDeleteConcurrencyError, String.Concat(new object[] { Resources.MT_GENERAL, " ", AGeneralDTO.KeyName })); 
                return Json(dResultFail, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion
        #region GeneralAuthorize
        [AuthorizeUserAttribute(Entity = "MT_GENERAL", AccessLevel = new UserActivities[] { UserActivities.Authorize })]
        public ActionResult GeneralAuthorize(string AKeyName, string AOrganizationCode, string AMode, string QId)
        {
            ViewBag.RequestQueueId = QId;
            ViewBag.TableName = "GENRL";
            if (!String.IsNullOrWhiteSpace(AKeyName))
            {
                GeneralDTO generalDTO = tMasterManager.FindByGeneralKeyName(AKeyName, AOrganizationCode);

                if (!String.IsNullOrWhiteSpace(generalDTO.ApprovalStatus) && generalDTO.ApprovalStatus.Equals("P"))
                {
                    var reasonCodes = tLookUpManager.GetAll<ApprovalContextDTO>("GENRL", "ReasonContext");
                    ViewBag.ReasonCodes = reasonCodes;
                }

                ViewBag.Privileges = tCommonManager.GetUserPrivilegesForEntity("MT_GENERAL");
                ViewData["FormCall"] = "ApprovalQueue";
                ViewData["FormStatus"] = generalDTO.ApprovalStatus;
                return View("GeneralAdd", generalDTO);
            }
            else
                return RedirectToAction("ApprovalQueueList", "Approval");
        }
        #endregion
        #endregion

        #region Contract

        #region ContractView
        [AuthorizeUserAttribute(Entity = "MT_CONTRACT", AccessLevel = new UserActivities[] { UserActivities.View })]
        public ActionResult ContractView(string AContractCode, string AOrganizationCode)
        {
            try
            {

                ContractDTO contractDTO;
                ViewBag.TableName = "CNT";

                var varContractTemplateList = tLookUpManager.GetAll<ContractTemplateDTO>("", "ContractTemplate");
                var varProductList = tLookUpManager.GetAll<ProductDTO>("", "Product");

                ViewBag.ContractTemplateList = varContractTemplateList;
                ViewBag.ProductList = varProductList;

                ViewBag.Privileges = tCommonManager.GetUserPrivilegesForEntity("MT_CONTRACT");

                if (String.IsNullOrWhiteSpace(AContractCode))
                //return View("ContractList");
                {
                    contractDTO = new ContractDTO();
                    contractDTO.EffectiveFromDate = Utils.BusinessDate;
                    contractDTO.EffectiveToDate = Utils.BusinessDate;
                    contractDTO.NotarySignDate = Utils.BusinessDate;
                    // contractDTO.OrganizationCode = Utils.OrganizationCode;
                    ViewData["FormStatus"] = contractDTO.ApprovalStatus;
                    return View("ContractAdd", contractDTO);
                }
                else
                {
                    contractDTO = tMasterManager.FindByContractCode(AContractCode, AOrganizationCode);
                    ViewBag.ReasonCodes = null;
                }

                ViewData["FormStatus"] = contractDTO.ApprovalStatus;

                return View("ContractAdd", contractDTO);
            }
            catch (Exception ex)
            {
                LoggerFactory.CreateLog().LogError("Error" + ex.Message, 100, 2);
                ViewBag.ErrorMessage = Resources.MsgErrServerException;
                ViewBag.ErrorType = SessionErrorType.ServerException;
                return View("Error");
            }
        }
        #endregion
        #region ContractAdd
        [AuthorizeUserAttribute(Entity = "MT_CONTRACT", AccessLevel = new UserActivities[] { UserActivities.Add, UserActivities.Update, UserActivities.View })]
        public ActionResult ContractAdd(string AContractCode, string AOrganizationCode)
        {
            try
            {

                ContractDTO contractDTO;

                var varContractTemplateList = tLookUpManager.GetAll<ContractTemplateDTO>("", "ContractTemplate");
                var varProductList = tLookUpManager.GetAll<ProductDTO>("", "Product");
                if (AContractCode == null)
                {
                    contractDTO = new ContractDTO();
                    // contractDTO.OrganizationCode = Utils.OrganizationCode;
                    contractDTO.EffectiveFromDate = Utils.BusinessDate;
                    contractDTO.EffectiveToDate = Utils.BusinessDate;
                    contractDTO.NotarySignDate = Utils.BusinessDate;
                }
                else
                {
                    contractDTO = tMasterManager.FindByContractCode(AContractCode, AOrganizationCode);
                }
                if (!String.IsNullOrWhiteSpace(contractDTO.ApprovalStatus) && contractDTO.ApprovalStatus.Equals("P"))
                {
                    var reasonCodes = tLookUpManager.GetAll<ApprovalContextDTO>("CNT", "ReasonContext");
                    ViewBag.ReasonCodes = reasonCodes;
                }

                ViewBag.Privileges = tCommonManager.GetUserPrivilegesForEntity("MT_CONTRACT");

                ViewData["FormStatus"] = contractDTO.ApprovalStatus;
                ViewBag.ContractTemplateList = varContractTemplateList;
                ViewBag.ProductList = varProductList;
                return View(contractDTO);
            }
            catch (Exception ex)
            {
                LoggerFactory.CreateLog().LogError("Error" + ex.Message, 100, 2);
                ViewBag.ErrorMessage = Resources.MsgErrServerException;
                ViewBag.ErrorType = SessionErrorType.ServerException;
                return View("Error");
            }
        }
        #endregion
        #region ContractSave
        [HttpPost]
        [AuthorizeUserAttribute(Entity = "MT_CONTRACT", AccessLevel = new UserActivities[] { UserActivities.Add, UserActivities.Update })]
        public ActionResult ContractSave(string AContractCode, string AOrganizationCode, string Command, ContractDTO AContractDTO)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    if (Command.Equals(Resources.Submit))
                        AContractDTO.ApprovalStatus = "P";
                    else if (Command.Equals(Resources.Save))
                        AContractDTO.ApprovalStatus = "D";

                    if (Command.Equals(Resources.Submit) || Command.Equals(Resources.Save))
                    {
                        if (string.IsNullOrWhiteSpace(tMasterManager.FindByContractCode(AContractDTO.ContractCode, AOrganizationCode).ContractCode))
                        {
                            AContractDTO.CreatedBy = tCommonManager.GetLoggedUserName();
                            AContractDTO.LastUpdatedBy = tCommonManager.GetLoggedUserName();
                            AContractDTO.OrganizationCode = Utils.OrganizationCode;
                            tMasterManager.SaveContractDetails(AContractDTO);
                            TempData["Message"] = String.Format(Resources.MsgInfoSaveSuccess, String.Concat(new object[] { Resources.Contract, " ", AContractDTO.ContractCode })); 
                        }
                        else
                        {
                            if (!string.IsNullOrWhiteSpace(AContractCode))
                            {
                                AContractDTO.LastUpdatedBy = tCommonManager.GetLoggedUserName();
                                tMasterManager.UpdateContractDetails(AContractCode, AOrganizationCode, AContractDTO);
                                TempData["Message"] = String.Format(Resources.MsgInfoSaveSuccess, String.Concat(new object[] { Resources.Contract, " ", AContractDTO.ContractCode })); 
                            }
                            else
                            {
                                TempData["Message"] = String.Format(Resources.MsgErrAlreadyExists, String.Concat(new object[] { Resources.Contract, " ", AContractDTO.ContractCode })); 
                            }
                        }
                    }
                    else if (Command.Equals(Resources.Delete))
                    {
                        try
                        {
                            AContractDTO.LastUpdatedBy = tCommonManager.GetLoggedUserName();
                            AContractDTO.ApprovalStatus = Utils.GetApprovalStatusCode(Utils.ApprovalStatus.Deleted);
                            tMasterManager.UpdateContractDetails(AContractCode, AOrganizationCode, AContractDTO);

                            TempData["Message"] = String.Format(Resources.MsgInfoDeleteSuccess, String.Concat(new object[] { Resources.Contract, " ", AContractDTO.ContractCode })); 
                        }
                        catch (Exception ex)
                        {
                            LoggerFactory.CreateLog().LogError("Error" + ex.Message, 100, 2);
                            TempData["Message"] = String.Format(Resources.MsgErrDeleteConcurrencyError, String.Concat(new object[] { Resources.Contract, " ", AContractCode })); 
                            return RedirectToAction("ContractAdd", "master", new { AContractCode = AContractCode, AOrganizationCode = AOrganizationCode });
                        }
                    }
                }

            }

            catch (Exception ex)
            {
                LoggerFactory.CreateLog().LogError("Error" + ex.Message, 100, 2);
                ViewBag.ErrorMessage = Resources.MsgErrServerException;
                ViewBag.ErrorType = SessionErrorType.ServerException;
                return View("Error");
            }

            return ((Command.Equals(Resources.Submit) || Command.Equals(Resources.Delete)) ? RedirectToAction("ContractAdd", "Master") : RedirectToAction("ContractAdd", "Master", new { AContractCode = AContractCode, AOrganizationCode = AOrganizationCode }));

        }
        #endregion
        #region ContractDelete
        [HttpPost]
        [AuthorizeUserAttribute(Entity = "MT_CONTRACT", AccessLevel = new UserActivities[] { UserActivities.Delete })]
        public JsonResult ContractDelete(string AContractCode, string AOrganizationCode)
        {
            ContractDTO AContractDTO = new ContractDTO();
            try
            {

                AContractDTO = tMasterManager.FindByContractCode(AContractCode, AOrganizationCode);
                AContractDTO.LastUpdatedBy = tCommonManager.GetLoggedUserName();
                AContractDTO.ApprovalStatus = Utils.GetApprovalStatusCode(Utils.ApprovalStatus.Deleted);
                tMasterManager.UpdateContractDetails(AContractCode, AOrganizationCode, AContractDTO);

                var dResult = new { result = "success", action = "ContractList", controller = "Master" };
                TempData["Message"] = String.Format(Resources.MsgInfoDeleteSuccess, String.Concat(new object[] { Resources.Contract, " ", AContractCode })); 
                return Json(dResult, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                var dResultFail = new { result = "fail", action = "ContractList", controller = "Master" };
                TempData["Message"] = String.Format(Resources.MsgErrDeleteConcurrencyError, String.Concat(new object[] { Resources.Contract, " ", AContractDTO.ContractCode })); 
                return Json(dResultFail, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion
        #region ContractList
        [AuthorizeUserAttribute(Entity = "MT_CONTRACT", AccessLevel = new UserActivities[] { UserActivities.View })]
        public ActionResult ContractList(ContractSearch AVMContractSearch, string ClearButton, int? page)
        {
            try
            {
                int iPageNumber = Convert.ToInt32((page == null) ? 0 : page);

                if (ClearButton == Resources.Clear)
                {
                    AVMContractSearch.ContractCode = null;
                    AVMContractSearch.ContractDescription = null;
                    ModelState.Clear();
                }

                if ((Utils.OrganizationCode != "") || (!(string.IsNullOrEmpty(AVMContractSearch.SearchButton)) &&
                   !((string.IsNullOrWhiteSpace(AVMContractSearch.ContractCode) && (string.IsNullOrWhiteSpace(AVMContractSearch.ContractDescription))))))
                {
                    var entities = tMasterManager.FindAll(iPageNumber, new ContractDTO()
                    {
                        ContractCode = AVMContractSearch.ContractCode,
                        ContractDescription = AVMContractSearch.ContractDescription
                    });
                    AVMContractSearch.SearchResults = entities.AsQueryable();
                }
                else
                {
                    var currResult = tMasterManager.FindContract(1, Utils.GRD_ROWS_PER_PAGE).AsQueryable();

                    AVMContractSearch.SearchResults = currResult.AsQueryable();
                }

                AVMContractSearch.SearchResults = AVMContractSearch.SearchResults.Where(x => x.ApprovalStatus != "DEL" && x.ApprovalStatus != "DIS");
                ViewBag.Privileges = tCommonManager.GetUserPrivilegesForEntity("MT_CONTRACT");
                return View(AVMContractSearch);
            }
            catch (Exception ex)
            {
                LoggerFactory.CreateLog().LogError("Error" + ex.Message, 100, 2);
                ViewBag.ErrorMessage = Resources.MsgErrServerException;
                ViewBag.ErrorType = SessionErrorType.ServerException;
                return View("Error");
            }
        }
        #endregion
        #region ContractAuthorize
        [AuthorizeUserAttribute(Entity = "MT_CONTRACT", AccessLevel = new UserActivities[] { UserActivities.Authorize })]
        public ActionResult ContractAuthorize(FormCollection ACollection, string AContractCode, string AOrganizationCode, string AMode, string QId)
        {
            ViewBag.RequestQueueId = QId;
            ViewBag.TableName = "CNT";
            if (!String.IsNullOrWhiteSpace(AContractCode))
            {
                ContractDTO contractDTO = tMasterManager.FindByContractCode(AContractCode, AOrganizationCode);
                var contractTemplateList = tLookUpManager.GetAll<ContractTemplateDTO>("", "ContractTemplate");
                var productList = tLookUpManager.GetAll<ProductDTO>("", "Product");
                if (!String.IsNullOrWhiteSpace(contractDTO.ApprovalStatus) && contractDTO.ApprovalStatus.Equals("P"))
                {
                    var reasonCodes = tLookUpManager.GetAll<ApprovalContextDTO>("CNT", "ReasonContext");
                    ViewBag.ReasonCodes = reasonCodes;
                }

                ViewBag.Privileges = tCommonManager.GetUserPrivilegesForEntity("MT_CONTRACT");
                ViewData["FormCall"] = "ApprovalQueue";
                ViewData["FormStatus"] = contractDTO.ApprovalStatus;
                ViewBag.ContractTemplateList = contractTemplateList;
                ViewBag.ProductList = productList;
                return View("ContractAdd", contractDTO);
            }
            else
                return RedirectToAction("ApprovalQueueList", "Approval");
        }
        #endregion

        #endregion

        #region Product Type
        #region ProductTypeView
        [AuthorizeUserAttribute(Entity = "MT_PRODUCT_TYPE", AccessLevel = new UserActivities[] { UserActivities.View })]
        public ActionResult ProductTypeView(string AProductTypeCode, string AOrganizationCode)
        {
            ProductTypeDTO productTypeDTO;
            ViewBag.TableName = "PDTTYPE";

            ViewBag.Privileges = tCommonManager.GetUserPrivilegesForEntity("MT_PRODUCT_TYPE");

            if (String.IsNullOrWhiteSpace(AProductTypeCode))
            //return View("ProductTypeList");
            {
                productTypeDTO = new ProductTypeDTO();
                productTypeDTO.OrganizationCode = Utils.OrganizationCode;
                ViewData["FormStatus"] = productTypeDTO.ApprovalStatus;
                return View("ProductTypeAdd", productTypeDTO);
            }
            else
            {
                productTypeDTO = tMasterManager.FindByProductTypeCode(AProductTypeCode, AOrganizationCode);
                ViewBag.ReasonCodes = null;
            }

            ViewData["FormStatus"] = productTypeDTO.ApprovalStatus;

            return View("ProductTypeAdd", productTypeDTO);
        }
        #endregion
        #region ProductTypeAdd

        [AuthorizeUserAttribute(Entity = "MT_PRODUCT_TYPE", AccessLevel = new UserActivities[] { UserActivities.Add, UserActivities.Update, UserActivities.View })]
        public ActionResult ProductTypeAdd(string AProductTypeCode, string AOrganizationCode)
        {
            ProductTypeDTO productTypeDTO;

            if (AProductTypeCode == null)
            {
                productTypeDTO = new ProductTypeDTO();
                productTypeDTO.OrganizationCode = Utils.OrganizationCode;
            }
            else
            {
                productTypeDTO = tMasterManager.FindByProductTypeCode(AProductTypeCode, AOrganizationCode);
            }

            if (!String.IsNullOrWhiteSpace(productTypeDTO.ApprovalStatus) && productTypeDTO.ApprovalStatus.Equals("P"))
            {
                var reasonCodes = tLookUpManager.GetAll<ApprovalContextDTO>("PDTTYPE", "ReasonContext");
                ViewBag.ReasonCodes = reasonCodes;
            }

            ViewBag.Privileges = tCommonManager.GetUserPrivilegesForEntity("MT_PRODUCT_TYPE");

            ViewData["FormStatus"] = productTypeDTO.ApprovalStatus;

            return View(productTypeDTO);
        }
        #endregion
        #region ProductTypeSave
        [HttpPost]
        [AuthorizeUserAttribute(Entity = "MT_PRODUCT_TYPE", AccessLevel = new UserActivities[] { UserActivities.Add, UserActivities.Update })]
        public ActionResult ProductTypeSave(string AProductTypeCode, string AOrganizationCode, string Command, ProductTypeDTO AProductTypeDTO)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    if (Command.Equals(Resources.Submit))
                        AProductTypeDTO.ApprovalStatus = "P";
                    else if (Command.Equals(Resources.Save))
                        AProductTypeDTO.ApprovalStatus = "D";

                    if (Command.Equals(Resources.Submit) || Command.Equals(Resources.Save))
                    {
                        if (string.IsNullOrWhiteSpace(tMasterManager.FindByProductTypeCode(AProductTypeDTO.ProductTypeCode, AOrganizationCode).ProductTypeCode))
                        {
                            AProductTypeDTO.CreatedBy = tCommonManager.GetLoggedUserName();
                            AProductTypeDTO.LastUpdatedBy = tCommonManager.GetLoggedUserName();
                            AProductTypeDTO.OrganizationCode = Utils.OrganizationCode;
                            tMasterManager.SaveProductTypeDetails(AProductTypeDTO);
                            TempData["Message"] = String.Format(Resources.MsgInfoSaveSuccess, String.Concat(new object[] { Resources.ProductType, " ", AProductTypeDTO.ProductTypeCode })); 
                        }
                        else
                        {
                            if (!string.IsNullOrWhiteSpace(AProductTypeCode))
                            {
                                AProductTypeDTO.LastUpdatedBy = tCommonManager.GetLoggedUserName();
                                tMasterManager.UpdateProductTypeDetails(AProductTypeCode, AOrganizationCode, AProductTypeDTO);
                                TempData["Message"] = String.Format(Resources.MsgInfoSaveSuccess, String.Concat(new object[] { Resources.ProductType, " ", AProductTypeDTO.ProductTypeCode })); 
                            }
                            else
                            {
                                TempData["Message"] = String.Format(Resources.MsgErrAlreadyExists, String.Concat(new object[] { Resources.ProductType, " ", AProductTypeDTO.ProductTypeCode })); 
                            }
                        }
                    }
                    else if (Command.Equals(Resources.Delete))
                    {
                        try
                        {
                            AProductTypeDTO.LastUpdatedBy = tCommonManager.GetLoggedUserName();
                            AProductTypeDTO.ApprovalStatus = Utils.GetApprovalStatusCode(Utils.ApprovalStatus.Deleted);
                            tMasterManager.UpdateProductTypeDetails(AProductTypeCode, AOrganizationCode, AProductTypeDTO);

                            TempData["Message"] = String.Format(Resources.MsgInfoDeleteSuccess, String.Concat(new object[] { Resources.ProductType, " ", AProductTypeDTO.ProductTypeCode })); 
                        }
                        catch (Exception ex)
                        {
                            LoggerFactory.CreateLog().LogError("Error" + ex.Message, 100, 2);
                            TempData["Message"] = String.Format(Resources.MsgErrDeleteConcurrencyError, String.Concat(new object[] { Resources.ProductType, " ", AProductTypeDTO.ProductTypeCode })); 
                            return RedirectToAction("ProductTypeAdd", "master", new { AProductTypeCode = AProductTypeCode, AOrganizationCode = AOrganizationCode });
                        }
                    }

                }

            }

            catch (Exception ex)
            {
                LoggerFactory.CreateLog().LogError("Error" + ex.Message, 100, 2);
            }

            return ((Command.Equals(Resources.Submit) || Command.Equals(Resources.Delete)) ? RedirectToAction("ProductTypeAdd", "Master") : RedirectToAction("ProductTypeAdd", "Master", new { AProductTypeCode = AProductTypeCode, AOrganizationCode = AOrganizationCode }));

        }
        #endregion
        #region ProductTypeDelete
        [HttpPost]
        [AuthorizeUserAttribute(Entity = "MT_PRODUCT_TYPE", AccessLevel = new UserActivities[] { UserActivities.Delete })]
        public JsonResult ProductTypeDelete(string AProductTypeCode, string AOrganizationCode)
        {
            ProductTypeDTO AProductTypeDTO = new ProductTypeDTO();
            try
            {
                AProductTypeDTO = tMasterManager.FindByProductTypeCode(AProductTypeCode, AOrganizationCode);
                AProductTypeDTO.LastUpdatedBy = tCommonManager.GetLoggedUserName();
                AProductTypeDTO.ApprovalStatus = Utils.GetApprovalStatusCode(Utils.ApprovalStatus.Deleted);
                tMasterManager.UpdateProductTypeDetails(AProductTypeCode, AOrganizationCode, AProductTypeDTO);

                var dResult = new { result = "success", action = "ProductTypeList", controller = "Master" };
                TempData["Message"] = String.Format(Resources.MsgInfoDeleteSuccess, String.Concat(new object[] { Resources.ProductType, " ", AProductTypeDTO.ProductTypeCode })); 
                return Json(dResult, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                var dResultFail = new { result = "fail", action = "ProductTypeList", controller = "Master" };
                TempData["Message"] = String.Format(Resources.MsgErrDeleteConcurrencyError, String.Concat(new object[] { Resources.ProductType, " ", AProductTypeDTO.ProductTypeCode })); 
                return Json(dResultFail, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion
        #region ProductTypeList
        [AuthorizeUserAttribute(Entity = "MT_PRODUCT_TYPE", AccessLevel = new UserActivities[] { UserActivities.View })]
        public ActionResult ProductTypeList(ProductTypeSearch AVMProductTypeSearch, string ClearButton, int? page)
        {
            try
            {
                int iPageNumber = Convert.ToInt32((page == null) ? 0 : page);

                if (ClearButton == Resources.Clear)
                {
                    AVMProductTypeSearch.ProductTypeCode = null;
                    AVMProductTypeSearch.Description = null;
                    ModelState.Clear();
                }

                if ((Utils.OrganizationCode != "") || (!(string.IsNullOrEmpty(AVMProductTypeSearch.SearchButton)) &&
                     !((string.IsNullOrWhiteSpace(AVMProductTypeSearch.ProductTypeCode) && (string.IsNullOrWhiteSpace(AVMProductTypeSearch.Description))))))
                {
                    var entities = tMasterManager.FindAll(iPageNumber, new ProductTypeDTO()
                    {
                        ProductTypeCode = AVMProductTypeSearch.ProductTypeCode,
                        Description = AVMProductTypeSearch.Description
                    });
                    AVMProductTypeSearch.SearchResults = entities.AsQueryable();
                }
                else
                {
                    var currResult = tMasterManager.FindProductType(1, Utils.GRD_ROWS_PER_PAGE).AsQueryable();

                    AVMProductTypeSearch.SearchResults = currResult.AsQueryable();
                }

                AVMProductTypeSearch.SearchResults = AVMProductTypeSearch.SearchResults.Where(x => x.ApprovalStatus != "DEL" && x.ApprovalStatus != "DIS");
                ViewBag.Privileges = tCommonManager.GetUserPrivilegesForEntity("MT_PRODUCT_TYPE");
                return View(AVMProductTypeSearch);
            }
            catch (Exception ex)
            {

                LoggerFactory.CreateLog().LogError("Error" + ex.Message, 100, 2);
                return View("ProductTypeAdd");
            }
        }
        #endregion
        #region ProductTypeAuthorize
        [AuthorizeUserAttribute(Entity = "MT_PRODUCT_TYPE", AccessLevel = new UserActivities[] { UserActivities.Authorize })]
        public ActionResult ProductTypeAuthorize(string AProductTypeCode, string AOrganizationCode, string AMode, string QId)
        {
            ViewBag.RequestQueueId = QId;
            ViewBag.TableName = "PDTTYPE";
            if (!String.IsNullOrWhiteSpace(AProductTypeCode))
            {
                ProductTypeDTO productTypeDTO = tMasterManager.FindByProductTypeCode(AProductTypeCode, AOrganizationCode);

                if (!String.IsNullOrWhiteSpace(productTypeDTO.ApprovalStatus) && productTypeDTO.ApprovalStatus.Equals("P"))
                {
                    var reasonCodes = tLookUpManager.GetAll<ApprovalContextDTO>("PDTTYPE", "ReasonContext");
                    ViewBag.ReasonCodes = reasonCodes;
                }

                ViewBag.Privileges = tCommonManager.GetUserPrivilegesForEntity("MT_PRODUCT_TYPE");
                ViewData["FormCall"] = "ApprovalQueue";
                ViewData["FormStatus"] = productTypeDTO.ApprovalStatus;
                return View("ProductTypeAdd", productTypeDTO);
            }
            else
                return RedirectToAction("ApprovalQueueList", "Approval");
        }
        #endregion
        #endregion

        #region Transaction
        #region TransactionView
        [AuthorizeUserAttribute(Entity = "MT_TRANSACTION", AccessLevel = new UserActivities[] { UserActivities.View })]
        public ActionResult TransactionView(string Id)
        {
            try
            {
                TransactionDTO transactionDTO;
                var CreditSegmentCode = tLookUpManager.GetAll<CreditSegmentDTO>(string.Empty, "CreditSegment");
                var organizationList = tLookUpManager.GetAll<OrganizationDTO>("", "Organization");
                ViewBag.TableName = "TRANSCTN";

                ViewBag.Privileges = tCommonManager.GetUserPrivilegesForEntity("MT_TRANSACTION");

                ViewBag.CreditSegmentCodeList = CreditSegmentCode;
                ViewBag.OrganizationList = organizationList;

                if (String.IsNullOrWhiteSpace(Id))
                //return View("TransactionList");
                {
                    transactionDTO = new TransactionDTO();
                    ViewData["FormStatus"] = transactionDTO.ApprovalStatus;
                    return View("TransactionAdd", transactionDTO);
                }
                else
                {
                    transactionDTO = tMasterManager.FindByTransactionCode(Id);
                    ViewData["FormStatus"] = transactionDTO.ApprovalStatus;
                    ViewBag.ReasonCodes = null;
                }


                return View("TransactionAdd", transactionDTO);
            }
            catch (Exception ex)
            {
                LoggerFactory.CreateLog().LogError("Error" + ex.Message, 100, 2);
                ViewBag.ErrorMessage = Resources.MsgErrServerException;
                ViewBag.ErrorType = SessionErrorType.ServerException;
                return View("Error");
            }
        }
        #endregion
        #region TransactionAdd
        [AuthorizeUserAttribute(Entity = "MT_TRANSACTION", AccessLevel = new UserActivities[] { UserActivities.Add, UserActivities.Update, UserActivities.View })]
        public ActionResult TransactionAdd(string Id)
        {
            try
            {
                TransactionDTO transactionDTO;
                var CreditSegmentCode = tLookUpManager.GetAll<CreditSegmentDTO>("", "CreditSegment");
                var organizationList = tLookUpManager.GetAll<OrganizationDTO>("", "Organization");
                if (Id == null)
                {
                    transactionDTO = new TransactionDTO();
                }
                else
                {
                    transactionDTO = tMasterManager.FindByTransactionCode(Id);
                }

                if (!String.IsNullOrWhiteSpace(transactionDTO.ApprovalStatus) && transactionDTO.ApprovalStatus.Equals("P"))
                {
                    var reasonCodes = tLookUpManager.GetAll<ApprovalContextDTO>("TRANSCTN", "ReasonContext");
                    ViewBag.ReasonCodes = reasonCodes;
                }


                ViewBag.Privileges = tCommonManager.GetUserPrivilegesForEntity("MT_TRANSACTION");

                ViewData["FormStatus"] = transactionDTO.ApprovalStatus;
                ViewBag.CreditSegmentCodeList = CreditSegmentCode;
                ViewBag.OrganizationList = organizationList;

                return View(transactionDTO);
            }
            catch (Exception ex)
            {
                LoggerFactory.CreateLog().LogError("Error" + ex.Message, 100, 2);
                ViewBag.ErrorMessage = Resources.MsgErrServerException;
                ViewBag.ErrorType = SessionErrorType.ServerException;
                return View("Error");
            }
        }
        #endregion
        #region TransactionSave
        [HttpPost]
        [AuthorizeUserAttribute(Entity = "MT_TRANSACTION", AccessLevel = new UserActivities[] { UserActivities.Add, UserActivities.Update })]
        public ActionResult TransactionSave(string Id, string Command, TransactionDTO ATransactionDTO)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (Command.Equals(Resources.Submit))
                        ATransactionDTO.ApprovalStatus = "P";
                    else if (Command.Equals(Resources.Save))
                        ATransactionDTO.ApprovalStatus = "D";

                    if (Command.Equals(Resources.Submit) || Command.Equals(Resources.Save))
                    {
                        if (string.IsNullOrWhiteSpace(tMasterManager.FindByTransactionCode(ATransactionDTO.TransactionCode).TransactionCode))
                        {
                            ATransactionDTO.CreatedBy = tCommonManager.GetLoggedUserName();
                            ATransactionDTO.LastUpdatedBy = tCommonManager.GetLoggedUserName();
                            ATransactionDTO.OrganizationCode = Utils.OrganizationCode;

                            tMasterManager.SaveTransactionDetails(ATransactionDTO);
                            TempData["Message"] = String.Format(Resources.MsgInfoSaveSuccess, String.Concat(new object[] { Resources.TransactionCode, " ", ATransactionDTO.TransactionCode })); 
                        }
                        else
                        {
                            if (!string.IsNullOrWhiteSpace(Convert.ToString(Id)))
                            {
                                ATransactionDTO.LastUpdatedBy = tCommonManager.GetLoggedUserName();

                                tMasterManager.UpdateTransactionDetails(Id, ATransactionDTO);
                                TempData["Message"] = String.Format(Resources.MsgInfoSaveSuccess, String.Concat(new object[] { Resources.TransactionCode, " ", ATransactionDTO.TransactionCode })); 
                            }
                            else
                            {
                                TempData["Message"] = String.Format(Resources.MsgErrAlreadyExists, String.Concat(new object[] { Resources.TransactionCode, " ", ATransactionDTO.TransactionCode })); 
                            }
                        }
                    }
                    else if (Command.Equals(Resources.Delete))
                    {
                        try
                        {
                            ATransactionDTO.LastUpdatedBy = tCommonManager.GetLoggedUserName();
                            ATransactionDTO.ApprovalStatus = Utils.GetApprovalStatusCode(Utils.ApprovalStatus.Deleted);
                            tMasterManager.UpdateTransactionDetails(Id, ATransactionDTO);

                            TempData["Message"] = String.Format(Resources.MsgInfoDeleteSuccess, String.Concat(new object[] { Resources.TransactionCode, " ", ATransactionDTO.TransactionCode })); 
                        }
                        catch (Exception ex)
                        {
                            LoggerFactory.CreateLog().LogError("Error" + ex.Message, 100, 2);
                            TempData["Message"] = String.Format(Resources.MsgErrDeleteConcurrencyError, String.Concat(new object[] { Resources.TransactionCode, " ", ATransactionDTO.TransactionCode })); 
                            return RedirectToAction("TransactionAdd", "master", new { Id = Id });
                        }
                    }

                }

            }

            catch (Exception ex)
            {
                LoggerFactory.CreateLog().LogError("Error" + ex.Message, 100, 2);
                ViewBag.ErrorMessage = Resources.MsgErrServerException;
                ViewBag.ErrorType = SessionErrorType.ServerException;
                return View("Error");
            }

            return ((Command.Equals(Resources.Submit) || Command.Equals(Resources.Delete)) ? RedirectToAction("TransactionAdd", "Master") : RedirectToAction("TransactionAdd", "Master", new { Id = Id }));

        }
        #endregion
        #region TransactionDelete
        [HttpPost]
        [AuthorizeUserAttribute(Entity = "MT_TRANSACTION", AccessLevel = new UserActivities[] { UserActivities.Delete })]
        public ActionResult TransactionDelete(string Id)
        {
            TransactionDTO ATransactionDTO = new TransactionDTO();
            try
            {
                ATransactionDTO = tMasterManager.FindByTransactionCode(Id);
                ATransactionDTO.LastUpdatedBy = tCommonManager.GetLoggedUserName();
                ATransactionDTO.ApprovalStatus = Utils.GetApprovalStatusCode(Utils.ApprovalStatus.Deleted);
                tMasterManager.UpdateTransactionDetails(Id, ATransactionDTO);

                var dResult = new { result = "success", action = "TransactionList", controller = "Master" };
                TempData["Message"] = String.Format(Resources.MsgInfoDeleteSuccess, String.Concat(new object[] { Resources.TransactionCode, " ", ATransactionDTO.TransactionCode })); 
                return Json(dResult, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                var dResultFail = new { result = "fail", action = "TransactionList", controller = "Master" };
                TempData["Message"] = String.Format(Resources.MsgErrDeleteConcurrencyError, String.Concat(new object[] { Resources.TransactionCode, " ", ATransactionDTO.TransactionCode })); 
                return Json(dResultFail, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion
        #region TransactionList
        [AuthorizeUserAttribute(Entity = "MT_TRANSACTION", AccessLevel = new UserActivities[] { UserActivities.View })]
        public ActionResult TransactionList(TransactionSearch AVMTransactionSearch, string ClearButton, int? page)
        {
            try
            {
                int iPageNumber = Convert.ToInt32((page == null) ? 0 : page);
                var CreditSegmentCode = tLookUpManager.GetAll<CreditSegmentDTO>("", "CreditSegment");
                var organizationList = tLookUpManager.GetAll<OrganizationDTO>("", "Organization");

                if (ClearButton == Resources.Clear)
                {
                    AVMTransactionSearch.TransactionCode = null;
                    AVMTransactionSearch.Description = null;
                    ModelState.Clear();
                }

                if ((!string.IsNullOrEmpty(Utils.OrganizationCode)) || (!(string.IsNullOrEmpty(AVMTransactionSearch.SearchButton)) &&
                     !((string.IsNullOrWhiteSpace(AVMTransactionSearch.TransactionCode) && (string.IsNullOrWhiteSpace(AVMTransactionSearch.Description))))))
                {
                    var entities = tMasterManager.FindAll(iPageNumber, new TransactionDTO()
                    {
                        TransactionCode = AVMTransactionSearch.TransactionCode,
                        Description = AVMTransactionSearch.Description
                    });
                    AVMTransactionSearch.SearchResults = entities.AsQueryable();
                }
                else
                {
                    var currResult = tMasterManager.FindTransaction(1, Utils.GRD_ROWS_PER_PAGE).AsQueryable();

                    AVMTransactionSearch.SearchResults = currResult.AsQueryable();
                }
                ViewBag.CreditSegmentCodeList = CreditSegmentCode;
                ViewBag.OrganizationList = organizationList;

                AVMTransactionSearch.SearchResults = AVMTransactionSearch.SearchResults.Where(x => x.ApprovalStatus != "DEL" && x.ApprovalStatus != "DIS");

                return View(AVMTransactionSearch);
            }
            catch (Exception ex)
            {
                LoggerFactory.CreateLog().LogError("Error" + ex.Message, 100, 2);
                ViewBag.ErrorMessage = Resources.MsgErrServerException;
                ViewBag.ErrorType = SessionErrorType.ServerException;
                return View("Error");
            }
        }
        #endregion
        #region TransactionAuthorize
        [AuthorizeUserAttribute(Entity = "MT_TRANSACTION", AccessLevel = new UserActivities[] { UserActivities.Authorize })]
        public ActionResult TransactionAuthorize(string Id, string AMode, string QId)
        {
            ViewBag.RequestQueueId = QId;
            ViewBag.TableName = "TRANSCTN";
            var CreditSegmentCode = tLookUpManager.GetAll<CreditSegmentDTO>("", "CreditSegment");
            var organizationList = tLookUpManager.GetAll<OrganizationDTO>("", "Organization");

            if (!String.IsNullOrWhiteSpace(Id))
            {
                TransactionDTO transactionDTO = tMasterManager.FindByTransactionCode(Id);

                if (!String.IsNullOrWhiteSpace(transactionDTO.ApprovalStatus) && transactionDTO.ApprovalStatus.Equals("P"))
                {
                    var reasonCodes = tLookUpManager.GetAll<ApprovalContextDTO>("TRANSCTN", "ReasonContext");
                    ViewBag.ReasonCodes = reasonCodes;
                }

                ViewBag.Privileges = tCommonManager.GetUserPrivilegesForEntity("MT_TRANSACTION");
                ViewData["FormCall"] = "ApprovalQueue";
                ViewData["FormStatus"] = transactionDTO.ApprovalStatus;
                ViewBag.CreditSegmentCodeList = CreditSegmentCode;
                ViewBag.OrganizationList = organizationList;

                return View("TransactionAdd", transactionDTO);
            }
            else
                return RedirectToAction("ApprovalQueueList", "Approval");
        }
        #endregion
        #endregion

        #region SystemMaintenanceConfiguration
        #region SystemMaintenanceConfigurationView
        [AuthorizeUserAttribute(Entity = "MT_SystemMaintenanceConfiguration", AccessLevel = new UserActivities[] { UserActivities.View })]
        public ActionResult SystemMaintenanceConfigurationView(string Id)
        {
            SystemMaintenanceConfigurationDTO systemMaintenanceConfigurationDTO;
            var BaseCurrency = tLookUpManager.GetAll<CurrencyDTO>(string.Empty, "Currency");
            ViewBag.TableName = "SYSMTNCNFIG";

            ViewBag.Privileges = tCommonManager.GetUserPrivilegesForEntity("MT_SystemMaintenanceConfiguration");

            if (String.IsNullOrWhiteSpace(Id))
            {
                systemMaintenanceConfigurationDTO = new SystemMaintenanceConfigurationDTO();
                ViewData["FormStatus"] = systemMaintenanceConfigurationDTO.ApprovalStatus;
                return View("SystemMaintenanceConfigurationAdd", systemMaintenanceConfigurationDTO);
            }
            else
            {
                systemMaintenanceConfigurationDTO = tMasterManager.FindByBaseCurrency(Id);
                ViewBag.ReasonCodes = null;
            }

            ViewData["FormStatus"] = "R";
            ViewBag.CurrencyList = BaseCurrency;
            return View("SystemMaintenanceConfigurationAdd", systemMaintenanceConfigurationDTO);
        }
        #endregion
        #region SystemMaintenanceConfigurationAdd
        [AuthorizeUserAttribute(Entity = "MT_SystemMaintenanceConfiguration", AccessLevel = new UserActivities[] { UserActivities.Add, UserActivities.Update, UserActivities.View })]
        public ActionResult SystemMaintenanceConfigurationAdd(string Id)
        {
            SystemMaintenanceConfigurationDTO systemMaintenanceConfigurationDTO;
            var Currency = tLookUpManager.GetAll<CurrencyDTO>("", "Currency");
            if (Id == null)
            {
                systemMaintenanceConfigurationDTO = new SystemMaintenanceConfigurationDTO();
            }
            else
            {
                systemMaintenanceConfigurationDTO = tMasterManager.FindByBaseCurrency(Id);
            }

            if (!String.IsNullOrWhiteSpace(systemMaintenanceConfigurationDTO.ApprovalStatus) && systemMaintenanceConfigurationDTO.ApprovalStatus.Equals("P"))
            {
                var reasonCodes = tLookUpManager.GetAll<ApprovalContextDTO>("MTNCNFG", "ReasonContext");
                ViewBag.ReasonCodes = reasonCodes;
            }

            ViewBag.Privileges = tCommonManager.GetUserPrivilegesForEntity("MT_SystemMaintenanceConfiguration");

            ViewData["FormStatus"] = systemMaintenanceConfigurationDTO.ApprovalStatus;
            ViewBag.CurrencyList = Currency;
            return View(systemMaintenanceConfigurationDTO);
        }
        #endregion
        #region SystemMaintenanceConfigurationSave
        [HttpPost]
        [AuthorizeUserAttribute(Entity = "MT_SystemMaintenanceConfiguration", AccessLevel = new UserActivities[] { UserActivities.Add, UserActivities.Update })]
        public ActionResult SystemMaintenanceConfigurationSave(string Id, string Command, SystemMaintenanceConfigurationDTO ASystemMaintenanceConfigurationDTO)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (Command.Equals(Resources.Add))
                    {
                        if (string.IsNullOrWhiteSpace(tMasterManager.FindByBaseCurrency(ASystemMaintenanceConfigurationDTO.BaseCurrency).BaseCurrency))
                        {
                            ASystemMaintenanceConfigurationDTO.CreatedBy = tCommonManager.GetLoggedUserName();
                            ASystemMaintenanceConfigurationDTO.LastUpdatedBy = tCommonManager.GetLoggedUserName();
                            tMasterManager.SaveSystemMaintenanceConfigurationDetails(ASystemMaintenanceConfigurationDTO);
                            TempData["Message"] = string.Format(Utils.MSG_RECORD_SAVE_SUCESS);
                        }
                        else
                        {
                            TempData["Message"] = string.Format(Resources.MsgErrAlreadyExists, ASystemMaintenanceConfigurationDTO.BaseCurrency);
                        }
                    }
                    else if (Command.Equals(Resources.Save))
                    {
                        ASystemMaintenanceConfigurationDTO.LastUpdatedBy = tCommonManager.GetLoggedUserName();
                        tMasterManager.UpdateSystemMaintenanceConfigurationDetails(Id, ASystemMaintenanceConfigurationDTO);
                        TempData["Message"] = string.Format(Utils.MSG_RECORD_SAVE_SUCESS);
                    }
                    else if (Command.Equals(Resources.Delete))
                    {
                        try
                        {
                            tMasterManager.DeleteSystemMaintenanceConfiguration(Id);
                            TempData["Message"] = string.Format(Utils.MSG_RECORD_DELETE_SUCCESS);
                        }
                        catch (Exception ex)
                        {
                            LoggerFactory.CreateLog().LogError("Error" + ex.Message, 100, 2);
                            TempData["Message"] = string.Format(Resources.MsgErrDeleteConcurrencyError, Id);
                            return RedirectToAction("SystemMaintenanceConfigurationAdd", "master", new { Id = Id });
                        }
                    }
                }
            }

            catch (Exception ex)
            {
                LoggerFactory.CreateLog().LogError("Error" + ex.Message, 100, 2);
            }

            return ((Command.Equals(Resources.Add) || Command.Equals(Resources.Delete)) ? RedirectToAction("SystemMaintenanceConfigurationAdd", "Master") : RedirectToAction("SystemMaintenanceConfigurationAdd", "Master", new { Id = Id }));

        }
        #endregion
        #region SystemMaintenanceConfigurationDelete
        [HttpPost]
        [AuthorizeUserAttribute(Entity = "MT_SystemMaintenanceConfiguration", AccessLevel = new UserActivities[] { UserActivities.Delete })]
        public ActionResult SystemMaintenanceConfigurationDelete(string Id, SystemMaintenanceConfigurationDTO ASystemMaintenanceConfigurationDTO)
        {
            try
            {
                tMasterManager.DeleteSystemMaintenanceConfiguration(Id);
                var dResult = new { result = "success", action = "SystemMaintenanceConfigurationList", controller = "Master" };
                TempData["Message"] = string.Format(Utils.MSG_RECORD_DELETE_SUCCESS);
                return Json(dResult, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                var dResultFail = new { result = "fail", action = "SystemMaintenanceConfigurationList", controller = "Master" };
                TempData["Message"] = string.Format(Utils.MSG_RECORD_DELETE_CONCURRENCY_ERROR);
                return Json(dResultFail, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion
        #region SystemMaintenanceConfigurationList
        [AuthorizeUserAttribute(Entity = "MT_SystemMaintenanceConfiguration", AccessLevel = new UserActivities[] { UserActivities.View })]
        public ActionResult SystemMaintenanceConfigurationList(SystemMaintenanceConfigurationSearch AVMSystemMaintenanceConfigurationSearch, string ClearButton, int? page)
        {
            try
            {
                int iPageNumber = Convert.ToInt32((page == null) ? 0 : page);
                var Currency = tLookUpManager.GetAll<CurrencyDTO>("", "Currency");
                if (ClearButton == Resources.Clear)
                {
                    AVMSystemMaintenanceConfigurationSearch.BaseCurrency = null;

                    ModelState.Clear();
                }

                if (!(string.IsNullOrEmpty(AVMSystemMaintenanceConfigurationSearch.SearchButton)) &&
                     !((string.IsNullOrWhiteSpace(AVMSystemMaintenanceConfigurationSearch.BaseCurrency))))
                {
                    var entities = tMasterManager.FindAll(iPageNumber, new SystemMaintenanceConfigurationDTO()
                    {
                        BaseCurrency = AVMSystemMaintenanceConfigurationSearch.BaseCurrency,

                    });
                    AVMSystemMaintenanceConfigurationSearch.SearchResults = entities.AsQueryable();
                }
                else
                {
                    var currResult = tMasterManager.FindSystemMaintenanceConfiguration(1, Utils.GRD_ROWS_PER_PAGE).AsQueryable();

                    AVMSystemMaintenanceConfigurationSearch.SearchResults = currResult.AsQueryable();
                }
                ViewBag.CurrencyList = Currency;
                return View(AVMSystemMaintenanceConfigurationSearch);
            }
            catch (Exception ex)
            {

                LoggerFactory.CreateLog().LogError("Error" + ex.Message, 100, 2);
                return View("SystemMaintenanceConfigurationAdd");
            }
        }
        #endregion
        #region SystemMaintenanceConfigurationAuthorize
        [AuthorizeUserAttribute(Entity = "MT_SystemMaintenanceConfiguration", AccessLevel = new UserActivities[] { UserActivities.Authorize })]
        public ActionResult SystemMaintenanceConfigurationAuthorize(string Id, string AMode, string QId)
        {
            ViewBag.RequestQueueId = QId;
            ViewBag.TableName = "SYSMTNCNFIG";
            var Currency = tLookUpManager.GetAll<CurrencyDTO>("", "Currency");
            if (!String.IsNullOrWhiteSpace(Id))
            {
                SystemMaintenanceConfigurationDTO systemMaintenanceConfigurationDTO = tMasterManager.FindByBaseCurrency(Id);

                if (!String.IsNullOrWhiteSpace(systemMaintenanceConfigurationDTO.ApprovalStatus) && systemMaintenanceConfigurationDTO.ApprovalStatus.Equals("P"))
                {
                    var reasonCodes = tLookUpManager.GetAll<ApprovalContextDTO>("MTNCNFG", "ReasonContext");
                    ViewBag.ReasonCodes = reasonCodes;
                }

                ViewBag.Privileges = tCommonManager.GetUserPrivilegesForEntity("MT_SystemMaintenanceConfiguration");
                ViewData["FormCall"] = "ApprovalQueue";
                ViewData["FormStatus"] = systemMaintenanceConfigurationDTO.ApprovalStatus;
                ViewBag.CurrencyList = Currency;
                return View("systemMaintenanceConfigurationAdd", systemMaintenanceConfigurationDTO);
            }
            else
                return RedirectToAction("ApprovalQueueList", "Approval");
        }
        #endregion
        #endregion

        #region Boost Maintenance
        #region BoostMaintenanceView
        [AuthorizeUserAttribute(Entity = "MT_BOOST_MAINTENANCE", AccessLevel = new UserActivities[] { UserActivities.View })]
        public ActionResult BoostMaintenanceView(string Id)
        {
            try
            {
                BoostMaintenanceDTO boostMaintenanceDTO;
                ViewBag.TableName = "BOOST";

                var productList = tLookUpManager.GetAll<ProductDTO>(string.Empty, "Product");
                var transactionList = tLookUpManager.GetAll<TransactionDTO>(string.Empty, "Transaction");

                ViewBag.Privileges = tCommonManager.GetUserPrivilegesForEntity("MT_BOOST_MAINTENANCE");

                ViewBag.ProductList = productList;
                ViewBag.TransactionList = transactionList;

                if (String.IsNullOrWhiteSpace(Id))
                {
                    boostMaintenanceDTO = new BoostMaintenanceDTO();
                    ViewData["FormStatus"] = boostMaintenanceDTO.ApprovalStatus;
                    return View("BoostMaintenanceAdd", boostMaintenanceDTO);
                }
                else
                {
                    boostMaintenanceDTO = tMasterManager.FindByBoostId(Convert.ToInt32(Id));
                    ViewBag.ReasonCodes = null;
                }

                ViewData["FormStatus"] = boostMaintenanceDTO.ApprovalStatus;

                return View("BoostMaintenanceAdd", boostMaintenanceDTO);
            }
            catch (Exception ex)
            {
                LoggerFactory.CreateLog().LogError("Error" + ex.Message, 100, 2);
                ViewBag.ErrorMessage = Resources.MsgErrServerException;
                ViewBag.ErrorType = SessionErrorType.ServerException;
                return View("Error");
            }
        }
        #endregion
        #region BoostMaintenanceAdd
        [AuthorizeUserAttribute(Entity = "MT_BOOST_MAINTENANCE", AccessLevel = new UserActivities[] { UserActivities.Add, UserActivities.Update, UserActivities.View })]
        public ActionResult BoostMaintenanceAdd(string Id)
        {
            try
            {
                BoostMaintenanceDTO boostMaintenanceDTO;

                var productList = tLookUpManager.GetAll<ProductTypeDTO>(string.Empty, "Product");
                var transactionList = tLookUpManager.GetAll<TransactionDTO>(string.Empty, "Transaction");
                if (Id == null)
                {
                    boostMaintenanceDTO = new BoostMaintenanceDTO();
                }
                else
                {
                    boostMaintenanceDTO = tMasterManager.FindByBoostId(Convert.ToInt32(Id));
                }
                if (!String.IsNullOrWhiteSpace(boostMaintenanceDTO.ApprovalStatus) && boostMaintenanceDTO.ApprovalStatus.Equals("P"))
                {
                    var reasonCodes = tLookUpManager.GetAll<ApprovalContextDTO>("BOOST", "ReasonContext");
                    ViewBag.ReasonCodes = reasonCodes;
                }

                ViewBag.Privileges = tCommonManager.GetUserPrivilegesForEntity("MT_BOOST_MAINTENANCE");

                ViewData["FormStatus"] = boostMaintenanceDTO.ApprovalStatus;
                ViewBag.ProductList = productList;
                ViewBag.TransactionList = transactionList;
                return View(boostMaintenanceDTO);
            }
            catch (Exception ex)
            {
                LoggerFactory.CreateLog().LogError("Error" + ex.Message, 100, 2);
                ViewBag.ErrorMessage = Resources.MsgErrServerException;
                ViewBag.ErrorType = SessionErrorType.ServerException;
                return View("Error");
            }
        }
        #endregion
        #region BoostMaintenanceSave
        [HttpPost]
        [AuthorizeUserAttribute(Entity = "MT_BOOST_MAINTENANCE", AccessLevel = new UserActivities[] { UserActivities.Add, UserActivities.Update })]
        public ActionResult BoostMaintenanceSave(int Id, string Command, BoostMaintenanceDTO ABoostMaintenanceDTO)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    if (Command.Equals(Resources.Submit))
                        ABoostMaintenanceDTO.ApprovalStatus = "P";
                    else if (Command.Equals(Resources.Save))
                        ABoostMaintenanceDTO.ApprovalStatus = "D";

                    ABoostMaintenanceDTO.OrganizationCode = Utils.OrganizationCode;
                    if (Command.Equals(Resources.Submit) || Command.Equals(Resources.Save))
                    {
                        if (string.IsNullOrWhiteSpace(tMasterManager.FindByBoostId(Id).TransactionCode))
                        {
                            ABoostMaintenanceDTO.CreatedBy = tCommonManager.GetLoggedUserName();
                            ABoostMaintenanceDTO.LastUpdatedBy = tCommonManager.GetLoggedUserName();
                            tMasterManager.SaveBoostMaintenanceDetails(ABoostMaintenanceDTO);
                            TempData["Message"] = String.Format(Resources.MsgInfoSaveSuccess, String.Concat(new object[] { Resources.MT_BOOST_MAINTENANCE, " ", ABoostMaintenanceDTO.ProductCode + "-" + ABoostMaintenanceDTO.TransactionCode })); 
                        }
                        else
                        {
                            if (!string.IsNullOrWhiteSpace(Convert.ToString(Id)))
                            {
                                ABoostMaintenanceDTO.LastUpdatedBy = tCommonManager.GetLoggedUserName();
                                tMasterManager.UpdateBoostMaintenanceDetails(Convert.ToInt32(Id), ABoostMaintenanceDTO);
                                TempData["Message"] = String.Format(Resources.MsgInfoSaveSuccess, String.Concat(new object[] { Resources.MT_BOOST_MAINTENANCE, " ", ABoostMaintenanceDTO.ProductCode + "-" + ABoostMaintenanceDTO.TransactionCode })); 
                            }
                            else
                            {
                                TempData["Message"] = String.Format(Resources.MsgErrAlreadyExists, String.Concat(new object[] { Resources.MT_BOOST_MAINTENANCE, " ", ABoostMaintenanceDTO.ProductCode + "-" + ABoostMaintenanceDTO.TransactionCode })); 
                            }
                        }
                    }
                    else if (Command.Equals(Resources.Delete))
                    {
                        try
                        {
                            ABoostMaintenanceDTO.LastUpdatedBy = tCommonManager.GetLoggedUserName();
                            ABoostMaintenanceDTO.ApprovalStatus = Utils.GetApprovalStatusCode(Utils.ApprovalStatus.Deleted);
                            tMasterManager.UpdateBoostMaintenanceDetails(Convert.ToInt32(Id), ABoostMaintenanceDTO);
                            TempData["Message"] = String.Format(Resources.MsgInfoDeleteSuccess, String.Concat(new object[] { Resources.MT_BOOST_MAINTENANCE, " ", ABoostMaintenanceDTO.ProductCode + "-" + ABoostMaintenanceDTO.TransactionCode })); 
                        }
                        catch (Exception ex)
                        {
                            LoggerFactory.CreateLog().LogError("Error" + ex.Message, 100, 2);
                            TempData["Message"] = String.Format(Resources.MsgErrDeleteConcurrencyError, String.Concat(new object[] { Resources.MT_BOOST_MAINTENANCE, " ", ABoostMaintenanceDTO.ProductCode + "-" + ABoostMaintenanceDTO.TransactionCode })); 
                            return RedirectToAction("BoostMaintenanceAdd", "master", new { Id = Id });
                        }
                    }
                }
            }

            catch (Exception ex)
            {
                LoggerFactory.CreateLog().LogError("Error" + ex.Message, 100, 2);
                ViewBag.ErrorMessage = Resources.MsgErrServerException;
                ViewBag.ErrorType = SessionErrorType.ServerException;
                return View("Error");
            }

            return ((Command.Equals(Resources.Submit) || Command.Equals(Resources.Delete)) ? RedirectToAction("BoostMaintenanceAdd", "Master") : RedirectToAction("BoostMaintenanceAdd", "Master", new { Id = Id }));

        }
        #endregion
        #region BoostMaintenanceDelete
        [HttpPost]
        [AuthorizeUserAttribute(Entity = "MT_BOOST_MAINTENANCE", AccessLevel = new UserActivities[] { UserActivities.Delete })]
        public ActionResult BoostMaintenanceDelete(string Id, BoostMaintenanceDTO ABoostMaintenanceDTO)
        {
            try
            {
                ABoostMaintenanceDTO = tMasterManager.FindByBoostId(Convert.ToInt32(Id));
                ABoostMaintenanceDTO.LastUpdatedBy = tCommonManager.GetLoggedUserName();
                ABoostMaintenanceDTO.ApprovalStatus = Utils.GetApprovalStatusCode(Utils.ApprovalStatus.Deleted);
                ABoostMaintenanceDTO.OrganizationCode = Utils.OrganizationCode;
                tMasterManager.UpdateBoostMaintenanceDetails(Convert.ToInt32(Id), ABoostMaintenanceDTO);

                var dResult = new { result = "success", action = "BoostMaintenanceList", controller = "Master" };
                TempData["Message"] = String.Format(Resources.MsgInfoDeleteSuccess, String.Concat(new object[] { Resources.MT_BOOST_MAINTENANCE, " ", ABoostMaintenanceDTO.ProductCode + "-" + ABoostMaintenanceDTO.TransactionCode })); 
                return Json(dResult, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                var dResultFail = new { result = "fail", action = "BoostMaintenanceList", controller = "Master" };
                TempData["Message"] = String.Format(Resources.MsgErrDeleteConcurrencyError, String.Concat(new object[] { Resources.MT_BOOST_MAINTENANCE, " ", ABoostMaintenanceDTO.ProductCode + "-" + ABoostMaintenanceDTO.TransactionCode })); 
                return Json(dResultFail, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion
        #region BoostMaintenanceList
        [AuthorizeUserAttribute(Entity = "MT_BOOST_MAINTENANCE", AccessLevel = new UserActivities[] { UserActivities.View })]
        public ActionResult BoostMaintenanceList(BoostMaintenanceSearch AVMBoostMaintenanceSearch, string ClearButton, int? page)
        {
            try
            {
                int iPageNumber = Convert.ToInt32((page == null) ? 0 : page);

                var productList = tLookUpManager.GetAll<ProductTypeDTO>(string.Empty, "Product");
                var transactionList = tLookUpManager.GetAll<TransactionDTO>(string.Empty, "Transaction");

                if (ClearButton == Resources.Clear)
                {
                    AVMBoostMaintenanceSearch.ProductCode = null;
                    AVMBoostMaintenanceSearch.TransactionCode = null;
                    ModelState.Clear();
                }

                if ((Utils.OrganizationCode != string.Empty) || (!(string.IsNullOrEmpty(AVMBoostMaintenanceSearch.SearchButton)) &&
                    !((string.IsNullOrWhiteSpace(AVMBoostMaintenanceSearch.ProductCode)) &&
                     (string.IsNullOrWhiteSpace(AVMBoostMaintenanceSearch.TransactionCode)))))
                {
                    var entities = tMasterManager.FindAll(iPageNumber, new BoostMaintenanceDTO()
                    {
                        ProductCode = AVMBoostMaintenanceSearch.ProductCode,
                        TransactionCode = AVMBoostMaintenanceSearch.TransactionCode
                    });
                    AVMBoostMaintenanceSearch.SearchResults = entities.AsQueryable();
                }
                else
                {
                    var currResult = tMasterManager.FindBoostMaintenance(1, Utils.GRD_ROWS_PER_PAGE).AsQueryable();
                    AVMBoostMaintenanceSearch.SearchResults = currResult.AsQueryable();
                }

                ViewBag.ProductList = productList;
                ViewBag.TransactionList = transactionList;
                ViewBag.Privileges = tCommonManager.GetUserPrivilegesForEntity("MT_BOOST_MAINTENANCE");
                return View(AVMBoostMaintenanceSearch);
            }
            catch (Exception ex)
            {
                LoggerFactory.CreateLog().LogError("Error" + ex.Message, 100, 2);
                ViewBag.ErrorMessage = Resources.MsgErrServerException;
                ViewBag.ErrorType = SessionErrorType.ServerException;
                return View("Error");
            }
        }
        #endregion
        #region BoostMaintenanceAuthorize
        [AuthorizeUserAttribute(Entity = "MT_BOOST_MAINTENANCE", AccessLevel = new UserActivities[] { UserActivities.Authorize })]
        public ActionResult BoostMaintenanceAuthorize(string Id, string AMode, string QId)
        {
            ViewBag.RequestQueueId = QId;
            ViewBag.TableName = "BOOST";
            if (!String.IsNullOrWhiteSpace(Id))
            {
                BoostMaintenanceDTO boostMaintenanceDTO = tMasterManager.FindByBoostId(Convert.ToInt32(Id));
                var productList = tLookUpManager.GetAll<ProductTypeDTO>(string.Empty, "Product");
                var transactionList = tLookUpManager.GetAll<TransactionDTO>(string.Empty, "Transaction");
                if (!String.IsNullOrWhiteSpace(boostMaintenanceDTO.ApprovalStatus) && boostMaintenanceDTO.ApprovalStatus.Equals("P"))
                {
                    var reasonCodes = tLookUpManager.GetAll<ApprovalContextDTO>("BOOST", "ReasonContext");
                    ViewBag.ReasonCodes = reasonCodes;
                }

                ViewBag.Privileges = tCommonManager.GetUserPrivilegesForEntity("MT_BOOST_MAINTENANCE");
                ViewData["FormCall"] = "ApprovalQueue";
                ViewData["FormStatus"] = boostMaintenanceDTO.ApprovalStatus;
                ViewBag.ProductList = productList;
                ViewBag.TransactionList = transactionList;
                return View("BoostMaintenanceAdd", boostMaintenanceDTO);
            }
            else
                return RedirectToAction("ApprovalQueueList", "Approval");
        }
        #endregion
        #endregion

        #region Fee
        #region FeeView
        [AuthorizeUserAttribute(Entity = "MT_FEE", AccessLevel = new UserActivities[] { UserActivities.View })]
        public ActionResult FeeView(string AFeeCode, string AOrganizationCode)
        {
            try
            {
                FeeDTO FeeDTO;
                ViewBag.TableName = "FEE";

                var productList = tLookUpManager.GetAll<ProductDTO>(string.Empty, "Product");
                var customerSegmentList = tLookUpManager.GetAll<CustomerSegmentDTO>(string.Empty, "CustomerSegment");
                var customerSubSegmentList = tLookUpManager.GetAll<CustomerSubSegmentDTO>(string.Empty, "CustomerSubSegment");
                var finActivityList = tLookUpManager.GetAll<FinancialActivityDTO>(string.Empty, "FinancialActivity");
                var feeTypeList = tLookUpManager.GetAll<SystemParameterDTO>(string.Empty, "FeeType");
                var currencyList = tLookUpManager.GetAll<SystemParameterDTO>(string.Empty, "Currency");

                ViewBag.Privileges = tCommonManager.GetUserPrivilegesForEntity("MT_FEE");

                ViewBag.ProductList = productList;
                ViewBag.CustomerSegmentList = customerSegmentList;
                ViewBag.CustomerSubSegmentList = customerSubSegmentList;
                ViewBag.FinActivityList = finActivityList;
                ViewBag.FeeTypeList = feeTypeList;
                ViewBag.CurrencyList = currencyList;

                if (String.IsNullOrWhiteSpace(AFeeCode))
                {
                    FeeDTO = new FeeDTO();
                    // FeeDTO.OrganizationCode = Utils.OrganizationCode;
                }
                else
                {
                    FeeDTO = tMasterManager.FindByFeeCode(AFeeCode, AOrganizationCode);
                    ViewBag.ReasonCodes = null;
                }
                ViewData["FormStatus"] = FeeDTO.ApprovalStatus;
                ViewBag.Fee = new JavaScriptSerializer().Serialize(FeeDTO);

                return View("FeeAdd", FeeDTO);
            }
            catch (Exception ex)
            {
                LoggerFactory.CreateLog().LogError("Error" + ex.Message, 100, 2);
                ViewBag.ErrorMessage = Resources.MsgErrServerException;
                ViewBag.ErrorType = SessionErrorType.ServerException;
                return View("Error");
            }
        }
        #endregion
        #region FeeAdd
        [AuthorizeUserAttribute(Entity = "MT_FEE", AccessLevel = new UserActivities[] { UserActivities.Add, UserActivities.Update, UserActivities.View })]
        public ActionResult FeeAdd(string AFeeCode, string AOrganizationCode)
        {
            try
            {
                FeeDTO feeDTO;

                //var productMemberTypeList = tLookUpManager.GetAll<MemberTypeDTO>(string.Empty, "ProductMemberType");
                var productList = tLookUpManager.GetAll<ProductDTO>(string.Empty, "Product");
                var customerSegmentList = tLookUpManager.GetAll<CustomerSegmentDTO>(string.Empty, "CustomerSegment");
                var customerSubSegmentList = tLookUpManager.GetAll<CustomerSubSegmentDTO>(string.Empty, "CustomerSubSegment");
                var finActivityList = tLookUpManager.GetAll<FinancialActivityDTO>(string.Empty, "FinancialActivity");
                var feeTypeList = tLookUpManager.GetAll<SystemParameterDTO>(string.Empty, "FeeType");
                var currencyList = tLookUpManager.GetAll<SystemParameterDTO>(string.Empty, "Currency");

                if (AFeeCode == null)
                {
                    feeDTO = new FeeDTO();
                    //  feeDTO.OrganizationCode = AOrganizationCode;
                }
                else
                {
                    feeDTO = tMasterManager.FindByFeeCode(AFeeCode, AOrganizationCode);
                }

                if (!String.IsNullOrWhiteSpace(feeDTO.ApprovalStatus) && feeDTO.ApprovalStatus.Equals("P"))
                {
                    var reasonCodes = tLookUpManager.GetAll<ApprovalContextDTO>("FEE", "ReasonContext");
                    ViewBag.ReasonCodes = reasonCodes;
                }
                ViewBag.Privileges = tCommonManager.GetUserPrivilegesForEntity("MT_FEE");
                ViewData["FormStatus"] = feeDTO.ApprovalStatus;

                ViewBag.ProductList = productList;
                ViewBag.CustomerSegmentList = customerSegmentList;
                ViewBag.CustomerSubSegmentList = customerSubSegmentList;
                ViewBag.FinActivityList = finActivityList;
                ViewBag.FeeTypeList = feeTypeList;
                ViewBag.CurrencyList = currencyList;
                ViewBag.Fee = new JavaScriptSerializer().Serialize(feeDTO);

                return View(feeDTO);
            }
            catch (Exception ex)
            {
                LoggerFactory.CreateLog().LogError("Error" + ex.Message, 100, 2);
                ViewBag.ErrorMessage = Resources.MsgErrServerException;
                ViewBag.ErrorType = SessionErrorType.ServerException;
                return View("Error");
            }
        }
        #endregion
        #region FeeSave
        [AuthorizeUserAttribute(Entity = "MT_FEE", AccessLevel = new UserActivities[] { UserActivities.Add, UserActivities.Update })]
        public ActionResult FeeSave(string AFeeCode, string AOrganizationCode, FeeDTO AFeeDTO, string ACommand)
        {
            AOrganizationCode = Utils.OrganizationCode;
            string redirectUrl = string.Empty, messageToShow = string.Empty;
            try
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors);

                if (ModelState.IsValid)
                {
                    if (ACommand == Resources.Save)
                    {
                        AFeeDTO.ApprovalStatus = "D";
                    }
                    else if (ACommand == Resources.Submit)
                    {
                        AFeeDTO.ApprovalStatus = "P";
                    }

                    var duplicateFeeType = AFeeDTO.FeeTypeDTO.GroupBy(x => x.FeeTypeCode).Where(x => x.Count() > 1).Select(x => new { feeTypeCode = x.Key });
                    if (duplicateFeeType.Any())
                    {
                        string message = string.Empty;

                        duplicateFeeType.ToList().ForEach(x =>
                            message = Convert.ToString(message +
                            (message.Length > 0 ? "," : string.Empty) + "'" + tMasterManager.GetFeeTypes(x.feeTypeCode).FeeTypeDescription) + "'");
                        messageToShow = String.Format(Resources.MsgErrAlreadyExists, String.Concat(new object[] { Resources.FeeType, " ", message})); 
                    }
                    else
                    {
                        if (string.IsNullOrWhiteSpace(tMasterManager.FindByFeeCode(AFeeDTO.FeeCode, AOrganizationCode).FeeCode))
                        {
                            AFeeDTO.CreatedBy = tCommonManager.GetLoggedUserName();
                            AFeeDTO.LastUpdatedBy = tCommonManager.GetLoggedUserName();
                            AFeeDTO.OrganizationCode = Utils.OrganizationCode;
                            tMasterManager.SaveFeeDetails(AFeeDTO);
                            messageToShow = String.Format(Resources.MsgInfoSaveSuccess, String.Concat(new object[] { Resources.FeeCode, " ", AFeeDTO.FeeCode })); 
                            redirectUrl = Url.Action("FeeAdd", "Master");//New Mode
                        }
                        else
                        {
                            if (!string.IsNullOrWhiteSpace(AFeeCode))
                            {
                                {
                                    AFeeDTO.LastUpdatedBy = tCommonManager.GetLoggedUserName();
                                    tMasterManager.UpdateFeeDetails(AFeeCode, AOrganizationCode, AFeeDTO);
                                    messageToShow = String.Format(Resources.MsgInfoSaveSuccess, String.Concat(new object[] { Resources.FeeCode, " ", AFeeDTO.FeeCode })); 
                                    if (ACommand.Equals(Resources.Save))
                                    {
                                        redirectUrl = Url.Action("FeeAdd", "Master", new { AFeeCode = AFeeCode, AOrganizationCode = AOrganizationCode });//Update mode
                                    }
                                    else
                                    {
                                        redirectUrl = Url.Action("FeeAdd", "Master");//New Mode
                                    }
                                }
                            }
                            else
                            {
                                messageToShow = String.Format(Resources.MsgErrAlreadyExists, String.Concat(new object[] { Resources.FeeCode, " ", AFeeDTO.FeeCode })); 
                                redirectUrl = Url.Action("FeeAdd", "Master");//New Mode
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LoggerFactory.CreateLog().LogError("Error" + ex.Message, 100, 2);
                messageToShow = string.Format(Resources.MsgErrSaveFailure);
                redirectUrl = Url.Action("FeeAdd", "Master");
            }
            return Json(new { RedirectUrl = redirectUrl, Message = messageToShow }, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region FeeList
        [AuthorizeUserAttribute(Entity = "MT_FEE", AccessLevel = new UserActivities[] { UserActivities.View })]
        public ActionResult FeeList(FeeSearch AVMFeeSearch, string ClearButton, int? page)
        {
            try
            {
                int iPageNumber = Convert.ToInt32((page == null) ? 0 : page);

                var productList = tLookUpManager.GetAll<ProductDTO>(string.Empty, "Product");
                var customerSegmentList = tLookUpManager.GetAll<CustomerSegmentDTO>(string.Empty, "CustomerSegment");
                var customerSubSegmentList = tLookUpManager.GetAll<CustomerSubSegmentDTO>(string.Empty, "CustomerSubSegment");

                if (ClearButton == Resources.Clear)
                {
                    AVMFeeSearch.FeeCode = null;
                    AVMFeeSearch.ProductCode = null;
                    AVMFeeSearch.CustomerSegmentCode = null;
                    AVMFeeSearch.CustomerSubSegmentCode = null;
                    ModelState.Clear();
                }

                if ((Utils.OrganizationCode != "") ||
                    (!(string.IsNullOrEmpty(AVMFeeSearch.SearchButton)) &&
                     !((string.IsNullOrWhiteSpace(AVMFeeSearch.FeeCode) &&
                     (string.IsNullOrWhiteSpace(AVMFeeSearch.ProductCode) &&
                     (string.IsNullOrWhiteSpace(AVMFeeSearch.CustomerSegmentCode) &&
                     (string.IsNullOrWhiteSpace(AVMFeeSearch.CustomerSubSegmentCode))))))))
                {
                    var entities = tMasterManager.FindAll(iPageNumber, new FeeDTO()
                    {
                        FeeCode = AVMFeeSearch.FeeCode,
                        ProductCode = AVMFeeSearch.ProductCode,
                        CustomerSegmentCode = AVMFeeSearch.CustomerSegmentCode,
                        CustomerSubSegmentCode = AVMFeeSearch.CustomerSubSegmentCode
                    });
                    AVMFeeSearch.SearchResults = entities.AsQueryable();
                }
                else
                {
                    var currResult = tMasterManager.FindFee(1, Utils.GRD_ROWS_PER_PAGE).AsQueryable();
                    AVMFeeSearch.SearchResults = currResult.AsQueryable();
                }
                ViewBag.ProductList = productList;
                ViewBag.CustomerSegmentList = customerSegmentList;
                ViewBag.CustomerSubSegmentList = customerSubSegmentList;
                ViewBag.Privileges = tCommonManager.GetUserPrivilegesForEntity("MT_FEE");
                return View(AVMFeeSearch);
            }
            catch (Exception ex)
            {
                LoggerFactory.CreateLog().LogError("Error" + ex.Message, 100, 2);
                ViewBag.ErrorMessage = Resources.MsgErrServerException;
                ViewBag.ErrorType = SessionErrorType.ServerException;
                return View("Error");
            }
        }
        #endregion
        #region FeeDelete
        [HttpPost]
        [AuthorizeUserAttribute(Entity = "MT_FEE", AccessLevel = new UserActivities[] { UserActivities.Delete })]
        public ActionResult FeeDelete(string AFeeCode, string AOrganizationCode, bool AIsDeleteCallFromSearchTab = false)
        {
            FeeDTO objFeeDTO = new FeeDTO();
            string redirectUrl = string.Empty, customerName = string.Empty;
            try
            {
                objFeeDTO = tMasterManager.FindByFeeCode(AFeeCode, AOrganizationCode);

                objFeeDTO.LastUpdatedBy = tCommonManager.GetLoggedUserName();
                objFeeDTO.ApprovalStatus = Utils.GetApprovalStatusCode(Utils.ApprovalStatus.Deleted);
                tMasterManager.UpdateFeeDetails(AFeeCode, Utils.OrganizationCode, objFeeDTO);
                if (AIsDeleteCallFromSearchTab)
                {
                    TempData["Message"] = String.Format(Resources.MsgInfoDeleteSuccess, String.Concat(new object[] { Resources.Fee, " ", AFeeCode })); 
                    redirectUrl = Url.Action("FeeList");
                }
                else
                {
                    TempData["Message"] = string.Empty;
                    redirectUrl = Url.Action("FeeAdd");
                }
            }
            catch (Exception ex)
            {
                LoggerFactory.CreateLog().LogError("Error" + ex.Message, 100, 2);
                if (AIsDeleteCallFromSearchTab)
                {
                    TempData["Message"] = String.Format(Resources.MsgErrDeleteConcurrencyError, String.Concat(new object[] { Resources.Fee, " ", AFeeCode })); 
                    redirectUrl = Url.Action("FeeList", "Master");
                }
                else
                {
                    TempData["Message"] = string.Empty;
                    redirectUrl = Url.Action("FeeAdd", new { AFeeCode = AFeeCode });
                }
                return Json(new { RedirectUrl = redirectUrl, Success = false, Message = String.Format(Resources.MsgErrDeleteConcurrencyError, String.Concat(new object[] { Resources.Fee, " ", AFeeCode }))  }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { RedirectUrl = redirectUrl, Success = true, Message = String.Format(Resources.MsgInfoDeleteSuccess, String.Concat(new object[] { Resources.Fee, " ", AFeeCode })) }, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region FeeAuthorize
        [AuthorizeUserAttribute(Entity = "MT_FEE", AccessLevel = new UserActivities[] { UserActivities.Authorize })]
        public ActionResult FeeAuthorize(string AFeeCode, string AOrganizationCode, string AMode, string QId)
        {
            ViewBag.RequestQueueId = QId;
            ViewBag.TableName = "FEE";
            if (!String.IsNullOrWhiteSpace(AFeeCode))
            {
                FeeDTO feeDTO = tMasterManager.FindByFeeCode(AFeeCode, AOrganizationCode);

                //var productMemberTypeList = tLookUpManager.GetAll<MemberTypeDTO>(string.Empty, "ProductMemberType");
                var productList = tLookUpManager.GetAll<ProductDTO>(string.Empty, "Product");
                var customerSegmentList = tLookUpManager.GetAll<CustomerSegmentDTO>(string.Empty, "CustomerSegment");
                var customerSubSegmentList = tLookUpManager.GetAll<CustomerSubSegmentDTO>(string.Empty, "CustomerSubSegment");
                var finActivityList = tLookUpManager.GetAll<FinancialActivityDTO>(string.Empty, "FinancialActivity");
                var feeTypeList = tLookUpManager.GetAll<SystemParameterDTO>(string.Empty, "FeeType");
                var currencyList = tLookUpManager.GetAll<SystemParameterDTO>(string.Empty, "Currency");

                if (!String.IsNullOrWhiteSpace(feeDTO.ApprovalStatus) && feeDTO.ApprovalStatus.Equals("P"))
                {
                    var reasonCodes = tLookUpManager.GetAll<ApprovalContextDTO>("FEE", "ReasonContext");
                    ViewBag.ReasonCodes = reasonCodes;
                }

                ViewBag.Privileges = tCommonManager.GetUserPrivilegesForEntity("MT_FEE");
                ViewData["FormCall"] = "ApprovalQueue";
                ViewData["FormStatus"] = feeDTO.ApprovalStatus;

                ViewBag.ProductList = productList;
                ViewBag.CustomerSegmentList = customerSegmentList;
                ViewBag.CustomerSubSegmentList = customerSubSegmentList;
                //ViewBag.ProductMemberTypeList = productMemberTypeList;
                ViewBag.FinActivityList = finActivityList;
                ViewBag.FeeTypeList = feeTypeList;
                ViewBag.Fee = new JavaScriptSerializer().Serialize(feeDTO);
                ViewBag.CurrencyList = currencyList;

                return View("FeeAdd", feeDTO);
            }
            else
                return RedirectToAction("ApprovalQueueList", "Approval");
        }
        #endregion
        #endregion

        #region Product

        #region ProductView
        [AuthorizeUserAttribute(Entity = "MT_PRODUCT", AccessLevel = new UserActivities[] { UserActivities.View })]
        public ActionResult ProductView(string AProductCode, string AOrganizationCode)
        {
            try
            {
                ProductDTO ProductDTO;
                ViewBag.TableName = "PDT";

                var currencyCode = tLookUpManager.GetAll<CurrencyDTO>("", "Currency");
                var billingCycleCode = tLookUpManager.GetAll<BillingCycleDTO>("", "BillingCycle");
                //var defaultAffinityCode = tLookUpManager.GetAll<DefaultAffinityDTO>("", "DefaultAffinity");
                var interestMethodCode = tLookUpManager.GetAll<InterestMethodDTO>("", "InterestMethod", false);
                var productClassCode = tLookUpManager.GetAll<ProductClassDTO>("", "ProductClass");
                var productMemberTypeCode = tLookUpManager.GetAll<ProductMemberTypeDTO>("", "ProductMemberType");
                var productStatusCode = tLookUpManager.GetAll<ProductStatusDTO>("", "SysParamProductStatus");
                var productCategoryCode = tLookUpManager.GetAll<ProductCategoryDTO>("", "ProductCategory");
                var productSubCategoryCode = tLookUpManager.GetAll<ProductSubCategoryDTO>("", "ProductSubCategory");
                var serviceCode = tLookUpManager.GetAll<ServiceDTO>("", "Service");
                var productFranchiseCode = tLookUpManager.GetAll<FranchiseDTO>("", "ProductFranchise");
                var paymentAllocationSequence = tLookUpManager.GetAll<CreditSegmentDTO>("", "CreditSegment");
                var revolverChargeFlag = Utils.GetRevolverChargeFlag();
                var sysParamAvailCashLimit = tLookUpManager.GetAll<SystemParameterDTO>("", "SysParamAvlCashLimit");
                var sysParamCardNoLen = tLookUpManager.GetAll<SystemParameterDTO>("", "SysParamCardNoLen");
                var sysParamCardNoPattern = tLookUpManager.GetAll<SystemParameterDTO>("", "SysParamCardNoPattern");

                var contractList = tLookUpManager.GetAll<ContractDTO>("", "Contract");
                ViewBag.ContractList = contractList;

                ViewBag.BillingCycle = billingCycleCode;
                //ViewBag.DefaultAffinity = defaultAffinityCode;
                ViewBag.InterestMethod = interestMethodCode;
                ViewBag.ProductClass = productClassCode;
                ViewBag.ProductMemberType = productMemberTypeCode;
                ViewBag.ProductStatus = productStatusCode;
                ViewBag.ProductCategory = productCategoryCode;
                ViewBag.ProductSubCategory = productSubCategoryCode;
                ViewBag.Service = serviceCode;
                ViewBag.Currency = currencyCode;
                ViewBag.ProductFranchise = productFranchiseCode;
                ViewBag.PaymentAllocationSequenceList = paymentAllocationSequence;
                ViewBag.RevolverChargeFlagList = revolverChargeFlag;
                ViewBag.SysParamAvailCashLimit = sysParamAvailCashLimit;
                ViewBag.SysParamCardNoLen = sysParamCardNoLen;
                ViewBag.SysParamCardNoPattern = sysParamCardNoPattern;

                ViewBag.Privileges = tCommonManager.GetUserPrivilegesForEntity("MT_PRODUCT");

                if (String.IsNullOrWhiteSpace(AProductCode))
                {
                    ProductDTO = new ProductDTO();
                    //   ProductDTO.OrganizationCode = Utils.OrganizationCode;
                    ViewData["FormStatus"] = ProductDTO.ApprovalStatus;
                    ProductDTO.AssignedList = new List<String>();
                    ProductDTO.MemberTypeAssignedList = new List<String>();
                    return View("ProductAdd", ProductDTO);
                }
                else
                {

                    ProductDTO = tMasterManager.FindByProductCode(Convert.ToString(AProductCode), AOrganizationCode);

                    // Added to set datasource for the selected product memeber types listbox
                    List<SelectListItem> lstSelectedPTypes = new List<SelectListItem>();
                    var ProductTypeSelectedItems = ProductDTO.ProductMemberTypeCode;
                    string[] PSelectedItems = ProductTypeSelectedItems.Split(',');
                    foreach (string items in PSelectedItems)
                    {
                        List<SelectListItem> lst = new List<SelectListItem>();
                        lst = tLookUpManager.GetAll<ProductMemberTypeDTO>(items, "ProductMemberTypeByCode");
                        lst.RemoveAt(0);
                        lstSelectedPTypes = lstSelectedPTypes.Union(lst).ToList();

                    }
                    ViewBag.SelectedProductMemberTypesitem = lstSelectedPTypes;
                    // Ended

                    ViewBag.ReasonCodes = null;
                }

                ViewData["FormStatus"] = ProductDTO.ApprovalStatus;

                return View("ProductAdd", ProductDTO);
            }
            catch (Exception ex)
            {
                LoggerFactory.CreateLog().LogError("Error" + ex.Message, 100, 2);
                ViewBag.ErrorMessage = Resources.MsgErrServerException;
                ViewBag.ErrorType = SessionErrorType.ServerException;
                return View("Error");
            }
        }
        #endregion
        #region ProductAdd
        [AuthorizeUserAttribute(Entity = "MT_PRODUCT", AccessLevel = new UserActivities[] { UserActivities.Add, UserActivities.Update, UserActivities.View })]
        public ActionResult ProductAdd(string AProductCode, string AOrganizationCode)
        {
            try
            {
                ProductDTO productDTO;

                var currencyCode = tLookUpManager.GetAll<CurrencyDTO>("", "Currency");
                var billingCycleCode = tLookUpManager.GetAll<BillingCycleDTO>("", "BillingCycle");
                //var defaultAffinityCode = tLookUpManager.GetAll<DefaultAffinityDTO>("", "DefaultAffinity");
                var interestMethodCode = tLookUpManager.GetAll<InterestMethodDTO>("", "InterestMethod", false);
                var productClassCode = tLookUpManager.GetAll<ProductClassDTO>("", "ProductClass");
                var productMemberTypeCode = tLookUpManager.GetAll<ProductMemberTypeDTO>("", "ProductMemberType");
                var productStatusCode = tLookUpManager.GetAll<ProductStatusDTO>("", "SysParamProductStatus");
                var productCategoryCode = tLookUpManager.GetAll<ProductCategoryDTO>("", "ProductCategory");
                var productSubCategoryCode = tLookUpManager.GetAll<ProductSubCategoryDTO>("", "ProductSubCategory");
                var serviceCode = tLookUpManager.GetAll<ServiceDTO>("", "Service");
                var productFranchiseCode = tLookUpManager.GetAll<FranchiseDTO>("", "ProductFranchise");
                var paymentAllocationSequence = tLookUpManager.GetAll<CreditSegmentDTO>("", "CreditSegment");
                var revolverChargeFlag = Utils.GetRevolverChargeFlag();
                var sysParamAvailCashLimit = tLookUpManager.GetAll<SystemParameterDTO>("", "SysParamAvlCashLimit");
                var sysParamCardNoLen = tLookUpManager.GetAll<SystemParameterDTO>("", "SysParamCardNoLen");
                var sysParamCardNoPattern = tLookUpManager.GetAll<SystemParameterDTO>("", "SysParamCardNoPattern");

                var contractList = tLookUpManager.GetAll<ContractDTO>("", "Contract");
                ViewBag.ContractList = contractList;

                if (String.IsNullOrWhiteSpace(AProductCode))
                {
                    productDTO = new ProductDTO();
                    productDTO.OrganizationCode = Utils.OrganizationCode;
                    productDTO.AssignedList = new List<String>();
                    productDTO.MemberTypeAssignedList = new List<String>();
                    ViewBag.SelectedProductMemberTypesitem = null;
                }
                else
                {
                    productDTO = tMasterManager.FindByProductCode(Convert.ToString(AProductCode), AOrganizationCode);

                    // Added to set datasource for the selected product memeber types listbox
                    List<SelectListItem> lstSelectedPTypes = new List<SelectListItem>();
                    var ProductTypeSelectedItems = productDTO.ProductMemberTypeCode;
                    string[] PSelectedItems = ProductTypeSelectedItems.Split(',');
                    foreach (string items in PSelectedItems)
                    {
                        List<SelectListItem> lst = new List<SelectListItem>();
                        lst = tLookUpManager.GetAll<ProductMemberTypeDTO>(items, "ProductMemberTypeByCode");
                        lst.RemoveAt(0);
                        lstSelectedPTypes = lstSelectedPTypes.Union(lst).ToList();
                    }
                    ViewBag.SelectedProductMemberTypesitem = lstSelectedPTypes;
                    // Ended
                }

                if (!String.IsNullOrWhiteSpace(productDTO.ApprovalStatus) && productDTO.ApprovalStatus.Equals("P"))
                {
                    var reasonCodes = tLookUpManager.GetAll<ApprovalContextDTO>("PDT", "ReasonContext");
                    ViewBag.ReasonCodes = reasonCodes;
                }

                ViewBag.Privileges = tCommonManager.GetUserPrivilegesForEntity("MT_PRODUCT");

                ViewData["FormStatus"] = productDTO.ApprovalStatus;

                ViewBag.BillingCycle = billingCycleCode;
                //ViewBag.DefaultAffinity = defaultAffinityCode;
                ViewBag.InterestMethod = interestMethodCode;
                ViewBag.ProductClass = productClassCode;
                ViewBag.ProductMemberType = productMemberTypeCode;
                ViewBag.ProductStatus = productStatusCode;
                ViewBag.ProductCategory = productCategoryCode;
                ViewBag.ProductSubCategory = productSubCategoryCode;
                ViewBag.Service = serviceCode;
                ViewBag.Currency = currencyCode;
                ViewBag.ProductFranchise = productFranchiseCode;
                ViewBag.PaymentAllocationSequenceList = paymentAllocationSequence;
                ViewBag.RevolverChargeFlagList = revolverChargeFlag;

                ViewBag.SelectedProductMemberType = productMemberTypeCode;
                ViewBag.SysParamAvailCashLimit = sysParamAvailCashLimit;
                ViewBag.SysParamCardNoLen = sysParamCardNoLen;
                ViewBag.SysParamCardNoPattern = sysParamCardNoPattern;


                if (productDTO.ApprovalStatus == "DEL" || productDTO.ApprovalStatus == "DIS")
                {
                    productDTO = new ProductDTO();
                    productDTO.OrganizationCode = Utils.OrganizationCode;
                    productDTO.AssignedList = new List<String>();
                    productDTO.MemberTypeAssignedList = new List<String>();
                    ViewBag.SelectedProductMemberTypesitem = null;
                    ViewData["FormStatus"] = productDTO.ApprovalStatus;
                }

                return View(productDTO);
            }
            catch (Exception ex)
            {
                LoggerFactory.CreateLog().LogError("Error" + ex.Message, 100, 2);
                ViewBag.ErrorMessage = Resources.MsgErrServerException;
                ViewBag.ErrorType = SessionErrorType.ServerException;
                return View("Error");
            }
        }
        #endregion
        #region ProductSave


        [HttpPost]
        [AuthorizeUserAttribute(Entity = "MT_PRODUCT", AccessLevel = new UserActivities[] { UserActivities.Add, UserActivities.Update, UserActivities.Delete })]
        public ActionResult ProductSave(string AProductCode, string AOrganizationCode, string Command, ProductDTO AProductDTO)
        {
            try
            {
                var List = AProductDTO.AssignedList;
                //Sheeba:11-9-2015
                if (Command.Equals(Resources.Submit) == true && List == null)
                {
                    tMasterManager.UpdateProductMaximumCreditLine(AProductCode, AOrganizationCode, AProductDTO.MaximumCreditLine);
                    TempData["Message"] = String.Format(Resources.MsgInfoSaveSuccess, String.Concat(new object[] { Resources.Product, " ", AProductDTO.ProductCode }));
                }
                else
                {


                    string strPaymentAllocSeq = "";

                    var items = AProductDTO.AssignedList;

                    foreach (var item in items)
                    {
                        strPaymentAllocSeq += "," + item;
                    }

                    if (strPaymentAllocSeq.Length > 0)
                    {
                        strPaymentAllocSeq = strPaymentAllocSeq.Substring(1, strPaymentAllocSeq.Length - 1);
                    }

                    AProductDTO.PaymentAllocationType = strPaymentAllocSeq;

                    string strMemberTypeSeq = "";

                    var itemsMemberType = AProductDTO.MemberTypeAssignedList;

                    foreach (var item in itemsMemberType)
                    {
                        strMemberTypeSeq += "," + item;
                    }

                    if (strMemberTypeSeq.Length > 0)
                    {
                        strMemberTypeSeq = strMemberTypeSeq.Substring(1, strMemberTypeSeq.Length - 1);
                    }

                    AProductDTO.ProductMemberTypeCode = strMemberTypeSeq;

                    ModelState.Clear();

                    if (!ValidateProductCode(AProductDTO))
                    {
                        ModelState.AddModelError("ProductCode", String.Format(Resources.MsgErrInvalid, Resources.ProductCode));

                        var currencyCode = tLookUpManager.GetAll<CurrencyDTO>("", "Currency");
                        var billingCycleCode = tLookUpManager.GetAll<BillingCycleDTO>("", "BillingCycle");
                        //var defaultAffinityCode = tLookUpManager.GetAll<DefaultAffinityDTO>("", "DefaultAffinity");
                        var interestMethodCode = tLookUpManager.GetAll<InterestMethodDTO>("", "InterestMethod", false);
                        var productClassCode = tLookUpManager.GetAll<ProductClassDTO>("", "ProductClass");
                        var productMemberTypeCode = tLookUpManager.GetAll<ProductMemberTypeDTO>("", "ProductMemberType");
                        var productStatusCode = tLookUpManager.GetAll<ProductStatusDTO>("", "SysParamProductStatus");
                        var productCategoryCode = tLookUpManager.GetAll<ProductCategoryDTO>("", "ProductCategory");
                        var productSubCategoryCode = tLookUpManager.GetAll<ProductSubCategoryDTO>("", "ProductSubCategory");
                        var serviceCode = tLookUpManager.GetAll<ServiceDTO>("", "Service");
                        var productFranchiseCode = tLookUpManager.GetAll<FranchiseDTO>("", "ProductFranchise");
                        var paymentAllocationSequence = tLookUpManager.GetAll<CreditSegmentDTO>("", "CreditSegment");
                        var revolverChargeFlag = Utils.GetRevolverChargeFlag();
                        var sysParamAvailCashLimit = tLookUpManager.GetAll<SystemParameterDTO>("", "SysParamAvlCashLimit");
                        var sysParamCardNoLen = tLookUpManager.GetAll<SystemParameterDTO>("", "SysParamCardNoLen");
                        var sysParamCardNoPattern = tLookUpManager.GetAll<SystemParameterDTO>("", "SysParamCardNoPattern");

                        var contractList = tLookUpManager.GetAll<ContractDTO>("", "Contract");
                        ViewBag.ContractList = contractList;

                        ViewBag.BillingCycle = billingCycleCode;
                        //ViewBag.DefaultAffinity = defaultAffinityCode;
                        ViewBag.InterestMethod = interestMethodCode;
                        ViewBag.ProductClass = productClassCode;
                        ViewBag.ProductMemberType = productMemberTypeCode;
                        ViewBag.ProductStatus = productStatusCode;
                        ViewBag.ProductCategory = productCategoryCode;
                        ViewBag.ProductSubCategory = productSubCategoryCode;
                        ViewBag.Service = serviceCode;
                        ViewBag.Currency = currencyCode;
                        ViewBag.ProductFranchise = productFranchiseCode;
                        ViewBag.PaymentAllocationSequenceList = paymentAllocationSequence;
                        ViewBag.RevolverChargeFlagList = revolverChargeFlag;
                        ViewBag.SysParamAvailCashLimit = sysParamAvailCashLimit;
                        ViewBag.SysParamCardNoLen = sysParamCardNoLen;
                        ViewBag.SysParamCardNoPattern = sysParamCardNoPattern;

                        ViewData["FormStatus"] = "";
                        ViewBag.Privileges = tCommonManager.GetUserPrivilegesForEntity("MT_PRODUCT");

                        // Added to set datasource for the selected product memeber types listbox
                        List<SelectListItem> lstSelectedPTypes = new List<SelectListItem>();
                        var ProductTypeSelectedItems = AProductDTO.ProductMemberTypeCode;
                        string[] PSelectedItems = ProductTypeSelectedItems.Split(',');
                        foreach (string sitems in PSelectedItems)
                        {
                            List<SelectListItem> lst = new List<SelectListItem>();
                            lst = tLookUpManager.GetAll<ProductMemberTypeDTO>(sitems, "ProductMemberTypeByCode");
                            lst.RemoveAt(0);
                            lstSelectedPTypes = lstSelectedPTypes.Union(lst).ToList();

                        }
                        ViewBag.SelectedProductMemberTypesitem = lstSelectedPTypes;

                        AProductDTO.ApprovalStatus = "";
                        return View("ProductAdd", AProductDTO);

                    }

                    var errors = ModelState.Values.SelectMany(x => x.Errors);

                    if (ModelState.IsValid)
                    {

                        if (Command.Equals(Resources.Submit))
                            AProductDTO.ApprovalStatus = "P";
                        else if (Command.Equals(Resources.Save))
                            AProductDTO.ApprovalStatus = "D";

                        if (Command.Equals(Resources.Submit) || Command.Equals(Resources.Save))
                        {
                            if (string.IsNullOrWhiteSpace(tMasterManager.FindByProductCode(AProductDTO.ProductCode, Utils.OrganizationCode).ProductCode))
                            {
                                AProductDTO.CreatedBy = tCommonManager.GetLoggedUserName();
                                AProductDTO.LastUpdatedBy = tCommonManager.GetLoggedUserName();
                                AProductDTO.OrganizationCode = Utils.OrganizationCode;
                                tMasterManager.SaveProductDetails(AProductDTO);
                                TempData["Message"] = String.Format(Resources.MsgInfoSaveSuccess, String.Concat(new object[] { Resources.Product, " ", AProductDTO.ProductCode }));
                            }
                            else
                            {
                                if (!string.IsNullOrWhiteSpace(Convert.ToString(AProductCode)))
                                {
                                    AProductDTO.LastUpdatedBy = tCommonManager.GetLoggedUserName();
                                    tMasterManager.UpdateProductDetails(AProductCode, AOrganizationCode, AProductDTO);
                                    TempData["Message"] = String.Format(Resources.MsgInfoSaveSuccess, String.Concat(new object[] { Resources.Product, " ", AProductDTO.ProductCode }));
                                }
                                else
                                {
                                    TempData["Message"] = String.Format(Resources.MsgErrAlreadyExists, String.Concat(new object[] { Resources.Product, " ", AProductDTO.ProductCode }));
                                }
                            }
                        }
                        else if (Command.Equals(Resources.Delete))
                        {
                            try
                            {
                                AProductDTO.LastUpdatedBy = tCommonManager.GetLoggedUserName();
                                AProductDTO.ApprovalStatus = Utils.GetApprovalStatusCode(Utils.ApprovalStatus.Deleted);
                                tMasterManager.UpdateProductDetails(AProductCode, AOrganizationCode, AProductDTO);
                                TempData["Message"] = String.Format(Resources.MsgInfoDeleteSuccess, String.Concat(new object[] { Resources.Product, " ", AProductDTO.ProductCode }));
                            }
                            catch (Exception ex)
                            {
                                LoggerFactory.CreateLog().LogError("Error" + ex.Message, 100, 2);
                                TempData["Message"] = String.Format(Resources.MsgErrDeleteConcurrencyError, String.Concat(new object[] { Resources.Product, " ", AProductDTO.ProductCode }));
                                return RedirectToAction("ProductAdd", "master", new { AProductCode = AProductCode, AOrganizationCode = AOrganizationCode });
                            }
                        }

                    }

                }
            }

            catch (Exception ex)
            {
                LoggerFactory.CreateLog().LogError("Error" + ex.Message, 100, 2);
                ViewBag.ErrorMessage = Resources.MsgErrServerException;
                ViewBag.ErrorType = SessionErrorType.ServerException;
                return View("Error");
            }


            if (Command.Equals(Resources.Delete))
            {
                ModelState.Clear();
                AProductDTO = new ProductDTO();
                //return RedirectToAction("ProductAdd", "Master");
                RedirectToAction("ProductAdd", "Master", new { AProductCode = "", AOrganizationCode = "" });
            }

            return ((Command.Equals(Resources.Save)) ? RedirectToAction("ProductAdd", "Master") : RedirectToAction("ProductAdd", "Master", new { AProductCode = AProductDTO.ProductCode, AOrganizationCode = AOrganizationCode }));

        }
        #endregion
        #region ProductDelete
        [HttpPost]
        [AuthorizeUserAttribute(Entity = "MT_PRODUCT", AccessLevel = new UserActivities[] { UserActivities.Delete })]
        public JsonResult ProductDelete(string AProductCode, string AOrganizationCode)
        {
            ProductDTO AProductDTO = new ProductDTO();
            try
            {
                AProductDTO = tMasterManager.FindByProductCode(AProductCode, AOrganizationCode);
                AProductDTO.LastUpdatedBy = tCommonManager.GetLoggedUserName();
                AProductDTO.ApprovalStatus = Utils.GetApprovalStatusCode(Utils.ApprovalStatus.Deleted);
                tMasterManager.UpdateProductDetails(AProductCode, AOrganizationCode, AProductDTO);

                var dResult = new { result = "success", action = "ProductList", controller = "Master" };
                TempData["Message"] = String.Format(Resources.MsgInfoDeleteSuccess, String.Concat(new object[] { Resources.Product, " ", AProductDTO.ProductCode })); 
                return Json(dResult, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                var dResultFail = new { result = "fail", action = "ProductList", controller = "Master" };
                TempData["Message"] = String.Format(Resources.MsgErrDeleteConcurrencyError, String.Concat(new object[] { Resources.Product, " ", AProductDTO.ProductCode })); 
                return Json(dResultFail, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion
        #region ProductList
        [AuthorizeUserAttribute(Entity = "MT_PRODUCT", AccessLevel = new UserActivities[] { UserActivities.View })]
        public ActionResult ProductList(ProductSearch AVMProductSearch, string ClearButton, int? page)
        {
            try
            {
                var productCategoryCode = tLookUpManager.GetAll<ProductCategoryDTO>("", "ProductCategory");
                var productSubCategoryCode = tLookUpManager.GetAll<ProductSubCategoryDTO>("", "ProductSubCategory");
                var productFranchiseCode = tLookUpManager.GetAll<FranchiseDTO>("", "ProductFranchise");
                int iPageNumber = Convert.ToInt32((page == null) ? 0 : page);

                if (ClearButton == Resources.Clear)
                {
                    AVMProductSearch.ProductCode = null;
                    AVMProductSearch.ProductName = null;
                    AVMProductSearch.ProductCategoryCode = null;
                    AVMProductSearch.ProductSubCategoryCode = null;
                    ModelState.Clear();
                }

                if ((Utils.OrganizationCode != "") || (!(string.IsNullOrEmpty(AVMProductSearch.SearchButton)) &&
                     !((string.IsNullOrWhiteSpace(AVMProductSearch.ProductCode)
                     && (string.IsNullOrWhiteSpace(AVMProductSearch.ProductName))
                     && (string.IsNullOrWhiteSpace(AVMProductSearch.ProductCategoryCode))
                     && (string.IsNullOrWhiteSpace(AVMProductSearch.ProductSubCategoryCode))
                     ))))
                {
                    var entities = tMasterManager.FindAll(iPageNumber, new ProductDTO()
                    {
                        ProductCode = AVMProductSearch.ProductCode,
                        ProductName = AVMProductSearch.ProductName,
                        ProductCategoryCode = AVMProductSearch.ProductCategoryCode,
                        ProductSubCategoryCode = AVMProductSearch.ProductSubCategoryCode,
                    });
                    AVMProductSearch.SearchResults = entities.AsQueryable();
                }
                else
                {
                    var currResult = tMasterManager.FindProduct(1, Utils.GRD_ROWS_PER_PAGE).AsQueryable();

                    AVMProductSearch.SearchResults = currResult.AsQueryable();
                }

                ViewBag.ProductCategory = productCategoryCode;
                ViewBag.ProductSubCategory = productSubCategoryCode;
                ViewBag.ProductFranchise = productFranchiseCode;


                var SearchResults = AVMProductSearch.SearchResults.Where(x => x.ApprovalStatus != "DEL" && x.ApprovalStatus != "DIS").ToList();

                SearchResults.ForEach(a => a.ProductStatusDesc = tLookUpManager.GetAll<SystemParameterDTO>(a.ProductStatusCode, "SysParamProductStatus", false).FirstOrDefault().Text);

                AVMProductSearch.SearchResults = SearchResults.AsQueryable();

                //AVMProductSearch.SearchResults = AVMProductSearch.SearchResults.Where(x => x.ApprovalStatus != "DEL" && x.ApprovalStatus != "DIS");
                ViewBag.Privileges = tCommonManager.GetUserPrivilegesForEntity("MT_PRODUCT");
                return View(AVMProductSearch);
            }
            catch (Exception ex)
            {
                LoggerFactory.CreateLog().LogError("Error" + ex.Message, 100, 2);
                ViewBag.ErrorMessage = Resources.MsgErrServerException;
                ViewBag.ErrorType = SessionErrorType.ServerException;
                return View("Error");
            }
        }
        #endregion
        #region ProductAuthorize
        [AuthorizeUserAttribute(Entity = "MT_PRODUCT", AccessLevel = new UserActivities[] { UserActivities.Authorize })]
        public ActionResult ProductAuthorize(string AProductCode, string AOrganizationCode, string AMode, string QId)
        {
            ViewBag.RequestQueueId = QId;
            ViewBag.TableName = "PDT";

            var currencyCode = tLookUpManager.GetAll<CurrencyDTO>("", "Currency");
            var billingCycleCode = tLookUpManager.GetAll<BillingCycleDTO>("", "BillingCycle");
            //var defaultAffinityCode = tLookUpManager.GetAll<DefaultAffinityDTO>("", "DefaultAffinity");
            var interestMethodCode = tLookUpManager.GetAll<InterestMethodDTO>("", "InterestMethod", false);
            var productClassCode = tLookUpManager.GetAll<ProductClassDTO>("", "ProductClass");
            var productMemberTypeCode = tLookUpManager.GetAll<ProductMemberTypeDTO>("", "ProductMemberType");
            var productStatusCode = tLookUpManager.GetAll<ProductStatusDTO>("", "SysParamProductStatus");
            var productCategoryCode = tLookUpManager.GetAll<ProductCategoryDTO>("", "ProductCategory");
            var productSubCategoryCode = tLookUpManager.GetAll<ProductSubCategoryDTO>("", "ProductSubCategory");
            var serviceCode = tLookUpManager.GetAll<ServiceDTO>("", "Service");
            var productFranchiseCode = tLookUpManager.GetAll<FranchiseDTO>("", "ProductFranchise");
            var paymentAllocationSequence = tLookUpManager.GetAll<CreditSegmentDTO>("", "CreditSegment");
            var revolverChargeFlag = Utils.GetRevolverChargeFlag();
            var sysParamAvailCashLimit = tLookUpManager.GetAll<SystemParameterDTO>("", "SysParamAvlCashLimit");
            var sysParamCardNoLen = tLookUpManager.GetAll<SystemParameterDTO>("", "SysParamCardNoLen");
            var sysParamCardNoPattern = tLookUpManager.GetAll<SystemParameterDTO>("", "SysParamCardNoPattern");

            var contractList = tLookUpManager.GetAll<ContractDTO>("", "Contract");
            ViewBag.ContractList = contractList;

            if (!String.IsNullOrWhiteSpace(AProductCode))
            {
                ProductDTO productDTO = tMasterManager.FindByProductCode(AProductCode, AOrganizationCode);

                // Added to set datasource for the selected product memeber types listbox
                List<SelectListItem> lstSelectedPTypes = new List<SelectListItem>();
                var ProductTypeSelectedItems = productDTO.ProductMemberTypeCode;
                string[] PSelectedItems = ProductTypeSelectedItems.Split(',');
                foreach (string items in PSelectedItems)
                {
                    List<SelectListItem> lst = new List<SelectListItem>();
                    lst = tLookUpManager.GetAll<ProductMemberTypeDTO>(items, "ProductMemberTypeByCode");
                    lst.RemoveAt(0);
                    lstSelectedPTypes = lstSelectedPTypes.Union(lst).ToList();
                }
                ViewBag.SelectedProductMemberTypesitem = lstSelectedPTypes;
                // Ended

                if (!String.IsNullOrWhiteSpace(productDTO.ApprovalStatus) && productDTO.ApprovalStatus.Equals("P"))
                {
                    var reasonCodes = tLookUpManager.GetAll<ApprovalContextDTO>("PDT", "ReasonContext");
                    ViewBag.ReasonCodes = reasonCodes;
                }

                ViewBag.Privileges = tCommonManager.GetUserPrivilegesForEntity("MT_PRODUCT");

                ViewData["FormStatus"] = productDTO.ApprovalStatus;
                ViewData["FormCall"] = "ApprovalQueue";

                ViewBag.BillingCycle = billingCycleCode;
                //ViewBag.DefaultAffinity = defaultAffinityCode;
                ViewBag.InterestMethod = interestMethodCode;
                ViewBag.ProductClass = productClassCode;
                ViewBag.ProductMemberType = productMemberTypeCode;
                ViewBag.ProductStatus = productStatusCode;
                ViewBag.ProductCategory = productCategoryCode;
                ViewBag.ProductSubCategory = productSubCategoryCode;
                ViewBag.Service = serviceCode;
                ViewBag.Currency = currencyCode;
                ViewBag.ProductFranchise = productFranchiseCode;
                ViewBag.PaymentAllocationSequenceList = paymentAllocationSequence;
                ViewBag.RevolverChargeFlagList = revolverChargeFlag;
                ViewBag.SelectedProductMemberType = productMemberTypeCode;
                ViewBag.SysParamAvailCashLimit = sysParamAvailCashLimit;
                ViewBag.SysParamCardNoLen = sysParamCardNoLen;
                ViewBag.SysParamCardNoPattern = sysParamCardNoPattern;

                return View("ProductAdd", productDTO);
            }
            else
                return RedirectToAction("ApprovalQueueList", "Approval");
        }
        #endregion
        #region ValidateProductCode
        public bool ValidateProductCode(ProductDTO AProductDTO)
        {
            bool IsValid = true;

            //string ProductFirstChar = tMasterManager.GetProductFirstChar();

            //string[] strProductFirstChar = ProductFirstChar.Split(',');

            List<string> lstPriv = new List<string>();
            string stringToCheck = AProductDTO.ProductCode.Substring(0, 1);

            var SysParamProductFirstDigit = tLookUpManager.GetAll<SystemParameterDTO>("", "SysParamProductFirstDigit");
            SysParamProductFirstDigit.RemoveAt(0);

            foreach (SelectListItem item in SysParamProductFirstDigit)
            {
                lstPriv.Add(item.Value);
            }

            if (lstPriv.Any(stringToCheck.Contains))
            {
                IsValid = true;
            }
            else
            {
                IsValid = false;
            }

            return IsValid;
            //return Json(new { IsValid = IsValid }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #endregion

        #region Block Code
        #region BlockCodeView
        [AuthorizeUserAttribute(Entity = "MT_BLOCKCODE", AccessLevel = new UserActivities[] { UserActivities.View })]
        public ActionResult BlockCodeView(string Id)
        {
            try
            {
                BlockCodeDTO blockCodeDTO;
                ViewBag.TableName = "BLKCDE";
                ViewBag.Privileges = tCommonManager.GetUserPrivilegesForEntity("MT_BLOCKCODE");

                if (String.IsNullOrWhiteSpace(Id))
                {
                    blockCodeDTO = new BlockCodeDTO();
                    ViewData["FormStatus"] = blockCodeDTO.ApprovalStatus;
                    return View("BlockCodeAdd", blockCodeDTO);
                }
                else
                {
                    blockCodeDTO = tMasterManager.FindByBlockCode(Id);
                    ViewBag.ReasonCodes = null;
                }

                ViewData["FormStatus"] = blockCodeDTO.ApprovalStatus;
                return View("BlockCodeAdd", blockCodeDTO);
            }
            catch (Exception ex)
            {
                LoggerFactory.CreateLog().LogError("Error" + ex.Message, 100, 2);
                ViewBag.ErrorMessage = Resources.MsgErrServerException;
                ViewBag.ErrorType = SessionErrorType.ServerException;
                return View("Error");
            }
        }
        #endregion
        #region BlockCodeAdd
        [AuthorizeUserAttribute(Entity = "MT_BLOCKCODE", AccessLevel = new UserActivities[] { UserActivities.Add, UserActivities.Update, UserActivities.View })]
        public ActionResult BlockCodeAdd(string Id)
        {
            try
            {
                BlockCodeDTO blockCodeDTO;

                if (String.IsNullOrWhiteSpace(Id))
                    blockCodeDTO = new BlockCodeDTO();
                else
                    blockCodeDTO = tMasterManager.FindByBlockCode(Id);

                if (!String.IsNullOrWhiteSpace(blockCodeDTO.ApprovalStatus) && blockCodeDTO.ApprovalStatus.Equals("P"))
                {
                    var reasonCodes = tLookUpManager.GetAll<ApprovalContextDTO>("BLKCDE", "ReasonContext");
                    ViewBag.ReasonCodes = reasonCodes;

                }

                ViewBag.Privileges = tCommonManager.GetUserPrivilegesForEntity("MT_BLOCKCODE");

                ViewData["FormStatus"] = blockCodeDTO.ApprovalStatus;

                return View(blockCodeDTO);
            }
            catch (Exception ex)
            {
                LoggerFactory.CreateLog().LogError("Error" + ex.Message, 100, 2);
                ViewBag.ErrorMessage = Resources.MsgErrServerException;
                ViewBag.ErrorType = SessionErrorType.ServerException;
                return View("Error");
            }
        }
        #endregion
        #region BlockCodeSave
        [HttpPost]
        [AuthorizeUserAttribute(Entity = "MT_BLOCKCODE", AccessLevel = new UserActivities[] { UserActivities.Add, UserActivities.Update })]
        public ActionResult BlockCodeSave(string Id, string Command, BlockCodeDTO ABlockCodeDTO)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    if (Command.Equals(Resources.Submit))
                        ABlockCodeDTO.ApprovalStatus = "P";
                    else if (Command.Equals(Resources.Save))
                        ABlockCodeDTO.ApprovalStatus = "D";

                    ABlockCodeDTO.OrganizationCode = Utils.OrganizationCode;
                    if (Command.Equals(Resources.Submit) || Command.Equals(Resources.Save))
                    {
                        if (string.IsNullOrWhiteSpace(tMasterManager.FindByBlockCode(ABlockCodeDTO.Code).Code))
                        {
                            ABlockCodeDTO.CreatedBy = tCommonManager.GetLoggedUserName();
                            ABlockCodeDTO.LastUpdatedBy = tCommonManager.GetLoggedUserName();
                            tMasterManager.SaveBlockCodeDetails(ABlockCodeDTO);
                            TempData["Message"] = String.Format(Resources.MsgInfoSaveSuccess, String.Concat(new object[] { Resources.BlockCode, " ", ABlockCodeDTO.Code})); 
                        }
                        else
                        {
                            if (!string.IsNullOrWhiteSpace(Id))
                            {
                                ABlockCodeDTO.LastUpdatedBy = tCommonManager.GetLoggedUserName();
                                tMasterManager.UpdateBlockCodeDetails(ABlockCodeDTO.Code, ABlockCodeDTO);
                                TempData["Message"] = String.Format(Resources.MsgInfoSaveSuccess, String.Concat(new object[] { Resources.BlockCode, " ", ABlockCodeDTO.Code}));
                            }
                            else
                            {
                                TempData["Message"] = String.Format(Resources.MsgErrAlreadyExists, String.Concat(new object[] { Resources.BlockCode, " ", ABlockCodeDTO.Code})); 
                            }
                        }
                    }
                    else if (Command.Equals(Resources.Delete))
                    {
                        try
                        {
                            ABlockCodeDTO.LastUpdatedBy = tCommonManager.GetLoggedUserName();
                            ABlockCodeDTO.ApprovalStatus = Utils.GetApprovalStatusCode(Utils.ApprovalStatus.Deleted);
                            tMasterManager.UpdateBlockCodeDetails(ABlockCodeDTO.Code, ABlockCodeDTO);
                            TempData["Message"] = String.Format(Resources.MsgInfoDeleteSuccess, String.Concat(new object[] { Resources.BlockCode, " ", Id}));
                        }
                        catch (Exception ex)
                        {
                            LoggerFactory.CreateLog().LogError("Error" + ex.Message, 100, 2);
                            TempData["Message"] = String.Format(Resources.MsgErrDeleteConcurrencyError, String.Concat(new object[] { Resources.BlockCode, " ", Id}));
                            return RedirectToAction("BlockCodeAdd", "master", new { Id = Id });
                        }
                    }
                }
            }

            catch (Exception ex)
            {
                LoggerFactory.CreateLog().LogError("Error" + ex.Message, 100, 2);
                ViewBag.ErrorMessage = Resources.MsgErrServerException;
                ViewBag.ErrorType = SessionErrorType.ServerException;
                return View("Error");
            }

            return ((Command.Equals(Resources.Submit) || Command.Equals(Resources.Delete)) ? RedirectToAction("BlockCodeAdd", "Master") : RedirectToAction("BlockCodeAdd", "Master", new { Id = Id }));

        }
        #endregion
        #region BlockCodeDelete
        [HttpPost]
        [AuthorizeUserAttribute(Entity = "MT_BLOCKCODE", AccessLevel = new UserActivities[] { UserActivities.Delete })]
        public JsonResult BlockCodeDelete(string Id)
        {
            BlockCodeDTO ABlockCodeDTO = new BlockCodeDTO();
            try
            {
                ABlockCodeDTO = tMasterManager.FindByBlockCode(Id);
                ABlockCodeDTO.LastUpdatedBy = tCommonManager.GetLoggedUserName();
                ABlockCodeDTO.ApprovalStatus = Utils.GetApprovalStatusCode(Utils.ApprovalStatus.Deleted);
                ABlockCodeDTO.OrganizationCode = Utils.OrganizationCode;
                tMasterManager.UpdateBlockCodeDetails(ABlockCodeDTO.Code, ABlockCodeDTO);

                var dResult = new { result = "success", action = "BlockCodeList", controller = "Master" };
                TempData["Message"] = string.Format(Resources.MsgInfoDeleteSuccess, Resources.BlockCode + Id);
                return Json(dResult, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                var dResultFail = new { result = "fail", action = "BlockCodeList", controller = "Master" };
                TempData["Message"] = string.Format(Resources.MsgErrDeleteConcurrencyError,Resources.BlockCode + Id);
                return Json(dResultFail, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion
        #region BlockCodeList
        [AuthorizeUserAttribute(Entity = "MT_BLOCKCODE", AccessLevel = new UserActivities[] { UserActivities.View })]
        public ActionResult BlockCodeList(BlockCodeSearch AVMBlockCodeSearch, string ClearButton, int? page)
        {
            try
            {
                int iPageNumber = Convert.ToInt32((page == null) ? 0 : page);

                if (ClearButton == Resources.Clear)
                {
                    AVMBlockCodeSearch.Code = null;
                    AVMBlockCodeSearch.Description = null;
                    AVMBlockCodeSearch.ShortDescription = null;
                    ModelState.Clear();
                }

                if ((Utils.OrganizationCode != "") || (!(string.IsNullOrEmpty(AVMBlockCodeSearch.SearchButton)) &&
                     !((string.IsNullOrWhiteSpace(AVMBlockCodeSearch.Code)
                     && (string.IsNullOrWhiteSpace(AVMBlockCodeSearch.Description))
                     && (string.IsNullOrWhiteSpace(AVMBlockCodeSearch.ShortDescription))))))
                {
                    var entities = tMasterManager.FindAll(iPageNumber, new BlockCodeDTO()
                    {
                        Code = AVMBlockCodeSearch.Code,
                        Description = AVMBlockCodeSearch.Description,
                        ShortDescription = AVMBlockCodeSearch.ShortDescription
                    });
                    AVMBlockCodeSearch.SearchResults = entities.AsQueryable();
                }
                else
                {
                    var currResult = tMasterManager.FindBlockCode(1, Utils.GRD_ROWS_PER_PAGE).AsQueryable();
                    AVMBlockCodeSearch.SearchResults = currResult.AsQueryable();
                }
                ViewBag.Privileges = tCommonManager.GetUserPrivilegesForEntity("MT_BLOCKCODE");
                return View(AVMBlockCodeSearch);
            }
            catch (Exception ex)
            {
                LoggerFactory.CreateLog().LogError("Error" + ex.Message, 100, 2);
                ViewBag.ErrorMessage = Resources.MsgErrServerException;
                ViewBag.ErrorType = SessionErrorType.ServerException;
                return View("Error");
            }
        }
        #endregion
        #region BlockCodeAuthorize
        [AuthorizeUserAttribute(Entity = "MT_BLOCKCODE", AccessLevel = new UserActivities[] { UserActivities.Authorize })]
        public ActionResult BlockCodeAuthorize(string Id, string AMode, string QId)
        {
            ViewBag.RequestQueueId = QId;
            ViewBag.TableName = "BLKCDE";
            if (!String.IsNullOrWhiteSpace(Id))
            {
                BlockCodeDTO blockCodeDTO = tMasterManager.FindByBlockCode(Id);

                if (!String.IsNullOrWhiteSpace(blockCodeDTO.ApprovalStatus) && blockCodeDTO.ApprovalStatus.Equals("P"))
                {
                    var reasonCodes = tLookUpManager.GetAll<ApprovalContextDTO>("BLKCDE", "ReasonContext");
                    ViewBag.ReasonCodes = reasonCodes;
                }

                ViewBag.Privileges = tCommonManager.GetUserPrivilegesForEntity("MT_BLOCKCODE");
                ViewData["FormCall"] = "ApprovalQueue";
                ViewData["FormStatus"] = blockCodeDTO.ApprovalStatus;
                return View("BlockCodeAdd", blockCodeDTO);
            }
            else
                return RedirectToAction("ApprovalQueueList", "Approval");
        }
        #endregion
        #endregion

        #region Customer
        #region CustomerView
        [AuthorizeUserAttribute(Entity = "MT_CUSTOMER", AccessLevel = new UserActivities[] { UserActivities.View })]
        public ActionResult CustomerView(string ACustomerId)
        {
            try
            {
                CustomerDTO customerDTO;
                /*Sales and policy*/
                //var branchList = tLookUpManager.GetAll<BranchDTO>("", "Branch");
                var organizationList = tLookUpManager.GetAll<OrganizationDTO>("", "Organization");
                var productTypeList = tLookUpManager.GetAll<ProductTypeDTO>("", "ProductType");
                var productCategoryList = tLookUpManager.GetAll<ProductCategoryDTO>("", "ProductCategory");
                var campaignSourceList = tLookUpManager.GetAll<SourcingDTO>("", "Sourcing");
                /*Personal Details*/
                var customerType = Utils.GetCustomerType();
                var genderList = Utils.GetGender();
                var maritalStatusList = tLookUpManager.GetAll<ProductDTO>("", "MaritalStatus");
                var educationList = tLookUpManager.GetAll<ProductDTO>("", "Education");
                var professionList = tLookUpManager.GetAll<ProductDTO>("", "Profession");
                var housingList = Utils.GetHousing();
                var nationality = tLookUpManager.GetAll<CountryDTO>("", "Nationality");
                /*Customer Address*/
                var addressTypeList = Utils.GetCustomerAddressType();
                var residenceClearanceList = Utils.GetResidenceClearanceType();
                /*Customer Occupation*/
                var workTypeList = Utils.GetWorkType();
                var incomeTypeList = Utils.GetIncomeType();
                var designationList = tLookUpManager.GetAll<DesignationDTO>("", "Designation");
                /*Customer Decision*/
                var decisionList = tLookUpManager.GetAll<DecisionDTO>("", "Decision");
                var contractList = tLookUpManager.GetAll<ContractDTO>("", "Contract");
                var decisionReasonList = Utils.GetCustomerDecisionReason();//tLookUpManager.GetAll<DecisionReasonDTO>("", "DecisionReason");//
                var unemploymentList = Utils.GetUnemployment();
                /*Customer House Consumption Details*/
                var typeList = Utils.GetCommercialHouseConsumptionType();
                var accountStatusList = Utils.GetAccountStatus();
                /*Customer Current Account details*/
                var bankTypeList = Utils.GetBankType();
                var accountBankList = tLookUpManager.GetAll<BankDTO>("", "Bank");
                /*Customer Billing*/
                LoginUserDTO loggedInUser = (LoginUserDTO)System.Web.HttpContext.Current.Session["LOGGED_IN_USER"];
                var branchList = tLookUpManager.GetAll<UserAllowedBranchDTO>(loggedInUser.Username, "UserBranch");//tLookUpManager.GetAll<BranchDTO>("", "Branch");
                /*Customer Address*/
                //var regionList = tLookUpManager.GetAll<RegionDTO>("IND", "Region");
                var cityList = tLookUpManager.GetAll<CityDTO>("", "City");
                //var communaList = tLookUpManager.GetAll<CommunaDTO>("IND", "Communa");
                /*Customer Credit Card Detail*/
                var cardBankInstitutionList = tLookUpManager.GetAll<BankDTO>("", "Bank");
                var cardTypeList = tLookUpManager.GetAll<CardTypeDTO>("", "CardType");
                var cardBrandList = Utils.GetCardBrand();
                var customerDependentAgeGroup = Utils.GetCustomerDependentAgeGroup();

                if (ACustomerId == null)
                {
                    customerDTO = new CustomerDTO();
                    customerDTO.OrganizationCode = Utils.OrganizationCode;
                }
                else
                {
                    customerDTO = tMasterManager.FindByCustomerId(Convert.ToInt32(ACustomerId));
                }

                ViewBag.Privileges = tCommonManager.GetUserPrivilegesForEntity("MT_CUSTOMER");

                ViewData["FormStatus"] = "P";
                ViewBag.FromSearchPage = true;

                ViewBag.OrganizationList = organizationList;
                ViewBag.ProductTypeList = productTypeList;
                ViewBag.ProductCategoryList = productCategoryList;
                ViewBag.CampaignSourceList = campaignSourceList;

                ViewBag.AddressTypeList = addressTypeList;
                ViewBag.ResidenceClearanceList = residenceClearanceList;
                ViewBag.UnemploymentList = unemploymentList;
                ViewBag.TypeList = typeList;
                ViewBag.BankTypeList = bankTypeList;
                ViewBag.CardBankInstitutionList = cardBankInstitutionList;
                ViewBag.CardTypeList = cardTypeList;
                ViewBag.CardBrandList = cardBrandList;

                ViewBag.CustomerTypeList = customerType;
                ViewBag.GenderList = genderList;
                ViewBag.MaritalStatusList = maritalStatusList;
                ViewBag.EducationListList = educationList;
                ViewBag.ProfessionList = professionList;
                ViewBag.HousingList = housingList;
                ViewBag.NationalityList = nationality;
                ViewBag.WorkTypeList = workTypeList;
                ViewBag.DesignationList = designationList;
                ViewBag.DecisionList = decisionList;
                ViewBag.ContractList = contractList;
                ViewBag.DecisionReasonList = decisionReasonList;
                ViewBag.AccountBankList = accountBankList;
                ViewBag.AccountStatus = accountStatusList;
                ViewBag.BranchList = branchList;
                //ViewBag.RegionList = regionList;
                ViewBag.CityList = cityList;
                //ViewBag.CommunaList = communaList;
                ViewBag.CardBankList = cardBankInstitutionList;
                ViewBag.IncomeTypeList = incomeTypeList;
                ViewBag.CustomerDependentAgeGroup = customerDependentAgeGroup;

                ViewBag.CustomerValue = new JavaScriptSerializer().Serialize(customerDTO);

                if (customerDTO.ProductMemberTypeCode == "A")
                {
                    var productList = tLookUpManager.GetAll<ProductDTO>(customerDTO.ProductCategoryCode, "ProductByProductCategory");
                    ViewBag.ProductList = productList;
                    return View("AddOnCustomerAdd", customerDTO);
                }
                else
                {
                    var productList = tLookUpManager.GetAll<ProductDTO>(string.Empty, "Product");
                    ViewBag.ProductList = productList;
                    return View("CustomerAdd", customerDTO);
                }
            }
            catch (Exception ex)
            {
                LoggerFactory.CreateLog().LogError("Error" + ex.Message, 100, 2);
                ViewBag.ErrorMessage = Resources.MsgErrServerException;
                ViewBag.ErrorType = SessionErrorType.ServerException;
                return View("Error");
            }
        }
        #endregion
        #region CustomerAdd
        [AuthorizeUserAttribute(Entity = "MT_CUSTOMER", AccessLevel = new UserActivities[] { UserActivities.Add, UserActivities.Update, UserActivities.View })]
        public ActionResult CustomerAdd(string ACustomerId, string AProductMemberTypeCode = null)
        {
            try
            {
                CustomerDTO customerDTO;
                /*Sales and policy*/
                var organizationList = tLookUpManager.GetAll<OrganizationDTO>("", "Organization");
                var productTypeList = tLookUpManager.GetAll<ProductTypeDTO>("", "ProductType");
                var productCategoryList = tLookUpManager.GetAll<ProductCategoryDTO>("", "ProductCategory");
                var productList = tLookUpManager.GetAll<ProductDTO>("", "Product");
                var campaignSourceList = tLookUpManager.GetAll<SourcingDTO>("", "Sourcing");
                /*Personal Details*/
                var customerType = Utils.GetCustomerType();
                var genderList = Utils.GetGender();
                var maritalStatusList = tLookUpManager.GetAll<ProductDTO>("", "MaritalStatus");
                var educationList = tLookUpManager.GetAll<ProductDTO>("", "Education");
                var professionList = tLookUpManager.GetAll<ProductDTO>("", "Profession");
                var housingList = Utils.GetHousing();
                var nationality = tLookUpManager.GetAll<CountryDTO>("", "Nationality");
                /*Customer Address*/
                var addressTypeList = Utils.GetCustomerAddressType();
                var residenceClearanceList = Utils.GetResidenceClearanceType();
                /*Customer Occupation*/
                var workTypeList = Utils.GetWorkType();
                var incomeTypeList = Utils.GetIncomeType();
                var designationList = tLookUpManager.GetAll<DesignationDTO>("", "Designation");
                /*Customer Decision*/
                var decisionList = tLookUpManager.GetAll<DecisionDTO>("", "Decision");
                var contractList = tLookUpManager.GetAll<ContractDTO>("", "Contract");
                var decisionReasonList = Utils.GetCustomerDecisionReason();
                var unemploymentList = Utils.GetUnemployment();
                /*Customer House Consumption Details*/
                var typeList = Utils.GetCommercialHouseConsumptionType();
                var accountStatusList = Utils.GetAccountStatus();
                /*Customer Current Account details*/
                var bankTypeList = Utils.GetBankType();
                var accountBankList = tLookUpManager.GetAll<BankDTO>("", "Bank");
                /*Customer Billing*/
                LoginUserDTO loggedInUser = (LoginUserDTO)System.Web.HttpContext.Current.Session["LOGGED_IN_USER"];
                var branchList = tLookUpManager.GetAll<UserAllowedBranchDTO>(loggedInUser.Username, "UserBranch");
                //var branchList = tLookUpManager.GetAll<BranchDTO>("", "Branch");
                /*Customer Address*/
                var cityList = tLookUpManager.GetAll<CityDTO>("", "City");
                /*Customer Credit Card Detail*/
                var cardBankInstitutionList = tLookUpManager.GetAll<BankDTO>("", "Bank");
                var cardTypeList = tLookUpManager.GetAll<CardTypeDTO>("", "CardType");
                var cardBrandList = Utils.GetCardBrand();
                var customerDependentAgeGroup = Utils.GetCustomerDependentAgeGroup();

                if (ACustomerId == null)
                {
                    customerDTO = new CustomerDTO();
                    customerDTO.OrganizationCode = Utils.OrganizationCode;
                    customerDTO.ProductMemberTypeCode = AProductMemberTypeCode;
                }
                else
                {
                    customerDTO = tMasterManager.FindByCustomerId(Convert.ToInt32(ACustomerId));
                }
                if (!String.IsNullOrWhiteSpace(customerDTO.ApprovalStatus) && customerDTO.ApprovalStatus.Equals("P"))
                {
                    bool isCreditScoreViolated = tMasterManager.ValidateCreditScore(customerDTO.ProductCode, customerDTO.CreditScore);
                    bool isHRP = tMasterManager.ValidateCustomerDesignationForHRP(customerDTO.CustomerOccupationDTO.FirstOrDefault().Post);
                    string ContextCode = string.Empty;
                    if ((isCreditScoreViolated || isHRP) && customerDTO.AutoApprove)
                    {
                        ContextCode = "CUSTV";
                    }
                    else if (!customerDTO.AutoApprove)
                    {
                        ContextCode = "CUST";
                    }
                    var reasonCodes = tLookUpManager.GetAll<ApprovalContextDTO>(ContextCode, "ReasonContext");
                    ViewBag.ReasonCodes = reasonCodes;
                }
                ViewBag.Privileges = tCommonManager.GetUserPrivilegesForEntity("MT_CUSTOMER");
                ViewData["FormStatus"] = customerDTO.ApprovalStatus;

                ViewBag.OrganizationList = organizationList;
                ViewBag.ProductTypeList = productTypeList;
                ViewBag.ProductCategoryList = productCategoryList;
                ViewBag.CampaignSourceList = campaignSourceList;

                ViewBag.AddressTypeList = addressTypeList;
                ViewBag.ResidenceClearanceList = residenceClearanceList;
                ViewBag.UnemploymentList = unemploymentList;
                ViewBag.TypeList = typeList;
                ViewBag.BankTypeList = bankTypeList;
                ViewBag.CardBankInstitutionList = cardBankInstitutionList;
                ViewBag.CardTypeList = cardTypeList;
                ViewBag.CardBrandList = cardBrandList;

                ViewBag.CustomerTypeList = customerType;
                ViewBag.GenderList = genderList;
                ViewBag.MaritalStatusList = maritalStatusList;
                ViewBag.EducationListList = educationList;
                ViewBag.ProfessionList = professionList;
                ViewBag.HousingList = housingList;
                ViewBag.NationalityList = nationality;
                ViewBag.WorkTypeList = workTypeList;
                ViewBag.DesignationList = designationList;
                ViewBag.DecisionList = decisionList;
                ViewBag.ContractList = contractList;
                ViewBag.DecisionReasonList = decisionReasonList;
                ViewBag.AccountBankList = accountBankList;
                ViewBag.AccountStatus = accountStatusList;
                ViewBag.BranchList = branchList;
                ViewBag.CityList = cityList;
                ViewBag.CardBankList = cardBankInstitutionList;
                ViewBag.IncomeTypeList = incomeTypeList;
                ViewBag.ProductList = productList;
                ViewBag.CustomerDependentAgeGroup = customerDependentAgeGroup;

                ViewBag.CustomerValue = new JavaScriptSerializer().Serialize(customerDTO);
                return View(customerDTO);
            }
            catch (Exception ex)
            {
                LoggerFactory.CreateLog().LogError("Error" + ex.Message, 100, 2);
                ViewBag.ErrorMessage = Resources.MsgErrServerException;
                ViewBag.ErrorType = SessionErrorType.ServerException;
                return View("Error");
            }
        }
        #endregion
        #region CustomerSave
        [HttpPost]
        [AuthorizeUserAttribute(Entity = "MT_CUSTOMER", AccessLevel = new UserActivities[] { UserActivities.Add, UserActivities.Update })]
        public ActionResult CustomerSave(CustomerDTO ACustomerDTO, string ACommand)
        {
            string redirectUrl = string.Empty, messageToShow = string.Empty;
            try
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors);

                if (ModelState.IsValid)
                {
                    if (ACommand == Resources.Save)
                    {
                        ACustomerDTO.ApprovalStatus = "D";
                    }
                    else if (ACommand == Resources.Submit)
                    {
                        ACustomerDTO.ApprovalStatus = "P";
                    }
                    ACustomerDTO.LastUpdatedBy = tCommonManager.GetLoggedUserName();
                    if (ACustomerDTO.CustomerId == 0)
                    {
                        ACustomerDTO.CreatedBy = tCommonManager.GetLoggedUserName();
                        tMasterManager.SaveCustomerDetails(ACustomerDTO);
                    }
                    else
                    {
                        tMasterManager.UpdateCustomerDetails(ACustomerDTO.CustomerId, ACustomerDTO);
                    }
                    redirectUrl = Url.Action("CustomerRUT", "CardBooking");
                    messageToShow = String.Format(Resources.MsgInfoSaveSuccess, Resources.Customer); 
                }
            }
            catch (Exception ex)
            {
                LoggerFactory.CreateLog().LogError("Error" + ex.Message, 100, 2);
                messageToShow = String.Format(Resources.MsgErrSaveFailure, Resources.Customer); 
            }
            return Json(new { RedirectUrl = redirectUrl, Message = messageToShow }, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region CustomerDelete
        [HttpPost]
        [AuthorizeUserAttribute(Entity = "MT_CUSTOMER", AccessLevel = new UserActivities[] { UserActivities.Delete })]
        public ActionResult CustomerDelete(int ACustomerId, bool AIsDeleteCallFromSearchTab)
        {
            string redirectUrl = string.Empty, customerName = string.Empty;
            CustomerDTO objCustomerDTO = new CustomerDTO();
            try
            {
                objCustomerDTO = tMasterManager.FindByCustomerId(ACustomerId);
                objCustomerDTO.LastUpdatedBy = tCommonManager.GetLoggedUserName();
                objCustomerDTO.ApprovalStatus = Utils.GetApprovalStatusCode(Utils.ApprovalStatus.Deleted);
                objCustomerDTO.OrganizationCode = Utils.OrganizationCode;
                tMasterManager.UpdateCustomerDetails(ACustomerId, objCustomerDTO);

                if (AIsDeleteCallFromSearchTab)
                {
                    TempData["Message"] = String.Format(Resources.MsgInfoDeleteSuccess, Resources.Customer);
                    redirectUrl = Url.Action("CustomerList", "Master");
                }
                else
                {
                    TempData["Message"] = string.Empty;
                    redirectUrl = Url.Action("CustomerRUT", "CardBooking");
                }
            }
            catch (Exception ex)
            {
                LoggerFactory.CreateLog().LogError("Error" + ex.Message, 100, 2);
                if (AIsDeleteCallFromSearchTab)
                {
                    TempData["Message"] = String.Format(Resources.MsgErrDeleteConcurrencyError, Resources.Customer);
                    redirectUrl = Url.Action("CustomerList", "Master");
                }
                else
                {
                    TempData["Message"] = string.Empty;
                    redirectUrl = Url.Action("CustomerRUT", "CardBooking");
                }
                return Json(new { RedirectUrl = redirectUrl, Success = false, Message = String.Format(Resources.MsgErrDeleteConcurrencyError, Resources.Customer) }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { RedirectUrl = redirectUrl, Success = true, Message = String.Format(Resources.MsgInfoDeleteSuccess, Resources.Customer) }, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region CustomerList
        [AuthorizeUserAttribute(Entity = "MT_CUSTOMER", AccessLevel = new UserActivities[] { UserActivities.View })]
        public ActionResult CustomerList(CustomerSearch AVMCustomerSearch, string ClearButton, int? page)
        {
            try
            {
                var customerType = Utils.GetCustomerType();
                var customerStatus = Utils.GetCustomerStatus();
                int iPageNumber = Convert.ToInt32((page == null) ? 0 : page);
                if (ClearButton == Resources.Clear)
                {
                    AVMCustomerSearch.Name = AVMCustomerSearch.RUT = AVMCustomerSearch.CustomerType = null;
                    ModelState.Clear();
                }
                if ((Utils.OrganizationCode != "") || (!(string.IsNullOrEmpty(AVMCustomerSearch.SearchButton)) &&
                     !((string.IsNullOrWhiteSpace(AVMCustomerSearch.RUT) &&
                     (string.IsNullOrWhiteSpace(AVMCustomerSearch.CustomerType)) &&
                     (string.IsNullOrWhiteSpace(AVMCustomerSearch.Name)) &&
                     (string.IsNullOrWhiteSpace(AVMCustomerSearch.CustomerStatus))))))
                {
                    var entities = tMasterManager.GetCustomersForListing(AVMCustomerSearch.RUT == null ? null : AVMCustomerSearch.RUT.Replace("-", string.Empty),
                        AVMCustomerSearch.CustomerType, AVMCustomerSearch.Name,AVMCustomerSearch.CustomerStatus,Utils.OrganizationCode, false);
                    AVMCustomerSearch.SearchResults = entities.AsQueryable();
                }
              
                ViewBag.CustomerType = customerType;
                ViewBag.CustomerStatus = customerStatus;
                return View(AVMCustomerSearch);
            }
            catch (Exception ex)
            {
                LoggerFactory.CreateLog().LogError("Error" + ex.Message, 100, 2);
                ViewBag.ErrorMessage = Resources.MsgErrServerException;
                ViewBag.ErrorType = SessionErrorType.ServerException;
                return View("Error");
            }
        }
        #endregion
        #region CustomerAuthorize
        [AuthorizeUserAttribute(Entity = "MT_CUSTOMER", AccessLevel = new UserActivities[] { UserActivities.Authorize })]
        public ActionResult CustomerAuthorize(string ACustomerId, string AMode, string QId)
        {
            ViewBag.RequestQueueId = QId;
            ViewBag.TableName = "CUST";
            if (!String.IsNullOrWhiteSpace(ACustomerId))
            {
                CustomerDTO customerDTO = tMasterManager.FindByCustomerId(Convert.ToInt32(ACustomerId));
                /*Sales and policy*/
                //var branchList = tLookUpManager.GetAll<BranchDTO>("", "Branch");
                var organizationList = tLookUpManager.GetAll<OrganizationDTO>("", "Organization");
                var productTypeList = tLookUpManager.GetAll<ProductTypeDTO>("", "ProductType");
                var productCategoryList = tLookUpManager.GetAll<ProductCategoryDTO>("", "ProductCategory");
                var productList = tLookUpManager.GetAll<ProductDTO>("", "ProductByProductCategory");
                var campaignSourceList = tLookUpManager.GetAll<SourcingDTO>("", "Sourcing");//Utils.GetCampaignSource();
                /*Personal Details*/
                var customerType = Utils.GetCustomerType();
                var genderList = Utils.GetGender();
                var maritalStatusList = tLookUpManager.GetAll<ProductDTO>("", "MaritalStatus");
                var educationList = tLookUpManager.GetAll<ProductDTO>("", "Education");
                var professionList = tLookUpManager.GetAll<ProductDTO>("", "Profession");
                var housingList = Utils.GetHousing();
                var nationality = tLookUpManager.GetAll<CountryDTO>("", "Nationality");
                /*Customer Address*/
                var addressTypeList = Utils.GetCustomerAddressType();
                var residenceClearanceList = Utils.GetResidenceClearanceType();
                /*Customer Occupation*/
                var workTypeList = Utils.GetWorkType();
                var incomeTypeList = Utils.GetIncomeType();
                var designationList = tLookUpManager.GetAll<DesignationDTO>("", "Designation");
                /*Customer Decision*/
                var decisionList = tLookUpManager.GetAll<DecisionDTO>("", "Decision");
                var contractList = tLookUpManager.GetAll<ContractDTO>("", "Contract");
                var decisionReasonList = Utils.GetCustomerDecisionReason();//tLookUpManager.GetAll<DecisionReasonDTO>("", "DecisionReason");//
                var unemploymentList = Utils.GetUnemployment();
                /*Customer House Consumption Details*/
                var typeList = Utils.GetCommercialHouseConsumptionType();
                var accountStatusList = Utils.GetAccountStatus();
                /*Customer Current Account details*/
                var bankTypeList = Utils.GetBankType();
                var accountBankList = tLookUpManager.GetAll<BankDTO>("", "Bank");
                /*Customer Billing*/
                LoginUserDTO loggedInUser = (LoginUserDTO)System.Web.HttpContext.Current.Session["LOGGED_IN_USER"];
                var branchList = tLookUpManager.GetAll<UserAllowedBranchDTO>(loggedInUser.Username, "UserBranch");
                //var branchList = tLookUpManager.GetAll<BranchDTO>("", "Branch");
                /*Customer Address*/
                //var regionList = tLookUpManager.GetAll<RegionDTO>("IND", "Region");
                var cityList = tLookUpManager.GetAll<CityDTO>("", "City");
                //var communaList = tLookUpManager.GetAll<CommunaDTO>("IND", "Communa");
                /*Customer Credit Card Detail*/
                var cardBankInstitutionList = tLookUpManager.GetAll<BankDTO>("", "Bank");
                var cardTypeList = tLookUpManager.GetAll<CardTypeDTO>("", "CardType");
                var cardBrandList = Utils.GetCardBrand();
                var customerDependentAgeGroup = Utils.GetCustomerDependentAgeGroup();

                var result = new List<SelectListItem>();
                bool isCreditScoreViolated = tMasterManager.ValidateCreditScore(customerDTO.ProductCode, customerDTO.CreditScore);
                bool isHRP = tMasterManager.ValidateCustomerDesignationForHRP(customerDTO.CustomerOccupationDTO.FirstOrDefault().Post);
                if (!String.IsNullOrWhiteSpace(customerDTO.ApprovalStatus) && customerDTO.ApprovalStatus.Equals("P"))
                {                 
                    string ContextCode = string.Empty;
                    if ((isCreditScoreViolated || isHRP) && customerDTO.AutoApprove)
                    {
                        ContextCode = "CUSTV";
                    }
                    else if (!customerDTO.AutoApprove)
                    {
                        ContextCode = "CUST";
                    }
                    var reasonCodes = tLookUpManager.GetAll<ApprovalContextDTO>(ContextCode, "ReasonContext");
                    if (ContextCode == "CUSTV")
                    {
                        result = reasonCodes.Where(x => x.Value != "CUSTVPR").Select(x => x).ToList();
                        if (!isCreditScoreViolated)
                        {
                            result.RemoveAll(x => x.Value == "CUSTVUR");
                        }
                    }
                    if (ContextCode == "CUST")
                    {
                        result = reasonCodes.Where(x => x.Value != "CUSTPR").Select(x => x).ToList();
                        if (!isCreditScoreViolated)
                        {
                            result.RemoveAll(x => x.Value == "CUSTUR");
                        }
                    }                    
                }
                ViewBag.ReasonCodes = result;
                ViewData["CreditScoreViolated"] = isCreditScoreViolated;
                ViewData["HRPViolated"] = isHRP;

                ViewData["FormCall"] = "ApprovalQueue";
                ViewData["FormStatus"] = customerDTO.ApprovalStatus;
                ViewBag.Privileges = tCommonManager.GetUserPrivilegesForEntity("MT_CUSTOMER");

                ViewBag.OrganizationList = organizationList;
                ViewBag.ProductTypeList = productTypeList;
                ViewBag.ProductCategoryList = productCategoryList;
                ViewBag.CampaignSourceList = campaignSourceList;

                ViewBag.AddressTypeList = addressTypeList;
                ViewBag.ResidenceClearanceList = residenceClearanceList;
                ViewBag.UnemploymentList = unemploymentList;
                ViewBag.TypeList = typeList;
                ViewBag.BankTypeList = bankTypeList;
                ViewBag.CardBankInstitutionList = cardBankInstitutionList;
                ViewBag.CardTypeList = cardTypeList;
                ViewBag.CardBrandList = cardBrandList;

                ViewBag.CustomerTypeList = customerType;
                ViewBag.GenderList = genderList;
                ViewBag.MaritalStatusList = maritalStatusList;
                ViewBag.EducationListList = educationList;
                ViewBag.ProfessionList = professionList;
                ViewBag.HousingList = housingList;
                ViewBag.NationalityList = nationality;
                ViewBag.WorkTypeList = workTypeList;
                ViewBag.DesignationList = designationList;
                ViewBag.DecisionList = decisionList;
                ViewBag.ContractList = contractList;
                ViewBag.DecisionReasonList = decisionReasonList;
                ViewBag.AccountBankList = accountBankList;
                ViewBag.AccountStatus = accountStatusList;
                ViewBag.BranchList = branchList;
                ViewBag.CityList = cityList;
                ViewBag.CardBankList = cardBankInstitutionList;
                ViewBag.IncomeTypeList = incomeTypeList;
                ViewBag.ProductList = productList;
                ViewBag.CustomerDependentAgeGroup = customerDependentAgeGroup;
                ViewBag.CustomerValue = new JavaScriptSerializer().Serialize(customerDTO);

                return View("CustomerAdd", customerDTO);
            }
            else
                return RedirectToAction("ApprovalQueueList", "Approval");
        }
        #endregion

        #region GetProductByProductCategory
        public JsonResult GetProductByProductCategory(string Id)
        {
            var products = tLookUpManager.GetAll<ProductDTO>(Id, "ProductByProductCategory");
            return Json(products, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region GetProductMemberTypeByProductCode
        public JsonResult GetProductMemberTypeByProductCode(string AProductCode)
        {
            List<SelectListItem> lstSelectedPTypes = new List<SelectListItem>();
            if (!string.IsNullOrWhiteSpace(AProductCode))
            {
                ProductDTO ProductDTO = tMasterManager.FindByProductCode(Convert.ToString(AProductCode), Utils.OrganizationCode);

                // Added to set datasource for the selected product memeber types listbox
                var ProductTypeSelectedItems = ProductDTO.ProductMemberTypeCode;
                string[] PSelectedItems = ProductTypeSelectedItems.Split(',');
                foreach (string items in PSelectedItems)
                {
                    List<SelectListItem> lst = new List<SelectListItem>();
                    lst = tLookUpManager.GetAll<ProductMemberTypeDTO>(items, "ProductMemberTypeByCode");
                    lst.RemoveAt(0);
                    lstSelectedPTypes = lstSelectedPTypes.Union(lst).ToList();
                }
                lstSelectedPTypes.Insert(0, new SelectListItem() { Text = Resources.SelectOne, Value = string.Empty });
            }

            return Json(lstSelectedPTypes, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region ValidateCustomerDesignationForHRP
        public JsonResult ValidateCustomerDesignationForHRP(string ADesignationCode, bool AIsRequestFromUnderWriter, bool AIsAutoApprove )
        {
            bool isHRP = false;
            string Message = string.Empty;
            isHRP = tMasterManager.ValidateCustomerDesignationForHRP(ADesignationCode);
            if (isHRP)
            {
                if (AIsRequestFromUnderWriter || !AIsAutoApprove)
                {
                    Message = @Resources.MsgErrDesignationHRP;
                }
                else
                {
                    Message = @Resources.MsgErrDesignationHRP + @Resources.MsgConfApproval;
                }
            }
            else
            {
                Message = string.Empty;
            }
            return Json(new { Message, isHRP }, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region ValidateCreditScore
        public JsonResult ValidateCreditScore(string AProductCode, string ACreditScore, bool AIsRequestFromUnderWriter, bool AIsAutoApproval)
        {
            bool isCreditScoreViolated = false;
            string Message = string.Empty;
            isCreditScoreViolated = tMasterManager.ValidateCreditScore(AProductCode, Convert.ToDecimal(ACreditScore));
            if (isCreditScoreViolated)
            {
                if (AIsRequestFromUnderWriter || !AIsAutoApproval)
                {
                    Message = Resources.MsgErrCombinedScoreLess;
                }
                else
                {
                    Message = Resources.MsgErrCombinedScoreLess + Resources.MsgConfApproval;
                }
            }
            else
            {
                Message = string.Empty;
            }
            return Json(new { Message, isCreditScoreViolated }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region GetAutoApprovalOfSelectedProduct
        public JsonResult GetAutoApprovalOfSelectedProduct(string AProductCode)
        {
            bool autoApproval = false;
            autoApproval = tMasterManager.GetAutoApprovalPropertyOfProduct(AProductCode);
            return Json(new { IsAutoApproval = autoApproval }, JsonRequestBehavior.AllowGet);
        } 
        #endregion


        #endregion

        #region Customer without SalesId
        public JsonResult GetCustomerWithoutSalesIdDetailsIfExists(string ARUTNumber,string AProductMemberType, string AParentCustId)
        {
            string strMessage = string.Empty;
            CustomerInfo objCustomerInfo = tMasterManager.ValidateCustomerWithoutSalesId(ARUTNumber,AProductMemberType, AParentCustId, Utils.OrganizationCode);          
            if (objCustomerInfo.ReturnValue == 1)
            {
                strMessage = Resources.MsgErrCustomerAlreadyExistsWithSalesId;
            }
            else if (objCustomerInfo.ReturnValue == 4) //new customer
            {
                strMessage = String.Format(Resources.MsgErrPleaseSelect, Resources.ProductMemberType); 
                
            }
            return Json(new { WarningMessage = strMessage, ProductMemberType = objCustomerInfo.ProductMemberTypeCode, ParentCustomerId = objCustomerInfo.ParentCustomerId, ParentCustomerName = objCustomerInfo.ParentCustomerName }, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region Add On Customer
        #region AddOnCustomerAdd
        [AuthorizeUserAttribute(Entity = "MT_CUSTOMER", AccessLevel = new UserActivities[] { UserActivities.Add, UserActivities.Update, UserActivities.View })]
        public ActionResult AddOnCustomerAdd(string ACustomerId, string AParentCustomerId, string AProductMemberTypeCode)
        {
            CustomerDTO customerDTO;
            /*Sales and policy*/
            //var branchList = tLookUpManager.GetAll<BranchDTO>("", "Branch");
            var organizationList = tLookUpManager.GetAll<OrganizationDTO>("", "Organization");
            var productTypeList = tLookUpManager.GetAll<ProductTypeDTO>("", "ProductType");
            var productCategoryList = tLookUpManager.GetAll<ProductCategoryDTO>("", "ProductCategory");
            var campaignSourceList = tLookUpManager.GetAll<SourcingDTO>("", "Sourcing");//Utils.GetCampaignSource();
            /*Personal Details*/
            var customerType = Utils.GetCustomerType();
            var genderList = Utils.GetGender();
            var maritalStatusList = tLookUpManager.GetAll<ProductDTO>("", "MaritalStatus");
            var educationList = tLookUpManager.GetAll<ProductDTO>("", "Education");
            var professionList = tLookUpManager.GetAll<ProductDTO>("", "Profession");
            var housingList = Utils.GetHousing();
            var nationality = tLookUpManager.GetAll<CountryDTO>("", "Nationality");
            /*Customer Address*/
            var addressTypeList = Utils.GetCustomerAddressType();
            var residenceClearanceList = Utils.GetResidenceClearanceType();
            /*Customer Occupation*/
            var workTypeList = Utils.GetWorkType();
            var incomeTypeList = Utils.GetIncomeType();
            var designationList = tLookUpManager.GetAll<DesignationDTO>("", "Designation");
            /*Customer Decision*/
            var decisionList = tLookUpManager.GetAll<DecisionDTO>("", "Decision");
            var contractList = tLookUpManager.GetAll<ContractDTO>("", "Contract");
            var decisionReasonList = Utils.GetCustomerDecisionReason();//tLookUpManager.GetAll<DecisionReasonDTO>("", "DecisionReason");//
            var unemploymentList = Utils.GetUnemployment();
            /*Customer House Consumption Details*/
            var typeList = Utils.GetCommercialHouseConsumptionType();
            var accountStatusList = Utils.GetAccountStatus();
            /*Customer Current Account details*/
            var bankTypeList = Utils.GetBankType();
            var accountBankList = tLookUpManager.GetAll<BankDTO>("", "Bank");
            /*Customer Billing*/
            //var branchList = tLookUpManager.GetAll<BranchDTO>("", "Branch");
            LoginUserDTO loggedInUser = (LoginUserDTO)System.Web.HttpContext.Current.Session["LOGGED_IN_USER"];
            var branchList = tLookUpManager.GetAll<UserAllowedBranchDTO>(loggedInUser.Username, "UserBranch");
            /*Customer Address*/
            //var regionList = tLookUpManager.GetAll<RegionDTO>("IND", "Region");
            var cityList = tLookUpManager.GetAll<CityDTO>("", "City");
            //var communaList = tLookUpManager.GetAll<CommunaDTO>("IND", "Communa");
            /*Customer Credit Card Detail*/
            var cardBankInstitutionList = tLookUpManager.GetAll<BankDTO>("", "Bank");
            var cardTypeList = tLookUpManager.GetAll<CardTypeDTO>("", "CardType");
            var cardBrandList = Utils.GetCardBrand();
            var customerDependentAgeGroup = Utils.GetCustomerDependentAgeGroup();

            if (ACustomerId == null && !string.IsNullOrEmpty(AParentCustomerId))
            {
                customerDTO = tMasterManager.FindByCustomerId(Convert.ToInt32(AParentCustomerId));

                customerDTO.CustomerId = 0;
                customerDTO.ParentCustomerId = Convert.ToInt32(AParentCustomerId);
                customerDTO.ParentCustomerName = string.Concat(customerDTO.Name, " ", customerDTO.PaternalSurname);
                customerDTO.ProductMemberTypeCode = AProductMemberTypeCode;
                customerDTO.CustomerType = string.Empty;
                customerDTO.Name = string.Empty;
                customerDTO.PaternalSurname = string.Empty;
                customerDTO.MaternalSurname = string.Empty;
                customerDTO.Nationality = string.Empty;
                customerDTO.DateOfBirth = Convert.ToDateTime(Utils.BusinessDate);
                customerDTO.Gender = string.Empty;
                customerDTO.MaritalStatus = string.Empty;
                customerDTO.NumberofDependents = 0;
                customerDTO.Education = string.Empty;
                customerDTO.Profession = string.Empty;
                customerDTO.Housing = string.Empty;
                customerDTO.EmailID = string.Empty;
                customerDTO.LegacyCardNumber = string.Empty;

                customerDTO.IsCardGenerated = false;
                customerDTO.ApprovalStatus = string.Empty;
                customerDTO.CreatedBy = customerDTO.LastUpdatedBy = customerDTO.ApprovedBy = string.Empty;
                customerDTO.CreatedOn = customerDTO.LastUpdatedOn = Convert.ToDateTime(null);
                customerDTO.ApprovedOn = null;

                customerDTO.CustomerAddressDTO = new List<CustomerAddressDTO>();
                customerDTO.CustomerOccupationDTO = new List<CustomerOccupationDTO>();
                customerDTO.CustomerBillingDetailDTO = new List<CustomerBillingDetailDTO>();

                customerDTO.CustomerCommericalHouseConsumptionDTO.ForEach(x => x.CustomerId = 0);
                customerDTO.CustomerDecisionDTO.ForEach(x => x.CustomerId = 0);
                customerDTO.CustomerDependentDTO.ForEach(x => x.CustomerId = 0);
                customerDTO.CustomerCreditCardDetailDTO.ForEach(x => x.CustomerId = 0);
                customerDTO.CustomerCurrentAccountDetailDTO.ForEach(x => x.CustomerId = 0);
            }
            else
            {
                customerDTO = tMasterManager.FindByCustomerId(Convert.ToInt32(ACustomerId));
            }
            if (!String.IsNullOrWhiteSpace(customerDTO.ApprovalStatus) && customerDTO.ApprovalStatus.Equals("P"))
            {
                var result = new List<SelectListItem>();
                bool isCreditScoreViolated = tMasterManager.ValidateCreditScore(customerDTO.ProductCode, customerDTO.CreditScore);
                bool isHRP = tMasterManager.ValidateCustomerDesignationForHRP(customerDTO.CustomerOccupationDTO.FirstOrDefault().Post);
                string ContextCode = string.Empty;
                if ((isCreditScoreViolated || isHRP) && customerDTO.AutoApprove)
                {
                    ContextCode = "CUSTV";
                }
                else if (!customerDTO.AutoApprove)
                {
                    ContextCode = "CUST";
                }
                var reasonCodes = tLookUpManager.GetAll<ApprovalContextDTO>(ContextCode, "ReasonContext");
                if (ContextCode == "CUSTV")
                {
                    result = reasonCodes.Where(x => x.Value != "CUSTVPR").Select(x => x).ToList();
                    if (!isCreditScoreViolated)
                    {
                        result.RemoveAll(x => x.Value == "CUSTVUR");                    
                    }
                }
                if (ContextCode == "CUST")
                {
                    result = reasonCodes.Where(x => x.Value != "CUSTPR").Select(x => x).ToList();
                    if (!isCreditScoreViolated)
                    {
                        result.RemoveAll(x => x.Value == "CUSTUR");
                    }
                }
                ViewBag.ReasonCodes = result;
            }

            var productList = tLookUpManager.GetAll<ProductDTO>(customerDTO.ProductCategoryCode, "ProductByProductCategory");

            ViewBag.Privileges = tCommonManager.GetUserPrivilegesForEntity("MT_CUSTOMER");
            ViewData["FormStatus"] = customerDTO.ApprovalStatus;

            ViewBag.OrganizationList = organizationList;
            ViewBag.ProductTypeList = productTypeList;
            ViewBag.ProductCategoryList = productCategoryList;
            ViewBag.CampaignSourceList = campaignSourceList;

            ViewBag.AddressTypeList = addressTypeList;
            ViewBag.ResidenceClearanceList = residenceClearanceList;
            ViewBag.UnemploymentList = unemploymentList;
            ViewBag.TypeList = typeList;
            ViewBag.BankTypeList = bankTypeList;
            ViewBag.CardBankInstitutionList = cardBankInstitutionList;
            ViewBag.CardTypeList = cardTypeList;
            ViewBag.CardBrandList = cardBrandList;

            ViewBag.CustomerTypeList = customerType;
            ViewBag.GenderList = genderList;
            ViewBag.MaritalStatusList = maritalStatusList;
            ViewBag.EducationListList = educationList;
            ViewBag.ProfessionList = professionList;
            ViewBag.HousingList = housingList;
            ViewBag.NationalityList = nationality;
            ViewBag.WorkTypeList = workTypeList;
            ViewBag.DesignationList = designationList;
            ViewBag.DecisionList = decisionList;
            ViewBag.ContractList = contractList;
            ViewBag.DecisionReasonList = decisionReasonList;
            ViewBag.AccountBankList = accountBankList;
            ViewBag.AccountStatus = accountStatusList;
            ViewBag.BranchList = branchList;
            //ViewBag.RegionList = regionList;
            ViewBag.CityList = cityList;
            //ViewBag.CommunaList = communaList;
            ViewBag.CardBankList = cardBankInstitutionList;
            ViewBag.IncomeTypeList = incomeTypeList;
            ViewBag.ProductList = productList;
            ViewBag.CustomerDependentAgeGroup = customerDependentAgeGroup;

            ViewBag.CustomerValue = new JavaScriptSerializer().Serialize(customerDTO);
            return View(customerDTO);
        }
        #endregion

        #region GetCustomerDetailsIfExists
        public JsonResult GetCustomerDetailsIfExists(string ARUTNumber, string ASalesId)
        {
            string strMessage = string.Empty;
            CustomerInfo objCustomerInfo = tMasterManager.ValidateCustomer(ARUTNumber, ASalesId, Utils.OrganizationCode, ((LoginUserDTO)(System.Web.HttpContext.Current.Session["LOGGED_IN_USER"])).Username);
            if (objCustomerInfo.ReturnValue == 4)
            {
                strMessage = Resources.MsgErrInvalidParentCustomer;
            }
            else if (objCustomerInfo.ReturnValue == 5)
            {
                strMessage = Resources.MsgErrUnAuthParentCustomer;
            }
            else if (objCustomerInfo.ReturnValue == 7)
            {
                strMessage = Resources.MsgErrMaxAddOnCountExceed;
            }
            else if (objCustomerInfo.ReturnValue == 8)
            {
                strMessage = Resources.MsgErrInvalidSalesIdBranch;
            }
            else if (objCustomerInfo.ReturnValue == 9)
            {
                strMessage = Resources.MsgErrCustomerAlreadyExistsInOtherMode;
            }

            return Json(new { WarningMessage = strMessage, ProductMemberType = objCustomerInfo.ProductMemberTypeCode, ParentCustomerId = objCustomerInfo.ParentCustomerId, ParentCustomerName = objCustomerInfo.ParentCustomerName }, JsonRequestBehavior.AllowGet);            
        } 
        #endregion

        #endregion

        #region Customer Underwriting
        #region ListCustomersToUnderwrite
        [AuthorizeUserAttribute(Entity = "MT_CUSTOMER_UNDERWRITE", AccessLevel = new UserActivities[] { UserActivities.View })]
        public ActionResult ListCustomersToUnderwrite(CustomerSearch AVMCustomerSearch, string ClearButton, int? page)
        {
            try
            {
                var customerType = Utils.GetCustomerType();
                var customerStatus = Utils.GetCustomerStatus();
                int iPageNumber = Convert.ToInt32((page == null) ? 0 : page);
                if (ClearButton == Resources.Clear)
                {
                    AVMCustomerSearch.Name = AVMCustomerSearch.RUT = AVMCustomerSearch.CustomerType = null;
                    ModelState.Clear();
                }
                if ((Utils.OrganizationCode != "") || (!(string.IsNullOrEmpty(AVMCustomerSearch.SearchButton)) &&
                     !((string.IsNullOrWhiteSpace(AVMCustomerSearch.RUT) &&
                     (string.IsNullOrWhiteSpace(AVMCustomerSearch.CustomerType)) &&
                     (string.IsNullOrWhiteSpace(AVMCustomerSearch.Name)) &&
                     (string.IsNullOrWhiteSpace(AVMCustomerSearch.CustomerStatus))))))
                {
                    var entities = tMasterManager.GetCustomersForListing(AVMCustomerSearch.RUT == null ? null : AVMCustomerSearch.RUT.Replace("-", string.Empty),
                       AVMCustomerSearch.CustomerType, AVMCustomerSearch.Name,AVMCustomerSearch.CustomerStatus, Utils.OrganizationCode, true);
                    AVMCustomerSearch.SearchResults = entities.AsQueryable();

                    //var entities = tMasterManager.ListCustomersToUnderwrite(new CustomerDTO()
                    //{
                    //    RUT = AVMCustomerSearch.RUT,
                    //    CustomerType = AVMCustomerSearch.CustomerType,
                    //    Name = AVMCustomerSearch.Name
                    //});
                    AVMCustomerSearch.SearchResults = entities.AsQueryable();
                }

                AVMCustomerSearch.isUnderWriter = true;
                ViewBag.CustomerType = customerType;
                ViewBag.CustomerStatus = customerStatus;

                return View("CustomerList", AVMCustomerSearch);
            }
            catch (Exception ex)
            {
                LoggerFactory.CreateLog().LogError("Error" + ex.Message, 100, 2);
                ViewBag.ErrorMessage = Resources.MsgErrServerException;
                ViewBag.ErrorType = SessionErrorType.ServerException;
                return View("Error");
            }
        }
        #endregion
        #region ViewCustomerToUnderwrite
        [AuthorizeUserAttribute(Entity = "MT_CUSTOMER_UNDERWRITE", AccessLevel = new UserActivities[] { UserActivities.View })]
        public ActionResult ViewCustomerToUnderwrite(string ACustomerId)
        {
            try
            {
                CustomerDTO customerDTO;
                /*Sales and policy*/
                var organizationList = tLookUpManager.GetAll<OrganizationDTO>("", "Organization");
                var productTypeList = tLookUpManager.GetAll<ProductTypeDTO>("", "ProductType");
                var productCategoryList = tLookUpManager.GetAll<ProductCategoryDTO>("", "ProductCategory");
                var campaignSourceList = tLookUpManager.GetAll<SourcingDTO>("", "Sourcing");
                /*Personal Details*/
                var customerType = Utils.GetCustomerType();
                var genderList = Utils.GetGender();
                var maritalStatusList = tLookUpManager.GetAll<ProductDTO>("", "MaritalStatus");
                var educationList = tLookUpManager.GetAll<ProductDTO>("", "Education");
                var professionList = tLookUpManager.GetAll<ProductDTO>("", "Profession");
                var housingList = Utils.GetHousing();
                var nationality = tLookUpManager.GetAll<CountryDTO>("", "Nationality");
                /*Customer Address*/
                var addressTypeList = Utils.GetCustomerAddressType();
                var residenceClearanceList = Utils.GetResidenceClearanceType();
                /*Customer Occupation*/
                var workTypeList = Utils.GetWorkType();
                var incomeTypeList = Utils.GetIncomeType();
                var designationList = tLookUpManager.GetAll<DesignationDTO>("", "Designation");
                /*Customer Decision*/
                var decisionList = tLookUpManager.GetAll<DecisionDTO>("", "Decision");
                var contractList = tLookUpManager.GetAll<ContractDTO>("", "Contract");
                var decisionReasonList = Utils.GetCustomerDecisionReason();
                var unemploymentList = Utils.GetUnemployment();
                /*Customer House Consumption Details*/
                var typeList = Utils.GetCommercialHouseConsumptionType();
                var accountStatusList = Utils.GetAccountStatus();
                /*Customer Current Account details*/
                var bankTypeList = Utils.GetBankType();
                var accountBankList = tLookUpManager.GetAll<BankDTO>("", "Bank");
                /*Customer Billing*/
                LoginUserDTO loggedInUser = (LoginUserDTO)System.Web.HttpContext.Current.Session["LOGGED_IN_USER"];
                var branchList = tLookUpManager.GetAll<UserAllowedBranchDTO>(loggedInUser.Username, "UserBranch");
                //var branchList = tLookUpManager.GetAll<BranchDTO>("", "Branch");
                /*Customer Address*/
                var cityList = tLookUpManager.GetAll<CityDTO>("", "City");
                /*Customer Credit Card Detail*/
                var cardBankInstitutionList = tLookUpManager.GetAll<BankDTO>("", "Bank");
                var cardTypeList = tLookUpManager.GetAll<CardTypeDTO>("", "CardType");
                var cardBrandList = Utils.GetCardBrand();
                var customerDependentAgeGroup = Utils.GetCustomerDependentAgeGroup();

                customerDTO = tMasterManager.FindByCustomerId(Convert.ToInt32(ACustomerId));
                if (!String.IsNullOrWhiteSpace(customerDTO.ApprovalStatus) && customerDTO.ApprovalStatus.Equals("P"))//no need
                {
                    var reasonCodes = tLookUpManager.GetAll<ApprovalContextDTO>("CUST", "ReasonContext");
                    ViewBag.ReasonCodes = reasonCodes;
                }
                var productList = tLookUpManager.GetAll<ProductDTO>(customerDTO.ProductCategoryCode, "ProductByProductCategory");
                ViewBag.Privileges = tCommonManager.GetUserPrivilegesForEntity("MT_CUSTOMER_UNDERWRITE");
                ViewData["FormStatus"] = customerDTO.ApprovalStatus;
                ViewBag.FromSearchPage = true;
                ViewBag.OrganizationList = organizationList;
                ViewBag.ProductTypeList = productTypeList;
                ViewBag.ProductCategoryList = productCategoryList;
                ViewBag.CampaignSourceList = campaignSourceList;

                ViewBag.AddressTypeList = addressTypeList;
                ViewBag.ResidenceClearanceList = residenceClearanceList;
                ViewBag.UnemploymentList = unemploymentList;
                ViewBag.TypeList = typeList;
                ViewBag.BankTypeList = bankTypeList;
                ViewBag.CardBankInstitutionList = cardBankInstitutionList;
                ViewBag.CardTypeList = cardTypeList;
                ViewBag.CardBrandList = cardBrandList;

                ViewBag.CustomerTypeList = customerType;
                ViewBag.GenderList = genderList;
                ViewBag.MaritalStatusList = maritalStatusList;
                ViewBag.EducationListList = educationList;
                ViewBag.ProfessionList = professionList;
                ViewBag.HousingList = housingList;
                ViewBag.NationalityList = nationality;
                ViewBag.WorkTypeList = workTypeList;
                ViewBag.DesignationList = designationList;
                ViewBag.DecisionList = decisionList;
                ViewBag.ContractList = contractList;
                ViewBag.DecisionReasonList = decisionReasonList;
                ViewBag.AccountBankList = accountBankList;
                ViewBag.AccountStatus = accountStatusList;
                ViewBag.BranchList = branchList;
                ViewBag.CityList = cityList;
                ViewBag.CardBankList = cardBankInstitutionList;
                ViewBag.IncomeTypeList = incomeTypeList;
                ViewBag.ProductList = productList;
                ViewBag.CustomerDependentAgeGroup = customerDependentAgeGroup;

                ViewBag.CustomerValue = new JavaScriptSerializer().Serialize(customerDTO);
                return View("UnderwriterCustomer", customerDTO);
            }
            catch (Exception ex)
            {
                LoggerFactory.CreateLog().LogError("Error" + ex.Message, 100, 2);
                ViewBag.ErrorMessage = Resources.MsgErrServerException;
                ViewBag.ErrorType = SessionErrorType.ServerException;
                return View("Error");
            }
        }
        #endregion
        #region UpdateCustomerToUnderwrite
        [HttpPost]
        [AuthorizeUserAttribute(Entity = "MT_CUSTOMER_UNDERWRITE", AccessLevel = new UserActivities[] { UserActivities.Add, UserActivities.Update })]
        public ActionResult UpdateCustomerToUnderwrite(CustomerDTO ACustomerDTO)
        {
            string redirectUrl = string.Empty, messageToShow = string.Empty;
            try
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors);

                if (ModelState.IsValid)
                {
                    ACustomerDTO.AutoApprove = false;
                    ACustomerDTO.LastUpdatedBy = tCommonManager.GetLoggedUserName();
                    tMasterManager.UpdateCustomerDetails(ACustomerDTO.CustomerId, ACustomerDTO);

                    redirectUrl = Url.Action("ListCustomersToUnderwrite", "Master");
                    messageToShow = String.Format(Resources.MsgInfoSaveSuccess, Resources.Customer);
                }
            }
            catch (Exception ex)
            {
                LoggerFactory.CreateLog().LogError("Error" + ex.Message, 100, 2);
                messageToShow = String.Format(Resources.MsgErrSaveFailure, Resources.Customer);
            }
            return Json(new { RedirectUrl = redirectUrl, Message = messageToShow }, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region LoadUnderwriterAuthorizePopup
        public ActionResult LoadUnderwriterAuthorizePopup(int ACustomerId, int ACreditScore, decimal ACreditLimit, string ARemarks)
        {
            bool isCreditScoreViolated = false, isHRP = false;
            string ContextCode = string.Empty;
            if (Utils.OrganizationTypeCode == "IND")
            {
               IndustrialCustomerDTO  industrialCustomerDTO = tMasterManager.GetIndustrialCustomerByCustomerId(Convert.ToInt32(ACustomerId));
               isCreditScoreViolated = tMasterManager.ValidateCreditScore(industrialCustomerDTO.ProductCode, industrialCustomerDTO.CreditBureauScore);
               ContextCode = "CUST";               
            }
            else
            {
                CustomerDTO customerDTO = tMasterManager.FindByCustomerId(Convert.ToInt32(ACustomerId));
                isCreditScoreViolated = tMasterManager.ValidateCreditScore(customerDTO.ProductCode, customerDTO.CreditScore);
                isHRP = tMasterManager.ValidateCustomerDesignationForHRP(customerDTO.CustomerOccupationDTO.FirstOrDefault().Post);
                if ((isCreditScoreViolated || isHRP) && customerDTO.AutoApprove)
                {
                    ContextCode = "CUSTV";
                }
                else if (!customerDTO.AutoApprove)
                {
                    ContextCode = "CUST";
                }
            }             
         
            var result = new List<SelectListItem>();            
            var reasonCodes = tLookUpManager.GetAll<ApprovalContextDTO>(ContextCode, "ReasonContext");
            if (ContextCode == "CUSTV")
            {
                result = reasonCodes.Where(x => x.Value == "CUSTVA" || x.Value == "CUSTVPR" || x.Value == string.Empty).Select(x => x).ToList();
            }
            if (ContextCode == "CUST")
            {
                result = reasonCodes.Where(x => x.Value == "CUSTA" || x.Value == "CUSTPR" || x.Value == string.Empty).Select(x => x).ToList();                
            }
            ViewBag.ReasonCodes = result;
            ViewBag.CustomerId = ACustomerId;
            ViewBag.CreditScore = ACreditScore;
            ViewBag.CreditLimit = ACreditLimit;
            ViewBag.Remarks = ARemarks;

            return PartialView("_UnderwriterAuthorize");
        }
        #endregion
        #region AuthorizationByUnderwriter
        public ActionResult AuthorizationByUnderwriter(FormCollection AFormCollection, int ACustomerId, int ACreditScore, decimal ACreditLimit, string ARemarks)
        {
            int retValue = 0;
            string approvalStatus, reasonCode, reasonDescription, approvedBy;

            reasonCode = AFormCollection["ReasonCode"];
            reasonDescription = AFormCollection["ReasonDescription"];
            approvalStatus = AFormCollection["AuthorizeOption"] == "Approve" ? "A" : "PR";
            approvedBy = tCommonManager.GetLoggedUserName();

            retValue = tMasterManager.AuthorizationByUnderwriter(ACustomerId, ACreditScore, ACreditLimit, ARemarks, approvalStatus, approvedBy, reasonCode, reasonDescription);

            return RedirectToAction("ListCustomersToUnderwrite");
        }
        #endregion
        #endregion

        #region Industrial Customer
        #region IndustrialCustomerAdd
        [AuthorizeUserAttribute(Entity = "MT_CUSTOMER", AccessLevel = new UserActivities[] { UserActivities.View })]
        public ActionResult IndustrialCustomerAdd(string ACustomerId, string ARUT, string AProductMemberTypeCode, string ASalesId, string AParentCustomerId)
        {
            try
            {
                IndustrialCustomerDTO industrialCustomerDTO;

                LoginUserDTO loggedInUser = (LoginUserDTO)System.Web.HttpContext.Current.Session["LOGGED_IN_USER"];
                var branchList = tLookUpManager.GetAll<UserAllowedBranchDTO>(loggedInUser.Username, "UserBranch");
                var billingCycleCodeList = tLookUpManager.GetAll<BillingCycleDTO>("", "BillingCycle");
                var customerTypeList = Utils.GetCustomerType();
                var organizationList = tLookUpManager.GetAll<OrganizationDTO>("", "Organization");
                //var cityList = tLookUpManager.GetAll<CityDTO>("", "City");
                var bankList = tLookUpManager.GetAll<BankDTO>("", "Bank");
                var residenceTypeList = Utils.GetResidenceType();
                var addressTypeList = Utils.GetCustomerAddressType().Where(x => (x.Value == string.Empty || x.Value == "C" || x.Value == "PR"));
                var productCategoryList = tLookUpManager.GetAll<ProductCategoryDTO>("", "ProductCategory");
                var nationality = tLookUpManager.GetAll<CountryDTO>("", "Nationality");
                /*Customer Decision*/
                var decisionList = tLookUpManager.GetAll<DecisionDTO>("", "Decision");
                var decisionReasonList = Utils.GetCustomerDecisionReason();
                var unemploymentList = Utils.GetUnemployment();
                DirectSalesAgentDTO objDirectSalesAgentDTO = null;

                if (!string.IsNullOrEmpty(ASalesId))
                    objDirectSalesAgentDTO = tMasterManager.GetDirectSalesAgentDetails(ARUT, ASalesId);
                if (string.IsNullOrEmpty(ACustomerId) || ACustomerId == "0")
                {
                    industrialCustomerDTO = new IndustrialCustomerDTO();
                    industrialCustomerDTO.OrganizationCode = Utils.OrganizationCode;
                    industrialCustomerDTO.ProductMemberTypeCode = AProductMemberTypeCode;
                    industrialCustomerDTO.ParentCustomerId = Convert.ToInt32(AParentCustomerId);
                    if (!string.IsNullOrEmpty(AParentCustomerId) && AParentCustomerId != "0")
                    {
                        IndustrialCustomerDTO objParentCustomer = tMasterManager.GetIndustrialCustomerByCustomerId(Convert.ToInt32(AParentCustomerId));
                        if (objParentCustomer != null)
                        {
                            industrialCustomerDTO.ParentCustomerId = Convert.ToInt32(AParentCustomerId);
                            industrialCustomerDTO.ParentCustomerName = objParentCustomer.Name + " " + objParentCustomer.PaternalSurname;
                            industrialCustomerDTO.ProductCategoryCode = objParentCustomer.ProductCategoryCode;
                            industrialCustomerDTO.ProductCode = objParentCustomer.ProductCode;
                            industrialCustomerDTO.ObjIndustrialCustomerAddressDTO.Add(new IndustrialCustomerAddressDTO() { AddressTypeCode = "C" });
                            industrialCustomerDTO.ObjIndustrialCustomerAddressDTO.Add(new IndustrialCustomerAddressDTO() { AddressTypeCode = "PR" });
                            //Statement address of Addon customer will be that of its parent.
                            IndustrialCustomerAddressDTO statementAddress = new IndustrialCustomerAddressDTO();
                            statementAddress = objParentCustomer.ObjIndustrialCustomerAddressDTO.Where(x => x.AddressTypeCode == "ST").FirstOrDefault();
                            industrialCustomerDTO.ObjIndustrialCustomerAddressDTO.Add(new IndustrialCustomerAddressDTO()
                            {
                                AddressTypeCode = statementAddress.AddressTypeCode,
                                ReferenceAddressTypeCode = statementAddress.ReferenceAddressTypeCode,
                                AddressDesc = statementAddress.AddressDesc,
                                Village = statementAddress.Village,
                                CommunaCode = statementAddress.CommunaCode,
                                CityCode = statementAddress.CityCode,
                                Phone = statementAddress.Phone,
                                Mobile = statementAddress.Mobile,
                                EmailID = statementAddress.EmailID,
                                ResidenceType = statementAddress.ResidenceType,
                                Other = statementAddress.Other
                            });

                            industrialCustomerDTO.ObjIndustrialCustomerDecisionDTO = objParentCustomer.ObjIndustrialCustomerDecisionDTO;
                            industrialCustomerDTO.ObjIndustrialCustomerDecisionDTO.ForEach(x => x.CustomerId = 0);
                            industrialCustomerDTO.BranchCode = objParentCustomer.BranchCode;
                        }
                    }

                    if (objDirectSalesAgentDTO != null)
                    {
                        industrialCustomerDTO.DSACode = objDirectSalesAgentDTO.DSACode;
                        industrialCustomerDTO.DSAName = objDirectSalesAgentDTO.DSAName;
                        industrialCustomerDTO.SalesId = ASalesId;
                        industrialCustomerDTO.UEN = objDirectSalesAgentDTO.UEN;
                        industrialCustomerDTO.DSARUT = objDirectSalesAgentDTO.RUT;
                        industrialCustomerDTO.BranchCode = objDirectSalesAgentDTO.CommercialBranchCode;
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(industrialCustomerDTO.BranchCode))
                        {
                            objDirectSalesAgentDTO = tMasterManager.FindDSAByCommercialBranchCode(industrialCustomerDTO.BranchCode);
                            industrialCustomerDTO.DSACode = objDirectSalesAgentDTO.DSACode;
                            industrialCustomerDTO.DSAName = objDirectSalesAgentDTO.DSAName;
                            industrialCustomerDTO.UEN = objDirectSalesAgentDTO.UEN;
                            industrialCustomerDTO.DSARUT = objDirectSalesAgentDTO.RUT;
                        }
                    }
                }
                else
                {
                    industrialCustomerDTO = tMasterManager.GetIndustrialCustomerByCustomerId(Convert.ToInt32(ACustomerId));

                    if (objDirectSalesAgentDTO == null)
                    {
                        objDirectSalesAgentDTO = tMasterManager.FindDSAByCommercialBranchCode(industrialCustomerDTO.BranchCode);
                    }

                    industrialCustomerDTO.DSACode = objDirectSalesAgentDTO.DSACode;
                    industrialCustomerDTO.DSAName = objDirectSalesAgentDTO.DSAName;
                    industrialCustomerDTO.UEN = objDirectSalesAgentDTO.UEN;
                    industrialCustomerDTO.DSARUT = objDirectSalesAgentDTO.RUT == null ? "" : objDirectSalesAgentDTO.RUT.Replace("-", "");

                }

                if (industrialCustomerDTO.ParentCustomerId != 0)
                {
                    var productList = tLookUpManager.GetAll<ProductDTO>(industrialCustomerDTO.ProductCategoryCode, "ProductByProductCategory");
                    ViewBag.ProductList = productList;
                }
                decimal maxCreditLimitOfProduct = tMasterManager.FindByProductCode(industrialCustomerDTO.ProductCode, Utils.OrganizationCode).MaximumCreditLine;
                ViewData["MaxCreditLimitOfProduct"] = maxCreditLimitOfProduct;
                ViewBag.Privileges = tCommonManager.GetUserPrivilegesForEntity("MT_CUSTOMER");
                ViewData["FormStatus"] = industrialCustomerDTO.ApprovalStatus;
                ViewBag.FromSearchPage = false;

                ViewBag.billingCycleCodes = billingCycleCodeList;
                ViewBag.BranchList = branchList;
                ViewBag.CustomerTypeList = customerTypeList;
                ViewBag.OrganizationList = organizationList;
                ViewBag.BankList = bankList;
                ViewBag.ResidenceTypeList = residenceTypeList;
                ViewBag.AddressTypeList = addressTypeList;
                ViewBag.ProductCategoryList = productCategoryList;
                ViewBag.NationalityList = nationality;
                ViewBag.IndustrialCustomerValue = new JavaScriptSerializer().Serialize(industrialCustomerDTO);
                ViewBag.DecisionList = decisionList;
                ViewBag.DecisionReasonList = decisionReasonList;
                ViewBag.UnemploymentList = unemploymentList;

                if (!string.IsNullOrEmpty(ASalesId))
                    return View("IndustrialCustomerAdd", industrialCustomerDTO);
                else
                    return View("IndustrialCustomerWithoutSalesIdAdd", industrialCustomerDTO); // Customer without salesId
            }
            catch (Exception ex)
            {
                LoggerFactory.CreateLog().LogError("Error" + ex.Message, 100, 2);
                ViewBag.ErrorMessage = Resources.MsgErrServerException;
                ViewBag.ErrorType = SessionErrorType.ServerException;
                return View("Error");
            }
        }
        #endregion
        #region IndustrialCustomerView
        [AuthorizeUserAttribute(Entity = "MT_CUSTOMER", AccessLevel = new UserActivities[] { UserActivities.View })]
        public ActionResult IndustrialCustomerView(string ACustomerId)
        {
            try
            {
                IndustrialCustomerDTO industrialCustomerDTO;
                LoginUserDTO loggedInUser = (LoginUserDTO)System.Web.HttpContext.Current.Session["LOGGED_IN_USER"];
                var branchList = tLookUpManager.GetAll<UserAllowedBranchDTO>(loggedInUser.Username, "UserBranch");
                var customerTypeList = Utils.GetCustomerType();
                var billingCycleCodeList = tLookUpManager.GetAll<BillingCycleDTO>("", "BillingCycle");
                var organizationList = tLookUpManager.GetAll<OrganizationDTO>("", "Organization");
                var cityList = tLookUpManager.GetAll<CityDTO>("", "City");
                var bankList = tLookUpManager.GetAll<BankDTO>("", "Bank");
                var residenceTypeList = Utils.GetResidenceType();
                var addressTypeList = Utils.GetCustomerAddressType().Where(x => (x.Value == string.Empty || x.Value == "C" || x.Value == "PR"));
                var productCategoryList = tLookUpManager.GetAll<ProductCategoryDTO>("", "ProductCategory");
                var nationality = tLookUpManager.GetAll<CountryDTO>("", "Nationality");
                /*Customer Decision*/
                var decisionList = tLookUpManager.GetAll<DecisionDTO>("", "Decision");
                var decisionReasonList = Utils.GetCustomerDecisionReason();
                var unemploymentList = Utils.GetUnemployment();

                industrialCustomerDTO = tMasterManager.GetIndustrialCustomerByCustomerId(Convert.ToInt32(ACustomerId));
                DirectSalesAgentDTO objDirectSalesAgentDTO;
                if (!string.IsNullOrEmpty(industrialCustomerDTO.SalesId))
                    objDirectSalesAgentDTO = tMasterManager.GetDirectSalesAgentDetails(industrialCustomerDTO.RUT, industrialCustomerDTO.SalesId);
                else
                    objDirectSalesAgentDTO = tMasterManager.FindDSAByCommercialBranchCode(industrialCustomerDTO.BranchCode);
                if (objDirectSalesAgentDTO != null)
                {
                    industrialCustomerDTO.DSACode = objDirectSalesAgentDTO.DSACode;
                    industrialCustomerDTO.DSAName = objDirectSalesAgentDTO.DSAName;
                    industrialCustomerDTO.UEN = objDirectSalesAgentDTO.UEN;
                    industrialCustomerDTO.DSARUT = objDirectSalesAgentDTO.RUT;
                }
                if (industrialCustomerDTO.ParentCustomerId != 0)
                {
                    var productList = tLookUpManager.GetAll<ProductDTO>(industrialCustomerDTO.ProductCategoryCode, "ProductByProductCategory");
                    ViewBag.ProductList = productList;
                }
                ViewBag.Privileges = tCommonManager.GetUserPrivilegesForEntity("MT_CUSTOMER");
                ViewData["FormStatus"] = "P";
                ViewBag.FromSearchPage = true;
                //ViewBag.IsCustomerForUnderWriting = false;
                ViewBag.billingCycleCodes = billingCycleCodeList;
                ViewBag.BranchList = branchList;
                ViewBag.CustomerTypeList = customerTypeList;
                ViewBag.OrganizationList = organizationList;
                ViewBag.CityList = cityList;
                ViewBag.BankList = bankList;
                ViewBag.ResidenceTypeList = residenceTypeList;
                ViewBag.AddressTypeList = addressTypeList;
                ViewBag.ProductCategoryList = productCategoryList;
                ViewBag.NationalityList = nationality;
                ViewBag.IndustrialCustomerValue = new JavaScriptSerializer().Serialize(industrialCustomerDTO);
                ViewBag.DecisionList = decisionList;
                ViewBag.DecisionReasonList = decisionReasonList;
                ViewBag.UnemploymentList = unemploymentList;
                if (!string.IsNullOrEmpty(industrialCustomerDTO.SalesId)) // Customer with salesId
                    return View("IndustrialCustomerAdd", industrialCustomerDTO);
                else
                    return View("IndustrialCustomerWithoutSalesIdAdd", industrialCustomerDTO); //Customer without salesId
            }
            catch (Exception ex)
            {
                LoggerFactory.CreateLog().LogError("Error" + ex.Message, 100, 2);
                ViewBag.ErrorMessage = Resources.MsgErrServerException;
                ViewBag.ErrorType = SessionErrorType.ServerException;
                return View("Error");
            }
        }
        #endregion
        #region IndustrialCustomerSave
        [HttpPost]
        //[AuthorizeUserAttribute(Entity = "MT_CUSTOMER", AccessLevel = new UserActivities[] { UserActivities.Add, UserActivities.Update })]
        public ActionResult IndustrialCustomerSave(IndustrialCustomerDTO AIndustrialCustomerDTO, string ACommand)
        {
            string redirectUrl = string.Empty, messageToShow = string.Empty;
            try
            {
                //var errors = ModelState.Values.SelectMany(v => v.Errors);

                //if (ModelState.IsValid)
                //{
                    if (ACommand == Resources.Save)
                    {
                        if (AIndustrialCustomerDTO.IsCustomerForUnderWriting)
                        {
                            AIndustrialCustomerDTO.ApprovalStatus = "UR";
                        }
                        else
                        {
                            AIndustrialCustomerDTO.ApprovalStatus = "D";
                        }
                    }
                    else if (ACommand == Resources.Submit)
                    {
                        AIndustrialCustomerDTO.ApprovalStatus = "P";
                    }
                    AIndustrialCustomerDTO.LastUpdatedBy = tCommonManager.GetLoggedUserName();
                    if (AIndustrialCustomerDTO.CustomerId == 0)
                    {
                        AIndustrialCustomerDTO.CreatedBy = tCommonManager.GetLoggedUserName();
                    }
                    tMasterManager.SaveIndustrialCustomerDetails(AIndustrialCustomerDTO.CustomerId, AIndustrialCustomerDTO);
                    if (AIndustrialCustomerDTO.IsCustomerForUnderWriting)
                    {
                        redirectUrl = Url.Action("ListCustomersToUnderwrite", "Master");
                    }
                    else
                    {
                    if (!string.IsNullOrEmpty(AIndustrialCustomerDTO.SalesId)) // Customer with salesId
                        redirectUrl = Url.Action("CustomerRUT", "CardBooking");
                    else
                        redirectUrl = Url.Action("CustomerRUTWithoutSalesId", "CardBooking"); //Customer without salesId
                    }
                    messageToShow = String.Format(Resources.MsgInfoSaveSuccess, Resources.Customer);
                //}
            }
            catch (Exception ex)
            {
                LoggerFactory.CreateLog().LogError("Error" + ex.Message, 100, 2);
                messageToShow = String.Format(Resources.MsgErrSaveFailure, Resources.Customer);
            }
            return Json(new { RedirectUrl = redirectUrl, Message = messageToShow }, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region IndustrialCustomerAuthorize
        [AuthorizeUserAttribute(Entity = "MT_CUSTOMER", AccessLevel = new UserActivities[] { UserActivities.Authorize })]
        public ActionResult IndustrialCustomerAuthorize(string ACustomerId, string AMode, string QId)
        {
            ViewBag.RequestQueueId = QId;
            ViewBag.TableName = "CUST";
            if (!String.IsNullOrWhiteSpace(ACustomerId))
            {
                IndustrialCustomerDTO industrialCustomerDTO = tMasterManager.GetIndustrialCustomerByCustomerId(Convert.ToInt32(ACustomerId));
                LoginUserDTO loggedInUser = (LoginUserDTO)System.Web.HttpContext.Current.Session["LOGGED_IN_USER"];
                var branchList = tLookUpManager.GetAll<UserAllowedBranchDTO>(loggedInUser.Username, "UserBranch");
                var billingCycleCodeList = tLookUpManager.GetAll<BillingCycleDTO>("", "BillingCycle");
                var customerTypeList = Utils.GetCustomerType();               
                var organizationList = tLookUpManager.GetAll<OrganizationDTO>("", "Organization");
                var cityList = tLookUpManager.GetAll<CityDTO>("", "City");
                var bankList = tLookUpManager.GetAll<BankDTO>("", "Bank");
                var residenceTypeList = Utils.GetResidenceType();
                var addressTypeList = Utils.GetCustomerAddressType().Where(x => (x.Value == string.Empty || x.Value == "C" || x.Value == "PR"));
                var productCategoryList = tLookUpManager.GetAll<ProductCategoryDTO>("", "ProductCategory");
                var nationality = tLookUpManager.GetAll<CountryDTO>("", "Nationality");               
                /*Customer Decision*/
                var decisionList = tLookUpManager.GetAll<DecisionDTO>("", "Decision");
                var decisionReasonList = Utils.GetCustomerDecisionReason();
                var unemploymentList = Utils.GetUnemployment();

                var result = new List<SelectListItem>();
                DirectSalesAgentDTO objDirectSalesAgentDTO;
                if (!string.IsNullOrEmpty(industrialCustomerDTO.SalesId))
                    objDirectSalesAgentDTO = tMasterManager.GetDirectSalesAgentDetails(industrialCustomerDTO.RUT, industrialCustomerDTO.SalesId);
                else
                    objDirectSalesAgentDTO = tMasterManager.FindDSAByCommercialBranchCode(industrialCustomerDTO.BranchCode);
                if (objDirectSalesAgentDTO != null)
                {
                    industrialCustomerDTO.DSACode = objDirectSalesAgentDTO.DSACode;
                    industrialCustomerDTO.DSAName = objDirectSalesAgentDTO.DSAName;
                    industrialCustomerDTO.UEN = objDirectSalesAgentDTO.UEN;
                    industrialCustomerDTO.DSARUT = objDirectSalesAgentDTO.RUT;
                }
                bool isCreditScoreViolated = false;
                if (industrialCustomerDTO.ParentCustomerId == 0)
                {
                    isCreditScoreViolated = tMasterManager.ValidateCreditScore(industrialCustomerDTO.ProductCode, industrialCustomerDTO.CreditBureauScore);
                }
                if (!String.IsNullOrWhiteSpace(industrialCustomerDTO.ApprovalStatus) && industrialCustomerDTO.ApprovalStatus.Equals("P"))
                {
                    string ContextCode = string.Empty;
                    if (isCreditScoreViolated)
                    {
                        ContextCode = "CUSTV";
                    }
                    else 
                    {
                        ContextCode = "CUST";
                    }
                    var reasonCodes = tLookUpManager.GetAll<ApprovalContextDTO>(ContextCode, "ReasonContext");
                    if (ContextCode == "CUSTV")
                    {
                        result = reasonCodes.Where(x => x.Value != "CUSTVPR").Select(x => x).ToList();                        
                    }
                    if (ContextCode == "CUST")
                    {
                        result = reasonCodes.Where(x => x.Value != "CUSTPR" && x.Value != "CUSTUR").Select(x => x).ToList();                       
                    }                                                          
                }
                if (industrialCustomerDTO.ParentCustomerId != 0)
                {
                    var productList = tLookUpManager.GetAll<ProductDTO>(industrialCustomerDTO.ProductCategoryCode, "ProductByProductCategory");
                    ViewBag.ProductList = productList;
                }

                ViewBag.ReasonCodes = result;
                ViewData["CreditScoreViolated"] = isCreditScoreViolated;
                ViewData["FormCall"] = "ApprovalQueue";
                ViewData["FormStatus"] = industrialCustomerDTO.ApprovalStatus;
                ViewBag.Privileges = tCommonManager.GetUserPrivilegesForEntity("MT_CUSTOMER");

                ViewBag.billingCycleCodes = billingCycleCodeList;
                ViewBag.BranchList = branchList;
                ViewBag.CustomerTypeList = customerTypeList;                
                ViewBag.OrganizationList = organizationList;
                ViewBag.CityList = cityList;
                ViewBag.BankList = bankList;
                ViewBag.ResidenceTypeList = residenceTypeList;
                ViewBag.AddressTypeList = addressTypeList;
                ViewBag.ProductCategoryList = productCategoryList;
                ViewBag.NationalityList = nationality;                
                ViewBag.IndustrialCustomerValue = new JavaScriptSerializer().Serialize(industrialCustomerDTO);
                ViewBag.DecisionList = decisionList;
                ViewBag.DecisionReasonList = decisionReasonList;
                ViewBag.UnemploymentList = unemploymentList;

                if (!string.IsNullOrEmpty(industrialCustomerDTO.SalesId)) // Customer with salesId
                    return View("IndustrialCustomerAdd", industrialCustomerDTO);
                else
                    return View("IndustrialCustomerWithoutSalesIdAdd", industrialCustomerDTO); //Customer without salesId
            }
            else
                return RedirectToAction("ApprovalQueueList", "Approval");
        }
        #endregion
        #region IndustrialCustomerDelete
        [HttpPost]
        [AuthorizeUserAttribute(Entity = "MT_CUSTOMER", AccessLevel = new UserActivities[] { UserActivities.Delete })]
        public ActionResult IndustrialCustomerDelete(int ACustomerId, bool AIsDeleteCallFromSearchTab)
        {
            string redirectUrl = string.Empty, customerName = string.Empty, message = string.Empty;
            IndustrialCustomerDTO objIndustrialCustomerDTO = new IndustrialCustomerDTO();
            try
            {
                objIndustrialCustomerDTO = tMasterManager.GetIndustrialCustomerByCustomerId(Convert.ToInt32(ACustomerId));
                objIndustrialCustomerDTO.LastUpdatedBy = tCommonManager.GetLoggedUserName();
                objIndustrialCustomerDTO.ApprovalStatus = Utils.GetApprovalStatusCode(Utils.ApprovalStatus.Deleted);
                objIndustrialCustomerDTO.OrganizationCode = Utils.OrganizationCode;
                int retValue = tMasterManager.DeleteIndustrialCustomer(ACustomerId, objIndustrialCustomerDTO);
                if (retValue == 44)
                {
                    message = String.Format(Resources.MsgInfoDeleteSuccess, Resources.Customer);
                }
                else if (retValue == 55)
                {
                    message = String.Format(Resources.MsgErrDeleteConcurrencyError, Resources.Customer);
                }
                else if (retValue == -99)
                {
                    message = Resources.MsgErrWhileDelete;
                }

                if (AIsDeleteCallFromSearchTab)
                {
                    TempData["Message"] = message;
                    redirectUrl = Url.Action("CustomerList", "Master");
                }
                else
                {
                    TempData["Message"] = string.Empty;
                    redirectUrl = Url.Action("CustomerRUT", "CardBooking");
                }
            }
            catch (Exception ex)
            {
                LoggerFactory.CreateLog().LogError("Error" + ex.Message, 100, 2);
                if (AIsDeleteCallFromSearchTab)
                {
                    TempData["Message"] = message;
                    redirectUrl = Url.Action("CustomerList", "Master");
                }
                else
                {
                    TempData["Message"] = string.Empty;
                    redirectUrl = Url.Action("CustomerRUT", "CardBooking");
                }
                return Json(new { RedirectUrl = redirectUrl, Success = false, Message = message }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { RedirectUrl = redirectUrl, Success = true, Message = message }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region IndustrialCustomerForUnderWritingView
        [AuthorizeUserAttribute(Entity = "MT_CUSTOMER_UNDERWRITE", AccessLevel = new UserActivities[] { UserActivities.View })]
        public ActionResult IndustrialCustomerForUnderWritingView(string ACustomerId)
        {
            IndustrialCustomerDTO industrialCustomerDTO;
            LoginUserDTO loggedInUser = (LoginUserDTO)System.Web.HttpContext.Current.Session["LOGGED_IN_USER"];
            var branchList = tLookUpManager.GetAll<UserAllowedBranchDTO>(loggedInUser.Username, "UserBranch");
            var billingCycleCodeList = tLookUpManager.GetAll<BillingCycleDTO>("", "BillingCycle");
            var customerTypeList = Utils.GetCustomerType();            
            var organizationList = tLookUpManager.GetAll<OrganizationDTO>("", "Organization");
            var cityList = tLookUpManager.GetAll<CityDTO>("", "City");
            var bankList = tLookUpManager.GetAll<BankDTO>("", "Bank");
            var residenceTypeList = Utils.GetResidenceType();
            var addressTypeList = Utils.GetCustomerAddressType().Where(x => (x.Value == string.Empty || x.Value == "C" || x.Value == "PR"));
            var productCategoryList = tLookUpManager.GetAll<ProductCategoryDTO>("", "ProductCategory");
            var nationality = tLookUpManager.GetAll<CountryDTO>("", "Nationality");
            /*Customer Decision*/
            var decisionList = tLookUpManager.GetAll<DecisionDTO>("", "Decision");
            var decisionReasonList = Utils.GetCustomerDecisionReason();
            var unemploymentList = Utils.GetUnemployment();

            industrialCustomerDTO = tMasterManager.GetIndustrialCustomerByCustomerId(Convert.ToInt32(ACustomerId));            
            DirectSalesAgentDTO objDirectSalesAgentDTO;
            if (!string.IsNullOrEmpty(industrialCustomerDTO.SalesId))
                objDirectSalesAgentDTO = tMasterManager.GetDirectSalesAgentDetails(industrialCustomerDTO.RUT, industrialCustomerDTO.SalesId);
            else
                objDirectSalesAgentDTO = tMasterManager.FindDSAByCommercialBranchCode(industrialCustomerDTO.BranchCode);
            if (objDirectSalesAgentDTO != null)
            {
                industrialCustomerDTO.DSACode = objDirectSalesAgentDTO.DSACode;
                industrialCustomerDTO.DSAName = objDirectSalesAgentDTO.DSAName;
                industrialCustomerDTO.UEN = objDirectSalesAgentDTO.UEN;
                industrialCustomerDTO.DSARUT = objDirectSalesAgentDTO.RUT;
            }
           
            var productList = tLookUpManager.GetAll<ProductDTO>(industrialCustomerDTO.ProductCategoryCode, "ProductByProductCategory");
            ViewBag.ProductList = productList;            
            ViewBag.Privileges = tCommonManager.GetUserPrivilegesForEntity("MT_CUSTOMER_UNDERWRITE");
            decimal maxCreditLimitOfProduct = tMasterManager.FindByProductCode(industrialCustomerDTO.ProductCode, Utils.OrganizationCode).MaximumCreditLine;
            ViewData["MaxCreditLimitOfProduct"] = maxCreditLimitOfProduct;
            ViewData["FormStatus"] = industrialCustomerDTO.ApprovalStatus;
            //ViewBag.IsCustomerForUnderWriting = true;
            ViewBag.FromSearchPage = true;
            ViewBag.billingCycleCodes = billingCycleCodeList;
            ViewBag.BranchList = branchList;
            ViewBag.CustomerTypeList = customerTypeList;         
            ViewBag.OrganizationList = organizationList;
            ViewBag.CityList = cityList;
            ViewBag.BankList = bankList;
            ViewBag.ResidenceTypeList = residenceTypeList;
            ViewBag.AddressTypeList = addressTypeList;
            ViewBag.ProductCategoryList = productCategoryList;
            ViewBag.NationalityList = nationality;
            ViewBag.IndustrialCustomerValue = new JavaScriptSerializer().Serialize(industrialCustomerDTO);
            ViewBag.DecisionList = decisionList;
            ViewBag.DecisionReasonList = decisionReasonList;
            ViewBag.UnemploymentList = unemploymentList;

            if (!string.IsNullOrEmpty(industrialCustomerDTO.SalesId)) // Customer with salesId
                return View("IndustrialCustomerAdd", industrialCustomerDTO);
            else
                return View("IndustrialCustomerWithoutSalesIdAdd", industrialCustomerDTO); //Customer without salesId
        }
        #endregion

        #endregion

        #region CalendarDateMaintenance
        #region CalendarDateMaintenanceView
        [AuthorizeUserAttribute(Entity = "MT_CALENDAR_DATE_MAINTENANCE", AccessLevel = new UserActivities[] { UserActivities.View })]
        public ActionResult CalendarDateMaintenanceView(string Id)
        {
            try
            {
                CalendarDateMaintenanceDTO calendarDateMaintenanceDTO;
                ViewBag.TableName = "CLNDR";

                ViewBag.Privileges = tCommonManager.GetUserPrivilegesForEntity("MT_CALENDAR_DATE_MAINTENANCE");

                var typeList = Utils.GetCalenderType();
                ViewBag.TypeList = typeList;
                if (String.IsNullOrWhiteSpace(Id))
                {
                    calendarDateMaintenanceDTO = new CalendarDateMaintenanceDTO();
                    calendarDateMaintenanceDTO.Date = Utils.BusinessDate;
                    ViewData["FormStatus"] = calendarDateMaintenanceDTO.ApprovalStatus;
                    return View("CalendarDateMaintenanceAdd", calendarDateMaintenanceDTO);
                }
                else
                {
                    calendarDateMaintenanceDTO = tMasterManager.FindByCalendarDateMaintenance(Convert.ToInt32(Id));
                    ViewBag.ReasonCodes = null;
                }

                ViewData["FormStatus"] = calendarDateMaintenanceDTO.ApprovalStatus;

                return View("CalendarDateMaintenanceAdd", calendarDateMaintenanceDTO);
            }
            catch (Exception ex)
            {
                LoggerFactory.CreateLog().LogError("Error" + ex.Message, 100, 2);
                ViewBag.ErrorMessage = Resources.MsgErrServerException;
                ViewBag.ErrorType = SessionErrorType.ServerException;
                return View("Error");
            }
        }
        #endregion
        #region CalendarDateMaintenanceList
        [AuthorizeUserAttribute(Entity = "MT_CALENDAR_DATE_MAINTENANCE", AccessLevel = new UserActivities[] { UserActivities.View })]
        public ActionResult CalendarDateMaintenanceList(CalendarDateMaintenanceSearch AVMCalendarDateMaintenanceSearch, string ClearButton, int? page)
        {
            try
            {

                int iPageNumber = Convert.ToInt32((page == null) ? 0 : page);

                var typeList = Utils.GetCalenderType();

                if (ClearButton == Resources.Clear)
                {
                    AVMCalendarDateMaintenanceSearch.Type = null;
                    AVMCalendarDateMaintenanceSearch.Description = null;
                    ModelState.Clear();
                }

                if ((Utils.OrganizationCode != "") || (!(string.IsNullOrEmpty(AVMCalendarDateMaintenanceSearch.SearchButton)) &&
                     !((string.IsNullOrWhiteSpace(AVMCalendarDateMaintenanceSearch.Type)
                     && (string.IsNullOrWhiteSpace(AVMCalendarDateMaintenanceSearch.Description))
                     ))))
                {
                    var entities = tMasterManager.FindAll(iPageNumber, new CalendarDateMaintenanceDTO()
                    {
                        Type = AVMCalendarDateMaintenanceSearch.Type,
                        Description = AVMCalendarDateMaintenanceSearch.Description
                    });
                    AVMCalendarDateMaintenanceSearch.SearchResults = entities.AsQueryable();
                }
                else
                {
                    var currResult = tMasterManager.FindCalendarDateMaintenance(1, Utils.GRD_ROWS_PER_PAGE).AsQueryable();
                    AVMCalendarDateMaintenanceSearch.SearchResults = currResult.AsQueryable();
                }
                ViewBag.TypeList = typeList;
                ViewBag.Privileges = tCommonManager.GetUserPrivilegesForEntity("MT_CALENDAR_DATE_MAINTENANCE");
                return View(AVMCalendarDateMaintenanceSearch);
            }
            catch (Exception ex)
            {
                LoggerFactory.CreateLog().LogError("Error" + ex.Message, 100, 2);
                ViewBag.ErrorMessage = Resources.MsgErrServerException;
                ViewBag.ErrorType = SessionErrorType.ServerException;
                return View("Error");
            }
        }
        #endregion
        #region CalendarDateMaintenanceAdd
        [AuthorizeUserAttribute(Entity = "MT_CALENDAR_DATE_MAINTENANCE", AccessLevel = new UserActivities[] { UserActivities.Add, UserActivities.Update, UserActivities.View })]
        public ActionResult CalendarDateMaintenanceAdd(string Id)
        {
            try
            {
                CalendarDateMaintenanceDTO CalendarDateMaintenanceDTO;

                var typeList = Utils.GetCalenderType();

                if (Id == null || Convert.ToInt32(Id) == 0)
                {
                    CalendarDateMaintenanceDTO = new CalendarDateMaintenanceDTO();
                    CalendarDateMaintenanceDTO.Date = Utils.BusinessDate;
                }
                else
                    CalendarDateMaintenanceDTO = tMasterManager.FindByCalendarDateMaintenance(Convert.ToInt32(Id));

                if (!String.IsNullOrWhiteSpace(CalendarDateMaintenanceDTO.ApprovalStatus) && CalendarDateMaintenanceDTO.ApprovalStatus.Equals("P"))
                {
                    var reasonCodes = tLookUpManager.GetAll<ApprovalContextDTO>("CLNDR", "ReasonContext");
                    ViewBag.ReasonCodes = reasonCodes;
                }

                ViewBag.Privileges = tCommonManager.GetUserPrivilegesForEntity("MT_CALENDAR_DATE_MAINTENANCE");

                ViewData["FormStatus"] = CalendarDateMaintenanceDTO.ApprovalStatus;

                ViewBag.TypeList = typeList;

                return View(CalendarDateMaintenanceDTO);
            }
            catch (Exception ex)
            {
                LoggerFactory.CreateLog().LogError("Error" + ex.Message, 100, 2);
                ViewBag.ErrorMessage = Resources.MsgErrServerException;
                ViewBag.ErrorType = SessionErrorType.ServerException;
                return View("Error");
            }
        }
        #endregion
        #region CalendarDateMaintenanceSave
        [HttpPost]
        [AuthorizeUserAttribute(Entity = "MT_CALENDAR_DATE_MAINTENANCE", AccessLevel = new UserActivities[] { UserActivities.Add, UserActivities.Update })]
        public ActionResult CalendarDateMaintenanceSave(int Id, string Command, CalendarDateMaintenanceDTO ACalendarDateMaintenanceDTO)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    if (Command.Equals(Resources.Submit))
                        ACalendarDateMaintenanceDTO.ApprovalStatus = "P";
                    else if (Command.Equals(Resources.Save))
                        ACalendarDateMaintenanceDTO.ApprovalStatus = "D";

                    ACalendarDateMaintenanceDTO.OrganizationCode = Utils.OrganizationCode;
                    if (Command.Equals(Resources.Submit) || Command.Equals(Resources.Save))
                    {
                        if (string.IsNullOrWhiteSpace(tMasterManager.FindByCalendarDateMaintenance(Id).Description))
                        {
                            ACalendarDateMaintenanceDTO.CreatedBy = tCommonManager.GetLoggedUserName();
                            ACalendarDateMaintenanceDTO.LastUpdatedBy = tCommonManager.GetLoggedUserName();
                            tMasterManager.SaveCalendarDateMaintenanceDetails(ACalendarDateMaintenanceDTO);
                            TempData["Message"] = String.Format(Resources.MsgInfoSaveSuccess, String.Concat(new object[] { Resources.Calendar, " ", ACalendarDateMaintenanceDTO.Date.ToShortDateString() })); 
                        }
                        else
                        {
                            if (!string.IsNullOrWhiteSpace(Convert.ToString(Id)))
                            {
                                ACalendarDateMaintenanceDTO.LastUpdatedBy = tCommonManager.GetLoggedUserName();
                                tMasterManager.UpdateCalendarDateMaintenanceDetails(ACalendarDateMaintenanceDTO.Id, ACalendarDateMaintenanceDTO);
                                TempData["Message"] = String.Format(Resources.MsgInfoSaveSuccess, String.Concat(new object[] { Resources.Calendar, " ", ACalendarDateMaintenanceDTO.Date.ToShortDateString() })); 
                            }
                            else
                            {
                                TempData["Message"] = String.Format(Resources.MsgErrAlreadyExists, String.Concat(new object[] { Resources.Calendar, " ", ACalendarDateMaintenanceDTO.Date.ToShortDateString() })); 
                            }
                        }
                    }
                    else if (Command.Equals(Resources.Delete))
                    {
                        try
                        {
                            ACalendarDateMaintenanceDTO.LastUpdatedBy = tCommonManager.GetLoggedUserName();
                            ACalendarDateMaintenanceDTO.ApprovalStatus = Utils.GetApprovalStatusCode(Utils.ApprovalStatus.Deleted);
                            tMasterManager.UpdateCalendarDateMaintenanceDetails(ACalendarDateMaintenanceDTO.Id, ACalendarDateMaintenanceDTO);
                            TempData["Message"] = String.Format(Resources.MsgInfoDeleteSuccess, String.Concat(new object[] { Resources.Calendar, " ", ACalendarDateMaintenanceDTO.Date.ToShortDateString() })); 
                        }
                        catch (Exception ex)
                        {
                            LoggerFactory.CreateLog().LogError("Error" + ex.Message, 100, 2);
                            TempData["Message"] = String.Format(Resources.MsgErrDeleteConcurrencyError, String.Concat(new object[] { Resources.Calendar, " ", ACalendarDateMaintenanceDTO.Date.ToShortDateString() })); 
                            return RedirectToAction("CalendarDateMaintenanceAdd", "master", new { Id = Id });
                        }
                    }
                }
            }

            catch (Exception ex)
            {
                LoggerFactory.CreateLog().LogError("Error" + ex.Message, 100, 2);
                ViewBag.ErrorMessage = Resources.MsgErrServerException;
                ViewBag.ErrorType = SessionErrorType.ServerException;
                return View("Error");
            }

            return ((Command.Equals(Resources.Submit) || Command.Equals(Resources.Delete)) ? RedirectToAction("CalendarDateMaintenanceAdd", "master") : RedirectToAction("CalendarDateMaintenanceAdd", "master", new { Id = Id }));

        }
        #endregion
        #region CalendarDateMaintenanceDelete
        [HttpPost]
        [AuthorizeUserAttribute(Entity = "MT_CALENDAR_DATE_MAINTENANCE", AccessLevel = new UserActivities[] { UserActivities.Delete })]
        public JsonResult CalendarDateMaintenanceDelete(int Id, CalendarDateMaintenanceDTO ACalendarDateMaintenanceDTO)
        {
            try
            {
                ACalendarDateMaintenanceDTO = tMasterManager.FindByCalendarDateMaintenance(Id);
                ACalendarDateMaintenanceDTO.LastUpdatedBy = tCommonManager.GetLoggedUserName();
                ACalendarDateMaintenanceDTO.ApprovalStatus = Utils.GetApprovalStatusCode(Utils.ApprovalStatus.Deleted);
                ACalendarDateMaintenanceDTO.OrganizationCode = Utils.OrganizationCode;
                tMasterManager.UpdateCalendarDateMaintenanceDetails(ACalendarDateMaintenanceDTO.Id, ACalendarDateMaintenanceDTO);

                var dResult = new { result = "success", action = "CalendarDateMaintenanceList", controller = "Master" };
                TempData["Message"] = string.Format(Resources.MsgInfoDeleteSuccess, "Calendar date maintenance for " + ACalendarDateMaintenanceDTO.Date.ToShortDateString());
                return Json(dResult, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                var dResultFail = new { result = "fail", action = "CalendarDateMaintenanceList", controller = "Master" };
                TempData["Message"] = string.Format(Resources.MsgErrDeleteConcurrencyError, "Calendar date maintenance for " + ACalendarDateMaintenanceDTO.Date.ToShortDateString());
                return Json(dResultFail, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion
        #region CalendarDateMaintenanceAuthorize
        [AuthorizeUserAttribute(Entity = "MT_CALENDAR_DATE_MAINTENANCE", AccessLevel = new UserActivities[] { UserActivities.Authorize })]
        public ActionResult CalendarDateMaintenanceAuthorize(string Id, string AMode, string QId)
        {
            ViewBag.RequestQueueId = QId;
            ViewBag.TableName = "CLNDR";

            var typeList = Utils.GetCalenderType();

            if (!String.IsNullOrWhiteSpace(Id))
            {
                CalendarDateMaintenanceDTO calendarDateMaintenanceDTO = tMasterManager.FindByCalendarDateMaintenance(Convert.ToInt32(Id));

                if (!String.IsNullOrWhiteSpace(calendarDateMaintenanceDTO.ApprovalStatus) && calendarDateMaintenanceDTO.ApprovalStatus.Equals("P"))
                {
                    var reasonCodes = tLookUpManager.GetAll<ApprovalContextDTO>("CLNDR", "ReasonContext");
                    ViewBag.ReasonCodes = reasonCodes;
                }

                ViewBag.Privileges = tCommonManager.GetUserPrivilegesForEntity("MT_CALENDAR_DATE_MAINTENANCE");

                ViewData["FormStatus"] = calendarDateMaintenanceDTO.ApprovalStatus;
                ViewData["FormCall"] = "ApprovalQueue";
                ViewBag.TypeList = typeList;

                return View("CalendarDateMaintenanceAdd", calendarDateMaintenanceDTO);
            }
            else
                return RedirectToAction("ApprovalQueueList", "Approval");
        }
        #endregion
        #endregion

        #region TransactionSubCode
        #region TransactionSubCodeView
        [AuthorizeUserAttribute(Entity = "MT_TRANSACTION_SUB_CODE", AccessLevel = new UserActivities[] { UserActivities.View })]
        public ActionResult TransactionSubCodeView(string Id)
        {
            try
            {
                TransactionSubCodeDTO TransactionSubCodeDTO;
                ViewBag.TableName = "TRNSUBCOD";

                var CreditSubSegmentCode = tLookUpManager.GetAll<CreditSubSegmentDTO>("", "CreditSubSegment");
                var TransactionCodes = tLookUpManager.GetAll<TransactionDTO>("", "Transaction");
                var transType = Utils.GetDebitOrCredit();
                var incomingTransType = tLookUpManager.GetAll<SystemParameterDTO>("", "TaggedIncomingTransType");
                var feeTypes = tLookUpManager.GetAll<FeeTypesDTO>("", "FeeType");
                var SysParamTransSubCodes = tLookUpManager.GetAll<SystemParameterDTO>("", "SysParamTransSubCode");

                ViewBag.Privileges = tCommonManager.GetUserPrivilegesForEntity("MT_TRANSACTION_SUB_CODE");
                ViewData["TransactionCodes"] = TransactionCodes;
                ViewData["TransType"] = transType;
                ViewBag.CreditSubSegmentCodeList = CreditSubSegmentCode;
                ViewBag.IncomingTransTypeList = incomingTransType;
                ViewBag.FeeTypesList = feeTypes;
                ViewBag.SysParamTransactionCodes = SysParamTransSubCodes;

                if (String.IsNullOrWhiteSpace(Id))
                //return View("TransactionSubCodeList");
                {
                    TransactionSubCodeDTO = new TransactionSubCodeDTO();
                    ViewData["FormStatus"] = TransactionSubCodeDTO.ApprovalStatus;
                    return View("TransactionSubCodeAdd", TransactionSubCodeDTO);
                }
                else
                {
                    TransactionSubCodeDTO = tMasterManager.FindByTransactionSubCode(Convert.ToInt32(Id));

                    if (Convert.ToString(TransactionSubCodeDTO.IncomingTransType) != "" && Convert.ToString(TransactionSubCodeDTO.IncomingTransType) != null)
                    {
                        List<SelectListItem> lst = new List<SelectListItem>();
                        lst = tLookUpManager.GetAll<SystemParameterDTO>(Convert.ToString(TransactionSubCodeDTO.IncomingTransType), "IncomingTransTypeById");
                        lst.RemoveAt(0);
                        incomingTransType = incomingTransType.Union(lst).ToList();
                        ViewBag.IncomingTransTypeList = incomingTransType;
                    }

                    ViewBag.ReasonCodes = null;
                }

                ViewData["FormStatus"] = TransactionSubCodeDTO.ApprovalStatus;

                return View("TransactionSubCodeAdd", TransactionSubCodeDTO);
            }
            catch (Exception ex)
            {
                LoggerFactory.CreateLog().LogError("Error" + ex.Message, 100, 2);
                ViewBag.ErrorMessage = Resources.MsgErrServerException;
                ViewBag.ErrorType = SessionErrorType.ServerException;
                return View("Error");
            }
        }
        #endregion
        #region TransactionSubCodeAdd
        [AuthorizeUserAttribute(Entity = "MT_TRANSACTION_SUB_CODE", AccessLevel = new UserActivities[] { UserActivities.Add, UserActivities.Update, UserActivities.View })]
        public ActionResult TransactionSubCodeAdd(string Id)
        {
            try
            {
                TransactionSubCodeDTO transactionSubCodeDTO;

                var CreditSubSegmentCode = tLookUpManager.GetAll<CreditSubSegmentDTO>("", "CreditSubSegment");
                var TransactionCodes = tLookUpManager.GetAll<TransactionDTO>("", "Transaction");
                var transType = Utils.GetDebitOrCredit();
                var incomingTransType = tLookUpManager.GetAll<SystemParameterDTO>("", "IncomingTransType");
                var feeTypes = tLookUpManager.GetAll<FeeTypesDTO>("", "FeeType");
                var SysParamTransSubCodes = tLookUpManager.GetAll<SystemParameterDTO>("", "SysParamTransSubCode");


                if (String.IsNullOrWhiteSpace(Id))
                {
                    transactionSubCodeDTO = new TransactionSubCodeDTO();
                }
                else
                {
                    transactionSubCodeDTO = tMasterManager.FindByTransactionSubCode(Convert.ToInt32(Id));

                    if (Convert.ToString(transactionSubCodeDTO.IncomingTransType) != "" && Convert.ToString(transactionSubCodeDTO.IncomingTransType) != null)
                    {
                        List<SelectListItem> lst = new List<SelectListItem>();
                        lst = tLookUpManager.GetAll<SystemParameterDTO>(Convert.ToString(transactionSubCodeDTO.IncomingTransType), "IncomingTransTypeById");
                        lst.RemoveAt(0);
                        incomingTransType = incomingTransType.Union(lst).ToList();
                    }
                }

                if (!String.IsNullOrWhiteSpace(transactionSubCodeDTO.ApprovalStatus) && transactionSubCodeDTO.ApprovalStatus.Equals("P"))
                {
                    var reasonCodes = tLookUpManager.GetAll<ApprovalContextDTO>("TRNSUBCOD", "ReasonContext");
                    ViewBag.ReasonCodes = reasonCodes;
                }
                ViewData["FormStatus"] = transactionSubCodeDTO.ApprovalStatus;

                ViewBag.Privileges = tCommonManager.GetUserPrivilegesForEntity("MT_TRANSACTION_SUB_CODE");

                ViewData["TransactionCodes"] = TransactionCodes;
                ViewData["TransType"] = transType;
                ViewBag.CreditSubSegmentCodeList = CreditSubSegmentCode;
                ViewBag.IncomingTransTypeList = incomingTransType;
                ViewBag.FeeTypesList = feeTypes;
                ViewBag.SysParamTransactionCodes = SysParamTransSubCodes;

                return View(transactionSubCodeDTO);
            }
            catch (Exception ex)
            {
                LoggerFactory.CreateLog().LogError("Error" + ex.Message, 100, 2);
                ViewBag.ErrorMessage = Resources.MsgErrServerException;
                ViewBag.ErrorType = SessionErrorType.ServerException;
                return View("Error");
            }
        }
        #endregion
        #region TransactionSubCodeSave
        [HttpPost]
        [AuthorizeUserAttribute(Entity = "MT_TRANSACTION_SUB_CODE", AccessLevel = new UserActivities[] { UserActivities.Add, UserActivities.Update })]
        public ActionResult TransactionSubCodeSave(int Id, string Command, TransactionSubCodeDTO ATransactionSubCodeDTO)
        {
            try
            {

                if (ModelState.IsValid)
                {

                    if (Command.Equals(Resources.Submit))
                        ATransactionSubCodeDTO.ApprovalStatus = "P";
                    else if (Command.Equals(Resources.Save))
                        ATransactionSubCodeDTO.ApprovalStatus = "D";

                    if (Command.Equals(Resources.Submit) || Command.Equals(Resources.Save))
                    {

                        if (string.IsNullOrWhiteSpace(tMasterManager.FindByTransactionSubCode(Id).SubCode))
                        {
                            ATransactionSubCodeDTO.CreatedBy = tCommonManager.GetLoggedUserName();
                            ATransactionSubCodeDTO.LastUpdatedBy = tCommonManager.GetLoggedUserName();
                            ATransactionSubCodeDTO.OrganizationCode = Utils.OrganizationCode;

                            tMasterManager.SaveTransactionSubCodeDetails(ATransactionSubCodeDTO);
                            TempData["Message"] = String.Format(Resources.MsgInfoSaveSuccess, String.Concat(new object[] { Resources.TransactionSubCode, " ", ATransactionSubCodeDTO.SubCode })); 
                        }
                        else
                        {
                            if (!string.IsNullOrWhiteSpace(Convert.ToString(Id)))
                            {
                                ATransactionSubCodeDTO.LastUpdatedBy = tCommonManager.GetLoggedUserName();
                                tMasterManager.UpdateTransactionSubCodeDetails(Id, ATransactionSubCodeDTO);
                                TempData["Message"] = String.Format(Resources.MsgInfoSaveSuccess, String.Concat(new object[] { Resources.TransactionSubCode, " ", ATransactionSubCodeDTO.SubCode })); 
                            }
                            else
                            {
                                TempData["Message"] = String.Format(Resources.MsgErrAlreadyExists, String.Concat(new object[] { Resources.TransactionSubCode, " ", ATransactionSubCodeDTO.SubCode })); 
                            }
                        }
                    }
                    else if (Command.Equals(Resources.Delete))
                    {
                        try
                        {
                            ATransactionSubCodeDTO.LastUpdatedBy = tCommonManager.GetLoggedUserName();
                            ATransactionSubCodeDTO.ApprovalStatus = Utils.GetApprovalStatusCode(Utils.ApprovalStatus.Deleted);
                            tMasterManager.UpdateTransactionSubCodeDetails(Id, ATransactionSubCodeDTO);

                            TempData["Message"] = String.Format(Resources.MsgInfoDeleteSuccess, String.Concat(new object[] { Resources.TransactionSubCode, " ", ATransactionSubCodeDTO.SubCode })); 
                        }
                        catch (Exception ex)
                        {
                            LoggerFactory.CreateLog().LogError("Error" + ex.Message, 100, 2);
                            TempData["Message"] = String.Format(Resources.MsgErrDeleteConcurrencyError, String.Concat(new object[] { Resources.TransactionSubCode, " ", ATransactionSubCodeDTO.SubCode })); 
                            return RedirectToAction("TransactionSubCodeAdd", "master", new { Id = Id });
                        }
                    }

                }

            }

            catch (Exception ex)
            {
                LoggerFactory.CreateLog().LogError("Error" + ex.Message, 100, 2);
                ViewBag.ErrorMessage = Resources.MsgErrServerException;
                ViewBag.ErrorType = SessionErrorType.ServerException;
                return View("Error");
            }

            return ((Command.Equals(Resources.Submit) || Command.Equals(Resources.Delete)) ? RedirectToAction("TransactionSubCodeAdd", "Master") : RedirectToAction("TransactionSubCodeAdd", "Master", new { Id = Id }));

        }
        #endregion
        #region TransactionSubCodeDelete
        [HttpPost]
        [AuthorizeUserAttribute(Entity = "MT_TRANSACTION_SUB_CODE", AccessLevel = new UserActivities[] { UserActivities.Delete })]
        public JsonResult TransactionSubCodeDelete(int Id)
        {
            TransactionSubCodeDTO ATransactionSubCodeDTO = new TransactionSubCodeDTO();
            try
            {
                ATransactionSubCodeDTO = tMasterManager.FindByTransactionSubCode(Id);
                ATransactionSubCodeDTO.LastUpdatedBy = tCommonManager.GetLoggedUserName();
                ATransactionSubCodeDTO.ApprovalStatus = Utils.GetApprovalStatusCode(Utils.ApprovalStatus.Deleted);
                tMasterManager.UpdateTransactionSubCodeDetails(Id, ATransactionSubCodeDTO);

                var dResult = new { result = "success", action = "TransactionSubCodeList", controller = "Master" };
                TempData["Message"] = String.Format(Resources.MsgInfoDeleteSuccess, String.Concat(new object[] { Resources.TransactionSubCode, " ", ATransactionSubCodeDTO.SubCode })); 
                return Json(dResult, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                var dResultFail = new { result = "fail", action = "TransactionSubCodeList", controller = "Master" };
                TempData["Message"] = String.Format(Resources.MsgErrDeleteConcurrencyError, String.Concat(new object[] { Resources.TransactionSubCode, " ", ATransactionSubCodeDTO.SubCode })); 
                return Json(dResultFail, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion
        #region TransactionSubCodeList
        [AuthorizeUserAttribute(Entity = "MT_TRANSACTION_SUB_CODE", AccessLevel = new UserActivities[] { UserActivities.View })]
        public ActionResult TransactionSubCodeList(TransactionSubCodeSearch AVMTransactionSubCodeSearch, string ClearButton, int? page)
        {
            try
            {
                int iPageNumber = Convert.ToInt32((page == null) ? 0 : page);

                var TransactionCodes = tLookUpManager.GetAll<TransactionDTO>("", "Transaction");
                var transType = Utils.GetDebitOrCredit();
                var SysParamTransSubCodes = tLookUpManager.GetAll<SystemParameterDTO>("", "SysParamTransSubCode");
                ViewBag.SysParamTransactionCodes = SysParamTransSubCodes;

                if (ClearButton == Resources.Clear)
                {
                    AVMTransactionSubCodeSearch.SubCode = null;
                    AVMTransactionSubCodeSearch.Description = null;
                    AVMTransactionSubCodeSearch.TransactionCode = null;
                    AVMTransactionSubCodeSearch.IsDebitOrCredit = null;
                    ModelState.Clear();
                }

                if ((!string.IsNullOrEmpty(Utils.OrganizationCode)) || (!(string.IsNullOrEmpty(AVMTransactionSubCodeSearch.SearchButton)) &&
                         !((string.IsNullOrWhiteSpace(AVMTransactionSubCodeSearch.SubCode)
                         && (string.IsNullOrWhiteSpace(AVMTransactionSubCodeSearch.Description))
                         && (string.IsNullOrWhiteSpace(AVMTransactionSubCodeSearch.TransactionCode))
                         && (string.IsNullOrWhiteSpace(AVMTransactionSubCodeSearch.IsDebitOrCredit))
                         ))))
                {
                    var entities = tMasterManager.FindAll(iPageNumber, new TransactionSubCodeDTO()
                    {
                        SubCode = AVMTransactionSubCodeSearch.SubCode,
                        Description = AVMTransactionSubCodeSearch.Description,
                        TransactionCode = AVMTransactionSubCodeSearch.TransactionCode,
                        IsDebitOrCredit = AVMTransactionSubCodeSearch.IsDebitOrCredit
                    });
                    AVMTransactionSubCodeSearch.SearchResults = entities.AsQueryable();
                }
                else
                {
                    var currResult = tMasterManager.FindTransactionSubCode(1, Utils.GRD_ROWS_PER_PAGE).AsQueryable();

                    AVMTransactionSubCodeSearch.SearchResults = currResult.AsQueryable();
                }

                ViewData["TransactionCodes"] = TransactionCodes;
                ViewData["TransType"] = transType;

                AVMTransactionSubCodeSearch.SearchResults = AVMTransactionSubCodeSearch.SearchResults.Where(x => x.ApprovalStatus != "DEL" && x.ApprovalStatus != "DIS");
                ViewBag.Privileges = tCommonManager.GetUserPrivilegesForEntity("MT_TRANSACTION_SUB_CODE");
                return View(AVMTransactionSubCodeSearch);
            }
            catch (Exception ex)
            {
                LoggerFactory.CreateLog().LogError("Error" + ex.Message, 100, 2);
                ViewBag.ErrorMessage = Resources.MsgErrServerException;
                ViewBag.ErrorType = SessionErrorType.ServerException;
                return View("Error");
            }
        }
        #endregion
        #region TransactionSubCodeAuthorize
        [AuthorizeUserAttribute(Entity = "MT_TRANSACTION_SUB_CODE", AccessLevel = new UserActivities[] { UserActivities.Authorize })]
        public ActionResult TransactionSubCodeAuthorize(int Id, string AMode, string QId)
        {
            ViewBag.RequestQueueId = QId;
            ViewBag.TableName = "TRNSUBCOD";

            var CreditSubSegmentCode = tLookUpManager.GetAll<CreditSubSegmentDTO>("", "CreditSubSegment");
            var TransactionCodes = tLookUpManager.GetAll<TransactionDTO>("", "Transaction");
            var transType = Utils.GetDebitOrCredit();
            var incomingTransType = tLookUpManager.GetAll<SystemParameterDTO>("", "IncomingTransType");
            var feeTypes = tLookUpManager.GetAll<FeeTypesDTO>("", "FeeType");
            var SysParamTransSubCodes = tLookUpManager.GetAll<SystemParameterDTO>("", "SysParamTransSubCode");

            if (Id > 0)
            {
                TransactionSubCodeDTO transactionSubCodeDTO = tMasterManager.FindByTransactionSubCode(Id);

                if (Convert.ToString(transactionSubCodeDTO.IncomingTransType) != "" && Convert.ToString(transactionSubCodeDTO.IncomingTransType) != null)
                {
                    List<SelectListItem> lst = new List<SelectListItem>();
                    lst = tLookUpManager.GetAll<SystemParameterDTO>(Convert.ToString(transactionSubCodeDTO.IncomingTransType), "IncomingTransTypeById");
                    lst.RemoveAt(0);
                    incomingTransType = incomingTransType.Union(lst).ToList();
                }

                if (!String.IsNullOrWhiteSpace(transactionSubCodeDTO.ApprovalStatus) && transactionSubCodeDTO.ApprovalStatus.Equals("P"))
                {
                    var reasonCodes = tLookUpManager.GetAll<ApprovalContextDTO>("TRNSUBCOD", "ReasonContext");
                    ViewBag.ReasonCodes = reasonCodes;
                }
                ViewData["FormStatus"] = transactionSubCodeDTO.ApprovalStatus;
                ViewData["FormCall"] = "ApprovalQueue";
                ViewData["TransactionCodes"] = TransactionCodes;
                ViewData["TransType"] = transType;
                ViewBag.CreditSubSegmentCodeList = CreditSubSegmentCode;
                ViewBag.IncomingTransTypeList = incomingTransType;
                ViewBag.FeeTypesList = feeTypes;
                ViewBag.SysParamTransactionCodes = SysParamTransSubCodes;

                ViewBag.Privileges = tCommonManager.GetUserPrivilegesForEntity("MT_TRANSACTION_SUB_CODE");

                return View("TransactionSubCodeAdd", transactionSubCodeDTO);
            }
            else
                return RedirectToAction("ApprovalQueueList", "Approval");
        }
        #endregion
        #endregion

        #region Financial Account

        #region FinancialAccountView
        [AuthorizeUserAttribute(Entity = "MT_FINANCIAL_ACCOUNT", AccessLevel = new UserActivities[] { UserActivities.View })]
        public ActionResult FinancialAccountView(string Id)
        {
            try
            {
                FinancialAccountDTO financialAccountDTO;
                ViewBag.TableName = "FNACNT";

                var ProductList = tLookUpManager.GetAll<ProductDTO>("", "Product");
                var branchCode = tLookUpManager.GetAll<BranchDTO>("", "Branch");
                var financialActivityCode = tLookUpManager.GetAll<TransactionSubCodeDTO>("", "TransCodeSubCode");
                var sysParamDelPortFolio = tLookUpManager.GetAll<SystemParameterDTO>("", "SysParamDelPortFolio");

                ViewBag.Privileges = tCommonManager.GetUserPrivilegesForEntity("MT_FINANCIAL_ACCOUNT");

                ViewBag.ProductList = ProductList;
                ViewBag.Branch = branchCode;
                ViewBag.FinancialActivity = financialActivityCode;
                ViewBag.DelPortFolio = sysParamDelPortFolio;

                if (String.IsNullOrWhiteSpace(Id))
                {
                    financialAccountDTO = new FinancialAccountDTO();
                    ViewData["FormStatus"] = financialAccountDTO.ApprovalStatus;
                    return View("FinancialAccountAdd", financialAccountDTO);
                }
                else
                {
                    financialAccountDTO = tMasterManager.FindByFinancialAccountId(Convert.ToInt32(Id));
                    ViewBag.ReasonCodes = null;
                }

                ViewData["FormStatus"] = financialAccountDTO.ApprovalStatus;

                return View("FinancialAccountAdd", financialAccountDTO);
            }
            catch (Exception ex)
            {
                LoggerFactory.CreateLog().LogError("Error" + ex.Message, 100, 2);
                ViewBag.ErrorMessage = Resources.MsgErrServerException;
                ViewBag.ErrorType = SessionErrorType.ServerException;
                return View("Error");
            }
        }
        #endregion
        #region FinancialAccountList
        [AuthorizeUserAttribute(Entity = "MT_FINANCIAL_ACCOUNT", AccessLevel = new UserActivities[] { UserActivities.View })]
        public ActionResult FinancialAccountList(FinancialAccountSearch AVMFinancialAccountSearch, string ClearButton, int? page)
        {
            try
            {

                int iPageNumber = Convert.ToInt32((page == null) ? 0 : page);

                var ProductList = tLookUpManager.GetAll<ProductDTO>("", "Product");
                var branchCode = tLookUpManager.GetAll<BranchDTO>("", "Branch");
                var financialActivityCode = tLookUpManager.GetAll<TransactionSubCodeDTO>("", "TransCodeSubCode");

                if (ClearButton == Resources.Clear)
                {
                    AVMFinancialAccountSearch.BranchCode = null;
                    AVMFinancialAccountSearch.ModuleName = null;
                    AVMFinancialAccountSearch.ProductCode = null;
                    AVMFinancialAccountSearch.TransactionSubCodeId = 0;
                    ModelState.Clear();
                }

                if ((!string.IsNullOrEmpty(Utils.OrganizationCode)) || (!(string.IsNullOrEmpty(AVMFinancialAccountSearch.SearchButton)) &&
                     !((string.IsNullOrWhiteSpace(AVMFinancialAccountSearch.BranchCode)
                     && (string.IsNullOrWhiteSpace(AVMFinancialAccountSearch.ModuleName))
                     && (string.IsNullOrWhiteSpace(AVMFinancialAccountSearch.ProductCode))
                     && (string.IsNullOrWhiteSpace(Convert.ToString(AVMFinancialAccountSearch.TransactionSubCodeId)))
                     ))))
                {
                    var entities = tMasterManager.FindAll(iPageNumber, new FinancialAccountDTO()
                    {
                        BranchCode = AVMFinancialAccountSearch.BranchCode,
                        ModuleName = AVMFinancialAccountSearch.ModuleName,
                        ProductCode = AVMFinancialAccountSearch.ProductCode,
                        TransactionSubCodeId = AVMFinancialAccountSearch.TransactionSubCodeId
                    });
                    AVMFinancialAccountSearch.SearchResults = entities.AsQueryable();
                }
                else
                {
                    var currResult = tMasterManager.FindFinancialAccount(1, Utils.GRD_ROWS_PER_PAGE).AsQueryable();

                    AVMFinancialAccountSearch.SearchResults = currResult.AsQueryable();
                }

                ViewBag.ProductList = ProductList;
                ViewBag.Branch = branchCode;
                ViewBag.FinancialActivity = financialActivityCode;

                AVMFinancialAccountSearch.SearchResults = AVMFinancialAccountSearch.SearchResults.Where(x => x.ApprovalStatus != "DEL" && x.ApprovalStatus != "DIS");
                ViewBag.Privileges = tCommonManager.GetUserPrivilegesForEntity("MT_FINANCIAL_ACCOUNT");
                return View(AVMFinancialAccountSearch);
            }
            catch (Exception ex)
            {
                LoggerFactory.CreateLog().LogError("Error" + ex.Message, 100, 2);
                ViewBag.ErrorMessage = Resources.MsgErrServerException;
                ViewBag.ErrorType = SessionErrorType.ServerException;
                return View("Error");
            }
        }
        #endregion
        #region FinancialAccountAdd
        [AuthorizeUserAttribute(Entity = "MT_FINANCIAL_ACCOUNT", AccessLevel = new UserActivities[] { UserActivities.Add, UserActivities.Update, UserActivities.View })]
        public ActionResult FinancialAccountAdd(string Id)
        {
            try
            {
                FinancialAccountDTO financialAccountDTO;

                var ProductList = tLookUpManager.GetAll<ProductDTO>("", "Product");
                var branchCode = tLookUpManager.GetAll<BranchDTO>("", "Branch");
                var financialActivityCode = tLookUpManager.GetAll<TransactionSubCodeDTO>("", "TransCodeSubCode");
                var sysParamDelPortFolio = tLookUpManager.GetAll<SystemParameterDTO>("", "SysParamDelPortFolio");

                if (Id == null || Id == "0")
                {
                    financialAccountDTO = new FinancialAccountDTO();
                    financialAccountDTO.EffectiveDate = Utils.BusinessDate;
                }
                else
                    financialAccountDTO = tMasterManager.FindByFinancialAccountId(Convert.ToInt32(Id));

                if (!String.IsNullOrWhiteSpace(financialAccountDTO.ApprovalStatus) && financialAccountDTO.ApprovalStatus.Equals("P"))
                {
                    var reasonCodes = tLookUpManager.GetAll<ApprovalContextDTO>("FNACNT", "ReasonContext");
                    ViewBag.ReasonCodes = reasonCodes;
                }

                ViewBag.Privileges = tCommonManager.GetUserPrivilegesForEntity("MT_FINANCIAL_ACCOUNT");

                ViewData["FormStatus"] = financialAccountDTO.ApprovalStatus;

                ViewBag.ProductList = ProductList;
                ViewBag.Branch = branchCode;
                ViewBag.FinancialActivity = financialActivityCode;
                ViewBag.DelPortFolio = sysParamDelPortFolio;

                return View(financialAccountDTO);
            }
            catch (Exception ex)
            {
                LoggerFactory.CreateLog().LogError("Error" + ex.Message, 100, 2);
                ViewBag.ErrorMessage = Resources.MsgErrServerException;
                ViewBag.ErrorType = SessionErrorType.ServerException;
                return View("Error");
            }
        }
        #endregion
        #region FinancialAccountSave
        [HttpPost]
        [AuthorizeUserAttribute(Entity = "MT_FINANCIAL_ACCOUNT", AccessLevel = new UserActivities[] { UserActivities.Add, UserActivities.Update })]
        public ActionResult FinancialAccountSave(int Id, string Command, FinancialAccountDTO AFinancialAccountDTO)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    if (Command.Equals(Resources.Submit))
                        AFinancialAccountDTO.ApprovalStatus = "P";
                    else if (Command.Equals(Resources.Save))
                        AFinancialAccountDTO.ApprovalStatus = "D";

                    if (Command.Equals(Resources.Submit) || Command.Equals(Resources.Save))
                    {
                        if (tMasterManager.FindByFinancialAccountId(Id).TransactionSubCodeId == 0)
                        {
                            AFinancialAccountDTO.CreatedBy = tCommonManager.GetLoggedUserName();
                            AFinancialAccountDTO.LastUpdatedBy = tCommonManager.GetLoggedUserName();
                            AFinancialAccountDTO.OrganizationCode = Utils.OrganizationCode;

                            tMasterManager.SaveFinancialAccountDetails(AFinancialAccountDTO);
                            TempData["Message"] = String.Format(Resources.MsgInfoSaveSuccess, Resources.MT_FINANCIAL_ACCOUNT); 
                        }
                        else
                        {
                            if (!string.IsNullOrWhiteSpace(Convert.ToString(Id)))
                            {
                                AFinancialAccountDTO.LastUpdatedBy = tCommonManager.GetLoggedUserName();
                                tMasterManager.UpdateFinancialAccountDetails(Id, AFinancialAccountDTO);
                                TempData["Message"] = String.Format(Resources.MsgInfoSaveSuccess, Resources.MT_FINANCIAL_ACCOUNT); 
                            }
                            else
                            {
                                TempData["Message"] = String.Format(Resources.MsgErrAlreadyExists, Resources.MT_FINANCIAL_ACCOUNT); 
                            }
                        }
                    }
                    else if (Command.Equals(Resources.Delete))
                    {
                        try
                        {
                            AFinancialAccountDTO.LastUpdatedBy = tCommonManager.GetLoggedUserName();
                            AFinancialAccountDTO.ApprovalStatus = Utils.GetApprovalStatusCode(Utils.ApprovalStatus.Deleted);
                            tMasterManager.UpdateFinancialAccountDetails(Id, AFinancialAccountDTO);

                            TempData["Message"] = String.Format(Resources.MsgInfoDeleteSuccess, Resources.MT_FINANCIAL_ACCOUNT); 
                        }
                        catch (Exception ex)
                        {
                            LoggerFactory.CreateLog().LogError("Error" + ex.Message, 100, 2);
                            TempData["Message"] = String.Format(Resources.MsgErrDeleteConcurrencyError, Resources.MT_FINANCIAL_ACCOUNT); 
                            return RedirectToAction("FinancialAccountAdd", "master", new { Id = Id });
                        }
                    }
                }
            }

            catch (Exception ex)
            {
                LoggerFactory.CreateLog().LogError("Error" + ex.Message, 100, 2);
                ViewBag.ErrorMessage = Resources.MsgErrServerException;
                ViewBag.ErrorType = SessionErrorType.ServerException;
                return View("Error");
            }

            return ((Command.Equals(Resources.Submit) || Command.Equals(Resources.Delete)) ? RedirectToAction("FinancialAccountAdd", "master") : RedirectToAction("FinancialAccountAdd", "master", new { Id = Id }));

        }
        #endregion
        #region FinancialAccountDelete
        [HttpPost]
        [AuthorizeUserAttribute(Entity = "MT_FINANCIAL_ACCOUNT", AccessLevel = new UserActivities[] { UserActivities.Delete })]
        public JsonResult FinancialAccountDelete(int Id)
        {
            FinancialAccountDTO AFinancialAccountDTO = new FinancialAccountDTO();
            try
            {
                AFinancialAccountDTO = tMasterManager.FindByFinancialAccountId(Id);
                AFinancialAccountDTO.LastUpdatedBy = tCommonManager.GetLoggedUserName();
                AFinancialAccountDTO.ApprovalStatus = Utils.GetApprovalStatusCode(Utils.ApprovalStatus.Deleted);
                tMasterManager.UpdateFinancialAccountDetails(Id, AFinancialAccountDTO);

                var dResult = new { result = "success", action = "FinancialAccountList", controller = "Master" };
                TempData["Message"] = String.Format(Resources.MsgInfoDeleteSuccess, Resources.MT_FINANCIAL_ACCOUNT); 
                return Json(dResult, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                var dResultFail = new { result = "fail", action = "FinancialAccountList", controller = "Master" };
                TempData["Message"] = String.Format(Resources.MsgErrDeleteConcurrencyError, Resources.MT_FINANCIAL_ACCOUNT); 
                return Json(dResultFail, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion
        #region FinancialAccountAuthorize
        [AuthorizeUserAttribute(Entity = "MT_FINANCIAL_ACCOUNT", AccessLevel = new UserActivities[] { UserActivities.Authorize })]
        public ActionResult FinancialAccountAuthorize(string Id, string AMode, string QId)
        {
            ViewBag.RequestQueueId = QId;
            ViewBag.TableName = "FNACNT";

            var ProductList = tLookUpManager.GetAll<ProductDTO>("", "Product");
            var branchCode = tLookUpManager.GetAll<BranchDTO>("", "Branch");
            var financialActivityCode = tLookUpManager.GetAll<TransactionSubCodeDTO>("", "TransCodeSubCode");
            var sysParamDelPortFolio = tLookUpManager.GetAll<SystemParameterDTO>("", "SysParamDelPortFolio");

            if (!String.IsNullOrWhiteSpace(Id))
            {
                FinancialAccountDTO financialAccountDTO = tMasterManager.FindByFinancialAccountId(Convert.ToInt32(Id));

                if (!String.IsNullOrWhiteSpace(financialAccountDTO.ApprovalStatus) && financialAccountDTO.ApprovalStatus.Equals("P"))
                {
                    var reasonCodes = tLookUpManager.GetAll<ApprovalContextDTO>("FNACNT", "ReasonContext");
                    ViewBag.ReasonCodes = reasonCodes;
                }
                ViewBag.Privileges = tCommonManager.GetUserPrivilegesForEntity("MT_FINANCIAL_ACCOUNT");

                ViewData["FormStatus"] = financialAccountDTO.ApprovalStatus;
                ViewData["FormCall"] = "ApprovalQueue";
                ViewBag.ProductList = ProductList;
                ViewBag.Branch = branchCode;
                ViewBag.FinancialActivity = financialActivityCode;
                ViewBag.DelPortFolio = sysParamDelPortFolio;

                return View("FinancialAccountAdd", financialAccountDTO);
            }
            else
                return RedirectToAction("ApprovalQueueList", "Approval");
        }
        #endregion

        #endregion

        #region BillingCycleParameter

        #region BillingCycleParameterView
        [AuthorizeUserAttribute(Entity = "MT_BILLING_CYCLE_PARAMETER", AccessLevel = new UserActivities[] { UserActivities.View })]
        public ActionResult BillingCycleParameterView(string Id)
        {
            try
            {
                BillingCycleParameterDTO billingCycleParameterDTO;
                ViewBag.TableName = "BLCYCPAR";

                var billingCycleCode = tLookUpManager.GetAll<BillingCycleDTO>("", "BillingCycle");
                var months = Utils.GetMonths();
                var Years = Utils.GetYearList();

                ViewBag.Privileges = tCommonManager.GetUserPrivilegesForEntity("MT_BILLING_CYCLE_PARAMETER");

                ViewBag.BillingCycle = billingCycleCode;
                ViewBag.Month = months;
                ViewBag.Year = Years;

                if (String.IsNullOrWhiteSpace(Id))
                //return View("BillingCycleParameterList");
                {
                    billingCycleParameterDTO = new BillingCycleParameterDTO();
                    billingCycleParameterDTO.PaymentDueDate = Utils.BusinessDate;
                    billingCycleParameterDTO.StatementDate = Utils.BusinessDate;
                    billingCycleParameterDTO.BillDate = Utils.BusinessDate;
                    billingCycleParameterDTO.BillFrom = Utils.BusinessDate;
                    billingCycleParameterDTO.BillTo = Utils.BusinessDate;

                    ViewData["FormStatus"] = billingCycleParameterDTO.ApprovalStatus;
                    return View("BillingCycleParameterAdd", billingCycleParameterDTO);
                }
                else
                {
                    billingCycleParameterDTO = tMasterManager.FindByBillingCycleParameterId(Convert.ToInt32(Id));
                    ViewBag.ReasonCodes = null;
                }

                ViewData["FormStatus"] = billingCycleParameterDTO.ApprovalStatus;

                return View("BillingCycleParameterAdd", billingCycleParameterDTO);
            }
            catch (Exception ex)
            {
                LoggerFactory.CreateLog().LogError("Error" + ex.Message, 100, 2);
                ViewBag.ErrorMessage = Resources.MsgErrServerException;
                ViewBag.ErrorType = SessionErrorType.ServerException;
                return View("Error");
            }
        }
        #endregion
        #region BillingCycleParameterList
        [AuthorizeUserAttribute(Entity = "MT_BILLING_CYCLE_PARAMETER", AccessLevel = new UserActivities[] { UserActivities.View })]
        public ActionResult BillingCycleParameterList(BillingCycleParameterSearch AVMBillingCycleParameterSearch, string ClearButton, int? page)
        {
            try
            {

                int iPageNumber = Convert.ToInt32((page == null) ? 0 : page);

                var billingCycleCode = tLookUpManager.GetAll<BillingCycleDTO>("", "BillingCycle");
                var months = Utils.GetMonths();

                if (ClearButton == Resources.Clear)
                {
                    AVMBillingCycleParameterSearch.BillMonth = null;
                    AVMBillingCycleParameterSearch.BillingCycleCode = null;
                    ModelState.Clear();
                }

                if ((!string.IsNullOrEmpty(Utils.OrganizationCode)) || (!(string.IsNullOrEmpty(AVMBillingCycleParameterSearch.SearchButton)) &&
                     !(((Convert.ToInt32(AVMBillingCycleParameterSearch.BillMonth) == 0)
                     && (string.IsNullOrWhiteSpace(AVMBillingCycleParameterSearch.BillingCycleCode))
                     ))))
                {
                    var entities = tMasterManager.FindAll(iPageNumber, new BillingCycleParameterDTO()
                    {
                        BillMonth = Convert.ToInt32(AVMBillingCycleParameterSearch.BillMonth),
                        BillCycleSysParamId = Convert.ToInt32(AVMBillingCycleParameterSearch.BillingCycleCode)
                    });
                    AVMBillingCycleParameterSearch.SearchResults = entities.AsQueryable();
                }
                else
                {
                    var currResult = tMasterManager.FindBillingCycleParameter(1, Utils.GRD_ROWS_PER_PAGE).AsQueryable();

                    AVMBillingCycleParameterSearch.SearchResults = currResult.AsQueryable();
                }

                ViewBag.BillingCycle = billingCycleCode;
                ViewBag.Month = months;

                AVMBillingCycleParameterSearch.SearchResults = AVMBillingCycleParameterSearch.SearchResults.Where(x => x.ApprovalStatus != "DEL" && x.ApprovalStatus != "DIS");
                ViewBag.Privileges = tCommonManager.GetUserPrivilegesForEntity("MT_BILLING_CYCLE_PARAMETER");
                return View(AVMBillingCycleParameterSearch);
            }
            catch (Exception ex)
            {
                LoggerFactory.CreateLog().LogError("Error" + ex.Message, 100, 2);
                ViewBag.ErrorMessage = Resources.MsgErrServerException;
                ViewBag.ErrorType = SessionErrorType.ServerException;
                return View("Error");
            }
        }
        #endregion
        #region BillingCycleParameterAdd
        [AuthorizeUserAttribute(Entity = "MT_BILLING_CYCLE_PARAMETER", AccessLevel = new UserActivities[] { UserActivities.Add, UserActivities.Update, UserActivities.View })]
        public ActionResult BillingCycleParameterAdd(string Id)
        {
            try
            {
                BillingCycleParameterDTO billingCycleParameterDTO;

                var billingCycleCode = tLookUpManager.GetAll<BillingCycleDTO>("", "BillingCycle");
                var months = Utils.GetMonths();
                var Years = Utils.GetYearList();

                //int billdate = Convert.ToInt32(tMasterManager.FindBySystemParameterName("BILLDATE").DefaultValue);
                //int statementdate = Convert.ToInt32(tMasterManager.FindBySystemParameterName("STATEMENTDATE").DefaultValue);
                //int paymentdate = Convert.ToInt32(tMasterManager.FindBySystemParameterName("PDD").DefaultValue);

                if (Id == null || Id == "0")
                {
                    billingCycleParameterDTO = new BillingCycleParameterDTO();

                    billingCycleParameterDTO.BillMonth = Utils.BusinessDate.Month;
                    billingCycleParameterDTO.BillYear = Utils.BusinessDate.Year;

                    //billingCycleParameterDTO.BillDate = tMasterManager.GetNextWorkingDate(new DateTime(DateTime.Now.Year, DateTime.Now.Month, billdate));
                    //billingCycleParameterDTO.StatementDate = tMasterManager.GetNextWorkingDate(new DateTime(DateTime.Now.Year, DateTime.Now.Month, billingCycleParameterDTO.BillDate.Day + statementdate));
                    //billingCycleParameterDTO.PaymentDueDate = tMasterManager.GetNextWorkingDate(new DateTime(DateTime.Now.Year, DateTime.Now.Month, billingCycleParameterDTO.BillDate.Day + paymentdate));


                    billingCycleParameterDTO.BillDate = tMasterManager.GetWorkingDate(billingCycleParameterDTO.BillMonth, billingCycleParameterDTO.BillYear, "B");
                    billingCycleParameterDTO.StatementDate = tMasterManager.GetWorkingDate(billingCycleParameterDTO.BillMonth, billingCycleParameterDTO.BillYear, "S");
                    billingCycleParameterDTO.PaymentDueDate = tMasterManager.GetWorkingDate(billingCycleParameterDTO.BillMonth, billingCycleParameterDTO.BillYear, "P");


                    billingCycleParameterDTO.BillFrom = Utils.BusinessDate;
                    billingCycleParameterDTO.BillTo = Utils.BusinessDate;

                }
                else
                    billingCycleParameterDTO = tMasterManager.FindByBillingCycleParameterId(Convert.ToInt32(Id));

                if (!String.IsNullOrWhiteSpace(billingCycleParameterDTO.ApprovalStatus) && billingCycleParameterDTO.ApprovalStatus.Equals("P"))
                {
                    var reasonCodes = tLookUpManager.GetAll<ApprovalContextDTO>("BLCYCPAR", "ReasonContext");
                    ViewBag.ReasonCodes = reasonCodes;
                }


                ViewBag.Privileges = tCommonManager.GetUserPrivilegesForEntity("MT_BILLING_CYCLE_PARAMETER");

                ViewData["FormStatus"] = billingCycleParameterDTO.ApprovalStatus;

                ViewBag.BillingCycle = billingCycleCode;
                ViewBag.Month = months;
                ViewBag.Year = Years;

                return View(billingCycleParameterDTO);
            }
            catch (Exception ex)
            {
                LoggerFactory.CreateLog().LogError("Error" + ex.Message, 100, 2);
                ViewBag.ErrorMessage = Resources.MsgErrServerException;
                ViewBag.ErrorType = SessionErrorType.ServerException;
                return View("Error");
            }
        }
        #endregion
        #region BillingCycleParameterSave
        [HttpPost]
        [AuthorizeUserAttribute(Entity = "MT_BILLING_CYCLE_PARAMETER", AccessLevel = new UserActivities[] { UserActivities.Add, UserActivities.Update })]
        public ActionResult BillingCycleParameterSave(int Id, string Command, BillingCycleParameterDTO ABillingCycleParameterDTO)
        {
            try
            {

                //if (ABillingCycleParameterDTO.BillMonth != ABillingCycleParameterDTO.BillDate.Month)
                //{
                //    ModelState.AddModelError("BillDate", "Invalid Bill Date");

                //    var billingCycleCode = tLookUpManager.GetAll<BillingCycleDTO>("", "BillingCycle");
                //    var months = Utils.GetMonths();
                //    var Years = Utils.GetYearList();

                //    ViewBag.Privileges = tCommonManager.GetUserPrivilegesForEntity("MT_BILLING_CYCLE_PARAMETER");

                //    ViewData["FormStatus"] = ABillingCycleParameterDTO.ApprovalStatus;

                //    ViewBag.BillingCycle = billingCycleCode;
                //    ViewBag.Month = months;
                //    ViewBag.Year = Years;

                //    return View("BillingCycleParameterAdd", ABillingCycleParameterDTO);
                //}


                if (ModelState.IsValid)
                {
                    List<SelectListItem> lstCycleCode = new List<SelectListItem>();
                    string CycleCode = Convert.ToString(ABillingCycleParameterDTO.BillCycleSysParamId);
                    if (Command.Equals(Resources.Submit))
                        ABillingCycleParameterDTO.ApprovalStatus = "P";
                    else if (Command.Equals(Resources.Save))
                        ABillingCycleParameterDTO.ApprovalStatus = "D";

                    if (Command.Equals(Resources.Submit) || Command.Equals(Resources.Save))
                    {
                        if (tMasterManager.FindByBillingCycleParameterId(Id).BillCycleSysParamId == 0)
                        {
                            ABillingCycleParameterDTO.CreatedBy = tCommonManager.GetLoggedUserName();
                            ABillingCycleParameterDTO.LastUpdatedBy = tCommonManager.GetLoggedUserName();
                            ABillingCycleParameterDTO.OrganizationCode = Utils.OrganizationCode;

                            tMasterManager.SaveBillingCycleParameterDetails(ABillingCycleParameterDTO);

                            TempData["Message"] = String.Format(Resources.MsgInfoSaveSuccess, Resources.MT_BILLING_CYCLE_PARAMETER); 
                        }
                        else
                        {
                            if (!string.IsNullOrWhiteSpace(Convert.ToString(Id)))
                            {
                                ABillingCycleParameterDTO.LastUpdatedBy = tCommonManager.GetLoggedUserName();
                                tMasterManager.UpdateBillingCycleParameterDetails(Id, ABillingCycleParameterDTO);

                                lstCycleCode = tLookUpManager.GetAll<ProductMemberTypeDTO>(CycleCode, "CycleCodeByCode");
                                lstCycleCode.RemoveAt(0);
                                TempData["Message"] = String.Format(Resources.MsgInfoSaveSuccess, Resources.MT_BILLING_CYCLE_PARAMETER); 
                            }
                            else
                            {
                                TempData["Message"] = String.Format(Resources.MsgErrAlreadyExists, Resources.MT_BILLING_CYCLE_PARAMETER); 
                            }
                        }
                    }
                    else if (Command.Equals(Resources.Delete))
                    {
                        try
                        {
                            ABillingCycleParameterDTO.LastUpdatedBy = tCommonManager.GetLoggedUserName();
                            ABillingCycleParameterDTO.ApprovalStatus = Utils.GetApprovalStatusCode(Utils.ApprovalStatus.Deleted);
                            tMasterManager.UpdateBillingCycleParameterDetails(Id, ABillingCycleParameterDTO);

                            lstCycleCode = tLookUpManager.GetAll<ProductMemberTypeDTO>(CycleCode, "CycleCodeByCode");
                            lstCycleCode.RemoveAt(0);
                            TempData["Message"] = String.Format(Resources.MsgInfoDeleteSuccess, Resources.MT_BILLING_CYCLE_PARAMETER); 
                        }
                        catch (Exception ex)
                        {
                            LoggerFactory.CreateLog().LogError("Error" + ex.Message, 100, 2);
                            TempData["Message"] = String.Format(Resources.MsgErrDeleteConcurrencyError, Resources.MT_BILLING_CYCLE_PARAMETER); 
                            return RedirectToAction("BillingCycleParameterAdd", "master", new { Id = Id });
                        }
                    }
                }
            }

            catch (Exception ex)
            {
                LoggerFactory.CreateLog().LogError("Error" + ex.Message, 100, 2);
                ViewBag.ErrorMessage = Resources.MsgErrServerException;
                ViewBag.ErrorType = SessionErrorType.ServerException;
                return View("Error");
            }

            return ((Command.Equals(Resources.Submit) || Command.Equals(Resources.Delete)) ? RedirectToAction("BillingCycleParameterAdd", "master") : RedirectToAction("BillingCycleParameterAdd", "master", new { Id = Id }));

        }
        #endregion
        #region BillingCycleParameterDelete
        [HttpPost]
        [AuthorizeUserAttribute(Entity = "MT_BILLING_CYCLE_PARAMETER", AccessLevel = new UserActivities[] { UserActivities.Delete })]
        public JsonResult BillingCycleParameterDelete(int Id)
        {
            BillingCycleParameterDTO ABillingCycleParameterDTO = new BillingCycleParameterDTO();
            try
            {
                ABillingCycleParameterDTO = tMasterManager.FindByBillingCycleParameterId(Id);
                ABillingCycleParameterDTO.LastUpdatedBy = tCommonManager.GetLoggedUserName();
                ABillingCycleParameterDTO.ApprovalStatus = Utils.GetApprovalStatusCode(Utils.ApprovalStatus.Deleted);
                tMasterManager.UpdateBillingCycleParameterDetails(Id, ABillingCycleParameterDTO);

                var dResult = new { result = "success", action = "BillingCycleParameterList", controller = "Master" };
                TempData["Message"] = String.Format(Resources.MsgInfoDeleteSuccess, Resources.MT_BILLING_CYCLE_PARAMETER); 
                return Json(dResult, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                var dResultFail = new { result = "fail", action = "BillingCycleParameterList", controller = "Master" };
                TempData["Message"] = String.Format(Resources.MsgErrDeleteConcurrencyError, Resources.MT_BILLING_CYCLE_PARAMETER); 
                return Json(dResultFail, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion
        #region BillingCycleParameterAuthorize
        [AuthorizeUserAttribute(Entity = "MT_BILLING_CYCLE_PARAMETER", AccessLevel = new UserActivities[] { UserActivities.Authorize })]
        public ActionResult BillingCycleParameterAuthorize(string Id, string AMode, string QId)
        {
            ViewBag.RequestQueueId = QId;
            ViewBag.TableName = "BLCYCPAR";

            var billingCycleCode = tLookUpManager.GetAll<BillingCycleDTO>("", "BillingCycle");
            var months = Utils.GetMonths();
            var Years = Utils.GetYearList();

            if (!String.IsNullOrWhiteSpace(Id))
            {
                BillingCycleParameterDTO billingCycleParameterDTO = tMasterManager.FindByBillingCycleParameterId(Convert.ToInt32(Id));

                if (!String.IsNullOrWhiteSpace(billingCycleParameterDTO.ApprovalStatus) && billingCycleParameterDTO.ApprovalStatus.Equals("P"))
                {
                    var reasonCodes = tLookUpManager.GetAll<ApprovalContextDTO>("BLCYCPAR", "ReasonContext");
                    ViewBag.ReasonCodes = reasonCodes;
                }

                ViewBag.Privileges = tCommonManager.GetUserPrivilegesForEntity("MT_BILLING_CYCLE_PARAMETER");
                ViewData["FormCall"] = "ApprovalQueue";
                ViewData["FormStatus"] = billingCycleParameterDTO.ApprovalStatus;

                ViewBag.BillingCycle = billingCycleCode;
                ViewBag.Month = months;
                ViewBag.Year = Years;

                return View("BillingCycleParameterAdd", billingCycleParameterDTO);
            }
            else
                return RedirectToAction("ApprovalQueueList", "Approval");
        }
        #endregion
        #region GetDates
        public ActionResult GetDates(BillingCycleParameterDTO ABillingCycleParameterDTO)
        {
            DateTime BillDate = new DateTime(), StatementDate = new DateTime(), PaymentDate = new DateTime();
            try
            {
                //int billdate = Convert.ToInt32(tMasterManager.FindBySystemParameterName("BILLDATE").DefaultValue);
                //int statementdate = Convert.ToInt32(tMasterManager.FindBySystemParameterName("STATEMENTDATE").DefaultValue);
                //int paymentdate = Convert.ToInt32(tMasterManager.FindBySystemParameterName("PDD").DefaultValue);

                BillDate = tMasterManager.GetWorkingDate(ABillingCycleParameterDTO.BillMonth, ABillingCycleParameterDTO.BillYear, "B");
                StatementDate = tMasterManager.GetWorkingDate(ABillingCycleParameterDTO.BillMonth, ABillingCycleParameterDTO.BillYear, "S");
                PaymentDate = tMasterManager.GetWorkingDate(ABillingCycleParameterDTO.BillMonth, ABillingCycleParameterDTO.BillYear, "P");

            }
            catch
            {

            }

            return Json(new { BillDate = BillDate, StatementDate = StatementDate, PaymentDate = PaymentDate }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #endregion

        #region SBIFInterest

        #region SBIFInterestView
        [AuthorizeUserAttribute(Entity = "MT_SBIF_INTEREST", AccessLevel = new UserActivities[] { UserActivities.View })]
        public ActionResult SBIFInterestView(string ASBIFInterestId)
        {
            try
            {
                SBIFInterestDTO sBIFInterestDTO;
                ViewBag.TableName = "SBIFINT";

                var interestDefinitions = Utils.GetInterestDefinition();
                var months = Utils.GetMonths();

                ViewBag.Privileges = tCommonManager.GetUserPrivilegesForEntity("MT_SBIF_INTEREST");
                ViewBag.InterestDefinition = interestDefinitions;
                ViewBag.Month = months;

                if (String.IsNullOrWhiteSpace(ASBIFInterestId))
                //return View("SBIFInterestList");
                {
                    sBIFInterestDTO = new SBIFInterestDTO();
                    sBIFInterestDTO.EffectiveDate = Utils.BusinessDate;
                    ViewBag.SBIFInterest = new JavaScriptSerializer().Serialize(sBIFInterestDTO);
                    ViewData["FormStatus"] = sBIFInterestDTO.ApprovalStatus;
                    return View("SBIFInterestAdd", sBIFInterestDTO);
                }
                else
                {
                    sBIFInterestDTO = tMasterManager.FindBySBIFInterestId(Convert.ToInt32(ASBIFInterestId));
                    ViewBag.SBIFInterest = new JavaScriptSerializer().Serialize(sBIFInterestDTO);
                    ViewBag.ReasonCodes = null;
                }

                ViewData["FormStatus"] = sBIFInterestDTO.ApprovalStatus;

                return View("SBIFInterestAdd", sBIFInterestDTO);
            }
            catch (Exception ex)
            {
                LoggerFactory.CreateLog().LogError("Error" + ex.Message, 100, 2);
                ViewBag.ErrorMessage = Resources.MsgErrServerException;
                ViewBag.ErrorType = SessionErrorType.ServerException;
                return View("Error");
            }
        }
        #endregion
        #region SBIFInterestList
        [AuthorizeUserAttribute(Entity = "MT_SBIF_INTEREST", AccessLevel = new UserActivities[] { UserActivities.View })]
        public ActionResult SBIFInterestList(SBIFInterestSearch AVMSBIFInterestSearch, string ClearButton, int? page)
        {
            try
            {

                int iPageNumber = Convert.ToInt32((page == null) ? 0 : page);

                var interestDefinitions = Utils.GetInterestDefinition();
                var months = Utils.GetMonths();

                if (ClearButton == Resources.Clear)
                {
                    AVMSBIFInterestSearch.TenureFromMonths = 0;
                    AVMSBIFInterestSearch.TenureToMonths = 0;
                    AVMSBIFInterestSearch.MaxInterestDefinitionFor = null;
                    ModelState.Clear();
                }

                if ((!string.IsNullOrEmpty(Utils.OrganizationCode)) || (!(string.IsNullOrEmpty(AVMSBIFInterestSearch.SearchButton)) &&
                     !(((AVMSBIFInterestSearch.TenureFromMonths == 0) && (AVMSBIFInterestSearch.TenureToMonths == 0)
                     && (string.IsNullOrWhiteSpace(AVMSBIFInterestSearch.MaxInterestDefinitionFor))
                     ))))
                {
                    var entities = tMasterManager.FindAll(iPageNumber, new SBIFInterestDTO()
                    {
                        MaxInterestDefinitionFor = AVMSBIFInterestSearch.MaxInterestDefinitionFor,
                        TenureFromMonths = AVMSBIFInterestSearch.TenureFromMonths,
                        TenureToMonths = AVMSBIFInterestSearch.TenureToMonths
                    });
                    AVMSBIFInterestSearch.SearchResults = entities.AsQueryable();
                }
                else
                {
                    var currResult = tMasterManager.FindSBIFInterest(1, Utils.GRD_ROWS_PER_PAGE).AsQueryable();
                    AVMSBIFInterestSearch.SearchResults = currResult.AsQueryable();
                }

                ViewBag.InterestDefinition = interestDefinitions;
                ViewBag.Month = months;

                AVMSBIFInterestSearch.SearchResults = AVMSBIFInterestSearch.SearchResults.Where(x => x.ApprovalStatus != "DEL" && x.ApprovalStatus != "DIS");
                ViewBag.Privileges = tCommonManager.GetUserPrivilegesForEntity("MT_SBIF_INTEREST");
                return View(AVMSBIFInterestSearch);
            }
            catch (Exception ex)
            {
                LoggerFactory.CreateLog().LogError("Error" + ex.Message, 100, 2);
                ViewBag.ErrorMessage = Resources.MsgErrServerException;
                ViewBag.ErrorType = SessionErrorType.ServerException;
                return View("Error");
            }
        }
        #endregion
        #region SBIFInterestAdd
        [AuthorizeUserAttribute(Entity = "MT_SBIF_INTEREST", AccessLevel = new UserActivities[] { UserActivities.Add, UserActivities.Update, UserActivities.View })]
        public ActionResult SBIFInterestAdd(string ASBIFInterestId)
        {
            try
            {
                SBIFInterestDTO SBIFInterestDTO;

                var interestDefinitions = Utils.GetInterestDefinition();
                var months = Utils.GetMonths();

                if (Convert.ToInt32(ASBIFInterestId) == 0)
                {
                    SBIFInterestDTO = new SBIFInterestDTO();
                    SBIFInterestDTO.EffectiveDate = Utils.BusinessDate;
                }
                else
                    SBIFInterestDTO = tMasterManager.FindBySBIFInterestId(Convert.ToInt32(ASBIFInterestId));

                if (!String.IsNullOrWhiteSpace(SBIFInterestDTO.ApprovalStatus) && SBIFInterestDTO.ApprovalStatus.Equals("P"))
                {
                    var reasonCodes = tLookUpManager.GetAll<ApprovalContextDTO>("SBIFINT", "ReasonContext");
                    ViewBag.ReasonCodes = reasonCodes;
                }

                ViewBag.Privileges = tCommonManager.GetUserPrivilegesForEntity("MT_SBIF_INTEREST");
                ViewBag.SBIFInterest = new JavaScriptSerializer().Serialize(SBIFInterestDTO);
                ViewData["FormStatus"] = SBIFInterestDTO.ApprovalStatus;

                ViewBag.InterestDefinition = interestDefinitions;
                ViewBag.Month = months;

                return View(SBIFInterestDTO);
            }
            catch (Exception ex)
            {
                LoggerFactory.CreateLog().LogError("Error" + ex.Message, 100, 2);
                ViewBag.ErrorMessage = Resources.MsgErrServerException;
                ViewBag.ErrorType = SessionErrorType.ServerException;
                return View("Error");
            }
        }
        #endregion
        #region SBIFInterestSave
        [HttpPost]
        [AuthorizeUserAttribute(Entity = "MT_SBIF_INTEREST", AccessLevel = new UserActivities[] { UserActivities.Add, UserActivities.Update })]
        public ActionResult SBIFInterestSave(SBIFInterestDTO ASBIFInterestDTO, string Command)
        {
            string redirectUrl = string.Empty, messageToShow = string.Empty;

            if (Command.Equals(Resources.Submit))
                ASBIFInterestDTO.ApprovalStatus = "P";
            else if (Command.Equals(Resources.Save))
                ASBIFInterestDTO.ApprovalStatus = "D";

            if (Command.Equals(Resources.Submit) || Command.Equals(Resources.Save))
            {
                if (string.IsNullOrWhiteSpace(tMasterManager.FindBySBIFInterestId(Convert.ToInt32(ASBIFInterestDTO.SBIFInterestId)).MaxInterestDefinitionFor))
                {
                    ASBIFInterestDTO.CreatedBy = tCommonManager.GetLoggedUserName();
                    ASBIFInterestDTO.LastUpdatedBy = tCommonManager.GetLoggedUserName();
                    ASBIFInterestDTO.OrganizationCode = Utils.OrganizationCode;

                    tMasterManager.SaveSBIFInterestDetails(ASBIFInterestDTO);
                    redirectUrl = Url.Action("SBIFInterestAdd", "Master", new { ASBIFInterestId = ASBIFInterestDTO.SBIFInterestId });
                    messageToShow = String.Format(Resources.MsgInfoSaveSuccess, Resources.SBIFInterest); 
                }
                else
                {
                    ASBIFInterestDTO.LastUpdatedBy = tCommonManager.GetLoggedUserName();
                    tMasterManager.UpdateSBIFInterestDetails(ASBIFInterestDTO.SBIFInterestId, ASBIFInterestDTO);
                    redirectUrl = Url.Action("SBIFInterestAdd", "Master", new { ASBIFInterestId = ASBIFInterestDTO.SBIFInterestId });
                    messageToShow = String.Format(Resources.MsgInfoSaveSuccess, Resources.SBIFInterest); 
                }
            }

            return Json(new { RedirectUrl = redirectUrl, Message = messageToShow }, JsonRequestBehavior.AllowGet);


        }
        #endregion
        #region SBIFInterestDelete
        [HttpPost]
        [AuthorizeUserAttribute(Entity = "MT_SBIF_INTEREST", AccessLevel = new UserActivities[] { UserActivities.Delete })]
        public JsonResult SBIFInterestDelete(string ASBIFInterestId, bool ASearchGridClick = false)
        {
            SBIFInterestDTO ASBIFInterestDTO = new SBIFInterestDTO();
            string redirectUrl = string.Empty, messageToShow = string.Empty;
            try
            {
                ASBIFInterestDTO = tMasterManager.FindBySBIFInterestId(Convert.ToInt32(ASBIFInterestId));
                ASBIFInterestDTO.LastUpdatedBy = tCommonManager.GetLoggedUserName();
                ASBIFInterestDTO.ApprovalStatus = Utils.GetApprovalStatusCode(Utils.ApprovalStatus.Deleted);
                tMasterManager.UpdateSBIFInterestDetails(ASBIFInterestDTO.SBIFInterestId, ASBIFInterestDTO);

                redirectUrl = Url.Action("SBIFInterestAdd", "Master");
                messageToShow = String.Format(Resources.MsgInfoDeleteSuccess, Resources.SBIFInterest); 
            }
            catch
            {
                redirectUrl = Url.Action("SBIFInterestAdd", "Master");
                messageToShow = String.Format(Resources.MsgErrDeleteConcurrencyError, Resources.SBIFInterest); 
            }

            if (ASearchGridClick)
            {
                messageToShow = String.Format(Resources.MsgInfoDeleteSuccess, Resources.SBIFInterest); 
            }
            else
            {
                TempData["Message"] = null;
            }

            return Json(new { RedirectUrl = redirectUrl, Message = messageToShow }, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region SBIFInterestAuthorize
        [AuthorizeUserAttribute(Entity = "MT_SBIF_INTEREST", AccessLevel = new UserActivities[] { UserActivities.Authorize })]
        public ActionResult SBIFInterestAuthorize(string ASBIFInterestId, string AMode, string QId)
        {
            ViewBag.RequestQueueId = QId;
            ViewBag.TableName = "SBIFINT";

            var interestDefinitions = Utils.GetInterestDefinition();
            var months = Utils.GetMonths();

            if (!String.IsNullOrWhiteSpace(ASBIFInterestId))
            {
                SBIFInterestDTO SBIFInterestDTO = tMasterManager.FindBySBIFInterestId(Convert.ToInt32(ASBIFInterestId));

                if (!String.IsNullOrWhiteSpace(SBIFInterestDTO.ApprovalStatus) && SBIFInterestDTO.ApprovalStatus.Equals("P"))
                {
                    var reasonCodes = tLookUpManager.GetAll<ApprovalContextDTO>("SBIFINT", "ReasonContext");
                    ViewBag.ReasonCodes = reasonCodes;
                }

                ViewBag.Privileges = tCommonManager.GetUserPrivilegesForEntity("MT_SBIF_INTEREST");
                ViewBag.SBIFInterest = new JavaScriptSerializer().Serialize(SBIFInterestDTO);
                ViewData["FormStatus"] = SBIFInterestDTO.ApprovalStatus;
                ViewData["FormCall"] = "ApprovalQueue";
                ViewBag.InterestDefinition = interestDefinitions;
                ViewBag.Month = months;

                return View("SBIFInterestAdd", SBIFInterestDTO);
            }
            else
                return RedirectToAction("ApprovalQueueList", "Approval");
        }
        #endregion
        #region Validate SBIFInterest Details
        public JsonResult ValidateSBIFInterestDetails(SBIFInterestDTO ASBIFInterestDTO, string Command)
        {
            bool isOverlapped = false;
            string Message = string.Empty;

            if (ASBIFInterestDTO != null)
            {
                if (ASBIFInterestDTO.SBIFInterestDetailsDTO.Count > 0)
                {

                    int intloop = 0;
                    for (int intLoop = 0; intLoop < ASBIFInterestDTO.SBIFInterestDetailsDTO.Count; intLoop++)
                    {
                        for (intloop = intLoop + 1; intloop != intLoop && intloop < ASBIFInterestDTO.SBIFInterestDetailsDTO.Count; intloop++)
                        {
                            if (Utils.CheckRangeOverlaps(ASBIFInterestDTO.SBIFInterestDetailsDTO[intLoop].CreditLimitFrom, ASBIFInterestDTO.SBIFInterestDetailsDTO[intLoop].CreditLimitTo, ASBIFInterestDTO.SBIFInterestDetailsDTO[intloop].CreditLimitFrom, ASBIFInterestDTO.SBIFInterestDetailsDTO[intloop].CreditLimitTo))
                            {
                                isOverlapped = true;
                            }
                        }
                    }
                }
                else
                {
                    Message = String.Format(Resources.MsgErrOneValid, Resources.CreditLimit);
                }
            }
            else
            {
                Message = String.Format(Resources.MsgErrOneValid, Resources.CreditLimit);
            }

            if (isOverlapped)
            {
                Message = String.Format(Resources.MsgErrOverlapValid, Resources.CreditLimit);
            }

            if (Message != string.Empty)
            {
                isOverlapped = true;
            }


            return Json(new { Message, isOverlapped }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #endregion

        #region New Application
        public ActionResult ApplicationAdd()
        {
            return View("ApplicationAdd");
        }

        public JsonResult CheckIfValidCard(string Id)
        {
            string redirectUrl = string.Empty;
            bool isvalid;
            isvalid = tMasterManager.CheckIfValidCard(Id);
            return Json(new { RedirectUrl = redirectUrl, Isvalid = isvalid });
        }

        public JsonResult CheckIfAlreadyExistsCard(string ARUTNumber)
        {
            string customerId, custApprovalStatus, cardNumber, redirectUrl = string.Empty;
            string[] retValues = tMasterManager.CheckIfAlreadyExistsCard(ARUTNumber);
            customerId = retValues[0];
            custApprovalStatus = retValues[1];
            cardNumber = retValues[2];

            if (string.IsNullOrWhiteSpace(cardNumber))
            {
                //string[] arr = tMasterManager.CheckIfAlreadyExistsCard(ARUTNumber);
                if (string.IsNullOrWhiteSpace(customerId) || customerId == "0")
                {

                    //redirectUrl = Url.Action("CustomerAdd", "Master");
                    redirectUrl = Url.Action("CustomerAdd", "Master", new { ARUT = ARUTNumber });
                    // return RedirectToAction("CustomerAdd", "Master");
                }
                else if (custApprovalStatus != "A")
                {
                    // return RedirectToAction("CustomerAdd", "Master", new { ACustomerId = customerId });
                    redirectUrl = Url.Action("CustomerAdd", "Master", new { ACustomerId = customerId });
                }
            }
            // return View("ApplicationAdd");

            return Json(new { RedirectUrl = redirectUrl, CardNumber = cardNumber }, JsonRequestBehavior.AllowGet);
        }


        #endregion

        #region InterestMaster

        #region InterestMasterView
        [AuthorizeUserAttribute(Entity = "MT_INTEREST_MASTER", AccessLevel = new UserActivities[] { UserActivities.View })]
        public ActionResult InterestMasterView(string AInterestMasterId)
        {
            try
            {
                InterestMasterDTO InterestMasterDTO;
                ViewBag.TableName = "INTMSTR";

                var interestDefinitions = Utils.GetInterestDefinition();
                var productCodes = tLookUpManager.GetAll<ProductDTO>("", "Product");
                var CustomerSegmentCode = tLookUpManager.GetAll<CustomerSegmentDTO>("", "CustomerSegment");
                var CustomerSubSegmentCode = tLookUpManager.GetAll<CustomerSubSegmentDTO>("", "CustomerSubSegment");
                var months = Utils.GetMonths();

                ViewBag.Privileges = tCommonManager.GetUserPrivilegesForEntity("MT_INTEREST_MASTER");

                ViewBag.InterestDefinition = interestDefinitions;
                ViewBag.InterestDefinition = interestDefinitions;
                ViewBag.ProductList = productCodes;
                ViewBag.CustomerSegment = CustomerSegmentCode;
                ViewBag.CustomerSubSegment = CustomerSubSegmentCode;
                ViewBag.Month = months;

                if (Convert.ToInt32(AInterestMasterId) == 0)
                //return View("InterestMasterList");
                {
                    InterestMasterDTO = new InterestMasterDTO();
                    InterestMasterDTO.EffectiveDate = Utils.BusinessDate;
                    ViewData["FormStatus"] = InterestMasterDTO.ApprovalStatus;
                    ViewBag.InterestMasters = new JavaScriptSerializer().Serialize(InterestMasterDTO);
                    return View("InterestMasterAdd", InterestMasterDTO);
                }
                else
                {
                    InterestMasterDTO = tMasterManager.FindByInterestMasterId(Convert.ToInt32(AInterestMasterId));
                    ViewBag.InterestMasters = new JavaScriptSerializer().Serialize(InterestMasterDTO);
                    ViewBag.ReasonCodes = null;
                }
                ViewData["FormStatus"] = InterestMasterDTO.ApprovalStatus;

                return View("InterestMasterAdd", InterestMasterDTO);
            }
            catch (Exception ex)
            {
                LoggerFactory.CreateLog().LogError("Error" + ex.Message, 100, 2);
                ViewBag.ErrorMessage = Resources.MsgErrServerException;
                ViewBag.ErrorType = SessionErrorType.ServerException;
                return View("Error");
            }
        }
        #endregion
        #region InterestMasterList
        [AuthorizeUserAttribute(Entity = "MT_INTEREST_MASTER", AccessLevel = new UserActivities[] { UserActivities.View })]
        public ActionResult InterestMasterList(InterestMasterSearch AVMInterestMasterSearch, string ClearButton, int? page)
        {
            try
            {

                int iPageNumber = Convert.ToInt32((page == null) ? 0 : page);

                var interestDefinitions = Utils.GetInterestDefinition();
                var productCodes = tLookUpManager.GetAll<ProductDTO>("", "Product");
                var CustomerSegmentCode = tLookUpManager.GetAll<CustomerSegmentDTO>("", "CustomerSegment");
                var CustomerSubSegmentCode = tLookUpManager.GetAll<CustomerSubSegmentDTO>("", "CustomerSubSegment");

                if (ClearButton == Resources.Clear)
                {
                    AVMInterestMasterSearch.ProductCode = null;
                    AVMInterestMasterSearch.CustomerSegmentCode = null;
                    AVMInterestMasterSearch.CustomerSubSegmentCode = null;
                    ModelState.Clear();
                }

                if ((!string.IsNullOrEmpty(Utils.OrganizationCode)) || (!(string.IsNullOrEmpty(AVMInterestMasterSearch.SearchButton)) &&
                     !(((string.IsNullOrWhiteSpace(AVMInterestMasterSearch.ProductCode))
                     && (string.IsNullOrWhiteSpace(AVMInterestMasterSearch.CustomerSegmentCode))
                     && (string.IsNullOrWhiteSpace(AVMInterestMasterSearch.CustomerSubSegmentCode))
                     ))))
                {
                    var entities = tMasterManager.FindAll(iPageNumber, new InterestMasterDTO()
                    {
                        ProductCode = AVMInterestMasterSearch.ProductCode,
                        CustomerSegmentCode = AVMInterestMasterSearch.CustomerSegmentCode,
                        CustomerSubSegmentCode = AVMInterestMasterSearch.CustomerSubSegmentCode
                    });
                    AVMInterestMasterSearch.SearchResults = entities.AsQueryable();
                }
                else
                {
                    var currResult = tMasterManager.FindInterestMaster(1, Utils.GRD_ROWS_PER_PAGE).AsQueryable();

                    AVMInterestMasterSearch.SearchResults = currResult.AsQueryable();
                }

                ViewBag.InterestDefinition = interestDefinitions;
                ViewBag.ProductList = productCodes;
                ViewBag.CustomerSegment = CustomerSegmentCode;
                ViewBag.CustomerSubSegment = CustomerSubSegmentCode;

                AVMInterestMasterSearch.SearchResults = AVMInterestMasterSearch.SearchResults.Where(x => x.ApprovalStatus != "DEL" && x.ApprovalStatus != "DIS");
                ViewBag.Privileges = tCommonManager.GetUserPrivilegesForEntity("MT_INTEREST_MASTER");
                return View(AVMInterestMasterSearch);
            }
            catch (Exception ex)
            {
                LoggerFactory.CreateLog().LogError("Error" + ex.Message, 100, 2);
                ViewBag.ErrorMessage = Resources.MsgErrServerException;
                ViewBag.ErrorType = SessionErrorType.ServerException;
                return View("Error");
            }
        }
        #endregion
        #region InterestMasterAdd
        [AuthorizeUserAttribute(Entity = "MT_INTEREST_MASTER", AccessLevel = new UserActivities[] { UserActivities.Add, UserActivities.Update, UserActivities.View })]
        public ActionResult InterestMasterAdd(string AInterestMasterId)
        {
            try
            {
                InterestMasterDTO InterestMasterDTO;

                var interestDefinitions = Utils.GetInterestDefinition();
                var productCodes = tLookUpManager.GetAll<ProductDTO>("", "Product");
                var CustomerSegmentCode = tLookUpManager.GetAll<CustomerSegmentDTO>("", "CustomerSegment");
                var CustomerSubSegmentCode = tLookUpManager.GetAll<CustomerSubSegmentDTO>("", "CustomerSubSegment");
                var months = Utils.GetMonths();

                if (Convert.ToInt32(AInterestMasterId) == 0)
                {
                    InterestMasterDTO = new InterestMasterDTO();
                    InterestMasterDTO.EffectiveDate = Utils.BusinessDate;
                }
                else
                    InterestMasterDTO = tMasterManager.FindByInterestMasterId(Convert.ToInt32(AInterestMasterId));

                if (!String.IsNullOrWhiteSpace(InterestMasterDTO.ApprovalStatus) && InterestMasterDTO.ApprovalStatus.Equals("P"))
                {
                    var reasonCodes = tLookUpManager.GetAll<ApprovalContextDTO>("INTMSTR", "ReasonContext");
                    ViewBag.ReasonCodes = reasonCodes;
                }

                ViewBag.Privileges = tCommonManager.GetUserPrivilegesForEntity("MT_INTEREST_MASTER");

                ViewBag.InterestMasters = new JavaScriptSerializer().Serialize(InterestMasterDTO);
                ViewData["FormStatus"] = InterestMasterDTO.ApprovalStatus;

                ViewBag.InterestDefinition = interestDefinitions;
                ViewBag.ProductList = productCodes;
                ViewBag.CustomerSegment = CustomerSegmentCode;
                ViewBag.CustomerSubSegment = CustomerSubSegmentCode;
                ViewBag.Month = months;

                return View(InterestMasterDTO);
            }
            catch (Exception ex)
            {
                LoggerFactory.CreateLog().LogError("Error" + ex.Message, 100, 2);
                ViewBag.ErrorMessage = Resources.MsgErrServerException;
                ViewBag.ErrorType = SessionErrorType.ServerException;
                return View("Error");
            }
        }
        #endregion
        #region InterestMasterSave
        [HttpPost]
        [AuthorizeUserAttribute(Entity = "MT_INTEREST_MASTER", AccessLevel = new UserActivities[] { UserActivities.Add, UserActivities.Update })]
        public ActionResult InterestMasterSave(InterestMasterDTO AInterestMasterDTO, string Command)
        {
            string redirectUrl = string.Empty, messageToShow = string.Empty;

            if (Command.Equals(Resources.Submit))
                AInterestMasterDTO.ApprovalStatus = "P";
            else if (Command.Equals(Resources.Save))
                AInterestMasterDTO.ApprovalStatus = "D";

            if (Command.Equals(Resources.Submit) || Command.Equals(Resources.Save))
            {
                if (string.IsNullOrWhiteSpace(tMasterManager.FindByInterestMasterId(Convert.ToInt32(AInterestMasterDTO.InterestMasterId)).ProductCode))
                {
                    AInterestMasterDTO.CreatedBy = tCommonManager.GetLoggedUserName();
                    AInterestMasterDTO.LastUpdatedBy = tCommonManager.GetLoggedUserName();
                    AInterestMasterDTO.OrganizationCode = Utils.OrganizationCode;

                    tMasterManager.SaveInterestMasterDetails(AInterestMasterDTO);
                    redirectUrl = Url.Action("InterestMasterAdd", "Master", new { AInterestMasterId = AInterestMasterDTO.InterestMasterId });
                    messageToShow = String.Format(Resources.MsgInfoSaveSuccess, Resources.InterestMaster); 
                }
                else
                {
                    AInterestMasterDTO.LastUpdatedBy = tCommonManager.GetLoggedUserName();
                    tMasterManager.UpdateInterestMasterDetails(AInterestMasterDTO.InterestMasterId, AInterestMasterDTO);
                    redirectUrl = Url.Action("InterestMasterAdd", "Master", new { AInterestMasterId = AInterestMasterDTO.InterestMasterId });
                    messageToShow = String.Format(Resources.MsgInfoSaveSuccess, Resources.InterestMaster); 
                }
            }

            return Json(new { RedirectUrl = redirectUrl, Message = messageToShow }, JsonRequestBehavior.AllowGet);

        }
        #endregion
        #region InterestMasterDelete
        [HttpPost]
        [AuthorizeUserAttribute(Entity = "MT_INTEREST_MASTER", AccessLevel = new UserActivities[] { UserActivities.Delete })]
        public JsonResult InterestMasterDelete(string AInterestMasterId, bool ASearchGridClick = false)
        {
            InterestMasterDTO AInterestMasterDTO = new InterestMasterDTO();
            string redirectUrl = string.Empty, messageToShow = string.Empty;
            try
            {
                AInterestMasterDTO = tMasterManager.FindByInterestMasterId(Convert.ToInt32(AInterestMasterId));
                AInterestMasterDTO.LastUpdatedBy = tCommonManager.GetLoggedUserName();
                AInterestMasterDTO.ApprovalStatus = Utils.GetApprovalStatusCode(Utils.ApprovalStatus.Deleted);
                tMasterManager.UpdateInterestMasterDetails(AInterestMasterDTO.InterestMasterId, AInterestMasterDTO);

                redirectUrl = Url.Action("InterestMasterAdd", "Master");
                messageToShow = String.Format(Resources.MsgInfoDeleteSuccess, Resources.InterestMaster); 
            }
            catch
            {
                redirectUrl = Url.Action("InterestMasterAdd", "Master");
                messageToShow = String.Format(Resources.MsgErrDeleteConcurrencyError, Resources.InterestMaster); 
            }

            if (ASearchGridClick)
            {
                messageToShow = String.Format(Resources.MsgInfoSaveSuccess, Resources.InterestMaster); 
            }
            else
            {
                TempData["Message"] = null;
            }

            return Json(new { RedirectUrl = redirectUrl, Message = messageToShow }, JsonRequestBehavior.AllowGet);

        }
        #endregion
        #region InterestMasterAuthorize
        [AuthorizeUserAttribute(Entity = "MT_INTEREST_MASTER", AccessLevel = new UserActivities[] { UserActivities.Authorize })]
        public ActionResult InterestMasterAuthorize(string AInterestMasterId, string AMode, string QId)
        {
            ViewBag.RequestQueueId = QId;
            ViewBag.TableName = "INTMSTR";

            var interestDefinitions = Utils.GetInterestDefinition();
            var productCodes = tLookUpManager.GetAll<ProductDTO>("", "Product");
            var CustomerSegmentCode = tLookUpManager.GetAll<CustomerSegmentDTO>("", "CustomerSegment");
            var CustomerSubSegmentCode = tLookUpManager.GetAll<CustomerSubSegmentDTO>("", "CustomerSubSegment");
            var months = Utils.GetMonths();

            if (!String.IsNullOrWhiteSpace(AInterestMasterId))
            {
                InterestMasterDTO InterestMasterDTO = tMasterManager.FindByInterestMasterId(Convert.ToInt32(AInterestMasterId));

                if (!String.IsNullOrWhiteSpace(InterestMasterDTO.ApprovalStatus) && InterestMasterDTO.ApprovalStatus.Equals("P"))
                {
                    var reasonCodes = tLookUpManager.GetAll<ApprovalContextDTO>("INTMSTR", "ReasonContext");
                    ViewBag.ReasonCodes = reasonCodes;
                }

                ViewBag.Privileges = tCommonManager.GetUserPrivilegesForEntity("MT_INTEREST_MASTER");

                ViewBag.InterestMasters = new JavaScriptSerializer().Serialize(InterestMasterDTO);
                ViewData["FormStatus"] = InterestMasterDTO.ApprovalStatus;
                ViewData["FormCall"] = "ApprovalQueue";
                ViewBag.InterestDefinition = interestDefinitions;
                ViewBag.ProductList = productCodes;
                ViewBag.CustomerSegment = CustomerSegmentCode;
                ViewBag.CustomerSubSegment = CustomerSubSegmentCode;
                ViewBag.Month = months;

                return View("InterestMasterAdd", InterestMasterDTO);
            }
            else
                return RedirectToAction("ApprovalQueueList", "Approval");
        }
        #endregion
        #region Validate InterestMaster Details
        public JsonResult ValidateInterestMasterDetails(InterestMasterDTO AInterestMasterDTO, string Command)
        {
            bool isOverlapped = false;
            string Message = string.Empty;

            if (AInterestMasterDTO != null)
            {
                if (AInterestMasterDTO.InterestMasterDetailsDTO.Count > 0)
                {

                    int intloop = 0;
                    for (int intLoop = 0; intLoop < AInterestMasterDTO.InterestMasterDetailsDTO.Count; intLoop++)
                    {
                        for (intloop = intLoop + 1; intloop != intLoop && intloop < AInterestMasterDTO.InterestMasterDetailsDTO.Count; intloop++)
                        {
                            if (Utils.CheckRangeOverlaps(AInterestMasterDTO.InterestMasterDetailsDTO[intLoop].CreditLimitFrom, AInterestMasterDTO.InterestMasterDetailsDTO[intLoop].CreditLimitTo, AInterestMasterDTO.InterestMasterDetailsDTO[intloop].CreditLimitFrom, AInterestMasterDTO.InterestMasterDetailsDTO[intloop].CreditLimitTo))
                            {
                                isOverlapped = true;
                            }
                        }
                    }
                }
                else
                {
                    Message = String.Format(Resources.MsgErrOneValid, Resources.CreditLimit);
                }
            }
            else
            {
                Message = String.Format(Resources.MsgErrOneValid, Resources.CreditLimit);
            }

            if (isOverlapped)
            {
                Message = String.Format(Resources.MsgErrOverlapValid, Resources.CreditLimit);
            }
            if (Message != string.Empty)
            {
                isOverlapped = true;
            }

            return Json(new { Message, isOverlapped }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #endregion

        #region Commission

        #region CommissionView
        [AuthorizeUserAttribute(Entity = "MT_COMMISSION", AccessLevel = new UserActivities[] { UserActivities.View })]
        public ActionResult CommissionView(string Id)
        {
            try
            {
                CommissionDTO CommissionDTO;
                ViewBag.TableName = "COMSNCDE";

                ViewBag.Privileges = tCommonManager.GetUserPrivilegesForEntity("MT_COMMISSION");

                if (String.IsNullOrWhiteSpace(Id))
                {
                    CommissionDTO = new CommissionDTO();
                    ViewData["FormStatus"] = CommissionDTO.ApprovalStatus;
                    return View("CommissionAdd", CommissionDTO);
                }
                else
                {
                    CommissionDTO = tMasterManager.FindByCommission(Convert.ToString(Id));
                    ViewBag.ReasonCodes = null;
                }

                ViewData["FormStatus"] = CommissionDTO.ApprovalStatus;

                return View("CommissionAdd", CommissionDTO);
            }
            catch (Exception ex)
            {
                LoggerFactory.CreateLog().LogError("Error" + ex.Message, 100, 2);
                ViewBag.ErrorMessage = Resources.MsgErrServerException;
                ViewBag.ErrorType = SessionErrorType.ServerException;
                return View("Error");
            }
        }
        #endregion
        #region CommissionList
        [AuthorizeUserAttribute(Entity = "MT_COMMISSION", AccessLevel = new UserActivities[] { UserActivities.View })]
        public ActionResult CommissionList(CommissionSearch AVMCommissionSearch, string ClearButton, int? page)
        {
            try
            {

                int iPageNumber = Convert.ToInt32((page == null) ? 0 : page);

                if (ClearButton == Resources.Clear)
                {
                    AVMCommissionSearch.CommissionCode = null;
                    AVMCommissionSearch.Description = null;
                    ModelState.Clear();
                }

                if ((Utils.OrganizationCode != string.Empty) || (!(string.IsNullOrEmpty(AVMCommissionSearch.SearchButton)) &&
                     !((string.IsNullOrWhiteSpace(AVMCommissionSearch.CommissionCode)
                     && (string.IsNullOrWhiteSpace(AVMCommissionSearch.Description))))))
                {
                    var entities = tMasterManager.FindAll(iPageNumber, new CommissionDTO()
                    {
                        CommissionCode = AVMCommissionSearch.CommissionCode,
                        Description = AVMCommissionSearch.Description,
                    });
                    AVMCommissionSearch.SearchResults = entities.AsQueryable();
                }
                else
                {
                    var currResult = tMasterManager.FindCommission(1, Utils.GRD_ROWS_PER_PAGE).AsQueryable();
                    AVMCommissionSearch.SearchResults = currResult.AsQueryable();
                }
                ViewBag.Privileges = tCommonManager.GetUserPrivilegesForEntity("MT_COMMISSION");
                return View(AVMCommissionSearch);
            }
            catch (Exception ex)
            {
                LoggerFactory.CreateLog().LogError("Error" + ex.Message, 100, 2);
                ViewBag.ErrorMessage = Resources.MsgErrServerException;
                ViewBag.ErrorType = SessionErrorType.ServerException;
                return View("Error");
            }
        }
        #endregion
        #region CommissionAdd
        [AuthorizeUserAttribute(Entity = "MT_COMMISSION", AccessLevel = new UserActivities[] { UserActivities.Add, UserActivities.Update, UserActivities.View })]
        public ActionResult CommissionAdd(string Id)
        {
            try
            {
                CommissionDTO commissionDTO;

                if (Id == null)
                    commissionDTO = new CommissionDTO();
                else
                    commissionDTO = tMasterManager.FindByCommission(Convert.ToString(Id));

                if (!String.IsNullOrWhiteSpace(commissionDTO.ApprovalStatus) && commissionDTO.ApprovalStatus.Equals("P"))
                {
                    var reasonCodes = tLookUpManager.GetAll<ApprovalContextDTO>("COMSNCDE", "ReasonContext");
                    ViewBag.ReasonCodes = reasonCodes;
                }

                ViewBag.Privileges = tCommonManager.GetUserPrivilegesForEntity("MT_COMMISSION");

                ViewData["FormStatus"] = commissionDTO.ApprovalStatus;

                return View(commissionDTO);
            }
            catch (Exception ex)
            {
                LoggerFactory.CreateLog().LogError("Error" + ex.Message, 100, 2);
                ViewBag.ErrorMessage = Resources.MsgErrServerException;
                ViewBag.ErrorType = SessionErrorType.ServerException;
                return View("Error");
            }
        }
        #endregion
        #region CommissionSave
        [HttpPost]
        [AuthorizeUserAttribute(Entity = "MT_COMMISSION", AccessLevel = new UserActivities[] { UserActivities.Add, UserActivities.Update })]
        public ActionResult CommissionSave(string Id, string Command, CommissionDTO ACommissionDTO)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    if (Command.Equals(Resources.Submit))
                        ACommissionDTO.ApprovalStatus = "P";
                    else if (Command.Equals(Resources.Save))
                        ACommissionDTO.ApprovalStatus = "D";

                    ACommissionDTO.OrganizationCode = Utils.OrganizationCode;
                    if (Command.Equals(Resources.Submit) || Command.Equals(Resources.Save))
                    {
                        if (string.IsNullOrEmpty(tMasterManager.FindByCommission(ACommissionDTO.CommissionCode).CommissionCode))
                        {
                            ACommissionDTO.CreatedBy = tCommonManager.GetLoggedUserName();
                            ACommissionDTO.LastUpdatedBy = tCommonManager.GetLoggedUserName();
                            tMasterManager.SaveCommissionDetails(ACommissionDTO);
                            TempData["Message"] = String.Format(Resources.MsgInfoSaveSuccess, String.Concat(new object[] { Resources.Commission, " ", ACommissionDTO.CommissionCode })); 
                        }
                        else
                        {
                            if (!string.IsNullOrWhiteSpace(Convert.ToString(Id)))
                            {
                                ACommissionDTO.LastUpdatedBy = tCommonManager.GetLoggedUserName();
                                tMasterManager.UpdateCommissionDetails(Id, ACommissionDTO);
                                TempData["Message"] = String.Format(Resources.MsgInfoSaveSuccess, String.Concat(new object[] { Resources.Commission, " ", ACommissionDTO.CommissionCode })); 
                            }
                            else
                            {
                                TempData["Message"] = String.Format(Resources.MsgErrAlreadyExists, String.Concat(new object[] { Resources.Commission, " ", ACommissionDTO.CommissionCode })); 
                            }
                        }
                    }
                    else if (Command.Equals(Resources.Delete))
                    {
                        try
                        {
                            ACommissionDTO.LastUpdatedBy = tCommonManager.GetLoggedUserName();
                            ACommissionDTO.ApprovalStatus = Utils.GetApprovalStatusCode(Utils.ApprovalStatus.Deleted);
                            tMasterManager.UpdateCommissionDetails(Id, ACommissionDTO);

                            TempData["Message"] = String.Format(Resources.MsgInfoDeleteSuccess, String.Concat(new object[] { Resources.Commission, " ", ACommissionDTO.CommissionCode })); 
                        }
                        catch (Exception ex)
                        {
                            LoggerFactory.CreateLog().LogError("Error" + ex.Message, 100, 2);
                            TempData["Message"] = String.Format(Resources.MsgErrDeleteConcurrencyError, String.Concat(new object[] { Resources.Commission, " ", ACommissionDTO.CommissionCode })); 
                            return RedirectToAction("CommissionAdd", "master", new { Id = Id });
                        }
                    }
                }

            }

            catch (Exception ex)
            {
                TempData["Message"] = string.Format(Resources.MsgErrWhileSave);
                LoggerFactory.CreateLog().LogError("Error" + ex.Message, 100, 2);
            }

            return ((Command.Equals(Resources.Submit) || Command.Equals(Resources.Delete)) ? RedirectToAction("CommissionAdd", "master") : RedirectToAction("CommissionAdd", "master", new { Id = Id }));

        }
        #endregion
        #region CommissionDelete
        [HttpPost]
        [AuthorizeUserAttribute(Entity = "MT_COMMISSION", AccessLevel = new UserActivities[] { UserActivities.Delete })]
        public JsonResult CommissionDelete(string Id)
        {
            CommissionDTO ACommissionDTO = new CommissionDTO();
            try
            {
                ACommissionDTO = tMasterManager.FindByCommission(Id);
                ACommissionDTO.LastUpdatedBy = tCommonManager.GetLoggedUserName();
                ACommissionDTO.OrganizationCode = Utils.OrganizationCode;
                ACommissionDTO.ApprovalStatus = Utils.GetApprovalStatusCode(Utils.ApprovalStatus.Deleted);
                tMasterManager.UpdateCommissionDetails(Id, ACommissionDTO);

                var dResult = new { result = "success", action = "CommissionList", controller = "Master" };
                TempData["Message"] = String.Format(Resources.MsgInfoDeleteSuccess, String.Concat(new object[] { Resources.Commission, " ", ACommissionDTO.CommissionCode })); 
                return Json(dResult, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                var dResultFail = new { result = "fail", action = "CommissionList", controller = "Master" };
                TempData["Message"] = String.Format(Resources.MsgErrDeleteConcurrencyError, String.Concat(new object[] { Resources.Commission, " ", ACommissionDTO.CommissionCode })); 
                return Json(dResultFail, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion
        #region CommissionAuthorize
        [AuthorizeUserAttribute(Entity = "MT_COMMISSION", AccessLevel = new UserActivities[] { UserActivities.Authorize })]
        public ActionResult CommissionAuthorize(string Id, string AMode, string QId)
        {
            ViewBag.RequestQueueId = QId;
            ViewBag.TableName = "COMSNCDE";
            if (!String.IsNullOrWhiteSpace(Id))
            {
                CommissionDTO commissionDTO = tMasterManager.FindByCommission(Id);

                if (!String.IsNullOrWhiteSpace(commissionDTO.ApprovalStatus) && commissionDTO.ApprovalStatus.Equals("P"))
                {
                    var reasonCodes = tLookUpManager.GetAll<ApprovalContextDTO>("COMSNCDE", "ReasonContext");
                    ViewBag.ReasonCodes = reasonCodes;
                }

                ViewBag.Privileges = tCommonManager.GetUserPrivilegesForEntity("MT_COMMISSION");
                ViewData["FormCall"] = "ApprovalQueue";
                ViewData["FormStatus"] = commissionDTO.ApprovalStatus;
                return View("CommissionAdd", commissionDTO);
            }
            else
                return RedirectToAction("ApprovalQueueList", "Approval");
        }
        #endregion

        #endregion

        #region CommissionCharge

        #region CommissionChargeView
        [AuthorizeUserAttribute(Entity = "MT_COMMISSION_CHARGE", AccessLevel = new UserActivities[] { UserActivities.View })]
        public ActionResult CommissionChargeView(string ACommissionChargeId)
        {
            try
            {
                CommissionChargeDTO commissionChargeDTO;

                var productCodes = tLookUpManager.GetAll<ProductDTO>("", "Product");
                var commissionCode = tLookUpManager.GetAll<CommissionDTO>("", "Commission");
                var currencyCode = tLookUpManager.GetAll<CurrencyDTO>("", "Currency");
                ViewBag.Privileges = tCommonManager.GetUserPrivilegesForEntity("MT_COMMISSION_CHARGE");
                ViewBag.TableName = "COMSNCHG";

                ViewBag.ProductList = productCodes;
                ViewBag.Commission = commissionCode;
                ViewBag.Currency = currencyCode;

                if (Convert.ToInt32(ACommissionChargeId) == 0)
                {
                    commissionChargeDTO = new CommissionChargeDTO();
                    ViewBag.CommissionCharge = new JavaScriptSerializer().Serialize(commissionChargeDTO);
                    ViewData["FormStatus"] = commissionChargeDTO.ApprovalStatus;
                    return View("CommissionChargeAdd", commissionChargeDTO);
                }
                else
                {
                    commissionChargeDTO = tMasterManager.FindByCommissionCharge(Convert.ToInt32(ACommissionChargeId));
                    ViewBag.CommissionCharge = new JavaScriptSerializer().Serialize(commissionChargeDTO);
                    ViewBag.ReasonCodes = null;
                }

                ViewData["FormStatus"] = commissionChargeDTO.ApprovalStatus;
                return View("CommissionChargeAdd", commissionChargeDTO);
            }
            catch (Exception ex)
            {
                LoggerFactory.CreateLog().LogError("Error" + ex.Message, 100, 2);
                ViewBag.ErrorMessage = Resources.MsgErrServerException;
                ViewBag.ErrorType = SessionErrorType.ServerException;
                return View("Error");
            }
        }
        #endregion
        #region CommissionChargeList
        [AuthorizeUserAttribute(Entity = "MT_COMMISSION_CHARGE", AccessLevel = new UserActivities[] { UserActivities.View })]
        public ActionResult CommissionChargeList(CommissionChargeSearch AVMCommissionChargeSearch, string ClearButton, int? page)
        {
            try
            {

                int iPageNumber = Convert.ToInt32((page == null) ? 0 : page);

                if (ClearButton == Resources.Clear)
                {
                    AVMCommissionChargeSearch.ProductCode = null;
                    ModelState.Clear();
                }

                var productCodes = tLookUpManager.GetAll<ProductDTO>("", "Product");
                var contractCode = tLookUpManager.GetAll<ContractDTO>("", "Contract");

                if ((Utils.OrganizationCode != string.Empty) ||
                    (!(string.IsNullOrEmpty(AVMCommissionChargeSearch.SearchButton)) &&
                     !((string.IsNullOrWhiteSpace(AVMCommissionChargeSearch.ProductCode)))))
                {
                    var entities = tMasterManager.FindAll(iPageNumber, new CommissionChargeDTO()
                    {
                        ProductCode = AVMCommissionChargeSearch.ProductCode
                    });
                    AVMCommissionChargeSearch.SearchResults = entities.AsQueryable();
                }
                else
                {
                    var currResult = tMasterManager.FindCommissionCharge(1, Utils.GRD_ROWS_PER_PAGE).AsQueryable();
                    AVMCommissionChargeSearch.SearchResults = currResult.AsQueryable();
                }

                ViewBag.ProductList = productCodes;
                ViewBag.ContractList = contractCode;
                ViewBag.Privileges = tCommonManager.GetUserPrivilegesForEntity("MT_COMMISSION_CHARGE");
                return View(AVMCommissionChargeSearch);
            }
            catch (Exception ex)
            {
                LoggerFactory.CreateLog().LogError("Error" + ex.Message, 100, 2);
                ViewBag.ErrorMessage = Resources.MsgErrServerException;
                ViewBag.ErrorType = SessionErrorType.ServerException;
                return View("Error");
            }
        }
        #endregion
        #region CommissionChargeAdd
        [AuthorizeUserAttribute(Entity = "MT_COMMISSION_CHARGE", AccessLevel = new UserActivities[] { UserActivities.Add, UserActivities.Update, UserActivities.View })]
        public ActionResult CommissionChargeAdd(string ACommissionChargeId)
        {
            try
            {
                CommissionChargeDTO commissionChargeDTO;

                var productCodes = tLookUpManager.GetAll<ProductDTO>("", "Product");
                var commissionCode = tLookUpManager.GetAll<CommissionDTO>("", "Commission");
                var currencyCode = tLookUpManager.GetAll<CurrencyDTO>("", "Currency");
                ViewBag.Privileges = tCommonManager.GetUserPrivilegesForEntity("MT_COMMISSION_CHARGE");

                if (Convert.ToInt32(ACommissionChargeId) == 0)
                    commissionChargeDTO = new CommissionChargeDTO();
                else
                    commissionChargeDTO = tMasterManager.FindByCommissionCharge(Convert.ToInt32(ACommissionChargeId));

                if (!String.IsNullOrWhiteSpace(commissionChargeDTO.ApprovalStatus) && commissionChargeDTO.ApprovalStatus.Equals("P"))
                {
                    var reasonCodes = tLookUpManager.GetAll<ApprovalContextDTO>("COMSNCHG", "ReasonContext");
                    ViewBag.ReasonCodes = reasonCodes;
                }
                ViewData["FormStatus"] = commissionChargeDTO.ApprovalStatus;
                ViewBag.CommissionCharge = new JavaScriptSerializer().Serialize(commissionChargeDTO);

                ViewBag.ProductList = productCodes;
                ViewBag.Commission = commissionCode;
                ViewBag.Currency = currencyCode;

                return View(commissionChargeDTO);
            }
            catch (Exception ex)
            {
                LoggerFactory.CreateLog().LogError("Error" + ex.Message, 100, 2);
                ViewBag.ErrorMessage = Resources.MsgErrServerException;
                ViewBag.ErrorType = SessionErrorType.ServerException;
                return View("Error");
            }
        }
        #endregion
        #region CommissionChargeSave
        [HttpPost]
        [AuthorizeUserAttribute(Entity = "MT_COMMISSION_CHARGE", AccessLevel = new UserActivities[] { UserActivities.Add, UserActivities.Update })]
        public ActionResult CommissionChargeSave(CommissionChargeDTO ACommissionChargeDTO, string Command)
        {
            string redirectUrl = string.Empty, messageToShow = string.Empty;

            if (Command.Equals(Resources.Submit))
                ACommissionChargeDTO.ApprovalStatus = "P";
            else if (Command.Equals(Resources.Save))
                ACommissionChargeDTO.ApprovalStatus = "D";

            ACommissionChargeDTO.OrganizationCode = Utils.OrganizationCode;
            if (Command.Equals(Resources.Submit) || Command.Equals(Resources.Save))
            {
                if (string.IsNullOrWhiteSpace(tMasterManager.FindByCommissionCharge(Convert.ToInt32(ACommissionChargeDTO.CommissionChargeId)).ProductCode))
                {
                    ACommissionChargeDTO.CreatedBy = tCommonManager.GetLoggedUserName();
                    ACommissionChargeDTO.LastUpdatedBy = tCommonManager.GetLoggedUserName();
                    tMasterManager.SaveCommissionChargeDetails(ACommissionChargeDTO);
                    redirectUrl = Url.Action("CommissionChargeAdd", "Master", new { ACommissionChargeId = ACommissionChargeDTO.CommissionChargeId });
                    messageToShow = String.Format(Resources.MsgInfoSaveSuccess, Resources.CommissionCharge); 
                }
                else
                {
                    ACommissionChargeDTO.LastUpdatedBy = tCommonManager.GetLoggedUserName();
                    tMasterManager.UpdateCommissionChargeDetails(ACommissionChargeDTO.CommissionChargeId, ACommissionChargeDTO);
                    redirectUrl = Url.Action("CommissionChargeAdd", "Master", new { ACommissionChargeId = ACommissionChargeDTO.CommissionChargeId });
                    messageToShow = String.Format(Resources.MsgInfoSaveSuccess, Resources.CommissionCharge); 
                }
            }

            return Json(new { RedirectUrl = redirectUrl, Message = messageToShow }, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region CommissionChargeDelete
        [HttpPost]
        [AuthorizeUserAttribute(Entity = "MT_COMMISSION_CHARGE", AccessLevel = new UserActivities[] { UserActivities.Delete })]
        public ActionResult CommissionChargeDelete(string ACommissionChargeId, bool ASearchGridClick = false)
        {
            CommissionChargeDTO ACommissionChargeDTO = new CommissionChargeDTO();
            string redirectUrl = string.Empty, messageToShow = string.Empty;
            try
            {
                ACommissionChargeDTO = tMasterManager.FindByCommissionCharge(Convert.ToInt32(ACommissionChargeId));
                ACommissionChargeDTO.LastUpdatedBy = tCommonManager.GetLoggedUserName();
                ACommissionChargeDTO.ApprovalStatus = Utils.GetApprovalStatusCode(Utils.ApprovalStatus.Deleted);
                ACommissionChargeDTO.OrganizationCode = Utils.OrganizationCode;
                tMasterManager.UpdateCommissionChargeDetails(ACommissionChargeDTO.CommissionChargeId, ACommissionChargeDTO);

                redirectUrl = Url.Action("CommissionChargeAdd", "Master");
                messageToShow = String.Format(Resources.MsgInfoDeleteSuccess, Resources.CommissionCharge); 
            }
            catch
            {
                redirectUrl = Url.Action("CommissionChargeAdd", "Master");
                messageToShow = String.Format(Resources.MsgErrDeleteConcurrencyError, Resources.CommissionCharge); 
            }

            if (ASearchGridClick)
            {
                messageToShow = String.Format(Resources.MsgInfoDeleteSuccess, Resources.CommissionCharge); 
            }
            else
            {
                TempData["Message"] = null;
            }
            return Json(new { RedirectUrl = redirectUrl, Message = messageToShow }, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region CommissionChargeAuthorize
        [AuthorizeUserAttribute(Entity = "MT_COMMISSION_CHARGE", AccessLevel = new UserActivities[] { UserActivities.Authorize })]
        public ActionResult CommissionChargeAuthorize(string ACommissionChargeId, string AMode, string QId)
        {
            ViewBag.RequestQueueId = QId;
            ViewBag.TableName = "COMSNCHG";

            var productCodes = tLookUpManager.GetAll<ProductDTO>("", "Product");
            var commissionCode = tLookUpManager.GetAll<CommissionDTO>("", "Commission");
            var currencyCode = tLookUpManager.GetAll<CurrencyDTO>("", "Currency");
            ViewBag.Privileges = tCommonManager.GetUserPrivilegesForEntity("MT_COMMISSION_CHARGE");

            if (!String.IsNullOrWhiteSpace(ACommissionChargeId))
            {
                CommissionChargeDTO commissionChargeDTO = tMasterManager.FindByCommissionCharge(Convert.ToInt32(ACommissionChargeId));

                if (!String.IsNullOrWhiteSpace(commissionChargeDTO.ApprovalStatus) && commissionChargeDTO.ApprovalStatus.Equals("P"))
                {
                    var reasonCodes = tLookUpManager.GetAll<ApprovalContextDTO>("COMSNCHG", "ReasonContext");
                    ViewBag.ReasonCodes = reasonCodes;
                }
                ViewData["FormStatus"] = commissionChargeDTO.ApprovalStatus;
                ViewBag.CommissionCharge = new JavaScriptSerializer().Serialize(commissionChargeDTO);
                ViewData["FormCall"] = "ApprovalQueue";
                ViewBag.ProductList = productCodes;
                ViewBag.Commission = commissionCode;
                ViewBag.Currency = currencyCode;

                return View("CommissionChargeAdd", commissionChargeDTO);
            }
            else
                return RedirectToAction("ApprovalQueueList", "Approval");
        }
        #endregion
        #region GetCommissionAmountByCommissionCode
        public JsonResult GetCommissionAmountByCommissionCode(string ACommissionCode)
        {
            //decimal commissionAmount = tMasterManager.FindByCommission(ACommissionCode).CommissionAmount;
            decimal commissionAmount = 0;
            return Json(new { CommissionAmount = commissionAmount }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #endregion

        #region Block Code For Commission Exclusion

        #region BlockCodeForCommissionExclusionView
        [AuthorizeUserAttribute(Entity = "MT_BLOCK_CODE_FOR_COMMISSION_EXCLUSION", AccessLevel = new UserActivities[] { UserActivities.View })]
        public ActionResult BlockCodeForCommissionExclusionView(string Id)
        {
            try
            {
                BlockCodeForCommissionExclusionDTO BlockCodeForCommissionExclusionDTO;
                ViewBag.TableName = "COMSNXCL";

                var blockCodeList = tLookUpManager.GetAll<BlockCodeDTO>(string.Empty, "BlockCode");
                var commissionList = tLookUpManager.GetAll<CommissionDTO>(string.Empty, "Commission");

                ViewBag.Privileges = tCommonManager.GetUserPrivilegesForEntity("MT_BLOCK_CODE_FOR_COMMISSION_EXCLUSION");

                ViewBag.BlockCodeList = blockCodeList;
                ViewBag.CommissionList = commissionList;

                if (String.IsNullOrWhiteSpace(Id))
                //return View("BlockCodeForCommissionExclusionList");
                {
                    BlockCodeForCommissionExclusionDTO = new BlockCodeForCommissionExclusionDTO();
                    ViewData["FormStatus"] = BlockCodeForCommissionExclusionDTO.ApprovalStatus;
                    return View("BlockCodeForCommissionExclusionAdd", BlockCodeForCommissionExclusionDTO);
                }
                else
                {
                    BlockCodeForCommissionExclusionDTO = tMasterManager.FindByBlockCodeForCommissionExclusionId(Convert.ToInt32(Id));
                    ViewBag.ReasonCodes = null;
                }


                ViewData["FormStatus"] = BlockCodeForCommissionExclusionDTO.ApprovalStatus;


                return View("BlockCodeForCommissionExclusionAdd", BlockCodeForCommissionExclusionDTO);
            }
            catch (Exception ex)
            {
                LoggerFactory.CreateLog().LogError("Error" + ex.Message, 100, 2);
                ViewBag.ErrorMessage = Resources.MsgErrServerException;
                ViewBag.ErrorType = SessionErrorType.ServerException;
                return View("Error");
            }
        }
        #endregion
        #region BlockCodeForCommissionExclusionAdd
        [AuthorizeUserAttribute(Entity = "MT_BLOCK_CODE_FOR_COMMISSION_EXCLUSION", AccessLevel = new UserActivities[] { UserActivities.Add, UserActivities.Update, UserActivities.View })]
        public ActionResult BlockCodeForCommissionExclusionAdd(string Id)
        {
            try
            {
                BlockCodeForCommissionExclusionDTO blockCodeForCommissionExclusionDTO;

                var blockCodeList = tLookUpManager.GetAll<BlockCodeDTO>(string.Empty, "BlockCode");
                var commissionList = tLookUpManager.GetAll<CommissionDTO>(string.Empty, "Commission");

                if (Id == null)
                {
                    blockCodeForCommissionExclusionDTO = new BlockCodeForCommissionExclusionDTO();
                }
                else
                {
                    blockCodeForCommissionExclusionDTO = tMasterManager.FindByBlockCodeForCommissionExclusionId(Convert.ToInt32(Id));
                }
                if (!String.IsNullOrWhiteSpace(blockCodeForCommissionExclusionDTO.ApprovalStatus) && blockCodeForCommissionExclusionDTO.ApprovalStatus.Equals("P"))
                {
                    var reasonCodes = tLookUpManager.GetAll<ApprovalContextDTO>("COMSNXCL", "ReasonContext");
                    ViewBag.ReasonCodes = reasonCodes;
                }

                ViewBag.Privileges = tCommonManager.GetUserPrivilegesForEntity("MT_BLOCK_CODE_FOR_COMMISSION_EXCLUSION");

                ViewData["FormStatus"] = blockCodeForCommissionExclusionDTO.ApprovalStatus;
                ViewBag.BlockCodeList = blockCodeList;
                ViewBag.CommissionList = commissionList;
                return View(blockCodeForCommissionExclusionDTO);
            }
            catch (Exception ex)
            {
                LoggerFactory.CreateLog().LogError("Error" + ex.Message, 100, 2);
                ViewBag.ErrorMessage = Resources.MsgErrServerException;
                ViewBag.ErrorType = SessionErrorType.ServerException;
                return View("Error");
            }
        }
        #endregion
        #region BlockCodeForCommissionExclusionSave
        [HttpPost]
        [AuthorizeUserAttribute(Entity = "MT_BLOCK_CODE_FOR_COMMISSION_EXCLUSION", AccessLevel = new UserActivities[] { UserActivities.Add, UserActivities.Update })]
        public ActionResult BlockCodeForCommissionExclusionSave(string Id, string Command, BlockCodeForCommissionExclusionDTO ABlockCodeForCommissionExclusionDTO)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    if (Command.Equals(Resources.Submit))
                        ABlockCodeForCommissionExclusionDTO.ApprovalStatus = "P";
                    else if (Command.Equals(Resources.Save))
                        ABlockCodeForCommissionExclusionDTO.ApprovalStatus = "D";

                    ABlockCodeForCommissionExclusionDTO.OrganizationCode = Utils.OrganizationCode;
                    if (Command.Equals(Resources.Submit) || Command.Equals(Resources.Save))
                    {
                        if (string.IsNullOrWhiteSpace(tMasterManager.FindByBlockCodeForCommissionExclusionId(Convert.ToInt32(Id)).BlockCode))
                        {
                            ABlockCodeForCommissionExclusionDTO.CreatedBy = tCommonManager.GetLoggedUserName();
                            ABlockCodeForCommissionExclusionDTO.LastUpdatedBy = tCommonManager.GetLoggedUserName();
                            tMasterManager.SaveBlockCodeForCommissionExclusionDetails(ABlockCodeForCommissionExclusionDTO);
                            TempData["Message"] = String.Format(Resources.MsgInfoSaveSuccess, Resources.MT_BLOCK_CODE_FOR_COMMISSION_EXCLUSION); 
                        }
                        else
                        {
                            if (!string.IsNullOrWhiteSpace(Convert.ToString(Id)))
                            {
                                ABlockCodeForCommissionExclusionDTO.LastUpdatedBy = tCommonManager.GetLoggedUserName();
                                tMasterManager.UpdateBlockCodeForCommissionExclusionDetails(Convert.ToInt32(Id), ABlockCodeForCommissionExclusionDTO);
                                TempData["Message"] = String.Format(Resources.MsgInfoSaveSuccess, Resources.MT_BLOCK_CODE_FOR_COMMISSION_EXCLUSION); 
                            }
                            else
                            {
                                TempData["Message"] = String.Format(Resources.MsgErrAlreadyExists, Resources.MT_BLOCK_CODE_FOR_COMMISSION_EXCLUSION); 
                            }
                        }
                    }
                    else if (Command.Equals(Resources.Delete))
                    {
                        try
                        {
                            ABlockCodeForCommissionExclusionDTO.LastUpdatedBy = tCommonManager.GetLoggedUserName();
                            ABlockCodeForCommissionExclusionDTO.ApprovalStatus = Utils.GetApprovalStatusCode(Utils.ApprovalStatus.Deleted);
                            tMasterManager.UpdateBlockCodeForCommissionExclusionDetails(Convert.ToInt32(Id), ABlockCodeForCommissionExclusionDTO);

                            TempData["Message"] = String.Format(Resources.MsgInfoDeleteSuccess, Resources.MT_BLOCK_CODE_FOR_COMMISSION_EXCLUSION); 
                        }
                        catch (Exception ex)
                        {
                            LoggerFactory.CreateLog().LogError("Error" + ex.Message, 100, 2);
                            TempData["Message"] = String.Format(Resources.MsgErrDeleteConcurrencyError, Resources.MT_BLOCK_CODE_FOR_COMMISSION_EXCLUSION); 
                            return RedirectToAction("BlockCodeForCommissionExclusionAdd", "master", new { Id = Id });
                        }
                    }
                }
            }

            catch (Exception ex)
            {
                LoggerFactory.CreateLog().LogError("Error" + ex.Message, 100, 2);
                ViewBag.ErrorMessage = Resources.MsgErrServerException;
                ViewBag.ErrorType = SessionErrorType.ServerException;
                return View("Error");
            }

            return ((Command.Equals(Resources.Submit) || Command.Equals(Resources.Delete)) ? RedirectToAction("BlockCodeForCommissionExclusionAdd", "Master") : RedirectToAction("BlockCodeForCommissionExclusionAdd", "Master", new { Id = Id }));

        }
        #endregion
        #region BlockCodeForCommissionExclusionDelete
        [HttpPost]
        [AuthorizeUserAttribute(Entity = "MT_BLOCK_CODE_FOR_COMMISSION_EXCLUSION", AccessLevel = new UserActivities[] { UserActivities.Delete })]
        public JsonResult BlockCodeForCommissionExclusionDelete(string Id)
        {
            BlockCodeForCommissionExclusionDTO ABlockCodeForCommissionExclusionDTO = new BlockCodeForCommissionExclusionDTO();
            try
            {
                ABlockCodeForCommissionExclusionDTO = tMasterManager.FindByBlockCodeForCommissionExclusionId(Convert.ToInt32(Id));
                ABlockCodeForCommissionExclusionDTO.LastUpdatedBy = tCommonManager.GetLoggedUserName();
                ABlockCodeForCommissionExclusionDTO.ApprovalStatus = Utils.GetApprovalStatusCode(Utils.ApprovalStatus.Deleted);
                ABlockCodeForCommissionExclusionDTO.OrganizationCode = Utils.OrganizationCode;
                tMasterManager.UpdateBlockCodeForCommissionExclusionDetails(Convert.ToInt32(Id), ABlockCodeForCommissionExclusionDTO);

                var dResult = new { result = "success", action = "BlockCodeForCommissionExclusionList", controller = "Master" };
                TempData["Message"] = String.Format(Resources.MsgInfoDeleteSuccess, Resources.MT_BLOCK_CODE_FOR_COMMISSION_EXCLUSION); 
                return Json(dResult, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                var dResultFail = new { result = "fail", action = "BlockCodeForCommissionExclusionList", controller = "Master" };
                TempData["Message"] = String.Format(Resources.MsgErrDeleteConcurrencyError, Resources.MT_BLOCK_CODE_FOR_COMMISSION_EXCLUSION); 
                return Json(dResultFail, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion
        #region BlockCodeForCommissionExclusionList
        [AuthorizeUserAttribute(Entity = "MT_BLOCK_CODE_FOR_COMMISSION_EXCLUSION", AccessLevel = new UserActivities[] { UserActivities.View })]
        public ActionResult BlockCodeForCommissionExclusionList(BlockCodeForCommissionExclusionSearch AVMBlockCodeForCommissionExclusionSearch, string ClearButton, int? page)
        {
            try
            {
                int iPageNumber = Convert.ToInt32((page == null) ? 0 : page);

                var blockCodeList = tLookUpManager.GetAll<BlockCodeDTO>(string.Empty, "BlockCode");
                var commissionList = tLookUpManager.GetAll<CommissionDTO>(string.Empty, "Commission");

                if (ClearButton == Resources.Clear)
                {
                    AVMBlockCodeForCommissionExclusionSearch.BlockCode = null;
                    AVMBlockCodeForCommissionExclusionSearch.CommissionCode = null;
                    ModelState.Clear();
                }

                if ((Utils.OrganizationCode != string.Empty) ||
                    (!(string.IsNullOrEmpty(AVMBlockCodeForCommissionExclusionSearch.SearchButton)) &&
                     !((string.IsNullOrWhiteSpace(AVMBlockCodeForCommissionExclusionSearch.BlockCode) &&
                     (string.IsNullOrWhiteSpace(AVMBlockCodeForCommissionExclusionSearch.CommissionCode))))))
                {
                    var entities = tMasterManager.FindAll(iPageNumber, new BlockCodeForCommissionExclusionDTO()
                    {
                        BlockCode = AVMBlockCodeForCommissionExclusionSearch.BlockCode,
                        CommissionCode = AVMBlockCodeForCommissionExclusionSearch.CommissionCode
                    });
                    AVMBlockCodeForCommissionExclusionSearch.SearchResults = entities.AsQueryable();
                }
                else
                {
                    var currResult = tMasterManager.FindBlockCodeForCommissionExclusion(1, Utils.GRD_ROWS_PER_PAGE).AsQueryable();

                    AVMBlockCodeForCommissionExclusionSearch.SearchResults = currResult.AsQueryable();
                }

                ViewBag.BlockCodeList = blockCodeList;
                ViewBag.CommissionList = commissionList;

                AVMBlockCodeForCommissionExclusionSearch.SearchResults = AVMBlockCodeForCommissionExclusionSearch.SearchResults.Where(x => x.ApprovalStatus != "DEL" && x.ApprovalStatus != "DIS");
                ViewBag.Privileges = tCommonManager.GetUserPrivilegesForEntity("MT_BLOCK_CODE_FOR_COMMISSION_EXCLUSION");
                return View(AVMBlockCodeForCommissionExclusionSearch);
            }
            catch (Exception ex)
            {
                LoggerFactory.CreateLog().LogError("Error" + ex.Message, 100, 2);
                ViewBag.ErrorMessage = Resources.MsgErrServerException;
                ViewBag.ErrorType = SessionErrorType.ServerException;
                return View("Error");
            }
        }
        #endregion
        #region BlockCodeForCommissionExclusionAuthorize
        [AuthorizeUserAttribute(Entity = "MT_BLOCK_CODE_FOR_COMMISSION_EXCLUSION", AccessLevel = new UserActivities[] { UserActivities.Authorize })]
        public ActionResult BlockCodeForCommissionExclusionAuthorize(string Id, string AMode, string QId)
        {
            ViewBag.RequestQueueId = QId;
            ViewBag.TableName = "COMSNXCL";
            if (!String.IsNullOrWhiteSpace(Id))
            {
                BlockCodeForCommissionExclusionDTO blockCodeForCommissionExclusionDTO = tMasterManager.FindByBlockCodeForCommissionExclusionId(Convert.ToInt32(Id));

                var blockCodeList = tLookUpManager.GetAll<BlockCodeDTO>(string.Empty, "BlockCode");
                var commissionList = tLookUpManager.GetAll<CommissionDTO>(string.Empty, "Commission");

                if (!String.IsNullOrWhiteSpace(blockCodeForCommissionExclusionDTO.ApprovalStatus) && blockCodeForCommissionExclusionDTO.ApprovalStatus.Equals("P"))
                {
                    var reasonCodes = tLookUpManager.GetAll<ApprovalContextDTO>("COMSNXCL", "ReasonContext");
                    ViewBag.ReasonCodes = reasonCodes;
                }

                ViewBag.Privileges = tCommonManager.GetUserPrivilegesForEntity("MT_BLOCK_CODE_FOR_COMMISSION_EXCLUSION");
                ViewData["FormCall"] = "ApprovalQueue";
                ViewData["FormStatus"] = blockCodeForCommissionExclusionDTO.ApprovalStatus;
                ViewBag.BlockCodeList = blockCodeList;
                ViewBag.CommissionList = commissionList;
                return View("BlockCodeForCommissionExclusionAdd", blockCodeForCommissionExclusionDTO);
            }
            else
                return RedirectToAction("ApprovalQueueList", "Approval");
        }
        #endregion

        #endregion

        #region DiscountOnMgmtCollection

        #region DiscountOnMgmtCollectionView
        [AuthorizeUserAttribute(Entity = "MT_DISCOUNT_ON_MGMT_COLLECTION", AccessLevel = new UserActivities[] { UserActivities.View })]
        public ActionResult DiscountOnMgmtCollectionView(string ADiscountOnMgmtCollectionId)
        {
            try
            {
                DiscountOnMgmtCollectionDTO DiscountOnMgmtCollectionDTO;
                ViewBag.TableName = "DSMGMCOL";

                var productCodes = tLookUpManager.GetAll<ProductDTO>("", "Product");
                var contractCodes = tLookUpManager.GetAll<ContractDTO>("", "Contract");
                var commissionCode = tLookUpManager.GetAll<CommissionDTO>("", "Commission");

                ViewBag.Privileges = tCommonManager.GetUserPrivilegesForEntity("MT_DISCOUNT_ON_MGMT_COLLECTION");
                ViewBag.ProductList = productCodes;
                ViewBag.ContractList = contractCodes;
                ViewBag.Commission = commissionCode;

                if (Convert.ToInt32(ADiscountOnMgmtCollectionId) == 0)
                {
                    DiscountOnMgmtCollectionDTO = new DiscountOnMgmtCollectionDTO();
                    DiscountOnMgmtCollectionDTO.OrganizationCode = Utils.OrganizationCode;
                    ViewBag.DiscountOnMgmtCollections = new JavaScriptSerializer().Serialize(DiscountOnMgmtCollectionDTO);
                    ViewData["FormStatus"] = DiscountOnMgmtCollectionDTO.ApprovalStatus;
                    return View("DiscountOnMgmtCollectionAdd", DiscountOnMgmtCollectionDTO);
                }
                else
                {
                    DiscountOnMgmtCollectionDTO = tMasterManager.FindByDiscountOnMgmtCollectionId(Convert.ToInt32(ADiscountOnMgmtCollectionId));
                    ViewBag.DiscountOnMgmtCollections = new JavaScriptSerializer().Serialize(DiscountOnMgmtCollectionDTO);
                    ViewBag.ReasonCodes = null;
                }

                ViewData["FormStatus"] = DiscountOnMgmtCollectionDTO.ApprovalStatus;

                return View("DiscountOnMgmtCollectionAdd", DiscountOnMgmtCollectionDTO);
            }
            catch (Exception ex)
            {
                LoggerFactory.CreateLog().LogError("Error" + ex.Message, 100, 2);
                ViewBag.ErrorMessage = Resources.MsgErrServerException;
                ViewBag.ErrorType = SessionErrorType.ServerException;
                return View("Error");
            }
        }
        #endregion
        #region DiscountOnMgmtCollectionList
        [AuthorizeUserAttribute(Entity = "MT_DISCOUNT_ON_MGMT_COLLECTION", AccessLevel = new UserActivities[] { UserActivities.View })]
        public ActionResult DiscountOnMgmtCollectionList(DiscountOnMgmtCollectionSearch AVMDiscountOnMgmtCollectionSearch, string ClearButton, int? page)
        {
            try
            {

                int iPageNumber = Convert.ToInt32((page == null) ? 0 : page);

                if (ClearButton == Resources.Clear)
                {
                    AVMDiscountOnMgmtCollectionSearch.ProductCode = null;
                    AVMDiscountOnMgmtCollectionSearch.ContractCode = null;
                    AVMDiscountOnMgmtCollectionSearch.CommissionCode = null;
                    ModelState.Clear();
                }

                var productCodes = tLookUpManager.GetAll<ProductDTO>("", "Product");
                var contractCodes = tLookUpManager.GetAll<ContractDTO>("", "Contract");
                var commissionCode = tLookUpManager.GetAll<CommissionDTO>("", "Commission");

                if ((Utils.OrganizationCode != string.Empty) ||
                    (!(string.IsNullOrEmpty(AVMDiscountOnMgmtCollectionSearch.SearchButton)) &&
                     !((string.IsNullOrWhiteSpace(AVMDiscountOnMgmtCollectionSearch.ProductCode)
                     && (string.IsNullOrWhiteSpace(AVMDiscountOnMgmtCollectionSearch.ContractCode))
                     && (string.IsNullOrWhiteSpace(AVMDiscountOnMgmtCollectionSearch.CommissionCode))))))
                {
                    var entities = tMasterManager.FindAll
                        (
                            iPageNumber,
                                new DiscountOnMgmtCollectionDTO()
                                {
                                    ProductCode = AVMDiscountOnMgmtCollectionSearch.ProductCode,
                                    ContractCode = AVMDiscountOnMgmtCollectionSearch.ContractCode,
                                    CommissionCode = AVMDiscountOnMgmtCollectionSearch.CommissionCode
                                }
                        );
                    AVMDiscountOnMgmtCollectionSearch.SearchResults = entities.AsQueryable();
                }
                else
                {
                    var currResult = tMasterManager.FindDiscountOnMgmtCollection(1, Utils.GRD_ROWS_PER_PAGE).AsQueryable();
                    AVMDiscountOnMgmtCollectionSearch.SearchResults = currResult.AsQueryable();
                }
                ViewBag.ProductList = productCodes;
                ViewBag.ContractList = contractCodes;
                ViewBag.Commission = commissionCode;
                ViewBag.Privileges = tCommonManager.GetUserPrivilegesForEntity("MT_DISCOUNT_ON_MGMT_COLLECTION");
                return View(AVMDiscountOnMgmtCollectionSearch);
            }
            catch (Exception ex)
            {
                LoggerFactory.CreateLog().LogError("Error" + ex.Message, 100, 2);
                ViewBag.ErrorMessage = Resources.MsgErrServerException;
                ViewBag.ErrorType = SessionErrorType.ServerException;
                return View("Error");
            }
        }
        #endregion
        #region DiscountOnMgmtCollectionAdd
        [AuthorizeUserAttribute(Entity = "MT_DISCOUNT_ON_MGMT_COLLECTION", AccessLevel = new UserActivities[] { UserActivities.Add, UserActivities.Update, UserActivities.View })]
        public ActionResult DiscountOnMgmtCollectionAdd(string ADiscountOnMgmtCollectionId)
        {
            try
            {
                DiscountOnMgmtCollectionDTO DiscountOnMgmtCollectionDTO;

                var productCodes = tLookUpManager.GetAll<ProductDTO>("", "Product");
                var contractCodes = tLookUpManager.GetAll<ContractDTO>("", "Contract");
                var commissionCode = tLookUpManager.GetAll<CommissionDTO>("", "Commission");

                if (Convert.ToInt32(ADiscountOnMgmtCollectionId) == 0)
                {
                    DiscountOnMgmtCollectionDTO = new DiscountOnMgmtCollectionDTO();
                    DiscountOnMgmtCollectionDTO.OrganizationCode = Utils.OrganizationCode;
                }
                else
                    DiscountOnMgmtCollectionDTO = tMasterManager.FindByDiscountOnMgmtCollectionId(Convert.ToInt32(ADiscountOnMgmtCollectionId));

                if (!String.IsNullOrWhiteSpace(DiscountOnMgmtCollectionDTO.ApprovalStatus) && DiscountOnMgmtCollectionDTO.ApprovalStatus.Equals("P"))
                {
                    var reasonCodes = tLookUpManager.GetAll<ApprovalContextDTO>("DSMGMCOL", "ReasonContext");
                    ViewBag.ReasonCodes = reasonCodes;
                }

                ViewBag.Privileges = tCommonManager.GetUserPrivilegesForEntity("MT_DISCOUNT_ON_MGMT_COLLECTION");
                ViewBag.DiscountOnMgmtCollections = new JavaScriptSerializer().Serialize(DiscountOnMgmtCollectionDTO);
                ViewData["FormStatus"] = DiscountOnMgmtCollectionDTO.ApprovalStatus;

                ViewBag.ProductList = productCodes;
                ViewBag.ContractList = contractCodes;
                ViewBag.Commission = commissionCode;

                return View(DiscountOnMgmtCollectionDTO);
            }
            catch (Exception ex)
            {
                LoggerFactory.CreateLog().LogError("Error" + ex.Message, 100, 2);
                ViewBag.ErrorMessage = Resources.MsgErrServerException;
                ViewBag.ErrorType = SessionErrorType.ServerException;
                return View("Error");
            }
        }
        #endregion
        #region DiscountOnMgmtCollectionSave
        [HttpPost]
        [AuthorizeUserAttribute(Entity = "MT_DISCOUNT_ON_MGMT_COLLECTION", AccessLevel = new UserActivities[] { UserActivities.Add, UserActivities.Update })]
        public ActionResult DiscountOnMgmtCollectionSave(DiscountOnMgmtCollectionDTO ADiscountOnMgmtCollectionDTO, string Command)
        {

            string redirectUrl = string.Empty, messageToShow = string.Empty;

            if (Command.Equals(Resources.Submit))
                ADiscountOnMgmtCollectionDTO.ApprovalStatus = "P";
            else if (Command.Equals(Resources.Save))
                ADiscountOnMgmtCollectionDTO.ApprovalStatus = "D";

            ADiscountOnMgmtCollectionDTO.OrganizationCode = Utils.OrganizationCode;
            if (Command.Equals(Resources.Submit) || Command.Equals(Resources.Save))
            {
                if (string.IsNullOrWhiteSpace(tMasterManager.FindByDiscountOnMgmtCollectionId(Convert.ToInt32(ADiscountOnMgmtCollectionDTO.DiscountOnMgmtCollectionId)).ProductCode))
                {
                    ADiscountOnMgmtCollectionDTO.CreatedBy = tCommonManager.GetLoggedUserName();
                    ADiscountOnMgmtCollectionDTO.LastUpdatedBy = tCommonManager.GetLoggedUserName();
                    tMasterManager.SaveDiscountOnMgmtCollectionDetails(ADiscountOnMgmtCollectionDTO);
                    redirectUrl = Url.Action("DiscountOnMgmtCollectionAdd", "Master", new { ADiscountOnMgmtCollectionId = ADiscountOnMgmtCollectionDTO.DiscountOnMgmtCollectionId });
                    messageToShow = String.Format(Resources.MsgInfoSaveSuccess, Resources.MT_DISCOUNT_ON_MGMT_COLLECTION); 
                }
                else
                {
                    ADiscountOnMgmtCollectionDTO.LastUpdatedBy = tCommonManager.GetLoggedUserName();
                    tMasterManager.UpdateDiscountOnMgmtCollectionDetails(ADiscountOnMgmtCollectionDTO.DiscountOnMgmtCollectionId, ADiscountOnMgmtCollectionDTO);
                    redirectUrl = Url.Action("DiscountOnMgmtCollectionAdd", "Master", new { ADiscountOnMgmtCollectionId = ADiscountOnMgmtCollectionDTO.DiscountOnMgmtCollectionId });
                    messageToShow = String.Format(Resources.MsgInfoSaveSuccess, Resources.MT_DISCOUNT_ON_MGMT_COLLECTION); 
                }
            }

            return Json(new { RedirectUrl = redirectUrl, Message = messageToShow }, JsonRequestBehavior.AllowGet);

        }
        #endregion
        #region DiscountOnMgmtCollectionDelete
        [HttpPost]
        [AuthorizeUserAttribute(Entity = "MT_DISCOUNT_ON_MGMT_COLLECTION", AccessLevel = new UserActivities[] { UserActivities.Delete })]
        public JsonResult DiscountOnMgmtCollectionDelete(string ADiscountOnMgmtCollectionId, bool ASearchGridClick = false)
        {
            DiscountOnMgmtCollectionDTO ADiscountOnMgmtCollectionDTO = new DiscountOnMgmtCollectionDTO();
            string redirectUrl = string.Empty, messageToShow = string.Empty;
            try
            {
                ADiscountOnMgmtCollectionDTO = tMasterManager.FindByDiscountOnMgmtCollectionId(Convert.ToInt32(ADiscountOnMgmtCollectionId));
                ADiscountOnMgmtCollectionDTO.LastUpdatedBy = tCommonManager.GetLoggedUserName();
                ADiscountOnMgmtCollectionDTO.ApprovalStatus = Utils.GetApprovalStatusCode(Utils.ApprovalStatus.Deleted);
                ADiscountOnMgmtCollectionDTO.OrganizationCode = Utils.OrganizationCode;
                tMasterManager.UpdateDiscountOnMgmtCollectionDetails(ADiscountOnMgmtCollectionDTO.DiscountOnMgmtCollectionId, ADiscountOnMgmtCollectionDTO);

                redirectUrl = Url.Action("DiscountOnMgmtCollectionAdd", "Master");
                messageToShow = String.Format(Resources.MsgInfoDeleteSuccess, Resources.MT_DISCOUNT_ON_MGMT_COLLECTION); 
            }
            catch
            {
                redirectUrl = Url.Action("DiscountOnMgmtCollectionAdd", "Master");
                messageToShow = String.Format(Resources.MsgErrDeleteConcurrencyError, Resources.MT_DISCOUNT_ON_MGMT_COLLECTION); 
            }

            if (ASearchGridClick)
            {
                messageToShow = String.Format(Resources.MsgInfoDeleteSuccess, Resources.MT_DISCOUNT_ON_MGMT_COLLECTION); 
            }
            else
            {
                TempData["Message"] = null;
            }


            return Json(new { RedirectUrl = redirectUrl, Message = messageToShow }, JsonRequestBehavior.AllowGet);

        }
        #endregion
        #region DiscountOnMgmtCollectionAuthorize
        [AuthorizeUserAttribute(Entity = "MT_DISCOUNT_ON_MGMT_COLLECTION", AccessLevel = new UserActivities[] { UserActivities.Authorize })]
        public ActionResult DiscountOnMgmtCollectionAuthorize(string ADiscountOnMgmtCollectionId, string AMode, string QId)
        {
            ViewBag.RequestQueueId = QId;
            ViewBag.TableName = "DSMGMCOL";

            var productCodes = tLookUpManager.GetAll<ProductDTO>("", "Product");
            var contractCodes = tLookUpManager.GetAll<ContractDTO>("", "Contract");
            var commissionCode = tLookUpManager.GetAll<CommissionDTO>("", "Commission");

            if (!String.IsNullOrWhiteSpace(ADiscountOnMgmtCollectionId))
            {
                DiscountOnMgmtCollectionDTO DiscountOnMgmtCollectionDTO = tMasterManager.FindByDiscountOnMgmtCollectionId(Convert.ToInt32(ADiscountOnMgmtCollectionId));

                if (!String.IsNullOrWhiteSpace(DiscountOnMgmtCollectionDTO.ApprovalStatus) && DiscountOnMgmtCollectionDTO.ApprovalStatus.Equals("P"))
                {
                    var reasonCodes = tLookUpManager.GetAll<ApprovalContextDTO>("DSMGMCOL", "ReasonContext");
                    ViewBag.ReasonCodes = reasonCodes;
                }

                ViewBag.Privileges = tCommonManager.GetUserPrivilegesForEntity("MT_DISCOUNT_ON_MGMT_COLLECTION");

                ViewBag.DiscountOnMgmtCollections = new JavaScriptSerializer().Serialize(DiscountOnMgmtCollectionDTO);
                ViewData["FormStatus"] = DiscountOnMgmtCollectionDTO.ApprovalStatus;
                ViewData["FormCall"] = "ApprovalQueue";
                ViewBag.ProductList = productCodes;
                ViewBag.ContractList = contractCodes;
                ViewBag.Commission = commissionCode;

                return View("DiscountOnMgmtCollectionAdd", DiscountOnMgmtCollectionDTO);
            }
            else
                return RedirectToAction("ApprovalQueueList", "Approval");
        }
        #endregion
        #region Validate DiscountOnMgmtCollection Details
        public JsonResult ValidateDiscountOnMgmtCollectionDetails(DiscountOnMgmtCollectionDTO ADiscountOnMgmtCollectionDTO, string Command)
        {
            bool isOverlapped = false;
            string Message = string.Empty;

            if (ADiscountOnMgmtCollectionDTO != null)
            {
                if (ADiscountOnMgmtCollectionDTO.DiscountOnMgmtCollectionDetailsDTO.Count > 0)
                {

                    int intloop = 0;
                    for (int intLoop = 0; intLoop < ADiscountOnMgmtCollectionDTO.DiscountOnMgmtCollectionDetailsDTO.Count; intLoop++)
                    {
                        for (intloop = intLoop + 1; intloop != intLoop && intloop < ADiscountOnMgmtCollectionDTO.DiscountOnMgmtCollectionDetailsDTO.Count; intloop++)
                        {
                            if (Utils.CheckRangeOverlaps(ADiscountOnMgmtCollectionDTO.DiscountOnMgmtCollectionDetailsDTO[intLoop].ConsumptionRangeFrom, ADiscountOnMgmtCollectionDTO.DiscountOnMgmtCollectionDetailsDTO[intLoop].ConsumptionRangeTo, ADiscountOnMgmtCollectionDTO.DiscountOnMgmtCollectionDetailsDTO[intloop].ConsumptionRangeFrom, ADiscountOnMgmtCollectionDTO.DiscountOnMgmtCollectionDetailsDTO[intloop].ConsumptionRangeTo))
                            {
                                isOverlapped = true;
                            }
                        }
                    }
                }
                else
                {
                    Message = String.Format(Resources.MsgErrOneValid, Resources.ConsumptionRange);
                }
            }
            else
            {
                Message = String.Format(Resources.MsgErrOneValid, Resources.ConsumptionRange);
            }

            if (isOverlapped)
            {
                Message = String.Format(Resources.MsgErrOverlapValid, Resources.ConsumptionRange);
            }
            if (Message != string.Empty)
            {
                isOverlapped = true;
            }


            return Json(new { Message, isOverlapped }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #endregion

        #region CollectionCharge

        #region CollectionChargeView
        [AuthorizeUserAttribute(Entity = "MT_COLLECTION_CHARGE", AccessLevel = new UserActivities[] { UserActivities.View })]
        public ActionResult CollectionChargeView(string ACollectionChargeId)
        {
            try
            {
                CollectionChargeDTO CollectionChargeDTO;
                ViewBag.TableName = "COLCHRG";
                var productCodes = tLookUpManager.GetAll<ProductDTO>("", "Product");
                var currencyList = tLookUpManager.GetAll<SystemParameterDTO>(string.Empty, "Currency");
                ViewBag.Currency = currencyList;
                ViewBag.Privileges = tCommonManager.GetUserPrivilegesForEntity("MT_COLLECTION_CHARGE");
                ViewBag.ProductList = productCodes;

                if (Convert.ToInt32(ACollectionChargeId) == 0)
                {
                    CollectionChargeDTO = new CollectionChargeDTO();
                    CollectionChargeDTO.EffectiveDate = Utils.BusinessDate;
                    ViewBag.CollectionCharges = new JavaScriptSerializer().Serialize(CollectionChargeDTO);
                    ViewData["FormStatus"] = CollectionChargeDTO.ApprovalStatus;
                    return View("CollectionChargeAdd", CollectionChargeDTO);
                }
                else
                {
                    CollectionChargeDTO = tMasterManager.FindByCollectionChargeId(Convert.ToInt32(ACollectionChargeId));
                    ViewBag.CollectionCharges = new JavaScriptSerializer().Serialize(CollectionChargeDTO);
                    ViewBag.ReasonCodes = null;
                }

                ViewData["FormStatus"] = CollectionChargeDTO.ApprovalStatus;
                return View("CollectionChargeAdd", CollectionChargeDTO);
            }
            catch (Exception ex)
            {
                LoggerFactory.CreateLog().LogError("Error" + ex.Message, 100, 2);
                ViewBag.ErrorMessage = Resources.MsgErrServerException;
                ViewBag.ErrorType = SessionErrorType.ServerException;
                return View("Error");
            }
        }
        #endregion
        #region CollectionChargeList
        [AuthorizeUserAttribute(Entity = "MT_COLLECTION_CHARGE", AccessLevel = new UserActivities[] { UserActivities.View })]
        public ActionResult CollectionChargeList(CollectionChargeSearch AVMCollectionChargeSearch, string ClearButton, int? page)
        {
            try
            {

                int iPageNumber = Convert.ToInt32((page == null) ? 0 : page);

                var productCodes = tLookUpManager.GetAll<ProductDTO>("", "Product");

                if (ClearButton == Resources.Clear)
                {
                    AVMCollectionChargeSearch.ProductCode = null;
                    AVMCollectionChargeSearch.EffectiveDate = Utils.BusinessDate;
                    ModelState.Clear();
                }

                if ((Utils.OrganizationCode != string.Empty) || 
                    (!(string.IsNullOrEmpty(AVMCollectionChargeSearch.SearchButton)) &&
                     !((string.IsNullOrWhiteSpace(AVMCollectionChargeSearch.ProductCode) &&
                       (AVMCollectionChargeSearch.EffectiveDate == null)))))
                {
                    var entities = tMasterManager.FindAll
                        (
                            iPageNumber,
                                new CollectionChargeDTO()
                                {
                                    ProductCode = AVMCollectionChargeSearch.ProductCode,
                                    EffectiveDate = AVMCollectionChargeSearch.EffectiveDate
                                }
                        );
                    AVMCollectionChargeSearch.SearchResults = entities.AsQueryable();
                }
                else
                {
                    var currResult = tMasterManager.FindCollectionCharge(1, Utils.GRD_ROWS_PER_PAGE).AsQueryable();
                    AVMCollectionChargeSearch.EffectiveDate = Utils.BusinessDate;
                    AVMCollectionChargeSearch.SearchResults = currResult.AsQueryable();
                }
                ViewBag.ProductList = productCodes;
                ViewBag.Privileges = tCommonManager.GetUserPrivilegesForEntity("MT_COLLECTION_CHARGE");
                return View(AVMCollectionChargeSearch);
            }
            catch (Exception ex)
            {
                LoggerFactory.CreateLog().LogError("Error" + ex.Message, 100, 2);
                ViewBag.ErrorMessage = Resources.MsgErrServerException;
                ViewBag.ErrorType = SessionErrorType.ServerException;
                return View("Error");
            }
        }
        #endregion
        #region CollectionChargeAdd
        [AuthorizeUserAttribute(Entity = "MT_COLLECTION_CHARGE", AccessLevel = new UserActivities[] { UserActivities.Add, UserActivities.Update, UserActivities.View })]
        public ActionResult CollectionChargeAdd(string ACollectionChargeId)
        {
            try
            {
                CollectionChargeDTO CollectionChargeDTO;
                var productCodes = tLookUpManager.GetAll<ProductDTO>("", "Product");
                var currencyList = tLookUpManager.GetAll<SystemParameterDTO>(string.Empty, "Currency");
                ViewBag.Currency = currencyList;
                if (Convert.ToInt32(ACollectionChargeId) == 0)
                {
                    CollectionChargeDTO = new CollectionChargeDTO();
                    CollectionChargeDTO.EffectiveDate = Utils.BusinessDate;
                }
                else
                    CollectionChargeDTO = tMasterManager.FindByCollectionChargeId(Convert.ToInt32(ACollectionChargeId));

                if (!String.IsNullOrWhiteSpace(CollectionChargeDTO.ApprovalStatus) && CollectionChargeDTO.ApprovalStatus.Equals("P"))
                {
                    var reasonCodes = tLookUpManager.GetAll<ApprovalContextDTO>("COLCHRG", "ReasonContext");
                    ViewBag.ReasonCodes = reasonCodes;
                }

                ViewBag.Privileges = tCommonManager.GetUserPrivilegesForEntity("MT_COLLECTION_CHARGE");
                ViewBag.CollectionCharges = new JavaScriptSerializer().Serialize(CollectionChargeDTO);
                ViewData["FormStatus"] = CollectionChargeDTO.ApprovalStatus;

                ViewBag.ProductList = productCodes;

                return View(CollectionChargeDTO);
            }
            catch (Exception ex)
            {
                LoggerFactory.CreateLog().LogError("Error" + ex.Message, 100, 2);
                ViewBag.ErrorMessage = Resources.MsgErrServerException;
                ViewBag.ErrorType = SessionErrorType.ServerException;
                return View("Error");
            }
        }
        #endregion
        #region CollectionChargeSave
        [HttpPost]
        [AuthorizeUserAttribute(Entity = "MT_COLLECTION_CHARGE", AccessLevel = new UserActivities[] { UserActivities.Add, UserActivities.Update })]
        public ActionResult CollectionChargeSave(CollectionChargeDTO ACollectionChargeDTO, string Command)
        {

            string redirectUrl = string.Empty, messageToShow = string.Empty;

            if (Command.Equals(Resources.Submit))
                ACollectionChargeDTO.ApprovalStatus = "P";
            else if (Command.Equals(Resources.Save))
                ACollectionChargeDTO.ApprovalStatus = "D";

            ACollectionChargeDTO.OrganizationCode = Utils.OrganizationCode;
            if (Command.Equals(Resources.Submit) || Command.Equals(Resources.Save))
            {
                if (string.IsNullOrWhiteSpace(tMasterManager.FindByCollectionChargeId(Convert.ToInt32(ACollectionChargeDTO.CollectionChargeId)).ProductCode))
                {
                    ACollectionChargeDTO.CreatedBy = tCommonManager.GetLoggedUserName();
                    ACollectionChargeDTO.LastUpdatedBy = tCommonManager.GetLoggedUserName();
                    tMasterManager.SaveCollectionChargeDetails(ACollectionChargeDTO);
                    redirectUrl = Url.Action("CollectionChargeAdd", "Master", new { ACollectionChargeId = ACollectionChargeDTO.CollectionChargeId });
                    messageToShow = String.Format(Resources.MsgInfoSaveSuccess, Resources.CollectionCharges); 
                }
                else
                {
                    ACollectionChargeDTO.LastUpdatedBy = tCommonManager.GetLoggedUserName();
                    tMasterManager.UpdateCollectionChargeDetails(ACollectionChargeDTO.CollectionChargeId, ACollectionChargeDTO);
                    redirectUrl = Url.Action("CollectionChargeAdd", "Master", new { ACollectionChargeId = ACollectionChargeDTO.CollectionChargeId });
                    messageToShow = String.Format(Resources.MsgInfoSaveSuccess, Resources.CollectionCharges); 
                }
            }

            return Json(new { RedirectUrl = redirectUrl, Message = messageToShow }, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region CollectionChargeDelete
        [HttpPost]
        [AuthorizeUserAttribute(Entity = "MT_COLLECTION_CHARGE", AccessLevel = new UserActivities[] { UserActivities.Delete })]
        public JsonResult CollectionChargeDelete(string ACollectionChargeId, bool ASearchGridClick = false)
        {
            CollectionChargeDTO ACollectionChargeDTO = new CollectionChargeDTO();
            string redirectUrl = string.Empty, messageToShow = string.Empty;
            try
            {
                ACollectionChargeDTO = tMasterManager.FindByCollectionChargeId(Convert.ToInt32(ACollectionChargeId));
                ACollectionChargeDTO.LastUpdatedBy = tCommonManager.GetLoggedUserName();
                ACollectionChargeDTO.ApprovalStatus = Utils.GetApprovalStatusCode(Utils.ApprovalStatus.Deleted);
                ACollectionChargeDTO.OrganizationCode = Utils.OrganizationCode;
                tMasterManager.UpdateCollectionChargeDetails(ACollectionChargeDTO.CollectionChargeId, ACollectionChargeDTO);

                redirectUrl = Url.Action("CollectionChargeAdd", "Master");
                messageToShow = String.Format(Resources.MsgInfoDeleteSuccess, Resources.CollectionCharges); 
            }
            catch
            {
                redirectUrl = Url.Action("CollectionChargeAdd", "Master");
                messageToShow = String.Format(Resources.MsgErrDeleteConcurrencyError, Resources.CollectionCharges); 
            }

            if (ASearchGridClick)
            {
                messageToShow = String.Format(Resources.MsgInfoDeleteSuccess, Resources.CollectionCharges); 
            }
            else
            {
                TempData["Message"] = null;
            }

            return Json(new { RedirectUrl = redirectUrl, Message = messageToShow }, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region CollectionChargeAuthorize
        [AuthorizeUserAttribute(Entity = "MT_COLLECTION_CHARGE", AccessLevel = new UserActivities[] { UserActivities.Authorize })]
        public ActionResult CollectionChargeAuthorize(string ACollectionChargeId, string AMode, string QId)
        {
            ViewBag.RequestQueueId = QId;
            ViewBag.TableName = "COLCHRG";

            var productCodes = tLookUpManager.GetAll<ProductDTO>("", "Product");
            var currencyList = tLookUpManager.GetAll<SystemParameterDTO>(string.Empty, "Currency");
            ViewBag.Currency = currencyList;

            if (!String.IsNullOrWhiteSpace(ACollectionChargeId))
            {
                CollectionChargeDTO CollectionChargeDTO = tMasterManager.FindByCollectionChargeId(Convert.ToInt32(ACollectionChargeId));

                if (!String.IsNullOrWhiteSpace(CollectionChargeDTO.ApprovalStatus) && CollectionChargeDTO.ApprovalStatus.Equals("P"))
                {
                    var reasonCodes = tLookUpManager.GetAll<ApprovalContextDTO>("COLCHRG", "ReasonContext");
                    ViewBag.ReasonCodes = reasonCodes;
                }

                ViewBag.Privileges = tCommonManager.GetUserPrivilegesForEntity("MT_COLLECTION_CHARGE");
                ViewData["FormStatus"] = CollectionChargeDTO.ApprovalStatus;
                ViewData["FormCall"] = "ApprovalQueue";
                ViewBag.ProductList = productCodes;
                ViewBag.CollectionCharges = new JavaScriptSerializer().Serialize(CollectionChargeDTO);
                return View("CollectionChargeAdd", CollectionChargeDTO);
            }
            else
                return RedirectToAction("ApprovalQueueList", "Approval");
        }
        #endregion
        #region Validate CollectionCharge Details
        public JsonResult ValidateCollectionChargeDetails(CollectionChargeDTO ACollectionChargeDTO, string Command)
        {
            bool isOverlapped = false;
            string Message = string.Empty;

            if (ACollectionChargeDTO != null)
            {
                if (ACollectionChargeDTO.CollectionChargeDetailsDTO.Count > 0)
                {

                    int intloop = 0;
                    for (int intLoop = 0; intLoop < ACollectionChargeDTO.CollectionChargeDetailsDTO.Count; intLoop++)
                    {
                        for (intloop = intLoop + 1; intloop != intLoop && intloop < ACollectionChargeDTO.CollectionChargeDetailsDTO.Count; intloop++)
                        {
                            if (Utils.CheckRangeOverlaps(ACollectionChargeDTO.CollectionChargeDetailsDTO[intLoop].DelinquencyAmountFrom, ACollectionChargeDTO.CollectionChargeDetailsDTO[intLoop].DelinquencyAmountTo, ACollectionChargeDTO.CollectionChargeDetailsDTO[intloop].DelinquencyAmountFrom, ACollectionChargeDTO.CollectionChargeDetailsDTO[intloop].DelinquencyAmountTo))
                            {
                                isOverlapped = true;
                            }
                        }
                    }
                }
                else
                {
                    Message = String.Format(Resources.MsgErrOneValid, Resources.CollectionCharges);
                }
            }
            else
            {
                Message = String.Format(Resources.MsgErrOneValid, Resources.CollectionCharges);
            }
            if (isOverlapped)
            {
                Message = String.Format(Resources.MsgErrOverlapValid, Resources.DelinquencyAmount);
            }
            if (Message != string.Empty)
            {
                isOverlapped = true;
            }

            return Json(new { Message, isOverlapped }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #endregion

        #region Plan Master

        #region PlanMasterList
        [AuthorizeUserAttribute(Entity = "MT_PLAN_MASTER", AccessLevel = new UserActivities[] { UserActivities.View })]
        public ActionResult PlanMasterList(PlanMasterSearch AVMPlanMasterSearch, string ClearButton, int? page)
        {

            try
            {

                int iPageNumber = Convert.ToInt32((page == null) ? 0 : page);

                if (ClearButton == Resources.Clear)
                {
                    AVMPlanMasterSearch.CreditSegmentCode = null;
                    AVMPlanMasterSearch.ProductCode = null;
                    AVMPlanMasterSearch.CustomerSegmentCode = null;
                    AVMPlanMasterSearch.CustomerSubSegmentCode = null;
                    ModelState.Clear();
                }

                var CreditSegmentCodeList = tLookUpManager.GetAll<CreditSegmentDTO>(string.Empty, "CreditSegmentPromo");
                var ProductCodeList = tLookUpManager.GetAll<ProductDTO>(string.Empty, "Product");
                var CustomerSegmentCodeList = tLookUpManager.GetAll<CustomerSegmentDTO>(string.Empty, "CustomerSegment");
                var CustomerSubSegmentCodeList = tLookUpManager.GetAll<CustomerSubSegmentDTO>(string.Empty, "CustomerSubSegment");


                if ((!string.IsNullOrEmpty(Utils.OrganizationCode)) || (!(string.IsNullOrEmpty(AVMPlanMasterSearch.SearchButton)) &&
                    !((string.IsNullOrWhiteSpace(AVMPlanMasterSearch.ProductCode)
                    && (string.IsNullOrWhiteSpace(AVMPlanMasterSearch.CreditSegmentCode))
                    && (string.IsNullOrWhiteSpace(AVMPlanMasterSearch.CustomerSegmentCode))
                    && (string.IsNullOrWhiteSpace(AVMPlanMasterSearch.CustomerSubSegmentCode))
                    ))))
                {
                    var entities = tMasterManager.FindAll(iPageNumber, new PlanMasterDTO()
                    {
                        CreditSegmentCode = AVMPlanMasterSearch.CreditSegmentCode,
                        ProductCode = AVMPlanMasterSearch.ProductCode,
                        CustomerSegmentCode = AVMPlanMasterSearch.CustomerSegmentCode,
                        CustomerSubSegmentCode = AVMPlanMasterSearch.CustomerSubSegmentCode
                    });
                    AVMPlanMasterSearch.SearchResults = entities.AsQueryable();
                }
                else
                {
                    var planResult = tMasterManager.FindPlanMaster(1, Utils.GRD_ROWS_PER_PAGE).AsQueryable();
                    AVMPlanMasterSearch.SearchResults = planResult.AsQueryable();
                }

                ViewBag.Privileges = tCommonManager.GetUserPrivilegesForEntity("MT_PLAN_MASTER");

                ViewBag.CreditSegmentCodeItems = CreditSegmentCodeList;
                ViewBag.ProductCodeList = ProductCodeList;
                ViewBag.CustomerSegmentCodeList = CustomerSegmentCodeList;
                ViewBag.CustomerSubSegmentCodeList = CustomerSubSegmentCodeList;

                AVMPlanMasterSearch.SearchResults = AVMPlanMasterSearch.SearchResults.Where(x => x.ApprovalStatus != "DEL" && x.ApprovalStatus != "DIS");
                ViewBag.Privileges = tCommonManager.GetUserPrivilegesForEntity("MT_PLAN_MASTER");
                return View(AVMPlanMasterSearch);
            }
            catch (Exception ex)
            {
                LoggerFactory.CreateLog().LogError("Error" + ex.Message, 100, 2);
                ViewBag.ErrorMessage = Resources.MsgErrServerException;
                ViewBag.ErrorType = SessionErrorType.ServerException;
                return View("Error");
            }
        }
        #endregion
        #region PlanMasterAdd
        [AuthorizeUserAttribute(Entity = "MT_PLAN_MASTER", AccessLevel = new UserActivities[] { UserActivities.Add, UserActivities.Update, UserActivities.View })]
        public ActionResult PlanMasterAdd(string Id)
        {
            try
            {
                PlanMasterDTO planmasterDTO;
                var CreditSegmentCodeList = tLookUpManager.GetAll<CreditSegmentDTO>(string.Empty, "CreditSegmentPromo");
                var ProductCodeList = tLookUpManager.GetAll<ProductDTO>(string.Empty, "Product");
                var CustomerSegmentCodeList = tLookUpManager.GetAll<CustomerSegmentDTO>(string.Empty, "CustomerSegment");
                var CustomerSubSegmentCodeList = tLookUpManager.GetAll<CustomerSubSegmentDTO>(string.Empty, "CustomerSubSegment");
                var TransactionCodeList = tLookUpManager.GetAll<TransactionDTO>(string.Empty, "PlanTransaction");
                var FirstBillingList = tLookUpManager.GetAll<FirstBillingDTO>(string.Empty, "FirstBilling");
                var InterestCalculationList = tLookUpManager.GetAll<InterestCalculationDTO>(string.Empty, "InterestCalculation");
                ViewBag.TableName = "PLNMSTR";


                if (Id == null)
                {
                    planmasterDTO = new PlanMasterDTO();
                    planmasterDTO.ValidFromDate = Utils.BusinessDate;
                    planmasterDTO.ExpectedInterestRate = tMasterManager.GetSBIFRate(Utils.BusinessDate);
                    planmasterDTO.SBIFInterest = tMasterManager.GetSBIFRate(Utils.BusinessDate);
                    //planmasterDTO.ValidToDate = Utils.BusinessDate;
                }
                else
                    planmasterDTO = tMasterManager.FindByPlanID(Convert.ToInt32(Id));
                if (!String.IsNullOrWhiteSpace(planmasterDTO.ApprovalStatus) && planmasterDTO.ApprovalStatus.Equals("P"))
                {
                    var reasonCodes = tLookUpManager.GetAll<ApprovalContextDTO>("PLNMSTR", "ReasonContext");
                    ViewBag.ReasonCodes = reasonCodes;

                }


                ViewBag.Privileges = tCommonManager.GetUserPrivilegesForEntity("MT_PLAN_MASTER");
                ViewData["FormStatus"] = planmasterDTO.ApprovalStatus;
                ViewBag.CreditSegmentCodeItems = CreditSegmentCodeList;
                ViewBag.ProductCodeList = ProductCodeList;
                ViewBag.CustomerSegmentCodeList = CustomerSegmentCodeList;
                ViewBag.CustomerSubSegmentCodeList = CustomerSubSegmentCodeList;
                ViewBag.TransactionCodeList = TransactionCodeList;
                ViewBag.FirstBillingList = FirstBillingList;
                ViewBag.InterestCalculationList = InterestCalculationList;
                return View(planmasterDTO);
            }
            catch (Exception ex)
            {
                LoggerFactory.CreateLog().LogError("Error" + ex.Message, 100, 2);
                ViewBag.ErrorMessage = Resources.MsgErrServerException;
                ViewBag.ErrorType = SessionErrorType.ServerException;
                return View("Error");
            }
        }
        #endregion
        #region PlanMasterView
        [AuthorizeUserAttribute(Entity = "MT_PLAN_MASTER", AccessLevel = new UserActivities[] { UserActivities.View })]
        public ActionResult PlanMasterView(string Id)
        {
            try
            {
                PlanMasterDTO planmasterDTO;
                var CreditSegmentCodeList = tLookUpManager.GetAll<CreditSegmentDTO>(string.Empty, "CreditSegmentPromo");
                var ProductCodeList = tLookUpManager.GetAll<ProductDTO>(string.Empty, "Product");
                var CustomerSegmentCodeList = tLookUpManager.GetAll<CustomerSegmentDTO>(string.Empty, "CustomerSegment");
                var CustomerSubSegmentCodeList = tLookUpManager.GetAll<CustomerSubSegmentDTO>(string.Empty, "CustomerSubSegment");
                var TransactionCodeList = tLookUpManager.GetAll<TransactionDTO>(string.Empty, "PlanTransaction");
                var FirstBillingList = tLookUpManager.GetAll<FirstBillingDTO>(string.Empty, "FirstBilling");
                var InterestCalculationList = tLookUpManager.GetAll<InterestCalculationDTO>(string.Empty, "InterestCalculation");
                ViewBag.TableName = "PLNMSTR";

                ViewBag.Privileges = tCommonManager.GetUserPrivilegesForEntity("MT_PLAN_MASTER");
                ViewBag.CreditSegmentCodeItems = CreditSegmentCodeList;
                ViewBag.ProductCodeList = ProductCodeList;
                ViewBag.CustomerSegmentCodeList = CustomerSegmentCodeList;
                ViewBag.CustomerSubSegmentCodeList = CustomerSubSegmentCodeList;
                ViewBag.TransactionCodeList = TransactionCodeList;
                ViewBag.FirstBillingList = FirstBillingList;
                ViewBag.InterestCalculationList = InterestCalculationList;



                if (String.IsNullOrWhiteSpace(Id))
                {
                    planmasterDTO = new PlanMasterDTO();
                    planmasterDTO.ValidFromDate = Utils.BusinessDate;
                    //planmasterDTO.ValidToDate = Utils.BusinessDate;
                    ViewData["FormStatus"] = "";
                    return View("PlanMasterAdd", planmasterDTO);
                }
                else
                {
                    planmasterDTO = tMasterManager.FindByPlanID(Convert.ToInt32(Id));
                    ViewData["FormStatus"] = planmasterDTO.ApprovalStatus;
                    ViewBag.ReasonCodes = null;
                }


                return View("PlanMasterAdd", planmasterDTO);
            }
            catch (Exception ex)
            {
                LoggerFactory.CreateLog().LogError("Error" + ex.Message, 100, 2);
                ViewBag.ErrorMessage = Resources.MsgErrServerException;
                ViewBag.ErrorType = SessionErrorType.ServerException;
                return View("Error");
            }
        }
        #endregion
        #region PlanMasterSave
        [HttpPost]
        [AuthorizeUserAttribute(Entity = "MT_PLAN_MASTER", AccessLevel = new UserActivities[] { UserActivities.Add, UserActivities.Update })]
        public ActionResult PlanMasterSave(string Id, string Command, PlanMasterDTO APlanMasterDTO)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (Command.Equals(Resources.Submit))
                        APlanMasterDTO.ApprovalStatus = "P";
                    else if (Command.Equals(Resources.Save))
                        APlanMasterDTO.ApprovalStatus = "D";

                    if (Command.Equals(Resources.Submit) || Command.Equals(Resources.Save))
                    {
                        if (string.IsNullOrWhiteSpace(tMasterManager.FindByPlanID(Convert.ToInt32(Id)).CreditSegmentCode))
                        {
                            APlanMasterDTO.CreatedBy = tCommonManager.GetLoggedUserName();
                            APlanMasterDTO.LastUpdatedBy = tCommonManager.GetLoggedUserName();
                            APlanMasterDTO.OrganizationCode = Utils.OrganizationCode;

                            tMasterManager.SavePlanMasterDetails(APlanMasterDTO);
                            TempData["Message"] = String.Format(Resources.MsgInfoSaveSuccess, Resources.PlanMaster); 
                        }
                        else
                        {
                            if (!string.IsNullOrWhiteSpace(Id))
                            {
                                APlanMasterDTO.LastUpdatedBy = tCommonManager.GetLoggedUserName();
                                tMasterManager.UpdatePlanMasterDetails(Convert.ToInt32(Id), APlanMasterDTO);
                                TempData["Message"] = String.Format(Resources.MsgInfoSaveSuccess, Resources.PlanMaster); 
                            }
                            else
                            {
                                TempData["Message"] = String.Format(Resources.MsgErrAlreadyExists, Resources.PlanMaster); 
                            }
                        }
                    }
                    else if (Command.Equals(Resources.Delete))
                    {
                        try
                        {
                            APlanMasterDTO.LastUpdatedBy = tCommonManager.GetLoggedUserName();
                            APlanMasterDTO.ApprovalStatus = Utils.GetApprovalStatusCode(Utils.ApprovalStatus.Deleted);
                            tMasterManager.UpdatePlanMasterDetails(Convert.ToInt32(Id), APlanMasterDTO);

                            TempData["Message"] = String.Format(Resources.MsgInfoDeleteSuccess, Resources.PlanMaster); 
                        }
                        catch (Exception ex)
                        {
                            LoggerFactory.CreateLog().LogError("Error" + ex.Message, 100, 2);
                            TempData["Message"] = String.Format(Resources.MsgErrDeleteConcurrencyError, Resources.PlanMaster); 
                            return RedirectToAction("CityAdd", "master", new { Id = Id });
                        }
                    }
                }
            }

            catch (Exception ex)
            {
                LoggerFactory.CreateLog().LogError("Error" + ex.Message, 100, 2);
                ViewBag.ErrorMessage = Resources.MsgErrServerException;
                ViewBag.ErrorType = SessionErrorType.ServerException;
                return View("Error");
            }

            return ((Command.Equals(Resources.Submit) || Command.Equals(Resources.Delete)) ? RedirectToAction("PlanMasterAdd", "master") : RedirectToAction("PlanMasterAdd", "master", new { Id = Id }));

        }
        #endregion
        #region PlanMasterDelete
        [HttpPost]
        [AuthorizeUserAttribute(Entity = "MT_PLAN_MASTER", AccessLevel = new UserActivities[] { UserActivities.Delete })]
        public JsonResult PlanMasterDelete(int Id)
        {
            PlanMasterDTO APlanMasterDTO = new PlanMasterDTO();
            try
            {
                APlanMasterDTO = tMasterManager.FindByPlanID(Id);
                APlanMasterDTO.LastUpdatedBy = tCommonManager.GetLoggedUserName();
                APlanMasterDTO.ApprovalStatus = Utils.GetApprovalStatusCode(Utils.ApprovalStatus.Deleted);
                tMasterManager.UpdatePlanMasterDetails(Convert.ToInt32(Id), APlanMasterDTO);

                var dResult = new { result = "success", action = "PlanMasterList", controller = "Master" };
                TempData["Message"] = String.Format(Resources.MsgInfoDeleteSuccess, Resources.PlanMaster); 
                return Json(dResult, JsonRequestBehavior.AllowGet);

            }
            catch 
            {
                var dResultFail = new { result = "fail", action = "PlanMasterList", controller = "Master" };
                TempData["Message"] = String.Format(Resources.MsgErrDeleteConcurrencyError, Resources.PlanMaster); 
                return Json(dResultFail, JsonRequestBehavior.AllowGet);

            }
        }
        #endregion
        #region PlanMasterAuthorize
        [AuthorizeUserAttribute(Entity = "MT_PLAN_MASTER", AccessLevel = new UserActivities[] { UserActivities.Authorize })]
        public ActionResult PlanMasterAuthorize(int Id, string AMode, string QId)
        {


            ViewBag.RequestQueueId = QId;
            ViewBag.TableName = "PLNMSTR";
            if (!String.IsNullOrWhiteSpace(Id.ToString()))
            {
                PlanMasterDTO planmasterDTO = tMasterManager.FindByPlanID(Id);
                var CreditSegmentCodeList = tLookUpManager.GetAll<CreditSegmentDTO>(string.Empty, "CreditSegmentPromo");
                var ProductCodeList = tLookUpManager.GetAll<ProductDTO>(string.Empty, "Product");
                var CustomerSegmentCodeList = tLookUpManager.GetAll<CustomerSegmentDTO>(string.Empty, "CustomerSegment");
                var CustomerSubSegmentCodeList = tLookUpManager.GetAll<CustomerSubSegmentDTO>(string.Empty, "CustomerSubSegment");
                var TransactionCodeList = tLookUpManager.GetAll<TransactionDTO>(string.Empty, "PlanTransaction");
                var FirstBillingList = tLookUpManager.GetAll<FirstBillingDTO>(string.Empty, "FirstBilling");
                var InterestCalculationList = tLookUpManager.GetAll<InterestCalculationDTO>(string.Empty, "InterestCalculation");
                if (!String.IsNullOrWhiteSpace(planmasterDTO.ApprovalStatus) && planmasterDTO.ApprovalStatus.Equals("P"))
                {
                    var reasonCodes = tLookUpManager.GetAll<ApprovalContextDTO>("PLNMSTR", "ReasonContext");
                    ViewBag.ReasonCodes = reasonCodes;
                }
                ViewBag.Privileges = tCommonManager.GetUserPrivilegesForEntity("MT_PLAN_MASTER");
                ViewData["FormStatus"] = planmasterDTO.ApprovalStatus;
                ViewData["FormCall"] = "ApprovalQueue";
                ViewBag.CreditSegmentCodeItems = CreditSegmentCodeList;
                ViewBag.ProductCodeList = ProductCodeList;
                ViewBag.CustomerSegmentCodeList = CustomerSegmentCodeList;
                ViewBag.CustomerSubSegmentCodeList = CustomerSubSegmentCodeList;
                ViewBag.TransactionCodeList = TransactionCodeList;
                ViewBag.FirstBillingList = FirstBillingList;
                ViewBag.InterestCalculationList = InterestCalculationList;
                return View("PlanMasterAdd", planmasterDTO);
            }
            else
                return RedirectToAction("ApprovalQueueList", "Approval");

        }
        #endregion
        #region GetSBIFRate
        public JsonResult GetSBIFRate()
        {
           var SBIFRate = tMasterManager.GetSBIFRate(Utils.BusinessDate);
            var SBIFRatedecimal = Convert.ToDecimal(SBIFRate, CultureInfo.CurrentCulture).ToString();
            return Json(SBIFRatedecimal, JsonRequestBehavior.AllowGet);
            
        }
        #endregion

        #endregion

        #region CardCreditSegment

        #region CardCreditSegmentList
        [AuthorizeUserAttribute(Entity = "MT_CardCreditSegment", AccessLevel = new UserActivities[] { UserActivities.View })]
        public ActionResult CardCreditSegmentList(CardCreditSegmentSearch AVMCardCreditSegmentSearch, string ClearButton, int? page)
        {
            try
            {
                int iPageNumber = Convert.ToInt32((page == null) ? 0 : page);

                if (ClearButton == Resources.Clear)
                {
                    AVMCardCreditSegmentSearch.RUT = null;
                    AVMCardCreditSegmentSearch.CardNumber = null;
                    AVMCardCreditSegmentSearch.CustomerSegmentCode = null;
                    AVMCardCreditSegmentSearch.CustomerSubSegmentCode = null;
                    AVMCardCreditSegmentSearch.CreditSegmentCode = null;
                    ModelState.Clear();
                }

                //var CreditSegmentList = tLookUpManager.GetAll<CreditSegmentDTO>(string.Empty, "CreditSegment");
                var CreditSegmentList = tLookUpManager.GetAll<CreditSegmentDTO>(string.Empty, "CreditSegmentPromo");
                var CustomerSegmentCodeList = tLookUpManager.GetAll<CustomerSegmentDTO>(string.Empty, "CustomerSegment");
                var CustomerSubSegmentCodeList = tLookUpManager.GetAll<CustomerSubSegmentDTO>(string.Empty, "CustomerSubSegment");

                if ((!string.IsNullOrEmpty(Utils.OrganizationCode)) || (!(string.IsNullOrEmpty(AVMCardCreditSegmentSearch.SearchButton)) &&
                !((string.IsNullOrWhiteSpace(AVMCardCreditSegmentSearch.RUT)) &&
                ((string.IsNullOrWhiteSpace(AVMCardCreditSegmentSearch.CardNumber)) &&
                ((string.IsNullOrWhiteSpace(AVMCardCreditSegmentSearch.CustomerSegmentCode)) &&
                ((string.IsNullOrWhiteSpace(AVMCardCreditSegmentSearch.CustomerSubSegmentCode)) &&
                ((string.IsNullOrWhiteSpace(AVMCardCreditSegmentSearch.CreditSegmentCode)))))))))
                {
                    var entities = tMasterManager.FindAllCardCreditSegments(iPageNumber, new CardCreditSegmentDTO()
                    {
                        RUT = AVMCardCreditSegmentSearch.RUT,
                        CardNumber = AVMCardCreditSegmentSearch.CardNumber,
                        CreditSegmentCode = AVMCardCreditSegmentSearch.CreditSegmentCode,
                        CustomerSegmentCode = AVMCardCreditSegmentSearch.CustomerSegmentCode,
                        CustomerSubSegmentCode = AVMCardCreditSegmentSearch.CustomerSubSegmentCode
                    });
                    AVMCardCreditSegmentSearch.SearchResults = entities.AsQueryable();
                }
                else
                {
                    var cardResult = tMasterManager.FindCardCreditSegment(1, Utils.GRD_ROWS_PER_PAGE).AsQueryable();
                    AVMCardCreditSegmentSearch.SearchResults = cardResult.AsQueryable();
                }

                ViewBag.Privileges = tCommonManager.GetUserPrivilegesForEntity("MT_CardCreditSegment");

                ViewBag.CustomerSegmentCodeList = CustomerSegmentCodeList;
                ViewBag.CustomerSubSegmentCodeList = CustomerSubSegmentCodeList;
                ViewBag.CreditSegmentList = CreditSegmentList;
                ViewBag.Privileges = tCommonManager.GetUserPrivilegesForEntity("MT_CardCreditSegment");
                return View(AVMCardCreditSegmentSearch);
            }
            catch (Exception ex)
            {
                LoggerFactory.CreateLog().LogError("Error" + ex.Message, 100, 2);
                ViewBag.ErrorMessage = Resources.MsgErrServerException;
                ViewBag.ErrorType = SessionErrorType.ServerException;
                return View("Error");
            }
        }
        #endregion
        #region CardCreditSegmentAdd
        [AuthorizeUserAttribute(Entity = "MT_CardCreditSegment", AccessLevel = new UserActivities[] { UserActivities.Add, UserActivities.Update, UserActivities.View })]
        public ActionResult CardCreditSegmentAdd(string ACardNumber, string ACreditSegmentCode)
        {
            try
            {
                CardCreditSegmentDTO cardCreditSegmentDTO;
                var ProductCodeList = tLookUpManager.GetAll<ProductDTO>(string.Empty, "Product");
                var CustomerSegmentCodeList = tLookUpManager.GetAll<CustomerSegmentDTO>(string.Empty, "CustomerSegment");
                var CustomerSubSegmentCodeList = tLookUpManager.GetAll<CustomerSubSegmentDTO>(string.Empty, "CustomerSubSegment");
                //var CreditSegmentList = tLookUpManager.GetAll<CreditSegmentDTO>(string.Empty, "CreditSegment");
                var CreditSegmentList = tLookUpManager.GetAll<CreditSegmentDTO>(string.Empty, "CreditSegmentPromo");
                var OrganizationCodeList = tLookUpManager.GetAll<OrganizationDTO>(string.Empty, "Organization");

                ViewBag.TableName = "CRDCRSG";

                if (ACardNumber == null && ACreditSegmentCode == null)
                {
                    cardCreditSegmentDTO = new CardCreditSegmentDTO();
                    cardCreditSegmentDTO.ValidFrom = Utils.BusinessDate;
                    cardCreditSegmentDTO.ValidTo = Utils.BusinessDate;
                }
                else
                {
                    cardCreditSegmentDTO = tMasterManager.FindByCardNumber(ACardNumber, ACreditSegmentCode);
                    //cardCreditSegmentDTO.RUT = cardCreditSegmentDTO.RUT.Contains("-") ? cardCreditSegmentDTO.RUT.Replace("-", string.Empty) : cardCreditSegmentDTO.RUT;
                }
                if (!String.IsNullOrWhiteSpace(cardCreditSegmentDTO.ApprovalStatus) && cardCreditSegmentDTO.ApprovalStatus.Equals("P"))
                {
                    var reasonCodes = tLookUpManager.GetAll<ApprovalContextDTO>("CRDCRSG", "ReasonContext");
                    ViewBag.ReasonCodes = reasonCodes;
                }
                ViewBag.Privileges = tCommonManager.GetUserPrivilegesForEntity("MT_CardCreditSegment");
                ViewData["FormStatus"] = cardCreditSegmentDTO.ApprovalStatus;
                ViewBag.ProductCodeList = ProductCodeList;
                ViewBag.CustomerSegmentCodeList = CustomerSegmentCodeList;
                ViewBag.CustomerSubSegmentCodeList = CustomerSubSegmentCodeList;
                ViewBag.CreditSegmentList = CreditSegmentList;
                ViewBag.OrganizationCodeList = OrganizationCodeList;
                return View(cardCreditSegmentDTO);
            }
            catch (Exception ex)
            {
                LoggerFactory.CreateLog().LogError("Error" + ex.Message, 100, 2);
                ViewBag.ErrorMessage = Resources.MsgErrServerException;
                ViewBag.ErrorType = SessionErrorType.ServerException;
                return View("Error");
            }
        }
        #endregion
        #region CardCreditSegmentView
        [AuthorizeUserAttribute(Entity = "MT_CardCreditSegment", AccessLevel = new UserActivities[] { UserActivities.View })]
        public ActionResult CardCreditSegmentView(string ACardNumber, string ACreditSegmentCode)
        {
            try
            {
                CardCreditSegmentDTO cardCreditSegmentDTO;
                var ProductCodeList = tLookUpManager.GetAll<ProductDTO>(string.Empty, "Product");
                var CustomerSegmentCodeList = tLookUpManager.GetAll<CustomerSegmentDTO>(string.Empty, "CustomerSegment");
                var CustomerSubSegmentCodeList = tLookUpManager.GetAll<CustomerSubSegmentDTO>(string.Empty, "CustomerSubSegment");
                //var CreditSegmentList = tLookUpManager.GetAll<CreditSegmentDTO>(string.Empty, "CreditSegment");
                var CreditSegmentList = tLookUpManager.GetAll<CreditSegmentDTO>(string.Empty, "CreditSegmentPromo");
                var OrganizationCodeList = tLookUpManager.GetAll<OrganizationDTO>(string.Empty, "Organization");
                ViewBag.TableName = "CRDCRSG";

                if (String.IsNullOrWhiteSpace(ACardNumber) || String.IsNullOrWhiteSpace(ACreditSegmentCode))
                {
                    cardCreditSegmentDTO = new CardCreditSegmentDTO();
                    cardCreditSegmentDTO.ValidFrom = Utils.BusinessDate;
                    cardCreditSegmentDTO.ValidTo = Utils.BusinessDate;
                }
                else
                {
                    cardCreditSegmentDTO = tMasterManager.FindByCardNumber(ACardNumber, ACreditSegmentCode);
                    cardCreditSegmentDTO.RUT = cardCreditSegmentDTO.RUT.Contains("-") ? cardCreditSegmentDTO.RUT.Replace("-", string.Empty) : cardCreditSegmentDTO.RUT;
                    ViewBag.ReasonCodes = null;
                }

                ViewBag.Privileges = tCommonManager.GetUserPrivilegesForEntity("MT_CardCreditSegment");
                ViewData["FormStatus"] = cardCreditSegmentDTO.ApprovalStatus;
                ViewBag.ProductCodeList = ProductCodeList;
                ViewBag.CustomerSegmentCodeList = CustomerSegmentCodeList;
                ViewBag.CustomerSubSegmentCodeList = CustomerSubSegmentCodeList;
                ViewBag.CreditSegmentList = CreditSegmentList;
                ViewBag.OrganizationCodeList = OrganizationCodeList;
                return View("CardCreditSegmentAdd", cardCreditSegmentDTO);
            }
            catch (Exception ex)
            {
                LoggerFactory.CreateLog().LogError("Error" + ex.Message, 100, 2);
                ViewBag.ErrorMessage = Resources.MsgErrServerException;
                ViewBag.ErrorType = SessionErrorType.ServerException;
                return View("Error");
            }
        }
        #endregion
        #region CardCreditSegmentSave
        [HttpPost]
        [AuthorizeUserAttribute(Entity = "MT_CardCreditSegment", AccessLevel = new UserActivities[] { UserActivities.Add, UserActivities.Update })]
        public ActionResult CardCreditSegmentSave(string ACardNumber, string ACreditSegmentCode, string Command, CardCreditSegmentDTO ACardCreditSegmentDTO)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (Command.Equals(Resources.Submit))
                        ACardCreditSegmentDTO.ApprovalStatus = "P";
                    else if (Command.Equals(Resources.Save))
                        ACardCreditSegmentDTO.ApprovalStatus = "D";

                    ACardCreditSegmentDTO.RUT = ACardCreditSegmentDTO.RUT.Contains("-") ? ACardCreditSegmentDTO.RUT.Replace("-", string.Empty) : ACardCreditSegmentDTO.RUT;
                    if (Command.Equals(Resources.Save) || Command.Equals(Resources.Submit))
                    {
                        var result = tMasterManager.FindByCardNumber(ACardCreditSegmentDTO.CardNumber, ACardCreditSegmentDTO.CreditSegmentCode);
                        if (string.IsNullOrWhiteSpace(result.CardNumber) && string.IsNullOrWhiteSpace(result.CreditSegmentCode))
                        {
                            ACardCreditSegmentDTO.CreatedBy = tCommonManager.GetLoggedUserName();
                            ACardCreditSegmentDTO.LastUpdatedBy = tCommonManager.GetLoggedUserName();

                            tMasterManager.SaveCardCreditSegmentDetails(ACardCreditSegmentDTO);
                            TempData["Message"] = String.Format(Resources.MsgInfoSaveSuccess, Resources.MT_CardCreditSegment); 
                        }
                        else
                        {
                            if (!(string.IsNullOrWhiteSpace(ACardNumber) && string.IsNullOrWhiteSpace(ACreditSegmentCode)))
                            {
                                ACardCreditSegmentDTO.LastUpdatedBy = tCommonManager.GetLoggedUserName();
                                tMasterManager.UpdateCardCreditSegmentDetails(ACardNumber, ACreditSegmentCode, ACardCreditSegmentDTO);
                                TempData["Message"] = String.Format(Resources.MsgInfoSaveSuccess, Resources.MT_CardCreditSegment); 
                            }
                            else
                            {
                                TempData["Message"] = String.Format(Resources.MsgErrAlreadyExists, Resources.MT_CardCreditSegment); 
                            }
                        }
                    }
                    else if (Command.Equals(Resources.Delete))
                    {
                        try
                        {
                            ACardCreditSegmentDTO.LastUpdatedBy = tCommonManager.GetLoggedUserName();
                            ACardCreditSegmentDTO.ApprovalStatus = Utils.GetApprovalStatusCode(Utils.ApprovalStatus.Deleted);
                            tMasterManager.UpdateCardCreditSegmentDetails(ACardNumber, ACreditSegmentCode, ACardCreditSegmentDTO);

                            TempData["Message"] = String.Format(Resources.MsgInfoDeleteSuccess, Resources.MT_CardCreditSegment); 
                        }
                        catch (Exception ex)
                        {
                            LoggerFactory.CreateLog().LogError("Error" + ex.Message, 100, 2);
                            TempData["Message"] = String.Format(Resources.MsgErrDeleteConcurrencyError, Resources.MT_CardCreditSegment); 
                            return RedirectToAction("CardCreditSegmentAdd", "master", new { ACardNumber = ACardNumber, ACreditSegmentCode = ACreditSegmentCode });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LoggerFactory.CreateLog().LogError("Error" + ex.Message, 100, 2);
                ViewBag.ErrorMessage = Resources.MsgErrServerException;
                ViewBag.ErrorType = SessionErrorType.ServerException;
                return View("Error");
            }

            return ((Command.Equals(Resources.Submit) || Command.Equals(Resources.Delete)) ? RedirectToAction("CardCreditSegmentAdd", "master")
                : RedirectToAction("CardCreditSegmentAdd", "master", new { ACardNumber = ACardNumber, ACreditSegmentCode = ACreditSegmentCode }));
        }
        #endregion
        #region CardCreditSegmentDelete
        [HttpPost]
        [AuthorizeUserAttribute(Entity = "MT_CardCreditSegment", AccessLevel = new UserActivities[] { UserActivities.Delete })]
        public JsonResult CardCreditSegmentDelete(string ACardNumber, string ACreditSegmentCode)
        {
            try
            {
                tMasterManager.DeleteCardCreditSegment(ACardNumber, ACreditSegmentCode);
                var dResult = new { result = "success", action = "CardCreditSegmentList", controller = "Master" };
                TempData["Message"] = String.Format(Resources.MsgInfoDeleteSuccess, Resources.MT_CardCreditSegment); 
                return Json(dResult, JsonRequestBehavior.AllowGet);
            }
            catch 
            {
                var dResultFail = new { result = "fail", action = "CardCreditSegmentList", controller = "Master" };
                TempData["Message"] = String.Format(Resources.MsgErrDeleteConcurrencyError, Resources.MT_CardCreditSegment); 
                return Json(dResultFail, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion
        #region CardCreditSegmentAuthorize
        [AuthorizeUserAttribute(Entity = "MT_CardCreditSegment", AccessLevel = new UserActivities[] { UserActivities.Authorize })]
        public ActionResult CardCreditSegmentAuthorize(string ACardNumber, string ACreditSegmentCode, string AMode, string QId)
        {
            ViewBag.RequestQueueId = QId;
            ViewBag.TableName = "CRDCRSG";
            if (!String.IsNullOrWhiteSpace(ACardNumber.ToString()) && !String.IsNullOrWhiteSpace(ACardNumber.ToString()))
            {
                CardCreditSegmentDTO cardCreditSegmentDTO = tMasterManager.FindByCardNumber(ACardNumber, ACreditSegmentCode);
                var ProductCodeList = tLookUpManager.GetAll<ProductDTO>(string.Empty, "Product");
                var CustomerSegmentCodeList = tLookUpManager.GetAll<CustomerSegmentDTO>(string.Empty, "CustomerSegment");
                var CustomerSubSegmentCodeList = tLookUpManager.GetAll<CustomerSubSegmentDTO>(string.Empty, "CustomerSubSegment");
                //var CreditSegmentList = tLookUpManager.GetAll<CreditSegmentDTO>(string.Empty, "CreditSegment");
                var CreditSegmentList = tLookUpManager.GetAll<CreditSegmentDTO>(string.Empty, "CreditSegmentPromo");
                var OrganizationCodeList = tLookUpManager.GetAll<OrganizationDTO>(string.Empty, "OrganizationCode");

                cardCreditSegmentDTO.RUT = cardCreditSegmentDTO.RUT.Contains("-") ? cardCreditSegmentDTO.RUT.Replace("-", string.Empty) : cardCreditSegmentDTO.RUT;
                if (!String.IsNullOrWhiteSpace(cardCreditSegmentDTO.ApprovalStatus) && cardCreditSegmentDTO.ApprovalStatus.Equals("P"))
                {
                    var reasonCodes = tLookUpManager.GetAll<ApprovalContextDTO>("CRDCRSG", "ReasonContext");
                    ViewBag.ReasonCodes = reasonCodes;
                }
                ViewBag.Privileges = tCommonManager.GetUserPrivilegesForEntity("MT_CardCreditSegment");
                ViewData["FormStatus"] = cardCreditSegmentDTO.ApprovalStatus;
                ViewData["FormCall"] = "ApprovalQueue";
                ViewBag.ProductCodeList = ProductCodeList;
                ViewBag.CustomerSegmentCodeList = CustomerSegmentCodeList;
                ViewBag.CustomerSubSegmentCodeList = CustomerSubSegmentCodeList;
                ViewBag.CreditSegmentList = CreditSegmentList;
                ViewBag.OrganizationCodeList = OrganizationCodeList;
                return View("CardCreditSegmentAdd", cardCreditSegmentDTO);
            }
            else
                return RedirectToAction("ApprovalQueueList", "Approval");
        }
        #endregion
        #region RUTCardNumberSearchList
        public ActionResult RUTCardNumberSearchList(string ARUT, string ACardnumber, string ACustomername,bool AIsLoading = false, bool AIsParentCustomerSearch = false)
        {
            List<RUTCardNumberSearchDTO> objRUTCardNumberSearchDTO = new List<RUTCardNumberSearchDTO>();
            //To get parent customers whose addon count doesnot exceeds maximum possible addon count of product to which the customer is tagged.(used in CustomerRUT.cshtml only)
            if (AIsParentCustomerSearch) //without SalesId
            {
                if (AIsLoading)
                  objRUTCardNumberSearchDTO = tMasterManager.GetParentCustomers(ARUT, ACardnumber, ACustomername).OrderBy(x => x.CustomerName).ToList();

                TempData["isParentCustomerSearch"] = true;
            }
            else //To list all customers(general purpose)
            {
                //objRUTCardNumberSearchDTO = tMasterManager.GetRUTCardNumberDetails(ARUT, ACardnumber, ACustomername).OrderBy(x => x.CustomerName).ToList();  // commented to use procedure to remove customer join.
                if (AIsLoading)
                    objRUTCardNumberSearchDTO = tMasterManager.GetAllRUTZoomDetails(ARUT, ACardnumber, ACustomername).OrderBy(x => x.CustomerName).ToList(); 
            }
            return PartialView("_RUTCardNumberSearchList", objRUTCardNumberSearchDTO);
        } 
        #endregion

        #endregion

        #region InsuranceCharge

        #region InsuranceChargeView
        [AuthorizeUserAttribute(Entity = "MT_INSURANCE_CHARGE", AccessLevel = new UserActivities[] { UserActivities.View })]
        public ActionResult InsuranceChargeView(string Id)
        {
            try
            {
                InsuranceChargeDTO insuranceChargeDTO;
                ViewBag.TableName = "INSCHRG";

                var contractList = tLookUpManager.GetAll<ContractDTO>(string.Empty, "Contract");
                var productList = tLookUpManager.GetAll<ProductDTO>(string.Empty, "Product");
                var insuranceTypeList = Utils.GetInsuranceType();
                var insuranceOptionList = Utils.GetInsuranceOption();
                var generalSpecialList = Utils.GetGeneralSpecial();
                var currencyList = tLookUpManager.GetAll<CurrencyDTO>(string.Empty, "Currency");

                if (Id == null)
                {
                    insuranceChargeDTO = new InsuranceChargeDTO();
                    insuranceChargeDTO.EffectiveDate = Utils.BusinessDate;
                    ViewData["FormStatus"] = insuranceChargeDTO.ApprovalStatus;
                }
                else
                {
                    insuranceChargeDTO = tMasterManager.FindByInsuranceChargeId(Convert.ToInt32(Id));
                    ViewData["FormStatus"] = insuranceChargeDTO.ApprovalStatus;
                }
                if (!String.IsNullOrWhiteSpace(insuranceChargeDTO.ApprovalStatus) && insuranceChargeDTO.ApprovalStatus.Equals("P"))
                {
                    var reasonCodes = tLookUpManager.GetAll<ApprovalContextDTO>("INSCHRG", "ReasonContext");
                    ViewBag.ReasonCodes = reasonCodes;
                }
                ViewBag.Privileges = tCommonManager.GetUserPrivilegesForEntity("MT_INSURANCE_CHARGE");
                ViewBag.ContractList = contractList;
                ViewBag.ProductList = productList;
                ViewBag.InsuranceTypeList = insuranceTypeList;
                ViewBag.InsuranceOptionList = insuranceOptionList;
                ViewBag.CurrencyList = currencyList;
                ViewBag.GeneralSpecialList = generalSpecialList;
                return View("InsuranceChargeAdd", insuranceChargeDTO);
            }
            catch (Exception ex)
            {
                LoggerFactory.CreateLog().LogError("Error" + ex.Message, 100, 2);
                ViewBag.ErrorMessage = Resources.MsgErrServerException;
                ViewBag.ErrorType = SessionErrorType.ServerException;
                return View("Error");
            }
        }
        #endregion
        #region InsuranceChargeAdd
        [AuthorizeUserAttribute(Entity = "MT_INSURANCE_CHARGE", AccessLevel = new UserActivities[] { UserActivities.Add, UserActivities.Update, UserActivities.View })]
        public ActionResult InsuranceChargeAdd(string Id)
        {
            try
            {
                InsuranceChargeDTO insuranceChargeDTO;

                var contractList = tLookUpManager.GetAll<ContractDTO>(string.Empty, "Contract");
                var productList = tLookUpManager.GetAll<ProductDTO>(string.Empty, "Product");
                var insuranceTypeList = Utils.GetInsuranceType();
                var insuranceOptionList = Utils.GetInsuranceOption();
                var generalSpecialList = Utils.GetGeneralSpecial();
                var currencyList = tLookUpManager.GetAll<CurrencyDTO>(string.Empty, "Currency");

                if (Id == null || Convert.ToInt32(Id) == 0)
                {
                    insuranceChargeDTO = new InsuranceChargeDTO();
                    insuranceChargeDTO.EffectiveDate = Utils.BusinessDate;
                }
                else
                {
                    insuranceChargeDTO = tMasterManager.FindByInsuranceChargeId(Convert.ToInt32(Id));
                }
                if (!String.IsNullOrWhiteSpace(insuranceChargeDTO.ApprovalStatus) && insuranceChargeDTO.ApprovalStatus.Equals("P"))
                {
                    var reasonCodes = tLookUpManager.GetAll<ApprovalContextDTO>("INSCHRG", "ReasonContext");
                    ViewBag.ReasonCodes = reasonCodes;
                }
                ViewBag.Privileges = tCommonManager.GetUserPrivilegesForEntity("MT_INSURANCE_CHARGE");
                ViewData["FormStatus"] = insuranceChargeDTO.ApprovalStatus;
                ViewBag.ContractList = contractList;
                ViewBag.ProductList = productList;
                ViewBag.InsuranceTypeList = insuranceTypeList;
                ViewBag.InsuranceOptionList = insuranceOptionList;
                ViewBag.CurrencyList = currencyList;
                ViewBag.GeneralSpecialList = generalSpecialList;
                return View(insuranceChargeDTO);
            }
            catch (Exception ex)
            {
                LoggerFactory.CreateLog().LogError("Error" + ex.Message, 100, 2);
                ViewBag.ErrorMessage = Resources.MsgErrServerException;
                ViewBag.ErrorType = SessionErrorType.ServerException;
                return View("Error");
            }            
        }
        #endregion
        #region InsuranceChargeSave
        [HttpPost]
        [AuthorizeUserAttribute(Entity = "MT_INSURANCE_CHARGE", AccessLevel = new UserActivities[] { UserActivities.Add, UserActivities.Update })]
        public ActionResult InsuranceChargeSave(string Id, string Command, InsuranceChargeDTO AInsuranceChargeDTO)
        {
            try
            {
                var errors = ModelState.Values.SelectMany(x => x.Errors);
                if (ModelState.IsValid)
                {
                    if (Command.Equals(Resources.Submit))
                        AInsuranceChargeDTO.ApprovalStatus = "P";
                    else if (Command.Equals(Resources.Save))
                        AInsuranceChargeDTO.ApprovalStatus = "D";

                    AInsuranceChargeDTO.OrganizationCode = Utils.OrganizationCode;
                    if (Command.Equals(Resources.Submit) || Command.Equals(Resources.Save))
                    {

                        if (tMasterManager.FindByInsuranceChargeId(Convert.ToInt32(Id)).InsuranceChargeId == 0)
                        {
                            AInsuranceChargeDTO.CreatedBy = tCommonManager.GetLoggedUserName();
                            AInsuranceChargeDTO.LastUpdatedBy = tCommonManager.GetLoggedUserName();
                            tMasterManager.SaveInsuranceChargeDetails(AInsuranceChargeDTO);
                            TempData["Message"] = String.Format(Resources.MsgInfoSaveSuccess, Resources.InsuranceCharges); 
                        }
                        else
                        {
                            if (!string.IsNullOrWhiteSpace(Id) && Convert.ToInt32(Id) > 0)
                            {
                                AInsuranceChargeDTO.LastUpdatedBy = tCommonManager.GetLoggedUserName();
                                tMasterManager.UpdateInsuranceChargeDetails(Convert.ToInt32(Id), AInsuranceChargeDTO);
                                TempData["Message"] = String.Format(Resources.MsgInfoSaveSuccess, Resources.InsuranceCharges); 
                            }
                            else
                            {
                                TempData["Message"] = String.Format(Resources.MsgErrAlreadyExists, Resources.InsuranceCharges); 
                            }
                        }

                    }
                    else if (Command.Equals(Resources.Delete))
                    {
                        try
                        {
                            AInsuranceChargeDTO.LastUpdatedBy = tCommonManager.GetLoggedUserName();
                            AInsuranceChargeDTO.ApprovalStatus = Utils.GetApprovalStatusCode(Utils.ApprovalStatus.Deleted);
                            tMasterManager.UpdateInsuranceChargeDetails(Convert.ToInt32(Id), AInsuranceChargeDTO);
                            TempData["Message"] = String.Format(Resources.MsgInfoDeleteSuccess, Resources.InsuranceCharges); 
                        }
                        catch (Exception ex)
                        {
                            LoggerFactory.CreateLog().LogError("Error" + ex.Message, 100, 2);
                            TempData["Message"] = String.Format(Resources.MsgErrDeleteConcurrencyError, Resources.InsuranceCharges); 
                            return RedirectToAction("InsuranceChargeAdd", "master", new { Id = Id });
                        }
                    }
                }
            }

            catch (Exception ex)
            {
                LoggerFactory.CreateLog().LogError("Error" + ex.Message, 100, 2);
                ViewBag.ErrorMessage = Resources.MsgErrServerException;
                ViewBag.ErrorType = SessionErrorType.ServerException;
                return View("Error");
            }
            return ((Command.Equals(Resources.Submit) || Command.Equals(Resources.Delete)) ? RedirectToAction("InsuranceChargeAdd", "Master") : RedirectToAction("InsuranceChargeAdd", "Master", new { Id = Id }));
        }
        #endregion
        #region InsuranceChargeDelete
        [AuthorizeUserAttribute(Entity = "MT_INSURANCE_CHARGE", AccessLevel = new UserActivities[] { UserActivities.Delete })]
        public JsonResult InsuranceChargeDelete(string Id)
        {
            InsuranceChargeDTO AInsuranceChargeDTO = new InsuranceChargeDTO();
            try
            {
                AInsuranceChargeDTO = tMasterManager.FindByInsuranceChargeId(Convert.ToInt32(Id));
                AInsuranceChargeDTO.LastUpdatedBy = tCommonManager.GetLoggedUserName();
                AInsuranceChargeDTO.ApprovalStatus = Utils.GetApprovalStatusCode(Utils.ApprovalStatus.Deleted);
                AInsuranceChargeDTO.OrganizationCode = Utils.OrganizationCode;
                tMasterManager.UpdateInsuranceChargeDetails(Convert.ToInt32(Id), AInsuranceChargeDTO);

                var dResult = new { result = "success", action = "InsuranceChargeList", controller = "Master" };
                TempData["Message"] = String.Format(Resources.MsgInfoDeleteSuccess, Resources.InsuranceCharges); 
                return Json(dResult, JsonRequestBehavior.AllowGet);

            }
            catch 
            {
                var dResultFail = new { result = "fail", action = "InsuranceChargeList", controller = "Master" };
                TempData["Message"] = String.Format(Resources.MsgErrDeleteConcurrencyError, Resources.InsuranceCharges); 
                return Json(dResultFail, JsonRequestBehavior.AllowGet);

            }
        }
        #endregion
        #region InsuranceChargeList
        [AuthorizeUserAttribute(Entity = "MT_INSURANCE_CHARGE", AccessLevel = new UserActivities[] { UserActivities.View })]
        public ActionResult InsuranceChargeList(InsuranceChargeSearch AVMInsuranceChargeSearch, string ClearButton, int? page)
        {
            try
            {
                int iPageNumber = Convert.ToInt32((page == null) ? 0 : page);

                var contractList = tLookUpManager.GetAll<ContractDTO>(string.Empty, "Contract");
                var productList = tLookUpManager.GetAll<ProductDTO>(string.Empty, "Product");
                var insuranceTypeList = Utils.GetInsuranceType();
                if (ClearButton == Resources.Clear)
                {                    
                    AVMInsuranceChargeSearch.ProductCode = null;
                    AVMInsuranceChargeSearch.InsuranceType = null;
                    AVMInsuranceChargeSearch.PolicyCode = null;
                    ModelState.Clear();
                }
                if ((Utils.OrganizationCode != string.Empty) || (!(string.IsNullOrEmpty(AVMInsuranceChargeSearch.SearchButton)) &&
                     !((string.IsNullOrWhiteSpace(AVMInsuranceChargeSearch.ProductCode)) &&
                       (string.IsNullOrWhiteSpace(AVMInsuranceChargeSearch.InsuranceType)) &&
                       (string.IsNullOrWhiteSpace(AVMInsuranceChargeSearch.PolicyCode)))))
                {
                    var entities = tMasterManager.FindAll(iPageNumber, new InsuranceChargeDTO()
                    {
                        ProductCode = AVMInsuranceChargeSearch.ProductCode,
                        PolicyCode = AVMInsuranceChargeSearch.PolicyCode,
                        InsuranceType = AVMInsuranceChargeSearch.InsuranceType
                    });
                    AVMInsuranceChargeSearch.SearchResults = entities.AsQueryable();
                }
                else
                {
                    var currResult = tMasterManager.FindInsuranceCharge(1, Utils.GRD_ROWS_PER_PAGE).AsQueryable();
                    AVMInsuranceChargeSearch.SearchResults = currResult.AsQueryable();
                }

                ViewBag.ContractList = contractList;
                ViewBag.ProductList = productList;
                ViewBag.InsuranceTypeList = insuranceTypeList;
                ViewBag.Privileges = tCommonManager.GetUserPrivilegesForEntity("MT_INSURANCE_CHARGE");
                return View(AVMInsuranceChargeSearch);
            }
            catch (Exception ex)
            {
                LoggerFactory.CreateLog().LogError("Error" + ex.Message, 100, 2);
                ViewBag.ErrorMessage = Resources.MsgErrServerException;
                ViewBag.ErrorType = SessionErrorType.ServerException;
                return View("Error");
            }
        }
        #endregion
        #region InsuranceChargeAuthorize
        [AuthorizeUserAttribute(Entity = "MT_INSURANCE_CHARGE", AccessLevel = new UserActivities[] { UserActivities.Authorize })]
        public ActionResult InsuranceChargeAuthorize(string Id, string AMode, string QId)
        {
            ViewBag.RequestQueueId = QId;
            ViewBag.TableName = "INSCHRG";
            if (!String.IsNullOrWhiteSpace(Id))
            {
                InsuranceChargeDTO insuranceChargeDTO = tMasterManager.FindByInsuranceChargeId(Convert.ToInt32(Id));

                var contractList = tLookUpManager.GetAll<ContractDTO>(string.Empty, "Contract");
                var productList = tLookUpManager.GetAll<ProductDTO>(string.Empty, "Product");
                var insuranceTypeList = Utils.GetInsuranceType();
                var insuranceOptionList = Utils.GetInsuranceOption();
                var currencyList = tLookUpManager.GetAll<CurrencyDTO>(string.Empty, "Currency");
                var generalSpecialList = Utils.GetGeneralSpecial();
                if (!String.IsNullOrWhiteSpace(insuranceChargeDTO.ApprovalStatus) && insuranceChargeDTO.ApprovalStatus.Equals("P"))
                {
                    var reasonCodes = tLookUpManager.GetAll<ApprovalContextDTO>("INSCHRG", "ReasonContext");
                    ViewBag.ReasonCodes = reasonCodes;
                }
                ViewBag.Privileges = tCommonManager.GetUserPrivilegesForEntity("MT_INSURANCE_CHARGE");
                ViewData["FormCall"] = "ApprovalQueue";
                ViewData["FormStatus"] = insuranceChargeDTO.ApprovalStatus;
                ViewBag.ContractList = contractList;
                ViewBag.ProductList = productList;
                ViewBag.InsuranceTypeList = insuranceTypeList;
                ViewBag.InsuranceOptionList = insuranceOptionList;
                ViewBag.CurrencyList = currencyList;
                ViewBag.GeneralSpecialList = generalSpecialList;
                return View("InsuranceChargeAdd", insuranceChargeDTO);
            }
            else
                return RedirectToAction("ApprovalQueueList", "Approval");
        }
        #endregion

        #endregion

        #region StatementGrouping

        #region StatementGroupingView
        [AuthorizeUserAttribute(Entity = "MT_STATEMENT_GROUPING", AccessLevel = new UserActivities[] { UserActivities.View })]
        public ActionResult StatementGroupingView(string Id)
        {
            try
            {
                StatementGroupingDTO StatementGroupingDTO;
                ViewBag.TableName = "STMNTGRP";

                var transactionList = tLookUpManager.GetAll<TransactionDTO>(string.Empty, "Transaction");
                var productList = tLookUpManager.GetAll<ProductDTO>(string.Empty, "Product");
                //var productTypeList = tLookUpManager.GetAll<ProductTypeDTO>(string.Empty, "ProductType");
                var networkList = tLookUpManager.GetAll<NetworkDTO>(string.Empty, "Network");

                ViewBag.Privileges = tCommonManager.GetUserPrivilegesForEntity("MT_STATEMENT_GROUPING");

                ViewBag.TransactionList = transactionList;
                ViewBag.ProductList = productList;
                ViewBag.NetworkList = networkList;

                if (String.IsNullOrWhiteSpace(Id))
                {
                    StatementGroupingDTO = new StatementGroupingDTO();
                    ViewData["FormStatus"] = StatementGroupingDTO.ApprovalStatus;
                    return View("StatementGroupingAdd", StatementGroupingDTO);
                }
                else
                {
                    StatementGroupingDTO = tMasterManager.FindByStatementGroupingId(Convert.ToInt32(Id));
                    ViewBag.ReasonCodes = null;
                }

                ViewData["FormStatus"] = StatementGroupingDTO.ApprovalStatus;

                return View("StatementGroupingAdd", StatementGroupingDTO);
            }
            catch (Exception ex)
            {
                LoggerFactory.CreateLog().LogError("Error" + ex.Message, 100, 2);
                ViewBag.ErrorMessage = Resources.MsgErrServerException;
                ViewBag.ErrorType = SessionErrorType.ServerException;
                return View("Error");
            }
        }
        #endregion
        #region StatementGroupingAdd
        [AuthorizeUserAttribute(Entity = "MT_STATEMENT_GROUPING", AccessLevel = new UserActivities[] { UserActivities.Add, UserActivities.Update, UserActivities.View })]
        public ActionResult StatementGroupingAdd(string Id)
        {
            try
            {
                StatementGroupingDTO StatementGroupingDTO;

                var transactionList = tLookUpManager.GetAll<TransactionDTO>(string.Empty, "Transaction");
                var productList = tLookUpManager.GetAll<ProductDTO>(string.Empty, "Product");
                //var productTypeList = tLookUpManager.GetAll<ProductTypeDTO>(string.Empty, "ProductType");
                var networkList = tLookUpManager.GetAll<NetworkDTO>(string.Empty, "Network");

                if (Id == null)
                {
                    StatementGroupingDTO = new StatementGroupingDTO();
                }
                else
                {
                    StatementGroupingDTO = tMasterManager.FindByStatementGroupingId(Convert.ToInt32(Id));
                }
                if (!String.IsNullOrWhiteSpace(StatementGroupingDTO.ApprovalStatus) && StatementGroupingDTO.ApprovalStatus.Equals("P"))
                {
                    var reasonCodes = tLookUpManager.GetAll<ApprovalContextDTO>("STMNTGRP", "ReasonContext");
                    ViewBag.ReasonCodes = reasonCodes;
                }

                ViewBag.Privileges = tCommonManager.GetUserPrivilegesForEntity("MT_STATEMENT_GROUPING");

                ViewData["FormStatus"] = StatementGroupingDTO.ApprovalStatus;

                ViewBag.TransactionList = transactionList;
                ViewBag.ProductList = productList;
                ViewBag.NetworkList = networkList;

                return View(StatementGroupingDTO);
            }
            catch (Exception ex)
            {
                LoggerFactory.CreateLog().LogError("Error" + ex.Message, 100, 2);
                ViewBag.ErrorMessage = Resources.MsgErrServerException;
                ViewBag.ErrorType = SessionErrorType.ServerException;
                return View("Error");
            }
        }
        #endregion
        #region StatementGroupingSave
        [HttpPost]
        [AuthorizeUserAttribute(Entity = "MT_STATEMENT_GROUPING", AccessLevel = new UserActivities[] { UserActivities.Add, UserActivities.Update })]
        public ActionResult StatementGroupingSave(int Id, string Command, StatementGroupingDTO AStatementGroupingDTO)
        {
            try
            {
                var errors = ModelState.Values.SelectMany(x => x.Errors);
                if (ModelState.IsValid)
                {

                    if (Command.Equals(Resources.Submit))
                        AStatementGroupingDTO.ApprovalStatus = "P";
                    else if (Command.Equals(Resources.Save))
                        AStatementGroupingDTO.ApprovalStatus = "D";

                    if (Command.Equals(Resources.Submit) || Command.Equals(Resources.Save))
                    {
                        if (tMasterManager.FindByStatementGroupingId(Id).StatementGroupingId == 0)
                        {
                            AStatementGroupingDTO.CreatedBy = tCommonManager.GetLoggedUserName();
                            AStatementGroupingDTO.LastUpdatedBy = tCommonManager.GetLoggedUserName();
                            AStatementGroupingDTO.OrganizationCode = Utils.OrganizationCode;

                            tMasterManager.SaveStatementGroupingDetails(AStatementGroupingDTO);
                            TempData["Message"] = String.Format(Resources.MsgInfoSaveSuccess, Resources.MT_STATEMENT_GROUPING); 
                        }
                        else
                        {
                            if (!string.IsNullOrWhiteSpace(Convert.ToString(Id)))
                            {
                                AStatementGroupingDTO.LastUpdatedBy = tCommonManager.GetLoggedUserName();

                                tMasterManager.UpdateStatementGroupingDetails(Id, AStatementGroupingDTO);

                                TempData["Message"] = String.Format(Resources.MsgInfoSaveSuccess, Resources.MT_STATEMENT_GROUPING); 
                            }
                            else
                            {
                                TempData["Message"] = String.Format(Resources.MsgErrAlreadyExists, Resources.MT_STATEMENT_GROUPING); 
                            }
                        }
                    }
                    else if (Command.Equals(Resources.Delete))
                    {
                        try
                        {
                            AStatementGroupingDTO.LastUpdatedBy = tCommonManager.GetLoggedUserName();
                            AStatementGroupingDTO.ApprovalStatus = Utils.GetApprovalStatusCode(Utils.ApprovalStatus.Deleted);
                            tMasterManager.UpdateStatementGroupingDetails(Id, AStatementGroupingDTO);

                            TempData["Message"] = String.Format(Resources.MsgInfoDeleteSuccess, Resources.MT_STATEMENT_GROUPING); 
                        }
                        catch (Exception ex)
                        {
                            LoggerFactory.CreateLog().LogError("Error" + ex.Message, 100, 2);
                            TempData["Message"] = String.Format(Resources.MsgErrDeleteConcurrencyError, Resources.MT_STATEMENT_GROUPING); 
                            return RedirectToAction("StatementGroupingAdd", "master", new { Id = Id });
                        }
                    }
                }
            }

            catch (Exception ex)
            {
                LoggerFactory.CreateLog().LogError("Error" + ex.Message, 100, 2);
                ViewBag.ErrorMessage = Resources.MsgErrServerException;
                ViewBag.ErrorType = SessionErrorType.ServerException;
                return View("Error");
            }

            return ((Command.Equals(Resources.Submit) || Command.Equals(Resources.Delete)) ? RedirectToAction("StatementGroupingAdd", "Master") : RedirectToAction("StatementGroupingAdd", "Master", new { Id = Id }));

        }
        #endregion
        #region StatementGroupingDelete
        [HttpPost]
        [AuthorizeUserAttribute(Entity = "MT_STATEMENT_GROUPING", AccessLevel = new UserActivities[] { UserActivities.Delete })]
        public JsonResult StatementGroupingDelete(int Id)
        {
            StatementGroupingDTO AStatementGroupingDTO = new StatementGroupingDTO();
            try
            {
                AStatementGroupingDTO = tMasterManager.FindByStatementGroupingId(Id);
                AStatementGroupingDTO.LastUpdatedBy = tCommonManager.GetLoggedUserName();
                AStatementGroupingDTO.ApprovalStatus = Utils.GetApprovalStatusCode(Utils.ApprovalStatus.Deleted);
                tMasterManager.UpdateStatementGroupingDetails(Id, AStatementGroupingDTO);

                var dResult = new { result = "success", action = "StatementGroupingList", controller = "Master" };
                TempData["Message"] = String.Format(Resources.MsgInfoDeleteSuccess, Resources.MT_STATEMENT_GROUPING); 
                return Json(dResult, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                var dResultFail = new { result = "fail", action = "StatementGroupingList", controller = "Master" };
                TempData["Message"] = String.Format(Resources.MsgErrDeleteConcurrencyError, Resources.MT_STATEMENT_GROUPING); 
                return Json(dResultFail, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion
        #region StatementGroupingList
        [AuthorizeUserAttribute(Entity = "MT_STATEMENT_GROUPING", AccessLevel = new UserActivities[] { UserActivities.View })]
        public ActionResult StatementGroupingList(StatementGroupingSearch AVMStatementGroupingSearch, string ClearButton, int? page)
        {
            try
            {
                int iPageNumber = Convert.ToInt32((page == null) ? 0 : page);

                var transactionList = tLookUpManager.GetAll<TransactionDTO>(string.Empty, "Transaction");
                var productList = tLookUpManager.GetAll<ProductDTO>(string.Empty, "Product");
                //var productTypeList = tLookUpManager.GetAll<ProductTypeDTO>(string.Empty, "ProductType");
                var networkList = tLookUpManager.GetAll<NetworkDTO>(string.Empty, "Network");

                if (ClearButton == Resources.Clear)
                {
                    AVMStatementGroupingSearch.ProductCode = null;
                    AVMStatementGroupingSearch.NetworkCode = null;
                    AVMStatementGroupingSearch.TransactionCode = null;
                    AVMStatementGroupingSearch.GroupCode = null;
                    AVMStatementGroupingSearch.GroupCodeDescription = null;
                    ModelState.Clear();
                }

                if ((!string.IsNullOrEmpty(Utils.OrganizationCode)) || (!(string.IsNullOrEmpty(AVMStatementGroupingSearch.SearchButton)) &&
                     !((string.IsNullOrWhiteSpace(AVMStatementGroupingSearch.ProductCode) &&
                       (string.IsNullOrWhiteSpace(AVMStatementGroupingSearch.NetworkCode)) &&
                       (string.IsNullOrWhiteSpace(AVMStatementGroupingSearch.TransactionCode)) &&
                       (string.IsNullOrWhiteSpace(AVMStatementGroupingSearch.GroupCode)) &&
                       (string.IsNullOrWhiteSpace(AVMStatementGroupingSearch.GroupCodeDescription))))))
                {
                    var entities = tMasterManager.FindAll(iPageNumber, new StatementGroupingDTO()
                    {
                        ProductCode = AVMStatementGroupingSearch.ProductCode,
                        NetworkCode = AVMStatementGroupingSearch.NetworkCode,
                        TransactionCode = AVMStatementGroupingSearch.TransactionCode,
                        GroupCode = AVMStatementGroupingSearch.GroupCode,
                        GroupCodeDescription = AVMStatementGroupingSearch.GroupCodeDescription
                    });
                    AVMStatementGroupingSearch.SearchResults = entities.AsQueryable();
                }
                else
                {
                    var currResult = tMasterManager.FindStatementGrouping(1, Utils.GRD_ROWS_PER_PAGE).AsQueryable();
                    AVMStatementGroupingSearch.SearchResults = currResult.AsQueryable();
                }

                ViewBag.TransactionList = transactionList;
                ViewBag.ProductList = productList;
                ViewBag.NetworkList = networkList;

                AVMStatementGroupingSearch.SearchResults = AVMStatementGroupingSearch.SearchResults.Where(x => x.ApprovalStatus != "DEL" && x.ApprovalStatus != "DIS");
                ViewBag.Privileges = tCommonManager.GetUserPrivilegesForEntity("MT_STATEMENT_GROUPING");
                return View(AVMStatementGroupingSearch);
            }
            catch (Exception ex)
            {
                LoggerFactory.CreateLog().LogError("Error" + ex.Message, 100, 2);
                ViewBag.ErrorMessage = Resources.MsgErrServerException;
                ViewBag.ErrorType = SessionErrorType.ServerException;
                return View("Error");
            }
        }
        #endregion
        #region StatementGroupingAuthorize
        [AuthorizeUserAttribute(Entity = "MT_STATEMENT_GROUPING", AccessLevel = new UserActivities[] { UserActivities.Authorize })]
        public ActionResult StatementGroupingAuthorize(string Id, string AMode, string QId)
        {
            ViewBag.RequestQueueId = QId;
            ViewBag.TableName = "STMNTGRP";
            if (!String.IsNullOrWhiteSpace(Id))
            {
                StatementGroupingDTO statementGroupingDTO = tMasterManager.FindByStatementGroupingId(Convert.ToInt32(Id));

                var transactionList = tLookUpManager.GetAll<TransactionDTO>(string.Empty, "Transaction");
                var productList = tLookUpManager.GetAll<ProductDTO>(string.Empty, "Product");
                //var productTypeList = tLookUpManager.GetAll<ProductTypeDTO>(string.Empty, "ProductType");
                var networkList = tLookUpManager.GetAll<NetworkDTO>(string.Empty, "Network");

                if (!String.IsNullOrWhiteSpace(statementGroupingDTO.ApprovalStatus) && statementGroupingDTO.ApprovalStatus.Equals("P"))
                {
                    var reasonCodes = tLookUpManager.GetAll<ApprovalContextDTO>("STMNTGRP", "ReasonContext");
                    ViewBag.ReasonCodes = reasonCodes;
                }

                ViewBag.Privileges = tCommonManager.GetUserPrivilegesForEntity("MT_STATEMENT_GROUPING");

                ViewData["FormStatus"] = statementGroupingDTO.ApprovalStatus;
                ViewData["FormCall"] = "ApprovalQueue";
                ViewBag.TransactionList = transactionList;
                ViewBag.ProductList = productList;
                ViewBag.NetworkList = networkList;

                return View("StatementGroupingAdd", statementGroupingDTO);
            }
            else
                return RedirectToAction("ApprovalQueueList", "Approval");
        }
        #endregion

        #endregion

        #region BillingParameter

        #region BillingParameterAdd

        [AuthorizeUserAttribute(Entity = "MT_BILLING_PARAMETER", AccessLevel = new UserActivities[] { UserActivities.Add, UserActivities.Update, UserActivities.View })]
        public ActionResult BillingParameterAdd(string Id)
        {
            try
            {
                BillingParameterDTO billingParameterDTO;

                var productList = tLookUpManager.GetAll<ProductDTO>(string.Empty, "Product");

                if (Id == null)
                {
                    billingParameterDTO = new BillingParameterDTO();
                }
                else
                {
                    billingParameterDTO = tMasterManager.FindByCardTypeCode(Id);
                }
                if (!String.IsNullOrWhiteSpace(billingParameterDTO.ApprovalStatus) && billingParameterDTO.ApprovalStatus.Equals("P"))
                {
                    var reasonCodes = tLookUpManager.GetAll<ApprovalContextDTO>("BLNGPARAM", "ReasonContext");
                    ViewBag.ReasonCodes = reasonCodes;

                }
                ViewBag.Privileges = tCommonManager.GetUserPrivilegesForEntity("MT_BILLING_PARAMETER");
                ViewData["FormStatus"] = billingParameterDTO.ApprovalStatus;
                ViewBag.ProductList = productList;

                return View(billingParameterDTO);
            }
            catch (Exception ex)
            {
                LoggerFactory.CreateLog().LogError("Error" + ex.Message, 100, 2);
                ViewBag.ErrorMessage = Resources.MsgErrServerException;
                ViewBag.ErrorType = SessionErrorType.ServerException;
                return View("Error");
            }
        }
        #endregion
        #region BillingParameterSave
        [HttpPost]
        [AuthorizeUserAttribute(Entity = "MT_BILLING_PARAMETER", AccessLevel = new UserActivities[] { UserActivities.Add, UserActivities.Update })]
        public ActionResult BillingParameterSave(string Id, string Command, BillingParameterDTO ABillingParameterDTO)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    if (Command.Equals(Resources.Submit))
                        ABillingParameterDTO.ApprovalStatus = "P";
                    else if (Command.Equals(Resources.Save))
                        ABillingParameterDTO.ApprovalStatus = "D";

                    if (Command.Equals(Resources.Submit) || Command.Equals(Resources.Save))
                    {
                        if (string.IsNullOrWhiteSpace(tMasterManager.FindByCardTypeCode(ABillingParameterDTO.ProductCode).ProductCode))
                        {
                            ABillingParameterDTO.CreatedBy = tCommonManager.GetLoggedUserName();
                            ABillingParameterDTO.LastUpdatedBy = tCommonManager.GetLoggedUserName();
                            ABillingParameterDTO.OrganizationCode = Utils.OrganizationCode;

                            tMasterManager.SaveBillingParameterDetails(ABillingParameterDTO);
                            TempData["Message"] = String.Format(Resources.MsgInfoSaveSuccess, Resources.MT_BILLING_PARAMETER); 
                        }
                        else
                        {
                            if (!string.IsNullOrWhiteSpace(Id))
                            {
                                ABillingParameterDTO.LastUpdatedBy = tCommonManager.GetLoggedUserName();
                                tMasterManager.UpdateBillingParameterDetails(ABillingParameterDTO.ProductCode, ABillingParameterDTO);
                                TempData["Message"] = String.Format(Resources.MsgInfoSaveSuccess, Resources.MT_BILLING_PARAMETER); 
                            }
                            else
                            {
                                TempData["Message"] = String.Format(Resources.MsgErrAlreadyExists, Resources.MT_BILLING_PARAMETER); 
                            }
                        }
                    }
                    else if (Command.Equals(Resources.Delete))
                    {
                        try
                        {
                            ABillingParameterDTO.LastUpdatedBy = tCommonManager.GetLoggedUserName();
                            ABillingParameterDTO.ApprovalStatus = Utils.GetApprovalStatusCode(Utils.ApprovalStatus.Deleted);
                            tMasterManager.UpdateBillingParameterDetails(ABillingParameterDTO.ProductCode, ABillingParameterDTO);

                            TempData["Message"] = String.Format(Resources.MsgInfoDeleteSuccess, Resources.MT_BILLING_PARAMETER); 
                        }
                        catch (Exception ex)
                        {
                            LoggerFactory.CreateLog().LogError("Error" + ex.Message, 100, 2);
                            TempData["Message"] = String.Format(Resources.MsgErrDeleteConcurrencyError, Resources.MT_BILLING_PARAMETER); 
                            return RedirectToAction("BillingParameterAdd", "master", new { Id = Id });
                        }
                    }
                }
            }

            catch (Exception ex)
            {
                LoggerFactory.CreateLog().LogError("Error" + ex.Message, 100, 2);
                ViewBag.ErrorMessage = Resources.MsgErrServerException;
                ViewBag.ErrorType = SessionErrorType.ServerException;
                return View("Error");
            }

            return ((Command.Equals(Resources.Submit) || Command.Equals(Resources.Delete)) ? RedirectToAction("BillingParameterAdd", "Master") : RedirectToAction("BillingParameterAdd", "Master", new { Id = Id }));

        }
        #endregion
        #region BillingParameterDelete
        [HttpPost]
        [AuthorizeUserAttribute(Entity = "MT_BILLING_PARAMETER", AccessLevel = new UserActivities[] { UserActivities.Delete })]
        public JsonResult BillingParameterDelete(string Id)
        {
            BillingParameterDTO ABillingParameterDTO = new BillingParameterDTO();
            try
            {
                ABillingParameterDTO = tMasterManager.FindByCardTypeCode(Id);
                ABillingParameterDTO.LastUpdatedBy = tCommonManager.GetLoggedUserName();
                ABillingParameterDTO.ApprovalStatus = Utils.GetApprovalStatusCode(Utils.ApprovalStatus.Deleted);
                tMasterManager.UpdateBillingParameterDetails(ABillingParameterDTO.ProductCode, ABillingParameterDTO);

                var dResult = new { result = "success", action = "BillingParameterList", controller = "Master" };
                TempData["Message"] = String.Format(Resources.MsgInfoDeleteSuccess, Resources.MT_BILLING_PARAMETER); 
                return Json(dResult, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                var dResultFail = new { result = "fail", action = "BillingParameterList", controller = "Master" };
                TempData["Message"] = String.Format(Resources.MsgErrDeleteConcurrencyError, Resources.MT_BILLING_PARAMETER); 
                return Json(dResultFail, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion
        #region BillingParameterList
        [AuthorizeUserAttribute(Entity = "MT_BILLING_PARAMETER", AccessLevel = new UserActivities[] { UserActivities.View })]
        public ActionResult BillingParameterList(BillingParameterSearch AVMBillingParameterSearch, string ClearButton, int? page)
        {
            try
            {
                int iPageNumber = Convert.ToInt32((page == null) ? 0 : page);
                var productList = tLookUpManager.GetAll<ProductDTO>(string.Empty, "Product");

                if (ClearButton == Resources.Clear)
                {
                    AVMBillingParameterSearch.ProductCode = null;
                    ModelState.Clear();
                }

                if ((!string.IsNullOrEmpty(Utils.OrganizationCode)) || (!(string.IsNullOrEmpty(AVMBillingParameterSearch.SearchButton)) &&
                     !((string.IsNullOrWhiteSpace(AVMBillingParameterSearch.ProductCode)))))
                {
                    var entities = tMasterManager.FindAll(iPageNumber, new BillingParameterDTO()
                    {
                        ProductCode = AVMBillingParameterSearch.ProductCode

                    });
                    AVMBillingParameterSearch.SearchResults = entities.AsQueryable();
                }
                else
                {
                    var billResult = tMasterManager.FindBillingParameter(1, Utils.GRD_ROWS_PER_PAGE).AsQueryable();

                    AVMBillingParameterSearch.SearchResults = billResult.AsQueryable();
                }
                ViewBag.ProductList = productList;

                AVMBillingParameterSearch.SearchResults = AVMBillingParameterSearch.SearchResults.Where(x => x.ApprovalStatus != "DEL" && x.ApprovalStatus != "DIS");
                ViewBag.Privileges = tCommonManager.GetUserPrivilegesForEntity("MT_BILLING_PARAMETER");
                return View(AVMBillingParameterSearch);
            }
            catch (Exception ex)
            {
                LoggerFactory.CreateLog().LogError("Error" + ex.Message, 100, 2);
                ViewBag.ErrorMessage = Resources.MsgErrServerException;
                ViewBag.ErrorType = SessionErrorType.ServerException;
                return View("Error");
            }
        }
        #endregion
        #region BillingParameterView
        [AuthorizeUserAttribute(Entity = "MT_BILLING_PARAMETER", AccessLevel = new UserActivities[] { UserActivities.View })]
        public ActionResult BillingParameterView(string Id)
        {
            try
            {
                BillingParameterDTO billingParameterDTO;
                var productList = tLookUpManager.GetAll<ProductDTO>(string.Empty, "Product");
                ViewBag.TableName = "BLNGPARAM";

                ViewBag.Privileges = tCommonManager.GetUserPrivilegesForEntity("MT_BILLING_PARAMETER");

                ViewBag.ProductList = productList;

                if (String.IsNullOrWhiteSpace(Id))
                {
                    billingParameterDTO = new BillingParameterDTO();
                    ViewData["FormStatus"] = "";
                    return View("BillingParameterAdd", billingParameterDTO);
                }
                else
                {
                    billingParameterDTO = tMasterManager.FindByCardTypeCode(Id);
                    ViewData["FormStatus"] = billingParameterDTO.ApprovalStatus;
                    ViewBag.ReasonCodes = null;
                }

                return View("BillingParameterAdd", billingParameterDTO);
            }
            catch (Exception ex)
            {
                LoggerFactory.CreateLog().LogError("Error" + ex.Message, 100, 2);
                ViewBag.ErrorMessage = Resources.MsgErrServerException;
                ViewBag.ErrorType = SessionErrorType.ServerException;
                return View("Error");
            }
        }
        #endregion
        #region BillingParameterAuthorize

        [AuthorizeUserAttribute(Entity = "MT_BILLING_PARAMETER", AccessLevel = new UserActivities[] { UserActivities.Authorize })]
        public ActionResult BillingParameterAuthorize(string Id, string AMode, string QId)
        {
            ViewBag.RequestQueueId = QId;
            ViewBag.TableName = "BLNGPARAM";
            var productList = tLookUpManager.GetAll<ProductDTO>(string.Empty, "Product");
            if (!string.IsNullOrWhiteSpace(Id))
            {
                BillingParameterDTO billingParameterDTO = tMasterManager.FindByCardTypeCode(Id);
                if (!String.IsNullOrWhiteSpace(billingParameterDTO.ApprovalStatus) && billingParameterDTO.ApprovalStatus.Equals("P"))
                {
                    var reasonCodes = tLookUpManager.GetAll<ApprovalContextDTO>("BLNGPARAM", "ReasonContext");
                    ViewBag.ReasonCodes = reasonCodes;
                }

                ViewBag.Privileges = tCommonManager.GetUserPrivilegesForEntity("MT_BILLING_PARAMETER");
                ViewData["FormCall"] = "ApprovalQueue";
                ViewData["FormStatus"] = billingParameterDTO.ApprovalStatus;
                ViewBag.ProductList = productList;

                return View("BillingParameterAdd", billingParameterDTO);
            }
            else
                return RedirectToAction("ApprovalQueueList", "Approval");
        }
        #endregion

        #endregion

        #region PaymentAllocationType

        #region PaymentAllocationTypeView
        [AuthorizeUserAttribute(Entity = "MT_PAYMENT_ALLOCATION_TYPE", AccessLevel = new UserActivities[] { UserActivities.View })]
        public ActionResult PaymentAllocationTypeView(string Id)
        {
            try
            {
                PaymentAllocationTypeDTO PaymentAllocationTypeDTO;
                ViewBag.TableName = "PALOCTYP";

                var paymentAllocationType = Utils.GetPaymentAllocationType();
                var paymentAllocationSequence = tLookUpManager.GetAll<CreditSegmentDTO>("", "CreditSegment"); ;

                List<string> strAllPrivileges = tCommonManager.GetUserPrivilegesForEntity("MT_PAYMENT_ALLOCATION_TYPE");
                if (strAllPrivileges.Contains("Add"))
                {
                    strAllPrivileges.Remove("Add");
                }
                ViewBag.Privileges = strAllPrivileges;

                ViewBag.PaymentAllocationTypeList = paymentAllocationType;
                ViewBag.PaymentAllocationSequenceList = paymentAllocationSequence;

                if (Convert.ToInt32(Id) == 0)
                {
                    PaymentAllocationTypeDTO = new PaymentAllocationTypeDTO();
                    ViewData["FormStatus"] = PaymentAllocationTypeDTO.ApprovalStatus;
                    return View("PaymentAllocationTypeAdd", PaymentAllocationTypeDTO);
                }
                else
                {
                    PaymentAllocationTypeDTO = tMasterManager.FindByPaymentAllocationTypeId(Convert.ToInt32(Id));
                    ViewBag.ReasonCodes = null;
                }

                ViewData["FormStatus"] = PaymentAllocationTypeDTO.ApprovalStatus;


                return View("PaymentAllocationTypeAdd", PaymentAllocationTypeDTO);
            }
            catch (Exception ex)
            {
                LoggerFactory.CreateLog().LogError("Error" + ex.Message, 100, 2);
                ViewBag.ErrorMessage = Resources.MsgErrServerException;
                ViewBag.ErrorType = SessionErrorType.ServerException;
                return View("Error");
            }
        }
        #endregion
        #region PaymentAllocationTypeAdd
        [AuthorizeUserAttribute(Entity = "MT_PAYMENT_ALLOCATION_TYPE", AccessLevel = new UserActivities[] { UserActivities.Add, UserActivities.Update, UserActivities.View })]
        public ActionResult PaymentAllocationTypeAdd(string Id)
        {
            try
            {
                PaymentAllocationTypeDTO PaymentAllocationTypeDTO;

                var paymentAllocationType = Utils.GetPaymentAllocationType();
                var paymentAllocationSequence = tLookUpManager.GetAll<CreditSegmentDTO>("", "CreditSegment");

                if (Convert.ToInt32(Id) == 0)
                {
                    PaymentAllocationTypeDTO = new PaymentAllocationTypeDTO();
                    PaymentAllocationTypeDTO.AssignedList = new List<String>();
                }
                else
                {
                    PaymentAllocationTypeDTO = tMasterManager.FindByPaymentAllocationTypeId(Convert.ToInt32(Id));
                }


                if (!String.IsNullOrWhiteSpace(PaymentAllocationTypeDTO.ApprovalStatus) && PaymentAllocationTypeDTO.ApprovalStatus.Equals("P"))
                {
                    var reasonCodes = tLookUpManager.GetAll<ApprovalContextDTO>("PALOCTYP", "ReasonContext");
                    ViewBag.ReasonCodes = reasonCodes;
                }
                ViewData["FormStatus"] = PaymentAllocationTypeDTO.ApprovalStatus;
                List<string> strAllPrivileges = tCommonManager.GetUserPrivilegesForEntity("MT_PAYMENT_ALLOCATION_TYPE");
                if (strAllPrivileges.Contains("Add"))
                {
                    strAllPrivileges.Remove("Add");
                }
                ViewBag.Privileges = strAllPrivileges;

                ViewBag.PaymentAllocationTypeList = paymentAllocationType;
                ViewBag.PaymentAllocationSequenceList = paymentAllocationSequence;

                return View(PaymentAllocationTypeDTO);
            }
            catch (Exception ex)
            {
                LoggerFactory.CreateLog().LogError("Error" + ex.Message, 100, 2);
                ViewBag.ErrorMessage = Resources.MsgErrServerException;
                ViewBag.ErrorType = SessionErrorType.ServerException;
                return View("Error");
            }
        }
        #endregion
        #region PaymentAllocationTypeSave
        [HttpPost]
        [AuthorizeUserAttribute(Entity = "MT_PAYMENT_ALLOCATION_TYPE", AccessLevel = new UserActivities[] { UserActivities.Add, UserActivities.Update, UserActivities.View })]
        public ActionResult PaymentAllocationTypeSave(string Id, string Command, PaymentAllocationTypeDTO APaymentAllocationTypeDTO)
        {
            try
            {

                string strPaymentAllocSeq = "";

                var items = APaymentAllocationTypeDTO.AssignedList;

                foreach (var item in items)
                {
                    strPaymentAllocSeq += "," + item;
                }

                if (strPaymentAllocSeq.Length > 0)
                {
                    strPaymentAllocSeq = strPaymentAllocSeq.Substring(1, strPaymentAllocSeq.Length - 1);
                }

                APaymentAllocationTypeDTO.PaymentAllocationSequence = strPaymentAllocSeq;

                var errors = ModelState.Values.SelectMany(x => x.Errors);
                if (ModelState.IsValid)
                {

                    if (Command.Equals(Resources.Submit))
                        APaymentAllocationTypeDTO.ApprovalStatus = "P";
                    else if (Command.Equals(Resources.Save))
                        APaymentAllocationTypeDTO.ApprovalStatus = "D";

                    APaymentAllocationTypeDTO.OrganizationCode = Utils.OrganizationCode;
                    if (Command.Equals(Resources.Submit) || Command.Equals(Resources.Save))
                    {
                        if (string.IsNullOrWhiteSpace(tMasterManager.FindByPaymentAllocationTypeId(Convert.ToInt32(Id)).PaymentAllocationSequence))
                        {
                            APaymentAllocationTypeDTO.CreatedBy = tCommonManager.GetLoggedUserName();
                            APaymentAllocationTypeDTO.LastUpdatedBy = tCommonManager.GetLoggedUserName();
                            tMasterManager.SavePaymentAllocationTypeDetails(APaymentAllocationTypeDTO);
                            TempData["Message"] = String.Format(Resources.MsgInfoSaveSuccess, Resources.PaymentAllocationType); 
                        }
                        else
                        {
                            if (!string.IsNullOrWhiteSpace(Convert.ToString(Id)))
                            {
                                APaymentAllocationTypeDTO.LastUpdatedBy = tCommonManager.GetLoggedUserName();
                                tMasterManager.UpdatePaymentAllocationTypeDetails(Convert.ToInt32(Id), APaymentAllocationTypeDTO);
                                TempData["Message"] = String.Format(Resources.MsgInfoSaveSuccess, Resources.PaymentAllocationType); 
                            }
                            else
                            {
                                TempData["Message"] = String.Format(Resources.MsgErrAlreadyExists, Resources.PaymentAllocationType); 
                            }
                        }
                    }
                    else if (Command.Equals(Resources.Delete))
                    {
                        try
                        {
                            APaymentAllocationTypeDTO.LastUpdatedBy = tCommonManager.GetLoggedUserName();
                            APaymentAllocationTypeDTO.ApprovalStatus = Utils.GetApprovalStatusCode(Utils.ApprovalStatus.Deleted);
                            tMasterManager.UpdatePaymentAllocationTypeDetails(Convert.ToInt32(Id), APaymentAllocationTypeDTO);

                            TempData["Message"] = String.Format(Resources.MsgInfoDeleteSuccess, Resources.PaymentAllocationType); 
                        }
                        catch (Exception ex)
                        {
                            LoggerFactory.CreateLog().LogError("Error" + ex.Message, 100, 2);
                            TempData["Message"] = String.Format(Resources.MsgErrDeleteConcurrencyError, Resources.PaymentAllocationType); 
                            return RedirectToAction("PaymentAllocationTypeAdd", "master", new { Id = Id });
                        }
                    }
                }
            }

            catch (Exception ex)
            {
                LoggerFactory.CreateLog().LogError("Error" + ex.Message, 100, 2);
                ViewBag.ErrorMessage = Resources.MsgErrServerException;
                ViewBag.ErrorType = SessionErrorType.ServerException;
                return View("Error");
            }

            return ((Command.Equals(Resources.Submit) || Command.Equals(Resources.Delete)) ? RedirectToAction("PaymentAllocationTypeAdd", "Master") : RedirectToAction("PaymentAllocationTypeAdd", "Master", new { Id = Id }));

        }
        #endregion
        #region PaymentAllocationTypeDelete
        [AuthorizeUserAttribute(Entity = "MT_PAYMENT_ALLOCATION_TYPE", AccessLevel = new UserActivities[] { UserActivities.Delete })]
        public JsonResult PaymentAllocationTypeDelete(string Id)
        {
            PaymentAllocationTypeDTO APaymentAllocationTypeDTO = new PaymentAllocationTypeDTO();
            try
            {
                APaymentAllocationTypeDTO = tMasterManager.FindByPaymentAllocationTypeId(Convert.ToInt32(Id));
                APaymentAllocationTypeDTO.LastUpdatedBy = tCommonManager.GetLoggedUserName();
                APaymentAllocationTypeDTO.ApprovalStatus = Utils.GetApprovalStatusCode(Utils.ApprovalStatus.Deleted);
                APaymentAllocationTypeDTO.OrganizationCode = Utils.OrganizationCode;
                tMasterManager.UpdatePaymentAllocationTypeDetails(Convert.ToInt32(Id), APaymentAllocationTypeDTO);

                var dResult = new { result = "success", action = "PaymentAllocationTypeList", controller = "Master" };
                TempData["Message"] = String.Format(Resources.MsgInfoDeleteSuccess, Resources.PaymentAllocationType); 
                return Json(dResult, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                var dResultFail = new { result = "fail", action = "PaymentAllocationTypeList", controller = "Master" };
                TempData["Message"] = String.Format(Resources.MsgErrDeleteConcurrencyError, Resources.PaymentAllocationType); 
                return Json(dResultFail, JsonRequestBehavior.AllowGet);
            }

        }
        #endregion
        #region PaymentAllocationTypeList
        [AuthorizeUserAttribute(Entity = "MT_PAYMENT_ALLOCATION_TYPE", AccessLevel = new UserActivities[] { UserActivities.View })]
        public ActionResult PaymentAllocationTypeList(PaymentAllocationTypeSearch AVMPaymentAllocationTypeSearch, string ClearButton, int? page)
        {
            try
            {
                int iPageNumber = Convert.ToInt32((page == null) ? 0 : page);

                var paymentAllocationType = Utils.GetPaymentAllocationType();

                if (ClearButton == Resources.Clear)
                {
                    AVMPaymentAllocationTypeSearch.CardType = null;
                    AVMPaymentAllocationTypeSearch.SeqNr = null;
                    ModelState.Clear();
                }

                if ((Utils.OrganizationCode != string.Empty) || (!(string.IsNullOrEmpty(AVMPaymentAllocationTypeSearch.SearchButton)) &&
                     !((string.IsNullOrWhiteSpace(AVMPaymentAllocationTypeSearch.CardType) &&
                       (string.IsNullOrWhiteSpace(AVMPaymentAllocationTypeSearch.SeqNr))))))
                {
                    var entities = tMasterManager.FindAll(iPageNumber, new PaymentAllocationTypeDTO()
                    {
                        CardType = AVMPaymentAllocationTypeSearch.CardType,
                        SeqNr = AVMPaymentAllocationTypeSearch.SeqNr
                    });
                    AVMPaymentAllocationTypeSearch.SearchResults = entities.AsQueryable();
                }
                else
                {
                    var currResult = tMasterManager.FindPaymentAllocationType(1, Utils.GRD_ROWS_PER_PAGE).AsQueryable();
                    AVMPaymentAllocationTypeSearch.SearchResults = currResult.AsQueryable();
                }
                ViewBag.PaymentAllocationTypeList = paymentAllocationType;
                List<string> strAllPrivileges = tCommonManager.GetUserPrivilegesForEntity("MT_PAYMENT_ALLOCATION_TYPE");
                if (strAllPrivileges.Contains("Add"))
                {
                    strAllPrivileges.Remove("Add");
                }
                ViewBag.Privileges = strAllPrivileges;
                return View(AVMPaymentAllocationTypeSearch);
            }
            catch (Exception ex)
            {
                LoggerFactory.CreateLog().LogError("Error" + ex.Message, 100, 2);
                ViewBag.ErrorMessage = Resources.MsgErrServerException;
                ViewBag.ErrorType = SessionErrorType.ServerException;
                return View("Error");
            }
        }
        #endregion
        #region PaymentAllocationTypeAuthorize
        [AuthorizeUserAttribute(Entity = "MT_PAYMENT_ALLOCATION_TYPE", AccessLevel = new UserActivities[] { UserActivities.Authorize })]
        public ActionResult PaymentAllocationTypeAuthorize(string Id, string AMode, string QId)
        {
            ViewBag.RequestQueueId = QId;
            ViewBag.TableName = "PALOCTYP";
            if (!String.IsNullOrWhiteSpace(Id))
            {
                PaymentAllocationTypeDTO PaymentAllocationType = tMasterManager.FindByPaymentAllocationTypeId(Convert.ToInt32(Id));

                var paymentAllocationType = Utils.GetPaymentAllocationType();
                var paymentAllocationSequence = tLookUpManager.GetAll<CreditSegmentDTO>("", "CreditSegment");

                if (!String.IsNullOrWhiteSpace(PaymentAllocationType.ApprovalStatus) && PaymentAllocationType.ApprovalStatus.Equals("P"))
                {
                    var reasonCodes = tLookUpManager.GetAll<ApprovalContextDTO>("PALOCTYP", "ReasonContext");
                    ViewBag.ReasonCodes = reasonCodes;
                }

                ViewBag.Privileges = tCommonManager.GetUserPrivilegesForEntity("MT_PAYMENT_ALLOCATION_TYPE");

                ViewData["FormStatus"] = PaymentAllocationType.ApprovalStatus;
                ViewData["FormCall"] = "ApprovalQueue";
                ViewBag.PaymentAllocationTypeList = paymentAllocationType;
                ViewBag.PaymentAllocationSequenceList = paymentAllocationSequence;

                return View("PaymentAllocationTypeAdd", PaymentAllocationType);
            }
            else
                return RedirectToAction("ApprovalQueueList", "Approval");
        }
        #endregion

        #endregion

        #region DiscountPercentage

        #region DiscountPercentageView
        [AuthorizeUserAttribute(Entity = "MT_DISCOUNT_PERCENTAGE", AccessLevel = new UserActivities[] { UserActivities.View })]
        public ActionResult DiscountPercentageView(string ADiscountPercentageId)
        {
            try
            {
                DiscountPercentageDTO DiscountPercentageDTO;
                ViewBag.TableName = "DSCNTPERC";

                var productCodes = tLookUpManager.GetAll<ProductDTO>("", "Product");
                var customerSegmentCodes = tLookUpManager.GetAll<CustomerSegmentDTO>("", "CustomerSegment");
                var customerSubSegmentCodes = tLookUpManager.GetAll<CustomerSubSegmentDTO>("", "CustomerSubSegment");

                ViewBag.Privileges = tCommonManager.GetUserPrivilegesForEntity("MT_DISCOUNT_PERCENTAGE");

                ViewBag.ProductList = productCodes;
                ViewBag.CustomerSegmentList = customerSegmentCodes;
                ViewBag.CustomerSubSegmentList = customerSubSegmentCodes;

                if (String.IsNullOrWhiteSpace(ADiscountPercentageId))
                //return View("CollectionChargeList");
                {
                    DiscountPercentageDTO = new DiscountPercentageDTO();
                    DiscountPercentageDTO.ValidFrom = Utils.BusinessDate;
                    DiscountPercentageDTO.ValidTo = Utils.BusinessDate;
                    ViewBag.DiscountPercentages = new JavaScriptSerializer().Serialize(DiscountPercentageDTO);
                    ViewData["FormStatus"] = DiscountPercentageDTO.ApprovalStatus;
                    return View("DiscountPercentageAdd", DiscountPercentageDTO);
                }
                else
                {
                    DiscountPercentageDTO = tMasterManager.FindByDiscountPercentageId(Convert.ToInt32(ADiscountPercentageId));
                    ViewBag.DiscountPercentages = new JavaScriptSerializer().Serialize(DiscountPercentageDTO);
                    ViewBag.ReasonCodes = null;
                }

                ViewData["FormStatus"] = DiscountPercentageDTO.ApprovalStatus;

                return View("DiscountPercentageAdd", DiscountPercentageDTO);
            }
            catch (Exception ex)
            {
                LoggerFactory.CreateLog().LogError("Error" + ex.Message, 100, 2);
                ViewBag.ErrorMessage = Resources.MsgErrServerException;
                ViewBag.ErrorType = SessionErrorType.ServerException;
                return View("Error");
            }
        }
        #endregion
        #region DiscountPercentageList
        [AuthorizeUserAttribute(Entity = "MT_DISCOUNT_PERCENTAGE", AccessLevel = new UserActivities[] { UserActivities.View })]
        public ActionResult DiscountPercentageList(DiscountPercentageSearch AVMDiscountPercentageSearch, string ClearButton, int? page)
        {
            try
            {

                int iPageNumber = Convert.ToInt32((page == null) ? 0 : page);

                var productCodes = tLookUpManager.GetAll<ProductDTO>("", "Product");
                var customerSegmentCodes = tLookUpManager.GetAll<CustomerSegmentDTO>("", "CustomerSegment");
                var customerSubSegmentCodes = tLookUpManager.GetAll<CustomerSubSegmentDTO>("", "CustomerSubSegment");

                if (ClearButton == Resources.Clear)
                {
                    AVMDiscountPercentageSearch.ProductCode = null;
                    AVMDiscountPercentageSearch.CustomerSegmentCode = null;
                    AVMDiscountPercentageSearch.CustomerSubSegmentCode = null;
                    ModelState.Clear();
                }

                if ((!string.IsNullOrEmpty(Utils.OrganizationCode)) || (!(string.IsNullOrEmpty(AVMDiscountPercentageSearch.SearchButton)) &&
                     !((string.IsNullOrWhiteSpace(AVMDiscountPercentageSearch.ProductCode) &&
                            string.IsNullOrWhiteSpace(AVMDiscountPercentageSearch.CustomerSegmentCode) &&
                            string.IsNullOrWhiteSpace(AVMDiscountPercentageSearch.CustomerSubSegmentCode)
                      ))))
                {
                    var entities = tMasterManager.FindAll
                        (
                            iPageNumber,
                                new DiscountPercentageDTO()
                                {
                                    ProductCode = AVMDiscountPercentageSearch.ProductCode,
                                    SegmentCode = AVMDiscountPercentageSearch.CustomerSegmentCode,
                                    SubSegmentCode = AVMDiscountPercentageSearch.CustomerSubSegmentCode
                                }
                        );
                    AVMDiscountPercentageSearch.SearchResults = entities.AsQueryable();
                }
                else
                {
                    var currResult = tMasterManager.FindDiscountPercentage(1, Utils.GRD_ROWS_PER_PAGE).AsQueryable();

                    AVMDiscountPercentageSearch.SearchResults = currResult.AsQueryable();
                }

                ViewBag.ProductList = productCodes;
                ViewBag.CustomerSegmentList = customerSegmentCodes;
                ViewBag.CustomerSubSegmentList = customerSubSegmentCodes;

                AVMDiscountPercentageSearch.SearchResults = AVMDiscountPercentageSearch.SearchResults.Where(x => x.ApprovalStatus != "DEL" && x.ApprovalStatus != "DIS");
                ViewBag.Privileges = tCommonManager.GetUserPrivilegesForEntity("MT_DISCOUNT_PERCENTAGE");
                return View(AVMDiscountPercentageSearch);
            }
            catch (Exception ex)
            {
                LoggerFactory.CreateLog().LogError("Error" + ex.Message, 100, 2);
                ViewBag.ErrorMessage = Resources.MsgErrServerException;
                ViewBag.ErrorType = SessionErrorType.ServerException;
                return View("Error");
            }
        }
        #endregion
        #region DiscountPercentageAdd
        [AuthorizeUserAttribute(Entity = "MT_DISCOUNT_PERCENTAGE", AccessLevel = new UserActivities[] { UserActivities.Add, UserActivities.Update, UserActivities.View })]
        public ActionResult DiscountPercentageAdd(string ADiscountPercentageId)
        {
            try
            {
                DiscountPercentageDTO DiscountPercentageDTO;

                var productCodes = tLookUpManager.GetAll<ProductDTO>("", "Product");
                var customerSegmentCodes = tLookUpManager.GetAll<CustomerSegmentDTO>("", "CustomerSegment");
                var customerSubSegmentCodes = tLookUpManager.GetAll<CustomerSubSegmentDTO>("", "CustomerSubSegment");

                if (Convert.ToInt32(ADiscountPercentageId) == 0)
                {
                    DiscountPercentageDTO = new DiscountPercentageDTO();
                }
                else
                    DiscountPercentageDTO = tMasterManager.FindByDiscountPercentageId(Convert.ToInt32(ADiscountPercentageId));

                if (!String.IsNullOrWhiteSpace(DiscountPercentageDTO.ApprovalStatus) && DiscountPercentageDTO.ApprovalStatus.Equals("P"))
                {
                    var reasonCodes = tLookUpManager.GetAll<ApprovalContextDTO>("DSCNTPERC", "ReasonContext");
                    ViewBag.ReasonCodes = reasonCodes;
                }

                ViewBag.Privileges = tCommonManager.GetUserPrivilegesForEntity("MT_DISCOUNT_PERCENTAGE");
                ViewBag.DiscountPercentages = new JavaScriptSerializer().Serialize(DiscountPercentageDTO);
                ViewData["FormStatus"] = DiscountPercentageDTO.ApprovalStatus;

                ViewBag.ProductList = productCodes;
                ViewBag.CustomerSegmentList = customerSegmentCodes;
                ViewBag.CustomerSubSegmentList = customerSubSegmentCodes;

                return View(DiscountPercentageDTO);
            }
            catch (Exception ex)
            {
                LoggerFactory.CreateLog().LogError("Error" + ex.Message, 100, 2);
                ViewBag.ErrorMessage = Resources.MsgErrServerException;
                ViewBag.ErrorType = SessionErrorType.ServerException;
                return View("Error");
            }
        }
        #endregion
        #region DiscountPercentageSave
        [HttpPost]
        [AuthorizeUserAttribute(Entity = "MT_DISCOUNT_PERCENTAGE", AccessLevel = new UserActivities[] { UserActivities.Add, UserActivities.Update })]
        public ActionResult DiscountPercentageSave(DiscountPercentageDTO ADiscountPercentageDTO, string Command)
        {

            string redirectUrl = string.Empty, messageToShow = string.Empty;

            if (Command.Equals(Resources.Submit))
                ADiscountPercentageDTO.ApprovalStatus = "P";
            else if (Command.Equals(Resources.Save))
                ADiscountPercentageDTO.ApprovalStatus = "D";

            if (Command.Equals(Resources.Submit) || Command.Equals(Resources.Save))
            {
                if (string.IsNullOrWhiteSpace(tMasterManager.FindByDiscountPercentageId(Convert.ToInt32(ADiscountPercentageDTO.DiscountPercentageId)).ProductCode))
                {
                    ADiscountPercentageDTO.CreatedBy = tCommonManager.GetLoggedUserName();
                    ADiscountPercentageDTO.LastUpdatedBy = tCommonManager.GetLoggedUserName();
                    ADiscountPercentageDTO.OrganizationCode = Utils.OrganizationCode;

                    tMasterManager.SaveDiscountPercentageDetails(ADiscountPercentageDTO);
                    redirectUrl = Url.Action("DiscountPercentageAdd", "Master", new { ADiscountPercentageId = ADiscountPercentageDTO.DiscountPercentageId });
                    messageToShow = String.Format(Resources.MsgInfoSaveSuccess, Resources.DiscountPercentage); 
                }
                else
                {
                    ADiscountPercentageDTO.LastUpdatedBy = tCommonManager.GetLoggedUserName();
                    tMasterManager.UpdateDiscountPercentageDetails(ADiscountPercentageDTO.DiscountPercentageId, ADiscountPercentageDTO);
                    redirectUrl = Url.Action("DiscountPercentageAdd", "Master", new { ADiscountPercentageId = ADiscountPercentageDTO.DiscountPercentageId });
                    messageToShow = String.Format(Resources.MsgInfoSaveSuccess, Resources.DiscountPercentage); 
                }
            }

            return Json(new { RedirectUrl = redirectUrl, Message = messageToShow }, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region DiscountPercentageDelete
        [HttpPost]
        [AuthorizeUserAttribute(Entity = "MT_DISCOUNT_PERCENTAGE", AccessLevel = new UserActivities[] { UserActivities.Delete })]
        public JsonResult DiscountPercentageDelete(string ADiscountPercentageId, bool ASearchGridClick = false)
        {
            DiscountPercentageDTO ADiscountPercentageDTO = new DiscountPercentageDTO();
            string redirectUrl = string.Empty, messageToShow = string.Empty;
            try
            {

                ADiscountPercentageDTO = tMasterManager.FindByDiscountPercentageId(Convert.ToInt32(ADiscountPercentageId));
                ADiscountPercentageDTO.LastUpdatedBy = tCommonManager.GetLoggedUserName();
                ADiscountPercentageDTO.ApprovalStatus = Utils.GetApprovalStatusCode(Utils.ApprovalStatus.Deleted);
                tMasterManager.UpdateDiscountPercentageDetails(ADiscountPercentageDTO.DiscountPercentageId, ADiscountPercentageDTO);

                redirectUrl = Url.Action("DiscountPercentageAdd", "Master");
                messageToShow = String.Format(Resources.MsgInfoDeleteSuccess, Resources.DiscountPercentage); 
            }
            catch
            {
                redirectUrl = Url.Action("DiscountPercentageAdd", "Master");
                messageToShow = String.Format(Resources.MsgErrDeleteConcurrencyError, Resources.DiscountPercentage); 
            }

            if (ASearchGridClick)
            {
                messageToShow = String.Format(Resources.MsgInfoDeleteSuccess, Resources.DiscountPercentage); 
            }
            else
            {
                TempData["Message"] = null;
            }

            return Json(new { RedirectUrl = redirectUrl, Message = messageToShow }, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region DiscountPercentageAuthorize
        [AuthorizeUserAttribute(Entity = "MT_DISCOUNT_PERCENTAGE", AccessLevel = new UserActivities[] { UserActivities.Authorize })]
        public ActionResult DiscountPercentageAuthorize(string ADiscountPercentageId, string AMode, string QId)
        {
            ViewBag.RequestQueueId = QId;
            ViewBag.TableName = "DSCNTPERC";

            var productCodes = tLookUpManager.GetAll<ProductDTO>("", "Product");
            var customerSegmentCodes = tLookUpManager.GetAll<CustomerSegmentDTO>("", "CustomerSegment");
            var customerSubSegmentCodes = tLookUpManager.GetAll<CustomerSubSegmentDTO>("", "CustomerSubSegment");

            if (!String.IsNullOrWhiteSpace(ADiscountPercentageId))
            {
                DiscountPercentageDTO DiscountPercentageDTO = tMasterManager.FindByDiscountPercentageId(Convert.ToInt32(ADiscountPercentageId));

                if (!String.IsNullOrWhiteSpace(DiscountPercentageDTO.ApprovalStatus) && DiscountPercentageDTO.ApprovalStatus.Equals("P"))
                {
                    var reasonCodes = tLookUpManager.GetAll<ApprovalContextDTO>("DSCNTPERC", "ReasonContext");
                    ViewBag.ReasonCodes = reasonCodes;
                }

                ViewBag.Privileges = tCommonManager.GetUserPrivilegesForEntity("MT_DISCOUNT_PERCENTAGE");

                ViewData["FormStatus"] = DiscountPercentageDTO.ApprovalStatus;
                ViewData["FormCall"] = "ApprovalQueue";

                ViewBag.ProductList = productCodes;
                ViewBag.CustomerSegmentList = customerSegmentCodes;
                ViewBag.CustomerSubSegmentList = customerSubSegmentCodes;

                ViewBag.DiscountPercentages = new JavaScriptSerializer().Serialize(DiscountPercentageDTO);
                return View("DiscountPercentageAdd", DiscountPercentageDTO);
            }
            else
                return RedirectToAction("ApprovalQueueList", "Approval");
        }
        #endregion

        #region Validate DiscountPercentage Details
        public JsonResult ValidateDiscountPercentageDetails(DiscountPercentageDTO ADiscountPercentageDTO, string Command)
        {
            bool isOverlapped = false;
            string Message = string.Empty;

            if (ADiscountPercentageDTO != null)
            {
                if (ADiscountPercentageDTO.DiscountPercentageDetailsDTO.Count > 0)
                {

                    int intloop = 0;
                    for (int intLoop = 0; intLoop < ADiscountPercentageDTO.DiscountPercentageDetailsDTO.Count; intLoop++)
                    {
                        for (intloop = intLoop + 1; intloop != intLoop && intloop < ADiscountPercentageDTO.DiscountPercentageDetailsDTO.Count; intloop++)
                        {
                            if (Utils.CheckRangeOverlaps(ADiscountPercentageDTO.DiscountPercentageDetailsDTO[intLoop].PurchaseAmountFrom, ADiscountPercentageDTO.DiscountPercentageDetailsDTO[intLoop].PurchaseAmountTo, ADiscountPercentageDTO.DiscountPercentageDetailsDTO[intloop].PurchaseAmountFrom, ADiscountPercentageDTO.DiscountPercentageDetailsDTO[intloop].PurchaseAmountTo))
                            {
                                isOverlapped = true;
                            }
                        }
                    }
                }
                else
                {
                    Message = String.Format(Resources.MsgErrOneValid, Resources.DiscountPer);
                }
            }
            else
            {
                Message = String.Format(Resources.MsgErrOneValid, Resources.DiscountPer);
            }

            if (isOverlapped)
            {
                Message = String.Format(Resources.MsgErrOverlapValid, Resources.PurchaseAmount);
            }
            if (Message != string.Empty)
            {
                isOverlapped = true;
            }

            return Json(new { Message, isOverlapped }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #endregion

        #region Branch

        #region BranchList
        [AuthorizeUserAttribute(Entity = "MT_BRANCH", AccessLevel = new UserActivities[] { UserActivities.View })]
        public ActionResult BranchList(BranchSearch AVMBranchSearch, string ClearButton, int? page)
        {
            try
            {
                                
                if (ClearButton == Resources.Clear)
                {
                    AVMBranchSearch.BranchCode = null;
                    AVMBranchSearch.BranchName = null;

                    ModelState.Clear();
                }

                int iPageNumber = Convert.ToInt32((page == null) ? 0 : page);

                if ((Utils.OrganizationCode != "") || (!(string.IsNullOrEmpty(AVMBranchSearch.SearchButton)) &&
                   !((string.IsNullOrWhiteSpace(AVMBranchSearch.BranchCode) &&
                   (string.IsNullOrWhiteSpace(AVMBranchSearch.BranchName)) 
                   ))))
                {
                    var entities = tMasterManager.FindAll(iPageNumber, new BranchDTO()
                    {
                        BranchCode = AVMBranchSearch.BranchCode,
                        BranchName = AVMBranchSearch.BranchName
                    });
                    AVMBranchSearch.SearchResults = entities.AsQueryable();
                }
                else
                {
                    var currResult = tMasterManager.FindBranch(1, Utils.GRD_ROWS_PER_PAGE).AsQueryable();
                    AVMBranchSearch.SearchResults = currResult.AsQueryable();
                }
                                
                AVMBranchSearch.SearchResults = AVMBranchSearch.SearchResults.Where(x => x.ApprovalStatus != "DEL" && x.ApprovalStatus != "DIS");
                ViewBag.Privileges = tCommonManager.GetUserPrivilegesForEntity("MT_BRANCH");
                return View(AVMBranchSearch);
            }
            catch (Exception ex)
            {
                LoggerFactory.CreateLog().LogError("Error" + ex.Message, 100, 2);
                ViewBag.ErrorMessage = Resources.MsgErrServerException;
                ViewBag.ErrorType = SessionErrorType.ServerException;
                return View("Error");
            }
        }
        #endregion

        #region BranchAdd
        [AuthorizeUserAttribute(Entity = "MT_BRANCH", AccessLevel = new UserActivities[] { UserActivities.Add, UserActivities.Update, UserActivities.View })]
        public ActionResult BranchAdd(string ABranchCode, string AOrganizationCode)
        {
            try
            {
                BranchDTO branchDTO;

                var countryCode = tLookUpManager.GetAll<CountryDTO>("", "Country");
                var regionCode = tLookUpManager.GetAll<RegionDTO>("", "RegionByCommuna");
                var communaCode = tLookUpManager.GetAll<CommunaDTO>("", "Communa");
                var cityList = tLookUpManager.GetAll<CityDTO>("", "CityByCommuna");

                if (ABranchCode == null)
                {
                    branchDTO = new BranchDTO();
                    branchDTO.OrganizationCode = Utils.OrganizationCode;
                }
                else
                    branchDTO = tMasterManager.FindByBranchCode(Convert.ToString(ABranchCode), AOrganizationCode);


                if (branchDTO.ApprovalStatus == "DEL")
                {
                    branchDTO = new BranchDTO();
                    branchDTO.OrganizationCode = Utils.OrganizationCode;
                }

                if (!String.IsNullOrWhiteSpace(branchDTO.ApprovalStatus) && branchDTO.ApprovalStatus.Equals("P"))
                {
                    var reasonCodes = tLookUpManager.GetAll<ApprovalContextDTO>("BRANCH", "ReasonContext");
                    ViewBag.ReasonCodes = reasonCodes;
                }

                ViewBag.Privileges = tCommonManager.GetUserPrivilegesForEntity("MT_BRANCH");

                ViewData["FormStatus"] = branchDTO.ApprovalStatus;
                ViewBag.Countries = countryCode;
                ViewBag.Regions = regionCode;
                ViewBag.Communas = communaCode;
                ViewBag.City = cityList;

                return View(branchDTO);
            }
            catch (Exception ex)
            {
                LoggerFactory.CreateLog().LogError("Error" + ex.Message, 100, 2);
                ViewBag.ErrorMessage = Resources.MsgErrServerException;
                ViewBag.ErrorType = SessionErrorType.ServerException;
                return View("Error");
            }
        }
        #endregion
        #region BranchSave
        [HttpPost]
        [AuthorizeUserAttribute(Entity = "MT_BRANCH", AccessLevel = new UserActivities[] { UserActivities.Add, UserActivities.Update })]
        public ActionResult BranchSave(string ABranchCode, string AOrganizationCode, string Command, BranchDTO ABranchDTO)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    if (Command.Equals(Resources.Submit))
                        ABranchDTO.ApprovalStatus = "P";
                    else if (Command.Equals(Resources.Save))
                        ABranchDTO.ApprovalStatus = "D";

                    if (Command.Equals(Resources.Submit) || Command.Equals(Resources.Save))
                    {
                        if (string.IsNullOrWhiteSpace(tMasterManager.FindByBranchCode(ABranchDTO.BranchCode, AOrganizationCode).BranchCode))
                        {
                            ABranchDTO.CreatedBy = tCommonManager.GetLoggedUserName();
                            ABranchDTO.LastUpdatedBy = tCommonManager.GetLoggedUserName();
                            ABranchDTO.OrganizationCode = Utils.OrganizationCode;
                            tMasterManager.SaveBranchDetails(ABranchDTO);
                            TempData["Message"] = String.Format(Resources.MsgInfoSaveSuccess, String.Concat(new object[] { Resources.Branch, " ", ABranchDTO.BranchCode })); 
                        }
                        else
                        {
                            if (!string.IsNullOrWhiteSpace(ABranchCode))
                            {
                                ABranchDTO.LastUpdatedBy = tCommonManager.GetLoggedUserName();
                                tMasterManager.UpdateBranchDetails(ABranchCode, ABranchDTO);
                                TempData["Message"] = String.Format(Resources.MsgInfoSaveSuccess, String.Concat(new object[] { Resources.Branch, " ", ABranchDTO.BranchCode })); 
                            }
                            else
                            {
                                TempData["Message"] = String.Format(Resources.MsgErrAlreadyExists, String.Concat(new object[] { Resources.Branch, " ", ABranchDTO.BranchCode })); 
                            }
                        }
                    }
                    else if (Command.Equals(Resources.Delete))
                    {
                        try
                        {
                            ABranchDTO.LastUpdatedBy = tCommonManager.GetLoggedUserName();
                            ABranchDTO.ApprovalStatus = Utils.GetApprovalStatusCode(Utils.ApprovalStatus.Deleted);
                            tMasterManager.UpdateBranchDetails(ABranchCode, ABranchDTO);
                            TempData["Message"] = String.Format(Resources.MsgInfoDeleteSuccess, String.Concat(new object[] { Resources.Branch, " ", ABranchCode })); 
                        }
                        catch (Exception ex)
                        {
                            LoggerFactory.CreateLog().LogError("Error" + ex.Message, 100, 2);
                            TempData["Message"] = String.Format(Resources.MsgErrDeleteConcurrencyError, String.Concat(new object[] { Resources.Branch, " ", ABranchCode })); 
                            return RedirectToAction("BranchAdd", "master", new { ABranchCode = ABranchCode, AOrganizationCode = AOrganizationCode });
                        }
                    }
                }
            }

            catch (Exception ex)
            {
                LoggerFactory.CreateLog().LogError("Error" + ex.Message, 100, 2);
                ViewBag.ErrorMessage = Resources.MsgErrServerException;
                ViewBag.ErrorType = SessionErrorType.ServerException;
                return View("Error");
            }

            return ((Command.Equals(Resources.Submit) || Command.Equals(Resources.Delete)) ? RedirectToAction("BranchAdd", "master") : RedirectToAction("BranchAdd", "master", new { ABranchCode = ABranchCode, AOrganizationCode = AOrganizationCode }));

        }
        #endregion
        #region BranchView
        [AuthorizeUserAttribute(Entity = "MT_BRANCH", AccessLevel = new UserActivities[] { UserActivities.View })]
        public ActionResult BranchView(string ABranchCode, string AOrganizationCode)
        {
            try
            {
                BranchDTO branchDTO;
                ViewBag.TableName = "BRANCH";

                var countryCode = tLookUpManager.GetAll<CountryDTO>("", "Country");
                var regionCode = tLookUpManager.GetAll<RegionDTO>("", "Region");
                var communaCode = tLookUpManager.GetAll<CommunaDTO>("", "Communa");
                //var cityList = tLookUpManager.GetAll<CityDTO>("", "CityByRegion");
                var cityList = tLookUpManager.GetAll<CityDTO>("", "City");

                ViewBag.Countries = countryCode;
                ViewBag.Regions = regionCode;
                ViewBag.Communas = communaCode;
                ViewBag.City = cityList;

                ViewBag.Privileges = tCommonManager.GetUserPrivilegesForEntity("MT_BRANCH");

                if (String.IsNullOrWhiteSpace(ABranchCode))
                {

                    branchDTO = new BranchDTO();
                    branchDTO.OrganizationCode = Utils.OrganizationCode;
                    ViewData["FormStatus"] = branchDTO.ApprovalStatus;
                    return View("BranchAdd", branchDTO);
                }
                else
                {
                    branchDTO = tMasterManager.FindByBranchCode(Convert.ToString(ABranchCode), AOrganizationCode);
                    ViewBag.ReasonCodes = null;
                }

                ViewData["FormStatus"] = branchDTO.ApprovalStatus;

                return View("BranchAdd", branchDTO);
            }
            catch (Exception ex)
            {
                LoggerFactory.CreateLog().LogError("Error" + ex.Message, 100, 2);
                ViewBag.ErrorMessage = Resources.MsgErrServerException;
                ViewBag.ErrorType = SessionErrorType.ServerException;
                return View("Error");
            }
        }
        #endregion
        #region BranchAuthorize
        [AuthorizeUserAttribute(Entity = "MT_BRANCH", AccessLevel = new UserActivities[] { UserActivities.Authorize })]
        public ActionResult BranchAuthorize(string ABranchCode, string AOrganizationCode, string AMode, string QId)
        {
            ViewBag.RequestQueueId = QId;
            ViewBag.TableName = "BRANCH";
            if (!String.IsNullOrWhiteSpace(ABranchCode))
            {
                BranchDTO branchDTO = tMasterManager.FindByBranchCode(ABranchCode, AOrganizationCode);

                var countryCode = tLookUpManager.GetAll<CountryDTO>("", "Country");
                var regionCode = tLookUpManager.GetAll<RegionDTO>("", "Region");
                var communaCode = tLookUpManager.GetAll<CommunaDTO>("", "Communa");
                var cityCode = tLookUpManager.GetAll<CityDTO>("", "City");

                if (!String.IsNullOrWhiteSpace(branchDTO.ApprovalStatus) && branchDTO.ApprovalStatus.Equals("P"))
                {
                    var reasonCodes = tLookUpManager.GetAll<ApprovalContextDTO>("BRANCH", "ReasonContext");
                    ViewBag.ReasonCodes = reasonCodes;
                }

                ViewBag.Privileges = tCommonManager.GetUserPrivilegesForEntity("MT_BRANCH");
                ViewData["FormCall"] = "ApprovalQueue";
                ViewData["FormStatus"] = branchDTO.ApprovalStatus;

                ViewBag.Countries = countryCode;
                ViewBag.Regions = regionCode;
                ViewBag.Communas = communaCode;
                ViewBag.City = cityCode;

                return View("BranchAdd", branchDTO);
            }
            else
                return RedirectToAction("ApprovalQueueList", "Approval");
        }
        #endregion
        #region BranchDelete
        [HttpPost]
        [AuthorizeUserAttribute(Entity = "MT_BRANCH", AccessLevel = new UserActivities[] { UserActivities.Delete })]
        public JsonResult BranchDelete(string ABranchCode, string AOrganizationCode)
        {
            BranchDTO ABranchDTO = new BranchDTO();
            try
            {
                ABranchDTO = tMasterManager.FindByBranchCode(ABranchCode, AOrganizationCode);
                ABranchDTO.LastUpdatedBy = tCommonManager.GetLoggedUserName();
                ABranchDTO.ApprovalStatus = Utils.GetApprovalStatusCode(Utils.ApprovalStatus.Deleted);
                tMasterManager.UpdateBranchDetails(ABranchCode, ABranchDTO);

                var dResult = new { result = "success", action = "BranchList", controller = "Master" };
                TempData["Message"] = String.Format(Resources.MsgInfoDeleteSuccess, String.Concat(new object[] { Resources.Branch, " ", ABranchDTO.BranchCode })); 
                return Json(dResult, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                var dResultFail = new { result = "fail", action = "BranchList", controller = "Master" };
                TempData["Message"] = String.Format(Resources.MsgErrDeleteConcurrencyError, String.Concat(new object[] { Resources.Branch, " ", ABranchCode })); 
                return Json(dResultFail, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #endregion

        #region StatementInterest

        #region StatementInterestList



        [AuthorizeUserAttribute(Entity = "MT_INTERESTRATE", AccessLevel = new UserActivities[] { UserActivities.View })]
        public ActionResult StatementInterestList(StatementInterestSearch AVMStatementInterestSearch, string ClearButton, int? page)
        {
            try
            {

                int? currentPage = page == null ? 0 : page;

                var months = Utils.GetMonths();
                var Years = Utils.GetYearList();

                ViewBag.Month = months;
                ViewBag.Year = Years;

                if (AVMStatementInterestSearch.SearchButton == null)
                {

                    AVMStatementInterestSearch.BillingMonth = Utils.BusinessDate.Month;
                    AVMStatementInterestSearch.BillingYear = Utils.BusinessDate.Year;
                    ModelState.Clear();
                }

                if (AVMStatementInterestSearch.SearchButton == null && currentPage == 0)
                {
                    Session["StatementInterest"] = null;
                }

                int iRecordCount = 0;
                int iPageNumber = Convert.ToInt32((page == null) ? 0 : page - 1);
                StatementInterestDTO searchFilter = new StatementInterestDTO();

                if (!(string.IsNullOrWhiteSpace(Convert.ToString(AVMStatementInterestSearch.BillingMonth))) ||
                    !(string.IsNullOrWhiteSpace(Convert.ToString(AVMStatementInterestSearch.BillingYear))))
                {
 

                    searchFilter.BillingMonth = (Int32?)AVMStatementInterestSearch.BillingMonth == 0 ? (Int32?)null : AVMStatementInterestSearch.BillingMonth;
                    searchFilter.BillingYear = (Int32?)AVMStatementInterestSearch.BillingYear == 0 ? (Int32?)null : AVMStatementInterestSearch.BillingYear;

                    var entities = tMasterManager.FindAll(iPageNumber, searchFilter);
                    iRecordCount = entities.ToList().Count;

                    AVMStatementInterestSearch.SearchResults = entities.AsQueryable();

                }
                else
                {
                    var currResult = tMasterManager.FindStatmentInterest(1, Utils.GRD_ROWS_PER_PAGE).AsQueryable();
                    AVMStatementInterestSearch.SearchResults = currResult.AsQueryable();
                }

                AVMStatementInterestSearch.RecordCount = iRecordCount;
                AVMStatementInterestSearch.CurrentPageNumber = iPageNumber == 0 ? 1 : iPageNumber;

                AVMStatementInterestSearch.SearchResults = AVMStatementInterestSearch.SearchResults.Where(x => x.ApprovalStatus != "DEL" && x.ApprovalStatus != "DIS");
                ViewBag.Privileges = tCommonManager.GetUserPrivilegesForEntity("MT_INTERESTRATE");
                return View(AVMStatementInterestSearch);


            }
            catch (Exception ex)
            {
                LoggerFactory.CreateLog().LogError("Error" + ex.Message, 100, 2);
                ViewBag.ErrorMessage = Resources.MsgErrServerException;
                ViewBag.ErrorType = SessionErrorType.ServerException;
                return View("Error");
            }
        }

        #endregion

        #region StatementInterestAdd
        [AuthorizeUserAttribute(Entity = "MT_INTERESTRATE", AccessLevel = new UserActivities[] { UserActivities.Add, UserActivities.Update, UserActivities.View })]
        public ActionResult StatementInterestAdd(int? ABillingMonth, int? ABillingYear, string AOrganizationCode)
        {
            try
            {
                StatementInterestDTO statementinterestDTO;
                var months = Utils.GetMonths();
                var Years = Utils.GetYearList();

                if (ABillingMonth == null || ABillingYear == null)
                {
                    statementinterestDTO = new StatementInterestDTO();
                    statementinterestDTO.OrganizationCode = Utils.OrganizationCode;
                }
                else
                    statementinterestDTO = tMasterManager.FindByBillingMonthYear(AOrganizationCode,ABillingMonth, ABillingYear);


                if (statementinterestDTO.ApprovalStatus == "DEL")
                {
                    statementinterestDTO = new StatementInterestDTO();
                    statementinterestDTO.OrganizationCode = Utils.OrganizationCode;
                }

                if (!String.IsNullOrWhiteSpace(statementinterestDTO.ApprovalStatus) && statementinterestDTO.ApprovalStatus.Equals("P"))
                {
                    var reasonCodes = tLookUpManager.GetAll<ApprovalContextDTO>("SINTEREST", "ReasonContext");
                    ViewBag.ReasonCodes = reasonCodes;
                }

                ViewBag.Privileges = tCommonManager.GetUserPrivilegesForEntity("MT_INTERESTRATE");

                ViewData["FormStatus"] = statementinterestDTO.ApprovalStatus;

                ViewBag.Month = months;
                ViewBag.Year = Years;

                return View(statementinterestDTO);
            }
            catch (Exception ex)
            {
                LoggerFactory.CreateLog().LogError("Error" + ex.Message, 100, 2);
                ViewBag.ErrorMessage = Resources.MsgErrServerException;
                ViewBag.ErrorType = SessionErrorType.ServerException;
                return View("Error");
            }
        }


        #endregion

        #region StatementInterestSave
        [HttpPost]
        [AuthorizeUserAttribute(Entity = "MT_INTERESTRATE", AccessLevel = new UserActivities[] { UserActivities.Add, UserActivities.Update })]
        public ActionResult StatementInterestSave(int? ABillingMonth, int? ABillingYear, string AOrganizationCode, string Command, StatementInterestDTO AStatementInterestDTO)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    if (Command.Equals(Resources.Submit))
                        AStatementInterestDTO.ApprovalStatus = "P";
                    else if (Command.Equals(Resources.Save))
                        AStatementInterestDTO.ApprovalStatus = "D";

                    if (ABillingYear > 0 && ABillingMonth > 0)
                    {
                        if (ABillingYear > 0 && ABillingMonth > 0)
                            AStatementInterestDTO.BillingMonth = ABillingMonth;
                        AStatementInterestDTO.BillingYear = ABillingYear;
                    }
                    AStatementInterestDTO.BillMonthDesc = System.Globalization.DateTimeFormatInfo.CurrentInfo.GetMonthName(Convert.ToInt32(AStatementInterestDTO.BillingMonth.ToString()));

                    if (Command.Equals(Resources.Submit) || Command.Equals(Resources.Save))
                    {
                        if (string.IsNullOrWhiteSpace(tMasterManager.FindByBillingMonthYear(AOrganizationCode, AStatementInterestDTO.BillingMonth, AStatementInterestDTO.BillingYear).OrganizationCode))
                        {
                            AStatementInterestDTO.CreatedBy = tCommonManager.GetLoggedUserName();
                            AStatementInterestDTO.LastUpdatedBy = tCommonManager.GetLoggedUserName();
                            AStatementInterestDTO.OrganizationCode = Utils.OrganizationCode;
                            tMasterManager.SaveStatementInterestDetails(AStatementInterestDTO);
                            TempData["Message"] = String.Format(Resources.MsgInfoSaveSuccess, String.Concat(new object[] { Resources.StatementInterest, " ", AStatementInterestDTO.BillMonthDesc + "-" + AStatementInterestDTO.BillingYear }));
                        }
                        else
                        {
                            if (ABillingMonth != null && ABillingYear != null)
                            {
                                AStatementInterestDTO.LastUpdatedBy = tCommonManager.GetLoggedUserName();
                                tMasterManager.UpdateStatementInterestDetails(ABillingMonth, ABillingYear, AStatementInterestDTO);
                               TempData["Message"] = String.Format(Resources.MsgInfoSaveSuccess, String.Concat(new object[] { Resources.StatementInterest, " ", AStatementInterestDTO.BillMonthDesc + "-" + AStatementInterestDTO.BillingYear }));
                            }
                            else
                            {

                                TempData["Message"] = String.Format(Resources.MsgErrAlreadyExists, String.Concat(new object[] { Resources.StatementInterest, " ", AStatementInterestDTO.BillMonthDesc + "-" + AStatementInterestDTO.BillingYear }));
                            }
                        }
                    }
                    else if (Command.Equals(Resources.Delete))
                    {
                        try
                        {
                            AStatementInterestDTO.LastUpdatedBy = tCommonManager.GetLoggedUserName();
                            AStatementInterestDTO.ApprovalStatus = Utils.GetApprovalStatusCode(Utils.ApprovalStatus.Deleted);
                            tMasterManager.UpdateStatementInterestDetails(ABillingMonth, ABillingYear, AStatementInterestDTO);
                            TempData["Message"] = String.Format(Resources.MsgInfoDeleteSuccess, String.Concat(new object[] { Resources.StatementInterest, " ", AStatementInterestDTO.BillMonthDesc + "-" + ABillingYear }));
                        }
                        catch (Exception ex)
                        {
                            LoggerFactory.CreateLog().LogError("Error" + ex.Message, 100, 2);
                            TempData["Message"] = String.Format(Resources.MsgErrDeleteConcurrencyError, String.Concat(new object[] { Resources.StatementInterest, " ", AStatementInterestDTO.BillMonthDesc + "-" + ABillingYear }));
                            return RedirectToAction("StatementInterestAdd", "master", new { ABillingMonth = ABillingMonth, ABillingYear = ABillingYear, AOrganizationCode = AOrganizationCode });
                        }
                    }
                }
            }

            catch (Exception ex)
            {
                LoggerFactory.CreateLog().LogError("Error" + ex.Message, 100, 2);
                ViewBag.ErrorMessage = Resources.MsgErrServerException;
                ViewBag.ErrorType = SessionErrorType.ServerException;
                return View("Error");
            }

            return ((Command.Equals(Resources.Submit) || Command.Equals(Resources.Delete)) ? RedirectToAction("StatementInterestAdd", "master") : RedirectToAction("StatementInterestAdd", "master", new { ABillingMonth = ABillingMonth, ABillingYear = ABillingYear, AOrganizationCode = AOrganizationCode }));

        }
        #endregion

        #region StatementInterestView
        [AuthorizeUserAttribute(Entity = "MT_INTERESTRATE", AccessLevel = new UserActivities[] { UserActivities.View })]
        public ActionResult StatementInterestView(int? ABillingMonth, int? ABillingYear, string AOrganizationCode)
        {
            try
            {
                StatementInterestDTO StatementInterestDTO;
                ViewBag.TableName = "SINTEREST";

                var months = Utils.GetMonths();
                var Years = Utils.GetYearList();
                ViewBag.Month = months;
                ViewBag.Year = Years;

                ViewBag.Privileges = tCommonManager.GetUserPrivilegesForEntity("MT_INTERESTRATE");

                if (ABillingMonth>0 && ABillingYear >0)
                {
                    StatementInterestDTO = new StatementInterestDTO();
                
                    StatementInterestDTO = tMasterManager.FindByBillingMonthYear(AOrganizationCode, ABillingMonth, ABillingYear);
                    ViewData["FormStatus"] = StatementInterestDTO.ApprovalStatus;
                    return View("StatementInterestAdd", StatementInterestDTO);
                }
                else
                {

                    StatementInterestDTO = new StatementInterestDTO();
                    ViewData["FormStatus"] = StatementInterestDTO.ApprovalStatus;

                    return View("StatementInterestAdd", StatementInterestDTO);


                   
                    //ViewBag.ReasonCodes = null;

                }


                //if (ABillingMonth != null || ABillingYear != null)
                //{

                //    statementInterestDTO = new StatementInterestDTO();
                //    statementInterestDTO.OrganizationCode = Utils.OrganizationCode;
                //    ViewData["FormStatus"] = statementInterestDTO.ApprovalStatus;
                //    return View("StatementInterestAdd", statementInterestDTO);
                //}
                //else
                //{
                //    statementInterestDTO = tMasterManager.FindByBillingMonthYear( AOrganizationCode,ABillingMonth, ABillingYear);
                //    ViewBag.ReasonCodes = null;
                //}

                ViewData["FormStatus"] = StatementInterestDTO.ApprovalStatus;

                return View("StatementInterestAdd", StatementInterestDTO);
            }
            catch (Exception ex)
            {
                LoggerFactory.CreateLog().LogError("Error" + ex.Message, 100, 2);
                ViewBag.ErrorMessage = Resources.MsgErrServerException;
                ViewBag.ErrorType = SessionErrorType.ServerException;
                return View("Error");
            }
        }
        #endregion

        #region StatementInterestAuthorize
        [AuthorizeUserAttribute(Entity = "MT_INTERESTRATE", AccessLevel = new UserActivities[] { UserActivities.Authorize })]
        public ActionResult StatementInterestAuthorize(int AbillingMonth, int ABillingYear, string AOrganizationCode, string AMode, string QId)
        {
            ViewBag.RequestQueueId = QId;
            var months = Utils.GetMonths();
            var Years = Utils.GetYearList();
            ViewBag.Month = months;
            ViewBag.Year = Years;
            ViewBag.TableName = "SINTEREST";
            if (AbillingMonth != null || ABillingYear != null)
            {
                StatementInterestDTO statementInterestDTO = tMasterManager.FindByBillingMonthYear( AOrganizationCode,AbillingMonth, ABillingYear);

                if (!String.IsNullOrWhiteSpace(statementInterestDTO.ApprovalStatus) && statementInterestDTO.ApprovalStatus.Equals("P"))
                {
                    var reasonCodes = tLookUpManager.GetAll<ApprovalContextDTO>("SINTEREST", "ReasonContext");
                    ViewBag.ReasonCodes = reasonCodes;
                }

                ViewBag.Privileges = tCommonManager.GetUserPrivilegesForEntity("MT_INTERESTRATE");
                ViewData["FormCall"] = "ApprovalQueue";
                ViewData["FormStatus"] = statementInterestDTO.ApprovalStatus;

                return View("StatementInterestAdd", statementInterestDTO);
            }
            else
                return RedirectToAction("ApprovalQueueList", "Approval");
        }
        #endregion

        #region StatementInterestDelete
        [HttpPost]
        [AuthorizeUserAttribute(Entity = "MT_INTERESTRATE", AccessLevel = new UserActivities[] { UserActivities.Delete })]
        public JsonResult StatementInterestDelete(int ABillingMonth, int ABillingYear, string AOrganizationCode)
        {
         

            StatementInterestDTO AStatementInterestDTO = new StatementInterestDTO();
            try
            {
                AStatementInterestDTO = tMasterManager.FindByBillingMonthYear( AOrganizationCode,ABillingMonth, ABillingYear);
                AStatementInterestDTO.LastUpdatedBy = tCommonManager.GetLoggedUserName();
                AStatementInterestDTO.ApprovalStatus = Utils.GetApprovalStatusCode(Utils.ApprovalStatus.Deleted);
                tMasterManager.UpdateStatementInterestDetails(ABillingMonth, ABillingYear, AStatementInterestDTO);
                AStatementInterestDTO.BillMonthDesc = System.Globalization.DateTimeFormatInfo.CurrentInfo.GetMonthName(Convert.ToInt32(ABillingMonth.ToString()));
                var dResult = new { result = "success", action = "StatementInterestList", controller = "Master" };
                TempData["Message"] = String.Format(Resources.MsgInfoDeleteSuccess, String.Concat(new object[] { Resources.StatementInterest, " ", AStatementInterestDTO.BillMonthDesc + "-" + AStatementInterestDTO.BillingYear }));
                return Json(dResult, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                AStatementInterestDTO.BillMonthDesc = System.Globalization.DateTimeFormatInfo.CurrentInfo.GetMonthName(Convert.ToInt32(ABillingMonth.ToString()));
                var dResultFail = new { result = "fail", action = "StatementInterestList", controller = "Master" };
                TempData["Message"] = String.Format(Resources.MsgErrDeleteConcurrencyError, String.Concat(new object[] { Resources.StatementInterest, " ", AStatementInterestDTO.BillMonthDesc + "-" + ABillingYear }));
                return Json(dResultFail, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion



        #endregion

        #region Change Business Date

        #region BusinessDateList
        [AuthorizeUserAttribute(Entity = "BT_DATESETTING", AccessLevel = new UserActivities[] { UserActivities.View })]
        public ActionResult BusinessDateList()
        {
            try
            {
                
                ChangeBusinessDateDTO objChangeBusinessDateDTO = new ChangeBusinessDateDTO();

                objChangeBusinessDateDTO = tMasterManager.FindByBusinessDateOrgCode(Utils.OrganizationCode);

                objChangeBusinessDateDTO.OldBusinessDate = Utils.BusinessDate;

                ViewBag.Privileges = tCommonManager.GetUserPrivilegesForEntity("BT_DATESETTING");

                return View("ChangeBusinessDateAdd", objChangeBusinessDateDTO);
            }
            catch (Exception ex)
            {
                LoggerFactory.CreateLog().LogError("Error" + ex.Message, 100, 2);
                ViewBag.ErrorMessage = Resources.MsgErrServerException;
                ViewBag.ErrorType = SessionErrorType.ServerException;
                return View("Error");
            }
        }
        #endregion
        #region BusinessPasswordSave
        public ActionResult BusinessPasswordSave(ChangeBusinessDateDTO AChangeBusinessDateDTO)
        {
            try
            {
                tMasterManager.UpdateBusinessDate(AChangeBusinessDateDTO);
                Utils.BusinessDate = AChangeBusinessDateDTO.BusinessDate;
                AChangeBusinessDateDTO.OldBusinessDate = AChangeBusinessDateDTO.BusinessDate;
                ViewBag.Privileges = tCommonManager.GetUserPrivilegesForEntity("BT_DATESETTING");
            }
            catch (Exception ex)
            {
                LoggerFactory.CreateLog().LogError("Error" + ex.Message, 100, 2);
                ModelState.AddModelError("", Resources.MsgErrChangePwd);
            }

            TempData["Message"] = String.Format(Resources.MsgInfoUpdateSuccess, Resources.BusinessDate); 

            //return View(new ChangePassword());
            return View("ChangeBusinessDateAdd", AChangeBusinessDateDTO);
        }
        #endregion

        #endregion
        
        #region Merchant

        #region MerchantView
        [AuthorizeUserAttribute(Entity = "MT_MERCHANT", AccessLevel = new UserActivities[] { UserActivities.View })]
        public ActionResult MerchantView(string Id)
        {
            try
            {
                MerchantsDTO merchantDTO;
                ViewBag.TableName = "MERCHANT";

                var countryCode = tLookUpManager.GetAll<CountryDTO>("", "Country");
                var regionCode = tLookUpManager.GetAll<RegionDTO>("", "RegionByCommuna");
                var cityCode = tLookUpManager.GetAll<CityDTO>("", "Communa");
                var communaCode = tLookUpManager.GetAll<CommunaDTO>("", "CityByCommuna");


                if (String.IsNullOrWhiteSpace(Id))
                {
                    merchantDTO = new MerchantsDTO();
                }
                else
                {
                    merchantDTO = tMasterManager.FindByMerchantsCode(Convert.ToString(Id));
                    ViewBag.ReasonCodes = null;
                }
                ViewBag.Countries = countryCode;
                ViewBag.City = cityCode;
                ViewBag.Regions = regionCode;
                ViewBag.Communas = communaCode;
                ViewBag.Privileges = tCommonManager.GetUserPrivilegesForEntity("MT_MERCHANT");
                ViewData["FormStatus"] = merchantDTO.ApprovalStatus;

                return View("MerchantsAdd", merchantDTO);
            }
            catch (Exception ex)
            {
                LoggerFactory.CreateLog().LogError("Error" + ex.Message, 100, 2);
                ViewBag.ErrorMessage = Resources.MsgErrServerException;
                ViewBag.ErrorType = SessionErrorType.ServerException;
                return View("Error");
            }
        }
        #endregion
        #region MerchantsAdd
        [AuthorizeUserAttribute(Entity = "MT_MERCHANT", AccessLevel = new UserActivities[] { UserActivities.Add, UserActivities.Update, UserActivities.View })]
        public ActionResult MerchantsAdd(string Id)
        {
            try
            {
                MerchantsDTO merchantsDTO;
                var countryCode = tLookUpManager.GetAll<CountryDTO>("", "Country");
                var regionCode = tLookUpManager.GetAll<RegionDTO>("", "RegionByCommuna");
                var communaCode = tLookUpManager.GetAll<CommunaDTO>("", "Communa");
                var cityCode = tLookUpManager.GetAll<CityDTO>("", "CityByCommuna");

                if (Id == null)
                    merchantsDTO = new MerchantsDTO();
                else
                    merchantsDTO = tMasterManager.FindByMerchantsCode(Convert.ToString(Id));

                if (!String.IsNullOrWhiteSpace(merchantsDTO.ApprovalStatus) && merchantsDTO.ApprovalStatus.Equals("P"))
                {
                    var reasonCodes = tLookUpManager.GetAll<ApprovalContextDTO>("MERCHANT", "ReasonContext");
                    ViewBag.ReasonCodes = reasonCodes;
                }

                ViewBag.Privileges = tCommonManager.GetUserPrivilegesForEntity("MT_MERCHANT");
                ViewData["FormStatus"] = merchantsDTO.ApprovalStatus;
                ViewBag.Countries = countryCode;
                ViewBag.Regions = regionCode;
                ViewBag.Communas = communaCode;
                ViewBag.City = cityCode;

                return View(merchantsDTO);
            }
            catch (Exception ex)
            {
                LoggerFactory.CreateLog().LogError("Error" + ex.Message, 100, 2);
                ViewBag.ErrorMessage = Resources.MsgErrServerException;
                ViewBag.ErrorType = SessionErrorType.ServerException;
                return View("Error");
            }
        }

        #endregion
        #region MerchantSave
        [HttpPost]
        [AuthorizeUserAttribute(Entity = "MT_MERCHANT", AccessLevel = new UserActivities[] { UserActivities.Add, UserActivities.Update })]
        public ActionResult MerchantsSave(string Id, string Command, MerchantsDTO AMerchantDTO)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (Command.Equals(Resources.Submit))
                        AMerchantDTO.ApprovalStatus = "P";
                    else if (Command.Equals(Resources.Save))
                        AMerchantDTO.ApprovalStatus = "D";

                    
                    if (Command.Equals(Resources.Submit) || Command.Equals(Resources.Save))
                    {
                        AMerchantDTO.OrganizationCode = Utils.OrganizationCode;
                        if (string.IsNullOrWhiteSpace(tMasterManager.FindByMerchantsCode(AMerchantDTO.MerchantCode).MerchantCode))
                        {
                            AMerchantDTO.CreatedBy = tCommonManager.GetLoggedUserName();
                            AMerchantDTO.LastUpdatedBy = tCommonManager.GetLoggedUserName();
                            tMasterManager.SaveMerchantsDetails(AMerchantDTO);
                            TempData["Message"] = String.Format(Resources.MsgInfoSaveSuccess, String.Concat(new object[] { Resources.Merchant, " ", AMerchantDTO.MerchantCode })); 
                        }
                        else
                        {
                            if (!string.IsNullOrWhiteSpace(Id))
                            {
                                AMerchantDTO.LastUpdatedBy = tCommonManager.GetLoggedUserName();
                                tMasterManager.UpdateMerchantsDetails(Id, AMerchantDTO);
                                TempData["Message"] = String.Format(Resources.MsgInfoSaveSuccess, String.Concat(new object[] { Resources.Merchant, " ", AMerchantDTO.MerchantCode })); 
                            }
                            else
                            {
                                TempData["Message"] = String.Format(Resources.MsgErrAlreadyExists, String.Concat(new object[] { Resources.Merchant, " ", AMerchantDTO.MerchantCode })); 
                            }
                        }
                    }
                    if (Command.Equals(Resources.Delete))
                    {
                        try
                        {
                            AMerchantDTO.LastUpdatedBy = tCommonManager.GetLoggedUserName();
                            AMerchantDTO.ApprovalStatus = Utils.GetApprovalStatusCode(Utils.ApprovalStatus.Deleted);
                            tMasterManager.UpdateMerchantsDetails(Id, AMerchantDTO);

                            TempData["Message"] = String.Format(Resources.MsgInfoDeleteSuccess, String.Concat(new object[] { Resources.Merchant, " ", Id })); 
                        }
                        catch (Exception ex)
                        {
                            LoggerFactory.CreateLog().LogError("Error" + ex.Message, 100, 2);
                            TempData["Message"] = String.Format(Resources.MsgErrDeleteConcurrencyError, String.Concat(new object[] { Resources.Merchant, " ", Id })); 
                            return RedirectToAction("MerchantsAdd", "master", new { Id = Id });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LoggerFactory.CreateLog().LogError("Error" + ex.Message, 100, 2);
                ViewBag.ErrorMessage = Resources.MsgErrServerException;
                ViewBag.ErrorType = SessionErrorType.ServerException;
                return View("Error");
            }
            return ((Command.Equals(Resources.Add) || Command.Equals(Resources.Delete)) ? RedirectToAction("MerchantsAdd", "master") : RedirectToAction("MerchantsAdd", "master", new { Id = Id }));

        }

        #endregion
        #region MerchantList
        [AuthorizeUserAttribute(Entity = "MT_MERCHANT", AccessLevel = new UserActivities[] { UserActivities.View })]
        public ActionResult MerchantList(MerchantSearch AVMMerchantSearch, string ClearButton, int? page)
        {
            try
            {

                var countryCode = tLookUpManager.GetAll<CountryDTO>("", "Country");


                if (ClearButton == Resources.Clear)
                {
                    AVMMerchantSearch.MerchantCode = null;
                    AVMMerchantSearch.MerchantName = null;
                    AVMMerchantSearch.CountryCode = null;
                    ModelState.Clear();
                }

                int iPageNumber = Convert.ToInt32((page == null) ? 0 : page);

                if ((!(string.IsNullOrEmpty(AVMMerchantSearch.SearchButton)) &&
                     !((string.IsNullOrWhiteSpace(AVMMerchantSearch.MerchantCode) &&
                     (string.IsNullOrWhiteSpace(AVMMerchantSearch.MerchantName)) &&
                     (string.IsNullOrWhiteSpace(AVMMerchantSearch.CountryCode))))))
                {
                    var entities = tMasterManager.FindAll(iPageNumber, new MerchantsDTO()
                    {
                        MerchantCode = AVMMerchantSearch.MerchantCode,
                        MerchantName = AVMMerchantSearch.MerchantName,
                        CountryCode = AVMMerchantSearch.CountryCode
                    });
                    AVMMerchantSearch.SearchResults = entities.AsQueryable();
                }
                else
                {
                    var currResult = tMasterManager.FindMerchant(1, 5).AsQueryable();
                    AVMMerchantSearch.SearchResults = currResult.AsQueryable();
                }

                AVMMerchantSearch.SearchResults = AVMMerchantSearch.SearchResults.Where(x => x.ApprovalStatus != "DEL" && x.ApprovalStatus != "DIS");

                ViewBag.Countries = countryCode;
                ViewBag.Privileges = tCommonManager.GetUserPrivilegesForEntity("MT_MERCHANT");
                return View(AVMMerchantSearch);
            }
            catch (Exception ex)
            {
                LoggerFactory.CreateLog().LogError("Error" + ex.Message, 100, 2);
                ViewBag.ErrorMessage = Resources.MsgErrServerException;
                ViewBag.ErrorType = SessionErrorType.ServerException;
                return View("Error");
            }
        }
        #endregion
        #region MerchantAuthorize
        [AuthorizeUserAttribute(Entity = "MT_MERCHANT", AccessLevel = new UserActivities[] { UserActivities.Authorize })]
        public ActionResult MerchantAuthorize(string Id, string AMode, string QId)
        {
            ViewBag.RequestQueueId = QId;
            ViewBag.TableName = "MERCHANT";
            if (!String.IsNullOrWhiteSpace(Id))
            {
                MerchantsDTO merchantDTO = tMasterManager.FindByMerchantsCode(Id);

                var countryCode = tLookUpManager.GetAll<CountryDTO>("", "Country");
                var regionCode = tLookUpManager.GetAll<RegionDTO>("", "RegionByCommuna");
                var cityCode = tLookUpManager.GetAll<CityDTO>("", "CityByCommuna");
                var communaCode = tLookUpManager.GetAll<CommunaDTO>("", "Communa");

                if (!String.IsNullOrWhiteSpace(merchantDTO.ApprovalStatus) && merchantDTO.ApprovalStatus.Equals("P"))
                {
                    var reasonCodes = tLookUpManager.GetAll<ApprovalContextDTO>("MERCHANT", "ReasonContext");
                    ViewBag.ReasonCodes = reasonCodes;
                }

                ViewBag.Privileges = tCommonManager.GetUserPrivilegesForEntity("MT_MERCHANT");
                ViewData["FormCall"] = "ApprovalQueue";
                ViewData["FormStatus"] = merchantDTO.ApprovalStatus;
                ViewBag.Countries = countryCode;
                ViewBag.Regions = regionCode;
                ViewBag.City = cityCode;
                ViewBag.Communas = communaCode;
                return View("MerchantsAdd", merchantDTO);
            }
            else
                return RedirectToAction("ApprovalQueueList", "Approval");
        }
        #endregion
        #region MerchantDelete
        [HttpPost]
        [AuthorizeUserAttribute(Entity = "MT_MERCHANT", AccessLevel = new UserActivities[] { UserActivities.Delete })]
        public JsonResult MerchantDelete(string Id, MerchantsDTO AMerchantDTO)
        {
            MerchantsDTO AMerchantsDTO = new MerchantsDTO();
            try
            {
                AMerchantsDTO = tMasterManager.FindByMerchantsCode(Id);
                AMerchantsDTO.LastUpdatedBy = tCommonManager.GetLoggedUserName();
                AMerchantsDTO.ApprovalStatus = Utils.GetApprovalStatusCode(Utils.ApprovalStatus.Deleted);
                AMerchantsDTO.OrganizationCode = Utils.OrganizationCode;
                tMasterManager.UpdateMerchantsDetails(Id, AMerchantsDTO);

                var dResult = new { result = "success", action = "MerchantList", controller = "Master" };
                TempData["Message"] = String.Format(Resources.MsgInfoDeleteSuccess, String.Concat(new object[] { Resources.Merchant, " ", Id })); 
                return Json(dResult, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                var dResultFail = new { result = "fail", action = "MerchantList", controller = "Master" };
                TempData["Message"] = String.Format(Resources.MsgErrDeleteConcurrencyError, String.Concat(new object[] { Resources.Merchant, " ", Id })); 
                return Json(dResultFail, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #endregion

        #region MerchantTransactions

        [AuthorizeUserAttribute(Entity = "MT_MERCHANTTRANSACTION", AccessLevel = new UserActivities[] { UserActivities.View })]
        public ActionResult MerchantTransactions(MerchantTransactions AMerchantTrans, string ClearButton, int? page)
        {
            MerchantTransactions objMerchantTrans = new MerchantTransactions();           
            objMerchantTrans.TxnFromDate = Utils.BusinessDate;
            objMerchantTrans.TxnToDate = Utils.BusinessDate;
            objMerchantTrans.SearchResults = new List<MerchantTransactionsDTO>();
            try
            {
                int iPageNumber = Convert.ToInt32((page == null) ? 0 : page);
                if ((Utils.OrganizationCode != string.Empty) &&
                    (!(string.IsNullOrEmpty(AMerchantTrans.SearchButton)) ))
                {

                    objMerchantTrans.SearchResults = tMasterManager.GetMerchantTransactionDetails(AMerchantTrans.MerchantCode, AMerchantTrans.MerchantName, AMerchantTrans.TxnFromDate, AMerchantTrans.TxnToDate);
                    objMerchantTrans.TxnFromDate = AMerchantTrans.TxnFromDate==DateTime.MinValue ? Utils.BusinessDate:AMerchantTrans.TxnFromDate;
                    objMerchantTrans.TxnToDate = AMerchantTrans.TxnToDate == DateTime.MinValue ? Utils.BusinessDate : AMerchantTrans.TxnToDate;
                    objMerchantTrans.MerchantCode = AMerchantTrans.MerchantCode;
                    objMerchantTrans.MerchantName = AMerchantTrans.MerchantName;
                    objMerchantTrans.SearchButton = AMerchantTrans.SearchButton;
                }
                
                ViewBag.Privileges = tCommonManager.GetUserPrivilegesForEntity("MT_MERCHANTTRANSACTION");
                return View(objMerchantTrans);
            }
            catch (Exception ex)
            {
                LoggerFactory.CreateLog().LogError("Error" + ex.Message, 100, 2);
                ViewBag.ErrorMessage = Resources.MsgErrServerException;
                ViewBag.ErrorType = SessionErrorType.ServerException;
                return View("Error");
            }
        }

        #endregion

        #region MerchantDiscount

        #region MerchantDiscountView
        [AuthorizeUserAttribute(Entity = "MT_MERCHANTDISCOUNT", AccessLevel = new UserActivities[] { UserActivities.View })]
        public ActionResult MerchantDiscountView(string Id)
        {
            try
            {
                MerchantDiscountDTO merchantDiscountDTO;
                ViewBag.TableName = "MRCHDISC";
                var merchants = tLookUpManager.GetAll<MerchantsDTO>("", "Merchant");
                var transactionCheckFrequency = tLookUpManager.GetAll<TransactionCheckFrequencyDTO>("", "TransactionCheckFrequency");

                if (String.IsNullOrWhiteSpace(Id) || Id == "0")
                {
                    merchantDiscountDTO = new MerchantDiscountDTO();
                    merchantDiscountDTO.EffectiveFromDate = Utils.BusinessDate;

                }
                else
                {
                    merchantDiscountDTO = tMasterManager.FindByMerchantDiscountId(Convert.ToInt32(Id));
                    ViewBag.ReasonCodes = null;
                }
                ViewBag.Merchants = merchants;
                ViewBag.TransactionCheckFrequency = transactionCheckFrequency;
                ViewBag.Privileges = tCommonManager.GetUserPrivilegesForEntity("MT_MERCHANTDISCOUNT");
                ViewData["FormStatus"] = merchantDiscountDTO.ApprovalStatus;

                return View("MerchantDiscountAdd", merchantDiscountDTO);
            }
            catch (Exception ex)
            {
                LoggerFactory.CreateLog().LogError("Error" + ex.Message, 100, 2);
                ViewBag.ErrorMessage = Resources.MsgErrServerException;
                ViewBag.ErrorType = SessionErrorType.ServerException;
                return View("Error");
            }
        }
        #endregion
        #region MerchantDiscountAdd
        [AuthorizeUserAttribute(Entity = "MT_MERCHANTDISCOUNT", AccessLevel = new UserActivities[] { UserActivities.Add, UserActivities.Update, UserActivities.View })]
        public ActionResult MerchantDiscountAdd(string Id)
        {
            try
            {
                MerchantDiscountDTO merchantDiscountDTO;
                var merchants = tLookUpManager.GetAll<MerchantsDTO>("", "Merchant");
                var transactionCheckFrequency = tLookUpManager.GetAll<TransactionCheckFrequencyDTO>("", "TransactionCheckFrequency");

                if (Id == null || Id == "0")
                {
                    merchantDiscountDTO = new MerchantDiscountDTO();
                    merchantDiscountDTO.EffectiveFromDate = Utils.BusinessDate;

                }
                else
                {
                    merchantDiscountDTO = tMasterManager.FindByMerchantDiscountId(Convert.ToInt32(Id));
                }

                if (!String.IsNullOrWhiteSpace(merchantDiscountDTO.ApprovalStatus) && merchantDiscountDTO.ApprovalStatus.Equals("P"))
                {
                    var reasonCodes = tLookUpManager.GetAll<ApprovalContextDTO>("MRCHDISC", "ReasonContext");
                    ViewBag.ReasonCodes = reasonCodes;
                }

                ViewBag.Privileges = tCommonManager.GetUserPrivilegesForEntity("MT_MERCHANTDISCOUNT");
                ViewData["FormStatus"] = merchantDiscountDTO.ApprovalStatus;
                ViewBag.Merchants = merchants;
                ViewBag.TransactionCheckFrequency = transactionCheckFrequency;

                return View(merchantDiscountDTO);
            }
            catch (Exception ex)
            {
                LoggerFactory.CreateLog().LogError("Error" + ex.Message, 100, 2);
                ViewBag.ErrorMessage = Resources.MsgErrServerException;
                ViewBag.ErrorType = SessionErrorType.ServerException;
                return View("Error");
            }
        }

        #endregion
        #region MerchantDiscountSave
        [HttpPost]
        [AuthorizeUserAttribute(Entity = "MT_MERCHANTDISCOUNT", AccessLevel = new UserActivities[] { UserActivities.Add, UserActivities.Update })]
        public ActionResult MerchantDiscountSave(string Id, string Command, MerchantDiscountDTO AMerchantDiscountDTO)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (tMasterManager.CheckIfExistsValidDiscountForSelectedMerchant(Convert.ToInt32(Id), AMerchantDiscountDTO.MerchantCode, AMerchantDiscountDTO.EffectiveFromDate))
                    {
                        ModelState.AddModelError("EffectiveFromDate", string.Format(Resources.MsgErrMerchantDiscountAlreadyExists,Resources.MerchantDiscount));
                        var merchants = tLookUpManager.GetAll<MerchantsDTO>("", "Merchant");
                        var transactionCheckFrequency = tLookUpManager.GetAll<TransactionCheckFrequencyDTO>("", "TransactionCheckFrequency");
                        ViewBag.Privileges = tCommonManager.GetUserPrivilegesForEntity("MT_CUSTOMER_SEGMENT");                       
                        ViewBag.Merchants = merchants;
                        ViewBag.TransactionCheckFrequency = transactionCheckFrequency;
                        ViewData["FormStatus"] = "";
                        return View("MerchantDiscountAdd", AMerchantDiscountDTO);
                    }

                    if (Command.Equals(Resources.Submit))
                        AMerchantDiscountDTO.ApprovalStatus = "P";
                    else if (Command.Equals(Resources.Save))
                        AMerchantDiscountDTO.ApprovalStatus = "D";
                    if (Command.Equals(Resources.Submit) || Command.Equals(Resources.Save))
                    {
                        AMerchantDiscountDTO.OrganizationCode = Utils.OrganizationCode;
                        if (string.IsNullOrWhiteSpace(tMasterManager.FindByMerchantDiscountId(Convert.ToInt32(Id)).MerchantCode))
                        {
                            AMerchantDiscountDTO.CreatedBy = tCommonManager.GetLoggedUserName();
                            AMerchantDiscountDTO.LastUpdatedBy = tCommonManager.GetLoggedUserName();
                            tMasterManager.SaveMerchantDiscountDetails(AMerchantDiscountDTO);
                            TempData["Message"] = String.Format(Resources.MsgInfoSaveSuccess, Resources.MerchantDiscount);
                        }
                        else
                        {
                            if (!string.IsNullOrWhiteSpace(Id) && Id != "0")
                            {
                                AMerchantDiscountDTO.LastUpdatedBy = tCommonManager.GetLoggedUserName();
                                tMasterManager.UpdateMerchantDiscountDetails(Convert.ToInt32(Id), AMerchantDiscountDTO);
                                TempData["Message"] = String.Format(Resources.MsgInfoSaveSuccess, Resources.MerchantDiscount);
                            }
                            else
                            {
                                TempData["Message"] = String.Format(Resources.MsgErrAlreadyExists, Resources.MerchantDiscount);
                            }
                        }
                    }
                    if (Command.Equals(Resources.Delete))
                    {
                        try
                        {
                            AMerchantDiscountDTO.LastUpdatedBy = tCommonManager.GetLoggedUserName();
                            AMerchantDiscountDTO.ApprovalStatus = Utils.GetApprovalStatusCode(Utils.ApprovalStatus.Deleted);
                            tMasterManager.UpdateMerchantDiscountDetails(Convert.ToInt32(Id), AMerchantDiscountDTO);

                            TempData["Message"] = String.Format(Resources.MsgInfoDeleteSuccess, Resources.MerchantDiscount);
                        }
                        catch (Exception ex)
                        {
                            LoggerFactory.CreateLog().LogError("Error" + ex.Message, 100, 2);
                            TempData["Message"] = String.Format(Resources.MsgErrDeleteConcurrencyError, Resources.MerchantDiscount);
                            return RedirectToAction("MerchantDiscountAdd", "master", new { Id = Id });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LoggerFactory.CreateLog().LogError("Error" + ex.Message, 100, 2);
                ViewBag.ErrorMessage = Resources.MsgErrServerException;
                ViewBag.ErrorType = SessionErrorType.ServerException;
                return View("Error");
            }
            return ((Command.Equals(Resources.Add) || Command.Equals(Resources.Delete)) ? RedirectToAction("MerchantDiscountAdd", "master") : RedirectToAction("MerchantDiscountAdd", "master", new { Id = Id }));

        }

        #endregion
        #region MerchantDiscountList
        [AuthorizeUserAttribute(Entity = "MT_MERCHANTDISCOUNT", AccessLevel = new UserActivities[] { UserActivities.View })]
        public ActionResult MerchantDiscountList(MerchantDiscountSearch AVMMerchantDiscountSearch, string ClearButton, int? page)
        {
            try
            {
                var merchants = tLookUpManager.GetAll<MerchantsDTO>("", "Merchant");

                if (ClearButton == Resources.Clear)
                {
                    AVMMerchantDiscountSearch.MerchantCode = null;
                    ModelState.Clear();
                }

                int iPageNumber = Convert.ToInt32((page == null) ? 0 : page);

                if ((Utils.OrganizationCode != string.Empty) ||
                    (!(string.IsNullOrEmpty(AVMMerchantDiscountSearch.SearchButton)) &&
                     !((string.IsNullOrWhiteSpace(AVMMerchantDiscountSearch.MerchantCode)))))
                {
                    var entities = tMasterManager.FindAll(iPageNumber, new MerchantDiscountDTO()
                    {
                        MerchantCode = AVMMerchantDiscountSearch.MerchantCode
                    });
                    AVMMerchantDiscountSearch.SearchResults = entities.AsQueryable();
                }
                else
                {
                    var currResult = tMasterManager.FindMerchantDiscount(1, 5).AsQueryable();
                    AVMMerchantDiscountSearch.SearchResults = currResult.AsQueryable();
                }

                ViewBag.Merchants = merchants;
                ViewBag.Privileges = tCommonManager.GetUserPrivilegesForEntity("MT_MERCHANTDISCOUNT");
                return View(AVMMerchantDiscountSearch);
            }
            catch (Exception ex)
            {
                LoggerFactory.CreateLog().LogError("Error" + ex.Message, 100, 2);
                ViewBag.ErrorMessage = Resources.MsgErrServerException;
                ViewBag.ErrorType = SessionErrorType.ServerException;
                return View("Error");
            }
        }
        #endregion
        #region MerchantDiscountAuthorize
        [AuthorizeUserAttribute(Entity = "MT_MERCHANTDISCOUNT", AccessLevel = new UserActivities[] { UserActivities.Authorize })]
        public ActionResult MerchantDiscountAuthorize(string Id, string AMode, string QId)
        {
            ViewBag.RequestQueueId = QId;
            ViewBag.TableName = "MRCHDISC";
            if (!String.IsNullOrWhiteSpace(Id))
            {
                MerchantDiscountDTO merchantDiscountDTO = tMasterManager.FindByMerchantDiscountId(Convert.ToInt32(Id));
                var merchants = tLookUpManager.GetAll<MerchantsDTO>("", "Merchant");
                var transactionCheckFrequency = tLookUpManager.GetAll<TransactionCheckFrequencyDTO>("", "TransactionCheckFrequency");

                if (!String.IsNullOrWhiteSpace(merchantDiscountDTO.ApprovalStatus) && merchantDiscountDTO.ApprovalStatus.Equals("P"))
                {
                    var reasonCodes = tLookUpManager.GetAll<ApprovalContextDTO>("MRCHDISC", "ReasonContext");
                    ViewBag.ReasonCodes = reasonCodes;
                }

                ViewBag.Merchants = merchants;
                ViewBag.TransactionCheckFrequency = transactionCheckFrequency;
                ViewBag.Privileges = tCommonManager.GetUserPrivilegesForEntity("MT_MERCHANTDISCOUNT");
                ViewData["FormStatus"] = merchantDiscountDTO.ApprovalStatus;
                ViewData["FormCall"] = "ApprovalQueue";

                return View("MerchantDiscountAdd", merchantDiscountDTO);
            }
            else
                return RedirectToAction("ApprovalQueueList", "Approval");
        }
        #endregion
        #region MerchantDiscountDelete
        [HttpPost]
        [AuthorizeUserAttribute(Entity = "MT_MERCHANTDISCOUNT", AccessLevel = new UserActivities[] { UserActivities.Delete })]
        public JsonResult MerchantDiscountDelete(string Id, MerchantDiscountDTO AMerchantDiscountDTO)
        {
            MerchantDiscountDTO merchantDiscountDTO = new MerchantDiscountDTO();
            try
            {
                AMerchantDiscountDTO = tMasterManager.FindByMerchantDiscountId(Convert.ToInt32(Id));
                AMerchantDiscountDTO.LastUpdatedBy = tCommonManager.GetLoggedUserName();
                AMerchantDiscountDTO.ApprovalStatus = Utils.GetApprovalStatusCode(Utils.ApprovalStatus.Deleted);
                AMerchantDiscountDTO.OrganizationCode = Utils.OrganizationCode;
                tMasterManager.UpdateMerchantDiscountDetails(Convert.ToInt32(Id), AMerchantDiscountDTO);

                var dResult = new { result = "success", action = "MerchantDiscountList", controller = "Master" };
                TempData["Message"] = String.Format(Resources.MsgInfoDeleteSuccess, Resources.MerchantDiscount);
                return Json(dResult, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                var dResultFail = new { result = "fail", action = "MerchantDiscountList", controller = "Master" };
                TempData["Message"] = String.Format(Resources.MsgErrDeleteConcurrencyError, Resources.MerchantDiscount);
                return Json(dResultFail, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion       

        #endregion

        #region UFRateSettings

        #region UFRateSettingsView
        public ActionResult UFRateSettingsView(string Id)
        {
            try
            {
                UFRateSettingsDTO objUFRateSettingsDTO = new UFRateSettingsDTO();

                //objUFRateSettingsDTO = tMasterManager.FindByUFRateSettingsEffectiveDate(Utils.BusinessDate);

                if (Convert.ToDateTime(tMasterManager.FindByUFRateSettingsEffectiveDate(Convert.ToDateTime(Id)).EffectiveDate) == DateTime.MinValue)
                {
                    objUFRateSettingsDTO = tMasterManager.FindByUFRateSettingsEffectiveDate(Utils.BusinessDate);
                }

                objUFRateSettingsDTO.EffectiveDate = Utils.BusinessDate;

                ViewBag.Privileges = tCommonManager.GetUserPrivilegesForEntity("UFRATE_SETTINGS");

                return View(objUFRateSettingsDTO);
            }
            catch (Exception ex)
            {
                LoggerFactory.CreateLog().LogError("Error" + ex.Message, 100, 2);
                ViewBag.ErrorMessage = Resources.MsgErrServerException;
                ViewBag.ErrorType = SessionErrorType.ServerException;
                return View("Error");
            }
            
        }
        #endregion
        #region UFRateSettingsAdd
        [AuthorizeUserAttribute(Entity = "UFRATE_SETTINGS", AccessLevel = new UserActivities[] { UserActivities.Add, UserActivities.Update, UserActivities.View })]
        public ActionResult UFRateSettingsAdd(string Id)
        {
            try
            {

                UFRateSettingsDTO objUFRateSettingsDTO = new UFRateSettingsDTO();

                //objUFRateSettingsDTO = tMasterManager.FindByUFRateSettingsEffectiveDate(Utils.BusinessDate);

                if (Convert.ToDateTime(tMasterManager.FindByUFRateSettingsEffectiveDate(Convert.ToDateTime(Id)).EffectiveDate) != DateTime.MinValue)
                {
                    objUFRateSettingsDTO = tMasterManager.FindByUFRateSettingsEffectiveDate(Convert.ToDateTime(Id));
                }
                else 
                {
                    objUFRateSettingsDTO.EffectiveDate = Utils.BusinessDate;
                }

                ViewBag.Privileges = tCommonManager.GetUserPrivilegesForEntity("UFRATE_SETTINGS");
                
                return View(objUFRateSettingsDTO);
            }
            catch (Exception ex)
            {
                LoggerFactory.CreateLog().LogError("Error" + ex.Message, 100, 2);
                ViewBag.ErrorMessage = Resources.MsgErrServerException;
                ViewBag.ErrorType = SessionErrorType.ServerException;
                return View("Error");
            }
        }
        #endregion
        #region UFRateSettingsList
        [AuthorizeUserAttribute(Entity = "UFRATE_SETTINGS", AccessLevel = new UserActivities[] { UserActivities.View })]
        public ActionResult UFRateSettingsList(UFRateSettingsSearch AVMUFRateSettingsSearch, string ClearButton, int? page)
        {
            try
            {
                int iPageNumber = Convert.ToInt32((page == null) ? 0 : page);

                if (ClearButton == @Resources.Clear)
                {
                    AVMUFRateSettingsSearch.BillMonth = Utils.BusinessDate.Month;
                    AVMUFRateSettingsSearch.BillYear = Utils.BusinessDate.Year;
                    ModelState.Clear();
                }

                var months = Utils.GetMonths();
                var Years = Utils.GetYearList();
                ViewBag.Month = months;
                ViewBag.Year = Years;

                if (string.IsNullOrEmpty(AVMUFRateSettingsSearch.SearchButton)) 
                {
                    AVMUFRateSettingsSearch.BillMonth = Utils.BusinessDate.Month;
                    AVMUFRateSettingsSearch.BillYear = Utils.BusinessDate.Year;
                }
                
                AVMUFRateSettingsSearch.SearchButton = @Resources.Search;

                if (!(string.IsNullOrEmpty(AVMUFRateSettingsSearch.SearchButton)) &&
                !((
                    (string.IsNullOrEmpty(Convert.ToString(AVMUFRateSettingsSearch.BillMonth))) &&
                    (string.IsNullOrEmpty(Convert.ToString(AVMUFRateSettingsSearch.BillYear)))
                )))
                {
                    var currResult = tMasterManager.FilterUFRateSettings(AVMUFRateSettingsSearch.BillMonth, AVMUFRateSettingsSearch.BillYear);
                    AVMUFRateSettingsSearch.SearchResults = currResult.AsQueryable();
                }
                else
                {
                    var currResult = tMasterManager.FindUFRateSettings(1, 5).AsQueryable();
                    AVMUFRateSettingsSearch.SearchResults = currResult.AsQueryable();
                }


                return View(AVMUFRateSettingsSearch);
            }
            catch (Exception ex)
            {
                LoggerFactory.CreateLog().LogError("Error" + ex.Message, 100, 2);
                ViewBag.ErrorMessage = Resources.MsgErrServerException;
                ViewBag.ErrorType = SessionErrorType.ServerException;
                return View("Error");
            }
        }
        #endregion
        #region UFRateSettingsSave
        [HttpPost]
        [AuthorizeUserAttribute(Entity = "UFRATE_SETTINGS", AccessLevel = new UserActivities[] { UserActivities.Add, UserActivities.Update })]
        public ActionResult UFRateSettingsSave(string Command, UFRateSettingsDTO AUFRateSettingsDTO)
        {
            DateTime Id = AUFRateSettingsDTO.EffectiveDate;
            try
            {
                if (ModelState.IsValid)
                {
                    if (Command.Equals(Resources.Submit))
                    {
                        AUFRateSettingsDTO.OrganizationCode = Utils.OrganizationCode;
                        if (Convert.ToDateTime(tMasterManager.FindByUFRateSettingsEffectiveDate(AUFRateSettingsDTO.EffectiveDate).EffectiveDate) == DateTime.MinValue)
                        {
                            AUFRateSettingsDTO.CreatedBy = tCommonManager.GetLoggedUserName();
                            AUFRateSettingsDTO.LastUpdatedBy = tCommonManager.GetLoggedUserName();
                            tMasterManager.SaveUFRateSettings(AUFRateSettingsDTO);
                            TempData["Message"] = String.Format(Resources.MsgInfoSaveSuccess, Resources.UFRate);
                        }
                        else
                        {
                            if (Convert.ToDateTime(Id) != DateTime.MinValue)
                            {
                                AUFRateSettingsDTO.LastUpdatedBy = tCommonManager.GetLoggedUserName();
                                tMasterManager.UpdateUFRateSettings(Id, AUFRateSettingsDTO);
                                TempData["Message"] = String.Format(Resources.MsgInfoSaveSuccess, Resources.UFRate);
                            }
                            else
                            {
                                TempData["Message"] = String.Format(Resources.MsgErrAlreadyExists, Resources.UFRate);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LoggerFactory.CreateLog().LogError("Error" + ex.Message, 100, 2);
                ViewBag.ErrorMessage = Resources.MsgErrServerException;
                ViewBag.ErrorType = SessionErrorType.ServerException;
                return View("Error");
            }
            return (RedirectToAction("UFRateSettingsAdd", "master"));
        }


        #endregion

        #endregion

        #region SalesId

        #region SalesIdList
        [AuthorizeUserAttribute(Entity = "MT_SalesId", AccessLevel = new UserActivities[] { UserActivities.View })]
        public ActionResult SalesIdList(SalesIdSearch AVMSalesIdSearch, string ClearButton, int? page)
        {
            try
            {

                if (ClearButton == Resources.Clear)
                {
                    AVMSalesIdSearch.SalesId = null;
                    AVMSalesIdSearch.RUT = null;
                    AVMSalesIdSearch.BranchCode = null;

                    ModelState.Clear();
                }

                int iPageNumber = Convert.ToInt32((page == null) ? 0 : page);

                if ((Utils.OrganizationCode != "") || (!(string.IsNullOrEmpty(AVMSalesIdSearch.SearchButton)) &&
                   !((string.IsNullOrWhiteSpace(AVMSalesIdSearch.SalesId) &&
                   (string.IsNullOrWhiteSpace(AVMSalesIdSearch.RUT)) &&
                   (string.IsNullOrWhiteSpace(AVMSalesIdSearch.BranchCode))
                   ))))
                {
                    var entities = tMasterManager.FindAll(iPageNumber, new SalesIdDTO()
                    {
                        SalesId = AVMSalesIdSearch.SalesId,
                        RUT= AVMSalesIdSearch.RUT,
                        BranchCode  = AVMSalesIdSearch.BranchCode
                    });
                    AVMSalesIdSearch.SearchResults = entities.AsQueryable();
                }
                else
                {
                    var currResult = tMasterManager.FindSalesId(1, Utils.GRD_ROWS_PER_PAGE).AsQueryable();
                    AVMSalesIdSearch.SearchResults = currResult.AsQueryable();
                }

                AVMSalesIdSearch.SearchResults = AVMSalesIdSearch.SearchResults.Where(x => x.ApprovalStatus != "DEL" && x.ApprovalStatus != "DIS");
                ViewBag.Privileges = tCommonManager.GetUserPrivilegesForEntity("MT_SalesId");
                return View(AVMSalesIdSearch);
            }
            catch (Exception ex)
            {
                LoggerFactory.CreateLog().LogError("Error" + ex.Message, 100, 2);
                ViewBag.ErrorMessage = Resources.MsgErrServerException;
                ViewBag.ErrorType = SessionErrorType.ServerException;
                return View("Error");
            }
        }
        #endregion
        #region SalesIdAdd
        [AuthorizeUserAttribute(Entity = "MT_SalesId", AccessLevel = new UserActivities[] { UserActivities.Add, UserActivities.Update, UserActivities.View })]
        public ActionResult SalesIdAdd(string ASalesId,string ARUT, string AOrganizationCode)
        {
            try
            {
                SalesIdDTO salesIdDTO;

                var branchcode = tLookUpManager.GetAll<CommunaDTO>("", "Branch");
                var communaCode = tLookUpManager.GetAll<CommunaDTO>("", "Communabyutils");
                var cityList = tLookUpManager.GetAll<CityDTO>("", "CityByCommuna");

                if (ASalesId == null)
                {
                    salesIdDTO = new SalesIdDTO();
                    salesIdDTO.OrganizationCode = Utils.OrganizationCode;
                }
                else
                    salesIdDTO = tMasterManager.FindBySalesId(ASalesId, ARUT.Replace("-", ""), AOrganizationCode);


                if (salesIdDTO.ApprovalStatus == "DEL")
                {
                    salesIdDTO = new SalesIdDTO();
                    salesIdDTO.OrganizationCode = Utils.OrganizationCode;
                }

                if (!String.IsNullOrWhiteSpace(salesIdDTO.ApprovalStatus) && salesIdDTO.ApprovalStatus.Equals("P"))
                {
                    var reasonCodes = tLookUpManager.GetAll<ApprovalContextDTO>("SALESID", "ReasonContext");
                    ViewBag.ReasonCodes = reasonCodes;
                }

                ViewBag.Privileges = tCommonManager.GetUserPrivilegesForEntity("MT_SalesId");

                ViewData["FormStatus"] = salesIdDTO.ApprovalStatus;
                ViewBag.Branch = branchcode;
                ViewBag.Communas = communaCode;
                ViewBag.City = cityList;

                return View(salesIdDTO);
            }
            catch (Exception ex)
            {
                LoggerFactory.CreateLog().LogError("Error" + ex.Message, 100, 2);
                ViewBag.ErrorMessage = Resources.MsgErrServerException;
                ViewBag.ErrorType = SessionErrorType.ServerException;
                return View("Error");
            }
        }
        #endregion
        #region SalesIdSave
        [HttpPost]
        [AuthorizeUserAttribute(Entity = "MT_SalesId", AccessLevel = new UserActivities[] { UserActivities.Add, UserActivities.Update })]
        public ActionResult SalesIdSave(string ASalesId, string ARUT, string AOrganizationCode, string Command, SalesIdDTO ASalesDTO)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    if (Command.Equals(Resources.Submit))
                        ASalesDTO.ApprovalStatus = "P";
                    else if (Command.Equals(Resources.Save))
                        ASalesDTO.ApprovalStatus = "D";

                    if (Command.Equals(Resources.Submit) || Command.Equals(Resources.Save))
                    {
                        if (string.IsNullOrWhiteSpace(tMasterManager.FindBySalesId(ASalesDTO.SalesId,ASalesDTO.RUT.Replace ("-","")  , AOrganizationCode).SalesId))
                        {
                            ASalesDTO.CreatedBy = tCommonManager.GetLoggedUserName();
                            ASalesDTO.LastUpdatedBy = tCommonManager.GetLoggedUserName();
                            ASalesDTO.OrganizationCode = Utils.OrganizationCode;
                            tMasterManager.SaveSalesDetails (ASalesDTO);
                            TempData["Message"] = String.Format(Resources.MsgInfoSaveSuccess, String.Concat(new object[] { Resources.SalesId, " ", ASalesDTO.SalesId }));
                        }
                        else
                        {
                            if (!string.IsNullOrWhiteSpace(ASalesId))
                            {
                                ASalesDTO.LastUpdatedBy = tCommonManager.GetLoggedUserName();
                                tMasterManager.UpdateSalesIdDetails(ASalesId, ARUT.Replace("-", ""), ASalesDTO);
                                TempData["Message"] = String.Format(Resources.MsgInfoSaveSuccess, String.Concat(new object[] { Resources.SalesId, " ", ASalesDTO.SalesId }));
                            }
                            else
                            {
                                TempData["Message"] = String.Format(Resources.MsgErrAlreadyExists, String.Concat(new object[] { Resources.SalesId, " ", ASalesDTO.SalesId }));
                            }
                        }
                    }
                    else if (Command.Equals(Resources.Delete))
                    {
                        try
                        {
                            ASalesDTO.LastUpdatedBy = tCommonManager.GetLoggedUserName();
                            ASalesDTO.ApprovalStatus = Utils.GetApprovalStatusCode(Utils.ApprovalStatus.Deleted);
                            tMasterManager.UpdateSalesIdDetails(ASalesId, ARUT.Replace("-", ""), ASalesDTO);
                            TempData["Message"] = String.Format(Resources.MsgInfoDeleteSuccess, String.Concat(new object[] { Resources.SalesId, " ", ASalesId }));
                        }
                        catch (Exception ex)
                        {
                            LoggerFactory.CreateLog().LogError("Error" + ex.Message, 100, 2);
                            TempData["Message"] = String.Format(Resources.MsgErrWhileDelete, String.Concat(new object[] { Resources.SalesId, " ", ASalesId }));
                            return RedirectToAction("SalesIdAdd", "master", new { ASalesId = ASalesId, ARUT = ARUT, AOrganizationCode = AOrganizationCode });
                        }
                    }
                }
            }

            catch (Exception ex)
            {
                LoggerFactory.CreateLog().LogError("Error" + ex.Message, 100, 2);
                ViewBag.ErrorMessage = Resources.MsgErrServerException;
                ViewBag.ErrorType = SessionErrorType.ServerException;
                return View("Error");
            }

            return ((Command.Equals(Resources.Submit) || Command.Equals(Resources.Delete)) ? RedirectToAction("SalesIdAdd", "master") : RedirectToAction("SalesIdAdd", "master", new { ASalesID = ASalesId, ARUT = ARUT, AOrganizationCode = AOrganizationCode }));

        }
        #endregion
        #region SalesIdView
        [AuthorizeUserAttribute(Entity = "MT_SalesId", AccessLevel = new UserActivities[] { UserActivities.View })]
        public ActionResult SalesIdView(string ASalesId,string ARUT, string AOrganizationCode)
        {
            try
            {
                SalesIdDTO salesIdDTO;
                ViewBag.TableName = "SALESID";

                var communaCode = tLookUpManager.GetAll<CommunaDTO>("", "Communabyutils");
                var cityList = tLookUpManager.GetAll<CityDTO>("", "City");
                var branchlist = tLookUpManager.GetAll<CityDTO>("", "Branch");


                ViewBag.Branch = branchlist;
                ViewBag.Communas = communaCode;
                ViewBag.City = cityList;

                ViewBag.Privileges = tCommonManager.GetUserPrivilegesForEntity("MT_SalesId");

                if (String.IsNullOrWhiteSpace(ASalesId))
                {

                    salesIdDTO = new SalesIdDTO();
                    salesIdDTO.OrganizationCode = Utils.OrganizationCode;
                    ViewData["FormStatus"] = salesIdDTO.ApprovalStatus;
                    return View("SalesIdAdd", salesIdDTO);
                }
                else
                {
                    salesIdDTO = tMasterManager.FindBySalesId(ASalesId, ARUT.Replace("-", ""), AOrganizationCode);
                    ViewBag.ReasonCodes = null;
                }

                ViewData["FormStatus"] = salesIdDTO.ApprovalStatus;

                return View("SalesIdAdd", salesIdDTO);
            }
            catch (Exception ex)
            {
                LoggerFactory.CreateLog().LogError("Error" + ex.Message, 100, 2);
                ViewBag.ErrorMessage = Resources.MsgErrServerException;
                ViewBag.ErrorType = SessionErrorType.ServerException;
                return View("Error");
            }
        }
        #endregion
        #region SalesAuthorize
        [AuthorizeUserAttribute(Entity = "MT_SalesId", AccessLevel = new UserActivities[] { UserActivities.Authorize })]
        public ActionResult SalesIdAuthorize(string ASalesId, string ARUT, string AOrganizationCode, string AMode, string QId)
        {
            ViewBag.RequestQueueId = QId;
            ViewBag.TableName = "SALEID";
            if (!String.IsNullOrWhiteSpace(ASalesId))
            {
                SalesIdDTO salesIdDTO = tMasterManager.FindBySalesId(ASalesId, ARUT.Replace ("-",""), AOrganizationCode);

               
                var branch = tLookUpManager.GetAll<RegionDTO>("", "Branch");
                var communaCode = tLookUpManager.GetAll<CommunaDTO>("", "Communabyutils");

                var cityCode = tLookUpManager.GetAll<CityDTO>("", "City");

                if (!String.IsNullOrWhiteSpace(salesIdDTO.ApprovalStatus) && salesIdDTO.ApprovalStatus.Equals("P"))
                {
                    var reasonCodes = tLookUpManager.GetAll<ApprovalContextDTO>("SALEID", "ReasonContext");
                    ViewBag.ReasonCodes = reasonCodes;
                }

                ViewBag.Privileges = tCommonManager.GetUserPrivilegesForEntity("MT_SalesId");
                ViewData["FormCall"] = "ApprovalQueue";
                ViewData["FormStatus"] = salesIdDTO.ApprovalStatus;

                ViewBag.Branch = branch;
                ViewBag.Communas = communaCode;
                ViewBag.City = cityCode;

                return View("SalesIdAdd", salesIdDTO);
            }
            else
                return RedirectToAction("ApprovalQueueList", "Approval");
        }
        #endregion
        #region BranchDelete
        [HttpPost]
        [AuthorizeUserAttribute(Entity = "MT_SalesId", AccessLevel = new UserActivities[] { UserActivities.Delete })]
        public JsonResult SalesDelete(string ASalesId,string ARUT, string AOrganizationCode)
        {
            SalesIdDTO ASalesIdDTO = new SalesIdDTO();
            try
            {
                ASalesIdDTO = tMasterManager.FindBySalesId(ASalesId, ARUT.Replace ("-",""), AOrganizationCode);
                ASalesIdDTO.LastUpdatedBy = tCommonManager.GetLoggedUserName();
                ASalesIdDTO.ApprovalStatus = Utils.GetApprovalStatusCode(Utils.ApprovalStatus.Deleted);
                tMasterManager.UpdateSalesIdDetails(ASalesId, ARUT.Replace("-", ""), ASalesIdDTO);

                var dResult = new { result = "success", action = "SalesIdList", controller = "Master" };
                TempData["Message"] = String.Format(Resources.MsgInfoDeleteSuccess, String.Concat(new object[] { Resources.SalesId, " ", ASalesIdDTO.SalesId }));
                return Json(dResult, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                var dResultFail = new { result = "fail", action = "SalesIdList", controller = "Master" };
                TempData["Message"] = String.Format(Resources.MsgErrWhileDelete, String.Concat(new object[] { Resources.SalesId, " ", ASalesIdDTO.SalesId }));
                return Json(dResultFail, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #endregion

        #region DirectSalesAgent

        #region DSAList
        [AuthorizeUserAttribute(Entity = "DSA", AccessLevel = new UserActivities[] { UserActivities.View })]
        public ActionResult DSAList(DSASearch AVMDSASearch, string ClearButton, int? page)
        {
            try
            {

                if (ClearButton == Resources.Clear)
                {
                    AVMDSASearch.DSACode = null;
                    AVMDSASearch.DSAName = null;

                    ModelState.Clear();
                }

                int iPageNumber = Convert.ToInt32((page == null) ? 0 : page);

                if ((Utils.OrganizationCode != "") || (!(string.IsNullOrEmpty(AVMDSASearch.SearchButton)) &&
                   !((string.IsNullOrWhiteSpace(AVMDSASearch.DSACode) &&
                   (string.IsNullOrWhiteSpace(AVMDSASearch.DSAName)) 
                   ))))
                {
                    var entities = tMasterManager.FindAll(iPageNumber, new DirectSalesAgentDTO()
                    {
                        DSACode = AVMDSASearch.DSACode,
                        DSAName = AVMDSASearch.DSAName
                    });
                    AVMDSASearch.SearchResults = entities.AsQueryable();
                }
                else
                {
                    var currResult = tMasterManager.FindDirectSalesAgent(1, Utils.GRD_ROWS_PER_PAGE).AsQueryable();
                    AVMDSASearch.SearchResults = currResult.AsQueryable();
                }

                AVMDSASearch.SearchResults = AVMDSASearch.SearchResults.Where(x => x.ApprovalStatus != "DEL" && x.ApprovalStatus != "DIS");
                ViewBag.Privileges = tCommonManager.GetUserPrivilegesForEntity("DSA");
                return View(AVMDSASearch);
            }
            catch (Exception ex)
            {
                LoggerFactory.CreateLog().LogError("Error" + ex.Message, 100, 2);
                ViewBag.ErrorMessage = Resources.MsgErrServerException;
                ViewBag.ErrorType = SessionErrorType.ServerException;
                return View("Error");
            }
        }
        #endregion
        #region DSAAdd
        [AuthorizeUserAttribute(Entity = "DSA", AccessLevel = new UserActivities[] { UserActivities.Add, UserActivities.Update, UserActivities.View })]
        public ActionResult DSAAdd(string ADSACode, string AOrganizationCode)
        {
            try
            {
                DirectSalesAgentDTO directSalesAgentDTO;

                var branchcode = tLookUpManager.GetAll<BranchDTO>("", "Branch");

                if (ADSACode == null)
                {
                    directSalesAgentDTO = new DirectSalesAgentDTO();
                    directSalesAgentDTO.OrganizationCode = Utils.OrganizationCode;
                }
                else
                    directSalesAgentDTO = tMasterManager.FindByDirectSalesAgent(ADSACode);

                if (directSalesAgentDTO.ApprovalStatus == "DEL")
                {
                    directSalesAgentDTO = new DirectSalesAgentDTO();
                    directSalesAgentDTO.OrganizationCode = Utils.OrganizationCode;
                }

                if (!String.IsNullOrWhiteSpace(directSalesAgentDTO.ApprovalStatus) && directSalesAgentDTO.ApprovalStatus.Equals("P"))
                {
                    var reasonCodes = tLookUpManager.GetAll<ApprovalContextDTO>("DSACNTXT", "ReasonContext");
                    ViewBag.ReasonCodes = reasonCodes;
                }

                ViewBag.Privileges = tCommonManager.GetUserPrivilegesForEntity("DSA");

                ViewData["FormStatus"] = directSalesAgentDTO.ApprovalStatus;
                ViewBag.Branch = branchcode;

                return View(directSalesAgentDTO);
            }
            catch (Exception ex)
            {
                LoggerFactory.CreateLog().LogError("Error" + ex.Message, 100, 2);
                ViewBag.ErrorMessage = Resources.MsgErrServerException;
                ViewBag.ErrorType = SessionErrorType.ServerException;
                return View("Error");
            }
        }
        #endregion
        #region DirectSalesAgentSave
        [HttpPost]
        [AuthorizeUserAttribute(Entity = "DSA", AccessLevel = new UserActivities[] { UserActivities.Add, UserActivities.Update })]
        public ActionResult DSASave(string ADSACode, string AOrganizationCode, string Command, DirectSalesAgentDTO ADirectSalesAgentDTO)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    if (Command.Equals(Resources.Submit))
                        ADirectSalesAgentDTO.ApprovalStatus = "P";
                    else if (Command.Equals(Resources.Save))
                        ADirectSalesAgentDTO.ApprovalStatus = "D";

                    if (Command.Equals(Resources.Submit) || Command.Equals(Resources.Save))
                    {
                        if (string.IsNullOrWhiteSpace(tMasterManager.FindByDirectSalesAgent(ADirectSalesAgentDTO.DSACode).DSACode))
                        {
                            ADirectSalesAgentDTO.CreatedBy = tCommonManager.GetLoggedUserName();
                            ADirectSalesAgentDTO.LastUpdatedBy = tCommonManager.GetLoggedUserName();
                            ADirectSalesAgentDTO.OrganizationCode = Utils.OrganizationCode;
                            tMasterManager.SaveDirectSalesAgent(ADirectSalesAgentDTO);
                            TempData["Message"] = String.Format(Resources.MsgInfoSaveSuccess, String.Concat(new object[] { Resources.DSA, " ", ADirectSalesAgentDTO.DSACode }));
                        }
                        else
                        {
                            if (!string.IsNullOrWhiteSpace(ADSACode))
                            {
                                ADirectSalesAgentDTO.LastUpdatedBy = tCommonManager.GetLoggedUserName();
                                tMasterManager.UpdateDirectSalesAgent(ADSACode, ADirectSalesAgentDTO);
                                TempData["Message"] = String.Format(Resources.MsgInfoSaveSuccess, String.Concat(new object[] { Resources.DSA, " ", ADirectSalesAgentDTO.DSACode }));
                            }
                            else
                            {
                                TempData["Message"] = String.Format(Resources.MsgErrAlreadyExists, String.Concat(new object[] { Resources.DSA, " ", ADirectSalesAgentDTO.DSACode }));
                            }
                        }
                    }
                    else if (Command.Equals(Resources.Delete))
                    {
                        try
                        {
                            ADirectSalesAgentDTO.LastUpdatedBy = tCommonManager.GetLoggedUserName();
                            ADirectSalesAgentDTO.ApprovalStatus = Utils.GetApprovalStatusCode(Utils.ApprovalStatus.Deleted);
                            tMasterManager.UpdateDirectSalesAgent(ADSACode, ADirectSalesAgentDTO);
                            TempData["Message"] = String.Format(Resources.MsgInfoDeleteSuccess, String.Concat(new object[] { Resources.DSA, " ", ADSACode }));
                        }
                        catch (Exception ex)
                        {
                            LoggerFactory.CreateLog().LogError("Error" + ex.Message, 100, 2);
                            TempData["Message"] = String.Format(Resources.MsgErrWhileDelete, String.Concat(new object[] { Resources.DSA, " ", ADSACode }));
                            return RedirectToAction("DSAAdd", "master", new { ADSACode = ADSACode, AOrganizationCode = AOrganizationCode });
                        }
                    }
                }
            }

            catch (Exception ex)
            {
                LoggerFactory.CreateLog().LogError("Error" + ex.Message, 100, 2);
                ViewBag.ErrorMessage = Resources.MsgErrServerException;
                ViewBag.ErrorType = SessionErrorType.ServerException;
                return View("Error");
            }

            return ((Command.Equals(Resources.Submit) || Command.Equals(Resources.Delete)) ? RedirectToAction("DSAAdd", "master") : RedirectToAction("DSAAdd", "master", new { ADSACode = ADSACode, AOrganizationCode = AOrganizationCode }));

        }
        #endregion
        #region DirectSalesAgentView
        [AuthorizeUserAttribute(Entity = "DSA", AccessLevel = new UserActivities[] { UserActivities.View })]
        public ActionResult DirectSalesAgentView(string ADSACode, string AOrganizationCode)
        {
            try
            {
                DirectSalesAgentDTO directSalesAgentDTO;
                ViewBag.TableName = "DSACNTXT";

                var branchlist = tLookUpManager.GetAll<BranchDTO>("", "Branch");

                ViewBag.Branch = branchlist;

                ViewBag.Privileges = tCommonManager.GetUserPrivilegesForEntity("DSA");

                if (String.IsNullOrWhiteSpace(ADSACode))
                {

                    directSalesAgentDTO = new DirectSalesAgentDTO();
                    directSalesAgentDTO.OrganizationCode = Utils.OrganizationCode;
                    ViewData["FormStatus"] = directSalesAgentDTO.ApprovalStatus;
                    return View("DSAAdd", directSalesAgentDTO);
                }
                else
                {
                    directSalesAgentDTO = tMasterManager.FindByDirectSalesAgent(ADSACode);
                    ViewBag.ReasonCodes = null;
                }

                ViewData["FormStatus"] = directSalesAgentDTO.ApprovalStatus;

                return View("DSAAdd", directSalesAgentDTO);
            }
            catch (Exception ex)
            {
                LoggerFactory.CreateLog().LogError("Error" + ex.Message, 100, 2);
                ViewBag.ErrorMessage = Resources.MsgErrServerException;
                ViewBag.ErrorType = SessionErrorType.ServerException;
                return View("Error");
            }
        }
        #endregion
        #region DirectSalesAgentAuthorize
        [AuthorizeUserAttribute(Entity = "DSA", AccessLevel = new UserActivities[] { UserActivities.Authorize })]
        public ActionResult DSAAuthorize(string ADSACode, string AOrganizationCode, string AMode, string QId)
        {
            ViewBag.RequestQueueId = QId;
            ViewBag.TableName = "DSACNTXT";
            if (!String.IsNullOrWhiteSpace(ADSACode))
            {
                DirectSalesAgentDTO directSalesAgentDTO = tMasterManager.FindByDirectSalesAgent(ADSACode);
                
                var branch = tLookUpManager.GetAll<BranchDTO>("", "Branch");
                
                if (!String.IsNullOrWhiteSpace(directSalesAgentDTO.ApprovalStatus) && directSalesAgentDTO.ApprovalStatus.Equals("P"))
                {
                    var reasonCodes = tLookUpManager.GetAll<ApprovalContextDTO>("DSACNTXT", "ReasonContext");
                    ViewBag.ReasonCodes = reasonCodes;
                }

                ViewBag.Privileges = tCommonManager.GetUserPrivilegesForEntity("DSA");
                ViewData["FormCall"] = "ApprovalQueue";
                ViewData["FormStatus"] = directSalesAgentDTO.ApprovalStatus;

                ViewBag.Branch = branch;

                return View("DSAAdd", directSalesAgentDTO);
            }
            else
                return RedirectToAction("ApprovalQueueList", "Approval");
        }
        #endregion
        #region DirectSalesAgentDelete
        [HttpPost]
        [AuthorizeUserAttribute(Entity = "DSA", AccessLevel = new UserActivities[] { UserActivities.Delete })]
        public JsonResult DirectSalesAgentDelete(string ADSACode, string AOrganizationCode)
        {
            DirectSalesAgentDTO directSalesAgentDTO = new DirectSalesAgentDTO();
            try
            {
                directSalesAgentDTO = tMasterManager.FindByDirectSalesAgent(ADSACode);
                directSalesAgentDTO.LastUpdatedBy = tCommonManager.GetLoggedUserName();
                directSalesAgentDTO.ApprovalStatus = Utils.GetApprovalStatusCode(Utils.ApprovalStatus.Deleted);
                tMasterManager.UpdateDirectSalesAgent(ADSACode, directSalesAgentDTO);

                var dResult = new { result = "success", action = "DSAList", controller = "Master" };
                TempData["Message"] = String.Format(Resources.MsgInfoDeleteSuccess, String.Concat(new object[] { Resources.DSA, " ", directSalesAgentDTO.DSACode }));
                return Json(dResult, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                var dResultFail = new { result = "fail", action = "DSAList", controller = "Master" };
                TempData["Message"] = String.Format(Resources.MsgErrWhileDelete, String.Concat(new object[] { Resources.DSA, " ", directSalesAgentDTO.DSACode }));
                return Json(dResultFail, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #endregion

        #region Adjustment Financial Account

        #region AdjustmentFinancialAccountView
        [AuthorizeUserAttribute(Entity = "MT_ADJUSTMENT_FINANCIAL_ACCOUNT", AccessLevel = new UserActivities[] { UserActivities.View })]
        public ActionResult AdjustmentFinancialAccountView(string Id)
        {
            try
            {
                AdjustmentFinancialAccountDTO adjustmentFinancialAccountDTO;
                ViewBag.TableName = "FNACNT";

                var ProductList = tLookUpManager.GetAll<ProductDTO>("", "Product");
                var branchCode = tLookUpManager.GetAll<BranchDTO>("", "Branch");
                var financialActivityCode = tLookUpManager.GetAll<TransactionSubCodeDTO>("", "TransCodeSubCode");

                ViewBag.Privileges = tCommonManager.GetUserPrivilegesForEntity("MT_ADJUSTMENT_FINANCIAL_ACCOUNT");

                ViewBag.ProductList = ProductList;
                ViewBag.Branch = branchCode;
                ViewBag.FinancialActivity = financialActivityCode;

                if (String.IsNullOrWhiteSpace(Id))
                {
                    adjustmentFinancialAccountDTO = new AdjustmentFinancialAccountDTO();
                    adjustmentFinancialAccountDTO.EffectiveDate = Utils.BusinessDate;
                    ViewData["FormStatus"] = adjustmentFinancialAccountDTO.ApprovalStatus;
                    return View("AdjustmentFinancialAccountAdd", adjustmentFinancialAccountDTO);
                }
                else
                {
                    adjustmentFinancialAccountDTO = tMasterManager.FindByAdjustmentFinancialAccountId(Convert.ToInt32(Id));
                    ViewBag.ReasonCodes = null;
                }

                ViewData["FormStatus"] = adjustmentFinancialAccountDTO.ApprovalStatus;

                return View("AdjustmentFinancialAccountAdd", adjustmentFinancialAccountDTO);
            }
            catch (Exception ex)
            {
                LoggerFactory.CreateLog().LogError("Error" + ex.Message, 100, 2);
                ViewBag.ErrorMessage = Resources.MsgErrServerException;
                ViewBag.ErrorType = SessionErrorType.ServerException;
                return View("Error");
            }
        }
        #endregion
        #region AdjustmentFinancialAccountList
        [AuthorizeUserAttribute(Entity = "MT_ADJUSTMENT_FINANCIAL_ACCOUNT", AccessLevel = new UserActivities[] { UserActivities.View })]
        public ActionResult AdjustmentFinancialAccountList(AdjustmentFinancialAccountSearch AVMAdjustmentFinancialAccountSearch, string ClearButton, int? page)
        {
            try
            {

                int iPageNumber = Convert.ToInt32((page == null) ? 0 : page);

                var ProductList = tLookUpManager.GetAll<ProductDTO>("", "Product");
                var branchCode = tLookUpManager.GetAll<BranchDTO>("", "Branch");
                var financialActivityCode = tLookUpManager.GetAll<TransactionSubCodeDTO>("", "TransCodeSubCode");

                if (ClearButton == Resources.Clear)
                {
                    AVMAdjustmentFinancialAccountSearch.BranchCode = null;
                    AVMAdjustmentFinancialAccountSearch.ModuleName = null;
                    AVMAdjustmentFinancialAccountSearch.ProductCode = null;
                    AVMAdjustmentFinancialAccountSearch.TransactionSubCodeId = 0;
                    ModelState.Clear();
                }

                if ((!string.IsNullOrEmpty(Utils.OrganizationCode)) || (!(string.IsNullOrEmpty(AVMAdjustmentFinancialAccountSearch.SearchButton)) &&
                     !((string.IsNullOrWhiteSpace(AVMAdjustmentFinancialAccountSearch.BranchCode)
                     && (string.IsNullOrWhiteSpace(AVMAdjustmentFinancialAccountSearch.ModuleName))
                     && (string.IsNullOrWhiteSpace(AVMAdjustmentFinancialAccountSearch.ProductCode))
                     && (string.IsNullOrWhiteSpace(Convert.ToString(AVMAdjustmentFinancialAccountSearch.TransactionSubCodeId)))
                     ))))
                {
                    var entities = tMasterManager.FindAll(iPageNumber, new AdjustmentFinancialAccountDTO()
                    {
                        BranchCode = AVMAdjustmentFinancialAccountSearch.BranchCode,
                        ModuleName = AVMAdjustmentFinancialAccountSearch.ModuleName,
                        ProductCode = AVMAdjustmentFinancialAccountSearch.ProductCode,
                        TransactionSubCodeId = AVMAdjustmentFinancialAccountSearch.TransactionSubCodeId
                    });
                    AVMAdjustmentFinancialAccountSearch.SearchResults = entities.AsQueryable();
                }
                else
                {
                    var currResult = tMasterManager.FindAdjustmentFinancialAccount(1, Utils.GRD_ROWS_PER_PAGE).AsQueryable();

                    AVMAdjustmentFinancialAccountSearch.SearchResults = currResult.AsQueryable();
                }

                ViewBag.ProductList = ProductList;
                ViewBag.Branch = branchCode;
                ViewBag.FinancialActivity = financialActivityCode;

                AVMAdjustmentFinancialAccountSearch.SearchResults = AVMAdjustmentFinancialAccountSearch.SearchResults.Where(x => x.ApprovalStatus != "DEL" && x.ApprovalStatus != "DIS");
                ViewBag.Privileges = tCommonManager.GetUserPrivilegesForEntity("MT_ADJUSTMENT_FINANCIAL_ACCOUNT");
                return View(AVMAdjustmentFinancialAccountSearch);
            }
            catch (Exception ex)
            {
                LoggerFactory.CreateLog().LogError("Error" + ex.Message, 100, 2);
                ViewBag.ErrorMessage = Resources.MsgErrServerException;
                ViewBag.ErrorType = SessionErrorType.ServerException;
                return View("Error");
            }
        }
        #endregion
        #region AdjustmentFinancialAccountAdd
        [AuthorizeUserAttribute(Entity = "MT_ADJUSTMENT_FINANCIAL_ACCOUNT", AccessLevel = new UserActivities[] { UserActivities.Add, UserActivities.Update, UserActivities.View })]
        public ActionResult AdjustmentFinancialAccountAdd(string Id)
        {
            try
            {
                AdjustmentFinancialAccountDTO adjustmentFinancialAccountDTO;

                var ProductList = tLookUpManager.GetAll<ProductDTO>("", "Product");
                var branchCode = tLookUpManager.GetAll<BranchDTO>("", "Branch");
                var financialActivityCode = tLookUpManager.GetAll<TransactionSubCodeDTO>("", "TransCodeSubCode");

                if (Id == null || Id == "0")
                {
                    adjustmentFinancialAccountDTO = new AdjustmentFinancialAccountDTO();
                    adjustmentFinancialAccountDTO.EffectiveDate = Utils.BusinessDate;
                }
                else
                    adjustmentFinancialAccountDTO = tMasterManager.FindByAdjustmentFinancialAccountId(Convert.ToInt32(Id));

                if (!String.IsNullOrWhiteSpace(adjustmentFinancialAccountDTO.ApprovalStatus) && adjustmentFinancialAccountDTO.ApprovalStatus.Equals("P"))
                {
                    var reasonCodes = tLookUpManager.GetAll<ApprovalContextDTO>("ADJFNACNT", "ReasonContext");
                    ViewBag.ReasonCodes = reasonCodes;
                }

                ViewBag.Privileges = tCommonManager.GetUserPrivilegesForEntity("MT_ADJUSTMENT_FINANCIAL_ACCOUNT");

                ViewData["FormStatus"] = adjustmentFinancialAccountDTO.ApprovalStatus;

                ViewBag.ProductList = ProductList;
                ViewBag.Branch = branchCode;
                ViewBag.FinancialActivity = financialActivityCode;

                return View(adjustmentFinancialAccountDTO);
            }
            catch (Exception ex)
            {
                LoggerFactory.CreateLog().LogError("Error" + ex.Message, 100, 2);
                ViewBag.ErrorMessage = Resources.MsgErrServerException;
                ViewBag.ErrorType = SessionErrorType.ServerException;
                return View("Error");
            }
        }
        #endregion
        #region AdjustmentFinancialAccountSave
        [HttpPost]
        [AuthorizeUserAttribute(Entity = "MT_ADJUSTMENT_FINANCIAL_ACCOUNT", AccessLevel = new UserActivities[] { UserActivities.Add, UserActivities.Update })]
        public ActionResult AdjustmentFinancialAccountSave(int Id, string Command, AdjustmentFinancialAccountDTO AAdjustmentFinancialAccountDTO)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    if (Command.Equals(Resources.Submit))
                        AAdjustmentFinancialAccountDTO.ApprovalStatus = "P";
                    else if (Command.Equals(Resources.Save))
                        AAdjustmentFinancialAccountDTO.ApprovalStatus = "D";

                    if (Command.Equals(Resources.Submit) || Command.Equals(Resources.Save))
                    {
                        if (tMasterManager.FindByAdjustmentFinancialAccountId(Id).TransactionSubCodeId == 0)
                        {
                            AAdjustmentFinancialAccountDTO.CreatedBy = tCommonManager.GetLoggedUserName();
                            AAdjustmentFinancialAccountDTO.LastUpdatedBy = tCommonManager.GetLoggedUserName();
                            AAdjustmentFinancialAccountDTO.OrganizationCode = Utils.OrganizationCode;

                            tMasterManager.SaveAdjustmentFinancialAccountDetails(AAdjustmentFinancialAccountDTO);
                            TempData["Message"] = String.Format(Resources.MsgInfoSaveSuccess, Resources.MT_ADJUSTMENT_FINANCIAL_ACCOUNT);
                        }
                        else
                        {
                            if (!string.IsNullOrWhiteSpace(Convert.ToString(Id)))
                            {
                                AAdjustmentFinancialAccountDTO.LastUpdatedBy = tCommonManager.GetLoggedUserName();
                                tMasterManager.UpdateAdjustmentFinancialAccountDetails(Id, AAdjustmentFinancialAccountDTO);
                                TempData["Message"] = String.Format(Resources.MsgInfoSaveSuccess, Resources.MT_ADJUSTMENT_FINANCIAL_ACCOUNT);
                            }
                            else
                            {
                                TempData["Message"] = String.Format(Resources.MsgErrAlreadyExists, Resources.MT_ADJUSTMENT_FINANCIAL_ACCOUNT);
                            }
                        }
                    }
                    else if (Command.Equals(Resources.Delete))
                    {
                        try
                        {
                            AAdjustmentFinancialAccountDTO.LastUpdatedBy = tCommonManager.GetLoggedUserName();
                            AAdjustmentFinancialAccountDTO.ApprovalStatus = Utils.GetApprovalStatusCode(Utils.ApprovalStatus.Deleted);
                            tMasterManager.UpdateAdjustmentFinancialAccountDetails(Id, AAdjustmentFinancialAccountDTO);

                            TempData["Message"] = String.Format(Resources.MsgInfoDeleteSuccess, Resources.MT_ADJUSTMENT_FINANCIAL_ACCOUNT);
                        }
                        catch (Exception ex)
                        {
                            LoggerFactory.CreateLog().LogError("Error" + ex.Message, 100, 2);
                            TempData["Message"] = String.Format(Resources.MsgErrDeleteConcurrencyError, Resources.MT_ADJUSTMENT_FINANCIAL_ACCOUNT);
                            return RedirectToAction("AdjustmentFinancialAccountAdd", "master", new { Id = Id });
                        }
                    }
                }
            }

            catch (Exception ex)
            {
                LoggerFactory.CreateLog().LogError("Error" + ex.Message, 100, 2);
                ViewBag.ErrorMessage = Resources.MsgErrServerException;
                ViewBag.ErrorType = SessionErrorType.ServerException;
                return View("Error");
            }

            return ((Command.Equals(Resources.Submit) || Command.Equals(Resources.Delete)) ? RedirectToAction("AdjustmentFinancialAccountAdd", "master") : RedirectToAction("AdjustmentFinancialAccountAdd", "master", new { Id = Id }));

        }
        #endregion
        #region AdjustmentFinancialAccountDelete
        [HttpPost]
        [AuthorizeUserAttribute(Entity = "MT_ADJUSTMENT_FINANCIAL_ACCOUNT", AccessLevel = new UserActivities[] { UserActivities.Delete })]
        public JsonResult AdjustmentFinancialAccountDelete(int Id)
        {
            AdjustmentFinancialAccountDTO AAdjustmentFinancialAccountDTO = new AdjustmentFinancialAccountDTO();
            try
            {
                AAdjustmentFinancialAccountDTO = tMasterManager.FindByAdjustmentFinancialAccountId(Id);
                AAdjustmentFinancialAccountDTO.LastUpdatedBy = tCommonManager.GetLoggedUserName();
                AAdjustmentFinancialAccountDTO.ApprovalStatus = Utils.GetApprovalStatusCode(Utils.ApprovalStatus.Deleted);
                tMasterManager.UpdateAdjustmentFinancialAccountDetails(Id, AAdjustmentFinancialAccountDTO);

                var dResult = new { result = "success", action = "AdjustmentFinancialAccountList", controller = "Master" };
                TempData["Message"] = String.Format(Resources.MsgInfoDeleteSuccess, Resources.MT_ADJUSTMENT_FINANCIAL_ACCOUNT);
                return Json(dResult, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                var dResultFail = new { result = "fail", action = "AdjustmentFinancialAccountList", controller = "Master" };
                TempData["Message"] = String.Format(Resources.MsgErrDeleteConcurrencyError, Resources.MT_ADJUSTMENT_FINANCIAL_ACCOUNT);
                return Json(dResultFail, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion
        #region AdjustmentFinancialAccountAuthorize
        [AuthorizeUserAttribute(Entity = "MT_ADJUSTMENT_FINANCIAL_ACCOUNT", AccessLevel = new UserActivities[] { UserActivities.Authorize })]
        public ActionResult AdjustmentFinancialAccountAuthorize(string Id, string AMode, string QId)
        {
            ViewBag.RequestQueueId = QId;
            ViewBag.TableName = "ADJFNACNT";

            var ProductList = tLookUpManager.GetAll<ProductDTO>("", "Product");
            var branchCode = tLookUpManager.GetAll<BranchDTO>("", "Branch");
            var financialActivityCode = tLookUpManager.GetAll<TransactionSubCodeDTO>("", "TransCodeSubCode");

            if (!String.IsNullOrWhiteSpace(Id))
            {
                AdjustmentFinancialAccountDTO adjustmentFinancialAccountDTO = tMasterManager.FindByAdjustmentFinancialAccountId(Convert.ToInt32(Id));

                if (!String.IsNullOrWhiteSpace(adjustmentFinancialAccountDTO.ApprovalStatus) && adjustmentFinancialAccountDTO.ApprovalStatus.Equals("P"))
                {
                    var reasonCodes = tLookUpManager.GetAll<ApprovalContextDTO>("ADJFNACNT", "ReasonContext");
                    ViewBag.ReasonCodes = reasonCodes;
                }
                ViewBag.Privileges = tCommonManager.GetUserPrivilegesForEntity("MT_ADJUSTMENT_FINANCIAL_ACCOUNT");

                ViewData["FormStatus"] = adjustmentFinancialAccountDTO.ApprovalStatus;
                ViewData["FormCall"] = "ApprovalQueue";
                ViewBag.ProductList = ProductList;
                ViewBag.Branch = branchCode;
                ViewBag.FinancialActivity = financialActivityCode;

                return View("AdjustmentFinancialAccountAdd", adjustmentFinancialAccountDTO);
            }
            else
                return RedirectToAction("ApprovalQueueList", "Approval");
        }
        #endregion
        #endregion

        #endregion

        #region Common Methods

        #region GetContractByProductCode
        public JsonResult GetContractByProductCode(string AProductCode)
        {
            List<SelectListItem> lstSelectedPTypes = new List<SelectListItem>();
            if (!string.IsNullOrWhiteSpace(AProductCode))
            {
                lstSelectedPTypes = tLookUpManager.GetAll<ProductMemberTypeDTO>(AProductCode, "ContractByProductCode");
            }

            return Json(lstSelectedPTypes, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region GetCityByRegionCode
        public JsonResult GetCityByRegionCode(string Id)
        {
            var cityList = tLookUpManager.GetAll<CommunaDTO>(Id, "CityByRegion");
            return Json(cityList, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region GetNextWorkingDate
        public ActionResult GetNextWorkingDate(ChangeBusinessDateDTO AChangeBusinessDateDTO)
        {
            DateTime NextWorkingDate = new DateTime();

            DateTime ACurrentDate = AChangeBusinessDateDTO.BusinessDate;

            try
            {
                NextWorkingDate = tMasterManager.GetNextWorkingDate(new DateTime(ACurrentDate.Year, ACurrentDate.Month, ACurrentDate.Day));
            }
            catch
            {

            }

            return Json(new { NextWorkingDate = NextWorkingDate }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region GetProductCurrency
        public JsonResult GetProductCurrency(string AProductCode)
        {
            var currencyCode = tMasterManager.GetProductCurrency(AProductCode);
            var precision = tMasterManager.GetCurrencyPrecision(currencyCode);
            return Json(new { currencyCode, precision }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region GetCurrencyPrecision
        public JsonResult GetCurrencyPrecision(string ACurrencyCode)
        {
            var precision = "0";
            if (!string.IsNullOrEmpty(ACurrencyCode))
            {
                precision = tMasterManager.GetCurrencyPrecision(ACurrencyCode);
            }
            
            return Json(new { precision }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region GetSalesIdByRUT
        public JsonResult GetSalesIdByRUT(string ARUT)
        {
            List<SelectListItem> lstSelectedSalesId = new List<SelectListItem>();
            if (!string.IsNullOrWhiteSpace(ARUT))
            {
                lstSelectedSalesId = tLookUpManager.GetAll<ProductMemberTypeDTO>(ARUT, "SalesIdByRUT");
            }

            return Json(lstSelectedSalesId, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region GetProductProperties
        public JsonResult GetProductProperties(string AProductCode)
        {
            ProductDTO objProductDTO = tMasterManager.FindByProductCode(AProductCode, Utils.OrganizationCode);           
            return Json(new { BillCycleSysParamId = objProductDTO.BillCycleSysParamId, BillingCycleCode = objProductDTO.BillingCycleCode, MaxCreditLimit = objProductDTO.MaximumCreditLine }, JsonRequestBehavior.AllowGet);
        } 
        #endregion

        #region SelectProductProperties
        public JsonResult SelectProductProperties(string AProductCode)
        {
            ProductDTO objProductDTO = tMasterManager.FindByProductCode(AProductCode, Utils.OrganizationCode);            
            return Json(new { BillCycleSysParamId = objProductDTO.BillCycleSysParamId, BillingCycleCode = objProductDTO.BillingCycleCode, MaxCreditLimit = objProductDTO.MaximumCreditLine }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region GetDSAforBranch

        public JsonResult GetDSAforBranch(string ABranchCode)
        {
            DirectSalesAgentDTO objDirectSalesAgentDTO = tMasterManager.FindDSAByCommercialBranchCode(ABranchCode);
            return Json(new { DSAName = objDirectSalesAgentDTO.DSAName, DSACode = objDirectSalesAgentDTO.DSACode, UEN = objDirectSalesAgentDTO.UEN, RUT = objDirectSalesAgentDTO.RUT }, JsonRequestBehavior.AllowGet);
        }

        #endregion
                
        #endregion
    }
}
