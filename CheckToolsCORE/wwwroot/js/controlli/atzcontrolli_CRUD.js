var showing;
var Edit = function (id) {

    var url = "ATZ_CONTROLLI/Edit?id=" + id;
   
    $('#title').html("Modifica dati controllo");
    
    $("#ControlliFormModelDiv").load(url, function (response, status, xhr) {

        if (status == "error") {
            Swal.fire({
                title: "Pagina non accessibile",
                html: xhr.statusText,
                type: "error"

            });
        }
        else {
            $("#ControlliFormModel").modal("show");
        }

        
    });
};

var Crea = function (id) {

   
    if (showing == id)
        return;

    showing = id;

    $("body").css("cursor", "progress");

    var url = "/ATZ_CONTROLLI/Create?id=" + id;

    $('#title').html("Nuovo controllo");
        
    $("#ControlliFormModelDiv").load(url, function (response, status, xhr) {
        debugger;
        if (status == "error") {
            var errMsg = response;
            try {
                var resObj = JSON.parse(response);
                errMsg = resObj.responseText;
            } catch (e) {

            }

            Swal.fire({
                title: "Pagina non accessibile",
                html: errMsg,
                type: "error"

            });
            $("body").css("cursor", "default");
        }
        else {

            $("#ControlliFormModel").modal("show");
            $("body").css("cursor", "default");
            showing = "";
        }


    });
};

var ShowEsito = function (id) {

    var url = "ATZ_CONTROLLI/DownloadEsito?id=" + id;

    popupwindow(url, "Allegati", 800, 600);

    //window.open(url, 'insertPopupNameHere', 'width=400,height=400')

    //$('#title').html("ShowCheck List");


    //$("#ControlliFormModelDiv").load(url, function (response, status, xhr) {

    //    if (status == "error") {
    //        Swal.fire({
    //            title: "Pagina non accessibile",
    //            html: xhr.statusText,
    //            type: "error"

    //        });
    //    }
    //    else {
    //        $("#ControlliFormModel").modal("show");
    //    }


    //});


    //$.ajax({
    //    url: url,
    //    type: "GET",
    //    dataType: "html",
    //    success: function (data) {
    //        console.log(data);
    //        $('#ControlliFormModel').html(data);
    //        $('#ControlliFormModel').modal('show');
    //    }
    //});


};

var ShowCheck = function (id) {

    var url = "ATZ_CHECK/DownloadCheckList?codArt=" + id;

    popupwindow(url, "Allegati", 800, 600);

    //window.open(url, 'insertPopupNameHere', 'width=400,height=400')

    //$('#title').html("ShowCheck List");


    //$("#ControlliFormModelDiv").load(url, function (response, status, xhr) {

    //    if (status == "error") {
    //        Swal.fire({
    //            title: "Pagina non accessibile",
    //            html: xhr.statusText,
    //            type: "error"

    //        });
    //    }
    //    else {
    //        $("#ControlliFormModel").modal("show");
    //    }


    //});
    
    
    //$.ajax({
    //    url: url,
    //    type: "GET",
    //    dataType: "html",
    //    success: function (data) {
    //        console.log(data);
    //        $('#ControlliFormModel').html(data);
    //        $('#ControlliFormModel').modal('show');
    //    }
    //});
    

};
function popupwindow(url, title, w, h) {
    var left = (screen.width / 2) - (w / 2);
    var top = (screen.height / 2) - (h / 2);
    return window.open(url, title, 'toolbar=no, location=no, directories=no, status=no, menubar=no, scrollbars=no, resizable=no, copyhistory=no, width=' + w + ', height=' + h + ', top=' + top + ', left=' + left);
}


var Upload = function (id) {
    var url = "/ATZ_CHECK/Create?id=" + id;

    $('#title').html("Upload Check List");


    $("#ControlliFormModelDiv").load(url, function (response, status, xhr) {
       
        if (status == "error") {
            Swal.fire({
                title: "Pagina non accessibile",
                html: xhr.statusText,
                type: "error"

            });
        }
        else {
            $("#ControlliFormModel").modal("show");
        }


    });
};

