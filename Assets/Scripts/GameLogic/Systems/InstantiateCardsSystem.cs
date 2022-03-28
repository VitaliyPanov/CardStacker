using CardStacker.Data;
using CardStacker.GameLogic.Components;
using CardStacker.GameLogic.Components.Events;
using CardStacker.GameLogic.Services;
using CardStacker.GameLogic.Views;
using CardStacker.General.Services;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace CardStacker.GameLogic.Systems
{
    internal sealed class InstantiateCardsSystem : IEcsInitSystem, IEcsRunSystem
    {
        private readonly IViewService _viewService;
        private readonly CardData _cardData;
        private readonly RuntimeData _runtimeData;
        private readonly EcsWorldInject _world = default;
        private EcsSystems _systems;
        private readonly EcsFilterInject<Inc<CellComponent, PositionIdComponent, EmptyCellComponent>, Exc<HeroComponent>>
            _emptyCellsFilter = default;
        private readonly EcsPoolInject<PositionIdComponent> _positionIdPool = default;
        private readonly EcsPoolInject<PositionComponent> _positionPool = default;
        private readonly EcsPoolInject<EmptyCellComponent> _emptyPool = default;
        private readonly EcsPoolInject<CardViewComponent> _cardViewPool = default;
        private readonly EcsPoolInject<TransformComponent> _transformPool = default;
        private readonly EcsPoolInject<CardEntityComponent> _cardEntityPool = default;

        public InstantiateCardsSystem(IViewService viewService, CardData cardData, RuntimeData runtimeData)
        {
            _viewService = viewService;
            _cardData = cardData;
            _runtimeData = runtimeData;
        }

        public void Init(EcsSystems systems)
        {
            _systems = systems;
            FillEmptyCells();
            CreateHero();
        }

        public void Run(EcsSystems systems) => FillEmptyCells();

        private void FillEmptyCells()
        {
            foreach (var i in _emptyCellsFilter.Value)
            {
                Vector2 cellPosition = GetPositionById(i);
                _cardEntityPool.Value.Add(i).Value = InstantiateCard(cellPosition);
                _emptyPool.Value.Del(i);
            }
        }

        private void CreateHero()
        {
            var heroFilter = _world.Value.Filter<EmptyCellComponent>().Inc<HeroComponent>().End();
            var heroPool = _world.Value.GetPool<HeroComponent>();
            foreach (var i in heroFilter)
            {
                var heroPosition = GetPositionById(i);
                _emptyPool.Value.Del(i);
                var heroCardEntity = InstantiateCard(heroPosition);
                _cardEntityPool.Value.Add(i).Value = heroCardEntity;
                heroPool.Add(heroCardEntity);
            }
        }

        private Vector2 GetPositionById(int i)
        {
            Vector2Int idPosition = _positionIdPool.Value.Get(i).Value;
            return new Vector2(idPosition.x * _runtimeData.HorizontalOffset, idPosition.y * _runtimeData.VerticalOffset);
        }

        private int InstantiateCard(Vector2 cellPosition)
        {
            var cardView = CreateCardView(cellPosition);
            var cardEntity = CreateCardEntity(cardView, cellPosition);
            _systems.SetEvent<NewCardEvent>().Entity = cardEntity;
            return cardEntity;
        }

        private int CreateCardEntity(ICardView cardView, Vector2 cellPosition)
        {
            var cardEntity = _world.Value.NewEntity();
            _cardViewPool.Value.Add(cardEntity).Value = cardView;
            _positionPool.Value.Add(cardEntity).Value = cellPosition;
            _transformPool.Value.Add(cardEntity).Value = cardView.Transform;
            return cardEntity;
        }

        private CardView CreateCardView(Vector2 cellPosition)
        {
            CardView cardView = _viewService.CreateView<CardView>(_cardData.CardPrefab, null);
            cardView.Transform.position = new Vector3(cellPosition.x, cellPosition.y, 0);
            return cardView;
        }
    }
}