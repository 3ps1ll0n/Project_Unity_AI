using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GameManager : MonoBehaviour {
    public static GameManager instance;

    public EtatJeu etat;

    public static event Action<EtatJeu> changementEtatJeu;

    void Awake(){
        instance = this;
    }

    void Start(){
        updateEtatJeu(EtatJeu.Menu);
    }
    
    public void updateEtatJeu(EtatJeu nouvelEtat){
        etat = nouvelEtat;

        switch (nouvelEtat){
            case EtatJeu.Menu: 
                break;
            case EtatJeu.Options:
                break;
            case EtatJeu.Jeu:
                break;
            case EtatJeu.EditeurNIveau:
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(nouvelEtat), nouvelEtat, null);

        }
        changementEtatJeu?.Invoke(nouvelEtat);
        
    }
   
}

public enum EtatJeu {
    Menu,
    Options,
    Jeu,
    EditeurNIveau
}
