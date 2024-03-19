using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Modifier le script pour qu'il ne remonte pas instantanément 
public class Comportement : MonoBehaviour
{
[Header("Attributs Spike Head")]
 private Vector3 destination;
 
 [SerializeField]private float vitesse;
 [SerializeField]private float portee;
[SerializeField] private float delaisAttaque;

private float checkTimer;


 private bool descend;
private bool remonte;


private Vector3 positionInitiale;

    private void Awake()
    {
        positionInitiale = this.transform.position;
    }

    private void Update(){

//Bouger le spike head à sa destination
    if(descend){
            
        transform.Translate(destination * Time.deltaTime * vitesse);
    }
    else {
        if (remonte)
            {
                transform.Translate(Vector3.up * Time.deltaTime * vitesse);
                checkTimer = 0;
                if (this.transform.position.y >= positionInitiale.y) { remonte = false; }
            }
        checkTimer += Time.deltaTime;
        if (checkTimer > delaisAttaque){
            descend = true;
            destination = -transform.up * portee;
            checkTimer = 0;
            
        }
    }
 
 }

private void Stop(){
    
    descend = false;
}

private void OnTriggerEnter2D(){
    Stop();
        
        remonte = true;
    
}

}

