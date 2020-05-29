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


    // Ajax call used to set the QuestionAmount field's max value in exam creation page.
    $("#examCreationModuleSelect").change(function () {
        var selectedModuleId = $(this).val();

        if (selectedModuleId !== "") {
            jQuery.get("/Exams/GetModuleQuestionsAmount?moduleId=" + selectedModuleId).done((res) => {
                // Sets the input's max data and value
                if (res.data > 0) {
                    $("#QuestionAmount").prop('readonly', false).prop("max", res.data).val(1);
                    $("#module-question-amount-insufficient-error").prop("hidden", true);
                } else {
                    $("#QuestionAmount").prop('readonly', true).val(0);
                    $("#module-question-amount-insufficient-error").prop("hidden", false);
                }
            });
        }
    });
});