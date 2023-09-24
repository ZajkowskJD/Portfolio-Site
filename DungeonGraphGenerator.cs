using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Array2DEditor;
using Unity.AI.Navigation;

public class DungeonGraphGenerator : MonoBehaviour
{
    [SerializeField] private int desiredFloors = 0;
    [SerializeField] private int attemptCeiling = 100;
    [SerializeField] private GameObject floorTile;
    [SerializeField] private GameObject wallTile;
    [SerializeField] private GameObject floorParent;
    [SerializeField] private GameObject vcam;
    [SerializeField] private Array2DInt dungeonGraph;
    [SerializeField] private FloorShape[] floorShapes;
    [SerializeField] int floorShapeIndex = 0;
    [SerializeField] private Array2DBool splitGraph;
    [SerializeField] private Transform minimapCam;

    private int floorsAdded = 0;
    private Dictionary<int, Vector2Int> roomPivots;
    private Dictionary<int, List<Vector2Int>> roomWalls;
    private List<Vector2Int> floorTiles;

    private void Awake()
    {
        minimapCam.position = new Vector3(dungeonGraph.GridSize.x * 0.5f, minimapCam.position.y, dungeonGraph.GridSize.y * 0.5f);
        vcam.transform.position = new Vector3(dungeonGraph.GridSize.x * 0.5f, vcam.transform.position.y, dungeonGraph.GridSize.y * 0.5f);
        roomPivots = new Dictionary<int, Vector2Int>();
        floorTiles = new List<Vector2Int>();
        roomPivots.Add(-1, new Vector2Int((dungeonGraph.GridSize.x * dungeonGraph.GridSize.x) + 1, (dungeonGraph.GridSize.y * dungeonGraph.GridSize.y) + 1));
        roomWalls = new Dictionary<int, List<Vector2Int>>();
        if (splitGraph.GridSize != dungeonGraph.GridSize) splitGraph.RequestGridSizeChange(dungeonGraph.GridSize);
        StartCoroutine(_generateDungeonGraph());
    }
    private IEnumerator _generateDungeonGraph()
    {
        Debug.Log("......Generating dungeon graph.........");
        int attemptIndex = 0;
        while (floorsAdded < desiredFloors && attemptIndex < attemptCeiling) {
            BuildFloorPlan(ref attemptIndex);
            yield return new WaitForEndOfFrame();
        }
        FillDungeonHoles();
        yield return StartCoroutine(DrawDungeonPaths());
        PrintDungeon();
        GenerateNavMesh();
        Debug.Log("Generated.");     
        
    }

    private IEnumerator DrawDungeonPaths()
    {
        List<int> roomsToClose = new List<int>();
        for (int i = 0; i < desiredFloors; i++) roomsToClose.Add(i);
        int r = 0;
        while(roomsToClose.Count > 1) {
            Vector2Int homePivot = roomPivots[r];
            //Debug.Log(homePivot + ", room " + r);
            int[] closestPivots = new int[2];
            closestPivots[0] = -1; //shortest distance
            closestPivots[1] = -1; //second shortest distance
            for(int i = 0; i < roomPivots.Count - 1; i++) {
                //Debug.Log("testing room " + i + ", pivot at " + roomPivots[i]);
                if(i != r && (homePivot - roomPivots[i]).magnitude < (homePivot - roomPivots[closestPivots[0]]).magnitude && roomsToClose.Contains(i)) {
                    //Debug.Log("valid closest pivot found");
                    closestPivots[1] = closestPivots[0];
                    closestPivots[0] = i;
                }
                else if(i != r && (homePivot - roomPivots[i]).magnitude < (homePivot - roomPivots[closestPivots[1]]).magnitude && roomsToClose.Contains(i))
                {
                    //Debug.Log("valid second closest pivot found");
                    closestPivots[1] = i;
                }
            }

            //Debug.Log("rooms to close: " + roomsToClose.Count);
            //Debug.Log(closestPivots[0] + ", " + closestPivots[1]);
            if(closestPivots[0] != -1)
            {
                //draw path to shortest room
                DrawCorridor(closestPivots[0], r);
                //close shortest room and set as new location to search from
                roomsToClose.Remove(r);

            }
            if (closestPivots[1] != -1)
            {
                //draw path to second shortest room
                DrawCorridor(closestPivots[1], r);
                //close second shortest room
                roomsToClose.Remove(closestPivots[1]);
            }
            else roomsToClose.Remove(closestPivots[0]);
            r = closestPivots[0];
            //Debug.Log("new rooms to close: " + roomsToClose.Count);
            yield return new WaitForEndOfFrame();
        }
    }

