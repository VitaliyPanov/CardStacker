namespace CardStacker.GameLogic.Components.Events
{
    internal struct TryMoveEvent
    {
        public MoveDirection Direction;
    }


    internal enum MoveDirection
    {
        None,
        Up,
        Down,
        Right,
        Left
    }
}