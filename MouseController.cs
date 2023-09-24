using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using TMPro;

public class MouseController : MonoBehaviour
{
    [SerializeField] float rayLength = 100;
    [SerializeField] LayerMask mask;
    [SerializeField] TextMeshProUGUI turnTracker;
    [SerializeField] TextMeshProUGUI winText;
    private bool redsTurn = true;
    private List<TileController> highlightedTiles;
    private List<TileController> targetedTiles;
    private TileController selectedTile;

    private int redPieces = 12;
    private int bluePieces = 12;
    private void Start()
    {
        highlightedTiles = new List<TileController>();
        targetedTiles = new List<TileController>();
        selectedTile = null;
    }

    //0 = blank
    //1 = red piece
    //2 = red king
    //3 = red highlight
    //4 = red target
    //5 = blue piece
    //6 = blue king
    //7 = blue highlight
    //8 = blue target
    //9 = red king target
    //10 = blue king target

    void Update()
    {
        if(Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if (hit)
            {
                //Debug.Log(hit.collider.name);
                int state = hit.collider.gameObject.GetComponent<TileController>().GetState();
                //Debug.Log(state);
                switch(state)
                {
                    case 0:
                        ClearHighlights();
                        ClearTargets();
                        selectedTile = null;
                        break;
                    case 1:
                        ClearHighlights();
                        ClearTargets();
                        selectedTile = hit.collider.gameObject.GetComponent<TileController>();
                        if (redsTurn)
                        {
                            hit.collider.gameObject.GetComponent<TileController>().RedPieceHighlights();
                        }
                        break;
                    case 2:
                        ClearHighlights();
                        ClearTargets();
                        selectedTile = hit.collider.gameObject.GetComponent<TileController>();
                        if (redsTurn)
                        {
                            hit.collider.gameObject.GetComponent<TileController>().RedKingHighlights();
                        }
                        break;
                    case 3:
                        ClearHighlights();
                        hit.collider.gameObject.GetComponent<TileController>().FindTargetPath(selectedTile, hit.collider.gameObject.GetComponent<TileController>(), 0);
                        selectedTile = null;
                        ClearTargets();
                        highlightedTiles = new List<TileController>();
                        targetedTiles = new List<TileController>();
                        redsTurn = !redsTurn;
                        turnTracker.text = "Blue's turn";
                        turnTracker.color = Color.cyan;
                        break;
                    case 5:
                        ClearHighlights();
                        ClearTargets();
                        selectedTile = hit.collider.gameObject.GetComponent<TileController>();
                        if(!redsTurn)
                        {
                            hit.collider.gameObject.GetComponent<TileController>().BluePieceHighlights();
                        }
                        break;
                    case 6:
                        ClearHighlights();
                        ClearTargets();
                        selectedTile = hit.collider.gameObject.GetComponent<TileController>();
                        if(!redsTurn)
                        {
                            hit.collider.gameObject.GetComponent<TileController>().BlueKingHighlights();
                        }
                        break;
                    case 7:
                        ClearHighlights();
                        hit.collider.gameObject.GetComponent<TileController>().FindTargetPath(selectedTile, hit.collider.gameObject.GetComponent<TileController>(), 1);
                        selectedTile = null;
                        ClearTargets();
                        highlightedTiles = new List<TileController>();
                        targetedTiles = new List<TileController>();
                        redsTurn = !redsTurn;
                        turnTracker.text = "Red's turn";
                        turnTracker.color = Color.red;
                        break;
                }
            }
        }
    }

    public void AddHighlightedTile(TileController toAdd)
    {
        if (!highlightedTiles.Contains(toAdd))
        {
            highlightedTiles.Add(toAdd);
        }
    }

    public void AddTargetedTile(TileController toAdd)
    {
        if(!targetedTiles.Contains(toAdd))
        targetedTiles.Add(toAdd);
    }

    private void ClearHighlights()
    {
        foreach(TileController t in highlightedTiles)
        {
            t.SetState(0);
        }
    }

    private void ClearTargets()
    {
        foreach(TileController t in targetedTiles)
        {
            switch(t.GetState())
            {
                case 4:
                    t.SetState(5);
                    break;
                case 8:
                    t.SetState(1);
                    break;
                case 9:
                    t.SetState(6);
                    break;
                case 10:
                    t.SetState(2);
                    break;
            }
        }
    }

    public void RemovePieces(int subtract, int color)
    {
        switch (color)
        {
            case 0:
                bluePieces -= Mathf.Abs(subtract);
                break;
            case 1:
                redPieces -= Mathf.Abs(subtract);
                break;
        }
        Debug.Log("Red: " + redPieces + ", Blue: " + bluePieces);
        if(redPieces == 0)
        {
            winText.enabled = true;
            winText.text = "Blue wins";
        }
        if(bluePieces == 0)
        {
            winText.enabled = true;
            winText.text = "Red wins";
        }
    }
}
