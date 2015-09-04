using UnityEngine;
using System.Collections;

public class CharacterAI : MonoBehaviour {

    public enum CharacterMoveState { Idle, Left, Rigth }
    public CharacterMoveState characterMoveState = CharacterMoveState.Idle;

    float speed = 0.5f;

    public int characterState;

    float moveChangeDeley;
    
	// Use this for initialization
	void Start () {

        characterMoveState = CharacterMoveState.Left;
	}

    public void Init(int characterState) 
    {
        this.characterState = characterState;
    }

	// Update is called once per frame
	void Update () {

        DeleyCheck();
        NotMoveCheck();
        MoveDoing();

        if(Input.GetKeyDown(KeyCode.RightArrow))
        {
            ChangeMove(CharacterMoveState.Rigth); 
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            ChangeMove(CharacterMoveState.Idle);
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            ChangeMove(CharacterMoveState.Left);
        }
	}

    void DeleyCheck()
    {
        moveChangeDeley -= Time.deltaTime;

        if (moveChangeDeley < 0)
        {
            MoveChangeSet();
        }
    }


    void NotMoveCheck() 
    {
        Vector3 pos = transform.localPosition;

        if (characterMoveState == CharacterMoveState.Left && pos.x < -160)
        {
            ChangeMove(CharacterMoveState.Rigth);
        }
        else if (characterMoveState == CharacterMoveState.Rigth && pos.x > 160)
        {
            ChangeMove(CharacterMoveState.Left);
        }
    }

    void MoveDoing() 
    {
        Vector3 moveVecter = Vector3.zero;

        if (characterMoveState != CharacterMoveState.Idle)
        {
            moveVecter = Vector3.left * speed;
        }


        transform.Translate(moveVecter);
    }

    void MoveChangeSet() 
    {
        moveChangeDeley = Random.Range(0, 5);

        int state = Random.Range(0, 3);

        ChangeMove((CharacterMoveState)state);
    }
    void ChangeMove(CharacterMoveState moveState) 
    {
        characterMoveState = moveState;

        string characterAniName = "캐릭터" + (characterState+1)+"-";


        bool aniActive = false;

        if (characterMoveState == CharacterMoveState.Idle)
        {
            GetComponent<UISprite>().spriteName = characterAniName + "정면";
        }
        else
        {
            characterAniName += "걷기";

            if (characterMoveState == CharacterMoveState.Left)
            {
                transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
            }
            else if (characterMoveState == CharacterMoveState.Rigth)
            {
                transform.rotation = Quaternion.Euler(new Vector3(0, 180, 0));
            }

            aniActive = true;
        }


        GetComponent<UISpriteAnimation>().enabled = aniActive;

        GetComponent<UISpriteAnimation>().namePrefix = characterAniName;

    }


    
 }
