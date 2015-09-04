using UnityEngine;
using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;

public class MainUI : DontDestroy<MainUI>
{
	//	private StringBuilder text = new StringBuilder();
	
	public enum Pivot
	{
		None = 0,
		Left = -1,
		Right = 1,
	}
	
	#region UIControl
	public class UIControl
	{
		public GameObject uiGameObject { get; private set; }
		public Vector3 originalLocalPosition = Vector3.zero;
		public Vector3 localPosition = Vector3.zero;
		public float space = 0.0f;
		public bool isShow = false;
		public Pivot pivot = Pivot.None;
		public float showTime = 0.0f;
		
		public UIControl(GameObject gameObject)
		{
			this.uiGameObject = gameObject;
			this.originalLocalPosition = this.uiGameObject.transform.localPosition;
			this.localPosition = this.originalLocalPosition;
			
			if( this.originalLocalPosition.x < 0.0f )
				pivot = Pivot.Left;
			if( this.originalLocalPosition.x > 0.0f )
				pivot = Pivot.Right;
		}
		
		public void ShowUI(bool fixingPositionX = false, bool fixingPositionY = false, float positionX = 0.0f, float positionY = 0.0f)
		{
			if( !fixingPositionX )
				localPosition.x = positionX;
			if( !fixingPositionY )
				localPosition.y = positionY;
			
			uiGameObject.transform.localPosition = localPosition;
			uiGameObject.SetActive(true);
			isShow = true;
		}
		
		public void HideUI()
		{
			uiGameObject.transform.localPosition = originalLocalPosition;
			uiGameObject.SetActive(false);
			isShow = false;
		}
		
		public void ResetUI()
		{
			uiGameObject.SetActive(true);
		}
	}
	#endregion
	
	[HideInInspector] public GameObject buildKindSelectPopupUI;
	[HideInInspector] public GameObject titleUI;
	[HideInInspector] public GameObject upgradePopupUI;
	[HideInInspector] public GameObject upgrade0PopupUI;
	[HideInInspector] public GameObject upgrade1PopupUI;
	[HideInInspector] public GameObject upgrade2PopupUI;
	[HideInInspector] public GameObject upgrade3PopupUI;
	[HideInInspector] public GameObject upgrade4PopupUI;
	[HideInInspector] public GameObject itemUsePopupUI;
	[HideInInspector] public GameObject itemUse0PopupUI;
	[HideInInspector] public GameObject itemUse1PopupUI;
	[HideInInspector] public GameObject itemUse2PopupUI;
	[HideInInspector] public GameObject itemUse3PopupUI;

	UIControl buildKindSelectPopupUIControl;
	UIControl titleUIControl;
	UIControl upgradePopupUIControl;
	UIControl upgrade0PopupUIControl;
	UIControl upgrade1PopupUIControl;
	UIControl upgrade2PopupUIControl;
	UIControl upgrade3PopupUIControl;
	UIControl upgrade4PopupUIControl;
	UIControl itemUsePopupUIControl;
	UIControl itemUse0PopupUIControl;
	UIControl itemUse1PopupUIControl;
	UIControl itemUse2PopupUIControl;
	UIControl itemUse3PopupUIControl;

	
	public GameObject top;
	public GameObject center;
	public GameObject bottom;

	GameObject buildOnBtn;
	
	int buildKindState;

	[HideInInspector] public GameObject buildLayerSelectPopup;
	GameObject buildStartOnPopup;

    public GameObject leftArrow;
    public GameObject rigthArrow;
    //int randPage;

    int randState;

    List<BuildingAI> buildingAIList = new List<BuildingAI>(); 

	override protected void OnAwake()
	{
		buildKindSelectPopupUI	= GameObject.Find("/Main/BuildKindSelectPopup");
		titleUI					= GameObject.Find("/Main/Title");
		upgradePopupUI			= GameObject.Find("/Main/UpgradePopup");
		upgrade0PopupUI			= GameObject.Find("/Main/Upgrade0Popup");
		upgrade1PopupUI			= GameObject.Find("/Main/Upgrade1Popup");
		upgrade2PopupUI			= GameObject.Find("/Main/Upgrade2Popup");
		upgrade3PopupUI			= GameObject.Find("/Main/Upgrade3Popup");
		upgrade4PopupUI			= GameObject.Find("/Main/Upgrade4Popup");
		itemUsePopupUI			= GameObject.Find("/Main/ItemUsePopup");
		itemUse0PopupUI			= GameObject.Find("/Main/ItemUse0Popup");
		itemUse1PopupUI			= GameObject.Find("/Main/ItemUse1Popup");
		itemUse2PopupUI			= GameObject.Find("/Main/ItemUse2Popup");
		itemUse3PopupUI			= GameObject.Find("/Main/ItemUse3Popup");

		buildOnBtn				= GameObject.Find("/Main/Center/BtnPanel/BuildOn");
	}
	
