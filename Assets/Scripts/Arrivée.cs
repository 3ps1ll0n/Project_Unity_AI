using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrivée : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Time.timeScale = 0.0f;
        }
    }
}
