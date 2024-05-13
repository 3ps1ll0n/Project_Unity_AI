using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AlgoritmeNEAT : MonoBehaviour
{
    public VueIA collecteDonne;
    public Mouvement mouvementJoueur;

    private int[,] vueIA;
    private bool pause;
    [SerializeField] private NEAT neat;
    // Start is called before the first frame update
    void Start()
    {}

    void FixedUpdate()//Ici, C'est où la classe NEAT va tester toute sa population
    {
        if(!collecteDonne.getIAActivee()) return;
        if(neat == default) neat = new NEAT(30, mouvementJoueur, collecteDonne.getTailleVue().x, collecteDonne.getTailleVue().y);
        int[, ] vueIA = collecteDonne.getVue();
        Vector3 arrive = collecteDonne.getPositionArrive();
        neat.passerDonneEntree(vueIA);
        //if(arrive == default) Debug.Log("Veuillez Posez Un Drapeau");
        neat.jouerDonneSortie();
        neat.calculerFitnessIAActuelle(collecteDonne.getPositionJoueur() , arrive);
        Debug.Log("Fitness : " + neat.getFitnessActive());


    }
}
