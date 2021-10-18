namespace Base
{
    public interface IController
    {
        void OnInteract<T>(T interactData);

        void OnInteract();
    }
}