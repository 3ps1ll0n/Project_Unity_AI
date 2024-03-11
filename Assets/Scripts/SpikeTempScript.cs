using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeTempScript : MonoBehaviour
{
   

    // Update is called once per frame
    void Update()
    {
        Vector2 positionEcran = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        Vector2 position = Camera.main.ScreenToWorldPoint(positionEcran);
        transform.position = position;

    }
}
