using PDollarGestureRecognizer;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System.IO;

public class SymbolHandler : MonoBehaviour
{

    public float minimumPourcentage = 0.9f;

    [System.Serializable]
    public class SymbolEvent : UnityEngine.Events.UnityEvent<string> { }
    public SymbolEvent OnSymbol = new SymbolEvent();

    private TrailRenderer trail;


    public SymbolPath[] symbolPaths;

    // Start is called before the first frame update
    void Start()
    {
        trail = GetComponent<TrailRenderer>();
        trail.enabled = false;
        string[] gestureFiles = Directory.GetFiles(Application.persistentDataPath, "*.xml");
        foreach (string gestureFile in gestureFiles)
        {
            trainingSet.Add(GestureIO.ReadGestureFromFile(gestureFile));
        }
    }

    private SymbolPath playerPath;

    public Action<InputAction.CallbackContext> StartDraw2 { get; private set; }

    public void StartDraw()
    {
        Debug.Log("Start Draw");
        playerPath = new SymbolPath("PlayerPath");
        trail.enabled = true;
    }

    public bool creationMode = true;
    public string newGestureName;

    private List<Gesture> trainingSet = new List<Gesture>();

    public void EndDraw()
    {
       
        // Get the points from the trail renderer
        Vector3[] trailPoints = new Vector3[trail.positionCount];
        trail.GetPositions(trailPoints);

        // Add the points to the player path
        playerPath.points.AddRange(trailPoints);

        Point[] pointArray = new Point[playerPath.points.Count];

        for (int i = 0; i < playerPath.points.Count; i++)
        {
            Vector2 screenPoint = Camera.main.WorldToScreenPoint(playerPath.points[i]);
            pointArray[i] = new Point(screenPoint.x, screenPoint.y, 0);
        }

        Gesture newGesture = new Gesture(pointArray);

        if(creationMode)
        {
            newGesture.Name = newGestureName;
            trainingSet.Add(newGesture);

            string fileName = Application.persistentDataPath + "/" + newGestureName + ".xml";
            GestureIO.WriteGesture(pointArray, newGestureName, fileName);
        }
        else
        {
            Result result = PointCloudRecognizer.Classify(newGesture, trainingSet.ToArray());
            Debug.Log(result.GestureClass + " " + result.Score);
            if (result.Score > minimumPourcentage)
            {
                OnSymbol.Invoke(result.GestureClass);
            }
        }

        

       

        //Draw playerPath on LineRenderer
        //LineRenderer lineRenderer = GetComponent<LineRenderer>();
        //lineRenderer.positionCount = playerPath.points.Count;
        //lineRenderer.SetPositions(playerPath.points.ToArray());

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
