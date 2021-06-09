//Tạo gui id
function Guid() {
    function s4() {
        return Math.floor((1 + Math.random()) * 0x10000)
            .toString(16)
            .substring(1);
    }
    return s4() + s4() + '-' + s4() + '-' + s4() + '-' + s4() + '-' + s4() + s4() + s4();
}

function SetModelFormFix(prefix, objectModel) {
    Object.keys(objectModel).forEach(function (key) {
        var element = $('input[id =' + prefix + key + '],' + 'select[id = ' + prefix + key + '],' + 'textarea[id = ' + prefix + key + ']');
        if (element.length == 1) {
            element.val(objectModel[key]);
            if (element[0].type === "date") {
                if (objectModel[key] != null) {
                    var date = new Date(parseInt(objectModel[key].substr(6)));
                    element.val(formatDate(date));
                }
            } else if (element[0].type === "checkbox") {
                element.checked = objectModel[key];
            }
        }
        else if (element.length > 1) {
            switch (element[0].type) {
                case 'radio':
                    $.each(element, function (index, item) {
                        if (item.value == objectModel[key]) {
                            item.checked = true;
                        } else {
                            item.checked = false;
                        }
                    });
                    break;
                case 'checkbox':
                    objectModel[key] = [];
                    $.each(element, function (index, item) {
                        if (item.value === objectModel[key]) {
                            if (item.value === objectModel[key]) {
                                item.checked = true;
                            } else {
                                item.checked = false;
                            }
                        }
                    });
                    break;
            }
        }
    });
}

function SetModelForm(objectModel) {
    Object.keys(objectModel).forEach(function (key) {
        var element = $('input[id =' + key + '],' + 'select[id = ' + key + '],' + 'textarea[id = ' + key + ']');
        if (element.length == 1) {
            element.val(objectModel[key]);
            //if (element[0].type === "date" && objectModel[key] === "") {
            //    objectModel[key] = null;
            //} else if (element[0].type === "select") {
            //    objectModel[key] = element.value;
            //} else 
            if (element.type === "checkbox") {
                element.checked = objectModel[key];
            }
        }
        else if (element.length > 1) {
            switch (element[0].type) {
                case 'radio':
                    $.each(element, function (index, item) {
                        if (item.value === objectModel[key]) {
                            item.checked = true;
                        } else {
                            item.checked = false;
                        }
                    });
                    break;
                case 'checkbox':
                    objectModel[key] = [];
                    $.each(element, function (index, item) {
                        if (item.value === objectModel[key]) {
                            if (item.value === objectModel[key]) {
                                item.checked = true;
                            } else {
                                item.checked = false;
                            }
                        }
                    });
                    break;
            }
        }
    });
}

//Đổ dữ liệu vào model
function GetModelFormFix(prefix, objectModel) {
    Object.keys(objectModel).forEach(function (key) {
        var element = $('input[id =' + prefix + key + '],' + 'select[id = ' + prefix + key + '],' + 'textarea[id = ' + prefix + key + ']');
        if (element.length == 1) {
            objectModel[key] = element.val();
            if (element[0].type === "date" && objectModel[key] === "") {
                objectModel[key] = null;
            } else if (element[0].type === "select") {
                objectModel[key] = element.value;
            } else if (element[0].type === "checkbox") {
                objectModel[key] = element.is(':checked');
            }
        }
        else if (element.length > 1) {
            switch (element[0].type) {
                case 'radio':
                    $.each(element, function (index, item) {
                        if (item.checked) {
                            objectModel[key] = item.value;
                        }
                    });
                    break;
                case 'checkbox':
                    objectModel[key] = [];
                    $.each(element, function (index, item) {
                        if (item.checked) {
                            objectModel[key].push(item.value);
                        }
                    });
                    break;
            }
        }
    });
}

function GetModelForm(objectModel) {
    Object.keys(objectModel).forEach(function (key) {
        var element = $('input[id =' + key + '],' + 'select[id = ' + key + '],' + 'textarea[id = ' + key + ']');
        if (element.length == 1) {
            objectModel[key] = element.val();
            if (element[0].type === "date" && objectModel[key] === "") {
                objectModel[key] = null;
            } else if (element[0].type === "select") {
                objectModel[key] = element.value;
            } else if (element[0].type === "checkbox") {
                objectModel[key] = element.is(':checked');
            }
        }
        else if (element.length > 1) {
            switch (element[0].type) {
                case 'radio':
                    $.each(element, function (index, item) {
                        if (item.checked) {
                            objectModel[key] = item.value;
                        }
                    });
                    break;
                case 'checkbox':
                    objectModel[key] = [];
                    $.each(element, function (index, item) {
                        if (item.checked) {
                            objectModel[key].push(item.value);
                        }
                    });
                    break;
            }
        }
    });
}

