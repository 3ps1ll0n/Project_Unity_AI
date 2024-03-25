using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeTempScript : MonoBehaviour
{
 
   
    void Update()
    {
        Vector2 positionEcran = new Vector2(Input.mousePosition.x, Input.mousePosition.y); // Suivre la souris de l'utilisateur
        Vector2 position = Camera.main.ScreenToWorldPoint(positionEcran);
        transform.position = position;

    }
}
