function CallControllerMethod(model, controllerName, actionMethod) {
    $.ajax({
        url: '@Url.Action(' + actionMethod + ',' + controllerName + ')',
        cache: false,
        type: 'POST',
        contentType: 'application/json; charset=utf-8',
        data: ko.toJSON(model),
        success: function () {
            return true;
        }
    }).fail(function () { return false; });

}
