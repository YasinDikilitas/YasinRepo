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
    [Table("KodlatvUsers")]
    public class KodlatvUser:MyEntityBase
    {
        [DisplayName("İsim"),StringLength(30)]
        public string Name { get; set; }
        [DisplayName("Soyadı"), StringLength(30)]
        public string Surname { get; set; }
        [DisplayName("Kullanıcı Adı"), Required, StringLength(30)]
        public string Username { get; set; }
        [DisplayName("Email"), Required, StringLength(100)]
        public string Email { get; set; }
        [DisplayName("Şifre"), Required, StringLength(25)]
        public string Password { get; set; }
        [DisplayName("Aktif")]
        public bool IsActive { get; set; }
        [Required, ScaffoldColumn(false)]
        public Guid ActivateGuid { get; set; }
        [StringLength(150), ScaffoldColumn(false)]
        public string Imagefile { get; set; }
        [DisplayName("Yönetici")]
        public bool isAdmin { get; set; }
        [Required, StringLength(30)]
        public string ModifiedUser { get; set; }


        public virtual List<Channel> Channels { get; set; }
        public virtual List<Subscribe> Subscribes { get; set; }
        public virtual List<Follow> Follows { get; set; }
        public virtual List<SendMessage> SendMessages { get; set; }
        public virtual List<StreamerInfo> StreamerInfos { get; set; }
        public virtual List<CreditCard> CreditCards { get; set; }


    }
}
