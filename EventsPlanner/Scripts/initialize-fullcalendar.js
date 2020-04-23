
//var debugRes;

//var calendar = $('#calendar');
//calendar.fullCalendar({

//    displayEventTime: false,

//});
var Debug;



    var calendarEl = document.getElementById('calendar');

    var calendar = new FullCalendar.Calendar(calendarEl, {
        plugins: ['dayGrid', 'timeGrid', 'list', 'interaction'],
        locale: 'ru',
        eventTimeFormat: {
            hour: '2-digit',
            minute: '2-digit',
            second: '2-digit',
            hour12: false
        },
        allDaySlot: false,

        customButtons: {
            refreshButton: {
                text: '↻',
                click: function () {
                    calendar.refetchEvents();
                }
            }
        },
        header: {
            left: 'prev,next,today refreshButton',
            center: 'title',
            right: 'dayGridMonth,timeGridWeek,timeGridDay,listMonth'
        },

        navLinks: true, // can click day/week names to navigate views
        editable: true,
        selectable: true,
        eventLimit: true,
        eventLimit: 2,
        firstDay: 1,
        eventSources: [
            {
                url: '/Home/GetCreatedEvents'
            },
            {
                url: '/Home/GetInvitedEvents',
                color: '#4caf50',
                editable: false
            }
        ],


        /// DATE

        select: function (info) {
            moment.locale('ru');
            var start = moment(info.start);
            var end = moment(info.end);
            var endBuff = moment(info.end);
            Debug = info;
            var patternDate = 'DD.MM.YYYY HH:mm:ss'

            // выбран один день
            if (start.format() == endBuff.subtract(1, "days").format()) {
                GetListEventForm(start.format(patternDate))
            }
            // выбрано несколько дней
            else {
                var startFormat = start.format(patternDate);
                var endFormat = end.format(patternDate);
                GetAddEventFormStartEndDate(startFormat, endFormat);
            }
        },

        //dateClick: function (info) {

        //    var patternDate = 'DD.MM.YYYY HH:mm:ss'
        //    GetListEventForm(moment(info.start).format(patternDate));
        //},

        /// EVENT
        eventClick: function (info) {
            
            GetInfoEventForm(info.event.id);
        },


        eventResize: function (info) {

            EditEvent(info.event, true);
        },


        eventDrop: function (info) {

            EditEvent(info.event, true);
           
        },


        datesRender: function (info) {
                if(sessionStorage['regionLocation']!=null)
                    AddWeatherToCalendar(info.view, sessionStorage['regionLocation']);
                else 
                    AddWeatherToCalendar(info.view, 'minsk');
        }
    });

    calendar.render();