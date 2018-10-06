using System;
using System.Text;
using System.Linq;
using System.Collections.Generic;
using System.Xml.Linq;

using Newtonsoft.Json;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

using Instagraph.Data;
using Instagraph.Models;
using System.ComponentModel.DataAnnotations;
using Instagraph.DataProcessor.Dtos.Import;

namespace Instagraph.DataProcessor
{
    public class Deserializer
    {
        public static string ImportPictures(InstagraphContext context, string jsonString)
        {
            var deserializedPictures = JsonConvert.DeserializeObject<PictureDto[]>(jsonString);

            var sb = new StringBuilder();
            var pictureList = new List<Picture>();

            foreach (var deserializedPicture in deserializedPictures)
            {
                var isPathUnique = pictureList.Any(p => p.Path == deserializedPicture.Path);

                if(!IsValid(deserializedPicture) || isPathUnique)
                {
                   sb.AppendLine("Error: Invalid data.");
                    continue;
                }

                var picture = new Picture
                {
                    Path = deserializedPicture.Path,
                    Size = deserializedPicture.Size
                };

                pictureList.Add(picture);
                sb.AppendLine($"Successfully imported Picture {deserializedPicture.Path}.");
            }

            context.AddRange(pictureList);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportUsers(InstagraphContext context, string jsonString)
        {
            throw new NotImplementedException();
        }

        public static string ImportFollowers(InstagraphContext context, string jsonString)
        {
            throw new NotImplementedException();
        }

        public static string ImportPosts(InstagraphContext context, string xmlString)
        {
            throw new NotImplementedException();
        }

        public static string ImportComments(InstagraphContext context, string xmlString)
        {
            throw new NotImplementedException();
        }

        private static bool IsValid(object obj)
        {
            var validationContext = new System.ComponentModel.DataAnnotations.ValidationContext(obj);
            var validationResults = new List<ValidationResult>();

            var isValid = Validator.TryValidateObject(obj, validationContext, validationResults, true);
            return isValid;
        }
    }

   
}
