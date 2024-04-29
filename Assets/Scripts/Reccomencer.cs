using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Reccomencer : MonoBehaviour
{
 public void ReccomencerJeu()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
