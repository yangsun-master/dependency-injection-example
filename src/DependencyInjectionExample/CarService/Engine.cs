namespace DependencyInjectionExample
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class Engine : IEngine
    {
        public void Run()
        {
            Console.WriteLine("Engine is running.");
        }

        public void Stop()
        {
            Console.WriteLine("Engine stopped.");
        }
    }
}
