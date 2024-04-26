using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;

public class Board : MonoBehaviour
{
    public GameObject blackTile;
    public GameObject whiteTile;
    public GameObject chessBoarder;
    public GameObject pieceHolder;
    public GameObject legends;
    public GameObject[,] tiles = new GameObject[8, 8]; 
    public GameObject[] whitePieces = new GameObject[6]; //array of objects, saved in this order: pawn, rook, knight, bishop, queen, king
    public GameObject[] blackPieces = new GameObject[6]; //array of objects, saved in this order: pawn, rook, knight, bishop, queen, king
    public Piece.PieceType pieceType;
    


    private void Start()
    {
        MakeBoard();
        Invoke("placePieces", 1);
        Invoke("testmove", 2);
        

    }

    private void Update()
    {
        //if a pieces is the child of a tile, set the pieces position to the tile position
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                if (tiles[i, j].transform.childCount > 0)
                {
                    tiles[i, j].transform.GetChild(0).transform.position = tiles[i, j].transform.position;
                    //set the z value to 1 so that the piece is on top of the tile
                    tiles[i, j].transform.GetChild(0).transform.localPosition = new Vector3(tiles[i, j].transform.GetChild(0).transform.localPosition.x, tiles[i, j].transform.GetChild(0).transform.localPosition.y, 1);
                }
            }
        }
    }

    private void MakeBoard()
    {
        // Get the size of the chessBoarder
        Vector3 boardSize = chessBoarder.GetComponent<Renderer>().bounds.size;

        // Calculate the size of each tile
        float tileSize = boardSize.x / 8;

        // Generate the board
        for (int j = 0; j < 8; j++)
        {
            for (int i = 0; i < 8; i++)
            {
                GameObject tile;
                if ((i + j) % 2 == 0)
                {
                    tile = Instantiate(whiteTile, transform);
                    tile.AddComponent<BoxCollider2D>();
                    tile.GetComponent<BoxCollider2D>().isTrigger = true;
                    //change the name of the tile to the file and rank
                    tile.name = ((char)(65 + i)).ToString() + (j + 1).ToString();
                    tile.layer = 11;
                }
                else
                {
                    tile = Instantiate(blackTile, transform);
                    tile.AddComponent<BoxCollider2D>();
                    tile.GetComponent<BoxCollider2D>().isTrigger = true;
                    //change the name of the tile to the file and rank
                    tile.name = ((char)(65 + i)).ToString() + (j + 1).ToString();
                    tile.layer = 11;

                }

                // Calculate position for each tile
                float xPos = (i * tileSize) - (boardSize.x / 2) + (tileSize / 2);
                float yPos = (j * tileSize) - (boardSize.y / 2) + (tileSize / 2);

                // Set position
                tile.transform.localPosition = new Vector3(xPos, yPos, 0);

                // Set scale
                tile.transform.localScale = new Vector3(tileSize, tileSize, -1);

                //save the tile in an array for later use
                tiles[i, j] = tile;
            }
        }
        MakeBoardLegend();
    }

    private void MakeBoardLegend()
    {
        //make for loop to make the letters and numbers
        for (int i = 0; i < 8; i++)
        {
            //make the letters
            GameObject letter = new GameObject();
            letter.AddComponent<TextMesh>();
            letter.GetComponent<TextMesh>().text = ((char)(65 + i)).ToString();
            letter.GetComponent<TextMesh>().fontSize = 60;
            letter.GetComponent<TextMesh>().color = Color.black;
            letter.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
            letter.GetComponent<TextMesh>().anchor = TextAnchor.MiddleCenter;
            letter.transform.localPosition = new Vector3(tiles[i, 0].transform.localPosition.x, tiles[i, 0].transform.localPosition.y - 1, 0);
            letter.transform.parent = legends.transform;
            letter.name = ((char)(65 + i)).ToString();

            //make the numbers
            GameObject number = new GameObject();
            number.AddComponent<TextMesh>();
            number.GetComponent<TextMesh>().text = (i + 1).ToString();
            number.GetComponent<TextMesh>().fontSize = 60;
            number.GetComponent<TextMesh>().color = Color.black;
            number.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
            number.GetComponent<TextMesh>().anchor = TextAnchor.MiddleLeft;
            number.transform.localPosition = new Vector3(tiles[0, i].transform.localPosition.x - 1, tiles[0, i].transform.localPosition.y, 0);
            number.transform.parent = legends.transform;
            number.name = (i + 1).ToString();
        }
    }


    public void placePieces()
    {
        //scale all pieces down 10%. 
        for (int i = 0; i < whitePieces.Length; i++)
        {
            whitePieces[i].transform.localScale = new Vector3(0.9f, 0.9f, 0.9f);
            blackPieces[i].transform.localScale = new Vector3(0.9f, 0.9f, 0.9f);
        }



        for (int i = 0; i < 8; i++)
        {
            //place the white pawns
            placePiece(whitePieces[0], 1, i, Piece.PieceType.W_Pawn);
            //place the black pawns
            placePiece(blackPieces[0], 6, i, Piece.PieceType.B_Pawn);
        }

        //place the white pieces
        placePiece(whitePieces[1], 0, 0, Piece.PieceType.W_Rook);
        placePiece(whitePieces[2], 0, 1, Piece.PieceType.W_Knight);
        placePiece(whitePieces[3], 0, 2, Piece.PieceType.W_Bishop);
        placePiece(whitePieces[4], 0, 3, Piece.PieceType.W_Queen);
        placePiece(whitePieces[5], 0, 4, Piece.PieceType.W_King);
        placePiece(whitePieces[3], 0, 5, Piece.PieceType.W_Bishop);
        placePiece(whitePieces[2], 0, 6, Piece.PieceType.W_Knight);
        placePiece(whitePieces[1], 0, 7, Piece.PieceType.W_Rook);

        //place the black pieces
        placePiece(blackPieces[1], 7, 0, Piece.PieceType.B_Rook);
        placePiece(blackPieces[2], 7, 1, Piece.PieceType.B_Knight);
        placePiece(blackPieces[3], 7, 2, Piece.PieceType.B_Bishop);
        placePiece(blackPieces[4], 7, 3, Piece.PieceType.B_Queen);
        placePiece(blackPieces[5], 7, 4, Piece.PieceType.B_King);
        placePiece(blackPieces[3], 7, 5, Piece.PieceType.B_Bishop);
        placePiece(blackPieces[2], 7, 6, Piece.PieceType.B_Knight);
        placePiece(blackPieces[1], 7, 7, Piece.PieceType.B_Rook);

    }

    public void placePiece(GameObject piece, int rank , int file, Piece.PieceType pieceType)
    {
        //instantiate the piece on the tile
        GameObject newPiece = Instantiate(piece, tiles[file, rank].transform.localPosition, Quaternion.identity, tiles[file, rank].transform);
        //set the z value to 1 so that the piece is on top of the tile
        newPiece.transform.localPosition = new Vector3(newPiece.transform.localPosition.x, newPiece.transform.localPosition.y, 1);
        //tag the piece with type of piece it is
        newPiece.tag = pieceType.ToString().ToUpper();
        //set the layer of the piece to 10 so that it is on top of the tile
        newPiece.layer = 10;
        //give the piece a box collider2D
        newPiece.AddComponent<BoxCollider2D>(); 
        //give the piece the piece script
        newPiece.AddComponent<Piece>();
        //set the type of the piece
        newPiece.GetComponent<Piece>().type = pieceType;
    }

    public void testmove()
    {
        //move the white pawn at A2 to A4
        tiles[0, 1].transform.GetChild(0).transform.parent = tiles[0, 3].transform;
    }

}
