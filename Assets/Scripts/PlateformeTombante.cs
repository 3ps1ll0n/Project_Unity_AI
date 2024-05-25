using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateformeTombante : MonoBehaviour
{
    private float delaiChute = 1f; // Délai avant que la plateforme ne commence à tomber
    private float delaiDestruction = 2f; // Délai avant que la plateforme ne soit détruite après avoir commencé à tomber

    [SerializeField] private Rigidbody2D rb; // Référence au composant Rigidbody2D de la plateforme

    /// <summary>
    /// OnCollisionEnter2D est appelé lorsque la plateforme entre en collision avec un autre objet 2D
    /// </summary>
    /// <param name="collision">Les détails de la collision</param>
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Vérifie si l'objet en collision a le tag "Player"
        if (collision.gameObject.CompareTag("Player"))
        {
            StartCoroutine(Chute()); // Commence la coroutine pour faire tomber la plateforme
        }
    }

    /// <summary>
    /// Coroutine pour gérer la chute de la plateforme
    /// </summary>
    private IEnumerator Chute()
    {
        yield return new WaitForSeconds(delaiChute); // Attend un délai avant de commencer à tomber
        rb.bodyType = RigidbodyType2D.Dynamic; // Change le type de corps du Rigidbody à Dynamique pour activer la physique
        Destroy(gameObject, delaiDestruction); // Détruit la plateforme après un certain délai
    }
}
