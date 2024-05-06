using UnityEngine;
public class MainMenu : MonoBehaviour
{
    public void PlayGame(){
        GameManager.instance.UpdateEtatJeu(EtatJeu.Jeu);
    }

    public void AfficherOptions(){
        GameManager.instance.UpdateEtatJeu(EtatJeu.Options);
    }

    public void Quitter(){
        Application.Quit();
    }
}
