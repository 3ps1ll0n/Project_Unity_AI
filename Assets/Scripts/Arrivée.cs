using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrivée : MonoBehaviour
{
    public Camera camEditeur; // Caméra de l'éditeur à activer lorsque le joueur atteint l'arrivée
    public Camera camJeu; // Caméra du jeu à désactiver lorsque le joueur atteint l'arrivée
    public GameObject ecranFin; // Écran de fin à afficher lorsque le joueur atteint l'arrivée

    /// <summary>
    /// Déclenché lorsqu'un objet entre dans le trigger
    /// </summary>
    /// <param name="other">Le collider de l'objet entrant</param>
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Vérifie si l'objet entrant est le joueur
        if (other.CompareTag("Player"))
        {
            Time.timeScale = 0.0f; // Arrête le temps de jeu
            ecranFin.SetActive(true); // Affiche l'écran de fin
            camEditeur.gameObject.SetActive(true); // Active la caméra de l'éditeur
            camJeu.gameObject.SetActive(false); // Désactive la caméra de jeu
        }
    }
}

