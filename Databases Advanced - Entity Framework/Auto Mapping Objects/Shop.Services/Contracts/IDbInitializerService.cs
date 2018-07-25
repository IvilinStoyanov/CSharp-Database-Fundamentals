using System;
using System.Collections.Generic;
using System.Text;

namespace Shop.Services.Contracts
{
    public interface IDbInitializerService
    {
        void InitializeDatabase();
    }
}
