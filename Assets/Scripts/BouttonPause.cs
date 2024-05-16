using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BouttonPause : MonoBehaviour
{
 public Camera camEditeur;
 public Camera camJeu;

    // Start is called before the first frame update
    void Start()
    {
       
        Time.timeScale = 0.0f;
    }

    public void TogglePauseAndSwitchCamera()
    {
        // Toggle pause
        Time.timeScale = Mathf.Approximately(Time.timeScale, 0.0f) ? 1.0f : 0.0f;
          if (camEditeur.gameObject.activeSelf)
        {
            camEditeur.gameObject.SetActive(false);
            camJeu.gameObject.SetActive(true);
            
        }
        else
        {
            camEditeur.gameObject.SetActive(true);
            camJeu.gameObject.SetActive(false);
        }
    }
}

