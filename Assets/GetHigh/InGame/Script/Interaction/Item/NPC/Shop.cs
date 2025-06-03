using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{

    [SerializeField]
    GameObject[] guns; //3개
    [SerializeField]
    GameObject[] glocks; //2개
    [SerializeField]

    Transform[] itemPos; //5개
    [SerializeField]
    Transform[] drugPos; //5개

    public GameObject[] itemSoldout;
    public GameObject[] drugSoldout;

    int curIndex = 0;

    void Start()
    {
        ItemSpawn();

        for (int i = 0; i < drugPos.Length; i++)
        {
            Drug j = PoolManager.Instance.GetDrug((EDrugColor)i);
            j.ShopItem(drugPos[i].position, this);
            j.shopIndex = i;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
    void ItemSpawn()
    {
        int isGunSpawn = Random.Range(0, 6);
        //Debug.Log("랜덤시드: " + isGunSpawn);

        if (isGunSpawn == 3)
        {
            Item i = Instantiate(glocks[Random.Range(0, glocks.Length)]).GetComponent<Item>();
            i.ShopItem(itemPos[curIndex].position, this);
            i.shopIndex = curIndex++; // 무기일땐 +5를 무조건 해줄것
        }
        else if (isGunSpawn == 4)
        {
            Item i = Instantiate(guns[Random.Range(0, guns.Length)]).GetComponent<Item>();
            i.ShopItem(itemPos[curIndex].position, this);
            i.shopIndex = curIndex++;

        }
        else if (isGunSpawn == 5)
        {
            Item i = Instantiate(glocks[Random.Range(0, glocks.Length)]).GetComponent<Item>();
            i.ShopItem(itemPos[curIndex].position, this);
            i.shopIndex = curIndex++;
            Item j = Instantiate(guns[Random.Range(0, guns.Length)]).GetComponent<Item>();
            j.ShopItem(itemPos[curIndex].position, this);
            j.shopIndex = curIndex++;
        }

        for (int i=curIndex; i<itemPos.Length;i++)
        {
            int itemRand = Random.Range(0, 6);

            //Debug.Log("값 : "+curIndex);
            if (itemRand < 2)
            {
                Item m = PoolManager.Instance.GetMagazine(itemRand);
                m.ShopItem(itemPos[curIndex].position, this);
                m.shopIndex = curIndex++;

            }
            else
            {
                Item m = PoolManager.Instance.GetActiveItem((EActiveItems)(itemRand - 2));
                m.ShopItem(itemPos[curIndex].position, this);
                m.shopIndex = curIndex++;
            }
        }
    }

    public void ItemSoldout(int index)
    {
        SoundManager.Instance.PlaySFX(SFX.SoldOut);
        itemSoldout[index].SetActive(true);
    }

    public void DrugSoldout(int index)
    {
        SoundManager.Instance.PlaySFX(SFX.SoldOut);
        drugSoldout[index].SetActive(true);
    }
}
