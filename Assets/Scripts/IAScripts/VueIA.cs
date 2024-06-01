using System;
using UnityEngine;
using UnityEngine.Tilemaps;

public class VueIA : MonoBehaviour{

    //*==================={PUBLIC}===================
    public GameObject joueur;
    public int resolutionHauteur;
    public int resolutionLongueur;
    public int tailleAffichageCellule;
    public int camDecalageY;
    //*==================={PRIVATE}===================
    private Tilemap tileMapDuNiveau;
    private int[,] tilesData;
    private int[,] vueIA;
    private bool montrerAI = false;
    private Vector3Int vueIARelative;
    void Start()
    {
        tileMapDuNiveau = GameObject.Find("Terrain").GetComponent<Tilemap>();
        //levelTiledMap = GetComponent<Tilemap>();
        if(tileMapDuNiveau == null) Debug.Log("CAN'T FIND REQUESTED TILEDMAP !");

        for(int i = 0; i < Display.displays.Length; i++){
                Display.displays[i].Activate(Display.displays[i].systemHeight, Display.displays[i].systemWidth, new RefreshRate());
        }
    }

    // Update is called once per frame
    void Update()
    {
        tilesData = getDonneTiledMap(tileMapDuNiveau);
        vueIA = CopierDonneTilesAVue();
        vueIA = lireObjet(getToutLesObjets(tileMapDuNiveau), tileMapDuNiveau, vueIA);
        //DessinerData(tilesData);

        if(Input.GetKeyDown("p")) montrerAI = !montrerAI;
        if(Input.GetKeyDown(KeyCode.RightArrow)) {
            Time.timeScale += 0.2f;
        }
        if(Input.GetKeyDown(KeyCode.LeftArrow)) {
            Time.timeScale -= 0.2f;
        }
    }

    int[,] getDonneTiledMap(Tilemap map){
        BoundsInt vueTotal = tileMapDuNiveau.cellBounds;

        TileBase[] allTiles = map.GetTilesBlock(vueTotal);

        tilesData = tableauAMatrice(allTiles, vueTotal.size.x, vueTotal.size.y);

        return tilesData;
    }

    //*==================={Matrix Manipulation}===================

    int[,] tableauAMatrice(TileBase[] tab, int longueur, int hauteur){
        int[,] m = new int[hauteur, longueur];

        for(int i = 0; i < hauteur; i++){
            for(int j = 0; j < longueur; j++){
                int valeur = 0;
                if(tab[j + (i * longueur)] != null) {
                    valeur = 1;
                }
                m[m.GetLength(0) - 1 - i, j] = valeur;
            }
        }
        return m;
    }

