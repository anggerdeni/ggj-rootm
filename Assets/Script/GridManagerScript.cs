using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine;
using TMPro;

public class GridManagerScript : MonoBehaviour
{
    public int width, height;
    public SpriteRenderer soilTileSprite;
    public SpriteRenderer stoneTileSprite;
    public SpriteRenderer waterTileSprite;
    public SpriteRenderer soilTileSprite2;
    public SpriteRenderer stoneTileSprite2;
    public SpriteRenderer waterTileSprite2;
    public SpriteRenderer soilTileSprite3;
    public SpriteRenderer stoneTileSprite3;
    public SpriteRenderer waterTileSprite3;
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

    public SpriteRenderer[] grass;
    public SpriteRenderer[] stone;
    public SpriteRenderer[] sidestone;

    public Transform cam;
    private SpriteRenderer rootTip;

    public Transform tiles;


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

    private float timeElapsed;
    public int waterEnergy = 100;

    public bool isPaused = false;
    public TMP_Text waterEnergyText;
    public TMP_Text scoreText;

    public GameObject pauseScreen;
    public GameObject winScreen;
    public GameObject loseScreen;

    private GameObject pauseGameObject;
    private GameObject finishGameObject;
    
    public bool gameFinished;

    public Transform waterEnergyUI;

    public AudioSource music;
    public AudioSource winSound;
    public AudioSource loseSound;
    public AudioSource missSound;

    public float beatTempo;
    public int numOfBeatTillStartMusic;
    private bool isMusicStarted;
    private float timeElapsedBeforeStartingMusic = 0;

