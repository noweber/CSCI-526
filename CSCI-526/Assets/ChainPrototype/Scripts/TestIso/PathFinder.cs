using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;

public class PathFinder
{
    private Dictionary<Tuple<int,int>, Overlay> searchableTiles;

    public List<Overlay> FindPath(Overlay start, Overlay end, List<Overlay> inRangeTiles)
    {
        searchableTiles = new Dictionary<Tuple<int,int>, Overlay>();

        List<Overlay> openList = new List<Overlay>();
        HashSet<Overlay> closedList = new HashSet<Overlay>();

        if (inRangeTiles.Count > 0)
        {
            foreach (var item in inRangeTiles)
            {
                var loc = new Tuple<int, int>(item.grid2DLocation.x, item.grid2DLocation.y);
                searchableTiles.Add(loc, LevelMono.Instance.overlayTiles[loc]);
            }
        }
        else
        {
            searchableTiles = LevelMono.Instance.overlayTiles;
        }

        openList.Add(start);

        while (openList.Count > 0)
        {
            Overlay currentOverlayTile = openList.OrderBy(x => x.F).First();

            openList.Remove(currentOverlayTile);
            closedList.Add(currentOverlayTile);

            if (currentOverlayTile == end)
            {
                return GetFinishedList(start, end);
            }

            foreach (var tile in GetNeightbourOverlayTiles(currentOverlayTile))
            {
                if (tile.isBlocked || closedList.Contains(tile) || Mathf.Abs(currentOverlayTile.transform.position.z - tile.transform.position.z) > 1)
                {
                    continue;
                }

                tile.G = GetManhattenDistance(start, tile);
                tile.H = GetManhattenDistance(end, tile);

                tile.Previous = currentOverlayTile;


                if (!openList.Contains(tile))
                {
                    openList.Add(tile);
                }
            }
        }

        return new List<Overlay>();
    }

    private List<Overlay> GetFinishedList(Overlay start, Overlay end)
    {
        List<Overlay> finishedList = new List<Overlay>();
        Overlay currentTile = end;

        while (currentTile != start)
        {
            finishedList.Add(currentTile);
            currentTile = currentTile.Previous;
        }

        finishedList.Reverse();

        return finishedList;
    }

    private int GetManhattenDistance(Overlay start, Overlay tile)
    {
        return Mathf.Abs(start.gridLocation.x - tile.gridLocation.x) + Mathf.Abs(start.gridLocation.y - tile.gridLocation.y);
    }

    private List<Overlay> GetNeightbourOverlayTiles(Overlay currentOverlayTile)
    {
        var map = LevelMono.Instance.overlayTiles;

        List<Overlay> neighbours = new List<Overlay>();

        //right
        Tuple<int,int> locationToCheck = new Tuple<int,int>(
            currentOverlayTile.gridLocation.x + 1,
            currentOverlayTile.gridLocation.y
        );

        if (searchableTiles.ContainsKey(locationToCheck))
        {
            neighbours.Add(searchableTiles[locationToCheck]);
        }

        //left
        locationToCheck = new Tuple<int,int>(
            currentOverlayTile.gridLocation.x - 1,
            currentOverlayTile.gridLocation.y
        );

        if (searchableTiles.ContainsKey(locationToCheck))
        {
            neighbours.Add(searchableTiles[locationToCheck]);
        }

        //top
        locationToCheck = new Tuple<int,int>(
            currentOverlayTile.gridLocation.x,
            currentOverlayTile.gridLocation.y + 1
        );

        if (searchableTiles.ContainsKey(locationToCheck))
        {
            neighbours.Add(searchableTiles[locationToCheck]);
        }

        //bottom
        locationToCheck = new Tuple<int,int>(
            currentOverlayTile.gridLocation.x,
            currentOverlayTile.gridLocation.y - 1
        );

        if (searchableTiles.ContainsKey(locationToCheck))
        {
            neighbours.Add(searchableTiles[locationToCheck]);
        }

        return neighbours;
    }

    
}
