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
            AddWeatherToCalendar(view, element);

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

var daysWeather = [];

var ViewDebug;

function AddWeatherToCalendar(view, element) {
    ViewDebug = view;
    ElemDebug = element;
    if (view.title != $(".fc-center > h2").text()) { alert('AddWeatherToCalendar'); }

    if (daysWeather != undefined &&
        daysWeather.hasOwnProperty(moment(view.start._i).format("YYYY-MM-DD")) &&
        daysWeather.hasOwnProperty(moment(view.end._i).format("YYYY-MM-DD"))) {
        DrawMonthDaysWeatherToCalendar(view, element);
        return;
    }
    $.ajax({
        url: "/Home/GetThreeMonthTemperature?firstMonth=" + moment(view.start._i).format("MM") + "&year=" + moment(view.start._i).format("YYYY"),
        success: function (data) {
            var responseDataDictionary = JSON.parse(data);
            for (var key in responseDataDictionary) {
                daysWeather[key] = responseDataDictionary[key];
            }
            DrawMonthDaysWeatherToCalendar(view, element);

        }
    });

}

function DrawMonthDaysWeatherToCalendar(view, element) {

    $(element).find(".fc-content-skeleton thead td").each(function (i, v) {
        if ($.contains($(".weatherCalendarDiv"), $(v))) {
            console.log("cont");
            return;
        }


        var newDiv = document.createElement("div");
        try {
            newDiv.className = "weatherCalendarDiv";
            newDiv.style.display = "inline-block";
            newDiv.style.position = "relative";
            var imageDescription = daysWeather[$(this).attr('data-date')].description;
            var imageUrl = descriptionToImage[imageDescription] == null ? descriptionToImage["Ясно"] : descriptionToImage[imageDescription];
            newDiv.innerHTML = "<img style='float:left; height:40px; padding-left:24px;' src='" + imageUrl + "' title = '" + imageDescription + "'/>";
            newDiv.innerHTML += "<span style='position:absolute; left:5px;top:3px; font-size:16px;'>" + daysWeather[$(this).attr('data-date')].dayTemp + "°</span>";
            newDiv.innerHTML += "<span style='position:absolute; left:5px;top:19px;  color:#949494'>" + daysWeather[$(this).attr('data-date')].nightTemp + "°</span>";
            $(this).prepend(newDiv);
        }
        catch (e) { }
    });

}

function debug(e) {

}


var descriptionToImage =
{
    "Частично облачно": "https://img.icons8.com/color/48/000000/partly-cloudy-day.png",
    "Частично облачно и слабый дождь": "https://img.icons8.com/color/48/000000/partly-cloudy-rain.png",
    "Облачно": "https://img.icons8.com/color/48/000000/partly-cloudy-day.png",
    "Ясно": "https://img.icons8.com/color/50/000000/summer.png",
    "Облачно и слабый снег": "https://img.icons8.com/color/48/000000/partly-cloudy-rain.png",
    "Преимущественно облачно": "https://img.icons8.com/color/48/000000/partly-cloudy-day.png",
    "Облачно и слабый дождь": "https://img.icons8.com/color/48/000000/partly-cloudy-rain.png",
    "Облачно и временами снег с дождем": "https://img.icons8.com/color/48/000000/partly-cloudy-rain.png",
    "Облачно и слабый мокрый снег": "https://img.icons8.com/color/48/000000/partly-cloudy-rain.png",
    "Облачно и мокрый снег": "https://img.icons8.com/color/48/000000/partly-cloudy-rain.png",
    "Преимущественно ясно": "https://img.icons8.com/color/48/000000/partly-cloudy-day.png",
    "Облачно и дождь": "https://img.icons8.com/color/48/000000/partly-cloudy-rain.png",
    "Облачно и кратковременные осадки": "https://img.icons8.com/color/48/000000/partly-cloudy-rain.png"
};

// Дни, которых нет на сайте
daysWeather['2020-04-30'] = { date: "2020-04-29T00:00:00", dayTemp: 16, nightTemp: 10, description: "Ясно" };
daysWeather['2020-06-30'] = { date: "2020-06-29T00:00:00", dayTemp: 22, nightTemp: 16, description: "Частично облачно" };
daysWeather['2020-09-30'] = { date: "2020-09-29T00:00:00", dayTemp: 15, nightTemp: 10, description: "Частично облачно" };
daysWeather['2020-11-30'] = { date: "2020-11-29T00:00:00", dayTemp: 0, nightTemp: -4, description: "Частично облачно" };
daysWeather['2020-07-31'] = { date: "2020-07-30T00:00:00", dayTemp: 26, nightTemp: 21, description: "Частично облачно" };
daysWeather['2020-08-31'] = { date: "2020-08-30T00:00:00", dayTemp: 21, nightTemp: 16, description: "Частично облачно" };
daysWeather['2020-10-31'] = { date: "2020-10-30T00:00:00", dayTemp: 7, nightTemp: 3, description: "Ясно" };
daysWeather['2020-12-31'] = { date: "2020-12-30T00:00:00", dayTemp: 1, nightTemp: -3, description: "Облачно" };