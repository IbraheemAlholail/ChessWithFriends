using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece : MonoBehaviour
{
    public enum PieceType
    {
        W_Pawn,
        W_Rook,
        W_Knight,
        W_Bishop,
        W_Queen,
        W_King,
        B_Pawn,
        B_Rook,
        B_Knight,
        B_Bishop,
        B_Queen,
        B_King

    }
    public PieceType type;
}
