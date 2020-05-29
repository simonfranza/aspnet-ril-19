// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
$(function () {
    $(document).ready(() => {
        $('.answers').hide();
        $('#yesno').show();
    });

    $('#questionTypeSelect').on('change', function () {
        $('.answers').hide();
        switch (this.value) {
            case '0': $('#yesno').show();
                break;
            case '1': $('#singlechoice').show();
                break;
            case '2': $('#multichoice').show();
                break;
        }
    });

    $('#add-single-choice').on('click', function () {
        jQuery.get('/Questions/AddSingleAnswer').done(function (html) {
            $('#scq').append(html);
        });
    });

    $('#add-multi-choice').on('click', function () {
        jQuery.get('/Questions/AddMultiAnswer').done(function (html) {
            $('#mcq').append(html);
        });
    });
});