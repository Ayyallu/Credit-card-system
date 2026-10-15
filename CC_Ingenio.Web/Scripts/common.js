/*To populate a cascading dropdown list

ADropDownListId : Id of the dropdown control to be populated
AUrl : <controller>/<action>?parameterName = parameterValue
*/


if (!String.format) {
    String.format = function (format) {
        var args = Array.prototype.slice.call(arguments, 1);
        return format.replace(/{(\d+)}/g, function (match, number) {
            return typeof args[number] != 'undefined'
              ? args[number]
              : match
            ;
        });
    };
}


function FormatOnBlur(e) {

    var element = e.target || window.event.srcElement;

    if (resourceProvider.ResourceCulture == "en-GB") {
        var num = parseFloat(element.value.replace(/,/g, ''))
        var formattedNum = num.toString().replace(/(\d)(?=(\d{3})+(?!\d))/g, "$1,")
    }
    else {
        var num = parseFloat(element.value.replace(/\./g, ''))
        var formattedNum = num.toString().replace(/(\d)(?=(\d{3})+(?!\d))/g, "$1.")
    }

    if (num.toString().length <= 9) {
        element.value = formattedNum
    }
    if (!num) {
        element.value = parseFloat(0)
    }
}



function FormatOnBlurWithDecimal(e) {

    var element = e.target || window.event.srcElement;

    if (resourceProvider.ResourceCulture == "en-GB") {
        var x = element.value.replace(/,/g, '').split('.');
        var x1 = x.length > 1 ? '.' + x[1] : '';
        var num = parseFloat(x[0].replace(/,/g, ''))
        var formattedNum = num.toString().replace(/(\d)(?=(\d{3})+(?!\d))/g, "$1,")
    }
    else {
        var x = element.value.replace(/\./g, '').split(',');
        var x1 = x.length > 1 ? ',' + x[1] : '';
        var num = parseFloat(x[0].replace(/\./g, ''))
        var formattedNum = num.toString().replace(/(\d)(?=(\d{3})+(?!\d))/g, "$1.")
    }


    if (num.toString().length <= 9) {
        if (!isNaN(parseFloat(formattedNum))) {
            element.value = formattedNum + x1
        }
        else {
            element.value = x1
        }
    }

    //if (isNaN(parseFloat(num))) {
    //    element.value = parseFloat(0)
    //}
}


function validateFloatKeyPress(el, evt) {
    var charCode = (evt.which) ? evt.which : event.keyCode;

    var dot = Globalize.cultures[Globalize.cultureSelector].numberFormat['.']; // Get decimal seperator according to current culture.
    
    //var number = el.value.split(dot);
    //if (charCode != 44 && charCode != 46 && charCode > 31 && (charCode < 48 || charCode > 57)) {
    //    return false;
    //}
    //if (charCode == 46 && number.length > 1) {
    //    return false;
    //}

    var number = el.value.split(dot);
    if (resourceProvider.ResourceCulture == "en-GB") {
        if (charCode != 46  && charCode > 31 && (charCode < 48 || charCode > 57)) {
            return false;
        }
        if (charCode == 46 && number.length > 1) {
            return false;
        }
    }
    else {
        if (charCode != 44 && charCode > 31 && (charCode < 48 || charCode > 57)) {
            return false;
        }
        if (charCode == 44 && number.length > 1) {
            return false;
        }
    }



    //get the carat position
    var caratPos = getSelectionStart(el);
    //var dotPos = el.value.indexOf(".");
    var dotPos = el.value.indexOf(dot);
    if (caratPos > dotPos && dotPos > -1 && (number[1].length > 1)) {
        return false;
    }
    return true;
}

function validateFiveDecFloatKeyPress(el, evt) {
    var charCode = (evt.which) ? evt.which : event.keyCode;

    var dot = Globalize.cultures[Globalize.cultureSelector].numberFormat['.']; // Get decimal seperator according to current culture.

    //var number = el.value.split('.');
    var number = el.value.split(dot);
    if (charCode != 44 && charCode != 46 && charCode > 31 && (charCode < 48 || charCode > 57)) {
        return false;
    }
    if (charCode == 46 && number.length > 4) {
        return false;
    }
    //get the carat position
    var caratPos = getSelectionStart(el);
    //var dotPos = el.value.indexOf(".");
    var dotPos = el.value.indexOf(dot);
    if (caratPos > dotPos && dotPos > -1 && (number[1].length > 4)) {
        return false;
    }
    return true;
}

