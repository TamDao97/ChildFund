var urlRoot = "http://" + window.location.hostname;
var IsRemoteImg = false;
function uploadImage() {
    $('#FileUpload').trigger('click');
    $('#FileUpload').change(function () {
        var data = new FormData();
        let reader = new FileReader();
        var files = $("#FileUpload").get(0).files;
        if (files.length > 0) {
            data.append("Avatar", files[0]);
            reader.readAsDataURL(files[0]);
            reader.onload = () => {
                $('#imgPreview').attr("src", reader.result);
            };
        }
    });
}
function clearImage() {
    IsRemoteImg = true;
    $('#imgPreview').attr("src", "/img/avatar.png");
}
var getUrlParameter = function getUrlParameter(sParam) {
    var sPageURL = window.location.search.substring(1),
        sURLVariables = sPageURL.split('&'),
        sParameterName,
        i;

    for (i = 0; i < sURLVariables.length; i++) {
        sParameterName = sURLVariables[i].split('=');

        if (sParameterName[0] === sParam) {
            return sParameterName[1] === undefined ? true : decodeURIComponent(sParameterName[1]);
        }
    }
};

