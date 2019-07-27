using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace DockerRedisApp
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.Run(async (context) =>
            {
                int visits = GetVisits();
                await context.Response.WriteAsync($"Number of Visits is: {visits}");
            });
        }

        private int GetVisits()
        {
            try
            {
                ConnectionMultiplexer redis = ConnectionMultiplexer.Connect("192.168.99.100:6379");
                IDatabase db = redis.GetDatabase();

                int visits = 0;
                var stVal = db.StringGet("client_visits");

                if (stVal.TryParse(out visits))
                {
                    visits++;
                    db.StringSet("client_visits", visits.ToString());
                }

                return visits;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERROR: {ex.ToString()}");
                return 0;
            }
        }
    }
}
