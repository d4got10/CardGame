namespace CardGame
{
    public interface IWinListenable
    {
        void AddListener(IWinListener listener);
        bool GetWinState();
    }
}