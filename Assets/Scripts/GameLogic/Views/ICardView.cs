using CardStacker.General.Services;
using UnityEngine;

namespace CardStacker.GameLogic.Views
{
    public interface ICardView : IView
    {
        void SetPoints(int value);
        void SetOverlay(Sprite sprite,int layerOrder);
    }
}