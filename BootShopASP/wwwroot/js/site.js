// Write your JavaScript code.
$(function () {
    var PlaceHolderElement = $('#PlaceHolderHere');
    $('button[data-toggle="ajax-modal"]').click(function (event) {
        var url = $(this).data('url');
        $.get(url).done(function (data) {
            PlaceHolderElement.html(data);
            PlaceHolderElement.find('.modal').modal('show');
        });
        PlaceHolderElement.on('click', '[data-save="modal"]', function (event) {
            console.log('Remove');
            PlaceHolderElement.find('.modal').modal('hide');
        })

        PlaceHolderElement.on('click', '[data-dismiss="modal"]', function (event) {
            console.log('Close');
            PlaceHolderElement.find('.modal').modal('hide');
        })
    })
})

$("#addAnother").click(function () {
    $.get('/Admin/MovieEntryRow', function (template) {
        $("#variants").append(template);
    });
});
