using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BuildingAI : MonoBehaviour {

    public string billboardURL;
    public string buildingName;
    public int randState;

    public bool tenantTimeOn;
    public int tenantState;
    public float tenantDeleyTime;

    List<BuildingLayer> buildingLayerList = new List<BuildingLayer>();
    List<GameObject> buildingLayerOBJList = new List<GameObject>();

    float getMoneyTime;
    float getMoneyDeley;

    float garbageTime;
    float garbageDeley;
    int garbageLayerState = -1;


    float noiseTime;
    float noiseDeley;
    bool noiseOn;
    int noiseTouchCount = 0;
    GameObject noiseOBJ;

    public bool tempOn;



    public int houselevel = 1;
    public int tradelevel = 1;
    public int factorylevel = 1;
    public int culturelevel = 1;
    public int businesslevel = 1;

    public string randName;

    public bool skyloungeOn;
    GameObject skyloungeOBJ;
    public string skyURL;


	// Use this for initialization
	void Start () {

        if (tempOn)
        {
            TempLayerDataSet();
        }
        
        
	}

    public void SetData(List<BuildingLayer> buildingLayerList, int randState ,string randName)
    {
        this.buildingLayerList = buildingLayerList;
        this.randState = randState;
        this.randName = randName;
        Init();
        tenantTimeOn = TenantHaveCheck();
    }

    void TempLayerDataSet()
    {
        buildingLayerList = new List<BuildingLayer>()
        {
            new BuildingLayer(false,0,0),
            new BuildingLayer(false,0,0),
            new BuildingLayer(false,0,0),
            new BuildingLayer(false,0,0),
            new BuildingLayer(false,0,0),
        };

        Init();
        tenantTimeOn = TenantHaveCheck();
    }

    void Init()
    {
        getMoneyTime = 1;
        getMoneyDeley = getMoneyTime;

        garbageTime = 30;
        garbageDeley = garbageTime;

        noiseTime = 47;
        noiseDeley = noiseTime;

        skyURL = "https://play.google.com/store/apps/details?id=com.emiyamuljomdao.AfroRun";
    

        LayerCreat();
        SkyloungeCheck();
    }
    
    void LayerCreat()
    {
        GameObject layerListPanel = Instantiate(Resources.Load("Prefabs/BuildLayer/BuildLayerPanel")) as GameObject;
        layerListPanel.name = "layerListPanel";
        layerListPanel.transform.parent = transform;
        layerListPanel.transform.localPosition = new Vector3(0,45,0);

        GameObject layerList = layerListPanel.transform.FindChild("ListPanel").gameObject;
        

        for (int i = 0; i < buildingLayerList.Count; i++)
        {
            GameObject layerOBJ = Instantiate(Resources.Load("Prefabs/BuildLayer/Layer2")) as GameObject;
            layerOBJ.transform.parent = layerList.transform;
            layerOBJ.transform.localPosition = new Vector3(0, i * 250,0);

            GameObject characterManager = new GameObject("CharacterManager");
            characterManager.transform.parent = layerOBJ.transform;
            characterManager.transform.localPosition = Vector3.zero;
            characterManager.AddComponent<CharacterManager>();

            buildingLayerOBJList.Add(layerOBJ);
            ResetLayerImage(i);
        }

        

        if (buildingLayerList.Count < 5)
        {
            layerListPanel.GetComponent<UIScrollView>().enabled = false;
        }


    }

    public void SkyloungeCheck()
    {
        

        if (skyloungeOn && skyloungeOBJ == null)
        {
            GameObject layerList = transform.FindChild("layerListPanel/ListPanel").gameObject;

            
            skyloungeOBJ = Instantiate(Resources.Load("Prefabs/BuildLayer/Layer3")) as GameObject;
            skyloungeOBJ.name = "skylounge";
            skyloungeOBJ.transform.parent = layerList.transform;
            skyloungeOBJ.transform.localPosition = new Vector3(0, buildingLayerList.Count * 250, 0);

            UIEventListener.Get(skyloungeOBJ).onClick += DoingClick;

        }
    }



    void ResetLayerImage(int state)
    {
        BuildingLayer data = buildingLayerList[state];
        GameObject obj = buildingLayerOBJList[state];

        string imageName = "";
        UIAtlas atlas = null;

        GameObject characterManager = obj.transform.FindChild("CharacterManager").gameObject;

        if (data.tenantOn == false)
        {
            imageName = "빈 방";
            
            characterManager.SetActive(false);
        }
        else 
        {
            BuildLayerData buildLayerData = BuildDataManager.instance.BuildLayerDataGet(data.buildingKind, data.buildingToggle);

            if (buildLayerData != null )
            {
                imageName = buildLayerData.imageName;
            }

            characterManager.SetActive(true);
        }

        atlas = AtlasLoadManager.instance.GetAtlas(imageName);

        if (atlas != null)
        {
            UISprite imageOBJ = obj.transform.FindChild("BGPanel/BG").GetComponent<UISprite>();

            imageOBJ.atlas = atlas;
            imageOBJ.spriteName = imageName;
        }

        

    }

	// Update is called once per frame
	void Update () {

        GetMoneyTimeCheck();

        TenantDeleyCheck();

        GarbageDeleyCheck();

        NoiseDeleyCheck();
	}

    void GetMoneyTimeCheck()
    {
        if (noiseOn)
            return;
        
        getMoneyDeley -= Time.deltaTime;

        if (getMoneyDeley < 0)
        {
            getMoneyDeley = getMoneyTime;
            GetMoney();
        }
    }

    void GetMoney() 
    {
        MainDataManager.instance.MoneyChange(100);
        //transform.parent.parent.GetComponent<MainUI>().MoneyChange();
    }


    bool TenantHaveCheck()
    {
        bool tenantHave = false;

        for (int i = 0; i < buildingLayerList.Count; i++)
        {
            if (buildingLayerList[i].tenantOn == false)
            {
                tenantState = i;
                tenantHave = true;
                tenantDeleyTime = 1;

                break;
            }
        }

        return tenantHave;

    }

    void TenantDeleyCheck() 
    {
        if (tenantTimeOn == false || garbageLayerState != -1)
            return;

        tenantDeleyTime -= Time.deltaTime;


        if (tenantDeleyTime < 0)
        {
            buildingLayerList[tenantState].tenantOn = true;
            ResetLayerImage(tenantState);
            tenantTimeOn = TenantHaveCheck();

        }

    }

    void GarbageDeleyCheck()
    {
        garbageDeley -= Time.deltaTime;

        if (garbageDeley < 0)
        {
            garbageDeley = garbageTime;

            if (garbageLayerState != -1)
            {
                buildingLayerList[garbageLayerState].tenantOn = false;
                ResetLayerImage(garbageLayerState);
            }

            garbageLayerState = -1;

            for (int i = 0; i < buildingLayerOBJList.Count; i++ )
            {
                if (buildingLayerOBJList[i].transform.FindChild("GarbageOBJ") == null)
                {
                    garbageLayerState = i;
                    break;
                }
                
            }

            //garbageLayerState += 1;
            if (garbageLayerState != -1)
            {
                GameObject garbageOBJ = Instantiate(Resources.Load("Prefabs/BuildLayer/GarbageOBJ")) as GameObject;
                garbageOBJ.name = "GarbageOBJ";
                garbageOBJ.transform.parent = buildingLayerOBJList[garbageLayerState].transform;
                garbageOBJ.transform.localPosition = Vector3.zero;
                UIEventListener.Get(garbageOBJ).onClick += DoingClick;
                Debug.Log("GarbageOn");

                transform.parent.parent.GetComponent<MainController>().notificationController.TextOn("쓰레기 발생! 언넝 치워요");

            }
           
        }
    }

    void DoingClick(GameObject go) 
    {
        if (go.name == "GarbageOBJ")
        {
            Destroy(go);
        }
        else if (go.name == "NoiseOBJ")
        {
            NoiseClick();
        }
        else if (go.name == "skylounge")
        {
            Application.OpenURL(skyURL);
            Debug.Log("sky");
        }
        
        
    }
    void GarbageClick() 
    {
        garbageLayerState = -1;
    }


    void NoiseDeleyCheck() 
    {
        noiseDeley -= Time.deltaTime;

        if (noiseDeley < 0)
        {
            noiseOn = true;
            noiseDeley = noiseTime;

            if (noiseTouchCount < 30)
            {
                noiseTouchCount += 5;
            }

            if (noiseOBJ == null)
            {

                int ran = Random.Range(0, buildingLayerOBJList.Count);

                noiseOBJ = Instantiate(Resources.Load("Prefabs/BuildLayer/NoiseOBJ")) as GameObject;
                noiseOBJ.name = "NoiseOBJ";
                noiseOBJ.transform.parent = buildingLayerOBJList[ran].transform;
                noiseOBJ.transform.localPosition = Vector3.zero;
                UIEventListener.Get(noiseOBJ).onClick += DoingClick;
            }

            transform.parent.parent.GetComponent<MainController>().notificationController.TextOn("노이즈 발생! 시끄러워요!");
        }
    }

    void NoiseClick() 
    {
        noiseTouchCount -= 1;

        if (noiseTouchCount <= 0)
        {
            noiseOn = false;
            noiseDeley = noiseTime;

            Destroy(noiseOBJ);
        }
    }
}

public class BuildingLayer
{
    public bool tenantOn;
    public int buildingKind;
    public int buildingToggle;

    public BuildingLayer() { }
    public BuildingLayer(bool tenantOn, int buildingKind, int buildingToggle)
    {
        this.tenantOn = tenantOn;
        this.buildingKind = buildingKind;
        this.buildingToggle = buildingToggle;
    }
}