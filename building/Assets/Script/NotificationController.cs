using UnityEngine;
using System.Collections;

public class NotificationController : MonoBehaviour {

    public UILabel text;

    Vector3 targetPos;

    bool moveStart;

    int speed = 120;

    Queue queueList = new Queue();

	// Use this for initialization
	void Start () {
        //TextOn();
	}
	
	// Update is called once per frame
	void Update () {

        if (moveStart)
        {
            MoveDoing();
        }
	}

    public void TextOn(string textValue)
    {
        queueList.Enqueue(textValue);

        if (moveStart == false)
        {
            MoveTextOn();
        }
    }

    void MoveTextOn() 
    {
        if (queueList.Count == 0)
        {
            moveStart = false;
            gameObject.SetActive(false);
            return;
        }

        text.text = (string)queueList.Dequeue();

        text.transform.localPosition = Vector3.zero;
        targetPos = Vector3.zero;
        targetPos.x = -1100 - text.width;

        moveStart = true;

        gameObject.SetActive(true);
    }


    void MoveDoing()
    {
        text.transform.Translate(Vector3.left * Time.deltaTime * speed);

        if (text.transform.localPosition.x < targetPos.x)
        {
            MoveTextOn();
        }

    }



}