	override protected void OnStart()
	{
		buildKindSelectPopupUIControl	= new UIControl(buildKindSelectPopupUI);
		titleUIControl					= new UIControl(titleUI);
		upgradePopupUIControl			= new UIControl(upgradePopupUI);
		upgrade0PopupUIControl			= new UIControl(upgrade0PopupUI);
		upgrade1PopupUIControl			= new UIControl(upgrade1PopupUI);
		upgrade2PopupUIControl			= new UIControl(upgrade2PopupUI);
		upgrade3PopupUIControl			= new UIControl(upgrade3PopupUI);
		upgrade4PopupUIControl			= new UIControl(upgrade4PopupUI);
		itemUsePopupUIControl			= new UIControl(itemUsePopupUI);
		itemUse0PopupUIControl			= new UIControl(itemUse0PopupUI);
		itemUse1PopupUIControl			= new UIControl(itemUse1PopupUI);
		itemUse2PopupUIControl			= new UIControl(itemUse2PopupUI);
		itemUse3PopupUIControl			= new UIControl(itemUse3PopupUI);

		AllHidePopupUI();

		top.transform.localPosition = new Vector3 (5000.0f, 0.0f, 0.0f);
		center.transform.localPosition = new Vector3 (5000.0f, 0.0f, 0.0f);
		center.transform.localPosition = new Vector3 (5000.0f, 0.0f, 0.0f);

		ShowTitleUI();

		SetBtn();

        MoneyChange();

        ArrowClick(true);

        SoundManager.instans.BGMPlay(SoundManager.BGMSoundEnum.Title);
	}

	void SetBtn()
	{
		UIEventListener.Get(buildOnBtn).onClick += DoingClick;

        UIEventListener.Get(leftArrow).onClick += DoingClick;
        UIEventListener.Get(rigthArrow).onClick += DoingClick;


		UIEventListener.Get(transform.FindChild("BuildKindSelectPopup/BtnPanel/BuildKindSelectPopupClose").gameObject).onClick += DoingClick;

		UIEventListener.Get(transform.FindChild("Title/BtnPanel/TitleClose").gameObject).onClick += DoingClick;

		UIEventListener.Get(transform.FindChild("UpgradePopup/BtnPanel/UpgradePopupClose").gameObject).onClick += DoingClick;
		UIEventListener.Get(transform.FindChild("ItemUsePopup/BtnPanel/ItemUsePopupClose").gameObject).onClick += DoingClick;

		UIEventListener.Get(transform.FindChild("Upgrade0Popup/BtnPanel/Upgrade0PopupClose").gameObject).onClick += DoingClick;
		UIEventListener.Get(transform.FindChild("Upgrade1Popup/BtnPanel/Upgrade1PopupClose").gameObject).onClick += DoingClick;
		UIEventListener.Get(transform.FindChild("Upgrade2Popup/BtnPanel/Upgrade2PopupClose").gameObject).onClick += DoingClick;
		UIEventListener.Get(transform.FindChild("Upgrade3Popup/BtnPanel/Upgrade3PopupClose").gameObject).onClick += DoingClick;
		UIEventListener.Get(transform.FindChild("Upgrade4Popup/BtnPanel/Upgrade4PopupClose").gameObject).onClick += DoingClick;

		UIEventListener.Get(transform.FindChild("ItemUse0Popup/BtnPanel/ItemUse0PopupClose").gameObject).onClick += DoingClick;
		UIEventListener.Get(transform.FindChild("ItemUse1Popup/BtnPanel/ItemUse1PopupClose").gameObject).onClick += DoingClick;
		UIEventListener.Get(transform.FindChild("ItemUse2Popup/BtnPanel/ItemUse2PopupClose").gameObject).onClick += DoingClick;
		UIEventListener.Get(transform.FindChild("ItemUse3Popup/BtnPanel/ItemUse3PopupClose").gameObject).onClick += DoingClick;

		
		GameObject buildKindPanel = transform.FindChild("BuildKindSelectPopup/BtnPanel/BuildKindPanel").gameObject;
		
		for (int i = 0; i < buildKindPanel.transform.childCount; i++)
		{
			GameObject obj = buildKindPanel.transform.FindChild("BuildKind"+i).gameObject;
			
			obj.name = "BuildKind";
			
			UIEventListener.Get(obj).onClick += DoingClick;
			UIEventListener.Get(obj).eventCount = i;
		}
		
		
		GameObject upgradePanel = transform.FindChild("UpgradePopup/BtnPanel/UpgradePanel").gameObject;
		
		for (int i = 0; i < upgradePanel.transform.childCount; i++)
		{
			GameObject obj = upgradePanel.transform.FindChild("Upgrade"+i).gameObject;
			
			obj.name = "Upgrade";
			
			UIEventListener.Get(obj).onClick += DoingClick;
			UIEventListener.Get(obj).eventCount = i;
		}
		
		
		GameObject itemUsePanel = transform.FindChild("ItemUsePopup/BtnPanel/ItemUsePanel").gameObject;
		
		for (int i = 0; i < itemUsePanel.transform.childCount; i++)
		{
			GameObject obj = itemUsePanel.transform.FindChild("ItemUse"+i).gameObject;
			
			obj.name = "ItemUse";
			
			UIEventListener.Get(obj).onClick += DoingClick;
			UIEventListener.Get(obj).eventCount = i;
		}
		
		
		GameObject mainMenuPanel = GameObject.Find("/Main/Bottom/BtnPanel/MainMenuPanel");
		
		for (int i = 0; i < mainMenuPanel.transform.childCount; i++)
		{
			GameObject obj = mainMenuPanel.transform.FindChild("MainMenu"+i).gameObject;
			
			obj.name = "MainMenu";
			
			UIEventListener.Get(obj).onClick += DoingClick;
			UIEventListener.Get(obj).eventCount = i;
		}
	}

