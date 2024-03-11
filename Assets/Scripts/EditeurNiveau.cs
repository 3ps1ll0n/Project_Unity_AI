using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditeurNiveau : MonoBehaviour
{
    public GameObject[] image;
    public GameObject[] prefabs;
    public Controleur[] boutons;
    public int boutonAppuye;

    private void Update()
    {
        Vector2 positionEcran = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        Vector2 position = Camera.main.ScreenToWorldPoint(positionEcran);

        if (Input.GetMouseButtonDown(0) && boutons[boutonAppuye].appuye)
        {
            boutons[boutonAppuye].appuye = false;
            Instantiate(prefabs[boutonAppuye], new Vector3(position.x, position.y, 0), Quaternion.identity);

            Destroy(GameObject.FindGameObjectWithTag("image"));
        }
    
    }


}
