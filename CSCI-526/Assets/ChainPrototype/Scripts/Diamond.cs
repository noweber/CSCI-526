using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Diamond : Piece
{
    public override List<Tuple<int, int>> LegalMoves(int boardWidth, int boardHeight)
    {
        List<Tuple<int, int>> legalSpots = new List<Tuple<int, int>>();

        var pos = new Vector2Int ((int)transform.position.x, (int)transform.position.y);
        var lvlModel = GridManager.Instance.levelModel;

        for (int i = 0; i < boardWidth; i++)
        {
            for (int j = 0; j < boardHeight; j++)
            {
                //get diagonals - those are ok
                if (Mathf.Abs(i-pos.x) == 1 && Mathf.Abs(j-pos.y) == 1)
                {
                    var availableMove = new Tuple<int, int>(i, j);
                    if (lvlModel.TryGetUnit(i, j) != null)
                    {
                        if (lvlModel.TryGetUnit(i, j).Item1 == this.isWhite) {continue;}
                    }
                    // if (GridManager.Instance.GetPiece(availableMove) != null)
                    // {
                    //     if (GridManager.Instance.GetPiece(availableMove).isWhite == this.isWhite) { continue; }
                    // }
                    legalSpots.Add(availableMove);
                }

                //get +1 to cardinal directions
                if (Mathf.Abs(i - pos.x) == 2 && Mathf.Abs(j - pos.y) == 0)
                {
                    var availableMove = new Tuple<int, int>(i, j);
                    if (lvlModel.TryGetUnit(i, j) != null)
                    {
                        if (lvlModel.TryGetUnit(i, j).Item1 == this.isWhite) {continue;}
                    }
                    legalSpots.Add(availableMove);
                }
                if (Mathf.Abs(i - pos.x) == 0 && Mathf.Abs(j - pos.y) == 2)
                {
                    var availableMove = new Tuple<int, int>(i, j);
                    if (lvlModel.TryGetUnit(i, j) != null)
                    {
                        if (lvlModel.TryGetUnit(i, j).Item1 == this.isWhite) {continue;}
                    }
                    legalSpots.Add(availableMove);
                }
            }
        }

		if (GameManagerChain.Instance.SceneName == "TutorialLevel" && GameManagerChain.Instance.TotalMoves == 0) 
		{
			// first move of tutorial
			Debug.Log("FIRST MOVE");
			var availableMove = new Tuple<int, int>(1, 0);
			legalSpots.Clear();
			legalSpots.Add(availableMove);
		} else if (GameManagerChain.Instance.SceneName == "TutorialLevel" && GameManagerChain.Instance.TotalMoves == 1) {
            legalSpots.Clear();
        }

        return legalSpots;
    }

    public override List<Tuple<int, int>> adjacentAllies(Tuple<int, int> unitPosition)
    {
        var adjacentList = new List<Tuple<int, int>>();
        adjacentList.Add(new Tuple<int, int>(unitPosition.Item1 + 1, unitPosition.Item2));
        adjacentList.Add(new Tuple<int, int>(unitPosition.Item1 - 1, unitPosition.Item2));
        adjacentList.Add(new Tuple<int, int>(unitPosition.Item1, unitPosition.Item2 + 1));
        adjacentList.Add(new Tuple<int, int>(unitPosition.Item1, unitPosition.Item2 - 1));

        var adjAlly = new List<Tuple<int, int>>();
        var lvlModel = GridManager.Instance.levelModel;
        foreach (Tuple<int, int> coord in adjacentList)
        {
            if (lvlModel.TryGetUnit(coord.Item1, coord.Item2).Item1 == this.isWhite)
            {
                adjAlly.Add(coord);
            }
        }
        if (adjAlly.Count == 0)
            return null;
        return adjAlly;
    }

    public override void UpdateLocation(Vector3 location)
    {
        this.transform.position = location;
    }
}
