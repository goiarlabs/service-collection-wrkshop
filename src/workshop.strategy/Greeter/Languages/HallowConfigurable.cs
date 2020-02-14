namespace workshop.strategy.Greeter.Languages
{
    public class HallowConfigurable : IHellowLanguage
    {
        public HallowConfigurable(string hellow)
        {
            Hellow = hellow;
        }

        public string Hellow { get; }
    }
}
