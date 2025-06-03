using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public bool isOpened = false;
    public bool firstOpenCheck = false;
    public bool lineDrawn = false;

    private GameObject qMark;
    private Animator animator;
    public BoxCollider2D boxCol;
    private GameObject sideCol;
    private GameObject lineObject;
    private LineRenderer lineRenderer;
    public Material lineMat;

    public Transform[] wayPoints;

    bool isSide = false;
    float dist;


    [SerializeField]
    private Door nextDoor;

    private void Awake()
    {
		qMark = transform.GetChild(0).gameObject;

		animator = GetComponent<Animator>();
        boxCol = GetComponent<BoxCollider2D>();

        if (transform.localScale.y != 1) //사이드 체크
        {
            isSide = true;
            sideCol = transform.GetChild(1).gameObject;

            qMark.transform.localPosition = new Vector3(0, -1.2413f, 0);
            qMark.transform.localScale = new Vector3(2, 1.379f, 1);
        }
	}

	private void Start()
	{
		LineInit();
	}

	private void LineInit()
    {
        lineObject = new GameObject("LineRendererObject");
        lineObject.transform.parent = this.transform;

        lineRenderer = lineObject.AddComponent<LineRenderer>();

        lineRenderer.material = lineMat;
		lineRenderer.sortingOrder = 3;
        lineObject.layer = 6;
    }

    public void DrawLine()
    {
        if (!nextDoor.lineDrawn)
            DrawLineBetweenDoors(qMark.transform.position, nextDoor.qMark.transform.position, wayPoints);
	}
    public void nextQMOn()
    {
        if (!firstOpenCheck)
        {
            nextDoor.qMark.SetActive(true);
		}
    }
    
    public void nextClearCheck()
    {
		nextDoor.firstOpenCheck = true;
		DrawLine();
	}

    public void QMOff()
    {
        qMark.SetActive(false);
    }

    public void DoorLock()
    {
        boxCol.isTrigger = false;
        DoorClose();
    }

    public void DoorUnlock()
    {
        boxCol.isTrigger = true;
    }

    IEnumerator DoorReverse()
    {
        yield return new WaitForSeconds(0.3f);
        transform.localScale += new Vector3(2, 0, 0);
        qMark.transform.localScale = new Vector3(2, 1.379f, 1);
    }


    public void DoorClose()
    {
        if (!isOpened) return;

        if (isSide) sideCol.SetActive(false);
        animator.SetTrigger("Close");
        isOpened = false;

        if (transform.localScale.x < 0)
            StartCoroutine(DoorReverse());
    }

    void DoorOpen()
    {
        SoundManager.Instance.PlaySFX(SFX.Door_Open);

        if (isSide)
        {
            if (dist < 0)
            {
                transform.localScale -= new Vector3(2, 0, 0);
				qMark.transform.localScale = new Vector3(-2, 1.379f, 1);

			}

            animator.SetTrigger("Side");

            sideCol.SetActive(true);
        }
        else
        {
            if (dist > 0)
            {
                animator.SetTrigger("Up");
            }
            else
            {
                animator.SetTrigger("Down");
            }
        }

        isOpened = true;
        firstOpenCheck = true;

    }

    public void Doorcol(bool check)
    {
        boxCol.enabled = check;
    }

    public void CheckDoor()
    {
        if (isSide) dist = (transform.position.x - InGameManager.Instance.player.transform.position.x);
        else dist = (transform.position.y - InGameManager.Instance.player.transform.position.y);

        DoorOpen();
    }

	private void DrawLineBetweenDoors(Vector2 vec1, Vector2 vec2, Transform[] wayPoints)
	{
   //     if (!isSide)
   //     {
   //         if (dist < 0)
   //         {
   //             vec1 = new Vector2(vec1.x, vec1.y + 0.75f);
   //             vec2 = new Vector2(vec2.x, vec2.y - 0.75f);
   //         }
   //         else
   //         {
			//	vec1 = new Vector2(vec1.x, vec1.y - 0.75f);
			//	vec2 = new Vector2(vec2.x, vec2.y + 0.75f);
			//}
   //     }

        if (!lineDrawn)
        {
            if (wayPoints.Length == 0)
            {

                lineRenderer.positionCount = 2;

                lineRenderer.SetPosition(0, vec1);
                lineRenderer.SetPosition(1, vec2);

                lineDrawn = true;
            }
            else
            {
                lineRenderer.positionCount = 2 + wayPoints.Length;
                lineRenderer.SetPosition(0, vec1);
                for (int i = 1; i <= wayPoints.Length; i++)
                {
                    lineRenderer.SetPosition(i, wayPoints[i - 1].position);
                }
                lineRenderer.SetPosition(wayPoints.Length + 1, vec2);
				lineDrawn = true;
			}
        }
    }

	private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !isOpened)
        {
            /*if (isSide) dist = (transform.position.x - InGameManager.Instance.player.transform.position.x);
            else dist = (transform.position.y - InGameManager.Instance.player.transform.position.y);

            DoorOpen();*/
            CheckDoor();
        }
    }
}
