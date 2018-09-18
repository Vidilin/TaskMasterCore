using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace TaskMasterCore.DAL.Models
{
    public class Request
    {
        public int Id { get; set; }

        public int? ParentId { get; set; }

        [Required(ErrorMessage = "NameRequired")]
        [StringLength(256, ErrorMessage = "ErrLength")]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Display(Name = "Comment")]
        [DataType(DataType.MultilineText)]
        [StringLength(1024, ErrorMessage = "ErrLength")]
        public string Comment { get; set; }

        [Display(Name = "Performers")]
        [StringLength(256, ErrorMessage = "ErrLength")]
        public string Performers { get; set; }

        [Display(Name = "CreateDate")]
        [DataType(DataType.DateTime)]
        public DateTime CreateDate { get; set; }

        [Display(Name = "Status")]
        public Enums.Statuses Status { get; set; }

        [Display(Name = "StartDate")]
        [DataType(DataType.DateTime)]
        [Required(ErrorMessage = "DateRequired")]
        public DateTime StartDate { get; set; }

        [Display(Name = "EndDate")]
        [DataType(DataType.DateTime)]
        public DateTime? EndDate { get; set; }

        [Display(Name = "Deadline")]
        [DataType(DataType.DateTime)]
        [Required(ErrorMessage = "DateRequired")]
        public DateTime Deadline { get; set; }

        [Display(Name = "LaborIntensity")]
        public TimeSpan LaborIntensity
        {
            get => Deadline - StartDate;
        }

        [Display(Name = "ExecutionTime")]
        public TimeSpan? ExecutionTime
        {
            get => EndDate - StartDate;
        }

        [Display(Name = "ParentName")]
        public string ParentName { get; set; }

        [Display(Name = "ChildsNames")]
        public IList<string> ChildsNames { get; set; }
        //public IList<Request> SubReqests { get; set; }
    }
}
