using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReccomencerMemeNiveau : MonoBehaviour
{
   public GameObject joueur;

   public void Reccomencer()
	{
		joueur.transform.position = new Vector3(-0.852f, -0.226f, 0f);
	}
}
