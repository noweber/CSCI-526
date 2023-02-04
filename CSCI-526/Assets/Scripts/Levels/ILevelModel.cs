using Assets.Scripts.Units;
using System;

namespace Assets.Scripts.Levels
{
    public interface ILevelModel
    {
        int GetWidth();

        int GetHeight();

        Tuple<bool, UnitType> TryGetUnit(int xPosition, int yPosition);

        void PutUnit(Tuple<int, int> position, bool isWhite, UnitType playerUnit);

        void TryMoveUnit(Tuple<int, int> fromPosition, Tuple<int, int> toPosition);
    }
}
