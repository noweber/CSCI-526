using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Piece", menuName = "Scriptable Piece")]
public class ScriptablePiece : ScriptableObject
{
    public Alignment Faction;
    public PieceController UnitPrefab;

    //add data later for stats, hp, etc.
}

public enum Alignment
{
    Player, Enemy
}