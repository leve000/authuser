
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

namespace authuserAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddDbContext<Models.AuthuserContext>(options =>
                options.UseMySQL(builder.Configuration.GetConnectionString("MySql")));

            builder.Services.AddScoped<Services.PasswordHasher>();
            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapScalarApiReference();


            app.MapControllers();

            app.Run();
        }
    }
}
