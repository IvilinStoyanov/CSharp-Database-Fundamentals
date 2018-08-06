using CarDealer.App.Core.Contracts;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CarDealer.App.Core.Export
{
    public class JsonExporter : IExporter
    {
        public void Export<TModel>(string filePath, TModel[] collection)
        {
            var jsonString = JsonConvert.SerializeObject(collection, Formatting.Indented);
            File.WriteAllText(filePath, jsonString);
        }

        public void Export<TModel>(string filePath, TModel model)
        {
            var jsonString = JsonConvert.SerializeObject(model, Formatting.Indented);
            File.WriteAllText(filePath, jsonString);
        }
    }
}
