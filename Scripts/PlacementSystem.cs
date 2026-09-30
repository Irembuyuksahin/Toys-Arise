using System.Collections.Generic;
using UnityEngine;

public class PlacementSystem : MonoBehaviour
{
    [SerializeField] private GameObject mouseIndicator;
    [SerializeField] private GameObject cellIndicator;
    [SerializeField] private GameObject player;
    [SerializeField] private InputManager inputManager;
    [SerializeField] private Grid grid;
    [SerializeField] private LegoDataBaseSO database;
    [SerializeField] private GameObject gridVisualization;
    [SerializeField] private VoidEvent onGridPlace;
    [SerializeField] private VoidEvent onDetermine;
    [SerializeField] private VoidEvent onLegoDestroy;
    [SerializeField] private ShopManager shopManager;
    [SerializeField] private AudioManager audioManager;

    private int selectedObjectIndex = -1;
    private GridData legoData;
    private Renderer previewRenderer;
    private List<GameObject> placedGameObject = new();

    public Vector3Int gridPosition;
    private bool placementValidity;

    private void OnEnable()
    {
        onGridPlace.AddListerner(PlaceStructure);
        onDetermine.AddListerner(DetermineGridPosition);
    }

    private void OnDisable()
    {
        onGridPlace.RemoveListerner(StartPlacement);
        onDetermine.RemoveListerner(DetermineGridPosition);
    }

    private void Start()
    {
        StopPlacement();
        legoData = new();
        previewRenderer = cellIndicator.GetComponentInChildren<Renderer>();
    }

    public void StartPlacement()
    {
        StopPlacement();
        selectedObjectIndex = 0;

        if (selectedObjectIndex < 0)
        {
            Debug.LogError("No ID found");
            return;
        }

        gridVisualization.SetActive(true);
        cellIndicator.SetActive(true);
        mouseIndicator.SetActive(true);
    }

    public void DetermineGridPosition()
    {
        Vector3 mousePosition = inputManager.GetSelectedMapPosition();
        gridPosition = new Vector3Int(grid.WorldToCell(mousePosition).x, 0, grid.WorldToCell(mousePosition).z);
        placementValidity = CheckPlacementValidity(gridPosition, selectedObjectIndex);
    }

    public void PlaceStructure()
    {
        if (!placementValidity)
            return;

        GameObject newObject = Instantiate(database.legoData[selectedObjectIndex].Prefab);
        newObject.transform.position = grid.CellToWorld(gridPosition);
        placedGameObject.Add(newObject);

        GridData selectedData = legoData;
        selectedData.AddObjectAt(gridPosition, database.legoData[selectedObjectIndex].Size, database.legoData[selectedObjectIndex].ID, placedGameObject.Count - 1);

        audioManager.PlaySFX(audioManager.legoYerlestirme);

        if (shopManager.shopItems[3, 1] > 0)
        {
            shopManager.shopItems[3, 1] -= 1;
        }
    }

    private bool CheckPlacementValidity(Vector3Int gridPosition, int selectedObjectIndex)
    {
        if (CheckNeighbourCells())
        {
            GridData selectedData = legoData;
            return selectedData.CanPlaceObjectAt(gridPosition, database.legoData[selectedObjectIndex].Size);
        }

        return false;
    }

    public void StopPlacement()
    {
        selectedObjectIndex = -1;
        gridVisualization.SetActive(false);
        cellIndicator.SetActive(false);
        mouseIndicator.SetActive(false);
    }

    public bool CheckNeighbourCells()
    {
        Vector3Int standCell = new Vector3Int(grid.WorldToCell(player.transform.position).x, 0, grid.WorldToCell(player.transform.position).z);
        Vector3 mousePosition = inputManager.GetSelectedMapPosition();
        Vector3Int lookCell = new Vector3Int(grid.WorldToCell(mousePosition).x, 0, grid.WorldToCell(mousePosition).z);

        Vector3Int[] neighbourCells =
        {
            new Vector3Int(standCell.x + 1, standCell.y, standCell.z),
            new Vector3Int(standCell.x + 1, standCell.y, standCell.z + 1),
            new Vector3Int(standCell.x, standCell.y, standCell.z + 1),
            new Vector3Int(standCell.x - 1, standCell.y, standCell.z),
            new Vector3Int(standCell.x - 1, standCell.y, standCell.z - 1),
            new Vector3Int(standCell.x, standCell.y, standCell.z - 1),
            new Vector3Int(standCell.x + 1, standCell.y, standCell.z - 1),
            new Vector3Int(standCell.x - 1, standCell.y, standCell.z + 1),
            new Vector3Int(standCell.x + 2, standCell.y, standCell.z),
            new Vector3Int(standCell.x + 2, standCell.y, standCell.z + 2),
            new Vector3Int(standCell.x + 1, standCell.y, standCell.z + 2),
            new Vector3Int(standCell.x + 2, standCell.y, standCell.z + 1),
            new Vector3Int(standCell.x, standCell.y, standCell.z + 2),
            new Vector3Int(standCell.x - 2, standCell.y, standCell.z),
            new Vector3Int(standCell.x - 2, standCell.y, standCell.z - 2),
            new Vector3Int(standCell.x - 1, standCell.y, standCell.z - 2),
            new Vector3Int(standCell.x - 2, standCell.y, standCell.z - 1),
            new Vector3Int(standCell.x, standCell.y, standCell.z - 2),
            new Vector3Int(standCell.x + 2, standCell.y, standCell.z - 2),
            new Vector3Int(standCell.x + 1, standCell.y, standCell.z - 2),
            new Vector3Int(standCell.x + 2, standCell.y, standCell.z - 1),
            new Vector3Int(standCell.x - 2, standCell.y, standCell.z + 2),
            new Vector3Int(standCell.x - 1, standCell.y, standCell.z + 2),
            new Vector3Int(standCell.x - 2, standCell.y, standCell.z + 1)
        };

        foreach (Vector3Int cell in neighbourCells)
        {
            if (cell.Equals(lookCell))
            {
                return true;
            }
        }

        return false;
    }

    private void Update()
    {
        if (selectedObjectIndex < 0)
        {
            return;
        }

        Vector3 mousePosition = inputManager.GetSelectedMapPosition();
        Vector3Int gridPosition = new Vector3Int(grid.WorldToCell(mousePosition).x, 0, grid.WorldToCell(mousePosition).z);
        bool placementValidity = CheckPlacementValidity(gridPosition, selectedObjectIndex);

        previewRenderer.material.color = placementValidity ? Color.white : Color.red;

        mouseIndicator.transform.position = mousePosition;
        cellIndicator.transform.position = grid.CellToWorld(gridPosition);
    }
}