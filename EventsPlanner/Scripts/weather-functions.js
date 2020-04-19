var daysWeather = [];

var ViewDebug;
var responseDataDictionary;
function AddWeatherToCalendar(view, city) {


    if (daysWeather != undefined &&
        daysWeather.hasOwnProperty(city)&&
        daysWeather[city].hasOwnProperty(moment(view.start._i).format("YYYY-MM-DD")) &&
        daysWeather[city].hasOwnProperty(moment(view.end._i).format("YYYY-MM-DD"))) {
        DrawMonthDaysWeatherToCalendar(city);
        return;
    }
    console.log("[AddWeatherToCalendar] sended ajax to get weather from " + city);
    $.ajax({
        url: "/Weather/GetThreeMonthTemperature?city=" + city +"&firstMonth=" + moment(view.start._i).format("MM") + "&year=" + moment(view.start._i).format("YYYY"),
        success: function (data) {
            responseDataDictionary = JSON.parse(data);
            daysWeather[city] = Object.assign(responseDataDictionary[city], daysWeather[city]);

            for (var key in responseDataDictionary[city]) {
            }
            DrawMonthDaysWeatherToCalendar(city);

        }
    });

}

function DrawMonthDaysWeatherToCalendar(city) {

    $(document).find(".fc-content-skeleton thead td").each(function (i, v) {


        var newDiv = document.createElement("div");
        try {
            newDiv.className = "weatherCalendarDiv";
            newDiv.style.display = "inline-block";
            newDiv.style.position = "relative";
            var imageDescription = daysWeather[city][$(this).attr('data-date')].description;
            var imageUrl = descriptionToImage[imageDescription] == null ? descriptionToImage["Ясно"] : descriptionToImage[imageDescription];
            newDiv.innerHTML = "<img style='float:left; height:40px; padding-left:24px;' src='" + imageUrl + "' title = '" + imageDescription + "'/>";
            newDiv.innerHTML += "<span style='position:absolute; left:5px;top:3px; font-size:16px;'>" + daysWeather[city][$(this).attr('data-date')].dayTemp + "°</span>";
            newDiv.innerHTML += "<span style='position:absolute; left:5px;top:19px;  color:#949494'>" + daysWeather[city][$(this).attr('data-date')].nightTemp + "°</span>";
            $(this).children("." + newDiv.className).remove();
            $(this).prepend(newDiv);
        }
        catch (e) { }
    });

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
//daysWeather['2020-04-30'] = { date: "2020-04-29T00:00:00", dayTemp: 16, nightTemp: 10, description: "Ясно" };
//daysWeather['2020-06-30'] = { date: "2020-06-29T00:00:00", dayTemp: 22, nightTemp: 16, description: "Частично облачно" };
//daysWeather['2020-09-30'] = { date: "2020-09-29T00:00:00", dayTemp: 15, nightTemp: 10, description: "Частично облачно" };
//daysWeather['2020-11-30'] = { date: "2020-11-29T00:00:00", dayTemp: 0, nightTemp: -4, description: "Частично облачно" };
//daysWeather['2020-07-31'] = { date: "2020-07-30T00:00:00", dayTemp: 26, nightTemp: 21, description: "Частично облачно" };
//daysWeather['2020-08-31'] = { date: "2020-08-30T00:00:00", dayTemp: 21, nightTemp: 16, description: "Частично облачно" };
//daysWeather['2020-10-31'] = { date: "2020-10-30T00:00:00", dayTemp: 7, nightTemp: 3, description: "Ясно" };
//daysWeather['2020-12-31'] = { date: "2020-12-30T00:00:00", dayTemp: 1, nightTemp: -3, description: "Облачно" };



function GetEngRegionNaimFromAddress(address) {
    for (var city in Object.keys(regionsToEnglishCitys)) {
        if (address.toLowerCase().indexOf(Object.keys(regionsToEnglishCitys)[city].toLowerCase()) != -1)
            return regionsToEnglishCitys[Object.keys(regionsToEnglishCitys)[city]];
    }
}

var regionsToEnglishCitys =
{
    'Брест': 'brest',
    'Брестская область': 'brest',

    'Витебск': 'vitebsk',
    'Витебская область': 'vitebsk',

    'Гомель': 'gomel',
    'Гомельсксая  область': 'gomel',

    'Гродно': 'grodno',
    'Гродненская область': 'grodno',

    'Минск': 'minsk',
    'Минская область': 'minsk',

    'Могилев': 'mogilev',
    'Могилевская область': 'mogilev',

    'Могилёв': 'mogilev',
    'Могилёвская область': 'mogilev',
}