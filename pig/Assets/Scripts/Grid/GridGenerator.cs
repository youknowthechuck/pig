using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridGenerator : PigScript
{
    public GameObject gridCellPrefab = null;
    public int gridWidth = 1;
    public int gridHeight = 1;
    public float cellSize = 1;

    // Start is called before the first frame update
    void Start()
    {
        List<Vector3> centers = GetGridCenters();

        foreach(Vector3 pos in centers)
        {
            Instantiate(gridCellPrefab, pos, Quaternion.identity, transform);
        }
    }

    public List<Vector3> GetGridCenters()
    {
        List<Vector3> positions = new List<Vector3>();

        float halfWidth = ((float)gridWidth * cellSize) / 2.0f;
        float halfHeight = ((float)gridHeight * cellSize) / 2.0f;
        float halfSize = cellSize / 2.0f;

        Vector3 bottomLeft = new Vector3(-halfWidth + halfSize, 0, -halfHeight + halfSize);

        for (float x = 0; x < gridWidth; x += 1)
        {
            for (float y = 0; y < gridHeight; y += 1)
            {
                Vector3 pos = bottomLeft + new Vector3(x * cellSize, 0, y * cellSize);
                positions.Add(pos);
            }
        }

        return positions;
    }
}
