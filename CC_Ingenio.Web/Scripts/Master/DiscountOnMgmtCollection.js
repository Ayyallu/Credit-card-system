
ko.validation.configure({
    registerExtenders: true,
    messagesOnModified: true,
    insertMessages: true,
    parseInputAttributes: true,
    messageTemplate: null
});

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
                viewModelObj.DiscountOnMgmtCollection.ContractCode = ko.observable(valueToSet.ContractCode).extend({
                    required: {
                        params: true,
                        message: String.format(resourceProvider.MsgErrPleaseSelect, resourceProvider.Contract)
                    }
                });
            });
    }
}



function FormattedDecimal(val) {

    if (resourceProvider.ResourceCulture == "en-GB") {
        var num = Globalize(resourceProvider.ResourceCulture).parseFloat(val.replace(/,/g, ''))
    }
    else {
        var num = Globalize(resourceProvider.ResourceCulture).parseFloat(val.replace(/\./g, ''))
    }
    return num;
}

var checkMax = function (val, max) {
    val = Globalize(resourceProvider.ResourceCulture).parseFloat(val);
    if (!isNaN(max))
        return parseFloat(val) >= parseFloat(FormattedDecimal(max[0]));
    else
        return parseFloat(val) >= parseFloat(FormattedDecimal(max[0]()));
};


var checkMaxLimit = function (val, max) {
    val = Globalize(resourceProvider.ResourceCulture).parseFloat(val);
    if (!isNaN(max))
        return parseFloat(max) >= parseFloat(val);
};

var checkInterestRate = function (val, max) {
    val = Globalize(resourceProvider.ResourceCulture).parseFloat(val);
    return parseFloat(val) <= parseFloat(100);
};


function DiscountOnMgmtCollectionDetails(value) {
    var self = this;
    self.DiscountOnMgmtCollectionDetailsId = ko.observable(0);
    if (value != null && value != "" && !isNaN(value))
        self.DiscountOnMgmtCollectionId = ko.observable(value);
    else
        self.DiscountOnMgmtCollectionId = ko.observable(0);

    self.ConsumptionRangeFrom = ko.observable("").extend({
        required: {
            params: true,
            message:String.format(resourceProvider.MsgErrPleaseEnter2, resourceProvider.ConsumptionRangeFrom) 
        },
        validation: {
            validator: checkMaxLimit, message: String.format(resourceProvider.MsgErrValueShouldBeLessThan, resourceProvider.ConsumptionRangeFrom, "999999999"), params: 999999999
        }
    });
    self.ConsumptionRangeTo = ko.observable("").extend({
        required: {
            params: true,
            message:String.format(resourceProvider.MsgErrPleaseEnter2, resourceProvider.ConsumptionRangeTo) 
        },
        validation: {
            validator: checkMax,
            message: String.format(resourceProvider.MsgErrValueShouldBeGreaterThan, resourceProvider.RangeTo,resourceProvider.RangeFrom) , params: [self.ConsumptionRangeFrom]
        }
    }).extend({
        validation: {
            validator: checkMaxLimit, message: String.format(resourceProvider.MsgErrValueShouldBeLessThan, resourceProvider.ConsumptionRangeTo, "999999999"), params: 999999999
        }
    });
    self.DiscountPercentage = ko.observable("").extend({
        required: {
            params: true,
            message: String.format(resourceProvider.MsgErrPleaseEnter2, resourceProvider.DiscountPer)
        },
        validation: {
            validator: checkInterestRate,
            message: String.format(resourceProvider.MsgErrValueShouldBeLessThan, resourceProvider.DiscountPer,"100"), params: 100
        }
    });
}


