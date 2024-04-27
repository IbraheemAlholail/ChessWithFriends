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

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, float.PositiveInfinity, draggableLayers);
            if (hit)
            {
<<<<<<< Updated upstream
                UnityEngine.Debug.Log("hit balls");

                dragging = hit.transform;
                offset = dragging.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);

=======
                dragging = hit.transform; // Start dragging
                dragging.position = new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y, -1);
                offset = dragging.position - Camera.main.ScreenToWorldPoint(Input.mousePosition); // Calculate the offset
            }
            else
            {
                UnityEngine.Debug.LogWarning("No Piece found");
>>>>>>> Stashed changes
            }
        }
        else if (Input.GetMouseButtonUp(0)) //stop dragging
        {
            dragging = null;
        }

        if (dragging != null) //move object
        {
            // Snap to the nearest grid position
            Vector3 snappedPosition = SnapToGrid(Camera.main.ScreenToWorldPoint(Input.mousePosition) + offset);

            // Get the tile under the snapped position
            GameObject tile = GetTileAtPosition(snappedPosition);

            if (tile != null && dragging != null)
            {
                // Make the dragged piece a child of the tile
                dragging.SetParent(tile.transform);
<<<<<<< Updated upstream
                dragging.localPosition = new Vector3(0,0,1); // Center the piece on the tile

                UnityEngine.Debug.Log("Piece dropped on tile: " + tile.name);
=======
                dragging.localPosition = new Vector3(0, 0, 1); // Center the piece on the tile
>>>>>>> Stashed changes
            }
            else if(dragging != null && tile == null)
            {
                UnityEngine.Debug.LogWarning("No tile found under dropped position.");
            }else
            {
                UnityEngine.Debug.LogWarning("No piece to drop.");
            }
<<<<<<< Updated upstream
=======
            dragging = null;            
        }

        if (dragging != null) //move object
        {
            dragging.position = Camera.main.ScreenToWorldPoint(Input.mousePosition) + offset;

>>>>>>> Stashed changes
        }
    }

    // Snap a given position to the nearest grid position
    private Vector3 SnapToGrid(Vector3 position)
    {
        float gridSize = 1.0f; // Adjust this according to your grid size
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
            return hit.transform.gameObject;
        }
        return null;
    }
}
