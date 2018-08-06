using Newtonsoft.Json;
using System;

namespace CarDealer.App.Core.Dtos
{
   public class CustomerDto
    {
        [JsonProperty("Id")]
        public int Id { get; set; }

        [JsonProperty("Name")]
        public string Name { get; set; }

        [JsonProperty("BirthDate")]
        public DateTime BirthDate { get; set; }

        [JsonProperty("IsYoungDriver")]
        public bool IsYoungDriver { get; set; }

        [JsonProperty("SalesCount")]
        public int SalesCount { get; set; }
    }
}
