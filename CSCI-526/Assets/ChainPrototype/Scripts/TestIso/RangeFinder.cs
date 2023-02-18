using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;

public class RangeFinder
{
    public List<Overlay> GetTilesInRange(Tuple<int,int> location, int range)
    {
        var startingTile = LevelMono.Instance.overlayTiles[location];
        var inRangeTiles = new List<Overlay>();
        int stepCount = 0;

        inRangeTiles.Add(startingTile);

        //Should contain the surroundingTiles of the previous step. 
        var tilesForPreviousStep = new List<Overlay>();
        tilesForPreviousStep.Add(startingTile);
        while (stepCount < range)
        {
            var surroundingTiles = new List<Overlay>();

            foreach (var item in tilesForPreviousStep)
            {
                surroundingTiles.AddRange(LevelMono.Instance.GetSurroundingTiles(new Tuple<int,int>(item.gridLocation.x, item.gridLocation.y)));
            }

            inRangeTiles.AddRange(surroundingTiles);
            tilesForPreviousStep = surroundingTiles.Distinct().ToList();
            stepCount++;
        }

        return inRangeTiles.Distinct().ToList();
    }
}
