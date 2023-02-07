using Assets.Scripts.Piece;
using Assets.Scripts.Units;
using System;

namespace Assets.Scripts.Levels
{
    public interface ILevelModel
    {
        int GetWidth();

        int GetHeight();

        IPiece TryGetUnit(Tuple<int, int> position);

        void PutUnit(Tuple<int, int> position, IPiece playerUnit);

        bool TryMoveUnit(Tuple<int, int> fromPosition, Tuple<int, int> toPosition);

        bool TryCaptureUnit(Tuple<int, int> fromPosition, Tuple<int, int> toPosition);
    }
}
