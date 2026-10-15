function RefreshDropDown(ADropDownListId) {

    var selectorArray = $("[id = " + ADropDownListId + "]");   
    if (selectorArray.length > 1) {
        $.each(selectorArray, function () {
            var select = $(this);
            var selectedVal = select.val();            
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

    if (ADropDownListId == "Product") {
        /*Product dropdown*/
        var productCategoryCode = valueToSet.ProductCategoryCode;
        if (productCategoryCode != "") {
            $.getJSON("/Master/GetProductByProductCategory?Id=" + productCategoryCode,
                function (modeldata) {
                    var select = $("[id = Product]");
                    select.empty();
                    $.each(modeldata, function (index, itemData) {
                        select.append($('<option/>', {
                            value: itemData.Value,
                            text: itemData.Text
                        }));
                    });
                    select.val(valueToSet.ProductCode);
                    viewModelObj.IndustrialCustomer.ProductCode = ko.observable(valueToSet.ProductCode).extend(
                    {
                        required: {
                            params: true,
                            message: String.format(resourceProvider.MsgErrPleaseSelect, resourceProvider.Product)
                        }
                    });
                });
        }
    }
    else if (ADropDownListId == "Contract") {
        /*Contract dropdown*/
        var productCode = valueToSet.ProductCode;
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
                    select.val(valueToSet.ObjIndustrialCustomerDecisionDTO[0].ContractCode);
                    viewModelObj.IndustrialCustomer.IndustrialCustomerDecisionDet()[0].ContractCode = ko.observable(valueToSet.ObjIndustrialCustomerDecisionDTO[0].ContractCode);
                });           
        }
    }
    else if (ADropDownListId == "Communa" || ADropDownListId == "ParticularAddrCommuna" || ADropDownListId == "StatementAddrCommuna") {
        /*Communa dropdown*/        
        nationalityId = valueToSet.Nationality;
        if (nationalityId != "") {
            var url;           
            url = "/Master/GetCommunas?Id=";
            $.getJSON(url + nationalityId,
             function (modelData) {
                 var select = $("[id = " + ADropDownListId + "]");
                 select.empty();
                 $.each(modelData, function (index, itemData) {
                     select.append($('<option/>',
                     {
                         value: itemData.Value,
                         text: itemData.Text
                     }));
                 });

                 $.each(valueToSet.ObjIndustrialCustomerAddressDTO, function (index) {
                     
                     if (ADropDownListId == "Communa" && valueToSet.ObjIndustrialCustomerAddressDTO[index].AddressTypeCode == "C") {                        
                         select.val(valueToSet.ObjIndustrialCustomerAddressDTO[index].CommunaCode);
                         viewModelObj.IndustrialCustomer.CommercialAddressDet()[0].CommunaCode = ko.observable(valueToSet.ObjIndustrialCustomerAddressDTO[index].CommunaCode);
                     }
                     else if (ADropDownListId == "ParticularAddrCommuna" && valueToSet.ObjIndustrialCustomerAddressDTO[index].AddressTypeCode == "PR") {
                         select.val(valueToSet.ObjIndustrialCustomerAddressDTO[index].CommunaCode);
                         viewModelObj.IndustrialCustomer.ParticularAddressDet()[0].CommunaCode = ko.observable(valueToSet.ObjIndustrialCustomerAddressDTO[index].CommunaCode);
                     }
                     else if (ADropDownListId == "StatementAddrCommuna" && valueToSet.ObjIndustrialCustomerAddressDTO[index].AddressTypeCode == "ST") {
                         select.val(valueToSet.ObjIndustrialCustomerAddressDTO[index].CommunaCode);
                         viewModelObj.IndustrialCustomer.StatementAddressDet()[0].CommunaCode = ko.observable(valueToSet.ObjIndustrialCustomerAddressDTO[index].CommunaCode);
                     }
                 });
             });
        }
    }
    else if (ADropDownListId == "City" || ADropDownListId == "ParticularAddrCity" || ADropDownListId == "StatementAddrCity") {
        /*City dropdown*/
        var communaCode;
        $.each(valueToSet.ObjIndustrialCustomerAddressDTO, function (index) {
            if (ADropDownListId == "City" && valueToSet.ObjIndustrialCustomerAddressDTO[index].AddressTypeCode == "C") {
                communaCode = valueToSet.ObjIndustrialCustomerAddressDTO[index].CommunaCode;
            }
            else if (ADropDownListId == "ParticularAddrCity" && valueToSet.ObjIndustrialCustomerAddressDTO[index].AddressTypeCode == "PR") {
                communaCode = valueToSet.ObjIndustrialCustomerAddressDTO[index].CommunaCode;
            }
            else if (ADropDownListId == "StatementAddrCity" && valueToSet.ObjIndustrialCustomerAddressDTO[index].AddressTypeCode == "ST") {
                communaCode = valueToSet.ObjIndustrialCustomerAddressDTO[index].CommunaCode;
            }
        });
         
        if ((communaCode != null) && communaCode != "") {
            var url;
            url = "/Master/GetCityByCommuna?ACommunaCode=" + communaCode;
            $.getJSON(url,
             function (modelData) {
                 var select = $("[id = " + ADropDownListId + "]");
                 select.empty();
                 $.each(modelData.cityCodes, function (index, itemData) {
                     select.append($('<option/>',
                     {
                         value: itemData.Value,
                         text: itemData.Text
                     }));
                 });

                 $.each(valueToSet.ObjIndustrialCustomerAddressDTO, function (index) {

                     if (ADropDownListId == "City" && valueToSet.ObjIndustrialCustomerAddressDTO[index].AddressTypeCode == "C") {
                         select.val(valueToSet.ObjIndustrialCustomerAddressDTO[index].CityCode);
                         viewModelObj.IndustrialCustomer.CommercialAddressDet()[0].CityCode = ko.observable(valueToSet.ObjIndustrialCustomerAddressDTO[index].CityCode);
                     }
                     else if (ADropDownListId == "ParticularAddrCity" && valueToSet.ObjIndustrialCustomerAddressDTO[index].AddressTypeCode == "PR") {
                         select.val(valueToSet.ObjIndustrialCustomerAddressDTO[index].CityCode);
                         viewModelObj.IndustrialCustomer.ParticularAddressDet()[0].CityCode = ko.observable(valueToSet.ObjIndustrialCustomerAddressDTO[index].CityCode);
                     }
                     else if (ADropDownListId == "StatementAddrCity" && valueToSet.ObjIndustrialCustomerAddressDTO[index].AddressTypeCode == "ST") {
                         select.val(valueToSet.ObjIndustrialCustomerAddressDTO[index].CityCode);
                         viewModelObj.IndustrialCustomer.StatementAddressDet()[0].CityCode = ko.observable(valueToSet.ObjIndustrialCustomerAddressDTO[index].CityCode);
                     }
                 });
             });
        }
    }
}

function GetAutoApprovalOfSelectedProduct(ProductCode, objViewModel, callFromPage) {
    $.getJSON("/Master/GetAutoApprovalOfSelectedProduct?AProductCode=" + ProductCode,        
        function (returnData) {
            if (returnData) {
                if (callFromPage == true) {
                    objViewModel.IndustrialCustomer.IsAutoApprove = ko.observable(returnData.IsAutoApproval);
                }
                else {
                    objViewModel.IsAutoApprove = ko.observable(returnData.IsAutoApproval);
                }
            }
        });    
}

