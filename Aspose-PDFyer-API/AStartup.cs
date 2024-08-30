using AsposeTriage.Services;

namespace AsposeTriage
{
    public class AStartup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            // Add services to the container.
            services.AddControllers();
            services.AddTransient<IPDFGenerator, PDFGenerator>();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
            services.AddCors(options =>
            {
                options.AddPolicy("MyPolicy", builder =>
                {
                    builder.AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowAnyOrigin();
                });
            }
            );
            services.AddAWSLambdaHosting(LambdaEventSource.HttpApi);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment() || env.IsStaging())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseRouting();
            app.UseCors("MyPolicy");
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
