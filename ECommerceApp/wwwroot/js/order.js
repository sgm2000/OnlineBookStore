var dataTable;

$(function () {
    const status = $('#orderStatusFilter').val() || 'all';
    loadDataTable(status);

    $('#orderStatusFilter').on('change', function () {
        const selectedStatus = $(this).val();
        dataTable.ajax.url(`/Admin/Order/GetAll?status=${selectedStatus}`).load();
    });
});

function loadDataTable(status) {
    dataTable =  $('#tblData').DataTable({
        "ajax": {
            "url": `/Admin/Order/GetAll?status=${status}`,
            "type": "GET",
            "datatype": "json",
            "dataSrc": "data"
        },
        "columns": [
            { "data": "id", "width": "10%" },           // lowercase
            { "data": "name", "width": "10%" },
            { "data": "phoneNumber", "width": "10%" },
            { "data": "applicationUser.email", "width": "10%" },
            { "data": "orderStatus", "width": "10%" },   
            { "data": "orderTotal", "width": "10%" },   
            {
                "data": "id",
                "render": function (data) {
                    return `
                        <div class="w-10 btn-group" role="group">
                            <a href="/admin/order/details?orderId=${data}" class="btn btn-primary mx-2" style="cursor:pointer; width:100px;">
                                <i class="bi bi-pencil-square"></i>
                            </a>
                            &nbsp;                            
                        </div>
                    `;
                },
                "width" : "15%",

            }
            
        ]
    });
}

