using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class SpikeScript : MonoBehaviour
{
    public int identification; // Pour suivre quel objet on est en train d'ajouter
    private EditeurNiveau editeur;
    private Boolean detruit;
  
    void Start()
    {
        detruit = false; 
        editeur = GameObject.FindGameObjectWithTag("EditeurNiveau").GetComponent<EditeurNiveau>(); //Retrouver le bon objet

    }



  
    
    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0)) //Clique gauche
        {
            detruit = true;
            supprimerObjet();
        }
    }

    private void supprimerObjet()
    {
        if (this.gameObject != null)
        {
            Destroy(this.gameObject);
        }
    }
}
