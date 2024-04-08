using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

public class AIScripts : MonoBehaviour{

    //*==================={PUBLIC}===================
    public GameObject joueur;
    public int resolutionHauteur;
    public int resolutionLongueur;
    //*==================={PRIVATE}===================
    private Tilemap levelTiledMap;
    private int[,] tilesData;
    private int[,] aiView;
    private bool montrerAI = false;
    Vector3 v;
    void Start()
    {
        levelTiledMap = GameObject.Find("Terrain").GetComponent<Tilemap>();
        //levelTiledMap = GetComponent<Tilemap>();
        if(levelTiledMap == null) Debug.Log("CAN'T FIND REQUESTED TILEDMAP !");

        for(int i = 0; i < Display.displays.Length; i++){
                Display.displays[i].Activate(Display.displays[i].systemHeight, Display.displays[i].systemWidth, new RefreshRate());
        }
    }

    // Update is called once per frame
    void Update()
    {
        tilesData = getTiledMapData(levelTiledMap);
        aiView = copyTilesDataToView();
        //DessinerData(tilesData);

        if(Input.GetKeyDown("p")) montrerAI = !montrerAI;
    }

    int[,] getTiledMapData(Tilemap map){

        BoundsInt vueTotal = levelTiledMap.cellBounds;

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
                m[i, j] = valeur;
            }
        }
        return m;
    }

    int[,] copyTilesDataToView(){
        int[,] copiedMatrix = new int[resolutionHauteur, resolutionLongueur];

        float longueurJoueur = joueur.GetComponent<SpriteRenderer>().bounds.size.x;
        float hauteurJoueur = joueur.GetComponent<SpriteRenderer>().bounds.size.y;

        Vector3Int relativeTileMapPos = levelTiledMap.layoutGrid.WorldToCell(levelTiledMap.transform.position);
        relativeTileMapPos.x = relativeTileMapPos.x - tilesData.GetLength(1)/2;
        relativeTileMapPos.y = relativeTileMapPos.y - tilesData.GetLength(0)/2;

        Vector3Int relativePlayerPos = levelTiledMap.layoutGrid.WorldToCell(
                                                                            new Vector3(
                                                                                        joueur.transform.position.x + longueurJoueur/2,
                                                                                        joueur.transform.position.y + hauteurJoueur/2
                                                                            ));
        Vector3Int relativeAIView = new Vector3Int  (
                                                    relativePlayerPos.x - (resolutionLongueur/2),
                                                    relativePlayerPos.y + 1
                                                    );

        Vector3Int deltaPos = new Vector3Int(
                                                relativeTileMapPos.x - relativeAIView.x,
                                                relativeTileMapPos.y - relativeAIView.y
                                            );

        int i = deltaPos.y < 0 ? 0 : deltaPos.y;

        for(; i < resolutionHauteur; i++){
            int j = deltaPos.x < 0 ? 0 : deltaPos.x;
            for(; j < resolutionLongueur; j++){
                int valueToCopy = 0;
                if( i - deltaPos.y < tilesData.GetLength(0) && j - deltaPos.x < tilesData.GetLength(1) &&
                    i - deltaPos.y >= 0 && j - deltaPos.x >= 0
                ) {
                    valueToCopy = tilesData[i - deltaPos.y, j - deltaPos.x];
                }
                if(copiedMatrix.GetLength(0) - i < copiedMatrix.GetLength(0) && copiedMatrix.GetLength(0) - i >= 0){
                    copiedMatrix[copiedMatrix.GetLength(0) - i,j] = valueToCopy;
                }
            }
        }

        v =  levelTiledMap.layoutGrid.CellToWorld(relativeAIView);
        
        return copiedMatrix;
    }

    //*==================={Draw Method}===================

    
    void OnGUI()
    {
        if(!montrerAI) return;
        //Rect rectScreen = cam.pixelRect;
        float squareSize = 10;

        for(int i = 0; i < aiView.GetLength(0); i++){
            EditorGUI.TextField(new Rect(0, i*squareSize, squareSize, squareSize), i.ToString());
            for(int j = 0; j < aiView.GetLength(1); j++){
                Color color = new Color(0, 0, 0, 0.5f);
                if(aiView[i, j] > 0) color = new Color(255, 255, 255, 0.5f);
                else if (aiView[i, j] < 0) color = new Color(255, 0, 0, 0.5f);
                EditorGUI.DrawRect(new Rect(j * squareSize, i * squareSize, squareSize, squareSize), color);
            }
        }
        for (int i = 0; i < aiView.GetLength(0) + 1; i++){
            EditorGUI.DrawRect(new Rect(0, i * squareSize, 2, aiView.GetLength(1) * squareSize), Color.black);
        }
         for(int j = 0; j < aiView.GetLength(1) + 1; j++){
            EditorGUI.DrawRect(new Rect(j * squareSize, 0, aiView.GetLength(0) * squareSize, 2), Color.black);
        }

        EditorGUI.DrawRect(new Rect(
           v.x,
           v.y,
           10,
           10
        ), 
        Color.red);
        
    }
}


