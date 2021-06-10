
$(document).ready(function () {
    
    $("#btnModifica").click(function (e) {
        e.preventDefault();
       
        if ($("#ControlliForm").valid()) {
            var myformdata = $("#ControlliForm").serialize();
            $.ajax({
                type: "POST",
                url: "/ATZ_CONTROLLI/Edit",
                data: myformdata,
                success: function (result) {

                    if (result == "OK") {
                        Swal.fire({
                            title: "INFO",
                            text: "Modifica registrata correttamente.",
                            type: "success"
                        }).then(function () {
                            $('#tblControlli').DataTable().ajax.reload();
                        });

                        $("#ControlliFormModel").modal("hide");
                    }
                    else {
                        Swal.fire({
                            title: "ERRORE",
                            text: "Errore in fase di registrazione.",
                            type: "error"
                        })

                    }
                },
                error: function (errormessage) {

                    alert(errormessage.responseText);
                }
            });
        }
               
    });

});



$("#DateOfBirth").datepicker({
    dateFormat: 'yy-mm-dd',
    maxDate: '0',
    changeMonth: true,
    changeYear: true
});

$.validate({
    lang: 'en',
    modules: 'location, date, security, file',
    onModulesLoaded: function () {
        $('#country').suggestCountry();
    }
});


