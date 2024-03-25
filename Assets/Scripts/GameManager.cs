using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public sealed class GameManager : MonoBehaviour {
    private static GameManager _instance;

    public static GameManager Instance {
        get
        {
            if (_instance is null)
                Debug.LogError("Le GameManager est NULL!");
            
            return _instance;
            
        }
    }


    public static EtatJeu etat;

    void Awake(){
        _instance = this;
    }

    void Start(){
        UpdateEtatJeu(EtatJeu.Menu);
    }
    
    public void UpdateEtatJeu(EtatJeu nouvelEtat){
        etat = nouvelEtat;

        switch (nouvelEtat){
            case EtatJeu.Menu: ChangerScene(0);
                break;
            case EtatJeu.Options: ChangerScene(1);
                break;
            case EtatJeu.Jeu: ChangerScene(2);
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
