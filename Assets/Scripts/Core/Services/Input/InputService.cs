using CardStacker.General.Services.Input;

namespace CardStacker.Core.Services.Input
{
    internal sealed class InputService : IInputService
    {
        private readonly InputControls _controls;

        public InputService() => _controls = new InputControls();

        public void AddOrRemoveGeneralListener(InputControls.IGeneralActions listener)
        {
            _controls.General.SetCallbacks(listener);
            if (listener != null)
                _controls.General.Enable();
            else
                _controls.General.Disable();
        }
    }
}