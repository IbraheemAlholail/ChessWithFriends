using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class DragAll : MonoBehaviour
{ 
    private Transform dragging = null;
    private GameObject ghostPiece = null;
    private Vector3 offset;

    [SerializeField] private LayerMask draggableLayers;
    [SerializeField] private LayerMask boardTileLayers;
    [SerializeField] private float gridSize = 0.25f;

    private GameObject tile;
    private GameObject originalTile;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            //Mouse location compared to the camera and checks for draggable objects
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, float.PositiveInfinity, draggableLayers);
            if (hit)
            {
                UnityEngine.Debug.Log("hit successful"); //debug for checking if the raycast is good

                dragging = hit.transform;

                // Get the original tile before detaching
                originalTile = dragging.parent != null ? dragging.parent.gameObject : null;

                // Detach from the original tile
                if (originalTile != null)
                {
                    dragging.SetParent(null);
                }

                CreateGhostPiece (dragging); //placement suggestion

                //center the piece on the mouse
                dragging.position = new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y, -2);
                offset = dragging.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);

            }
        }
        else if (Input.GetMouseButtonUp(0)) //stop dragging
        {
            // Snap to the nearest grid position
            Vector3 snappedPosition = SnapToGrid(Camera.main.ScreenToWorldPoint(Input.mousePosition) + offset);

            // Get the tile under the snapped position
            tile = GetTileAtPosition(snappedPosition);
            Destroy(ghostPiece);

            if (tile != null)
                {
                    if (tile == originalTile || tile.transform.childCount == 0)
                    {
                        // Make the dragged piece a child of the tile
                        dragging.SetParent(tile.transform);
                        dragging.localPosition = new Vector3(0, 0, 1); // Center the piece on the tile
                        //UnityEngine.Debug.Log("Piece dropped on tile: " + tile.name);
                    }
                    else
                    {
                        Transform existingPiece = tile.transform.GetChild(0);
                        if (IsOpposingPiece(dragging, existingPiece))
                        {
                            Destroy(existingPiece.gameObject);
                            dragging.SetParent(tile.transform);
                            dragging.localPosition = new Vector3(0, 0, 1); // Center the piece on the tile
                        }
                        else
                        {
                            UnityEngine.Debug.LogWarning("Cannot replace a friendly piece.");
                        }
                    }
                }
                else
                {
                    UnityEngine.Debug.LogWarning("No tile found under dropped position.");
                }
                dragging = null;
                originalTile = null; // Reset original tile
        }          
        


        if (dragging != null) //move object
        {
            dragging.position = Camera.main.ScreenToWorldPoint(Input.mousePosition) + offset;

            // Snap the ghost piece to the nearest grid position based on the mouse
            Vector3 ghostSnappedPosition = SnapToGrid(Camera.main.ScreenToWorldPoint(Input.mousePosition) + offset);
             // Get the tile under the ghost's snapped position
            GameObject tileUnderGhost = GetTileAtPosition(ghostSnappedPosition);

            if (tileUnderGhost != null)  //centre
            {
                ghostPiece.transform.position = tileUnderGhost.transform.position + new Vector3(0, 0, -1);
            }
            else
            {
                ghostPiece.transform.position = dragging.position;
            }
        
        }
    }

    private bool IsOpposingPiece(Transform draggedPiece, Transform existingPiece)
    {
        UnityEngine.Debug.Log($"Dragged Piece Tag: {draggedPiece.tag}, Existing Piece Tag: {existingPiece.tag}");

        return draggedPiece.tag != existingPiece.tag;
    }

    // Create a semi-transparent ghost version of the dragged piece
    private void CreateGhostPiece(Transform original)
    {
        ghostPiece = Instantiate(original.gameObject, original.position, Quaternion.identity);      
        SetPieceTransparency(ghostPiece, 0.5f); // Set transparency to 50%
    }

    // Set transparency of a piece
    private void SetPieceTransparency(GameObject piece, float alpha)
    {
        SpriteRenderer[] renderers = piece.GetComponentsInChildren<SpriteRenderer>();
        foreach (var renderer in renderers)
        {
            Color color = renderer.color;
            color.a = alpha;
            renderer.color = color;
        }
    }

    // Snap a given position to the nearest grid position
    private Vector3 SnapToGrid(Vector3 position)
    {
        float snappedX = Mathf.Round(position.x / gridSize) * gridSize;
        float snappedY = Mathf.Round(position.y / gridSize) * gridSize;
        return new Vector3(snappedX, snappedY, position.z);
    }

    // Get the tile at a given position
    private GameObject GetTileAtPosition(Vector3 position)
    {
        RaycastHit2D hit = Physics2D.Raycast(position, Vector2.zero, 0.1f, boardTileLayers);
        if (hit)
        {
            if (hit.transform.childCount == 0 || hit.transform.gameObject == originalTile) 
            {
                return hit.transform.gameObject; //empty position
            }
            else
            {
                return hit.transform.gameObject; //filled position
            }
        }
        return null;
    }
}
    

