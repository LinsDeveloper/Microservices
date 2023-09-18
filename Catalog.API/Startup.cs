using Catalog.API.Data;
using Catalog.API.Repositories;
using Microsoft.OpenApi.Models;

namespace Catalog.API
{
    public class Startup
    {
        // Configurações obtidas do appsettings.json e outros sources
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // Este método é usado para adicionar serviços ao contêiner.
        public void ConfigureServices(IServiceCollection services)
        {
            // Aqui você pode adicionar suas dependências
            services.AddScoped<ICatalogContext, CatalogContext>();
            services.AddScoped<IProductRepository, ProductRepository>();
            // services.AddTransient<IMinhaInterface, MinhaImplementacao>();
            // services.AddSingleton<IMinhaInterface, MinhaImplementacao>();

            services.AddControllers();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Catalog.API", Version = "v1" });
            });
        }

        // Este método é usado para configurar o pipeline de requisição HTTP.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Catalog API V1");
                });

            }
            

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}