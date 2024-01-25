using Autofac;
using NLayer.Caching.Caches;
using NLayer.Caching.Redis.Repositories;
using NLayer.Core.Repositories;
using NLayer.Core.Services;
using NLayer.Core.UnitOfWorks;
using NLayer.Repository.Entityframework.Contexts.AppDbContexts;
using NLayer.Repository.Repositories;
using NLayer.Repository.UnitOfWorks;
using NLayer.Service.Mapping;
using NLayer.Service.Services;
using System.Reflection;
using Module = Autofac.Module;

namespace NLayer.API.Modules
{
    public class RepoServiceModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterGeneric(typeof(GenericRepository<>)).As(typeof(IGenericRepository<>)).InstancePerLifetimeScope();
            builder.RegisterGeneric(typeof(Service<>)).As(typeof(IService<>)).InstancePerLifetimeScope();


            builder.RegisterType<UnitOfWork>().As<IUnitOfWork>().InstancePerLifetimeScope();

            var apiAssembly = Assembly.GetExecutingAssembly();

            var repoAssembly = Assembly.GetAssembly(typeof(AppDbContext));

            var serviceAssembly = Assembly.GetAssembly(typeof(MapProfile));

            builder.RegisterAssemblyTypes(apiAssembly, repoAssembly, serviceAssembly).
                Where(x => x.Name.EndsWith("Repository")).AsImplementedInterfaces().InstancePerLifetimeScope();
            builder.RegisterAssemblyTypes(apiAssembly, repoAssembly, serviceAssembly).
                Where(x => x.Name.EndsWith("Service")).AsImplementedInterfaces().InstancePerLifetimeScope();

            //builder.RegisterType<ProductServiceWithCaching>().As<IProductService>();
            builder.RegisterType<ProductServiceWithCacheDecorator>().As<IProductService>();

            // upgrade 

            builder.RegisterGeneric(typeof(ServiceWithDto<,>)).As(typeof(IServiceWithDto<,>)).InstancePerLifetimeScope();

            builder.RegisterType<ProductServiceWithDto>().As<IProductServiceWithDto>().InstancePerLifetimeScope();

            builder.RegisterAssemblyTypes(apiAssembly, repoAssembly, serviceAssembly).
                Where(x => x.Name.EndsWith("ServiceWithDto")).AsImplementedInterfaces().InstancePerLifetimeScope();

            /*
             * InstancePerLifetimeScope => scope
             * InstancePerDependency => transient
             */


        }
    }
}
