
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Web.Mvc;
using GemCryptor;
using System.Collections;
using System.Globalization;
using System.Reflection;
using CC_Ingenio_Resources;
using System.Text.RegularExpressions;
using System.Data;
using System.ComponentModel;


namespace CC_Ingenio.Common.Utils
{
    public static class Utils
    {

        #region constants
        public const string MSG_RECORD_SAVE_SUCESS = "{0} saved successfully.";       
        public const string MSG_RECORD_DELETE_SUCCESS = "{0} deleted successfully.";
        public const string MSG_RECORD_DELETE_CONCURRENCY_ERROR = "Cannot delete {0}. Record already in use.";
            

        public const string MSG_ALLOWED_CHARS = @"^[a-zA-Z0-9]+$";
        public const string MSG_ALLOWED_NUMERALS = @"^(([0-9]{1}\d{0,2},(\d{3},)*\d{3})|([0-9]{1}\d{0,}))$";
        public const string MSG_ALLOWED_NOSPACE = "{0:#.#}";
        public const string MSG_ALLOWED_PERCENTAGE = @"[0-9]+(\.\,[0-9][0-9]?)?";
     //   public const string MSG_RUT = "###0-*";
        public const string CNST_DEFAULT_CULTURE = "en-GB";
        public const string MSG_VALID_PHONENO = "\\d{10}";
        public const string MSG_VALID_EMAIL = "^[_a-z0-9-]+(\\.[_a-z0-9-]+)*@[a-z0-9-]+(\\.[a-z0-9-]+)*(\\.[a-z]{2,4})$";
        public const string CNST_CUST_DOCUMENT_PATH = "~/Reports/CustomerDocs/";
        public const string MSG_VALID_MERCHANTCODE = @"^([0-9]{5,9})*(\\-[kK0-9]{1,1})$";

        public const int GRD_ROWS_PER_PAGE = 15;

        public const string CNST_SEC_PRIVATE_KEY = "Cc@1ngE#Io";

       // public static string connString = ConfigurationManager.ConnectionStrings["CC_IngenioDbContext"].ConnectionString;

        public static string CNST_ADMIN = "ADMINISTRATOR";

        public static string OrganizationCode ;
        public static DateTime BusinessDate;
        public static string ShortOrganization;
        public static CultureInfo Culture;
        public static string OrganizationTypeCode;/*Retail - RET, Industrial - IND*/
        public static string CountryCode = "1";
        public static string UFCurrencyCode = "UF";
        public static decimal CurrentUFRate;
        public static string EMITransactionCode = "013";
        public static string EMIFirstBilling = "B1";
        public static string EMIInterestCalculation = "I2";
        public static string Purchase;
        public static string CreditNote;
        public static string Payments;


        public enum ApprovalStatus
        {
            Pending, Approved, Rejected, Draft, Deleted, Discarded
        }

        #endregion

        #region BuildConnectionString
        public static string BuildConnectionString()
        {
            StringBuilder sConnectionString = new StringBuilder();
            string str = CC_Ingenio.Common.Utils.Utils.Helper.EncryptPassword("P*4f$#hst07");

            sConnectionString.Append("Data Source = " + ConfigurationManager.AppSettings["Server"].ToString() + ";");
            sConnectionString.Append("Initial Catalog = " + ConfigurationManager.AppSettings["Database"].ToString() + ";");
            sConnectionString.Append("Integrated Security = " + ConfigurationManager.AppSettings["IntergratedSecurity"].ToString() + ";");
            sConnectionString.Append("User Id = " + ConfigurationManager.AppSettings["UserId"].ToString() + ";");
            sConnectionString.Append("Password = " + CC_Ingenio.Common.Utils.Utils.Helper.DecryptPassword(ConfigurationManager.AppSettings["Password"].ToString()) + ";");
            sConnectionString.Append("MultipleActiveResultSets = " + ConfigurationManager.AppSettings["MultipleActiveResultSets"].ToString() + ";");
            //sConnectionString.Append("providerName = " + ConfigurationManager.AppSettings["Provider"].ToString() + ";");
            return sConnectionString.ToString();
        }
        #endregion
        
        #region Sub Classes


        public class ExpressionList
        {
            public string FilterColumn { get; set; }
            public object FilterValue { get; set; }
            public string Operator { get; set; }
        }

        #region Helpers
        public static class Helper
        {
            public static string DecryptPassword(string AValue)
            {
                return TwoFishCrypto.Decrypt(AValue, CNST_SEC_PRIVATE_KEY);
            }

            public static string EncryptPassword(string AValue)
            {
                return TwoFishCrypto.Encrypt(AValue, CNST_SEC_PRIVATE_KEY);
            }

            public static string EncryptPassword1(string AValue)
            {
                return TwoFishCrypto.Encrypt(AValue, "Cc@1ngE#IoL@cEnCe");
            }
        }
        #endregion
        #endregion
        
        #region Static Methods

