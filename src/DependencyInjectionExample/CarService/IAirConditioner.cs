namespace DependencyInjectionExample
{
    public interface IAirConditioner
    {
        int Temperature
        {
            get;
            set;
        }

        void TurnOn();

        void TurnOff();
    }
}
