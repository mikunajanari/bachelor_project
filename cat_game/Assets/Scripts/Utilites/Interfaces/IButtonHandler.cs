namespace cats
{
    public interface IButtonHandler
    {
        string ButtonId { get; }
        void Handle();
    }
}
