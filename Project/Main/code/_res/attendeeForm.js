$(function () {
    var $form = $('.attendee-form');

    $form.on('submit', function() {
        var $btn = $('.attendee-form .btn-success');
        $btn.addClass('btn-loading');
        $btn.val('');
        $btn.attr('disabled', '');
    });
})