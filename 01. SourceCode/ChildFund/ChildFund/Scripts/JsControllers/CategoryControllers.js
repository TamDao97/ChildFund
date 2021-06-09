var urlRoot = "http://localhost:8811";
var model =
{
    Name: '',
    Status: '',
    PageSize: 10,
    PageNumber: 0
};
function Lammoi() {
    $('#categoryStatus').val('');
    $('#categoryName').val('');
    model.PageSize = 10;
    model.PageNumber = 0;
    Timkiem();
}
function GetModel() {
    model.PageSize = $('#pageSize').val();
    model.Name = $('#categoryName').val();
    model.Status = $('#categoryStatus').val();
}
function Timkiem() {
    model.Name = $('#categoryName').val();
    model.Status = $('#categoryStatus').val();
    $.post(urlRoot + "/Category/ListCategory", model, function (result) {
        $("#div_category").html(result);
    });
}

function phantrang(PageNumber) {
    GetModel();
    model.PageNumber = PageNumber;
    $.post(urlRoot + "/Category/ListCategory", model, function (result) {
        $("#div_category").html(result);
    });
}

