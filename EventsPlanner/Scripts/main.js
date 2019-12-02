$(function ()
    {
    $('#calendar').fullCalendar({
        editable: true,
        selectable: true,
        eventLimit: true,
        eventLimit: 2, 
        header: {
            left: 'prev,next,today',
            center: 'title',
            right: 'listMonth,month'
        },

    //DAY
        dayClick: function (date, jsEvent, view) {
            //alert(date.format());
			    
        },

        select: function (start, end, jsEvent, view) {
            if (start.format() == end.subtract(1, "days").format())
                alert(start.format())

            else
                alert(start.format() + ' - ' + end.format())
        },


    //EVENT

        eventClick: function (event, jsEvent, view) {
            if (event.end != null)
                alert(event.id + ' ' + event.title + " " + event.start.format() + " - " + event.end.format());
            else
                alert(event.id + ' ' + event.title + " " + event.start.format() + " (one day)");

        },
        eventResize: function (eventResizeInfo) {
            alert(eventResizeInfo.id + ' : ' + eventResizeInfo.start.format() + " - " + eventResizeInfo.end.subtract(1).format())
        },

        eventDrop: function (event) {
            if (event.end != null)
                alert(event.id + ' ' + event.title + " " + event.start.format() + " - " + event.end.format());
            else
                alert(event.id + ' ' + event.title + " " + event.start.format() + " (one day)");
        },


        events:
            [
                {
                    allDay: true,
                    title: "Событие один descr",
                    start: "12/27/2019",
                    id: 12,
                    color: 'red',
                },
                {
                    allDay: true,
                    title: "Событие два",
                    start: "12/13/2019",

                    id: 13
                }, {
                    allDay: true,
                    title: "Событие еще одно",
                    start: "12/13/2019",
                    id: 14
                },
                {
                    allDay: true,
                    title: "Событие длинное",
                    start: "12/15/2019",
                    end: "12/18/2019",
                    id: 15
                }
            ]
    });
    }
)
