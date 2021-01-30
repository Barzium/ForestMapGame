using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


[Serializable] public class MapParams
{
    public uint chunksPerLine;
    public float chunkSize;
    public int startx;
    public int starty;
}
public static class Map
{
    public static uint chunksPerLine;
    public static float chunkSize;
    public static Position startPosition;
    public static Chunk[,] map;
    private static System.Random rand = new System.Random();
    private static List<Position> allPositions = new List<Position>();

    private static int mapGenWatchdog = 0;
    private static (Position,ChunkType)[] defaultMap =
    {
        (new Position(7,5), ChunkType.WATER_TOWER), //watertower
        (new Position(2,5),ChunkType.GRAVE), //grave
        (new Position(5,1),ChunkType.CAVE), //cave
        (new Position(5,7),ChunkType.SHACK), //shack
        (new Position(0,2),ChunkType.POND), //well/pond
        (new Position(5,4),ChunkType.GLADE), //glade/clearing
        (new Position(2,1),ChunkType.CHURCH) //church
    };

    public static void GenerateMap(MapParams mapParams)
    {
        chunksPerLine = mapParams.chunksPerLine;
        chunkSize = mapParams.chunkSize;
        startPosition = new Position(mapParams.startx, mapParams.starty);
        map = new Chunk[chunksPerLine, chunksPerLine];

        for (int column = 0; column < chunksPerLine; column++)
        {
            for (int row = 0; row < chunksPerLine; row++)
            {
                // initialize spot to empty
                Chunk chunk = new Chunk(ChunkType.EMPTY, Variant.BLUE, new Position(column, row));
                map[row, column] = chunk;
                // list all chunk positions in shuffleList
                allPositions.Add(new Position(column, row));
            }
        }
        mapGenWatchdog = 0;
        ChunkType[] chunkTypes = (ChunkType[])Enum.GetValues(typeof(ChunkType));
        int index = 0;
        if(!PlaceChunk(chunkTypes, index))
        {
            Debug.LogError("No legal placements AAAAAAAAAH");
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
                //var chunk_object = GameObject.Instantiate(chunk_prefab, position, Quaternion.identity);
                var chunk_object = GameObject.Instantiate(chunk_prefab, position, Quaternion.identity, MapManager.instance.transform); //DEBUG
                chunk_object.name = string.Format("Chunk {0},{1}", row, column);
            }
        }
    }

    private static void GenDefaultMap()
    {
        for (int column = 0; column < chunksPerLine; column++)
        {
            for (int row = 0; row < chunksPerLine; row++)
            {
                // initialize spot to empty
                Chunk chunk = new Chunk(ChunkType.EMPTY, Variant.BLUE, new Position(column, row));
                map[row, column] = chunk;
            }
        }
        foreach ((Position,ChunkType) specialChunk in defaultMap)
        {
            Position pos = specialChunk.Item1;
            ChunkType type = specialChunk.Item2;
            map[pos.y, pos.x] = new Chunk(type, Variant.BLUE, pos);
        }
    }

    private static bool PlaceChunk(ChunkType[] chunkTypes, int index)
    {
        mapGenWatchdog++;
        if (mapGenWatchdog > 1000)
        {
            //generation failed. Give up and use default
            Debug.LogWarning("Generation failed. Defaulting");
            GenDefaultMap();
            return true;
        }
        if (index >= chunkTypes.Length)
        {
            return true; //we're done!
        }
        ChunkType type = chunkTypes[index];
        if (type == ChunkType.EMPTY)
        {
            //nothing to do. Move on
            return PlaceChunk(chunkTypes, index + 1);
        }
        List<Position> shuffledPositions = GetShuffledPositions();
        foreach (Position curr_pos in shuffledPositions)
        {
            if (IsPositionLegal(curr_pos, type)) //check surroundings
            {
                //try placing
                map[curr_pos.y, curr_pos.x] = new Chunk(type, Variant.BLUE, curr_pos);
                if (CheckMapRules()) //check consistency of map rules
                {
                    if (PlaceChunk(chunkTypes, index + 1)) //try placing next chunks. If successful this is successful!
                    {
                        return true;
                    }
                }
                // chunk placement failed. remove
                map[curr_pos.y, curr_pos.x] = new Chunk(ChunkType.EMPTY, Variant.BLUE, curr_pos);
            }
            // otherwise keep iterating
        }
        return false; //no legal position found
    }

    private static List<Position> GetShuffledPositions()
    {
        var list = new List<Position>(allPositions);
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

    private static bool IsPositionLegal(Position checked_pos, ChunkType type)
    {
        //check this isn't the start position
        if (checked_pos.IsEqual(startPosition))
        {
            return false;
        }
        //check surroundings aren't occupied
        List<Position> surroundings = checked_pos.GetSurroundingPositions();
        foreach (Position current_pos in surroundings)
        {
            Chunk current_chunk = map[current_pos.y, current_pos.x];
            if (current_chunk.Type != ChunkType.EMPTY)
            {
                return false;
            }
        }
        return true;
    }

    private static bool CheckMapRules()
    {
        // rule 1
        var water_tower_pos = FindChunk(ChunkType.WATER_TOWER);
        var church_pos = FindChunk(ChunkType.CHURCH);
        if (water_tower_pos != null && church_pos != null)
        {
            if (water_tower_pos.IsCorner() && !church_pos.IsCorner())
                return false;
        }
        // rule 2
        var glade_pos = FindChunk(ChunkType.GLADE);
        if (water_tower_pos != null && glade_pos != null)
        {
            if (!water_tower_pos.IsCorner() && !glade_pos.IsAdjacentRow(water_tower_pos))
                return false;
        }
        // rule 3
        var pond_pos = FindChunk(ChunkType.POND);
        var shack_pos = FindChunk(ChunkType.SHACK);
        if (pond_pos != null && shack_pos != null)
        {
            if (!pond_pos.IsSameDiagonal(shack_pos))
                return false;
        }
        // rule 4
        var cave_pos = FindChunk(ChunkType.CAVE);
        if (cave_pos != null && glade_pos != null)
        {
            if (!cave_pos.IsSameColumn(glade_pos) && !cave_pos.IsSameRow(glade_pos))
                return false;
        }
        // rule 5
        var graveyard_pos = FindChunk(ChunkType.GRAVE);
        if (graveyard_pos != null && church_pos != null)
        {
            if (!graveyard_pos.IsSameColumn(church_pos) && !graveyard_pos.IsSameRow(church_pos))
                return false;
        }
        // rule 6
        if (cave_pos != null && shack_pos != null)
        {
            if (cave_pos.IsNorth() || cave_pos.IsSouth())
                if (!cave_pos.IsSameColumn(shack_pos))
                    return false;
        }
        // rule 7
        if (pond_pos != null && graveyard_pos != null)
        {
            if (!(pond_pos.IsEast() || pond_pos.IsWest()))
                if (!pond_pos.IsSameRow(graveyard_pos))
                    return false;
        }
        return true;
    }

    public static Position FindChunk(ChunkType type)
    {
        foreach(Position curr_pos in allPositions)
        {
            if (map[curr_pos.y,curr_pos.x].Type == type)
            {
                //found!
                return curr_pos;
            }
        }
        return null; //chunk not in map
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
        chunkSO = MapManager.GetChunkData(_type);
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

    public bool IsEqual(Position other)
    {
        return (x == other.x && y == other.y);
    }
    public bool IsCorner()
    {
        int end = (int)(Map.chunksPerLine - 1);
        return ((x == 0 || x == end) && (y == 0 || y == end));
    }

    public bool IsSameRow(Position other)
    {
        return other.y == y;
    }

    public bool IsAdjacentRow(Position other)
    {
        int ydiff = other.y - y;
        return (ydiff == 1 || ydiff == -1);
    }

    public bool IsSameColumn(Position other)
    {
        return other.x == x;
    }

    public bool IsAdjacentColumn(Position other)
    {
        int xdiff = other.x - x;
        return (xdiff == 1 || xdiff == -1);
    }

    public bool IsSameDiagonal(Position other)
    {
        int xdiff = x - other.x;
        int ydiff = y - other.y;
        return (xdiff == ydiff || xdiff == -ydiff); //both diagonals checked
    }

    public bool IsNorth()
    {
        return (y == 0);
    }
    public bool IsSouth()
    {
        int end = (int)(Map.chunksPerLine - 1);
        return (y == end);
    }
    public bool IsWest()
    {
        return (x == 0);
    }
    public bool IsEast()
    {
        int end = (int)(Map.chunksPerLine - 1);
        return (x == end);
    }

    //placeholder
    public Position GetNext()
    {
        return null;
    }
}