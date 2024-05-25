using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using LootLocker.Requests;

public class LootLockerDir : MonoBehaviour
{
    /// <summary>
    /// Lancer le jeu
    /// </summary>
    public void Jouer()
    {

        LootLockerSDKManager.StartGuestSession((reponse) => {//Ouvrir la session dans lootlocker
            if (reponse.success)
            {
                GameManager.instance.UpdateEtatJeu(EtatJeu.Jeu);;//load la scene de jeu
            }
            else
            {
                Debug.Log("pas reussi");//Avertissement si la connexion � lootlocker ne fonctionne pas
            }
        });
    }

    public void Credits()
    {
        GameManager.instance.UpdateEtatJeu(EtatJeu.Credits);
    }
    public void retourMenu()
    {
         GameManager.instance.UpdateEtatJeu(EtatJeu.Menu);
    }

    
    public void Quitter()
    {
        Application.Quit();
    }
    
}