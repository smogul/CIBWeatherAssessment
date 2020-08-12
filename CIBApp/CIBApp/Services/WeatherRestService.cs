/// <summary>
/// 
/// </summary>

namespace CIBApp.Services
{
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Threading.Tasks;
    using CIBApp.WeatherJsonData;
    using Newtonsoft.Json;

    public class WeatherRestService
    {
        HttpClient weatherHttpClient;

        public WeatherRestService()
        {
            weatherHttpClient = new HttpClient();
        }


        public  async Task<IEnumerable<Root>> GetDailyWeatherDataAsync(string queryString)
        {
            Root data = null;
            List<Root> rootResult = null;

            try
            {
                var response = await weatherHttpClient.GetAsync(queryString).ConfigureAwait(false);

                if (response != null)
                {

                    string json = response.Content.ReadAsStringAsync().Result;
                    data = JsonConvert.DeserializeObject<Root>(json);

                    rootResult = new List<Root>
                    {
                        new Root
                        {
                           Lat = data.Lat,
                           Lon = data.Lon,
                           Timezone = data.Timezone,
                           TimezoneOffset = data.TimezoneOffset,
                           Daily = data.Daily
                        }
                    };
                    
                }
            }
            catch (Exception e)
            {

                throw new Exception(e.Message);
            }
            return rootResult;
        }


    }
}
