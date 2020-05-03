namespace DependencyInjectionExample
{
    public interface ICar
    {
        string Name { get; set; }

        void Start();

        void Stop();

        void TurnOnAirConditioner();

        void PlayMusic();
    }
}
