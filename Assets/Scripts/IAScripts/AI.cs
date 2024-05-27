using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


[Serializable]
public enum TypesNeuronne{
    NULL,
    Entree,
    Sortie,
    Cache,
}
[Serializable]
public class NEAT{
    public int populationSize;
    public int generation;
    public int iaActive = 0;
    public double bestFitness;
    public double pourentageIndividuTue = 0.85;
    public double distanceMaxEntreIndividu = 2;
    public static List<Connexion> inovations;
    public bool doitReset = false;
    public bool aEteInitialise = false;
    
    public int NBRE_OUTPUT = 3;

    public AI[] ais;
    private Mouvement mouvementJoueur;

    /// <summary>
    /// Constructeur de la class NEAT
    /// </summary>
    public NEAT(){}
    public void initNEAT(int populationSize){
        this.populationSize = populationSize;
        ais = new AI[populationSize];
        iaActive = 0;
        inovations = new List<Connexion>();
        aEteInitialise = true;
        for(int i = 0; i < populationSize; i++){
            ais[i] = new AI();
        }
    }
    public void resetAI(){
        for(int i = 0; i < ais[iaActive].getNeuronnes().Count; i++){
            ais[iaActive].getNeuronnes()[i].resetValeur();
        }
    }
    public void passerProchainIndividu(){
        if(ais[iaActive].getFitness() > bestFitness) bestFitness = ais[iaActive].getFitness();
        iaActive++;
        if(iaActive >= populationSize){
            doitReset = true;
            iaActive = 0;
        }
    }
    public void genererNouvelleGeneration(){
        reproduireIndividuesElites(retirerIAFaible(separerIaEnEspeces()));

        foreach(AI ai in ais){
            appliquerMutationAleatoire(ai);
        }
        generation++;
    }
    public void calculerFitnessIAActuelle(Vector3 pos, Vector3 posArrive){
        ais[iaActive].calculerFitness(pos, posArrive);
    }

    public void trierAI(){

    }
    /// <summary>
    /// Classer ia en espèce
    /// </summary>
    /// <returns>Dictionnaire des ia classé par espèces</returns>

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
    /// <summary>
    /// Selection naturelle
    /// </summary>
    /// <param name="touteLesEspeces">Dictionnaire des différentes espèces</param>
    /// <returns></returns>
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
    /// <summary>
    /// Choisir qui va se reproduire avec qui
    /// </summary>
    /// <param name="iaElites"></param>
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
        int sommeRestante = populationSize;

        for(int i = 0; i < sommesDesFitnessRelatives.Count; i++)  {
            if(sommesDesFitnessRelatives[i] == 0.0) {
                sommeRestante--;
            }
        }
        //Debug.Log(sommeRestante + " - " + generation);
        //Debug.LogWarning(sommesDesFitnessRelatives.Sum() + " | " + tot + " / Count : " + sommesDesFitnessRelatives.Count + "  - " + generation);
        for(int i = 0; i < sommesDesFitnessRelatives.Count; i++){//Determine combien d'individu il y aura dans chaque espece
            if(sommesDesFitnessRelatives[i] == 0.0){
                individuesParEspeces.Add(1);
            } else {
                individuesParEspeces.Add((int)(Math.Floor((sommesDesFitnessRelatives[i]/tot)*sommeRestante))); 
                //Debug.Log("fr : " + sommesDesFitnessRelatives[i] + "  - " + generation);
                //Debug.LogWarning(((double)sommesDesFitnessRelatives[i]/tot)*100.0 + "% | " + Math.Floor((sommesDesFitnessRelatives[i]/tot)*sommeRestante) + "  - " + generation);
            }
        }
        //Debug.LogWarning("Somme : " + individuesParEspeces.Sum());
        //Debug.Log(individuesParEspeces.Sum());
        //Debug.Log(individuesParEspeces.ToString());
        while(individuesParEspeces.Sum() < populationSize){//S'assure que le nombre d'individu reste constant
            int index = individuesParEspeces.IndexOf(individuesParEspeces.Max());
            individuesParEspeces[index] = individuesParEspeces[index] + 1;
        }

        Dictionary<int, List<AI>> nouvellePopulation = new Dictionary<int, List<AI>>();

