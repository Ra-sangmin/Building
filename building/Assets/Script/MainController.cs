using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MainController : MonoBehaviour {

    public GameObject mainUI;

    public BuildReadyPopupController buildReadyPopupController;

    GameObject buildOnBtn;
    

    GameObject leftArrow;
    GameObject rigthArrow;
    //int randPage;

    public int randState;
    
    GameObject buildingAIPanel;

    public NotificationController notificationController;

    bool menuOpen;

    bool menuAniDoing;

    bool menuHideOn;

    void OnEnable()
    {
        MainDataManager.instance.moneyChageOn += MoneyChange;
    }
    void OnDisable()
    {
        MainDataManager.instance.moneyChageOn -= MoneyChange;
    }

   

	// Use this for initialization
	void Start () {
        SetBtn();

        buildingAIPanel = transform.FindChild("BuildingAIPanel").gameObject;

        MainUIHide(false);

        MoneyChange();

        ArrowClick(true);

        SoundManager.instans.BGMPlay(SoundManager.BGMSoundEnum.Main);


        notificationController.TextOn("지금은 알림중입니다. 건물주 만세!");
        notificationController.TextOn("두번째 알림 테스트입니다. 알림중입니다. 건물주 만세!");

        StartCoroutine(MenuChangeOn());
        
	}

    void SetBtn()
    {
        buildOnBtn = transform.FindChild("MainUI/Center/BtnPanel/BuildOn").gameObject;
        UIEventListener.Get(buildOnBtn).onClick += DoingClick;

        leftArrow = transform.FindChild("MainUI/Bottom/BtnPanel/ArrowPanel/LeftArrow").gameObject;
        UIEventListener.Get(leftArrow).onClick += DoingClick;

        rigthArrow = transform.FindChild("MainUI/Bottom/BtnPanel/ArrowPanel/RigthArrow").gameObject;
        UIEventListener.Get(rigthArrow).onClick += DoingClick;

        UIEventListener.Get(transform.FindChild("MainUI/Bottom/BtnPanel/MenuPanel/Upgrade").gameObject).onClick += DoingClick;
        UIEventListener.Get(transform.FindChild("MainUI/Bottom/BtnPanel/MenuPanel/UseItem").gameObject).onClick += DoingClick;
        UIEventListener.Get(transform.FindChild("MainUI/Bottom/BtnPanel/MenuPanel/Option").gameObject).onClick += DoingClick;

        UIEventListener.Get(transform.FindChild("MainUI/Bottom/BtnPanel/MenuChange").gameObject).onClick += DoingClick;
        
    }
    
    void DoingClick(GameObject go)
    {
        if (go.name == "BuildOn")
        {
            StartCoroutine(BuildOnClick());
            
        }
        else if (go.name == "LeftArrow")
        {
            ArrowClick(true);
        }
        else if (go.name == "RigthArrow")
        {
            ArrowClick(false);
        }
        else if (go.name == "Upgrade")
        {
            Debug.Log("1");
        }
        else if (go.name == "UseItem")
        {
            StartCoroutine(UserItemCreat());
            Debug.Log("2");
        }
        else if (go.name == "Option")
        {
            Debug.Log("3");
        }
        else if (go.name == "MenuChange" && menuAniDoing == false)
        {
            menuOpen = !menuOpen;
            StartCoroutine(MenuChangeOn());
            
            Debug.Log("MenuChange");
        }
        
            
            
    }

    IEnumerator MenuChangeOn() 
    {

        menuAniDoing = true;

        GameObject arrowPanel = transform.FindChild("MainUI/Bottom/BtnPanel/ArrowPanel").gameObject;
        GameObject menuPanel = transform.FindChild("MainUI/Bottom/BtnPanel/MenuPanel").gameObject;

        TweenPosition[] arrowTweenPosOBJ = arrowPanel.transform.GetComponentsInChildren<TweenPosition>();
        TweenPosition[] menuTweenPosOBJ = menuPanel.transform.GetComponentsInChildren<TweenPosition>();
        
        if (menuOpen)
        {
            for (int i = 0; i < arrowTweenPosOBJ.Length; i++)
            {
                arrowTweenPosOBJ[i].PlayReverse();
            }

            yield return new WaitForSeconds(0.5f);

            for (int i = 0; i < menuTweenPosOBJ.Length; i++)
            {
                menuTweenPosOBJ[i].PlayForward();
            }

        }
        else 
        {
            for (int i = 0; i < menuTweenPosOBJ.Length; i++)
            {
                menuTweenPosOBJ[i].PlayReverse();
            }

            yield return new WaitForSeconds(0.5f);

            for (int i = 0; i < arrowTweenPosOBJ.Length; i++)
            {
                arrowTweenPosOBJ[i].PlayForward();
            }

            
        }


        menuAniDoing = false;
    }


    IEnumerator BuildOnClick()
    {
        float waitTime = MainUIHide(true);
        yield return new WaitForSeconds(waitTime);

        GameObject buildReadyPopupControllerOBJ = Instantiate(Resources.Load("Prefabs/BuildReadPopupController")) as GameObject;
        buildReadyPopupControllerOBJ.transform.parent = transform;
        buildReadyPopupController = buildReadyPopupControllerOBJ.GetComponent<BuildReadyPopupController>();
        buildReadyPopupController.buildStateChage(0);

    }

    public float MainUIHide(bool hideon)
    {
        Vector3 topPos = Vector3.zero;
        Vector3 bottmPos = Vector3.zero;

        float anitime = 0.5f;

        menuHideOn = hideon;

        if (menuHideOn)
        {
            topPos = new Vector3(0, 350, 0);
            bottmPos = new Vector3(0, -700, 0);
            buildOnBtn.gameObject.SetActive(false);
            //buildingAIPanel.SetActive(false);

        }

        TweenPosition.Begin(mainUI.transform.FindChild("Top").gameObject, anitime, topPos);
        TweenPosition.Begin(mainUI.transform.FindChild("Bottom").gameObject, anitime, bottmPos);

        //GameObject buildOnBtn = transform.FindChild("MainUI/Center/BtnPanel/BuildOn").gameObject;
        //buildOnBtn.gameObject.SetActive(!hideon);
        LandReset();

        return anitime;
    }


    void ArrowClick(bool left)
    {

        if (left == true && randState > 0)
        {
            randState = randState - 1;
        }
        else if (left == false && randState < (MainDataManager.instance.buildingAIList.Count))
        {
            randState = randState + 1;
        }

        Debug.Log("ArrowClick randPage = " + randState);

        ArrowReset();
    }

    

    public void BuildStartCancle() 
    {
        MainUIHide(false);
        ArrowReset();
    }
    public void BuildStartClear()
    {
        MainUIHide(false);
        //TweenPosition.Begin(top, 0.5f, Vector3.zero);
        //TweenPosition.Begin(bottom, 0.5f, Vector3.zero);

        if (randState >= MainDataManager.instance.buildingAIList.Count)
        {
            GameObject obj = new GameObject();
            BuildingAI buildingAI = obj.AddComponent<BuildingAI>();

            obj.transform.parent = buildingAIPanel.transform;
            obj.transform.localPosition = new Vector3(randState * 1080, 0, 0);

            MainDataManager.instance.buildingAIList.Add(buildingAI);
        }

        List<BuildingLayer> buildingLayerDataList = buildReadyPopupController.buildLayerSelectPopup.buildingLayerDataList;
        string randName = buildReadyPopupController.buildStartOnPopup.gameObject.transform.FindChild("InputPanel/NameInput").GetComponent<UIInput>().value;

        MainDataManager.instance.buildingAIList[randState].SetData(buildingLayerDataList, randState, randName);

        int addMoneyValue = buildReadyPopupController.buildLayerSelectPopup.totalPriceValue;

        MainDataManager.instance.MoneyChange(-addMoneyValue);
        
        Destroy(buildReadyPopupController.gameObject);
        


        ArrowReset();

    }




    public void ArrowReset()
    {
        if (randState == 0)
        {
            leftArrow.GetComponent<BoxCollider>().enabled = false;
            leftArrow.GetComponent<UISprite>().color = new Color(0.5f, 0.5f, 0.5f);

            if (MainDataManager.instance.buildingAIList.Count == 0)
            {
                rigthArrow.GetComponent<BoxCollider>().enabled = false;
                rigthArrow.GetComponent<UISprite>().color = new Color(0.5f, 0.5f, 0.5f);

            }
            else
            {
                rigthArrow.GetComponent<BoxCollider>().enabled = true;
                rigthArrow.GetComponent<UISprite>().color = Color.white;
            }

        }
        else if (randState == MainDataManager.instance.buildingAIList.Count)
        {
            rigthArrow.GetComponent<BoxCollider>().enabled = false;
            rigthArrow.GetComponent<UISprite>().color = new Color(0.5f, 0.5f, 0.5f);

            if (randState != 0)
            {
                leftArrow.GetComponent<BoxCollider>().enabled = true;
                leftArrow.GetComponent<UISprite>().color = Color.white;
            }
        }
        else
        {
            leftArrow.GetComponent<BoxCollider>().enabled = true;
            leftArrow.GetComponent<UISprite>().color = Color.white;

            rigthArrow.GetComponent<BoxCollider>().enabled = true;
            rigthArrow.GetComponent<UISprite>().color = Color.white;

        }
        LandReset();
    }
    void LandReset()
    {
        //string randName = "부지이름";
        if (randState >= MainDataManager.instance.buildingAIList.Count || menuHideOn)
        {
            buildingAIPanel.transform.localPosition = new Vector3(-1080 * (randState + 1), 0, 0);
        }
        else
        {
            buildingAIPanel.transform.localPosition = new Vector3(-1080 * (randState), 0, 0);
        }

        if(menuHideOn == false)
        {
            buildOnBtn.SetActive(randState >= MainDataManager.instance.buildingAIList.Count);
        }
        
        
    }



    public void MoneyChange()
    {
        mainUI.transform.FindChild("Top/LabelPanel/Money").GetComponent<UILabel>().text = MainDataManager.instance.money.ToString();
    }

    IEnumerator UserItemCreat()
    {
        float waitTime = MainUIHide(true);
        yield return new WaitForSeconds(waitTime);

        GameObject uesItemPopupOBJ = Instantiate(Resources.Load("Prefabs/UesItemPopup")) as GameObject;
        uesItemPopupOBJ.transform.parent = transform;
    }

    public void ItemUserClose()
    {
        MainUIHide(false);
        ArrowReset();
    }


	// Update is called once per frame
	void Update () {
	
	}
}
