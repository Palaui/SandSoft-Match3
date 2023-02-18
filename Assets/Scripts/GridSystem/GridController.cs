using System;
using UnityEngine;

public class GridController : MonoBehaviour
{
    private const int ELEMENTS_TYPES_AMOUNT = 6;
    private const int BASE_GRID_SIZE = 8;
    private const int MIN_MATCHES_AMOUNT = 3;


    private int[,] grid;

    [SerializeField] private GameHUDMenuView gameHUDMenuView;
    [SerializeField] private GridView gridView;
    [SerializeField] private int width = BASE_GRID_SIZE;
    [SerializeField] private int height = BASE_GRID_SIZE;


    private void Awake()
    {
        gameHUDMenuView.RegeneratePressed += OnRegeneratePressed;
        GenerateRandom();
    }

    private void OnDestroy()
    {
        gameHUDMenuView.RegeneratePressed -= OnRegeneratePressed;
    }

    private void GenerateRandom()
    {
        grid = new int[width, height];

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                Vector2Int coord = new Vector2Int(i, j);
                grid[i, j] = GetRandomElementType(GetNeighbour(coord, new Vector2Int(-1, 0)), GetNeighbour(coord, new Vector2Int(0, -1)));
            }
        }

        int matchesAmount = GetMatchesAmount();
        if (matchesAmount < MIN_MATCHES_AMOUNT)
            CreateNewMatch(MIN_MATCHES_AMOUNT - matchesAmount);

        gridView.Setup(grid);
    }

    private int GetNeighbour(Vector2Int coord, Vector2Int neighbourRelativePosition)
    {
        Vector2Int neighbourCoord = coord + neighbourRelativePosition;
        if (neighbourCoord.x < 0 || neighbourCoord.x >= width || neighbourCoord.y < 0 || neighbourCoord.y >= height)
            return -1;
        return grid[neighbourCoord.x, neighbourCoord.y];
    }

    private int GetRandomElementType(int typeNotAllowedA, int typeNotAllowedB)
    {
        int randomType = UnityEngine.Random.Range(0, ELEMENTS_TYPES_AMOUNT);
        while (randomType == typeNotAllowedA || randomType == typeNotAllowedB)
            randomType = UnityEngine.Random.Range(0, ELEMENTS_TYPES_AMOUNT);
        return randomType;
    }

    private int GetMatchesAmount()
    {
        int matches = 0;

        // Horizontals
        for (int i = 1; i < width - 1; i++)
        {
            for (int j = 0; j < height; j++)
            {
                Vector2Int coord = new Vector2Int(i, j);
                int leftNeighbour = GetNeighbour(coord, new Vector2Int(-1, 0));
                int rightNeighbour = GetNeighbour(coord, new Vector2Int(1, 0));
                if (leftNeighbour != rightNeighbour)
                    continue;

                int upNeighbour = GetNeighbour(coord, new Vector2Int(0, 1));
                int downNeighbour = GetNeighbour(coord, new Vector2Int(0, -1));

                if (upNeighbour == leftNeighbour || downNeighbour == leftNeighbour)
                {
                    matches++;
                }
            }
        }

        // Verticals
        for (int i = 0; i < width; i++)
        {
            for (int j = 1; j < height - 1; j++)
            {
                Vector2Int coord = new Vector2Int(i, j);
                int upNeighbour = GetNeighbour(coord, new Vector2Int(0, 1));
                int downNeighbour = GetNeighbour(coord, new Vector2Int(0, -1));
                if (upNeighbour != downNeighbour)
                    continue;

                int leftNeighbour = GetNeighbour(coord, new Vector2Int(-1, 0));
                int rightNeighbour = GetNeighbour(coord, new Vector2Int(1, 0));

                if (leftNeighbour == upNeighbour || rightNeighbour == upNeighbour)
                    matches++;
            }
        }

        return matches;
    }

    private void CreateNewMatch(int amount)
    {
        // Used for impossible boards
        int maxIterations = 10000;
        
        while (amount > 0 && maxIterations > 0)
        {
            for (int i = 0; i < amount; i++)
            {
                Vector2Int coord = new Vector2Int(UnityEngine.Random.Range(1, width - 1), UnityEngine.Random.Range(1, height - 1));
                int type = UnityEngine.Random.Range(0, ELEMENTS_TYPES_AMOUNT);

                if (HasNoNeighbourOfType(coord + new Vector2Int(-1, 0), type) && HasNoNeighbourOfType(coord + new Vector2Int(1, 0), type) && HasNoNeighbourOfType(coord + new Vector2Int(0, 1), type))
                {
                    grid[coord.x - 1, coord.y] = type;
                    grid[coord.x + 1, coord.y] = type;
                    grid[coord.x, coord.y + 1] = type;
                }
            }

            maxIterations--;
        }

        if (maxIterations == 0)
            Debug.LogError("Board could not generate matches");
    }

    private bool HasNoNeighbourOfType(Vector2Int coord, int type)
    {
        Vector2Int[] neighboursRelativePositions = new Vector2Int[]
        {
            new Vector2Int(-1, 0),
            new Vector2Int(1, 0),
            new Vector2Int(0, -1),
            new Vector2Int(0, 1)
        };

        foreach (Vector2Int neighbourRelativePosition in neighboursRelativePositions)
        {
            if (GetNeighbour(coord, neighbourRelativePosition) == type)
                return false;
        }

        return true;
    }

        private void OnRegeneratePressed(object sender, EventArgs args)
    {
        GenerateRandom();
    }
}