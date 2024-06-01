using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuivreCamera : MonoBehaviour
{
    private Vector3 decalage = new Vector3(0f, 0f, -10f);
    private float temps = 0.25f;
    private Vector3 velocite = Vector3.zero;

    [SerializeField] private Transform cible;

    /// <summary>
    /// Camera qui suit le personnage lorsque le mode jouer est activ�
    /// </summary>
    private void Update()
    {
        Vector3 positionCible = cible.position + decalage;
        transform.position = positionCible;
    }
}
