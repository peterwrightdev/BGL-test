using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.SqlServer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using BGLOrdersAPI.DataContexts;
using BGLOrdersAPI.Models;
using BGLOrdersAPI.Reports;
using BGLOrdersAPI.Services;
using Microsoft.EntityFrameworkCore;

namespace BGLOrdersAPI
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
            // Connection string should be configurable, but this is fast for now.
            services.AddDbContext<BGLContext>(options => options.UseSqlServer("Data Source=localhost;Initial Catalog=BGLOrdersDB;Integrated Security=True"));
            services.AddScoped<IBGLContext>(provider => provider.GetService<BGLContext>());
            services.AddSingleton(new DateTimeService());
            services.AddSingleton<ILogicContext<Item, ItemReport>>(new ItemLogicContext(new BGLContext(), new DateTimeService()));
            services.AddSingleton<ILogicContext<Order, OrderReport>>(new OrderLogicContext(new BGLContext(), new DateTimeService()));
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
