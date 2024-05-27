using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class Controleur : MonoBehaviour
{
    private EditeurNiveau editeur;
    public bool appuye = false;
    public int identification; //suivre quel type d'objet on ajoute
    public TextMeshProUGUI quantiteTexte; //texte

 
    void Start()
    {
        editeur = GameObject.FindGameObjectWithTag("EditeurNiveau").GetComponent<EditeurNiveau>();
    }

    //Ajouter un objet
    public void BoutonAppuye()
    {
        //AudioManager.instance.JouerBruitage("Clic");

        Vector2 positionEcran = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        Vector2 position = Camera.main.ScreenToWorldPoint(positionEcran);
        Instantiate(editeur.image[identification], new Vector3(position.x, position.y, 0), Quaternion.identity);

        appuye = true;
        editeur.boutonAppuye = identification;

      
    }
  

}
