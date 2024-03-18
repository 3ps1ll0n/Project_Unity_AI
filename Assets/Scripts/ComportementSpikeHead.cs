using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Modifier le script pour qu'il remonte
public class Comportement : MonoBehaviour
{
[Header("Attributs Spike Head")]
 private Vector3 destination;
 
 [SerializeField]private float vitesse;
 [SerializeField]private float portee;
[SerializeField] private float delaisAttaque;

private float checkTimer;


 private bool attaque;


private Vector3 positionInitiale;


 private void Update(){

//Bouger le spike head à sa destination
    if(attaque){
        transform.Translate(destination * Time.deltaTime * vitesse);
    }
    else {
        
        checkTimer += Time.deltaTime;
        if (checkTimer > delaisAttaque){
            attaque = true;
            destination = -transform.up * portee;
            checkTimer = 0;
            
        }
    }
 
 }

private void Stop(){
    
    attaque = false;
}

private void OnTriggerEnter2D(){
    Stop();
    
}

}

