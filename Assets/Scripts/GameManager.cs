using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
    public static GameManager instance;
    private EtatJeu etat;

    private void Awake(){
        Time.timeScale = 0;
        //Créer Singleton
        if (instance == null){
            
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else{
            Destroy(gameObject);
        }
        
    }

    
    public void UpdateEtatJeu(EtatJeu nouvelEtat){
        etat = nouvelEtat;

        switch (nouvelEtat){
            case EtatJeu.Menu: ChangerScene(0);
                break;
            case EtatJeu.Options: ChangerScene(2);
                break;
            case EtatJeu.Jeu:{ 
                ChangerScene(1);

                    Time.timeScale = 0;
                AudioManager.instance.sourceMusique.Stop();
                AudioManager.instance.JouerMusique("Jeu"+UnityEngine.Random.Range(1,4));

            }
                break;
            case EtatJeu.EditeurNiveau: ChangerScene(3);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(nouvelEtat), nouvelEtat, null);

        }
    }
    private void ChangerScene(int numero){
        SceneManager.LoadScene(numero);
    }
   
}



public enum EtatJeu {
    Menu,
    Options,
    Jeu,
    EditeurNiveau
}
