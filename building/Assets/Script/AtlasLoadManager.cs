using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AtlasLoadManager {

    private static AtlasLoadManager ins;
    public static AtlasLoadManager instance
    {
        get
        {
            if (ins == null)
            {
                ins = new AtlasLoadManager();
            }

            return ins;
        }
    }

    List<UIAtlas> atlasList = new List<UIAtlas>();

    private AtlasLoadManager() 
    {
        DataLoad();
    }

    void DataLoad()
    {
        atlasList = new List<UIAtlas>();

        string buildPanelPath = "Atlas/Build";

        string nowPathName = "";

        string fullPath = "";

        int maxCount = 5;
        for (int i = 0; i < maxCount; i++)
        {
            nowPathName = "Build" + i;

            fullPath = buildPanelPath + "/" + nowPathName + "/" + nowPathName + "_atlas";

            GameObject obj = Resources.Load(fullPath) as GameObject;

            UIAtlas atlasOBJ = obj.GetComponent<UIAtlas>();

            atlasList.Add(atlasOBJ);
        }

    }

    public UIAtlas GetAtlas(string imageName) 
    {
        UIAtlas atlas = null;
        for (int i = 0; i < atlasList.Count; i++)
        {
            if (atlasList[i].GetSprite(imageName) != null)
            {
                atlas = atlasList[i];
                break;
            }
        }

        return atlas;

    }

    
}