    private void DrawCorridor(int roomToReach, int roomStartingFrom)
    {
        Vector2Int corridorEnd = GetCorridorAnchor(roomToReach);
        Vector2Int endNormal = GetCorridorNormal(corridorEnd);
        while(corridorEnd.x+endNormal.x == 0 || corridorEnd.y + endNormal.y == 0 || corridorEnd.x+endNormal.x == dungeonGraph.GridSize.x - 1 || corridorEnd.y+endNormal.y == dungeonGraph.GridSize.y -1)
        {
            corridorEnd = GetCorridorAnchor(roomToReach);
            endNormal = GetCorridorNormal(corridorEnd);
        }

        Vector2Int corridorStart = GetCorridorAnchor(roomStartingFrom);
        Vector2Int startNormal = GetCorridorNormal(corridorStart);
        while (corridorStart.x + startNormal.x == 0 || corridorStart.y + startNormal.y == 0 || corridorStart.x + startNormal.x == dungeonGraph.GridSize.x - 1 || corridorStart.y + startNormal.y == dungeonGraph.GridSize.y - 1)
        {
            corridorStart = GetCorridorAnchor(roomStartingFrom);
            startNormal = GetCorridorNormal(corridorStart);
        }

        dungeonGraph.SetCell(corridorStart.x, corridorStart.y, 1);
        roomWalls[roomStartingFrom].Remove(corridorStart);
        dungeonGraph.SetCell(corridorStart.x + startNormal.x, corridorStart.y + startNormal.y, 1);

        Vector2Int walkPos = corridorStart + startNormal;
        //Debug.Log("entering line 101");
        while (walkPos != corridorEnd + endNormal)
        {
            //Debug.Log("Walk pos: "+walkPos);
            if (startNormal.x != 0 && walkPos.y < (corridorEnd.y + endNormal.y)) walkPos.y++;
            else if (startNormal.x != 0 && walkPos.y > (corridorEnd.y + endNormal.y)) walkPos.y--;
            else if (startNormal.y != 0 && walkPos.x < (corridorEnd.x + endNormal.x)) walkPos.x++;
            else if (startNormal.y != 0 && walkPos.x > (corridorEnd.x + endNormal.x)) walkPos.x--;
            else if (walkPos.x == (corridorEnd.x + endNormal.x) && walkPos.y < (corridorEnd.y + endNormal.y)) walkPos.y++;
            else if (walkPos.x == (corridorEnd.x + endNormal.x) && walkPos.y > (corridorEnd.y + endNormal.y)) walkPos.y--;
            else if (walkPos.y == (corridorEnd.y + endNormal.y) && walkPos.x < (corridorEnd.x + endNormal.x)) walkPos.x++;
            else if (walkPos.y == (corridorEnd.y + endNormal.y) && walkPos.x > (corridorEnd.x + endNormal.x)) walkPos.x--;

            switch (dungeonGraph.GetCell(walkPos.x, walkPos.y))
            {
                case 1:
                    break;
                case 2:
                    if(walkPos.x != 0 && walkPos.y != 0 && walkPos.x != dungeonGraph.GridSize.x -1 && walkPos.y != dungeonGraph.GridSize.y -1)
                        dungeonGraph.SetCell(walkPos.x, walkPos.y, 1);
                    bool foundWall = false;
                    for (int i = 0; i < desiredFloors; i++)
                    {
                        if (!foundWall && roomWalls[i].Contains(walkPos))
                        {
                            roomWalls[i].Remove(walkPos);
                            foundWall = true;
                        }
                    }
                    break;
                case 3:
                    if (walkPos.x != 0 && walkPos.y != 0 && walkPos.x != dungeonGraph.GridSize.x - 1 && walkPos.y != dungeonGraph.GridSize.y - 1)
                        dungeonGraph.SetCell(walkPos.x, walkPos.y, 1);
                    break;
            }
        }
        dungeonGraph.SetCell(corridorEnd.x + endNormal.x, corridorEnd.y + endNormal.y, 1);
        dungeonGraph.SetCell(corridorEnd.x, corridorEnd.y, 1);
        roomWalls[roomToReach].Remove(corridorEnd);
    }

