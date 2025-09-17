using Autofac.Extensions.DependencyInjection;
using Roulette;

public class Program
{
    public static void Main(string[] args)
    {
        CreateHostBuilder(args).Build().Run();
    }

    private static IHostBuilder CreateHostBuilder(string[] args)
    {
        var pathToContentRoot = AppDomain.CurrentDomain.BaseDirectory;
        var configurationRoot = new ConfigurationBuilder()
            .SetBasePath(pathToContentRoot)
            .AddJsonFile("appsettings.json", true)
            .Build();

        return Host.CreateDefaultBuilder(args)

            //inyeccion de dependencias con Autofac
            .UseServiceProviderFactory(new AutofacServiceProviderFactory())
            .ConfigureServices(services => { services.AddAutofac(); })

            //Configuracion de la aplicacion
            .ConfigureAppConfiguration((hostingContext, config) =>
            {
                config.SetBasePath(pathToContentRoot);
                config.AddJsonFile("appsettings.json", true);
                config.AddConfiguration(configurationRoot);
            })

            //configuracion del servidor web
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder
                    .UseKestrel()
                .UseIISIntegration()
                    .UseStartup<Startup>();
            });
    }
}