

var checkInterestRate = function (val, max) {
    val = Globalize(resourceProvider.ResourceCulture).parseFloat(val);
    return parseFloat(val) <= parseFloat(100);
};


function PopulateProductMemberTypeCascadeDDL(valueToSet, viewModelObj) {

    /*Product Member Type dropdown*/
    var productCode = $("#Product").val();
    if (productCode != "") {
        $.getJSON("/Master/GetProductMemberTypeByProductCode?AProductCode=" + productCode,
            function (modeldata) {
                var select = $("[id = ProductMemberType]");
                select.empty();
                $.each(modeldata, function (index, itemData) {
                    select.append($('<option/>', {
                        value: itemData.Value,
                        text: itemData.Text
                    }));
                });
                select.val(valueToSet.ProductMemberTypeCode);                
                viewModelObj.Fee.ProductMemberTypeCode = ko.observable(valueToSet.ProductMemberTypeCode).extend({
                    required: {
                        params: true,
                        message: String.format(resourceProvider.MsgErrPleaseSelect, resourceProvider.ProductMemberType)
                    }
                });
            });
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
                viewModelObj.Fee.ContractCode = ko.observable(valueToSet.ContractCode).extend({
                    required: {
                        params: true,
                        message: String.format(resourceProvider.MsgErrPleaseSelect, resourceProvider.Contract)
                    }
                });
            });
    }
}


function currentDate() {
    return moment(); //sends current datetime
};

var checkMinMaxLimit = function (val) {
    val = Globalize(resourceProvider.ResourceCulture).parseFloat(val);
    return parseFloat(999999999) >= parseFloat(val) && parseFloat(val) >= parseFloat(1);
};

var checkMaxLimit = function (val) {
    val = Globalize(resourceProvider.ResourceCulture).parseFloat(val);
    return parseFloat(999999999) >= parseFloat(val) ;
};

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

