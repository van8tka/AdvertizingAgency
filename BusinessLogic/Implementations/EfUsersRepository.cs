using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLogic.Interfaces;
using DAL;

namespace BusinessLogic.Implementations
{
    public class EfUsersRepository:IUsersRepository
    {
        private AdvertizingAgency1Entities context;

        public EfUsersRepository(AdvertizingAgency1Entities context)
        {
            this.context = context;
        }
        public IEnumerable<Сотрудники> GetUsers()
        {
            return context.Сотрудники;
        }

        public Сотрудники GetUsersById(int id)
        {
            return context.Сотрудники.FirstOrDefault(x => x.Код_сотрудника == id);
        }

        public Сотрудники GetUsersByName(string name)
        {
            return context.Сотрудники.FirstOrDefault(x => x.ФИО_сотрудника == name);
        }

        public void CreateUser(int idUser, string FIO, string Dolgnost, string Adres, string Phone, string Email)
        {
            Сотрудники user = new Сотрудники
            {
                Код_сотрудника = idUser,
                ФИО_сотрудника = FIO,
                Должность = Dolgnost,
                Адрес = Adres,
                Телефон = Phone,
                Email = Email
            };
            SaveUser(user);
        }

        public void DeleteUser(Сотрудники user)
        {
            context.Сотрудники.Remove(user);
            context.SaveChanges();
        }

        public void SaveUser(Сотрудники user)
        {
            if (user.Код_сотрудника == 0)
            {
                context.Сотрудники.Add(user);
            }
            else
            {
                context.Entry(user).State = EntityState.Modified;
            }
            context.SaveChanges();
        }

        public IEnumerable<Администрирование> GetRegisterUser()
        {
            return context.Администрирование;
        }

        public void RegisterUser(int idUser, string Role, string Login, string Password)
        {
            Администрирование admin = new Администрирование
            {
                Код_сотрудника = idUser,
                Группа_доступа = Role,
                Логин = Login,
                Пароль = Password
            };
            context.Администрирование.Add(admin);
            context.SaveChanges();
        }

        public void DeleteRegisteUser(Администрирование user)
        {
            context.Администрирование.Remove(user);
            context.SaveChanges();
        }

        public void ChangeRegisterUser(int idUser, string Role, string Login, string Password)
        {
            Администрирование admin = new Администрирование
            {
                Код_сотрудника = idUser,
                Группа_доступа = Role,
                Логин = Login,
                Пароль = Password
            };
            DeleteRegisteUser(GetRegisterUser().Where(x=>x.Код_сотрудника==admin.Код_сотрудника).FirstOrDefault());
            context. Администрирование.Add(admin);
            context.SaveChanges();
        }
    }
}