        #region GetExpression
        public static Expression<Func<TEntity, bool>> GetExpression<TEntity>(List<ExpressionList> AExpressionList)
        {
            Expression whereExpr = Expression.Constant(true);
            var parameter = Expression.Parameter((typeof(TEntity)), "Class");
            Expression exp;
            if (AExpressionList != null && AExpressionList.Count > 0)
            {

                foreach (ExpressionList item in AExpressionList)
                {
                    if (!(item.FilterValue == null))
                    {
                        
                        var filterColumn = Expression.Property(parameter, item.FilterColumn);
                        var filterValue = Expression.Constant(item.FilterValue);
                        var dataType = filterValue.Type;
                        

                        System.Reflection.MethodInfo method = null;
                        if (dataType == typeof(string))
                            method = dataType.GetMethod("Contains", new[] { typeof(string) });
                        else if (dataType == typeof(DateTime))
                            method = dataType.GetMethod("GreaterThanOrEqual", new[] { typeof(string) });
                        switch (item.Operator)
                        {
                            case "Contains":
                                method = typeof(string).GetMethod("Contains", new[] { typeof(string) });
                                exp = Expression.Call(filterColumn, method, filterValue);
                                break;
                            case "EndsWith":
                            case "StartsWith":

                                method = typeof(string).GetMethod(item.Operator.ToString(), new[] { typeof(string) });
                                exp = Expression.Call(filterColumn, method, filterValue);
                                break;
                            case "GreaterThan" :
                                exp = Expression.GreaterThan(filterColumn,  filterValue);
                                break;
                            case "GreaterThanOrEqualTo":
                                exp = Expression.GreaterThanOrEqual(filterColumn, filterValue);
                                break;
                            case "LessThan":
                                exp = Expression.LessThan(filterColumn, filterValue);
                                break;
                            case "LessThanOrEqualTo":
                                exp = Expression.LessThanOrEqual(filterColumn, filterValue);
                                break;
                            case "Equals" :
                                if (filterValue.Type != filterColumn.Type)
                                {
                                    var converted = filterValue.Type != filterColumn.Type
                                                ? (Expression)Expression.Convert(filterValue, filterColumn.Type)
                                                : (Expression)filterValue;

                                    exp = Expression.Equal(filterColumn, converted);
                                }
                                else 
                                {
                                    exp = Expression.Equal(filterColumn, filterValue);
                                }
                                break;

                            case "Equal":
                                if (filterValue.Type != filterColumn.Type)
                                {
                                    var converted = filterValue.Type != filterColumn.Type
                                                ? (Expression)Expression.Convert(filterValue, filterColumn.Type)
                                                : (Expression)filterValue;

                                    exp = Expression.Equal(filterColumn, converted);
                                }
                                else
                                {
                                    exp = Expression.Equal(Expression.Property(parameter, item.FilterColumn.ToLower()), Expression.Constant(item.FilterValue.ToString().ToLower()));
                                }
                                break;
                            case "NotEquals":
                                exp = Expression.NotEqual(filterColumn, filterValue);
                                break;
                            default:
                                throw new ArgumentException("Filter Type could not be determined");
        
                        }
                        //var call = Expression.Call(filterColumn, method, filterValue);
                        whereExpr = Expression.And(whereExpr, exp);
                    }
                }
            }
            return Expression.Lambda<Func<TEntity, bool>>(whereExpr, parameter);

        } 
        #endregion

        #region GetGender
        public static List<SelectListItem> GetGender()
        {
            List<SelectListItem> genderList = new List<SelectListItem>()
            {
                new SelectListItem(){ Text = Resources.SelectOne, Value = string.Empty },
                new SelectListItem(){ Text = "Male", Value = "M" },
                new SelectListItem(){ Text = "Female", Value = "F" }

            };

            return genderList;
        } 
        #endregion

        #region GetMaritalStatus
        public static List<SelectListItem> GetMaritalStatus()
        {
            List<SelectListItem> maritalStatusList = new List<SelectListItem>()
            {
                new SelectListItem(){ Text = Resources.SelectOne, Value = string.Empty },
                new SelectListItem(){ Text = "Single", Value = "S" },
                new SelectListItem(){ Text = "Married", Value = "M" },
                new SelectListItem(){ Text = "Living Together", Value = "L" },
                new SelectListItem(){ Text = "Separated", Value = "SP"},
                new SelectListItem(){ Text = "Widowed", Value = "W" }
            };

            return maritalStatusList;
        }
        #endregion

        #region GetEducation
        public static List<SelectListItem> GetEducation()
        {
            List<SelectListItem> educationList = new List<SelectListItem>()
            {
                new SelectListItem(){ Text = Resources.SelectOne, Value = string.Empty },
                new SelectListItem(){ Text = "Under Graduate", Value = "E1" },
                new SelectListItem(){ Text = "Graduate", Value = "E2" },
                new SelectListItem(){ Text = "Post Graduate", Value = "E3" },
                new SelectListItem(){ Text = "Doctorate", Value = "E4" }
            };

            return educationList;
        }
        #endregion

        #region GetProfession
        public static List<SelectListItem> GetProfession()
        {
            List<SelectListItem> professionList = new List<SelectListItem>()
            {
                new SelectListItem(){ Text =  Resources.SelectOne, Value = string.Empty },
                new SelectListItem(){ Text = "Engineer", Value = "P1" },
                new SelectListItem(){ Text = "Acadamics", Value = "P2"},
                new SelectListItem(){ Text = "Scientist", Value = "P3"},
                new SelectListItem(){ Text = "Banker", Value = "P4"},
                new SelectListItem(){ Text = "Marketing", Value = "P5"},
                new SelectListItem(){ Text = "HR", Value = "P6"},
                new SelectListItem(){ Text = "Author", Value = "P7"}
            };

            return professionList;
        }
        #endregion

        #region GetHousing
        public static List<SelectListItem> GetHousing()
        {
            List<SelectListItem> housingList = new List<SelectListItem>()
            {
                new SelectListItem(){ Text =  Resources.SelectOne, Value = string.Empty },
                new SelectListItem(){ Text = "Housing1", Value = "H1" },
                new SelectListItem(){ Text = "Housing2", Value = "H2"}
            };

            return housingList;
        }
        #endregion

