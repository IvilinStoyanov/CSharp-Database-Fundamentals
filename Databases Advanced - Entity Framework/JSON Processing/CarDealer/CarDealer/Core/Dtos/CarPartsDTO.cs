using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace CarDealer.App.Core.Dtos
{
    [JsonObject("car")]
    public class CarPartsDTO
    {
        [JsonProperty("Make")]
        public string Make { get; set; }

        [JsonProperty("Model")]
        public string Model { get; set; }

        [JsonProperty("TravelledDistance")]
        public long TravelledDistance { get; set; }

        [JsonProperty("parts")]
        public PartDto[] CarParts { get; set; }
    }
}

