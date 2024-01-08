using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab.Gym.Web.Application.Models
{
    public class UserProfile
    {
        [StringLength(300)]
        public string FullName { get; set; }

        public DateTime BirthDate { get; set; }

        [StringLength(50)]
        public string Address1 { get; set; }

        [StringLength(50)]
        public string Address2 { get; set; }

        [StringLength(25)]
        public string City { get; set; }

        [StringLength(25)]
        public string County { get; set; }

        [StringLength(20)]
        public string PostalCode { get; set; }

        [StringLength(50)]
        public string Phone1 { get; set; }

        [StringLength(50)]
        public string Phone2 { get; set; }
    }
}