        #region GetCalenderType
        public static List<SelectListItem> GetCalenderType()
        {
            List<SelectListItem> typeList = new List<SelectListItem>()
            {
                new SelectListItem(){ Text =  Resources.SelectOne, Value = string.Empty },
                new SelectListItem(){ Text = "HOLIDAY", Value = "HOLIDAY" },
                new SelectListItem(){ Text = "WORKING DAY", Value = "WORKINGDAY"}
            };

            return typeList;
        }
        #endregion

        #region GetDebitOrCredit
        public static List<SelectListItem> GetDebitOrCredit()
        {
            List<SelectListItem> typeList = new List<SelectListItem>()
            {
                new SelectListItem(){ Text =  Resources.SelectOne, Value = string.Empty },
                new SelectListItem(){ Text = "Debit", Value = "D" },
                new SelectListItem(){ Text = "Credit", Value = "C"}
            };

            return typeList;
        }
        #endregion

        #region GetCustomerType
        public static List<SelectListItem> GetCustomerType()
        {
            List<SelectListItem> typeList = new List<SelectListItem>()
            {
                new SelectListItem(){ Text =  Resources.SelectOne, Value = string.Empty },
                new SelectListItem(){ Text = "Company", Value = "C" },
                new SelectListItem(){ Text = "Individual", Value = "I"}
            };
           
            return typeList;
        }
        #endregion

        #region GetWorkType
        public static List<SelectListItem> GetWorkType()
        {
            List<SelectListItem> typeList = new List<SelectListItem>()
            {
                new SelectListItem(){ Text =  Resources.SelectOne, Value = string.Empty },
                new SelectListItem(){ Text = "Salaried", Value = "S" },
                new SelectListItem(){ Text = "Independent", Value = "I"}
            };

            return typeList;
        }
        #endregion

        #region GetAccountStatus
        public static List<SelectListItem> GetAccountStatus()
        {
            List<SelectListItem> accountStatusList = new List<SelectListItem>()
            {
                new SelectListItem(){ Text =  Resources.SelectOne, Value = string.Empty },
                new SelectListItem(){ Text = "Joint", Value = "J" },
                new SelectListItem(){ Text = "Individual", Value = "I"}
            };

            return accountStatusList;
        }
        #endregion

        #region GetAuthCode
        public static List<SelectListItem> GetAuthCode()
        {
            List<SelectListItem> authCodeList = new List<SelectListItem>()
            {
                new SelectListItem(){ Text =  Resources.SelectOne, Value = string.Empty },
                new SelectListItem(){ Text = "Code1", Value = "C1" },
                new SelectListItem(){ Text = "Code2", Value = "C2"}
            };

            return authCodeList;
        }
        #endregion

        #region GetSegmentCode
        public static List<SelectListItem> GetSegmentCode()
        {
            List<SelectListItem> segmentCodeList = new List<SelectListItem>()
            {
                new SelectListItem(){ Text =  Resources.SelectOne, Value = string.Empty },
                new SelectListItem(){ Text = "Segment1", Value = "S1" },
                new SelectListItem(){ Text = "Segment2", Value = "S2"}
            };

            return segmentCodeList;
        }
        #endregion

        #region GetResponseCode
        public static List<SelectListItem> GetResponseCode()
        {
            List<SelectListItem> responseCodeList = new List<SelectListItem>()
            {
                new SelectListItem(){ Text =  Resources.SelectOne, Value = string.Empty },
                new SelectListItem(){ Text = "Response1", Value = "R1" },
                new SelectListItem(){ Text = "Response2", Value = "R2"}
            };

            return responseCodeList;
        }
        #endregion

        #region GetMerchantCode
        public static List<SelectListItem> GetMerchantCode()
        {
            List<SelectListItem> merCodeList = new List<SelectListItem>()
            {
                
                new SelectListItem(){ Value = "MER001", Text = "Merchant 001" },
                new SelectListItem(){ Value = "MER002", Text = "Merchant 002"}
            };

            return merCodeList;
        }
        #endregion

        #region GetMCCCode
        public static List<SelectListItem> GetMCCCode()
        {
            List<SelectListItem> mccCodeList = new List<SelectListItem>()
            {
                
                new SelectListItem(){ Value = "MCC001", Text = "Merchant Category 001" },
                new SelectListItem(){ Value = "MCC002", Text = "Merchant Category 002"},
                new SelectListItem(){ Value = "MCC003", Text = "Merchant Category 003"}
            };

            return mccCodeList;
        }

        public static List<CodeValue> GetListMCCCode()
        {
            List<CodeValue> mccCodeList = new List<CodeValue>()
            {
                
                new CodeValue(){ Value = "MCC001", Text = "Merchant Category 001" },
                new CodeValue(){ Value = "MCC002", Text = "Merchant Category 002"},
                new CodeValue(){ Value = "MCC003", Text = "Merchant Category 003"}
            };

            return mccCodeList;
        }

        #region GetMerchantCode
        public static List<CodeValue> GetListMerCode()
        {
            List<CodeValue> merCodeList = new List<CodeValue>()
            {                
                new CodeValue(){ Value = "MER001", Text = "Merchant 001" },
                new CodeValue(){ Value = "MER002", Text = "Merchant 002"}
            };

            return merCodeList;
        }
        #endregion
        #endregion

        #region GetAuthStatus
        public static List<SelectListItem> GetAuthStatus()
        {
            List<SelectListItem> authStatusList = new List<SelectListItem>()
            {
                new SelectListItem(){ Text =  Resources.SelectOne, Value = string.Empty },
                new SelectListItem(){ Text = "Posted", Value = "P" },
                new SelectListItem(){ Text = "Authorized", Value = "A"}
            };

            return authStatusList;
        }
        #endregion

