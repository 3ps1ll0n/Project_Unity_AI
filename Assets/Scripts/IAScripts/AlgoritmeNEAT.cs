using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;


public class AlgoritmeNEAT : MonoBehaviour
{
    public VueIA collecteDonne;
    public Mouvement mouvementJoueur;
    public string nomFichier = Directory.GetCurrentDirectory() + "\\defaultAI.xml";
    public int nombreIndividusParEspece;
    private int[,] vueIA = new int[1,1];
    private bool pause;
    private int imageSansProgresser = 0;
    private int IMAGE_SANS_PROGRESSER_MAX = 75;
    private double fitnessMaxIndividuActuelle = -int.MaxValue;
    private Vector3 dernierePos = default;
    private NEAT neat;
    // Start is called before the first frame update
    void Start(){}

    void FixedUpdate()//Ici, C'est où la classe NEAT va tester toute sa population
    {
        if(!collecteDonne.getIAActivee()) return;
        if(neat == default) {
            //neat = chargerNEAT();
            neat =  new NEAT(nombreIndividusParEspece, mouvementJoueur, collecteDonne.getTailleVue().x, collecteDonne.getTailleVue().y);
        }
        //if(neat == default) {return;}
        vueIA = collecteDonne.getVue();
        Vector3 arrive = collecteDonne.getPositionArrive();
        neat.resetAI();
        neat.passerDonneEntree(vueIA);
        neat.jouerDonneSortie();
        neat.calculerFitnessIAActuelle(collecteDonne.getPositionJoueur() , arrive);

        if(fitnessMaxIndividuActuelle < neat.avoirIAActive().getFitness()) {
            fitnessMaxIndividuActuelle = neat.avoirIAActive().getFitness();
            imageSansProgresser = 0;
        }else {
            imageSansProgresser++;
            if (imageSansProgresser > IMAGE_SANS_PROGRESSER_MAX) {
                collecteDonne.desactiverJoueur();
            }
        }
        //Debug.Log("Fitness : " + neat.getFitnessActive());

        

        if(collecteDonne.getJoueurMort()) {
            neat.passerProchainIndividu();
            collecteDonne.setPosJoueur(mouvementJoueur.positionInitiale);
            collecteDonne.activerJoueur();
            imageSansProgresser = 0;
            fitnessMaxIndividuActuelle = -int.MaxValue;
        }
        if(neat.doitReset){
            sauvegarderNEAT();
            neat.genererNouvelleGeneration();
            neat.doitReset = false;
        }
        dernierePos = collecteDonne.getPositionJoueur();
    }

    void OnGUI(){
        if(neat == default) return;
        /*GUIStyle style = new GUIStyle();
        style.fontSize = 30;
        GUI.Label(new Rect(0, 270, 150, 30), "Fitness Max Individu : " + fitnessMaxIndividuActuelle, style);
        GUI.Label(new Rect(0, 300, 150, 30), "Fitness : " + neat.getFitnessActive(), style);
        GUI.Label(new Rect(0,330, 150, 30), "Individu : " + (neat.getIdentifiantIAActive() + 1), style);
        GUI.Label(new Rect(0, 360, 150, 30), "Generation : " + neat.getGeneration(), style);
        GUI.Label(new Rect(0, 390, 150, 30), "Connexions : " + neat.avoirIAActive().getConnexions().Count, style);
        GUI.Label(new Rect(0, 510, 150, 30), "Vitesse temps : " + Time.timeScale, style);

        int somme = 0;*/
        
        /*for(int i = 0; i < vueIA.GetLength(0); i++){
            for(int j = 0; j < vueIA.GetLength(1); j++){
                somme += vueIA[i,j];
            }
        }
        double sommeCo = 0;
        for(int i = 0; i < neat.avoirIAActive().getConnexions().Count; i++){
            sommeCo += neat.avoirIAActive().getConnexions()[i].getPoids();
        }

        GUI.Label(new Rect(0, 420, 150, 30), "Somme Donne Entre : " + somme, style);
        GUI.Label(new Rect(0, 450, 150, 30), "Taille Input : " + vueIA.GetLength(0) * vueIA.GetLength(1), style);
        GUI.Label(new Rect(0, 480, 150, 30), "Poids Total : " + sommeCo, style);
        */
    }
    public void sauvegarderNEAT(){
        if (neat == null) { return;}
        try
        {
            XmlDocument xmlDocument = new XmlDocument();
            XmlSerializer serializer = new XmlSerializer(neat.GetType());
            using (MemoryStream stream = new MemoryStream())
            {
                serializer.Serialize(stream, neat);
                stream.Position = 0;
                xmlDocument.Load(stream);
                xmlDocument.Save(nomFichier);
            }
        }
        catch (Exception ex)
        {
            Debug.LogError("" + ex.Message);
        } 
    }

    /*public NEAT chargerNEAT(){
        if (string.IsNullOrEmpty(nomFichier) || !File.Exists(nomFichier)) { return new NEAT(nombreIndividusParEspece, mouvementJoueur, collecteDonne.getTailleVue().x, collecteDonne.getTailleVue().y); }
        NEAT objectOut = new NEAT(nombreIndividusParEspece, mouvementJoueur, collecteDonne.getTailleVue().x, collecteDonne.getTailleVue().y);
        try
        {
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load(nomFichier);
            string xmlString = xmlDocument.OuterXml;

            using (StringReader read = new StringReader(xmlString))
            {
                Type outType = typeof(NEAT);
                XmlSerializer serializer = new XmlSerializer(outType);
                using (XmlReader reader = new XmlTextReader(read))
                {
                    objectOut = (NEAT)serializer.Deserialize(reader);
                }
                Debug.Log("Fichier " + nomFichier + " a ete charge");
            }
        }
        catch (Exception ex)
        {
            Debug.LogError(ex.Message);
            return new NEAT(nombreIndividusParEspece, mouvementJoueur, collecteDonne.getTailleVue().x, collecteDonne.getTailleVue().y);
        }
        return objectOut;
    }*/
    public bool neatEstDefini(){return neat != default;}
    public bool[] getMouvementAJouer(){
        if (neat == default) return new bool[0];
        return neat.getMouvementAJouer();
    }
    public List<Connexion> getConexionsActuelle(){
        if (neat == default) return new List<Connexion>();
        return neat.avoirIAActive().getConnexions();
    }
    public int getNombreNeurones(){
        if (neat == default) return 0;
        return neat.avoirIAActive().getNeuronnes().Count;
    }
    public double getFitnessMaxIndividus(){
        if (neat == default) return 0;
        return fitnessMaxIndividuActuelle;
    }
    public double getFitnessActive(){
        if (neat == default) return 0;
        return neat.getFitnessActive();
    }
    public int getIdentifiantActif(){
        if (neat == default) return 0;
        return neat.getIdentifiantIAActive() + 1;
    }
    public int getGeneration(){
        if (neat == default) return 0;
        return neat.getGeneration();
    }
    public int getConnexions(){
        if (neat == default) return 0;
        return neat.avoirIAActive().getConnexions().Count;
    }
    public int getNbreNeuronnesCache(){
        if (neat == default) return 0;
        return neat.avoirIAActive().getNeuronnes().Count - (vueIA.GetLength(1) * vueIA.GetLength(0) + 3);
    }
    public double getMeilleurFitness(){
        if (neat == default) return 0;
        return neat.getMeilleurFitness();
    }

}
