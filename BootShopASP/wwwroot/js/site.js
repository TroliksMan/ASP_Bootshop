// Write your JavaScript code.
$(function () {
    var PlaceHolderElement = $('#PlaceHolderHere');
    $('button[data-toggle="ajax-modal"]').click(function (event) {
        var url = $(this).data('url');
        var id = this.value
        $.get(url).done(function (data) {
            PlaceHolderElement.html(data);
            PlaceHolderElement.find('.modal').modal('show');
        });
        PlaceHolderElement.on('click', '[data-save="modal"]', function (event) {
            console.log(id)
            $.post('/Admin/RemoveProductPost/' + id, function (event) {
                location.reload();
            })

            PlaceHolderElement.find('.modal').modal('hide');
        })

        PlaceHolderElement.on('click', '[data-dismiss="modal"]', function (event) {
            PlaceHolderElement.find('.modal').modal('hide');
        })
    })
})

$("#addAnother").click(function () {
    $.get('/Admin/AddVariantRow', function (template) {
        $("#variants").append(template);
    });
});
