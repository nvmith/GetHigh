using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

// hp같은 정보는 어차피 싱글게임이라 게임매니저에서 관리해도 좋을듯

// 플레이어 방향 상태, 왼쪽은 오른쪽에서 뒤집기
public enum PlayerVetor
{
    Front, Cross, Side, Back
}

public enum PlayerAnimator
{
    Idle, Run, Roll
}


public abstract class Player : MonoBehaviour
{
    protected Rigidbody2D rigid;
    protected SpriteRenderer spriteRenderer;
    protected Animator anim;
    [SerializeField]
    protected SpriteRenderer silhouette;//피격 전용 실루엣

    // Status
    // Status - Basic
    public float rollingSpeed = 2.5f;

    [SerializeField]
    protected float skillDelay = 15f;
    public float SkillDelay => skillDelay;

    // Status - movement
    protected PlayerVetor curVec = PlayerVetor.Front;

    protected Vector2 inputVec;
    public Vector2 InputVec => inputVec;
    protected Vector2 moveVec;
    protected Vector2 rollVec;
    protected Vector2 nextVec;

    // Status - animation
    protected PlayerAnimator curAnim = PlayerAnimator.Idle;

    private bool isReverse = false;
    public bool IsReverse => isReverse;
    protected bool isWalk = false;
    public bool isAttack = false;
    public bool isSkill = false;
    public bool isReload = false;

    // Status - curStatus
    public int initPower = 0; // 고유 무기 공격력
    public int initMeleePower = 0;
    protected bool avoidCheck;
    public bool AvoidCheck => avoidCheck;
    public int avoidChance;

    public float reloadTime = 3.0f;

    public float speedApply; // 실제 스피드
    public float speed;
    public bool rollCnt = false; // 구른후 빨라지는 버프 체크

    protected bool isHit = false; // 피격당함
    public bool IsHit => isHit;
    protected bool isDead;
    protected bool isRoll = false;
    public bool IsRoll => isRoll;
    protected bool rollReverse = false; // 애니메이션 대칭

    // InputKey
    protected bool attackKey;
    public bool AttackKey => attackKey;
    protected bool skillKey;
    protected bool rollKey;
    protected bool swapKey1;
    protected bool swapKey2;
    protected bool swapKey3;
    protected bool swapKey4;
    protected bool interaction;
    protected bool qKey;
    protected bool shiftKey;
    protected bool reLoadKey;

    // MoveSound
    protected bool moveSoundCheck = true; //참이면 소리 활성화

    // Weapon
    public int tempWeaponIndex = 2;
    public int gunCnt = 0; // 파랑 마약 전용 인덱스 카운팅
    public bool mainGunCheck = false; // 주무기 총 보유 여부 체크
    public int gunValue = -1; // 0은 주무기, 1은 보조무기
    public bool gunCheck = false; // 현재 총을들었는지 체크

    public GameObject[] mainWeapon; // 라이플, 샷건, 스나 순 -> 추후 무기가 추가된다면 권총처럼 총기별로 분류하거나 그대로 추가
    public Weapons tempGun; // 교체용 변수

    public GameObject[] subWeapon; // 이글, 아나콘다 순
    public BasicWeapon basicWeapon;
    [SerializeField]
    protected GameObject weaponPivot;
    public SpriteRenderer hadnSprite;
    public RectTransform playerCanvas; // 추후 다른 UI도 추가되어야한다면 slider로 교체
    public Slider reloadSlider;
    public Coroutine reloadCoroutine;
    public int weaponPower = 0; // 현재 장착한 무기의 공격력(단순 UI표기용), 초기값은 플레이어 별 무기 공격력
    public int reloadIndex = -1; // 장전할 무기의 인덱스

    [SerializeField]
    public GameObject fireEffect;

    public Image[] RageImages;

    public bool fogIn = false; //안개 안에 있는지 여부
    public float skillAnimTime = 0;

    public Animator dieAnim;

    protected virtual void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    protected virtual void Start()
    {
        speedApply = InGameManager.Instance.Speed;
        speed = speedApply;
    }


    protected virtual void Update()
    {
        if (InGameManager.Instance.IsPause || isDead) return;

        InputKey();
        StartCoroutine(MoveSound());
        if (DrugManager.Instance.isCrazy) inputVec *= -1;

        VectorStatus(curVec);
        Roll();
        Swap();
        Skill();
        Reload();
        Interaction();
        EatDrug();
        UserGenade();
    }

    protected virtual void FixedUpdate()
    {
        if (InGameManager.Instance.IsPause || isDead) return;

        Move();
    }

