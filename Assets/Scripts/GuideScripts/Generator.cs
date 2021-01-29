using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generator : MonoBehaviour
{
    [SerializeField] TileData[] tiles;
    Collider2D spawnArea;
    void Start() {
        spawnArea = GetComponent<Collider2D>();
        GenerateTiles();
    }

    private void GenerateTiles() {
        foreach (TileData tileType in tiles)
            for (int i = 0; i < tileType.amount; i++) {
                var tile = Instantiate(tileType.prefab, transform.parent);
                tile.transform.position = new Vector3(Random.Range(spawnArea.bounds.min.x + 0.5f, spawnArea.bounds.max.x - 0.5f),
                    Random.Range(spawnArea.bounds.min.y + 0.5f, spawnArea.bounds.max.y - 0.5f),
                    transform.position.z);
            }
    }
    [System.Serializable]
    class TileData
    {
        public GameObject prefab;
        public int amount;
    }
}
