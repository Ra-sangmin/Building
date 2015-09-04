using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MainDataManager {

    private static MainDataManager ins;
    public static MainDataManager instance
    {
        get
        {
            if (ins == null)
            {
                ins = new MainDataManager();
            }

            return ins;
        }
    }


    public int money;
    public List<BuildingAI> buildingAIList = new List<BuildingAI>();

    public delegate void MoneyChageOn();
    public MoneyChageOn moneyChageOn;

    private MainDataManager()
    {
        money = 1000;
    }

    public void MoneyChange(int addMoney)
    {
        money += addMoney;

        if (moneyChageOn != null)
        {
            moneyChageOn();
        }
    }


}
