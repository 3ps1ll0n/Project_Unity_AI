using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class SpikeScript : MonoBehaviour
{
    public int identification;
    private EditeurNiveau editeur;
    private Boolean detruit;
    // Start is called before the first frame update
    void Start()
    {
        detruit = false; 
        editeur = GameObject.FindGameObjectWithTag("EditeurNiveau").GetComponent<EditeurNiveau>();

    }

    void OnBecameInvisible()
    {
        if (!detruit)
        {
            Destroy(this.gameObject);
            editeur.boutons[identification].quantite++;
            editeur.boutons[identification].quantiteTexte.text = editeur.boutons[identification].quantite.ToString();
        }
    }

  

    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0))
        {
            detruit = true;
            Destroy(this.gameObject);
            editeur.boutons[identification].quantite++;
            editeur.boutons[identification].quantiteTexte.text = editeur.boutons[identification].quantite.ToString();
        }
    }
}
