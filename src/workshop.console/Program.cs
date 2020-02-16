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
            // No se con exactitud en donde se tiene que resolver dinamicamente el ServiceCollection
            // el idioma con el que tiene que saludar un usuario o si lo tiene que resolver obligatoriamente el ServiceCollection
            var user = SetUserInformation();

            ConfigureServices(user);

            using (var scope = serviceProvider.CreateScope())
            {
                DoWork(scope.ServiceProvider, user);
            }
        }

        private static void DoWork(IServiceProvider serviceProvider, Usuario user)
        {
            var controller = new GreeterController(serviceProvider.GetService<IGreetService>());

            controller.Greet(user.nombre);
        }

        private static void ConfigureServices(Usuario user)
        {
            IServiceCollection services = new ServiceCollection();

            // La forma de resolverlo
            switch (user.idioma)
            {
                case "Español":
                    services.AddGreeter(a => a.UseSpagnolo());
                    break;
                default:
                    services.AddGreeter(a => a.UseDefault());
                    break;
            }
            
            serviceProvider = services.BuildServiceProvider();
        }


        // La forma de levantar informacion de un usuario
        public static Usuario SetUserInformation()
        {
            Console.WriteLine("Ingresa tu nombre: ");
            var nombre = Console.ReadLine();

            Console.WriteLine("Ingresa tu idioma: ");
            var idioma = Console.ReadLine();

            return new Usuario(nombre, idioma);
        }

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


// Un objecto usuario para transportar la informacion
public class Usuario
{
    public string nombre;
    public string idioma;

    public Usuario(string nombre, string idioma)
    {
        this.nombre = nombre;
        this.idioma = idioma;
    }
}