        for(int i = 1; i <= iaElites.Count; i++){
            var rand = new System.Random();
            nouvellePopulation.Add(i, new List<AI>());
            for(int j = 0; j < individuesParEspeces[i - 1]; j++){
                int index1 = rand.Next(0, iaElites[i].Count());
                int index2;
                if (iaElites[i].Count() == 1){
                    nouvellePopulation[i].Add(iaElites[i][0]);
                }
                else {
                    do{
                        index2 = rand.Next(0, iaElites[i].Count());
                    }while(index2 == index1);
                    nouvellePopulation[i].Add(appliquerCrossover(iaElites[i][index1], iaElites[i][index2]));
                }
            }
        }
        ais = new AI[populationSize];
        int k = 0;
        for(int i = 1; i <= nouvellePopulation.Count; i++){
            for(int j = 0; j < nouvellePopulation[i].Count; j++){
                //Debug.Log(" valeur de k : " + k + " | ind par espece : " + individuesParEspeces.Sum());
                ais[k] = nouvellePopulation[i][j];
                k++;
            }
        }
        if(k != ais.Count()) Debug.LogError("PAS ASSEZ D'ÉLÉMENTS CRÉÉES");

    }
    /// <summary>
    /// Faire se reproduire deux ia
    /// </summary>
    /// <param name="iaElites1">IA 1</param>
    /// <param name="iaElites2">IA 2</param>
    /// <returns></returns>
    AI appliquerCrossover(AI iaElites1, AI iaElites2){
        AI nouvelleIA = new AI();

        var c1 = AIMathFunction.trierListConnexion(iaElites1.getConnexions());
        var c2 = AIMathFunction.trierListConnexion(iaElites2.getConnexions());

        if(c1.Count == 0 && c2.Count == 0){
            return new AI();
        }

        if(c1.Count == 0) return iaElites2;
        else if (c2.Count == 0) return iaElites1;
        else {
            for(int i = 0; i < c1.Count; i++){
                for(int j = 0; j < c2.Count; j++){
                //Debug.Log(i + " : " + c1.Count + " | " + j + " : " + c2.Count);
                    if(c1[i].getNombreInovation() == c2[j].getNombreInovation()){
                        Connexion co = new Connexion();
                        co.initConnexions((
                            iaElites1.getFitness() < iaElites2.getFitness()) ? c1[i].getPoids() : c2[j].getPoids(),
                            c1[i].getPositionEntre(),
                            c1[i].getPositionSortie()
                        );
                        
                        nouvelleIA.addConnexion(co);
                        nouvelleIA.ajouterNeuronne(c1[i].getPositionSortie());
                        nouvelleIA.ajouterNeuronne(c1[i].getPositionEntre());
                        c1.RemoveAt(i);
                        c2.RemoveAt(j);
                        //j--;
                        //i--;
                        if(c2.Count == j || c1.Count == i)break;
                    }
                }
                if(c1.Count == i) break;
            }
        }

        for(int i = 0; i < c1.Count(); i++){
            nouvelleIA.addConnexion(c1[i]);
            nouvelleIA.ajouterNeuronne(c1[i].getPositionSortie());
            nouvelleIA.ajouterNeuronne(c1[i].getPositionEntre());
        }
        for(int i = 0; i < c2.Count(); i++){
            nouvelleIA.addConnexion(c2[i]);
            nouvelleIA.ajouterNeuronne(c2[i].getPositionSortie());
            nouvelleIA.ajouterNeuronne(c2[i].getPositionEntre());
        }

        return nouvelleIA;
    }

    public void appliquerMutationAleatoire(AI ai){
        var rand = new System.Random();
        int nbreTentative = 1;
        for(int i = 0;i < nbreTentative; i++){
            if(rand.NextDouble() < 0.3){
                var nbre = rand.Next(0, 10000);
                if(nbre < 3000){
                    ai.mutationAjouterConnexion();
                    //Debug.Log("Connexion ajouté");
                }
                else if(nbre < 3010){
                    ai.mutationAjouterNeuronne();
                    //Debug.Log("Neurone ajouté");
                }
                else if(nbre < 3500){
                    ai.mutationChangerEtatConnexion();
                    //Debug.Log("Etat Connexion changé");
                }
                else if(nbre < 9500){
                    ai.mutationModifierPoids();
                    //Debug.Log("Poids Connexion Modifié");
                }  
            }
        }
    }
    /// <summary>
    /// Ajoute un lien dans la liste des innovations si il n'a jamais été découvert
    /// </summary>
    /// <param name="connexion"></param>
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
    public static Connexion[] getInovationList(){
        return inovations.ToArray();
    }
    public bool[] getMouvementAJouer(){
        return ais[iaActive].getDonneSortie();
    }
    public void donnerEntree(int[, ] entree){
        ais[iaActive].avoirDonneEntre(entree);
    }
    public double getFitnessActive(){return ais[iaActive].getFitness();}
    public int getIdentifiantIAActive(){
        return iaActive;
    }
    public int getGeneration(){
        return generation;
    }
    public double getMeilleurFitness () {return bestFitness;}
    /// <summary>
    /// Permet d'avoir l'IA qui joue
    /// </summary>
    public AI avoirIAActive(){
        return ais[iaActive];
    }
    public void setMouvement(Mouvement m){
        mouvementJoueur = m;
    }
}
//*============================{CLASS POUR IA}============================
[Serializable]
public class Connexion{
    public double poids;
    public int positionEntre;
    public int positionSortie;
    public int innovNbre;
    public bool active = true;
    public Connexion(){}
    public void initConnexions(double poids, int positionEntre, int positionSortie){
        this.poids = poids;
        this.positionEntre = positionEntre;
        this.positionSortie = positionSortie;
    }
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
[Serializable]
public class Neuronnes{
    public double valeurStocke;
    public TypesNeuronne type; 

