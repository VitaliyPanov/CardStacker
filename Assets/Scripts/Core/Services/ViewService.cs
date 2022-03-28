using CardStacker.General.Services;
using DG.Tweening;
using UnityEngine;

namespace CardStacker.Core.Services
{
    internal sealed class ViewService : IViewService
    {
        private const float c_scaleDuration = 2f;
        private readonly IPoolService _poolService;
        public ViewService(IPoolService poolService) => _poolService = poolService;

        public T CreateView<T>(GameObject prefab, Transform parent) where T : Component, IView
        { 
            T view = _poolService.Instantiate<T>(prefab, parent);
            view.Transform.localScale = Vector3.zero;
            view.InitializeView();
            view.Transform.DOScale(Vector3.one, c_scaleDuration);
            return view;
        }

        public void DestroyView(IView view)
        {
            _poolService.Destroy(view.GameObject);
            view.Transform.DOScale(Vector3.zero, c_scaleDuration);
        }
    }
}