function GetProductProperties(ProductCode, objViewModel, callFromPage) {
    $.getJSON("/Master/GetProductProperties?AProductCode=" + ProductCode,
        function (returnData) {
            if (returnData) {
                if (callFromPage == true) {
                    objViewModel.IndustrialCustomer.IndustrialCustomerDecisionDet()[0].CreditLimit = ko.observable(returnData.MaxCreditLimit);
                    objViewModel.IndustrialCustomer.IndustrialCustomerDecisionDet()[0].BillCycleSysParamId = ko.observable(returnData.BillCycleSysParamId);
                    objViewModel.IndustrialCustomer.IndustrialCustomerDecisionDet()[0].BillingCycleCode = ko.observable(returnData.BillingCycleCode);
                    $('#CreditLimit').val(Globalize(resourceProvider.ResourceCulture).format(returnData.MaxCreditLimit, resourceProvider.FormatAmountJSn0));
                    $('#BillCycle').val(returnData.BillCycleSysParamId);
                    $('#hidMaxCreditLimitOfProduct').val(returnData.MaxCreditLimit);
                }
                else {
                    objViewModel.IndustrialCustomerDecisionDet()[0].CreditLimit = ko.observable(returnData.MaxCreditLimit);
                    objViewModel.IndustrialCustomerDecisionDet()[0].BillCycleSysParamId = ko.observable(returnData.BillCycleSysParamId);
                    objViewModel.IndustrialCustomerDecisionDet()[0].BillingCycleCode = ko.observable(returnData.BillingCycleCode);
                    $('#CreditLimit').val(Globalize(resourceProvider.ResourceCulture).format(returnData.MaxCreditLimit, resourceProvider.FormatAmountJSn0));
                    $('#BillCycle').val(returnData.BillCycleSysParamId);
                    $('#hidMaxCreditLimitOfProduct').val(returnData.MaxCreditLimit);
                }

            }
        });  
}


function getAge(dateString) {//dateformat -- dd/mm/yyyy
    var now = new Date();
    var today = new Date(now.getYear(), now.getMonth(), now.getDate());

    var yearNow = now.getYear();
    var monthNow = now.getMonth();
    var dateNow = now.getDate();

    //take year|month|date from datestring in dd/mm/yyyy format
    var dob = new Date(dateString.substring(6, 10),/*Year*/
                      dateString.substring(3, 5) - 1,/*Month*/
                      dateString.substring(0, 2)/*Date*/
                      );

    var yearDob = dob.getYear();
    var monthDob = dob.getMonth();
    var dateDob = dob.getDate();
  
    var age = {};
    var ageString = "";
    var yearString = "";
    var monthString = "";
    var dayString = "";


    yearAge = yearNow - yearDob;

    if (monthNow >= monthDob)
        var monthAge = monthNow - monthDob;
    else {
        yearAge--;
        var monthAge = 12 + monthNow - monthDob;
    }

    if (dateNow >= dateDob)
        var dateAge = dateNow - dateDob;
    else {
        monthAge--;
        var dateAge = 31 + dateNow - dateDob;

        if (monthAge < 0) {
            monthAge = 11;
            yearAge--;
        }
    }

    age = {
        years: yearAge,
        months: monthAge,
        days: dateAge
    };

    return age;
}

function CommercialAddress() {
    var self = this;

    self.AddressId = ko.observable(0);
    self.AddressDesc = ko.observable("");
    self.Village = ko.observable("");
    self.CommunaCode = ko.observable("");
    self.CityCode = ko.observable("");
    self.Phone = ko.observable("");
    self.Mobile = ko.observable("");
    self.EmailID = ko.observable("");
    self.ResidenceType = ko.observable("");
}

function ParticularAddress() {
    var self = this;

    self.AddressId = ko.observable(0);
    self.AddressDesc = ko.observable("");
    self.Village = ko.observable("");
    self.CommunaCode = ko.observable("");
    self.CityCode = ko.observable("");
    self.Phone = ko.observable("");
    self.Mobile = ko.observable("");
    self.EmailID = ko.observable("");
    self.ResidenceType = ko.observable("");
}

function StatementAddress() {
    var self = this;

    self.AddressId = ko.observable(0);
    self.AddressType = ko.observable("");
    self.ReferenceAddressTypeCode = ko.observable("");
    self.AddressDesc = ko.observable("");
    self.Village = ko.observable("");
    self.CommunaCode = ko.observable("");
    self.CityCode = ko.observable("");
    self.Phone = ko.observable("");
    self.Mobile = ko.observable("");
    self.EmailID = ko.observable("");
    self.Other = ko.observable("");
}

function Address() {
    var self = this;

    self.AddressId  = ko.observable(0);
    self.CustomerId = ko.observable(0);
    self.AddressTypeCode = ko.observable("");
    self.ReferenceAddressTypeCode = ko.observable("");
    self.AddressDesc = ko.observable("");
    self.Village  = ko.observable("");
    self.CommunaCode = ko.observable("");
    self.CityCode  = ko.observable("");
    self.Phone = ko.observable("");
    self.Mobile	 = ko.observable("");
    self.EmailID = ko.observable("");
    self.ResidenceType  = ko.observable("");
    self.Other = ko.observable("");
}

function DecisionDetails() {

    var self = this;

    self.DecisionID = ko.observable(0);
    self.CustomerId = ko.observable(0);
    self.DecisionCode = ko.observable("");
    self.ReasonCode = ko.observable("");
    self.Remarks = ko.observable("");
    self.ContractCode = ko.observable("");
    self.Unemployment = ko.observable("");
    self.IsContractAcceptedByCustomer = ko.observable(false);
    self.CreditLimit = ko.observable(0);
    self.BillingCycleCode = ko.observable("");
    self.BillCycleSysParamId = ko.observable(0);
}