        #region GetCustomerStatus
        public static List<SelectListItem> GetCustomerStatus()
        {
            List<SelectListItem> custStatusList = new List<SelectListItem>()
            {
                new SelectListItem(){ Text =  Resources.SelectOne, Value = string.Empty },
                new SelectListItem(){ Text = Resources.StatusPendingApproval, Value = "P" },
                new SelectListItem(){ Text = Resources.StatusApproved, Value = "A"},              
                new SelectListItem(){ Text = Resources.StatusDraft, Value = "D"}              
            };

            return custStatusList;
        }
        #endregion

        #region GetCustomerStatus

        #endregion

        #region GetRecordStatus
        public static string GetRecordStatus(string AStatusCode)
        {
            string sReturnValue = "";
            switch (AStatusCode)
            { 
                case "P":
                    sReturnValue = Resources.StatusPendingApproval;
                        break;

                case "A":
                        sReturnValue = Resources.StatusApproved;
                        break;

                case "R":
                        sReturnValue = Resources.StatusRejected;
                        break;

                case "D":
                        sReturnValue = Resources.StatusDraft;
                        break;

                case "DEL":
                        sReturnValue = Resources.StatusDeleted;
                        break;

                case "DIS":
                        sReturnValue = Resources.StatusDiscarded;
                        break;
                case "UR":
                        sReturnValue = Resources.StatusRejected;
                        break;
                case "EXP":
                        sReturnValue = Resources.StatusExpired;
                        break;

            }
            return sReturnValue;
        }
        #endregion

        #region GetApprovalStatusCode
        public static string GetApprovalStatusCode(ApprovalStatus AStatusCode)
        {
            string sReturnValue = "";
            switch (AStatusCode)
            {
                case ApprovalStatus.Pending:
                    sReturnValue = "P";
                    break;

                case ApprovalStatus.Approved:
                    sReturnValue = "A";
                    break;

                case ApprovalStatus.Rejected:
                    sReturnValue = "R";
                    break;

                case ApprovalStatus.Draft:
                    sReturnValue = "D";
                    break;

                case ApprovalStatus.Deleted:
                    sReturnValue = "DEL";
                    break;

                case ApprovalStatus.Discarded:
                    sReturnValue = "DIS";
                    break;

            }
            return sReturnValue;
        }
        #endregion

        #region GetBillCycleCode
        public static List<SelectListItem> GetBillCycleCode()
        {
            List<SelectListItem> billCycleList = new List<SelectListItem>()
            {
                new SelectListItem(){ Text =  Resources.SelectOne, Value = string.Empty },
                new SelectListItem(){ Text = "Cycle1", Value = "C1" },
                new SelectListItem(){ Text = "Cycle2", Value = "C2"}
            };

            return billCycleList;
        }
        #endregion

        #region GetCommercialHouseConsumptionType
        public static List<SelectListItem> GetCommercialHouseConsumptionType()
        {
            List<SelectListItem> typeList = new List<SelectListItem>()
            {
                new SelectListItem(){ Text =  Resources.SelectOne, Value = string.Empty},
                new SelectListItem(){ Text = "Casa commercial" , Value = "CC"},
                new SelectListItem(){ Text = "Division De Consumo", Value="DDC"},
                new SelectListItem(){ Text = "Customer House Consumptio", Value="CHC"}
            };

            return typeList;
        }
        #endregion

        #region GetUnemployment
        public static List<SelectListItem> GetUnemployment()
        {
            List<SelectListItem> employmentList = new List<SelectListItem>()
            {
                new SelectListItem(){ Text =  Resources.SelectOne, Value = string.Empty},
                new SelectListItem(){ Text = "Yes" , Value = "Y"},
                new SelectListItem(){ Text = "No", Value="N"}
            };

            return employmentList;
        }
        #endregion

        #region GetBankType
        public static List<SelectListItem> GetBankType()
        {
            List<SelectListItem> bankTypeList = new List<SelectListItem>()
            {
                new SelectListItem(){ Text =  Resources.SelectOne, Value = string.Empty},
                new SelectListItem(){ Text = "Bank Type1" , Value = "BT1"},
                new SelectListItem(){ Text = "Bank Type2", Value="BT2"},
                new SelectListItem(){ Text = "Bank Type3", Value="BT3"}
            };

            return bankTypeList;
        }
        #endregion

        #region GetCardBrand
        public static List<SelectListItem> GetCardBrand()
        {
            List<SelectListItem> cardBrandList = new List<SelectListItem>()
            {
                new SelectListItem(){ Text =  Resources.SelectOne, Value = string.Empty},
                new SelectListItem(){ Text = "Card brand1" , Value = "CB1"},
                new SelectListItem(){ Text = "Card brand2", Value = "CB2"},
                new SelectListItem(){ Text = "Card brand3", Value = "CB3"}
            };

            return cardBrandList;
        }
        #endregion

        #region GetCustomerAddressType
        public static List<SelectListItem> GetCustomerAddressType()
        {
            List<SelectListItem> addressTypeList = new List<SelectListItem>()
            {
                new SelectListItem(){ Text =  Resources.SelectOne, Value = string.Empty},
                new SelectListItem(){ Text = "Particular" , Value = "PR"},
                //new SelectListItem(){ Text = "Office", Value = "O"},
                 new SelectListItem(){Text ="Commercial", Value="C"},
                 new SelectListItem(){Text ="Statement Address", Value="ST"}
               
                //new SelectListItem(){Text = "Particular", Value = "PR"}               
            };

            return addressTypeList;
        }
        #endregion

        #region GetCustomerDecisionReason
        public static List<SelectListItem> GetCustomerDecisionReason()
        {
            List<SelectListItem> decisionReasonList = new List<SelectListItem>()
            {
                new SelectListItem(){ Text =  Resources.SelectOne, Value = string.Empty},
                new SelectListItem(){ Text = "Reason1" , Value = "RSN1"},
                new SelectListItem(){ Text = "Reason2", Value = "RSN2"}
            };

            return decisionReasonList;
        }
        #endregion

