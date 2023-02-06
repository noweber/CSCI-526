using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Piece
{
    public interface IPiece
    {
        bool IsControlledByHuman();

        bool HasMoved();

        void SetMoveState(bool state);

        string Name();

        string Summary();

        List<Tuple<int, int>> AdjacentAllies(Tuple<int, int> unitPosition);

        List<Tuple<int, int>> LegalMoves(int boardWidth, int boardHeight);

        void UpdateLocation(Tuple<int, int> location);
    }
}
