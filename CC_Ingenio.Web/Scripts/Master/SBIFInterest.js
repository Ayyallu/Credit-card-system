


function FormattedDecimal(val) {

    if (resourceProvider.ResourceCulture == "en-GB") {
        var num = Globalize(resourceProvider.ResourceCulture).parseFloat(val.replace(/,/g, '')) 
    }
    else {
        var num = Globalize(resourceProvider.ResourceCulture).parseFloat(val.replace(/\./g, ''))  
    }
    return num;
}



function formattedNumericObservable(initialValue, precision) {
    var _raw = ko.observable(initialValue),
        precision = precision || formattedNumericObservable.defaultPrecision,
        //the dependentObservable that we will return
        result = ko.dependentObservable({
            read: function () {
                return _raw().toFixed(precision);
            },
            write: _raw
        });

    //expose raw value (if necessary) for binding
    result.raw = _raw;

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

function SBIFInterestDetails(value) {
    var self = this;

    if (value != null && value != "" && !isNaN(value))
        self.SBIFInterestId = ko.observable(value);
    else
        self.SBIFInterestId = ko.observable(0);

    self.SBIFInterestDetailsId = ko.observable(0);
    
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
            message: String.format(resourceProvider.MsgErrValueShouldBeGreaterThan, resourceProvider.AmountTo, resourceProvider.AmountFrom),params: [self.CreditLimitFrom]
        }
    }).extend({
        validation: {
            validator: checkMaxLimit, message: String.format(resourceProvider.MsgErrValueShouldBeLessThan, resourceProvider.CreditLimitTo, "999999999"), params: 999999999
        }
    });
    self.SBIFInterestRate = ko.observable("").extend({
        required: {
            params: true,
            message: String.format(resourceProvider.MsgErrPleaseEnter2, resourceProvider.SBIFInterestRate)
        },
        validation: {
            validator: checkInterestRate,
            message: String.format(resourceProvider.MsgErrValueShouldBeLessThan, resourceProvider.SBIFInterestRate,"100"), params: 100
        }
    });
}


function SBIFInterest(value) {
    var self = this;

    self.SBIFInterestId = ko.observable(value ? value.SBIFInterestId : "");
    self.EffectiveDate = ko.observable(value ? (value.EffectiveDate ? $.datepicker.formatDate(resourceProvider.FormatDateJS, new Date(parseInt(value.EffectiveDate.substr(6)))) : null) : null).extend({
        required: {
            params: true,
            message: String.format(resourceProvider.MsgErrPleaseEnter2, resourceProvider.EffectiveDate)
        }
    });;
    self.MaxInterestDefinitionFor = ko.observable(value ? value.MaxInterestDefinitionFor : "").extend({
        required: {
            params: true,
            message: String.format(resourceProvider.MsgErrPleaseSelect, resourceProvider.MaxInterestDefinition)
        }
    });

    self.SBIFInterestDetailsData = ko.observableArray([]);
    self.SBIFInterestDetailsDTO = ko.observableArray([]);
    
    if (value && value.SBIFInterestDetailsDTO) {
        $.each(
            value.SBIFInterestDetailsDTO, function (index, values) {
                if (values) {

                 
                    self.SBIFInterestDetailsData.push(new SBIFInterestDetails(values.SBIFInterestId));

                    self.SBIFInterestDetailsData._latestValue[index].SBIFInterestDetailsId = values.SBIFInterestDetailsId ? values.SBIFInterestDetailsId : 0;
                    self.SBIFInterestDetailsData._latestValue[index].SBIFInterestId = values.SBIFInterestId ? values.SBIFInterestId : 0;
                    self.SBIFInterestDetailsData._latestValue[index].CreditLimitFrom = ko.observable(values ?
                        Globalize(resourceProvider.ResourceCulture).format(values.CreditLimitFrom, resourceProvider.FormatAmountJSn5) : "").extend({
                            required: { params: true, message: String.format(resourceProvider.MsgErrPleaseEnter2, resourceProvider.CreditLimitFrom) },
                            validation: {
                                validator: checkMaxLimit, message: String.format(resourceProvider.MsgErrValueShouldBeLessThan, resourceProvider.CreditLimitFrom, "999999999"),
                                params: 999999999
                            }
                        });
                    self.SBIFInterestDetailsData._latestValue[index].CreditLimitTo = ko.observable(values ?
                        Globalize(resourceProvider.ResourceCulture).format(values.CreditLimitTo, resourceProvider.FormatAmountJSn5) : "").extend({
                        required: { params: true, message: String.format(resourceProvider.MsgErrPleaseEnter2, resourceProvider.CreditLimitTo) },
                        validation: {
                            validator: checkMax, message: String.format(resourceProvider.MsgErrValueShouldBeGreaterThan, resourceProvider.AmountTo,resourceProvider.AmountFrom),
                            params: [self.SBIFInterestDetailsData._latestValue[index].CreditLimitFrom]
                        }
                        }).extend({
                            validation: {
                                validator: checkMaxLimit, message: String.format(resourceProvider.MsgErrValueShouldBeLessThan, resourceProvider.CreditLimitTo, "999999999"), params: 999999999
                            }
                        });
                    self.SBIFInterestDetailsData._latestValue[index].SBIFInterestRate = ko.observable(values ?
                        Globalize(resourceProvider.ResourceCulture).format(values.SBIFInterestRate, resourceProvider.FormatAmountJSn2) : "").extend({
                        required: { params: true, message: String.format(resourceProvider.MsgErrPleaseEnter2, resourceProvider.SBIFInterestRate) },
                        validation: {
                            validator: checkInterestRate,
                            message: String.format(resourceProvider.MsgErrValueShouldBeLessThan, resourceProvider.SBIFInterestRate, "100"), params: 100
                        }
                    });

                }
            })
    }
    else {
        var itemValue = new SBIFInterestDetails();
        self.SBIFInterestDetailsData.push(itemValue);
    }

}



