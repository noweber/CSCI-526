using System;
using System.Collections.Generic;

namespace Assets.Scripts.Piece
{
    public abstract class Piece : IPiece
    {
        protected Tuple<int, int> Position;

        protected bool HasUnitMoved;

        protected bool IsHumanUnit;

        public Piece(Tuple<int, int> piecePosition, bool isControlledByHumanPlayer)
        {
            Position = piecePosition;
            HasUnitMoved = false;
            IsHumanUnit = isControlledByHumanPlayer;
        }

        public bool HasMoved()
        {
            return HasUnitMoved;
        }

        public void SetMoveState(bool state)
        {
            HasUnitMoved = state;
        }

        public bool IsControlledByHuman()
        {
            return IsHumanUnit;
        }

        // TODO: Move these strings back to the inspector via prefab?
        public abstract string Info();

        public void UpdateLocation(Tuple<int, int> location)
        {
            Position = location;
        }

        public abstract string Name();

        public abstract List<Tuple<int, int>> LegalMoves(int boardWidth, int boardHeight);

        public abstract List<Tuple<int, int>> AdjacentAllies(Tuple<int, int> unitPosition);
    }
}
