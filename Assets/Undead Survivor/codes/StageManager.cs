using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    public int currentStage;

    void Stage2ButtonClick()
    {
        currentStage = 1;
    }

    void Stage3ButtonClick()
    {
        currentStage = 2;
    }

}