function getSelectionStart(o) {
    if (o.createTextRange) {
        var r = document.selection.createRange().duplicate()
        r.moveEnd('character', o.value.length)
        if (r.text == '') return o.value.length
        return o.value.lastIndexOf(r.text)
    } else return o.selectionStart
}



/*New button page reload function*/
$(function () {
    $("input#New").on('click', function () {
        var newpath = '';
        var path = window.location.pathname;
        var href = window.location.href.split('/');
        var substring = href[4].split('?');
        if (substring.length > 0) {
            newpath = '/' + href[3] + '/' + substring[0];
        }
        else {
            newpath = '/' + href[3] + '/' + href[4];
        }

        document.location.href = newpath
    });
});

function AllowOnlyAmountWithoutDot(event) {
    var charCode = 0;
    if (event.charCode) {
        charCode = event.charCode;
    }
    else {
        charCode = event.keyCode;
    }


    if (resourceProvider.ResourceCulture == "en-GB") {
        if ((charCode < 48 || charCode > 57)) {
            if (charCode == 8 || charCode == 0 || charCode == 9 || charCode == 44) {
                return true;
            }
            else {
                return false;
            }
        }
    }
    else {
        if ((charCode < 48 || charCode > 57)) {
            if (charCode == 8 || charCode == 0 || charCode == 9 || charCode == 46) {
                return true;
            }
            else {
                return false;
            }
        }
    }


    //if ((charCode < 48 || charCode > 57)) {
    //    if (charCode == 8 || charCode == 0 || charCode == 9 || charCode == 44) {
    //        return true;
    //    }
    //    else {
    //        return false;
    //    }
    //}

}

//function AllowOnlyAmountByCurrencyPrecision(el, evt, precision) {
    
//    var charCode = (evt.which) ? evt.which : event.keyCode;
    
//    var dot = Globalize.cultures[Globalize.cultureSelector].numberFormat['.']; // Get decimal seperator according to current culture.

//    var number = el.value.split(dot);
         
//    if (charCode != 44 && charCode != 46 && charCode > 31 && (charCode < 48 || charCode > 57)) {
//        return false;
//    }
    
//    if ((charCode == 44 || charCode == 46) && number.length > 1) {
//        return false;
//    }
//    if ((charCode == 44 || charCode == 46) && precision == 0) {
//        return false;
//    }

//    //get the carat position
//    var caratPos = getSelectionStart(el);

//    var dotPos = el.value.indexOf(dot);
             
//    if (caratPos > dotPos && dotPos > -1 && (number[1].length >= precision)) {
//        return false;
//    }
//    return true;
//}



function AllowOnlyAmountByCurrencyPrecision(el, evt, precision) {
    
    var charCode = (evt.which) ? evt.which : event.keyCode;
    
    var dot = Globalize.cultures[Globalize.cultureSelector].numberFormat['.']; // Get decimal seperator according to current culture.

    var number = el.value.split(dot);
         
    if (charCode != 44 && charCode != 46 && charCode > 31 && (charCode < 48 || charCode > 57)) {
        return false;
    }
    

    if (resourceProvider.ResourceCulture == "en-GB") {
        if ((charCode == 46) && number.length > 1) {
            return false;
        }
        if ((charCode == 46) && precision == 0) {
            return false;
        }
    }
    else {
        if ((charCode == 44) && number.length > 1) {
            return false;
        }
        if ((charCode == 44) && precision == 0) {
            return false;
        }
    }


   

    //get the carat position
    var caratPos = getSelectionStart(el);

    var dotPos = el.value.indexOf(dot);
             
    if (caratPos > dotPos && dotPos > -1 && (number[1].length >= precision)) {
        return false;
    }
    return true;
}



