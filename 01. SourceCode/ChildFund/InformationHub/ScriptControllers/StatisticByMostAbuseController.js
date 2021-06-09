$('.datepicker').datepicker({
    format: 'dd/mm/yyyy'
});

var modelSearch =
{
    DateFrom: '',
    DateTo: ''
};

function Refresh() {
    $('#dateFromSearch').val('');
    $('#dateToSearch').val('');
    Search();
}
function GetModelSearch() {
    modelSearch.DateFrom = $('#dateFromSearch').val();
    modelSearch.DateTo = $('#dateToSearch').val();
}
function Search() {
    var keyMess = sessionStorage.getItem("keyMess");
    if (keyMess !== null && keyMess !== '') {
        toastr.success(keyMess, { timeOut: 5000 });
        sessionStorage.removeItem("keyMess");
    }
    GetModelSearch();
    OpenWaiting();
    $.post("/StatisticByMostAbuse/StatisticWitMostAbuse", modelSearch, function (result) {
        GenProvinceTable(result.lstProvince, result.lstAbuse);
        GenDistrictTable(result.lstDistrict, result.lstAbuse);
        GenWardTable(result.lstWard, result.lstAbuse);
        CloseWaiting();
    });
}

function GenProvinceTable(lstProvince, lstAbuse) {
    var table = '';
    for (var j = 0; j < lstProvince.length; j++) {
        table += ' <tr>';
        table += ' <td>' + lstProvince[j].LableName + '</td>';
        table += ' <td class="text-center">' + lstProvince[j].Count1 + '</td>';
        table += ' <td class="text-center">' + lstProvince[j].Count2 + '</td>';
        table += ' <td class="text-center">' + lstProvince[j].Count3 + '</td>';
        table += ' <td class="text-center">' + lstProvince[j].Count4 + '</td>';
        table += ' <td class="text-center">' + lstProvince[j].Count5 + '</td>';
        table += ' <td class="text-center">' + lstProvince[j].Total + '</td>';
        table += ' </tr>';
    }
    $('#province_table').html(table);
}
function GenDistrictTable(lstDistrict, lstAbuse) {
    var table = '';
    for (var j = 0; j < lstDistrict.length; j++) {
        table += ' <tr>';
        table += ' <td>' + lstDistrict[j].LableName + '</td>';
        table += ' <td class="text-center">' + lstDistrict[j].Count1 + '</td>';
        table += ' <td class="text-center">' + lstDistrict[j].Count2 + '</td>';
        table += ' <td class="text-center">' + lstDistrict[j].Count3 + '</td>';
        table += ' <td class="text-center">' + lstDistrict[j].Count4 + '</td>';
        table += ' <td class="text-center">' + lstDistrict[j].Count5 + '</td>';
        table += ' <td class="text-center">' + lstDistrict[j].Total + '</td>';
        table += ' </tr>';
    }
    $('#district_table').html(table);
}
function GenWardTable(lstWard, lstAbuse) {
    var table = '';
    for (var j = 0; j < lstWard.length; j++) {
        table += ' <tr>';
        table += ' <td>' + lstWard[j].LableName + '</td>';
        table += ' <td class="text-center">' + lstWard[j].Count1 + '</td>';
        table += ' <td class="text-center">' + lstWard[j].Count2 + '</td>';
        table += ' <td class="text-center">' + lstWard[j].Count3 + '</td>';
        table += ' <td class="text-center">' + lstWard[j].Count4 + '</td>';
        table += ' <td class="text-center">' + lstWard[j].Count5 + '</td>';
        table += ' <td class="text-center">' + lstWard[j].Total + '</td>';
        table += ' </tr>';
    }
    $('#ward_table').html(table);
}