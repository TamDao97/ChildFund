
using Microsoft.Owin;

using Owin;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Http;

[assembly: OwinStartup(typeof(SwipeSafe.Api.Startup))]
[assembly: log4net.Config.XmlConfigurator(ConfigFile = "Web.config", Watch = true)]
namespace SwipeSafe.Api
{
    /// <summary>
    /// This class will be fired once our server starts, notice the “assembly” attribute which states which class to fire on start-up.
    /// The “Configuration” method accepts parameter of type “IAppBuilder” this parameter will be supplied by the host at run-time.
    /// This “app” parameter is an interface which will be used to compose the application for our Owin server.
    /// The “HttpConfiguration” object is used to configure API routes, so we’ll pass this object to method “Register” in “WebApiConfig” class.
    /// </summary>
    public class Startup
    {
     
    }
}