function setTimezoneCookie() {
    let timezone_cookie = "timezoneoffset";
    // if the timezone cookie not exists create one.
    if (!$.cookie(timezone_cookie)) {
        // check if the browser supports cookie
        let test_cookie = 'test cookie';
        $.cookie(test_cookie, true);

        // browser supports cookie
        if ($.cookie(test_cookie)) {

            // delete the test cookie
            $.cookie(test_cookie, null);

            // create a new cookie
            $.cookie(timezone_cookie, -(new Date().getTimezoneOffset()));
        }
    }
    else {
        // if the current timezone and the one stored in cookie are different
        // then store the new timezone in the cookie and refresh the page.
        let storedOffset = parseInt($.cookie(timezone_cookie));
        let currentOffset = -(new Date().getTimezoneOffset());

        // user may have changed the timezone
        if (storedOffset !== currentOffset) {
            $.cookie(timezone_cookie, -(new Date().getTimezoneOffset()));
        }
    }
}

function SendForgotPasswordEmail(_this) {
    EnableDisableButton($(_this), true);
    $("#toast-container").remove();

    if ($("#ForgotPasswordFrm").valid()) {
        $.ajax({
            type: 'Post',
            url: "/Account/ForgetPassword",
            data: $("#ForgotPasswordFrm").serialize(),
            success: function (data) {
                toastr.success(data.message);
                setTimeout(function () {
                    window.location.href = "/Account/Login";
                }, setTimeoutIntervalEnum.onRedirection);
            },
            error: function (data) {
                if (data.responseJSON)
                {
                    toastr.error(data.responseJSON?.message)
                }
                else {
                    toastr.error(somethingWrongMsg);
                }
                EnableDisableButton($(_this), false);
            }
        });
    }
    else {
        EnableDisableButton($(_this), false);
        return false;
    }
}

function ResetPassword(_this) {
    EnableDisableButton($(_this), true);
    $("#toast-container").remove();
    let password = $("#Password").val().trim();
    if ($("#RestPasswordFrm").valid() && password != "") {
        $.ajax({
            type: 'Post',
            url: "/Account/ResetPassword",
            data: $("#RestPasswordFrm").serialize(),
            success: function (response) {
                toastr.success(response.message);
                setTimeout(function () {
                    window.location.href = "/Account/Login";
                }, setTimeoutIntervalEnum.onRedirection);
            },
            error: function (response) {
                toastr.error(response.responseJSON?.message);
                EnableDisableButton($(_this), false);
            }
        });
    }
    else {
        EnableDisableButton($(_this), false);
        return false;
    }
}

function SignUp(_this) {
    EnableDisableButton($(_this), true);
    $("#toast-container").remove();
    let password = $("#Password").val().trim();
    if ($("#frmSignup").valid() && password != "") {
        $.ajax({
            type: 'Post',
            url: "/Account/Registration",
            data: $("#frmSignup").serialize(),
            success: function (response) {
                toastr.success(response.message);
                setTimeout(function () {
                    window.location.href = "/Account/Login";
                }, setTimeoutIntervalEnum.onRedirection);
            },
            error: function (response) {
                toastr.error(response.responseJSON?.message);
                EnableDisableButton($(_this), false);
            }
        });
    }
    else {
        EnableDisableButton($(_this), false);
        return false;
    }
}
function SendVerifyEmailLink(_this) {
    EnableDisableButton($(_this), true);
    $("#toast-container").remove();

    if ($("#VerifyEmailFrm").valid()) {
        $.ajax({
            type: 'Post',
            url: "/Account/VerifyEmail",
            data: $("#VerifyEmailFrm").serialize(),
            success: function (data) {
                toastr.success(data.message);
                setTimeout(function () {
                    window.location.href = "/Account/Login";
                }, setTimeoutIntervalEnum.onRedirection);
            },
            error: function (data) {
                if (data.responseJSON)
                {
                    toastr.error(data.responseJSON?.message);
                }
                else {
                    toastr.error(somethingWrongMsg);
                }
                EnableDisableButton($(_this), false);
            }
        });
    }
    else {
        EnableDisableButton($(_this), false);
        return false;
    }
}
