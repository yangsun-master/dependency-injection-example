namespace DependencyInjectionExample
{
    using System;
    using Microsoft.Extensions.DependencyInjection;
    using BenchmarkDotNet.Attributes;
    using BenchmarkDotNet.Running;

    public class Program
    {
        private DynamicMethodServiceFactory dynamicMethodServiceFactory;
        private LambdaExpressionServiceFactory lambdaExpressionServiceFactory;
        private ServiceProvider serviceProvider;

        public Program()
        {
        }

        static void Main(string[] args)
        {
            try
            {
                //Program program = new Program();
                //program.Setup();
                //program.Run();
                var summary = BenchmarkRunner.Run<Program>();
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }
        }

        [GlobalSetup]
        public void Setup()
        {
            this.BuildServiceProvider();
            this.dynamicMethodServiceFactory = new DynamicMethodServiceFactory();
            this.lambdaExpressionServiceFactory = new LambdaExpressionServiceFactory();
            this.RegisterService(this.dynamicMethodServiceFactory);
            this.RegisterService(this.lambdaExpressionServiceFactory);
        }

        public void Run()
        {
            var car = this.SimpleCreate();
            car.Name = "ManuallyCreated";
            this.TestCar(car);
            car = this.LibraryCreate();
            car.Name = "LibraryCreated";
            this.TestCar(car);
            car = this.DynamicMethodCreate();
            car.Name = "DynamicMethodCreated";
            this.TestCar(car);
            car = this.LambdaExpressionCreate();
            car.Name = "LambdaExpressionCreated";
            this.TestCar(car);
        }

        public void SaveAssembly()
        {
            var assemblyCreater = new AssemblyCreater();
            assemblyCreater.CreateDynamicMethodCreaterType(this.dynamicMethodServiceFactory.GetServiceMetadatas());
            assemblyCreater.CreateLambdaExpressionCreaterType(this.lambdaExpressionServiceFactory.GetServiceMetadatas());
            assemblyCreater.Save();
        }

        public void TestCar(ICar car)
        {
            car.Start();
            car.TurnOnAirConditioner();
            car.PlayMusic();
            car.Stop();
        }

        [Benchmark(Baseline = true)]
        public ICar SimpleCreate()
        {
            IEngine engine = new Engine();
            IAirConditioner airConditioner = new AirConditioner();
            ICDPlayer cdPlayer = new CDPlayer();
            IControlPanel controlPanel = new ControlPanel(airConditioner, cdPlayer);
            return new Car(engine, controlPanel);
        }

        [Benchmark]
        public ICar LibraryCreate()
        {
            return this.serviceProvider.GetService<ICar>();
        }

        [Benchmark]
        public ICar DynamicMethodCreate()
        {
            return this.dynamicMethodServiceFactory.GetService<ICar>();
        }

        [Benchmark]
        public ICar LambdaExpressionCreate()
        {
            return this.lambdaExpressionServiceFactory.GetService<ICar>();
        }

        private void BuildServiceProvider()
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddTransient<ICar, Car>();
            serviceCollection.AddTransient<IAirConditioner, AirConditioner>();
            serviceCollection.AddTransient<ICDPlayer, CDPlayer>();
            serviceCollection.AddTransient<IEngine, Engine>();
            serviceCollection.AddTransient<IControlPanel, ControlPanel>();
            this.serviceProvider = serviceCollection.BuildServiceProvider();
        }

        private void RegisterService(ServiceFactoryBase serviceFactory)
        {
            serviceFactory.AddService<ICar, Car>();
            serviceFactory.AddService<IEngine, Engine>();
            serviceFactory.AddService<IControlPanel, ControlPanel>();
            serviceFactory.AddService<IAirConditioner, AirConditioner>();
            serviceFactory.AddService<ICDPlayer, CDPlayer>();
        }
    }
}
