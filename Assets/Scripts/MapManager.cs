using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    public static MapManager instance = null;

    [SerializeField] ChunkSO[] chunkPack;
    [SerializeField] MapParams mapParams;
    [SerializeField] List<GameObject> emptyChunkPrefabs;
    static Dictionary<ChunkType, ChunkSO> allChunks = new Dictionary<ChunkType, ChunkSO>();

    private static System.Random rand = new System.Random();

    public static ChunkSO GetChunkData(ChunkType type)
    {
        return allChunks[type];
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            foreach (ChunkSO chunk_data in chunkPack)
            {
                allChunks.Add(chunk_data.type, chunk_data);
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        Map.GenerateMap(mapParams);
        //Map.InstantiateMap();
        InstantiateMap();
    }
    // Update is called once per frame
    void Update()
    {
        //Debug purposes
        if (Input.GetKeyDown(KeyCode.R))
        {
            Debug.Log("Refreshing");
            foreach (Transform child in transform)
            {
                GameObject.Destroy(child.gameObject);
            }
            Map.GenerateMap(mapParams);
            InstantiateMap();
        }
    }

    void InstantiateMap()
    {
        List<GameObject> shuffledEmptyPrefabs = ShuffleEmptyPrefabs();
        int emptyPrefabsIndex = 0;
        for (int row = 0; row < Map.chunksPerLine; row++)
        {
            for (int column = 0; column < Map.chunksPerLine; column++)
            {
                GameObject chunk_prefab;
                Chunk chunk = Map.map[row, column];
                if (chunk.Type == ChunkType.EMPTY)
                {
                    if (emptyPrefabsIndex >= shuffledEmptyPrefabs.Count)
                    {
                        Debug.LogErrorFormat("Illegal emptyPrefab index {0}", emptyPrefabsIndex);
                        emptyPrefabsIndex = 0;
                    }
                    chunk_prefab = emptyChunkPrefabs[emptyPrefabsIndex];
                    emptyPrefabsIndex++;
                }
                else
                {
                    chunk_prefab = chunk.chunkSO.ChunkPrefabs[0]; //placeholder
                }
                Vector3 position = new Vector3(Map.chunkSize * row, 0, Map.chunkSize * column);
                var chunk_object = GameObject.Instantiate(chunk_prefab, position, Quaternion.identity, this.transform);
                chunk_object.name = string.Format("Chunk {0},{1}", row, column);
            }
        }
    }

    List<GameObject> ShuffleEmptyPrefabs()
    {
        var list = new List<GameObject>(emptyChunkPrefabs);
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = rand.Next(n + 1);
            var temp = list[k];
            list[k] = list[n];
            list[n] = temp;
        }
        return list;
    }

}
