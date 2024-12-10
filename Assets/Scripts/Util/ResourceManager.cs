using System.Collections.Generic;
using UnityEngine;

public class ResourceManager
{
    public static Dictionary<string, Sprite> spriteDic = new Dictionary<string, Sprite>();

    public static void LoadCharacterImage()
    {
        Sprite[] temp = Resources.LoadAll<Sprite>("WhiteScape/Assets");
        foreach (var t in temp)
        {
            spriteDic.Add(t.name, t);
        }
    }
}
