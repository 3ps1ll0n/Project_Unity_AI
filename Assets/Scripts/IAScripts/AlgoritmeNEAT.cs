using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AlgoritmeNEAT : MonoBehaviour
{
    public VueIA collecteDonne;

    private int[,] vueIA;
    private bool pause;
    [SerializeField] private NEAT neat;
    // Start is called before the first frame update
    void Start()
    {}
    // Update is called once per frame
    void Update()
    {
        if(!collecteDonne.getIAActivee()) return;
        if(neat == default) neat = new NEAT(10, collecteDonne.getJoueur(), collecteDonne.getTailleVue().x, collecteDonne.getTailleVue().y);
        int[, ] vueIA = collecteDonne.getVue();
        Vector3 arrive = collecteDonne.getPositionArrive();
        if(arrive == default) Debug.Log("Veuillez Posez Un Drapeau");
        neat.calculerFitnessIAActuelle(collecteDonne.getPositionJoueur() , arrive);
        Debug.Log("" + neat.getFitnessActive() + ": 0.01");
    }
}
