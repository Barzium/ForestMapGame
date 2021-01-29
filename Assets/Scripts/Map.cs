using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


[Serializable] public class MapParams
{
    public uint chunksPerLine;
    public float chunkSize;
}
public static class Map
{
    public static uint chunksPerLine;
    public static float chunkSize;
    public static Chunk[,] map;

    public static void GenerateMap(MapParams mapParams)
    {
        chunksPerLine = mapParams.chunksPerLine;
        chunkSize = mapParams.chunkSize;
        map = new Chunk[chunksPerLine, chunksPerLine];

        for (int column = 0; column < chunksPerLine; column++)
        {
            for (int row = 0; row < chunksPerLine; row++)
            {
                Chunk chunk = new Chunk(ChunkType.EMPTY, Variant.BLUE, new Position(column, row));
                map[column, row] = chunk;
            }
        }
        foreach(ChunkType type in Enum.GetValues(typeof(ChunkType)))
        {
            if (type != ChunkType.EMPTY)
            {
                if (!PlaceChunk(type))
                {
                    Debug.LogError("NO LEGAL POSITIONS FOR CHUNK");
                }
            }
        }
    }

    public static void InstantiateMap()
    {
        for (int row = 0; row < chunksPerLine; row++)
        {
            for (int column = 0; column < chunksPerLine; column++)
            {
                Chunk chunk = map[row, column];
                GameObject chunk_prefab = chunk.chunkSO.ChunkPrefabs[0]; //placeholder
                Vector3 position = new Vector3(chunkSize * row, 0, chunkSize * column);
                var chunk_object = GameObject.Instantiate(chunk_prefab, position, Quaternion.identity);
                chunk_object.name = string.Format("Chunk {0},{1}", row, column);
            }
        }
    }

    private static bool PlaceChunk(ChunkType type)
    {
        System.Random rand = new System.Random();
        //generate positon
        Position pos = new Position();
        pos.x = rand.Next(0, (int)(chunksPerLine - 1));
        pos.y = rand.Next(0, (int)(chunksPerLine - 1));
        int infiniteLoopWatchdog = 0;
        while (!IsPositionLegal(pos))
        {
            pos.x = rand.Next(0, (int)(chunksPerLine - 1));
            pos.y = rand.Next(0, (int)(chunksPerLine - 1));
            infiniteLoopWatchdog++;
            if (infiniteLoopWatchdog > 10000)
            {
                return false;
            }
        }
        //generate variant
        Array variants = Enum.GetValues(typeof(Variant));
        Variant variant = (Variant)variants.GetValue(rand.Next(variants.Length));
        // create chunk
        Chunk chunk = new Chunk(type, variant, pos);
        map[pos.x, pos.y] = chunk;
        return true;
    }

    private static bool IsPositionLegal(Position pos)
    {
        List<Position> to_check = pos.GetSurroundingPositions();
        foreach (Position current_pos in to_check)
        {
            Chunk current_chunk = map[current_pos.x, current_pos.y];
            if (current_chunk.Type != ChunkType.EMPTY)
            {
                return false;
            }
        }
        return true;
    }
}

public class Chunk
{
    public ChunkSO chunkSO;
    public Variant variant;
    public Position position;

    public ChunkType Type { get => chunkSO.type; }

    public Chunk(ChunkType _type, Variant _variant, Position _position)
    {
        variant = _variant;
        position = _position;
        chunkSO = GameManager.GetChunkData(_type);
    }
}



public enum Variant
{
    BLUE,
    GREEN,
    RED,
    YELLOW
}

public class Position
{

    public int x;
    public int y;

    public Position(int _x, int _y)
    {
        x = _x;
        y = _y;
    }
    public Position()
    {
        x = 0;
        y = 0;
    }

    public List<Position> GetSurroundingPositions()
    {
        List<Position> result = new List<Position>();
        for (int column = x-1; column <= x+1; column++)
        {
            for (int row = y-1; row <= y+1; row++)
            {
                if (column >= 0 && column < Map.chunksPerLine && row >=0 && row < Map.chunksPerLine)
                {
                    result.Add(new Position(column, row));
                }
            }
        }
        return result;
    }

    //placeholder
    public Position GetNext()
    {
        return null;
    }
}