using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="ForestMap/Chunk")]
public class ChunkSO : ScriptableObject
{
    public ChunkType type;
    public bool isRiddle;
    public GameObject[] ChunkPrefabs; //array in case of multiple variants
}

/*
public class ChunkPack : ScriptableObject
{
    public ChunkSO grave;
    public ChunkSO cave;
    public ChunkSO shack;
    public ChunkSO pond;
    public ChunkSO water_tower;
    public ChunkSO glade;
    public ChunkSO church;
}*/

public enum ChunkType
{
    EMPTY,
    GRAVE,
    CAVE,
    SHACK,
    POND,
    WATER_TOWER,
    GLADE,
    CHURCH
}