function DiscountOnMgmtCollection(value) {
    var self = this;
    self.DiscountOnMgmtCollectionId = ko.observable(value ? value.DiscountOnMgmtCollectionId : "");

    self.ProductCode = ko.observable(value ? value.ProductCode : "").extend({
        required: {
            params: true,
            message: String.format(resourceProvider.MsgErrPleaseSelect, resourceProvider.Product) 
        }
    });;
    self.ContractCode = ko.observable(value ? value.ContractCode : "").extend({
        required: {
            params: true,
            message: String.format(resourceProvider.MsgErrPleaseSelect, resourceProvider.Contract)
        }
    });;
    self.CommissionCode = ko.observable(value ? value.CommissionCode : "").extend({
        required: {
            params: true,
            message: String.format(resourceProvider.MsgErrPleaseSelect, resourceProvider.CommissionCode) 
        }
    });;

    self.ProductCode.subscribe(function (productCode) {
        if (productCode) {
            PopulateDropDownViaAjax("#Contract", "/Master/GetContractByProductCode?AProductCode=" + productCode);
            if (self.DiscountOnMgmtCollectionId._latestValue > 0) {
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

    self.DiscountOnMgmtCollectionDetailsData = ko.observableArray();
    self.DiscountOnMgmtCollectionDetailsDTO = ko.observableArray();


    if (value && value.DiscountOnMgmtCollectionDetailsDTO) {
        $.each(
            value.DiscountOnMgmtCollectionDetailsDTO, function (index, values) {
                if (values) {


                    self.DiscountOnMgmtCollectionDetailsData.push(new DiscountOnMgmtCollectionDetails(values.DiscountOnMgmtCollectionId));

                    self.DiscountOnMgmtCollectionDetailsData._latestValue[index].DiscountOnMgmtCollectionDetailsId = values.DiscountOnMgmtCollectionDetailsId ? values.DiscountOnMgmtCollectionDetailsId : 0;
                    self.DiscountOnMgmtCollectionDetailsData._latestValue[index].DiscountOnMgmtCollectionId = values.DiscountOnMgmtCollectionId ? values.DiscountOnMgmtCollectionId : 0;
                    self.DiscountOnMgmtCollectionDetailsData._latestValue[index].ConsumptionRangeFrom = ko.observable(values ?
                        Globalize(resourceProvider.ResourceCulture).format(values.ConsumptionRangeFrom, resourceProvider.FormatAmountJSn5) : "").extend({
                            required: {
                                params: true, message: String.format(resourceProvider.MsgErrPleaseEnter2, resourceProvider.ConsumptionRangeFrom)
                            },
                            validation: {
                                validator: checkMaxLimit, message: String.format(resourceProvider.MsgErrValueShouldBeLessThan, resourceProvider.ConsumptionRangeFrom, "999999999"), params: 999999999
                            }
                    });
                    self.DiscountOnMgmtCollectionDetailsData._latestValue[index].ConsumptionRangeTo = ko.observable(values ?
                        Globalize(resourceProvider.ResourceCulture).format(values.ConsumptionRangeTo, resourceProvider.FormatAmountJSn5) : "").extend({
                        required: { params: true, message: String.format(resourceProvider.MsgErrPleaseEnter2, resourceProvider.ConsumptionRangeTo) },
                        validation: {
                            validator: checkMax, message: String.format(resourceProvider.MsgErrValueShouldBeGreaterThan, resourceProvider.RangeTo, resourceProvider.RangeFrom),
                            params: [self.DiscountOnMgmtCollectionDetailsData._latestValue[index].ConsumptionRangeFrom]
                        }
                        }).extend({
                            validation: {
                                validator: checkMaxLimit, message: String.format(resourceProvider.MsgErrValueShouldBeLessThan, resourceProvider.ConsumptionRangeTo, "999999999"), params: 999999999
                            }
                        });
                    self.DiscountOnMgmtCollectionDetailsData._latestValue[index].DiscountPercentage = ko.observable(values.DiscountPercentage ?
                    Globalize(resourceProvider.ResourceCulture).format(values.DiscountPercentage, resourceProvider.FormatAmountJSn2) : "").extend({
                        required: { params: true, message: String.format(resourceProvider.MsgErrPleaseEnter2, resourceProvider.DiscountPer) },
                        validation: { validator: checkInterestRate, message: String.format(resourceProvider.MsgErrValueShouldBeLessThan, resourceProvider.DiscountPer, "100"), params: 100 }
                    });

                }
            })
    }
    else {
        var itemValue = new DiscountOnMgmtCollectionDetails();
        self.DiscountOnMgmtCollectionDetailsData.push(itemValue);
    }
}


function DiscountOnMgmtCollectionVM(DiscountOnMgmtCollectionData) {
    var self = this;
    self.DiscountOnMgmtCollection = new DiscountOnMgmtCollection(DiscountOnMgmtCollectionData);
    self.DiscountOnMgmtCollectionDetails = new DiscountOnMgmtCollectionDetails();

    
    self.Submit = function () {
        var isValid = true;
        //DD
        var DiscountOnMgmtCollectionError = ko.observable({});
        DiscountOnMgmtCollectionError = ko.validation.group(self.DiscountOnMgmtCollection, { deep: false });
        if (DiscountOnMgmtCollectionError._latestValue.filter(function (element) { return element != null; }).length > 0) {
            if (self.DiscountOnMgmtCollection.DiscountOnMgmtCollectionId._latestValue > 0) {
                if ($('#Contract').val() == '') {
                    $('#lblValidateContract').text(String.format(resourceProvider.MsgErrPleaseSelect, resourceProvider.Contract));
                    isValid = false;
                }
            }
        }
        //--

        self.DiscountOnMgmtCollectionErrors = ko.validation.group(self.DiscountOnMgmtCollection, { deep: true });

        
        if (self.DiscountOnMgmtCollectionErrors().length != 0) {
            self.DiscountOnMgmtCollectionErrors.showAllMessages();
            isValid = false;
        }
        if (isValid) {

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

                        self.DiscountOnMgmtCollection.DiscountOnMgmtCollectionDetailsDTO = self.DiscountOnMgmtCollection.DiscountOnMgmtCollectionDetailsData;
                        var json1 = ko.toJSON(self.DiscountOnMgmtCollection);
                        var data2Send = '{"ADiscountOnMgmtCollectionDTO":' + json1 + ',"Command":"' + resourceProvider.Submit + '"}';

                        /*  Validate Credit Limit Overlap */

                        $.ajax({
                            type: 'POST',
                            url: '/Master/ValidateDiscountOnMgmtCollectionDetails',
                            data: data2Send,
                            dataType: 'json',
                            contentType: 'application/json; charset=utf-8',
                            success: function (returnData) {
                                $('#dialog').text("");

                                if (returnData.isOverlapped) {
                                    $('#dialog').dialog({
                                        resizable: false,
                                        maxWidth: 400,
                                        maxHeight: 150,
                                        width: 400,
                                        height: 150,
                                        modal: true,
                                        title: resourceProvider.ApplicationName,
                                        buttons: [{
                                            text: resourceProvider.OK,
                                            class: "ButtonStyle",
                                            width: "100%",
                                            click: function () {
                                                $(this).dialog("close");
                                            }
                                        }]
                                    });
                                    $('#dialog').text(returnData.Message);
                                }
                                else {
                                    $.ajax({
                                        url: '/Master/DiscountOnMgmtCollectionSave/',
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
                        });
                        /*-------------------*/

                        
                    }},
                    {   text: resourceProvider.No,
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
        var DiscountOnMgmtCollectionError = ko.observable({});
        DiscountOnMgmtCollectionError = ko.validation.group(self.DiscountOnMgmtCollection, { deep: false });
        if (DiscountOnMgmtCollectionError._latestValue.filter(function (element) { return element != null; }).length > 0) {
            if (self.DiscountOnMgmtCollection.DiscountOnMgmtCollectionId._latestValue > 0) {
                if ($('#Contract').val() == '') {
                    $('#lblValidateContract').text(String.format(resourceProvider.MsgErrPleaseSelect, resourceProvider.Contract));
                    isValid = false;
                }
            }
        }
        //--
        self.DiscountOnMgmtCollection.DiscountOnMgmtCollectionDetailsDTO = self.DiscountOnMgmtCollection.DiscountOnMgmtCollectionDetailsData;
        var json1 = ko.toJSON(self.DiscountOnMgmtCollection);
        var data2Send = '{"ADiscountOnMgmtCollectionDTO":' + json1 + ',"Command":"' + resourceProvider.Save + '"}';

        self.DiscountOnMgmtCollectionErrors = ko.validation.group(self.DiscountOnMgmtCollection, { deep: true });        
        if (self.DiscountOnMgmtCollectionErrors().length != 0) {
            self.DiscountOnMgmtCollectionErrors.showAllMessages();
            isValid = false;
        }
        if (isValid) {

            /*  Validate Credit Limit Overlap */

            $.ajax({
                type: 'POST',
                url: '/Master/ValidateDiscountOnMgmtCollectionDetails',
                data: data2Send,
                dataType: 'json',
                contentType: 'application/json; charset=utf-8',
                success: function (returnData) {
                    $('#dialog').text("");

                    if (returnData.isOverlapped) {
                        $('#dialog').dialog({
                            resizable: false,
                            maxWidth: 400,
                            maxHeight: 150,
                            width: 400,
                            height: 150,
                            modal: true,
                            title: resourceProvider.ApplicationName,
                            buttons: [{
                                text: resourceProvider.OK,
                                class: "ButtonStyle",
                                width: "100%",
                                click: function () {
                                    $(this).dialog("close");
                                }
                            }]
                        });
                        $('#dialog').text(returnData.Message);
                    }
                    else {
                        $.ajax({
                            url: '/Master/DiscountOnMgmtCollectionSave/',
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
            });
            /*-------------------*/

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

                    var json1 = ko.toJSON(self.DiscountOnMgmtCollection.DiscountOnMgmtCollectionId);
                    var data2Send = '{"ADiscountOnMgmtCollectionId":' + json1 + '}';
                    $.ajax({
                        url: '/Master/DiscountOnMgmtCollectionDelete/',
                        type: 'POST',
                        dataType: 'json',
                        data: data2Send,
                        contentType: 'application/json; charset=utf-8',
                        success: function (returnData) {
                            ShowMessage(returnData.RedirectUrl, returnData.Message);
                        }
                    });
                }},
                {   text: resourceProvider.No,
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
        self.DiscountOnMgmtCollection.DiscountOnMgmtCollectionDetailsData.push({
            DiscountOnMgmtCollectionId: self.DiscountOnMgmtCollection.DiscountOnMgmtCollectionId,
            DiscountOnMgmtCollectionDetailsId: 0,
            ConsumptionRangeFrom: ko.observable("").extend({
                required: {
                    params: true,
                    message:String.format(resourceProvider.MsgErrPleaseEnter2, resourceProvider.ConsumptionRangeFrom)
                }
            }),
            ConsumptionRangeTo: ko.observable("").extend({
                required: {
                    params: true,
                    message: String.format(resourceProvider.MsgErrPleaseEnter2, resourceProvider.ConsumptionRangeTo)
                }
            }),
            DiscountPercentage: ko.observable("").extend({
                required: {
                    params: true,
                    message: String.format(resourceProvider.MsgErrPleaseEnter2, resourceProvider.DiscountPer)
                }
            })
        });
    };

    self.AddItem = function () {
        self.DiscountOnMgmtCollection.DiscountOnMgmtCollectionDetailsData.push(new DiscountOnMgmtCollectionDetails(self.DiscountOnMgmtCollection.DiscountOnMgmtCollectionId._latestValue));
    };

    self.remove = function () {
        self.DiscountOnMgmtCollection.DiscountOnMgmtCollectionDetailsData.remove(this);
    };

}