function FeeType() {
    var self = this;
    self.FeeCode = ko.observable("");
    self.FeeTypeCode = ko.observable("").extend({
        required: {
            params: true,
            message: String.format(resourceProvider.MsgErrPleaseSelect, resourceProvider.FeeType)
        }
    });
    self.FeeAmount = ko.observable("").extend({
        required: {
            params: true,
            message: String.format(resourceProvider.MsgErrPleaseEnter2, resourceProvider.FeeAmount)
        },
        //min: {
        //    message: resourceProvider.MsgErrValueShouldBeGreaterThanCustom,
        //    params: 1
        //}
        validation: {
            validator: checkMinMaxLimit,
            message: String.format(resourceProvider.MsgErrValueShouldBeBetween, resourceProvider.FeeAmount, "1", "999999999")
        }
    });
    self.MonthsFrom = ko.observable(0).extend({
        required: {
            params: true,
            message: String.format(resourceProvider.MsgErrPleaseEnter2, resourceProvider.MonthsFrom)
        },
        min: {
            message: resourceProvider.MsgErrValueShouldBeGreaterThanCustom,
            params: 1
        },
        max: {
            message: resourceProvider.MsgErrValueShouldBeLessThanCustom,
            params: 12
        }
    });
    self.Period = ko.observable(1).extend({
        required: {
            params: true,
            message: String.format(resourceProvider.MsgErrPleaseEnter2, resourceProvider.Period)
        }
    });
    self.ReasonCode = ko.observable("").extend({
        required: {
            params: true,
            message: String.format(resourceProvider.MsgErrPleaseEnter2, resourceProvider.ReasonCode)
        }
    });
    self.FirstYearAmount = ko.observable("0").extend({
        validation: {
            validator: checkMaxLimit,
            message: String.format(resourceProvider.MsgErrValueShouldBeBetween, resourceProvider.Amount, "0", "999999999")
        }
    });
    self.RenwalFee1 = ko.observable("0").extend({
        validation: {
            validator: checkMaxLimit,
            message: String.format(resourceProvider.MsgErrValueShouldBeBetween, resourceProvider.Amount, "0", "999999999")
        }
    });
    self.RenwalFee2 = ko.observable("0").extend({
        validation: {
            validator: checkMaxLimit,
            message: String.format(resourceProvider.MsgErrValueShouldBeBetween, resourceProvider.Amount, "0", "999999999")
        }
    });
    self.RenwalFee3 = ko.observable("0").extend({
        validation: {
            validator: checkMaxLimit,
            message: String.format(resourceProvider.MsgErrValueShouldBeBetween, resourceProvider.Amount, "0", "999999999")
        }
    });
    self.RenwalFee4 = ko.observable("0").extend({
        validation: {
            validator: checkMaxLimit,
            message: String.format(resourceProvider.MsgErrValueShouldBeBetween, resourceProvider.Amount, "0", "999999999")
        }
    });
    self.RenwalFee5 = ko.observable("0").extend({
        validation: {
            validator: checkMaxLimit,
            message: String.format(resourceProvider.MsgErrValueShouldBeBetween, resourceProvider.Amount, "0", "999999999")
        }
    });
    self.RenwalFee6 = ko.observable("0").extend({
        validation: {
            validator: checkMaxLimit,
            message: String.format(resourceProvider.MsgErrValueShouldBeBetween, resourceProvider.Amount, "0", "999999999")
        }
    });

    self.AdjustCode = ko.observable("");

    self.DiscountRatePercent = ko.observable("0").extend({
        validation: {
            validator: checkInterestRate,
            message: String.format(resourceProvider.MsgErrValueShouldBeLessThan, resourceProvider.DiscountRatePercent, "100"), params: 100
        }
    });

    self.FlatFee = ko.observable("0").extend({
        validation: {
            validator: checkMaxLimit,
            message: String.format(resourceProvider.MsgErrValueShouldBeBetween, resourceProvider.Amount, "0", "999999999")
        }
    });
    self.MinFee = ko.observable("0").extend({
        validation: {
            validator: checkMaxLimit,
            message: String.format(resourceProvider.MsgErrValueShouldBeBetween, resourceProvider.Amount, "0", "999999999")
        }
    });

    self.Flag = ko.observable(false);
}

