using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KodlaTv.Entities
{
    [Table("WebsiteInfos")]
    public class WebsiteInfo
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        [StringLength(100)]
        public string Webname { get; set; }
        [StringLength(100)]
        public string Slogana { get; set; }
        [StringLength(100)]
        public string Sloganb { get; set; }
        [StringLength(100)]
        public string Sloganc { get; set; }
    }
}
