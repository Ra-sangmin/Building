using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BuildLayerSelectPopup : MonoBehaviour {

    public UILabel myMoneyOBJ;
    int myMoneysValue;

    public UILabel priceOBJ;
    public int totalPriceValue;
    
    int buildLayerCount = -1;
    int buildToggleState;
    public int buildKindState;

    public int beforeBuildKindState = -1;

    int oneLayerPrice;

    public GameObject toggleSelectImage;
    public GameObject layerSelectImage;

    public GameObject layerParantOBJ;

    public UISprite previewOBJ;

    bool nonStopReady;
    float nonStopReadyDeley;

    bool nonStopBuildOn;
    float nonStopBuildOnDeley;

    List<GameObject> creatLayerOBJList = new List<GameObject>();
    public List<BuildingLayer> buildingLayerDataList = new List<BuildingLayer>();

    GameObject selectLayerOBJ;
    public GameObject selectImage;


    BuildReadyPopupController buildReadyPopupController;

    void OnEnable() 
    {
        MainDataManager.instance.moneyChageOn += MoneyLabelReset;
    }
    void OnDisable()
    {
        MainDataManager.instance.moneyChageOn -= MoneyLabelReset;
    }

    void MoneyLabelReset()
    {
        myMoneysValue = MainDataManager.instance.money;
        myMoneyOBJ.text = myMoneysValue.ToString();
    }
    void Awake() 
    {
        SetBtn();

        MoneyLabelReset();
        
        buildReadyPopupController = transform.parent.GetComponent<BuildReadyPopupController>();
    }
	// Use this for initialization
	void Start ()
	{
        
	}

    void SetBtn() 
    {
        UIEventListener.Get(transform.FindChild("BtnPanel/BuildLayerSelectPopupClose").gameObject).onClick += DoingClick;
        UIEventListener.Get(transform.FindChild("BtnPanel/BuildReadyOn").gameObject).onPress += DoingPress;
        UIEventListener.Get(transform.FindChild("BtnPanel/BuildStart").gameObject).onClick += DoingClick;
        UIEventListener.Get(transform.FindChild("BtnPanel/BuildLastDestroy").gameObject).onClick += DoingClick;

        UIEventListener.Get(transform.FindChild("BtnPanel/BuildLayerPanel/ListPanel/Preview").gameObject).onClick += DoingClick;

        selectLayerOBJ = transform.FindChild("BtnPanel/BuildLayerPanel/ListPanel/Preview").gameObject;

        GameObject togglePanel = transform.FindChild("BtnPanel/BuildTogglePanel").gameObject;

        for (int i = 0; i < togglePanel.transform.childCount; i++)
        {
            if (togglePanel.transform.FindChild("Toggle"+i) == null)
                continue;

            GameObject obj = togglePanel.transform.FindChild("Toggle" + i).gameObject;
            obj.name = "Toggle";

            UIEventListener.Get(obj).onClick += DoingClick;
            UIEventListener.Get(obj).eventCount = i;
        }
    }

    public void Init(int buildKindstate) 
    {
        this.buildKindState = buildKindstate;
        

        if (beforeBuildKindState == buildKindState)
            return;

        ToggleSelectOn();

        beforeBuildKindState = buildKindState;

        switch(buildKindstate)
        {
            case 0: oneLayerPrice = 500; break;
            case 1: oneLayerPrice = 1000; break;
            case 2: oneLayerPrice = 2000; break;
            case 3: oneLayerPrice = 3000; break;
        }

        

        buildLayerCount = -1;
        totalPriceValue = 0;
        PriceTextReset();

        for (int i = 0; i < creatLayerOBJList.Count; i++)
        {
            Destroy(creatLayerOBJList[i]);
        }
        creatLayerOBJList.Clear();

        buildingLayerDataList.Clear();

        layerParantOBJ.transform.localPosition = new Vector3(0, -495, 0);

        UIPanel panel = layerParantOBJ.transform.parent.GetComponent<UIPanel>();
        
        if(panel.GetComponent<SpringPanel>())
        {
            Destroy(panel.GetComponent<SpringPanel>());
        }

        panel.transform.localPosition = new Vector2(0, 108);
        panel.clipOffset = Vector2.zero;

        previewOBJ.transform.localPosition = Vector3.zero;
   
    }

    void DoingClick(GameObject go) 
    {
        SoundManager.instans.EffectPlay(SoundManager.EffectSoundEnum.Click, false);

        if (go.name == "BuildLayerSelectPopupClose")
        {
            buildReadyPopupController.buildStateChage(0);
        }
        else if (go.name == "BuildReadyOn")
        {
            LayerUpOn();
        }
        else if (go.name == "BuildStart")
        {
            buildReadyPopupController.buildStateChage(2);
            Debug.Log("BuildStart");
        }
        else if (go.name == "Toggle")
        {
            int count = UIEventListener.Get(go).eventCount;

            buildToggleState = count;

            toggleSelectImage.transform.position = go.transform.position;

            Debug.Log("toggleSelectImage.transform.position = " + toggleSelectImage.transform.position);

            ToggleSelectOn();
        }
        else if (go.name == "BuildLastDestroy")
        {
            LayerDown();
        }
        else if (go.name == "Layer")
        {
            LayerSelect(go);
        }
        else if (go.name == "Preview")
        {
            LayerSelect(go);
        }
    }

    void DoingPress(GameObject go , bool onPress)
    {
        if (go.name == "BuildReadyOn")
        {
            nonStopReady = onPress;

            if (onPress)
            {
                LayerUpOn();
                nonStopBuildOn = false;
                nonStopReadyDeley = 1f;
            }
            else 
            {

            }
        }
    }

    void ToggleSelectOn() 
    {
        BuildLayerData buildLayerData = BuildDataManager.instance.BuildLayerDataGet(buildKindState, buildToggleState);

        if (buildLayerData != null && selectLayerOBJ != null)
        {
            if (selectLayerOBJ.name == "Preview")
            {
                string imageName = buildLayerData.imageName;
                UIAtlas atlas = AtlasLoadManager.instance.GetAtlas(imageName);

                if (atlas != null)
                {
                    previewOBJ.atlas = atlas;
                    previewOBJ.spriteName = buildLayerData.imageName;
                }

                
            }
            else 
            {
                DataChange(selectLayerOBJ); 
                
            }
        }
    }

    void LayerUpOn()
    {
        int templayerCount = buildLayerCount + 1;
        int tempTotalPriceValue = (templayerCount + 1) * oneLayerPrice;

        Debug.Log("templayerCount = " + templayerCount + " tempTotalPriceValue=   " + tempTotalPriceValue);

        if (myMoneysValue < tempTotalPriceValue)
        {
            Debug.Log("생산 가격이 더 많음");
            //return;
        }




        buildLayerCount = templayerCount;
        totalPriceValue = tempTotalPriceValue;
        PriceTextReset();

        GameObject layerOBJ = Instantiate(Resources.Load("Prefabs/BuildLayer/Layer")) as GameObject;

        layerOBJ.transform.parent = layerParantOBJ.transform;
        layerOBJ.transform.localPosition = new Vector3(0, 250 * buildLayerCount, 0);

        UIEventListener.Get(layerOBJ).onClick += DoingClick;
        layerOBJ.name = "Layer";

        DataChange(layerOBJ);

        
        layerOBJ.transform.FindChild("LabelPanel/LayerCount").GetComponent<UILabel>().text = (buildLayerCount + 1).ToString();
        creatLayerOBJList.Add(layerOBJ);
        buildingLayerDataList.Add(new BuildingLayer(false, buildKindState, buildToggleState));

        previewOBJ.transform.localPosition = new Vector3(0, 250 * (buildLayerCount + 1), 0);

        if (previewOBJ.transform.position.y > 680)
        {
            Vector3 pos = previewOBJ.transform.parent.position;
            pos.y -= 250;
            previewOBJ.transform.parent.position = pos;
        }

        SoundManager.instans.EffectPlay(SoundManager.EffectSoundEnum.BuildOn,false);

        if(selectLayerOBJ!= null && selectLayerOBJ.name != "Preview")
        {
            LayerSelect(previewOBJ.gameObject); 
        }
        

    }

    void DataChange(GameObject go) 
    {
        string imageName = "";
        string kindValue = "";
        string peopleCountValue = "";
        string taxValue = "";
        //Color cor = new Color();
       
        switch (buildToggleState)
        {
            case 0: imageName = "btn8"; kindValue = "주택"; peopleCountValue = "▲"; taxValue = "10"; break;
            case 1: imageName = "btn9"; kindValue = "상업"; peopleCountValue = "▼"; taxValue = "20"; break;
            case 2: imageName = "btn10"; kindValue = "공장"; peopleCountValue = "▼"; taxValue = "50"; break;
            case 3: imageName = "btn11"; kindValue = "문화"; peopleCountValue = "▲"; taxValue = "80"; break;
            case 4: imageName = "btn12"; kindValue = "사무"; peopleCountValue = "▼"; taxValue = "100"; break;
        }

        go.transform.FindChild("BGPanel/Icon").GetComponent<UISprite>().spriteName = imageName;
        go.transform.FindChild("LabelPanel/Kind").GetComponent<UILabel>().text = kindValue;
        go.transform.FindChild("LabelPanel/peopleCount").GetComponent<UILabel>().text = peopleCountValue;
        go.transform.FindChild("LabelPanel/tax").GetComponent<UILabel>().text = taxValue;
    }


    void LayerDown()
    {
        SoundManager.instans.EffectPlay(SoundManager.EffectSoundEnum.BuildCancle, false);

        if (buildLayerCount < 0)
            return;
        
        int removeCount = buildLayerCount;
        Destroy(creatLayerOBJList[removeCount].gameObject);
        creatLayerOBJList.RemoveAt(removeCount);

        buildLayerCount -= 1;
        PriceTextReset();

        previewOBJ.transform.localPosition = new Vector3(0, 250 * (buildLayerCount + 1), 0);


        Debug.Log(" buildLayerCount = " + buildLayerCount +"  "+previewOBJ.transform.position);

        if (buildLayerCount == -1)
        {
            previewOBJ.transform.parent.localPosition = new Vector3(0,-484,0);
        }
        else if ( previewOBJ.transform.position.y < -300 )
        {
            Vector3 pos = previewOBJ.transform.parent.position;
            pos.y += 250;
            previewOBJ.transform.parent.position = pos;
        }

        
    }

    void PriceTextReset() 
    {
        //totalPriceValue = (buildLayerCount + 1) * 100;
        priceOBJ.text = buildLayerCount == -1 ? "0" : "-" + totalPriceValue.ToString();
    }

    void LayerSelect(GameObject go)    
    {
        selectLayerOBJ = go;

        selectImage.transform.parent = go.transform;

        selectImage.transform.localPosition = Vector3.zero;
        selectImage.SetActive(true);
    }

	
	// Update is called once per frame
	void Update ()
	{
        if (nonStopReady)
        {
            NonStopDeleyCheck();
        }
	}


    void NonStopDeleyCheck()
    {
        if (nonStopBuildOn)
        {
            nonStopBuildOnDeley -= Time.deltaTime;

            if (nonStopBuildOnDeley < 0)
            {
                nonStopBuildOnDeley = 0.05f;
                LayerUpOn();
            }
        }
        else 
        {
            nonStopReadyDeley -= Time.deltaTime;

            if (nonStopReadyDeley < 0)
            {
                nonStopBuildOn = true;
                
            }

        }
    }
}
