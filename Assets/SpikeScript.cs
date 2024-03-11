using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SpikeScript : MonoBehaviour
{
    public int identification;
    private EditeurNiveau editeur;
    // Start is called before the first frame update
    void Start()
    {
        editeur = GameObject.FindGameObjectWithTag("EditeurNiveau").GetComponent<EditeurNiveau>();

    }


    private void detruireObjet()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Destroy(this.gameObject);
            editeur.boutons[identification].quantite++;
            editeur.boutons[identification].quantiteTexte.text = editeur.boutons[identification].quantite.ToString();
        }
    }
}
