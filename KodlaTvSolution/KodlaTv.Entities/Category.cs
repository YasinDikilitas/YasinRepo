using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KodlaTv.Entities
{
    [Table("Categories")]
    public class Category:MyEntityBase
    {
        [DisplayName("Başlık"), StringLength(150)]
        public string Coursetitle { get; set; }
        [DisplayName("Alt Başlık"), StringLength(150)]
        public string Coursesubcategory { get; set; }
        [DisplayName("Resim"), StringLength(150)]
        public string Imagefile { get; set; }
        [Required, StringLength(30)]
        public string ModifiedUser { get; set; }

        public virtual List<Video> Videos { get; set; }

        public Category()
        {

            Videos = new List<Video>();
        }
    }
}
