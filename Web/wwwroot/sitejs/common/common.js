$('input[type="text"], textarea').on('blur', function () {
    // Trim the value of the input and update it
    $(this).val($(this).val().trim());
});

function EnableDisableButton(element, action) {
    if (action) {
        $(element).attr("disabled", "disabled");
    }
    else {
        $(element).removeAttr("disabled");
    }
}

$(document).on('keydown', '.numbersOnly', function (event) {
    if (event.shiftKey == true) {
        event.preventDefault();
    }

    if ((event.keyCode >= 48 && event.keyCode <= 57) ||
        (event.keyCode >= 96 && event.keyCode <= 105) ||
        event.keyCode == 8 || event.keyCode == 9 || event.keyCode == 37 ||
        event.keyCode == 39 || event.keyCode == 46) {
    } else {
        event.preventDefault();
    }

    if ($(this).val().indexOf('.') !== -1 && event.keyCode == 190)
        event.preventDefault();
});

$('.numbersOnly').on("drag drop", function (e) {
    e.preventDefault();
});

$(document).on('keydown', '.decimalOnly', function (event) {
    if (event.shiftKey == true) {
        event.preventDefault();
    }

    if ((event.keyCode >= 48 && event.keyCode <= 57) ||
        (event.keyCode >= 96 && event.keyCode <= 105) ||
        event.keyCode == 8 || event.keyCode == 9 || event.keyCode == 37 ||
        event.keyCode == 39 || event.keyCode == 46 || event.keyCode == 190 || event.keyCode == 110) {

        if ($(this).val().indexOf('.') !== -1 && (event.keyCode == 190 || event.keyCode == 110)) {
            event.preventDefault();
        }
        if ($(this).val().indexOf('.') !== -1) {
            var indexOfDecimal = $(this).val().indexOf('.');

            var decimalPartLength = ($(this).val().split('.')[1]).length;
            if (decimalPartLength >= 2 && this.selectionStart > indexOfDecimal && event.keyCode != 8 && event.keyCode != 46 && event.keyCode != 37 && event.keyCode != 39) {
                event.preventDefault();
            }
        }
    } else {
        event.preventDefault();
    }
}).on("drag drop", function (e) {
    e.preventDefault();
});

let common = {

    isReferrer: function ()
    {
        let initreferrer = document.referrer;

        if (initreferrer == undefined || initreferrer == '' || initreferrer == window.location.origin + '/')
        {
            return true;
        }
        return initreferrer.toLowerCase() == window.location.href.toLowerCase();
    },
    validateDate: function ()
    {
        let dtValue = $('.datepicker').val();
        let dtRegex = new RegExp("^([0]?[1-9]|[1-2]\\d|3[0-1])-(JAN|FEB|MAR|APR|MAY|JUN|JULY|AUG|SEP|OCT|NOV|DEC)-[1-2]\\d{3}$", 'i');
        return dtRegex.test(dtValue);
    }


}


$(document).ajaxError(function (event, jqxhr, settings, exception) {
    if (jqxhr.status == 401) {
        toastr.error("Session Timeout");
        setTimeout(function () {
            location.href = "/Account/Login";
        }, 2000)
    }
});