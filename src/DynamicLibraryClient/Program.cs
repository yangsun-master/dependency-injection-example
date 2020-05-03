namespace DynamicLibraryClient
{
    using System;
    using DependencyInjectionExample;
    using DynamicLibrary;

    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                RunByDynamicMethod();
                RunByLambdaExpression();
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }
        }

        static void RunByDynamicMethod()
        {
            var engine = DynamicMethodCreater.NewEngine(new object[] { });
            var airConditioner = DynamicMethodCreater.NewAirConditioner(new object[] { });
            var cdPlayer = DynamicMethodCreater.NewCDPlayer(new object[] { });
            var controlPanel = DynamicMethodCreater.NewControlPanel(new object[] { airConditioner, cdPlayer });
            var car = (ICar)DynamicMethodCreater.NewCar(new object[] { engine, controlPanel });
            car.Name = "DynamicMethodCreater";
            car.Start();
            car.Stop();
        }

        static void RunByLambdaExpression()
        {
            var engine = LambdaExpressionCreater.NewEngine(new object[] { });
            var airConditioner = LambdaExpressionCreater.NewAirConditioner(new object[] { });
            var cdPlayer = LambdaExpressionCreater.NewCDPlayer(new object[] { });
            var controlPanel = LambdaExpressionCreater.NewControlPanel(new object[] { airConditioner, cdPlayer });
            var car = (ICar)LambdaExpressionCreater.NewCar(new object[] { engine, controlPanel });
            car.Name = "LambdaExpressionCreater";
            car.Start();
            car.Stop();
        }
    }
}
