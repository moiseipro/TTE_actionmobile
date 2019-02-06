using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneController : MonoBehaviour {

    public GameObject[] bossPrefabs;
    public GameObject[] mapPrefabs;
    public GameObject[] bridgePrefabs;
    public GameObject[] additPrefabs;
    public GameObject[] objectPrefab;
    public GameObject[] playerPrefabs;

    public Camera mainCamera;
    GameObject player;

    private int levelGame = 0;
    int soedValue = 0;
    private bool playerSpawn = false, bosssSpawn = false;
    private bool startConnect = false, endConnect = false, bossIsland = false, stopGenerator = false;

    public int maxMapSize;
    private int[,] mapMas = new int[5, 5];
    private int[] rotMas = new int[4];
    private int stepNext;
    private float[] masAngle = { -90, 0, 90, 180 };
    private int xPos, zPos, predXPos, predZPos,
        freeCell = 0, lockCell = 0,
        stepVal = 0;
    string debugMat = "";

    Vector3 hitToPath;

    // Use this for initialization
    void Start () {
        levelGame = PlayerPrefs.GetInt("Level");
        foreach(GameObject item in GameObject.FindGameObjectsWithTag("Item"))
        {
            if (item.GetComponent<GUIcontroller>().ObjectEquipt == false) Destroy(item);
        }
        GenerationMap();
    }

    void GenerationMap()
    {
        GenerationMapMas();
        GenerationPortals();
        while(!stopGenerator)
        {
            GenerationMapGrid();
        }
        debugMat = "";
        for (int i = 0; i < 5; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                debugMat += mapMas[i, j] + "\t";
            }
            debugMat += "\n";
        }
        print(debugMat);
    }

    void GenerationMapGrid()
    {
        CheckGridForPath();
        GenerationIsland();
        print(debugMat);
        print(xPos + " " + zPos + " nextStep: " + stepNext + " free and lock Cell: " + freeCell + ", " + lockCell + " stepVal: " + stepVal);
    }

    void GenerationIsland()
    {
        if (mapMas[xPos, zPos] == 0)
        {
            int counter = 0;
            if (lockCell != 3 && freeCell != 3)
            {
                if (freeCell == 2)
                {
                    if (!bossIsland)
                    {
                        mapMas[xPos, zPos] = Random.Range(3, 5);
                        if (mapMas[xPos, zPos] == 4) bossIsland = true;
                    }
                    else
                    {
                        if (lockCell == 2)
                        {
                            int minRot = 0;
                            mapMas[xPos, zPos] = 2;
                            for(int i = 0; i<4; i++)
                            {
                                if(rotMas[i] >-1 && (rotMas[minRot] <0 || rotMas[minRot] > rotMas[i]))
                                {
                                    minRot = i;
                                }
                            }
                            stepNext = minRot;
                            counter = 20;
                        }
                        else mapMas[xPos, zPos] = 3;
                    }
                }
                else if (freeCell == 1)
                {
                    if (!bossIsland)
                    {
                        mapMas[xPos, zPos] = Random.Range(2, 5);
                        if (mapMas[xPos, zPos] == 4) bossIsland = true;
                    }
                    else mapMas[xPos, zPos] = 2;
                }
                else if (freeCell == 0)
                {
                    mapMas[xPos, zPos] = 1;
                }
                while (rotMas[stepNext] != 0 && counter < 20){
                    stepNext = Random.Range(0, 4);
                    counter++;
                } 

            }
            else
            {
                stopGenerator = true;
                if(freeCell == 3)
                {
                    int maxRot = 0;
                    mapMas[xPos, zPos] = 3;
                    for (int i = 0; i < 4; i++)
                    {
                        if (rotMas[i] > rotMas[maxRot])
                        {
                            maxRot = i;
                        }
                    }
                    stepNext = maxRot;
                }
                
            }
        } else
        {
            stopGenerator = true;
            return;
        }



        //rotMas[stepNext] = 2;

        if (mapMas[xPos, zPos] == 4)
        {
            GameObject island = Instantiate(mapPrefabs[0],new Vector3(27*xPos,-5,27*zPos),Quaternion.identity);
        } else if (mapMas[xPos, zPos] == 3)
        {
            GameObject bridge = Instantiate(bridgePrefabs[2], new Vector3(27 * xPos, -5, 27 * zPos), Quaternion.identity);
            if(stepNext == 0)
            {
                if (rotMas[1] > 0)
                {
                    bridge.transform.rotation = Quaternion.AngleAxis(masAngle[1], Vector3.up);
                    if (rotMas[2] > 0)
                    {
                        bridge.transform.rotation = Quaternion.AngleAxis(masAngle[1], Vector3.up);
                    }
                    else if (rotMas[3] > 0)
                    {
                        bridge.transform.rotation = Quaternion.AngleAxis(masAngle[3], Vector3.up);
                    }

                }
                else if (rotMas[2] > 0)
                {
                    if (rotMas[1] > 0)
                    {
                        bridge.transform.rotation = Quaternion.AngleAxis(masAngle[1], Vector3.up);
                    }
                    else if (rotMas[3] > 0)
                    {
                        bridge.transform.rotation = Quaternion.AngleAxis(masAngle[0], Vector3.up);
                    }
                }
                else if (rotMas[3] > 0)
                {
                    if (rotMas[1] > 0)
                    {
                        bridge.transform.rotation = Quaternion.AngleAxis(masAngle[3], Vector3.up);
                    }
                    else if (rotMas[2] > 0)
                    {
                        bridge.transform.rotation = Quaternion.AngleAxis(masAngle[0], Vector3.up);
                    }
                }
                if (rotMas[1] == -1)
                {
                    bridge.transform.rotation = Quaternion.AngleAxis(masAngle[0], Vector3.up);
                }
                else if (rotMas[2] == -1)
                {
                    bridge.transform.rotation = Quaternion.AngleAxis(masAngle[3], Vector3.up);
                }
                else if (rotMas[3] == -1)
                {
                    bridge.transform.rotation = Quaternion.AngleAxis(masAngle[1], Vector3.up);
                }
            } else if (stepNext == 1)
            {
                if (rotMas[3] > 0)
                {
                    if (rotMas[0] > 0)
                    {
                        bridge.transform.rotation = Quaternion.AngleAxis(masAngle[3], Vector3.up);
                    }
                    else if (rotMas[2] > 0)
                    {
                        bridge.transform.rotation = Quaternion.AngleAxis(masAngle[2], Vector3.up);
                    }
                } else if (rotMas[0] > 0)
                {
                    if (rotMas[2] > 0)
                    {
                        bridge.transform.rotation = Quaternion.AngleAxis(masAngle[1], Vector3.up);
                    }
                    else if (rotMas[3] > 0)
                    {
                        bridge.transform.rotation = Quaternion.AngleAxis(masAngle[3], Vector3.up);
                    }
                } else if (rotMas[2] > 0)
                {
                    if (rotMas[0] > 0)
                    {
                        bridge.transform.rotation = Quaternion.AngleAxis(masAngle[1], Vector3.up);
                    }
                    else if (rotMas[3] > 0)
                    {
                        bridge.transform.rotation = Quaternion.AngleAxis(masAngle[2], Vector3.up);
                    }
                }
                if (rotMas[0] == -1)
                {
                    bridge.transform.rotation = Quaternion.AngleAxis(masAngle[2], Vector3.up);
                }
                else if (rotMas[2] == -1)
                {
                    bridge.transform.rotation = Quaternion.AngleAxis(masAngle[3], Vector3.up);
                }
                else if (rotMas[3] == -1)
                {
                    bridge.transform.rotation = Quaternion.AngleAxis(masAngle[1], Vector3.up);
                }
            }
            else if (stepNext == 2)
            {
                if (rotMas[3] > 0)
                {
                    if (rotMas[0] > 0)
                    {
                        bridge.transform.rotation = Quaternion.AngleAxis(masAngle[0], Vector3.up);
                    } else if (rotMas[1] > 0)
                    {
                        bridge.transform.rotation = Quaternion.AngleAxis(masAngle[3], Vector3.up);
                    }

                }
                else if (rotMas[0] > 0)
                {
                    if (rotMas[1] > 0)
                    {
                        bridge.transform.rotation = Quaternion.AngleAxis(masAngle[1], Vector3.up);
                    }
                    else if (rotMas[3] > 0)
                    {
                        bridge.transform.rotation = Quaternion.AngleAxis(masAngle[0], Vector3.up);
                    }
                }
                else if (rotMas[1] > 0)
                {
                    if (rotMas[0] > 0)
                    {
                        bridge.transform.rotation = Quaternion.AngleAxis(masAngle[0], Vector3.up);
                    }
                    else if (rotMas[3] > 0)
                    {
                        bridge.transform.rotation = Quaternion.AngleAxis(masAngle[2], Vector3.up);
                    }
                }
                if (rotMas[0] == -1)
                {
                    bridge.transform.rotation = Quaternion.AngleAxis(masAngle[2], Vector3.up);
                }
                else if (rotMas[1] == -1)
                {
                    bridge.transform.rotation = Quaternion.AngleAxis(masAngle[0], Vector3.up);
                }
                else if (rotMas[3] == -1)
                {
                    bridge.transform.rotation = Quaternion.AngleAxis(masAngle[1], Vector3.up);
                }
            }
            else if (stepNext == 3)
            {
                if (rotMas[0] > 0)
                {
                    if (rotMas[1] > 0)
                    {
                        bridge.transform.rotation = Quaternion.AngleAxis(masAngle[3], Vector3.up);
                    }
                    else if (rotMas[2] > 0)
                    {
                        bridge.transform.rotation = Quaternion.AngleAxis(masAngle[0], Vector3.up);
                    }

                }
                else if (rotMas[1] > 0)
                {
                    if (rotMas[0] > 0)
                    {
                        bridge.transform.rotation = Quaternion.AngleAxis(masAngle[3], Vector3.up);
                    }
                    else if (rotMas[2] > 0)
                    {
                        bridge.transform.rotation = Quaternion.AngleAxis(masAngle[2], Vector3.up);
                    }
                }
                else if (rotMas[2] > 0)
                {
                    if (rotMas[1] > 0)
                    {
                        bridge.transform.rotation = Quaternion.AngleAxis(masAngle[2], Vector3.up);
                    }
                    else if (rotMas[0] > 0)
                    {
                        bridge.transform.rotation = Quaternion.AngleAxis(masAngle[0], Vector3.up);
                    }
                }
                if (rotMas[0] == -1)
                {
                    bridge.transform.rotation = Quaternion.AngleAxis(masAngle[2], Vector3.up);
                }
                else if (rotMas[2] == -1)
                {
                    bridge.transform.rotation = Quaternion.AngleAxis(masAngle[3], Vector3.up);
                }
                else if (rotMas[1] == -1)
                {
                    bridge.transform.rotation = Quaternion.AngleAxis(masAngle[0], Vector3.up);
                }
            }

        } else if (mapMas[xPos, zPos] == 2)
        {
            if (stepNext == 0)
            {
                if (rotMas[1] > 0)
                {
                    GameObject bridge = Instantiate(bridgePrefabs[0], new Vector3(27 * xPos, -5, 27 * zPos), Quaternion.identity);
                    bridge.transform.rotation = Quaternion.AngleAxis(masAngle[0], Vector3.up);
                }
                else if (rotMas[2] > 0)
                {
                    GameObject bridge = Instantiate(bridgePrefabs[1], new Vector3(27 * xPos, -5, 27 * zPos), Quaternion.identity);
                    bridge.transform.rotation = Quaternion.AngleAxis(masAngle[0], Vector3.up);
                }
                else if (rotMas[3] > 0)
                {
                    GameObject bridge = Instantiate(bridgePrefabs[1], new Vector3(27 * xPos, -5, 27 * zPos), Quaternion.identity);
                    bridge.transform.rotation = Quaternion.AngleAxis(masAngle[3], Vector3.up);
                }
            } else if (stepNext == 1)
            {
                if (rotMas[0] > 0)
                {
                    GameObject bridge = Instantiate(bridgePrefabs[0], new Vector3(27 * xPos, -5, 27 * zPos), Quaternion.identity);
                    bridge.transform.rotation = Quaternion.AngleAxis(masAngle[0], Vector3.up);
                }
                else if (rotMas[2] > 0)
                {
                    GameObject bridge = Instantiate(bridgePrefabs[1], new Vector3(27 * xPos, -5, 27 * zPos), Quaternion.identity);
                    bridge.transform.rotation = Quaternion.AngleAxis(masAngle[1], Vector3.up);
                }
                else if (rotMas[3] > 0)
                {
                    GameObject bridge = Instantiate(bridgePrefabs[1], new Vector3(27 * xPos, -5, 27 * zPos), Quaternion.identity);
                    bridge.transform.rotation = Quaternion.AngleAxis(masAngle[2], Vector3.up);
                }
            } else if (stepNext == 2)
            {
                if (rotMas[1] > 0)
                {
                    GameObject bridge = Instantiate(bridgePrefabs[1], new Vector3(27 * xPos, -5, 27 * zPos), Quaternion.identity);
                    bridge.transform.rotation = Quaternion.AngleAxis(masAngle[1], Vector3.up);
                }
                else if (rotMas[0] > 0)
                {
                    GameObject bridge = Instantiate(bridgePrefabs[1], new Vector3(27 * xPos, -5, 27 * zPos), Quaternion.identity);
                    bridge.transform.rotation = Quaternion.AngleAxis(masAngle[0], Vector3.up);
                }
                else if (rotMas[3] > 0)
                {
                    GameObject bridge = Instantiate(bridgePrefabs[0], new Vector3(27 * xPos, -5, 27 * zPos), Quaternion.identity);
                    bridge.transform.rotation = Quaternion.AngleAxis(masAngle[1], Vector3.up);
                }
            } else if (stepNext == 3)
            {
                if (rotMas[1] > 0)
                {
                    GameObject bridge = Instantiate(bridgePrefabs[1], new Vector3(27 * xPos, -5, 27 * zPos), Quaternion.identity);
                    bridge.transform.rotation = Quaternion.AngleAxis(masAngle[2], Vector3.up);
                }
                else if (rotMas[2] > 0)
                {
                    GameObject bridge = Instantiate(bridgePrefabs[0], new Vector3(27 * xPos, -5, 27 * zPos), Quaternion.identity);
                    bridge.transform.rotation = Quaternion.AngleAxis(masAngle[1], Vector3.up);
                }
                else if (rotMas[0] > 0)
                {
                    GameObject bridge = Instantiate(bridgePrefabs[1], new Vector3(27 * xPos, -5, 27 * zPos), Quaternion.identity);
                    bridge.transform.rotation = Quaternion.AngleAxis(masAngle[3], Vector3.up);
                }
            }
        }
        if (mapMas[predXPos, predZPos] == 2 || mapMas[predXPos, predZPos] == 1) mapMas[predXPos, predZPos] = -1;
        else if (mapMas[predXPos, predZPos] == 3) mapMas[predXPos, predZPos] = 1;
        predXPos = xPos;
        predZPos = zPos;
        if (stepNext == 0) xPos--;
        else if (stepNext == 1) xPos++;
        else if (stepNext == 2) zPos++;
        else if (stepNext == 3) zPos--;
        stepVal++;
    }

    void CheckGridForPath()
    {
        freeCell = 0;
        lockCell = 0;
        for (int i = 0; i < 4; i++)
        {
            if (i == 0)
            {
                if(mapMas[xPos - 1, zPos] == 0)
                {
                    rotMas[i] = 0;
                    //freeCell++;
                } else if (mapMas[xPos - 1, zPos] == 1)
                {
                    rotMas[i] = 2;
                    freeCell++;
                } else if (mapMas[xPos - 1, zPos] > 1)
                {
                    rotMas[i] = 1;
                    freeCell++;
                } else
                {
                    rotMas[i] = -1;
                    lockCell++;
                }

            } else if (i == 1)
            {
                if(mapMas[xPos + 1, zPos] == 0)
                {
                    rotMas[i] = 0;
                    //freeCell++;
                } else if (mapMas[xPos + 1, zPos] == 1)
                {
                    rotMas[i] = 2;
                    freeCell++;
                } else if (mapMas[xPos + 1, zPos] > 1)
                {
                    rotMas[i] = 1;
                    freeCell ++;
                } else
                {
                    rotMas[i] = -1;
                    lockCell++;
                }

            } else if (i == 2)
            {
                if(mapMas[xPos, zPos + 1] == 0)
                {
                    rotMas[i] = 0;
                    //freeCell++;
                } else if (mapMas[xPos, zPos + 1] == 1)
                {
                    rotMas[i] = 2;
                    freeCell++;
                } else if (mapMas[xPos, zPos + 1] > 1)
                {
                    rotMas[i] = 1;
                    freeCell++;
                } else
                {
                    rotMas[i] = -1;
                    lockCell++;
                }

            } else if (i == 3)
            {
                if(mapMas[xPos, zPos - 1] == 0)
                {
                    rotMas[i] = 0;
                    //freeCell++;
                } else if (mapMas[xPos, zPos - 1] == 1)
                {
                    rotMas[i] = 2;
                    freeCell++;
                } else if (mapMas[xPos, zPos - 1] > 1)
                {
                    rotMas[i] = 1;
                    freeCell++;
                } else
                {
                    rotMas[i] = -1;
                    lockCell++;
                }
            }
            debugMat += " " + rotMas[i];
        }
        debugMat += "\n";
    }

    void GenerationPortals()
    {
        for (int i = 0; i < 5; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                if(mapMas[i, j] == 1)
                {
                    GameObject portal = Instantiate(additPrefabs[0], new Vector3(27 * i, -5, 27 * j), Quaternion.identity);
                    if(i==0) portal.transform.rotation = Quaternion.AngleAxis(masAngle[0], Vector3.up);
                    else if (i == 4) portal.transform.rotation = Quaternion.AngleAxis(masAngle[2], Vector3.up);
                    else if (j == 0) portal.transform.rotation = Quaternion.AngleAxis(masAngle[3], Vector3.up);
                    else if (j == 4) portal.transform.rotation = Quaternion.AngleAxis(masAngle[1], Vector3.up);
                }
            }
        }
    }

    void GenerationMapMas()
    {
        int randomNumX = 4, randomNumZ = 6;
        for(int i = 0; i < 5; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                if (i == 0 || j == 0)
                {
                    if (startConnect == false && (i!=0 || j!=0) && i!=4 && j!=4 && Random.Range(0, randomNumX - i) == 0)
                    {
                        mapMas[i, j] = 1;
                        startConnect = true;
                        if (i == 0)
                        {
                            xPos = i+1;
                            zPos = j;
                        }
                        if (j == 0)
                        {
                            xPos = i;
                            zPos = j+1;
                        }
                        predXPos = i;
                        predZPos = j;
                    } else mapMas[i, j] = -1;
                }
                else if (i == 4 || j == 4)
                {
                    if (endConnect == false && (i != 4 || j != 4) && i != 0 && j != 0 && Random.Range(0, randomNumZ) == 0)
                    {
                        mapMas[i, j] = 1;
                        endConnect = true;
                    } else mapMas[i, j] = -1;
                    randomNumZ--;
                }
                else
                {
                    mapMas[i, j] = 0;
                }
                debugMat += mapMas[i, j] + "\t";
            }
            debugMat += "\n";
        }
    }

    void SpawnBoss(int x, int z)
    {
        GameObject boss = Instantiate(bossPrefabs[Random.Range(0, bossPrefabs.Length)], new Vector3(x, 0, z), Quaternion.AngleAxis(180, Vector3.up));
        boss.GetComponent<BossHeartController>().bossLevel = levelGame;
        bosssSpawn = true;
    }

    void SpawnPlayer(int x, int z)
    {
        if (!GameObject.FindWithTag("Player"))
        {
            player = Instantiate(playerPrefabs[0], new Vector3(x, 0, z), Quaternion.identity);
        }
        else
        {
            player = GameObject.FindWithTag("Player");
        }
        player.GetComponent<HeartSystem>().heartImages = GameObject.Find("HealthBar").transform.GetComponentsInChildren<Image>();
        player.GetComponent<HeartSystem>().CheckHealthAmount();
        player.GetComponent<Move_Controller>().joystickMove = GameObject.Find("MovePlayer").GetComponent<Joystick>();
        player.GetComponent<Move_Controller>().joystickFire = GameObject.Find("FirePlayer").GetComponent<Joystick>();
        player.transform.position = new Vector3(x, 0, z);
        playerSpawn = true;
        mainCamera.GetComponent<Camera_Controller>().Player = player;
    }

	// Update is called once per frame
	void Update () {
		
	}

    public void ReloadLevel()
    {
        Destroy(player);
        //player.GetComponent<HeartSystem>().HealAll();
        PlayerPrefs.SetInt("Level", 0);
        SceneManager.LoadScene("Game");
    }

    public void NextLevel()
    {
        levelGame++;
        PlayerPrefs.SetInt("Level", levelGame);
        SceneManager.LoadScene("Game");
    }
}
