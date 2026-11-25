using UnityEngine;
using System.Collections.Generic;
public class GridManager : MonoBehaviour
{
    public int width = 14;
    public int height = 9;
    public Vector2 cellSize = new Vector2(1.28f, 1.1f);
    public Vector2 gridOrigin = new Vector2(-8.35f, -4.39f);

    [Header("Arena")]
    public SpriteRenderer arenaSprite;
    public bool blockBorders = true;


    [Header("Obstaculos")]
    public List<Vector2Int> obstacles = new List<Vector2Int>
    {
        new Vector2Int(5,3),
        new Vector2Int(10,5),
        new Vector2Int (7,6)
        
    };


    private bool[,] blocked;

    void Awake()
    {
        InitializeGrid();
    }
    void OnValidate()// para atualizar os gizmos 
    {
        InitializeGrid();
    }

    [ContextMenu("ajustar a grid do sprite na arena")]
    void FitToArena()
    {
        if (arenaSprite == null){
            Debug.LogWarning("COLOCA O SPRITE DA ARENA AI CARA");
            return;
        }

        Bounds b = arenaSprite.bounds;
        cellSize = new Vector2(b.size.x/ width, b.size.y/height);
        gridOrigin = new Vector2(b.min.x + cellSize.x * 0.5f, b.min.y + cellSize.y * 0.5f);
        InitializeGrid();

        #if UNITY_EDITOR
        UnityEditor.EditorUtility.SetDirty(this);  // garante que a cena salve os novos valores
        #endif
    }

    void InitializeGrid()
    {
        blocked = new bool[width, height];

        // 1. Bordas marrons bloqueadas
        for (int x = 0; x < width; x++)
        {
            blocked[x, 0] = true;          
            blocked[x, height - 1] = true; 
        }

        for (int y = 0; y < height; y++)
        {
            blocked[0, y] = true;         
            blocked[width - 1, y] = true; 
        }

        //(X = coluna, Y = linha)

        blocked[6, 1] = true;
        blocked[11, 1] = true; 
        blocked[4, 2] = true;
        blocked[6, 3] = true;  
        blocked[8, 3] = true;
        blocked[2, 4] = true;  
        blocked[11, 5] = true;
        blocked[6, 6] = true; 
        blocked[9, 7] = true;          
        blocked[11, 2] = true;
    }

    public bool IsWalkable(Vector2Int position)
    {
        if (blocked == null)
        {
            InitializeGrid();
        }

        if (position.x < 0 || position.x >= width || position.y < 0 || position.y >= height)
        {
            return false;
        }

        return !blocked[position.x, position.y];
    }

    public Vector2Int WorldToGrid(Vector3 worldPos)
    {
        return new Vector2Int(
            Mathf.RoundToInt((worldPos.x - gridOrigin.x) / cellSize.x),
            Mathf.RoundToInt((worldPos.y - gridOrigin.y) / cellSize.y)
        );
    }

    public Vector3 GridToWorld(Vector2Int gridPos)
    {
        return new Vector3(
            (gridPos.x * cellSize.x) + gridOrigin.x,
            (gridPos.y * cellSize.y) + gridOrigin.y,
            0f
        );
    }

    public int GridToState(Vector2Int position)
    {
        return position.y * width + position.x;
    }

    public Vector2Int StateToGrid(int state)
    {
        return new Vector2Int(state % width, state / width);
    }

   
}