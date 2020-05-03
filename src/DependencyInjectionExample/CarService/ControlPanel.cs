namespace DependencyInjectionExample
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class ControlPanel : IControlPanel
    {
        private readonly IAirConditioner airConditioner;
        private readonly ICDPlayer cdPlayer;

        public ControlPanel(IAirConditioner airConditioner, ICDPlayer cdPlayer)
        {
            this.airConditioner = airConditioner;
            this.cdPlayer = cdPlayer;
        }

        public void TurnOnAirConditioner()
        {
            this.airConditioner.TurnOn();
        }

        public void TurnOffAirConditioner()
        {
            this.airConditioner.TurnOff();
        }

        public void TurnOnMusic()
        {
            this.cdPlayer.Play();
        }

        public void TurnOffMusic()
        {
            this.cdPlayer.Stop();
        }
    }
}
