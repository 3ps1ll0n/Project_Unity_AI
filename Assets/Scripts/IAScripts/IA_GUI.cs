using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.iOS;
using System;
using System.Linq;
using UnityEngine.UIElements;
using Unity.Collections;
using System.Reflection;

public class IA_GUI : MonoBehaviour
{
    public int tailleAffichageCellule;
    public VueIA vue;
    public AlgoritmeNEAT neat;
    public Material mat;
    public int decalageX;
    public int decalageY;
    int[,] aiView;

    int HAUTEUR_DONNE_SORTIE = 50;
    int LARGEUR_DONNE_SORTIE = 150;
    int DECALAGE_NEURONNE_SORTIE = 200;
    int DECALAGE_NEURONNES_CACHES = 100;
    string[] bouton = {"Saut", "Droite", "Gauche"};

    void OnGUI(){
        if(!vue.getMontrerAI()) return;
        if(!neat.neatEstDefini()) return;
        aiView = vue.getVue();
        GUIStyle style = new GUIStyle();
        style.fontSize = 30;
        if(aiView == default) return;
        for(int i = 0; i < aiView.GetLength(0); i++){
            EditorGUI.TextField(new Rect(0 + decalageX, i*tailleAffichageCellule + decalageY, tailleAffichageCellule, tailleAffichageCellule), i.ToString());
            for(int j = 0; j < aiView.GetLength(1); j++){
                Color color = new Color(0, 0, 0, 0.5f);
                if(j==aiView.GetLength(1)/2 && i == aiView.GetLength(0) - 2) color = new Color(0, 0, 255, 1.0f);
                else if(aiView[i, j] == 2) color = new Color(255, 223, 0, 0.7f);
                else if(aiView[i, j] > 0) color = new Color(255, 255, 255, 0.5f);
                else if (aiView[i, j] < 0) color = new Color(255, 0, 0, 0.5f);
                
                EditorGUI.DrawRect(new Rect(j * tailleAffichageCellule + decalageX, i * tailleAffichageCellule + decalageY, tailleAffichageCellule, tailleAffichageCellule), color);
            }
        }
        for (int i = 0; i < aiView.GetLength(1); i++){
            EditorGUI.TextField(new Rect(decalageX + i*tailleAffichageCellule, decalageY, tailleAffichageCellule, tailleAffichageCellule), i.ToString());
        }
        for (int i = 0; i < aiView.GetLength(0) + 1; i++){
            EditorGUI.DrawRect(new Rect(decalageX, i * tailleAffichageCellule + decalageY, aiView.GetLength(1) * tailleAffichageCellule, 1), Color.black);
        }
         for(int j = 0; j < aiView.GetLength(1) + 1; j++){
            EditorGUI.DrawRect(new Rect(j * tailleAffichageCellule + decalageX, 0 + decalageY, 1, aiView.GetLength(0) * tailleAffichageCellule), Color.black);
        }

        var nbreNeuronnesCache = neat.getNombreNeurones() - (aiView.GetLength(0) * aiView.GetLength(1) + 3);

        Color couleurCache = new Color(0, 0, 255, 0.8f);
        for(int i = 0; i < nbreNeuronnesCache; i++){
            EditorGUI.DrawRect(new Rect(decalageX +  (aiView.GetLength(1)*tailleAffichageCellule) + (i/aiView.GetLength(0)) * tailleAffichageCellule + DECALAGE_NEURONNES_CACHES, decalageY + (i%aiView.GetLength(0)) * tailleAffichageCellule, tailleAffichageCellule, tailleAffichageCellule), couleurCache);
        }

        bool[] mouvementAJouer = neat.getMouvementAJouer();

        for(int i = 0; i < mouvementAJouer.Count(); i++){
            Color color = new Color(0, 0, 0, 0.6f);
            if(mouvementAJouer[i]) color = new Color(255, 255, 255, 0.6f);
            EditorGUI.DrawRect (new Rect(tailleAffichageCellule * aiView.GetLength(1) + DECALAGE_NEURONNE_SORTIE + decalageX, (HAUTEUR_DONNE_SORTIE + 10) * i + decalageY, LARGEUR_DONNE_SORTIE, HAUTEUR_DONNE_SORTIE ), color);
            GUI.Label(new Rect(tailleAffichageCellule * aiView.GetLength(1) + DECALAGE_NEURONNE_SORTIE + LARGEUR_DONNE_SORTIE + decalageX, (HAUTEUR_DONNE_SORTIE + 10) * i + decalageY, LARGEUR_DONNE_SORTIE, HAUTEUR_DONNE_SORTIE), bouton[i], style);
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
                    tailleAffichageCellule * aiView.GetLength(1) + 200 + LARGEUR_DONNE_SORTIE/2 + decalageX,
                    cam.pixelHeight - (HAUTEUR_DONNE_SORTIE + 10) * co.getPositionSortie() - decalageY - HAUTEUR_DONNE_SORTIE/2,
                    cam.nearClipPlane
                    );
                
            } else {
                sortie = new Vector3(
                    decalageX + (aiView.GetLength(1)*tailleAffichageCellule) + (co.getPositionSortie() - (3 + aiView.GetLength(1) * aiView.GetLength(0)))/aiView.GetLength(0) * tailleAffichageCellule + DECALAGE_NEURONNES_CACHES,
                    cam.pixelHeight - (decalageY + ((co.getPositionSortie() - (3 + aiView.GetLength(1) * aiView.GetLength(0)))%aiView.GetLength(0)) * tailleAffichageCellule),
                    cam.nearClipPlane
                );
            }
            sortie = cam.ScreenToWorldPoint(sortie);
            sortie.z = 0;
        
            if(co.getPositionEntre() >= 3 && co.getPositionEntre() < aiView.GetLength(0)*aiView.GetLength(1)){
                entre = new Vector3(
                    (co.getPositionEntre() % aiView.GetLength(1) * tailleAffichageCellule) + tailleAffichageCellule/2 + decalageX,
                    cam.pixelHeight - ((co.getPositionEntre() / aiView.GetLength(1)*tailleAffichageCellule) + tailleAffichageCellule/2) - decalageY,
                    cam.nearClipPlane
                );
                
            } else {
                entre = new Vector3(
                    decalageX + (aiView.GetLength(1)*tailleAffichageCellule) + (co.getPositionSortie() - (3 + aiView.GetLength(1) * aiView.GetLength(0)))/aiView.GetLength(0) * tailleAffichageCellule + DECALAGE_NEURONNES_CACHES,
                    cam.pixelHeight - (decalageY + ((co.getPositionSortie() - (3 + aiView.GetLength(1) * aiView.GetLength(0)))%aiView.GetLength(0)) * tailleAffichageCellule),
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
