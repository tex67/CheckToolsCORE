var chiCerca;
$(document).ready(function () {
    
        document.title = 'CHECK TOOLS';

        var table = $("#tblControlli").DataTable({
            paging: true,
            select: true,
            responsive: true,
            autoWidth: false,
            "order": [[0, "desc"]],
            dom: 'Bfrtip',
            "language": {
                "url": "/lib/DataTables/Italian.json",
                "processing": '<div id="loader"></div>'
            },
            buttons: [
                'pageLength',
                {
                    extend: 'collection',
                    text: 'Export',
                    buttons: [
                        {
                            extend: 'pdfHtml5',
                            customize: function (doc) {
                                //doc.content[1].margin = [100, 0, 100, 0];
                                //Remove the title created by datatTables
                                doc.content.splice(0, 1);
                                //Create a date string that we use in the footer. Format is dd-mm-yyyy
                                var now = new Date();
                                var jsDate = now.getDate() + '-' + (now.getMonth() + 1) + '-' + now.getFullYear();

                                doc.pageMargins = [20, 60, 20, 30];
                                // Set the font size fot the entire document
                                doc.defaultStyle.fontSize = 7;
                                // Set the fontsize for the table header
                                doc.styles.tableHeader.fontSize = 10;


                                doc['header'] = (function () {
                                    return {
                                        columns: [
                                            {
                                                alignment: 'center',  //center
                                                italics: true,
                                                text: 'MagAna',
                                                fontSize: 18,
                                                margin: [0, 0]
                                            }
                                        ],
                                        margin: 20
                                    }
                                });

                                // Create a footer object with 2 columns
                                doc['footer'] = (function (page, pages) {
                                    return {
                                        columns: [
                                            {
                                                alignment: 'left',
                                                text: ['Created on: ', { text: jsDate.toString() }]
                                            },
                                            {
                                                alignment: 'right',
                                                text: ['page ', { text: page.toString() }, ' of ', { text: pages.toString() }]
                                            }
                                        ],
                                        margin: 5
                                    }
                                });
                                // Change dataTable layout (Table styling)
                                // To use predefined layouts uncomment the line below and comment the custom lines below
                                // doc.content[0].layout = 'lightHorizontalLines'; // noBorders , headerLineOnly
                                var objLayout = {};
                                objLayout['hLineWidth'] = function (i) { return .5; };
                                objLayout['vLineWidth'] = function (i) { return .5; };
                                objLayout['hLineColor'] = function (i) { return '#aaa'; };
                                objLayout['vLineColor'] = function (i) { return '#aaa'; };
                                objLayout['paddingLeft'] = function (i) { return 0; };
                                objLayout['paddingRight'] = function (i) { return 0; };
                                doc.content[0].layout = objLayout;
                            },


                            orientation: 'landscape',// 'portrait', // 
                            pageSize: 'A4',
                            pageMargins: [0, 0, 0, 0], // try #1 setting margins
                            margin: [0, 0, 0, 0], // try #2 setting margins
                            text: '<u>PDF</u>',
                            key: { // press E for export PDF
                                key: 'e',
                                altKey: false
                            },
                            exportOptions: {
                                columns: [0, 1, 2, 3, 4, 5], //column id visible in PDF
                                modifier: {
                                    // DataTables core
                                    order: 'index',  // 'current', 'applied', 'index',  'original'
                                    page: 'all',      // 'all',     'current'
                                    search: 'none'     // 'none',    'applied', 'removed'
                                }
                            }
                        },
                        'copyHtml5',
                        'excelHtml5',
                        'csvHtml5',
                        {
                            extend: 'print',
                            exportOptions: {
                                columns: [0, 1, 2, 3, 4, 5],
                                page: 'all'
                            }
                        }
                    ]
                }
                


            ],

            "processing": true,
            "serverSide": true,
            "filter": true, //Search Box
            "orderMulti": false,
            "stateSave": true,
            "deferRender": true,
            "ajax": {
                "url": "ATZ_CONTROLLI/GetDataTableData",
                "type": "POST",
                "datatype": "json",
                "data": function (data) {
                    debugger;
                    if (dataScadenza.selectedDates.length > 0) {

                        var da = dataScadenza.selectedDates[0].toLocaleDateString();

                        if (da) {
                            //var newDa = moment(da, "DD/MM/YYYY").format('YYYY-MM-DD[T]HH:mm:ss');
                            data.GiorniScadenza = da;

                        }
                    }
                    
                    var filtriGRP = [];
                     
                    $('#filtroGruppiMerce input:checked').each(function () {
                        filtriGRP.push($(this).attr('value'));
                    });
                    data.CodGr2 = filtriGRP;
                },
                "dataSrc": function (json) {
                   
                    $("#info").val(json.extra);
                    return json.data;
                }
            },
            //caricamento DA FILE
            //"ajax": "/ATZ_CONTROLLI/GetDataTableDataF",
            //dataSrc: '',
           
            "columns": [
                {
                    "className": 'details-control',
                    "orderable": false,
                    "data": null,
                    "defaultContent": ''
                },
                { "data": "CodArt", "name": "Codice" },
                { "data": "Pn", "name": "Pn" },
                { "data": "Descrizione", "name": "Descrizione" },
              
                {
                    "title": "Data. Contr.", "data": "DataControllo", "searchable": true, sortable: true, render: function (d) {
                        if (d == null)
                            return "";
                        var str = d;
                        var date = moment(str);
                        var dateComponent = date.utc().format('DD/MM/YYYY');

                        return dateComponent;
                    }
                },
                //{ "data": "Scadenza", "name": "Scadenza" },
                {
                    "title": "Scadenza", "data": "Scadenza", "searchable": true, sortable: true, render: function (d) {
                        var str = d;
                        var date = moment(str);
                        var dateComponent = date.utc().local().format('DD/MM/YYYY');

                        return dateComponent;
                       
                    }
                },
                { "data": "Periodicita", "name": "Periodicità" },
                {
                    data: null, render: function (data, type, row) {
                        return "<a href='#' class='btn btn-primary btn-sm' onclick=Crea('" + row.CodArt.trim() + "')>Nuovo C.N.T.</a>";
                    }
                },
                {
                    data: null, render: function (data, type, row) {
                        return "<a href='#' class='btn btn-secondary btn-sm' onclick=Upload('" + row.CodArt.trim() + "');>Nuova C.L.</a>";
                    }
                },
                {
                    data: null, render: function (data, type, row) {
                       
                        if (row.FileName != null && row.FileName != '' && row.FileName != 'X') 
                            return "<a href='#' class='btn btn-info btn-sm' onclick=ShowEsito('" + row.Id + "')>File Esito</a>";
                        else
                            return "<button class='btn btn-info btn-sm' disabled>Esito</input>";
                       
                    }
                }

            ],
            columnDefs: [
                {
                    targets: 0,
                    className: 'dt-body-right',
                    "width": "2%"
                },
                {
                    targets: 1,
                    className: 'dt-body-left',
                    "width": "10%"
                },
                {
                    targets: 2,
                    className: 'dt-body-left',
                    "width": "12%"
                },
                {
                    targets: 3,
                    className: 'dt-body-left',
                    "width": "18%"
                },
                {
                    targets: 4,
                    className: 'dt-body-left',
                    "width": "10%"
                },
                {
                    targets: 5,
                    className: 'dt-body-left',
                    "width": "10%"
                },
                {
                    targets: 6,
                    className: 'dt-body-left',
                    "width": "5%"
                },
                {
                    targets: 7,
                    className: 'dt-body-center',
                    "width": "5%"
                },
                {
                    targets: 8,
                    className: 'dt-body-center',
                    "width": "5%"
                },
                {
                    targets: 9,
                    className: 'dt-body-center',
                    "width": "5%"
                }
            ],

            "lengthMenu": [[10, 15, 25, 50, 100, 200], [10, 15, 25, 50, 100, 200]]
        });

    table.on('search.dt', function () {
        if (!chiCerca) {
            $('#filtroGruppiMerce input:checkbox').prop('checked', false);
            $('#ggScad').val('');
        }
       
    });
    
    // Add event listener for opening and closing details
    $('#tblControlli tbody').on('click', 'td.details-control', function () {
        debugger;
        var tab = $("#tblControlli").dataTable();
        var tr = $(this).closest('tr');
        var row = table.row(tr);
        var mydata = table.row(tr).data();
        

        if (row.child.isShown()) {
            // This row is already open - close it
            row.child.hide();
            tr.removeClass('shown');
        }
        else {
           
            $.ajax({
                url: '/ATZ_CONTROLLI/ControlliStor',
                type: "POST",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: JSON.stringify("{ 'codArt': '" + mydata.CodArt+ "'}"),
                success: function (result) {
                    // Open this row
                    //row.child(format(row.data())).show();
                    row.child(format(result)).show();
                    tr.addClass('shown');
                },
                error: function (err) {
                    debugger;
                    Swal.fire({
                        title: "INFO",
                        html: err.responseText,
                        type: "info"

                    });
                }
            });

           
        }
        

    });

   
    $('#btCercaScadenze').click(function () {
        chiCerca = "GG";
        //azzero tutti i checkbox di filtro
       
        $('#filtroGruppiMerce input:checkbox').prop('checked', false);
              
        table.search('').draw();
       
    });

    $('#btFiltraGM').click(function () {
        chiCerca = "GM";
        $('#ggScad').val('');
        table.search('').draw();

    });
    $('.xx').click(function () {
        //debugger;
        var d = table.row(codArt).data();

        d.counter++;

        table
            .row(this)
            .data(d)
            .draw();
    });


});

