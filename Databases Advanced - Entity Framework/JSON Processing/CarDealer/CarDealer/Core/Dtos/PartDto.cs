using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace CarDealer.App.Core.Dtos
{
    [JsonObject("user")]
    public class PartDto
    {
        [JsonProperty("Name")]
        public string Name { get; set; }

        [JsonProperty("Price")]
        public decimal Price { get; set; }
    }
}
