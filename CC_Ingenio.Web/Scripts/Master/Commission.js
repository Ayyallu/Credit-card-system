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

function PopulateContractCascadeDDL(valueToSet, viewModelObj) {

    var productCode = $("#Product").val();
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
                select.val(valueToSet.ContractCode);
                
                viewModelObj.CommissionCharge.ContractCode = ko.observable(valueToSet.ContractCode).extend({
                    required: {
                        params: true,
                        message: String.format(resourceProvider.MsgErrPleaseSelect, resourceProvider.Contract)
                    }
                });
            });
    }
}

function setAmount(commissionCode) {
    //alert(commissionCode);
    var amt = '4';
    if (commissionCode)
    {
        amt = curamt(commissionCode);

        return parseInt(curamt(commissionCode))
    }
    //return parseInt(amt)
    //return parseInt("4")
};



function curamt(commissionCode)
{
    var result = "";
    $.ajax({
        type: 'post',
        url: '/Master/GetCommissionAmountByCommissionCode/',
        async: false,
        data: { ACommissionCode: commissionCode },
        success: function (returnData) {
            if (returnData.CommissionAmount) {
                result = returnData.CommissionAmount;
            }
        }
    });
    return result;
}


function ShowMessage(RedirectUrl, MessageToShow) {
    if (MessageToShow != "") {
        $('#dialog').text("");
        $('#dialog').dialog({
            closeOnEscape: false,
            open: function (event, ui) {
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

var checkMaxLimit = function (val, max) {
    val = Globalize(resourceProvider.ResourceCulture).parseFloat(val);
    if (!isNaN(max))
        return parseFloat(max) >= parseFloat(val);
};

function CommissionChargeDetails(value) {
    var self = this;
    if (value != null && value != "" && !isNaN(value))
        self.CommissionChargeId = ko.observable(value);
    else
        self.CommissionChargeId = ko.observable(0);

    //self.CommissionChargeId = ko.observable(0);
    self.CommissionChargeDetailsId = ko.observable(0);
    self.CommissionCode = ko.observable("").extend({
        required: {
            params: true,
            message: String.format(resourceProvider.MsgErrPleaseSelect, resourceProvider.Commission)
        }
    });
    self.CommissionAmount = ko.observable("").extend({
        required: {
            params: true,
            message: String.format(resourceProvider.MsgErrPleaseEnter2, resourceProvider.CommissionAmount)
        },
        validation: {
            validator: checkMaxLimit, message: String.format(resourceProvider.MsgErrValueShouldBeLessThan, resourceProvider.CommissionAmount, "999999999"), params: 999999999
        }
    });
    //self.CommissionAmount = ko.computed(function () {
    //    return setAmount(self.CommissionCode())
    //})
}


function CommissionCharge(value)
{
    var self = this; // CommissionCharge object
    self.CommissionChargeId = ko.observable(value ? value.CommissionChargeId : "");
    
    self.ProductCode = ko.observable(value ? value.ProductCode : "").extend({
        required: {
            params: true,
            message: String.format(resourceProvider.MsgErrPleaseSelect, resourceProvider.Product)
        }
    });
    self.ContractCode = ko.observable(value ? value.ContractCode : "").extend({
        required: {
            params: true,
            message: String.format(resourceProvider.MsgErrPleaseSelect, resourceProvider.Contract)
        }
    });
    self.CurrencyCode = ko.observable(value ? value.CurrencyCode : "").extend({
        required: {
            params: true,
            message: String.format(resourceProvider.MsgErrPleaseSelect, resourceProvider.Currency)
        }
    });

    self.ProductCode.subscribe(function (productCode) {
        if (productCode) {
            //self.ContractCode = ko.observable("").extend({ required: { params: true, message: "Please select Contract" } });
            PopulateDropDownViaAjax("#Contract", "/Master/GetContractByProductCode?AProductCode=" + productCode);
            
            if (self.CommissionChargeId._latestValue > 0) {
                self.ContractCode = ko.observable("").extend({
                    required: {
                        params: true,
                        message: String.format(resourceProvider.MsgErrPleaseSelect, resourceProvider.Contract)
                    }
                });
            }
        }
        else {
            RefreshDropDown("Contract");
        }
    }.bind(self));
 

    self.CommissionChargeDetailsData = ko.observableArray();
    self.CommissionChargeDetailsDTO = ko.observableArray();
  
    if (value && value.CommissionChargeDetailsDTO)
    {
        $.each( value.CommissionChargeDetailsDTO, function (index, values){
            if (values) {
                var itemValue = new CommissionChargeDetails();
                itemValue = values;
                self.CommissionChargeDetailsData.push({
                    CommissionChargeId: values.CommissionChargeId,
                    CommissionChargeDetailsId: values.CommissionChargeDetailsId,
                    CommissionCode: ko.observable(values.CommissionCode).extend({
                        required: {
                            params: true,
                            message: String.format(resourceProvider.MsgErrPleaseSelect, resourceProvider.Commision)
                        }
                    }),
                    CommissionAmount: ko.observable(values ?
                        Globalize(resourceProvider.ResourceCulture).format(values.CommissionAmount, (self.CurrencyCode._latestValue == resourceProvider.UFCurrencyCode ? resourceProvider.FormatAmountJSn5 : resourceProvider.FormatAmountJSn0)) : "").extend({
                            required: { params: true, message: String.format(resourceProvider.MsgErrPleaseEnter2, resourceProvider.CommissionAmount) },
                            validation: {
                                validator: checkMaxLimit, message: String.format(resourceProvider.MsgErrValueShouldBeLessThan, resourceProvider.CommissionAmount, "999999999"), params: 999999999
                            }
                        }),
                    //CommissionAmount: ko.computed(function () {
                    //    return setAmount(values.CommissionCode)
                    //})
                    ConvertedCommissionAmount: ko.computed(function () {
                        return self.CurrencyCode() == resourceProvider.UFCurrencyCode ? values.CommissionAmount * resourceProvider.CurrentUFRate : values.CommissionAmount
                    })

                });

                //self.CommissionChargeDetailsData._latestValue[index].CommissionAmount = ko.computed(function () {
                //    return setAmount(self.CommissionChargeDetailsData._latestValue[index].CommissionCode());
                //});
                
                self.CommissionChargeDetailsData._latestValue[index].ConvertedCommissionAmount = ko.computed(function () {
                    return self.CurrencyCode() == resourceProvider.UFCurrencyCode ? self.CommissionChargeDetailsData._latestValue[index].CommissionAmount() * resourceProvider.CurrentUFRate : self.CommissionChargeDetailsData._latestValue[index].CommissionAmount()
                });

                }
        })

        
    }
    else {
        var itemValue = new CommissionChargeDetails();
        self.CommissionChargeDetailsData.push(itemValue);
    }
}


function CommissionChargeVM(CommissionChargeData){
    var self = this;
    self.CommissionCharge = new CommissionCharge(CommissionChargeData);
    self.CommissionChargeDetails = new CommissionChargeDetails();
        
    self.Submit = function ()
    {
        var isValid = true;
        //DD
        var CommissionChargeError = ko.observable({});
        CommissionChargeError = ko.validation.group(self.CommissionCharge, { deep: false });
        if (CommissionChargeError._latestValue.filter(function (element) { return element != null; }).length > 0) {
            if (self.CommissionCharge.CommissionChargeId._latestValue > 0) {
                if ($('#Contract').val() == '') {
                    $('#lblValidateContract').text(String.format(resourceProvider.MsgErrPleaseSelect, resourceProvider.Contract));
                    isValid = false;
                }
            }
        }
        //--

        //Asigning template values into child DTO
        self.CommissionCharge.CommissionChargeDetailsDTO = self.CommissionCharge.CommissionChargeDetailsData;

        self.CommissionChargeErrors = ko.validation.group(self.CommissionCharge, { deep: true });        
        if (self.CommissionChargeErrors._latestValue.filter(function (element) { return element != null; }).length != 0) {

            self.CommissionChargeErrors.showAllMessages(true);
            isValid = false;
        }
        else {
            if (self.CommissionCharge.CommissionChargeDetailsDTO().length == 0) {
                ShowMessage("", String.format(resourceProvider.MsgErrOneValid, resourceProvider.CommissionCharge));
                isValid = false;
            }
        }
               
        if (isValid)
        {
            $("#dialog").dialog({
                resizable: false,
                height: 160,
                modal: true,
                title: resourceProvider.ApplicationName,
                buttons:[ {
                    class: "LeftButton",
                    text: resourceProvider.Yes,
                    click: function () {
                        $(this).dialog("close");

                        var json1 = ko.toJSON(self.CommissionCharge);
                        var data2Send = '{"ACommissionChargeDTO":' + json1 + ',"Command":"' + resourceProvider.Submit + '"}';
                        $.ajax({
                            url: '/Master/CommissionChargeSave/',
                            type: 'POST',
                            dataType: 'json',
                            data: data2Send,
                            contentType: 'application/json; charset=utf-8',
                            success: function (returnData) {
                                ShowMessage(returnData.RedirectUrl, returnData.Message);
                            }
                        });

                    }},
                    {
                        text: resourceProvider.No,
                        click: function () {
                        $(this).dialog("close");
                    }
                }]
            });

            $("#dialog").text(resourceProvider.MsgConfSubmit);
                        
        }
    }

    self.Save = function () {

        var isValid = true;
        //DD
        var CommissionChargeError = ko.observable({});
        CommissionChargeError = ko.validation.group(self.CommissionCharge, { deep: false });
        if (CommissionChargeError._latestValue.filter(function (element) { return element != null; }).length > 0) {
            if (self.CommissionCharge.CommissionChargeId._latestValue > 0) {
                if ($('#Contract').val() == '') {
                    $('#lblValidateContract').text(String.format(resourceProvider.MsgErrPleaseSelect, resourceProvider.Contract));
                    isValid = false;
                }
            }
        }
        //--

        //Asigning template values into child DTO
        self.CommissionCharge.CommissionChargeDetailsDTO = self.CommissionCharge.CommissionChargeDetailsData;
        var json1 = ko.toJSON(self.CommissionCharge);
        var data2Send = '{"ACommissionChargeDTO":' + json1 + ',"Command":"' + resourceProvider.Save + '"}';

        self.CommissionChargeErrors = ko.validation.group(self.CommissionCharge, { deep: true, observable: true });
        
        if (self.CommissionChargeErrors._latestValue.filter(function (element) { return element != null; }).length != 0) {
            self.CommissionChargeErrors.showAllMessages(true);
            isValid = false;
        }
        else {
            if (self.CommissionCharge.CommissionChargeDetailsDTO().length == 0) {
                ShowMessage("", String.format(resourceProvider.MsgErrOneValid, resourceProvider.CommissionCharge));
                isValid = false;
            }
        }

        if (isValid) {
            $.ajax({
                url: '/Master/CommissionChargeSave/',
                type: 'POST',
                dataType: 'json',
                data: data2Send,
                contentType: 'application/json; charset=utf-8',
                success: function (returnData) {
                    ShowMessage(returnData.RedirectUrl, returnData.Message);
                }
            });
        }
    }
    
    self.Delete = function () {
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

                    var json1 = ko.toJSON(self.CommissionCharge.CommissionChargeId);
                    var data2Send = '{"ACommissionChargeId":' + json1 + '}';
                    $.ajax({
                        url: '/Master/CommissionChargeDelete/',
                        type: 'POST',
                        dataType: 'json',
                        data: data2Send,
                        contentType: 'application/json; charset=utf-8',
                        success: function (returnData) {
                            ShowMessage(returnData.RedirectUrl, returnData.Message);
                        }
                    });
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
    

    self.AddItem = function () {
        self.CommissionCharge.CommissionChargeDetailsData.push(new CommissionChargeDetails(self.CommissionCharge.CommissionChargeId._latestValue));
    };
        

    //self.AddItem = function () {
    //    self.CommissionCharge.CommissionChargeDetailsData.push({
    //        CommissionChargeId: self.CommissionCharge.CommissionChargeId(),
    //        CommissionChargeDetailsId: ko.observable(0),
    //        CommissionCode: ko.observable("").extend({ required: { params: true, message: String.format(resourceProvider.MsgErrPleaseSelect, resourceProvider.Commision) } }),
    //        CommissionAmount: ko.observable(0).extend({ required: { params: true, message: "Please enter Commission Amount" } })
    //        //ConvertedCommissionAmount: ko.computed(function () {
    //        //    return self.CommissionCharge.CurrencyCode() == resourceProvider.UFCurrencyCode ? self.CommissionChargeDetails.CommissionAmount() * resourceProvider.CurrentUFRate : self.CommissionChargeDetails.CommissionAmount()
    //        //})
    //    });

    //    //var count = self.CommissionCharge.CommissionChargeDetailsData().length;
    //    //var index = count - 1;

    //    //self.CommissionCharge.CommissionChargeDetailsData._latestValue[index].ConvertedCommissionAmount = ko.computed(function () {
    //    //    return self.CommissionCharge.CurrencyCode() == resourceProvider.UFCurrencyCode ? self.CommissionCharge.CommissionChargeDetailsData()[index].CommissionAmount() * resourceProvider.CurrentUFRate : self.CommissionCharge.CommissionChargeDetailsData()[index].CommissionAmount()
    //    //});

    //};
    


    self.remove = function () {
        self.CommissionCharge.CommissionChargeDetailsData.remove(this);
    };

}



