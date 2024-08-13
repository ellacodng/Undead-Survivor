using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// stage 가 클리어 됐을 시, 뜨는 알림창 관리
public class StageAchieveManager : MonoBehaviour
{
    public GameObject[] lockStage;
    public GameObject[] unlockStage;
    public GameObject uiNotice;

    public StageManager stageManager;

    enum Achieve { UnlockStage2, UnlockStage3 }
    Achieve[] achives;
    WaitForSecondsRealtime wait;


    private void Start()
    {
        UnlockStage(); // 시작 시 UnlockStage() 함수를 활용하여 스테이지  해금
    }
    void Awake()
    {
        achives = (Achieve[])Enum.GetValues(typeof(Achieve));
        wait = new WaitForSecondsRealtime(5);

        // 만약 PlayerPrefs 에 "MyData"라는 키를 가지고 있는지 체크
        if (!PlayerPrefs.HasKey("MyData"))
        {
            Init();
        }
    }

    void Init()
    {
        PlayerPrefs.SetInt("MyData", 1); // key와 연결된 int형 데이터를 저장

        // 순차적으로 데이터 저장
        foreach (Achieve achive in achives)
        {
            PlayerPrefs.SetInt(achive.ToString(), 0);
        }
    }


    void UnlockStage()
    {
        for (int index = 0; index < lockStage.Length; index++)
        {
            string achiveName = achives[index].ToString();
            bool isUnlock = PlayerPrefs.GetInt(achiveName) == 1; // 저장된 업적상태를 가져와 버튼 활성화에 적용
            lockStage[index].SetActive(!isUnlock);
            unlockStage[index].SetActive(isUnlock);
        }
    }

    private void LateUpdate()
    {
        foreach (Achieve achive in achives)
        {
            CheckStageAchieve(achive);
        }
    }

    void CheckStageAchieve(Achieve achive)
    {
        bool isAchieve = false;

        switch (achive)
        {
            case Achieve.UnlockStage2:
                isAchieve = stageManager.currentStage == 1;
                break;
            case Achieve.UnlockStage3:
                isAchieve = stageManager.currentStage == 2;
                break;
            default:
                break;
        }

        if (isAchieve && PlayerPrefs.GetInt(achive.ToString()) == 0)
        {
            PlayerPrefs.SetInt(achive.ToString(), 1);

            // 수정 
            for (int i = 0; i < uiNotice.transform.childCount; i++)
            {
                bool isActive = i == (int)achive;
                uiNotice.transform.GetChild(i).gameObject.SetActive(isActive);
            }

            StartCoroutine(NoticeRoutine());
        }
    }

    IEnumerator NoticeRoutine()
    {
        uiNotice.SetActive(true);
        AudioManager.instance.PlaySfx(AudioManager.Sfx.LevelUp);


        yield return wait;

        uiNotice.SetActive(false);
    }
}
