using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherForecast.Core.Boundaries.Responses
{
    public class ErrorResponse
    {
        public string Message { get; set; } = default!;

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}