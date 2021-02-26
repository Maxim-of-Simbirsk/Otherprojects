using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tetris : MonoBehaviour
{
    [Header("Размеры игрового поля")]
    [SerializeField] int fieldX = 10;
    [SerializeField] int fieldY = 20;
    [SerializeField] float startSpeed = 0.5f;
    [Space(10)]
    public Text scoreTXT;
    public GameObject figSpr;
    public GameObject bortSpr;
    GameObject[,] allBlock;
    const int emptyField = 0;
    const int figur = 1;
    const int monolit = 2;
    const int bort = 5;
    int[,] matrix;
    int matrixX;
    int matrixY;
    int figPositionX;
    int figPositionY;
    int figSize;
    int lastFig;
    int score = 0;
    float speed;
    #region (Фигуры)
    int[,] N = { { figur, figur }, { figur, figur } };
    int[,] S = { { figur, 0, 0 }, { figur, figur, 0 }, { 0, figur, 0 } };
    int[,] _S = { { 0, figur, 0 }, { figur, figur, 0 }, { figur, 0, 0 } };
    int[,] T = { { 0, 0, 0 }, { figur, figur, figur }, { 0, figur, 0 } };
    int[,] L = { { 0, figur, 0 }, { 0, figur, 0 }, { 0, figur, figur } };
    int[,] _L = { { 0, figur, 0 }, { 0, figur, 0 }, { figur, figur, 0 } };
    int[,] I = { { 0, figur, 0, 0 }, { 0, figur, 0, 0 }, { 0, figur, 0, 0 }, { 0, figur, 0, 0 } };
    #endregion
    void Start()
    {
        matrixX = fieldX + 4;
        matrixY = fieldY + 6;
        matrix = new int[matrixY, matrixX];
        speed = startSpeed;
        Fiil();
        AddFigure();
        StartCoroutine(Tick());        
    }    
    void Update()
    {         
        if (Input.GetKeyDown(KeyCode.LeftArrow)) MoveLeft();
        if (Input.GetKeyDown(KeyCode.RightArrow)) MoveRight();
        if (Input.GetKeyDown(KeyCode.DownArrow)) MoveDown();
        if (Input.GetKeyDown(KeyCode.UpArrow)) Rotate();
        RedrawFild();
    }
    IEnumerator Tick()
    {
        while (true)
        {
            scoreTXT.text = (" СЧЁТ : " + score);
            StepDown();            
            yield return new WaitForSeconds(speed);
        }
    }
    #region (Отрисовка)
    void Fiil()
    {
        transform.position = Camera.main.ViewportToWorldPoint(new Vector3(0f, 1f, 0)) - new Vector3(0f, 3f, 0);
        allBlock = new GameObject[matrixY, matrixX];
        for (int y = 0; y < matrixY; y++)
            for (int x = 0; x < matrixX; x++)
                if ((y >= 4 && y < matrixY - 2) && (x > 1 && x < matrixX - 2))
                {
                    matrix[y, x] = emptyField;
                    allBlock[y, x] = Instantiate(figSpr, new Vector2(transform.position.x - 0.5f + x, transform.position.y + 3.5f - y), Quaternion.identity);
                }
                else if ((x > 0 && x < matrixX-1) && (y >= 4 && y < matrixY-1))
                {
                    matrix[y, x] = bort;
                    allBlock[y, x] = Instantiate(bortSpr, new Vector2(transform.position.x - 0.5f + x, transform.position.y + 3.5f - y), Quaternion.identity);
                }        
    }
    void RedrawFild()
    {        
        for (int y = 4; y < matrixY - 2; y++)
            for (int x = 2; x < matrixX - 2; x++)            
                allBlock[y, x].SetActive(matrix[y, x] != emptyField);        
    }
    #endregion
    #region(Прогресс)
    void StepDown()
    {
        for (int y = matrixY - 3; y >= 0; y--)
            for (int x = 2; x < matrixX - 2; x++)
            {                
                if (matrix[y, x] == figur && (matrix[y + 1, x] == monolit || y == matrixY - 3))
                {
                    SetMonolit();
                    StartCoroutine(CleanLines());                  
                    return;
                }
                else if (matrix[y, x] == figur && (matrix[y + 3, x] == monolit || y == matrixY - 5))                
                    speed = 0.5f;                
            }
        for (int y = matrixY - 3; y >= 0; y--)
            for (int x = 2; x < matrixX - 2; x++)
            {
                if (matrix[y, x] == figur)
                {
                    matrix[y + 1, x] = figur;
                    matrix[y, x] = emptyField;
                }
            }
        figPositionY++;
    }
    void SetMonolit()
    {
        for (int y = 0; y < matrixY - 2; y++)
            for (int x = 2; x < matrixX - 2; x++)
            {
                if (matrix[y, x] == figur)
                    matrix[y, x] = monolit;
            }
    }    
    IEnumerator CleanLines() // + Подсчёт очков
    {
        int match = 0;
        for (int y = matrixY - 3; y >= 0; y--)
        {
            int sum = 0;
            for (int x = 2; x < matrixX - 2; x++)
            {
                sum = sum + matrix[y, x];
                if (sum == fieldX * monolit)
                {
                    for (int l = 2; l < matrixX - 2; l++)
                    {
                        matrix[y, l] = emptyField;
                        yield return new WaitForSeconds(0.05f);
                    }
                    for (int l = y; l >= 1; l--)
                        for (int xx = 2; xx < matrixX - 2; xx++)
                            if (matrix[l, xx] == emptyField && matrix[l - 1, xx] == monolit)
                            {
                                matrix[l, xx] = monolit;
                                matrix[l - 1, xx] = emptyField;
                            }
                    match++;
                    y++;
                }
            }
        }
        score = score + ((match * fieldX) * match);
        StopCoroutine(CleanLines());
        AddFigure();
    }
    #endregion
    #region(Добавление фигуры)
    void AddFigure()
    {
                again:
        int switchFig = Random.Range(1, 8);
        if (switchFig != lastFig)
            lastFig = switchFig;
        else
            goto again;
        switch (switchFig)
        {
            case 1:
                figSize = 2;
                DrawFigure(N);
                break;
            case 2:
                figSize = 3;
                DrawFigure(S);
                break;
            case 3:
                figSize = 3;
                DrawFigure(_S);
                break;
            case 4:
                figSize = 3;
                DrawFigure(T);
                break;
            case 5:
                figSize = 3;
                DrawFigure(L);
                break;
            case 6:
                figSize = 3;
                DrawFigure(_L);
                break;
            case 7:
                figSize = 4;
                DrawFigure(I);
                break;
        }
    }
    void DrawFigure(int[,]fig)
    {
        figPositionX = matrixX/2-1;
        figPositionY = 0;
        for (int y = 0; y < figSize; y++)
            for (int x = 0; x < figSize; x++)            
                matrix[y + figPositionY, x + figPositionX] = fig[y, x];            
    }
    #endregion
    #region (Перемещение)
    public void MoveLeft()
    {
        for (int x = 2; x < matrixX - 2; x++)
            for (int y = 0; y < matrixY - 2; y++)
            {
                if (matrix[y, x] == figur && (matrix[y, x - 1] == bort || matrix[y, x - 1] == monolit))                
                    return;
            }
        for (int x = 2; x < matrixX - 2; x++)
            for (int y = 0; y < matrixY - 2; y++)
            {
                if (matrix[y, x] == figur)
                {
                    matrix[y, x - 1] = figur;
                    matrix[y, x] = emptyField;
                }
            }
        figPositionX--;
    }
    public void MoveRight()
    {
        for (int x = matrixX - 3; x > 1; x--)
            for (int y = 0; y < matrixY - 2; y++)
            {
                if (matrix[y, x] == figur && (matrix[y, x + 1] == bort || matrix[y, x + 1] == monolit))
                    return;
            }
        for (int x = matrixX - 3; x > 1; x--)
            for (int y = 0; y < matrixY - 2; y++)            
                if (matrix[y, x] == figur)
                {
                    matrix[y, x + 1] = figur;
                    matrix[y, x] = emptyField;
                }            
        figPositionX++;
    }
    public void MoveDown()
    {
        if (figPositionY > 3)
        speed = 0.01f;
    }
    public void Rotate()
    {
        int[,] cache = new int[figSize, figSize];
        for (int y = 0; y < figSize; y++)
            for (int x = 0; x < figSize; x++)
            {
                if (matrix[y + figPositionY, x + figPositionX] == figur)
                    cache[(figSize - 1) - x, y] = matrix[y + figPositionY, x + figPositionX];
            }
        
        for (int y = 0; y < figSize; y++)
            for (int x = 0; x < figSize; x++)
            {
                if (matrix[y + figPositionY, x + figPositionX] == bort && figPositionX < matrixX / 2)                
                    figPositionX = 2;                   
                
                else if (matrix[y + figPositionY, x + figPositionX] == bort && figPositionX > matrixX / 2)                
                    figPositionX = matrixX - 2 - figSize;                   
                
                if (matrix[y + figPositionY, x + figPositionX] == monolit || matrix[y + figPositionY, x + figPositionX] == bort)                    
                    return;
            }
        for (int y = 0; y < figSize; y++)
            for (int x = 0; x < figSize; x++)
                matrix[y + figPositionY, x + figPositionX] = cache[y, x];
    }
    #endregion
}
