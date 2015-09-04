using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UesItemPopup : MonoBehaviour {

    int panelState;
    int useItemState;

    List<UseItemData> useItemDataList = new List<UseItemData>();
    public List<GameObject> panelList = new List<GameObject>();

	// Use this for initialization
	void Start () {
        SetBtn();
        UseItemDataListSet();
        PanelActiveReset();
	}

    void SetBtn() 
    {
        UIEventListener.Get(transform.FindChild("ItemSelectPanel/BtnPanel/UesItemPopupClose").gameObject).onClick += DoingClick;

        GameObject itemUserPanel = transform.FindChild("ItemSelectPanel/BtnPanel/ItemUsePanel").gameObject;
        for (int i = 0; i < itemUserPanel.transform.childCount; i++)
        {
            GameObject obj = itemUserPanel.transform.FindChild("ItemUse"+i).gameObject;
            obj.name = "ItemUse";


            UIEventListener.Get(obj).onClick += DoingClick;
            UIEventListener.Get(obj).eventCount = i;

        }


        UIEventListener.Get(transform.FindChild("ItemUserOkPanel/BtnPanel/ItemUseOkPanelClose").gameObject).onClick += DoingClick;
        UIEventListener.Get(transform.FindChild("ItemUserOkPanel/BtnPanel/ItemUseOK").gameObject).onClick += DoingClick;
    }


    void UseItemDataListSet() 
    {
        useItemDataList = new List<UseItemData>()
        {
            new UseItemData("소음 방지권","6시간동안 소음이\n발생하지 않습니다.","btn1",50000),
            new UseItemData("위대한 미화원","6시간동안 쓰레기가\n발생하지 않습니다.","btn6",50000),
            new UseItemData("입주 광고","비어있는 층에 입주민이\n바로 입주합니다.","btn4",100000),
            new UseItemData("옥외광고판","건물 옥상에 간판을 설치\n간판 클릭시 수금 초기화!\n\n[ffb600ff]건물당1개[-]만 건설가능","btn7",-1),
        };



    }

    void PanelActiveReset() 
    {
        for (int i = 0; i < panelList.Count; i++)
        {
            panelList[i].SetActive(i == panelState);
        }

        if(panelState == 1)
        {
            ItemUsePanelDataReset();
        }

    }
    void ItemUsePanelDataReset() 
    {
        UseItemData data = useItemDataList[useItemState];

        UISprite icon = panelList[1].transform.FindChild("BGPanel/Icon").GetComponent<UISprite>();
        icon.spriteName = data.IconName;

        UILabel name = panelList[1].transform.FindChild("LabelPanel/Name").GetComponent<UILabel>();
        name.text = data.name;

        UILabel discrip = panelList[1].transform.FindChild("LabelPanel/Discrip").GetComponent<UILabel>();
        discrip.text = data.discrip;

        UILabel priceText = panelList[1].transform.FindChild("LabelPanel/PriceText").GetComponent<UILabel>();
        UILabel priceValue = panelList[1].transform.FindChild("LabelPanel/PriceValue").GetComponent<UILabel>();

        priceValue.text = data.price.ToString();

        priceText.gameObject.SetActive(data.price != -1);
        priceValue.gameObject.SetActive(data.price != -1);

    }


    void DoingClick(GameObject go) 
    {
        if (go.name == "UesItemPopupClose")
        {
            transform.parent.GetComponent<MainController>().ItemUserClose();
            Destroy(gameObject);
        }
        else if (go.name == "ItemUse") 
        {
            int count = UIEventListener.Get(go).eventCount;
            useItemState = count;

            panelState = 1;
            PanelActiveReset();
            
        }
        else if (go.name == "ItemUseOkPanelClose")
        {
            panelState = 0;
            PanelActiveReset();
        }
        else if (go.name == "ItemUseOK")
        {
            ItemUseOkClick();
            
        }
        
    }


    void ItemUseOkClick() 
    {
        int randState = transform.parent.GetComponent<MainController>().randState;
        BuildingAI data = MainDataManager.instance.buildingAIList[randState];

        data.skyloungeOn = true;

        data.SkyloungeCheck();

        transform.parent.GetComponent<MainController>().ItemUserClose();
        Destroy(gameObject);
    }

	
	// Update is called once per frame
	void Update () {
	
	}
}

public class UseItemData
{
    public string name;
    public string discrip;
    public string IconName;
    public int price;

    public UseItemData() { }

    public UseItemData( string name , string discrip , string IconName , int price) 
    {
        this.name = name;
        this.discrip = discrip;
        this.IconName = IconName;
        this.price = price;
    }

}

