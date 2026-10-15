namespace CC_Ingenio.Common.Utils
{
    /// <summary>
    /// Temporary stand-in for CC_Ingenio.Common.Utils.Utils, the one piece of
    /// the legacy .NET Framework CC_Ingenio.Common project this ported
    /// CC_Ingenio.DataTranfer actually depends on: a handful of regex-pattern
    /// constants used in [RegularExpression(...)] data annotations (attribute
    /// arguments must be compile-time constants, so the containing type has
    /// to resolve at compile time even though the value gets inlined).
    /// CC_Ingenio.Common itself hasn't been assessed/ported to .NET 8 yet (it
    /// has real System.Web/Enterprise Library 6.0 baggage) - see
    /// netcore/CC_Ingenio.Core/BusinessDateShim.cs for the same pattern.
    /// Values copied verbatim from CC_Ingenio.Common/Utils/Utils.cs.
    /// Delete this file once CC_Ingenio.Common is ported and reference it
    /// instead.
    /// </summary>
    public static class Utils
    {
        public const string MSG_ALLOWED_CHARS = @"^[a-zA-Z0-9]+$";
        public const string MSG_ALLOWED_NUMERALS = @"^(([0-9]{1}\d{0,2},(\d{3},)*\d{3})|([0-9]{1}\d{0,}))$";
        public const string MSG_ALLOWED_NOSPACE = "{0:#.#}";
        public const string MSG_ALLOWED_PERCENTAGE = @"[0-9]+(\.\,[0-9][0-9]?)?";
        public const string MSG_VALID_PHONENO = "\\d{10}";
        public const string MSG_VALID_EMAIL = "^[_a-z0-9-]+(\\.[_a-z0-9-]+)*@[a-z0-9-]+(\\.[a-z0-9-]+)*(\\.[a-z]{2,4})$";
        public const string MSG_VALID_MERCHANTCODE = @"^([0-9]{5,9})*(\\-[kK0-9]{1,1})$";
    }
}
