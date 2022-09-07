using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class SymbolPrefab : MonoBehaviour
{
    public SymbolPath symbolPath;
    public string symbolName;
    void Awake()
    {
        symbolPath = new SymbolPath(symbolName);
        LineRenderer lineRenderer = GetComponent<LineRenderer>();
        Vector3[] linePoints = new Vector3[lineRenderer.positionCount];
        symbolPath.points.AddRange(linePoints);

        lineRenderer.enabled = false;
    }

}
