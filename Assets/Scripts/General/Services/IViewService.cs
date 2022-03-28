using UnityEngine;

namespace CardStacker.General.Services
{
    public interface IViewService
    {
        T CreateView<T>(GameObject prefab, Transform parent) where T : Component, IView;

        void DestroyView(IView view);
    }
}