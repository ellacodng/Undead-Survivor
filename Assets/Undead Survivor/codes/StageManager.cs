using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    public int currentStage;
    public int clickStage;

    // 게임이 시작될 때, 스테이지 레벨 초기화
    private void Start()
    {
        currentStage = 0;
    }

    public void ClickStage1()
    {
        clickStage = 0; 
    }
    public void ClickStage2()
    {
        clickStage = 1; 
    }
    public void ClickStage3()
    {
        clickStage = 2; 
    }

}
