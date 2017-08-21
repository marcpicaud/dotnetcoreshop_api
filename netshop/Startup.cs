using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;
using netshop.Models;
using Microsoft.AspNetCore.Identity;

namespace netshop
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
			services.AddDbContext<NetshopDbContext>(opt => opt.UseInMemoryDatabase());

            // TODO : authentication

			services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, NetshopDbContext context)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            addMockData(context);

            app.UseMvc();
        }

        private static void addMockData(NetshopDbContext context)
        {
            // Mock users
			context.Users.Add(new User { Login = "admin", Password = "admin", Firstname = "Marc", Lastname = "Picaud", Admin = true, Token = "admin" });
			context.Users.Add(new User { Login = "client", Password = "client", Firstname = "Jean", Lastname = "Dupont", Admin = false, Token="client" });

            // Mock categories
			context.Categories.Add(new Category { Name = "category 1" });
			context.Categories.Add(new Category { Name = "category 2" });

			// Mock products
			context.Products.Add(new Product { Name = "product 1", Price = 4.2 });
			context.Products.Add(new Product { Name = "product 2", Price = 8.4 });

            // Mock carts
            context.Carts.Add(new Cart { UserId = 1, ProductIds = "1,2" });
            context.Carts.Add(new Cart { UserId = 2, ProductIds = "1" });

            // Mock orders
            // TODO

            context.SaveChanges();
        }
    }
}
