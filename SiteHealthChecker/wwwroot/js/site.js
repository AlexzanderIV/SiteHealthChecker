//$(document).ready(function () {
    
//});

function deleteWebResource(name, id) {
    
    if (id === undefined || id == '') {
        return;
    }

    if (confirm("Are you realy want to delete the Web resource '" + name + "' from the list?")) {
        alert(name + " ; " + id);
        $.post("/Admin/DeleteWebResource", { id: id })
            .done(function (data) {
                location.reload();
            });
    }
}


