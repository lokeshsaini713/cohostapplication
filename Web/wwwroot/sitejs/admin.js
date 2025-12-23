$(function () {
    $.when(users.ResetStateSaveDataTable()).then(function () {
        users.List();
    });
});

let MemberEmail = [];
let isValidate = true;
let table = $('#data-table_users');

let users = {

    ResetStateSaveDataTable: function () {
        if (common.isReferrer()) {
            table.DataTable().state.clear();
        }
    },

    List: function () {
        let columns = [];
        $(table).find('thead tr th').each(function (i, v) {

            let propName = $(v).data('name');
            let column;

            switch (propName.toLowerCase()) {
                case "id":
                    column = {
                        data: null, name: null, width: "23px", maxWidth: "23px", render: function (data, type, row, meta) {
                            return meta.row + meta.settings._iDisplayStart + 1;
                        }
                    };
                    break;
                case "name":
                    column = {
                        data: null, name: propName, class: 'fw-semibold', mRender: function (data, row) {
                            return `${data.name}`;
                        }
                    };
                    break;
                case "isactive":
                    column = {
                        data: null, name: propName, mRender: function (data, row) {
                            if (data.isActive) {
                                return `<label class="switch">
                                        <input type="checkbox" class="user-status" checked onclick="ChangeUserStatus(` + data.id + `, false, ` + propName.toLowerCase().includes("isdeleted") + `, true, this)">
                                        <span class="slider activeSlide"></span>
                                    </label>`;
                            }
                            else {
                                return `<label class="switch">
                                        <input type="checkbox" class="user-status" onclick="ChangeUserStatus(` + data.id + `, true, ` + propName.toLowerCase().includes("isdeleted") + `, true, this)">
                                        <span class="slider"></span>
                                    </label>`;
                            }
                        }
                    };
                    break;
                case "isdeleted":
                    column = {
                        data: null, name: propName, mRender: function (data, row) {
                            if (data.isDeleted) {
                                return `<i class="fas fa-check" title="Deleted" style="cursor: pointer;" onclick="ChangeUserStatus(` + data.id + `, ` + data.isActive + `, false, false, this)"></i>`;
                            }
                            else {
                                return `<i class="fas fa-trash" title="Not Deleted" style="cursor: pointer;" onclick="ChangeUserStatus(` + data.id + `, ` + data.isActive + `, true, false, this)"></i>`;
                            }
                        }
                    };
                    break;
                case "action":
                    column = {
                        data: null, name: null, mRender: function (data, row) {
                            return `<a href="/Admin/User/Detail?id=${data.id}" class="me-2" onclick = "" title = "Edit" > <i class="fas fa-edit"></i></a >`;
                        }
                    };
                    break;
                default:
                    column = { data: propName.charAt(0).toLowerCase() + propName.slice(1), name: propName };
                    break;
            }

            columns.push(column);
        });

        let orderColumns = [0, 7];
        tbl.BindTable(table, `/Admin/User/Index`, {}, columns, orderColumns);
    },
    UpdateUser: function (_this) {
        if (!$("#frmUpdateUser").valid()) {
            return;
        }

        EnableDisableButton($(_this), true);

        ajax.Post(`/Admin/User/Detail`, $('#frmUpdateUser').serialize(),
            function (response) {
                EnableDisableButton(this, false);
                toastr.success(response.message);
                setTimeout(function () {
                    location.href = '/Admin/User/Index'
                }, setTimeoutIntervalEnum.onRedirection);
            },
            function (error) {

            }
        );

    }
}



function ChangePasswordAdmin(_this) {
    EnableDisableButton($(_this), true);
    toastr.remove();
    if ($("#ChangePasswordFrm").valid()) {
        $.ajax({
            type: 'Post',
            url: "/Admin/User/ChangePassword",
            data: $("#ChangePasswordFrm").serialize(),
            success: function (response) {
                toastr.success(response.message);
                setTimeout(function () {
                    window.location.href = "/Account/Logout";
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

function ChangeUserStatus(userId, activeStatus, deleteStatus, isActivateStatusChange, _this) {
    toastr.remove();
    let isChecked = _this.hasAttribute("checked")
    let textMessage = "";

    if (isActivateStatusChange) {
        if (activeStatus) {
            textMessage = $("#activateUserMsg").val()
        }
        else {
            textMessage = $("#deactivateUserMsg").val()
        }
    }
    else if (deleteStatus) {
        textMessage = $("#deleteUserMsg").val();
    }
    else {
        textMessage = $("#recoverUserMsg").val()
    }

    swal({
        title: 'Are you sure?',
        text: textMessage,
        type: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes'
    }).then(function (result)
    {
        if (result) {
            $.ajax({
                url: '/Admin/User/ChangeUserStatus',
                data:
                {
                    UserId: userId,
                    ActiveStatus: activeStatus,
                    DeleteStatus: deleteStatus,
                    IsActiveStatusChange: isActivateStatusChange
                },
                type: "Post",
                success: function (response) {
                    toastr.success(response.message);
                    setTimeout(function () {
                        window.location.reload();
                    }, setTimeoutIntervalEnum.onRedirection);
                },
                error: function (response) {
                    toastr.error(response.responseJSON?.message);
                }
            });

            LoadTable();
        }
        else if (result.dismiss === Swal.DismissReason.cancel) {
            // Add your cancel button event handling here
            if (isChecked) {
                $(_this).addClass("activeSlide");
            }
            else {
                $(_this).removeClass("activeSlide");
            }
        }
    });
}
