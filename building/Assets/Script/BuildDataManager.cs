using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BuildDataManager {

    private static BuildDataManager ins;
    public static BuildDataManager instance
    {
        get 
        {
            if(ins == null)
            {
                ins = new BuildDataManager();
            }

            return ins;
        }
    }

    List<BuildLayerData> dataList = new List<BuildLayerData>();


    private BuildDataManager()
    {
        DataSet();
    }

    void DataSet() 
    {
        dataList = new List<BuildLayerData>();

        dataList.Add(new BuildLayerData(0, 0, "고시원"));
        dataList.Add(new BuildLayerData(0, 1, "재래시장"));
        dataList.Add(new BuildLayerData(0, 2, "미싱사"));
        dataList.Add(new BuildLayerData(0, 3, "카바레"));
        dataList.Add(new BuildLayerData(0, 4, "대부업체"));


        dataList.Add(new BuildLayerData(1, 0, "원룸"));
        dataList.Add(new BuildLayerData(1, 1, "편의점"));
        dataList.Add(new BuildLayerData(1, 2, "통조림공장"));
        dataList.Add(new BuildLayerData(1, 3, "노래방"));
        dataList.Add(new BuildLayerData(1, 4, "세무서"));

        
        dataList.Add(new BuildLayerData(2, 0, "오피스텔"));
        dataList.Add(new BuildLayerData(2, 1, "카페"));
        dataList.Add(new BuildLayerData(2, 2, "쇼핑몰의류제작"));
        dataList.Add(new BuildLayerData(2, 3, "클럽"));
        dataList.Add(new BuildLayerData(2, 4, "게임회사"));

        

        dataList.Add(new BuildLayerData(3, 0, "펜트하우스"));
        dataList.Add(new BuildLayerData(3, 1, "명품브랜드샵"));
        dataList.Add(new BuildLayerData(3, 2, "예술가공방"));
        dataList.Add(new BuildLayerData(3, 3, "영화관"));
        dataList.Add(new BuildLayerData(3, 4, "대기업사무실"));


        






    }

    public BuildLayerData BuildLayerDataGet(int buildKindState, int buildToggleState)
    {
        for (int i = 0; i < dataList.Count; i++)
        {
            if (dataList[i].buildKindState == buildKindState &&
                dataList[i].buildToggleState == buildToggleState)
            {
                return dataList[i];
            }
        }

        return null;
    }


    public List<BuildLayerData> AllReturn() 
    {
        return dataList;
    }

}






public class BuildLayerData 
{
    public int buildKindState;
    public int buildToggleState;

    public string imageName;

    public BuildLayerData(){}
    public BuildLayerData(int buildKindState, int buildToggleState, string imageName) 
    {
        this.buildKindState = buildKindState;
        this.buildToggleState = buildToggleState;
        this.imageName = imageName;
    }


}

