

function RefreshDropDown(ADropDownListId) {

    var selectorArray = $("[id = " + ADropDownListId + "]");
    //alert(selectorArray.length);
    if (selectorArray.length > 1) {
        $.each(selectorArray, function () {
            var select = $(this);
            var selectedVal = select.val();
            //var val = select.index;
            select.empty();
            select.append($('<option/>',
              {
                  value: "",
                  text: resourceProvider.SelectOne
              }));
        });
    }
    else {
        selectorArray.empty();
        selectorArray.append($('<option/>',
              {
                  value: "",
                  text: resourceProvider.SelectOne
              }));
    }
}

function PopulateCascadeDropDownLists(ADropDownListId, valueToSet, viewModelObj) {

    if (ADropDownListId == "Contract") {
        /*Contract dropdown*/
        var productCode = valueToSet.ProductCode;
        //var productCode = $("#Product").val();
        if (productCode != "") {

            $.getJSON("/Master/GetContractByProductCode?AProductCode=" + productCode,
                function (modeldata) {
                    var select = $("[id = Contract]");
                    select.empty();
                    $.each(modeldata, function (index, itemData) {
                        select.append($('<option/>', {
                            value: itemData.Value,
                            text: itemData.Text
                        }));
                    });
                    select.val(valueToSet.CustomerDecisionDTO[0].ContractCode);
                    viewModelObj.Customer.CustomerDecisionDet()[0].ContractCode = ko.observable(valueToSet.CustomerDecisionDTO[0].ContractCode).extend({
                        required: {
                            params: true,
                            message: String.format(resourceProvider.MsgErrPleaseSelect, resourceProvider.Contract)
                        }
                    });
                });
        }
    }
    else {
        /*Region and Communa dropdown*/
        //nationalityId = $("#Nationality").val();
        nationalityId = valueToSet.Nationality;
        if (nationalityId != "") {

            var url;
            if (ADropDownListId == "Communa") {

                url = "/Master/GetCommunas?Id=";
            }
            else {

                url = "/Master/GetRegions?Id=";
            }

            $.getJSON(url + nationalityId,
             function (modelData) {
                 var selectorArray = $("[id = " + ADropDownListId + "]");

                 $.each(selectorArray, function (index) {

                     var select = $(this);
                     select.empty();

                     $.each(modelData, function (index, itemData) {
                         select.append($('<option/>',
                         {
                             value: itemData.Value,
                             text: itemData.Text
                         }));
                     });

                     if (ADropDownListId == "Communa") {
                         select.val(valueToSet.CustomerAddressDTO[index].CommunaCode);
                         viewModelObj.Customer.CustomerAddressDet()[index].CommunaCode = ko.observable(valueToSet.CustomerAddressDTO[index].CommunaCode).extend({
                             required: {
                                 params: true,
                                 message: String.format(resourceProvider.MsgErrPleaseSelect, resourceProvider.Communa)
                             }
                         });
                     }
                     else {
                         select.val(valueToSet.CustomerAddressDTO[index].RegionCode);
                         viewModelObj.Customer.CustomerAddressDet()[index].RegionCode = ko.observable(valueToSet.CustomerAddressDTO[index].RegionCode).extend({
                             required: {
                                 params: true,
                                 message: String.format(resourceProvider.MsgErrPleaseSelect, resourceProvider.Region)
                             }
                         });
                     }
                 });
             });
        }
    }
}

function ShowMessage(RedirectUrl, MessageToShow) {
    if (MessageToShow != "") {
        $('#dialog').text("");
        $('#dialog').dialog({
            resizable: false,
            modal: true,
            title: resourceProvider.ApplicationName,
            buttons: [{
                closeOnEscape: false,
                open: function (event, ui) {
                    $(this).closest('.ui-dialog').find('.ui-dialog-titlebar-close').hide();
                },
                text: resourceProvider.OK,
                class: "ButtonStyle",
                width: "100%",
                click: function () {
                    $(this).dialog("close");
                    if (RedirectUrl.length != 0) {
                        window.location.href = RedirectUrl;
                    }
                }
            }]

        });
        $('#dialog').text(MessageToShow);
    }
}

function CustomerAddress()
{
    var self = this;

    self.AddressId = ko.observable(0);
    self.AddressTypeCode = ko.observable("").extend({
        required: {
            params: true,
            message: String.format(resourceProvider.MsgErrPleaseSelect, resourceProvider.AddressType)
        }
    });
    self.TypeOfResidenceClearance = ko.observable("").extend({
        required: {
            params: true,
            message: String.format(resourceProvider.MsgErrPleaseSelect, resourceProvider.ResidenceClearanceType)
        }
    });
    self.RegionCode = ko.observable("").extend({
        required: {
            params: true,
            message: String.format(resourceProvider.MsgErrPleaseSelect, resourceProvider.Region)
        }
    });
    self.CommunaCode = ko.observable("").extend({
        required: {
            params: true,
            message: String.format(resourceProvider.MsgErrPleaseSelect, resourceProvider.Communa)
        }
    });
    self.CityCode = ko.observable("").extend({
        required: {
            params: true,
            message: String.format(resourceProvider.MsgErrPleaseSelect, resourceProvider.City)
        }
    });
    self.Street = ko.observable("").extend({
        required: {
            params: true,
            message: String.format(resourceProvider.MsgErrPleaseEnter2, resourceProvider.Street)
        }
    });
    self.Number = ko.observable("").extend({
        required: {
            params: true,
            message: String.format(resourceProvider.MsgErrPleaseEnter2, resourceProvider.Number)
        }
    });
    self.Block = ko.observable("");
    self.ComplementOfAddress = ko.observable("");
    self.Town = ko.observable("");
    self.Landmark = ko.observable("");
    self.PostalCode = ko.observable("");
    self.POBox = ko.observable("");
    self.Phone = ko.observable("").extend({
        required: {
            params: true,
            message: String.format(resourceProvider.MsgErrPleaseEnter2, resourceProvider.Phone)
        }
    });
    self.Mobile = ko.observable("");
    self.Fax = ko.observable("");
}

function CustomerDependent() {
    var self = this;

    self.DependentID = ko.observable(0);
    self.CustomerId = ko.observable(0);
    self.AgeGroup = ko.observable("").extend({
        required: {
            params: true,
            message: String.format(resourceProvider.MsgErrPleaseEnter2, resourceProvider.AgeGroup)
        }
    });
    self.NoOfChildren = ko.observable(0).extend({
        required: {
            params: true,
            message: String.format(resourceProvider.MsgErrPleaseEnter2, resourceProvider.NoOfChildren)
        }
    });
}

