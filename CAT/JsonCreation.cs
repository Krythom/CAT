using System;
using System.IO;
using System.Text.Json;

namespace CAT;

public static class JsonCreation
{
    public static void CreateJson(Cell[,] world, int xMax, int yMax, int iterations)
    {
        CellInfo[] cells = new CellInfo[xMax * yMax];
        
        for (int x = 0; x < xMax; x++)
        {
            for (int y = 0; y < yMax; y++)
            {
                cells[x * xMax + y] = new CellInfo
                {
                    lastUpdateFrame = world[x, y].LastUpdate,
                    numUpdates = world[x, y].Updates
                };
            }
        }

        WorldInfo info = new WorldInfo
        {
            totalIterations = iterations,
            worldX = xMax,
            worldY = yMax,
            pixels = cells
        };

        string json = JsonSerializer.Serialize(info);
        string date = DateTime.Now.ToString("s").Replace("T", " ").Replace(":", "-");
        string filePath = $"{date}_i{iterations}.json";
        File.WriteAllText(filePath, json);
    }
}

[Serializable]
public class WorldInfo
{
    public required int totalIterations { get; set; }
    public required int worldX { get; set; }
    public required int worldY { get; set; }
    public required CellInfo[] pixels { get; set; }
}

[Serializable]
public class CellInfo
{
    public required int lastUpdateFrame { get; set; }
    public required int numUpdates { get; set; }
}