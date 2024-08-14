using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    public int currentStage;
    public int clickStage;

    AudioManager audioManager;

    // 게임이 시작될 때, 스테이지 레벨 초기화
    private void Start()
    {
        currentStage = 0;
    }

    public void ClickStage1()
    {

        // 클릭 시, 소리 생성
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
