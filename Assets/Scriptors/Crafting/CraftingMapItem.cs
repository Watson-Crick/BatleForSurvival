using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingMapItem : MonoBehaviour {

    private int mapId;
    public int MapId
    {
        set { mapId = value; }
        get { return mapId; }
    }

    private string[] mapContents;
    public string[] MapContents
    {
        set { mapContents = value; }
        get { return mapContents; }
    }

    private string mapName;
    public string MapName
    {
        set { mapName = value; }
        get { return mapName; }
    }

    public CraftingMapItem() { }

    public CraftingMapItem(int id, string[] contents, string name)
    {
        mapId = id;
        mapContents = contents;
        mapName = name;
    }

    public override string ToString()
    {
        return string.Format("id is {0}, mapNum is {1}, name is {2}", mapId, mapContents.Length, mapName);
    }
}
