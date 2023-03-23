using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace WpfNetFrameWork.Weather
{

    internal class OpenWeather
    {

        public class Coord
        {
            public double lon { get; set; }
            public double lat { get; set; }
        }
        public class Weather
        {
            public int id { get; set; }

            public string main { get; set; }

            public string description { get; set; }

            public string icon { get; set; }
        }
        public class Main
        {
            
            private double _feels_like { get; set; }
            public double feels_like
            {
                get
                {
                    return _feels_like;
                }
                set
                {
                    _feels_like = value - 273.15;
                }
            }
            public double temp1 { get; set; }
            private double _temp_max { get; set; }
            private double _temp_min { get; set; }
            private double _temp { get; set; }
            public double temp
            {
                get
                {
                    return _temp;
                }
                set
                {
                    _temp = value - 273.15;
                    temp1 = temp * 1.8 + 32;
                }
            }
            public double temp_min
            {
                get
                {
                    return _temp_min;
                }
                set
                {
                    _temp_min = value - 273.15;
                }
            }
            public double temp_max
            {
                get
                {
                    return _temp_max;
                }
                set
                {
                    _temp_max = value - 273.15;
                }
            }
            public double pressure { get; set; }

            public double humidity { get; set; }
        }
        public class Wind
        {
            public double deg { get; set; }
            private double _speed { get; set; }
            public double speed
            {
                get
                {
                    return _speed;
                }
                set
                {
                    _speed = value;
                }
            }
        }

        public class Sys
        {
            public double sunrise { get; set; }
            public double sunset { get; set; }
            public string country { get; set; }
        }
        public class Root
        {
            private double _visibility { get; set; }
            public double visibility
            {
                get
                {
                    return _visibility;
                }
                set
                {
                    _visibility = value / 1000;
                }
            }
            public string name { get; set; }
            public Sys sys { get; set; }
            public double dt { get; set; }
            public Wind wind { get; set; }
            public Main main { get; set; }
            public List<Weather> weather { get; set; }
            public Coord coord { get; set; }
        }    
    }
}