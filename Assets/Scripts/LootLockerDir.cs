using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using LootLocker.Requests;

public class LootLockerDir : MonoBehaviour
{
    public void Jouer()
    {
        LootLockerSDKManager.StartGuestSession((reponse) =>{//Ouvrir la session dans lootlocker
            if (reponse.success)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);//load la scène de jeu
                Debug.Log("youpidou");
            }
            else
            {
                Debug.Log("pas réussi");//Avertissement si la connexion à lootlocker ne fonctionne pas
            }
        });
    }
}
