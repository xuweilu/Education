using Education.Abstract;
using Education.Concrete;
using Education.Models;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Education.Infrastructure
{
    public class NinjectControllerFactory : DefaultControllerFactory
    {
        private IKernel ninjectKernel;
        public NinjectControllerFactory()
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
            ninjectKernel.Bind<IEntityRepository<Paper>>().To<EntityRepository<Paper>>();
            ninjectKernel.Bind<IEntityRepository<Exam>>().To<EntityRepository<Exam>>();
            ninjectKernel.Bind<IEntityRepository<ChoiceQuestion>>().To<EntityRepository<ChoiceQuestion>>();
            ninjectKernel.Bind<IEntityRepository<TrueOrFalseQuestion>>().To<EntityRepository<TrueOrFalseQuestion>>();
            ninjectKernel.Bind<IEntityRepository<Option>>().To<EntityRepository<Option>>();
            ninjectKernel.Bind<IEntityRepository<Exam>>().To<EntityRepository<Exam>>();
        }
    }
}