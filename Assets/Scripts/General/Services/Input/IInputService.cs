namespace CardStacker.General.Services.Input
{
    public interface IInputService
    {
        void AddOrRemoveGeneralListener(InputControls.IGeneralActions listener);
    }
}