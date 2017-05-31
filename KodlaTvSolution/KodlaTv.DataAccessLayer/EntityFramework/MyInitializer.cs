using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using KodlaTv.Entities;

namespace KodlaTv.DataAccessLayer.EntityFramework
{
    public class MyInitializer:CreateDatabaseIfNotExists<DatabaseContext>
    {
        protected override void Seed(DatabaseContext context)
        {
            KodlatvUser admin = new KodlatvUser()
            {
                Name = "Yasin",
                Surname = "Dikilitas",
                Email = "yasin@gmail.com",
                ActivateGuid = Guid.NewGuid(),
                IsActive = true,
                isAdmin = true,
                Username = "YasinDikilitas",
                Password = "123456",
                Imagefile="user.png",
                CreatedOn = DateTime.Now,
                ModifyDate = DateTime.Now.AddMinutes(5),
                ModifiedUser = "YasinDikilitas"

            };
            //Adding standart User...
            KodlatvUser standartuser = new KodlatvUser()
            {
                Name = "Yilmaz",
                Surname = "Dikilitas",
                Email = "yilmaz94@hotmail.com",
                ActivateGuid = Guid.NewGuid(),
                IsActive = true,
                isAdmin = false,
                Username = "YilmazDikilitas",
                Imagefile = "user.png",
                Password = "123456",
                CreatedOn = DateTime.Now,
                ModifyDate = DateTime.Now.AddMinutes(5),
                ModifiedUser = "YasinDikilitas"

            };
            context.KodlatvUsers.Add(admin);
            context.KodlatvUsers.Add(standartuser);

            context.SaveChanges();
        }
    }
}
