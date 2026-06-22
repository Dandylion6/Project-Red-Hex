using System.Collections.Generic;
using UnityEngine;

public class TurnManager : Singleton<TurnManager>
{
    private List<TilePiece> piecesWithTurn = new();


    private void Start()
    {
        piecesWithTurn.Add(GameManager.Instance.Player);
    }


    private void Update()
    {
        
    }
}
