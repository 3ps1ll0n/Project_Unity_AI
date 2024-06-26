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

    /// <summary>
    /// Changer la scène du jeu selon le nom
    /// </summary>
    public void UpdateEtatJeu(EtatJeu nouvelEtat){
        etat = nouvelEtat;

        switch (nouvelEtat){
            case EtatJeu.Menu:{
                 ChangerScene(0);

                AudioManager.instance.sourceMusique.Stop();
                AudioManager.instance.JouerMusique("Menu");

            
            }
                break;

            case EtatJeu.Jeu:{ 
                ChangerScene(1);

                    Time.timeScale = 0;
                AudioManager.instance.sourceMusique.Stop();
                AudioManager.instance.JouerMusique("Jeu"+UnityEngine.Random.Range(1,4));

            }
                break;
                
            case EtatJeu.Credits: {
                Time.timeScale = 1;
                ChangerScene(2);
            }
                break;

            default:
                throw new ArgumentOutOfRangeException(nameof(nouvelEtat), nouvelEtat, null);

        }
    }
    
    private void ChangerScene(int numero){
        SceneManager.LoadScene(numero);
    }
   
}


/// <summary>
/// Enum des états du jeu possibles
/// </summary>
public enum EtatJeu {
    Menu,
    Credits,
    Jeu
}
