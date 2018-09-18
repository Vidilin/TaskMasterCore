using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskMasterCore.DAL.Models;

namespace TaskMasterCore.Web.Models.ViewModels
{
    public class RequestNav
    {
        public int Id { get; set; }
        public int? ParentId { get; set; }
        public string Name { get; set; }

        public RequestNav (Request req)
        {
            Name = req.Name;
            Id = req.Id;
            ParentId = req.ParentId;
        }
    }
}
