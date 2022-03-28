using UnityEngine;

namespace CardStacker.General.Controllers
{
    public interface ICameraController : IController
    {
        void SetCamera(Camera camera);
    }
}