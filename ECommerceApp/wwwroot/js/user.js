var dataTable;

$(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#tblData').DataTable({
        "ajax": {
            "url": "/admin/user/getall",
            "type": "GET",
            "datatype": "json",
            "dataSrc": "data"
        },
        "columns": [
            { "data": "name", "width": "25%" },           // lowercase
            { "data": "email", "width": "10%" },
            { "data": "phoneNumber", "width": "10%" },
            { "data": "company.name", "width": "10%" },
            { "data": "role", "width": "10%" },
            {
                "data": { id: "id", lockoutEnd: "lockoutEnd" },
                "render": function (data) {
                    var today = new Date().getTime();
                    var lockout = new Date(data.lockoutEnd).getTime();
                    if (lockout > today) {
                        return `
                        <div class="w-10 btn-group" role="group">
                            <a onclick = LockUnlock('${data.id}') class="btn btn-danger mx-2" style="cursor:pointer; width:100px;">
                                <i class="bi bi-lock-fill"></i> UnLock
                            </a>
                            <a href = "/admin/user/RoleManagement?userId=${data.id}" class="btn btn-primary mx-2" style="cursor:pointer; width:150px;">
                                <i class="bi bi-pencil-square"></i> Permission
                            </a>
                        </div>
                    `;
                    }
                    else {
                        return `
                        <div class="w-10 btn-group" role="group">
                            <a  onclick = " LockUnlock('${data.id}')" class="btn btn-success mx-2" style="cursor:pointer; width:100px;">
                             <i class="bi bi-unlock-fill"></i> UnLock
                            </a>
                            <a  href = "/admin/user/RoleManagement?userId=${data.id}" class="btn btn-primary mx-2" style="cursor:pointer; width:150px;">
                                <i class="bi bi-pencil-square"></i> Permission
                            </a>
                        </div>
                    `;
                    }

                },
                "width": "15%",

            }

        ]
    });
}

function LockUnlock(id) {
    $.ajax({
        type: "POST",
        url: '/Admin/User/LockUnlock',
        data: JSON.stringify(id),
        contentType: "application/json",
        success: function (data) {
            if (data.success) {
                toastr.success(data.message);
                dataTable.ajax.reload();
            }
        }
    })
}