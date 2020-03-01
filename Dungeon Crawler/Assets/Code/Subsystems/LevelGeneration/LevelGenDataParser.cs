using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenDataParser
{

    public static List<GenerationAreaSettings> ParseGenerationDataJson(Resource textResource)
    {
        TextAsset textAsset = (TextAsset)textResource.loadedResources["level_gen_areas"];
        List<GenerationAreaSettings> generationAreaSettings = new List<GenerationAreaSettings>();

        GenerationAreaSettings currentArea = new GenerationAreaSettings();
        GenerationTurfSettings currentTurf = new GenerationTurfSettings();

        bool readingTiles = false;

        foreach(string line in textAsset.text.Split('\n'))
        {
            if (!readingTiles)
            {
                if (line.Contains(":"))
                {
                    string[] parts = line.Replace("\"", "").Split(':');
                    switch (parts[0])
                    {
                        case "name":
                            currentArea.area_name = parts[1];
                            continue;
                        case "probability":
                            int probability = 0;
                            int.TryParse(parts[1], out probability);
                            currentArea.weight = probability;
                            continue;
                        case "type":
                            currentArea.type = parts[2];
                            continue;
                        case "width":
                            int width = 0;
                            int.TryParse(parts[1], out width);
                            currentArea.width = width;
                            continue;
                        case "height":
                            int height = 0;
                            int.TryParse(parts[1], out height);
                            currentArea.height = height;
                            continue;
                        case "tiles":
                            readingTiles = true;
                            continue;
                    }
                }
                if (line.Contains("}"))
                {
                    //end of currentArea
                    generationAreaSettings.Add(currentArea);
                    currentArea = new GenerationAreaSettings();
                    continue;
                }
            }
            else
            {
                string[] parts = line.Replace("\"", "").Split(':');
                switch (parts[0])
                {
                    case "x":
                        int x = 0;
                        int.TryParse(parts[1], out x);
                        currentTurf.x = x;
                        continue;
                    case "y":
                        int y = 0;
                        int.TryParse(parts[1], out y);
                        currentTurf.y = y;
                        continue;
                    case "door_dir_x":
                        int door_dir_x = 0;
                        int.TryParse(parts[1], out door_dir_x);
                        currentTurf.door_dir_x = door_dir_x;
                        continue;
                    case "door_dir_y":
                        int door_dir_y = 0;
                        int.TryParse(parts[1], out door_dir_y);
                        currentTurf.door_dir_y = door_dir_y;
                        continue;
                    case "type":
                        currentTurf.type = parts[1];
                        continue;
                }
                if (line.Contains("}"))
                {
                    currentArea.generationTurfSettings.Add(currentTurf);
                    currentTurf = new GenerationTurfSettings();
                    continue;
                }
                if (line.Contains("]"))
                {
                    readingTiles = false;
                    continue;
                }
            }
        }

        return generationAreaSettings;
    }

}
