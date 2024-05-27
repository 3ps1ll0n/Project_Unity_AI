using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;



public class AlgoritmeNEAT : MonoBehaviour
{
    public VueIA collecteDonne;
    public Mouvement mouvementJoueur;
    public string nomFichier = "defaultAI.xml";
    public int nombreIndividusParEspece;
    public static Vector2 tailleVueIA;
    public static int NBRE_OUTPUT = 3;
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
            tailleVueIA = new Vector3(collecteDonne.getTailleVue().x, collecteDonne.getTailleVue().y);
            neat = chargerNEAT();
            if(!neat.aEteInitialise) neat.initNEAT(nombreIndividusParEspece);
            neat.setMouvement(mouvementJoueur);
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
            //sauvegarderNEAT();
            neat.genererNouvelleGeneration();
            neat.doitReset = false;
        }
        dernierePos = collecteDonne.getPositionJoueur();
    }

    /// <summary>
    /// Sert a sauvegardé dans un fichier xml l'algithme actuel
    /// </summary>
    private void sauvegarderNEAT() {
        System.Xml.Serialization.XmlSerializer x = new System.Xml.Serialization.XmlSerializer(neat.GetType());
        //Stream s = new Stream(nomFichier, "write");
        StreamWriter writer = File.CreateText(nomFichier);
        x.Serialize(writer, neat);
        Console.WriteLine();
        Console.ReadLine();
        writer.Close();
    }
    /// <summary>
    /// Sert a charger un algorithme enregistré
    /// </summary>
    /// <returns>Un algorithme</returns>
    private NEAT chargerNEAT(){
        if(!File.Exists(nomFichier)) return new NEAT();
        var mySerializer = new XmlSerializer(typeof(NEAT));
        using var myFileStream = new FileStream(nomFichier, FileMode.Open);
        var myObject = (NEAT)mySerializer.Deserialize(myFileStream);
        return myObject;
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