    private Vector2Int GetCorridorAnchor(int room)
    {
        Vector2Int corridorAnchor = Vector2Int.zero;
        Vector2Int corridorNormal = Vector2Int.zero;

        //Debug.Log("entering line 144");
        do {
            corridorAnchor = roomWalls[room][Random.Range(0, roomWalls[room].Count)];
            corridorNormal = GetCorridorNormal(corridorAnchor);
        } while (!(corridorAnchor.x + corridorNormal.x >= 0 && corridorAnchor.y + corridorNormal.y >= 0 &&
                     corridorAnchor.x + corridorNormal.x < dungeonGraph.GridSize.x && corridorAnchor.y + corridorNormal.y < dungeonGraph.GridSize.y));

        return corridorAnchor;
    }

    private Vector2Int GetCorridorNormal(Vector2Int anchor)
    {
        Vector2Int corridorNormal = Vector2Int.zero;
        if (dungeonGraph.GetCell(anchor.x, anchor.y + 1) == 1) corridorNormal = new Vector2Int(0, -1);
        else if (dungeonGraph.GetCell(anchor.x + 1, anchor.y) == 1) corridorNormal = new Vector2Int(-1, 0);
        else if (dungeonGraph.GetCell(anchor.x, anchor.y - 1) == 1) corridorNormal = new Vector2Int(0, 1);
        else if (dungeonGraph.GetCell(anchor.x - 1, anchor.y) == 1) corridorNormal = new Vector2Int(1, 0);
        return corridorNormal;
    }