    int[,] renverserMatrice(int[,] m){
        int[,] invM = new int[m.GetLength(0), m.GetLength(1)];

        return invM;
    }
    /// <summary>
    /// Passe les donne collecte a la vue de l'IA
    /// </summary>
    /// <returns>la vue de l'IA</returns>
    int[,] CopierDonneTilesAVue(){
        int[,] matriceCopie = new int[resolutionHauteur, resolutionLongueur];

        float longueurJoueur = joueur.GetComponent<BoxCollider2D>().bounds.size.x;
        float hauteurJoueur = joueur.GetComponent<BoxCollider2D>().bounds.size.y;

        Vector3Int relativeTileMapPos = tileMapDuNiveau.layoutGrid.WorldToCell(tileMapDuNiveau.transform.position);
        relativeTileMapPos.x -= tilesData.GetLength(1)/2;
        relativeTileMapPos.y += (int)Math.Floor((double)tilesData.GetLength(0)/2);

        Vector3Int relativePlayerPos = tileMapDuNiveau.layoutGrid.WorldToCell(
                                                                        new Vector3 (
                                                                            joueur.transform.position.x, 
                                                                            joueur.transform.position.y + hauteurJoueur/2
                                                                            )
        );
        vueIARelative = new Vector3Int  (
                                                    relativePlayerPos.x - (int)Math.Floor((double)resolutionLongueur/2) + 1,
                                                    relativePlayerPos.y + resolutionHauteur + camDecalageY
                                                    );

        Vector3Int deltaPos = new Vector3Int(
                                                relativeTileMapPos.x - vueIARelative.x,
                                                relativeTileMapPos.y - vueIARelative.y
                                            );

        int i = deltaPos.y >= 0 ? 0 : -deltaPos.y;

        for(; i < resolutionHauteur; i++){
            int j = deltaPos.x < 0 ? 0 : deltaPos.x;
            for(; j < resolutionLongueur; j++){
                int valueToCopy = 0;
                if(i + deltaPos.y < tilesData.GetLength(0) && j - deltaPos.x < tilesData.GetLength(1) &&
                    i + deltaPos.y >= 0 && j - deltaPos.x >= 0
                ) {
                    valueToCopy = tilesData[i + deltaPos.y, j - deltaPos.x];
                }
                if(matriceCopie.GetLength(0) - i < matriceCopie.GetLength(0) && matriceCopie.GetLength(0) - i >= 0 
                && i >= 0 && i < matriceCopie.GetLength(0)){
                    matriceCopie[i, j] = valueToCopy;
                }
            } 
        }
        return matriceCopie;
    }
    /// <summary>
    /// sert a regarder les autres objets
    /// </summary>
    /// <param name="objet"></param>
    /// <param name="map"></param>
    /// <param name="donne"></param>
    /// <returns>le prochain objet a copier dans la vue</returns>
    int[,] lireObjet(GameObject[] objet, Tilemap map, int[,] donne){
        foreach(GameObject gObj in objet){

            Vector3Int relPos = map.layoutGrid.WorldToCell(gObj.transform.position); //Pour avoir la position relatives des objects
            Vector3 cellSize = map.layoutGrid.cellSize; //Pour avoir la taille relative des objects
            
            Vector3Int relSize = new Vector3Int(
                (int)Math.Ceiling(gObj.GetComponent<BoxCollider2D>().bounds.size.x / cellSize.x),
                (int)Math.Ceiling(gObj.GetComponent<BoxCollider2D>().bounds.size.y / cellSize.y)
            );

            relPos.x -= (int)((double)relSize.x/2);
            relPos.y += (int)Math.Ceiling((double)relSize.y/2) + 4;


            Vector3Int deltaPos = new Vector3Int(
                                                relPos.x - vueIARelative.x,
                                                relPos.y - vueIARelative.y
                                            );

            int i = deltaPos.y >= 0 ? 0 : -deltaPos.y;
            int yMax = i + relSize.y;

            for(; i < yMax; i++){
                int j = deltaPos.x < 0 ? 0 : deltaPos.x;
                int xMax = j + relSize.x;
                for(; j < xMax; j++){
                    if(i < donne.GetLength(0) && i >= 0 && j < donne.GetLength(1)){
                        int value = -1;
                        if(gObj.ToString().IndexOf("Arrivée(Clone)") > -1) value = 2;
                        donne[i,j] = value;
                    }
                }  
            }
        }
    return donne;
    }

    //*==================={GETTER}===================
    GameObject[] getToutLesObjets(Tilemap map){
        GameObject[] tilesObject = new GameObject[map.transform.childCount];

        for (int i = 0; i < map.transform.childCount; ++i)
            tilesObject[i] = map.transform.GetChild(i).gameObject;

        return tilesObject;
    }
    public bool getMontrerAI(){return montrerAI;}

    public Vector3 getPositionJoueur(){
        return joueur.transform.position;
    }

    public Vector3 getPositionArrive(){
        return GameObject.Find("Arrivée").transform.position;
    }
    public bool getJoueurMort(){
        return !joueur.activeInHierarchy;
    }
    public void activerJoueur(){
        joueur.SetActive(true);
    }
    public void desactiverJoueur(){
        joueur.SetActive(false);
    }
    public void setPosJoueur(Vector3 pos){
        joueur.transform.position = pos;
    }
    public Vector2Int getTailleVue(){
        return new Vector2Int(resolutionLongueur, resolutionHauteur);
    }
    public GameObject getJoueur(){
        return joueur;
    }
    public int[,] getVue(){
        return vueIA;
    }
    public bool getIAActivee(){return montrerAI;}
}



