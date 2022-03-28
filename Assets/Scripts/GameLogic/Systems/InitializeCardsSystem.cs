using CardStacker.Data;
using CardStacker.GameLogic.Components;
using CardStacker.GameLogic.Components.Events;
using CardStacker.GameLogic.Services;
using CardStacker.GameLogic.Views;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace CardStacker.GameLogic.Systems
{
    internal sealed class InitializeCardsSystem : IEcsInitSystem, IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<NewCardEvent>> _eventFilter = WorldNames.EVENTS;
        private readonly EcsPoolInject<NewCardEvent> _eventPool = WorldNames.EVENTS;
        private readonly EcsPoolInject<CardViewComponent> _viewPool = default;
        private readonly EcsPoolInject<CardTypeComponent> _cardTypePool = default;
        private readonly EcsPoolInject<CardPointsComponent> _cardPointsPool = default;

        private const int c_defaultPoints = 1;
        private const int c_heroLayerOrder = 100;
        private readonly int _maxRedCardRate;
        private readonly CardData _cardData;
        private readonly RuntimeData _runtimeData;
        private readonly CardType[] _redCardChance = new CardType[10];
        private bool _isChanceFilled;


        public InitializeCardsSystem(CardData cardData, RuntimeData runtimeData)
        {
            _cardData = cardData;
            _runtimeData = runtimeData;
            for (int i = 0; i < _redCardChance.Length; i++)
            {
                _redCardChance[i] = CardType.Green;
            }

            _maxRedCardRate = Mathf.CeilToInt(_runtimeData.RedChanceRate / 0.1f);
        }

        public void Init(EcsSystems systems)
        {
            SetRedCardChance(_runtimeData.DifficultyLevel);
            var heroPool = systems.GetWorld().GetPool<HeroComponent>();
            foreach (var i in _eventFilter.Value)
            {
                ref var newCardEntity = ref _eventPool.Value.Get(i).Entity;
                ref var cardView = ref _viewPool.Value.Get(newCardEntity).Value;
                if (!heroPool.Has(newCardEntity))
                {
                    CreateStackableCard(cardView, newCardEntity);
                }
                else
                {
                    InitializeView(cardView, _cardData.HeroOverlay, c_heroLayerOrder, c_defaultPoints);
                    InitializeEntity(newCardEntity, CardType.Hero, c_defaultPoints);
                }

                _eventPool.Value.Del(i);
            }
        }

        public void Run(EcsSystems systems)
        {
            foreach (var i in _eventFilter.Value)
            {
                SetRedCardChance(_runtimeData.DifficultyLevel);
                ref var cardEntity = ref _eventPool.Value.Get(i).Entity;
                ref var cardView = ref _viewPool.Value.Get(cardEntity).Value;
                CreateStackableCard(cardView, cardEntity);
            }
        }

        private void CreateStackableCard(ICardView cardView, int i)
        {
            var cardType = _redCardChance[Random.Range(0, 10)];
            int cardPoints = c_defaultPoints + Random.Range(0, _runtimeData.DifficultyLevel + 1);
            InitializeView(cardView, cardType == CardType.Green ? _cardData.GreenOverlay : _cardData.RedOverlay, 0,
                cardPoints);
            InitializeEntity(i, cardType, cardPoints);
        }

        private void InitializeEntity(int entity, CardType type, int points)
        {
            _cardTypePool.Value.Add(entity).Value = type;
            _cardPointsPool.Value.Add(entity).Value = points;
        }

        private void InitializeView(ICardView cardView, Sprite overlay, int layerOrder, int points)
        {
            cardView.SetOverlay(overlay, layerOrder);
            cardView.SetPoints(points);
        }

        private void SetRedCardChance(int value)
        {
            if (_isChanceFilled) return;
            if (value > _maxRedCardRate || value > 10)
            {
                value = _maxRedCardRate;
                _isChanceFilled = true;
            }

            for (int i = 0; i < value; i++)
            {
                _redCardChance[i] = CardType.Red;
            }
        }
    }
}