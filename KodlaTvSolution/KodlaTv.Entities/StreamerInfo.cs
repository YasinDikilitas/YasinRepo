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
    [Table("StreamerInfos")]
    public class StreamerInfo
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        [DisplayName("İsim"), StringLength(30)]
        public string Name { get; set; }
        [DisplayName("Soyad"), StringLength(30)]
        public string Surname { get; set; }
        [DisplayName("İlgiler"), StringLength(200)]
        public string Interest { get; set; }
        [DisplayName("Deneyimler"), StringLength(200)]
        public string Experince { get; set; }
        [DisplayName("Kullanılan İşletim Sistemleri"), StringLength(200)]
        public string Usingos { get; set; }
        [DisplayName("Hobiler"), StringLength(200)]
        public string Hobby { get; set; }


        public virtual KodlatvUser Owner { get; set; }
    }
}