    protected void InputKey()
    {
        if (isSkill) return;

        inputVec.x = Input.GetAxisRaw("Horizontal");
        inputVec.y = Input.GetAxisRaw("Vertical");
        attackKey = Input.GetButton("Fire1");
        skillKey = Input.GetButtonDown("Jump");
        rollKey = Input.GetButton("Fire2");
        swapKey1 = Input.GetButtonDown("Swap1");
        swapKey2 = Input.GetButtonDown("Swap2");
        swapKey3 = Input.GetButtonDown("Swap3");
        swapKey4 = Input.GetButtonDown("Swap4");
        interaction = Input.GetButtonDown("Interaction");
        reLoadKey = Input.GetButtonDown("Reload");
        qKey = Input.GetKeyDown(KeyCode.Q);
        shiftKey = Input.GetKey(KeyCode.LeftShift);
    }

    protected void Move()
    {
        // 만약 inputVec 0이면 Idle, 아니면 Run으로 체인지
        if (isSkill) return;

        moveVec = inputVec.normalized;

        if(isAttack) InGameManager.Instance.basicWeaponPivot.transform.position = weaponPivot.transform.position;

        if (isRoll)
        {
            nextVec = rollVec.normalized * rollingSpeed * Time.fixedDeltaTime;
        }
        else
        {
            isWalk = moveVec != Vector2.zero ? true : false;
            nextVec = moveVec.normalized * Time.fixedDeltaTime * speed;
            anim.SetBool("Walk", isWalk);
        }

        if (DrugManager.Instance.green3) nextVec *= 1.25f;

        rigid.MovePosition(rigid.position + nextVec);
    }

    /*public IEnumerator MoveSound()
    {
        if (isWalk && moveSoundCheck && moveVec != null)
        {
            moveSoundCheck = false;
            SoundManager.Instance.PlaySFX(SFX.Walk);
            yield return new WaitForSeconds(0.75f);
            moveSoundCheck = true;
        }
    }*/

    public IEnumerator MoveSound()
    {
        if (moveSoundCheck)
        {
            moveSoundCheck = false;

            if (!isWalk) SoundManager.Instance.StopPutSound();
            else SoundManager.Instance.PlayPutSound();

            yield return new WaitForSeconds(0.5f);
            moveSoundCheck = true;
        }
    }

    protected void Roll()
    {
        if (isSkill || DrugManager.Instance.isRollBan) return;

        if (!isRoll && InputVec != Vector2.zero && rollKey && !isAttack)
        {
            rollVec = InputVec;
            StartCoroutine(IRoll());
        }
    }

    private IEnumerator IRoll()
    {
        if (isReload) CancleReload();

        string rollStatus = null;

        rollReverse = false;
        weaponPivot.SetActive(false);
        isRoll = true;

        // 구르기 방향 계산
        if (rollVec.x > 0)
        {
            if (rollVec.y > 0) rollStatus = "RollCross";
            else if (rollVec.y == 0) rollStatus = "RollSide";
            else rollStatus = "RollSide";
        }
        else if(rollVec.x ==0)
        {
            if (rollVec.y > 0) rollStatus = "RollBack";
            else if(rollVec.y < 0) rollStatus = "RollFront";
        }
        else
        {
            rollReverse = true;
            if (rollVec.y > 0) rollStatus = "RollCross";
            else if (rollVec.y == 0) rollStatus = "RollSide";
            else rollStatus = "RollSide";
        }

        if (rollReverse) transform.localScale = new Vector3(-1, 1, 1);
        else transform.localScale = new Vector3(1, 1, 1);

        rollingSpeed *= 2;

        SoundManager.Instance.PlaySFX(SFX.Roll);
        anim.SetTrigger("Roll");
        anim.SetTrigger(rollStatus);

        avoidCheck = true;
      
        yield return new WaitForSeconds(0.7f);

        avoidCheck = false;
        nextVec = Vector2.zero;

        yield return new WaitForSeconds(0.3f);

        weaponPivot.SetActive(true);
        isRoll = false;
        rollingSpeed *= 0.5f;

        if (!rollCnt && DrugManager.Instance.isRollSpeedUp)
        {
            speed *= 1.5f;
            rollCnt = true;
            yield return new WaitForSeconds(2.5f);
            if(!isReload) speed = speedApply;
            rollCnt = false;
        }
    }

    public void RollSpeedCheck(bool check)
    {
        if(check)
        {
            speed *= 1.5f;
            rollCnt = true;
        }
        else if(check != rollCnt)
        {
            speed = speedApply;
            rollCnt = false;
        }
    }

