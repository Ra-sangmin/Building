using UnityEngine;
using System.Collections;

public class BuildStartOnPopup : MonoBehaviour {
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
        transform.FindChild("LabelPanel/MyMoney").GetComponent<UILabel>().text = MainDataManager.instance.money.ToString();
    }


	// Use this for initialization
	void Start ()
	{
        SetBtn();
        Init();

        buildReadyPopupController = transform.parent.GetComponent<BuildReadyPopupController>();
	}
    
    void SetBtn()
    {
        UIEventListener.Get(transform.FindChild("BtnPanel/BuildStart").gameObject).onClick += DoingClick;
       // UIEventListener.Get(transform.FindChild("BtnPanel/BuildNameChange").gameObject).onClick += DoingClick;
        UIEventListener.Get(transform.FindChild("BtnPanel/BuildStartOnBack").gameObject).onClick += DoingClick;

    }
    
    void Init() 
    {
        MoneyLabelReset();
    }

    public void DataSet(int layerCount, int buildKindState, int totalPrice) 
    {
        transform.FindChild("LabelPanel/LayerCount").GetComponent<UILabel>().text = layerCount+"층";

        string text = "";
        switch (buildKindState)
        {
            case 0: text = "부실공사"; break;
            case 1: text = "일반공사"; break;
            case 2: text = "내진공사"; break;
            case 3: text = "복합공사"; break;
        }

        transform.FindChild("LabelPanel/BuildKindValue").GetComponent<UILabel>().text = text;
        transform.FindChild("LabelPanel/PriceValue").GetComponent<UILabel>().text = totalPrice.ToString();
        
    }

    void DoingClick(GameObject go) 
    {
        SoundManager.instans.EffectPlay(SoundManager.EffectSoundEnum.Click, false);

        if (go.name == "BuildStart")
        {
            buildReadyPopupController.buildStateChage(4);
        }
        else if (go.name == "BuildNameChange")
        {
            UIInput input = transform.FindChild("InputPanel/NameInput").GetComponent<UIInput>();
            input.isSelected = true;
        }
        else if (go.name == "BuildStartOnBack")
        {
            buildReadyPopupController.buildStateChage(1);
        }
    }
}
