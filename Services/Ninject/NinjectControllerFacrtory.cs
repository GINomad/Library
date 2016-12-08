using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Ninject;
using System.Web.Routing;
using LibraryApp.Infrastructure;
using LibraryApp.Services;

namespace LibraryApp.Services.Ninject
{
    public class NinjectControlerFactory : DefaultControllerFactory
    {
        private IKernel ninjectKernel;
        public NinjectControlerFactory()
        {
            ninjectKernel = new StandardKernel();
            AddBindings();
        }
        protected override IController GetControllerInstance(RequestContext requestContext, Type controllerType)
        {
            return controllerType == null
                ? null
                : (IController)ninjectKernel.Get(controllerType);

        }
        private void AddBindings()
        {
            string conn = @"Data Source =(Localdb)\v11.0;AttachDbFilename=""|DataDirectory|\Libr.mdf"";Integrated Security = True";
            ninjectKernel.Bind<IRepository>().To<SQLRepository>().WithConstructorArgument(conn);
        }

    }
}
