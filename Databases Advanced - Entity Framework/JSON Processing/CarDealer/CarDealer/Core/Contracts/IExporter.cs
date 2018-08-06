using System;
using System.Collections.Generic;
using System.Text;

namespace CarDealer.App.Core.Contracts
{
    public interface IExporter
    {
        void Export<TModel>(string filePath, TModel[] collection);

        void Export<TModel>(string filePath, TModel model);
    }
}
