/// <summary>
/// 
/// </summary>
namespace CIBApp.Views
{
    using CIBApp.ServiceModels;
    using CIBApp.WeatherJsonData;
    using Plugin.Connectivity;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Xamarin.Essentials;
    using Xamarin.Forms;
    using Xamarin.Forms.Xaml;

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class WeatherDashboard : ContentPage
    {
        private Services.WeatherRestService weatherRestService;
        private Label label = new Label();
        public WeatherDashboard()
        {
            InitializeComponent();
            weatherRestService = new Services.WeatherRestService();
            
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            PopulateListView();

        }

        async void PopulateListView()
        {
            
            var isConnectionEnabled = IsDeviceConnectedToInternet();
            if (isConnectionEnabled)
            {
                var constantApiUrl = WeatherServiceModel.WeatherApiAddress;
                var weatherApiUrl = ConstructRequestUri(constantApiUrl).Result;
                var listviewDataResult = await weatherRestService.GetDailyWeatherDataAsync(weatherApiUrl);
                if (listviewDataResult != null)
                {

                    List<Daily> dailyList = new List<Daily>();
                    foreach (var dailyItem in listviewDataResult)
                    {
                        foreach (var dailyEntry in dailyItem.Daily)
                        {
                            foreach (var weatherItem in dailyEntry.Weather)
                            {
                                Daily dailyResult = new Daily
                                {
                                    Dt = dailyEntry.Dt,
                                    Sunrise = dailyEntry.Sunrise,
                                    Sunset = dailyEntry.Sunset,
                                    Temp = new Temp
                                    {
                                        Day = dailyEntry.Temp.Day,
                                        Min = dailyEntry.Temp.Min,
                                        Max = dailyEntry.Temp.Max,
                                        Night = dailyEntry.Temp.Night,
                                        Eve = dailyEntry.Temp.Eve,
                                        Morn = dailyEntry.Temp.Morn
                                    },
                                    FeelsLike = new FeelsLike
                                    {
                                        Day = dailyEntry.FeelsLike.Day,
                                        Night = dailyEntry.FeelsLike.Night,
                                        Eve = dailyEntry.FeelsLike.Eve,
                                        Morn = dailyEntry.FeelsLike.Morn
                                    },
                                    Pressure = dailyEntry.Pressure,
                                    Humidity = dailyEntry.Humidity,
                                    DewPoint = dailyEntry.DewPoint,
                                    WindSpeed = dailyEntry.WindSpeed,
                                    WindDeg = dailyEntry.WindDeg,
                                    Weather = new List<Weather>
                                            {
                                               new Weather
                                               {
                                                   Id = weatherItem.Id,
                                                   Main = weatherItem.Main,
                                                   Description = weatherItem.Description,
                                                   Icon = weatherItem.Icon
                                               }
                                            },
                                    Clouds = dailyEntry.Clouds,
                                    Pop = dailyEntry.Pop,
                                    Uvi = dailyEntry.Uvi
                                };
                                dailyList.Add(dailyResult);
                                
                            }
                        }
                        
                    }
                    
                    listView.ItemsSource = dailyList;
                    this.Content = listView;
                    
                }
                else
                {
                    await DisplayAlert("Connection", "Device not connected to the internet. Please check connection settings", "Ok");
                }

            }
            
        }

         async void OnListViewItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem != null)
            {
                await Navigation.PushAsync(new WeatherDailyEntry
                {
                    BindingContext = e.SelectedItem as Daily
                },true);
            }
        }

        

        private async Task<string> ConstructRequestUri(string apiAddress)
        {
            double latitude = 0.0;
            double longitude = 0.0;
            var coordinates = await GrabCoordinates();
            for (int i = 0; i < coordinates.Count; i++)
            {
                latitude = coordinates[0];
                longitude = coordinates[1];
            }
            string requestUri = $"https://{apiAddress}";
            requestUri += $"onecall?lat={latitude}";
            requestUri += $"&lon={longitude}"; // or 
            requestUri += "&units=metric";
            requestUri += "&exclude=current,minutely,hourly";
            requestUri += $"&appid={WeatherServiceModel.WeatherApiKey}";
            return requestUri;
        }

        private bool IsDeviceConnectedToInternet()
        {
            return CrossConnectivity.Current.IsConnected;
        }

        private async Task<List<double>> GrabCoordinates()
        {
            var coords = new List<double>();
            var location = await Geolocation.GetLastKnownLocationAsync();
            if (location != null)
            {
                coords.Add(location.Latitude);
                coords.Add(location.Longitude);
            }
            else
            {
                await DisplayAlert("GPS", "Device cannot detect location. Please check your location settings", "Ok");
            }

            return coords;
        }
    }
}