function loadJson(url) {
    return fetch(url)
        .then(response => {
            if (response.status == 200) {
                return response.json();
            } else {
                throw new HttpError(response);
            }
        })
}
/* Formatting function for row details - modify as you need */
function format(d) {
    // `d` is the original data object for the row
    var tab = '<table cellpadding="5" cellspacing="0" border="0" style="margin-left:50px;">' +
        '<thead>' +
        '<tr>' +
        '<th>Data Documento</th>' +
        '<th>File Name</th>' +
        '<th>Apri</th>' +
        '<th>Elimina</th>' +
        '</tr>' +
        '</thead>' +
        '<tbody>';
    d.forEach(function (item) {
       
        var str = item.DataDoc;
        var date = moment(str);
        var dateComponent = date.utc().local().format('DD/MM/YYYY');
        var fName = item.FileName;
        
        tab += '<tr style ="background-color:#FFF"><td class=tdDet>' + dateComponent + '</td><td class=tdDet>' + fName + '</td><td class=tdDet>' + "<a href='#' style='color:#547cdd' onclick=ShowEsito('" + item.Id + "')>Apri</a> </td><td><a href='#' onclick=EliminaAllegato('"+item.Id+"','"+item.FileName+"')>Elimina </td></tr>";
    });

    tab += '</tbody>';
    return tab;  
};



 