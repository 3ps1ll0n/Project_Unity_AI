using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using LootLocker.Requests;

public class NiveauManager : MonoBehaviour
{
    public InputField nomNiveauInputField; //Pour entrer le nom du niveau
    private string nomNiveau; //Pour le sauvegarder
    public GameObject niveauUploadUI; //Pour visuellement voir le InputField

    public void creerNiveau() //Sauvegarder un niveau
    {

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
    }

    public void prendreCaptureEcran()
    {
        string reference = Directory.GetCurrentDirectory() + "/Assets/Screenshots/";
        ScreenCapture.CaptureScreenshot(Path.Combine(reference, "Niveau-Screenshot.png"));
        GetComponent<SauvegardeNiveau>().trouverAssetsSauvegarde();
    }

    IEnumerator attenteCaptureEcran() {//Pour prendre la capture d'écran avant que le UI ouvre
        prendreCaptureEcran();
        yield return new WaitForSeconds(1.0f);
        niveauUploadUI.SetActive(true);
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

}