function PopulateDropDownViaAjax(ADropDownListId, AUrl) {
    $.ajaxSetup({ cache: false });
    $.getJSON(AUrl,
        function (modelData) {
            var select = $(ADropDownListId);
            select.empty();
            /*  select.append($('<option/>',
                  {
                  value: 0,
                  text: "Select"
                  }));
                  */
            $.each(modelData, function (index, itemData) {
                select.append($('<option/>',
                {
                    value: itemData.Value,
                    text: itemData.Text
                }));
            });
        });

}

function PopulateAllDropDownWithSameIDViaAjax(ADropDownListId, AUrl) {

    $.getJSON(AUrl,
     function (modelData) {
         var selectorArray = $("[id = " + ADropDownListId + "]");

         $.each(selectorArray, function () {
             var select = $(this);
             var selectedVal = select.val();
             //var val = select.index;
             select.empty();
             $.each(modelData, function (index, itemData) {
                 select.append($('<option/>',
                 {
                     value: itemData.Value,
                     text: itemData.Text
                 }));
             });
             select.val(selectedVal);
         });
     });
}



function EnableDisableControls(formStatus) {

    if (formStatus == 'NA') {
        // FormStaus = 'NA' for pages which doesn't have any approval workflow.
        //DON'T Remove this condition. All buttons should be enabled.
    }

    if (formStatus == null || formStatus == '') {
        if ($("#Delete").length) { $("#Delete").removeClass('Greenbutton').addClass('BtnDisabled'); }
        if ($("#DeleteKO").length) { $("#DeleteKO").removeClass('Greenbutton').addClass('BtnDisabled'); }
        $("#Authorize").removeClass('Greenbutton').addClass('BtnDisabled');
        if ($("#Delete").length) { $("#Delete").attr('disabled', 'disabled'); }
        if ($("#DeleteKO").length) { $("#DeleteKO").attr('disabled', 'disabled'); }
        $("#Authorize").attr('disabled', 'disabled');
        return;
    }

    if (formStatus != null && (formStatus == 'EXP')) {

        $("#Save").removeClass('Greenbutton').addClass('BtnDisabled');
        if ($("#Submit").length) { $("#Submit").removeClass('Greenbutton').addClass('BtnDisabled'); }
        if ($("#SubmitKO").length) { $("#SubmitKO").removeClass('Greenbutton').addClass('BtnDisabled'); }
        $("#Authorize").removeClass('Greenbutton').addClass('BtnDisabled');

        $("#Save").attr('disabled', true);
        if ($("#Submit").length) { $("#Submit").attr('disabled', true); }
        if ($("#SubmitKO").length) { $("#SubmitKO").attr('disabled', true); }
        $("#Authorize").attr('disabled', true);
        return;
    }

    if (formStatus != null && formStatus != 'A') {

        if (formStatus == 'P' || formStatus == 'DIS' || formStatus == 'DEL') {

            $("#Save").removeClass('Greenbutton').addClass('BtnDisabled');
            if ($("#Submit").length) { $("#Submit").removeClass('Greenbutton').addClass('BtnDisabled'); }
            if ($("#SubmitKO").length) { $("#SubmitKO").removeClass('Greenbutton').addClass('BtnDisabled'); }
            if ($("#Delete").length) { $("#Delete").removeClass('Greenbutton').addClass('BtnDisabled'); }
            if ($("#DeleteKO").length) { $("#DeleteKO").removeClass('Greenbutton').addClass('BtnDisabled'); }

            $("#Save").attr('disabled', true);
            if ($("#Submit").length) { $("#Submit").attr('disabled', true); }
            if ($("#SubmitKO").length) { $("#SubmitKO").attr('disabled', true); }
            if ($("#Delete").length) { $("#Delete").attr('disabled', true); }
            if ($("#DeleteKO").length) { $("#DeleteKO").attr('disabled', 'disabled'); }
        }
        else if (formStatus == 'D') {
            if ($("#Authorize").length) { $("#Authorize").removeClass('Greenbutton').addClass('BtnDisabled'); }
            if ($("#Authorize").length) { $("#Authorize").attr('disabled', true); }
        }
        return;
    }
    if (formStatus != null && (formStatus == 'A')) {

        $("#Save").removeClass('Greenbutton').addClass('BtnDisabled');
        if ($("#Submit").length) { $("#Submit").removeClass('Greenbutton').addClass('BtnDisabled'); }
        if ($("#SubmitKO").length) { $("#SubmitKO").removeClass('Greenbutton').addClass('BtnDisabled'); }
        $("#Authorize").removeClass('Greenbutton').addClass('BtnDisabled');

        $("#Save").attr('disabled', true);
        if ($("#Submit").length) { $("#Submit").attr('disabled', true); }
        if ($("#SubmitKO").length) { $("#SubmitKO").attr('disabled', true); }
        $("#Authorize").attr('disabled', true);
    }
}

