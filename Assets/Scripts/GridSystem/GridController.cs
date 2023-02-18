using UnityEngine;

public class GridController: MonoBehaviour
{
    private const int ELEMENTS_TYPES_AMOUNT = 6;
    
    private int[,] gridSize;

    [SerializeField] private int width = 8;
    [SerializeField] private int height = 8;
    

    private void Awake()
    {
        gridSize = new int[width, height];
    }
    
    private void GenerateRandom()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                Vector2Int coord = new Vector2Int(i, j);
                gridSize[i, j] = GetRandomElementType(GetNeighbour(coord, new Vector2Int(-1, 0)), GetNeighbour(coord, new Vector2Int(0, -1)));
            }
        }
    }

    private int GetNeighbour(Vector2Int coord, Vector2Int neighbourRelativePosition)
    {
        Vector2Int neighbourCoord = coord + neighbourRelativePosition;
        if (neighbourCoord.x < 0 || neighbourCoord.x >= width || neighbourCoord.y < 0 || neighbourCoord.y >= height)
            return -1;
        return gridSize[neighbourCoord.x, neighbourCoord.y];
    }

    private int GetRandomElementType(int typeNotAllowedA, int typeNotAllowedB)
    {
        int randomType = Random.Range(0, ELEMENTS_TYPES_AMOUNT);
        while (randomType == typeNotAllowedA || randomType == typeNotAllowedB)
            randomType = Random.Range(0, ELEMENTS_TYPES_AMOUNT);
        return randomType;
    }
}