	void DoingClick(GameObject go) 
	{
        SoundManager.instans.EffectPlay(SoundManager.EffectSoundEnum.Click , false);
		if (go.name == "BuildOn")
		{
			PopupOpen(true, "BuildKindSelect");
		}
		else if (go.name == "TitleClose")
		{
			Debug.Log("!!");
			SoundManager.instans.BGMPlay(SoundManager.BGMSoundEnum.Main);
			HideTitleUI();
		}
		else if (go.name == "UpgradePopupClose")
		{
			PopupOpen(false, "Upgrade");
		}
		else if (go.name == "ItemUsePopupClose")
		{
			PopupOpen(false, "ItemUse");
		}
		else if (go.name == "BuildKindSelectPopupClose")
		{
			PopupOpen(false, "BuildKindSelect");
		}
		else if (go.name == "BuildKind")
		{
			int state = UIEventListener.Get(go).eventCount;
			
			buildKindState = state;
			
			CreatSelectPopupOpen(true); 
		}
		else if (go.name == "Upgrade0PopupClose")
		{
			PopupOpen(false, "Upgrade0");
		}
		else if (go.name == "Upgrade1PopupClose")
		{
			PopupOpen(false, "Upgrade1");
		}
		else if (go.name == "Upgrade2PopupClose")
		{
			PopupOpen(false, "Upgrade2");
		}
		else if (go.name == "Upgrade3PopupClose")
		{
			PopupOpen(false, "Upgrade3");
		}
		else if (go.name == "Upgrade4PopupClose")
		{
			PopupOpen(false, "Upgrade4");
		}
		else if (go.name == "ItemUse0PopupClose")
		{
			PopupOpen(false, "ItemUse0");
		}
		else if (go.name == "ItemUse1PopupClose")
		{
			PopupOpen(false, "ItemUse1");
		}
		else if (go.name == "ItemUse2PopupClose")
		{
			PopupOpen(false, "ItemUse2");
		}
		else if (go.name == "ItemUse3PopupClose")
		{
			PopupOpen(false, "ItemUse3");
		}
		else if (go.name == "Upgrade")
		{
			int state = UIEventListener.Get(go).eventCount;

			if(state == 0)
			{
				PopupOpen(true, "Upgrade0");
			}
			else if(state == 1)
			{
				PopupOpen(true, "Upgrade1");
			}
			else if(state == 2)
			{
				PopupOpen(true, "Upgrade2");
			}
			else if(state == 3)
			{
				PopupOpen(true, "Upgrade3");
			}
			else if(state == 4)
			{
				PopupOpen(true, "Upgrade4");
			}
		}
		else if (go.name == "ItemUse")
		{
			int state = UIEventListener.Get(go).eventCount;
			
			if(state == 0)
			{
				PopupOpen(true, "ItemUse0");
			}
			else if(state == 1)
			{
				PopupOpen(true, "ItemUse1");
			}
			else if(state == 2)
			{
				PopupOpen(true, "ItemUse2");
			}
			else if(state == 3)
			{
				PopupOpen(true, "ItemUse3");
			}
		}
		else if(go.name == "MainMenu")
		{
			int state = UIEventListener.Get(go).eventCount;
			
			if(state == 0)
			{
				PopupOpen(true, "Upgrade");
			}
			else if(state == 1)
			{
				PopupOpen(true, "ItemUse");
			}
			else if(state == 2)
			{
			}
		}
        else if (go.name == "LeftArrow")
        {
            ArrowClick(true);
        }
        else if (go.name == "RigthArrow")
        {
            ArrowClick(false);
        }
	}

