using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KodlaTv.Entities.ValueObjects
{
    public class PasswordViewModel
    {
        [DisplayName("E-Mail"),
         Required(ErrorMessage = "{0} alanı boş geçilemez."),
         StringLength(100, ErrorMessage = "{0} max. {1} karakter olmalı."),
         EmailAddress(ErrorMessage = "{0} alanı için geçerli bir e-posta adresi giriniz.")]
        public string Email { get; set; }
    }
}
