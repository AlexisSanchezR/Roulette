using Autofac;
using Microsoft.OpenApi.Models;
using Roulette.IoCContainer;
using Serilog;
using Serilog.Core;
using Serilog.Events;

namespace Roulette
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.IgnoreNullValues = true;
            });
            services.AddLogging(loggingBuilder => loggingBuilder.AddSerilog());
            services.AddMvc();
            services.AddHealthChecks();
            //services.AddSwaggerGen();

            // 🔹 Swagger configurado con header personalizado
            services.AddSwaggerGen(c =>
            {
                // Definición del header "User-Id"
                c.AddSecurityDefinition("User-Id", new OpenApiSecurityScheme
                {
                    Name = "User-Id",
                    Type = SecuritySchemeType.ApiKey,
                    In = ParameterLocation.Header,
                    Description = "User ID requerido en el header para realizar apuestas"
                });

                // Aplicamos un filtro para agregarlo a los endpoints
                c.OperationFilter<AddRequiredHeaderParameter>();
            });

        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.BuildContext(Configuration);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IHostApplicationLifetime appLifetime)
        {
            string SYSTEM_MANAGER_PATH = Environment.GetEnvironmentVariable("SYSTEM_MANAGER_PATH");

            var pathToContentRoot = AppDomain.CurrentDomain.BaseDirectory;
            IConfiguration Configuration = new ConfigurationBuilder()
                .SetBasePath(pathToContentRoot)
                .AddEnvironmentVariables()
                .Build();



            var level = LogEventLevel.Information;

            var levelSwitch = new LoggingLevelSwitch();
            levelSwitch.MinimumLevel = level;

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.ControlledBy(levelSwitch)
                .MinimumLevel.Override("System", LogEventLevel.Error)
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .MinimumLevel.Override("Microsoft.Hosting.Lifetime", LogEventLevel.Error)
                .WriteTo.Console(level, @"[{Level:u3}] {Message:lj}{NewLine}{Exception}")
                .CreateLogger();

            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();
            if (env.IsProduction() || env.IsStaging())
                app.UseExceptionHandler("/Error");

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseCors(x => x
              .AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader());
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHealthChecks("/check-status");
            });

            app.UseSwagger();
            app.UseSwaggerUI();
        }
    }
}
