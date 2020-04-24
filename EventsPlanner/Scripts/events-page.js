
var loader_div = '<div class="loader-div"><div class="loader"></div></div>'

function GetInfoEventForm(eventId) {
    $('#event-form').html(loader_div);
    $.ajax({
        url: "/Home/InfoEventForm/" + eventId,
        success: function (data) {
            $('#event-form').html(data);
        }
    });
}

function GetEditEventForm(eventId) {
    $('#event-form').html(loader_div);
    $.ajax({
        url: "/Home/EditEventForm/" + eventId,
        success: function (data) {
            $('#event-form').html(data);
        }
    });
}
function EditEventEndEditCalendar(form) {
    $('#event-form').html(loader_div);

    var url = "/Home/EditEvent";
    $.ajax({
        type: "POST",
        url: url,
        data: form.serialize(),
        success: function (data) {
            GetInfoEventForm(data);
        }
    });
}

function EventDelete(eventId) {
    $.ajax({

        url: "/Home/DelEvent/" + eventId,
        success: function (data) {
            $('#event-form').html('<small style="color:#b8b8b8">Удалено.</small>');

            //удаление из списка
            $('#eventsTable').find()
            $('#eventsTable>tbody>tr>td').filter(function () { return $(this).text().trim() == eventId }).parent().remove()

            // удаление из карты
            eventsMap.geoObjects.each(function (element, index) {
                if (element.id == eventId)
                    eventsMap.geoObjects.remove(element);
            })
        }
    });
}

function GetListEventForm(date){

    //метод-заглушка
}

function checkValidDatePicker() {
    if ($("#startEvent").val() != '') {
        $('#datetimepickerEnd').data("DateTimePicker").minDate($("#startEvent").val());
    }

    if ($("#endEvent").val() != '') {
        buffer = $("#startEvent").val();
        $('#datetimepickerStart').data("DateTimePicker").maxDate($("#endEvent").val());
        buffer = $("#startEvent").val(buffer);

    }
}
