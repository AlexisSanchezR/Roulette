using Autofac;
using Autofac.Extensions.DependencyInjection;
using Roulette.Bussines.Interfaces;
using Roulette.Bussines.Services;
using Roulette.Infrastructure.Client;
using Roulette.Infrastructure.Interfaces;
using Roulette.Infrastructure.Repositories;

namespace Roulette.IoCContainer
{
    public static class IoCServiceCollection
    {
        public static ContainerBuilder BuildContext(this IServiceCollection services, IConfiguration configuration)
        {
            var builder = new ContainerBuilder();
            builder.Populate(services);
            return BuildContext(builder, configuration);
        }

        public static ContainerBuilder BuildContext(this ContainerBuilder builder, IConfiguration configuration)
        {
            RegisterClients(builder, configuration);
            RegisterRepositories(builder, configuration);
            RegisterServices(builder, configuration);
            return builder;
        }

        private static void RegisterClients(ContainerBuilder builder, IConfiguration configuration)
        {
            builder
                .Register((context, parameters) =>
                {
                    var connectionString = configuration["ConnectionString"];
                    return new DBClient(connectionString);
                })
           .Named<IDBClient>("DBClient")
           .SingleInstance();

        }

        private static void RegisterRepositories(ContainerBuilder builder, IConfiguration configuration)
        {

            builder
                .Register((context, parameters) =>
                {
                    var dbClient = context.ResolveNamed<IDBClient>("DBClient");
                    return new DBRepository(dbClient);
                })
                .As<IDBRepository>()
                .SingleInstance();

        }

        private static void RegisterServices(ContainerBuilder builder, IConfiguration configuration)
        {
            builder
                .Register((context, parameters) => new UserService(
                    context.Resolve<IDBRepositoryEF>()
                    ))
                .As<IUserService>()
                .SingleInstance();
        }


    }
}
