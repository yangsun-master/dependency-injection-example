namespace DependencyInjectionExample
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class CDPlayer : ICDPlayer
    {
        public void Play()
        {
            Console.WriteLine("CD Player is playing music.");
        }

        public void Stop()
        {
            Console.WriteLine("CD Player stopped.");
        }
    }
}
