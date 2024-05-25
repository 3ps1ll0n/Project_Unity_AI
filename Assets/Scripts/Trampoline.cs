using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trampoline : MonoBehaviour
{
    public Animator animator; // R�f�rence � l'animator pour jouer les animations
    [SerializeField] public float forceSaut = 2f; // Force du saut lorsqu'un joueur utilise le trampoline

    /// <summary>
    /// OnCollisionEnter2D est appel� lorsque le trampoline entre en collision avec un autre objet 2D
    /// </summary>
    /// <param name="collision">Les d�tails de la collision</param>
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // V�rifie si l'objet en collision a le tag "Player"
        if (collision.transform.CompareTag("Player"))
        {
            // Applique une force de saut vers le haut au joueur
            collision.gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.up * forceSaut;
            // Joue l'animation de saut
            animator.Play("Jump");
        }
    }
}
