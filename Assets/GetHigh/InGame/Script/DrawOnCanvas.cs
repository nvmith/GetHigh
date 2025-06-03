using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DrawOnCanvas : MonoBehaviour
{
    public RawImage rawImage;   // �׸��� ǥ���� RawImage
    public RawImage copiedImage;
    public Texture2D drawingTexture;  // �׸���� �ؽ�ó
    public Color drawColor = Color.black;  // �׸��� ����
    public int penSize = 5;  // ���� ũ��
    public bool drawDone = false;
    public int paletteWidth;
    public int paletteHeight;


    private bool isDrawing = false;
    private Vector2 previousMousePos;

    private void Start()
    {
        // RawImage�� ����� �� �ؽ�ó ����
        drawingTexture = new Texture2D(paletteWidth, paletteHeight);
        rawImage.texture = drawingTexture;

        // �ؽ�ó�� �ʱ�ȭ (�����ϰ�)
        ClearTexture();
    }

    private void Update()
    {
        if (drawDone) return;

        if (Input.GetMouseButtonDown(0))
        {
            Vector2 localMousePos;
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(rawImage.rectTransform, Input.mousePosition, null, out localMousePos))
            {
                isDrawing = true;
                previousMousePos = localMousePos;

                // ���콺 ��ǥ�� �ؽ�ó ��ǥ�� ��ȯ�ϰ� �׸���
                DrawAtPosition(localMousePos);
            }
        }
        else if (Input.GetMouseButton(0) && isDrawing)
        {
            Vector2 localMousePos;
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(rawImage.rectTransform, Input.mousePosition, null, out localMousePos))
            {
                DrawLine(previousMousePos, localMousePos);
                previousMousePos = localMousePos;
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            isDrawing = false;
        }
    }

    // �ؽ�ó�� Ư�� ��ǥ�� �׸��� �Լ�
    private void DrawAtPosition(Vector2 pos)
    {
        int x = Mathf.RoundToInt(pos.x + drawingTexture.width / 2);
        int y = Mathf.RoundToInt(pos.y + drawingTexture.height / 2);

        for (int i = -penSize; i <= penSize; i++)
        {
            for (int j = -penSize; j <= penSize; j++)
            {
                if (x + i >= 0 && x + i < drawingTexture.width && y + j >= 0 && y + j < drawingTexture.height)
                {
                    drawingTexture.SetPixel(x + i, y + j, drawColor);
                }
            }
        }

        drawingTexture.Apply();
    }

    // �� �� ���̿� ���� �׸��� �Լ�
    private void DrawLine(Vector2 start, Vector2 end)
    {
        int x0 = Mathf.RoundToInt(start.x + drawingTexture.width / 2);
        int y0 = Mathf.RoundToInt(start.y + drawingTexture.height / 2);
        int x1 = Mathf.RoundToInt(end.x + drawingTexture.width / 2);
        int y1 = Mathf.RoundToInt(end.y + drawingTexture.height / 2);

        int dx = Mathf.Abs(x1 - x0);
        int dy = Mathf.Abs(y1 - y0);
        int sx = (x0 < x1) ? 1 : -1;
        int sy = (y0 < y1) ? 1 : -1;
        int err = dx - dy;

        while (true)
        {
            DrawAtPosition(new Vector2(x0 - drawingTexture.width / 2, y0 - drawingTexture.height / 2));

            if (x0 == x1 && y0 == y1) break;
            int e2 = err * 2;
            if (e2 > -dy)
            {
                err -= dy;
                x0 += sx;
            }
            if (e2 < dx)
            {
                err += dx;
                y0 += sy;
            }
        }
    }

    // �ؽ�ó�� �ʱ�ȭ (�����ϰ�)
    public void ClearTexture()
    {
        Color clearColor = new Color(0, 0, 0, 0);  // ������ ����
        for (int x = 0; x < drawingTexture.width; x++)
        {
            for (int y = 0; y < drawingTexture.height; y++)
            {
                drawingTexture.SetPixel(x, y, clearColor);
            }
        }
        drawingTexture.Apply();
    }

    public void Confirm()
    {
        copiedImage.texture = rawImage.texture;
        drawDone = true;
        gameObject.SetActive(false);
    }
}
