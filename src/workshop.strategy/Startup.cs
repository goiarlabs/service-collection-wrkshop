using Microsoft.Extensions.DependencyInjection;

using System;

using workshop.strategy.Greeter;

namespace workshop.strategy
{
    public static class Startup
    {
        public static IServiceCollection AddGreeter(
            this IServiceCollection services
            , Action<GreeterServiceBuilder> options = null)
        {
            services.AddScoped<IGreetService, GreetService>();

            options.Invoke(new GreeterServiceBuilder(services));

            return services;
        }
    }
}
