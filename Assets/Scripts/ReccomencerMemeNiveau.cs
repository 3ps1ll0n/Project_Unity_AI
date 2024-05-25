using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Remets le joueur à sa position initiale lorsqu'il reccomence le même niveau
public class ReccomencerMemeNiveau : MonoBehaviour
{
   public GameObject joueur;

   public void Reccomencer()
	{
		joueur.transform.position = new Vector3(-0.852f, -0.226f, 0f); //Renvoie le joueur à sa position initiale
	}
}
