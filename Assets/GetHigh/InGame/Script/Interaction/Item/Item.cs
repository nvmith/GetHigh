using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public enum EActiveItems
{
    Band, Key, Bulletproof, Grenade,
    None
}
// 순서 중요
// 붕대, 열쇠, 방탄, 수류탄, 돈 만사용함 나머진 사용x

public abstract class Item : MonoBehaviour
{
    [SerializeField]
    protected EActiveItems itemValues;
    public int price = 0;
    public bool isProduct = false;
    public int curPrice = 0;
    public bool playerCheck = false; // 플레이어와 충돌여부 체크
    public float distance = 999f; // 플레이어와 아이템의 거리
    public int shopIndex;
    public Shop shop;

    public TextMeshPro priceText;
    public SpriteRenderer spriteRenderer;
    public TextMeshPro eText;
    public SpriteRenderer moneyRenderer;

    protected Rigidbody2D itemRigid;

    public bool silhouetteActive = false; // 부하를 조금이라도 줄이기 위한 변수.

    protected virtual void Awake()
    {
        itemRigid = GetComponent<Rigidbody2D>();
        curPrice = price;
    }

    protected virtual void OnEnable()
    {
        SilhouetteCheck(false);
    }

    protected virtual void Start()
    {
        ItemUIPlay(false);
    }

    protected virtual void Update()
    {
        if(playerCheck)
        {
            distance = Vector2.Distance
                (transform.position, InGameManager.Instance.player.transform.position);
        }
    }


    public virtual void GetItem()
    {
        gameObject.SetActive(false);
    }

    public abstract void UseItem();

    public void ThrowItem(Vector2 pos) // 드랍 아이템 던질 방향 조절
    {
        transform.position = pos;

        float angle = Random.Range(0, 2 * Mathf.PI);
        Vector2 vec = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));

        itemRigid.AddForce(vec.normalized, ForceMode2D.Impulse);
    }

    public void ShopItem(Vector2 pos, Shop s)
    {
        isProduct = true;

        shop = s;

        transform.position = pos;

        curPrice = price;

        priceText.text = curPrice.ToString();
    }

    public void ItemUIPlay(bool check)
    {
        if (DrugManager.Instance.hostHateCheck)
        {
            curPrice = (price * 6) / 5;
            priceText.text = curPrice.ToString();
        }

        priceText.enabled = check;
        eText.enabled = check;
        moneyRenderer.enabled = check;
    }

    public void SilhouetteCheck(bool check)
    {
        if (check)
        {
            if (silhouetteActive) return;
            spriteRenderer.color = Color.white;
            spriteRenderer.material = InGameManager.Instance.material;
            silhouetteActive = true;
        }
        else
        {
            spriteRenderer.color = new Color(1, 1, 1, 0);
            silhouetteActive = false;
        }
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (isProduct) ItemUIPlay(true);

            InGameManager.Instance.tempItems.Add(this);
            playerCheck = true;
            //Debug.Log("리스트 체크(추가) : " + InGameManager.Instance.tempItems.Count);
        }
        else if (collision.tag == "Wall" || collision.tag == "MapObject")
        {
            itemRigid.velocity = Vector3.zero;
        }
    }

    protected virtual void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (isProduct) ItemUIPlay(false);

            SilhouetteCheck(false);
            InGameManager.Instance.tempItems.Remove(this);
            //Debug.Log("지금 먹은 아이템 : " + gameObject.name);
            distance = 999f;
            playerCheck = false;
            //Debug.Log("리스트 체크(삭제) : " + InGameManager.Instance.tempItems.Count);
        }
    }
}
