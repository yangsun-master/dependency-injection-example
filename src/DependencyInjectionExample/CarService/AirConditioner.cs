namespace DependencyInjectionExample
{
    using System;
    using System.Collections.Generic;

    public class AirConditioner : IAirConditioner
    {
        public int Temperature
        {
            get;
            set;
        }

        public void TurnOff()
        {
            Console.WriteLine("Air Conditioner is Off.");
        }

        public void TurnOn()
        {
            Console.WriteLine("Air Conditioner is On.");
        }
    }
}
