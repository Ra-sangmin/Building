using UnityEngine;
using System.Collections;

public class TitleController : MonoBehaviour {

	// Use this for initialization
	void Start () {
        SetBtn();
        SoundManager.instans.BGMPlay(SoundManager.BGMSoundEnum.Title);
	}


    void SetBtn()
    {
        UIEventListener.Get(transform.FindChild("BtnPanel/Start").gameObject).onClick += DoingClick;
    }


    void DoingClick(GameObject go)
    {
        if (go.name == "Start")
        {
            Application.LoadLevel("Main");
            Debug.Log("d");
        }
    }

	// Update is called once per frame
	void Update () {
	
	}
}