    public bool AttackCheck()
    {
        return attackKey && !isAttack && !isRoll && !isSkill;
    }

    public void Hit(int power) // 외부에서 사용가능하도록
    {
        Damage(power);
    }

    protected virtual void Damage(int power)
    {
        if (avoidCheck || isHit) return;
        else if(DrugManager.Instance.isBulletAvoid)
        {
            Debug.Log("로직 실행");

            avoidChance = Random.Range(1, 101);
            if (avoidChance <= 25)
            {
                Debug.Log("회피 적용");
                return;
            }
        }
        
        if(UIManager.Instance.isBulletProof)
        {
            UIManager.Instance.BulletproofUpdate(false);

            return;
        }

        if (DrugManager.Instance.doubleDamageCheck) power *= 2;

        InGameManager.Instance.Hit(power);
        UIManager.Instance.hpUpdate();

        if(InGameManager.Instance.IsDead)
        {
            isDead = true;
            return;
        }
        else
        {
            isHit = true;
            StartCoroutine(HitTime());
        }

        // 광전사 여부 체크
        if (DrugManager.Instance.red2)
        {
            DrugManager.Instance.RunRedBuff2();
        }
    }

    private IEnumerator HitTime()
    {
        int i = 0;
        for(i =0; i<3; i++)  // 추후 실루엣 스프라이트로 변경
        {
            spriteRenderer.color = Color.red;
            yield return new WaitForSeconds(0.4f);
            spriteRenderer.color = Color.white;
            yield return new WaitForSeconds(0.1f);
        }

        yield return null;
        isHit = false;
    }

    public void KnifeAttack(bool check)
    {
        InGameManager.Instance.basicWeaponPivot.transform.localScale = new Vector3(1, 1, 1);

        if (isReverse)
        {
            InGameManager.Instance.basicWeaponPivot.transform.localScale = new Vector3(-1, 1, 1);
        }

        if (check)
        {
            InGameManager.Instance.basicWeaponPivot.transform.position = weaponPivot.transform.position;
            InGameManager.Instance.basicWeaponPivot.transform.rotation = weaponPivot.transform.rotation;
            //InGameManager.Instance.knifeEffect.transform.rotation = weaponPivot.transform.rotation;
        }
    }

    
    protected void Reload()
    {
        if (!gunCheck || isSkill || isRoll || isReload) return;
        if (!reLoadKey) return;
        if (!InGameManager.Instance.CheckReload(gunValue)) return;

        reloadCoroutine = StartCoroutine(IReload());
    }

    protected IEnumerator IReload()
    {
        isReload = true;
        speed = speedApply * 0.5f; //장전중 이속 저하

        reloadSlider.value = 0f;
        float elapsed = 0f;
        float curReloadTime = DrugManager.Instance.reloadSpeed * reloadTime;
        reloadSlider.gameObject.SetActive(true);

        //Debug.Log("값 : " + (SFX)reloadIndex);
        SoundManager.Instance.PlayReload(reloadIndex);
        while (elapsed < curReloadTime)
        {
            elapsed += Time.unscaledDeltaTime;
            reloadSlider.value = Mathf.Clamp01(elapsed / curReloadTime);
            yield return null;
        }

        reloadSlider.value = 1f;
        reloadSlider.gameObject.SetActive(false);

        InGameManager.Instance.ReloadBullet(gunValue);

        if(gunValue == 1)
        UIManager.Instance.inGameUI.BulletTextInput(InGameManager.Instance.bulletMagazine[3],
                InGameManager.Instance.bulletMagazine[3]);
        else
            UIManager.Instance.inGameUI.BulletTextInput(
                InGameManager.Instance.bulletMagazine[InGameManager.Instance.curWeaponIndex],
                InGameManager.Instance.bulletMagazine[InGameManager.Instance.curWeaponIndex]);

        isReload = false;
        speed = speedApply; // 스피드 원상 복귀
    }

    public void CancleReload()
    {
        StopCoroutine(reloadCoroutine);
        SoundManager.Instance.StopReload();
        reloadSlider.value = 0f;
        reloadSlider.gameObject.SetActive(false);
        isReload = false;
        speed = speedApply;

        reloadCoroutine = null;
    }


