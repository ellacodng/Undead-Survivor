using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    public int currentStage;

    // 게임이 시작될 때, 스테이지 레벨 초기화
    private void Start()
    {
        currentStage = 0;
    }
}
