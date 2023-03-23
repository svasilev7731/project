using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Net;
using Newtonsoft.Json;
using WpfNetFrameWork.Weather;
using System.Diagnostics.Metrics;
using System.Security.Cryptography.X509Certificates;
using WpfApp2;
using static System.Net.Mime.MediaTypeNames;
using System.Printing;
using static WpfNetFrameWork.Weather.OpenWeather;

namespace WpfNetFrameWork
{
    
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
			getWeatherMainWindow();
		}
        private void BtnSearch_Click(object sender, EventArgs e)
        {
            getWeatherMainWindow();
        }
        DateTime convertDateTime(double millisec)
        {
            DateTime day = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc).ToLocalTime();
            day = day.AddSeconds(millisec).ToLocalTime();
            return day;
        }    
        
		public void getWeatherMainWindow()
        {
            using (WebClient webClient = new WebClient())
            {
                    
                    var url = string.Format("https://api.openweathermap.org/data/2.5/weather?q={0}&lang=RU&appid=197728a1fd1d0ac928159268a4340a99", TBCity.Text);
                    var json = webClient.DownloadString(url);

                    
                    var result = JsonConvert.DeserializeObject<OpenWeather.Root>(json);
                    OpenWeather.Root outPut = result;

                    City.Content = string.Format("{0}", outPut.name);
                    Country.Content = string.Format("{0}", outPut.sys.country);
                    Degrees.Content = string.Format("{0}°C", (outPut.main.temp).ToString("0"));
                    MinDegrees.Content = string.Format("Мин: {0}°C", Math.Floor(outPut.main.temp_min));
                    MaxDegrees.Content = string.Format("Макс: {0}°C", Math.Ceiling(outPut.main.temp_max));
                    Pressure.Content = string.Format("Давление: {0} ГПА", outPut.main.pressure);
                    SpeedOfWind.Content = string.Format("Ветер: {0} М/С", (outPut.wind.speed).ToString("0"));
                    Humadity.Content = string.Format("Влажность: {0} %", outPut.main.humidity);
                    Visibility.Content = string.Format("Видимость {0} КМ", Math.Ceiling(outPut.visibility));
                    TempFar.Content = string.Format(" {0} °F", Math.Floor(outPut.main.temp1));
                    Feels_like.Content = string.Format("Ощущается как {0}°C", Math.Floor(outPut.main.feels_like));
                    Sunrice.Content = string.Format("Восход {0}", convertDateTime(outPut.sys.sunrise).ToShortTimeString());
                    Sunset.Content = string.Format("Закат {0}", convertDateTime(outPut.sys.sunset).ToShortTimeString());
                    DateTime1.Content = string.Format("Текущая дата: {0}", convertDateTime(outPut.dt).ToLongDateString());

					if (outPut.weather[0].main == "Clouds")
                    {
                        mainen.Content = "Облачно";
                        BitmapImage ico = new BitmapImage();
                        ico.BeginInit();
                        ico.UriSource = new Uri("overcast.png", UriKind.Relative);
                        ico.EndInit();
                        picIcon.Source = ico;
                    }
                    else if (outPut.weather[0].main == "Rain")
                    {
                        mainen.Content = "Дождь";
                        BitmapImage ico = new BitmapImage();
                        ico.BeginInit();
                        ico.UriSource = new Uri("rain.png", UriKind.Relative);
                        ico.EndInit();
                        picIcon.Source = ico;
                    }
                    else if (outPut.weather[0].main == "Drizzle")
                    {
                        mainen.Content = "Моросящий дождь";
                        BitmapImage ico = new BitmapImage();
                        ico.BeginInit();
                        ico.UriSource = new Uri("drizzle.png", UriKind.Relative);
                        ico.EndInit();
                        picIcon.Source = ico;
                    }
                    else if (outPut.weather[0].main == "Clear")
                    {
                        mainen.Content = "Ясно";
                        BitmapImage ico = new BitmapImage();
                        ico.BeginInit();
                        ico.UriSource = new Uri("hot.png", UriKind.Relative);
                        ico.EndInit();
                        picIcon.Source = ico;
                    }

                    if (outPut.wind.deg > 270 && outPut.wind.deg < 360) Deg.Text = "Направление ветра: Северо-западный";
                    else if (outPut.wind.deg > 180 && outPut.wind.deg < 270) Deg.Text = "Направление ветра: Юго-западный";
                    else if (outPut.wind.deg > 90 && outPut.wind.deg < 180) Deg.Text = "Направление ветра: Юго-восточный";
                    else if (outPut.wind.deg > 0 && outPut.wind.deg < 90) Deg.Text = "Направление ветра: Северо-западный";
                    else if (outPut.wind.deg == 360 || outPut.wind.deg == 0) Deg.Text = "Направление ветра: Северный";
                    else if (outPut.wind.deg == 180) Deg.Text = "Направление ветра: Южный";
                    else if (outPut.wind.deg == 270) Deg.Text = "Направление ветра: Западный";
                    else if (outPut.wind.deg == 90) Deg.Text = "Направление ветра: Восточный";

				if (outPut.weather[0].main == "Clear" && outPut.main.temp >= 25 && outPut.wind.deg >= 90 && outPut.wind.deg <= 270) hat.Text = "Кепка";
				else if ((outPut.weather[0].main == "Clouds" || outPut.weather[0].main == "Clear") && outPut.main.temp >= 5 && outPut.main.temp <= 24 && outPut.wind.deg >= 90 && outPut.wind.deg <= 270) hat.Text = "Без шапки";
				else if (outPut.main.temp >= 2 && outPut.main.temp <= 4 && outPut.wind.deg >= 90 && outPut.wind.deg <= 270) hat.Text = "Шапка";

				if (outPut.weather[0].main == "Clear" && outPut.main.temp >= 25 && (outPut.wind.deg <= 90 || outPut.wind.deg >= 270)) hat.Text = "Кепка";
				else if ((outPut.weather[0].main == "Clouds" || outPut.weather[0].main == "Clear") && outPut.main.temp >= 6 && outPut.main.temp <= 24 && (outPut.wind.deg <= 90 || outPut.wind.deg >= 270)) hat.Text = "Без шапки";
				else if (outPut.main.temp <= 5 && (outPut.wind.deg <= 90 || outPut.wind.deg >= 270)) hat.Text = "Шапка";



				if (outPut.weather[0].main == "Clear" && outPut.main.temp >= 25 && outPut.wind.deg >= 90 && outPut.wind.deg <= 270) sleeveshirt.Text = "Футболка";
				else if ((outPut.weather[0].main == "Clouds" || outPut.weather[0].main == "Clear") && outPut.main.temp >= 15 && outPut.main.temp <= 24 && outPut.wind.deg >= 90 && outPut.wind.deg <= 270) sleeveshirt.Text = "Кофта";
				else if (outPut.main.temp <= 14 && outPut.wind.deg >= 90 && outPut.wind.deg <= 270) sleeveshirt.Text = "Куртка";

				if (outPut.weather[0].main == "Clear" && outPut.main.temp >= 20 && (outPut.wind.deg <= 90 || outPut.wind.deg >= 270)) sleeveshirt.Text = "Легкая рубашка";
				else if ((outPut.weather[0].main == "Clouds" || outPut.weather[0].main == "Clear") && outPut.main.temp >= 15 && outPut.main.temp <= 19 && (outPut.wind.deg <= 90 || outPut.wind.deg >= 270)) sleeveshirt.Text = "Ветровка";
				else if (outPut.main.temp >= 0 && outPut.main.temp <= 14 && (outPut.wind.deg <= 90 || outPut.wind.deg >= 270)) sleeveshirt.Text = "Пальто";
				else if (outPut.main.temp <= 0 && (outPut.wind.deg <= 90 || outPut.wind.deg >= 270)) sleeveshirt.Text = "Теплая куртка";



				if ((outPut.weather[0].main == "Clouds" || outPut.weather[0].main == "Clear") && outPut.main.temp >= 25 && outPut.wind.deg >= 90 && outPut.wind.deg <= 270) jean.Text = "Шорты";
				else if ((outPut.weather[0].main == "Clouds" || outPut.weather[0].main == "Clear") && outPut.main.temp >= 5 && outPut.main.temp <= 24 && outPut.wind.deg >= 90 && outPut.wind.deg <= 270) jean.Text = "Джинсы";
				else if (outPut.main.temp <= 4 && (outPut.wind.deg >= 90 || outPut.wind.deg <= 270)) jean.Text = "Утепленные брюки";

				if ((outPut.weather[0].main == "Clouds" || outPut.weather[0].main == "Clear") && outPut.main.temp >= 25 && (outPut.wind.deg <= 90 || outPut.wind.deg >= 270)) jean.Text = "Шорты";
				else if ((outPut.weather[0].main == "Clouds" || outPut.weather[0].main == "Clear") && outPut.main.temp >= 6 && outPut.main.temp <= 24 && (outPut.wind.deg <= 90 || outPut.wind.deg >= 270)) jean.Text = "Джоггеры";
				else if (outPut.main.temp <= 5 && (outPut.wind.deg <= 90 || outPut.wind.deg >= 270)) jean.Text = "Утепленные джоггеры";



				if (outPut.weather[0].main == "Clear" && outPut.main.temp >= 25 && outPut.wind.deg >= 90 && outPut.wind.deg <= 270) sneakers.Text = "Шлепанцы";
				else if ((outPut.weather[0].main == "Clouds" || outPut.weather[0].main == "Clear") && outPut.main.temp >= 5 && outPut.main.temp <= 24 && outPut.wind.deg >= 90 && outPut.wind.deg <= 270) sneakers.Text = "Кеды";
				else if (outPut.main.temp >= 2 && outPut.main.temp <= 4 && outPut.wind.deg >= 90 && outPut.wind.deg <= 270) sneakers.Text = "Ботинки";

				if (outPut.weather[0].main == "Clear" && outPut.main.temp >= 25 && (outPut.wind.deg <= 90 || outPut.wind.deg >= 270)) sneakers.Text = "Шлепанцы";
				else if ((outPut.weather[0].main == "Clouds" || outPut.weather[0].main == "Clear") && outPut.main.temp >= 6 && outPut.main.temp <= 24 && (outPut.wind.deg <= 90 || outPut.wind.deg >= 270)) sneakers.Text = "Кроссовки";
				else if (outPut.main.temp <= 5 && (outPut.wind.deg <= 90 || outPut.wind.deg >= 270)) sneakers.Text = "Ботинки";
				else if (outPut.weather[0].main == "Rain" && outPut.main.temp <= 25) sneakers.Text = "Резиновые сапоги";
			}
        }
    }
}