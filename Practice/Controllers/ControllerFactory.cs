using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Practice.Models;

namespace Practice.Controllers
{
    public class ControllerFactory : DefaultControllerFactory
    {
        protected override IController GetControllerInstance(System.Web.Routing.RequestContext request,
            Type controllertype)
        {
            return Activator.CreateInstance(controllertype, new DataManager()) as IController;
        }
    }
}