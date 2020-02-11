using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FcuCore.Communications;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FcuCore.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddFcu();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
            } else {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            /*
            app.UseMvc(routes => {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
                
            });*/
            app.UseMvc();

            app.UseWebSockets();

            app.Use(WebsocketHandler);
        }

        private async Task WebsocketHandler(HttpContext context, Func<Task> next)
        {
            if (context.Request.Path == "/ws") {
                if (context.WebSockets.IsWebSocketRequest) {
                    var webSocket = await context.WebSockets.AcceptWebSocketAsync();
                    var socketHandler = new WebsocketConnection();
                    await socketHandler.HandleConnection(context, webSocket);
                } else {
                    context.Response.StatusCode = 400;
                }
            } else {
                await next();
            }
        }

      
    }
}
