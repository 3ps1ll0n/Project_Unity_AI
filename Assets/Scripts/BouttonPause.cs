using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BouttonPause : MonoBehaviour
{
    public Camera cam;
    public float taille = 2.01f;
    // Start is called before the first frame update
    void Start()
    {
       
        GetComponent<Button>().onClick.AddListener(TogglePause);
        Time.timeScale = 0.0f;
        
    }

    public void TogglePause()
    {
        Time.timeScale = Mathf.Approximately(Time.timeScale, 0.0f) ? 1.0f : 0.0f;
    }
    public void setTailleCameraJouer()
    {
        cam.orthographicSize = taille;
    }
    
    
    
}
