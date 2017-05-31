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
    [Table("Videos")]
    public class Video : MyEntityBase
    {
        [DisplayName("Yayın Bilgi"), StringLength(150)]
        public string Videoinfo { get; set; }
        [DisplayName("Youtube URL"), StringLength(500), Required]
        public string Youtubeurl { get; set; }
        [DisplayName("Seviye"), StringLength(50)]
        public string Levelofvideo { get; set; }
        [DisplayName("İzlenme")]
        public int Watchnumber { get; set; }
        [DisplayName("Beğenme")]
        public int Likenumber { get; set; }
        public virtual Channel Channel { get; set; }
        public virtual Category Category { get; set; }
    }
}
