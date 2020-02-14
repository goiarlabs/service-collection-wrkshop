using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using workshop.strategy.Greeter.Languages;

namespace workshop.strategy
{
    public class GreeterServiceBuilder
    {
        private readonly IServiceCollection _services;

        public GreeterServiceBuilder(IServiceCollection services)
        {
            _services = services;
        }

        public void UseSpagnolo() => _services.AddTransient<IHellowLanguage, HellowSpagnolo>();

        public void UseDefault()
        {
            _services.AddTransient<IHellowLanguage, HellowDefault>();

            //_services.AddTransient<IHellowLanguage>(a => new HellowDefault());

            //_services.Add(new ServiceDescriptor(typeof(IHellowLanguage), typeof(HellowDefault), ServiceLifetime.Transient));

            /* _services.Add(
                new ServiceDescriptor(
                    typeof(IHellowLanguage),
                    factory => { return new HellowDefault(); },
                    ServiceLifetime.Transient));*/
        }

        public void UseConfigurable(string thing)
        {
            _services.AddTransient<IHellowLanguage, HallowConfigurable>(
                a => new HallowConfigurable(thing));
        }
    }
}