using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;

    [SerializeField] ChunkSO[] chunkPack;
    [SerializeField] MapParams mapParams;
    static Dictionary<ChunkType, ChunkSO> allChunks = new Dictionary<ChunkType, ChunkSO>();

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
        Map.InstantiateMap();
    }
    // Update is called once per frame
    void Update()
    {
        int a = 1;
    }

}
