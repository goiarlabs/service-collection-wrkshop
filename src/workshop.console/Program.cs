using Microsoft.Extensions.DependencyInjection;
using System;
using workshop.strategy;
using workshop.strategy.Greeter;

namespace workshop.console
{
    class Program
    {
        private static IServiceProvider serviceProvider;

        static void Main(string[] args)
        {
            ConfigureServices();

            using (var scope = serviceProvider.CreateScope())
            {
                DoWork(scope.ServiceProvider);
            }
        }

        private static void DoWork(IServiceProvider serviceProvider)
        {
            var controller = new GreeterController(serviceProvider.GetService<IGreetService>());

            controller.Greet("Pepe");
        }

        private static void ConfigureServices()
        {
            IServiceCollection services = new ServiceCollection();

            services.AddGreeter(a => a.UseSpagnolo());

            serviceProvider = services.BuildServiceProvider();
        }
    }

    public class GreeterController
    {
        private readonly IGreetService _greetService;

        public GreeterController(IGreetService greetService)
        {
            _greetService = greetService;
        }

        public void Greet(string name)
        {
            Console.WriteLine(_greetService.Greet(name));
        }
    }
}
