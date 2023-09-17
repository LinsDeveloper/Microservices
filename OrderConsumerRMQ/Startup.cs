
using Microsoft.OpenApi.Models;

namespace OrderConsumerRMQ
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




            services.AddControllers();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "OrderPublisherRMQ", Version = "v1" });
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
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Discount API V1");
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