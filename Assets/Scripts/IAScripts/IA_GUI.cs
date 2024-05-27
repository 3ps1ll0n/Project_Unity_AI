using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;
using UnityEngine.UIElements;
using System;

public class IA_GUI : MonoBehaviour
{
    public int tailleAffichageCellule;
    public VueIA vue;
    public AlgoritmeNEAT neat;
    public Material mat;
    public int decalageX;
    public int decalageY;
    int[,] vueIA;
    Texture2D pixelNoir;
    Texture2D pixelBlanc;
    Texture2D pixelRouge;
    Texture2D pixelBleu;
    Texture2D pixelJaune;
    Texture2D pixelBordure;
    GUIStyle stylePixelNoir;
    GUIStyle stylePixelBlanc;
    GUIStyle stylePixelRouge;
    GUIStyle stylePixelBleu;
    GUIStyle stylePixelJaune;
    GUIStyle stylePixelBordure;

    int HAUTEUR_DONNE_SORTIE = 50;
    int LARGEUR_DONNE_SORTIE = 150;
    int DECALAGE_NEURONNE_SORTIE = 200;
    int DECALAGE_NEURONNES_CACHES = 100;
    string[] bouton = {"Saut", "Droite", "Gauche"};

    void Start(){
        pixelNoir = new Texture2D(1, 1); 
        pixelNoir.SetPixel(0, 0, new Color(0, 0, 0, 0.5f));
        pixelNoir.Apply();
        stylePixelNoir = new GUIStyle();
        stylePixelNoir.normal.background = pixelNoir;

        pixelBlanc = new Texture2D(1, 1);
        pixelBlanc.SetPixel(0, 0, new Color(255, 255, 255, 0.5f));
        pixelBlanc.Apply();
        stylePixelBlanc = new GUIStyle();
        stylePixelBlanc.normal.background = pixelBlanc;

        pixelRouge = new Texture2D(1, 1);
        pixelRouge.SetPixel(0, 0, new Color(255, 0, 0, 0.5f));
        pixelRouge.Apply();
        stylePixelRouge = new GUIStyle();
        stylePixelRouge.normal.background = pixelRouge;

        pixelBleu = new Texture2D(1, 1);
        pixelBleu.SetPixel(0, 0, new Color(0, 0, 255, 0.5f));
        pixelBleu.Apply();
        stylePixelBleu = new GUIStyle();
        stylePixelBleu.normal.background = pixelBleu;

        pixelJaune = new Texture2D(1, 1);
        pixelJaune.SetPixel(0, 0, new Color(255, 223, 0, 0.7f));
        pixelJaune.Apply();
        stylePixelJaune = new GUIStyle();
        stylePixelJaune.normal.background = pixelJaune;

        pixelBordure = new Texture2D(1, 1);
        pixelBordure.SetPixel(0, 0, new Color(0, 0, 0, 1f));
        pixelBordure.Apply();
        stylePixelBordure = new GUIStyle();
        stylePixelBordure.normal.background = pixelBordure;
    }

