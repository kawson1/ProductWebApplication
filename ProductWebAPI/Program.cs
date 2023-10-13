using Microsoft.EntityFrameworkCore;
using ProductWebAPI.Models;
using ProductWebAPI.Services;

namespace ProductWebAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddDbContext<ProductDBContext>(options => options.UseInMemoryDatabase("Products"));
            builder.Services.AddScoped<IProductRepository, ProductRepository>();
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();

            // Add services to the container.
            builder.Services.AddAuthorization();

            var app = builder.Build();

            /*            app.UseRouting();
            */

            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                var context = services.GetRequiredService<ProductDBContext>();
                context.Database.EnsureCreated();
                ProductDBContext.Initialize(context);
            }

            app.UseRouting();
            app.UseAuthorization();
            app.MapControllers();

            /*            app.UseEndpoints(endpoints =>
                        {
                            endpoints.MapControllers();
                        });*/

            app.Run();
        }
    }
}