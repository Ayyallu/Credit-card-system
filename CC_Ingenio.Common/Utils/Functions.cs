using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace CC_Ingenio.Common.Utils
{
    public class Functions
    {
        public class CheckDigitResult
        {
            public bool IsValid { get; set; }
            public string ErrMsg { get; set; }
            public string CheckDigit { get; set; }
        }

        public static bool IsNotEmptyAndIsNumeric(string givenNumber)
        {
            // not null, only has digits 0 to 9

            if (string.IsNullOrEmpty(givenNumber))
            {
                return false;
            }

            const string allowed = "0123456789";

            for (int i = 0; i < givenNumber.Length; i++)
            {
                if (allowed.IndexOf(givenNumber.Substring(i, 1)) < 0)
                {
                    return false;
                }
            }

            return true;
        } /* IsNotEmptyAndIsNumeric */

        #region Mod10 Validation and Check Digit Generation
        public static bool IsValidCardNumber(string cardNumber)  /* expecting card number WITH check digit */
        {
            // Mod 10 Validation is done using the Luhn algorithm
            CheckDigitResult obj = new CheckDigitResult();
            bool isValid = false;

            if (!IsNotEmptyAndIsNumeric(cardNumber)) /* not null, only has digits 0 to 9 */
            {
                return false;
            }

            string givenCheckDigit = cardNumber.Substring(cardNumber.Length - 1, 1);
            obj = generateCheckDigitMod10(cardNumber.Substring(0, cardNumber.Length - 1));
            if (!obj.IsValid)
            {
                return false;
            }

            if (obj.CheckDigit == givenCheckDigit)
            {
                isValid = true;
            }

            return isValid;
        }


        public static CheckDigitResult generateCheckDigitMod10(string cardNumber) /* expecting card number WITHOUT check digit */
        {
            CheckDigitResult returnObj = new CheckDigitResult();
            returnObj.IsValid = true;
            returnObj.ErrMsg = " ";

            if (!IsNotEmptyAndIsNumeric(cardNumber)) /* not null, only has digits 0 to 9 */
            {
                returnObj.IsValid = false;
                returnObj.ErrMsg = "Card Number is empty or contains invalid characters";
                return returnObj;
            }

            if (cardNumber.Length != 15)
            {
                returnObj.IsValid = false;
                returnObj.ErrMsg = "Card number length error:  must be 16 characters,  including check digit ";
                return returnObj;
            }

            // Check Digit generated using Luhn algorithm
            int[] weight = { 2, 1, 2, 1, 2, 1, 2, 1, 2, 1, 2, 1, 2, 1, 2 };  /*alternate digits to be multiplied by 2*/
            int[] digits = new int[cardNumber.Length];

            int j = 0;
            for (int i = cardNumber.Length - 1; i >= 0; i--)
            {
                digits[j] = Int32.Parse(cardNumber.Substring(i, 1));
                j += 1;
            }

            int temp = 0;
            int sum = 0;

            for (int i = 0; i < digits.Length; i++)
            {
                temp = digits[i] * weight[i];
                if (temp > 9) /* if resultant product is a 2 digit number, then add the digits of the product */
                {
                    temp -= 9;
                }
                sum += temp;
            }

            returnObj.CheckDigit = ((sum * 9) % 10).ToString();

            return (returnObj);
        } /* generateCheckDigitMod10 */

        #endregion

        #region Mod11 Validation for RUT

        public static bool IsValidRUTNumber(string RUTNumber)  /* RUT number with hyphen followed by check digit  */
        {
            CheckDigitResult obj = new CheckDigitResult();
            bool isValid = false;
            RUTNumber = RUTNumber.Trim().ToUpperInvariant();  /*remove leading and trailing spaces, if any */

            int pos = RUTNumber.IndexOf("-");

            if (pos < 0)  /* If incoming RUT had no check digit, return invalid RUT */
            {
                return false;
            }

            string givenCheckDigit = RUTNumber.Substring(pos + 1, 1);
            string RUTRest = RUTNumber.Substring(0, pos);

            // Mod 11 Validation 

            obj = generateCheckDigitMod11(RUTRest);
            if (!obj.IsValid)
            {
                return false;
            }

            if (obj.CheckDigit == givenCheckDigit)
            {
                isValid = true;
            }

            return isValid;
        }

        public static CheckDigitResult generateCheckDigitMod11(string RUTRest) /* expecting RUT Number without check digit */
        {
            CheckDigitResult returnObj = new CheckDigitResult();
            returnObj.IsValid = true;
            returnObj.ErrMsg = " ";

            if (!IsNotEmptyAndIsNumeric(RUTRest)) /* not null, only has digits 0 to 9 */
            {
                returnObj.IsValid = false;
                returnObj.ErrMsg = "RUT Number is empty or contains invalid characters";
                return returnObj;
            }

            if (RUTRest.Length > 9)
            {
                returnObj.IsValid = false;
                returnObj.ErrMsg = "RUT number too long:  max length = 9, including check digit ";
                return returnObj;
            }

            int[] weight = { 2, 3, 4, 5, 6, 7, 2, 3, 4 };
            int[] digits = new int[9];

            int j = 0;
            for (int i = RUTRest.Length - 1; i >= 0; i--)
            {
                digits[j] = Int32.Parse(RUTRest.Substring(i, 1));
                j += 1;
            }

            int temp = 0;
            int sum = 0;

            for (int i = 0; i < digits.Length; i++)
            {
                temp = digits[i] * weight[i];

                sum += temp;
            }

            int rem = (11 - (sum % 11));

            returnObj.CheckDigit = rem.ToString();
            if (rem == 11) returnObj.CheckDigit = "0";
            if (rem == 10) returnObj.CheckDigit = "K";

            return returnObj;
        } /* generateCheckDigitMod11 */

        #endregion

        #region GenerateCardEmbossingFile

        public static string GenerateCardEmbossingFile(string CardNumber, string[] CardDetails,string  path)
        {


            System.Data.DataTable dtVendor = new System.Data.DataTable();
            char CChrSpace = System.Convert.ToChar("0");


            System.IO.FileStream fstAcctEntry = null;
            System.IO.StreamWriter strmWriter = null;



            try
            {

                   
                    string Path = "" + DateTime.Now.ToString().Replace(' ', '_');
                    Path = "" + DateTime.Now.ToString().Replace(':', '_');
                   // string FilePath = "D:\\Temp\\" + "Nisha_09_07_2014_78" + Path;

                    string FilePath = path + CardNumber+'-'+String.Format("{0:dd-MM-yyyy-hh-mm-ss}", DateTime.Now);
                    string[] SplitDetails = FilePath.Split('\\');

                    string FileNameWithExt = SplitDetails[SplitDetails.Length - 1];

                    string[] strFileName = FileNameWithExt.Split('.');
                    string FileName = strFileName[0];

                    fstAcctEntry = new System.IO.FileStream(FilePath
                       , System.IO.FileMode.Create
                       , System.IO.FileAccess.Write);
                    strmWriter = new System.IO.StreamWriter(fstAcctEntry);


                    #region File Header

                    string strDay = DateTime.Now.Day.ToString();
                    string strMonth = DateTime.Now.Month.ToString();
                    string strYear = DateTime.Now.Year.ToString();

                    strMonth = strMonth.PadLeft(2, '0');
                    strYear = strYear.Substring(2, 2);



                    string strprintname = CardDetails[2].ToString();
                    strprintname = strprintname.PadRight(26, ' ');
                    string CardExpiryDate = CardDetails[1].ToString();
                    string[] card = new string[5];
                   // card = CardDetails[1].ToString().Split('-');
                    CardExpiryDate = CardDetails[1].ToString();
                    string ServiceCode = CardDetails[3].ToString();
                    string Discrepancy = CardDetails[4].ToString();
                    string end = "?";
                    string Redundancy = CardDetails[6].ToString();
                    string strPrintHeader =
                                "%B" + CardNumber + '^'
                                + strprintname + '^'
                                + CardExpiryDate + ServiceCode + Discrepancy + end + Redundancy;                    


                    strmWriter.WriteLine(strPrintHeader);
                    string StrNextLine =    
                                ";B" + CardNumber + '='                               
                                + CardExpiryDate + ServiceCode + Discrepancy + end + Redundancy;
                    strmWriter.WriteLine(StrNextLine);


                    #endregion


                    return FilePath;

            }
            catch 
            {
                return "";
            }
            finally
            {
                if (strmWriter != null)
                    strmWriter.Close();
                if (fstAcctEntry != null)
                    fstAcctEntry.Close();

            }

        }

        #endregion


    }
}
