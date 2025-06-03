using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public enum EActiveItems
{
    Band, Key, Bulletproof, Grenade,
    None
}
// ���� �߿�
// �ش�, ����, ��ź, ����ź, �� ������� ������ ���x

public abstract class Item : MonoBehaviour
{
    [SerializeField]
    protected EActiveItems itemValues;
    public int price = 0;
    public bool isProduct = false;
    public int curPrice = 0;
    public bool playerCheck = false; // �÷��̾�� �浹���� üũ
    public float distance = 999f; // �÷��̾�� �������� �Ÿ�
    public int shopIndex;
    public Shop shop;

    public TextMeshPro priceText;
    public SpriteRenderer spriteRenderer;
    public TextMeshPro eText;
    public SpriteRenderer moneyRenderer;

    protected Rigidbody2D itemRigid;

    public bool silhouetteActive = false; // ���ϸ� �����̶� ���̱� ���� ����.

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

    public void ThrowItem(Vector2 pos) // ��� ������ ���� ���� ����
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
            //Debug.Log("����Ʈ üũ(�߰�) : " + InGameManager.Instance.tempItems.Count);
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
            //Debug.Log("���� ���� ������ : " + gameObject.name);
            distance = 999f;
            playerCheck = false;
            //Debug.Log("����Ʈ üũ(����) : " + InGameManager.Instance.tempItems.Count);
        }
    }
}
