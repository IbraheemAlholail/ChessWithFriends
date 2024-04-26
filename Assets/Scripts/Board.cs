using System.Collections;
using System.Collections.Generic;
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
    private GameObject[,] pieces = new GameObject[8, 8]; //array of objects, saved in this order: pawn, rook, knight, bishop, queen, king
    


    private void Start()
    {
        MakeBoard();
        Invoke("placePieces", 1);
        //wait 1 second then move 
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
                    //lock the transform so the tiles can't be moved

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
        //scale all pieces down 10%. then give them a new box collider2d
        for (int i = 0; i < whitePieces.Length; i++)
        {
            whitePieces[i].transform.localScale = new Vector3(0.9f, 0.9f, 0.9f);
            whitePieces[i].AddComponent<BoxCollider2D>();

            blackPieces[i].transform.localScale = new Vector3(0.9f, 0.9f, 0.9f);
            blackPieces[i].AddComponent<BoxCollider2D>();
        }


        for (int i = 0; i < 8; i++)
        {
            placePiece(whitePieces[0], 1, i, "White Pawn");
            placePiece(blackPieces[0], 6, i, "Black Pawn");
        }

        placePiece(whitePieces[1], 0, 0, "White Rook");
        placePiece(whitePieces[1], 0, 7, "White Rook");
        placePiece(blackPieces[1], 7, 0, "Black Rook");
        placePiece(blackPieces[1], 7, 7, "Black Rook");

        placePiece(whitePieces[2], 0, 1, "White Knight");
        placePiece(whitePieces[2], 0, 6, "White Knight");
        placePiece(blackPieces[2], 7, 1, "Black Knight");
        placePiece(blackPieces[2], 7, 6, "Black Knight");

        placePiece(whitePieces[3], 0, 2, "White Bishop");
        placePiece(whitePieces[3], 0, 5, "White Bishop");
        placePiece(blackPieces[3], 7, 2, "Black Bishop");
        placePiece(blackPieces[3], 7, 5, "Black Bishop");

        placePiece(whitePieces[4], 0, 3, "White Queen");
        placePiece(blackPieces[4], 7, 4, "Black Queen");

        placePiece(whitePieces[5], 0, 4, "White King");
        placePiece(blackPieces[5], 7, 3, "Black King");


    }

    public void placePiece(GameObject piece, int rank , int file, string pieceName)
    {
        //wait one second before placing the piece

        //instantiate the piece on the tile
        GameObject newPiece = Instantiate(piece, tiles[file, rank].transform.localPosition, Quaternion.identity, tiles[file, rank].transform);
        //set the z value to 1 so that the piece is on top of the tile
        newPiece.transform.localPosition = new Vector3(newPiece.transform.localPosition.x, newPiece.transform.localPosition.y, 1);
        //tag the piece with type of piece it is
        newPiece.tag = pieceName;
        newPiece.layer = 10;
    }

    public void testmove()
    {
        //move the white pawn at A2 to A4
        tiles[0, 1].transform.GetChild(0).transform.parent = tiles[0, 3].transform;
    }

}
