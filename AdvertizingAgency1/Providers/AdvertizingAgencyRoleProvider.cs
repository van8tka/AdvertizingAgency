using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Web;
using System.Web.Security;
using BusinessLogic;
using DAL;

namespace AdvertizingAgency1.Providers
{
    public class AdvertizingAgencyRoleProvider:RoleProvider
    {
       
      
        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override string ApplicationName
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public override void CreateRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            throw new NotImplementedException();
        }

        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            throw new NotImplementedException();
        }

        public override string[] GetAllRoles()
        {
            throw new NotImplementedException();
        }

                                public override string[] GetRolesForUser(string login)
                                {
                                   string[] role = new string[]{};
                                    using (AdvertizingAgency1Entities dbcontext = new AdvertizingAgency1Entities())
                                    {
                                           try
                                           {
                                               Администрирование user = (from u in dbcontext.Администрирование
                                                   where u.Логин == login
                                                   select u).FirstOrDefault();
                                                   
                                               if (user != null)
                                               {
                                                   //получаем роль
                                                   role = new  string[]{user.Группа_доступа};
                                               }
                                           }
                                            catch
                                           { role = new string[]{};}
                                        }
                                    return role;
                                }
        

        public override string[] GetUsersInRole(string roleName)
        {
            throw new NotImplementedException();
        }

                            public override bool IsUserInRole(string username, string roleName)
                            {
                                bool outputResult = false;
                                using (AdvertizingAgency1Entities dbcontext = new AdvertizingAgency1Entities())
                                {
                                    try
                                    {
                                        Администрирование user = (from u in dbcontext.Администрирование
                                            where u.Логин == username
                                            select u).FirstOrDefault();
                                        if (user != null)
                                        {
                                            if (user.Группа_доступа != null && user.Группа_доступа == roleName)
                                            {
                                                outputResult = true;
                                            }
                                        }
                                    }
                                    catch
                                    {
                                        outputResult = false;
                                    }
                                }
                                return outputResult;
                            }

        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override bool RoleExists(string roleName)
        {
            throw new NotImplementedException();
        }
    }
}