    /// <summary>
    /// Fonction pour dessiner le réseau de neuronne
    /// </summary>
    void OnGUI(){
        if(!vue.getMontrerAI()) return;
        if(!neat.neatEstDefini()) return;
        
        vueIA = vue.getVue();
        GUIStyle style = new GUIStyle();
        style.fontSize = 30;
        if(vueIA == default) return;
        
        for(int i = 0; i < vueIA.GetLength(0); i++){
            GUI.TextField(new Rect(0 + decalageX, i*tailleAffichageCellule + decalageY, tailleAffichageCellule, tailleAffichageCellule), i.ToString());
            for(int j = 0; j < vueIA.GetLength(1); j++){
                if(j==vueIA.GetLength(1)/2 && i == vueIA.GetLength(0) - 2) GUI.Label(new Rect(j * tailleAffichageCellule + decalageX, i * tailleAffichageCellule + decalageY, tailleAffichageCellule, tailleAffichageCellule), "" ,stylePixelBleu);
                else if(vueIA[i, j] == 2)  GUI.Label(new Rect(j * tailleAffichageCellule + decalageX, i * tailleAffichageCellule + decalageY, tailleAffichageCellule, tailleAffichageCellule), "" ,stylePixelJaune);
                else if(vueIA[i, j] > 0) GUI.Label(new Rect(j * tailleAffichageCellule + decalageX, i * tailleAffichageCellule + decalageY, tailleAffichageCellule, tailleAffichageCellule), "" , stylePixelBlanc);
                else if (vueIA[i, j] < 0) GUI.Label(new Rect(j * tailleAffichageCellule + decalageX, i * tailleAffichageCellule + decalageY, tailleAffichageCellule, tailleAffichageCellule), "" ,stylePixelRouge);
                else GUI.Label(new Rect(j * tailleAffichageCellule + decalageX, i * tailleAffichageCellule + decalageY, tailleAffichageCellule, tailleAffichageCellule), "" , stylePixelNoir);
                //EditorGUI.DrawRect(new Rect(j * tailleAffichageCellule + decalageX, i * tailleAffichageCellule + decalageY, tailleAffichageCellule, tailleAffichageCellule), color);
            }
        }
        for (int i = 0; i < vueIA.GetLength(1); i++){
            GUI.TextField(new Rect(decalageX + i*tailleAffichageCellule, decalageY, tailleAffichageCellule, tailleAffichageCellule), i.ToString());
        }

        for (int i = 0; i < vueIA.GetLength(0) + 1; i++){
            GUI.Label(new Rect(decalageX, i * tailleAffichageCellule + decalageY, vueIA.GetLength(1) * tailleAffichageCellule, 1), "", stylePixelBordure);
        }
         for(int j = 0; j < vueIA.GetLength(1) + 1; j++){
            GUI.Label(new Rect(j * tailleAffichageCellule + decalageX, 0 + decalageY, 1, vueIA.GetLength(0) * tailleAffichageCellule), "", stylePixelBordure);
        }

        var nbreNeuronnesCache = neat.getNombreNeurones() - (vueIA.GetLength(0) * vueIA.GetLength(1) + 3);

        for(int i = 0; i < nbreNeuronnesCache; i++){
            GUI.Label(new Rect(decalageX +  (vueIA.GetLength(1)*tailleAffichageCellule) + (i/vueIA.GetLength(0)) * tailleAffichageCellule + DECALAGE_NEURONNES_CACHES, decalageY + (i%vueIA.GetLength(0)) * tailleAffichageCellule, tailleAffichageCellule, tailleAffichageCellule), "", stylePixelBleu);
        }

        bool[] mouvementAJouer = neat.getMouvementAJouer();

        for(int i = 0; i < mouvementAJouer.Count(); i++){
            if(mouvementAJouer[i]) GUI.Label(new Rect(tailleAffichageCellule * vueIA.GetLength(1) + DECALAGE_NEURONNE_SORTIE + decalageX, (HAUTEUR_DONNE_SORTIE + 10) * i + decalageY, LARGEUR_DONNE_SORTIE, HAUTEUR_DONNE_SORTIE ), "", stylePixelBlanc);
            else GUI.Label(new Rect(tailleAffichageCellule * vueIA.GetLength(1) + DECALAGE_NEURONNE_SORTIE + decalageX, (HAUTEUR_DONNE_SORTIE + 10) * i + decalageY, LARGEUR_DONNE_SORTIE, HAUTEUR_DONNE_SORTIE ), "", stylePixelNoir);
            
            GUI.Label(new Rect(tailleAffichageCellule * vueIA.GetLength(1) + DECALAGE_NEURONNE_SORTIE + LARGEUR_DONNE_SORTIE + decalageX, (HAUTEUR_DONNE_SORTIE + 10) * i + decalageY, LARGEUR_DONNE_SORTIE, HAUTEUR_DONNE_SORTIE), bouton[i], style);
        }

        var touteLesConnexions = neat.getConexionsActuelle();
        List<Vector3> vertices = new List<Vector3>();
        List<Color> couleurs = new List<Color>();
        List<int> index = new List<int>();

        Camera cam = Camera.main;
        var camPos = cam.ScreenToWorldPoint(new Vector3(0, cam.pixelHeight, cam.nearClipPlane));

        //Debug.LogWarning(touteLesConnexions.Count());
        for(int i = 0; i < touteLesConnexions.Count(); i++){
            var co = touteLesConnexions[i];
            if(co.estActive()){
                if(co.getPoids()>0){
                    couleurs.Add(Color.green);
                    couleurs.Add(Color.green);
                }
                else {
                    couleurs.Add(Color.red);
                    couleurs.Add(Color.red);
                }
            } else {
                couleurs.Add(new Color(255, 255, 0, 0.2f));
                couleurs.Add(new Color(255, 255, 0, 0.2f));
            }

            Vector3 entre = default;
            Vector3 sortie = default;

            if(co.getPositionSortie() < 3) {
                sortie = new Vector3(
                    tailleAffichageCellule * vueIA.GetLength(1) + 200 + LARGEUR_DONNE_SORTIE/2 + decalageX,
                    cam.pixelHeight - (HAUTEUR_DONNE_SORTIE + 10) * co.getPositionSortie() - decalageY - HAUTEUR_DONNE_SORTIE/2,
                    cam.nearClipPlane
                    );
                
            } else {
                sortie = new Vector3(
                    decalageX + (vueIA.GetLength(1)*tailleAffichageCellule) + (co.getPositionSortie() - (3 + vueIA.GetLength(1) * vueIA.GetLength(0)))/vueIA.GetLength(0) * tailleAffichageCellule + DECALAGE_NEURONNES_CACHES,
                    cam.pixelHeight - (decalageY + ((co.getPositionSortie() - (3 + vueIA.GetLength(1) * vueIA.GetLength(0)))%vueIA.GetLength(0)) * tailleAffichageCellule),
                    cam.nearClipPlane
                );
            }
            sortie = cam.ScreenToWorldPoint(sortie);
            sortie.z = 0;
        
            if(co.getPositionEntre() >= 3 && co.getPositionEntre() < vueIA.GetLength(0)*vueIA.GetLength(1)){
                entre = new Vector3(
                    (co.getPositionEntre() % vueIA.GetLength(1) * tailleAffichageCellule) + tailleAffichageCellule/2 + decalageX,
                    cam.pixelHeight - ((co.getPositionEntre() / vueIA.GetLength(1)*tailleAffichageCellule) + tailleAffichageCellule/2) - decalageY,
                    cam.nearClipPlane
                );
                
            } else {
                entre = new Vector3(
                    decalageX + (vueIA.GetLength(1)*tailleAffichageCellule) + (co.getPositionSortie() - (3 + vueIA.GetLength(1) * vueIA.GetLength(0)))/vueIA.GetLength(0) * tailleAffichageCellule + DECALAGE_NEURONNES_CACHES,
                    cam.pixelHeight - (decalageY + ((co.getPositionSortie() - (3 + vueIA.GetLength(1) * vueIA.GetLength(0)))%vueIA.GetLength(0)) * tailleAffichageCellule),
                    cam.nearClipPlane
                );
            }
            entre = cam.ScreenToWorldPoint(entre);
            entre.z = 0;
            
            if((entre == default || sortie == default) && touteLesConnexions.Count != 0){
                Debug.Log("ERREURRR");
            }
            vertices.Add(entre);
            vertices.Add(sortie);
            index.Add(i * 2);
            index.Add(i * 2 + 1);
        }

        //Debug.Log(index.Count == vertices.Count && index.Count == couleurs.Count);


        //LineRenderer
        Mesh m = new Mesh();
 
        m.SetVertices(vertices.ToArray());
        m.SetColors(couleurs.ToArray());
        m.SetIndices(index.ToArray(), MeshTopology.Lines, 0);
 
        Graphics.DrawMesh(m, Vector3.zero, Quaternion.identity, mat, 0, Camera.main, 0);

        //*============================{AFFICHAGE DATA}==========================
        GUI.Label(new Rect(0 + decalageX, 240 + decalageY, 150, 30), "Habilite Max : " + neat.getMeilleurFitness(), style);
        GUI.Label(new Rect(0 + decalageX, 270 + decalageY, 150, 30), "Habilite Max Individu : " + neat.getFitnessMaxIndividus(), style);
        GUI.Label(new Rect(0 + decalageX, 300 + decalageY, 150, 30), "Habilite : " + neat.getFitnessActive(), style);
        GUI.Label(new Rect(0 + decalageX, 330 + decalageY, 150, 30), "Individu : " + neat.getIdentifiantActif(), style);
        GUI.Label(new Rect(0 + decalageX, 360 + decalageY, 150, 30), "Generation : " + neat.getGeneration(), style);
        GUI.Label(new Rect(0 + decalageX, 390 + decalageY, 150, 30), "Connexions : " + neat.getConnexions(), style);
        GUI.Label(new Rect(0 + decalageX, 420 + decalageY, 150, 30), "Neuronnes Caches : " + neat.getNbreNeuronnesCache(), style);
        GUI.Label(new Rect(0 + decalageX, 450 + decalageY, 150, 30), "Vitesse temps : " + Time.timeScale, style);
    }
}
