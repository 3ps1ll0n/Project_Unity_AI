using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using JetBrains.Annotations;

public class SauvegardeNiveau : MonoBehaviour
{
    public string tagAssetsSauvegarde = "Sauvegardable";
    private GameObject[] assetsAsauvegarder;
    [SerializeField] private string[] nomsAssets;
    [SerializeField] private Vector3[] positionsAssets;

    public GameObject[] objetsPossibles;

    /// <summary>
    /// Determiner ce qui doit etre sauvegarder dans un fichier texte
    /// </summary>
    public void trouverAssetsSauvegarde()
    {


        if (assetsAsauvegarder != null)
        {
            for (int i = 0; i < assetsAsauvegarder.Length; i++) { Destroy(assetsAsauvegarder[i]); }
        }


        assetsAsauvegarder = GameObject.FindGameObjectsWithTag(tagAssetsSauvegarde);

        //D�terminer la taille
        nomsAssets = new string[assetsAsauvegarder.Length];

        positionsAssets = new Vector3[assetsAsauvegarder.Length];

        for (int j = 0; j < assetsAsauvegarder.Length; j++)
        {
            nomsAssets[j] = assetsAsauvegarder[j].name;
            positionsAssets[j] = assetsAsauvegarder[j].transform.position;
        }
        sauvegarderDansFichier();
    }

    
    public void sauvegarderDansFichier()
    {//�crire dans le fichier texte la position de chaque asset avec son nom
        string referenceFichier = "Assets/Screenshots/Niveau-Donnee.txt";
        StreamWriter ecrire = new StreamWriter(referenceFichier, false);
        for (int i = 0; i < assetsAsauvegarder.Length; i++)
        {
            string info = nomsAssets[i] + "." + positionsAssets[i].x.ToString() + "." + positionsAssets[i].y.ToString() + "."
                + positionsAssets[i].z.ToString();
            ecrire.WriteLine(info);
        }
        ecrire.Close();
    }
    /// <summary>
    /// Placer les assets dans le niveau
    /// </summary>
    public void chargerNiveau()
    {
        //Retirer tous les objets du niveau pr�c�dent
        foreach (GameObject sauveugardableObject in GameObject.FindGameObjectsWithTag("Sauvegardable"))
        {
            Destroy(sauveugardableObject);
        }
        string referenceFichier = "Assets/Screenshots/Niveau-Donnee.txt";
        StreamReader lecteur = new StreamReader(referenceFichier);

        int nombreLignes = 0;
        while (lecteur.ReadLine() != null) { nombreLignes++; }//Pour s'assurer que les tableaux sont de la meme taille que le fichier texte

        nomsAssets = new string[0];
        positionsAssets = new Vector3[0];


        nomsAssets = new string[nombreLignes];
        positionsAssets = new Vector3[nombreLignes];

        lecteur.Close();

        StreamReader lecteur2 = new StreamReader(referenceFichier);
        while (!lecteur2.EndOfStream)
        {
            for (int i = 0; i < nombreLignes; i++)
            {//Separer le nom de l'asset de la position
                string[] donnee = lecteur2.ReadLine().Split('.');
                nomsAssets[i] = donnee[0];
                //positions x,y et z
                positionsAssets[i].x = float.Parse(donnee[1]);
                positionsAssets[i].y = float.Parse(donnee[2]);
                positionsAssets[i].z = float.Parse(donnee[3]);
            }
        }
        lecteur2.Close();
        creerAsset();


    }
    public void creerAsset()
    {
        for (int i = 0; i < nomsAssets.Length; i++)
        {
            for (int j = 0; j < objetsPossibles.Length; j++)
            {
                string nom = objetsPossibles[j].name + "(Clone)";
                if (nom == nomsAssets[i])
                {//Creer assets
                    Instantiate(objetsPossibles[j], positionsAssets[i], Quaternion.identity);

                }
            }
        }
    }
}