function Fee(value) {
    //Make the self as 'this' reference
    var self = this;
    //Declare observable which will be bind with UI
    self.FeeCode = ko.observable(value ? value.FeeCode : "").extend({
        required: {
            params: true,
            message: String.format(resourceProvider.MsgErrPleaseEnter2, resourceProvider.FeeCode)
        }
    });    

    self.ProductCode = ko.observable(value ? value.ProductCode : "").extend({
        required: {
            params: true,
            message: String.format(resourceProvider.MsgErrPleaseSelect, resourceProvider.Product)
        }
    });
            

    self.CurrencyCode = ko.observable(value ? value.CurrencyCode : "")
        .extend({
        required: {
            params: true,
            message: String.format(resourceProvider.MsgErrPleaseSelect, resourceProvider.CurrencyCode)
        }
    });



    self.ProductCode.subscribe(function (productCode) {
        if (productCode) {
            PopulateDropDownViaAjax("#ProductMemberType", "/Master/GetProductMemberTypeByProductCode?AProductCode=" + productCode);
            PopulateDropDownViaAjax("#Contract", "/Master/GetContractByProductCode?AProductCode=" + productCode);
        }
    }.bind(self));



    self.ProductMemberTypeCode = ko.observable(value ? value.ProductMemberTypeCode : "").extend({
        required: {
            params: true,
            message: String.format(resourceProvider.MsgErrPleaseSelect, resourceProvider.ProductMemberType)
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
    self.ContractCode = ko.observable(value ? value.ContractCode : "").extend({
        required: {
            params: true,
            message: String.format(resourceProvider.MsgErrPleaseSelect, resourceProvider.Contract)
        }
    });

    self.EffectiveFromDate = ko.observable(value ? value.EffectiveFromDate : "");

    self.EffectiveFromDate = ko.observable(value ? $.datepicker.formatDate(resourceProvider.FormatDateJS, new Date(parseInt(value.EffectiveFromDate.substr(6))))
      : moment(new Date()).format('DD/MM/YYYY')).extend({
          required: {
              params: true,
              message: String.format(resourceProvider.MsgErrPleaseEnter2, resourceProvider.EffectiveFromDate)
          }
      });

    self.EffectiveToDate = ko.observable(value ? $.datepicker.formatDate(resourceProvider.FormatDateJS, new Date(parseInt(value.EffectiveToDate.substr(6))))
        : moment(new Date()).format('DD/MM/YYYY')).extend({
            required: {
                params: true,
                message: String.format(resourceProvider.MsgErrPleaseEnter2, resourceProvider.EffectiveToDate)
            }
        });

    self.EffectiveFromDate.subscribe(function (newValue) {
        $('#EffectiveToDate').datepicker("option", "minDate", self.EffectiveFromDate());
        $('#EffectiveFromDate').datepicker("option", "maxDate", self.EffectiveToDate());
    });

    self.EffectiveToDate.subscribe(function (newValue) {
        $('#EffectiveFromDate').datepicker("option", "maxDate", self.EffectiveToDate());
        $('#EffectiveToDate').datepicker("option", "minDate", self.EffectiveFromDate());
    });

 

    /*----Fee Type Details----*/
    self.FeeTypeDet = ko.observableArray();
    self.FeeTypeDTO = ko.observableArray();

    if (value && value.FeeTypeDTO) {
        $.each(value.FeeTypeDTO, function (index, values) {
            if (values) {
                
                self.FeeTypeDet.push({
                    FeeCode: values.FeeCode,
                    FeeTypeCode: ko.observable(values.FeeTypeCode).extend({
                        required: {
                            params: true,
                            message: String.format(resourceProvider.MsgErrPleaseSelect, resourceProvider.FeeType)
                        }
                    }),
                    FeeAmount: ko.observable(values ?
                        Globalize(resourceProvider.ResourceCulture).format(values.FeeAmount, resourceProvider.FormatAmountJSn0) : "").extend({
                        required: {
                            params: true,
                            message: String.format(resourceProvider.MsgErrPleaseEnter2, resourceProvider.FeeAmount)
                        },
                        //min: {
                        //    message: resourceProvider.MsgErrValueShouldBeGreaterThanCustom,
                        //    params: 1
                        //}
                        validation: {
                            validator: checkMinMaxLimit,
                            message: String.format(resourceProvider.MsgErrValueShouldBeBetween, resourceProvider.FeeAmount, "1", "999999999")
                        }
                    }),
                    MonthsFrom: ko.observable(values.MonthsFrom).extend({
                        required: {
                            params: true,
                            message: String.format(resourceProvider.MsgErrPleaseEnter2, resourceProvider.MonthsFrom)
                        },
                        min: {
                            message: resourceProvider.MsgErrValueShouldBeGreaterThanCustom,
                            params: 1
                        },
                        max: {
                            message: resourceProvider.MsgErrValueShouldBeLessThanCustom,
                            params: 12
                        }
                    }),
                    ReasonCode: ko.observable(values.ReasonCode).extend({
                        required: {
                            params: true,
                            message: String.format(resourceProvider.MsgErrPleaseEnter2, resourceProvider.ReasonCode)
                        }
                    }),

                    FirstYearAmount: ko.observable(values ?
                        Globalize(resourceProvider.ResourceCulture).format(values.FirstYearAmount, resourceProvider.FormatAmountJSn0) : "0").extend({
                        validation: {
                            validator: checkMaxLimit,
                            message: String.format(resourceProvider.MsgErrValueShouldBeBetween, resourceProvider.Amount, "0", "999999999")
                        }
                    }),
                    RenwalFee1: ko.observable(values ?
                    Globalize(resourceProvider.ResourceCulture).format(values.RenwalFee1, resourceProvider.FormatAmountJSn0) : "0").extend({
                        validation: {
                            validator: checkMaxLimit,
                            message: String.format(resourceProvider.MsgErrValueShouldBeBetween, resourceProvider.Amount, "0", "999999999")
                        }
                    }),
                    RenwalFee2: ko.observable(values ?
                        Globalize(resourceProvider.ResourceCulture).format(values.RenwalFee2, resourceProvider.FormatAmountJSn0) : "0").extend({
                        validation: {
                            validator: checkMaxLimit,
                            message: String.format(resourceProvider.MsgErrValueShouldBeBetween, resourceProvider.Amount, "0", "999999999")
                        }
                    }),
                    RenwalFee3: ko.observable(values ?
                        Globalize(resourceProvider.ResourceCulture).format(values.RenwalFee3, resourceProvider.FormatAmountJSn0) : "0").extend({
                        validation: {
                            validator: checkMaxLimit,
                            message: String.format(resourceProvider.MsgErrValueShouldBeBetween, resourceProvider.Amount, "0", "999999999")
                        }
                    }),
                    RenwalFee4: ko.observable(values ?
                        Globalize(resourceProvider.ResourceCulture).format(values.RenwalFee4, resourceProvider.FormatAmountJSn0) : "0").extend({
                        validation: {
                            validator: checkMaxLimit,
                            message: String.format(resourceProvider.MsgErrValueShouldBeBetween, resourceProvider.Amount, "0", "999999999")
                        }
                    }),
                    RenwalFee5: ko.observable(values ?
                        Globalize(resourceProvider.ResourceCulture).format(values.RenwalFee5, resourceProvider.FormatAmountJSn0) : "0").extend({
                        validation: {
                            validator: checkMaxLimit,
                            message: String.format(resourceProvider.MsgErrValueShouldBeBetween, resourceProvider.Amount, "0", "999999999")
                        }
                    }),
                    RenwalFee6: ko.observable(values ?
                        Globalize(resourceProvider.ResourceCulture).format(values.RenwalFee6, resourceProvider.FormatAmountJSn0) : "0").extend({
                        validation: {
                            validator: checkMaxLimit,
                            message: String.format(resourceProvider.MsgErrValueShouldBeBetween, resourceProvider.Amount, "0", "999999999")
                        }
                    }),

                    AdjustCode: values.AdjustCode ? values.AdjustCode : "",
                    Period: ko.observable(values.Period).extend({
                        required: {
                            params: true,
                            message: String.format(resourceProvider.MsgErrPleaseEnter2, resourceProvider.Period)
                        }
                    }),//values.Period ? values.Period : 0,
                    //DiscountRatePercent: values.DiscountRatePercent ? values.DiscountRatePercent : 0,

                    DiscountRatePercent: ko.observable(values ?
                        Globalize(resourceProvider.ResourceCulture).format(values.DiscountRatePercent, resourceProvider.FormatAmountJSn2) : "0").extend({
                            validation: {
                                validator: checkInterestRate,
                                message: String.format(resourceProvider.MsgErrValueShouldBeLessThan, resourceProvider.DiscountRatePercent, "100"), params: 100
                            }
                        }),
                                       

                    FlatFee: ko.observable(values ?
                        Globalize(resourceProvider.ResourceCulture).format(values.FlatFee, resourceProvider.FormatAmountJSn0) : "0").extend({
                            validation: {
                                validator: checkMaxLimit,
                                message: String.format(resourceProvider.MsgErrValueShouldBeBetween, resourceProvider.Amount, "0", "999999999")
                            }
                        }),
                    MinFee: ko.observable(values ?
                        Globalize(resourceProvider.ResourceCulture).format(values.MinFee, resourceProvider.FormatAmountJSn0) : "0").extend({
                            validation: {
                                validator: checkMaxLimit,
                                message: String.format(resourceProvider.MsgErrValueShouldBeBetween, resourceProvider.Amount, "0", "999999999")
                            }
                        }),
                                        
                    Flag: values.Flag ? values.Flag : 0
                });                               
            }
        });
    }
    else {
        var itemValue = new FeeType();
        self.FeeTypeDet.push(itemValue);
    }
}



function FeeVM(FeeData) {

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

    var self = this;
    
    self.FeeErrors = ko.observable({});
    self.Fee = new Fee(FeeData);
    self.FeeType = new FeeType();    

    self.Save = function () {

        var varFeeCode = FeeData ? FeeData.FeeCode : "";//Inorder to identify whether the page is in new mode or edit mode

        var isValid = true;
        self.FeeErrors = ko.validation.group(self.Fee, { deep: true });
        if (self.FeeErrors().length != 0) {
            self.FeeErrors.showAllMessages(true);
            isValid = false;
        }

        if (parseFloat($('#FeeCode').val().trim()) <= parseFloat(0)) {
            $('#FeeCode_Validate').text(String.format(resourceProvider.MsgErrValueShouldBeBetween, resourceProvider.FeeCode,"001","999"));
            isValid = false;
        }
        
        if (isValid) {

            self.Fee.FeeTypeDTO = self.Fee.FeeTypeDet;
            var json1 = ko.toJSON(self.Fee);
            
            $.ajax({
                url: '/Master/FeeSave/',
                type: 'POST',
                dataType: 'json',
                data: ko.toJSON({ AFeeCode: varFeeCode, AFeeDTO: self.Fee, ACommand: resourceProvider.Save }),
                contentType: 'application/json; charset=utf-8',
                success: function (returnData) {
                    ShowMessage(returnData.RedirectUrl, returnData.Message);
                }
            });
        }
    };     

    self.Submit = function () {
        
        var isValid = true;
        //ko.validation.init({ grouping: { deep: true, observable: true } });
        self.FeeErrors = ko.validation.group(self.Fee, { deep: true });
        if (self.FeeErrors().length != 0) {
            self.FeeErrors.showAllMessages(true);
            isValid = false;
        }
        if (parseFloat($('#FeeCode').val().trim()) <= parseFloat(0)) {
            $('#FeeCode_Validate').text(String.format(resourceProvider.MsgErrValueShouldBeBetween, resourceProvider.FeeCode,"001","999"));
            isValid = false;
        }
       
        if (isValid) {
            var varFeeCode = FeeData ? FeeData.FeeCode : "";//Inorder to identify whether the page is in new mode or edit mode
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

                        /*---Submiting the record---*/
                        self.Fee.FeeTypeDTO = self.Fee.FeeTypeDet;
                        var json1 = ko.toJSON(self.Fee);

                        $.ajax({
                            url: '/Master/FeeSave/',
                            type: 'POST',
                            dataType: 'json',
                            data: ko.toJSON({ AFeeCode: varFeeCode, AFeeDTO: self.Fee, ACommand: resourceProvider.Submit }),
                            contentType: 'application/json; charset=utf-8',
                            success: function (returnData) {
                                ShowMessage(returnData.RedirectUrl, returnData.Message);
                            }
                        });
                        /*-----------*/
                    }},
                    { text: resourceProvider.No,
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
                        url: '/Master/FeeDelete/',
                        type: 'POST',
                        dataType: 'json',
                        data: ko.toJSON({ AFeeCode: self.Fee.FeeCode}),
                        contentType: 'application/json; charset=utf-8',
                        success: function (returnData) {
                            ShowMessage(returnData.RedirectUrl, returnData.Message);
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

        $("#dialog").text(resourceProvider.MsgConfDelete);
    };

    self.Cancel = function () {
        window.location.reload();
    };    

    self.AddFeeType = function () {
        self.Fee.FeeTypeDet.push({
            FeeId: self.Fee.FeeCode,
            FeeTypeCode: ko.observable("").extend({
                required: {
                    params: true,
                    message: String.format(resourceProvider.MsgErrPleaseSelect, resourceProvider.FeeType)
                }
            }),
            FeeAmount: ko.observable("").extend({
                required: {
                    params: true,
                    message: String.format(resourceProvider.MsgErrPleaseEnter2, resourceProvider.FeeAmount)
                },
                //min: {
                //    message: resourceProvider.MsgErrValueShouldBeGreaterThanCustom,
                //    params: 1
                //}
                validation: {
                    validator: checkMinMaxLimit,
                    message: String.format(resourceProvider.MsgErrValueShouldBeLessThan, resourceProvider.CreditLimitFrom, "999999999")
                }
            }),
            MonthsFrom: ko.observable(0).extend({
                required: {
                    params: true,
                    message: String.format(resourceProvider.MsgErrPleaseEnter2, resourceProvider.MonthsFrom)
                },
                min: {
                    message: resourceProvider.MsgErrValueShouldBeGreaterThanCustom,
                    params: 1
                },
                max: {
                    message: resourceProvider.MsgErrValueShouldBeLessThanCustom,
                    params: 12
                }
            }),
            ReasonCode: ko.observable("").extend({
                required: {
                    params: true,
                    message: String.format(resourceProvider.MsgErrPleaseEnter2, resourceProvider.ReasonCode)
                }
            }),

            FirstYearAmount: ko.observable("0").extend({
                validation: {
                    validator: checkMaxLimit,
                    message: String.format(resourceProvider.MsgErrValueShouldBeBetween, resourceProvider.Amount, "0", "999999999")
                }
            }),
            RenwalFee1: ko.observable("0").extend({
                validation: {
                    validator: checkMaxLimit,
                    message: String.format(resourceProvider.MsgErrValueShouldBeBetween, resourceProvider.Amount, "0", "999999999")
                }
            }),
            RenwalFee2: ko.observable("0").extend({
                validation: {
                    validator: checkMaxLimit,
                    message: String.format(resourceProvider.MsgErrValueShouldBeBetween, resourceProvider.Amount, "0", "999999999")
                }
            }),
            RenwalFee3: ko.observable("0").extend({
                validation: {
                    validator: checkMaxLimit,
                    message: String.format(resourceProvider.MsgErrValueShouldBeBetween, resourceProvider.Amount, "0", "999999999")
                }
            }),
            RenwalFee4: ko.observable("0").extend({
                validation: {
                    validator: checkMaxLimit,
                    message: String.format(resourceProvider.MsgErrValueShouldBeBetween, resourceProvider.Amount, "0", "999999999")
                }
            }),
            RenwalFee5: ko.observable("0").extend({
                validation: {
                    validator: checkMaxLimit,
                    message: String.format(resourceProvider.MsgErrValueShouldBeBetween, resourceProvider.Amount, "0", "999999999")
                }
            }),
            RenwalFee6: ko.observable("0").extend({
                validation: {
                    validator: checkMaxLimit,
                    message: String.format(resourceProvider.MsgErrValueShouldBeBetween, resourceProvider.Amount, "0", "999999999")
                }
            }),



            AdjustCode: "",
            Period: ko.observable(1).extend({
                required: {
                    params: true,
                    message: String.format(resourceProvider.MsgErrPleaseEnter2, resourceProvider.Period)
                }
            }),
            DiscountRatePercent: ko.observable("0").extend({
                validation: {
                    validator: checkInterestRate,
                    message: String.format(resourceProvider.MsgErrValueShouldBeLessThan, resourceProvider.DiscountRatePercent, "100"), params: 100
                }
            }),

            FlatFee : ko.observable("0").extend({
                validation: {
                    validator: checkMaxLimit,
                    message: String.format(resourceProvider.MsgErrValueShouldBeBetween, resourceProvider.Amount, "0", "999999999")
                }
            }),
            MinFee : ko.observable("0").extend({
                validation: {
                    validator: checkMaxLimit,
                    message: String.format(resourceProvider.MsgErrValueShouldBeBetween, resourceProvider.Amount, "0", "999999999")
                }
            }),

            Flag: 1
        });
    };

    self.RemoveFeeType = function () {
        self.Fee.FeeTypeDet.remove(this);
    };
}





