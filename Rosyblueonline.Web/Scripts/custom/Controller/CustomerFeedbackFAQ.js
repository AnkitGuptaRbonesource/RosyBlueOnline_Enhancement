var CustomerFeedbackFAQ = function () {
    var OnLoad = function () {
        dtOrder = new Datatable();
        objLRS = new LoginRegistrationService();

        BindQuestions(0, "NEXT");
        GetTotalFAQCount();
      
    }
    var count = 1;
    var RegisterEvent = function () {
       

        $('#btnFAQNext').click(function (e) {
            e.preventDefault();
            count=count + 1;
            var FAQId = $('#hfCurrentFAQID').val();
            var FAQTypeID = $('#hfFAQTypeID').val();
            var TextAnswer = "";
            var OptionId = "";
            if (FAQTypeID == "1") {

                TextAnswer = $('#txtqno' + FAQId).val();
                OptionId = FAQId;

            } else if (FAQTypeID == "2") {

                $('input:checkbox[name=qno' + FAQId + ']:checked').each(function () {

                    OptionId = OptionId == "" ? $(this).val() : OptionId + "," + $(this).val();
                    var idVal = $(this).attr("id");
                    TextAnswer = TextAnswer == "" ? $("label[for='" + idVal + "']").text() : TextAnswer + "," + $("label[for='" + idVal + "']").text();
                });

            } else if (FAQTypeID == "3") {

                OptionId = $('input[name=qno' + FAQId + ']:checked').val();
                var idVal = $('input[name=qno' + FAQId + ']:checked').attr("id");
                TextAnswer = $("label[for='" + idVal + "']").text();

            }
            if (OptionId == "" || TextAnswer == "") { 
                uiApp.Alert({ container: '#uiPanel', message: "Please fill the answers ", type: "danger" }); 

                return false;

            }
                objLRS.SubmitFAQAnswers(FAQId, FAQTypeID, OptionId, TextAnswer).then(function (data) {
                    if (data.IsSuccess) {
                        BindQuestions(FAQId, "NEXT");

                    } else {
                        uiApp.Alert({ container: '#uiPanel2', message: "Problem in submitting question", type: "error" })
                    }

                }, function (error) {
                });
          

        });




        $('#btnFAQPrev').click(function (e) {
            e.preventDefault();
            count = count - 1;
            var FAQId = $('#hfCurrentFAQID').val();
            BindQuestions(FAQId, "PRE");

        });


    };

    function BindQuestions(QTypeId, Flag) {
        objLRS.GetFeedbackQuestion(QTypeId, Flag).then(function (data) {
            if (data.IsSuccess && data.Result) {
                var FAQOptions = '';
                var FAQTemplate = '';
                $("#FeedbackQuestions").html("");
                $("#hfCurrentFAQID").val();
                $("#FAQCount").html("");
                if (data.Result[0].QuestionTypeId == "1") {
                    FAQOptions = '<input type="text" name="qno' + data.Result[0].ParentFaqID + '" value="" class="form-control" id="txtqno' + data.Result[0].ParentFaqID + '" />';

                } else if (data.Result[0].QuestionTypeId == "2") {

                    for (var i = 1; i <= data.Result.length - 1; i++) {

                        FAQOptions = FAQOptions + '<div> <label class="inline-checkbox">' +
                            '<input type="checkbox" id="qno' + data.Result[i].faqID + '" name="qno' + data.Result[i].ParentFaqID + '" value="' + data.Result[i].faqID + '" class="space_chk">' +
                            '<label for="qno' + data.Result[i].faqID + '">' + data.Result[i].Question + '</label>  <span class="checkmark"></span>  </label></div>';

                    }


                } else if (data.Result[0].QuestionTypeId == "3") {


                    for (var i = 1; i <= data.Result.length - 1; i++) {



                        FAQOptions = FAQOptions + '<div> <label class="radio-inline">' +
                            '<input type="radio" id="qno' + data.Result[i].faqID + '" name="qno' + data.Result[i].ParentFaqID + '" value="' + data.Result[i].faqID + '" checked="">' +
                            '<label for="qno' + data.Result[i].faqID + '">' + data.Result[i].Question + '</label>  <span class="checkmark"></span>  </label></div>';

                    }


                }




                FAQTemplate = '<div id="Faqno"' + data.Result[0].ParentFaqID + '" class="form-group">' +
                    '<label class="control-label mb-15">' +
                    '<label>' + data.Result[0].Question + '</label>' +
                    '</label>' + FAQOptions + '</div>';



                $('#FeedbackQuestions').append(FAQTemplate);
                $('#hfCurrentFAQID').val(data.Result[0].ParentFaqID);
                $('#hfFAQTypeID').val(data.Result[0].QuestionTypeId);

                if ($('#hfCurrentFAQID').val() != "1") {
                    $('#btnFAQPrev').show();
                } else {
                    $('#btnFAQPrev').hide();
                }


                BindPreviousQuestions();

                var Faqcount = count +" of  "+ $('#hfFAQTotalCount').val();
                $('#FAQCount').append(Faqcount);

                if ($('#hfFAQTotalCount').val() == count) {
                    
                    $("#btnFAQNext").html('Finish');

                } else {
                    $("#btnFAQNext").html('Next');
                }

               // uiApp.Alert({ container: '#uiPanel', message: "success", type: "success" });
            } else {
                if ($("#btnFAQNext").html() =="Finish") {

                    $("#FeedbackQuestions").html("");
                    $("#btnFAQNext").hide();
                    $('#btnFAQPrev').hide();
                    $('#FAQCount').hide();
                    var Final = "<h2 class='text-center text-success mt-20 mb-20'>Your Feedback submitted successfully !</h2>";
                    $("#FeedbackQuestions").html(Final);


                } else {
                   uiApp.Alert({ container: '#uiPanel', message: data.Message, type: "danger" });
                }
            }
        }, function (error) {
            uiApp.Alert({ container: '#uiPanel', message: "Record not updated", type: "danger" });
        });
    }



    function BindPreviousQuestions() {

        var QTypeId = $("#hfCurrentFAQID").val();
        objLRS.GetBindPreviousQuestions(QTypeId).then(function (data) {
            if (data.IsSuccess && data.Result) {

                if (data.Result.QuestionTypeId == "1") {
                    $('#txtqno' + data.Result.FAQId).val(data.Result.FAQTextAnswer);
                } else
                    if (data.Result.QuestionTypeId == "2") {
                        var array = data.Result.FAQOptionsAnswer.split(",");
                        $.each(array, function (i) {
                            $("#qno" + array[i]).prop('checked', true);
                        });

                    }
                    else
                        if (data.Result.QuestionTypeId == "3") {
                            $("#qno" + data.Result.FAQOptionsAnswer).prop("checked", true);

                        }


              //  uiApp.Alert({ container: '#uiPanel', message: "success", type: "success" });

            } else {
              //  uiApp.Alert({ container: '#uiPanel', message: data.Message, type: "danger" });
            }
        }, function (error) {
            uiApp.Alert({ container: '#uiPanel', message: "Record not updated", type: "danger" });
        });
    }



    function GetTotalFAQCount() { 
        objLRS.GetTotalFAQCount(0).then(function (data) {
            if (data.IsSuccess && data.Result) {

                $('#hfFAQTotalCount').val(data.Result);
               // uiApp.Alert({ container: '#uiPanel', message: "success", type: "success" });

            } else {
                uiApp.Alert({ container: '#uiPanel', message: data.Message, type: "danger" });
            }
        }, function (error) {
            uiApp.Alert({ container: '#uiPanel', message: "Record not updated", type: "danger" });
        });
    }


    return {
        init: function () {
            OnLoad();
            RegisterEvent();
        }
    }

}();
$(document).ready(function () {
    CustomerFeedbackFAQ.init();
});