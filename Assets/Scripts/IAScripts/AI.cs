using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Unity.Burst.Intrinsics;
using Unity.Mathematics;
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
    double pourentageIndividuTue = 0.7;
    double distanceMaxEntreIndividu = 2;
    static List<Connexion> inovations;
    public static Vector2 tailleVueIA;
    public static int NBRE_OUTPUT = 3;
    AI[] ais;
    Mouvement mouvementJoueur;

    /// <summary>
    /// Constructeur de la class NEAT
    /// </summary>
    public NEAT(int populationSize, Mouvement mouvementJoueur, int vueIAW, int vueIAH){
        this.populationSize = populationSize;
        ais = new AI[populationSize];
        inovations = new List<Connexion>();
        tailleVueIA.x = vueIAW;
        tailleVueIA.y = vueIAH;
        this.mouvementJoueur = mouvementJoueur;
        for(int i = 0; i < populationSize; i++){
            ais[i] = new AI();
        }
    }
    public void calculerFitnessIAActuelle(Vector3 pos, Vector3 posArrive){
        ais[iaActive].calculerFitness(pos, posArrive);
    }

    public void trierAI(){

    }
    public Dictionary<int, List<AI>> separerIaEnEspeces(){
        Dictionary<int, List<AI>> espece = new Dictionary<int, List<AI>>();
        for(int i = 0; i < ais.GetLength(0); i++){
            double distance;
            bool aEteClasse = false;
            for(int j = 1; j <= espece.Count; j++){
                distance = AIMathFunction.calculerDistanceIndividu(espece[j][0], ais[i]);
                if(distance < distanceMaxEntreIndividu){
                    espece[j].Add(ais[i]);
                    aEteClasse = true;
                    break;
                }
            }
            if(!aEteClasse){
                espece.Add(espece.Count + 1, new List<AI>());
                espece[espece.Count].Add(ais[i]);
            }
        }
        return espece;
    }
    public Dictionary<int, List<AI>> retirerIAFaible(Dictionary<int, List<AI>> touteLesEspeces){
        for(int i = 1; i <= touteLesEspeces.Count; i++){
            for(int j = 0; j < touteLesEspeces[i].Count; j++){
                AIMathFunction.corrigerFitness(touteLesEspeces[i][j], touteLesEspeces[i].Count); // Fitness Relative
            }
            var listTriee = AIMathFunction.trierListeAiFitness(touteLesEspeces[i]);
            int nbreATuer = (int)Math.Floor(listTriee.Count * pourentageIndividuTue);
            for(int k = 0; k < nbreATuer; k++){ // Tue les ia faibles
                listTriee.RemoveAt(0);
            }
        }
        return touteLesEspeces;
    }
    public void reproduireIndividuesElites(Dictionary<int, List<AI>> iaElites){
        List<double> sommesDesFitnessRelatives = new List<double>();
        List<int> individuesParEspeces = new List<int>();

        for(int i = 1; i <= iaElites.Count; i++){//Calcule la somme des fitness de la population d'une especes
            if(iaElites[i].Count == 0) Debug.LogWarning("ERROR, PAS POSSIBLE D'AVOIR 0 INDIVIDUS DANS UNE ESPECE");
            if(iaElites[i].Count > 1){
                sommesDesFitnessRelatives.Add(AIMathFunction.calculerFitnessTotalPopulation(iaElites[i]));
            } else {
                sommesDesFitnessRelatives.Add(0.0);
            }
        }

        double tot = sommesDesFitnessRelatives.Sum();
        
        for(int i = 0; i < sommesDesFitnessRelatives.Count; i++){//Determine combien d'individu il y aura dans chaque espece
            if(sommesDesFitnessRelatives[i] == 0.0) individuesParEspeces.Add(1);
            individuesParEspeces.Add((int)(sommesDesFitnessRelatives[i]/tot)); 
        }

        while(individuesParEspeces.Sum() < populationSize){//S'assure que le nombre d'individu reste constant
            int index = individuesParEspeces.IndexOf(individuesParEspeces.Max());
            individuesParEspeces[index] = individuesParEspeces[index] + 1;
        }

        Dictionary<int, List<AI>> nouvellePopulation = new Dictionary<int, List<AI>>();

        for(int i = 1; i <= iaElites.Count; i++){//! A FINIR
            nouvellePopulation.Add(i, new List<AI>());
            
        }

    }
    public void appliquerCrossover(AI iaElites1, AI iaElites2){

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
    public int getNombreInovation(){return innovNbre;}
    public bool estActive(){return active;}
    public void modifierPoids(double poids){this.poids = poids;}
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
    double fitnessCorrige;

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
        fitness = 200 - delta * 100;
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

        var NouvelleConnexionUne = new Connexion(1 , anciennePositionEntre, neuronnes.Count - 1);
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
        connexions[cIndex].modifierPoids(rand.NextDouble() * 2.0 * AIMathFunction.genererSigneAleatoire());
    }

    public void mutationChangerEtatCOnnexion(){
        if(connexions.Count == 0) return;
        var rand = new System.Random();
        var cIndex = rand.Next(0, connexions.Count());
        connexions[cIndex].setActive(!connexions[cIndex].estActive());
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
    public double getFitnessCorrige(){return fitnessCorrige;}
    public int getNombreConnection(){return connexions.Count();}
    public List<Connexion> getConnexions(){return connexions;}
    public void setDonneeEntree(int[,] donneeEntree){
        for(int i = 0; i < donneeEntree.GetLength(0); i++){
            for(int j = 0; j < donneeEntree.GetLength(1); j++){
                //Debug.Log(neuronnes.Count + " = " + donneeEntree.GetLength(1) * donneeEntree.GetLength(0));
                neuronnes[i * donneeEntree.GetLength(1) + j + VariablesGlobales.NBRE_DONNER_SORTIE].setValeur(donneeEntree[i,j]);
            }
        }
    }
    public void setFitnessCorrige(double cf){ fitnessCorrige = cf;}
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
    public static double calculerDistanceIndividu(AI ai1, AI  ai2){ //! Methodes vraiment importante
        double reponse;

        int maxGene = Math.Max(ai1.getNombreConnection(), ai2.getNombreConnection()); //Nombre de gene dans l'individu en contenant le plus
        int exces = Math.Abs(ai1.getNombreConnection() - ai2.getNombreConnection()); //Nombre de gene en exces

        var geneCoIa1 = trierListConnexion(ai1.getConnexions());
        var geneCoIa2 = trierListConnexion(ai2.getConnexions());

        int disjoint = 0;
        double differencePoids = 0;

        for(int i = 0; i < Math.Max(geneCoIa1.Count, geneCoIa2.Count); i++){
            bool estDisjoint = true;
            for(int j = 0; j < Math.Min(geneCoIa1.Count, geneCoIa2.Count); j++){
                if(geneCoIa1[i].getNombreInovation() == geneCoIa2[j].getNombreInovation()){//Calcule la différence de valeur entre les poids semblables
                    differencePoids += Math.Abs(geneCoIa1[i].getPoids() - geneCoIa2[j].getPoids());
                    estDisjoint = false;
                    break;
                }
            }
            if(estDisjoint) disjoint++; //Calcule le nombre de connexions disjointes
        }   
        //Formule du calcul de la distance entre les individues
        reponse = (IMPACT_EXCES * exces) / maxGene + (IMPACT_DISJOINT * disjoint) / maxGene + IMPACT_DIFFERENCES_POIDS * differencePoids;

        return reponse;
    }

    public static void corrigerFitness(AI ai, int taillePopulation){
        ai.setFitnessCorrige((double)(ai.getFitness()/taillePopulation));
    }

    public static double calculerFitnessTotalPopulation(List<AI> population){
        double somme = 0.0;

        foreach(AI ai in population) somme += ai.getFitnessCorrige();

        return somme;
    }

    static List<Connexion> trierListConnexion(List<Connexion> connexions){
        List<Connexion> list = new List<Connexion>();

        for(int i = 0; i < connexions.Count; i++){
            var c = connexions[i];
            bool aEtePlace = false;
            if(list.Count == 0) list.Add(c);
            else {
                for(int j = 0; j < list.Count; j++){
                    if(c.getNombreInovation() < list[j].getNombreInovation()){
                        list.Insert(j, c);
                        aEtePlace = true;
                        break;
                    }
                }
                if(!aEtePlace) list.Add(c);
            }
        }
        return list;
    }

    public static List<AI> trierListeAiFitness(List<AI> ais){
        List<AI> list = new List<AI>();

        for(int i = 0; i < ais.Count(); i++){
            var ai = ais[i];
            bool aEtePlace = false;
            if(list.Count == 0) list.Add(ai);
            else {
                for(int j = 0; j < list.Count; j++){
                    if(ai.getFitness() < list[j].getFitness()){
                        list.Insert(j, ai);
                        aEtePlace = true;
                        break;
                    }
                }
                if(!aEtePlace) list.Add(ai);
            }
        }

        return list;
    }

}