    protected void Swap()
    {
        if (isSkill || isRoll || isAttack) return;

        if (!swapKey1 && !swapKey2 && !swapKey3 && attackKey) return;

        if (swapKey1 && InGameManager.Instance.gunInven != null)
        {
            if (DrugManager.Instance.isManyWeapon && tempWeaponIndex == 0) mainGunCheck = true;
            else if (tempWeaponIndex == 0) return;
            tempWeaponIndex = 0;
        }
        else if (swapKey2 && InGameManager.Instance.pistolInven != null && tempWeaponIndex != 1)
            tempWeaponIndex = 1;
        else if (swapKey3 && tempWeaponIndex != 2)
            tempWeaponIndex = 2;
        else return;

        InGameManager.Instance.lastWeaponIndex = InGameManager.Instance.curWeaponIndex;
        InGameManager.Instance.lastPistolIndex = InGameManager.Instance.curPistolIndex;

        WeaponSwap(tempWeaponIndex);
    }

    // 수정 로직
    public void WeaponSwap(int idx)//, int weaponValue)
    {
        if (isReload) CancleReload();

        Debug.Log("idx : " + idx);

        // 무기를 전부 비활성화, 비용이 많이 든다면 추후 활성화 무기만 체크하여 비활성화로 돌리는 로직으로 수정
        if (InGameManager.Instance.lastWeaponIndex == 4)
        {
            basicWeapon.CancleAttack();
        }
        else if (InGameManager.Instance.lastWeaponIndex == 3)
        {

            InGameManager.Instance.PutBullet(EWeapons.Revolver);
            InGameManager.Instance.pistolInven.gameObject.SetActive(false);
            subWeapon[InGameManager.Instance.lastPistolIndex].SetActive(false);
        }
        else
        {
            InGameManager.Instance.PutBullet(InGameManager.Instance.gunInven.eWeapons);
            InGameManager.Instance.gunInven.gameObject.SetActive(false);
            mainWeapon[InGameManager.Instance.lastWeaponIndex].SetActive(false);
        }

        int temp;

        // 해당 무기만 활성화
        if (idx == 0)
        {
            // 주무기가 활성화 된 상태에서 주무기로 교체할 때
            if (mainGunCheck && InGameManager.Instance.blueGunInven != null)
            {
                // 현재 주무기와 마약 주무기 인벤 교체
                tempGun = InGameManager.Instance.gunInven;
                InGameManager.Instance.gunInven = InGameManager.Instance.blueGunInven;
                InGameManager.Instance.blueGunInven = tempGun;
                mainGunCheck = false;

                Debug.Log("주무기 끼리 교체");
            }

            InGameManager.Instance.GetBullet(InGameManager.Instance.gunInven.eWeapons,
                    InGameManager.Instance.gunInven.bulletCount); // 교체 후 현재 총알 등록

            temp = InGameManager.Instance.gunInven.index;
            Debug.Log("주무기 temp : " + temp);

            mainWeapon[temp].SetActive(true);
            UIManager.Instance.inGameUI.WeaponInven(temp);
            UIManager.Instance.inGameUI.BulletTextInput(InGameManager.Instance.gunInven.bulletCount,
                InGameManager.Instance.bulletMagazine[temp]);
            InGameManager.Instance.curWeaponIndex = temp;

            reloadIndex = (int)SFX.AK_Reload + InGameManager.Instance.gunInven.index;
            reloadTime = InGameManager.Instance.gunInven.reloadSpeed;
            gunValue = 0;
            gunCheck = true;
            weaponPower = InGameManager.Instance.gunInven.power;
        }
        else if (idx == 1)
        {
            InGameManager.Instance.GetBullet(InGameManager.Instance.pistolInven.eWeapons,
                    InGameManager.Instance.pistolInven.bulletCount); // 교체 후 현재 총알 등록

            temp = InGameManager.Instance.pistolInven.index;
            Debug.Log("보조무기 temp : " + temp);

            subWeapon[temp].SetActive(true);
            UIManager.Instance.inGameUI.WeaponInven(temp + 3);
            UIManager.Instance.inGameUI.BulletTextInput(InGameManager.Instance.pistolInven.bulletCount,
                InGameManager.Instance.bulletMagazine[3]);
            InGameManager.Instance.curWeaponIndex = 3;
            InGameManager.Instance.curPistolIndex = temp;

            reloadIndex = (int)SFX.Eagle_Reload + InGameManager.Instance.pistolInven.index;
            reloadTime = InGameManager.Instance.pistolInven.reloadSpeed;
            gunValue = 1;
            gunCheck = true;
            weaponPower = InGameManager.Instance.pistolInven.power;
        }
        else
        {
            InGameManager.Instance.lastWeaponIndex = InGameManager.Instance.curWeaponIndex;
            InGameManager.Instance.curWeaponIndex = 4;

            basicWeapon.gameObject.SetActive(true);
            UIManager.Instance.inGameUI.WeaponInven(5 + InGameManager.Instance.playerWeaponType);
            UIManager.Instance.inGameUI.KnifeTextUpdate();
            gunCheck = false;
            weaponPower = initMeleePower;
        }

        tempWeaponIndex = idx;
    }