function CustomerOccupation() {
    var self = this;

    self.OccupationId = ko.observable(0);
    self.CustomerId = ko.observable(0);
    self.WorkType = ko.observable("").extend({
        required: {
            params: true,
            message: String.format(resourceProvider.MsgErrPleaseSelect, resourceProvider.WorkType)
        }
    });
    self.TotalMonthlyIncome = ko.observable(0);
    self.CompanyRUT = ko.observable("");
    self.CompanyName = ko.observable("");
    self.DateOfJoining = ko.observable($.datepicker.formatDate(resourceProvider.FormatDateJS, new Date())).extend({
        required: {
            params: true,
            message: String.format(resourceProvider.MsgErrPleaseSelect, resourceProvider.DateOfJoining)
        }
    });
    self.Post = ko.observable("").extend({
        required: {
            params: true,
            message: String.format(resourceProvider.MsgErrPleaseSelect, resourceProvider.Designation)
        }
    });
    self.IsIncomeProofAvailable = ko.observable(false);
    self.IncomeType = ko.observable("").extend({
        required: {
            params: true,
            message: String.format(resourceProvider.MsgErrPleaseSelect, resourceProvider.IncomeType)
        }
    });
}

function CustomerCommericalHouseConsumption() {

    var self = this;

    self.CommericalId = ko.observable(0);
    self.CustomerId = ko.observable(0);
    self.Type = ko.observable("").extend({
        required: {
            params: true,
            message: String.format(resourceProvider.MsgErrPleaseSelect, resourceProvider.Type)
        }
    });
    self.Name = ko.observable("").extend({
        required: {
            params: true,
            message: String.format(resourceProvider.MsgErrPleaseEnter2, resourceProvider.Name)
        }
    });
    self.CardNumber = ko.observable("").extend({
        required: {
            params: true,
            message: String.format(resourceProvider.MsgErrPleaseSelect, resourceProvider.CardNumber)
        }
    });
    self.AccountStatus = ko.observable("").extend({
        required: {
            params: true,
            message: String.format(resourceProvider.MsgErrPleaseSelect, resourceProvider.AccountStatus)
        }
    });
}

function CustomerBillingDetail() {

    var self = this;

    self.BillingDetailId = ko.observable(0);
    self.CustomerId = ko.observable(0);
    self.IsApplicationSignedOffByCustomer = ko.observable(false);
    self.SendAccountStatementTo = ko.observable(0).extend({
        required: {
            params: true,
            message: String.format(resourceProvider.MsgErrPleaseSelect, resourceProvider.SendAccountStatementTo)
        }
    });
    self.AccountNo = ko.observable("");
    self.BranchCode = ko.observable("");
}

function CustomerCreditCardDetail() {
    
    var self = this;

    self.CreditCardId = ko.observable(0);
    self.CustomerId = ko.observable(0);
    self.CardType = ko.observable("");
    self.CardBrand = ko.observable("");
    self.BankInstitutionId = ko.observable("").extend({
        required: {
            params: true,
            message: String.format(resourceProvider.MsgErrPleaseSelect, resourceProvider.BankInstitutionId)
        }
    });
    self.CardNumber = ko.observable("").extend({
        required: {
            params: true,
            message: String.format(resourceProvider.MsgErrPleaseEnter2, resourceProvider.CardNumber)
        }
    });
    self.EffectiveToDate = ko.observable("");
    self.CreditLimit = ko.observable(0);
}

function CustomerCurrentAccountDetail()
{
    var self = this;

    self.AccountId = ko.observable(0);
    self.CustomerId = ko.observable(0);
    self.AccountNumber = ko.observable("").extend({
        required: {
            params: true,
            message: String.format(resourceProvider.MsgErrPleaseEnter2, resourceProvider.AccountNumber)
        }
    });
    self.BankType = ko.observable("").extend({
        required: {
            params: true,
            message: String.format(resourceProvider.MsgErrPleaseSelect, resourceProvider.BankType)
        }
    });
    self.BankCode = ko.observable("").extend({
        required: {
            params: true,
            message: String.format(resourceProvider.MsgErrPleaseSelect, resourceProvider.Bank)
        }
    });
}

function CustomerDecision() {

    var self = this;

    self.DecisionID = ko.observable(0);
    self.CustomerId = ko.observable(0);
    self.DecisionCode = ko.observable("").extend({
        required: {
            params: true,
            message: String.format(resourceProvider.MsgErrPleaseSelect, resourceProvider.Decision)
        }
    });
    self.ReasonCode = ko.observable("").extend({
        required: {
            params: true,
            message:String.format(resourceProvider.MsgErrPleaseSelect, resourceProvider.Reason)
        }
    });
    self.Remarks = ko.observable("").extend({
        required: {
            params: true,
            message: String.format(resourceProvider.MsgErrPleaseEnter2, resourceProvider.Remarks)
        }
    });
    self.ContractCode = ko.observable("").extend({
        required: {
            params: true,
            message: String.format(resourceProvider.MsgErrPleaseSelect, resourceProvider.Contract)
        }
    });
    self.Unemployment = ko.observable("");
    self.IsContractAcceptedByCustomer = ko.observable(false);
    self.CreditLimit = ko.observable("").extend({
        reuquired: {
            params: true,
            message: String.format(resourceProvider.MsgErrPleaseEnter2, resourceProvider.CreditLimit)
        }
    }).extend({ min: 1 });
}

