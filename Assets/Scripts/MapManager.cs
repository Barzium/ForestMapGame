using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


[Serializable]public class Solution{
    public GameObject prefab;
    public Variant variant;
}
public class MapManager : MonoBehaviour
{
    public static MapManager instance = null;

    [SerializeField] ChunkSO[] chunkPack;
    [SerializeField] MapParams mapParams;
    [SerializeField] GameObject startChunkPrefab;
    //[SerializeField] List<(GameObject, Variant)> graveSolutions;
    [SerializeField] List<Solution> graveSolutions;
    [SerializeField] List<Solution> churchSolutions;
    [SerializeField] List<Solution> gladeSolutions;
    [SerializeField] List<GameObject> emptyChunkPrefabs;
    static Dictionary<ChunkType, ChunkSO> allChunks = new Dictionary<ChunkType, ChunkSO>();

    private static System.Random rand = new System.Random();
    private static Variant graveVar;
    private static Variant gladeVar;
    private static Variant churchVar;

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
        CalculateCode();
    }
    // Update is called once per frame
    void Update()
    {
        //Debug purposes
        /*if (Input.GetKeyDown(KeyCode.R))
        {
            Debug.Log("Refreshing");
            foreach (Transform child in transform)
            {
                GameObject.Destroy(child.gameObject);
            }
            Map.GenerateMap(mapParams);
            InstantiateMap();
        }*/
    }

    void InstantiateMap()
    {
        List<GameObject> shuffledEmptyPrefabs = ShuffleEmptyPrefabs();
        int emptyPrefabsIndex = 0;
        int solIndex = 0;
        for (int row = 0; row < Map.chunksPerLine; row++)
        {
            for (int column = 0; column < Map.chunksPerLine; column++)
            {
                GameObject chunk_prefab;
                Chunk chunk = Map.map[row, column];
                if (chunk.Type == ChunkType.EMPTY)
                {
                    if (row == mapParams.starty && column == mapParams.startx)
                    {
                        chunk_prefab = startChunkPrefab;
                    }
                    else
                    {
                        if (emptyPrefabsIndex >= shuffledEmptyPrefabs.Count)
                        {
                            Debug.LogErrorFormat("Illegal emptyPrefab index {0}", emptyPrefabsIndex);
                            emptyPrefabsIndex = 0;
                        }
                        chunk_prefab = emptyChunkPrefabs[emptyPrefabsIndex];
                        emptyPrefabsIndex++;
                    }
                }
                else
                {
                    if (chunk.Type == ChunkType.CHURCH)
                    {
                        solIndex = rand.Next(churchSolutions.Count);
                        chunk_prefab = churchSolutions[solIndex].prefab;
                        churchVar = churchSolutions[solIndex].variant;
                    }
                    else
                    {
                        if (chunk.Type == ChunkType.GRAVE)
                        {
                            solIndex = rand.Next(graveSolutions.Count);
                            chunk_prefab = graveSolutions[solIndex].prefab;
                            churchVar = graveSolutions[solIndex].variant;
                        }
                        else
                        {
                            if (chunk.Type == ChunkType.GLADE)
                            {
                                solIndex = rand.Next(gladeSolutions.Count);
                                chunk_prefab = gladeSolutions[solIndex].prefab;
                                churchVar = gladeSolutions[solIndex].variant;
                            }
                            else
                            {
                                chunk_prefab = chunk.chunkSO.ChunkPrefabs[0]; //not riddle
                            }
                        }
                    }
                }
                Vector3 position = new Vector3(Map.chunkSize * row, 0, Map.chunkSize * column);
                var chunk_object = GameObject.Instantiate(chunk_prefab, position, Quaternion.identity, this.transform);
                chunk_object.name = string.Format("Chunk {0},{1}", row, column);
            }
        }
    }

    void CalculateCode()
    {
        int[] code = new int[3];
        code[0] = VariantToInt(gladeVar);
        code[1] = VariantToInt(churchVar);
        code[2] = VariantToInt(graveVar);
        // DELIVER CODE TO CODEPANEL HERE

    }

    int VariantToInt(Variant variant)
    {
        switch (variant)
        {
            case Variant.BLUE:
                return 1;
            case Variant.GREEN:
                return 2;
            case Variant.RED:
                return 3;
            case Variant.YELLOW:
                return 4;
        }
        return 0; //default. Shouldn't happen
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
