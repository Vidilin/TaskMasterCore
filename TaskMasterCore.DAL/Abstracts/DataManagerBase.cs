using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using TaskMasterCore.DAL;

namespace TaskMasterCore.DAL.Abstracts
{
    public abstract class DataManagerBase
    {
        protected DbContext GetConnect(string connectionString)
        {
            var optionsBuilder = new DbContextOptionsBuilder<DbContext>();
            var options = optionsBuilder
                .UseSqlServer(connectionString)
                .Options;
            return new DbContext(options);
        }
    }
}
