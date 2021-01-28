using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public static class Map
{
    public static uint mapSize = 8;
    public static Chunk[,] map = new Chunk[mapSize,mapSize];

    public static void GenerateMap()
    {
        for (int column = 0; column < mapSize; column++)
        {
            for (int row = 0; row < mapSize; row++)
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

    private static bool PlaceChunk(ChunkType type)
    {
        System.Random rand = new System.Random();
        //generate positon
        Position pos = new Position();
        pos.x = rand.Next(0, (int)(mapSize - 1));
        pos.y = rand.Next(0, (int)(mapSize - 1));
        int infiniteLoopWatchdog = 0;
        while (!IsPositionLegal(pos))
        {
            pos.x = rand.Next(0, (int)(mapSize - 1));
            pos.y = rand.Next(0, (int)(mapSize - 1));
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
            if (current_chunk.type != ChunkType.EMPTY)
            {
                return false;
            }
        }
        return true;
    }
}

public class Chunk
{
    //public ChunkSO chunkSO;
    public ChunkType type;
    public Variant variant;
    public Position position;

    public Chunk(ChunkType _type, Variant _variant, Position _position)
    {
        type = _type;
        variant = _variant;
        position = _position;
    }
}

public class ChunkSO: ScriptableObject
{
    public ChunkType type;
    public bool isRiddle;
    public GameObject ChunkPrefab;
}

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
                if (column >= 0 && column < Map.mapSize && row >=0 && row < Map.mapSize)
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