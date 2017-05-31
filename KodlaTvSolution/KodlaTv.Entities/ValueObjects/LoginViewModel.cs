using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace KodlaTv.Entities.ValueObjects
{
    public class LoginViewModel
    {
        [DisplayName("E-Mail"), Required(ErrorMessage = "{0} alanı boş geçilemez."), StringLength(50, ErrorMessage = "{0} max. {1} karakter olmalı.")]
        public string Email { get; set; }
        [DisplayName("Şifre"), Required(ErrorMessage = "{0} alanı boş geçilemez."), DataType(DataType.Password), StringLength(25, ErrorMessage = "{0} max. {1} karakter olmalı.")]
        public string Password { get; set; }
    }
}