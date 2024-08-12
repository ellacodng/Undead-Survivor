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
        UnlockCharacter(); // ���� �� UnlockCharacter() �Լ��� Ȱ���Ͽ� ĳ���� �ر�
    }
    void Awake()
    {
        achives = (Achieve[])Enum.GetValues(typeof(Achieve));
        wait = new WaitForSecondsRealtime(5);    

        // ���� PlayerPrefs �� "MyData"��� Ű�� ������ �ִ��� üũ
        if(!PlayerPrefs.HasKey("MyData"))
        {
            Init();
        }
    }

    void Init()
    {
        PlayerPrefs.SetInt("MyData", 1); // key�� ����� int�� �����͸� ����

        // ���������� ������ ����
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
            bool isUnlock = PlayerPrefs.GetInt(achiveName) == 1; // ����� �������¸� ������ ��ư Ȱ��ȭ�� ����
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
