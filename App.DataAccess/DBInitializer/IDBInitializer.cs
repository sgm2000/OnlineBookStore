using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.DataAccess.DBInitializer
{
    public interface IDBInitializer
    {
        Task InitializeAsync();// Responsible create Roles and User after production.
    }
}
