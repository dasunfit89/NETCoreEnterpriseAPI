var loading = $('.loader');
var forceShowLoading = false;
var ajaxRequestInProcess = false;

$(document)
    .ajaxStart(function () {
        ajaxRequestInProcess = true;
        loading.show();
    })
    .ajaxStop(function () {
        ajaxRequestInProcess = false;
        if (!forceShowLoading) {
            loading.hide();
        }
    })
    .ready(function () {
        if (!ajaxRequestInProcess)
            loading.hide();
    });

$(window).unload(function () {
    loading.show();
});