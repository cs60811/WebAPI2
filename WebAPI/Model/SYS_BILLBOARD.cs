using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Model
{
    public class SYS_BILLBOARD
    {
        public string ID { get; set; }
        [Required]
        [StringLength(400, ErrorMessage = "{0} length must be between {2} and {1}.", MinimumLength = 3)]
        [Display(Name = "內容")]
        public string Title { get; set; }
        [StringLength(2000, ErrorMessage = "{0} length must be between {2} and {1}.", MinimumLength = 0)]
        [Display(Name = "內容")]
        public string Content { get; set; }
        public string Tag { get; set; }
        [Required]
        public string Importance { get; set; }
        [Required]
        public string Daft { get; set; }
        [Required]
        public DateTime StartDate { get; set; }
        [Required]
        public DateTime EndDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public string Creator { get; set; }
        public string CreatorID { get; set; }
        public string Archive { get; set; }
        public SYS_BILLBOARD()
        {
            ID = "";
            Title = "";
            Content = "";
            Tag = "";
            Importance = "Normal";
            Daft = "Y";
            StartDate = DateTime.Now;
            EndDate = DateTime.Now;
            UpdateDate = DateTime.Now;
            Creator = "-1";
            CreatorID = "-1";
            Archive = "N";
        }
    }
}