function Customer(value) {
    var self = this;

    self.ApprovalStatus = ko.observable(value ? value.ApprovalStatus : null);
    self.CustomerId = ko.observable(value ? value.CustomerId : 0);
    self.ParentCustomerId = ko.observable(value ? value.ParentCustomerId : 0);
    self.BranchCode = ko.observable(value ? value.BranchCode : "").extend(
        {
            required: {
                params: true,
                message:String.format(resourceProvider.MsgErrPleaseSelect, resourceProvider.Branch)
            }
        });
    self.CampaignSource = ko.observable(value ? value.CampaignSource : "");
    self.ChannelCode = ko.observable(value ? value.ChannelCode : "");
   
    self.ApplicationReferenceNo = ko.observable(value ? value.ApplicationReferenceNo : "");
    self.OrganizationCode = ko.observable(value ? value.OrganizationCode : "").extend(
     {
         required: {
             params: true,
             message:String.format(resourceProvider.MsgErrPleaseSelect, resourceProvider.Organization)
         }
     });
    self.ProductMemberTypeCode = ko.observable(value ? value.ProductMemberTypeCode : "");
    self.ProductTypeCode = ko.observable(value ? value.ProductTypeCode : "");
   
    self.ProductCategoryCode = ko.observable(value ? value.ProductCategoryCode : "").extend(
    {
        required: {
            params: true,
            message:String.format(resourceProvider.MsgErrPleaseSelect, resourceProvider.ProductCategory)
        }
    });
    self.ProductCode = ko.observable(value ? value.ProductCode : "").extend(
    {
        required: {
            params: true,
            message: String.format(resourceProvider.MsgErrPleaseSelect, resourceProvider.Product)
        }
    });
    self.RUT = ko.observable(value ? value.RUT : "").extend({ 
        required: { 
            params:true,
            message:String.format(resourceProvider.MsgErrPleaseEnter2, resourceProvider.RUT)
        }
    });

    self.SerialNo = ko.observable(value ? value.SerialNo : "");
  
    self.ExpiryDate = ko.observable(value ? (value.ExpiryDate ? $.datepicker.formatDate(resourceProvider.FormatDateJS, new Date(parseInt(value.ExpiryDate.substr(6)))) : null) : null);
        //ko.observable(value ? $.datepicker.formatDate("dd/mm/yy", new Date(parseInt(value.ExpiryDate.substr(6)))) : null);

    self.CorporateRUT = ko.observable(value ? value.CorporateRUT : "");
    self.SalesId = ko.observable(value ? value.SalesId : "").extend({
        required: {
            params: true,
            message:String.format(resourceProvider.MsgErrPleaseEnter2, resourceProvider.SalesId)
        }
    });

    self.PhysicalApplicationRefNo = ko.observable(value ? value.PhysicalApplicationRefNo : "");
    self.PhysicalApplicationRefDate = ko.observable(value ? (value.PhysicalApplicationRefDate ? $.datepicker.formatDate(resourceProvider.FormatDateJS, new Date(parseInt(value.PhysicalApplicationRefDate.substr(6)))) : null) : null);
    self.ExternalCivilRegistryResult = ko.observable(value ? value.ExternalCivilRegistryResult : "");
    self.InputterId = ko.observable(value ? value.InputterId : "");
    self.InputterDate = ko.observable(value ? (value.InputterDate ? $.datepicker.formatDate(resourceProvider.FormatDateJS, new Date(parseInt(value.InputterDate.substr(6)))) : null) : null);

    self.CreditScore = ko.observable(value ? value.CreditScore : "").extend({
        required: {
            params: true,
            message: String.format(resourceProvider.MsgErrPleaseEnter2, resourceProvider.CombinedScore)
        }
    }).extend({ min: 1 });

    self.CustomerType = ko.observable(value ? value.CustomerType : "").extend({
        required: {
            params: true,
            message: String.format(resourceProvider.MsgErrPleaseSelect, resourceProvider.CustomerType)
        }
    });
   
    self.Name = ko.observable(value ? value.Name : "").extend({
        required: {
            params: true,
            message:String.format(resourceProvider.MsgErrPleaseEnter2, resourceProvider.Name)
        }
    });
    self.PaternalSurname = ko.observable(value ? value.PaternalSurname : "").extend({
        required: {
            params: true,
            message: String.format(resourceProvider.MsgErrPleaseEnter2, resourceProvider.PaternalName)
        }
    });
    self.MaternalSurname = ko.observable(value ? value.MaternalSurname : "").extend({
        required: {
            params: true,
            message: String.format(resourceProvider.MsgErrPleaseEnter2, resourceProvider.MaternalName)
        }
    });
    self.Nationality = ko.observable(value ? value.Nationality : "").extend({
        required: {
            params: true,
            message: String.format(resourceProvider.MsgErrPleaseSelect, resourceProvider.Nationality)
        }
    });
    self.DateOfBirth = ko.observable(value ? $.datepicker.formatDate(resourceProvider.FormatDateJS, new Date(parseInt(value.DateOfBirth.substr(6)))) : null).extend({
        required: {
            params: true,
            message: String.format(resourceProvider.MsgErrPleaseSelect, resourceProvider.DateOfBirth)
        }
    });
    self.Gender = ko.observable(value ? value.Gender : "").extend({
        required: {
            params: true,
            message:String.format(resourceProvider.MsgErrPleaseSelect, resourceProvider.Gender)
        }
    });
    self.MaritalStatus = ko.observable(value ? value.MaritalStatus : "").extend({
        required: {
            params: true,
            message:String.format(resourceProvider.MsgErrPleaseSelect, resourceProvider.MaritalStatus)
        }
    });
    self.NumberofDependents = ko.observable(value ? value.NumberofDependents : 0).extend({
        required: {
            params: true,
            message:String.format(resourceProvider.MsgErrPleaseEnter2, resourceProvider.NumberOfDependents)
        }
    });
    self.Education = ko.observable(value ? value.Education : "").extend({
        required: {
            params: true,
            message:String.format(resourceProvider.MsgErrPleaseSelect, resourceProvider.Education)
        }
    });
    self.Profession = ko.observable(value ? value.Profession : "").extend({
        required: {
            params: true,
            message:String.format(resourceProvider.MsgErrPleaseSelect, resourceProvider.Profession)
        }
    });
    self.Housing = ko.observable(value ? value.Housing : "").extend({
        required: {
            params: true,
            message: String.format(resourceProvider.MsgErrPleaseSelect, resourceProvider.Housing)
        }
    });
    self.EmailID = ko.observable(value ? value.EmailID : "").extend({
        required: {
            params: true,
            message: String.format(resourceProvider.MsgErrPleaseEnter2, resourceProvider.Email)
        }
    });
    self.LegacyCardNumber = ko.observable(value ? value.LegacyCardNumber : "");
    self.AutoApprove = ko.observable(value ? value.AutoApprove : false);

    self.IsActive = ko.observable(value ? value.IsActive : true);
    self.IsBlackListed = ko.observable(value ? value.IsBlackListed : false);

    //*Customer Occupation*/   
    self.CustomerOccupationDet = ko.observableArray();
    self.CustomerOccupationDTO = ko.observableArray();

    if (value && value.CustomerOccupationDTO.length > 0) {
        $.each(value.CustomerOccupationDTO, function (index, values) {
            if (values) {
              
                var itemValue = new CustomerOccupation();
                itemValue = values;
                self.CustomerOccupationDet.push({
                OccupationId : values.OccupationId,
                CustomerId : values.CustomerId,
                WorkType: ko.observable(values.WorkType).extend({
                    required: {
                        params: true,
                        message: String.format(resourceProvider.MsgErrPleaseSelect, resourceProvider.WorkType)
                    }
                }),
                TotalMonthlyIncome : values ? (values.TotalMonthlyIncome ? values.TotalMonthlyIncome:0) : 0,
                CompanyRUT : values.CompanyRUT,
                CompanyName : values.CompanyName,
                DateOfJoining: ko.observable($.datepicker.formatDate(resourceProvider.FormatDateJS, new Date(parseInt(values.DateOfJoining.substr(6))))).extend({
                    required: {
                        params: true,
                        message: String.format(resourceProvider.MsgErrPleaseSelect, resourceProvider.DateOfJoining)
                    }
                }),
                Post: ko.observable(values.Post).extend({
                    required: {
                        params: true,
                        message: String.format(resourceProvider.MsgErrPleaseSelect, resourceProvider.Designation)
                    }
                }),
                IsIncomeProofAvailable : values.IsIncomeProofAvailable,
                IncomeType: ko.observable(values.IncomeType).extend({
                    required: {
                        params: true,
                        message:String.format(resourceProvider.MsgErrPleaseSelect, resourceProvider.IncomeType) 
                    }
                })
                });
                /*self.CustomerOccupationDet(self.CustomerOccupationDet().concat(ko.utils.arrayMap(CustomerOccupationDet, itemValue)));*/
            }
        });
    }
    else {            
        var ItemValue = new CustomerOccupation();
        self.CustomerOccupationDet.push(ItemValue);
    }

    /*Customer Commerical House Consumption*/
    self.CustomerCommericalHouseConsumptionDet = ko.observableArray();
    self.CustomerCommericalHouseConsumptionDTO = ko.observableArray();

    if (value && value.CustomerCommericalHouseConsumptionDTO.length > 0) {
        $.each(value.CustomerCommericalHouseConsumptionDTO, function (index, values) {
            if (values) {

                var itemValue = new CustomerCommericalHouseConsumption();
                itemValue = values;
                self.CustomerCommericalHouseConsumptionDet.push({
                    CommericalId: values.CommericalId,
                    CustomerId: values.CustomerId,
                    Type: ko.observable(values.Type).extend({
                        required: {
                            params: true,
                            message: String.format(resourceProvider.MsgErrPleaseSelect, resourceProvider.Type)
                        }
                    }),
                    Name: ko.observable(values.Name).extend({
                        required: {
                            params: true,
                            message: String.format(resourceProvider.MsgErrPleaseEnter2, resourceProvider.Name)
                        }
                    }),
                    CardNumber: ko.observable(values.CardNumber).extend({
                        reuquired: {
                            params: true,
                            message: String.format(resourceProvider.MsgErrPleaseEnter2, resourceProvider.CardNumber)
                        }
                    }),
                    AccountStatus: ko.observable(values.AccountStatus).extend({
                        required: {
                            params: true,
                            message: String.format(resourceProvider.MsgErrPleaseSelect, resourceProvider.AccountStatus)
                        }
                    })
                });
            }
        });
    }
    else {
        var ItemValue = new CustomerCommericalHouseConsumption();
        self.CustomerCommericalHouseConsumptionDet.push(ItemValue);
    }

    /*Customer Billing Detail*/
    self.CustomerBillingDetailDet = ko.observableArray();
    self.CustomerBillingDetailDTO = ko.observableArray();

    if (value && value.CustomerBillingDetailDTO.length > 0) {
        $.each(value.CustomerBillingDetailDTO, function (index, values) {
            if (values) {
                var itemValue = new CustomerBillingDetail();
                itemValue = values;
                self.CustomerBillingDetailDet.push({
                    BillingDetailId: values.BillingDetailId,
                    CustomerId: values.CustomerId,
                    IsApplicationSignedOffByCustomer: values.IsApplicationSignedOffByCustomer,
                    SendAccountStatementTo: ko.observable(values.SendAccountStatementTo).extend({
                        required: {
                            params: true,
                            message: String.format(resourceProvider.MsgErrPleaseSelect, resourceProvider.SendAccountStatementTo)
                        }
                    }),
                    BranchCode: values.BranchCode,
                    AccountNo: values.AccountNo,
                    IsECSRequried: values.IsECSRequried,
                    PaymentDate: values.PaymentDate,
                    ECSFor: values.ECSFor,
                    BankCode: values.BankCode
                });
            }
        });
    }
    else {
        var itemValue = new CustomerBillingDetail();
        self.CustomerBillingDetailDet.push(itemValue);
    }

    /*Customer Decision*/
    self.CustomerDecisionDet = ko.observableArray();
    self.CustomerDecisionDTO = ko.observableArray();

    if (value && value.CustomerDecisionDTO.length > 0) {
        $.each(value.CustomerDecisionDTO, function (index, values) {
            if (values) {
                var itemValue = new CustomerDecision();
                itemValue = values;
                self.CustomerDecisionDet.push({
                    DecisionID: values.DecisionID,
                    CustomerId: values.CustomerId,
                    DecisionCode: ko.observable(values ? values.DecisionCode : "").extend({
                        required: {
                            params: true,
                            message: String.format(resourceProvider.MsgErrPleaseSelect, resourceProvider.Decision)
                        }
                    }),
                    ReasonCode: ko.observable(values ? values.ReasonCode : "").extend({
                        required: {
                            params: true,
                            message: String.format(resourceProvider.MsgErrPleaseSelect, resourceProvider.Reason)
                        }
                    }),
                    Remarks: ko.observable(values ? values.Remarks : "").extend({
                        required: {
                            params: true,
                            message: String.format(resourceProvider.MsgErrPleaseEnter2, resourceProvider.Remarks)
                        }
                    }),
                    ContractCode: ko.observable(values ? values.ContractCode : "").extend({
                        required: {
                            params: true,
                            message: String.format(resourceProvider.MsgErrPleaseSelect, resourceProvider.Contract)
                        }
                    }),
                    Unemployment: values.Unemployment,
                    IsContractAcceptedByCustomer: values.IsContractAcceptedByCustomer,
                    CreditLimit: ko.observable(values ? values.CreditLimit : "").extend({
                        reuquired: {
                            params: true,
                            message:String.format(resourceProvider.MsgErrPleaseEnter2, resourceProvider.CreditLimit) 
                        }
                    }).extend({ min: 1 }),
                });
            }
        });
    }
    else {
        var itemValue = new CustomerDecision();
        self.CustomerDecisionDet.push(itemValue);
    }

    self.Nationality.subscribe(function (nationalityId) //added subscribe method for the dropdown element
    {        
        if (nationalityId != null) {

            // populates all region dropdowns based on newly selected nationality
            PopulateAllDropDownWithSameIDViaAjax("Region", "/Master/GetRegions?Id=" + nationalityId);
            // populates all Communa dropdowns based on newly selected nationality
            PopulateAllDropDownWithSameIDViaAjax("Communa", "/Master/GetCommunas?Id=" + nationalityId);
            //new
            if (self.CustomerId._latestValue > 0) {
                var objCommunaList = $('[id = Communa]');
                $.each(objCommunaList, function (index) {
                    self.CustomerAddressDet()[index].CommunaCode = ko.observable("").extend({
                        required: {
                            params: true,
                            message: String.format(resourceProvider.MsgErrPleaseSelect, resourceProvider.Communa)
                        }
                    });
                    self.CustomerAddressDet()[index].RegionCode = ko.observable("").extend({
                        required: {
                            params: true,
                            message: String.format(resourceProvider.MsgErrPleaseSelect, resourceProvider.Region)
                        }
                    });
                });
            }
            //---
        }
        else {
            RefreshDropDown("Region");
            RefreshDropDown("Communa");
        }

    }.bind(self));
  
    /*Customer Address*/
    self.CustomerAddressDet = ko.observableArray();
    self.CustomerAddressDTO = ko.observableArray();

    if (value && value.CustomerAddressDTO.length > 0) {
        $.each(value.CustomerAddressDTO, function (index, values) {

            if (values) {               
                var itemValue = new CustomerAddress();
                itemValue = values;
                self.CustomerAddressDet.push({
                    AddressId: values ? values.AddressId : 0,
                    CustomerId: values ? values.CustomerId : "",
                    AddressTypeCode: ko.observable(values ? values.AddressTypeCode : "").extend({
                        required: {
                            params: true,
                            message: String.format(resourceProvider.MsgErrPleaseSelect, resourceProvider.AddressType)
                        }
                    }),
                    TypeOfResidenceClearance: ko.observable(values ? values.TypeOfResidenceClearance : "").extend({
                        required: {
                            params: true,
                            message: String.format(resourceProvider.MsgErrPleaseSelect, resourceProvider.TypeOfAddressClearance)
                        }
                    }),
                    RegionCode: ko.observable(values ? values.RegionCode : "").extend({
                        required: {
                            params: true,
                            message: String.format(resourceProvider.MsgErrPleaseSelect, resourceProvider.Region)
                        }
                    }),
                    CityCode: ko.observable(values ? values.CityCode : "").extend({
                        required: {
                            params: true,
                            message: String.format(resourceProvider.MsgErrPleaseSelect, resourceProvider.City)
                        }
                    }),
                    CommunaCode: ko.observable(values ? values.CommunaCode : "").extend({
                        required: {
                            params: true,
                            message: String.format(resourceProvider.MsgErrPleaseSelect, resourceProvider.Communa)
                        }
                    }),
                    Street: ko.observable(values ? values.Street : "").extend({
                        required: {
                            params: true,
                            message: String.format(resourceProvider.MsgErrPleaseEnter2, resourceProvider.Street)
                        }
                    }),
                    Number: ko.observable(values ? values.Number : "").extend({
                        required: {
                            params: true,
                            message: String.format(resourceProvider.MsgErrPleaseEnter2, resourceProvider.Number)
                        }
                    }),
                    Block: values ? values.Block : "",
                    ComplementOfAddress: values ? values.ComplementOfAddress : "",
                    Town: values ? values.Town : "",
                    Landmark: values ? values.Landmark : "",
                    PostalCode: values ? values.PostalCode : "",
                    POBox: values ? values.POBox : "",
                    Phone: ko.observable(values ? values.Phone : "").extend({
                        required: {
                            params: true,
                            message: String.format(resourceProvider.MsgErrPleaseEnter2, resourceProvider.Phone)
                        }
                    }),
                    Mobile: values ? values.Mobile : "",
                    Fax: values ? values.Fax : ""
                });
            }
        });
    }
    else {
        var itemValue = new CustomerAddress();
        self.CustomerAddressDet.push(itemValue);
      
    }

    /*Customer Current Account Detail*/
    self.CustomerCurrentAccountDetailDTO = ko.observableArray();
    self.CustomerCurrentAccountDetailDet = ko.observableArray();

    if (value && value.CustomerCurrentAccountDetailDTO.length > 0) {
        $.each(value.CustomerCurrentAccountDetailDTO, function (index, values) {
            if (values) {

                var itemValue = new CustomerCurrentAccountDetail();
                itemValue = values;
                self.CustomerCurrentAccountDetailDet.push({
                    AccountId: values.AccountId,
                    CustomerId: values.CustomerId,
                    AccountNumber: ko.observable(values.AccountNumber).extend({
                        required: {
                            params: true,
                            message: String.format(resourceProvider.MsgErrPleaseEnter2, resourceProvider.AccountNumber)
                        }
                    }),
                    BankType: ko.observable(values.BankType).extend({
                        required: {
                            params: true,
                            message:String.format(resourceProvider.MsgErrPleaseSelect, resourceProvider.BankType)
                        }
                    }),
                    BankCode: ko.observable(values.BankCode).extend({
                        required: {
                            params: true,
                            message: String.format(resourceProvider.MsgErrPleaseSelect, resourceProvider.Bank)
                        }
                    })
                });
            }
        });
    }

    /*Customer Credit Card Detail*/
    self.CustomerCreditCardDetailDTO = ko.observableArray();
    self.CustomerCreditCardDetailDet = ko.observableArray();
    
    if (value && value.CustomerCreditCardDetailDTO.length > 0) {
        $.each(value.CustomerCreditCardDetailDTO, function (index, values) {
            if (values) {
                var itemValue = new CustomerCreditCardDetail();
                itemValue = values;
                self.CustomerCreditCardDetailDet.push({
                    CreditCardId: values.CreditCardId,
                    CustomerId: values.CustomerId,
                    CardType: values.CardType,
                    CardBrand: values.CardBrand,
                    BankInstitutionId: ko.observable(values.BankInstitutionId).extend({
                        required: {
                            params: true,
                            message: String.format(resourceProvider.MsgErrPleaseSelect, resourceProvider.BankInstitutionId)
                        }
                    }),
                    CardNumber: ko.observable(values.CardNumber).extend({
                        required: {
                            params: true,
                            message: String.format(resourceProvider.MsgErrPleaseEnter2, resourceProvider.CardNumber)
                        }
                    }),
                    EffectiveToDate: values.EffectiveToDate,
                    CreditLimit: values.CreditLimit
                });
            }
        });
    }    

    /*Customer Dependent*/
    self.CustomerDependentDet = ko.observableArray();
    self.CustomerDependentDTO = ko.observableArray();

    if (value && value.CustomerDependentDTO.length > 0) {

        $.each(value.CustomerDependentDTO, function (index, values) {
            if (values) {
                var itemValue = new CustomerDependent();
                itemValue = values;
                self.CustomerDependentDet.push({
                    DependentID: values.DependentID,
                    CustomerId: values.CustomerId,
                    AgeGroup: ko.observable(values.AgeGroup).extend({
                        required: {
                            params: true,
                            message: String.format(resourceProvider.MsgErrPleaseEnter2, resourceProvider.AgeGroup)
                        }
                    }),
                    NoOfChildren: ko.observable(values ? values.NoOfChildren : 0).extend({
                        requires: {
                            params: true,
                            message: String.format(resourceProvider.MsgErrPleaseEnter2, resourceProvider.NoOfChildren)
                        }
                    })
                });
            }
        });
    }    
}

