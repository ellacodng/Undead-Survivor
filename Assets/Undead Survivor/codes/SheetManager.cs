using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class SheetManager : MonoBehaviour
{
    #region SingleTon Pattern
    public static SheetManager Instance { get; private set; }
    private void Awake()
    {
        // If an instance already exists and it's not this one, destroy this one
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        // Set this as the instance and ensure it persists across scenes
        Instance = this;
        DontDestroyOnLoad(this.gameObject);

        // Initialize other components or variables if needed
    }
    #endregion

    // 링크 뒤 export ~ 부분을 빼고 export?format=tsv 추가하기
    const string enemyDataURL = "https://docs.google.com/spreadsheets/d/1Ngc05MXFcm1ZRdU2JH1iBRJm9uIPmVFJ32h_R3LrDoM/export?format=tsv"; //Enemy
    // https://docs.google.com/spreadsheets/d/1Ngc05MXFcm1ZRdU2JH1iBRJm9uIPmVFJ32h_R3LrDoM/edit?gid=0#gid=0

    [SerializeField]
    public List<EnemyData> enemys = new List<EnemyData>();

    IEnumerator Start()
    {
        //Enemy
        UnityWebRequest www = UnityWebRequest.Get(enemyDataURL);
        yield return www.SendWebRequest();

        string data = www.downloadHandler.text;
        print(data);
        ParseEnemyData(data);
    }
    void ParseEnemyData(string data)
    {
        string[] lines = data.Split('\n');
        for (int i = 1; i < lines.Length; i++) // 첫 번째 줄은 헤더이므로 건너뜁니다.
        {
            string[] fields = lines[i].Split('\t');
            if (fields.Length >= 3) // 필드가 충분한지 확인
            {
                EnemyData enemy = new EnemyData()
                {
                    spawnData = int.Parse(fields[0]),
                    spriteType = int.Parse(fields[1]),
                    spawnTime = float.Parse(fields[2]),
                    Health = float.Parse(fields[3]),
                    Speed = float.Parse(fields[4]),
                };
                enemys.Add(enemy);
            }
        }
    }
    

}

[System.Serializable]
public class EnemyData
{
    public int spawnData;
    public int spriteType;
    public float spawnTime;
    public float Health;
    public float Speed;
}
