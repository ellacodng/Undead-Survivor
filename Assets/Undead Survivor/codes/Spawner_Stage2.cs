using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Spawner을 stage 1에 맞춰 수정
// 변수 수정함
public class Spawner_Stage2 : Spawner
{
    public Transform[] spawnPoint2;
    public SpawnData2[] spawnData2; // SpawnData -> SpawnData2로 수정
    public float levelTime2;

    int level;
    float timer;
    bool hasSpawned = false; // 스폰 여부를 확인하는 변수

    public StageManager stageManager2;

    private void Awake()
    {
        if (stageManager2.clickStage == 1)
        {
            spawnPoint2 = GetComponentsInChildren<Transform>();
            levelTime2 = GameManager.instance.maxGameTime / spawnData2.Length;
        }
    }

    void Update()
    {
        if (stageManager2.clickStage == 1)
        {
            if (!GameManager.instance.isLive || hasSpawned) // hasSpawned가 true면 스폰되지 않도록 함
                return;

            timer += Time.deltaTime;
            level = Mathf.Min(Mathf.FloorToInt(GameManager.instance.gameTime / levelTime2), spawnData2.Length - 1);

            if (timer > (spawnData2[level].spawnTime))
            {
                timer = 0;
                Spawn();
                hasSpawned = true; // 스폰이 완료되면 true로 설정
            }
        }
    }

    void Spawn()
    {
        if (stageManager2.clickStage == 1)
        {
            GameObject enemy = GameManager.instance.pool.Get(3);
            enemy.transform.position = spawnPoint2[Random.Range(1, spawnPoint2.Length)].position;
            enemy.GetComponent<BossEnemy1>().Init(spawnData2[level]);
        }
    }
}

[System.Serializable]
public class SpawnData2
{
    public int spriteType;
    public float spawnTime;
    public int health;
    public float speed;
}
