using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Model
{
    public class SYS_BOOKINFO
    {
        public string ID { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string NickName { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        [Phone]
        public string Phone { get; set; }
        public string Sex { get; set; }
        [Required]
        public string PlaceID { get; set; }
        [Required]
        public int PeopleCount { get; set; }
        public string Creator { get; set; }
        public string CreatorID { get; set; }
        public DateTime UpdateDate { get; set; }

        public SYS_BOOKINFO (){
            ID = "";
            Name = "-1";
            NickName = "";
            Email = "";
            Phone = "";
            Sex = "";
            PlaceID = "";
            PeopleCount = 1;
            Creator = "-1";
            CreatorID = "-1";
            UpdateDate = DateTime.Now;
        }
    }
}
