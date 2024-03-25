using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine;
using UnityEngine.Tilemaps;

public class EditeurNiveau : MonoBehaviour
{
    public GameObject[] image; //Images plus transparentes pour imaginer de quoi l'item a l'air
    public GameObject[] prefabs; //le GameObject lui-même
    public Controleur[] boutons;
    public Tilemap tilemap;
    public int boutonAppuye;

    private void Update()
    {
        Vector2 positionEcran = new Vector2(Input.mousePosition.x, Input.mousePosition.y); //suivre
        Vector2 position = Camera.main.ScreenToWorldPoint(positionEcran);

        if (Input.GetMouseButtonDown(0) && boutons[boutonAppuye].appuye)
        {
            boutons[boutonAppuye].appuye = false;
            InstantiateInsideTilemap(prefabs[boutonAppuye], position); // Instantiate prefab inside the Tilemap
            RemoveTemporaryImage(); // Remove the temporary image
        }
    }

    // Method to instantiate prefab inside the Tilemap
    private void InstantiateInsideTilemap(GameObject prefab, Vector2 position)
    {
        // Get the cell position in the Tilemap
        Vector3Int cellPosition = tilemap.WorldToCell(new Vector3(position.x, position.y, 0));

        // Get the world position of the cell center
        Vector3 cellCenter = tilemap.GetCellCenterWorld(cellPosition);

        // Instantiate the prefab at the center of the tile
        GameObject instantiatedPrefab = Instantiate(prefab, cellCenter, Quaternion.identity);
        // Optional: Parent the instantiated prefab to the Tilemap for organizational purposes
        instantiatedPrefab.transform.SetParent(tilemap.transform);
    }

    // Method to remove the temporary image
    private void RemoveTemporaryImage()
    {
        GameObject imageObject = GameObject.FindGameObjectWithTag("image");
        if (imageObject != null)
        {
            Destroy(imageObject);
        }
    }
}


