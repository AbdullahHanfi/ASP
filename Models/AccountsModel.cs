using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ASP.Models
{
    public class AccountsModel
    {
        [Key]
        public int Id { get; set; }
        [MaxLength(50)]
        public string Name { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required]
        [MaxLength(64)]
        [MinLength(8)]
        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        public string password { get; set; }
        public virtual ICollection<SchedulersModel> Schedulers { get; set; }
    }
}