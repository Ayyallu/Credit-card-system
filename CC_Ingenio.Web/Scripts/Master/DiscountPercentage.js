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


function DiscountPercentageDetails(value) {
    var self = this;

    if (value != null && value != "" && !isNaN(value))
        self.DiscountPercentageId = ko.observable(value);
    else
        self.DiscountPercentageId = ko.observable(0);

    self.DiscountPercentageDetailsId = ko.observable(0);
    self.PurchaseAmountFrom = ko.observable("").extend({
        required: {
            params: true,
            message: String.format(resourceProvider.MsgErrPleaseEnter2, resourceProvider.PurchaseAmountFrom)
        },
        validation: {
            validator: checkMaxLimit, message: String.format(resourceProvider.MsgErrValueShouldBeLessThan, resourceProvider.PurchaseAmountFrom, "999999999"), params: 999999999
        }
    }),
    self.PurchaseAmountTo = ko.observable("").extend({
        required: {
            params: true,
            message: String.format(resourceProvider.MsgErrPleaseEnter2, resourceProvider.PurchaseAmountTo)
        },
        validation: {
            validator: checkMax,
            message: String.format(resourceProvider.MsgErrValueShouldBeGreaterThan, resourceProvider.AmountTo, resourceProvider.AmountFrom), params: [self.PurchaseAmountFrom]
        }
    }).extend({
        validation: {
            validator: checkMaxLimit, message: String.format(resourceProvider.MsgErrValueShouldBeLessThan, resourceProvider.PurchaseAmountTo, "999999999"), params: 999999999
        }
    });
    self.DPDDaysFrom = ko.observable("").extend({
        required: {
            params: true,
            message: String.format(resourceProvider.MsgErrPleaseEnter2, resourceProvider.DPDDaysFrom)
        }
    }),
    self.DPDDaysTo = ko.observable("").extend({
        required: {
            params: true,
            message: String.format(resourceProvider.MsgErrPleaseEnter2, resourceProvider.DPDDaysTo)
        },
        validation: {
            validator: checkMax,
            message: String.format(resourceProvider.MsgErrValueShouldBeGreaterThan, resourceProvider.AmountTo,resourceProvider.AmountFrom), params: [self.DPDDaysFrom]
        }
    }),
    self.DiscountPercentage = ko.observable("").extend({
        required: {
            params: true,
            message: String.format(resourceProvider.MsgErrPleaseEnter2, resourceProvider.DiscountPer)
        },
        validation: {
            validator: checkInterestRate,
            message: String.format(resourceProvider.MsgErrValueShouldBeLessThan, resourceProvider.DiscountPer, "100"), params: 100
        }
    })
}

