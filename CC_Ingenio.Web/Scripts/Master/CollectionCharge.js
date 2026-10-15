

ko.validation.rules.number.validator = function (value, validate) {
    return !String.hasValue(value) || (validate && !isNaN(Globalize.parseFloat(value)));
};

ko.validation.rules.date.validator = function (value, validate) {
    return !String.hasValue(value) || (validate && Globalize.parseDate(value) != null);
};

ko.validation.configure({
    registerExtenders: true,
    messagesOnModified: true,
    insertMessages: true,
    parseInputAttributes: true,
    messageTemplate: null
});



function FormattedDecimal(val) {

    if (resourceProvider.ResourceCulture == "en-GB") {
        var num = Globalize(resourceProvider.ResourceCulture).parseFloat(val.replace(/,/g, ''))
    }
    else {
        var num = Globalize(resourceProvider.ResourceCulture).parseFloat(val.replace(/\./g, ''))
    }
    return num;
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


//var checkMax = function (val, max) {
//    val = Globalize(resourceProvider.ResourceCulture).parseFloat(val);
    
//    if (!isNaN(max)) {
//        var maxVal = Globalize(resourceProvider.ResourceCulture).parseFloat(max[0]);
//        return parseFloat(val) >= maxVal;
//    }
//    else {
//        var maxVal = Globalize(resourceProvider.ResourceCulture).parseFloat(max[0]());
//        return parseFloat(val) >= maxVal;
//    }
//};

//var checkInterestRate = function (val, max) {
//    val = Globalize(resourceProvider.ResourceCulture).parseFloat(val);
//    return parseFloat(val) <= parseFloat(100);
//};



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



function CollectionChargeDetails(value) {
    var self = this;

    if (value != null && value != "" && !isNaN(value))
        self.CollectionChargeId = ko.observable(value);
    else
        self.CollectionChargeId = ko.observable(0);
    self.CollectionChargeDetailsId = ko.observable(0);
    self.DelinquencyAmountFrom = ko.observable("").extend({
        required: {
            params: true,
            message: String.format(resourceProvider.MsgErrPleaseEnter2, resourceProvider.DelinquencyAmountFrom)
        },
        validation: {
            validator: checkMaxLimit, message: String.format(resourceProvider.MsgErrValueShouldBeLessThan, resourceProvider.DelinquencyAmountFrom, "999999999"), params: 999999999
        }
    }),
    self.DelinquencyAmountTo = ko.observable("").extend({
        required: {
            params: true,
            message: String.format(resourceProvider.MsgErrPleaseEnter2, resourceProvider.DelinquencyAmountTo)
        },
        validation: {
            validator: checkMax, message: String.format(resourceProvider.MsgErrValueShouldBeGreaterThan, resourceProvider.AmountTo, resourceProvider.AmountFrom),
            params: [self.DelinquencyAmountFrom]
        }
    }).extend({
        validation: {
            validator: checkMaxLimit, message: String.format(resourceProvider.MsgErrValueShouldBeLessThan, resourceProvider.DelinquencyAmountTo, "999999999"), params: 999999999
        }
    }),
    self.CollectionChargeAmount = ko.observable("").extend({
        required: {
            params: true,
            message: String.format(resourceProvider.MsgErrPleaseEnter2, resourceProvider.CollectionChargeAmountPercentage)
        },
        validation: {
            validator: checkInterestRate,
            message: String.format(resourceProvider.MsgErrValueShouldBeLessThan, resourceProvider.CollectionChargeAmountPercentage, "100"), params: 100
        }
    })
    return self

}


function CollectionCharge(value) {

    var self = this;
    self.CollectionChargeId = ko.observable(value ? value.CollectionChargeId : "");

    self.EffectiveDate = ko.observable(value ? (value.EffectiveDate ?
        $.datepicker.formatDate(resourceProvider.FormatDateJS, new Date(parseInt(value.EffectiveDate.substr(6)))) : null) : null).extend({
            required: {
                params: true,
                message: String.format(resourceProvider.MsgErrPleaseEnter2, resourceProvider.EffectiveDate)
            }
        });
    self.ProductCode = ko.observable(value ? value.ProductCode : "").extend({
        required: {
            params: true,
            message: String.format(resourceProvider.MsgErrPleaseSelect, resourceProvider.Product)
        }
    }),
    self.CurrencyCode = ko.observable(value ? value.CurrencyCode : "").extend({
        required: {
            params: true,
            message: String.format(resourceProvider.MsgErrPleaseSelect, resourceProvider.Currency)
        }
    });

    self.CollectionChargeDetailsData = ko.observableArray();
    self.CollectionChargeDetailsDTO = ko.observableArray();
    var i = 0;
    if (value && value.CollectionChargeDetailsDTO) {
        $.each(
            value.CollectionChargeDetailsDTO, function (index, values) {
                if (values) {

                    self.CollectionChargeDetailsData.push(new CollectionChargeDetails(values.CollectionChargeId));

                    self.CollectionChargeDetailsData._latestValue[index].CollectionChargeDetailsId = values.CollectionChargeDetailsId ? values.CollectionChargeDetailsId : 0;
                    self.CollectionChargeDetailsData._latestValue[index].CollectionChargeId = values.CollectionChargeId ? values.CollectionChargeId : 0;
                    self.CollectionChargeDetailsData._latestValue[index].DelinquencyAmountFrom = ko.observable(values ?
                        Globalize(resourceProvider.ResourceCulture).format(values.DelinquencyAmountFrom, (self.CurrencyCode._latestValue == resourceProvider.UFCurrencyCode ? resourceProvider.FormatAmountJSn5 : resourceProvider.FormatAmountJSn0)) : "").extend({
                            required: { params: true, message: String.format(resourceProvider.MsgErrPleaseEnter2, resourceProvider.DelinquencyAmountFrom) },
                            validation:
                                {
                                    validator: checkMaxLimit, message: String.format(resourceProvider.MsgErrValueShouldBeLessThan, resourceProvider.DelinquencyAmountFrom, "999999999"),
                                    params: 999999999
                                 }
                        });
                    self.CollectionChargeDetailsData._latestValue[index].DelinquencyAmountTo = ko.observable(values ?
                        Globalize(resourceProvider.ResourceCulture).format(values.DelinquencyAmountTo, (self.CurrencyCode._latestValue == resourceProvider.UFCurrencyCode ? resourceProvider.FormatAmountJSn5 : resourceProvider.FormatAmountJSn0)) : "").extend({
                            required: { params: true, message: String.format(resourceProvider.MsgErrPleaseEnter2, resourceProvider.DelinquencyAmountTo) },
                            validation: {
                                validator: checkMax, message: String.format(resourceProvider.MsgErrValueShouldBeGreaterThan, resourceProvider.AmountTo, resourceProvider.AmountFrom),
                                params: [self.CollectionChargeDetailsData._latestValue[index].DelinquencyAmountFrom]
                                //params: [Globalize(resourceProvider.ResourceCulture).parseFloat(self.CollectionChargeDetailsData._latestValue[index].DelinquencyAmountFrom._latestValue)]
                            }
                        }).extend({
                            validation: {
                                validator: checkMaxLimit, message: String.format(resourceProvider.MsgErrValueShouldBeLessThan, resourceProvider.DelinquencyAmountTo, "999999999"), params: 999999999
                            }
                        });
                    self.CollectionChargeDetailsData._latestValue[index].CollectionChargeAmount = ko.observable(values ?
                    Globalize(resourceProvider.ResourceCulture).format(values.CollectionChargeAmount, resourceProvider.FormatAmountJSn2) : "").extend({
                        required: { params: true, message: String.format(resourceProvider.MsgErrPleaseEnter2, resourceProvider.CollectionChargeAmountPercentage) },
                        validation: { validator: checkInterestRate, message: resourceProvider.MsgErrValueShouldBeLessThanCustom, params: 100 }
                    });

                }
            })
    }
    else {
        var itemValue = new CollectionChargeDetails();
        self.CollectionChargeDetailsData.push(itemValue);
    }
}

function CollectionChargeVM(CollectionChargeData) {
    var self = this;
    self.CollectionChargeErrors = ko.observable({});
    self.CollectionCharge = new CollectionCharge(CollectionChargeData);
    self.CollectionChargeDetails = new CollectionChargeDetails();

    self.Submit = function () {
        var isValid = true;

        self.CollectionChargeErrors = ko.validation.group(self.CollectionCharge, { deep: true });

        if (self.CollectionChargeErrors().length != 0) {
            self.CollectionChargeErrors.showAllMessages(true);
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

                        self.CollectionCharge.CollectionChargeDetailsDTO = self.CollectionCharge.CollectionChargeDetailsData;
                        var json1 = ko.toJSON(self.CollectionCharge);
                        var data2Send = '{"ACollectionChargeDTO":' + json1 + ',"Command":"' + resourceProvider.Submit + '"}';


                        /*  Validate Credit Limit Overlap */

                        $.ajax({
                            type: 'POST',
                            url: '/Master/ValidateCollectionChargeDetails',
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
                                        url: '/Master/CollectionChargeSave/',
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
                },
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

        self.CollectionCharge.CollectionChargeDetailsDTO = self.CollectionCharge.CollectionChargeDetailsData;
        var json1 = ko.toJSON(self.CollectionCharge);
        var data2Send = '{"ACollectionChargeDTO":' + json1 + ',"Command":"' + resourceProvider.Save + '"}';

        self.CollectionChargeErrors = ko.validation.group(self.CollectionCharge, { deep: true });
        var isValid = true;
        if (self.CollectionChargeErrors().length != 0) {
            self.CollectionChargeErrors.showAllMessages(true);
            isValid = false;
        }
        if (isValid) {

            /*  Validate Credit Limit Overlap */

            $.ajax({
                type: 'POST',
                url: '/Master/ValidateCollectionChargeDetails',
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
                            url: '/Master/CollectionChargeSave/',
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

                    var json1 = ko.toJSON(self.CollectionCharge.CollectionChargeId);
                    var data2Send = '{"ACollectionChargeId":' + json1 + '}';
                    $.ajax({
                        url: '/Master/CollectionChargeDelete/',
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
        self.CollectionCharge.CollectionChargeDetailsData.push(new CollectionChargeDetails(self.CollectionCharge.CollectionChargeId._latestValue));
    };


    self.remove = function () {
        self.CollectionCharge.CollectionChargeDetailsData.remove(this);
    };

}