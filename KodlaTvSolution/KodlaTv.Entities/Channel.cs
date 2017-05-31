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
    [Table("Channels")]
    public class Channel:MyEntityBase
    {
        [DisplayName("Kanal Adı"), StringLength(50)]
        public string ChannelName { get; set; }
        [DisplayName("Yayın Durumu")]
        public bool StreamStatus { get; set; }

        [DisplayName("Resim"), StringLength(150)]
        public string Imagefile { get; set; }
        [DisplayName("Şikayet")]
        public int complain { get; set; }


        public virtual KodlatvUser Owner { get; set; }
        public virtual List<Video> Videos { get; set; }
        public virtual List<Subscribe> Subscribes { get; set; }
        public virtual List<Follow> Follows { get; set; }

        public Channel()
        {

            Videos = new List<Video>();

            Follows = new List<Follow>();
        }
    }
}
