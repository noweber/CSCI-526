﻿using Assets.Scripts.Piece;
using System;
using System.Collections.Generic;

namespace Assets.Scripts.Levels
{
    public class LevelData
    {
        // TODO: add a constructor and make all setters private

        public int Width;

        public int Height;

        public Dictionary<Tuple<int, int>, IPiece> Units;
    }
}