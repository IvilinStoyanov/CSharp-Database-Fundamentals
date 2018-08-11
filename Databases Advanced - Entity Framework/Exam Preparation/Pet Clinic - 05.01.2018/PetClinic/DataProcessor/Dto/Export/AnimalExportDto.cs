using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace PetClinic.DataProcessor.Dto.Export
{
    [XmlType("AnimalAid")]
    public class AnimalExportDto
    {
        public string Name { get; set; }

        public decimal Price { get; set; }
    }
}
