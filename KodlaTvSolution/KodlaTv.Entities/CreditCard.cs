using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KodlaTv.Entities
{
    [Table("CreditCards")]
    public class CreditCard
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        [StringLength(50)]
        public string CardName { get; set; }
        [StringLength(20)]
        public string CardNumber { get; set; }
        public int Cardlastmonth { get; set; }
        public int Cardlastyear { get; set; }
        public int Cardcvc { get; set; }
        public double Amount { get; set; }

        public virtual KodlatvUser Owner { get; set; }

    }
}
