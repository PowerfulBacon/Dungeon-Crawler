using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerationAreaSettings
{
    public string area_name = "undefined";
    public int weight = 0;
    public string type = "hall";
    public int width = 0;
    public int height = 0;
    public List<GenerationTurfSettings> generationTurfSettings = new List<GenerationTurfSettings>();
}
