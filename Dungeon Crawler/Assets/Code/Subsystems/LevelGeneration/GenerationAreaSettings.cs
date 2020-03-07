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

    //List of directions for calculating if we are touching something
    private Vector2Int[] directions = {
        new Vector2Int(1, 0),
        new Vector2Int(-1, 0),
        new Vector2Int(0, 1),
        new Vector2Int(0, -1)
    };

    public GenerationAreaSettings(GenerationAreaSettings settings = null)
    {
        if (settings == null)
            return;
        area_name = settings.area_name;
        weight = settings.weight;
        type = settings.type;
        width = settings.width;
        height = settings.height;
        generationTurfSettings = settings.generationTurfSettings;
    }


    /// <summary>
    /// This is super scuffed just saying
    /// </summary>
    /// <param name="turfs"></param>
    /// <param name="door_x"></param>
    /// <param name="door_y"></param>
    /// <param name="door_dir"></param>
    /// <returns></returns>
    public List<GenerationAreaSettings> CheckSpace(Turf[,] turfs, int door_x, int door_y, Direction door_dir)
    {

        //Fetch the doors
        List<GenerationTurfSettings> doors = new List<GenerationTurfSettings>();
        foreach (GenerationTurfSettings turfSetting in generationTurfSettings)
        {
            if (turfSetting.door_dir_x != 0 || turfSetting.door_dir_y != 0)
            {
                doors.Add(turfSetting);
            }
        }

        List<GenerationAreaSettings> potentialOutputs = new List<GenerationAreaSettings>();

        //Generate translated copies of myself where the doors attack to the entrance points
        foreach (GenerationTurfSettings door in doors)
        {
            GenerationAreaSettings transformedRoom = new GenerationAreaSettings(this);
            while (door.door_dir_x == -1 && door_dir != Direction.WEST ||
                door.door_dir_x == 1 && door_dir != Direction.EAST ||
                door.door_dir_y == -1 && door_dir != Direction.NORTH ||
                door.door_dir_y == 1 && door_dir != Direction.SOUTH)
            {
                //Rotate until it points AWAY from the door
                int widthTemp = transformedRoom.width;
                transformedRoom.width = -transformedRoom.height;
                transformedRoom.height = widthTemp;
                foreach (GenerationTurfSettings element in transformedRoom.generationTurfSettings)
                {
                    //DEBUG If room rotations are fucked, problem is here
                    int temp = element.x;
                    element.x = -element.y;
                    element.y = temp;
                    temp = element.door_dir_x;
                    element.door_dir_x = -element.door_dir_y;
                    element.door_dir_y = temp;
                }
            }
            //Translate room so the door lines up :')
            Vector2 targetPosition = new Vector2(door_x + (door_dir == Direction.EAST ? -1 : door_dir == Direction.WEST ? 1 : 0), door_y + (door_dir == Direction.NORTH ? 1 : door_dir == Direction.SOUTH ? -1 : 0));
            Vector2 currentPosition = new Vector2(door.x, door.y);
            //DEBUG If room is misaligned problem is here
            Vector2 deltaPosition = targetPosition - currentPosition;
            foreach (GenerationTurfSettings element in transformedRoom.generationTurfSettings)
            {
                element.x += (int)deltaPosition.x;
                element.y += (int)deltaPosition.y;
            }

            potentialOutputs.Add(transformedRoom);
        }

        //Alright, now for the <i>Easy</i> part, just check if the tile is occupied, if ANY tile in it is, don't bother returning it.
        //Otherwise, retrun all that work and let the level generator do its job
        List<GenerationAreaSettings> validOutputs = new List<GenerationAreaSettings>();
        foreach (GenerationAreaSettings potentialRoom in potentialOutputs)
        {

            bool roomValid = true;

            foreach (GenerationTurfSettings tiles in potentialRoom.generationTurfSettings)
            {
                //Check if room is out of bounds
                if (turfs.GetLength(0) <= tiles.x || turfs.GetLength(0) <= tiles.y || tiles.x < 0 || tiles.y < 0)
                {
                    roomValid = false;
                    break;
                }

                //Check if room overlaps another
                try
                {
                    if (turfs[tiles.x, tiles.y] == null || turfs[tiles.x, tiles.y].calculated)
                    {
                        roomValid = false;
                        break;
                    }
                }
                catch
                {
                    Debug.Log(turfs.GetLength(0) + "," + tiles.x + "," + tiles.y);
                    roomValid = false;
                    break;
                }

                //Check if doors attack to a room they are not meant to (attatch to occupied with no door = bad)
                if (tiles.door_dir_x != 0 || tiles.door_dir_y != 0)
                {
                    Vector2Int doorPos = new Vector2Int(tiles.x + tiles.door_dir_x, tiles.y + tiles.door_dir_y);

                    //Check if it goes out of bounds
                    if (doorPos.x >= turfs.GetLength(0) || doorPos.y >= turfs.GetLength(0) || doorPos.x < 0 || doorPos.y < 0)
                    {
                        roomValid = false;
                        break;
                    }

                    //Check if a door is there (if occupied)
                    if (turfs[doorPos.x, doorPos.y].calculated && turfs[doorPos.x, doorPos.y].occupied)
                    {
                        roomValid = false;
                        break;
                    }
                }

                //Final check, see if any rooms that border us have doors that need filling
                foreach(Vector2Int direction in directions)
                {
                    Vector2Int checkPos = new Vector2Int(tiles.x + direction.x, tiles.y + direction.y);

                    //Check if it goes out of bounds
                    if (checkPos.x >= turfs.GetLength(0) || checkPos.y >= turfs.GetLength(0) || checkPos.x < 0 || checkPos.y < 0)
                    {
                        roomValid = false;
                        break;
                    }

                    //Check if occupied and has door
                    if (turfs[checkPos.x, checkPos.y].door && (tiles.door_dir_x == 0 && tiles.door_dir_y == 0))
                    {
                        roomValid = false;
                        break;
                    }
                }

            }

            if (roomValid)
            {
                //Convert tiledata to generation area settings data
                validOutputs.Add(potentialRoom);
            }

        }

        return validOutputs;
    }

}