function ShowModalDialog(InfoText, Content) {
    $("#dialog").dialog({
        resizable: false,
        height: 160,
        modal: true,
        title: InfoText,
        buttons: [{
            class: "LeftButton",
            text: resourceProvider.Yes,
            click: function (e) {

                $(this).dialog("close");

                return;
            }
        },
           {
               text: resourceProvider.No,
               click: function () {
                   $(this).dialog("close");
               }
           }]
    });

    $("#dialog").html(Content);
    $("#dialog").dialog("open");
    return false;
}

$(function () {
    $("a.delete-link").click(function (e) {
        var url = $(this).attr("href");
        e.preventDefault();
        $("#dialog").dialog({
            resizable: false,
            height: 160,
            modal: true,
            title: resourceProvider.ApplicationName,
            buttons: [
                {
                    class: "LeftButton",
                    text: resourceProvider.Yes,
                    click: function () {
                        $.post(url, function (data) {
                            $("#dialog").dialog("close");
                            $("#frmList").submit();
                        });
                    }
                },
                {
                    text: resourceProvider.No,
                    click: function () { $(this).dialog("close"); }
                }
            ]
        });
        $("#dialog").text(resourceProvider.MsgConfDelete);
        //$("#dialog").htmlEscape = "false";
        $("#dialog").dialog("open");
    });
});

function GetQueryStrings() {
    var urlParams;

    var match,
        pl = /\+/g,  // Regex for replacing addition symbol with a space
        search = /([^&=]+)=?([^&]*)/g,
        decode = function (s) { return decodeURIComponent(s.replace(pl, " ")); },
        query = window.location.search.substring(1);

    urlParams = {};
    while (match = search.exec(query)) {

        urlParams[decode(match[1])] = decode(match[2]);
    }
    return urlParams;
}

function setConfirmUnload(on) {

    window.onbeforeunload = (on) ? unloadMessage : null;

}

function unloadMessage() {

    return resourceProvider.MsgInfoUnload;

}

function HandleVisiblity(ButtonId, IsVisible) {
    $(function () {
        if (IsVisible)
            $(ButtonId).show();
        else
            $(ButtonId).hide();
    });
}

