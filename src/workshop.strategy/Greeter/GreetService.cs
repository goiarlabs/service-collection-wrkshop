using workshop.strategy.Greeter.Languages;

namespace workshop.strategy.Greeter
{
    public class GreetService : IGreetService
    {
        private readonly IHellowLanguage _hellowLanguage;

        public GreetService(IHellowLanguage hellowLanguage)
        {
            _hellowLanguage = hellowLanguage;
        }

        public string Greet(string name) => $"{_hellowLanguage.Hellow} {name}";
    }
}