    private void FillDungeonHoles()
    {
        for(int i = 0; i < dungeonGraph.GridSize.y; i++) {
            for(int j = 0; j < dungeonGraph.GridSize.x; j++) {
                if (dungeonGraph.GetCell(j, i) == 0) dungeonGraph.SetCell(j, i, 3);
            }
        }
    }
    private void BuildFloorPlan(ref int attemptIndex)
    {
        AddNewFloorShape(floorShapeIndex, ref floorsAdded);
        floorShapeIndex++;
        if (floorShapeIndex > floorShapes.Length - 1) floorShapeIndex = 0;
        attemptIndex++;
    }
    private void PrintDungeon()
    {
        for (int i = 0; i < dungeonGraph.GridSize.y; i++)
        {
            for (int j = 0; j < dungeonGraph.GridSize.x; j++)
            {
                switch (dungeonGraph.GetCell(j, i))
                {
                    case 0:
                        break;
                    case 1:
                        Instantiate(floorTile, new Vector3(j, 0, i), Quaternion.identity, floorParent.transform);
                        floorTiles.Add(new Vector2Int(j, i));
                        break;
                    case 3:
                    case 2:
                        Instantiate(wallTile, new Vector3(j, 0, i) + new Vector3(0, 0.5f, 0), Quaternion.identity, floorParent.transform);
                        break;
                    default:
                        break;
                }
            }
        }
    }
    private void AddNewFloorShape(int floorShapeIndex, ref int floorsAdded)
    {
        Vector2Int nudge = Vector2Int.zero;
        Vector2Int randompoint = new Vector2Int(Random.Range(0, dungeonGraph.GridSize.x - floorShapes[floorShapeIndex].floor.GridSize.x), Random.Range(0, dungeonGraph.GridSize.y - floorShapes[floorShapeIndex].floor.GridSize.y));
        if (!SplitGraphBorderCheck(randompoint, floorShapes[floorShapeIndex].floor.GridSize, ref nudge)) return;
        if (nudge != Vector2Int.zero) randompoint += nudge;
        roomPivots.Add(floorsAdded, randompoint);
            for (int i = randompoint.y; i < randompoint.y + floorShapes[floorShapeIndex].floor.GridSize.y; i++) {
                for (int j = randompoint.x; j < randompoint.x + floorShapes[floorShapeIndex].floor.GridSize.x; j++) {
                    if (floorShapes[floorShapeIndex].floor.GetCell(j - randompoint.x, i - randompoint.y) != 0 && dungeonGraph.GetCell(j,i) != 1) {
                        dungeonGraph.SetCell(j, i, floorShapes[floorShapeIndex].floor.GetCell(j - randompoint.x, i - randompoint.y));
                        switch(dungeonGraph.GetCell(j,i)) {
                            default: break;
                            case 2:
                            if (!roomWalls.ContainsKey(floorsAdded)) roomWalls.Add(floorsAdded, new List<Vector2Int>());
                            roomWalls[floorsAdded].Add(new Vector2Int(j, i));
                            break;
                        }
                    }
                    splitGraph.SetCell(j, i, true);
                }
            }
        floorsAdded++;
    }
    private bool SplitGraphBorderCheck(Vector2Int pivot, Vector2Int gridSize, ref Vector2Int nudge) {
        Vector2Int nudgeCheck = Vector2Int.zero;
        if(splitGraph.GetCell(pivot.x, pivot.y)) return false;

        //top side check
        for(int i = 0; i < gridSize.x; i++) {
            if (splitGraph.GetCell(pivot.x + i, pivot.y) && (-gridSize.x + i) < nudgeCheck.x) {
                nudgeCheck.x = -gridSize.x + i;
                break;
            }
        }
        //left side check
        for(int i = 0; i < gridSize.y; i++) {
            if (splitGraph.GetCell(pivot.x, pivot.y + i) && (-gridSize.y + i) < nudgeCheck.y) {
                nudgeCheck.y = -gridSize.y + i;
                break;
            }
        }
        //bottom side check
        for (int i = 0; i < gridSize.x; i++) {
            if (splitGraph.GetCell(pivot.x + i, pivot.y + gridSize.y - 1) && (-gridSize.x + i) < nudgeCheck.x) {
                nudgeCheck.x = -gridSize.x + i;
                break;
            }
        }
        //right side check
        for(int i = 0; i < gridSize.y; i++) {
            if (splitGraph.GetCell(pivot.x + gridSize.x - 1, pivot.y + i) && (-gridSize.y + i) < nudgeCheck.y) {
                nudgeCheck.y = -gridSize.y + i;
                break;
            }
        }
        if(nudgeCheck != Vector2Int.zero) {
            if(nudgeCheck.x != 0) {
                if (TestNudgeLocation(pivot, gridSize, new Vector2Int(nudgeCheck.x, 0), ref nudgeCheck)) {
                    nudge = new Vector2Int(nudgeCheck.x, 0);
                    return true;
                }
                else if (TestNudgeLocation(pivot, gridSize, new Vector2Int(-nudgeCheck.x, 0), ref nudgeCheck)) {
                    nudge = new Vector2Int(-nudgeCheck.x, 0);
                    return true;
                }
            }
            else {
                if(TestNudgeLocation(pivot, gridSize, new Vector2Int(gridSize.x, 0), ref nudgeCheck)) {
                    nudge = new Vector2Int(gridSize.x, 0);
                    return true;
                }
                else if (TestNudgeLocation(pivot, gridSize, new Vector2Int(-gridSize.x, 0), ref nudgeCheck)) {
                    nudge = new Vector2Int(gridSize.x, 0);
                    return true;
                }
            }
            if (nudgeCheck.y != 0) {
                if (TestNudgeLocation(pivot, gridSize, new Vector2Int(0, nudgeCheck.y), ref nudgeCheck)) {
                    nudge = new Vector2Int(0, nudgeCheck.y);
                    return true;
                }
                else if (TestNudgeLocation(pivot, gridSize, new Vector2Int(0, -nudgeCheck.y), ref nudgeCheck)) {
                    nudge = new Vector2Int(0, -nudgeCheck.y);
                    return true;
                }
            }
            else {
                if (TestNudgeLocation(pivot, gridSize, new Vector2Int(0, gridSize.y), ref nudgeCheck)) {
                    nudge = new Vector2Int(0, gridSize.y);
                    return true;
                }
                else if (TestNudgeLocation(pivot, gridSize, new Vector2Int(0, -gridSize.y), ref nudgeCheck)) {
                    nudge = new Vector2Int(0, -gridSize.y);
                    return true;
                }
            }
            if (TestNudgeLocation(pivot, gridSize, new Vector2Int(nudgeCheck.x, nudgeCheck.y))) {
                nudge = new Vector2Int(nudgeCheck.x, nudgeCheck.y);
                return true;
            }
            else if (TestNudgeLocation(pivot, gridSize, new Vector2Int(-nudgeCheck.x, -nudgeCheck.y))) {
                nudge = new Vector2Int(-nudgeCheck.x, -nudgeCheck.y);
                return true;
            }
            else return false;
        }
        return true;
    }
    private bool TestNudgeLocation(Vector2Int pivot, Vector2Int gridSize, Vector2Int nudgeCheck, ref Vector2Int nudge) {
        Vector2Int nudgeExtraStep = Vector2Int.zero;
        if (pivot.x + nudgeCheck.x >= 0 && pivot.y + nudgeCheck.y >= 0 && pivot.x + nudgeCheck.x < splitGraph.GridSize.x - gridSize.x && pivot.y + nudgeCheck.y < splitGraph.GridSize.y - gridSize.y && !splitGraph.GetCell(pivot.x + nudgeCheck.x, pivot.y + nudgeCheck.y)) { }
        else return false;        
        //nudge top check
        for (int i = 0; i < gridSize.x; i++) {
            if (splitGraph.GetCell(pivot.x + nudgeCheck.x + i, pivot.y + nudgeCheck.y)) {
                nudgeExtraStep.x = -gridSize.x + i;
                nudge = nudgeCheck + nudgeExtraStep;
                return false;
            }
        }
        //nudge left check
        for (int i = 0; i < gridSize.y; i++) {
            if (splitGraph.GetCell(pivot.x + nudgeCheck.x, pivot.y + nudgeCheck.y + i)) {
                nudgeExtraStep.y = -gridSize.y + i;
                nudge = nudgeCheck + nudgeExtraStep;
                return false;
            }
        }
        //nudge bottom check
        for (int i = 0; i < gridSize.x; i++) {
            if (splitGraph.GetCell(pivot.x + nudgeCheck.x + i, pivot.y + nudgeCheck.y + gridSize.y - 1))  return false;           
        }
        //nudge right check
        for (int i = 0; i < gridSize.y; i++) {
            if (splitGraph.GetCell(pivot.x + nudgeCheck.x + gridSize.x - 1, pivot.y + nudgeCheck.y + i)) return false;
        }
        return true;
    }
    private bool TestNudgeLocation(Vector2Int pivot, Vector2Int gridSize, Vector2Int nudgeCheck)
    {
        if (pivot.x + nudgeCheck.x >= 0 && pivot.y + nudgeCheck.y >= 0 && pivot.x + nudgeCheck.x < splitGraph.GridSize.x - gridSize.x && pivot.y + nudgeCheck.y < splitGraph.GridSize.y - gridSize.y && !splitGraph.GetCell(pivot.x + nudgeCheck.x, pivot.y + nudgeCheck.y)) { }
        else return false;
        //nudge top check
        for (int i = 0; i < gridSize.x; i++) {
            if (splitGraph.GetCell(pivot.x + nudgeCheck.x + i, pivot.y + nudgeCheck.y)) return false; 
        }
        //nudge left check
        for (int i = 0; i < gridSize.y; i++) {
            if (splitGraph.GetCell(pivot.x + nudgeCheck.x, pivot.y + nudgeCheck.y + i)) return false;
        }
        //nudge bottom check
        for (int i = 0; i < gridSize.x; i++) {
            if (splitGraph.GetCell(pivot.x + nudgeCheck.x + i, pivot.y + nudgeCheck.y + gridSize.y - 1)) return false;
        }
        //nudge right check
        for (int i = 0; i < gridSize.y; i++) {
            if (splitGraph.GetCell(pivot.x + nudgeCheck.x + gridSize.x - 1, pivot.y + nudgeCheck.y + i)) return false;
        }
        return true;
    }

    private void GenerateNavMesh() {
        NavMeshSurface surface = floorParent.GetComponent<NavMeshSurface>();
        if(surface.navMeshData != null) {
            surface.UpdateNavMesh(surface.navMeshData);
        }
        else {
            surface.BuildNavMesh();
        }
    }

    public List<Vector2Int> GetListedFloorTiles()
    {
        return floorTiles;
    }
}