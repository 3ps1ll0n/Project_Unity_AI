using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeTempScript : MonoBehaviour
{
 
   /// <summary>
   /// Objet qui suit la souris temporairement avant que l'utilisateur le place
   /// </summary>
    void Update()
    {
        Vector2 positionEcran = new Vector2(Input.mousePosition.x, Input.mousePosition.y); // Suivre la souris de l'utilisateur
        Vector2 position = Camera.main.ScreenToWorldPoint(positionEcran);
        transform.position = position;

    }
}
