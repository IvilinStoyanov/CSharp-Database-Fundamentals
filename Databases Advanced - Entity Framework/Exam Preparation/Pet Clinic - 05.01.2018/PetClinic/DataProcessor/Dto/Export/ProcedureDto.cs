﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace PetClinic.DataProcessor.Dto.Export
{
   [XmlType("Procedure")]
   public class ProcedureDto
    {
        public string Passport { get; set; }

        public string OwnerNumber { get; set; }

        public string DateTime { get; set; }

        public AnimalExportDto[] AnimalAids { get; set; }

        public decimal TotalPrice { get; set; }
    }
}