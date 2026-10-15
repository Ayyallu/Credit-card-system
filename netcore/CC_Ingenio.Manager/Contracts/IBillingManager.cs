using CC_Ingenio.DataTranfer.BillingInfo;
using CC_Ingenio.DataTranfer.MasterInfo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CC_Ingenio.Manager.Contracts
{
    public interface IBillingManager
    {

        #region OnlinePayment
        List<OnlinePaymentDTO> FindOnlinePayment(int APageIndex, int APageCount);
        OnlinePaymentDTO FindByOnlinePaymentId(int AOnlinePaymentId);
        void SaveOnlinePaymentDetails(OnlinePaymentDTO AOnlinePaymentDTO);
        List<OnlinePaymentDTO> FindAll(int APageNo, OnlinePaymentDTO AFilter);
        #endregion

        #region Refinancing
        List<RefinancingDTO> FindAll(int APageNo, RefinancingDTO AFilter);
        RefinancingDTO FindByRefinancingCardNo(string ACardNo);
        List<RefinancingDTO> FindRefinancing(int APageIndex, int APageCount);
        void SaveRefinancingDetails(RefinancingDTO ARefinancingDTO);
        List<RefinancingDTO> GetRefinancingDetails(string ACardNumber, string ARUT);
        #endregion

        #region Renegotiation
        List<RenegotiationDTO> FindAll(int APageNo, RenegotiationDTO AFilter);
        RenegotiationDTO FindByRenegotiationCardNo(string ACardNo);
        List<RenegotiationDTO> FindRenegotiation(int APageIndex, int APageCount);
        void SaveRenegotiationDetails(RenegotiationDTO ARenegotiationDTO);
        List<RenegotiationDTO> GetRenegotiationDetails(string ACardNumber, string ARUT);
        #endregion

        #region Billing
        void CreateDynamicBillingTables(string ABillingCycleCode);
        void BillingCycleSummaryInsertion(string ACardNumber);
        void ImportFromExcel(string filepath);
        void ExecBillingProcedureTestingProcs(DateTime ABillDate, string AType);
        void ExecMovementtoOverdue(string ACardNumber, String Loginname);
        void ExecMovementtoNormal(string ACardNumber, String Loginname);
        // Movement To Insurance
        void ExecMovementtoInsurance(string ACardNumber, String Loginname);
        #endregion

        #region Statement Generation
        List<StatementGenerationDTO> FindAll(int APageNo, StatementGenerationDTO AFilter);
        List<StatementGenerationDTO> FindStatementGeneration(int APageIndex, int APageCount);
        StatementGenerationDTO FindByStatementGenerationCardNo(string ACardNo);
        string GenerateReport(string Cardnumber, string path);
        void UpdateStatementGenerationFileName(string CardNumber, int? ABillMonth, int? ABillYear, string FileName);
        List<GeneratedStatementDTO> GetGeneratedStatementList(string ACardNumber, string ARUT, string ABillMonth, string ABillYear);
        #endregion

        #region GetParentCustomersForTransaction
        List<RUTCardNumberSearchDTO> GetParentCustomersForTransaction(string ARUT, string ACardNumber, string ACustomerName);
        #endregion
    }
}
