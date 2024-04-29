using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateGrid : MonoBehaviour
{
    public Grid grid;

    public Vector2Int startPoint;
    public Vector2Int endPoint;

    public Vector2Int[] blockedTiles;
    public Vector2Int[] pathTiles;
    public GameObject AICharacter;
    public Sprite sprite;

    public bool update;

    private AI_Movement aiMovement;
    // Start is called before the first frame update
    void Start()
    {
        Vector3 startPos = new Vector3(startPoint.x + 0.5f, 1, startPoint.y + 0.5f);
        Vector3 endPos = new Vector3(endPoint.x + 0.5f, 1f, endPoint.y + 0.5f);

        var character = Instantiate(AICharacter, startPos, Quaternion.identity);
        grid = new Grid(100,100);
        
        for (int i = 0; i < 100; i++)
        {
            for (int j = 0; j < 100; j++)
            {
                grid.gridArray[i, j].isWalkable = true;
                grid.gridArray[i,j].SetIsGrass();
            }
        }

        foreach (var pos in blockedTiles)
        {
            grid.gridArray[pos.x, pos.y].isWalkable = false;
        }

        foreach (var pos in pathTiles)
        {
            grid.gridArray[pos.x, pos.y].SetIsPath();
        }

        grid.sprite = this.sprite;
        
        grid.RenderTiles();
        aiMovement = character.GetComponent<AI_Movement>();
        aiMovement.Visualise = true;
        aiMovement.usePaths = false;
        aiMovement.grid = this.grid;
        aiMovement.test = true;
        aiMovement.VisulisationSpeed = AI_Movement.VisualisationSpeed.Fast;
        var tile = grid.TileFromWorldPoint(endPos);
        var visualiser = tile.renderer;
        visualiser.color = Color.magenta;
    }

    // Update is called once per frame
    void Update()
    {
        if (update && aiMovement != null)
        {
            Vector3 startPos = new Vector3(startPoint.x + 0.5f, 1, startPoint.y + 0.5f);
            Vector3 endPos = new Vector3(endPoint.x + 0.5f, 1f, endPoint.y + 0.5f);
            aiMovement.StartPathing(startPos, endPos);
            update = false;
        }
    }
}
