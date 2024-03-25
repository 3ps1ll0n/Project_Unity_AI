using System.Collections;
using UnityEngine.Events;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGame(){
        SceneManager.LoadScene(1);
        //GameManager.Instance.UpdateEtatJeu(EtatJeu.Jeu);
    }

    public void AfficherOptions(){
        GameManager.Instance.UpdateEtatJeu(EtatJeu.Options);
    }

    public void Quitter(){
        Application.Quit();
    }
}