        #region GetResidenceClearanceType
        public static List<SelectListItem> GetResidenceClearanceType()
        {
            List<SelectListItem> residenceClearanceTypeList = new List<SelectListItem>()
            {
                new SelectListItem(){ Text =  Resources.SelectOne, Value = string.Empty},
                new SelectListItem(){ Text = "Rent House" , Value = "TP1"},
                new SelectListItem(){ Text = "Own House", Value = "TP2"}
               
            };

            return residenceClearanceTypeList;
        }
        #endregion

        #region GetMonths
        public static List<SelectListItem> GetMonths()
        {
            List<SelectListItem> monthsList = new List<SelectListItem>()
            {
                new SelectListItem(){ Text =  Resources.SelectOne, Value = string.Empty},
                new SelectListItem(){ Text = "January" , Value = "1"},
                new SelectListItem(){ Text = "February", Value = "2"},
                new SelectListItem(){ Text = "March", Value = "3"},
                new SelectListItem(){ Text = "April", Value = "4"},
                new SelectListItem(){ Text = "May", Value = "5"},
                new SelectListItem(){ Text = "June", Value = "6"},
                new SelectListItem(){ Text = "July", Value = "7"},
                new SelectListItem(){ Text = "August", Value = "8"},
                new SelectListItem(){ Text = "September", Value = "9"},
                new SelectListItem(){ Text = "October", Value = "10"},
                new SelectListItem(){ Text = "November", Value = "11"},
                new SelectListItem(){ Text = "December", Value = "12"}
            };

            return monthsList;
        }
        #endregion
        
        #region GetYearList
        public static List<SelectListItem> GetYearList()
        {
            List<SelectListItem> yearList = new List<SelectListItem>();

            yearList.Add(new SelectListItem() { Text = Resources.SelectOne, Value = string.Empty });

            int yearFrom = DateTime.Now.Year - 5;
            int yearTo = DateTime.Now.Year + 5;

            for (int intYear = yearFrom; intYear <= yearTo; intYear++)
            {
                yearList.Add(new SelectListItem() { Text = "" + intYear + "", Value = "" + intYear + "" });
            }

            return yearList;
        }
        #endregion
        
        #region GetIncomeType
        public static List<SelectListItem> GetIncomeType()
        {
            List<SelectListItem> incomeTypeList = new List<SelectListItem>()
            {
                new SelectListItem(){ Text =  Resources.SelectOne, Value = string.Empty},
                new SelectListItem(){ Text = "Income Type 1" , Value = "I1"},
                new SelectListItem(){ Text = "Income Type 2", Value = "I2"},
                new SelectListItem(){ Text = "Income Type 3", Value = "I3"}
            };
            return incomeTypeList;
        }
        #endregion

        #region GetInterestDefinition
        public static List<SelectListItem> GetInterestDefinition()
        {
            List<SelectListItem> interestDefinition = new List<SelectListItem>()
            {
                new SelectListItem(){ Text = Resources.SelectOne, Value = string.Empty},
                new SelectListItem(){ Text = "Revolving" , Value = "R"},
                new SelectListItem(){ Text = "Delinquent", Value = "D"},
            };
            return interestDefinition;
        }
        #endregion

        #region GetInsuranceType
        public static List<SelectListItem> GetInsuranceType()
        {
            List<SelectListItem> insuranceTypeList = new List<SelectListItem>()
            {
                new SelectListItem(){ Text = Resources.SelectOne, Value = string.Empty},
                new SelectListItem(){ Text = "Insurance Type1" , Value = "TYP1"},
                new SelectListItem(){ Text = "Insurance Type2", Value = "TYP2"},
            };
            return insuranceTypeList;
        }
        #endregion

        #region GetInsuranceOption
        public static List<SelectListItem> GetInsuranceOption()
        {
            List<SelectListItem> insuranceOptionList = new List<SelectListItem>()
            {
                new SelectListItem(){ Text = Resources.SelectOne, Value = string.Empty},
                new SelectListItem(){ Text = "Fixed Amount" , Value = "AMT"},
                new SelectListItem(){ Text = "Rate", Value = "RATE"},
            };
            return insuranceOptionList;
        }
        #endregion

        #region GetPaymentAllocationType
        public static List<SelectListItem> GetPaymentAllocationType()
        {
            List<SelectListItem> paymentAllocationTypeList = new List<SelectListItem>()
            {
                new SelectListItem(){ Text = Resources.SelectOne, Value = string.Empty},
                new SelectListItem(){ Text = "Card No" , Value = "CN"},
                new SelectListItem(){ Text = "RUT No", Value = "RN"},
            };
            return paymentAllocationTypeList;
        }
        #endregion

        #region GetPaymentAllocationSequence
        public static List<SelectListItem> GetPaymentAllocationSequence()
        {
            List<SelectListItem> paymentAllocationSequenceList = new List<SelectListItem>()
            {
                new SelectListItem(){ Text = "O" , Value = "O"},
                new SelectListItem(){ Text = "I", Value = "I"},
                new SelectListItem(){ Text = "C", Value = "C"},
                new SelectListItem(){ Text = "P", Value = "P"},
                new SelectListItem(){ Text = "E", Value = "E"},
                new SelectListItem(){ Text = "L1", Value = "L1"},
                new SelectListItem(){ Text = "L2", Value = "L2"},
            };
            return paymentAllocationSequenceList;
        }
        #endregion