function DiscountPercentage(value) {
    var self = this;
    self.DiscountPercentageId = ko.observable(value ? value.DiscountPercentageId : "");
        
    self.ProductCode = ko.observable(value ? value.ProductCode : "").extend({
        required: {
            params: true,
            message: String.format(resourceProvider.MsgErrPleaseSelect, resourceProvider.Product)
        }
    });;
    self.SegmentCode = ko.observable(value ? value.SegmentCode : "").extend({
        required: {
            params: true,
            message: String.format(resourceProvider.MsgErrPleaseSelect, resourceProvider.SegmentCode)
        }
    });;
    self.SubSegmentCode = ko.observable(value ? value.SubSegmentCode : "").extend({
        required: {
            params: true,
            message: String.format(resourceProvider.MsgErrPleaseSelect, resourceProvider.SubSegmentCode)
        }
    });;

    self.ValidFrom = ko.observable(value ? $.datepicker.formatDate(resourceProvider.FormatDateJS, new Date(parseInt(value.ValidFrom.substr(6))))
      : moment(new Date()).format('DD/MM/YYYY')).extend({
          required: {
              params: true,
              message: String.format(resourceProvider.MsgErrPleaseEnter2, resourceProvider.ValidFrom)
          }
      });

    self.ValidTo = ko.observable(value ? $.datepicker.formatDate(resourceProvider.FormatDateJS, new Date(parseInt(value.ValidTo.substr(6))))
        : moment(new Date()).format('DD/MM/YYYY')).extend({
            required: {
                params: true,
                message: String.format(resourceProvider.MsgErrPleaseEnter2, resourceProvider.ValidTo)
            }
        });


    self.ValidFrom.subscribe(function (newValue) {
        $('#ValidTo').datepicker("option", "minDate", self.ValidFrom());
    });
    self.ValidTo.subscribe(function (newValue) {
        $('#ValidTo').datepicker("option", "minDate", self.ValidFrom());
    });

    self.DiscountPercentageDetailsData = ko.observableArray();
    self.DiscountPercentageDetailsDTO = ko.observableArray();


    if (value && value.DiscountPercentageDetailsDTO) {
        $.each(
            value.DiscountPercentageDetailsDTO, function (index, values) {
                if (values) {

                    self.DiscountPercentageDetailsData.push(new DiscountPercentageDetails(values.DiscountPercentageId));

                    self.DiscountPercentageDetailsData._latestValue[index].DiscountPercentageDetailsId = values.DiscountPercentageDetailsId ? values.DiscountPercentageDetailsId : 0;
                    self.DiscountPercentageDetailsData._latestValue[index].DiscountPercentageId = values.DiscountPercentageId ? values.DiscountPercentageId : 0;
                    self.DiscountPercentageDetailsData._latestValue[index].PurchaseAmountFrom = ko.observable(values ?
                        Globalize(resourceProvider.ResourceCulture).format(values.PurchaseAmountFrom, resourceProvider.FormatAmountJSn0) : "").extend({
                        required: { params: true, message: String.format(resourceProvider.MsgErrPleaseEnter2, resourceProvider.PurchaseAmountFrom) },
                        validation: {
                            validator: checkMaxLimit, message: String.format(resourceProvider.MsgErrValueShouldBeLessThan, resourceProvider.PurchaseAmountFrom, "999999999"), params: 999999999
                        }
                    });
                    self.DiscountPercentageDetailsData._latestValue[index].PurchaseAmountTo = ko.observable(values ?
                        Globalize(resourceProvider.ResourceCulture).format(values.PurchaseAmountTo, resourceProvider.FormatAmountJSn0) : "").extend({
                        required: { params: true, message: String.format(resourceProvider.MsgErrPleaseEnter2, resourceProvider.PurchaseAmountTo) },
                        validation: {
                            validator: checkMax, message: String.format(resourceProvider.MsgErrValueShouldBeGreaterThan, resourceProvider.AmountTo,resourceProvider.AmountFrom),
                            params: [self.DiscountPercentageDetailsData._latestValue[index].PurchaseAmountFrom]
                        }
                    }).extend({
                        validation: {
                            validator: checkMaxLimit, message: String.format(resourceProvider.MsgErrValueShouldBeLessThan, resourceProvider.PurchaseAmountTo, "999999999"), params: 999999999
                        }
                    });
                    self.DiscountPercentageDetailsData._latestValue[index].DPDDaysFrom = ko.observable(values.DPDDaysFrom ?
                    Globalize(resourceProvider.ResourceCulture).format(values.DPDDaysFrom, resourceProvider.FormatAmountJSn0) : "").extend({
                        required: { params: true, message: String.format(resourceProvider.MsgErrPleaseEnter2, resourceProvider.DPDDaysFrom) }
                    });
                    self.DiscountPercentageDetailsData._latestValue[index].DPDDaysTo = ko.observable(values.DPDDaysTo ?
                    Globalize(resourceProvider.ResourceCulture).format(values.DPDDaysTo, resourceProvider.FormatAmountJSn0) : "").extend({
                        required: { params: true, message:String.format(resourceProvider.MsgErrPleaseEnter2, resourceProvider.DPDDaysTo) },
                        validation: {
                            validator: checkMax, message: String.format(resourceProvider.MsgErrValueShouldBeGreaterThan, resourceProvider.DPDDaysTo,resourceProvider.DPDDaysFrom),
                            params: [self.DiscountPercentageDetailsData._latestValue[index].DPDDaysFrom]
                        }
                    });
                    self.DiscountPercentageDetailsData._latestValue[index].DiscountPercentage = ko.observable(values.DiscountPercentage ?
                    Globalize(resourceProvider.ResourceCulture).format(values.DiscountPercentage, resourceProvider.FormatAmountJSn2) : "").extend({
                        required: { params: true, message: String.format(resourceProvider.MsgErrPleaseEnter2, resourceProvider.DiscountPer) },
                        validation: { validator: checkInterestRate, message: String.format(resourceProvider.MsgErrValueShouldBeLessThan, resourceProvider.DiscountPer, "100"), params: 100 }
                    });


                }
            })
    }
    else {
        var itemValue = new DiscountPercentageDetails();
        self.DiscountPercentageDetailsData.push(itemValue);
    }
}



function DiscountPercentageVM(DiscountPercentageData) {
    var self = this;
    self.DiscountPercentage = new DiscountPercentage(DiscountPercentageData);
    self.DiscountPercentageDetails = new DiscountPercentageDetails();

    self.DiscountPercentageErrors = ko.validation.group(self.DiscountPercentage, { deep: true });

    self.Submit = function () {
        var isValid = true;
        if (self.DiscountPercentageErrors().length != 0) {
            self.DiscountPercentageErrors.showAllMessages();
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

                        self.DiscountPercentage.DiscountPercentageDetailsDTO = self.DiscountPercentage.DiscountPercentageDetailsData;
                        var json1 = ko.toJSON(self.DiscountPercentage);
                        var data2Send = '{"ADiscountPercentageDTO":' + json1 + ',"Command":"' + resourceProvider.Submit + '"}';


                        /*  Validate Credit Limit Overlap */

                        $.ajax({
                            type: 'POST',
                            url: '/Master/ValidateDiscountPercentageDetails',
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
                                        url: '/Master/DiscountPercentageSave/',
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

        self.DiscountPercentage.DiscountPercentageDetailsDTO = self.DiscountPercentage.DiscountPercentageDetailsData;
        var json1 = ko.toJSON(self.DiscountPercentage);
        var data2Send = '{"ADiscountPercentageDTO":' + json1 + ',"Command":"' + resourceProvider.Save + '"}';
        self.DiscountPercentageErrors = ko.validation.group(self.DiscountPercentage, { deep: true });
        var isValid = true;
        if (self.DiscountPercentageErrors().length != 0) {
            self.DiscountPercentageErrors.showAllMessages();
            isValid = false;
        }
        if (isValid) {


            /*  Validate Credit Limit Overlap */

            $.ajax({
                type: 'POST',
                url: '/Master/ValidateDiscountPercentageDetails',
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
                            url: '/Master/DiscountPercentageSave/',
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

                    var json1 = ko.toJSON(self.DiscountPercentage.DiscountPercentageId);
                    var data2Send = '{"ADiscountPercentageId":' + json1 + '}';
                    $.ajax({
                        url: '/Master/DiscountPercentageDelete/',
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
        self.DiscountPercentage.DiscountPercentageDetailsData.push(new DiscountPercentageDetails(self.DiscountPercentage.DiscountPercentageId._latestValue));
    };

    self.remove = function () {
        self.DiscountPercentage.DiscountPercentageDetailsData.remove(this);
    };

}