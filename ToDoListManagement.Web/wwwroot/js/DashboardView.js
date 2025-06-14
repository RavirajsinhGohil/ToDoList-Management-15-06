$(document).ready(function () {
    $("#addProjectModal").on('hidden.bs.modal', function () {
        $("#addProjectForm").trigger("reset");
        $("#addProjectForm").find(".text-danger").text("");
    });

    $("#updateProjectModal").on('hidden.bs.modal', function () {
        $("#editProjectForm").trigger("reset");
        $("#editProjectForm").find(".text-danger").text("");
    });

    $("#projectTable").DataTable({
        "columnDefs": [
            {
                targets: [0, 1],
                orderable: true,
            },
            {
                targets: [[2, 3, 4, 5], [2, 3, 4] ],
                // targets: [2, 3, 4, 5],
                orderable: false,
            }
        ],
        "pageLength": 5,
        "lengthMenu": [ [5, 10, 15, 20, -1], [5, 10, 15, 20, "All"] ],
        "paging": true,
        "language": {
            "paginate": {
                "previous": "&laquo;",
                "next": "&raquo;"
            },
        },
        "lengthChange": true,
        "searching": true,
        "scrollCollapse": true,
        "scrollY": '455px'
    });
});

function openDeleteProjectModal(projectId) {
    $("#deleteProjectLink").attr("href", `/Project/DeleteProject?projectId=${projectId}`);
    $('#deleteProjectModal').modal('show');
}

function openEditProjectModal(projectId) {
    $.ajax({
        url: `/Project/GetProjectById?projectId=${projectId}`,
        type: 'GET',
        success: function (response) {
            if (response.success === false) {
                toastr.error(response.message);
            }
            else{
                $("#editProjectId").val(projectId);
                $("#editProjectName").val(response.data.projectName);
                $("#editProjectDescription").val(response.data.description);
                $("#updateProjectModal").modal('show');
            }
        },
        error: function () {
            alert("Error fetching project details.");
        }
    });
}