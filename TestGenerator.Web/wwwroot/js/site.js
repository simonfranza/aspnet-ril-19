// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
$(function () {
    $(document).ready(() => {
        $('.answers').hide();
        $('#yesno').show();
        jQuery.get('/Questions/AddBinaryAnswer').done(function (html) {
            $('#binary').append(html);
        });
    });

    $('#questionTypeSelect').on('change', function () {
        $('.answers').hide();
        switch (this.value) {
            case '0': $('#yesno').show();
                jQuery.get('/Questions/AddBinaryAnswer').done(function (html) {
                    $('#binary').append(html);
                });
                $('#mcq').empty();
                $('#scq').empty();
                break;
            case '1': $('#singlechoice').show();
                jQuery.get('/Questions/AddSingleAnswer').done(function (html) {
                    $('#scq').append(html);
                });
                $('#mcq').empty();
                $('#binary').empty();
                break;
            case '2': $('#multichoice').show();
                jQuery.get('/Questions/AddMultiAnswer').done(function (html) {
                    $('#mcq').append(html);
                });
                $('#scq').empty();
                $('#binary').empty();
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

    $(document).on('click', '.rdb', function () {
        $('.rdb').prop('checked', false);
        $(this).prop('checked', true);
    });
});