        #region GetCampaignSource
        public static List<SelectListItem> GetCampaignSource()
        {
            List<SelectListItem> campaignSourceList = new List<SelectListItem>()
            {
                new SelectListItem(){ Text = Resources.SelectOne, Value = string.Empty},
                new SelectListItem(){ Text = "Campaign Source1" , Value = "CS1"},
                new SelectListItem(){ Text = "Campaign Source2", Value = "CS2"},
                new SelectListItem(){ Text = "Campaign Source3", Value = "CS3"},
            };
            return campaignSourceList;
        }
        #endregion

        #region GetGeneralSpecial
        public static List<SelectListItem> GetGeneralSpecial()
        {
            List<SelectListItem> generalSpecialList = new List<SelectListItem>()
            {
                new SelectListItem(){ Text = "General" , Value = "G"},
                new SelectListItem(){ Text = "Special", Value = "S"}
            };
            return generalSpecialList;
        }
        #endregion

        #region GetCreditSubSegment
        public static List<SelectListItem> GetCreditSubSegment()
        {
            List<SelectListItem> typeList = new List<SelectListItem>()
            {
                new SelectListItem(){ Text = Resources.SelectOne, Value = string.Empty },
                new SelectListItem(){ Text = "Principal", Value = "P" },
                new SelectListItem(){ Text = "Interest", Value = "I"},
                new SelectListItem(){ Text = "Other Charge", Value = "O"},
                new SelectListItem(){ Text = "Commission", Value = "C"},
                new SelectListItem(){ Text = "Tax", Value = "T"}
            };

            return typeList;
        }
        #endregion

        #region GetRevolverChargeFlag
        public static List<SelectListItem> GetRevolverChargeFlag()
        {
            List<SelectListItem> typeList = new List<SelectListItem>()
            {
                new SelectListItem(){ Text = Resources.SelectOne, Value = string.Empty },
                new SelectListItem(){ Text = "Revolver", Value = "R" },
                new SelectListItem(){ Text = "Charge", Value = "C"}
            };

            return typeList;
        }
        #endregion

        #region GetNotepadEntryMessageType
        public static List<SelectListItem> GetNotepadEntryMessageType()
        {
            List<SelectListItem> messageType = new List<SelectListItem>()
            {
                new SelectListItem(){ Text = Resources.SelectOne, Value = string.Empty },
                new SelectListItem(){ Text = "Address", Value = "ADDR" },
                new SelectListItem(){ Text = "Other", Value = "OTHR"}
            };

            return messageType;
        }
        #endregion

        #region GetLoginStatus
        public static List<SelectListItem> GetLoginStatus()
        {
            List<SelectListItem> loginStatusList = new List<SelectListItem>()
            {
                new SelectListItem(){ Text = string.Empty, Value = string.Empty },
                new SelectListItem(){ Text = "Successful", Value = "S" },
                new SelectListItem(){ Text = "Failed", Value = "F" }
            };

            return loginStatusList;
        }
        #endregion

        #region ValidateUserPrivilege
        public static bool ValidateUserPrivilege(Object AObject)
        {
            return true;
        } 
        #endregion

        #region GetCustomerDependentAgeGroup
        public static List<SelectListItem> GetCustomerDependentAgeGroup()
        {
             List<SelectListItem> messageType = new List<SelectListItem>()
            {
                new SelectListItem(){ Text = Resources.SelectOne, Value = string.Empty },
                new SelectListItem(){ Text = "1-10", Value = "1" },
                new SelectListItem(){ Text = "11-15", Value = "2"},
                new SelectListItem(){ Text = "16-20", Value = "3"},
                new SelectListItem(){ Text = "21-25", Value = "4"},
                new SelectListItem(){ Text = "26-30", Value = "5"},
                new SelectListItem(){ Text = "31-35", Value = "6" },
                new SelectListItem(){ Text = "36-40", Value = "7"},
                new SelectListItem(){ Text = "41-45", Value = "8"},
                new SelectListItem(){ Text = "46-50", Value = "9"},
                new SelectListItem(){ Text = "51-60", Value = "10"}
            };

            return messageType;
        } 
        #endregion

        #region GetProcedureNames
        public static List<SelectListItem> GetProcedureNames()
        {
            List<SelectListItem> ProcedureList = new List<SelectListItem>()
            {
                new SelectListItem(){ Text = Resources.SelectOne, Value = string.Empty},
                //new SelectListItem(){ Text = "Transaction Pulling" , Value = "TP"},
                new SelectListItem(){ Text = "EOD", Value = "BL"},
                //new SelectListItem(){ Text = "Clear Test Data", Value = "CL"}
            };

            return ProcedureList;
        }
        #endregion

        #region GetResidenceType
        public static List<SelectListItem> GetResidenceType()
        {
            List<SelectListItem> ResidenceTypeList = new List<SelectListItem>()
            {
                new SelectListItem(){ Text = Resources.SelectOne, Value = string.Empty},
                new SelectListItem(){ Text = "Own" , Value = "Own"},
                new SelectListItem(){ Text = "Rent", Value = "Rent"}
            };

            return ResidenceTypeList;
        } 
        #endregion

