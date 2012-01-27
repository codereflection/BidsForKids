var createEdit = (function () {
    return {
        donorEditAction: "",

        addDonor: function () {
            var templateText = $("#donors li")[0];
            var template = $(templateText).clone();
            $("select", template).val("");
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
            // get the changed item
            var newId = $(this).val();

            // change the li label
            $(this).parent().attr("id", "donor_" + newId);

            // change the view link
            $(this).siblings("#ViewDonor").attr('href', createEdit.donorEditAction + '/' + newId);

            // change the remove link
            $(this).siblings("#removeDonor").attr('tag', newId);
        }
    }
})();

$(document).ready(function () {
    $("#addDonor").click(createEdit.addDonor);
    $('#removeDonor').live('click', createEdit.removeDonor);
    $('#DonorId').live('change', createEdit.changeDonor);
});
