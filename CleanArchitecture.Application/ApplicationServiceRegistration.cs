using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using FluentValidation;
using MediatR;
using CleanArchitecture.Application.Behaviours;

namespace CleanArchitecture.Application
{
    // this is the container of all the dependencies that will be injected in the different classes we defined
    // the type of injections are the following:
    // Transient -> a new instance will be created each time the request is called
    // Scoped  -> the same instance will be used during the hole lifetime of the request
    // Singleton -> once a instance is created it will be used during the lifetime of the server (will be used in all the request)
    public static class ApplicationServiceRegistration
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            // gets all references to the autoMapper class
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            // gets all references to the fluent validator and static validator class
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            // gets all references to the mediatR class
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(UnhandledExceptionBehaviour<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));

            return services;
        }
    }
}
