using System;
using System.Collections.Generic;

namespace TaskMasterCore.DAL.Models.DbModels
{
    public partial class DbRequest
    {
        public DbRequest()
        {
            InverseParent = new HashSet<DbRequest>();
        }

        public int Id { get; set; }
        public int? ParentId { get; set; }
        public string Name { get; set; }
        public string Comment { get; set; }
        public string Performers { get; set; }
        public DateTime CreateDate { get; set; }
        public int Status { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime Deadline { get; set; }
        public DateTime StartDate { get; set; }
        public DbRequest Parent { get; set; }
        public ICollection<DbRequest> InverseParent { get; set; }
    }
}
