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

    public void trouverAssetsSauvegarde()
    {
        if(assetsAsauvegarder != null)
        {
            for(int i =0; i<assetsAsauvegarder.Length; i++) { Destroy(assetsAsauvegarder[i]); }
        }

        assetsAsauvegarder = GameObject.FindGameObjectsWithTag(tagAssetsSauvegarde);

        nomsAssets = new string[assetsAsauvegarder.Length];

        positionsAssets = new Vector3[assetsAsauvegarder.Length];

        for(int j = 0; j<assetsAsauvegarder.Length; j++)
        {
            nomsAssets[j] = assetsAsauvegarder[j].name;
            positionsAssets[j] = assetsAsauvegarder[j].transform.position;
        }
        sauvegarderDansFichier();
     }
    

    public void sauvegarderDansFichier()
    {
        string referenceFichier = "Assets/Screenshots/Niveau-Donnee.txt";
        StreamWriter ecrire = new StreamWriter(referenceFichier, false);
        for(int i = 0; i< assetsAsauvegarder.Length; i++)
        {
            string info = nomsAssets[i] + "," + positionsAssets[i].x.ToString() + "," + positionsAssets[i].y.ToString() + ","
                + positionsAssets[i].z.ToString();
            ecrire.WriteLine(info);
        }
        ecrire.Close();
    }
    
}
