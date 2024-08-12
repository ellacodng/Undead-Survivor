using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*using UnityEngine.InputSystem;
*/
public class Player : MonoBehaviour // 게임 로직 구성에 필요한 것들을 가진 클래스
{
    public Vector2 inputVec; // input을 넣는 벡터
    public float speed; // 속도
    public Scanner scanner;
    public Hand[] hands;
    public RuntimeAnimatorController[] animCon;

    Rigidbody2D rigid; // 게임 오브젝트의 리지드바디 2D를 저장할 변수 선언
    SpriteRenderer spriter;
    Animator anim;

    // 시작할 때 한번만 실행되는 생명주기 Awake에서 초기화
    void Awake()
    {
       // GetComponent<T>: 오브젝트에서 컴포넌트를 가져오는 함수, T자리 컴포넌트 타입 작성
       rigid = GetComponent<Rigidbody2D>();
       spriter = GetComponent<SpriteRenderer>();
       anim = GetComponent<Animator>();
       scanner = GetComponent<Scanner>();
       hands = GetComponentsInChildren<Hand>(true);
    }

    private void OnEnable()
    {
        speed *= Character.Speed; // 캐릭터에 따라 스피드 다르게 적용
        anim.runtimeAnimatorController = animCon[GameManager.instance.playerId];
    }

    // 하나의 프레임마다 한번씩 호출되는 생명주기 함수
    public void Update() // public 으로 할 시에 unity에 공유 가능
    {
        if (!GameManager.instance.isLive)
            return;

        // 방향값을 계속해서 받음
        // Input: 유니티 엔진에서 받는 모든 입력을 관리하는 클래스
        // Input Manager 에서 키의 이름을 가져옴
        // Axis 값의 범위는 -1 ~ 1 임
        inputVec.x = Input.GetAxisRaw("Horizontal"); // x축 값 입력
        inputVec.y = Input.GetAxisRaw("Vertical"); // y축 값 입력
    }

    // 물리 연산 프레임마다 호출되는 생명주기 함수
    void FixedUpdate()
    {
        /* 1. 힘을 준다: AddForce(inputVec)
        2. 속도 제어: velocity
        3. 위치 이동: MovePosition(rigid.position + inputVec), 현재위치 + 이동방향 값 */

        if (!GameManager.instance.isLive)
            return;

        Vector2 nextVector = inputVec.normalized * speed * Time.fixedDeltaTime;
        rigid.MovePosition(rigid.position + nextVector); //현재 위치 + 나아가는 방향값
    }

    // InputSystem을 활용한 코드
/*    void OnMove(InputValue value)
    {
        inputVec = value.Get<Vector2>();
    }*/

    // 후의 변화 코드
    void LateUpdate()
    {
        if (!GameManager.instance.isLive)
            return;

        anim.SetFloat("Speed", inputVec.magnitude); // 두번째 인자: 반영할 speed 값

        if(inputVec.x != 0)
        {
            spriter.flipX = inputVec.x < 0;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (!GameManager.instance.isLive)
            return;

        GameManager.instance.health -= Time.deltaTime * 10;

        if (GameManager.instance.health < 0)
        {
            for (int index = 2; index < transform.childCount; index++)
            {
                transform.GetChild(index).gameObject.SetActive(false);
            }

            anim.SetTrigger("Dead");
            GameManager.instance.GameOver();
        }
    }

    public void TakeDamage(float damage)
    {
        GameManager.instance.health -= damage;
    }
}


