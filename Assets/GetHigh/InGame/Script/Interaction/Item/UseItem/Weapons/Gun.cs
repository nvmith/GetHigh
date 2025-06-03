using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public enum EWeapons
{
    Rifle, Shotgun, Sniper, Revolver
}

public abstract class Gun : MonoBehaviour
{
    [SerializeField]
    protected EWeapons weapons;
    [SerializeField]
    protected EUsers users;
    
    public Transform fireEffectPos;
    public Transform muzzle; 
	public float fireSpeed = 10.0f;
    public float fireDelay = 3.0f;
    public float fireMaximumDelay = 0.2f;
    public float recoil = 0;
	public int bulletCount = 1;
    [SerializeField]
    protected float curRecoil = 0;

    protected float fireTime = 0;

    protected float[] muzzleRecoil;
    protected Vector3[] muzzleTransform;
    protected Quaternion[] muzzleRotation;
    protected Vector3[] muzzleUp;
    protected float[] powerSpeed;
    protected float[] bulletSpeed;


    protected virtual void OnEnable()
    {
        if(!InGameManager.Instance.player.IsRoll)
            fireTime = 0; // 롤 할때도 이렇게되니 그걸 체크할 방법이 필요함
    }


    protected void Start()
    {
        fireTime = fireDelay;
        muzzleRecoil = new float[bulletCount];
        muzzleTransform = new Vector3[bulletCount];
        muzzleRotation = new Quaternion[bulletCount];
        muzzleUp = new Vector3[bulletCount];
        powerSpeed = new float[bulletCount];
        bulletSpeed = new float[bulletCount];
    }

    // Update is called once per frame
    protected virtual void Update()
    {
		fireTime += Time.deltaTime;

		if (InGameManager.Instance.player.AttackCheck())
        {
            if(InGameManager.Instance.player.isReload)
            {
                InGameManager.Instance.player.CancleReload();
            }      
            if (InGameManager.Instance.curBullet[(int)weapons] <= 0) return;

			ShotDelay();
		}
    }



    // 일단 라이플이라 생각하고 작업 중
    protected abstract void ShotDelay();

    protected virtual IEnumerator Shot()
    {
        InGameManager.Instance.player.isAttack = true;

        if (recoil < InGameManager.Instance.Aim + DrugManager.Instance.aim)
            curRecoil = 0;
        else curRecoil = recoil - (InGameManager.Instance.Aim + DrugManager.Instance.aim);
        
        int value = (int)SFX.AK_Shot;
        if (InGameManager.Instance.curWeaponIndex < 3) value += InGameManager.Instance.gunInven.index;
        else if (InGameManager.Instance.curWeaponIndex == 3) value += 3 + InGameManager.Instance.pistolInven.index;
        else value += 4;

        //int value = (int)SFX.AK_Shot + InGameManager.Instance.curWeaponIndex;
        SoundManager.Instance.PlaySFX((SFX)value);
		for(int i=0;i<bulletCount; i++)
		{
            muzzleRecoil[i] = Random.Range(-90.0f - curRecoil, -90.0f + curRecoil);

            muzzle.localRotation = Quaternion.Euler(0, 0, muzzleRecoil[i]);
            muzzleRotation[i] = transform.rotation;

            Bullet bullet = PoolManager.Instance.GetBullet(users, (EBullets)weapons, muzzleRotation[i]);
            bullet.transform.position = muzzle.position;
            muzzleTransform[i] = bullet.transform.position;
            muzzleUp[i] = muzzle.up;
            bulletSpeed[i] = Random.Range(1, -1);

            bullet.MoveBullet(muzzle.up * (fireSpeed + bulletSpeed[i]));
		}

        fireTime = 0;
        UIManager.Instance.inGameUI.BulletTextUpdate(--InGameManager.Instance.curBullet[(int)weapons]);
        // UI수정

        SoundManager.Instance.PlaySFX((SFX)value);
        yield return new WaitForSeconds(0.1f);

        if (DrugManager.Instance.islucianPassive)
        {
            for (int i = 0; i < bulletCount; i++)
            {
                muzzle.localRotation = muzzleRotation[i];

                Bullet bullet = PoolManager.Instance.GetBullet(users, (EBullets)weapons, muzzleRotation[i]);
                bullet.transform.position = muzzleTransform[i];

                bullet.MoveBullet(muzzleUp[i] * (fireSpeed + bulletSpeed[i]));

                /*
                muzzle.localRotation = Quaternion.Euler(0, 0, muzzleRecoil[i]);

                GameObject fireBullet = Instantiate(bullet, muzzleTransform[i], muzzleRotation[i]);
                Rigidbody2D rb = fireBullet.GetComponent<Rigidbody2D>();
                rb.AddForce(muzzleUp[i] * powerSpeed[i], ForceMode2D.Impulse);
                */
            }
        }

        InGameManager.Instance.player.isAttack = false;
        yield return new WaitForSeconds(0.1f);
        InGameManager.Instance.player.fireEffect.SetActive(false);
	}
}
