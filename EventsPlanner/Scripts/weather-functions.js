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
        url: "/Home/GetThreeMonthTemperature?city=" + "minsk" +"&firstMonth=" + moment(view.start._i).format("MM") + "&year=" + moment(view.start._i).format("YYYY"),
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