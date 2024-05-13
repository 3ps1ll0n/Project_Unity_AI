using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
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
    static List<Connexion> inovations;
    public static Vector2 tailleVueIA;
    public static int NBRE_OUTPUT = 3;
    AI[] ais;
    Mouvement mouvementJoueur;

    /// <summary>
    /// Constructeur de la class NEAT
    /// </summary>
    public NEAT(int populationSize, Mouvement mouvementJoeur, int vueIAW, int vueIAH){
        this.populationSize = populationSize;
        ais = new AI[populationSize];
        inovations = new List<Connexion>();
        tailleVueIA.x = vueIAW;
        tailleVueIA.y = vueIAH;
        this.mouvementJoueur = mouvementJoeur;
        for(int i = 0; i < populationSize; i++){
            ais[i] = new AI();
        }
    }
    public void calculerFitnessIAActuelle(Vector3 pos, Vector3 posArrive){
        ais[iaActive].calculerFitness(pos, posArrive);
    }

    public void trierAI(){

    }
    public void separerIaEnEspeces(){
        Dictionary<int, List<Connexion>> espece = new Dictionary<int, List<Connexion>>();
    }
    public void retirerIAFaible(){

    }
    public void appliquerCrossover(){

    }
    public void appliquerMutationAleatoire(){

    }
    public static void ajouterSiInovation(Connexion connexion){
        for(int i = 0; i < inovations.Count(); i++){
            var c = inovations[i];
            if(c.getPositionEntre() == connexion.getPositionEntre() && c.getPositionSortie() == connexion.getPositionSortie()){
                connexion.setNombreInovation(i + 1);
                return;
            }
        }
        connexion.setNombreInovation(inovations.Count());
        inovations.Add(connexion);
    }
    public void jouerDonneSortie(){
        bool[] donneSortie = ais[iaActive].getDonneSortie();
        //Test. . .
        /*donneSortie[0] = true;
        donneSortie[1] = true;
        donneSortie[2] = true;*/
        if(donneSortie[0]) mouvementJoueur.sauter(); //Saute
        if(donneSortie[1]) mouvementJoueur.bougerDroite(); //Droite
        if(donneSortie[2]) mouvementJoueur.bougerGauche(); //Gauche
    }

    public void passerDonneEntree(int[,] donneEntree){
        ais[iaActive].setDonneeEntree(donneEntree);
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
//*============================{CLASS POUR IA}============================
class Connexion{
    double poids;
    int positionEntre;
    int positionSortie;
    int innovNbre;
    bool active = true;
    public Connexion(double poids, int positionEntre, int positionSortie){}
    public double passeValeur(double donneEntree){
        return donneEntree * poids;
    }
    public int getPositionEntre(){return positionEntre;}
    public int getPositionSortie(){return positionSortie;}
    public double getPoids() {return poids;}
    public bool estActive(){return active;}
    public void modifierPoids(double poidsAjoute){poids += poidsAjoute;}
    public void setNombreInovation(int innovNbre){this.innovNbre = innovNbre;}
    public void setActive(bool estActive){this.active = estActive;}
}
class Neuronnes{
    double biais;
    double valeurStocke;
    TypesNeuronne type; 

    public Neuronnes(TypesNeuronne tn){
        type = tn;
        valeurStocke = 0;
    }

    public void setValeur(double v) {valeurStocke = v;}
    public double getValeurStocke(){return valeurStocke;}
    public void modifierBiais(double valeurAjoutee){biais += valeurAjoutee;}
    public TypesNeuronne GetTypesNeuronne(){return type;}
}

class AI {
    List<Neuronnes> neuronnes = new List<Neuronnes>();
    List<Connexion> connexions = new List<Connexion>();
    int espece;
    double fitness;

    public AI(){
        for (int i = 0; i < NEAT.tailleVueIA.x * NEAT.tailleVueIA.y; i++)
        {
            neuronnes.Add(new Neuronnes(TypesNeuronne.Entree));
            //Debug.Log("Neuronnes Entree");
        }
        for (int i = 0; i < NEAT.NBRE_OUTPUT; i++){
            neuronnes.Add(new Neuronnes(TypesNeuronne.Sortie));
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
            if(connexion.estActive())neuronnes[connexion.getPositionSortie()].setValeur(connexion.passeValeur(neuronnes[connexion.getPositionEntre()].getValeurStocke()));
        }
    }

    //*========================={MUTATION}=========================

    public void mutationAjouterConnexion(){
        var rand = new System.Random();
        Connexion connexion;

        int positionEntre = rand.Next(VariablesGlobales.NBRE_DONNER_SORTIE, (int)(NEAT.tailleVueIA.x * NEAT.tailleVueIA.y));
        int positionSortie = 0;


        if(VariablesGlobales.NBRE_DONNER_SORTIE + (int)(NEAT.tailleVueIA.x * NEAT.tailleVueIA.y) > neuronnes.Count){
            switch(rand.Next(0, 1)){
                case 0:
                    positionSortie = rand.Next(0, VariablesGlobales.NBRE_DONNER_SORTIE);
                    break;
                case 1:
                    positionSortie = rand.Next((int)(NEAT.tailleVueIA.x * NEAT.tailleVueIA.y) + VariablesGlobales.NBRE_DONNER_SORTIE - 1, neuronnes.Count);
                    break;
            }
        } else {
            positionSortie = rand.Next(0, VariablesGlobales.NBRE_DONNER_SORTIE);
        }
        connexion = new Connexion(
                                    rand.NextDouble() * AIMathFunction.genererSigneAleatoire(), 
                                    positionEntre,
                                    positionSortie);
        connexions.Insert(0, connexion);

        if (neuronnes[positionSortie].GetTypesNeuronne() == TypesNeuronne.Cache){
            positionEntre = positionSortie;
            positionSortie = rand.Next(0, VariablesGlobales.NBRE_DONNER_SORTIE);
            
            connexion = new Connexion(
                                        rand.NextDouble() * AIMathFunction.genererSigneAleatoire(), 
                                        positionEntre,
                                        positionSortie);
            connexions.Add(connexion);
        }
        NEAT.ajouterSiInovation(connexion);
    }

    public void mutationAjouterNeuronne(){
        if(connexions.Count == 0) return;
        var rand = new System.Random();
        int cIndex = rand.Next(0, connexions.Count());

        neuronnes.Add(new Neuronnes(TypesNeuronne.Cache));

        int anciennePositionEntre = connexions[cIndex].getPositionEntre();
        int anciennePositionSortie = connexions[cIndex].getPositionSortie();
        double ancienPoids = connexions[cIndex].getPoids();

        var NouvelleConnexionUne = new Connexion(ancienPoids, anciennePositionEntre, neuronnes.Count - 1);
        var NouvelleConnexionDeux = new Connexion(ancienPoids, neuronnes.Count - 1, anciennePositionSortie);

        NEAT.ajouterSiInovation(NouvelleConnexionUne);
        NEAT.ajouterSiInovation(NouvelleConnexionDeux);

        connexions.Insert(0, NouvelleConnexionUne);
        connexions.Add(NouvelleConnexionDeux);

        connexions[cIndex].setActive(false);
    }

    /*public void mutationModifierBiais(){
        if(neuronnes.Count == 0) return;
        var rand = new System.Random();
        var nIndex = rand.Next(0, neuronnes.Count());
        neuronnes[nIndex].modifierBiais(rand.NextDouble() * 0.5 * AIMathFunction.genererSigneAleatoire());
    }*/

    public void mutationModifierPoids(){
        if(connexions.Count == 0) return;
        var rand = new System.Random();
        var cIndex = rand.Next(0, connexions.Count());
        connexions[cIndex].modifierPoids(rand.NextDouble() * 0.5 * AIMathFunction.genererSigneAleatoire());
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
    public int getNombreConnection(){return connexions.Count();}

    public void setDonneeEntree(int[,] donneeEntree){
        for(int i = 0; i < donneeEntree.GetLength(0); i++){
            for(int j = 0; j < donneeEntree.GetLength(1); j++){
                //Debug.Log(neuronnes.Count + " = " + donneeEntree.GetLength(1) * donneeEntree.GetLength(0));
                neuronnes[i * donneeEntree.GetLength(1) + j + VariablesGlobales.NBRE_DONNER_SORTIE].setValeur(donneeEntree[i,j]);
            }
        }
    }
}

class AIMathFunction {
    //*=============================={CONSTANTES}==============================
    private const double IMPACT_EXCES = 1;
    private const double IMPACT_DISJOINT = 1;
    private const double IMPACT_DIFFERENCES_POIDS = 1;
    //*==============================={METHODES}===============================
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
    public static double calculerDistanceIndividu(AI ai1, AI  ai2){
        double reponse = 0.0;

        int maxGene = Math.Max(ai1.getNombreConnection(), ai2.getNombreConnection());



        int exces = 0;
        int disjoint = 0;
        double differencePoids = 0;
 

        reponse = (IMPACT_EXCES * exces) / maxGene + (IMPACT_DISJOINT * disjoint) / maxGene + IMPACT_DIFFERENCES_POIDS * differencePoids;

        return reponse;
    }
}

