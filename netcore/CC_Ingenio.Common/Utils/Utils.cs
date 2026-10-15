
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using GemCryptor;
using System.Collections;
using System.Globalization;
using System.Reflection;
using CC_Ingenio_Resources;
using System.Text.RegularExpressions;
using System.Data;
using System.ComponentModel;

// NOTE ON THIS SPLIT (see PORT_NOTES.md):
// The legacy CC_Ingenio.Common/Utils/Utils.cs mixes pure business logic with
// ~43 `List<System.Web.Mvc.SelectListItem>` dropdown-population methods and
// ASP.NET MVC infrastructure (IModelBinder implementations,
// IClientValidatable attributes) that has no .NET 8 equivalent and is
// genuinely used by the legacy web layer. Per an explicit decision, this
// port keeps only the framework-independent logic here; the MVC-coupled
// members stay in the legacy CC_Ingenio.Common/Utils/Utils.cs until
// CC_Ingenio.Web itself is ported, at which point a real replacement
// (ASP.NET Core view models, proper option lists, etc.) should be designed
// instead of carrying SelectListItem-shaped logic forward as-is.
//
// GemCryptor.TwoFishCrypto (used by Helper.*) is referenced directly as the
// original compiled DLL (see the csproj) per an explicit decision to try
// this before attempting any decompile/rewrite - no source is available for
// it (see PHASE0_NOTES.md).

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
        public const string CNST_DEFAULT_CULTURE = "en-GB";
        public const string MSG_VALID_PHONENO = "\\d{10}";
        public const string MSG_VALID_EMAIL = "^[_a-z0-9-]+(\\.[_a-z0-9-]+)*@[a-z0-9-]+(\\.[a-z0-9-]+)*(\\.[a-z]{2,4})$";
        public const string CNST_CUST_DOCUMENT_PATH = "~/Reports/CustomerDocs/";
        public const string MSG_VALID_MERCHANTCODE = @"^([0-9]{5,9})*(\\-[kK0-9]{1,1})$";

        public const int GRD_ROWS_PER_PAGE = 15;

        public const string CNST_SEC_PRIVATE_KEY = "Cc@1ngE#Io";

        public static string CNST_ADMIN = "ADMINISTRATOR";

        public static string OrganizationCode;
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
                            case "GreaterThan":
                                exp = Expression.GreaterThan(filterColumn, filterValue);
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
                            case "Equals":
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
                        whereExpr = Expression.And(whereExpr, exp);
                    }
                }
            }
            return Expression.Lambda<Func<TEntity, bool>>(whereExpr, parameter);

        }
        #endregion

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

        #region ValidateUserPrivilege
        public static bool ValidateUserPrivilege(Object AObject)
        {
            return true;
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

    public class CodeValue
    {
        public string Text { get; set; }
        public string Value { get; set; }

        public List<CodeValue> TransList { get; set; }
    }

    public class BillGeneration
    {
        public DateTime BillDate { get; set; }
        public string BillCycleCode { get; set; }
    }

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
                if (data < _belowLimit)
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

}
