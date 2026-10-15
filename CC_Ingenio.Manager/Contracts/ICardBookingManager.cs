using CC_Ingenio.DataTranfer.MasterInfo;
using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CC_Ingenio.DataTranfer.CardBookingInfo;

namespace CC_Ingenio.Manager.Contracts
{
    public interface ICardBookingManager
    {
        #region CardMaster
        List<CustomerDTO> FindCustomerForCardGeneration(int APageNo, CustomerDTO ACustomerDTO);
        List<CustomerDTO> ListCustomers(string AUserName);
        int PopulateCardMaster(DataTable ASelectedCustomers,string AUserName);
        int PopulatePaymentAllocationType(DataTable ASelectedCustomers, string AUserName);

        List<CardMasterDTO> FindAll(int APageNo, CardMasterDTO AFilter);
        CardMasterDTO FindByCardMasterCardNumber(string ACardNumber);
        List<CardMasterDTO> FindCardMaster(int APageIndex, int APageCount);

        //void SaveCardMasterDetails(CardMasterDTO ACardMasterDTO);
        void UpdateCardMasterDetails(string ACardNumber, CardMasterDTO ACardMasterDTO);
        void DeleteCardMaster(string ACardNumber);
        #endregion


        #region Application 
        string[] CheckIfAlreadyExistsCard(string ARUTNumber, string ASalesId);
        string[] CheckIfValidCard(string ARUTNumber, string Checkdigit);
        #endregion 
        #region CardEmbossing
        List<CardEmbossingDTO> FindCardnumberForEmbossing(int APageNo, CardEmbossingDTO ACustomerDTO);
        List<CardEmbossingDTO> ListCardNumber(string AUserName);
       // string[] CardEmbossingData(string ARUTNumber);
      //  string[] CardEmbossingData( List<CardEmbossingDTO> AlistCardEmbossingData,string Path);
        bool  CardEmbossingData(DataTable DtType,string User);

        List<CardEmbossingDTO> GetPendingCardEmbossingItems(int APageNo, CardEmbossingDTO ACardEmbossingFilter,List<CardEmbossingDTO > AList);
        int GetPendingCardEmbossingItemsRecordCount(CardEmbossingDTO ACardEmbossingFilter);
        List<CardEmbossedDTO> GetEmbossedDetails(string ARUT, string ACardNumber, string ACustomerName);
        List<PaymentHolidayDTO> GetPaymentHolidayDetails(string ACardNumber, string AProductCode,string ARUT, DateTime? AFromDate, DateTime? AToDate);
        void SavePaymentHolidayDetails(PaymentHolidayDTO APaymentHolidayDTO);
        void UpdatePaymentHolidayDetails(string Slno, string ACardNumber, PaymentHolidayDTO APaymentHolidayDTO);
        void DeletePaymentHoliday(string Id,string ACardNumber);
        PaymentHolidayDTO GetPaymentHolidayIDForApproval(int ASlNo);
        PaymentHolidayDTO FindPaymentHolidayById(int ASlNo);
        #endregion

    }
}
