using ASP.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ASP.Models
{
    public class SchedulersModel
    {
        [Key]
        public int id { get; set; }
        [Required]
        [Display(Name = "Name")]
        [MaxLength(50)]
        public string Name { get; set; }
        [Required]
        [DataType(DataType.MultilineText)]
        [MaxLength(2000)]
        [Display(Name = "Description")]
        public string description { get; set; }
        [Required]
        [DataType(DataType.DateTime)]
        [Display(Name = "End Time")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
        public System.DateTime EndDate { get; set; }

        [DataType(DataType.DateTime)]
        [Display(Name = "Add On")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
        public System.DateTime AddOn { get; set; } = DateTime.Now;
        public int userId { get; set; }
        public virtual AccountsModel Account { get; set; }
    }
}