using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ���� ĳ���� �̸����� �̸� ����
public class Haeseong : Player
{
    public Animator playerSkillAnim;

    [SerializeField]
    private int SkillDamage = 300;

    private List<AI> agents = new List<AI>();

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    protected override void Damage(int power)
    {
        base.Damage(power);
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
    }

    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision);
    }

    protected override IEnumerator ESkill()
    {
        playerSkillAnim.SetTrigger("Skill");
        spriteRenderer.enabled = false;
        weaponPivot.SetActive(false);
        return base.ESkill();
    }

    protected override void PlayerSkill()
    {
        // �Ͻ� ���� ����
        InGameManager.Instance.Pause(true);

        //Debug.Log("�⺻ �÷��̾� ��ų");
        // ������Ʈ �ʱ�ȭ
        agents.Clear();

        if (isReload) CancleReload();

        Room playerRoom = RoomController.Instance.CurRoom();
        playerRoom.RoomAgent();

        int agentCnt = 0;
        Vector3 rangeVec = Vector3.zero;
        

        foreach(AI a in CameraController.Instance.Agents)
        {
            rangeVec = CameraController.Instance.Cam.WorldToViewportPoint(a.transform.position);
            if (rangeVec.x + (Mathf.Abs(a.transform.localScale.x) / (2 * 6.2f)) < 0 ||
                rangeVec.x - (Mathf.Abs(a.transform.localScale.x) / (2 * 6.2f)) > 1 || 
                rangeVec.y - (a.transform.localScale.y / (2 * 5.2f)) > 1 || 
                rangeVec.y + (a.transform.localScale.y / (2 * 5.2f)) < 0) continue;
            // 2�� ������, 6�� camera x�� ������ ����, 5�� y�� ������ ����
            agents.Add(a);
            agentCnt++;

            //Debug.Log("x :  " + a.transform.localScale.x);
            //Debug.Log("y /  : " + a.transform.localScale.y / (2 * 5));
        }

        foreach(AI a in agents)
        {
            a.Damage(SkillDamage / agentCnt, WeaponValue.Knife);
        }

    }
}
