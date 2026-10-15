using System;
using System.Web.Mvc;
using System.Threading;

namespace CC_Ingenio.Web.Helpers
{
    /// <span class="code-SummaryComment"><summary></span>
    /// JQuery UI DatePicker helper.
    /// <span class="code-SummaryComment"></summary></span>
    public static class DatePickerHelper
    {
        /// <span class="code-SummaryComment"><summary></span>
        /// Gets Common Year Range
        /// <span class="code-SummaryComment"></summary></span>   
        /// <span class="code-SummaryComment"><param name="html">HtmlHelper object.</param></span> 
        public static string GetYearRange()
        {
            return "1900:2020";
        }
        /// <span class="code-SummaryComment"><summary></span>
        /// Converts the .net supported date format current culture 
        /// format into JQuery Datepicker format.
        /// <span class="code-SummaryComment"></summary></span>   
        /// <span class="code-SummaryComment"><param name="html">HtmlHelper object.</param></span> 
        /// <span class="code-SummaryComment"><returns>Format string that supported in JQuery Datepicker.</returns></span>
        public static string ConvertDateFormat(this HtmlHelper html)
        {
            return ConvertDateFormat(html,
        Thread.CurrentThread.CurrentCulture.DateTimeFormat.ShortDatePattern);
        }

        /// <span class="code-SummaryComment"><summary></span>
        /// Converts the .net supported date format current culture 
        /// format into JQuery Datepicker format.
        /// <span class="code-SummaryComment"></summary></span>
        /// <span class="code-SummaryComment"><param name="html">HtmlHelper object.</param></span> 
        /// <span class="code-SummaryComment"><param name="format">Date format supported by .NET.</param></span>  
        /// <span class="code-SummaryComment"><returns>Format string that supported in JQuery Datepicker.</returns></span> 
        public static string ConvertDateFormat(this HtmlHelper html, string format)
        {
            /*
             *  Date used in this comment : 6th - Nov - 2014 (Thursday)
             *
             *  .NET    JQueryUI        Output      Comment
             *  --------------------------------------------------------------
             *  d       d               6           day of month(No leading zero)
             *  dd      dd              06          day of month(two digit)
             *  ddd     D               Thu         day short name
             *  dddd    DD              Thursday    day long name
             *  M       m               11          month of year(No leading zero)
             *  MM      mm              11          month of year(two digit)
             *  MMM     M               Nov         month name short
             *  MMMM    MM              November    month name long.
             *  yy      y               14          Year(two digit)
             *  yyyy    yy              2014        Year(four digit)             *
             */

            string currentFormat = format;

            // Convert the date
            currentFormat = currentFormat.Replace("dddd", "DD");
            currentFormat = currentFormat.Replace("ddd", "D");

            // Convert month
            if (currentFormat.Contains("MMMM"))
            {
                currentFormat = currentFormat.Replace("MMMM", "MM");
            }
            else if (currentFormat.Contains("MMM"))
            {
                currentFormat = currentFormat.Replace("MMM", "M");
            }
            else if (currentFormat.Contains("MM"))
            {
                currentFormat = currentFormat.Replace("MM", "mm");
            }
            else
            {
                currentFormat = currentFormat.Replace("M", "m");
            }

            // Convert year
            currentFormat = currentFormat.Contains("yyyy") ? currentFormat.Replace("yyyy", "yy") : currentFormat.Replace("yy", "y");

            return currentFormat;
        }
    }
}