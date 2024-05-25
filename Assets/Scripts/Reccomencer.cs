using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//Recharge la scène actuelle pour réinitialiser le niveau
public class Reccomencer : MonoBehaviour
{
    public void Awake()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void ReccomencerJeu()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
