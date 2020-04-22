using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Model
{
    public class SYS_PLACEINFO
    {
        public string ID { get; set; }
        [Required]
        [StringLength(400, ErrorMessage = "{0} length must be between {2} and {1}.", MinimumLength = 3)]
        public string ClubName { get; set; }
        [Required]
        [StringLength(255, ErrorMessage = "{0} length must be between {2} and {1}.", MinimumLength = 3)]
        public string PlaceName { get; set; }
        [Required]
        [StringLength(4000, ErrorMessage = "{0} length must be between {2} and {1}.", MinimumLength = 3)]
        public string PlaceDesc { get; set; }
        [Required]
        public DateTime FromDate { get; set; }
        [Required]
        public DateTime ToDate { get; set; }
        [Required]
        public int Max { get; set; }
        public string Accpet { get; set; }
        [Required]
        public string Level { get; set; }
        public string Creator { get; set; }
        public string CreatorID { get; set; }
        public DateTime UpdateDate { get; set; }

        public SYS_PLACEINFO()
        {
            ID = "-1";
            PlaceName = "-1";
            PlaceDesc = "-1";
            FromDate = DateTime.Now;
            ToDate = DateTime.Now;
            Max = 0;
            Accpet = "Y";
            Level = "-1";
            Creator = "-1";
            CreatorID = "-1";
            UpdateDate = DateTime.Now;
        }
    }

    
}
