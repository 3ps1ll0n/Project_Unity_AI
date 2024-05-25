using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BouttonPause : MonoBehaviour
{
    public Camera camEditeur; // Cam�ra de l'�diteur, activ�e en mode pause
    public Camera camJeu; // Cam�ra du jeu, activ�e en mode jeu


    void Start()
    {
        Time.timeScale = 0.0f; // Met le jeu en pause au d�marrage
    }

    /// <summary>
    /// Basculer entre pause et reprise du jeu et changer de cam�ra
    /// </summary>
    public void TogglePauseAndSwitchCamera()
    {
        // Bascule entre pause et reprise du jeu
        Time.timeScale = Mathf.Approximately(Time.timeScale, 0.0f) ? 1.0f : 0.0f;
        
        // V�rifie quelle cam�ra est active et bascule entre les cam�ras
        if (camEditeur.gameObject.activeSelf)
        {
            camEditeur.gameObject.SetActive(false); // D�sactive la cam�ra de l'�diteur
            camJeu.gameObject.SetActive(true); // Active la cam�ra du jeu
        }
        else
        {
            camEditeur.gameObject.SetActive(true); // Active la cam�ra de l'�diteur
            camJeu.gameObject.SetActive(false); // D�sactive la cam�ra du jeu
        }
    }
}

