using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projekt.Class
{
    using DAL.Entity;
    using Projekt.DAL.Repositories;

    class NewOwner
    {
        public NewOwner(string typ, params string[] data)
        {
            Wlasciciel owner = new Wlasciciel(data);
            string accountNumber = Methods.AccountNumberGenerator();
            while (RepositoryKonto.NumberExist(accountNumber) != true)
            {
                accountNumber = Methods.AccountNumberGenerator();
            }
            Konto account = new Konto(accountNumber, owner.Pesel, typ);
            RepositoryWlasciciel.CreateOwner(owner);
            RepositoryKonto.AddAccount(account);
        }
    }
}
