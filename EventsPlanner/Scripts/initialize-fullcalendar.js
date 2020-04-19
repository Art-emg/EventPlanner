
var debugRes;

var calendar = $('#calendar');
calendar.fullCalendar({
    schedulerLicenseKey: 'GPL-My-Project-Is-Open-Source',
    editable: true,
    selectable: true,
    eventLimit: true,
    eventLimit: 2,
    //themeSystem: 'bootstrap',
    //plugins: ['dayGrid', 'timeGrid', 'list', 'bootstrap'],
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
        right: 'listMonth,month,timeGridWeek'
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

    viewRender:
        function (view, element) {
            if(sessionStorage['regionLocation']!=null)
                AddWeatherToCalendar(view, sessionStorage['regionLocation']);
            else 
                AddWeatherToCalendar(view, 'minsk');
        },


    eventSources: [
        {
            url: '/Home/GetCreatedEvents'
        },
        {
            url: '/Home/GetInvitedEvents',
            color: '#4caf50',
            editable: false
        }
    ]

    //events:[{ allDay: true,name: "dfdfdsf",title: "Событие один descr",start: "12/27/2019",id: 12,color: 'red'},{allDay: true, title: "Событие длинное", start: "12/15/2019", end: "12/18/2019", id: 15}]

});