    public Neuronnes(){
        valeurStocke = 0;
    }
    public void resetValeur(){valeurStocke = 0;}
    public void ajouterValeur(double v) {valeurStocke += v;}
    public double getValeurStocke(){return valeurStocke;}
    public TypesNeuronne GetTypesNeuronne(){return type;}
}
[Serializable]
public class AI {
    public List<Neuronnes> neuronnes = new List<Neuronnes>();
    public List<Connexion> connexions = new List<Connexion>();
    public int espece;
    public double fitness;
    public double fitnessCorrige;

    public AI(){
        for (int i = 0; i < AlgoritmeNEAT.NBRE_OUTPUT; i++){
            neuronnes.Add(new Neuronnes());
        }
        for (int i = 0; i < AlgoritmeNEAT.tailleVueIA.x * AlgoritmeNEAT.tailleVueIA.y; i++)
        {
            neuronnes.Add(new Neuronnes());
            //Debug.Log("Neuronnes Entree");
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
            //if(connexion.getPositionSortie() >= connexions.Count) neuronnes.Add(new Neuronnes(TypesNeuronne.Cache));
            if(connexion.estActive()) neuronnes[connexion.getPositionSortie()].ajouterValeur(connexion.passeValeur(neuronnes[connexion.getPositionEntre()].getValeurStocke()));
        }
    }

    //*========================={METHODES}=========================

    public void ajouterNeuronne(int index){
        if(index >= neuronnes.Count()) neuronnes.Add(new Neuronnes());
    }

