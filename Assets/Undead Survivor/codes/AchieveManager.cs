using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AchieveManager : MonoBehaviour
{
    public GameObject[] lockCharacter;
    public GameObject[] unLockCharacter;
    public GameObject uiNotice;

    enum Achieve { UnlockPotato, UnlockBean }
    Achieve[] achives;
    WaitForSecondsRealtime wait;


    private void Start()
    {
        UnlockCharacter(); // 시작 시 UnlockCharacter() 함수를 활용하여 캐릭터 해금
    }
    void Awake()
    {
        achives = (Achieve[])Enum.GetValues(typeof(Achieve));
        wait = new WaitForSecondsRealtime(5);    

        // 만약 PlayerPrefs 에 "MyData"라는 키를 가지고 있는지 체크
        if(!PlayerPrefs.HasKey("MyData"))
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


    void UnlockCharacter()
    {
        for (int index = 0; index < lockCharacter.Length; index++)
        {
            string achiveName = achives[index].ToString();
            bool isUnlock = PlayerPrefs.GetInt(achiveName) == 1; // 저장된 업적상태를 가져와 버튼 활성화에 적용
            lockCharacter[index].SetActive(!isUnlock);
            unLockCharacter[index].SetActive(isUnlock);
        }
    }

    private void LateUpdate()
    {
        foreach(Achieve achive in achives)
        {
            CheckAchieve(achive);
        }
    }

    void CheckAchieve(Achieve achive)
    {
        bool isAchieve = false;

        switch (achive)
        {
            case Achieve.UnlockPotato:
                isAchieve = GameManager.instance.kill >= 10;
                break;
            case Achieve.UnlockBean:
                isAchieve = GameManager.instance.gameTime == GameManager.instance.maxGameTime;
                break;
            default:
                break;
        }

        if (isAchieve && PlayerPrefs.GetInt(achive.ToString()) == 0)
        {
            PlayerPrefs.SetInt(achive.ToString(), 1);

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
