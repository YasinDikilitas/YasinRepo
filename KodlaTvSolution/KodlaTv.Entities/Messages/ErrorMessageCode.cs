using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KodlaTv.Entities.Messages
{
    public enum ErrorMessageCode
    {
        PaymentNotFound = 101,
        EmailAlreadyExist = 102,
        UserIsNotActive = 151,
        UsernameOrPassWrong = 152,
        CheckYourEmail = 153,
        UserAlreadyActive = 154,
        ActivateIdDoesNotExists = 155,
        UserNotFound = 156,
        ProfileCouldNotUpdated = 157,
        UserCouldNotRemove = 158,
        UserCouldNotFind = 159,
        UserCouldNotUpdated = 160,
        ChannelorCategoryNotExist = 161
    }
}
