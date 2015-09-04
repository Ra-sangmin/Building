using UnityEngine;
using System.Collections;

public class BuildKindSelectPopup : MonoBehaviour
{
    public int kindState = -1;
    BuildReadyPopupController buildReadyPopupController;

    // Use this for initialization
	void Start () {
        SetBtn();

        buildReadyPopupController = transform.parent.GetComponent<BuildReadyPopupController>();

	}

    void SetBtn() 
    {
        UIEventListener.Get(transform.FindChild("BtnPanel/BuildKindSelectPopupClose").gameObject).onClick += DoingClick;

        GameObject buildKindPanel = transform.FindChild("BtnPanel/BuildKindPanel").gameObject;

        for (int i = 0; i < buildKindPanel.transform.childCount; i++)
        {
            GameObject obj = buildKindPanel.transform.FindChild("BuildKind" + i).gameObject;
            obj.name = "BuildKind";

            UIEventListener.Get(obj).onClick += DoingClick;
            UIEventListener.Get(obj).eventCount = i;
            
        }
        
    }
    
    void DoingClick(GameObject go) 
    {
        if (go.name == "BuildKindSelectPopupClose")
        {
            buildReadyPopupController.BuildKindSelect(-1);
        }
        else if (go.name == "BuildKind")
        {
            kindState = UIEventListener.Get(go).eventCount;

            buildReadyPopupController.BuildKindSelect(kindState);
        }
    }

	


	// Update is called once per frame
	void Update () {
	
	}
}
