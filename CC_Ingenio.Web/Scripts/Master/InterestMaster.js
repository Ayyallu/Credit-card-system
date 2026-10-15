


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


function InterestMasterDetails(value) {
    var self = this;

    if (value != null && value != "" && !isNaN(value))
        self.InterestMasterId = ko.observable(value);
    else
        self.InterestMasterId = ko.observable(0);

    self.InterestMasterIdDetailsId = ko.observable(0);
    self.CreditLimitFrom = ko.observable("").extend({
        required: {
            params: true,
            message: String.format(resourceProvider.MsgErrPleaseEnter2, resourceProvider.CreditLimitFrom)
        },
        validation: {
            validator: checkMaxLimit, message: String.format(resourceProvider.MsgErrValueShouldBeLessThan, resourceProvider.CreditLimitFrom, "999999999"), params: 999999999
        }
    });
    self.CreditLimitTo = ko.observable("").extend({
        required: {
            params: true,
            message: String.format(resourceProvider.MsgErrPleaseEnter2, resourceProvider.CreditLimitTo)
        },
        validation: {
            validator: checkMax,
            message: String.format(resourceProvider.MsgErrValueShouldBeGreaterThan, resourceProvider.AmountTo, resourceProvider.AmountFrom), params: [self.CreditLimitFrom]
        }
    }).extend({
        validation: {
            validator: checkMaxLimit, message: String.format(resourceProvider.MsgErrValueShouldBeLessThan, resourceProvider.CreditLimitTo, "999999999"), params: 999999999
        }
    });
    self.InterestRatePercent = ko.observable("").extend({
        required: {
            params: true,
            message: String.format(resourceProvider.MsgErrPleaseEnter2, resourceProvider.InterestRatePercentage)
        },
        validation: {
            validator: checkInterestRate,
            message: String.format(resourceProvider.MsgErrValueShouldBeLessThan, resourceProvider.InterestRate,"100"), params: 100
        }
    });
    
}


function InterestMaster(value) {
    var self = this;
    self.InterestMasterId = ko.observable(value ? value.InterestMasterId : "");

    self.ProductCode = ko.observable(value ? value.ProductCode : "").extend({
        required: {
            params: true,
            message: String.format(resourceProvider.MsgErrPleaseSelect, resourceProvider.Product)
        }
    });
    self.CustomerSegmentCode = ko.observable(value ? value.CustomerSegmentCode : "").extend({
        required: {
            params: true,
            message: String.format(resourceProvider.MsgErrPleaseSelect, resourceProvider.CustomerSegment)
        }
    });
    self.CustomerSubSegmentCode = ko.observable(value ? value.CustomerSubSegmentCode : "").extend({
        required: {
            params: true,
            message: String.format(resourceProvider.MsgErrPleaseSelect, resourceProvider.CustomerSubSegment)
        }
    });
    self.EffectiveDate = ko.observable(value ? (value.EffectiveDate ? $.datepicker.formatDate(resourceProvider.FormatDateJS, new Date(parseInt(value.EffectiveDate.substr(6)))) : null) : null).extend({
        required: {
            params: true,
            message: String.format(resourceProvider.MsgErrPleaseEnter2, resourceProvider.EffectiveDate)
        }
    });
    self.MaxInterestDefinitionFor = ko.observable(value ? value.MaxInterestDefinitionFor : "").extend({
        required: {
            params: true,
            message: String.format(resourceProvider.MsgErrPleaseSelect, resourceProvider.MaxInterestDefinitionFor)
        }
    });

    self.InterestMasterDetailsData = ko.observableArray();
    self.InterestMasterDetailsDTO = ko.observableArray();


    if (value && value.InterestMasterDetailsDTO) {
        $.each(
            value.InterestMasterDetailsDTO, function (index, values) {
                if (values) {
                    self.InterestMasterDetailsData.push(new InterestMasterDetails(values.InterestMasterId));

                    self.InterestMasterDetailsData._latestValue[index].InterestMasterDetailsId = values.InterestMasterDetailsId ? values.InterestMasterDetailsId : 0;
                    self.InterestMasterDetailsData._latestValue[index].InterestMasterId = values.InterestMasterId ? values.InterestMasterId : 0;
                    self.InterestMasterDetailsData._latestValue[index].CreditLimitFrom = ko.observable(values ?
                        Globalize(resourceProvider.ResourceCulture).format(values.CreditLimitFrom, resourceProvider.FormatAmountJSn5) : "").extend({
                            required: { params: true, message: String.format(resourceProvider.MsgErrPleaseEnter2, resourceProvider.CreditLimitFrom) },
                            validation: {
                                validator: checkMaxLimit, message: String.format(resourceProvider.MsgErrValueShouldBeLessThan, resourceProvider.CreditLimitFrom, "999999999"), params: 999999999
                            }
                    });
                    self.InterestMasterDetailsData._latestValue[index].CreditLimitTo = ko.observable(values ?
                        Globalize(resourceProvider.ResourceCulture).format(values.CreditLimitTo, resourceProvider.FormatAmountJSn5) : "").extend({
                        required: { params: true, message: String.format(resourceProvider.MsgErrPleaseEnter2, resourceProvider.CreditLimitTo) },
                        validation: {
                            validator: checkMax, message: String.format(resourceProvider.MsgErrValueShouldBeGreaterThan, resourceProvider.AmountTo, resourceProvider.AmountFrom),
                            params: [self.InterestMasterDetailsData._latestValue[index].CreditLimitFrom]
                        }
                    });
                    self.InterestMasterDetailsData._latestValue[index].InterestRatePercent = ko.observable(values ?
                        Globalize(resourceProvider.ResourceCulture).format(values.InterestRatePercent, resourceProvider.FormatAmountJSn2) : "").extend({
                        required: { params: true, message: String.format(resourceProvider.MsgErrPleaseEnter2, resourceProvider.InterestRatePercentage) },
                        validation: {
                            validator: checkInterestRate,
                            message: String.format(resourceProvider.MsgErrValueShouldBeLessThan, resourceProvider.InterestRate, "100"), params: 100
                        }
                    });
                }
            })
    }
    else {
        var itemValue = new InterestMasterDetails();
        self.InterestMasterDetailsData.push(itemValue);
    }
        
}



