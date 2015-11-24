using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BusinessLogic.Implementations;
using BusinessLogic.Interfaces;
using DAL;
using Ninject;

namespace AdvertizingAgency1
{
    public class NinjectControllerFactory:DefaultControllerFactory
    {
        private IKernel ninjectKernel;

        public NinjectControllerFactory()
        {
            ninjectKernel = new StandardKernel();
            AddBindings();
        }
        //извлекаем экземпляр контроллера для заданного контекста запроса и типа контроллера
        protected override IController GetControllerInstance(System.Web.Routing.RequestContext requestContext, Type controllerType)
        {
            return controllerType == null ? null : (IController) ninjectKernel.Get(controllerType);
        }
        //определяем все привязки
        private void AddBindings()
        {
            ninjectKernel.Bind<IPricesRepository>().To<EfPricesRepository>();
            ninjectKernel.Bind<IAdresRepository>().To<EfAdresRepository>();
            ninjectKernel.Bind<IClientsRepository>().To<EfClientsRepository>();
            ninjectKernel.Bind<IContaktnoeLicoRepository>().To<EfContaktnoeLicoRepository>();
            ninjectKernel.Bind<IDogovorRepository>().To<EfDogovorRepository>();
            ninjectKernel.Bind<IZakazyAdresyRepository>().To<EfZakazyAdresyRepository>();
            ninjectKernel.Bind<IZakazyRepository>().To<EfZakazyRepository>();
            ninjectKernel.Bind<IUsersRepository>().To<EfUsersRepository>();
            ninjectKernel.Bind<IRazrabotkiRepository>().To<EfRazrabotkiRepozitory>();
            //подключим контекст базы данных на саму себя
            ninjectKernel.Bind<AdvertizingAgency1Entities>()
                .ToSelf()
                .WithConstructorArgument("connectionString", ConfigurationManager.ConnectionStrings[0].ConnectionString);
        }
    }
}