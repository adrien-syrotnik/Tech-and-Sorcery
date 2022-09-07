using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SymbolHandler : MonoBehaviour
{

    public float tolerance = 0.1f;
    private TrailRenderer trail;


    public SymbolPath[] symbolPaths;
    public GameObject[] symbolPrefabs;

    // Start is called before the first frame update
    void Start()
    {
        trail = GetComponent<TrailRenderer>();
        //Get All SymbolPath from SymbolPrefabs tab
        symbolPaths = new SymbolPath[symbolPrefabs.Length];
        for (int i = 0; i < symbolPrefabs.Length; i++)
        {
            symbolPaths[i] = symbolPrefabs[i].GetComponent<SymbolPrefab>().symbolPath;
        }

        trail.enabled = false;

    }

    // Update is called once per frame
    void Update()
    {
    }

    

    private SymbolPath playerPath;

    public void StartDraw()
    {
        playerPath = new SymbolPath("PlayerPath");
        trail.enabled = true;
    }

    public void EndDraw()
    {
       
        // Get the points from the trail renderer
        Vector3[] trailPoints = new Vector3[trail.positionCount];
        trail.GetPositions(trailPoints);

        // Add the points to the player path
        playerPath.points.AddRange(trailPoints);

        //Remove camera rotation from playerPath to be flat
        for (int i = 0; i < playerPath.points.Count; i++)
        {
            playerPath.points[i] = Quaternion.Inverse(Camera.main.transform.rotation) * playerPath.points[i];
        }



        // Get the min and max points
        playerPath.minPoint = playerPath.points[0];
        playerPath.maxPoint = playerPath.points[0];
        foreach (Vector3 point in playerPath.points)
        {
            if (point.x < playerPath.minPoint.x)
            {
                playerPath.minPoint.x = point.x;
            }
            if (point.y < playerPath.minPoint.y)
            {
                playerPath.minPoint.y = point.y;
            }
            if (point.z < playerPath.minPoint.z)
            {
                playerPath.minPoint.z = point.z;
            }
            if (point.x > playerPath.maxPoint.x)
            {
                playerPath.maxPoint.x = point.x;
            }
            if (point.y > playerPath.maxPoint.y)
            {
                playerPath.maxPoint.y = point.y;
            }
            if (point.z > playerPath.maxPoint.z)
            {
                playerPath.maxPoint.z = point.z;
            }
        }

        //Rescale playerPath to fit in a 1x1x1 cube
        Vector3 scale = playerPath.maxPoint - playerPath.minPoint;
        for (int i = 0; i < playerPath.points.Count; i++)
        {
            playerPath.points[i] = new Vector3(
                (playerPath.points[i].x - playerPath.minPoint.x) / scale.x,
                (playerPath.points[i].y - playerPath.minPoint.y) / scale.y,
                (playerPath.points[i].z - playerPath.minPoint.z) / scale.z
            );
        }

        // Check if the player path is within the tolerance of the symbol
        foreach (SymbolPath symbolPath in symbolPaths)
        {

            bool match = false;
            int symbolPathIndex = 0;
            Vector3 pointToFind = symbolPath.points[0];
            
            for (int i = 0; i < playerPath.points.Count; i++)
            {
                if (Vector3.Distance(playerPath.points[i], pointToFind) < tolerance)
                {
                    symbolPathIndex++;
                    if (symbolPathIndex >= symbolPath.points.Count)
                    {
                        match = true;
                        break;
                    }
                    pointToFind = symbolPath.points[symbolPathIndex];
                }
            }
            
            if (match)
            {
                Debug.Log("Matched symbol: " + symbolPath.name);
                break;
            }

        }

        //Draw playerPath on LineRenderer
        LineRenderer lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = playerPath.points.Count;
        lineRenderer.SetPositions(playerPath.points.ToArray());

        trail.Clear();
        trail.enabled = false;
    }
}

public class SymbolPath
{
    public string name;
    public List<Vector3> points;
    public Vector3 minPoint;
    public Vector3 maxPoint;

    public SymbolPath(string name)
    {
        this.name = name;
        points = new List<Vector3>();
    }
}