function InterestMasterVM(InterestMasterData) {
    var self = this;
    self.InterestMaster = new InterestMaster(InterestMasterData);
    self.InterestMasterDetails = new InterestMasterDetails();

    self.InterestMasterErrors = ko.validation.group(self.InterestMaster, { deep: true });

    self.Submit = function () {
        var isValid = true;
        if (self.InterestMasterErrors().length != 0) {
            self.InterestMasterErrors.showAllMessages();
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

                        self.InterestMaster.InterestMasterDetailsDTO = self.InterestMaster.InterestMasterDetailsData;
                        var json1 = ko.toJSON(self.InterestMaster);
                        var data2Send = '{"AInterestMasterDTO":' + json1 + ',"Command":"' + resourceProvider.Submit + '"}';


                        /*  Validate Credit Limit Overlap */

                        $.ajax({
                            type: 'POST',
                            url: '/Master/ValidateInterestMasterDetails',
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
                                            text:resourceProvider.OK,
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
                                        url: '/Master/InterestMasterSave/',
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
                   {    text: resourceProvider.No,
                       click: function () {
                        $(this).dialog("close");
                    }
                }]
            });

            $("#dialog").text(resourceProvider.MsgConfSubmit);

        }
    }

    self.Save = function () {

        self.InterestMaster.InterestMasterDetailsDTO = self.InterestMaster.InterestMasterDetailsData;
        var json1 = ko.toJSON(self.InterestMaster);
        var data2Send = '{"AInterestMasterDTO":' + json1 + ',"Command":"' + resourceProvider.Save + '"}';

        self.InterestMasterErrors = ko.validation.group(self.InterestMaster, { deep: true });

        var isValid = true;
        if (self.InterestMasterErrors().length != 0) {
            self.InterestMasterErrors.showAllMessages();
            isValid = false;
        }
        if (isValid) {

            /*  Validate Credit Limit Overlap */

            $.ajax({
                type: 'POST',
                url: '/Master/ValidateInterestMasterDetails',
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
                            url: '/Master/InterestMasterSave/',
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

                    var json1 = ko.toJSON(self.InterestMaster.InterestMasterId);
                    var data2Send = '{"AInterestMasterId":' + json1 + '}';
                    $.ajax({
                        url: '/Master/InterestMasterDelete/',
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
        self.InterestMaster.InterestMasterDetailsData.push(new InterestMasterDetails(self.InterestMaster.InterestMasterId._latestValue));
    };


    self.remove = function () {
        self.InterestMaster.InterestMasterDetailsData.remove(this);
    };

}