    void ArrowClick(bool left)
    {

        if (left == true && randState > 0)
        {
            randState = randState - 1;
        }
        else if (left == false && randState < (buildingAIList.Count))
        {
            randState = randState + 1;
        }

        Debug.Log("ArrowClick randPage = " + randState);

        ArrowReset();
    }

    void ArrowReset() 
    {
        if (randState == 0)
        {
            leftArrow.GetComponent<BoxCollider>().enabled = false;
            leftArrow.GetComponent<UISprite>().color = new Color(0.5f, 0.5f, 0.5f);

            if (buildingAIList.Count == 0)
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
        else if (randState == buildingAIList.Count)
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

	#region PopupUI
	public void AllHidePopupUI()
	{
		HideBuildKindSelectPopupUI();
		HideUpgradePopupUI();
		HideUpgrade0PopupUI();
		HideUpgrade1PopupUI();
		HideUpgrade2PopupUI();
		HideUpgrade3PopupUI();
		HideUpgrade4PopupUI();
		HideItemUsePopupUI();
		HideItemUse0PopupUI();
		HideItemUse1PopupUI();
		HideItemUse2PopupUI();
		HideItemUse3PopupUI();
	}
	
	public void ShowTitleUI()
	{
		titleUIControl.ShowUI();
	}
	
	public void HideTitleUI()
	{
		titleUIControl.HideUI();

		top.transform.localPosition = new Vector3 (0.0f, 0.0f, 0.0f);
		center.transform.localPosition = new Vector3 (0.0f, 0.0f, 0.0f);
		center.transform.localPosition = new Vector3 (0.0f, 0.0f, 0.0f);
	}
	
	public void ShowBuildKindSelectPopupUI()
	{
		AllHidePopupUI();
		buildKindSelectPopupUIControl.ShowUI();
	}
	
	public void HideBuildKindSelectPopupUI()
	{
		buildKindSelectPopupUIControl.HideUI();
	}
	
	public void ShowUpgradePopupUI()
	{
		AllHidePopupUI();
		upgradePopupUIControl.ShowUI();
	}
	
	public void HideUpgradePopupUI()
	{
		upgradePopupUIControl.HideUI();
	}
	
	public void ShowUpgrade0PopupUI()
	{
		AllHidePopupUI();
		upgrade0PopupUIControl.ShowUI();
	}
	
	public void HideUpgrade0PopupUI()
	{
		upgrade0PopupUIControl.HideUI();
	}
	
	public void ShowUpgrade1PopupUI()
	{
		AllHidePopupUI();
		upgrade1PopupUIControl.ShowUI();
	}
	
	public void HideUpgrade1PopupUI()
	{
		upgrade1PopupUIControl.HideUI();
	}
	
	public void ShowUpgrade2PopupUI()
	{
		AllHidePopupUI();
		upgrade2PopupUIControl.ShowUI();
	}
	
	public void HideUpgrade2PopupUI()
	{
		upgrade2PopupUIControl.HideUI();
	}
	
	public void ShowUpgrade3PopupUI()
	{
		AllHidePopupUI();
		upgrade3PopupUIControl.ShowUI();
	}
	
	public void HideUpgrade3PopupUI()
	{
		upgrade3PopupUIControl.HideUI();
	}
	
	public void ShowUpgrade4PopupUI()
	{
		AllHidePopupUI();
		upgrade4PopupUIControl.ShowUI();
	}
	
	public void HideUpgrade4PopupUI()
	{
		upgrade4PopupUIControl.HideUI();
	}
	
	public void ShowItemUsePopupUI()
	{
		AllHidePopupUI();
		itemUsePopupUIControl.ShowUI();
	}
	
	public void HideItemUsePopupUI()
	{
		itemUsePopupUIControl.HideUI();
	}
	
	public void ShowItemUse0PopupUI()
	{
		AllHidePopupUI();
		itemUse0PopupUIControl.ShowUI();
	}
	
	public void HideItemUse0PopupUI()
	{
		itemUse0PopupUIControl.HideUI();
	}
	
	public void ShowItemUse1PopupUI()
	{
		AllHidePopupUI();
		itemUse1PopupUIControl.ShowUI();
	}
	
	public void HideItemUse1PopupUI()
	{
		itemUse1PopupUIControl.HideUI();
	}
	
	public void ShowItemUse2PopupUI()
	{
		AllHidePopupUI();
		itemUse2PopupUIControl.ShowUI();
	}
	
	public void HideItemUse2PopupUI()
	{
		itemUse2PopupUIControl.HideUI();
	}
	
	public void ShowItemUse3PopupUI()
	{
		AllHidePopupUI();
		itemUse3PopupUIControl.ShowUI();
	}
	
	public void HideItemUse3PopupUI()
	{
		itemUse3PopupUIControl.HideUI();
	}
	#endregion

	void SlideOnTop()
	{
		TweenPosition.Begin(top, 0.5f, Vector3.zero);
	}
	
	void SlideOnBottom()
	{
		TweenPosition.Begin(bottom, 0.5f, Vector3.zero);
	}
	
	IEnumerator SlideOffTop()
	{
		TweenPosition.Begin(top, 0.5f, new Vector3(0, 450, 0));
		TweenPosition.Begin(bottom, 0.5f, new Vector3(0, -550, 0));
		
		yield return new WaitForSeconds(0.5f);
	}
	
	void PopupOpen(bool open, string popupName = "") 
	{
		buildOnBtn.SetActive(!open);
		
//		GameObject buildKindSelectPopup = transform.FindChild("BuildKindSelectPopup").gameObject;
		
		if(open)
		{
			if(popupName == "BuildKindSelect")
			{
				StartCoroutine(SlideOffTop());
				MainUI.instance.ShowBuildKindSelectPopupUI();
			}
			else if(popupName == "Upgrade")
			{
				StartCoroutine(SlideOffTop());
				MainUI.instance.ShowUpgradePopupUI();
			}
			else if(popupName == "Upgrade0")
			{
				SlideOnTop();
				MainUI.instance.ShowUpgrade0PopupUI();
			}
			else if(popupName == "Upgrade1")
			{
				SlideOnTop();
				MainUI.instance.ShowUpgrade1PopupUI();
			}
			else if(popupName == "Upgrade2")
			{
				SlideOnTop();
				MainUI.instance.ShowUpgrade2PopupUI();
			}
			else if(popupName == "Upgrade3")
			{
				SlideOnTop();
				MainUI.instance.ShowUpgrade3PopupUI();
			}
			else if(popupName == "Upgrade4")
			{
				SlideOnTop();
				MainUI.instance.ShowUpgrade4PopupUI();
			}
			else if(popupName == "ItemUse")
			{
				StartCoroutine(SlideOffTop());
				MainUI.instance.ShowItemUsePopupUI();
			}
			else if(popupName == "ItemUse0")
			{
				SlideOnTop();
				MainUI.instance.ShowItemUse0PopupUI();
			}
			else if(popupName == "ItemUse1")
			{
				SlideOnTop();
				MainUI.instance.ShowItemUse1PopupUI();
			}
			else if(popupName == "ItemUse2")
			{
				SlideOnTop();
				MainUI.instance.ShowItemUse2PopupUI();
			}
			else if(popupName == "ItemUse3")
			{
				SlideOnTop();
				MainUI.instance.ShowItemUse3PopupUI();
			}
		}
		else 
		{
			if(popupName == "Upgrade0" || popupName == "Upgrade1" || popupName == "Upgrade2" || popupName == "Upgrade3" || popupName == "Upgrade4")
			{
				StartCoroutine(SlideOffTop());
				MainUI.instance.ShowUpgradePopupUI();
			}
			else if(popupName == "ItemUse0" || popupName == "ItemUse1" || popupName == "ItemUse2" || popupName == "ItemUse3")
			{
				StartCoroutine(SlideOffTop());
				MainUI.instance.ShowItemUsePopupUI();
			}
			else
			{
				MainUI.instance.AllHidePopupUI();
				SlideOnTop();
				SlideOnBottom();
			}
		}
	}

	public void CreatSelectPopupOpen(bool open) 
	{
		GameObject buildKindSelectPopup = transform.FindChild("BuildKindSelectPopup").gameObject;
		buildKindSelectPopup.gameObject.SetActive(!open);
		
		if (open)
		{
			buildLayerSelectPopup = Instantiate(Resources.Load("Prefabs/BuildLayerSelectPopup")) as GameObject;
			buildLayerSelectPopup.transform.parent = transform;
			buildLayerSelectPopup.GetComponent<BuildLayerSelectPopup>().buildKindState = buildKindState;
		}
		else 
		{
			if (buildLayerSelectPopup != null)
			{
				Destroy(buildLayerSelectPopup);
			}
		}
	}
	
	public void BuildStartPopupOpen(bool open)
	{
		buildLayerSelectPopup.gameObject.SetActive(!open);
		
		if (open)
		{
			buildStartOnPopup = Instantiate(Resources.Load("Prefabs/BuildStartOnPopup")) as GameObject;
			buildStartOnPopup.transform.parent = transform;

            BuildLayerSelectPopup buildLayerSelectPopupClass = buildLayerSelectPopup.GetComponent<BuildLayerSelectPopup>();

            buildStartOnPopup.GetComponent<BuildStartOnPopup>().DataSet(buildLayerSelectPopupClass.buildingLayerDataList.Count,
                                                                        buildKindState,
                                                                        buildLayerSelectPopupClass.totalPriceValue);
		}
		else
		{
			if (buildStartOnPopup != null)
			{
				Destroy(buildStartOnPopup);
			}
		}
	}
	
	public void BuildStartClear()
	{
		TweenPosition.Begin(top, 0.5f, Vector3.zero);
		TweenPosition.Begin(bottom, 0.5f, Vector3.zero);

        if ( randState >=  buildingAIList.Count)
        {
            GameObject obj = new GameObject();
            BuildingAI buildingAI = obj.AddComponent<BuildingAI>();

            obj.transform.parent = transform.FindChild("BuildingAIPanel");
            obj.transform.localPosition = new Vector3( randState * 1080 , 0, 0);

            buildingAIList.Add(buildingAI);
        }
		
		List<BuildingLayer> buildingLayerDataList = buildLayerSelectPopup.GetComponent<BuildLayerSelectPopup>().buildingLayerDataList;
        string randName = buildStartOnPopup.transform.FindChild("InputPanel/NameInput").GetComponent<UIInput>().value;

        buildingAIList[randState].SetData(buildingLayerDataList, randState, randName);

        MainDataManager.instance.money -= buildLayerSelectPopup.GetComponent<BuildLayerSelectPopup>().totalPriceValue;
        MoneyChange();
        Destroy(buildLayerSelectPopup);
        Destroy(buildStartOnPopup);

        
        ArrowReset();

	}

    void LandReset() 
    {
        string randName = "부지이름";
        if (randState < buildingAIList.Count)
        {
            randName = buildingAIList[randState].randName;
            transform.FindChild("BuildingAIPanel").gameObject.SetActive(true);

            transform.FindChild("BuildingAIPanel").transform.localPosition = new Vector3(-1080 * (randState), 0, 0);
        }
        else 
        {
            transform.FindChild("BuildingAIPanel").gameObject.SetActive(false);
        }

        top.transform.FindChild("LabelPanel/LandName").GetComponent<UILabel>().text = randName;

        buildOnBtn.SetActive(randState >= buildingAIList.Count);

        Debug.Log("LandReset randState !! = "+randState);
    }

    

    public void MoneyChange()
    {
        top.transform.FindChild("LabelPanel/Money").GetComponent<UILabel>().text = MainDataManager.instance.money.ToString();
    }
}
