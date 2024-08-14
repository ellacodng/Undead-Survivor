using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEnemy1 : Enemy
{
    public float specialAttackCooldown = 5f;  // 특수 공격 쿨타임
    private float specialAttackTimer = 0f;

    // 보스 초기화
    protected new void Awake()
    {
        base.Awake(); // 부모 클래스의 Awake() 호출
    }

    public void Init(SpawnData2 data)
    {
        // 보스 적에게 특화된 초기화 작업 수행
        maxHealth = data.health * 2f; // 예: 체력을 두 배로 설정
        health = maxHealth;
        speed = data.speed * 0.8f; // 예: 속도를 조금 느리게 설정
        anim.runtimeAnimatorController = animCon[data.spriteType];

        // 특수 애니메이션이나 특수 공격 패턴을 위한 추가 설정
    }

    // FixedUpdate는 부모 클래스와 동일하게 유지
    protected new void FixedUpdate()
    {
        base.FixedUpdate();

        // 보스의 특수 공격 타이머 업데이트
        specialAttackTimer += Time.fixedDeltaTime;
        if (specialAttackTimer >= specialAttackCooldown)
        {
            specialAttackTimer = 0f;
            SpecialAttack();
        }
    }

    // 보스의 특수 공격 기능 추가
    private void SpecialAttack()
    {
        if (!isLive)
            return;

        // 특수 공격 로직
        Debug.Log("BossEnemy1: Special Attack!");
        AudioManager.instance.PlaySfx(AudioManager.Sfx.SpecialAttack);

        // 특정 범위 내의 플레이어에게 피해를 주는 예제
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, 5f); // 범위 5의 원 안의 모든 Collider2D 감지
        foreach (Collider2D hit in hits)
        {
            if (hit.CompareTag("Player"))
            {
                hit.GetComponent<Player>().TakeDamage(10);
            }
        }
    }

    // OnTriggerEnter2D는 부모 클래스와 동일하게 유지
    protected new void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
    }

    // 보스의 죽음 처리를 부모와 다르게 할 수 있음
    protected new void Dead()
    {
        // 보스가 죽었을 때의 특별한 처리를 여기서 수행
        base.Dead(); // 기본 죽음 처리
        Debug.Log("BossEnemy1: Dead");
        // 추가적인 보스 죽음 연출이 필요하면 여기에 추가
    }
}