$(function () {
    $('.Number').blur(function () {
        var obj = $(this);
        //To set default value zero if any decimal/ integer textbox is empty
        if (obj.val().trim() == "") {
            obj.val(0);
            $(this).valid();
            //obj.focus();
        }

        var obj = $(this);
        //To trim white space
        if (obj.val().indexOf(' ') > 0) {
            obj.val($(this).val().replace(/\s/g, ""));
            $(this).valid();
        }
        //To trim zero on leftside of entered value
        if (parseFloat(this.value) > 0) {
            var obj = $(this);
            obj.val(parseFloat((ltrim(this.value, '0'))));
            $(this).valid();
        }
        else {
            if (parseFloat(this.value) == 0) {

                obj.val(parseFloat(this.value));
                $(this).valid();
            }
        }
    });
});
/*Allow 9 digit number in textbox.Call this function on 'keypress' event of textbox */
$(function () {
    $('.NumericOnly').keypress(function (e) {
        var val = $(this).val();
        var regex = /^\d{1,20}?$/;
        if (regex.test(val + String.fromCharCode(e.charCode))) {
            return true;
        }
        return false;
    });

    $('.NumericWithDotComma').keypress(function (e) {
        var val = $(this).val();
        /*currently it is given like 9 digits in integer part and 2 digits in decimal part.*/
        //var regex = /^\d{1,9}(\.\d{0,2})?$/;

        if (resourceProvider.ResourceCulture == "en-GB")
            var regex = /^\d{1,9}(\,\d{0,})?$/;
        else
        var regex = /^\d{1,9}(\.\d{0,2})?$/;

        if (regex.test(val + String.fromCharCode(e.charCode))) {
            return true;
        }
        else {
            //to do-if decimal part is 2 digit,take decimal part and check if its length = 2 and return true
        }
        return false;
    });
    //$('.NumericOnly').blur(function () {
    //    if (!this.value.match(/^\d{1,9}?$/)) {
    //        this.value = this.value.replace(/^\d{1,9}?$/, '');
    //    }
    //});

    /*Allow number upto 2 decimal place in textbox.Call this function on 'keypress' event of textbox */
    $('.NumericWithDecimalPlace').keypress(function (e) {
        var val = $(this).val();
        /*currently it is given like 9 digits in integer part and 2 digits in decimal part.*/
        //var regex = /^\d{1,9}(\.\d{0,2})?$/;
                
        if (resourceProvider.ResourceCulture == "en-GB")
            var regex = /^\d{1,9}(\.\d{0,2})?$/;
        else
            var regex = /^\d{1,9}(\,\d{0,2})?$/;
                
        if (regex.test(val + String.fromCharCode(e.charCode))) {
            return true;
        }
        else {
            //to do-if decimal part is 2 digit,take decimal part and check if its length = 2 and return true
        }
        return false;
    });

    $('.NumericWithFiveDecimalPlace').keypress(function (e) {
        var val = $(this).val();
        /*currently it is given like 9 digits in integer part and 2 digits in decimal part.*/
        //var regex = /^\d{1,9}(\.\d{0,2})?$/;

        if (resourceProvider.ResourceCulture == "en-GB")
            var regex = /^\d{1,9}(\.\d{0,5})?$/;
        else
            var regex = /^\d{1,9}(\,\d{0,5})?$/;

        if (regex.test(val + String.fromCharCode(e.charCode))) {
            return true;
        }
        else {
            //to do-if decimal part is 2 digit,take decimal part and check if its length = 2 and return true
        }
        return false;
    });

    /*Users should be allowed to enter only Alphabets and Numbers in a TextBox. 
    Non-alphanumeric characters should be disallowed.Call this function on 'keypress' event of textbox*/
    $('.AlphaNumericOnly').keypress(function (e) {
        var code = e.keyCode ? e.keyCode : e.which; // Get the key code.
        var pressedKey = String.fromCharCode(code); // Find the key pressed.
        if (!pressedKey.match(/[a-zA-Z0-9]/g)) // Check if it's a alpanumeric char or not.
        {
            e.preventDefault(); // If it is not then prevent the event from happening.
        }
    });
    /*This prevent users from copying and pasting non-alphanumeric characters into the TextBox.
    Call this function 'onBlur' event of textbox*/
    //$('.AlphaNumericOnly').blur(function (e) {
    //    if (this.value.match(/[^a-zA-Z0-9 ]/g)) {
    //        this.value = this.value.replace(/[^a-zA-Z0-9]/g, '');
    //    }
    //});

    $('.PasswordValidChars').keypress(function (e) {
        var code = e.keyCode ? e.keyCode : e.which; // Get the key code.
        var pressedKey = String.fromCharCode(code); // Find the key pressed.
        if (!pressedKey.match(/[a-zA-Z0-9@.]/g)) // Check if it's a alpanumeric char or not.
        {
            e.preventDefault(); // If it is not then prevent the event from happening.
        }
    });

    /*For number and comma*/
    $('.NumberComma').keypress(function (e) {
        var code = e.keyCode ? e.keyCode : e.which; // Get the key code.
        var pressedKey = String.fromCharCode(code); // Find the key pressed.
        if (!pressedKey.match(/[\d,]+/g)) // Check if it's a alpanumeric char or comma.
        {
            e.preventDefault(); // If it is not then prevent the event from happening.
        }
    });

    /*For number and comma*/
    $('.NumberCommaDot').keypress(function (e) {
        var code = e.keyCode ? e.keyCode : e.which; // Get the key code.
        var pressedKey = String.fromCharCode(code); // Find the key pressed.

        if (resourceProvider.ResourceCulture == "en-GB") {
            if (!pressedKey.match(/[\d,]+/g)) // Check if it's a alpanumeric char or comma.
            {
                e.preventDefault(); // If it is not then prevent the event from happening.
            }
        }
        else {
            if (!pressedKey.match(/[\d.]+/g)) // Check if it's a alpanumeric char or comma.
            {
                e.preventDefault(); // If it is not then prevent the event from happening.
            }
        }
        

    });

    /*For number and comma*/
    $('.NumberHyphen').keypress(function (e) {
        var code = e.keyCode ? e.keyCode : e.which; // Get the key code.
        var pressedKey = String.fromCharCode(code); // Find the key pressed.
        if (!pressedKey.match(/[\d-Kk]+/g)) // Check if it's a alpanumeric char or comma.
        {
            e.preventDefault(); // If it is not then prevent the event from happening.
        }
    });



});