var Lock = function (articolo) {
    $.ajax({
        type: "POST",
        url: "/MAGANA/Lock",
        data: JSON.stringify({ articolo: articolo }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (result) {
            $("#MaganaFormModel").modal("hide");

            Swal.fire({
                title: "Attenzione!",
                text: "Riga Protetta!",
                type: "Success"
            }).then(function () {
                $('#tblMagana').DataTable().ajax.reload();
            });
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
};

var UnLock = function (articolo) {
    $.ajax({
        type: "POST",
        url: "/MAGANA/UnLock",
        data: JSON.stringify({ articolo: articolo }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (result) {
            $("#MaganaFormModel").modal("hide");

            Swal.fire({
                title: "Attenzione!",
                text: "Riga Rilasciata!",
                type: "Success"
            }).then(function () {
                $('#tblMagana').DataTable().ajax.reload();
            });
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
};




var NewArticolo = function () {
    var url = "MAGANA/Create";
    $('#title').html("Nuovo Articolo");
    $("#MaganaFormModelDiv").load(url, function () {
        $("#MaganaFormModel").modal("show");

    });
};

$('body').on('click', "#btnCrea", function () {
    var myformdata = $("#MaganaForm").serialize();

    if ($.trim($('#FirstName').val()) === '') {
        Swal.fire({
            title: "Alert", text: "First Name can not be left blank.",
            icon: "info", closeOnConfirm: false,
            onAfterClose: () => {
                setTimeout(() => $("#FirstName").focus(), 110);
            }
        });
        return;
    }
    if ($.trim($('#LastName').val()) === '') {
        Swal.fire({
            title: "Alert", text: "Last Name can not be left blank.",
            icon: "info", closeOnConfirm: false,
            onAfterClose: () => {
                setTimeout(() => $("#LastName").focus(), 110);
            }
        });
        return;
    }


    if ($.trim($('#DateOfBirth').val()) === '') {
        Swal.fire({
            title: "Alert", text: "Date Of Birth can not be left blank.",
            icon: "info", closeOnConfirm: false,
            onAfterClose: () => {
                setTimeout(() => $("#DateOfBirth").focus(), 110);
            }
        });
        return;
    }

    $.ajax({
        type: "POST",
        url: "/Aziende/Create",
        data: myformdata,
        success: function (result) {
            $("#MaganaFormModel").modal("hide");

            Swal.fire({
                title: "Alert!",
                text: result,
                type: "Success"
            }).then(function () {
                $('#tblAziende').DataTable().ajax.reload();
            });
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
});




var DeleteAziende = function (id) {
    Swal.fire({
        title: 'Do you want to delete this item?',
        type: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes'
    }).then((result) => {
        if (result.value) {
            $.ajax({
                type: "POST",
                url: "/Aziende/Delete/" + id,
                success: function () {
                    Swal.fire({
                        title: 'Deleted!', text: 'Item has been deleted.',
                        icon: "success", closeOnConfirm: false,
                        onAfterClose: () => {
                            $('#tblAziende').DataTable().ajax.reload();
                        }
                    });
                }
            });
        }
    });
};

var EliminaAllegato = function (id, nomeFile) {
    debugger;
    Swal.fire({
        title: 'Confermi la cancellazione del file?' + nomeFile,
        type: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'SI'
    }).then((result) => {
        if (result.value) {
            $.ajax({
                type: "POST",
                url: "/ATZ_CONTROLLI/DeleteAll/" + id,
                success: function () {
                    Swal.fire({
                        title: 'Deleted!', text: 'Cancellazione effettuata',
                        icon: "success", closeOnConfirm: false,
                        onAfterClose: () => {
                            $('#tblControlli').DataTable().ajax.reload();
                        }
                    });
                }
            });
        }
    });
};

