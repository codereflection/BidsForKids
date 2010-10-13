$(document).ready(function () {
    $("#ResultContainer").hide();

    var theForm = $("#CreateReportForm");

    theForm.submit(function () {
        var action = theForm.attr("action");
        var serializedForm = theForm.serialize();

        $("#ReportStatus").text("Loading...").show();

        $.ajax({
            url: action,
            data: serializedForm,
            type: "POST",
            dataType: "html",
            success: function (data, textStatus, XMLHttpRequest) {
                $("#Result").html(data);
                $("#ResultContainer").show();
                $("#ReportStatus").text("Complete").fadeOut("slow");
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                alert("Error: " + textStatus);
            }
        });

        return false;
    });
});
