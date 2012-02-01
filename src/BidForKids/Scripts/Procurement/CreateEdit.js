var createEdit = (function () {

    getDonorTemplate = function (selectedValue) {
        var templateText = $("#donors li")[0];
        var template = $(templateText).clone();
        if (selectedValue != null)
            $("select", template).val(selectedValue);
        else
            $("select", template).val("");

        var newDonorId = "DonorId_" + ($('select[id^="DonorId_"]').length + 1);
        $("select", template).attr("name", newDonorId);
        $("select", template).attr("id", newDonorId);
        return template;
    };

    setupDonorIds = function (select) {
        // get the changed item
        var newId = $(select).val();

        // change the li label
        $(select).parent().attr("id", "donor_" + newId);

        // change the view link
        $(select).siblings("#ViewDonor").attr('href', createEdit.donorEditAction + '/' + newId);

        // change the remove link
        $(select).siblings("#removeDonor").attr('tag', newId);
    };

    return {
        donorEditAction: "",

        addDonor: function () {
            var template = getDonorTemplate(null);
            template.appendTo("#donorList");
        },

        addSelectedDonor: function (selectedDonor) {
            var currentDonors = $('select[id^="DonorId_"]');
            var foundBlank = false;
            $.each(currentDonors, function (index, value) {
                if ($(value).val() == "") {
                    $(value).val(selectedDonor);
                    foundBlank = true;
                    return false;
                }
            });
            if (foundBlank == true) return;

            var template = getDonorTemplate(selectedDonor);
            template.appendTo("#donorList");
        },

        removeDonor: function () {
            var id = $(this).attr("tag");
            var liId = "#donor_" + id;
            var numberOfDonors = $("#donors li[id^='donor_']").length;

            if (numberOfDonors == 1)
                $(liId + " #DonorId").val("");
            else if (numberOfDonors > 1)
                $(liId).remove();
        },

        changeDonor: function () {
            setupDonorIds(this);
        },

        loadForm: function () {
            var formValues = $.cookie("createProcurement");

            if (formValues == null || formValues == undefined)
                return;

            var donors = formValues.split("&");
            var donorCount = 0;
            $.each(donors, function (index, value) {
                if (value.substring(0, 7) == "DonorId")
                    donorCount += 1;
            });

            for (var i = 0; i < donorCount - 1; i++)
                createEdit.addDonor();

            $("form").deserialize(formValues);
        },

        saveForm: function () {
            var formValues = $("form").serialize();
            $.cookie("createProcurement", formValues, { expires: 1 });
        },

        clearSavedForm: function () {
            $.cookie("createProcurement", null);
        }
    }
})();

$(document).ready(function () {
    $("#addDonor").click(createEdit.addDonor);
    $('#removeDonor').live('click', createEdit.removeDonor);
    $('select[id^="DonorId_"]').live('change', createEdit.changeDonor);
    $('#createNewDonor').click(createEdit.saveForm);
    $('#createProcurement').click(createEdit.clearSavedForm);
    $('#resetCreateProcurementForm').click(createEdit.clearSavedForm);
});
