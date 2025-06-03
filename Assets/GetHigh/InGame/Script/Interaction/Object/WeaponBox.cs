using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class WeaponBox : MonoBehaviour
{
    // Start is called before the first frame update

    bool openKey;
    bool touchBox;
    public bool isLock;
    public bool isOpen = false;

    public Item[] guns;

    Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!touchBox || animator.enabled) return;

        OpenInputKey();
        OpenBox();
    }

    void OpenInputKey()
    {
        openKey = Input.GetKeyDown(KeyCode.E);
    }

    void OpenBox()
    {
        if (!openKey) return;
        if (isOpen) return;

        Debug.Log(InGameManager.Instance.key);
        if (isLock)
        {
            if (InGameManager.Instance.key > 0)
            {
                InGameManager.Instance.UpdateKey(-1);
                SoundManager.Instance.PlaySFX(SFX.UseKey);
            }
            else
                return;
        }

        isOpen = true;
        StartCoroutine(DropWeapon());
        Debug.Log("상자열기");
    }
    IEnumerator DropWeapon()
    {
        yield return new WaitForSeconds(0.35f);
        animator.enabled = true;

        yield return new WaitForSeconds(0.4f);

        SoundManager.Instance.PlaySFX(SFX.Box_Open);
        int value = Random.Range(1, 51);
        Debug.Log(value);
        int index = -1;
        if (value <= 10)
        {
            index = 0;
        }
        else if (value <= 20)
        {
            index = 1;
        }
        else if (value <= 30)
        {
            index = 2;
        }
        else if (value <= 40)
        {
            index = 3;
        }
        else if (value <= 50)
        {
            index = 4;
        }

        Instantiate(guns[index], transform.position, transform.rotation).GetComponent<Weapons>().PutWeapon(0.1f);
    }


    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            touchBox = true;
        }
    }

    public void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            touchBox = false;
        }
    }
}