    void Start()
    {
        Time.timeScale = 1;
        isPaused = false;
        maxMovableHeight = height - 3;
        gridState = new GridState[width, maxMovableHeight];
        gridTiles = new SpriteRenderer[width, maxMovableHeight];

        rootTipPos[0] = (int)width / 2;
        rootTipPos[1] = maxMovableHeight - 1;
        GenerateGrid();
        isMusicStarted = false;
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
            GridState.Empty,
            GridState.Empty,
            GridState.Empty,
            GridState.Empty,
            GridState.Empty,
            GridState.Empty,
            GridState.Empty,
            GridState.Empty,
            GridState.Empty,
            GridState.Stone,
            GridState.Stone,
            GridState.Stone,
            GridState.Stone,
            GridState.Stone,
            GridState.Stone,
            GridState.Stone,
            GridState.Stone,
            GridState.Stone,
            GridState.Stone,
            GridState.Stone,
            GridState.Stone,
            GridState.Water,
            GridState.Water,
            GridState.Water,
            GridState.Water,
            GridState.Water,
        };
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < maxMovableHeight; j++)
            {

                int index = random.Next(list.Count);
                GridState st = list[index];
                gridState[i, j] = st;

                int spriteidx = random.Next(3);

                SpriteRenderer[] soils = {soilTileSprite, soilTileSprite2, soilTileSprite3};
                SpriteRenderer[] waters = {waterTileSprite, waterTileSprite2, waterTileSprite3};
                SpriteRenderer[] stones = {stoneTileSprite, stoneTileSprite2, stoneTileSprite3};

                if (i == rootTipPos[0] && j == rootTipPos[1])
                {
                    st = list[0];
                    gridState[i, j] = st;
                    
                }

                switch (st)
                {
                    case GridState.Empty:
                        var spawnedTile = Instantiate(soils[spriteidx], new Vector3(i, j, 0), Quaternion.identity);
                        gridTiles[i, j] = spawnedTile;
                        spawnedTile.name = $"Tile {i} {j}";
                        spawnedTile.transform.SetParent(tiles);
                        gridTiles[i, j] = spawnedTile;
                        break;
                    case GridState.Water:
                        var spawnedTile2 = Instantiate(waters[spriteidx], new Vector3(i, j, 0), Quaternion.identity);
                        gridTiles[i, j] = spawnedTile2;
                        spawnedTile2.name = $"Tile {i} {j}";
                        spawnedTile2.transform.SetParent(tiles);
                        break;
                    case GridState.Stone:
                        var spawnedTile3 = Instantiate(stones[spriteidx], new Vector3(i, j, 0), Quaternion.identity);
                        gridTiles[i, j] = spawnedTile3;
                        spawnedTile3.name = $"Tile {i} {j}";
                        spawnedTile3.transform.SetParent(tiles);
                        break;
                }

            }
        }
        for (int j = 0; j < maxMovableHeight; j++) {
            Instantiate(sidestone[2+random.Next(2)], new Vector3(-1, j, 0), Quaternion.identity).transform.SetParent(tiles);
            Instantiate(stone[random.Next(3)], new Vector3(-2, j, 0), Quaternion.identity).transform.SetParent(tiles);
            Instantiate(stone[random.Next(3)], new Vector3(-3, j, 0), Quaternion.identity).transform.SetParent(tiles);

            Instantiate(sidestone[random.Next(2)], new Vector3(width, j, 0), Quaternion.identity).transform.SetParent(tiles);
            Instantiate(stone[random.Next(3)], new Vector3(width+1, j, 0), Quaternion.identity).transform.SetParent(tiles);
            Instantiate(stone[random.Next(3)], new Vector3(width+2, j, 0), Quaternion.identity).transform.SetParent(tiles);
            Instantiate(stone[random.Next(3)], new Vector3(width+3, j, 0), Quaternion.identity).transform.SetParent(tiles);
        }


        cam.transform.position = new Vector3((float)width / 2, (float)maxMovableHeight-4, -10);
        rootTip = Instantiate(rootTipSprite, new Vector3(rootTipPos[0], rootTipPos[1], -1), Quaternion.identity);
        rootTip.sprite = tipDown.sprite;

        Instantiate(stone[random.Next(3)], new Vector3(rootTipPos[0]-8, rootTipPos[1]+1, -1), Quaternion.identity).transform.SetParent(tiles);
        Instantiate(stone[random.Next(3)], new Vector3(rootTipPos[0]-7, rootTipPos[1]+1, -1), Quaternion.identity).transform.SetParent(tiles);
        Instantiate(stone[random.Next(3)], new Vector3(rootTipPos[0]-6, rootTipPos[1]+1, -1), Quaternion.identity).transform.SetParent(tiles);
        Instantiate(stone[3+random.Next(3)], new Vector3(rootTipPos[0]-5, rootTipPos[1]+1, -1), Quaternion.identity).transform.SetParent(tiles);
        Instantiate(stone[3+random.Next(3)], new Vector3(rootTipPos[0]-4, rootTipPos[1]+1, -1), Quaternion.identity).transform.SetParent(tiles);
        Instantiate(stone[3+random.Next(3)], new Vector3(rootTipPos[0]-3, rootTipPos[1]+1, -1), Quaternion.identity).transform.SetParent(tiles);
        Instantiate(stone[3+random.Next(3)], new Vector3(rootTipPos[0]-2, rootTipPos[1]+1, -1), Quaternion.identity).transform.SetParent(tiles);
        Instantiate(stone[3+random.Next(3)], new Vector3(rootTipPos[0]-1, rootTipPos[1]+1, -1), Quaternion.identity).transform.SetParent(tiles);
        Instantiate(grass[3], new Vector3(rootTipPos[0], rootTipPos[1]+1, -1), Quaternion.identity).transform.SetParent(tiles);
        Instantiate(stone[3+random.Next(3)], new Vector3(rootTipPos[0]+1, rootTipPos[1]+1, -1), Quaternion.identity).transform.SetParent(tiles);
        Instantiate(stone[3+random.Next(3)], new Vector3(rootTipPos[0]+2, rootTipPos[1]+1, -1), Quaternion.identity).transform.SetParent(tiles);
        Instantiate(stone[3+random.Next(3)], new Vector3(rootTipPos[0]+3, rootTipPos[1]+1, -1), Quaternion.identity).transform.SetParent(tiles);
        Instantiate(stone[3+random.Next(3)], new Vector3(rootTipPos[0]+4, rootTipPos[1]+1, -1), Quaternion.identity).transform.SetParent(tiles);
        Instantiate(stone[3+random.Next(3)], new Vector3(rootTipPos[0]+5, rootTipPos[1]+1, -1), Quaternion.identity).transform.SetParent(tiles);
        Instantiate(stone[random.Next(3)], new Vector3(rootTipPos[0]+6, rootTipPos[1]+1, -1), Quaternion.identity).transform.SetParent(tiles);
        Instantiate(stone[random.Next(3)], new Vector3(rootTipPos[0]+7, rootTipPos[1]+1, -1), Quaternion.identity).transform.SetParent(tiles);
        Instantiate(stone[random.Next(3)], new Vector3(rootTipPos[0]+8, rootTipPos[1]+1, -1), Quaternion.identity).transform.SetParent(tiles);
        Instantiate(stone[random.Next(3)], new Vector3(rootTipPos[0]+9, rootTipPos[1]+1, -1), Quaternion.identity).transform.SetParent(tiles);

        Instantiate(grass[random.Next(3)], new Vector3(rootTipPos[0]-8, rootTipPos[1]+2, -1), Quaternion.identity).transform.SetParent(tiles);
        Instantiate(grass[random.Next(3)], new Vector3(rootTipPos[0]-7, rootTipPos[1]+2, -1), Quaternion.identity).transform.SetParent(tiles);
        Instantiate(grass[random.Next(3)], new Vector3(rootTipPos[0]-6, rootTipPos[1]+2, -1), Quaternion.identity).transform.SetParent(tiles);
        Instantiate(grass[random.Next(3)], new Vector3(rootTipPos[0]-5, rootTipPos[1]+2, -1), Quaternion.identity).transform.SetParent(tiles);
        Instantiate(grass[random.Next(3)], new Vector3(rootTipPos[0]-4, rootTipPos[1]+2, -1), Quaternion.identity).transform.SetParent(tiles);
        Instantiate(grass[random.Next(3)], new Vector3(rootTipPos[0]-3, rootTipPos[1]+2, -1), Quaternion.identity).transform.SetParent(tiles);
        Instantiate(grass[random.Next(3)], new Vector3(rootTipPos[0]-2, rootTipPos[1]+2, -1), Quaternion.identity).transform.SetParent(tiles);
        Instantiate(grass[random.Next(3)], new Vector3(rootTipPos[0]-1, rootTipPos[1]+2, -1), Quaternion.identity).transform.SetParent(tiles);
        Instantiate(grass[3], new Vector3(rootTipPos[0], rootTipPos[1]+2, -1), Quaternion.identity).transform.SetParent(tiles);
        Instantiate(grass[random.Next(3)], new Vector3(rootTipPos[0]+1, rootTipPos[1]+2, -1), Quaternion.identity).transform.SetParent(tiles);
        Instantiate(grass[random.Next(3)], new Vector3(rootTipPos[0]+2, rootTipPos[1]+2, -1), Quaternion.identity).transform.SetParent(tiles);
        Instantiate(grass[random.Next(3)], new Vector3(rootTipPos[0]+3, rootTipPos[1]+2, -1), Quaternion.identity).transform.SetParent(tiles);
        Instantiate(grass[random.Next(3)], new Vector3(rootTipPos[0]+4, rootTipPos[1]+2, -1), Quaternion.identity).transform.SetParent(tiles);
        Instantiate(grass[random.Next(3)], new Vector3(rootTipPos[0]+5, rootTipPos[1]+2, -1), Quaternion.identity).transform.SetParent(tiles);
        Instantiate(grass[random.Next(3)], new Vector3(rootTipPos[0]+6, rootTipPos[1]+2, -1), Quaternion.identity).transform.SetParent(tiles);
        Instantiate(grass[random.Next(3)], new Vector3(rootTipPos[0]+7, rootTipPos[1]+2, -1), Quaternion.identity).transform.SetParent(tiles);
        Instantiate(grass[random.Next(3)], new Vector3(rootTipPos[0]+8, rootTipPos[1]+2, -1), Quaternion.identity).transform.SetParent(tiles);
        Instantiate(grass[random.Next(3)], new Vector3(rootTipPos[0]+9, rootTipPos[1]+2, -1), Quaternion.identity).transform.SetParent(tiles);

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
                waterEnergy -= 6;
                UpdateWaterText();
                missSound.Play();
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
                SpriteRenderer[] srs = gridTiles[(int)newPost.x, (int)newPost.y].GetComponentsInChildren<SpriteRenderer>();
                foreach(SpriteRenderer sr in srs)
                {
                    if(sr.name == "Water" || sr.name == "Square")
                        sr.sprite = null;
                }
                waterEnergy += 36;
                if (waterEnergy > 100) waterEnergy = 100;
                UpdateWaterText();
            }

            return true;
        }

        if (Input.GetKeyDown(KeyCode.W) && currentPos.y < maxMovableHeight - 1)
        {
            if (!allowedToMove)
            {
                waterEnergy -= 6;
                UpdateWaterText();
                missSound.Play();
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
                SpriteRenderer[] srs = gridTiles[(int)newPost.x, (int)newPost.y].GetComponentsInChildren<SpriteRenderer>();
                foreach(SpriteRenderer sr in srs)
                {
                    if(sr.name == "Water" || sr.name == "Square")
                        sr.sprite = null;
                }
                waterEnergy += 36;
                if (waterEnergy > 100) waterEnergy = 100;
                UpdateWaterText();
            }

            return true;
        }

        if (Input.GetKeyDown(KeyCode.A) && currentPos.x > 0)
        {
            if (!allowedToMove)
            {
                waterEnergy -= 6;
                UpdateWaterText();
                missSound.Play();
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
                SpriteRenderer[] srs = gridTiles[(int)newPost.x, (int)newPost.y].GetComponentsInChildren<SpriteRenderer>();
                foreach(SpriteRenderer sr in srs)
                {
                    if(sr.name == "Water" || sr.name == "Square")
                        sr.sprite = null;
                }
                waterEnergy += 36;
                if (waterEnergy > 100) waterEnergy = 100;
                UpdateWaterText();
            }

            return true;
        }
        if (Input.GetKeyDown(KeyCode.D) && currentPos.x < width - 1)
        {
            if (!allowedToMove)
            {
                waterEnergy -= 6;
                UpdateWaterText();
                missSound.Play();
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
                SpriteRenderer[] srs = gridTiles[(int)newPost.x, (int)newPost.y].GetComponentsInChildren<SpriteRenderer>();
                foreach(SpriteRenderer sr in srs)
                {
                    if(sr.name == "Water" || sr.name == "Square")
                        sr.sprite = null;
                }
                
                waterEnergy += 36;
                if (waterEnergy > 100) waterEnergy = 100;
                UpdateWaterText();
            }

            return true;
        }

        return false;
    }

    void Update()
    {
        if (gameFinished)
        {
            return;
        }
        
        if (isPaused)
        {
            if (Input.GetKeyDown(KeyCode.Escape) || pauseGameObject == null)
            {
                ResumeGame();
            }

            return;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseGame();
        }

        // win condition
        if (isMusicStarted && !music.isPlaying && !isPaused)
        {
            Win();
            music.Stop();
            winSound.Play();
            gameFinished = true;
        }

        // lose condition
        if (waterEnergy <= 0)
        {
            Lose();
            music.Stop();
            loseSound.Play();
            gameFinished = true;
        }

        timeElapsed += Time.deltaTime;

        attemptToMove();

        if (!isMusicStarted) {
            timeElapsedBeforeStartingMusic += Time.deltaTime;
            if (timeElapsedBeforeStartingMusic >= numOfBeatTillStartMusic * (60f/beatTempo)) {
                music.Play();
                isMusicStarted = true;
            }
            
        }
    }

    void PauseGame()
    {
        Time.timeScale = 0;
        isPaused = true;
        music.Pause();

        pauseGameObject = Instantiate(pauseScreen, new Vector3(0, 0, 0), Quaternion.identity);
        pauseGameObject.transform.SetParent(transform.parent);

    }

    void ResumeGame()
    {
        Time.timeScale = 1;
        isPaused = false;
        if (isMusicStarted) music.Play();

        if (pauseGameObject != null) Destroy(pauseGameObject);
    }


    public void UpdateWaterText()
    {
        // TODO
        var percentage = waterEnergy / 100f;

        var currentScaling = waterEnergyUI.transform.localScale;
        waterEnergyUI.transform.localScale = new Vector3(currentScaling.x, percentage * 5, currentScaling.z);
    }

    public void Lose()
    {
        var loseGameObject = Instantiate(loseScreen, new Vector3(0, 0, 0), Quaternion.identity);
        loseGameObject.transform.SetParent(transform.parent);
    }

    public void Win()
    {
        var winGameObject = Instantiate(winScreen, new Vector3(0, 0, 0), Quaternion.identity);
        winGameObject.transform.SetParent(transform.parent);
    }
}

