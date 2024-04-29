using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using LootLocker.Requests;
using UnityEngine.Networking;


public class NiveauManager : MonoBehaviour
{
    public InputField nomNiveauInputField; //Pour entrer le nom du niveau
    private string nomNiveau; //Pour le sauvegarder
    public GameObject niveauUploadUI; //Pour visuellement voir le InputField

    public GameObject entreeNiveauItem;
    public Transform contenuDonneeNiveau;
    HashSet<int> loadedAssetIdentifiers = new HashSet<int>();
    public void creerNiveau() //Sauvegarder un niveau
    {
        if (GameObject.FindGameObjectsWithTag("Sauvegardable").Length == 0)
        {
            Debug.Log("On ne peut sauvegarder un niveau vide");
            return;
        }

        nomNiveau = nomNiveauInputField.text;
        LootLockerSDKManager.CreatingAnAssetCandidate(nomNiveau, (reponse) =>
        {
            if (reponse.success)
            {
                uploadDonneeNiveau(reponse.asset_candidate_id);


            }
            else
            {
                Debug.Log("Erreur avec le candidat d'asset");//Si erreur
            }
        });
        Mouvement.setCanMove(true);
    }

    public void prendreCaptureEcran()
    {
        string reference = Directory.GetCurrentDirectory() + "/Assets/Screenshots/";
        ScreenCapture.CaptureScreenshot(Path.Combine(reference, "Niveau-Screenshot.png"));
        GetComponent<SauvegardeNiveau>().trouverAssetsSauvegarde();
    }

    IEnumerator attenteCaptureEcran()
    {//Pour prendre la capture d'écran avant que le UI ouvre
        prendreCaptureEcran();
        yield return new WaitForSeconds(1.0f);
        niveauUploadUI.SetActive(true);
        Mouvement.setCanMove(false);
    }
    public void ouvrirNiveauUploadUI()
    {
        StartCoroutine(attenteCaptureEcran());
    }

    public void uploadDonneeNiveau(int idNiveau)
    {
        string referenceCaptureEcran = "Assets/Screenshots/Niveau-Screenshot.png";
        LootLocker.LootLockerEnums.FilePurpose typeDossierCaptureEcran = LootLocker.LootLockerEnums.FilePurpose.primary_thumbnail;
        LootLockerSDKManager.AddingFilesToAssetCandidates(idNiveau, referenceCaptureEcran, "Niveau-Screenshot.png", typeDossierCaptureEcran, (reponseCaptureEcran) =>
        {
            if (reponseCaptureEcran.success)
            {
                string referenceDossierTexte = "Assets/Screenshots/Niveau-Donnee.txt";
                LootLocker.LootLockerEnums.FilePurpose typeDossierTexte = LootLocker.LootLockerEnums.FilePurpose.file;

                LootLockerSDKManager.AddingFilesToAssetCandidates(idNiveau, referenceDossierTexte, "Niveau-Donnee.txt", typeDossierTexte, (reponseTexte) =>
                {
                    if (reponseTexte.success)
                    {
                        LootLockerSDKManager.UpdatingAnAssetCandidate(idNiveau, true, (reponseUpdate) => { });
                    }
                    else
                    {
                        Debug.Log("erreur upload candidat asset");
                    }
                });
            }
            else
            {
                Debug.Log("erreur upload candidat asset");
            }
        });
    }

    public void telechargerDonneeNiveau()
    {
        LootLockerSDKManager.GetAssetListWithCount(10, (reponse) =>
        {
            for (int i = 0; i < reponse.assets.Length; i++)
            {
                if (loadedAssetIdentifiers.Contains(i))
                {
                    continue;
                }
                GameObject afficherItem = Instantiate(entreeNiveauItem, transform.position, Quaternion.identity);
                afficherItem.transform.SetParent(contenuDonneeNiveau);

                afficherItem.GetComponent<DonneeEntreeNiveau>().identification = i;
                afficherItem.GetComponent<DonneeEntreeNiveau>().nomNiveau = reponse.assets[i].name;

                LootLockerFile[] fichiersImageNiveau = reponse.assets[i].files;
                StartCoroutine(chargerImageNiveau(fichiersImageNiveau[0].url.ToString(), afficherItem.GetComponent<DonneeEntreeNiveau>().imageNiveau));

                afficherItem.GetComponent<DonneeEntreeNiveau>().dossierTexteURL = fichiersImageNiveau[1].url.ToString();

                loadedAssetIdentifiers.Add(i);
            }
        }, null, true);
    }
   
    IEnumerator chargerImageNiveau(string imageURL, Image imageNiveau)
    {
        UnityWebRequest www = UnityWebRequestTexture.GetTexture(imageURL);
        yield return www.SendWebRequest();

        Texture2D imageChargee = DownloadHandlerTexture.GetContent(www);
        imageNiveau.sprite = Sprite.Create(imageChargee, new Rect(0.0f, 0.0f, imageChargee.width, imageChargee.height), Vector2.zero);

    }

}
