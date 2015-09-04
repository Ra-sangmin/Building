using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BuildReadyPopupController : MonoBehaviour {

    public int buildState = -1;
    

    //public List<GameObject> popupList = new List<GameObject>();

    public BuildKindSelectPopup buildKindSelectPopup;
    public BuildLayerSelectPopup buildLayerSelectPopup;
    public BuildStartOnPopup buildStartOnPopup;

    
	// Use this for initialization
	void Start () {
	   
	}
	
	// Update is called once per frame
	void Update () {
	
	}


    public void buildStateChage(int state)
    {
        buildState = state;



        if (buildState == 4)
        {
            transform.parent.GetComponent<MainController>().BuildStartClear();
            return;
        }

        buildKindSelectPopup.gameObject.SetActive(false);
        buildLayerSelectPopup.gameObject.SetActive(false);
        buildStartOnPopup.gameObject.SetActive(false);


        if (buildState == 0)
        {
            buildKindSelectPopup.gameObject.SetActive(true);
        }
        else if (buildState == 1)
        {
            buildLayerSelectPopup.gameObject.SetActive(true);

            buildLayerSelectPopup.Init(buildKindSelectPopup.kindState);
            
        }
        else if (buildState == 2)
        {
            buildStartOnPopup.gameObject.SetActive(true);

            buildStartOnPopup.DataSet(  buildLayerSelectPopup.buildingLayerDataList.Count,
                                        buildKindSelectPopup.kindState,
                                        buildLayerSelectPopup.totalPriceValue);

            //buildStartOnPopup.DataSet(buildLayerSelectPopup.layer)
        }

    }
    
    public void BuildKindSelect(int kindState) 
    {
        if (kindState == -1)
        {
            buildStateChage(-1);
            //transform.parent.GetComponent<MainController>().MainUIHide(false);
            transform.parent.GetComponent<MainController>().BuildStartCancle();
            Debug.Log("취소");
        }
        else 
        {
            buildStateChage(1);
            Debug.Log("다음 진행");
        }
    }


    public void LayerSelectOn()
    {

    }






}
