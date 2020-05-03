namespace DependencyInjectionExample
{
    using System;
    using Microsoft.Extensions.DependencyInjection;

    class Program
    {
        private AssemblyCreater assemblyCreater;
        static void Main(string[] args)
        {
            try
            {
                Program program = new Program();
                program.Run();
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }
        }

        public void Run()
        {
            this.assemblyCreater = new AssemblyCreater();
            var car = this.CreateManually();
            this.TestCar(car);
            car = this.CreateByLibrary();
            this.TestCar(car);
            car = this.CreateByDynamicMethod();
            this.TestCar(car);
            car = this.CreateByLambdaExpression();
            this.TestCar(car);
            this.assemblyCreater.Save();

        }
        private ICar CreateByLibrary()
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddTransient<ICar, Car>();
            serviceCollection.AddTransient<IAirConditioner, AirConditioner>();
            serviceCollection.AddTransient<ICDPlayer, CDPlayer>();
            serviceCollection.AddTransient<IEngine, Engine>();
            serviceCollection.AddTransient<IControlPanel, ControlPanel>();
            using (ServiceProvider serviceProvider = serviceCollection.BuildServiceProvider())
            {
                var car = serviceProvider.GetService<ICar>();
                car.Name = "LibraryCreated";
                return car;
            }
        }

        private ICar CreateManually()
        {
            IEngine engine = new Engine();
            IAirConditioner airConditioner = new AirConditioner();
            ICDPlayer cdPlayer = new CDPlayer();
            IControlPanel controlPanel = new ControlPanel(airConditioner, cdPlayer);
            var car = new Car(engine, controlPanel);
            car.Name = "ManuallyCreated";
            return car;
        }

        private ICar CreateByDynamicMethod()
        {
            DynamicMethodServiceFactory serviceFactory = new DynamicMethodServiceFactory();
            serviceFactory.AddService<ICar, Car>();
            serviceFactory.AddService<IEngine, Engine>();
            serviceFactory.AddService<IControlPanel, ControlPanel>();
            serviceFactory.AddService<IAirConditioner, AirConditioner>();
            serviceFactory.AddService<ICDPlayer, CDPlayer>();
            this.assemblyCreater.CreateDynamicMethodCreaterType(serviceFactory.GetServiceMetadatas());
            ICar car = serviceFactory.GetService<ICar>();
            car.Name = "DynamicMethodCreated";
            return car;
            
        }

        private ICar CreateByLambdaExpression()
        {
            DynamicMethodServiceFactory serviceFactory = new DynamicMethodServiceFactory();
            serviceFactory.AddService<ICar, Car>();
            serviceFactory.AddService<IEngine, Engine>();
            serviceFactory.AddService<IControlPanel, ControlPanel>();
            serviceFactory.AddService<IAirConditioner, AirConditioner>();
            serviceFactory.AddService<ICDPlayer, CDPlayer>();
            this.assemblyCreater.CreateLambdaExpressionCreaterType(serviceFactory.GetServiceMetadatas());
            ICar car = serviceFactory.GetService<ICar>();
            car.Name = "LambdaExpressionCreated";
            return car;
        }

        void TestCar(ICar car)
        {
            car.Start();
            car.TurnOnAirConditioner();
            car.PlayMusic();
            car.Stop();
        }
    }
}
