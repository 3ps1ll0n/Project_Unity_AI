using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class EditeurNiveau : MonoBehaviour
{
    public GameObject[] image; // Images transparentes pour visualiser l'apparence des items
    public GameObject[] prefabs; // Les GameObjects eux-mêmes à instancier
    public Controleur[] boutons; // Les boutons de contrôle pour sélectionner les items
    public Tilemap tilemap; // La tilemap où les items seront placés
    public int boutonAppuye; // Index du bouton actuellement appuyé


    private void Update()
    {
        // Récupère la position de la souris sur l'écran
        Vector2 positionEcran = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        // Convertit la position de l'écran en position dans le monde
        Vector2 position = Camera.main.ScreenToWorldPoint(positionEcran);

        // Vérifie si le bouton de la souris est enfoncé et si le bouton de contrôle est activé
        if (Input.GetMouseButtonDown(0) && boutons[boutonAppuye].appuye)
        {
            boutons[boutonAppuye].appuye = false; // Désactive le bouton de contrôle
            CreerDansTilemaps(prefabs[boutonAppuye], position); // Instancie le prefab dans la tilemap
            EnleverImageTemporaire(); // Supprime l'image temporaire
        }
    }

    /// <summary>
    /// Instancie le prefab à l'intérieur de la tilemap
    /// </summary>
    /// <param name="prefab">Le prefab à instancier</param>
    /// <param name="position">La position où instancier le prefab</param>
    private void CreerDansTilemaps(GameObject prefab, Vector2 position)
    {
        // Récupère la position de la cellule dans la tilemap
        Vector3Int positionCase = tilemap.WorldToCell(new Vector3(position.x, position.y, 0));

        // Récupère la position centrale de la cellule
        Vector3 centreCase = tilemap.GetCellCenterWorld(positionCase);

        // Instancie le prefab au centre de la cellule
        GameObject prefabInstancie = Instantiate(prefab, centreCase, Quaternion.identity);
        // Facultatif : Parent le prefab instancié à la tilemap pour des raisons d'organisation
        prefabInstancie.transform.SetParent(tilemap.transform);
    }

    /// <summary>
    /// Supprime l'image temporaire
    /// </summary>
    private void EnleverImageTemporaire()
    {
        GameObject imageObject = GameObject.FindGameObjectWithTag("image"); // Trouve l'objet avec le tag "image"
        if (imageObject != null)
        {
            Destroy(imageObject); // Supprime l'objet si trouvé
        }
    }
}