function IndustrialCustomer(value) {
    var self = this;

    self.DSACode = ko.observable(value ? value.DSACode : "");
    self.DSAName = ko.observable(value ? value.DSAName : "");       
    self.UEN = ko.observable(value ? value.UEN : "");
    self.DSARUT = ko.observable(value ? value.DSARUT : "");

    self.CustomerId = ko.observable(value ? value.CustomerId : 0);
    self.ParentCustomerId = ko.observable(value ? value.ParentCustomerId : 0);
    self.OrganizationCode = ko.observable(value ? value.OrganizationCode : "");
    self.CustomerType = ko.observable(value ? value.CustomerType : "").extend({
        required: {
            params: true,
            message: String.format(resourceProvider.MsgErrPleaseSelect, resourceProvider.CustomerType)
        }
    });

    self.RUT = ko.observable(value ? value.RUT : "");
    self.SalesId = ko.observable(value ? value.SalesId : "");
    self.BranchCode = ko.observable(value ? value.BranchCode : "");
    self.ProductMemberTypeCode = ko.observable(value ? value.ProductMemberTypeCode : "");
    self.ProductCategoryCode = ko.observable(value ? value.ProductCategoryCode : "").extend(
    {
        required: {
            params: true,
            message: String.format(resourceProvider.MsgErrPleaseSelect, resourceProvider.ProductCategory)
        }
    });
    self.ProductCode = ko.observable(value ? value.ProductCode : "").extend(
    {
        required: {
            params: true,
            message: String.format(resourceProvider.MsgErrPleaseSelect, resourceProvider.Product)
        }
    });
    self.Nationality = ko.observable(value ? value.Nationality : "");

    self.LegacyCardNumber = ko.observable(value ? value.LegacyCardNumber : "");

    self.Name = ko.observable(value ? value.Name : "").extend({
        required: {
            params: true,
            message: String.format(resourceProvider.MsgErrPleaseEnter2, resourceProvider.Name)
        }
    });
    self.PaternalSurname = ko.observable(value ? value.PaternalSurname : "");
    self.MaternalSurname = ko.observable(value ? value.MaternalSurname : "");
    self.DateOfBirth = ko.observable(value ? (value.DateOfBirth ? $.datepicker.formatDate(resourceProvider.FormatDateJS, new Date(parseInt(value.DateOfBirth.substr(6)))) : null) : null);
    
    self.AgeYears = ko.computed(function () {
        if (self.DateOfBirth()) {
            if ($('#DateOfBirth').val() != '' && $('#DateOfBirth').val() != null) {
                if ($('#lblValidateDateOfBirth').text().length > 0) {
                    $('#lblValidateDateOfBirth').text('');
                }
            }
            return getAge(self.DateOfBirth()).years;
        }
        else { return 0;}
    }, this);
    self.AgeMonths = ko.computed(function () {
        if (self.DateOfBirth()) {
            if ($('#DateOfBirth').val() != '' && $('#DateOfBirth').val() != null) {
                if ($('#lblValidateDateOfBirth').text().length > 0) {
                    $('#lblValidateDateOfBirth').text('');
                }
            }
            return getAge(self.DateOfBirth()).months;
        }
        else { return 0;}
    }, this);

    self.AlcoholLicenseFlag = ko.observable(value ? value.AlcoholLicenseFlag : 0);
    self.CurrentAccountNo = ko.observable(value ? value.CurrentAccountNo : "");
    self.BankCode = ko.observable(value ? value.BankCode : "");
    self.CreditLineRequestedFlag = ko.observable(value ? value.CreditLineRequestedFlag : 0);

    self.Relevance = ko.observable(value ? value.Relevance : 0);
    self.GrowthPotentialPercentage = ko.observable(value ? value.GrowthPotentialPercentage : 0).extend({ max: 100 });
    self.PurchaseConsumptionOccasion = ko.observable(value ? value.PurchaseConsumptionOccasion : "");
    self.CreditBureauDicomFlag = ko.observable(value ? value.CreditBureauDicomFlag : 0);
    self.CreditBureauScore = ko.observable(value ? value.CreditBureauScore : 0);

    self.IsCustomerForUnderWriting = ko.observable(value ? value.IsCustomerForUnderWriting : false);
    self.IsAutoApprove = ko.observable(value ? value.IsAutoApprove : false);
    self.NeedToSendToApprovalQueue = ko.observable(value ? value.NeedToSendToApprovalQueue : false);  

    self.ProductCategoryCode.subscribe(function (productCategoryCode) {
        if (productCategoryCode) {
            PopulateDropDownViaAjax("#Product", "/Master/GetProductByProductCategory?Id=" + productCategoryCode);
            RefreshDropDown("Contract");
            if (self.CustomerId._latestValue != 0) {
                self.ProductCode = ko.observable("").extend({
                    required: {
                        params: true,
                        message: String.format(resourceProvider.MsgErrPleaseSelect, resourceProvider.Product)
                    }
                });
                self.IndustrialCustomerDecisionDet()[0].ContractCode = ko.observable("");
            }
            self.ProductCode.subscribe(function (productCode) {
                if (productCode) {
                    PopulateDropDownViaAjax("#Contract", "/Master/GetContractByProductCode?AProductCode=" + productCode);
                    if (self.CustomerId._latestValue != 0) {
                        self.ProductCode = ko.observable(productCode).extend({
                            required: {
                                params: true,
                                message:  String.format(resourceProvider.MsgErrPleaseSelect, resourceProvider.Product)
                            }
                        });
                        self.IndustrialCustomerDecisionDet()[0].ContractCode = ko.observable("");
                    }
                    else {
                        GetAutoApprovalOfSelectedProduct(productCode, self, false);
                        GetProductProperties(productCode, self, false);                      
                    }
                }
                else {
                    RefreshDropDown("Contract");
                    if (self.CustomerId._latestValue != 0) {
                        self.ProductCode = ko.observable("").extend({
                            required: {
                                params: true,
                                message: String.format(resourceProvider.MsgErrPleaseSelect, resourceProvider.Product)
                            }
                        });
                    }
                    else {
                        self.IsAutoApprove = ko.observable(false);
                        self.IndustrialCustomerDecisionDet()[0].BillCycleSysParamId = ko.observable(0);
                        self.IndustrialCustomerDecisionDet()[0].BillingCycleCode = ko.observable("");
                    }
                }                   
            }.bind(self));
        }
        else {
            RefreshDropDown("Product");
            RefreshDropDown("Contract");
        }
    }.bind(self));

    self.Nationality.subscribe(function (nationalityId) //added subscribe method for the dropdown element
    {
        if (nationalityId != null) {            
            // populates all Communa dropdowns based on newly selected nationality
            PopulateAllDropDownWithSameIDViaAjax("Communa", "/Master/GetCommunas?Id=" + nationalityId);
            PopulateAllDropDownWithSameIDViaAjax("ParticularAddrCommuna", "/Master/GetCommunas?Id=" + nationalityId);
            PopulateAllDropDownWithSameIDViaAjax("StatementAddrCommuna", "/Master/GetCommunas?Id=" + nationalityId);

            RefreshDropDown("City");
            RefreshDropDown("ParticularAddrCity");
            RefreshDropDown("StatementAddrCity");
            //new
            if (self.CustomerId._latestValue != 0) {
                var objCommunaList = $('[id = Communa]');
                $.each(objCommunaList, function (index) {
                    self.CommercialAddressDet()[index].CommunaCode = ko.observable("");
                    self.CommercialAddressDet()[index].CityCode = ko.observable("");
                });
                var objParticularAddrCommunaList = $('[id = ParticularAddrCommuna]');
                $.each(objParticularAddrCommunaList, function (index) {
                    self.ParticularAddressDet()[index].CommunaCode = ko.observable("");
                    self.ParticularAddressDet()[index].CityCode = ko.observable("");
                });
                var objStatementAddrCommunaList = $('[id = StatementAddrCommuna]');
                $.each(objStatementAddrCommunaList, function (index) {
                    self.StatementAddressDet()[index].CommunaCode = ko.observable("");
                    self.StatementAddressDet()[index].CityCode = ko.observable("");
                });
            }
            //---
        }
        else {            
            RefreshDropDown("Communa");
            RefreshDropDown("ParticularAddrCommuna");
            RefreshDropDown("StatementAddrCommuna");

            RefreshDropDown("City");
            RefreshDropDown("ParticularAddrCity");
            RefreshDropDown("StatementAddrCity");
        }

    }.bind(self));

    /*Commercial Address*/
    self.CommercialAddressDet = ko.observableArray();
    self.CommercialAddressDTO = ko.observableArray();
    if (value && value.ObjIndustrialCustomerAddressDTO) {
        $.each(value.ObjIndustrialCustomerAddressDTO, function (index, values) {
            if (values) {

                if (values["AddressTypeCode"] == "C") {
                    var itemValue = new CommercialAddress();
                    itemValue = values;
                    self.CommercialAddressDet.push({
                        AddressId: values.AddressId,
                        CustomerId: values.CustomerId,
                        AddressDesc: ko.observable(values ? values.AddressDesc : ""),
                        Village: ko.observable(values ?values.Village : ""),
                        CommunaCode: ko.observable(values ? values.CommunaCode : ""),
                        CityCode: ko.observable(values ? values.CityCode : ""),
                        Phone: ko.observable(values ? values.Phone : ""),
                        Mobile: ko.observable(values ? values.Mobile : ""),
                        EmailID: ko.observable(values ? values.EmailID : ""),
                        ResidenceType: ko.observable(values ? values.ResidenceType : "")
                    });
                }
            }
        });
    }
    else {
        var ItemValue = new CommercialAddress();
        self.CommercialAddressDet.push(ItemValue);
    }

    /*Particular Address*/
    self.ParticularAddressDet = ko.observableArray();
    self.ParticularAddressDTO = ko.observableArray();
    if (value && value.ObjIndustrialCustomerAddressDTO) {
        $.each(value.ObjIndustrialCustomerAddressDTO, function (index, values) {
            if (values) {
                if (values["AddressTypeCode"] == "PR") {
                    var itemValue = new ParticularAddress();
                    itemValue = values;
                    self.ParticularAddressDet.push({
                        AddressId: values.AddressId,
                        CustomerId: values.CustomerId,
                        AddressDesc: ko.observable(values ? values.AddressDesc : ""),
                        Village: ko.observable(values ? values.Village : ""),
                        CommunaCode: ko.observable(values ? values.CommunaCode : ""),
                        CityCode: ko.observable(values ? values.CityCode : ""),
                        Phone: ko.observable(values ? values.Phone : ""),
                        Mobile: ko.observable(values ? values.Mobile : ""),
                        EmailID: ko.observable(values ? values.EmailID : ""),
                        ResidenceType: ko.observable(values ? values.ResidenceType : "")
                    });
                }
            }
        });
    }
    else {
        var ItemValue = new ParticularAddress();
        self.ParticularAddressDet.push(ItemValue);
    }

    /*Statement Address*/
    self.StatementAddressDet = ko.observableArray();    
    if (value && value.ObjIndustrialCustomerAddressDTO) {
        $.each(value.ObjIndustrialCustomerAddressDTO, function (index, values) {
            if (values) {
                if (values["AddressTypeCode"] == "ST") {
                    var itemValue = new StatementAddress();
                    itemValue = values;
                    self.StatementAddressDet.push({
                        AddressId: values.AddressId,
                        CustomerId: values.CustomerId,
                        ReferenceAddressTypeCode: ko.observable(values ? values.ReferenceAddressTypeCode : ""),
                        AddressType: ko.observable(values ? values.AddressType : ""),
                        AddressDesc: ko.observable(values ? values.AddressDesc : ""),
                        Village: ko.observable(values ? values.Village : ""),
                        CommunaCode: ko.observable(values ? values.CommunaCode : ""),
                        CityCode: ko.observable(values ? values.CityCode : ""),
                        Phone: ko.observable(values ? values.Phone : ""),
                        Mobile: ko.observable(values ? values.Mobile : ""),
                        EmailID: ko.observable(values ? values.EmailID : ""),
                        Other: ko.observable(values ? values.Other : "")
                    });
                }
            }
        });
    }
    else {
        var ItemValue = new StatementAddress();
        self.StatementAddressDet.push(ItemValue);
    }

    /*Industrial Customer Decision*/
    self.IndustrialCustomerDecisionDet = ko.observableArray();
    self.ObjIndustrialCustomerDecisionDTO = ko.observableArray();

    if (value && value.ObjIndustrialCustomerDecisionDTO.length > 0) {
        $.each(value.ObjIndustrialCustomerDecisionDTO, function (index, values) {
            if (values) {
                var itemValue = new DecisionDetails();
                itemValue = values;
                self.IndustrialCustomerDecisionDet.push({
                    DecisionID: values.DecisionID,
                    CustomerId: values.CustomerId,
                    DecisionCode: ko.observable(values ? values.DecisionCode : ""),
                    ReasonCode: ko.observable(values ? values.ReasonCode : ""),
                    Remarks: ko.observable(values ? values.Remarks : ""),
                    ContractCode: ko.observable(values ? values.ContractCode : ""),
                    Unemployment: ko.observable(values ? values.Unemployment : ""),
                    IsContractAcceptedByCustomer: ko.observable(values ? values.IsContractAcceptedByCustomer : 0),
                    CreditLimit: ko.observable(values ? Globalize(resourceProvider.ResourceCulture).format(values.CreditLimit, resourceProvider.FormatAmountJSn0) : 0),
                    BillingCycleCode : ko.observable(values ? values.BillingCycleCode : ""),
                    BillCycleSysParamId : ko.observable(values ? values.BillCycleSysParamId : 0)

                });
            }
        });
    }
    else {
        var itemValue = new DecisionDetails();
        self.IndustrialCustomerDecisionDet.push(itemValue);
    }
}

