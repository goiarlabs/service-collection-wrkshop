using Microsoft.Extensions.Logging;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Under.the.hood
{
    public class ProveedorDeServicios
    {
        private readonly Dictionary<Type, Func<ProveedorDeServicios, object>> _internalDict;

        public ProveedorDeServicios(Dictionary<Type, Func<ProveedorDeServicios, object>> internalDict)
        {
            _internalDict = internalDict;
        }

        public T GetService<T>() => (T)GetService(typeof(T));


        public object GetService(Type type) => _internalDict[type].Invoke(this);
    }

    public class ColeccionDeServicios
    {
        private readonly Dictionary<Type, Func<ProveedorDeServicios, object>> uternalDic;

        public ColeccionDeServicios()
        {
            uternalDic = new Dictionary<Type, Func<ProveedorDeServicios, object>>();
        }

        public void AddService<T>(Func<ProveedorDeServicios, T> formaDeCrearUnT) where T : class =>
            uternalDic.Add(typeof(T), formaDeCrearUnT);

        public ProveedorDeServicios EnsamblarProveedorDeServicios()
        {
            var pepe = new ProveedorDeServicios(uternalDic);
            return pepe;
        }
    }

    #region A lot of classes


    public class Logger : ILogger<WeatherService>
    {
        public IDisposable BeginScope<TState>(TState state) => throw new NotImplementedException();
        public bool IsEnabled(LogLevel logLevel) => throw new NotImplementedException();
        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter) => Console.WriteLine(formatter.Invoke(state, exception));
    }

    public interface IQueryHandler<TOut, TIn>
    {
        Task<TOut> Handle(TIn query);
    }

    public class GetUserFieldsQuery
    {
        private Guid guid;

        public GetUserFieldsQuery(Guid guid)
        {
            this.guid = guid;
        }
    }
    public class Field { }

    public class FieldQueryHandler : IQueryHandler<List<Field>, GetUserFieldsQuery>
    {
        #region Fields

        private readonly WeatherService _weatherGetter;

        #endregion

        #region Constructor

        public FieldQueryHandler(WeatherService weatherGetter)
        {
            _weatherGetter = weatherGetter;
        }

        #endregion

        #region Public Methods

        public async Task<List<Field>> Handle(GetUserFieldsQuery query)
        {
            List<Field> field = null;

            await _weatherGetter.GetBetweenDates(12.4f, 12.5f, DateTime.Now, DateTime.Now);
            return await Task.FromResult(field);
        }

        #endregion
    }

    public interface IWeatherGetter
    {
        Task<ICollection<Weather>> GetBetweenDates(float Longitude, float Lattitude, DateTime from, DateTime To);
    }

    public class WeatherService : IWeatherGetter
    {

        private readonly IInfrastructureLogger<WeatherService> _logger;
        #region Constuctor

        public WeatherService(

            IInfrastructureLogger<WeatherService> logger)
        {
            _logger = logger;
        }

        public Task<ICollection<Weather>> GetBetweenDates(float Longitude, float Lattitude, DateTime from, DateTime To)
        {
            _logger.LogInfo("coso", "coso", "cosos");
            return Task.FromResult((ICollection<Weather>)new List<Weather>());
        }

        #endregion

    }

    public class Weather
    {

    }

    public interface IInfrastructureLogger<T>
    {
        void LogInfo(string coso, string coso2, string coso3);
    }

    public class InfrastructureLogger<T> : IInfrastructureLogger<T>
    {
        private readonly ILogger<T> _logger;
        private readonly Guid _transactionId;

        public InfrastructureLogger(ILogger<T> logger, Guid transactionId)
        {
            _logger = logger;
            _transactionId = transactionId;
        }

        public void LogInfo(string coso, string coso2, string coso3) => _logger.LogInformation($"[{_transactionId}]: {coso}, {coso2}, {coso3}");
    }

    #endregion

    public class StartUp
    {
        public void ConfigureServices(ColeccionDeServicios tito)
        {
            tito.AddService<ILogger<WeatherService>>(a => new Logger());

            tito.AddService(Logger);

            tito.AddService<WeatherService>(a => new WeatherService(a.GetService<InfrastructureLogger<WeatherService>>()));
        }

        public InfrastructureLogger<WeatherService> Logger(ProveedorDeServicios p)
        {
            return new InfrastructureLogger<WeatherService>(
                        p.GetService<ILogger<WeatherService>>(),
                        Guid.NewGuid());
        }

        public void Configure()
        {
            var collecionDeServicios = new ColeccionDeServicios();
            ConfigureServices(collecionDeServicios);

            var provedorDeServicios = collecionDeServicios.EnsamblarProveedorDeServicios();

            // El tipo de cosa a resolver
            var cosaAResolver = typeof(FieldQueryHandler);

            // Dame El constructor
            var constructor = cosaAResolver.GetConstructors().Single();

            // Dame los parametros del constructor
            var parameters = constructor.GetParameters();
            
            //resolver los parameteros del constructor
            var resolvedParameter = parameters.Select(a => provedorDeServicios.GetService(a.ParameterType));

            // Instanciar
            var fieldQueryHandler = (FieldQueryHandler)constructor.Invoke(resolvedParameter.ToArray());

            // Resultado de la operacion
            var result = fieldQueryHandler.Handle(new GetUserFieldsQuery(Guid.NewGuid())).GetAwaiter().GetResult();
        }
    }
}
