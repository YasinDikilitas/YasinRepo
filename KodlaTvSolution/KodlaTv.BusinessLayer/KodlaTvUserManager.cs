using KodlaTv.BusinessLayer.Abstract;
using KodlaTv.Common;
using KodlaTv.Common.Helpers;
using KodlaTv.DataAccessLayer.EntityFramework;
using KodlaTv.Entities;
using KodlaTv.Entities.Messages;
using KodlaTv.Entities.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KodlaTv.BusinessLayer
{
    public class KodlaTvUserManager : ManagerBase<KodlatvUser>
    {

        public BusinessLayerResult<KodlatvUser> RegisterUser(RegisterViewModel data)
        {
            KodlatvUser user = Find(x => x.Username == data.Username || x.Email == data.Email);
            BusinessLayerResult<KodlatvUser> layerResult = new BusinessLayerResult<KodlatvUser>();

            if (user != null)
            {
                if (user.Username == data.Username)
                {
                    layerResult.AddError(ErrorMessageCode.PaymentNotFound, "Kullanıcı adı kayıtlı.");
                }

                if (user.Email == data.Email)
                {
                    layerResult.AddError(ErrorMessageCode.EmailAlreadyExist, "Email adresi kayıtlı.");
                }
            }
            else
            {
                int dbResult = base.Insert(new KodlatvUser()
                {
                    Name=data.Name,
                    Surname=data.Surname,
                    Username = data.Username,
                    Email = data.Email,
                    Password = data.Password,
                    Imagefile="user.png",
                    ActivateGuid = Guid.NewGuid(),
                    ModifiedUser=App.Common.GetCurrentUsername(),
                    IsActive = false,
                    isAdmin = false

                });
                if (dbResult > 0)
                {
                    layerResult.Result =Find(x => x.Email == data.Email && x.Username == data.Username);

                    string siteUri = ConfigHelper.Get<string>("SiteRootUri");
                    string activateUri = $"{siteUri}/Home/UserActivate/{layerResult.Result.ActivateGuid}";
                    string body = $"Merhaba {layerResult.Result.Username};<br><br>Hesabınızı aktifleştirmek için <a href='{activateUri}' target='_blank'>tıklayınız</a>.";

                    MailHelper.SendMail(body, layerResult.Result.Email, "Kodla TV Hesap Aktifleştirme");


                }

            }
            return layerResult;
        }


        public BusinessLayerResult<KodlatvUser> LoginUser(LoginViewModel data)
        {
            BusinessLayerResult<KodlatvUser> layerResult = new BusinessLayerResult<KodlatvUser>();
            layerResult.Result = Find(x => x.Email == data.Email && x.Password == data.Password);

            if (layerResult.Result != null)
            {
                if (!layerResult.Result.IsActive)
                {
                    layerResult.AddError(ErrorMessageCode.UserIsNotActive, "Kullanıcı aktifleştirilmemiştir.");
                    layerResult.AddError(ErrorMessageCode.CheckYourEmail, "Lütfen email adresinizi kontrol ediniz.");
                }
            }
            else
            {
                layerResult.AddError(ErrorMessageCode.UsernameOrPassWrong, "Kullanıcı adı ya da şifre hatalı");
            }
            return layerResult;
        }


        public BusinessLayerResult<KodlatvUser> ChangePasswordUser(PasswordViewModel data)
        {
            KodlatvUser user = Find(x => x.Email == data.Email);
            BusinessLayerResult<KodlatvUser> layerResult = new BusinessLayerResult<KodlatvUser>();

            if (user != null)
            {
                if (user.Email == data.Email)
                {
                    layerResult.Result = Find(x => x.Email==data.Email);

                    string siteUri = ConfigHelper.Get<string>("SiteRootUri");
                    string activateUri = $"{siteUri}/Other/ChangePassword/{layerResult.Result.ActivateGuid}";
                    string body = $"Merhaba {layerResult.Result.Username};<br><br>Şifrenizi değiştirmek için <a href='{activateUri}' target='_blank'>tıklayınız</a>.";

                    MailHelper.SendMail(body, layerResult.Result.Email, "Kodla TV Şifre Değiştirme");
                }
            }
            
            return layerResult;
        }


        public BusinessLayerResult<KodlatvUser> ActivateChangePassword(Guid activateId)
        {
            BusinessLayerResult<KodlatvUser> res = new BusinessLayerResult<KodlatvUser>();
            res.Result = Find(x => x.ActivateGuid == activateId);

            if (res.Result != null)
            {
                if (res.Result.IsActive)
                {
                    res.AddError(ErrorMessageCode.UserAlreadyActive, "Şifre değiştirme işlemi zaten yapılmıştır.");
                    return res;
                }

                res.Result.IsActive = true;
                Uptade(res.Result);
            }
            else
            {
                res.AddError(ErrorMessageCode.ActivateIdDoesNotExists, "Şifre değiştirilecek kullanıcı bulunamadı.");
            }

            return res;
        }



        public BusinessLayerResult<KodlatvUser> ActivateUser(Guid activateId)
        {
            BusinessLayerResult<KodlatvUser> res = new BusinessLayerResult<KodlatvUser>();
            res.Result = Find(x => x.ActivateGuid == activateId);

            if (res.Result != null)
            {
                if (res.Result.IsActive)
                {
                    res.AddError(ErrorMessageCode.UserAlreadyActive, "Kullanıcı zaten aktif edilmiştir.");
                    return res;
                }

                res.Result.IsActive = true;
                Uptade(res.Result);
            }
            else
            {
                res.AddError(ErrorMessageCode.ActivateIdDoesNotExists, "Aktifleştirilecek kullanıcı bulunamadı.");
            }

            return res;
        }

        public BusinessLayerResult<KodlatvUser> GetUserById(int id)
        {
            BusinessLayerResult<KodlatvUser> res = new BusinessLayerResult<KodlatvUser>();
            res.Result = Find(x => x.id == id);

            if (res.Result == null)
            {
                res.AddError(ErrorMessageCode.UserNotFound, "Kullanıcı bulunamadı.");
            }

            return res;
        }

        public BusinessLayerResult<KodlatvUser> UpdateProfile(KodlatvUser data)
        {
            KodlatvUser db_user = Find(x => x.Username == data.Username || x.Email == data.Email);
            BusinessLayerResult<KodlatvUser> res = new BusinessLayerResult<KodlatvUser>();

            if (db_user != null && db_user.id != data.id)
            {
                if (db_user.Username == data.Username)
                {
                    res.AddError(ErrorMessageCode.PaymentNotFound, "Kullanıcı adı kayıtlı.");
                }

                if (db_user.Email == data.Email)
                {
                    res.AddError(ErrorMessageCode.EmailAlreadyExist, "E-posta adresi kayıtlı.");
                }

                return res;
            }

            res.Result = Find(x => x.id == data.id);
            res.Result.Email = data.Email;
            res.Result.Name = data.Name;
            res.Result.Surname = data.Surname;
            res.Result.Password = data.Password;
            res.Result.Username = data.Username;
            res.Result.ModifiedUser = App.Common.GetCurrentUsername();

            if (string.IsNullOrEmpty(data.Imagefile) == false)
            {
                res.Result.Imagefile = data.Imagefile;
            }

            if (Uptade(res.Result) == 0)
            {
                res.AddError(ErrorMessageCode.ProfileCouldNotUpdated, "Profil güncellenemedi.");
            }

            return res;
        }
        public BusinessLayerResult<KodlatvUser> RemoveUserById(int id)
        {
            BusinessLayerResult<KodlatvUser> res = new BusinessLayerResult<KodlatvUser>();
            KodlatvUser user = Find(x => x.id == id);

            if (user != null)
            {
                if (Delete(user) == 0)
                {
                    res.AddError(ErrorMessageCode.UserCouldNotRemove, "Kullanıcı silinemedi.");
                    return res;
                }
            }
            else
            {
                res.AddError(ErrorMessageCode.UserCouldNotFind, "Kullanıcı bulunamadı.");
            }

            return res;
        }


        public new BusinessLayerResult<KodlatvUser> Insert(KodlatvUser data)
        {
            KodlatvUser user = Find(x => x.Username == data.Username || x.Email == data.Email);
            BusinessLayerResult<KodlatvUser> res = new BusinessLayerResult<KodlatvUser>();

            res.Result = data;

            if (user != null)
            {
                if (user.Username == data.Username)
                {
                    res.AddError(ErrorMessageCode.PaymentNotFound, "Kullanıcı adı kayıtlı.");
                }

                if (user.Email == data.Email)
                {
                    res.AddError(ErrorMessageCode.EmailAlreadyExist, "E-posta adresi kayıtlı.");
                }
            }
            else
            {
                res.Result.Imagefile = "user_boy.png";
                res.Result.ActivateGuid = Guid.NewGuid();
                res.Result.ModifiedUser = App.Common.GetCurrentUsername();

                if (base.Insert(res.Result) == 0)
                {
                    res.AddError(ErrorMessageCode.UserCouldNotFind, "Kullanıcı eklenemedi.");
                }
            }

            return res;
        }

        public new BusinessLayerResult<KodlatvUser> Update(KodlatvUser data)
        {
            KodlatvUser db_user = Find(x => x.Username == data.Username || x.Email == data.Email);
            BusinessLayerResult<KodlatvUser> res = new BusinessLayerResult<KodlatvUser>();
            res.Result = data;

            if (db_user != null && db_user.id != data.id)
            {
                if (db_user.Username == data.Username)
                {
                    res.AddError(ErrorMessageCode.PaymentNotFound, "Kullanıcı adı kayıtlı.");
                }

                if (db_user.Email == data.Email)
                {
                    res.AddError(ErrorMessageCode.EmailAlreadyExist, "E-posta adresi kayıtlı.");
                }

                return res;
            }

            res.Result = Find(x => x.id == data.id);
            res.Result.Email = data.Email;
            res.Result.Name = data.Name;
            res.Result.Surname = data.Surname;
            res.Result.Password = data.Password;
            res.Result.Username = data.Username;
            res.Result.IsActive = data.IsActive;
            res.Result.isAdmin = data.isAdmin;
            res.Result.ModifiedUser = App.Common.GetCurrentUsername();

            if (base.Uptade(res.Result) == 0)
            {
                res.AddError(ErrorMessageCode.UserCouldNotUpdated, "Kullanıcı güncellenemedi.");
            }

            return res;
        }
    }
}