//Check validate trên form
function ValidateFormFix(prefix, objectModel) {
    var statusValidate = false;
    Object.keys(objectModel).forEach(function (key) {
        var element = $('#' + prefix + key);

        if (element != null && element.length > 0) {
            var attrValidate = element.attr('ntsvalidate');
            var parent = element.closest('div.controls');

            if (parent != null && parent.length > 0) {
                parent.children(".notify").remove();
                if (typeof attrValidate !== typeof undefined && attrValidate !== false) {

                    var message = "";
                    attrValidate.split(',').some(function (type) {
                        try {
                            var checknull = (objectModel[key] == null || objectModel[key] == '' || objectModel[key] == undefined);
                            if (type == 'required' && checknull) {
                                message = "Không được để trống";
                                return true;
                            } else if (!checknull && type.indexOf("max") > -1 && objectModel[key].length > type.split('=')[1]) {
                                message = "Độ dài tối đa " + type.split('=')[1] + " kí tự";
                                return true;
                            } else if (!checknull && type.indexOf("min") > -1 && objectModel[key].length < type.split('=')[1]) {
                                message = "Độ dài tối thiểu " + type.split('=')[1] + " kí tự";
                                return true;
                            }
                            else if (!checknull && type == 'email' && !isEmail(objectModel[key])) {
                                message = "Email không đúng định dạng.";
                                return true;
                            }
                            else if (!checknull && type == 'date' && !moment(objectModel[key], 'YYYY-MM-DD', true).isValid()) {
                                message = "Ngày không đúng định dạng.";
                                return true;
                            } else if (!checknull && type == 'number' && Number.isNaN(objectModel[key])) {
                                message = "Không đúng định dạng.";
                                return true;
                            }
                        } catch (ex) {
                            message = ex;
                        }
                    });

                    if (message != "") {
                        var htmlMessage = '<div class="notify"><span class="badge badge-error">!</span>' + message + '</div>';
                        parent.append(htmlMessage);
                        statusValidate = true;
                    }
                }
            }
        }
    });
    return statusValidate;
}


function ValidateForm(objectModel) {
    $("body").find(".notify").remove();
    var statusValidate = false;
    Object.keys(objectModel).forEach(function (key) {
        var element = $('#' + key);
        if (element !== null && element.length > 0) {
            var attrValidate = element.attr('ntsvalidate');
            var parent = element.closest('div.controls');
            if (parent !== null && parent.length > 0) {
                var message = "";
                attrValidate.split(',').some(function (type) {
                    try {
                        var checknull = (objectModel[key] === null || objectModel[key] === '' || objectModel[key] === undefined);
                        if (type === 'required' && checknull) {
                            message = "Không được để trống";
                            return true;
                        } else if (!checknull && type.indexOf("max") > -1 && objectModel[key].length > type.split('=')[1]) {
                            message = "Độ dài tối đa " + type.split('=')[1] + " kí tự";
                            return true;
                        } else if (!checknull && type.indexOf("min") > -1 && objectModel[key].length < type.split('=')[1]) {
                            message = "Độ dài tối thiểu " + type.split('=')[1] + " kí tự";
                            return true;
                        }
                        else if (!checknull && type == 'email' && !isEmail(objectModel[key])) {
                            message = "Email không đúng định dạng.";
                            return true;
                        }
                        else if (!checknull && type == 'date' && !moment(objectModel[key], 'YYYY-MM-DD', true).isValid()) {
                            message = "Ngày không đúng định dạng.";
                            return true;
                        } else if (!checknull && type == 'number' && Number.isNaN(objectModel[key])) {
                            message = "Không đúng định dạng.";
                            return true;
                        }
                    } catch (ex) {
                        message = ex;
                    }
                });
            }
        }

        if (message !== "") {
            var htmlMessage = '<div class="notify"><span class="badge badge-error">!</span>' + message + '</div>';
            parent.append(htmlMessage);
            statusValidate = true;
        }
    });
    return statusValidate;
};

function formatDate(date) {
    if (date != null && date != undefined && date != "") {
        var month = '' + (date.getMonth() + 1),
            day = '' + date.getDate(),
            year = date.getFullYear();

        if (month.length < 2) month = '0' + month;
        if (day.length < 2) day = '0' + day;

        return [year, month, day].join('-');
    }
    return '';
}

//Check email
function isEmail(email) {
    var regex = /^([a-zA-Z0-9_.+-])+\@(([a-zA-Z0-9-])+\.)+([a-zA-Z0-9]{2,4})+$/;
    return regex.test(email);
}