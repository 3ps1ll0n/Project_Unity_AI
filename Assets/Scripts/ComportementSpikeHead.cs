using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Modifier le script pour qu'il ne remonte pas instantanément 
using UnityEngine;
using UnityEngine.Events;

public class Comportement : MonoBehaviour
{
    [Header("Attributs Spike Head")]
    private Vector3 destination;

    [SerializeField] private float vitesse;
    [SerializeField] private float portee;
    [SerializeField] private float delaisAttaque;
    [SerializeField] private float delaisRemonte;

    private float checkTimer;
    private float checkTimerRemonte;

    private bool descend;
    private bool remonte;

    private Vector3 positionInitiale;

    // Event to notify when something collides with the trap
    public UnityEvent onTrapActivated;

    private void Awake()
    {
        positionInitiale = this.transform.position;
    }

    private void Update()
    {
        // Bouger le spike head à sa destination
        if (descend)
        {
            transform.Translate(destination * Time.deltaTime * vitesse);
        }
        else
        {
            if (remonte)
            {
                checkTimerRemonte += Time.deltaTime;
                if (checkTimerRemonte > delaisRemonte)
                {
                    transform.Translate(Vector3.up * Time.deltaTime * vitesse);
                    checkTimer = 0;
                    if (this.transform.position.y >= positionInitiale.y)
                    {
                        remonte = false;
                        checkTimerRemonte = 0;
                    }
                }
            }
            checkTimer += Time.deltaTime;
            if (checkTimer > delaisAttaque)
            {
                descend = true;
                destination = -transform.up * portee;
                checkTimer = 0;
            }
        }
    }

    private void Stop()
    {
        descend = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Stop the trap when something collides with it
        Stop();
        remonte = true;

        // Trigger the event when something collides with the trap
        onTrapActivated.Invoke();
    }
}

