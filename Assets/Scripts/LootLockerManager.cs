using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using LootLocker.Requests;

public class LootLockerDir : MonoBehaviour
{
    public void Jouer()
    {

        LootLockerSDKManager.StartGuestSession((reponse) => {//Ouvrir la session dans lootlocker
            if (reponse.success)
            {
                GameManager.instance.UpdateEtatJeu(EtatJeu.Jeu);;//load la sc�ne de jeu
            }
            else
            {
                Debug.Log("pas r�ussi");//Avertissement si la connexion � lootlocker ne fonctionne pas
            }
        });
    }

    public void Options()
    {
        GameManager.instance.UpdateEtatJeu(EtatJeu.Options);
    }
    public void retourMenu()
    {
         GameManager.instance.UpdateEtatJeu(EtatJeu.Menu);
    }

      

    }
    public void Quitter()
    {
        Application.Quit();
    }
    
}