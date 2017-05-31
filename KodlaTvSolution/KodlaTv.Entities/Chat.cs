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
    [Table("Chats")]
    public class Chat
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        [Required]
        public DateTime CreatedOn { get; set; }
        [DisplayName("Mesaj"), StringLength(100)]
        public string Message { get; set; }
        public virtual KodlatvUser Owner { get; set; }
        public virtual Channel Channel { get; set; }
    }
}