function IndustrialCustomerVM(IndustrialCustomerData) {
    var self = this;

    self.CustomerErrors = ko.observable({});
    self.IndustrialCustomer = new IndustrialCustomer(IndustrialCustomerData);

    function SetAddressObject() {
        self.IndustrialCustomer.ObjIndustrialCustomerAddressDTO = ko.observableArray();
        var CommercialAddressNew = new Address();
        self.IndustrialCustomer.ObjIndustrialCustomerAddressDTO.push(CommercialAddressNew);
        self.IndustrialCustomer.ObjIndustrialCustomerAddressDTO()[0].AddressId = self.IndustrialCustomer.CommercialAddressDet()[0].AddressId,
        self.IndustrialCustomer.ObjIndustrialCustomerAddressDTO()[0].CustomerId = self.IndustrialCustomer.CommercialAddressDet()[0].CustomerId,
        self.IndustrialCustomer.ObjIndustrialCustomerAddressDTO()[0].ReferenceAddressTypeCode = ko.observable(self.IndustrialCustomer.CommercialAddressDet()[0] ? self.IndustrialCustomer.CommercialAddressDet()[0].ReferenceAddressTypeCode : ""),
        self.IndustrialCustomer.ObjIndustrialCustomerAddressDTO()[0].AddressTypeCode = ko.observable("C"),
        self.IndustrialCustomer.ObjIndustrialCustomerAddressDTO()[0].AddressDesc = ko.observable(self.IndustrialCustomer.CommercialAddressDet()[0] ? self.IndustrialCustomer.CommercialAddressDet()[0].AddressDesc : ""),
        self.IndustrialCustomer.ObjIndustrialCustomerAddressDTO()[0].Village = ko.observable(self.IndustrialCustomer.CommercialAddressDet()[0] ? self.IndustrialCustomer.CommercialAddressDet()[0].Village : ""),
        self.IndustrialCustomer.ObjIndustrialCustomerAddressDTO()[0].CommunaCode = ko.observable(self.IndustrialCustomer.CommercialAddressDet()[0] ? self.IndustrialCustomer.CommercialAddressDet()[0].CommunaCode : ""),
        self.IndustrialCustomer.ObjIndustrialCustomerAddressDTO()[0].CityCode = ko.observable(self.IndustrialCustomer.CommercialAddressDet()[0] ? self.IndustrialCustomer.CommercialAddressDet()[0].CityCode : ""),
        self.IndustrialCustomer.ObjIndustrialCustomerAddressDTO()[0].Phone = ko.observable(self.IndustrialCustomer.CommercialAddressDet()[0] ? self.IndustrialCustomer.CommercialAddressDet()[0].Phone : ""),
        self.IndustrialCustomer.ObjIndustrialCustomerAddressDTO()[0].Mobile = ko.observable(self.IndustrialCustomer.CommercialAddressDet()[0] ? self.IndustrialCustomer.CommercialAddressDet()[0].Mobile : ""),
        self.IndustrialCustomer.ObjIndustrialCustomerAddressDTO()[0].EmailID = ko.observable(self.IndustrialCustomer.CommercialAddressDet()[0] ? self.IndustrialCustomer.CommercialAddressDet()[0].EmailID : ""),
        self.IndustrialCustomer.ObjIndustrialCustomerAddressDTO()[0].ResidenceType = ko.observable(self.IndustrialCustomer.CommercialAddressDet()[0] ? self.IndustrialCustomer.CommercialAddressDet()[0].ResidenceType : ""),
        self.IndustrialCustomer.ObjIndustrialCustomerAddressDTO()[0].Other = ko.observable(self.IndustrialCustomer.CommercialAddressDet()[0] ? self.IndustrialCustomer.CommercialAddressDet()[0].Other : "")


        var ParticularAddressNew = new Address();
        self.IndustrialCustomer.ObjIndustrialCustomerAddressDTO.push(ParticularAddressNew);
        self.IndustrialCustomer.ObjIndustrialCustomerAddressDTO()[1].AddressId = self.IndustrialCustomer.ParticularAddressDet()[0].AddressId,
        self.IndustrialCustomer.ObjIndustrialCustomerAddressDTO()[1].CustomerId = self.IndustrialCustomer.ParticularAddressDet()[0].CustomerId,
        self.IndustrialCustomer.ObjIndustrialCustomerAddressDTO()[1].ReferenceAddressTypeCode = ko.observable(self.IndustrialCustomer.ParticularAddressDet()[0] ? self.IndustrialCustomer.ParticularAddressDet()[0].ReferenceAddressTypeCode : ""),
        self.IndustrialCustomer.ObjIndustrialCustomerAddressDTO()[1].AddressTypeCode = ko.observable("PR"),
        self.IndustrialCustomer.ObjIndustrialCustomerAddressDTO()[1].AddressDesc = ko.observable(self.IndustrialCustomer.ParticularAddressDet()[0] ? self.IndustrialCustomer.ParticularAddressDet()[0].AddressDesc : ""),
        self.IndustrialCustomer.ObjIndustrialCustomerAddressDTO()[1].Village = ko.observable(self.IndustrialCustomer.ParticularAddressDet()[0] ? self.IndustrialCustomer.ParticularAddressDet()[0].Village : ""),
        self.IndustrialCustomer.ObjIndustrialCustomerAddressDTO()[1].CommunaCode = ko.observable(self.IndustrialCustomer.ParticularAddressDet()[0] ? self.IndustrialCustomer.ParticularAddressDet()[0].CommunaCode : ""),
        self.IndustrialCustomer.ObjIndustrialCustomerAddressDTO()[1].CityCode = ko.observable(self.IndustrialCustomer.ParticularAddressDet()[0] ? self.IndustrialCustomer.ParticularAddressDet()[0].CityCode : ""),
        self.IndustrialCustomer.ObjIndustrialCustomerAddressDTO()[1].Phone = ko.observable(self.IndustrialCustomer.ParticularAddressDet()[0] ? self.IndustrialCustomer.ParticularAddressDet()[0].Phone : ""),
        self.IndustrialCustomer.ObjIndustrialCustomerAddressDTO()[1].Mobile = ko.observable(self.IndustrialCustomer.ParticularAddressDet()[0] ? self.IndustrialCustomer.ParticularAddressDet()[0].Mobile : ""),
        self.IndustrialCustomer.ObjIndustrialCustomerAddressDTO()[1].EmailID = ko.observable(self.IndustrialCustomer.ParticularAddressDet()[0] ? self.IndustrialCustomer.ParticularAddressDet()[0].EmailID : ""),
        self.IndustrialCustomer.ObjIndustrialCustomerAddressDTO()[1].ResidenceType = ko.observable(self.IndustrialCustomer.ParticularAddressDet()[0] ? self.IndustrialCustomer.ParticularAddressDet()[0].ResidenceType : ""),
        self.IndustrialCustomer.ObjIndustrialCustomerAddressDTO()[1].Other = ko.observable(self.IndustrialCustomer.ParticularAddressDet()[0] ? self.IndustrialCustomer.ParticularAddressDet()[0].Other : "")

        var StatementAddressNew = new Address();
        self.IndustrialCustomer.ObjIndustrialCustomerAddressDTO.push(StatementAddressNew);
        self.IndustrialCustomer.ObjIndustrialCustomerAddressDTO()[2].AddressId = self.IndustrialCustomer.StatementAddressDet()[0].AddressId,
        self.IndustrialCustomer.ObjIndustrialCustomerAddressDTO()[2].CustomerId = self.IndustrialCustomer.StatementAddressDet()[0].CustomerId,
        self.IndustrialCustomer.ObjIndustrialCustomerAddressDTO()[2].ReferenceAddressTypeCode = ko.observable(self.IndustrialCustomer.StatementAddressDet()[0] ? self.IndustrialCustomer.StatementAddressDet()[0].ReferenceAddressTypeCode : ""),
        self.IndustrialCustomer.ObjIndustrialCustomerAddressDTO()[2].AddressTypeCode = ko.observable("ST"),
        self.IndustrialCustomer.ObjIndustrialCustomerAddressDTO()[2].AddressDesc = ko.observable(self.IndustrialCustomer.StatementAddressDet()[0] ? self.IndustrialCustomer.StatementAddressDet()[0].AddressDesc : ""),
        self.IndustrialCustomer.ObjIndustrialCustomerAddressDTO()[2].Village = ko.observable(self.IndustrialCustomer.StatementAddressDet()[0] ? self.IndustrialCustomer.StatementAddressDet()[0].Village : ""),
        self.IndustrialCustomer.ObjIndustrialCustomerAddressDTO()[2].CommunaCode = ko.observable(self.IndustrialCustomer.StatementAddressDet()[0] ? self.IndustrialCustomer.StatementAddressDet()[0].CommunaCode : ""),
        self.IndustrialCustomer.ObjIndustrialCustomerAddressDTO()[2].CityCode = ko.observable(self.IndustrialCustomer.StatementAddressDet()[0] ? self.IndustrialCustomer.StatementAddressDet()[0].CityCode : ""),
        self.IndustrialCustomer.ObjIndustrialCustomerAddressDTO()[2].Phone = ko.observable(self.IndustrialCustomer.StatementAddressDet()[0] ? self.IndustrialCustomer.StatementAddressDet()[0].Phone : ""),
        self.IndustrialCustomer.ObjIndustrialCustomerAddressDTO()[2].Mobile = ko.observable(self.IndustrialCustomer.StatementAddressDet()[0] ? self.IndustrialCustomer.StatementAddressDet()[0].Mobile : ""),
        self.IndustrialCustomer.ObjIndustrialCustomerAddressDTO()[2].EmailID = ko.observable(self.IndustrialCustomer.StatementAddressDet()[0] ? self.IndustrialCustomer.StatementAddressDet()[0].EmailID : ""),
        self.IndustrialCustomer.ObjIndustrialCustomerAddressDTO()[2].ResidenceType = ko.observable(self.IndustrialCustomer.StatementAddressDet()[0] ? self.IndustrialCustomer.StatementAddressDet()[0].ResidenceType : ""),
        self.IndustrialCustomer.ObjIndustrialCustomerAddressDTO()[2].Other = ko.observable(self.IndustrialCustomer.StatementAddressDet()[0] ? self.IndustrialCustomer.StatementAddressDet()[0].Other : "")
    }

    //To open error containg panel before save(one by one)
    function OpenErrorContainingPanelOneByOne() {

        var CustomerIdentificationErrors = [
             String.format(resourceProvider.MsgErrPleaseSelect, resourceProvider.CustomerType),
             String.format(resourceProvider.MsgErrPleaseEnter2, resourceProvider.Name),
             String.format(resourceProvider.MsgErrPleaseSelect, resourceProvider.ProductCategory),
             String.format(resourceProvider.MsgErrPleaseSelect, resourceProvider.Product)];
      
        var IndustrialCustomerError = ko.observable({});
        IndustrialCustomerError = ko.validation.group(self.IndustrialCustomer, { deep: false });        

        if (IndustrialCustomerError._latestValue.filter(FilterNull).length > 0) {
            $.each(IndustrialCustomerError._latestValue.filter(FilterNull), function (index) {
                var item = IndustrialCustomerError._latestValue.filter(FilterNull)[index];

                if (CustomerIdentificationErrors.filter(function (element) { return element != item; }).length != CustomerIdentificationErrors.length) {
                    $("#accordion").accordion({
                        active: 1
                    });
                    if (self.IndustrialCustomer.CustomerId() != 0) {
                        if ($('#Product').val() == '') {
                            $('#lblValidateProduct').text(String.format(resourceProvider.MsgErrPleaseSelect, resourceProvider.Product));
                        }
                        else {
                            if ($('#lblValidateProduct').text().length > 0) {
                                $('#lblValidateProduct').text('');
                            }
                        }
                    }
                    return false;
                }
            });
        }

        if ($('#lblValidateCreditLimit').text().length > 0) {
                $("#accordion").accordion({
                    active: 5
                });
                return false;
        }
        
        return true;
        //---------------------------------------------------------------
    }

    function OpenErrorContainingPanelOneByOneOnSubmit() {

        var IndustrialCustomerErrorCount = 0;
        var ParticularAddressErrorCount = 0;
        var StatementAddressErrorCount = 0;
        var DecisionErrorCount = 0;

        var CustomerIdentificationErrors = [
            String.format(resourceProvider.MsgErrPleaseSelect, resourceProvider.CustomerType),
            String.format(resourceProvider.MsgErrPleaseEnter2, resourceProvider.Name),
            String.format(resourceProvider.MsgErrPleaseSelect, resourceProvider.ProductCategory),
            String.format(resourceProvider.MsgErrPleaseSelect, resourceProvider.Product)];

        var IndustrialCustomerError = ko.observable({});
        IndustrialCustomerError = ko.validation.group(self.IndustrialCustomer, { deep: false });

        if (IndustrialCustomerError._latestValue.filter(FilterNull).length > 0) {
            $.each(IndustrialCustomerError._latestValue.filter(FilterNull), function (index) {
                var item = IndustrialCustomerError._latestValue.filter(FilterNull)[index];
                if (CustomerIdentificationErrors.filter(function (element) { return element != item; }).length != CustomerIdentificationErrors.length) {
                    IndustrialCustomerErrorCount += 1;
                }
            });
        }

        if (self.IndustrialCustomer.CustomerId() != 0) {

            if ($('#Product').val() == '') {
                $('#lblValidateProduct').text(String.format(resourceProvider.MsgErrPleaseSelect, resourceProvider.Product));
                IndustrialCustomerErrorCount += 1;
            }
            else {
                if ($('#lblValidateProduct').text().length > 0) {
                    $('#lblValidateProduct').text('');
                }
            }
        }

            if ($('#Nationality').val() == '') {
                $('#lblValidateNationality').text(String.format(resourceProvider.MsgErrPleaseSelect, resourceProvider.Nationality));
                IndustrialCustomerErrorCount += 1;
            }
            else {
                if ($('#lblValidateNationality').text().length > 0) {
                    $('#lblValidateNationality').text('');
                }
            }

            if ($('#CommercialAddress').val() == '') {
                $('#lblValidateCommercialAddress').text(String.format(resourceProvider.MsgErrPleaseEnter2, resourceProvider.CommercialAddress));
                IndustrialCustomerErrorCount += 1;
            }
            else {
                if ($('#lblValidateCommercialAddress').text().length > 0) {
                    $('#lblValidateCommercialAddress').text('');
                }
            }

            if ($('#Communa').val() == '') {
                $('#lblValidateCommuna').text(String.format(resourceProvider.MsgErrPleaseSelect, resourceProvider.Communa));
                IndustrialCustomerErrorCount += 1;
            }
            else {
                if ($('#lblValidateCommuna').text().length > 0) {
                    $('#lblValidateCommuna').text('');
                }
            }

            if ($('#City').val() == '') {
                $('#lblValidateCity').text(String.format(resourceProvider.MsgErrPleaseSelect, resourceProvider.City));
                IndustrialCustomerErrorCount += 1;
            }
            else {
                if ($('#lblValidateCity').text().length > 0) {
                    $('#lblValidateCity').text('');
                }
            }

            if (($('#DateOfBirth').val() == null || $('#DateOfBirth').val() == '') && $('#CustomerType').val() == 'I') {
                $('#lblValidateDateOfBirth').text(String.format(resourceProvider.MsgErrPleaseSelect, resourceProvider.DateOfBirth));
                IndustrialCustomerErrorCount += 1;
            }
            else {
                if ($('#lblValidateDateOfBirth').text().length > 0) {
                    $('#lblValidateDateOfBirth').text('');
                }
            }

            if ($('#ParticularAddress').val() == '') {
                $('#lblValidateParticularAddress').text(String.format(resourceProvider.MsgErrPleaseEnter2, resourceProvider.Address));
                ParticularAddressErrorCount += 1;
            }
            else {
                if ($('#lblValidateParticularAddress').text().length > 0) {
                    $('#lblValidateParticularAddress').text('');
                }
            }

            if ($('#ParticularAddrCommuna').val() == '') {
                $('#lblValidateParticularAddrCommuna').text(String.format(resourceProvider.MsgErrPleaseSelect, resourceProvider.Communa));
                ParticularAddressErrorCount += 1;
            }
            else {
                if ($('#lblValidateParticularAddrCommuna').text().length > 0) {
                    $('#lblValidateParticularAddrCommuna').text('');
                }
            }

            if ($('#ParticularAddrCity').val() == '') {
                $('#lblValidateParticularAddrCity').text(String.format(resourceProvider.MsgErrPleaseSelect, resourceProvider.City));
                ParticularAddressErrorCount += 1;
            }
            else {
                if ($('#lblValidateParticularAddrCity').text().length > 0) {
                    $('#lblValidateParticularAddrCity').text('');
                }
            }
            
            if (self.IndustrialCustomer.ParentCustomerId() == 0) {

                if ($('#SendStatementTo').val() == '') {
                    $('#lblValidateSendStatementTo').text(String.format(resourceProvider.MsgErrPleaseSelect, resourceProvider.SendStatementTo));
                    StatementAddressErrorCount += 1;
                }
                else {
                    if ($('#lblValidateSendStatementTo').text().length > 0) {
                        $('#lblValidateSendStatementTo').text('');
                    }
                }

                if ($('#StatementAddrAddress').val() == '') {
                    $('#lblValidateStatementAddrAddress').text(String.format(resourceProvider.MsgErrPleaseEnter2, resourceProvider.Address));
                    StatementAddressErrorCount += 1;
                }
                else {
                    if ($('#lblValidateStatementAddrAddress').text().length > 0) {
                        $('#lblValidateStatementAddrAddress').text('');
                    }
                }

                if ($('#StatementAddrCommuna').val() == '') {
                    $('#lblValidateStatementAddrCommuna').text(String.format(resourceProvider.MsgErrPleaseSelect, resourceProvider.Communa));
                    StatementAddressErrorCount += 1;
                }
                else {
                    if ($('#lblValidateStatementAddrCommuna').text().length > 0) {
                        $('#lblValidateStatementAddrCommuna').text('');
                    }
                }

                if ($('#StatementAddrCity').val() == '') {
                    $('#lblValidateStatementAddrCity').text(String.format(resourceProvider.MsgErrPleaseSelect, resourceProvider.City));
                    StatementAddressErrorCount += 1;
                }
                else {
                    if ($('#lblValidateStatementAddrCity').text().length > 0) {
                        $('#lblValidateStatementAddrCity').text('');
                    }
                }
            }

            if ($('#Decision').val() == '') {
                $('#lblValidateDecision').text(String.format(resourceProvider.MsgErrPleaseSelect, resourceProvider.Decision));
                DecisionErrorCount += 1;
            }
            else {
                if ($('#lblValidateDecision').text().length > 0) {
                    $('#lblValidateDecision').text('');
                }
            }

            if ($('#Reason').val() == '') {
                $('#lblValidateReason').text(String.format(resourceProvider.MsgErrPleaseSelect, resourceProvider.Reason));
                DecisionErrorCount += 1;
            }
            else {
                if ($('#lblValidateReason').text().length > 0) {
                    $('#lblValidateReason').text('');
                }
            }

            if ($('#Contract').val() == '') {
                $('#lblValidateContract').text(String.format(resourceProvider.MsgErrPleaseSelect, resourceProvider.Contract));
                DecisionErrorCount += 1;
            }
            else {
                if ($('#lblValidateContract').text().length > 0) {
                    $('#lblValidateContract').text('');
                }
            }

            if ($('#BillCycle').val() == '') {
                $('#lblValidateBillCycle').text(String.format(resourceProvider.MsgErrPleaseSelect, resourceProvider.BillingCycleCode));
                DecisionErrorCount += 1;
            }
            else {
                if ($('#lblValidateBillCycle').text().length > 0) {
                    $('#lblValidateBillCycle').text('');
                }
            }

        // if (Globalize(resourceProvider.ResourceCulture).parseFloat($('#CreditLimit').val().trim()) == parseFloat(0) || $('#CreditLimit').val().trim() == '') {
            if ( $('#CreditLimit').val().trim() == '') {
                $('#lblValidateCreditLimit').text(String.format(resourceProvider.MsgErrPleaseEnter2, resourceProvider.CreditLimit));
                DecisionErrorCount += 1;
            }
            else {
                if ($('#lblValidateCreditLimit').text().length > 0) {
                    DecisionErrorCount += 1;
                }
                else {
                    $('#lblValidateCreditLimit').text('');
                }
            }

            if ($('#Remarks').val() == '') {
                $('#lblValidateRemarks').text(String.format(resourceProvider.MsgErrPleaseEnter2, resourceProvider.Remarks));
                DecisionErrorCount += 1;
            }
            else {
                $('#lblValidateRemarks').text('');
            }

            if (IndustrialCustomerErrorCount > 0) {
                $("#accordion").accordion({
                    active: 1
                });
                return false;
            }
            if (ParticularAddressErrorCount > 0) {
                $("#accordion").accordion({
                    active: 2
                });
                return false;
            }
            if (StatementAddressErrorCount > 0) {
                $("#accordion").accordion({
                    active: 3
                });
                return false;
            }
            if (DecisionErrorCount > 0) {
                $("#accordion").accordion({
                    active: 5
                });
                return false;
            }
            return true;
        //}
        //---------------------------------------------------------------
    }

    function IndustrialCustomerSubmit() {
        SetAddressObject();
        self.IndustrialCustomer.ObjIndustrialCustomerDecisionDTO = self.IndustrialCustomer.IndustrialCustomerDecisionDet;
        $.ajax({
            url: '/Master/IndustrialCustomerSave/',
            type: 'POST',
            dataType: 'json',
            data: ko.toJSON({ AIndustrialCustomerDTO: self.IndustrialCustomer, ACommand: resourceProvider.Submit }),
            contentType: 'application/json; charset=utf-8',
            success: function (returnData) {
                ShowMessage(returnData.RedirectUrl, returnData.Message);
            }
        });
    }

    self.Save = function () {
        var isValid = true;
        self.CustomerErrors = ko.validation.group(self.IndustrialCustomer, { deep: true });
        //To open error containg panel before save(one by one)
        isValid = OpenErrorContainingPanelOneByOne();
        //----------------------------------------------------
        if (self.CustomerErrors._latestValue.filter(FilterNull).length != 0) {
            self.CustomerErrors.showAllMessages(true);
            isValid = false;
        }

        if (isValid) {

            SetAddressObject();
            self.IndustrialCustomer.ObjIndustrialCustomerDecisionDTO = self.IndustrialCustomer.IndustrialCustomerDecisionDet;

            var json1 = ko.toJSON(self.IndustrialCustomer);

            $.ajax({
                url: '/Master/IndustrialCustomerSave/',
                type: 'POST',
                dataType: 'json',
                data: ko.toJSON({ AIndustrialCustomerDTO: self.IndustrialCustomer, ACommand: resourceProvider.Save }),
                contentType: 'application/json; charset=utf-8',
                success: function (returnData) {
                    ShowMessage(returnData.RedirectUrl, returnData.Message);
                }
            });
        }
    };  

    self.Submit = function () {
        var isValid = true;
        self.CustomerErrors = ko.validation.group(self.IndustrialCustomer, { deep: true });
        //To open error containg panel before save(one by one)
        isValid = OpenErrorContainingPanelOneByOneOnSubmit();
        if (self.CustomerErrors._latestValue.filter(FilterNull).length != 0) {
            self.CustomerErrors.showAllMessages(true);
            isValid = false;
        }
        //----------------------------------------------------
        if (isValid) {

            var productCode = self.IndustrialCustomer.ProductCode();
            var creditScore = self.IndustrialCustomer.CreditBureauScore();
            if (creditScore == "") {
                creditScore = 0;
            }
            /*Confimation before submit*/
            $("#dialog").dialog({
                resizable: false,
                height: 160,
                modal: true,
                title: resourceProvider.ApplicationName,
                buttons: [{
                    class: "LeftButton",
                    text: resourceProvider.Yes,
                    click: function () {
                        $(this).dialog("close");

                        if (self.IndustrialCustomer.ParentCustomerId() != 0) {
                            self.IndustrialCustomer.NeedToSendToApprovalQueue = ko.observable(false);
                            IndustrialCustomerSubmit();
                        }
                        else {

                            if (self.IndustrialCustomer.IsAutoApprove() == false) {

                                /*  Validating Credit Score */
                                $.ajax({
                                    type: 'POST',
                                    url: '/Master/ValidateCreditScore',
                                    data: { AProductCode: productCode, ACreditScore: creditScore, AIsRequestFromUnderWriter: false, AIsAutoApproval: self.IndustrialCustomer.IsAutoApprove() },
                                    success: function (returnData) {
                                        $('#dialog').text("");

                                        if (returnData.isCreditScoreViolated) {
                                            $('#dialog').dialog({
                                                resizable: false,
                                                modal: true,
                                                title: resourceProvider.ApplicationName,
                                                height: 180,
                                                buttons: [{
                                                    text: resourceProvider.OK,
                                                    class: "ButtonStyle",
                                                    width: "100%",
                                                    click: function () {
                                                        $(this).dialog("close");
                                                        IndustrialCustomerSubmit();
                                                    }
                                                }]
                                            });
                                            if (returnData.Message.length > 0) {
                                                $('#dialog').text(returnData.Message);
                                            }
                                        }
                                        else {
                                            IndustrialCustomerSubmit();
                                        }
                                    }
                                });
                                /*-------------------*/
                            }
                            else {
                                /*  Validating Credit Score */
                                $.ajax({
                                    type: 'POST',
                                    url: '/Master/ValidateCreditScore',
                                    data: { AProductCode: productCode, ACreditScore: creditScore, AIsRequestFromUnderWriter: false, AIsAutoApproval: self.IndustrialCustomer.IsAutoApprove() },
                                    success: function (returnData) {
                                        $('#dialog').text("");

                                        if (returnData.isCreditScoreViolated) {
                                            $('#dialog').dialog({
                                                resizable: false,
                                                modal: true,
                                                title: resourceProvider.ApplicationName,
                                                height: 180,
                                                buttons: [{
                                                    class: "LeftButtonStyle",
                                                    text: resourceProvider.Yes,
                                                    width: 50,
                                                    click: function () {
                                                        $(this).dialog("close");
                                                        self.IndustrialCustomer.NeedToSendToApprovalQueue = ko.observable(true);
                                                        IndustrialCustomerSubmit();
                                                    }
                                                },
                                                {
                                                    text: resourceProvider.No,
                                                    class: "ButtonStyle",
                                                    width: 50,
                                                    click: function () {
                                                        $(this).dialog("close");
                                                        self.IndustrialCustomer.NeedToSendToApprovalQueue = ko.observable(false);
                                                        IndustrialCustomerSubmit();
                                                    }
                                                }]
                                            });
                                            if (returnData.Message.length > 0) {
                                                $('#dialog').text(returnData.Message);
                                            }
                                        }
                                        else {
                                            IndustrialCustomerSubmit();
                                        }
                                    }
                                });
                                /*-------------------*/
                            }
                        }
                    }
                },
                    {
                        text: resourceProvider.No,
                        click: function () {
                            $(this).dialog("close");
                        }
                    }]
            });

            $("#dialog").text(resourceProvider.MsgConfSubmit);
            /*------------------*/
        }
    };

    self.Delete = function () {
        /*Confimation before delete*/
        $("#dialog").dialog({
            resizable: false,
            height: 160,
            modal: true,
            title: resourceProvider.ApplicationName,
            buttons: [{
                class: "LeftButton",
                text: resourceProvider.Yes,
                click: function () {
                    $(this).dialog("close");

                    /*Deleting the record*/
                    $.ajax({
                        url: '/Master/IndustrialCustomerDelete/',
                        type: 'POST',
                        dataType: 'json',
                        data: ko.toJSON({ ACustomerId: self.IndustrialCustomer.CustomerId, AIsDeleteCallFromSearchTab: false }),
                        contentType: 'application/json; charset=utf-8',
                        success: function (returnData) {
                            ShowMessage(returnData.RedirectUrl, returnData.Message);
                        }
                    });
                    /*-------------------*/
                }
            },
                {
                    text: resourceProvider.No,
                    click: function () {
                        $(this).dialog("close");
                    }
                }]
        });

        $("#dialog").text(resourceProvider.MsgConfDelete);
    };

    self.Cancel = function () {
        window.location.reload();
    };

    self.UnderwriterAuthorize = function () {
        var isValid = true;
        var errorCount = 0;
      
        var productCode = self.IndustrialCustomer.ProductCode;
        var creditScore = self.IndustrialCustomer.CreditBureauScore;
        var customerId = self.IndustrialCustomer.CustomerId;
        var creditLimit = self.IndustrialCustomer.IndustrialCustomerDecisionDet()[0]["CreditLimit"];
        var remarks = self.IndustrialCustomer.IndustrialCustomerDecisionDet()[0]["Remarks"];

        //Validation for Remark and Creditlimit
        // if (creditLimit() == 0 || creditLimit() == '') {
        if (creditLimit() == '') {
            $('#lblValidateCreditLimit').text(String.format(resourceProvider.MsgErrPleaseEnter2, resourceProvider.CreditLimit));
            errorCount += 1;
        }
        else {
            if ($('#lblValidateCreditLimit').text().length > 0) {
                errorCount += 1;
            }
            else {
                $('#lblValidateCreditLimit').text('');
            }
        }

        if (remarks() == '') {
            $('#lblValidateRemarks').text(String.format(resourceProvider.MsgErrPleaseEnter2, resourceProvider.Remarks));
            errorCount += 1;
        }
        else {
            $('#lblValidateRemarks').text('');
        }

        if (errorCount > 0) {
            $("#accordion").accordion({
                active: 5
            });
            isValid = false;
        }

        if (isValid) {
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
                                            $("#UnderWriterAuthorizeDiv1").html(data);

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
                                $("#UnderWriterAuthorizeDiv1").html(data);
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

function closeloading() {
    $("div.loader").fadeOut('normal');
}

var popupStatus = 0; // set value

function loadPopup() {
    if (popupStatus == 0) { // if value is 0, show popup
        closeloading(); // fadeout loading
        $("#authPopup").fadeIn(0500); // fadein popup div
        $("#backgroundPopup").css("opacity", "0.7"); // css opacity, supports IE7, IE8
        $("#backgroundPopup").fadeIn(0001);
        popupStatus = 1; // and set value to 1
    }
}

function disablePopup() {
    if (popupStatus == 1) { // if value is 1, close popup
        $("#authPopup").fadeOut("normal");
        $("#backgroundPopup").fadeOut("normal");
        popupStatus = 0;  // and set value to 0
        $('#ContextDesc_validate1').text("");
        $('#ReasonCode_validate1').text("");
    }
}

function ShowMessage(RedirectUrl, MessageToShow) {
    if (MessageToShow != "") {
        $('#dialog').text("");
        $('#dialog').dialog({
            closeOnEscape: false,
            open: function(event, ui) {
                $(this).closest('.ui-dialog').find('.ui-dialog-titlebar-close').hide();
            },
            resizable: false,
            modal: true,
            title: resourceProvider.ApplicationName,
            buttons: [{
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

function FilterNull(element) {
    return element != null;
}

