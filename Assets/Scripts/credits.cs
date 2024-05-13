using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class credits : MonoBehaviour
{

    private float vitesse = 50;

    private Vector3 translation = new Vector3(0,1,0);

    public Transform position;
    

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.Escape)){
            GameManager.instance.UpdateEtatJeu(EtatJeu.Menu);
        }



        position.Translate(translation*Time.deltaTime*vitesse);

    
    }
}
