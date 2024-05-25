using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class EditeurNiveau : MonoBehaviour
{
    public GameObject[] image; // Images transparentes pour visualiser l'apparence des items
    public GameObject[] prefabs; // Les GameObjects eux-m�mes � instancier
    public Controleur[] boutons; // Les boutons de contr�le pour s�lectionner les items
    public Tilemap tilemap; // La tilemap o� les items seront plac�s
    public int boutonAppuye; // Index du bouton actuellement appuy�


    private void Update()
    {
        // R�cup�re la position de la souris sur l'�cran
        Vector2 positionEcran = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        // Convertit la position de l'�cran en position dans le monde
        Vector2 position = Camera.main.ScreenToWorldPoint(positionEcran);

        // V�rifie si le bouton de la souris est enfonc� et si le bouton de contr�le est activ�
        if (Input.GetMouseButtonDown(0) && boutons[boutonAppuye].appuye)
        {
            boutons[boutonAppuye].appuye = false; // D�sactive le bouton de contr�le
            CreerDansTilemaps(prefabs[boutonAppuye], position); // Instancie le prefab dans la tilemap
            EnleverImageTemporaire(); // Supprime l'image temporaire
        }
    }

    /// <summary>
    /// Instancie le prefab � l'int�rieur de la tilemap
    /// </summary>
    /// <param name="prefab">Le prefab � instancier</param>
    /// <param name="position">La position o� instancier le prefab</param>
    private void CreerDansTilemaps(GameObject prefab, Vector2 position)
    {
        // R�cup�re la position de la cellule dans la tilemap
        Vector3Int positionCase = tilemap.WorldToCell(new Vector3(position.x, position.y, 0));

        // R�cup�re la position centrale de la cellule
        Vector3 centreCase = tilemap.GetCellCenterWorld(positionCase);

        // Instancie le prefab au centre de la cellule
        GameObject prefabInstancie = Instantiate(prefab, centreCase, Quaternion.identity);
        // Facultatif : Parent le prefab instanci� � la tilemap pour des raisons d'organisation
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
            Destroy(imageObject); // Supprime l'objet si trouv�
        }
    }
}

