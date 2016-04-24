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
            //Mock<IPaperRepository> mock = new Mock<IPaperRepository>();
            //mock.Setup(m => m.Papers).Returns(new List<Paper>
            //{
            //    new Paper { Teacher = new Teacher { TrueName = "xuweilu"} }
            //}.AsQueryable());
            ninjectKernel.Bind<IEntityRepository<Paper>>().To<EntityRepository<Paper>>();
        }
    }
}