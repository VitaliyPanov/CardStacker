using UnityEngine;

namespace CardStacker.General.Services
{
    public interface IView
    {
        Transform Transform { get; }
        GameObject GameObject { get; }
        void InitializeView();
    }
}