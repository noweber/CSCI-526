using System;
using System.Collections.Generic;

namespace Assets.Scripts.Piece
{
    public abstract class PieceModel : IPiece
    {
        protected Tuple<int, int> Position;

        protected bool HasUnitMoved;

        protected bool IsHumanUnit;

        public PieceModel(Tuple<int, int> piecePosition, bool isControlledByHumanPlayer)
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

        public string Summary()
        {
            throw new NotImplementedException();
        }

        public void UpdateLocation(Tuple<int, int> location)
        {
            Position = location;
        }

        public abstract string Name();

        public abstract List<Tuple<int, int>> LegalMoves(int boardWidth, int boardHeight);

        public abstract List<Tuple<int, int>> AdjacentAllies(Tuple<int, int> unitPosition);
    }
}
