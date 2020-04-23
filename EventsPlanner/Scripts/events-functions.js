
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

function GetAddEventFormStartDate(startDate) {
    $('#event-form').html(loader_div);
    $.ajax({
        url: "/Home/AddEventFormStartDate?startDate=" + startDate,
        success: function (data) {
            $('#event-form').html(data);
        }
    });
}

function GetAddEventFormStartEndDate(startDate, endDate) {
    $('#event-form').html(loader_div);
    $.ajax({
        url: "/Home/AddEventFormStartEndDate?startDate=" + startDate + "&endDate=" + endDate,
        success: function (data) {
            $('#event-form').html(data);
        }
    });
}

function GetListEventForm(date) {
    $('#event-form').html(loader_div);
    $.ajax({
        url: "/Home/ListEventsForm?date=" + date,
        success: function (data) {
            $('#event-form').html(data);
        }
    });
}
function EventDelete(eventId) {
    $.ajax({

        url: "/Home/DelEvent/" + eventId,
        success: function (data) {
            var event = calendar.getEventById(eventId);
            event.remove();
        }
    });
}

function AddEventEndEditCalendar(form) {
    $('#event-form').html(loader_div);

    var url = "/Home/AddEvent";
    $.ajax({
        type: "POST",
        url: url,
        data: form.serialize(),
        success: function (data) {

            calendar.refetchEvents();
            GetInfoEventForm(data);
        }
    });
}
function EditEvent(event, resizeOrDrug = false) {
    var url = "/Home/EditEvent";
    if (event.end == null)
        event.end = event.start;

    $.ajax({
        type: "POST",
        url: url,
        data:
        {
            EventId: event.id,
            Name: event.title,
            StartDate: moment(event.start).format(),
            EndDate: moment(event.end).format(),
            ResizeOrDrug: resizeOrDrug
        },
        success: function (data) {

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
            calendar.refetchEvents();
            GetInfoEventForm(data);
        }
    });
}

function AddEndShowEvent(eventId) {
    calendar.addEvent()
}

///////////////////

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