        #region GetReportTypes
        public static List<SelectListItem> GetReportTypes()
        {
            List<SelectListItem> reportTypesList = new List<SelectListItem>()
            {
                new SelectListItem(){ Text = Resources.SelectOne, Value = string.Empty},
                new SelectListItem(){ Text = "Accepted Transactions", Value = "AC"},
                new SelectListItem(){ Text = "Rejected Transactions", Value = "RJ"},
                new SelectListItem(){ Text = "All Transactions", Value = "AL"},
                new SelectListItem(){ Text = "UnPosted Transactions", Value = "UN"},
                new SelectListItem(){ Text = "Control Report", Value = "CR"},
                new SelectListItem(){ Text = "Daily Balance - Normal" , Value = "DBN"},
                new SelectListItem(){ Text = "Daily Balance - OverDue" , Value = "DBO"},
                new SelectListItem(){ Text = "Daily Balance - WriteOff" , Value = "DBW"},
                new SelectListItem(){ Text = "Subsidiary Report - Normal" , Value = "SRN"},
                 new SelectListItem(){ Text = "Subsidiary Report - OverDue" , Value = "SRO"},
                  new SelectListItem(){ Text = "Subsidiary Report - WriteOff" , Value = "SRW"},
                 new SelectListItem(){ Text = "Merchant Settlement Report" , Value = "MR"},
                 new SelectListItem(){ Text = "GL Transaction Report - Normal" , Value = "GLN"},
                 new SelectListItem(){ Text = "GL Transaction Report - OverDue" , Value = "GLO"},
                 new SelectListItem(){ Text = "GL Transaction Report - WriteOff" , Value = "GLW"},
            };
            return reportTypesList;
        }
        #endregion

        #region GetApprovalStatusCode
        public static string GetApprovalStatusCode(string p)
        {
            throw new NotImplementedException();
        }
        #endregion

        #endregion

        #region Common Methods

        #region CheckRangeOverlaps
        public static bool CheckRangeOverlaps(decimal AFromFirst, decimal AToFirst, decimal AFromSecond, decimal AToSecond)
        {
            if (AFromFirst <= AToSecond && AFromSecond < AToFirst)
            {
                return true;
            }
            return false;
        }
        #endregion

        #region SplitIntoMultipleTables
        public static List<DataTable> SplitIntoMultipleTables(DataTable _splitTable, int _tableCount)
        {
            List<DataTable> tables = new List<DataTable>();

            int _rows = _splitTable.Rows.Count;

            DataTable copyTable = null;
            int cnt = 0;
            int _tCnt = 0;
            for (int i = 0; i < _tableCount; i++)
            {
                _tCnt = (_rows - cnt) / (_tableCount - i);
                copyTable = new DataTable();
                copyTable = (DataTable)_splitTable.AsEnumerable().Skip(cnt).Take(_tCnt).CopyToDataTable();
                tables.Add(copyTable);
                cnt += copyTable.Rows.Count;
            }

            return tables;
        }
        #endregion


        public static DataTable ToDataTable<T>(this IList<T> data)
        {
            PropertyDescriptorCollection properties =
                TypeDescriptor.GetProperties(typeof(T));
            DataTable table = new DataTable();
            foreach (PropertyDescriptor prop in properties)
                table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
            foreach (T item in data)
            {
                DataRow row = table.NewRow();
                foreach (PropertyDescriptor prop in properties)
                    row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                table.Rows.Add(row);
            }
            return table;
        }

        #endregion

    }

    #region Force Authorization(Static Page)



