namespace CC_Ingenio.Common.Utils
{
    /// <summary>
    /// Temporary stand-in for CC_Ingenio.Common.Utils.Utils.BusinessDate, the one
    /// piece of the legacy .NET Framework CC_Ingenio.Common project this ported
    /// CC_Ingenio.Core actually depends on. CC_Ingenio.Common itself hasn't been
    /// assessed/ported to .NET 8 yet (it carries real System.Web/Enterprise
    /// Library 6.0 baggage), so this shim exists only so the ~62 call sites in
    /// this project that reference CC_Ingenio.Common.Utils.Utils.BusinessDate
    /// keep compiling unchanged. It is NOT wired to the legacy app's business
    /// date and must be set by whatever process (app startup, test setup, a
    /// future real port of Common) owns business-date state on .NET 8.
    /// Delete this file once CC_Ingenio.Common is ported and reference it instead.
    /// </summary>
    public static class Utils
    {
        public static System.DateTime BusinessDate;
    }
}
