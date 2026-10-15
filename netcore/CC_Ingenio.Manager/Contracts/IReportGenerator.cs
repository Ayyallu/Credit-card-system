namespace CC_Ingenio.Manager.Contracts
{
    /// <summary>
    /// Seam over report generation. The legacy implementation
    /// (CC_Ingenio.Common.Utils.ReportGenerator) is built on SAP Crystal
    /// Reports, which has no .NET 8 support, so the net8 Manager depends on
    /// this interface instead and whoever hosts it supplies an implementation
    /// once the Crystal Reports decision is made (see PHASE2_NOTES.md /
    /// PHASE3_STATUS_AND_WEB_SCOPE.md). The signature mirrors the one legacy
    /// overload Manager actually calls.
    /// </summary>
    public interface IReportGenerator
    {
        /// <summary>
        /// Renders <paramref name="reportName"/> with the given comma-separated
        /// parameter names/values to a file under <paramref name="path"/> and
        /// returns the generated file name.
        /// </summary>
        string PrintReportWithParamNewWindow(string reportName, string parameterFields,
            string parameterValues, string path, string loginName);
    }
}
