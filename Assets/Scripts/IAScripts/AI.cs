using System;
using System.Collections.Generic;
using Unity.Burst.Intrinsics;
using UnityEngine;
using UnityEngine.Windows.Speech;

enum TypesNeuronne{
    NULL,
    Entree,
    Sortie,
    Cache,
}

class NEAT{

    int populationSize;
    int generation;
    int iaActive = 0;
    double bestFitness;
    public static Vector2 tailleVueIA;
    public static int NBRE_OUTPUT = 3;
    AI[] ais;

    /// <summary>
    /// Constructeur de la class NEAT
    /// </summary>
    public NEAT(int populationSize, int vueIAW, int vueIAH){
        this.populationSize = populationSize;
        ais = new AI[populationSize];
        tailleVueIA.x = vueIAW;
        tailleVueIA.y = vueIAH;

        for(int i = 0; i < populationSize; i++){
            ais[i] = new AI();
        }
    }

    public void donnerEntree(int[, ] entree){
        ais[iaActive].avoirDonneEntre(entree);
    }
    public void calculerFitnessIAActuelle(Vector3 pos, Vector3 posArrive){
        ais[iaActive].calculerFitness(pos, posArrive);
    }
    public double getFitnessActive(){return ais[iaActive].getFitness();}
    /// <summary>
    /// Permet d'avoir l'IA qui joue
    /// </summary>
    AI avoirIAActive(){
        return ais[iaActive];
    }
}

class Connexion{
    double poids;
    int positionEntre;
    int positionSortie;
    double passeValeur(double donneEntree){
        return donneEntree * poids;
    }
}

class Neuronnes{
    double biais;
    double valeurStocké;
    TypesNeuronne type; 

    public Neuronnes(double b, TypesNeuronne tn){
        biais = b;
        type = tn;
        valeurStocké = 0;
    }

    public void setValeur(double v) {valeurStocké = v;}
}

class AI {
    List<Neuronnes> neuronnes = new List<Neuronnes>();
    double fitness;

    public AI(){
        for (int i = 0; i < NEAT.tailleVueIA.x * NEAT.tailleVueIA.y; i++)
        {
            neuronnes.Add(new Neuronnes(0, TypesNeuronne.Entree));
        }

        for (int i = 0; i < NEAT.NBRE_OUTPUT; i++){
            neuronnes.Add(new Neuronnes(0, TypesNeuronne.Entree));
        }
    }
    public void calculerFitness(Vector3 pos, Vector3 posArrive){
        if(posArrive == default) return;
        double delta = Math.Sqrt(Math.Pow(pos.x - posArrive.x, 2) + Math.Pow(pos.y - posArrive.y, 2));
        fitness = 1000* Math.Exp(-delta);
    }

    public void avoirDonneEntre(int[,] donne){
        for(int i = 0; i < NEAT.tailleVueIA.y; i++){
            for(int j = 0; j < NEAT.tailleVueIA.x; j++){
                neuronnes[j + i * (int)NEAT.tailleVueIA.x].setValeur(donne[i, j]);
            }
        }
    }

    void executerDonneSortie(){
        if(0 > 0.5){

        }
    }

    public double getFitness(){return fitness;}
}

class AIMathFunction {
    static double[] sigmoid(double[] coucheData){
        double[] nouvelleDonne = new double[coucheData.Length];

        for(int i = 0; i < coucheData.Length; i++){
            nouvelleDonne[i] = 1/(1 + Math.Exp(-coucheData[i]));
        }
        return nouvelleDonne;
    }
}

