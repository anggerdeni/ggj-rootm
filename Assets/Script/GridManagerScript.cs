using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class GridManagerScript : MonoBehaviour
{
    public int width, height;
    public SpriteRenderer soilTileSprite;
    public SpriteRenderer stoneTileSprite;
    public SpriteRenderer waterTileSprite;
    public SpriteRenderer rootTipSprite;
    public SpriteRenderer tipUp;
    public SpriteRenderer tipUpUp;
    public SpriteRenderer tipUpRight;
    public SpriteRenderer tipUpLeft;

    public SpriteRenderer tipRight;
    public SpriteRenderer tipRightRight;
    public SpriteRenderer tipRightUp;
    public SpriteRenderer tipRightDown;

    public SpriteRenderer tipDown;
    public SpriteRenderer tipDownDown;
    public SpriteRenderer tipDownRight;
    public SpriteRenderer tipDownLeft;

    public SpriteRenderer tipLeft;
    public SpriteRenderer tipLeftLeft;
    public SpriteRenderer tipLeftUp;
    public SpriteRenderer tipLeftDown;
    public bool allowedToMove;

    public Transform cam;
    private SpriteRenderer rootTip;

    public Transform tiles;


    private float timer = 0;
    private int[] rootTipPos = { 0, 0 };

    private int maxMovableHeight;


    private enum GridState
    {
        Empty,
        Water,
        Stone,
        Root,
    }
    private GridState[,] gridState;
    private SpriteRenderer[,] gridTiles;

    private enum Origin
    {
        FromUp,
        FromRight,
        FromDown,
        FromLeft,
    }
    private Origin origin = Origin.FromUp;

    public int missStreak = 0;
    private float timeElapsed;
    public int waterEnergy = 100;

    public bool isPaused = false;
    public Text waterEnergyText;
    public Text missStreakText;

    void Start()
    {
        maxMovableHeight = height - 3;
        gridState = new GridState[width, maxMovableHeight];
        gridTiles = new SpriteRenderer[width, maxMovableHeight];

        rootTipPos[0] = (int)width / 2;
        rootTipPos[1] = maxMovableHeight - 1;
        GenerateGrid();
    }

    void GenerateGrid()
    {
        var random = new System.Random();
        var list = new List<GridState>{
            GridState.Empty,
            GridState.Empty,
            GridState.Empty,
            GridState.Empty,
            GridState.Empty,
            GridState.Empty,
            GridState.Empty,
            GridState.Empty,
            GridState.Empty,
            GridState.Empty,
            GridState.Water,
        };
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < maxMovableHeight; j++)
            {
                int index = random.Next(list.Count);
                GridState st = list[index];
                gridState[i, j] = st;

                switch (st)
                {
                    case GridState.Empty:
                        var spawnedTile = Instantiate(soilTileSprite, new Vector3(i, j, 0), Quaternion.identity);
                        gridTiles[i, j] = spawnedTile;
                        spawnedTile.name = $"Tile {i} {j}";
                        spawnedTile.transform.SetParent(tiles);
                        gridTiles[i, j] = spawnedTile;
                        break;
                    case GridState.Water:
                        var spawnedTile2 = Instantiate(waterTileSprite, new Vector3(i, j, 0), Quaternion.identity);
                        gridTiles[i, j] = spawnedTile2;
                        spawnedTile2.name = $"Tile {i} {j}";
                        spawnedTile2.transform.SetParent(tiles);
                        break;
                    case GridState.Stone:
                        var spawnedTile3 = Instantiate(stoneTileSprite, new Vector3(i, j, 0), Quaternion.identity);
                        gridTiles[i, j] = spawnedTile3;
                        spawnedTile3.name = $"Tile {i} {j}";
                        spawnedTile3.transform.SetParent(tiles);
                        break;
                }

            }
        }

        cam.transform.position = new Vector3((float)width / 2, (float)maxMovableHeight, -10);
        rootTip = Instantiate(rootTipSprite, new Vector3(rootTipPos[0], rootTipPos[1], -1), Quaternion.identity);
        rootTip.sprite = tipDown.sprite;

        Instantiate(tipDownDown, new Vector3(rootTipPos[0], rootTipPos[1] + 1, -1), Quaternion.identity);
        Instantiate(tipDownDown, new Vector3(rootTipPos[0], rootTipPos[1] + 2, -1), Quaternion.identity);
        Instantiate(tipDownDown, new Vector3(rootTipPos[0], rootTipPos[1] + 3, -1), Quaternion.identity);

    }

    Origin determineOrigin(Vector3 prevPos, Vector3 newPos)
    {
        if (prevPos.x < newPos.x) return Origin.FromLeft;
        if (prevPos.x > newPos.x) return Origin.FromRight;
        if (prevPos.y < newPos.y) return Origin.FromDown;
        return Origin.FromUp;
    }

    bool validPosition(Vector3 pos)
    {
        return gridState[(int)pos.x, (int)pos.y] != GridState.Stone && gridState[(int)pos.x, (int)pos.y] != GridState.Root;
    }

    bool attemptToMove()
    {
        var currentPos = rootTip.transform.position;
        if (Input.GetKeyDown(KeyCode.S) && currentPos.y > 0)
        {
            if (!allowedToMove)
            {
                missStreak += 1;
                return false;
            }
            var newPost = rootTip.transform.position + Vector3.down;
            if (!validPosition(newPost)) return false;

            rootTip.transform.position = newPost;
            rootTip.sprite = tipDown.sprite;
            if (rootTip.transform.position.y > -2)
            {
                cam.transform.position += Vector3.down;
            }

            switch (origin)
            {
                case Origin.FromUp:
                    Instantiate(tipDownDown, new Vector3(currentPos.x, currentPos.y, -1), Quaternion.identity);
                    break;
                case Origin.FromLeft:
                    Instantiate(tipRightDown, new Vector3(currentPos.x, currentPos.y, -1), Quaternion.identity);
                    break;
                case Origin.FromRight:
                    Instantiate(tipLeftDown, new Vector3(currentPos.x, currentPos.y, -1), Quaternion.identity);
                    break;
            }

            origin = Origin.FromUp;
            gridState[(int)currentPos.x, (int)currentPos.y] = GridState.Root;

            if (gridState[(int)newPost.x, (int)newPost.y] == GridState.Water)
            {
                gridTiles[(int)newPost.x, (int)newPost.y].sprite = soilTileSprite.sprite;
                waterEnergy += 36;
            }

            missStreak = 1;
            return true;
        }

        if (Input.GetKeyDown(KeyCode.W) && currentPos.y < maxMovableHeight - 1)
        {
            if (!allowedToMove)
            {
                missStreak += 1;
                return false;
            }
            var newPost = rootTip.transform.position + Vector3.up;
            if (!validPosition(newPost)) return false;

            rootTip.transform.position = newPost;
            rootTip.sprite = tipUp.sprite;
            if (rootTip.transform.position.y < height - 5)
            {
                cam.transform.position += Vector3.up;
            }

            switch (origin)
            {
                case Origin.FromDown:
                    Instantiate(tipUpUp, new Vector3(currentPos.x, currentPos.y, -1), Quaternion.identity);
                    break;
                case Origin.FromLeft:
                    Instantiate(tipRightUp, new Vector3(currentPos.x, currentPos.y, -1), Quaternion.identity);
                    break;
                case Origin.FromRight:
                    Instantiate(tipLeftUp, new Vector3(currentPos.x, currentPos.y, -1), Quaternion.identity);
                    break;
            }

            origin = Origin.FromDown;
            gridState[(int)currentPos.x, (int)currentPos.y] = GridState.Root;

            if (gridState[(int)newPost.x, (int)newPost.y] == GridState.Water)
            {
                gridTiles[(int)newPost.x, (int)newPost.y].sprite = soilTileSprite.sprite;
                waterEnergy += 36;
            }

            missStreak = 1;
            return true;
        }

        if (Input.GetKeyDown(KeyCode.A) && currentPos.x > 0)
        {
            if (!allowedToMove)
            {
                missStreak += 1;
                return false;
            }
            var newPost = rootTip.transform.position + Vector3.left;
            if (!validPosition(newPost)) return false;

            rootTip.transform.position = newPost;
            rootTip.sprite = tipLeft.sprite;

            switch (origin)
            {
                case Origin.FromRight:
                    Instantiate(tipLeftLeft, new Vector3(currentPos.x, currentPos.y, -1), Quaternion.identity);
                    break;
                case Origin.FromUp:
                    Instantiate(tipDownLeft, new Vector3(currentPos.x, currentPos.y, -1), Quaternion.identity);
                    break;
                case Origin.FromDown:
                    Instantiate(tipUpLeft, new Vector3(currentPos.x, currentPos.y, -1), Quaternion.identity);
                    break;
            }

            origin = Origin.FromRight;
            gridState[(int)currentPos.x, (int)currentPos.y] = GridState.Root;
            if (gridState[(int)newPost.x, (int)newPost.y] == GridState.Water)
            {
                gridTiles[(int)newPost.x, (int)newPost.y].sprite = soilTileSprite.sprite;
                waterEnergy += 36;
            }

            missStreak = 1;
            return true;
        }
        if (Input.GetKeyDown(KeyCode.D) && currentPos.x < width - 1)
        {
            if (!allowedToMove)
            {
                missStreak += 1;
                return false;
            }
            var newPost = rootTip.transform.position + Vector3.right;
            if (!validPosition(newPost)) return false;

            rootTip.transform.position = newPost;
            rootTip.sprite = tipRight.sprite;

            switch (origin)
            {
                case Origin.FromLeft:
                    Instantiate(tipRightRight, new Vector3(currentPos.x, currentPos.y, -1), Quaternion.identity);
                    break;
                case Origin.FromUp:
                    Instantiate(tipDownRight, new Vector3(currentPos.x, currentPos.y, -1), Quaternion.identity);
                    break;
                case Origin.FromDown:
                    Instantiate(tipUpRight, new Vector3(currentPos.x, currentPos.y, -1), Quaternion.identity);
                    break;
            }

            origin = Origin.FromLeft;
            gridState[(int)currentPos.x, (int)currentPos.y] = GridState.Root;

            if (gridState[(int)newPost.x, (int)newPost.y] == GridState.Water)
            {
                gridTiles[(int)newPost.x, (int)newPost.y].sprite = soilTileSprite.sprite;
                waterEnergy += 36;
            }

            missStreak = 1;
            return true;
        }

        return false;
    }

    void Update()
    {
        if (isPaused)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                ResumeGame();
            }

            return;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseGame();
        }

        waterEnergyText.text = $"{waterEnergy}";
        missStreakText.text = $"{missStreak}";
        // win condition
        if (timeElapsed >= 90)
        {
            Debug.Log("you win, time elapsed");
            PauseGame();
        }

        // lose condition
        if (missStreak >= 5 || waterEnergy <= 0)
        {
            Debug.Log($"you lose, {waterEnergy} {missStreak}");
            PauseGame();
        }

        timeElapsed += Time.deltaTime;

        attemptToMove();
    }

    void StartGame()
    {
        Time.timeScale = 1;
        isPaused = false;
    }

    void PauseGame()
    {
        Time.timeScale = 0;
        isPaused = true;
    }

    void ResumeGame()
    {
        Time.timeScale = 1;
        isPaused = false;
    }
}

