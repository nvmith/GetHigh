using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGun : Gun
{
    // Enemy는 Update를 쓰지 않음
    // 추 후 Gun의 하위 PlayerGun으로 분리해야 할 수 도 있음
    bool reverseCheck = false;
    protected override void OnEnable()
    {

    }

    protected override void Update()
    {

    }

    public void ShotReady(bool isReverse)
    {
        reverseCheck = isReverse;
        ShotDelay();
    }

    public void ShotReady(Vector2 pos, float angle, bool isReverse)
    {
        reverseCheck = isReverse;

        StartCoroutine(Shot(pos, angle));
    }

    public void ShotReady(Vector3 dir, Vector2 pos, int angle, bool isReverse) // 범위 각도
    {
        reverseCheck = isReverse;

        Shot(dir, angle);
    }

    public void Shot(Vector3 dir, Vector2 pos, int angle)
    {
        // 추가
        muzzle.localRotation = Quaternion.Euler(0, 0, Random.Range(-90.0f - curRecoil, -90.0f + curRecoil));
        Bullet bullet = PoolManager.Instance.GetBullet(EUsers.Enemy, EBullets.Revolver, muzzle.localRotation);
        if (reverseCheck) bullet.gameObject.transform.localScale = new Vector3(-0.2f, 0.2f, 1);
        else bullet.gameObject.transform.localScale = new Vector3(0.2f, 0.2f, 1);
        bullet.transform.position = pos;


        //Bullet bullet = PoolManager.Instance.GetBullet(EUsers.Enemy, EBullets.Revolver, Quaternion.Euler(0, 0, angle % 360));
        //bullet.transform.position = pos;
        
        bullet.MoveBullet(dir * fireSpeed);
    }

    public void KnifeShotReady(bool isReverse)
    {
        StartCoroutine(KnifeShot(isReverse));
    }
    protected IEnumerator KnifeShot(bool isReverse)
    {
        muzzle.localRotation = Quaternion.Euler(0, 0, -90.0f);

        KnifeBullet bullet = PoolManager.Instance.GetKnifeBullet(transform.rotation);
        if (isReverse) bullet.gameObject.transform.localScale = new Vector3(-1, 1, 1);
        else bullet.gameObject.transform.localScale = new Vector3(1, 1, 1);

        bullet.transform.position = muzzle.position;

        bullet.MoveBullet(muzzle.up * (fireSpeed + 5 + Random.Range(1, -1)));


        fireTime = 0;

        yield return new WaitForSeconds(0.1f);
        /*for (int i = 0; i < bulletCount; i++)
        {
            muzzleRecoil[i] = -90.0f;

            muzzle.localRotation = Quaternion.Euler(0, 0, muzzleRecoil[i]);
            muzzleRotation[i] = transform.rotation;

            KnifeBullet bullet = PoolManager.Instance.GetKnifeBullet(muzzleRotation[i]);
            if (isReverse) bullet.gameObject.transform.localScale = new Vector3(-1, 1, 1);
            else bullet.gameObject.transform.localScale = new Vector3(1, 1, 1);

            bullet.transform.position = muzzle.position;
            muzzleTransform[i] = bullet.transform.position;
            muzzleUp[i] = muzzle.up;
            bulletSpeed[i] = Random.Range(1, -1);

            bullet.MoveBullet(muzzle.up * (fireSpeed + bulletSpeed[i]));
        }

        fireTime = 0;

        yield return new WaitForSeconds(0.1f);
        //InGameManager.Instance.player.fireEffect.SetActive(false);
        */
    }


    protected override void ShotDelay()
    {
        StartCoroutine(Shot());
    }

    protected override IEnumerator Shot()
    {
        for (int i = 0; i < bulletCount; i++)
        {
            muzzleRecoil[i] = Random.Range(-90.0f - curRecoil, -90.0f + curRecoil);

            muzzle.localRotation = Quaternion.Euler(0, 0, muzzleRecoil[i]);
            muzzleRotation[i] = transform.rotation;

            Bullet bullet = PoolManager.Instance.GetBullet(users, (EBullets)weapons, muzzleRotation[i]);

            if (reverseCheck) bullet.gameObject.transform.localScale = new Vector3(-0.2f, 0.2f, 1);
            else bullet.gameObject.transform.localScale = new Vector3(0.2f, 0.2f, 1);

            bullet.transform.position = muzzle.position;
            muzzleTransform[i] = bullet.transform.position;
            muzzleUp[i] = muzzle.up;
            bulletSpeed[i] = Random.Range(1, -1);

            bullet.MoveBullet(muzzle.up * (fireSpeed + bulletSpeed[i]));
        }

        fireTime = 0;

        yield return new WaitForSeconds(0.1f);
        //InGameManager.Instance.player.fireEffect.SetActive(false);
    }

    protected virtual IEnumerator Shot(Vector2 pos, float angle)
    {
        yield return null;

        muzzle.localRotation = Quaternion.Euler(0, 0, angle);

        Bullet bullet = PoolManager.Instance.GetBullet(users, (EBullets)weapons, muzzle.localRotation);
        //Bullet bullet = PoolManager.Instance.GetBullet(users, (EBullets)weapons, transform.rotation);
        if (reverseCheck) bullet.gameObject.transform.localScale = new Vector3(-0.2f, 0.2f, 1);
        else bullet.gameObject.transform.localScale = new Vector3(0.2f, 0.2f, 1);
        //bullet.transform.position = transform.position;
        bullet.transform.position = pos;
        bullet.MoveBullet(muzzle.up * fireSpeed);
    }
}
