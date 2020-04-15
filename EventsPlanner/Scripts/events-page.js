
function GetInfoEventForm(eventId) {
    //$('#event-form').html(loader_div);
    $.ajax({
        url: "/Home/InfoEventForm/" + eventId,
        success: function (data) {
            $('#event-form').html(data);
        }
    });
}