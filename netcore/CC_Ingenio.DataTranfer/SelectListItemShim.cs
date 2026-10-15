namespace System.Web.Mvc
{
    /// <summary>
    /// Temporary stand-in for System.Web.Mvc.SelectListItem, which doesn't
    /// exist on .NET 8 (ASP.NET MVC 4/System.Web is .NET Framework only).
    /// UserDTO.UserAllowedBranch is genuinely populated and rendered by the
    /// legacy (not yet ported) CC_Ingenio.Web project's UserController and
    /// UserDetails.cshtml view as a multi-select branch picker - this isn't
    /// dead code like most of the System.Web.Mvc usings found elsewhere in
    /// this port, so it can't simply be deleted.
    /// This shim exists only so UserDTO.cs keeps compiling unchanged, in the
    /// same namespace/shape as the legacy type (Text/Value/Selected). It
    /// must be replaced with a real decision once CC_Ingenio.Web is ported:
    /// either a proper CC_Ingenio.DataTranfer-owned type (e.g. SelectOption)
    /// or ASP.NET Core's own Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
    /// if this DTO layer ends up depending on ASP.NET Core directly.
    /// Delete this file once that decision is made.
    /// </summary>
    public class SelectListItem
    {
        public string Text { get; set; }
        public string Value { get; set; }
        public bool Selected { get; set; }
    }
}
