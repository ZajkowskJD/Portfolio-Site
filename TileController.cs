using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileController : MonoBehaviour
{
    [SerializeField] TileController Neighbor_NW;
    [SerializeField] TileController Neighbor_NE;
    [SerializeField] TileController Neighbor_SW;
    [SerializeField] TileController Neighbor_SE;

    [SerializeField] int state = 0;
    [SerializeField] bool edge = false;
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

    private Animator anim;
    private MouseController controller;


    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        anim.SetInteger("state", state);
        controller = GameObject.Find("MouseController").GetComponent<MouseController>();
    }

    public int GetState()
    {
        return state;
    }

    public void SetState(int newState)
    {
        state = newState;
        anim.SetInteger("state", state);
    }

    public TileController GetNeighbor_NW()
    {
        return Neighbor_NW;
    }

    public TileController GetNeighbor_NE()
    {
        return Neighbor_NE;
    }

    public TileController GetNeighbor_SW()
    {
        return Neighbor_SW;
    }

    public TileController GetNeighbor_SE()
    {
        return Neighbor_SE;
    }

    public bool isEdge()
    {
        return edge;
    }
    public void BluePieceHighlights()
    {
        if(Neighbor_SW != null)
        {
            switch(Neighbor_SW.GetState())
            {
                default: break;
                case 0:
                    Neighbor_SW.SetState(7);
                    controller.AddHighlightedTile(Neighbor_SW);
                    break;
                case 1:
                    if(Neighbor_SW.GetNeighbor_SW() != null)
                    {
                        switch (Neighbor_SW.GetNeighbor_SW().GetState())
                        {
                            default: break;
                            case 0:
                                Neighbor_SW.SetState(8);
                                controller.AddTargetedTile(Neighbor_SW);
                                Neighbor_SW.GetNeighbor_SW().SetState(7);
                                controller.AddHighlightedTile(Neighbor_SW.GetNeighbor_SW());
                                ChainCheck(Neighbor_SW.GetNeighbor_SW(), 1, false);
                                break;
                            case 7:
                                Neighbor_SW.SetState(8);
                                controller.AddTargetedTile(Neighbor_SW);
                                Neighbor_SW.GetNeighbor_SW().SetState(7);
                                controller.AddHighlightedTile(Neighbor_SW.GetNeighbor_SW());
                                ChainCheck(Neighbor_SW.GetNeighbor_SW(), 1, false);
                                break;
                        }
                    }                   
                    break;
                case 2:
                    if(Neighbor_SW.GetNeighbor_SW() != null)
                    {
                        switch (Neighbor_SW.GetNeighbor_SW().GetState())
                        {
                            default: break;
                            case 0:
                                Neighbor_SW.SetState(10);
                                controller.AddTargetedTile(Neighbor_SW);
                                Neighbor_SW.GetNeighbor_SW().SetState(7);
                                controller.AddHighlightedTile(Neighbor_SW.GetNeighbor_SW());
                                ChainCheck(Neighbor_SW.GetNeighbor_SW(), 1, false);
                                break;
                            case 7:
                                Neighbor_SW.SetState(10);
                                controller.AddTargetedTile(Neighbor_SW);
                                Neighbor_SW.GetNeighbor_SW().SetState(7);
                                controller.AddHighlightedTile(Neighbor_SW.GetNeighbor_SW());
                                ChainCheck(Neighbor_SW.GetNeighbor_SW(), 1, false);
                                break;
                        }
                    }
                    break;
            }
        }
        if(Neighbor_SE != null)
        {
            switch (Neighbor_SE.GetState())
            {
                default: break;
                case 0:
                    Neighbor_SE.SetState(7);
                    controller.AddHighlightedTile(Neighbor_SE);
                    break;
                case 1:
                    if(Neighbor_SE.GetNeighbor_SE() != null)
                    {
                        switch (Neighbor_SE.GetNeighbor_SE().GetState())
                        {
                            default: break;
                            case 0:
                                Neighbor_SE.SetState(8);
                                controller.AddTargetedTile(Neighbor_SE);
                                Neighbor_SE.GetNeighbor_SE().SetState(7);
                                controller.AddHighlightedTile(Neighbor_SE.GetNeighbor_SE());
                                ChainCheck(Neighbor_SE.GetNeighbor_SE(), 1, false);
                                break;
                            case 7:
                                Neighbor_SE.SetState(8);
                                controller.AddTargetedTile(Neighbor_SE);
                                Neighbor_SE.GetNeighbor_SE().SetState(7);
                                controller.AddHighlightedTile(Neighbor_SE.GetNeighbor_SE());
                                ChainCheck(Neighbor_SE.GetNeighbor_SE(), 1, false);
                                break;
                        }
                    }
                    break;
                case 2:
                    if(Neighbor_SE.GetNeighbor_SE() != null)
                    {
                        switch (Neighbor_SE.GetNeighbor_SE().GetState())
                        {
                            default: break;
                            case 0:
                                Neighbor_SE.SetState(10);
                                controller.AddTargetedTile(Neighbor_SE);
                                Neighbor_SE.GetNeighbor_SE().SetState(7);
                                controller.AddHighlightedTile(Neighbor_SE.GetNeighbor_SE());
                                ChainCheck(Neighbor_SE.GetNeighbor_SE(), 1, false);
                                break;
                            case 7:
                                Neighbor_SE.SetState(10);
                                controller.AddTargetedTile(Neighbor_SE);
                                Neighbor_SE.GetNeighbor_SE().SetState(7);
                                controller.AddHighlightedTile(Neighbor_SE.GetNeighbor_SE());
                                ChainCheck(Neighbor_SE.GetNeighbor_SE(), 1, false);
                                break;
                        }
                    }   
                    break;
            }
        }
    }

    public void BlueKingHighlights()
    {
        if (Neighbor_NW != null)
        {
            switch (Neighbor_NW.GetState())
            {
                default: break;
                case 0:
                    Neighbor_NW.SetState(7);
                    controller.AddHighlightedTile(Neighbor_NW);
                    break;
                case 1:
                    if(Neighbor_NW.GetNeighbor_NW() != null)
                    {
                        switch (Neighbor_NW.GetNeighbor_NW().GetState())
                        {
                            default: break;
                            case 0:
                                Neighbor_NW.SetState(8);
                                controller.AddTargetedTile(Neighbor_NW);
                                Neighbor_NW.GetNeighbor_NW().SetState(7);
                                controller.AddHighlightedTile(Neighbor_NW.GetNeighbor_NW());
                                ChainCheck(Neighbor_NW.GetNeighbor_NW(), 1, false);
                                break;
                            case 7:
                                Neighbor_NW.SetState(8);
                                controller.AddTargetedTile(Neighbor_NW);
                                Neighbor_NW.GetNeighbor_NW().SetState(7);
                                controller.AddHighlightedTile(Neighbor_NW.GetNeighbor_NW());
                                ChainCheck(Neighbor_NW.GetNeighbor_NW(), 1, false);
                                break;
                        }
                    }                    
                    break;
                case 2:
                    if (Neighbor_NW.GetNeighbor_NW() != null)
                    {
                        switch (Neighbor_NW.GetNeighbor_NW().GetState())
                        {
                            default: break;
                            case 0:
                                Neighbor_NW.SetState(10);
                                controller.AddTargetedTile(Neighbor_NW);
                                Neighbor_NW.GetNeighbor_NW().SetState(7);
                                controller.AddHighlightedTile(Neighbor_NW.GetNeighbor_NW());
                                ChainCheck(Neighbor_NW.GetNeighbor_NW(), 1, false);
                                break;
                            case 7:
                                Neighbor_NW.SetState(10);
                                controller.AddTargetedTile(Neighbor_NW);
                                Neighbor_NW.GetNeighbor_NW().SetState(7);
                                controller.AddHighlightedTile(Neighbor_NW.GetNeighbor_NW());
                                ChainCheck(Neighbor_NW.GetNeighbor_NW(), 1, false);
                                break;
                        }
                    }
                    break;
            }
        }
        if (Neighbor_NE != null)
        {
            switch (Neighbor_NE.GetState())
            {
                default: break;
                case 0:
                    Neighbor_NE.SetState(7);
                    controller.AddHighlightedTile(Neighbor_NE);
                    break;
                case 1:
                    if(Neighbor_NE.GetNeighbor_NE() != null)
                    {
                        switch (Neighbor_NE.GetNeighbor_NE().GetState())
                        {
                            default: break;
                            case 0:
                                Neighbor_NE.SetState(8);
                                controller.AddTargetedTile(Neighbor_NE);
                                Neighbor_NE.GetNeighbor_NE().SetState(7);
                                controller.AddHighlightedTile(Neighbor_NE.GetNeighbor_NE());
                                ChainCheck(Neighbor_NE.GetNeighbor_NE(), 1, true);
                                break;
                            case 7:
                                Neighbor_NE.SetState(8);
                                controller.AddTargetedTile(Neighbor_NE);
                                Neighbor_NE.GetNeighbor_NE().SetState(7);
                                controller.AddHighlightedTile(Neighbor_NE.GetNeighbor_NE());
                                ChainCheck(Neighbor_NE.GetNeighbor_NE(), 1, true);
                                break;
                        }
                    }
                    break;
                case 2:
                    if (Neighbor_NE.GetNeighbor_NE() != null)
                    {
                        switch (Neighbor_NE.GetNeighbor_NE().GetState())
                        {
                            default: break;
                            case 0:
                                Neighbor_NE.SetState(10);
                                controller.AddTargetedTile(Neighbor_NE);
                                Neighbor_NE.GetNeighbor_NE().SetState(7);
                                controller.AddHighlightedTile(Neighbor_NE.GetNeighbor_NE());
                                ChainCheck(Neighbor_NE.GetNeighbor_NE(), 1, true);
                                break;
                            case 7:
                                Neighbor_NE.SetState(10);
                                controller.AddTargetedTile(Neighbor_NE);
                                Neighbor_NE.GetNeighbor_NE().SetState(7);
                                controller.AddHighlightedTile(Neighbor_NE.GetNeighbor_NE());
                                ChainCheck(Neighbor_NE.GetNeighbor_NE(), 1, true);
                                break;
                        }
                    }
                    break;
            }
        }
        if (Neighbor_SW != null)
        {
            switch (Neighbor_SW.GetState())
            {
                default: break;
                case 0:
                    Neighbor_SW.SetState(7);
                    controller.AddHighlightedTile(Neighbor_SW);
                    break;
                case 1:
                    if(Neighbor_SW.GetNeighbor_SW() != null)
                    {
                        switch (Neighbor_SW.GetNeighbor_SW().GetState())
                        {
                            default: break;
                            case 0:
                                Neighbor_SW.SetState(8);
                                controller.AddTargetedTile(Neighbor_SW);
                                Neighbor_SW.GetNeighbor_SW().SetState(7);
                                controller.AddHighlightedTile(Neighbor_SW.GetNeighbor_SW());
                                ChainCheck(Neighbor_SW.GetNeighbor_SW(), 1, false);
                                break;
                            case 7:
                                Neighbor_SW.SetState(8);
                                controller.AddTargetedTile(Neighbor_SW);
                                Neighbor_SW.GetNeighbor_SW().SetState(7);
                                controller.AddHighlightedTile(Neighbor_SW.GetNeighbor_SW());
                                ChainCheck(Neighbor_SW.GetNeighbor_SW(), 1, false);
                                break;
                        }
                    }
                    break;
                case 2:
                    if (Neighbor_SW.GetNeighbor_SW() != null)
                    {
                        switch (Neighbor_SW.GetNeighbor_SW().GetState())
                        {
                            default: break;
                            case 0:
                                Neighbor_SW.SetState(10);
                                controller.AddTargetedTile(Neighbor_SW);
                                Neighbor_SW.GetNeighbor_SW().SetState(7);
                                controller.AddHighlightedTile(Neighbor_SW.GetNeighbor_SW());
                                ChainCheck(Neighbor_SW.GetNeighbor_SW(), 1, false);
                                break;
                            case 7:
                                Neighbor_SW.SetState(10);
                                controller.AddTargetedTile(Neighbor_SW);
                                Neighbor_SW.GetNeighbor_SW().SetState(7);
                                controller.AddHighlightedTile(Neighbor_SW.GetNeighbor_SW());
                                ChainCheck(Neighbor_SW.GetNeighbor_SW(), 1, false);
                                break;
                        }
                    }
                    break;
            }
        }
        if (Neighbor_SE != null)
        {
            switch (Neighbor_SE.GetState())
            {
                default: break;
                case 0:
                    Neighbor_SE.SetState(7);
                    controller.AddHighlightedTile(Neighbor_SE);
                    break;
                case 1:
                    if(Neighbor_SE.GetNeighbor_SE() != null)
                    {
                        switch (Neighbor_SE.GetNeighbor_SE().GetState())
                        {
                            default: break;
                            case 0:
                                Neighbor_SE.SetState(8);
                                controller.AddTargetedTile(Neighbor_SE);
                                Neighbor_SE.GetNeighbor_SE().SetState(7);
                                controller.AddHighlightedTile(Neighbor_SE.GetNeighbor_SE());
                                ChainCheck(Neighbor_SE.GetNeighbor_SE(), 1, false);
                                break;
                            case 7:
                                Neighbor_SE.SetState(8);
                                controller.AddTargetedTile(Neighbor_SE);
                                Neighbor_SE.GetNeighbor_SE().SetState(7);
                                controller.AddHighlightedTile(Neighbor_SE.GetNeighbor_SE());
                                ChainCheck(Neighbor_SE.GetNeighbor_SE(), 1, false);
                                break;
                        }
                    }
                    break;
                case 2:
                    if (Neighbor_SE.GetNeighbor_SE() != null)
                    {
                        switch (Neighbor_SE.GetNeighbor_SE().GetState())
                        {
                            default: break;
                            case 0:
                                Neighbor_SE.SetState(10);
                                controller.AddTargetedTile(Neighbor_SE);
                                Neighbor_SE.GetNeighbor_SE().SetState(7);
                                controller.AddHighlightedTile(Neighbor_SE.GetNeighbor_SE());
                                ChainCheck(Neighbor_SE.GetNeighbor_SE(), 1, false);
                                break;
                            case 7:
                                Neighbor_SE.SetState(10);
                                controller.AddTargetedTile(Neighbor_SE);
                                Neighbor_SE.GetNeighbor_SE().SetState(7);
                                controller.AddHighlightedTile(Neighbor_SE.GetNeighbor_SE());
                                ChainCheck(Neighbor_SE.GetNeighbor_SE(), 1, false);
                                break;
                        }
                    }
                    break;
            }
        }
    }

    public void RedPieceHighlights()
    {
        if (Neighbor_NW != null)
        {
            switch(Neighbor_NW.GetState())
            {
                default: break;
                case 0:
                    Neighbor_NW.SetState(3);
                    controller.AddHighlightedTile(Neighbor_NW);
                    break;
                case 5:
                    if(Neighbor_NW.GetNeighbor_NW() != null)
                    {
                        switch (Neighbor_NW.GetNeighbor_NW().GetState())
                        {
                            default: break;
                            case 0:
                                Neighbor_NW.SetState(4);
                                controller.AddTargetedTile(Neighbor_NW);
                                Neighbor_NW.GetNeighbor_NW().SetState(3);
                                controller.AddHighlightedTile(Neighbor_NW.GetNeighbor_NW());
                                ChainCheck(Neighbor_NW.GetNeighbor_NW(), 0, false);
                                break;
                            case 3:
                                Neighbor_NW.SetState(4);
                                controller.AddTargetedTile(Neighbor_NW);
                                Neighbor_NW.GetNeighbor_NW().SetState(3);
                                controller.AddHighlightedTile(Neighbor_NW.GetNeighbor_NW());
                                ChainCheck(Neighbor_NW.GetNeighbor_NW(), 0, false);
                                break;
                        }
                    }
                    break;
                case 6:
                    if (Neighbor_NW.GetNeighbor_NW() != null)
                    {
                        switch (Neighbor_NW.GetNeighbor_NW().GetState())
                        {
                            default: break;
                            case 0:
                                Neighbor_NW.SetState(9);
                                controller.AddTargetedTile(Neighbor_NW);
                                Neighbor_NW.GetNeighbor_NW().SetState(3);
                                controller.AddHighlightedTile(Neighbor_NW.GetNeighbor_NW());
                                ChainCheck(Neighbor_NW.GetNeighbor_NW(), 0, false);
                                break;
                            case 3:
                                Neighbor_NW.SetState(9);
                                controller.AddTargetedTile(Neighbor_NW);
                                Neighbor_NW.GetNeighbor_NW().SetState(3);
                                controller.AddHighlightedTile(Neighbor_NW.GetNeighbor_NW());
                                ChainCheck(Neighbor_NW.GetNeighbor_NW(), 0, false);
                                break;
                        }
                    }
                    break;
            }
        }
        if(Neighbor_NE != null)
        {
            switch (Neighbor_NE.GetState())
            {
                default: break;
                case 0:
                    Neighbor_NE.SetState(3);
                    controller.AddHighlightedTile(Neighbor_NE);
                    break;
                case 5:
                    if(Neighbor_NE.GetNeighbor_NE() != null)
                    {
                        switch (Neighbor_NE.GetNeighbor_NE().GetState())
                        {
                            default: break;
                            case 0:
                                Neighbor_NE.SetState(4);
                                controller.AddTargetedTile(Neighbor_NE);
                                Neighbor_NE.GetNeighbor_NE().SetState(3);
                                controller.AddHighlightedTile(Neighbor_NE.GetNeighbor_NE());
                                ChainCheck(Neighbor_NE.GetNeighbor_NE(), 0, false);
                                break;
                            case 3:
                                Neighbor_NE.SetState(4);
                                controller.AddTargetedTile(Neighbor_NE);
                                Neighbor_NE.GetNeighbor_NE().SetState(3);
                                controller.AddHighlightedTile(Neighbor_NE.GetNeighbor_NE());
                                ChainCheck(Neighbor_NE.GetNeighbor_NE(), 0, false);
                                break;
                        }
                    }
                    break;
                case 6:
                    if (Neighbor_NE.GetNeighbor_NE() != null)
                    {
                        switch (Neighbor_NE.GetNeighbor_NE().GetState())
                        {
                            default: break;
                            case 0:
                                Neighbor_NE.SetState(9);
                                controller.AddTargetedTile(Neighbor_NE);
                                Neighbor_NE.GetNeighbor_NE().SetState(3);
                                controller.AddHighlightedTile(Neighbor_NE.GetNeighbor_NE());
                                ChainCheck(Neighbor_NE.GetNeighbor_NE(), 0, false);
                                break;
                            case 3:
                                Neighbor_NE.SetState(9);
                                controller.AddTargetedTile(Neighbor_NE);
                                Neighbor_NE.GetNeighbor_NE().SetState(3);
                                controller.AddHighlightedTile(Neighbor_NE.GetNeighbor_NE());
                                ChainCheck(Neighbor_NE.GetNeighbor_NE(), 0, false);
                                break;
                        }
                    }                    
                    break;
            }
        }
    }

    public void RedKingHighlights()
    {
        if (Neighbor_NW != null)
        {
            switch (Neighbor_NW.GetState())
            {
                default: break;
                case 0:
                    Neighbor_NW.SetState(3);
                    controller.AddHighlightedTile(Neighbor_NW);
                    break;
                case 5:
                    if(Neighbor_NW.GetNeighbor_NW() != null)
                    {
                        switch (Neighbor_NW.GetNeighbor_NW().GetState())
                        {
                            default: break;
                            case 0:
                                Neighbor_NW.SetState(4);
                                controller.AddTargetedTile(Neighbor_NW);
                                Neighbor_NW.GetNeighbor_NW().SetState(3);
                                controller.AddHighlightedTile(Neighbor_NW.GetNeighbor_NW());
                                ChainCheck(Neighbor_NW.GetNeighbor_NW(), 0, true);
                                break;
                            case 3:
                                Neighbor_NW.SetState(4);
                                controller.AddTargetedTile(Neighbor_NW);
                                Neighbor_NW.GetNeighbor_NW().SetState(3);
                                controller.AddHighlightedTile(Neighbor_NW.GetNeighbor_NW());
                                ChainCheck(Neighbor_NW.GetNeighbor_NW(), 0, true);
                                break;
                        }
                    }
                    break;
                case 6:
                    if (Neighbor_NW.GetNeighbor_NW() != null)
                    {
                        switch (Neighbor_NW.GetNeighbor_NW().GetState())
                        {
                            default: break;
                            case 0:
                                Neighbor_NW.SetState(9);
                                controller.AddTargetedTile(Neighbor_NW);
                                Neighbor_NW.GetNeighbor_NW().SetState(3);
                                controller.AddHighlightedTile(Neighbor_NW.GetNeighbor_NW());
                                ChainCheck(Neighbor_NW.GetNeighbor_NW(), 0, true);
                                break;
                            case 3:
                                Neighbor_NW.SetState(9);
                                controller.AddTargetedTile(Neighbor_NW);
                                Neighbor_NW.GetNeighbor_NW().SetState(3);
                                controller.AddHighlightedTile(Neighbor_NW.GetNeighbor_NW());
                                ChainCheck(Neighbor_NW.GetNeighbor_NW(), 0, true);
                                break;
                        }
                    }
                    break;
            }
        }
        if (Neighbor_NE != null)
        {
            switch (Neighbor_NE.GetState())
            {
                default: break;
                case 0:
                    Neighbor_NE.SetState(3);
                    controller.AddHighlightedTile(Neighbor_NE);
                    break;
                case 5:
                    if(Neighbor_NE.GetNeighbor_NE() != null)
                    {
                        switch (Neighbor_NE.GetNeighbor_NE().GetState())
                        {
                            default: break;
                            case 0:
                                Neighbor_NE.SetState(4);
                                controller.AddTargetedTile(Neighbor_NE);
                                Neighbor_NE.GetNeighbor_NE().SetState(3);
                                controller.AddHighlightedTile(Neighbor_NE.GetNeighbor_NE());
                                ChainCheck(Neighbor_NE.GetNeighbor_NE(), 0, true);
                                break;
                            case 3:
                                Neighbor_NE.SetState(4);
                                controller.AddTargetedTile(Neighbor_NE);
                                Neighbor_NE.GetNeighbor_NE().SetState(3);
                                controller.AddHighlightedTile(Neighbor_NE.GetNeighbor_NE());
                                ChainCheck(Neighbor_NE.GetNeighbor_NE(), 0, true);
                                break;

                        }
                    }
                    break;
                case 6:
                    if (Neighbor_NE.GetNeighbor_NE() != null)
                    {
                        switch (Neighbor_NE.GetNeighbor_NE().GetState())
                        {
                            default: break;
                            case 0:
                                Neighbor_NE.SetState(9);
                                controller.AddTargetedTile(Neighbor_NE);
                                Neighbor_NE.GetNeighbor_NE().SetState(3);
                                controller.AddHighlightedTile(Neighbor_NE.GetNeighbor_NE());
                                ChainCheck(Neighbor_NE.GetNeighbor_NE(), 0, true);
                                break;
                            case 3:
                                Neighbor_NE.SetState(9);
                                controller.AddTargetedTile(Neighbor_NE);
                                Neighbor_NE.GetNeighbor_NE().SetState(3);
                                controller.AddHighlightedTile(Neighbor_NE.GetNeighbor_NE());
                                ChainCheck(Neighbor_NE.GetNeighbor_NE(), 0, true);
                                break;
                        }
                    }
                    break;
            }
        }
        if(Neighbor_SW != null)
        {
            switch (Neighbor_SW.GetState())
            {
                default: break;
                case 0:
                    Neighbor_SW.SetState(3);
                    controller.AddHighlightedTile(Neighbor_SW);
                    break;
                case 5:
                    if(Neighbor_SW.GetNeighbor_SW() != null)
                    {
                        switch (Neighbor_SW.GetNeighbor_SW().GetState())
                        {
                            default: break;
                            case 0:
                                Neighbor_SW.SetState(4);
                                controller.AddTargetedTile(Neighbor_SW);
                                Neighbor_SW.GetNeighbor_SW().SetState(3);
                                controller.AddHighlightedTile(Neighbor_SW.GetNeighbor_SW());
                                ChainCheck(Neighbor_SW.GetNeighbor_SW(), 0, true);
                                break;
                            case 3:
                                Neighbor_SW.SetState(4);
                                controller.AddTargetedTile(Neighbor_SW);
                                Neighbor_SW.GetNeighbor_SW().SetState(3);
                                controller.AddHighlightedTile(Neighbor_SW.GetNeighbor_SW());
                                ChainCheck(Neighbor_SW.GetNeighbor_SW(), 0, true);
                                break;
                        }
                    }
                    break;
                case 6:
                    if (Neighbor_SW.GetNeighbor_SW() != null)
                    {
                        switch (Neighbor_SW.GetNeighbor_SW().GetState())
                        {
                            default: break;
                            case 0:
                                Neighbor_SW.SetState(9);
                                controller.AddTargetedTile(Neighbor_SW);
                                Neighbor_SW.GetNeighbor_SW().SetState(3);
                                controller.AddHighlightedTile(Neighbor_SW.GetNeighbor_SW());
                                ChainCheck(Neighbor_SW.GetNeighbor_SW(), 0, true);
                                break;
                            case 3:
                                Neighbor_SW.SetState(9);
                                controller.AddTargetedTile(Neighbor_SW);
                                Neighbor_SW.GetNeighbor_SW().SetState(3);
                                controller.AddHighlightedTile(Neighbor_SW.GetNeighbor_SW());
                                ChainCheck(Neighbor_SW.GetNeighbor_SW(), 0, true);
                                break;
                        }
                    }
                    break;
            }
        }
        if (Neighbor_SE != null)
        {
            switch (Neighbor_SE.GetState())
            {
                default: break;
                case 0:
                    Neighbor_SE.SetState(3);
                    controller.AddHighlightedTile(Neighbor_SE);
                    break;
                case 5:
                    if(Neighbor_SE.GetNeighbor_SE() != null)
                    {
                        switch (Neighbor_SE.GetNeighbor_SE().GetState())
                        {
                            default: break;
                            case 0:
                                Neighbor_SE.SetState(4);
                                controller.AddTargetedTile(Neighbor_SE);
                                Neighbor_SE.GetNeighbor_SE().SetState(3);
                                controller.AddHighlightedTile(Neighbor_SE.GetNeighbor_SE());
                                ChainCheck(Neighbor_SE.GetNeighbor_SE(), 0, true);
                                break;
                            case 3:
                                Neighbor_SE.SetState(4);
                                controller.AddTargetedTile(Neighbor_SE);
                                Neighbor_SE.GetNeighbor_SE().SetState(3);
                                controller.AddHighlightedTile(Neighbor_SE.GetNeighbor_SE());
                                ChainCheck(Neighbor_SE.GetNeighbor_SE(), 0, true);
                                break;
                        }
                    }
                    break;
                case 6:
                    if (Neighbor_SE.GetNeighbor_SE() != null)
                    {
                        switch (Neighbor_SE.GetNeighbor_SE().GetState())
                        {
                            default: break;
                            case 0:
                                Neighbor_SE.SetState(9);
                                controller.AddTargetedTile(Neighbor_SE);
                                Neighbor_SE.GetNeighbor_SE().SetState(3);
                                controller.AddHighlightedTile(Neighbor_SE.GetNeighbor_SE());
                                ChainCheck(Neighbor_SE.GetNeighbor_SE(), 0, true);
                                break;
                            case 3:
                                Neighbor_SE.SetState(9);
                                controller.AddTargetedTile(Neighbor_SE);
                                Neighbor_SE.GetNeighbor_SE().SetState(3);
                                controller.AddHighlightedTile(Neighbor_SE.GetNeighbor_SE());
                                ChainCheck(Neighbor_SE.GetNeighbor_SE(), 0, true);
                                break;
                        }
                    }
                    break;
            }
        }
    }

    private void ChainCheck(TileController root, int color, bool isKing)
    {
        switch(color)
        {
            case 0:
                if (root.Neighbor_NW != null &&
                    (root.Neighbor_NW.GetState() == 5 || root.Neighbor_NW.GetState() == 6) &&
                    root.Neighbor_NW.GetNeighbor_NW() != null && root.Neighbor_NW.GetNeighbor_NW().GetState() == 0)
                {
                    root.Neighbor_NW.SetState(4);
                    controller.AddTargetedTile(root.Neighbor_NW);
                    root.Neighbor_NW.GetNeighbor_NW().SetState(3);
                    controller.AddHighlightedTile(root.Neighbor_NW.GetNeighbor_NW());
                    ChainCheck(root.Neighbor_NW.GetNeighbor_NW(), color, isKing);
                }
                if (root.Neighbor_NE != null &&
                    (root.Neighbor_NE.GetState() == 5 || root.Neighbor_NE.GetState() == 6) &&
                    root.Neighbor_NE.GetNeighbor_NE() != null && root.Neighbor_NE.GetNeighbor_NE().GetState() == 0)
                {
                    root.Neighbor_NE.SetState(4);
                    controller.AddTargetedTile(root.Neighbor_NE);
                    root.Neighbor_NE.GetNeighbor_NE().SetState(3);
                    controller.AddHighlightedTile(root.Neighbor_NE.GetNeighbor_NE());
                    ChainCheck(root.Neighbor_NE.GetNeighbor_NE(), color, isKing);
                }
                if (isKing)
                {
                    if(root.Neighbor_SW != null &&
                        (root.Neighbor_SW.GetState() == 5 || root.Neighbor_SW.GetState() == 6) &&
                        root.Neighbor_SW.GetNeighbor_SW() != null &&
                        root.Neighbor_SW.GetNeighbor_SW().GetState() == 0)
                    {
                        root.Neighbor_SW.SetState(4);
                        controller.AddTargetedTile(root.Neighbor_SW);
                        root.Neighbor_SW.GetNeighbor_SW().SetState(3);
                        controller.AddHighlightedTile(root.Neighbor_SW.GetNeighbor_SW());
                        ChainCheck(root.Neighbor_SW.GetNeighbor_SW(), color, isKing);
                    }
                    if (root.Neighbor_SE != null &&
                        (root.Neighbor_SE.GetState() == 5 || root.Neighbor_SE.GetState() == 6) &&
                        root.Neighbor_SE.GetNeighbor_SE() != null && root.Neighbor_SE.GetNeighbor_SE().GetState() == 0)
                    {
                        root.Neighbor_SE.SetState(4);
                        controller.AddTargetedTile(root.Neighbor_SE);
                        root.Neighbor_SE.GetNeighbor_SE().SetState(3);
                        controller.AddHighlightedTile(root.Neighbor_SE.GetNeighbor_SE());
                        ChainCheck(root.Neighbor_SE.GetNeighbor_SE(), color, isKing);
                    }
                }
                break;
            case 1:
                if (root.Neighbor_SW != null &&
                    (root.Neighbor_SW.GetState() == 1 || root.Neighbor_SW.GetState() == 2) &&
                    root.Neighbor_SW.GetNeighbor_SW() != null && root.Neighbor_SW.GetNeighbor_SW().GetState() == 0)
                {
                    root.Neighbor_SW.SetState(8);
                    controller.AddTargetedTile(root.Neighbor_SW);
                    root.Neighbor_SW.GetNeighbor_SW().SetState(7);
                    controller.AddHighlightedTile(root.Neighbor_SW.GetNeighbor_SW());
                    ChainCheck(root.Neighbor_SW.GetNeighbor_SW(), color, isKing);
                }
                if (root.Neighbor_SE != null &&
                    (root.Neighbor_SE.GetState() == 1 || root.Neighbor_SE.GetState() == 2) &&
                    root.Neighbor_SE.GetNeighbor_SE() != null && root.Neighbor_SE.GetNeighbor_SE().GetState() == 0)
                {
                    root.Neighbor_SE.SetState(8);
                    controller.AddTargetedTile(root.Neighbor_SE);
                    root.Neighbor_SE.GetNeighbor_SE().SetState(7);
                    controller.AddHighlightedTile(root.Neighbor_SE.GetNeighbor_SE());
                    ChainCheck(root.Neighbor_SE.GetNeighbor_SE(), color, isKing);
                }
                if (isKing)
                {
                    if (root.Neighbor_NW != null &&
                        (root.Neighbor_NW.GetState() == 1 || root.Neighbor_NW.GetState() == 2) &&
                        root.Neighbor_NW.GetNeighbor_NW() != null &&
                        root.Neighbor_NW.GetNeighbor_NW().GetState() == 0)
                    {
                        root.Neighbor_NW.SetState(8);
                        controller.AddTargetedTile(root.Neighbor_NW);
                        root.Neighbor_NW.GetNeighbor_NW().SetState(7);
                        controller.AddHighlightedTile(root.Neighbor_NW.GetNeighbor_NW());
                        ChainCheck(root.Neighbor_NW.GetNeighbor_NW(), color, isKing);
                    }
                    if (root.Neighbor_NE != null &&
                        (root.Neighbor_NE.GetState() == 1 || root.Neighbor_NE.GetState() == 2) &&
                        root.Neighbor_NE.GetNeighbor_NE() != null && root.Neighbor_NE.GetNeighbor_NE().GetState() == 0)
                    {
                        root.Neighbor_NE.SetState(8);
                        controller.AddTargetedTile(root.Neighbor_NE);
                        root.Neighbor_NE.GetNeighbor_NE().SetState(7);
                        controller.AddHighlightedTile(root.Neighbor_NE.GetNeighbor_NE());
                        ChainCheck(root.Neighbor_NE.GetNeighbor_NE(), color, isKing);
                    }
                }
                break;

        }
    }

    public void FindTargetPath(TileController point, TileController destination, int color)
    {
        int pointY = int.Parse(point.gameObject.name.Substring(0, 1));
        int pointX = int.Parse(point.gameObject.name.Substring(2, 1));
        int destinationY = int.Parse(destination.gameObject.name.Substring(0, 1));
        int destinationX = int.Parse(destination.gameObject.name.Substring(2, 1));
        if(destinationX < pointX)
        {
            if(destinationY < pointY)
            {
                switch(point.Neighbor_SW.GetState())
                {
                    default:
                        jump();
                        break;
                    case 0:
                        point.Neighbor_SW.SetState(point.GetState());
                        point.SetState(0);
                        break;
                }
                void jump()
                {
                    point.Neighbor_SW.SetState(0);
                    TileController newPoint = point.Neighbor_SW.GetNeighbor_SW();
                    if (newPoint == destination)
                    {
                        controller.RemovePieces(1, color);
                        newPoint.SetState(point.GetState());
                        point.SetState(0);
                        return;
                    }
                    else
                    {
                        controller.RemovePieces(1, color);
                        newPoint.SetState(point.GetState());
                        point.SetState(0);
                        FindTargetPath(newPoint, destination, color);
                    }
                }

            }
            else if(destinationY == pointY)
            {
                if(color == 0)
                {
                    TileController newPoint;
                    if (point.Neighbor_NW != null &&
                        point.Neighbor_NW.GetNeighbor_NW() != null &&
                        (point.Neighbor_NW.GetNeighbor_NW().GetNeighbor_SW().GetState() == 4 || point.Neighbor_NW.GetNeighbor_NW().GetNeighbor_SW().GetState() == 9) &&
                        (point.Neighbor_NW.GetState() == 4 || point.Neighbor_NW.GetState() == 9))
                    {
                        point.Neighbor_NW.SetState(0);
                        point.Neighbor_NW.GetNeighbor_NW().GetNeighbor_SW().SetState(0);
                        newPoint = point.Neighbor_NW.GetNeighbor_NW().GetNeighbor_SW().GetNeighbor_SW();
                    }
                    else
                    {
                        point.Neighbor_SW.SetState(0);
                        point.Neighbor_SW.GetNeighbor_SW().GetNeighbor_NW().SetState(0);
                        newPoint = point.Neighbor_SW.GetNeighbor_SW().GetNeighbor_NW().GetNeighbor_NW();
                    }
                    controller.RemovePieces(2, color);
                    newPoint.SetState(point.GetState());
                    point.SetState(0);
                    return;
                }
                else
                {
                    TileController newPoint;
                    if (point.Neighbor_NW != null &&
                        point.Neighbor_NW.GetNeighbor_NW() != null &&
                        (point.Neighbor_NW.GetNeighbor_NW().GetNeighbor_SW().GetState() == 8 || point.Neighbor_NW.GetNeighbor_NW().GetNeighbor_SW().GetState() == 10) &&
                        (point.Neighbor_NW.GetState() == 8 || point.Neighbor_NW.GetState() == 10))
                    {
                        point.Neighbor_NW.SetState(0);
                        point.Neighbor_NW.GetNeighbor_NW().GetNeighbor_SW().SetState(0);
                        newPoint = point.Neighbor_NW.GetNeighbor_NW().GetNeighbor_SW().GetNeighbor_SW();
                    }
                    else
                    {
                        point.Neighbor_SW.SetState(0);
                        point.Neighbor_SW.GetNeighbor_SW().GetNeighbor_NW().SetState(0);
                        newPoint = point.Neighbor_SW.GetNeighbor_SW().GetNeighbor_NW().GetNeighbor_NW();
                    }
                    controller.RemovePieces(2, color);
                    newPoint.SetState(point.GetState());
                    point.SetState(0);
                    return;
                }
                
            }
            else if(destinationY > pointY)
            {
                switch(point.Neighbor_NW.GetState())
                {
                    default:
                        jump();
                        break;
                    case 0:
                        point.Neighbor_NW.SetState(point.GetState());
                        point.SetState(0);
                        break;
                }

                void jump()
                {
                    point.Neighbor_NW.SetState(0);
                    TileController newPoint = point.Neighbor_NW.GetNeighbor_NW();
                    if (newPoint == destination)
                    {
                        controller.RemovePieces(1, color);
                        newPoint.SetState(point.GetState());
                        point.SetState(0);
                        return;
                    }
                    else
                    {
                        controller.RemovePieces(1, color);
                        newPoint.SetState(point.GetState());
                        point.SetState(0);
                        FindTargetPath(newPoint, destination, color);
                    }
                }
            }
        }
        else if(destinationX == pointX)
        {
            if(destinationY < pointY)
            {
                if(color == 0)
                {
                    TileController newPoint;
                    if (point.Neighbor_SW != null &&
                        point.Neighbor_SW.GetNeighbor_SW() != null &&
                        (point.Neighbor_SW.GetNeighbor_SW().GetNeighbor_SE().GetState() == 4 || point.Neighbor_SW.GetNeighbor_SW().GetNeighbor_SE().GetState() == 9) &&
                        (point.Neighbor_SW.GetState() == 4 || point.Neighbor_SW.GetState() == 9))
                    {
                        point.Neighbor_SW.SetState(0);
                        point.Neighbor_SW.GetNeighbor_SW().GetNeighbor_SE().SetState(0);
                        newPoint = point.Neighbor_SW.GetNeighbor_SW().GetNeighbor_SE().GetNeighbor_SE();
                    }
                    else
                    {
                        point.Neighbor_SE.SetState(0);
                        point.Neighbor_SE.GetNeighbor_SE().GetNeighbor_SW().SetState(0);
                        newPoint = point.Neighbor_SE.GetNeighbor_SE().GetNeighbor_SW().GetNeighbor_SW();
                    }
                    controller.RemovePieces(2, color);
                    newPoint.SetState(point.GetState());
                    point.SetState(0);
                    return;
                }  
                else
                {
                    TileController newPoint;
                    if (point.Neighbor_SW != null &&
                        point.Neighbor_SW.GetNeighbor_SW() != null &&
                        (point.Neighbor_SW.GetNeighbor_SW().GetNeighbor_SE().GetState() == 8 || point.Neighbor_SW.GetNeighbor_SW().GetNeighbor_SE().GetState() == 10) &&
                        (point.Neighbor_SW.GetState() == 8 || point.Neighbor_SW.GetState() == 10))
                    {
                        point.Neighbor_SW.SetState(0);
                        point.Neighbor_SW.GetNeighbor_SW().GetNeighbor_SE().SetState(0);
                        newPoint = point.Neighbor_SW.GetNeighbor_SW().GetNeighbor_SE().GetNeighbor_SE();
                    }
                    else
                    {
                        point.Neighbor_SE.SetState(0);
                        point.Neighbor_SE.GetNeighbor_SE().GetNeighbor_SW().SetState(0);
                        newPoint = point.Neighbor_SE.GetNeighbor_SE().GetNeighbor_SW().GetNeighbor_SW();
                    }
                    controller.RemovePieces(2, color);
                    newPoint.SetState(point.GetState());
                    point.SetState(0);
                    return;
                }
            }
            else
            {
                if(color == 0)
                {
                    TileController newPoint;
                    if (point.Neighbor_NW != null &&
                        point.Neighbor_NW.GetNeighbor_NW() != null &&
                        (point.Neighbor_NW.GetNeighbor_NW().GetNeighbor_NE().GetState() == 4 || point.Neighbor_NW.GetNeighbor_NW().GetNeighbor_NE().GetState() == 9) &&
                        (point.Neighbor_NW.GetState() == 4 || point.Neighbor_NW.GetState() == 9))
                    {
                        point.Neighbor_NW.SetState(0);
                        point.Neighbor_NW.GetNeighbor_NW().GetNeighbor_NE().SetState(0);
                        newPoint = point.Neighbor_NW.GetNeighbor_NW().GetNeighbor_NE().GetNeighbor_NE();
                    }
                    else
                    {
                        point.Neighbor_NE.SetState(0);
                        point.Neighbor_NE.GetNeighbor_NE().GetNeighbor_NW().SetState(0);
                        newPoint = point.Neighbor_NE.GetNeighbor_NE().GetNeighbor_NW().GetNeighbor_NW();
                    }
                    controller.RemovePieces(2, color);
                    newPoint.SetState(point.GetState());
                    point.SetState(0);
                }
                else
                {
                    TileController newPoint;
                    if (point.Neighbor_NW != null &&
                        point.Neighbor_NW.GetNeighbor_NW() != null &&
                        (point.Neighbor_NW.GetNeighbor_NW().GetNeighbor_NE().GetState() == 8 || point.Neighbor_NW.GetNeighbor_NW().GetNeighbor_NE().GetState() == 10) &&
                        (point.Neighbor_NW.GetState() == 8 || point.Neighbor_NW.GetState() == 10))
                    {
                        point.Neighbor_NW.SetState(0);
                        point.Neighbor_NW.GetNeighbor_NW().GetNeighbor_NE().SetState(0);
                        newPoint = point.Neighbor_NW.GetNeighbor_NW().GetNeighbor_NE().GetNeighbor_NE();
                    }
                    else
                    {
                        point.Neighbor_NE.SetState(0);
                        point.Neighbor_NE.GetNeighbor_NE().GetNeighbor_NW().SetState(0);
                        newPoint = point.Neighbor_NE.GetNeighbor_NE().GetNeighbor_NW().GetNeighbor_NW();
                    }
                    controller.RemovePieces(2, color);
                    newPoint.SetState(point.GetState());
                    point.SetState(0);
                }
            }
        }
        else if (destinationX > pointX)
        {
            if(destinationY < pointY)
            {
                switch (point.Neighbor_SE.GetState())
                {
                    default:
                        jump();
                        break;
                    case 0:
                        point.Neighbor_SE.SetState(point.GetState());
                        point.SetState(0);
                        break;
                }
                void jump()
                {
                    point.Neighbor_SE.SetState(0);
                    TileController newPoint = point.Neighbor_SE.GetNeighbor_SE();
                    if (newPoint == destination)
                    {
                        controller.RemovePieces(1, color);
                        newPoint.SetState(point.GetState());
                        point.SetState(0);
                        return;
                    }
                    else
                    {
                        controller.RemovePieces(1, color);
                        newPoint.SetState(point.GetState());
                        point.SetState(0);
                        FindTargetPath(newPoint, destination, color);
                    }
                }
            }
            else if(destinationY == pointY)
            {
                if (color == 0)
                {
                    TileController newPoint;
                    if (point.Neighbor_NE != null && 
                        point.Neighbor_NE.GetNeighbor_NE() != null && 
                        (point.Neighbor_NE.GetNeighbor_NE().GetNeighbor_SE().GetState() == 4 || point.Neighbor_NE.GetNeighbor_NE().GetNeighbor_SE().GetState() == 9) && 
                        (point.Neighbor_NE.GetState() == 4 || point.Neighbor_NE.GetState() == 9))
                    {
                        point.Neighbor_NE.SetState(0);
                        point.Neighbor_NE.GetNeighbor_NE().GetNeighbor_SE().SetState(0);
                        newPoint = point.Neighbor_NE.GetNeighbor_NE().GetNeighbor_SE().GetNeighbor_SE();
                    }
                    else
                    {
                        point.Neighbor_SE.SetState(0);
                        point.Neighbor_SE.GetNeighbor_SE().GetNeighbor_NE().SetState(0);
                        newPoint = point.Neighbor_SE.GetNeighbor_SE().GetNeighbor_NE().GetNeighbor_NE();
                    }
                    controller.RemovePieces(2, color);
                    newPoint.SetState(point.GetState());
                    point.SetState(0);
                    return;
                }
                else
                {
                    TileController newPoint;
                    if (point.Neighbor_NE != null &&
                        point.Neighbor_NE.GetNeighbor_NE() != null &&
                        (point.Neighbor_NE.GetNeighbor_NE().GetNeighbor_SE().GetState() == 8 || point.Neighbor_NE.GetNeighbor_NE().GetNeighbor_SE().GetState() == 10) &&
                        (point.Neighbor_NE.GetState() == 8 || point.Neighbor_NE.GetState() == 10))
                    {
                        point.Neighbor_NE.SetState(0);
                        point.Neighbor_NE.GetNeighbor_NE().GetNeighbor_SE().SetState(0);
                        newPoint = point.Neighbor_NE.GetNeighbor_NE().GetNeighbor_SE().GetNeighbor_SE();
                    }
                    else
                    {
                        point.Neighbor_SE.SetState(0);
                        point.Neighbor_SE.GetNeighbor_SE().GetNeighbor_NE().SetState(0);
                        newPoint = point.Neighbor_SE.GetNeighbor_SE().GetNeighbor_NE().GetNeighbor_NE();
                    }
                    controller.RemovePieces(2, color);
                    newPoint.SetState(point.GetState());
                    point.SetState(0);
                    return;
                }
            }
            else if(destinationY > pointY)
            {
                switch (point.Neighbor_NE.GetState())
                {
                    default:
                        jump();
                        break;
                    case 0:
                        point.Neighbor_NE.SetState(point.GetState());
                        point.SetState(0);
                        break;
                }

                void jump()
                {
                    point.Neighbor_NE.SetState(0);
                    TileController newPoint = point.Neighbor_NE.GetNeighbor_NE();
                    if (newPoint == destination)
                    {
                        controller.RemovePieces(1, color);
                        newPoint.SetState(point.GetState());
                        point.SetState(0);
                        return;
                    }
                    else
                    {
                        controller.RemovePieces(1, color);
                        newPoint.SetState(point.GetState());
                        point.SetState(0);
                        FindTargetPath(newPoint, destination, color);
                    }
                }
            }
        }

        if(destination.isEdge())
        {
            switch(color)
            {
                case 0:
                    destination.SetState(2);
                    break;
                case 1:
                    destination.SetState(6);
                    break;
            }
        }
    }
}
