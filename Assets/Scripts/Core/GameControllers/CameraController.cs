using CardStacker.General.Controllers;
using CardStacker.General.Services;
using UnityEngine;

namespace CardStacker.Core.GameControllers
{
    internal sealed class CameraController : ICameraController, IStart
    {
        private readonly IDataService _dataService;
        private Camera _camera;

        public CameraController(IDataService dataService) => _dataService = dataService;
        public void SetCamera(Camera camera) => _camera = camera;

        public void Start()
        {
            var runtimeData = _dataService.RuntimeData;
            var staticData = _dataService.StaticData;

            float levelWight = (staticData.FieldColumns - 1) * runtimeData.HorizontalOffset;
            float levelHeight = (staticData.FieldRows - 1) * runtimeData.VerticalOffset;
            _camera.transform.position = new Vector3(levelWight * 0.5f, levelHeight * 0.5f, 0);
            _camera.orthographic = true;
            _camera.orthographicSize = (staticData.FieldColumns > staticData.FieldRows)
                ? (levelWight + runtimeData.CardWight) * 0.75f / _camera.aspect
                : (levelHeight + runtimeData.CardHeight) * 0.5f + 1f;
            _camera.nearClipPlane = -1;
            _camera.farClipPlane = 1;
        }
    }
}