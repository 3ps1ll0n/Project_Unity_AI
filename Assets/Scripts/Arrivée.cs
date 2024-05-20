using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrivée : MonoBehaviour
{
    public Camera camEditeur;
    public Camera camJeu;
    public GameObject ecranFin;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Time.timeScale = 0.0f;
            ecranFin.SetActive(true);
            camEditeur.gameObject.SetActive(true);
        camJeu.gameObject.SetActive(false);
        }

    }
}
