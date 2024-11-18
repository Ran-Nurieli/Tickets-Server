using Microsoft.EntityFrameworkCore;
using Tickets_Server.Models;

namespace Tickets_Server
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddControllers();


            // Add services to the container.

            // Add services to the container.

            builder.Services.AddControllers();

            #region Add Database context to Dependency Injection
            //Read connection string from app settings.json
            string connectionString = builder.Configuration
                .GetSection("ConnectionStrings")
                .GetSection("TicketsServerDB").Value;

            //Add Database to dependency injection
            builder.Services.AddDbContext<TicketsServerDBContext>(
                    options => options.UseSqlServer(connectionString));

            #endregion 

            #region Add Session
            builder.Services.AddDistributedMemoryCache();
            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(20);
                options.Cookie.HttpOnly = false;
                options.Cookie.IsEssential = true;
            });
            #endregion

            #region for debugginh UI
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            #endregion

            var app = builder.Build();

            #region for debugginh UI
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            #endregion


            


            // Configure the HTTP request pipeline.

            app.UseHttpsRedirection();

            app.UseAuthorization();

            #region Add Session
            app.UseSession(); //In order to enable session management
            #endregion 

            app.UseStaticFiles(); //Support static files delivery from wwwroot folder
            app.MapControllers(); //Map all controllers classes

            app.MapControllers();

            app.Run();
        }
    }
}
