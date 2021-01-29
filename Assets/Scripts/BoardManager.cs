using UnityEngine;

public class BoardManager : MonoBehaviour
{
    [SerializeField] int numOfTiles;
    [SerializeField] GameObject tilePrefab;
    [SerializeField] private float zDistance;
    // Start is called before the first frame update
    void Start() {
        GenerateTiles();
    }

    private void GenerateTiles() {
        for (int i = 0; i < numOfTiles; i++) {
            var tile = Instantiate(tilePrefab, transform);
            tile.transform.localPosition = new Vector3(UnityEngine.Random.Range(4, 8), UnityEngine.Random.Range(-3.5f, 3.5f), 0);
        }
    }
}
