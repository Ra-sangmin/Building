using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Test : MonoBehaviour {

	// Use this for initialization
	void Start () {

        List<BuildLayerData> allData = BuildDataManager.instance.AllReturn();

        for (int i = 0; i < allData.Count; i++)
        {
            UIAtlas atlas = AtlasLoadManager.instance.GetAtlas(allData[i].imageName);

            if (atlas == null)
            {
                Debug.Log(allData[i].imageName + " 이름없음");
            }

        }

        
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
