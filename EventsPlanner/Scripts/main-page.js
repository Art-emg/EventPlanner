﻿
var calendar = $('#calendar');

calendar.fullCalendar({
    editable: true,

    selectable: true,
    eventLimit: true,
    eventLimit: 4,
    displayEventTime: false,

    customButtons: {
        refreshButton: {
            text: '↻',
            click: function () {
                calendar.fullCalendar('refetchEvents');
            }
        }
    },
        header: {
            left: 'prev,next,today refreshButton',
            center: 'title',
            right: 'listMonth,month'
        },

    //DAY
        dayClick: function (date, jsEvent, view) {
            //alert(date.format());
			    
        },

        select: function (start, end, jsEvent, view) {
            // выбран один день
            if (start.format() == end.subtract(1, "days").format()) {
                GetListEventForm(start.format())
            }
            // выбрано несколько дней
            else {
                GetAddEventFormStartEndDate(start.format(), end.format());
            }
        },


    //EVENT

        eventClick: function (event, jsEvent, view) {
            GetInfoEventForm(event.id);
        },

        eventResize: function (eventResizeInfo) {
            alert(eventResizeInfo.id + ' : ' + eventResizeInfo.start.format() + " - " + eventResizeInfo.end.subtract(1).format())
        },

    eventDrop: function (event) {
        if (event.end != null) {
            event.end = event.end.subtract(1, 'day');
        }
        EditEvent(event);
            //if (event.end != null)
            //    alert(event.id + ' ' + event.title + " " + event.start.format() + " - " + event.end.format());
            //else
            //    alert(event.id + ' ' + event.title + " " + event.start.format() + " (one day)");
        },

        eventSources: [
            {
                url: '/Home/GetCreatedEvents'
            },
            {
                url: '/Home/GetInvitedEvents',
                color: '#4caf50',
                editable:false
            }    
        ]

        //events:[{ allDay: true,name: "dfdfdsf",title: "Событие один descr",start: "12/27/2019",id: 12,color: 'red'},{allDay: true, title: "Событие длинное", start: "12/15/2019", end: "12/18/2019", id: 15}]

    });



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
            $('#calendar').fullCalendar('removeEvents', eventId);
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

            calendar.fullCalendar('refetchEvents');
            GetInfoEventForm(data);
        }
    });
}
function EditEvent(event) {
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
            StartDate: event.start.format(),
            EndDate: event.end.format()
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
            calendar.fullCalendar('refetchEvents');
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

function debug(e) {
   
}