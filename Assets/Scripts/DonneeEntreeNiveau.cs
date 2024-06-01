using System.Collections;
using UnityEngine;
using System.IO;
using UnityEditor;
using UnityEngine.Networking;
using UnityEngine.UI;


public class DonneeEntreeNiveau : MonoBehaviour
{
    public int identification;
    public string nomNiveau;
    public Text nomTexte;
    public Image imageNiveau;
    public string dossierTexteURL;

    private void Start()
    {
        transform.position = Vector3.zero;
        transform.localScale = Vector3.one;

        nomTexte.text = nomNiveau;
        imageNiveau.sprite = imageNiveau.sprite;//Pour actualiser l'image t�l�charg�e

    }
   
    public void chargerNiveau()
    {
        StartCoroutine(telechargerDossierTexteNiveau(dossierTexteURL));
    }
    private IEnumerator telechargerDossierTexteNiveau(string dossierTexteURL)
    {
        UnityWebRequest www = UnityWebRequest.Get(dossierTexteURL); //trouver le dossier texte dans LootLocker
        yield return www.SendWebRequest();

        string referenceDossier = "Assets/Screenshots/Niveau-Donnee.txt"; //Sauvegarder ce dossier
        File.WriteAllText(referenceDossier, www.downloadHandler.text);

        yield return new WaitForSeconds(1.0f);
        GameObject.FindGameObjectWithTag("NiveauManager").GetComponent<SauvegardeNiveau>().chargerNiveau();
        yield return new WaitForSeconds(2.0f);
        GameObject.FindGameObjectWithTag("MenuTelechargement").SetActive(false);//fermer le menu


    }

}