function SBIFInterestVM(SBIFInterestData) {
    var self = this;
    self.SBIFInterest = new SBIFInterest(SBIFInterestData);
    self.SBIFInterestDetails = new SBIFInterestDetails();

    self.SBIFInterestErrors = ko.validation.group(self.SBIFInterest, { deep: true });
    

    self.Submit = function () {
        var isValid = true;
        if (self.SBIFInterestErrors().length != 0) {
            self.SBIFInterestErrors.showAllMessages();
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

                        self.SBIFInterest.SBIFInterestDetailsDTO = self.SBIFInterest.SBIFInterestDetailsData;
                        var json1 = ko.toJSON(self.SBIFInterest);
                        var data2Send = '{"ASBIFInterestDTO":' + json1 + ',"Command":"' + resourceProvider.Submit + '"}';



                        /*  Validate Credit Limit Overlap */

                        $.ajax({
                            type: 'POST',
                            url: '/Master/ValidateSBIFInterestDetails',
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
                                        title: '@Resources.ApplicationName',
                                        buttons: [{
                                            text: "OK",
                                            style: "font-size: .85em;font-family: 'Segoe UI', Verdana, Helvetica, Sans-Serif;",
                                            width: 50,
                                            click: function () {
                                                $(this).dialog("close");
                                            }
                                        }]
                                    });
                                    $('#dialog').text(returnData.Message);
                                }
                                else {
                                    $.ajax({
                                        url: '/Master/SBIFInterestSave/',
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

        self.SBIFInterest.SBIFInterestDetailsDTO = self.SBIFInterest.SBIFInterestDetailsData;
        var json1 = ko.toJSON(self.SBIFInterest);
        var data2Send = '{"ASBIFInterestDTO":' + json1 + ',"Command":"' + resourceProvider.Save + '"}';

        self.SBIFInterestErrors = ko.validation.group(self.SBIFInterest, { deep: true });
        var isValid = true;
        if (self.SBIFInterestErrors().length != 0) {
            self.SBIFInterestErrors.showAllMessages();
            isValid = false;
        }
        if (isValid) {
                        
            /*  Validate Credit Limit Overlap */

            $.ajax({
                type: 'POST',
                url: '/Master/ValidateSBIFInterestDetails',
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
                            url: '/Master/SBIFInterestSave/',
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

                    var json1 = ko.toJSON(self.SBIFInterest.SBIFInterestId);
                    var data2Send = '{"ASBIFInterestId":' + json1 + '}';
                    $.ajax({
                        url: '/Master/SBIFInterestDelete/',
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
        self.SBIFInterest.SBIFInterestDetailsData.push(new SBIFInterestDetails(self.SBIFInterest.SBIFInterestId._latestValue));
    };

    self.remove = function () {
        self.SBIFInterest.SBIFInterestDetailsData.remove(this);
    };

    
}

