using UnityEngine;
using System.Collections;

public class CharacterManager : MonoBehaviour {

    
	// Use this for initialization
	void Start () {
        CharacterDataSet();
	}

    public void CharacterDataSet() 
    {

        int leftCount = Random.Range(1, 4);
        GameObject leftParant = new GameObject("Left");
        leftParant.transform.parent = transform; 
        leftParant.transform.localPosition = new Vector3(-220 , 0 , 0);
        StartCoroutine(CreatAI(leftCount, leftParant));


        int rightCount = Random.Range(1, 4);
        GameObject rigthParant = new GameObject("Rigth");
        rigthParant.transform.parent = transform;
        rigthParant.transform.localPosition = new Vector3(220, 0, 0);
        StartCoroutine(CreatAI(rightCount, rigthParant));


    }

    
    IEnumerator CreatAI(int lastCount ,GameObject parant) 
    {
        for (int i = 0; i < lastCount; i++)
        {
            GameObject aiOBJ = Instantiate(Resources.Load("Prefabs/Character/CharacterAI")) as GameObject;

            aiOBJ.transform.parent = parant.transform;
            int xValue = Random.Range(-160, 160);
            aiOBJ.transform.localPosition = new Vector3(xValue,-45,0);

            yield return new WaitForSeconds(0.1f);
            CharacterAI aiClass = aiOBJ.GetComponent<CharacterAI>();


            int characterState = -1;

            while(true)
            {
                int tempState = Random.Range(0, 4);
                if (characterState != tempState)
                {
                    characterState = tempState;
                    break;
                }
            }
            
            aiClass.Init(characterState);
            
        }
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
