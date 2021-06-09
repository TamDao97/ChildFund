var modelAction = {
    ImageId: [],
    UserUploadId: ''
}

function DownLoadImage(id) {
    var listIdChecked = [];
    $('#divListImage input:checkbox').each(function () {
        vlItem = $(this).attr('value');
        if ($(this).is(':checked')) {
            listIdChecked.push(vlItem);
        }
    });
    if (listIdChecked.length > 0) {
        modelAction.ImageId = listIdChecked;
        modelAction.UserUploadId = id;
        $('#loader_id').addClass("loader");
        document.getElementById("overlay").style.display = "block";
        $.ajax({
            url: "/ImageLibrary/DownloadImage/",
            type: "POST",
            data: modelAction,
            success: function (data) {
                if (data.ok === true) {
                    var link = document.createElement('a');
                    link.setAttribute("type", "hidden");
                    link.href = urlRoot + data.mess;
                    link.download = 'Download.zip';
                    document.body.appendChild(link);
                    link.focus();
                    link.click();
                    toastr.success("Tải xuống 1 mục ảnh thành công!", { timeOut: 5000 });
                } else {
                    toastr.error(data.mess, { timeOut: 5000 });
                }
                $('#loader_id').removeClass("loader");
                document.getElementById("overlay").style.display = "none";
            },
            error: function (response) {
                $('#loader_id').removeClass("loader");
                document.getElementById("overlay").style.display = "none";
                toastr.error("Đã xảy ra lỗi!", { timeOut: 5000 });
            },
        });

    } else {
        toastr.error("Chưa chọn ảnh!", { timeOut: 5000 });
    }
    return false;
}
var listIdChecked = [];
var id = '';
function DeleteConfim(idDel) {
    id = idDel;
    listIdChecked = [];
    $('#divListImage input:checkbox').each(function () {
        vlItem = $(this).attr('value');
        if ($(this).is(':checked')) {
            listIdChecked.push(vlItem);
        }
    });
    if (listIdChecked.length > 0) {
        $('#valueDelete').val(id);
        $('#labelDelete').html('Bạn có chắc chắn muốn xóa ảnh được chọn này?');
        $('#modamDelete').modal({
            show: 'true'
        });
    } else {
        toastr.error("Chưa chọn ảnh!", { timeOut: 5000 });

    }

}
function Delete() {
    modelAction.ImageId = listIdChecked;
    modelAction.UserUploadId = id;
    $.post("/ImageLibrary/DeleteImage", modelAction, function (result) {
        window.location.href = '/ImageLibrary/ViewListUpload?id=' + id;
    });
}

function CloseModelImage() {
    $('#myModalImage').css('display', "none");
}

function ShowModelImage(path, alt) {
    $('#myModalImage').css('display', "block");
    $('#img01').attr('src', path);
    document.getElementById('#caption').innerHTML = alt;
}

//gallery bootrap - TuNM
let modalId = $('#image-gallery');

$(document)
    .ready(function () {

        loadGallery(true, 'a.thumbnail');

        //This function disables buttons when needed
        function disableButtons(counter_max, counter_current) {
            $('#show-previous-image, #show-next-image')
                .show();
            if (counter_max === counter_current) {
                $('#show-next-image')
                    .hide();
            } else if (counter_current === 1) {
                $('#show-previous-image')
                    .hide();
            }
        }

        /**
         *
         * @param setIDs        Sets IDs when DOM is loaded. If using a PHP counter, set to false.
         * @param setClickAttr  Sets the attribute for the click handler.
         */

        function loadGallery(setIDs, setClickAttr) {
            let current_image,
                selector,
                counter = 0;

            $('#show-next-image, #show-previous-image')
                .click(function () {
                    if ($(this)
                        .attr('id') === 'show-previous-image') {
                        current_image--;
                    } else {
                        current_image++;
                    }

                    selector = $('[data-image-id="' + current_image + '"]');
                    updateGallery(selector);
                });

            function updateGallery(selector) {
                let $sel = selector;
                current_image = $sel.data('image-id');
                $('#image-gallery-title')
                    .text($sel.data('title'));
                $('#image-gallery-image')
                    .attr('src', $sel.data('image'));
                disableButtons(counter, $sel.data('image-id'));
            }

            if (setIDs == true) {
                $('[data-image-id]')
                    .each(function () {
                        counter++;
                        $(this)
                            .attr('data-image-id', counter);
                    });
            }
            $(setClickAttr)
                .on('click', function () {
                    updateGallery($(this));
                });
        }
    });

// build key actions
$(document)
    .keydown(function (e) {
        switch (e.which) {
            case 37: // left
                if ((modalId.data('bs.modal') || {})._isShown && $('#show-previous-image').is(":visible")) {
                    $('#show-previous-image')
                        .click();
                }
                break;

            case 39: // right
                if ((modalId.data('bs.modal') || {})._isShown && $('#show-next-image').is(":visible")) {
                    $('#show-next-image')
                        .click();
                }
                break;

            default:
                return; // exit this handler for other keys
        }
        e.preventDefault(); // prevent the default action (scroll / move caret)
    });
