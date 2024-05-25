using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Comportement : MonoBehaviour
{
    [Header("Attributs Spike Head")]
    private Vector3 destination; // La destination vers laquelle se déplace le Spike Head

    [SerializeField] private float vitesse; // La vitesse de déplacement du Spike Head
    [SerializeField] private float portee; // La portée de l'attaque du Spike Head
    [SerializeField] private float delaisAttaque; // Le délai entre les attaques du Spike Head
    [SerializeField] private float delaisRemonte; // Le délai avant que le Spike Head ne remonte après une attaque

    private float checkTimer; // Timer pour gérer le délai entre les attaques
    private float checkTimerRemonte; // Timer pour gérer le délai de remontée

    private bool descend; // Indique si le Spike Head descend pour attaquer
    private bool remonte; // Indique si le Spike Head remonte après une attaque

    private Vector3 positionInitiale; // La position initiale du Spike Head


    private void Awake()
    {
        positionInitiale = this.transform.position; // Enregistre la position initiale du Spike Head
    }


    private void Update()
    {
        if (descend)
        {
            // Déplace le Spike Head vers la destination
            transform.Translate(destination * Time.deltaTime * vitesse);
        }
        else
        {
            if (remonte)
            {
                checkTimerRemonte += Time.deltaTime; // Incrémente le timer de remontée
                if (checkTimerRemonte > delaisRemonte)
                {
                    // Déplace le Spike Head vers le haut
                    transform.Translate(Vector3.up * Time.deltaTime * vitesse);
                    checkTimer = 0;
                    if (this.transform.position.y >= positionInitiale.y)
                    {
                        remonte = false; // Arrête la remontée si la position initiale est atteinte
                        checkTimerRemonte = 0;
                    }
                }
            }
            checkTimer += Time.deltaTime; // Incrémente le timer d'attaque
            if (checkTimer > delaisAttaque)
            {
                descend = true; // Commence à descendre pour attaquer
                destination = -transform.up * portee; // Calcule la destination de l'attaque
                checkTimer = 0;
            }
        }
    }

    /// <summary>
    /// Arrête la descente du Spike Head
    /// </summary>
    private void Stop()
    {
        descend = false;
    }


    private void OnTriggerEnter2D()
    {
        AudioManager.instance.JouerBruitage("Boom"); // Joue un bruitage lorsqu'un objet entre dans le trigger
        Stop(); // Arrête la descente
        remonte = true; // Commence à remonter
    }
}

