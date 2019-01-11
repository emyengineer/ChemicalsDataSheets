using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ChemicalsSheets.Startup))]
namespace ChemicalsSheets
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            //ConfigureAuth(app);
            app.MapSignalR();
        }
    }
}
