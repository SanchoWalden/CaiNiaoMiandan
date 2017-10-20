using log4net.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using AutoMapper;
using MianDanService.AutoMapper;

using System.Data.Entity;
using Models.DataModel;

namespace MianDanService
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            //应用程序启动时，自动加载配置log4Net
            XmlConfigurator.Configure();
            AutoMapperConfig.RegisterMappings();
            Database.SetInitializer<MianDanContext>(new DropCreateDatabaseIfModelChanges<MianDanContext>());
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}