function CustomerVM(customerData)
{
    var self = this;
    var isValidDecisionPanel = true;
    
    ko.bindingHandlers.datepicker = {
        // add event handler to handle change in the datepicker element and update the viewmodel
        init: function (element, valueAccessor, allBindingsAccessor) {
            //initialize datepicker with some optional options
            var options = allBindingsAccessor().datepickerOptions || {};
            $(element).datepicker(options);

            //handle the field changing
            ko.utils.registerEventHandler(element, "change", function () {
                var observable = valueAccessor();
                var newDate = moment($(element).datepicker("getDate"));
                observable = ko.observable(observable);
                //alert(ko.isObservable(observable));
                var jsonDate = "/Date(" + newDate.unix() * 1000 + newDate.format("ZZ") + ")/";
              
                observable(jsonDate);
                
            });

            //handle disposal (if KO removes by the template binding)
            ko.utils.domNodeDisposal.addDisposeCallback(element, function () {
                $(element).datepicker("destroy");
            });
        },
        // get the value from the viewmodel and format it for display
        update: function (element, valueAccessor) {
            var value = ko.utils.unwrapObservable(valueAccessor());
            var date = moment(value);
            current = $(element).datepicker("getDate");
    
            if (value != null) {
                if (value - current !== 0) {
                    var date = moment(value);
                    $(element).val(date.format("L"));
                }
            }
        }
       
    };
    
    ko.bindingHandlers.maskedInput = {
        init: function (element, valueAccessor, allBindings, viewModel, bindingContext) {
            ko.bindingHandlers.value.init(element, valueAccessor, allBindings, viewModel, bindingContext);
        },
        update: function (element, valueAccessor, allBindings, viewModel, bindingContext) {
            ko.bindingHandlers.value.update(element, valueAccessor, allBindings, viewModel, bindingContext);
            $(element).mask(allBindings.get('mask'));
            valueAccessor()($(element).val());
        }
    };

    self.CustomerErrors = ko.observable({});
    self.Customer = new Customer(customerData);

    //To open error containg panel before save(one by one)
    function OpenErrorContainingPanelOneByOne() {        
        isValidDecisionPanel = true;
        var SalesPolicyError = [
             String.format(resourceProvider.MsgErrPleaseSelect, resourceProvider.Branch),
             String.format(resourceProvider.MsgErrPleaseSelect, resourceProvider.Organization),
             String.format(resourceProvider.MsgErrPleaseSelect, resourceProvider.ProductCategory),
             String.format(resourceProvider.MsgErrPleaseSelect, resourceProvider.Product),
             String.format(resourceProvider.MsgErrPleaseEnter2, resourceProvider.RUT),
             String.format(resourceProvider.MsgErrPleaseEnter2, resourceProvider.SalesId),
             String.format(resourceProvider.MsgErrPleaseEnter2, resourceProvider.CombinedScore),
             String.format(resourceProvider.MsgErrPleaseEnter2, resourceProvider.MsgInfoValueGreaterThanOne)];
        var PersonalDetError = [
            String.format(resourceProvider.MsgErrPleaseSelect, resourceProvider.CustomerType),
            String.format(resourceProvider.MsgErrPleaseEnter2, resourceProvider.Name),
            String.format(resourceProvider.MsgErrPleaseEnter2, resourceProvider.PaternalName),
            String.format(resourceProvider.MsgErrPleaseEnter2, resourceProvider.MaternalName),
            String.format(resourceProvider.MsgErrPleaseSelect, resourceProvider.Nationality),
            String.format(resourceProvider.MsgErrPleaseSelect, resourceProvider.DateOfBirth),
            String.format(resourceProvider.MsgErrPleaseSelect, resourceProvider.Gender),
            String.format(resourceProvider.MsgErrPleaseSelect, resourceProvider.MaritalStatus),
            String.format(resourceProvider.MsgErrPleaseEnter2, resourceProvider.NumberOfDependents),
            String.format(resourceProvider.MsgErrPleaseSelect, resourceProvider.Education),
            String.format(resourceProvider.MsgErrPleaseSelect, resourceProvider.Profession),
            String.format(resourceProvider.MsgErrPleaseSelect, resourceProvider.Housing),
            String.format(resourceProvider.MsgErrPleaseEnter2, resourceProvider.Email)];
        var CustomerError = ko.observable({});
        var DecisionError = ko.observable({});
        var AddressError = ko.observable({});
        var OccupationError = ko.observable({});
        var CurrentAccntError = ko.observable({});
        var CreditCardError = ko.observable({});
        var CommercialHouseError = ko.observable({});
        var BillingError = ko.observable({});
        var DependentError = ko.observable({});
        CustomerError = ko.validation.group(self.Customer, { deep: false });
        AddressErrors = ko.validation.group(self.Customer.CustomerAddressDet, { deep: true });
        OccupationError = ko.validation.group(self.Customer.CustomerOccupationDet, { deep: true });
        CurrentAccntError = ko.validation.group(self.Customer.CustomerCurrentAccountDetailDet, { deep: true });
        CreditCardError = ko.validation.group(self.Customer.CustomerCreditCardDetailDet, { deep: true });
        CommercialHouseError = ko.validation.group(self.Customer.CustomerCommericalHouseConsumptionDet, { deep: true });
        BillingError = ko.validation.group(self.Customer.CustomerBillingDetailDet, { deep: true });
        DependentError = ko.validation.group(self.Customer.CustomerDependentDet, { deep: true });
        DecisionError = ko.validation.group(self.Customer.CustomerDecisionDet, { deep: true });

        if (CustomerError._latestValue.filter(FilterNull).length > 0) {
            $.each(CustomerError._latestValue.filter(FilterNull), function (index) {
                var item = CustomerError._latestValue.filter(FilterNull)[index];

                if (SalesPolicyError.filter(function (element) { return element != item; }).length != SalesPolicyError.length) {
                    $("#accordion").accordion({
                        active: 0
                    });
                    if (self.Customer.CustomerId._latestValue > 0) {
                        if ($('#Product').val() == '') {
                            $('#lblValidateProduct').text( String.format(resourceProvider.MsgErrPleaseSelect, resourceProvider.Product));
                        }
                    }
                    return false;
                }
                if (PersonalDetError.filter(function (element) { return element != item; }).length != PersonalDetError.length) {
                    $("#accordion").accordion({
                        active: 1
                    });
                    return false;
                }
            });
        }
        else if (AddressErrors._latestValue.filter(FilterNull).length > 0) {
            $("#accordion").accordion({
                active: 2
            });
            //new
            var objCommunaList = $('[id = Communa]');
            var objValidationList = $('[id = lblValidateCommuna]');
            $.each(objCommunaList, function (index) {
                if (self.Customer.CustomerAddressDet()[index].AddressId > 0) {
                    if ($(this).val() == '') {
                        $(objValidationList[index]).text(String.format(resourceProvider.MsgErrPleaseSelect, resourceProvider.Communa));
                    }
                    else {
                        if ($(objValidationList[index]).text().length > 0) {
                            $(objValidationList[index]).text('');
                        }
                    }
                }
            });
            var objRegionList = $('[id = Region]');
            var objValidationList = $('[id = lblValidateRegion]');
            $.each(objRegionList, function (index) {
                if (self.Customer.CustomerAddressDet()[index].AddressId > 0) {
                    if ($(this).val() == '') {
                        $(objValidationList[index]).text(String.format(resourceProvider.MsgErrPleaseSelect, resourceProvider.Region));
                    }
                    else {
                        if ($(objValidationList[index]).text().length > 0) {
                            $(objValidationList[index]).text('');
                        }
                    }
                }
            });
            //---
        }
        else if (OccupationError._latestValue.filter(FilterNull).length > 0) {
            $("#accordion").accordion({
                active: 3
            });
        }
        else if (CurrentAccntError._latestValue.filter(FilterNull).length > 0) {
            $("#accordion").accordion({
                active: 4
            });
        }
        else if (CreditCardError._latestValue.filter(FilterNull).length > 0) {
            $("#accordion").accordion({
                active: 5
            });
        }
        else if (CommercialHouseError._latestValue.filter(FilterNull).length > 0) {
            $("#accordion").accordion({
                active: 6
            });
        }
        else if (BillingError._latestValue.filter(FilterNull).length > 0) {
            $("#accordion").accordion({
                active: 7
            });
        }
        else if (DependentError._latestValue.filter(FilterNull).length > 0) {
            $("#accordion").accordion({
                active: 8
            });
        }
        else if (DecisionError._latestValue.filter(FilterNull).length > 0) {
            $("#accordion").accordion({
                active: 9
            });
            if (self.Customer.CustomerId._latestValue > 0) {
                if ($('#Contract').val() == '') {
                    $('#lblValidateContract').text(String.format(resourceProvider.MsgErrPleaseSelect, resourceProvider.Contract));
                }
            }
        }       
        //---------------------------------------------------------------
    }

    self.Save = function () {   
        var isValid = true;
        self.CustomerErrors = ko.validation.group(self.Customer, { deep: true });       
        //To open error containg panel before save(one by one)
        OpenErrorContainingPanelOneByOne();
        //----------------------------------------------------
        if (self.CustomerErrors._latestValue.filter(FilterNull).length != 0) {
            self.CustomerErrors.showAllMessages(true);
            isValid = false;
        }       
        
        if (isValid && isValidDecisionPanel) {
            
            self.Customer.CustomerOccupationDTO = self.Customer.CustomerOccupationDet;
            self.Customer.CustomerCommericalHouseConsumptionDTO = self.Customer.CustomerCommericalHouseConsumptionDet;
            self.Customer.CustomerBillingDetailDTO = self.Customer.CustomerBillingDetailDet;            
            self.Customer.CustomerDecisionDTO = self.Customer.CustomerDecisionDet;
            self.Customer.CustomerDependentDTO = self.Customer.CustomerDependentDet;
            self.Customer.CustomerAddressDTO = self.Customer.CustomerAddressDet;
            self.Customer.CustomerCreditCardDetailDTO = self.Customer.CustomerCreditCardDetailDet;
            self.Customer.CustomerCurrentAccountDetailDTO = self.Customer.CustomerCurrentAccountDetailDet;

            var json1 = ko.toJSON(self.Customer);
            var productCode = self.Customer.ProductCode;
            var creditScore = self.Customer.CreditScore;

            /*  Validating Credit Score */
            $.ajax({
                type: 'POST',
                url: '/Master/ValidateCreditScore',
                data: { AProductCode: productCode, ACreditScore: creditScore, AIsRequestFromUnderWriter: true, AIsAutoApproval: false },
                success: function (returnData) {
                    if ($('#dialog').text("").length > 0) {
                        $('#dialog').text("");
                    }

                    if (returnData.isCreditScoreViolated) {
                        $('#dialog').dialog({
                            resizable: false,
                            modal: true,
                            title: resourceProvider.ApplicationName,
                            buttons: [{
                                text: resourceProvider.OK,
                                class: "ButtonStyle",
                                width: "100%",
                                click: function () {
                                    $(this).dialog("close");
                                    $.ajax({
                                        url: '/Master/UpdateCustomerToUnderwrite/',
                                        type: 'POST',
                                        dataType: 'json',
                                        data: ko.toJSON({ ACustomerDTO: self.Customer, ACommand: resourceProvider.Save }),
                                        contentType: 'application/json; charset=utf-8',
                                        success: function (returnData) {
                                            ShowMessage(returnData.RedirectUrl, returnData.Message);
                                        }
                                    });
                                }
                            }]
                        });
                        if (returnData.Message.length > 0) {
                            $('#dialog').text(returnData.Message);
                        }
                    }
                    else {
                        $.ajax({
                            url: '/Master/UpdateCustomerToUnderwrite/',
                            type: 'POST',
                            dataType: 'json',
                            data: ko.toJSON({ ACustomerDTO: self.Customer, ACommand: resourceProvider.Save }),
                            contentType: 'application/json; charset=utf-8',
                            success: function (returnData) {
                                ShowMessage(returnData.RedirectUrl, returnData.Message);
                            }
                        });
                    }
                }
            });
            /*-------------------*/
        }
    };    

    self.Cancel = function () {
       window.location.reload();
    };

    self.UnderwriterAuthorize = function () {
        var isValid = true;
        self.CustomerErrors = ko.validation.group(self.Customer, { deep: true });
        //To open error containg panel before save(one by one)
        OpenErrorContainingPanelOneByOne();
        //----------------------------------------------------
        if (self.CustomerErrors._latestValue.filter(FilterNull).length != 0) {
            self.CustomerErrors.showAllMessages(true);
            isValid = false;
        }

        if (isValid && isValidDecisionPanel) {

            self.Customer.CustomerOccupationDTO = self.Customer.CustomerOccupationDet;
            self.Customer.CustomerCommericalHouseConsumptionDTO = self.Customer.CustomerCommericalHouseConsumptionDet;
            self.Customer.CustomerBillingDetailDTO = self.Customer.CustomerBillingDetailDet;
            self.Customer.CustomerDecisionDTO = self.Customer.CustomerDecisionDet;
            self.Customer.CustomerDependentDTO = self.Customer.CustomerDependentDet;
            self.Customer.CustomerAddressDTO = self.Customer.CustomerAddressDet;
            self.Customer.CustomerCreditCardDetailDTO = self.Customer.CustomerCreditCardDetailDet;
            self.Customer.CustomerCurrentAccountDetailDTO = self.Customer.CustomerCurrentAccountDetailDet;

            var json1 = ko.toJSON(self.Customer);
            var productCode = self.Customer.ProductCode;
            var creditScore = self.Customer.CreditScore;

            var customerId = self.Customer.CustomerId;
            var creditLimit = self.Customer.CustomerDecisionDet()[0]["CreditLimit"];
            var remarks = self.Customer.CustomerDecisionDet()[0]["Remarks"];
            /*  Validating Credit Score */
            $.ajax({
                type: 'POST',
                url: '/Master/ValidateCreditScore',
                data: { AProductCode: productCode, ACreditScore: creditScore, AIsRequestFromUnderWriter: true, AIsAutoApproval: false },
                success: function (returnData) {
                    if ($('#dialog').text("").length > 0) {
                        $('#dialog').text("");
                    }

                    if (returnData.isCreditScoreViolated) {
                        $('#dialog').dialog({
                            resizable: false,
                            modal: true,
                            title: resourceProvider.ApplicationName,
                            buttons: [{
                                text: resourceProvider.OK,
                                class: "ButtonStyle",
                                width: "100%",
                                click: function () {
                                    $(this).dialog("close");
                                    ///                                    
                                    $.ajax({
                                        url: "/Master/LoadUnderwriterAuthorizePopup",
                                        data: { ACustomerId: customerId, ACreditScore: creditScore, ACreditLimit: creditLimit, ARemarks: remarks },
                                        cache: false,
                                        contenttype: "application/json ; charset = utf-8",
                                        success: function (data) {                                            
                                            $("#UnderWriterAuthorizeDiv").html(data);
                                            
                                        },
                                        error: function (ts) { alert(ts.responseText) }
                                    });

                                    loading(); // loading
                                    setTimeout(function () { // then show popup, deley in .5 second
                                        loadPopup(); // function show popup
                                    }, 500); // .5 second
                                    ///
                                }
                            }]
                        });
                        if (returnData.Message.length > 0) {
                            $('#dialog').text(returnData.Message);
                        }
                    }
                    else {
                        
                        $.ajax({
                            url: "/Master/LoadUnderwriterAuthorizePopup",
                            data: { ACustomerId: customerId, ACreditScore: creditScore, ACreditLimit: creditLimit, ARemarks: remarks },
                            cache: false,
                            contenttype: "application/json ; charset = utf-8",
                            success: function (data) {
                                $("#UnderWriterAuthorizeDiv").html(data);
                            },
                            error: function (ts) { alert(ts.responseText) }
                        });

                        loading(); // loading
                        setTimeout(function () { // then show popup, deley in .5 second
                            loadPopup(); // function show popup
                        }, 500); // .5 second

                    }
                }
            });
            /*-------------------*/
        }
    };
}

function loading() {
    $("div.loader").show();
}

function SetKOPropertyOfCascadeDropdownLists(viewModelObj, customerValueObj) {

    viewModelObj.Customer.ProductCode = ko.observable(customerValueObj.ProductCode);

    var regionArray = $('[id = Region]');
    $.each(regionArray, function (index) {
        viewModelObj.Customer.CustomerAddressDet()[index].RegionCode = customerValueObj.CustomerAddressDTO[index].RegionCode;
    });

    var communaArray = $('[id = Communa]');
    $.each(communaArray, function (index) {
        viewModelObj.Customer.CustomerAddressDet()[index].CommunaCode = customerValueObj.CustomerAddressDTO[index].CommunaCode;
    });
}

function FilterNull(element) {
   return element != null;
}

