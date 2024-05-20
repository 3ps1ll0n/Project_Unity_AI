using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuivreCamera : MonoBehaviour
{
    private Vector3 decalage = new Vector3(0f, 0f, -10f);
    private float temps = 0.25f;
    private Vector3 velocite = Vector3.zero;

    [SerializeField] private Transform cible;

    private void Update()
    {
        Vector3 positionCible = cible.position + decalage;
        transform.position = Vector3.SmoothDamp(transform.position, positionCible, ref velocite, temps);
    }
}
