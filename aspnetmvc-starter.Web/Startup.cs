using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Owin;
using Owin;
using aspnetmvc_starter.Web.App_Start;

[assembly: OwinStartup(typeof(aspnetmvc_starter.Web.Startup))]

namespace aspnetmvc_starter.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}