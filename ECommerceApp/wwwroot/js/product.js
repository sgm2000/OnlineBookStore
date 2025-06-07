var dataTable;

$(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable =  $('#tblData').DataTable({
        "ajax": {
            "url": "/admin/product/getall",
            "type": "GET",
            "datatype": "json",
            "dataSrc": "data"
        },
        "columns": [
            { "data": "title", "width": "25%" },           // lowercase
            { "data": "isbn", "width": "10%" },
            { "data": "listPrice", "width": "10%" },
            { "data": "author", "width": "10%" },
            { "data": "category.name", "width": "10%" },   // nested property
            {
                "data": "id",
                "render": function (data) {
                    return `
                        <div class="w-10 btn-group" role="group">
                            <a href="/admin/product/upsert?id=${data}" class="btn btn-primary mx-2" style="cursor:pointer; width:100px;">
                                <i class="bi bi-pencil-square"></i>
                            </a>
                            &nbsp;
                            <a onClick=Delete('/admin/product/delete/${data}') class="btn btn-danger mx-2" style="cursor:pointer; width:100px;">
                                <i class="bi bi-trash3"></i>
                            </a>
                        </div>
                    `;
                },
                "width" : "15%",

            }
            
        ]
    });
}

function Delete(url) {
    Swal.fire({
        title: "Are you sure?",
        text: "You won't be able to revert this!",
        icon: "warning",
        showCancelButton: true,
        confirmButtonColor: "#3085d6",
        cancelButtonColor: "#d33",
        confirmButtonText: "Yes, delete it!"
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: url,
                type: 'DELETE',
                success: function (data) {
                    dataTable.ajax.reload();
                    toastr.success(data.message);
                }
            })
        }
    });
}