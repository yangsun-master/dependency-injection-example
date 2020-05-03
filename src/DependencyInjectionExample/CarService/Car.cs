namespace DependencyInjectionExample
{
    using System;
    using System.Collections.Generic;

    public class Car: ICar
    {
        private readonly IEngine engine;
        private readonly IControlPanel controlPanel;

        public string Name { get; set; }

        public Car(IEngine engine, IControlPanel controlPanel)
        {
            this.engine = engine;
            this.controlPanel = controlPanel;
            this.Name = "Black";
        }

        public void Start()
        {
            Console.WriteLine(this.Name + " car is starting.");
            this.engine.Run();
        }

        public void TurnOnAirConditioner()
        {
            this.controlPanel.TurnOnAirConditioner();
        }

        public void PlayMusic()
        {
            this.controlPanel.TurnOnMusic();
        }

        public void Stop()
        {
            Console.WriteLine(this.Name + " car is stopping.");
            this.engine.Stop();
            this.controlPanel.TurnOffAirConditioner();
            this.controlPanel.TurnOffMusic();
        }
    }
}
