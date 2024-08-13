using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    public int currentStage;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Stage2ButtonClick()
    {
        currentStage = 1;
    }

    void Stage3ButtonClick()
    {
        currentStage = 2;
    }

}