/*Allow number in textbox.Call this function on 'keypress' event of textbox */
function OnlyNumeric(event) {

    var charCode = 0;
    if (event.charCode) {
        charCode = event.charCode;
    }
    else {
        charCode = event.keyCode;
    }

    if ((charCode < 48 || charCode > 57)) {
        if (charCode == 8 || charCode == 0 || charCode == 9) {
            return true;
        }
        else {
            return false;
        }
    }
}

function ltrim(str, chars) {
    chars = chars || "\\s";
    return str.replace(new RegExp("^[" + chars + "]+", "g"), "");
}

$(function () {
    var flag = '0';
    $("input#Delete").on('click', function (e) {
        if ($('#Delete').is(':disabled') == true) { return false; };
        //$("#Reset").click();  // To reset changes if any done on fields before delete button click
        if (flag == '0') {
            e.preventDefault();
        }
        $("#dialog").dialog({
            resizable: false,
            height: 160,
            modal: true,
            title: resourceProvider.ApplicationName,
            buttons: [{
                class: "LeftButton",
                text: resourceProvider.Yes,
                click: function () {
                    flag = '1';
                    window.location.reload();
                    $("#Delete").click();
                    $(this).dialog("close");
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

        $("#dialog").dialog("open");
    });
});

$(function () {
    var flag = '0';
    $("input#Submit").on('click', function (e) {
        if ($('#Submit').is(':disabled') == true) { return false; };
        if (flag == '0') {
            var form = $(this).closest("form");
            var form_status = $(this).closest("form").valid();
            e.preventDefault();
            if (!form_status) {
                return false;
            }
        }
        $("#dialog").dialog({
            resizable: false,
            width: 400,
            height: 160,
            modal: true,
            title: resourceProvider.ApplicationName,
            buttons: [{
                class: "LeftButton",
                text: resourceProvider.Yes,
                click: function () {
                    flag = '1';
                    $("#Submit").click();
                    $(this).dialog("close");
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

        $("#dialog").dialog("open");

    });
});

/*
To show message box

    MessageToShow - Message to display in message box
    UrlToRedirect - Url of the page to which we need to redirect after OK button click in message box
                    (If you don't have any page redirect, give empty value)
*/
function ShowSimpleModalDialog(MessageToShow, UrlToRedirect) {

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
                class: "OKButton",
                width: "100%",
                click: function () {
                    $(this).dialog("close");
                    if (UrlToRedirect.length != 0) {
                        window.location.href = UrlToRedirect;
                    }
                }
            }]
        });
        $('#dialog').text(MessageToShow);
    }
}

function DisableBtnAuthorize() {
    if ($("#Authorize").length) { $("#Authorize").removeClass('Greenbutton').addClass('BtnDisabled'); }
    if ($("#Authorize").length) { $("#Authorize").attr('disabled', true); }
}

function RefreshDropDownList(ADropDownListId) {

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
