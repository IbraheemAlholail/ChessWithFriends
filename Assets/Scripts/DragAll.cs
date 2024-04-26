using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class DragAll : MonoBehaviour
{ 
    private Transform dragging = null;
    private Vector3 offset;

    [SerializeField] private LayerMask draggableLayers;
    [SerializeField] private LayerMask boardTileLayers;
    [SerializeField] private float gridSize = 0.25f;

    GameObject tile;

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
                offset = dragging.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);

            }
        }
        else if (Input.GetMouseButtonUp(0)) //stop dragging
        {
            // Snap to the nearest grid position
            Vector3 snappedPosition = SnapToGrid(Camera.main.ScreenToWorldPoint(Input.mousePosition) + offset);

            // Get the tile under the snapped position
            tile = GetTileAtPosition(snappedPosition);

            if (tile != null)
            {
                // Make the dragged piece a child of the tile
                dragging.SetParent(tile.transform);
                dragging.localPosition = new Vector3(0, 0, 1); // Center the piece on the tile

                //UnityEngine.Debug.Log("Piece dropped on tile: " + tile.name);
            }
            else
            {
                UnityEngine.Debug.LogWarning("No tile found under dropped position.");
            }
            dragging = null;            
        }

        if (dragging != null) //move object
        {
            dragging.position = Camera.main.ScreenToWorldPoint(Input.mousePosition) + offset;
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
            if (hit.transform.childCount == 0)
            {
                return hit.transform.gameObject;
            }
            else
            {
                UnityEngine.Debug.LogWarning("Tile already has a piece on it.");
            }
        }
        return null;
    }
}