    protected void Interaction()
    {
        if (interaction)
        {
            InGameManager.Instance.ItemUse();
        }
        // 수정 전
        /*
        if(interaction)
        {
            InGameManager.Instance.ItemUse();
            Debug.Log("이 함수가 문제인가? 2");
        }*/
    }
    protected void EatDrug()
    {
        if (qKey)
        {
            // 인벤 마약
            if (shiftKey && InGameManager.Instance.drugInven != null)
            {
                InGameManager.Instance.drugInven.UseItem();
                InGameManager.Instance.drugInven = null;
                UIManager.Instance.inGameUI.DrugInven(null);
            }
            // 땅에있는 마약
            /*else if (InGameManager.Instance.tempDrug.Count != 0 && !shiftKey)
            {
                InGameManager.Instance.tempDrug
                    [InGameManager.Instance.tempDrugIndex].UseItem();
            }*/
            else if (InGameManager.Instance.tempDrug.Count != 0 && !shiftKey)
            {
                InGameManager.Instance.DrugUse();
            }
        }
    }

    protected void UserGenade()
    {
        if(swapKey4)
        {
            InGameManager.Instance.UseGrenade();
        }
    }

    // 방향 전환
    public void ChangeVector(PlayerVetor pVec, bool checkReverse)
    {
        if (isAttack) return;

        isReverse = checkReverse;
        VectorStatus(pVec);
    }

    protected void VectorStatus(PlayerVetor pVec)
    {
        if (isRoll) return;

        if (isReverse && !InGameManager.Instance.IsPause)
        {
            transform.localScale = new Vector3(-1, 1, 1);
            playerCanvas.localScale = new Vector3(-1, 1, 1);
        }
        else
        {
            transform.localScale = new Vector3(1, 1, 1);
            playerCanvas.localScale = new Vector3(1, 1, 1);
        }

        if (pVec != curVec)
            AnimationChange(pVec);
        curVec = pVec;
    }

    public void AnimStatus(PlayerAnimator pAnim)
    {
        curAnim = pAnim;
    }

    public void AnimationChange(PlayerVetor pVec)
    {
        for (int i = 0; i < 4; i++)
        {
            PlayerVetor p = (PlayerVetor)i;
            anim.SetBool(p.ToString(), false);
        }

        anim.SetBool("Walk", false);

        anim.SetBool(pVec.ToString(), true);
    }

    protected void Skill()
    {
        if (isAttack || isSkill || !skillKey || !UIManager.Instance.inGameUI.skillUI.CanUseSkill || isRoll) return;

        isSkill = true;
        StartCoroutine(ESkill());
    }
    
    // 스킬 시작과 끝을 계산
    protected virtual IEnumerator ESkill()
    {
        yield return null;
        // 카메라 고정 및 위치 이동
        CameraController.Instance.CameraActive(false);
        CameraController.Instance.CameraPos(transform.position.x, transform.position.y);
        //playerSkillAnim.SetTrigger("Skill");

        // 일시 동작 정지
        //InGameManager.Instance.Pause(true);

        UIManager.Instance.inGameUI.skillUI.UseSkill();
        PlayerSkill();

        yield return new WaitForSecondsRealtime(skillAnimTime); // 타임스케일 영향 x
        InGameManager.Instance.Pause(false);
        CameraController.Instance.CameraActive(true);
        spriteRenderer.enabled = true;
        weaponPivot.SetActive(true);
        isSkill = false;
    }

    protected abstract void PlayerSkill();

    public void RageUpdate(int cnt)
    {
        for (int i = 0; i < 5; i++)
            RageImages[i].color = Color.white;

        for (int i = 0; i < cnt; i++)
        {
            RageImages[i].color = new Color(0.88f, 0.33f, 0, 1);
        }
    }

    public void DisableRenderer()
    {
        spriteRenderer.enabled = false;
        weaponPivot.SetActive(false);
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "EnemyBullet") //수정
        {
            Damage(1); // 데미지 로직 나중에 수정
        }
    }

    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Boss")
        {
            if (!InGameManager.Instance.bossRushCheck) return;

            Damage(1); // 데미지 로직 나중에 수정
        }
    }
}
