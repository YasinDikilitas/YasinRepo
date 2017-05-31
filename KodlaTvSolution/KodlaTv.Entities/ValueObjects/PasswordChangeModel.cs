using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KodlaTv.Entities.ValueObjects
{
    public class PasswordChangeModel
    {

        [DisplayName("Şifre"),
         Required(ErrorMessage = "{0} alanı boş geçilemez."),
         DataType(DataType.Password),
         StringLength(25, ErrorMessage = "Şifre {0} en az {2} karakter uzunluğunda olmalı.", MinimumLength = 8)
         RegularExpression("^((?=.*[A-Z])(?=.*\\d)(?=.*[a-z])|(?=.*[A-Z])(?=.*\\d)(?=.*[!@#$%&\\/=?_.-])|(?=.*[A-Z])(?=.*[a-z])(?=.*[!@#$%&\\/=?_.-])|(?=.*\\d)(?=.*[a-z])(?=.*[!@#$%&\\/=?_.-])).{7,15}$", ErrorMessage = "Şifre en az 1 küçük harf 1 büyük harf ve 1 rakam içermelidir.")]
        public string Password { get; set; }

        [DisplayName("Şifre Tekrar"),
            Required(ErrorMessage = "{0} alanı boş geçilemez."),
            DataType(DataType.Password),
            StringLength(25, ErrorMessage = "Şifre {0} en az {2} karakter uzunluğunda olmalı.", MinimumLength = 8),
            Compare("Password", ErrorMessage = "{0} ile {1} uyuşmuyor.")]
        public string RePassword { get; set; }

        public Guid PasswordGuid { get; set; }
    }
}
