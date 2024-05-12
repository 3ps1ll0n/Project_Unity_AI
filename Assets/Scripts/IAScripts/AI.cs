using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Burst.Intrinsics;
using UnityEngine;
using UnityEngine.Windows.Speech;

public class VariablesGlobales{
    public static int NBRE_DONNER_SORTIE = 3;
}

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
    GameObject joueur;

    /// <summary>
    /// Constructeur de la class NEAT
    /// </summary>
    public NEAT(int populationSize, GameObject joueur, int vueIAW, int vueIAH){
        this.populationSize = populationSize;
        ais = new AI[populationSize];
        tailleVueIA.x = vueIAW;
        tailleVueIA.y = vueIAH;
        this.joueur = joueur;
        for(int i = 0; i < populationSize; i++){
            ais[i] = new AI();
        }
    }

    public void calculerFitnessIAActuelle(Vector3 pos, Vector3 posArrive){
        ais[iaActive].calculerFitness(pos, posArrive);
    }

    public void trierAI(){

    }

    public void jouerDonneSortie(){
        bool[] donneSortie = ais[iaActive].getDonneSortie();

        if(donneSortie[0]); //Saute
        if(donneSortie[1]); //Gauche
        if(donneSortie[2]); //Droite

    }

    //*====================={GETTER ET SETTER}=====================

    public void donnerEntree(int[, ] entree){
        ais[iaActive].avoirDonneEntre(entree);
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
    public Connexion(double poids, int positionEntre, int positionSortie){}
    public double passeValeur(double donneEntree){
        return donneEntree * poids;
    }
    public int getPositionEntre(){return positionEntre;}
    public int getPositionSortie(){return positionSortie;}
}

class Neuronnes{
    double biais;
    double valeurStocke;
    TypesNeuronne type; 

    public Neuronnes(double b, TypesNeuronne tn){
        biais = b;
        type = tn;
        valeurStocke = 0;
    }

    public void setValeur(double v) {valeurStocke = v;}
    public double getValeurStocke(){return valeurStocke;}
    public TypesNeuronne GetTypesNeuronne(){return type;}
}

class AI {
    List<Neuronnes> neuronnes = new List<Neuronnes>();
    List<Connexion> connexions = new List<Connexion>();
    double fitness;

    public AI(){
        for(int i = 0; i < VariablesGlobales.NBRE_DONNER_SORTIE; i++){
            neuronnes.Append(new Neuronnes(0, TypesNeuronne.Sortie));
        }
        for (int i = 0; i < NEAT.tailleVueIA.x * NEAT.tailleVueIA.y; i++)
        {
            neuronnes.Append(new Neuronnes(0, TypesNeuronne.Entree));
        }

        for (int i = 0; i < NEAT.NBRE_OUTPUT; i++){
            neuronnes.Append(new Neuronnes(0, TypesNeuronne.Entree));
        }
    }
    //*========================={CALCUL}=========================
    public void calculerFitness(Vector3 pos, Vector3 posArrive){
        if(posArrive == default) return;
        double delta = Math.Sqrt(Math.Pow(pos.x - posArrive.x, 2) + Math.Pow(pos.y - posArrive.y, 2));
        fitness = 1000* Math.Exp(-delta);
    }

    public void calculerSortie(){
        foreach(Connexion connexion in connexions){
            neuronnes[connexion.getPositionSortie()].setValeur(connexion.passeValeur(neuronnes[connexion.getPositionEntre()].getValeurStocke()));
        }
    }

    //*========================={MUTATION}=========================

    void mutationAjouterConnexion(){
        var rand = new System.Random();

        int positionEntre = rand.Next(VariablesGlobales.NBRE_DONNER_SORTIE, (int)(NEAT.tailleVueIA.x * NEAT.tailleVueIA.y));
        int positionSortie = 0;


        if(VariablesGlobales.NBRE_DONNER_SORTIE + (int)(NEAT.tailleVueIA.x * NEAT.tailleVueIA.y) > neuronnes.Count){
            switch(rand.Next(0, 1)){
                case 0:
                    positionSortie = rand.Next(0, VariablesGlobales.NBRE_DONNER_SORTIE - 1);
                    break;
                case 1:
                    positionSortie = rand.Next((int)(NEAT.tailleVueIA.x * NEAT.tailleVueIA.y) + VariablesGlobales.NBRE_DONNER_SORTIE - 1, neuronnes.Count -1);
                    break;
            }
        } else {
            positionSortie = rand.Next(0, VariablesGlobales.NBRE_DONNER_SORTIE - 1);
        }

        connexions.Prepend(new Connexion(
            rand.NextDouble() * AIMathFunction.genererSigneAleatoire(), 
            positionEntre,
            positionSortie));

        if (neuronnes[positionSortie].GetTypesNeuronne() == TypesNeuronne.Cache){
            positionEntre = positionSortie;
            positionSortie = rand.Next(0, VariablesGlobales.NBRE_DONNER_SORTIE - 1);

            connexions.Append(new Connexion(
                rand.NextDouble() * AIMathFunction.genererSigneAleatoire(), 
                positionEntre,
                positionSortie));
        }
    }

    //*====================={GETTER ET SETTER}=====================
    public void avoirDonneEntre(int[,] donne){
        for(int i = 0; i < NEAT.tailleVueIA.y; i++){
            for(int j = 0; j < NEAT.tailleVueIA.x; j++){
                neuronnes[j + i * (int)NEAT.tailleVueIA.x].setValeur(donne[i, j]);
            }
        }
    }
    public bool[] getDonneSortie(){
        bool[] donneSortie = new bool[VariablesGlobales.NBRE_DONNER_SORTIE];

        for(int i = 0; i < donneSortie.GetLength(0); i++){
            donneSortie[i] = neuronnes[i].getValeurStocke() > 0.5;
        }

        return donneSortie;
    }
    public double getFitness(){return fitness;}
}

class AIMathFunction {
    public static int genererSigneAleatoire(){
        var rand = new System.Random();
        return rand.Next(0, 1) == 0 ?-11 : 1;
    }
    public static double[] sigmoid(double[] coucheData){
        double[] nouvelleDonne = new double[coucheData.Length];

        for(int i = 0; i < coucheData.Length; i++){
            nouvelleDonne[i] = 1/(1 + Math.Exp(-coucheData[i]));
        }
        return nouvelleDonne;
    }
}