    //*========================={MUTATION}=========================
    /// <summary>
    /// Ajoute une connection entre deux neuronnes
    /// </summary>
    public void mutationAjouterConnexion(){
        var rand = new System.Random();
        Connexion connexion;
        int positionEntre = rand.Next(AlgoritmeNEAT.NBRE_OUTPUT, (int)(AlgoritmeNEAT.tailleVueIA.x * AlgoritmeNEAT.tailleVueIA.y));
        int positionSortie = 0;


        if(AlgoritmeNEAT.NBRE_OUTPUT + (int)(AlgoritmeNEAT.tailleVueIA.x * AlgoritmeNEAT.tailleVueIA.y) < neuronnes.Count){
            switch(rand.Next(0, 2)){
                case 0:
                    positionSortie = rand.Next(0, AlgoritmeNEAT.NBRE_OUTPUT);
                    break;
                case 1:
                    positionSortie = rand.Next((int)(AlgoritmeNEAT.tailleVueIA.x * AlgoritmeNEAT.tailleVueIA.y) + AlgoritmeNEAT.NBRE_OUTPUT, neuronnes.Count);
                    break;
            }
        } else {
            positionSortie = rand.Next(0, AlgoritmeNEAT.NBRE_OUTPUT);
        }
        connexion = new Connexion();
        connexion.initConnexions(
                                    rand.NextDouble() * AIMathFunction.genererSigneAleatoire(), 
                                    positionEntre,
                                    positionSortie
                                );
        
        connexions.Insert(0, connexion);

        if (neuronnes[positionSortie].GetTypesNeuronne() == TypesNeuronne.Cache){
            positionEntre = positionSortie;
            positionSortie = rand.Next(0, AlgoritmeNEAT.NBRE_OUTPUT);
            
            connexion = new Connexion();
            connexion.initConnexions(
                                        rand.NextDouble() * AIMathFunction.genererSigneAleatoire(), 
                                        positionEntre,
                                        positionSortie
                                    );
            connexions.Add(connexion);
        }
        NEAT.ajouterSiInovation(connexion);
    }
    /// <summary>
    /// Ahoute un neuronne entre deux connexion
    /// </summary>
    public void mutationAjouterNeuronne(){
        if(connexions.Count == 0) return;
        var rand = new System.Random();
        int cIndex = rand.Next(0, connexions.Count());

        neuronnes.Add(new Neuronnes());

        int anciennePositionEntre = connexions[cIndex].getPositionEntre();
        int anciennePositionSortie = connexions[cIndex].getPositionSortie();
        double ancienPoids = connexions[cIndex].getPoids();

        var NouvelleConnexionUne = new Connexion();
        var NouvelleConnexionDeux = new Connexion();

        NouvelleConnexionUne.initConnexions(1 , anciennePositionEntre, neuronnes.Count() - 1);
        NouvelleConnexionDeux.initConnexions(ancienPoids, neuronnes.Count() - 1, anciennePositionSortie);

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
    /// <summary>
    /// Changer le poids d'une connexion
    /// </summary>
    public void mutationModifierPoids(){
        if(connexions.Count == 0) return;
        var rand = new System.Random();
        var cIndex = rand.Next(0, connexions.Count());
        connexions[cIndex].modifierPoids(rand.NextDouble() * 2.0 * AIMathFunction.genererSigneAleatoire());
    }
    /// <summary>
    /// Choisi si on allume ou eteint une connection
    /// </summary>
    public void mutationChangerEtatConnexion(){
        if(connexions.Count == 0) return;
        var rand = new System.Random();
        var cIndex = rand.Next(0, connexions.Count());
        connexions[cIndex].setActive(!connexions[cIndex].estActive());
    }

    //*====================={GETTER ET SETTER}=====================
    public void avoirDonneEntre(int[,] donne){
        for(int i = 0; i < AlgoritmeNEAT.tailleVueIA.y; i++){
            for(int j = 0; j < AlgoritmeNEAT.tailleVueIA.x; j++){
                neuronnes[j + i * (int)AlgoritmeNEAT.tailleVueIA.x].ajouterValeur(donne[i, j]);
            }
        }
    }
    public bool[] getDonneSortie(){
        calculerSortie();
        bool[] donneSortie = new bool[AlgoritmeNEAT.NBRE_OUTPUT];
        for(int i = 0; i < donneSortie.GetLength(0); i++){
            donneSortie[i] = neuronnes[i].getValeurStocke() > 0;
        }

        return donneSortie;
    }
    public double getFitness(){return fitness;}
    public double getFitnessCorrige(){return fitnessCorrige;}
    public int getNombreConnection(){return connexions.Count();}
    public List<Connexion> getConnexions(){return connexions;}
    public List<Neuronnes> getNeuronnes(){return neuronnes;}
    public void setDonneeEntree(int[,] donneeEntree){
        for(int i = 0; i < donneeEntree.GetLength(0); i++){
            for(int j = 0; j < donneeEntree.GetLength(1); j++){
                //Debug.Log(neuronnes.Count + " = " + donneeEntree.GetLength(1) * donneeEntree.GetLength(0));
                neuronnes[i * donneeEntree.GetLength(1) + j + AlgoritmeNEAT.NBRE_OUTPUT].ajouterValeur(donneeEntree[i,j]);
            }
        }
    }
    public void setFitnessCorrige(double cf){ fitnessCorrige = cf;}

    public void addConnexion(Connexion connexion){
        connexions.Add(connexion);
    }
}

class AIMathFunction {
    //*=============================={CONSTANTES}==============================
    private const double IMPACT_EXCES = 0.3;
    private const double IMPACT_DISJOINT = 0.3;
    private const double IMPACT_DIFFERENCES_POIDS = 0.05;
    //*==============================={METHODES}===============================
    public static int genererSigneAleatoire(){
        var rand = new System.Random();
        return rand.Next(0, 2) == 0 ? -1 : 1;
    }
    /// <summary>
    /// Sert a appliquer une fonction sigmoid sur un ensemble de donné
    /// Au final, on en aura pas besoin de cette fonction
    /// </summary>
    /// <param name="coucheData"> la couche de neuronne final</param>
    /// <returns> des valeurs lissé</returns>
    public static double[] sigmoid(double[] coucheData){
        double[] nouvelleDonne = new double[coucheData.Length];

        for(int i = 0; i < coucheData.Length; i++){
            nouvelleDonne[i] = 1/(1 + Math.Exp(-coucheData[i]));
        }
        return nouvelleDonne;
    }
    /// <summary>
    /// Formule pour calculer la similitude entre deux individus
    /// </summary>
    /// <param name="ai1">Première IA</param>
    /// <param name="ai2">Deuxième IA</param>
    /// <returns></returns>
    public static double calculerDistanceIndividu(AI ai1, AI  ai2){ //! Methodes vraiment importante
        double reponse;

        int maxGene = Math.Max(ai1.getNombreConnection(), ai2.getNombreConnection()); //Nombre de gene dans l'individu en contenant le plus
        int exces = Math.Abs(ai1.getNombreConnection() - ai2.getNombreConnection()); //Nombre de gene en exces

        if(maxGene == 0) return 0;

        var geneCoIa1 = trierListConnexion(ai1.getConnexions());
        var geneCoIa2 = trierListConnexion(ai2.getConnexions());

        int disjoint = 0;
        double differencePoids = 0;

        for(int i = 0; i < geneCoIa1.Count; i++){
            bool estDisjoint = true;
            for(int j = 0; j < geneCoIa2.Count; j++){
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
        //Debug.Log("Distance : " + reponse);
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

    public static List<Connexion> trierListConnexion(List<Connexion> connexions){
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

