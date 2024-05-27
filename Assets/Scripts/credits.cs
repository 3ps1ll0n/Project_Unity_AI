using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Credits : MonoBehaviour
{

    private float vitesse = 50;

    private Vector3 translation = new Vector3(0,1,0);

    public Transform position;
    

    /// <summary>
    /// Update en montant les crédits selon un translation
    /// </summary>
    void Update()
    {
        if(Input.GetKey(KeyCode.Escape)){
            GameManager.instance.UpdateEtatJeu(EtatJeu.Menu);
        }

        if(position.localPosition.y > 1050){
            GameManager.instance.UpdateEtatJeu(EtatJeu.Menu);
        }

        position.Translate(translation*Time.deltaTime*vitesse);
    
    }
}
