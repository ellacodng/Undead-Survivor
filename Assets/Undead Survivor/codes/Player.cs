using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*using UnityEngine.InputSystem;
*/
public class Player : MonoBehaviour // ���� ���� ������ �ʿ��� �͵��� ���� Ŭ����
{
    public Vector2 inputVec; // input�� �ִ� ����
    public float speed; // �ӵ�
    public Scanner scanner;
    public Hand[] hands;
    public RuntimeAnimatorController[] animCon;

    Rigidbody2D rigid; // ���� ������Ʈ�� ������ٵ� 2D�� ������ ���� ����
    SpriteRenderer spriter;
    Animator anim;

    // ������ �� �ѹ��� ����Ǵ� �����ֱ� Awake���� �ʱ�ȭ
    void Awake()
    {
       // GetComponent<T>: ������Ʈ���� ������Ʈ�� �������� �Լ�, T�ڸ� ������Ʈ Ÿ�� �ۼ�
       rigid = GetComponent<Rigidbody2D>();
       spriter = GetComponent<SpriteRenderer>();
       anim = GetComponent<Animator>();
       scanner = GetComponent<Scanner>();
       hands = GetComponentsInChildren<Hand>(true);
    }

    private void OnEnable()
    {
        speed *= Character.Speed; // ĳ���Ϳ� ���� ���ǵ� �ٸ��� ����
        anim.runtimeAnimatorController = animCon[GameManager.instance.playerId];
    }

    // �ϳ��� �����Ӹ��� �ѹ��� ȣ��Ǵ� �����ֱ� �Լ�
    public void Update() // public ���� �� �ÿ� unity�� ���� ����
    {
        if (!GameManager.instance.isLive)
            return;

        // ���Ⱚ�� ����ؼ� ����
        // Input: ����Ƽ �������� �޴� ��� �Է��� �����ϴ� Ŭ����
        // Input Manager ���� Ű�� �̸��� ������
        // Axis ���� ������ -1 ~ 1 ��
        inputVec.x = Input.GetAxisRaw("Horizontal"); // x�� �� �Է�
        inputVec.y = Input.GetAxisRaw("Vertical"); // y�� �� �Է�
    }

    // ���� ���� �����Ӹ��� ȣ��Ǵ� �����ֱ� �Լ�
    void FixedUpdate()
    {
        /* 1. ���� �ش�: AddForce(inputVec)
        2. �ӵ� ����: velocity
        3. ��ġ �̵�: MovePosition(rigid.position + inputVec), ������ġ + �̵����� �� */

        if (!GameManager.instance.isLive)
            return;

        Vector2 nextVector = inputVec.normalized * speed * Time.fixedDeltaTime;
        rigid.MovePosition(rigid.position + nextVector); //���� ��ġ + ���ư��� ���Ⱚ
    }

    // InputSystem�� Ȱ���� �ڵ�
/*    void OnMove(InputValue value)
    {
        inputVec = value.Get<Vector2>();
    }*/

    // ���� ��ȭ �ڵ�
    void LateUpdate()
    {
        if (!GameManager.instance.isLive)
            return;

        anim.SetFloat("Speed", inputVec.magnitude); // �ι�° ����: �ݿ��� speed ��

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


