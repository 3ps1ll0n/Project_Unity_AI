using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateformeTombante : MonoBehaviour
{
    private float delaiChute = 1f; // D�lai avant que la plateforme ne commence � tomber
    private float delaiDestruction = 2f; // D�lai avant que la plateforme ne soit d�truite apr�s avoir commenc� � tomber

    [SerializeField] private Rigidbody2D rb; // R�f�rence au composant Rigidbody2D de la plateforme

    /// <summary>
    /// OnCollisionEnter2D est appel� lorsque la plateforme entre en collision avec un autre objet 2D
    /// </summary>
    /// <param name="collision">Les d�tails de la collision</param>
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // V�rifie si l'objet en collision a le tag "Player"
        if (collision.gameObject.CompareTag("Player"))
        {
            StartCoroutine(Chute()); // Commence la coroutine pour faire tomber la plateforme
        }
    }

    /// <summary>
    /// Coroutine pour g�rer la chute de la plateforme
    /// </summary>
    private IEnumerator Chute()
    {
        yield return new WaitForSeconds(delaiChute); // Attend un d�lai avant de commencer � tomber
        rb.bodyType = RigidbodyType2D.Dynamic; // Change le type de corps du Rigidbody � Dynamique pour activer la physique
        Destroy(gameObject, delaiDestruction); // D�truit la plateforme apr�s un certain d�lai
    }
}
