using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class Controleur : MonoBehaviour
{
    private EditeurNiveau editeur;
    public bool appuye = false;
    public int identification;
    public int quantite;
    public TextMeshProUGUI quantiteTexte;

    // Start is called before the first frame update
    void Start()
    {
        quantiteTexte.text = quantite.ToString();
        editeur = GameObject.FindGameObjectWithTag("EditeurNiveau").GetComponent<EditeurNiveau>();
    }

    public void BoutonAppuye()
    {
        if (quantite > 0)
        {
            Vector2 positionEcran = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            Vector2 position = Camera.main.ScreenToWorldPoint(positionEcran);
            Instantiate(editeur.image[identification], new Vector3(position.x, position.y, 0), Quaternion.identity);

            appuye = true;
            quantite--;
            quantiteTexte.text = quantite.ToString();
            editeur.boutonAppuye = identification;

        }
    }
  
}