    public class Transaction
    {
        public bool IsCheck { get;set;}
        [Required]
        public string AccountNo { get; set; }
        [Required]
        [Display(Name = "Card No")]
        public string CardNo { get; set; }       
        [Required]
        [Display(Name = "Txn Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime TxnDate { get; set; }
        [Required]
        [Display(Name = "Txn Time")]
        public string TxnTime { get; set; }
        [Required]
        public decimal Amount { get; set; }
        [Required]
        [Display(Name = "Response   Code")]
        public string ResponseCode { get; set; }
        [Required]
        [Display(Name = "Merchant Code")]
        public string MerchantCode { get; set; }
        [Required]
        [Display(Name = "MCC Code")]
        public string MCCCode { get; set; }
        [Required]
        [Display(Name = "Auth Code")]
        public string AuthCode { get; set; }
        [Required]
        [Display(Name = "Segment Code")]
        public string SegmentCode { get; set; }
        [Required]
        public bool Post { get; set; }

        public string BatchNo { get; set; }
        public string TransactionNo { get; set; }
       

        public List<Transaction> TransList { get; set; }
    
    }

    public class CodeValue
    {
      
        public string Text { get; set; }      
        public string Value { get; set; }

   

        public List<CodeValue> TransList { get; set; }

    }


    public class TransactionEntrySearch
    {
        
        public string BatchNo { get; set; }
        public string AccountNo { get; set; }
        public string CardNo { get; set; }
        public System.Nullable<DateTime> TxnDate { get; set; }
        public System.Nullable<DateTime> TxnToDate { get; set; }
        public IQueryable<Transaction> SearchResults { get; set; }
        public string SearchButton { get; set; }
        public string AuthCode { get; set; }
        [Required(ErrorMessage="Please select a type")]
        public string Type { get; set; }
        //public bool Post { get; set; }
    }

    public class BillGeneration
    {
        public DateTime BillDate { get; set; }
        public string BillCycleCode { get; set; }
    }

    public class Statement
    {
        public List<Transaction> Transactions { get; set; }
    }

    #endregion

    #region R&D Custom Validator ------ Not Working
    [AttributeUsageAttribute(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = true)]
    public class AmountCompareAttribute : ValidationAttribute
    {
        private readonly int _belowLimit = 0;
        public AmountCompareAttribute(int belowLimit)
        {
            _belowLimit = belowLimit;
        }
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null)
            {
                int data = Convert.ToInt32(value);
                if(data < _belowLimit)
                {
                    var errorMessage = FormatErrorMessage(validationContext.DisplayName);
                    return new ValidationResult(ErrorMessage);
                }
            }
            return ValidationResult.Success;
        }
    }

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Field, Inherited = true, AllowMultiple = true)]
    public sealed class NotEqualToAttribute : ValidationAttribute
    {
        private const string DefaultErrorMessage = "{0} cannot be the same as {1}.";

        public string OtherProperty { get; private set; }

        public NotEqualToAttribute(string otherProperty)
            : base(DefaultErrorMessage)
        {
            if (string.IsNullOrEmpty(otherProperty))
            {
                throw new ArgumentNullException("otherProperty");
            }

            OtherProperty = otherProperty;
        }

        public override string FormatErrorMessage(string name)
        {
            return string.Format(ErrorMessageString, name, OtherProperty);
        }

        protected override ValidationResult IsValid(object value,
                              ValidationContext validationContext)
        {
            if (value != null)
            {
                var otherProperty = validationContext.ObjectInstance.GetType()
                                   .GetProperty(OtherProperty);

                var otherPropertyValue = otherProperty
                              .GetValue(validationContext.ObjectInstance, null);

                if (value.Equals(otherPropertyValue))
                {
                    return new ValidationResult(
                      FormatErrorMessage(validationContext.DisplayName));
                }
            }

            return ValidationResult.Success;
        }

  
    }

    #endregion

    #region Globalization
    public class DateTimeBinder : IModelBinder
    {
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var value = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
            var date = value.ConvertTo(typeof(DateTime), CultureInfo.CurrentCulture);

            return date;
        }
    }
    public class DecimalModelBinder : IModelBinder
    {
        public object BindModel(ControllerContext controllerContext,
            ModelBindingContext bindingContext)
        {
            ValueProviderResult valueResult = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
            ModelState modelState = new ModelState { Value = valueResult };
            object actualValue = null;
            try
            {
                actualValue = Convert.ToDecimal(valueResult.AttemptedValue,CultureInfo.CurrentCulture);
            }
            catch (FormatException e)
            {
                modelState.Errors.Add(e);
            }

            bindingContext.ModelState.Add(bindingContext.ModelName, modelState);
            return actualValue;
        }
    }
    #endregion

    public class ResourceAccessor
    {
        private string resourceName;
        private Type resourceType;
        private Func<string> accessor;
        private string _resourceValue;

        public ResourceAccessor(string resourceName, Type resourceType)
        {
            this.resourceName = resourceName;
            this.resourceType = resourceType;
        }

        public string resourceValue
        {
            get
            {
                SetupAccessor();
                return accessor();
            }
        }

        private void SetupAccessor()
        {
            if (accessor != null) //already set
                return;
            string localValue = _resourceValue;
            bool flag1 = !string.IsNullOrEmpty(resourceName);
            bool flag2 = !string.IsNullOrEmpty(localValue);
            bool flag3 = resourceType != (Type)null;
            if (flag1 == flag2)
            {
                throw new InvalidOperationException("Can't set resource value");
            }
            if (flag3 != flag1)
            {
                throw new InvalidOperationException("Resource name and type required");
            }
            if (flag1)
                PropertyLookup();
            else
            {
                accessor = (Func<string>)(() => localValue);
            }
        }

        private void PropertyLookup()
        {
            if (resourceType == (Type)null || string.IsNullOrEmpty(resourceName))
            {
                throw new InvalidOperationException("Resource name and type required");
            }

            PropertyInfo property = resourceType.GetProperty(resourceName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);
            if (property != (PropertyInfo)null)
            {
                MethodInfo getMethod = property.GetGetMethod(true);
                if (getMethod == (MethodInfo)null || !getMethod.IsAssembly && !getMethod.IsPublic)
                    property = (PropertyInfo)null;
            }
            if (property == (PropertyInfo)null)
            {
                throw new InvalidOperationException("Resource type doesn't have property");
            }
            else if (property.PropertyType != typeof(string))
            {
                throw new InvalidOperationException("Resource type must be string");
            }
            else
            {
                accessor = (Func<string>)(() => (string)property.GetValue((object)null, (object[])null));
            }
        }
    }

    public class RegularExpressionExAttribute : RegularExpressionAttribute, IClientValidatable
    {
        private Regex regex { get; set; }
        private string pattern;

        private string resourceName;
        private Type resourceType;

        /// <summary>
        /// constructor, calls base with ".*" basic regex
        /// </summary>
        /// <param name="resName">resource key</param>
        /// <param name="resType">resource type</param>
        public RegularExpressionExAttribute(string resName, Type resType)
            : base(".*")
        {
            resourceName = resName;
            resourceType = resType;
        }

        /// <summary>
        /// override RegularExpressionAttribute property
        /// </summary>
        public new string Pattern
        {
            get
            {
                SetupRegex();
                return pattern;
            }
        }

        /// <summary>
        /// loads regex from resources
        /// </summary>
        private void SetupRegex()
        {
            ResourceAccessor ra = new ResourceAccessor(resourceName, resourceType);
            pattern = ra.resourceValue;
            regex = new Regex(pattern);
        }

        /// <summary>
        /// override validation with our regex
        /// </summary>
        /// <param name="value">string for validation</param>
        /// <returns></returns>
        public override bool IsValid(object value)
        {
            SetupRegex();
            string val = Convert.ToString(value);
            if (string.IsNullOrEmpty(val))
                return true;
            var m = regex.Match(val);
            return (m.Success && (m.Index == 0));
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metaData, ControllerContext controllerContext)
        {
            yield return new ModelClientValidationRegexRule(base.ErrorMessageString, this.Pattern);
        }
    }

    //public class EmailAttribute : RegularExpressionAttribute, IClientValidatable
    //{
    //    public EmailAttribute()
    //        : base(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})$")
    //    {

    //    }

    //    public System.Collections.Generic.IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
    //    {
    //        var rule = new ModelClientValidationRegexRule(this.ErrorMessageString, base.Pattern);
    //        return new[] { rule };
    //    }
    //}

}
