using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawPath : MonoBehaviour
{
    LineRenderer lineRenderer;
    // Start is called before the first frame update
    void Start()
    {
        lineRenderer=this.GetComponent<LineRenderer>();
    }

    public void Draw(List<GridCell> paths)
    {
        if (lineRenderer != null && paths != null && paths.Count > 1)
        {
            lineRenderer.positionCount = 0;
            lineRenderer.positionCount = paths.Count;
            for (int i = 0; i < paths.Count; i++)
            {
                Vector3 worldPosition = new Vector3(paths[i].transform.position.x, paths[i].transform.position.y, 0);
                lineRenderer.SetPosition(i, worldPosition);
            }
        }
    }
    public void ResetLine()
    {
        lineRenderer.positionCount = 0;
    }
}
