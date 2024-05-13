using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlgoritmeNEAT : MonoBehaviour
{
    public VueIA collecteDonne;

    private int[,] vueIA;
    private bool pause;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        int[, ] vueIA = collecteDonne.getVue();
        Vector3 arrive = collecteDonne.getPositionArrive();
        if(arrive == default) Debug.Log("Veuillez Posez Un Drapeau");
    }
}
