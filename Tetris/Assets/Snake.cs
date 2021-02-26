using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

public class Snake : MonoBehaviour
{
    [Header("Размеры игрового поля")]
    [SerializeField] int fieldX = 10;
    [SerializeField] int fieldY = 10;
    GameObject block;
    int[,] matrix;
    int matrixX;
    int matrixY;
    int headX;
    int headY;
    int tailLength = 9; // длинна хвоста начинается с 11 значение должно быть 9 на начало игры.
    int dir = 0; // направление движения 1-вверх, 2-вправо, 3-вниз, 4-влево
    const int emptyField = 0;
    const int bort = 1;
    const int feed = 8;
    const int head = 9;    
    const int tailStart = 10;
    // цыфры больше 10 отвечают за длинну хвоста.
    [Space(10)]
    public GameObject bodySpr;
    public GameObject bortSpr;   
    public Text scor;
    public GameObject menu;
    public Sprite[] blockSp;
    GameObject[,] allBlock;
    void Start()
    {
        matrixX = fieldX + 2;
        matrixY = fieldY + 2;
        matrix = new int[matrixY, matrixX];
        Fiil();
        AddFeed();
        StartCoroutine(Tick());
    }    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow)) SetUp();
        if (Input.GetKeyDown(KeyCode.RightArrow)) SetRight();
        if (Input.GetKeyDown(KeyCode.DownArrow)) SetDown();
        if (Input.GetKeyDown(KeyCode.LeftArrow)) SetLeft();
    }
    IEnumerator Tick()
    {
        while (true)
        {
            switch (dir)
            {
                case 1:
                    MoveUp();
                    break;
                case 2:
                    MoveRight();
                    break;
                case 3:
                    MoveDown();
                    break;
                case 4:
                    MoveLeft();
                    break;
            }
            TailMuv();
            Redraw();
            scor.text = (" Длинна: " + (tailLength - 10));
            yield return new WaitForSeconds(0.5f);
        }
    }    
    void MoveUp()
    {        
        if (matrix[headY - 1, headX] == emptyField || matrix[headY - 1, headX] == feed || matrix[headY - 1, headX] >= tailLength)
        {
            if (matrix[headY - 1, headX] == feed)
                AddFeed();
            matrix[headY - 1, headX] = head;
            matrix[headY, headX] = tailStart;
            headY--;
            return;
        }
        else 
            GameOver();
    }
    void MoveDown()
    {
        if (matrix[headY + 1, headX] == emptyField || matrix[headY + 1, headX] == feed || matrix[headY + 1, headX] >= tailLength)
        {
            if (matrix[headY + 1, headX] == feed)
                AddFeed();
            matrix[headY + 1, headX] = head;
            matrix[headY, headX] = tailStart;
            headY++;
            return;
        }
        else
            GameOver();
    }
    void MoveLeft()
    {
        if (matrix[headY, headX - 1] == emptyField || matrix[headY, headX - 1] == feed || matrix[headY, headX - 1] >= tailLength)
        {
            if (matrix[headY, headX - 1] == feed)
                AddFeed();
            matrix[headY, headX - 1] = head;
            matrix[headY, headX] = tailStart;
            headX--;
            return;
        }
        else
            GameOver();
    }
    void MoveRight()
    {
        if (matrix[headY, headX + 1] == emptyField || matrix[headY, headX + 1] == feed || matrix[headY, headX + 1] >= tailLength)
        {
            if (matrix[headY, headX + 1] == feed)
                AddFeed();
            matrix[headY, headX + 1] = head;
            matrix[headY, headX] = tailStart;
            headX++;
            return;
        }
        else
            GameOver();
    }
    void AddFeed()
    {        
        int y = Random.Range(1, matrixY - 1);
        int x = Random.Range(1, matrixX - 1);      
        if (matrix[y, x] == emptyField)
        {
            matrix[y, x] = feed;
            tailLength++;
            return;
        }   
        else
            AddFeed();     // рекурсия - очень плохая идея           
    }
    void TailMuv()
    {
        for (int y = 0; y < matrixY; y++)        
            for (int x = 0; x < matrixX; x++)
            {
                if (matrix[y, x] >= tailStart)
                    matrix[y, x]++;
                if (matrix[y, x] > tailLength)
                    matrix[y, x] = emptyField;
            }       
    }
    void Fiil()
    {
        transform.position = Camera.main.ViewportToWorldPoint(new Vector3(0f, 1f, 0)) - new Vector3(0f, 1f, 0);        
        allBlock = new GameObject[matrixY, matrixX];
        for (int y = 0; y < matrixY; y++)        
            for (int x = 0; x < matrixX; x++)           
                if ((y > 0 && y < matrixY -1 )&& (x > 0 && x < matrixX - 1))
                {
                    matrix[y, x] = emptyField;
                    allBlock[y, x] = Instantiate(bodySpr, new Vector2(transform.position.x + 0.5f + x , transform.position.y - 0.5f - y), Quaternion.identity);                    
                }
                else
                {
                    matrix[y, x] = bort;
                    allBlock[y, x] = Instantiate(bortSpr, new Vector2(transform.position.x + 0.5f + x, transform.position.y - 0.5f - y), Quaternion.identity);
                }
        headX = matrixX / 2; // ставим голову в центр поля.
        headY = matrixY / 2; //
        matrix[headY, headX] = head; // 
    }
    void Redraw()
    {
        for (int y = 1; y < matrixY-1; y++)        
            for (int x = 1; x < matrixX-1; x++)
            {
                allBlock[y, x].SetActive(matrix[y, x] != emptyField);
                if (matrix[y, x] == feed)
                    allBlock[y, x].GetComponent<SpriteRenderer>().sprite = blockSp[5];
                else if (matrix[y, x] == head)
                    allBlock[y, x].GetComponent<SpriteRenderer>().sprite = blockSp[dir];                
                else
                    allBlock[y, x].GetComponent<SpriteRenderer>().sprite = blockSp[6];                
            }        
    }
    void GameOver()
    {
        Time.timeScale = 0.0f;
        menu.SetActive(true);
        menu.GetComponentInChildren<Text>().text = (" Длинна: " + (tailLength - 10));        
    }
    public void Restart()
    {
        for (int y = 0; y < matrixY; y++)
            for (int x = 0; x < matrixX; x++)
                if ((y > 0 && y < matrixY - 1) && (x > 0 && x < matrixX - 1))                
                    matrix[y, x] = emptyField;                      
        headX = matrixX / 2; // ставим голову в центр поля.
        headY = matrixY / 2; //
        matrix[headY, headX] = head; //
        tailLength = 9;
        dir = 0;
        AddFeed();
        menu.SetActive(false);
        Time.timeScale = 1.0f;
    }
    public void SetUp()
    {
        if (matrix[headY - 1, headX] != tailStart + 1)
            dir = 1;
    }
    public void SetRight()
    {
        if (matrix[headY, headX + 1] != tailStart + 1)
            dir = 2;
    }
    public void SetDown()
    {
        if (matrix[headY + 1, headX] != tailStart + 1)
            dir = 3;
    }
    public void SetLeft()
    {
        if (matrix[headY, headX - 1] != tailStart + 1)
            dir = 4;
    }
}
