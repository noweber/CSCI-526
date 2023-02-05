using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class Triangle : Piece
{
    public override List<Tuple<int, int>> LegalMoves(int boardWidth, int boardHeight)
    {
        List<Tuple<int, int>> legalSpots = new List<Tuple<int, int>>();

        /*triangle moves like a bishop (diagonal)
         *  delta(x) = delta(y)
        */
        var pos = new Vector2Int((int)transform.position.x, (int)transform.position.y);

        bool triangleAbilityCheck = this.TriangleAbilityCheck(pos);
        //bool useAbility = false;
        //var lvlModel = GridManager.Instance.levelModel;

        if (triangleAbilityCheck == true && GameManagerChain.Instance.UsedAbility != true)
        {
            //enable some ability option to add another movement turn
            //MenuManager Option pops up
            MenuManager.Instance.ShowAbilityButton();
            //if option is selected, ignore triangle legal moves + add 1 to NumMoves?

            //useAbility = true;
            //GameManager.Instance.UsedAbility = true;
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
            if (coord.Item1 >= 0 && coord.Item1 < GridManager.Instance._width && coord.Item2 >= 0 && coord.Item2 < GridManager.Instance._height)
            {
                if (lvlModel.TryGetUnit(coord.Item1, coord.Item2) != null && lvlModel.TryGetUnit(coord.Item1, coord.Item2).Item1 == this.isWhite)
                {
                    adjAlly.Add(coord);
                }
            }
        }
        if (adjAlly.Count == 0)
            return null;
        return adjAlly;
    }


    private bool TriangleAbilityCheck(Vector2Int pos)
    {
        var adjList = this.adjacentAllies(new Tuple<int, int>(pos.x, pos.y));
        var lvlModel = GridManager.Instance.levelModel;
        if (adjList != null)
        {
            foreach (Tuple<int, int> coord in adjList)
            {
                if (lvlModel.TryGetUnit(coord.Item1, coord.Item2).Item2 == Assets.Scripts.Units.UnitType.Circle)
                {
                    Debug.Log("Set Triangle Ability True");
                    return true;
                }
            }
        }
        return false;
    }

    public override void UpdateLocation(Vector3 location)
    {
        this.transform.position = location;
    }
}
