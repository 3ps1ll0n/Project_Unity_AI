using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BouttonPause : MonoBehaviour
{
    public Camera camEditeur; // Caméra de l'éditeur, activée en mode pause
    public Camera camJeu; // Caméra du jeu, activée en mode jeu


    void Start()
    {
        Time.timeScale = 0.0f; // Met le jeu en pause au démarrage
    }

    /// <summary>
    /// Basculer entre pause et reprise du jeu et changer de caméra
    /// </summary>
    public void TogglePauseAndSwitchCamera()
    {
        // Bascule entre pause et reprise du jeu
        Time.timeScale = Mathf.Approximately(Time.timeScale, 0.0f) ? 1.0f : 0.0f;
        
        // Vérifie quelle caméra est active et bascule entre les caméras
        if (camEditeur.gameObject.activeSelf)
        {
            camEditeur.gameObject.SetActive(false); // Désactive la caméra de l'éditeur
            camJeu.gameObject.SetActive(true); // Active la caméra du jeu
        }
        else
        {
            camEditeur.gameObject.SetActive(true); // Active la caméra de l'éditeur
            camJeu.gameObject.SetActive(false); // Désactive la caméra du jeu
        }
    }
}

