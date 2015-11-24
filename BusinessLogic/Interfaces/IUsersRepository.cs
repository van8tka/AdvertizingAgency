using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;

namespace BusinessLogic.Interfaces
{
   public interface IUsersRepository
   {
       IEnumerable<Сотрудники> GetUsers();
       Сотрудники GetUsersById(int id);
       Сотрудники GetUsersByName(string name);
       void CreateUser(int idUser, string FIO, string Dolgnost, string Adres, string Phone, string Email);
       void DeleteUser(Сотрудники user);
       void SaveUser(Сотрудники user);
       IEnumerable<Администрирование> GetRegisterUser();
       void RegisterUser(int idUser, string Role, string Login, string Password);
       void DeleteRegisteUser(Администрирование user);
       void ChangeRegisterUser(int idUser, string Role, string Login, string Password);
   }
}
