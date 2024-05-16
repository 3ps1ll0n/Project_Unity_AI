using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BouttonPause : MonoBehaviour
{
    public Camera camEditeur;
    public Camera camJeu;
    public float taille = 2.01f;
    private bool isCamEditeurActive = true;

    // Start is called before the first frame update
    void Start()
    {
        camEditeur.gameObject.SetActive(true);
        camJeu.gameObject.SetActive(false);
        GetComponent<Button>().onClick.AddListener(TogglePauseAndSwitchCamera);
        Time.timeScale = 0.0f;
    }

    public void TogglePauseAndSwitchCamera()
    {
        // Toggle pause
        Time.timeScale = Mathf.Approximately(Time.timeScale, 0.0f) ? 1.0f : 0.0f;

        // Switch cameras
        isCamEditeurActive = !isCamEditeurActive;
        camEditeur.gameObject.SetActive(isCamEditeurActive);
        camJeu.gameObject.SetActive(!isCamEditeurActive);
        if (!isCamEditeurActive)
        {
            camJeu.orthographicSize = taille;
        }
    }
}

