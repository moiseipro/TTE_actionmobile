using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneController : MonoBehaviour {

    public GameObject[] bossPrefabs;
    public GameObject[] mapPrefabs;
    public GameObject[] bridgePrefabs;
    public GameObject[] roomPrefabs;
    public GameObject[] additPrefabs;
    public GameObject[] objectPrefab;
    public GameObject[] objectInterPrefab;
    public GameObject[] objectTrapPrefab;
    public GameObject[] objectChestPrefab;
    public GameObject[] objectLightPrefab;
    public GameObject[] playerPrefabs;

    public Camera mainCamera;
    public GameObject GUI;
    GameObject player;

    
    int soedValue = 0;
    private bool playerSpawn = false, bosssSpawn = false, nextLevelPortal = false;
    private bool startConnect = false, endConnect = false, bossIsland = false, stopGenerator = false, startAndEndConnected = false;

    public int maxMapSize;
    private int[,] mapMas = new int[5, 5];
    private int[] rotMas = new int[4];
    private int stepNext;
    private float[] masAngle = { -90, 0, 90, 180 };
    private int xPos, zPos, predXPos, predZPos, portalX, portalZ,
        freeCell = 0, lockCell = 0,
        stepVal = 0, maxBossIslandGeneration = 0, generationRoomChanse = 5;
    string debugMat = "";

    Vector3 hitToPath;

    // Use this for initialization
    void Start () {
        gameObject.transform.rotation = Quaternion.identity;
        gameObject.transform.rotation = Quaternion.AngleAxis(50, Vector3.up);
        GenerationMap();
        foreach (GameObject item in GameObject.FindGameObjectsWithTag("Item"))
        {
            if (item.GetComponent<GUIcontroller>().ObjectEquipt == false) Destroy(item);
        }
    }

    void GenerationMap()
    {
        int countGen = 0;
        GenerationMapMas();
        GenerationPortals();
        while(!stopGenerator && countGen < 100)
        {
            GenerationMapGrid();
            countGen++;
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
        if(startAndEndConnected == false) CheckClosestWay();
        else CheckGridForPath();
        GenerationIsland();
        print(debugMat);
        print(xPos + " " + zPos + " nextStep: " + stepNext + " free and lock Cell: " + freeCell + ", " + lockCell + " stepVal: " + stepVal + "\n" + portalX + " " + portalZ);
    }

    void FindBossIsland()
    {
        for(int i = 0; i < 5; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                if (mapMas[i, j] == 4)
                {
                    xPos = i;
                    zPos = j;
                    predXPos = i;
                    predZPos = j;
                    mapMas[i, j] = 2;
                    CheckGridForPath();
                    if(rotMas[0] == 0)
                    {
                        xPos = i-1;
                        zPos = j;
                    } else if (rotMas[1] == 0)
                    {
                        xPos = i + 1;
                        zPos = j;
                    } else if (rotMas[2] == 0)
                    {
                        xPos = i;
                        zPos = j + 1;
                    } else if (rotMas[3] == 0)
                    {
                        xPos = i;
                        zPos = j - 1;
                    }
                } 
            }
        }
        CheckGridForPath();
    }

    void GenerationIsland()
    {
        stepNext = Random.Range(0, 4);
        if (mapMas[xPos, zPos] == 0)
        {
            bool counter = false;
            if (lockCell != 3 && freeCell != 3)
            {
                if (freeCell == 2)
                {
                    if (!bossIsland)
                    {
                        if (maxBossIslandGeneration > 2) mapMas[xPos, zPos] = 4;
                        else mapMas[xPos, zPos] = Random.Range(3, 5);
                        if (mapMas[xPos, zPos] == 4) bossIsland = true;
                        maxBossIslandGeneration++;
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
                            counter = true;
                        }
                        else mapMas[xPos, zPos] = 3;
                    }
                }
                else if (freeCell == 1)
                {
                    if (!bossIsland)
                    {
                        if (maxBossIslandGeneration > 2) mapMas[xPos, zPos] = 4;
                        else mapMas[xPos, zPos] = Random.Range(2, 5);
                        if (mapMas[xPos, zPos] == 4) bossIsland = true;
                        maxBossIslandGeneration++;
                    }
                    else
                    {
                        if (lockCell == 2)
                        {
                            int minRot = 0;
                            mapMas[xPos, zPos] = 2;
                            for (int i = 0; i < 4; i++)
                            {
                                if (rotMas[i] > -1 && (rotMas[minRot] < 0 || rotMas[minRot] > rotMas[i]))
                                {
                                    minRot = i;
                                }
                            }
                            stepNext = minRot;
                            counter = true;
                        }
                        else if (lockCell == 3)
                        {
                            mapMas[xPos, zPos] = 1;
                        }
                        else
                        {
                            mapMas[xPos, zPos] = 2;
                            /*mapMas[xPos, zPos] = Random.Range(0, 11);
                            if (mapMas[xPos, zPos] > 3)
                            {
                                mapMas[xPos, zPos] = 2;
                            } else
                            {
                                mapMas[xPos, zPos] = 3;
                            }*/
                            
                        }
                    }
                }
                if (rotMas[0] == 0 || rotMas[1] == 0 || rotMas[2] == 0 || rotMas[3] == 0)
                {
                    while (rotMas[stepNext] != 0 && counter == false)
                    {
                        stepNext = Random.Range(0, 4);
                    }
                } 
            }
            else
            {
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
                if (lockCell == 3)
                {
                    stopGenerator = true;
                    mapMas[xPos, zPos] = 1;
                    int maxRot = 0;
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
            if(startAndEndConnected == true) stopGenerator = true;
            startAndEndConnected = true;
            FindBossIsland();
            return;
        }


        if (mapMas[xPos, zPos] == 4)
        {
            GameObject island = Instantiate(mapPrefabs[Random.Range(0,mapPrefabs.Length)],new Vector3(27*xPos,-5,27*zPos),Quaternion.identity);
        } else if (mapMas[xPos, zPos] == 3)
        {
            int bridgeRoom = Random.Range(0,11);
            GameObject bridge;
            if (bridgeRoom > generationRoomChanse)
            {
                bridge = Instantiate(roomPrefabs[5], new Vector3(27 * xPos, -5, 27 * zPos), Quaternion.identity);
                generationRoomChanse++;
            } else
            {
                bridge = Instantiate(bridgePrefabs[2], new Vector3(27 * xPos, -5, 27 * zPos), Quaternion.identity);
            }
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
                    bridge.transform.rotation = Quaternion.AngleAxis(masAngle[1], Vector3.up);
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
                
                if (rotMas[1] == -2)
                {
                    bridge.transform.rotation = Quaternion.AngleAxis(masAngle[0], Vector3.up);
                }
                else if (rotMas[2] == -2)
                {
                    bridge.transform.rotation = Quaternion.AngleAxis(masAngle[3], Vector3.up);
                }
                else if (rotMas[3] == -2)
                {
                    bridge.transform.rotation = Quaternion.AngleAxis(masAngle[1], Vector3.up);
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
                
                if (rotMas[0] == -2)
                {
                    bridge.transform.rotation = Quaternion.AngleAxis(masAngle[2], Vector3.up);
                }
                else if (rotMas[2] == -2)
                {
                    bridge.transform.rotation = Quaternion.AngleAxis(masAngle[3], Vector3.up);
                }
                else if (rotMas[3] == -2)
                {
                    bridge.transform.rotation = Quaternion.AngleAxis(masAngle[1], Vector3.up);
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
                        bridge.transform.rotation = Quaternion.AngleAxis(masAngle[2], Vector3.up);
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
                
                if (rotMas[0] == -2)
                {
                    bridge.transform.rotation = Quaternion.AngleAxis(masAngle[2], Vector3.up);
                }
                else if (rotMas[1] == -2)
                {
                    bridge.transform.rotation = Quaternion.AngleAxis(masAngle[0], Vector3.up);
                }
                else if (rotMas[3] == -2)
                {
                    bridge.transform.rotation = Quaternion.AngleAxis(masAngle[1], Vector3.up);
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
                
                if (rotMas[0] == -2)
                {
                    bridge.transform.rotation = Quaternion.AngleAxis(masAngle[2], Vector3.up);
                }
                else if (rotMas[2] == -2)
                {
                    bridge.transform.rotation = Quaternion.AngleAxis(masAngle[3], Vector3.up);
                }
                else if (rotMas[1] == -2)
                {
                    bridge.transform.rotation = Quaternion.AngleAxis(masAngle[0], Vector3.up);
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
            int bridgeRoom = Random.Range(0,11);
            if (stepNext == 0)
            {
                if (rotMas[1] > 0)
                {
                    if (bridgeRoom > generationRoomChanse)
                    {
                        GameObject room = Instantiate(roomPrefabs[Random.Range(1,4)], new Vector3(27 * xPos, -5, 27 * zPos), Quaternion.identity);
                        room.transform.rotation = Quaternion.AngleAxis(masAngle[2], Vector3.up);
                        generationRoomChanse++;
                    } else
                    {
                        GameObject bridge = Instantiate(bridgePrefabs[0], new Vector3(27 * xPos, -5, 27 * zPos), Quaternion.identity);
                        bridge.transform.rotation = Quaternion.AngleAxis(masAngle[0], Vector3.up);
                    }
                    
                }
                else if (rotMas[2] > 0)
                {
                    if (bridgeRoom > generationRoomChanse)
                    {
                        GameObject room = Instantiate(roomPrefabs[4], new Vector3(27 * xPos, -5, 27 * zPos), Quaternion.identity);
                        room.transform.rotation = Quaternion.AngleAxis(masAngle[0], Vector3.up);
                    }
                    else
                    {
                        GameObject bridge = Instantiate(bridgePrefabs[1], new Vector3(27 * xPos, -5, 27 * zPos), Quaternion.identity);
                        bridge.transform.rotation = Quaternion.AngleAxis(masAngle[0], Vector3.up);
                    }
                    
                }
                else if (rotMas[3] > 0)
                {
                    if (bridgeRoom > generationRoomChanse)
                    {
                        GameObject room = Instantiate(roomPrefabs[4], new Vector3(27 * xPos, -5, 27 * zPos), Quaternion.identity);
                        room.transform.rotation = Quaternion.AngleAxis(masAngle[3], Vector3.up);
                        generationRoomChanse++;
                    }
                    else
                    {
                        GameObject bridge = Instantiate(bridgePrefabs[1], new Vector3(27 * xPos, -5, 27 * zPos), Quaternion.identity);
                        bridge.transform.rotation = Quaternion.AngleAxis(masAngle[3], Vector3.up);
                    }
                }
            } else if (stepNext == 1)
            {
                if (rotMas[0] > 0)
                {
                    if (bridgeRoom > generationRoomChanse)
                    {
                        GameObject room = Instantiate(roomPrefabs[Random.Range(1,4)], new Vector3(27 * xPos, -5, 27 * zPos), Quaternion.identity);
                        room.transform.rotation = Quaternion.AngleAxis(masAngle[2], Vector3.up);
                        generationRoomChanse++;
                    }
                    else
                    {
                        GameObject bridge = Instantiate(bridgePrefabs[0], new Vector3(27 * xPos, -5, 27 * zPos), Quaternion.identity);
                        bridge.transform.rotation = Quaternion.AngleAxis(masAngle[0], Vector3.up);
                    }
                }
                else if (rotMas[2] > 0)
                {
                    if (bridgeRoom > generationRoomChanse)
                    {
                        GameObject room = Instantiate(roomPrefabs[4], new Vector3(27 * xPos, -5, 27 * zPos), Quaternion.identity);
                        room.transform.rotation = Quaternion.AngleAxis(masAngle[1], Vector3.up);
                        generationRoomChanse++;
                    }
                    else
                    {
                        GameObject bridge = Instantiate(bridgePrefabs[1], new Vector3(27 * xPos, -5, 27 * zPos), Quaternion.identity);
                        bridge.transform.rotation = Quaternion.AngleAxis(masAngle[1], Vector3.up);
                    }
                }
                else if (rotMas[3] > 0)
                {
                    if (bridgeRoom > generationRoomChanse)
                    {
                        GameObject room = Instantiate(roomPrefabs[4], new Vector3(27 * xPos, -5, 27 * zPos), Quaternion.identity);
                        room.transform.rotation = Quaternion.AngleAxis(masAngle[2], Vector3.up);
                        generationRoomChanse++;
                    }
                    else
                    {
                        GameObject bridge = Instantiate(bridgePrefabs[1], new Vector3(27 * xPos, -5, 27 * zPos), Quaternion.identity);
                        bridge.transform.rotation = Quaternion.AngleAxis(masAngle[2], Vector3.up);
                    }
                }
            } else if (stepNext == 2)
            {
                if (rotMas[1] > 0)
                {
                    if (bridgeRoom > generationRoomChanse)
                    {
                        GameObject room = Instantiate(roomPrefabs[4], new Vector3(27 * xPos, -5, 27 * zPos), Quaternion.identity);
                        room.transform.rotation = Quaternion.AngleAxis(masAngle[1], Vector3.up);
                        generationRoomChanse++;
                    }
                    else
                    {
                        GameObject bridge = Instantiate(bridgePrefabs[1], new Vector3(27 * xPos, -5, 27 * zPos), Quaternion.identity);
                        bridge.transform.rotation = Quaternion.AngleAxis(masAngle[1], Vector3.up);
                    }
                }
                else if (rotMas[0] > 0)
                {
                    if (bridgeRoom > generationRoomChanse)
                    {
                        GameObject room = Instantiate(roomPrefabs[4], new Vector3(27 * xPos, -5, 27 * zPos), Quaternion.identity);
                        room.transform.rotation = Quaternion.AngleAxis(masAngle[0], Vector3.up);
                        generationRoomChanse++;
                    }
                    else
                    {
                        GameObject bridge = Instantiate(bridgePrefabs[1], new Vector3(27 * xPos, -5, 27 * zPos), Quaternion.identity);
                        bridge.transform.rotation = Quaternion.AngleAxis(masAngle[0], Vector3.up);
                    }
                }
                else if (rotMas[3] > 0)
                {
                    if (bridgeRoom > generationRoomChanse)
                    {
                        GameObject room = Instantiate(roomPrefabs[Random.Range(1,4)], new Vector3(27 * xPos, -5, 27 * zPos), Quaternion.identity);
                        room.transform.rotation = Quaternion.AngleAxis(masAngle[1], Vector3.up);
                        generationRoomChanse++;
                    }
                    else
                    {
                        GameObject bridge = Instantiate(bridgePrefabs[0], new Vector3(27 * xPos, -5, 27 * zPos), Quaternion.identity);
                        bridge.transform.rotation = Quaternion.AngleAxis(masAngle[1], Vector3.up);
                    }
                }
            } else if (stepNext == 3)
            {
                if (rotMas[1] > 0)
                {
                    if (bridgeRoom > generationRoomChanse)
                    {
                        GameObject room = Instantiate(roomPrefabs[4], new Vector3(27 * xPos, -5, 27 * zPos), Quaternion.identity);
                        room.transform.rotation = Quaternion.AngleAxis(masAngle[2], Vector3.up);
                        generationRoomChanse++;
                    }
                    else
                    {
                        GameObject bridge = Instantiate(bridgePrefabs[1], new Vector3(27 * xPos, -5, 27 * zPos), Quaternion.identity);
                        bridge.transform.rotation = Quaternion.AngleAxis(masAngle[2], Vector3.up);
                    }
                }
                else if (rotMas[2] > 0)
                {
                    if (bridgeRoom > generationRoomChanse)
                    {
                        GameObject room = Instantiate(roomPrefabs[Random.Range(1,4)], new Vector3(27 * xPos, -5, 27 * zPos), Quaternion.identity);
                        room.transform.rotation = Quaternion.AngleAxis(masAngle[1], Vector3.up);
                        generationRoomChanse++;
                    }
                    else
                    {
                        GameObject bridge = Instantiate(bridgePrefabs[0], new Vector3(27 * xPos, -5, 27 * zPos), Quaternion.identity);
                        bridge.transform.rotation = Quaternion.AngleAxis(masAngle[1], Vector3.up);
                    }
                }
                else if (rotMas[0] > 0)
                {
                    if (bridgeRoom > generationRoomChanse)
                    {
                        GameObject room = Instantiate(roomPrefabs[4], new Vector3(27 * xPos, -5, 27 * zPos), Quaternion.identity);
                        room.transform.rotation = Quaternion.AngleAxis(masAngle[3], Vector3.up);
                        generationRoomChanse++;
                    }
                    else
                    {
                        GameObject bridge = Instantiate(bridgePrefabs[1], new Vector3(27 * xPos, -5, 27 * zPos), Quaternion.identity);
                        bridge.transform.rotation = Quaternion.AngleAxis(masAngle[3], Vector3.up);
                    }
                }
            }
        } else if (mapMas[xPos, zPos] == 1)
        {
            GameObject room = Instantiate(roomPrefabs[0], new Vector3(27 * xPos, -5, 27 * zPos), Quaternion.identity);
            if(rotMas[0] > 0) room.transform.rotation = Quaternion.AngleAxis(masAngle[0], Vector3.up);
            else if (rotMas[1] > 0) room.transform.rotation = Quaternion.AngleAxis(masAngle[2], Vector3.up);
            else if (rotMas[2] > 0) room.transform.rotation = Quaternion.AngleAxis(masAngle[1], Vector3.up);
            else if (rotMas[3] > 0) room.transform.rotation = Quaternion.AngleAxis(masAngle[3], Vector3.up);
        }

        predCoordFix();

        predXPos = xPos;
        predZPos = zPos;
        if ((rotMas[0] == 5 && rotMas[2] == 5) || (rotMas[1] == 5 && rotMas[3] == 5))
        {
            startAndEndConnected = true;
            FindBossIsland();
            return;
        }else if ((rotMas[0] == 5 || rotMas[1] == 5 || rotMas[2] == 5 || rotMas[3] == 5) && stepVal > 0)
        {
            //stopGenerator = true;
            predCoordFix();
            if (startAndEndConnected == true) stopGenerator = true;
            startAndEndConnected = true;
            FindBossIsland();
            return;
        }
        if (stepNext == 0) xPos--;
        else if (stepNext == 1) xPos++;
        else if (stepNext == 2) zPos++;
        else if (stepNext == 3) zPos--;
        stepVal++;
    }

    void predCoordFix()
    {
        if (mapMas[predXPos, predZPos] == 2 || mapMas[predXPos, predZPos] == 1 || (mapMas[predXPos, predZPos] == 3 && freeCell == 2)) mapMas[predXPos, predZPos] = -1;
        else if (mapMas[predXPos, predZPos] == 3) mapMas[predXPos, predZPos] = 1;
    }

    void CheckClosestWay()
    {
        if (xPos == portalX && zPos == portalZ)
        {
            //stopGenerator = true;
            startAndEndConnected = true;
            FindBossIsland();
            return;
        }

        freeCell = 0;
        lockCell = 0;
        
        if ((xPos - 1) >= portalX)
        {
            if (mapMas[xPos - 1, zPos] == -1)
            {
                rotMas[0] = -1;
                lockCell++;
            }
            else if (mapMas[xPos - 1, zPos] == 5)
            {
                rotMas[0] = 5;
                freeCell++;
                maxBossIslandGeneration += 2;
            }
            else if (mapMas[xPos - 1, zPos] > 0)
            {
                rotMas[0] = 1;
                freeCell++;
            } else rotMas[0] = 0;
        } else
        {
            if (mapMas[xPos - 1, zPos] == 0)
            {
                rotMas[0] = -2;
                lockCell++;
            }
            else if (mapMas[xPos - 1, zPos] == 5)
            {
                rotMas[0] = 5;
                freeCell++;
                maxBossIslandGeneration += 2;
            }
            else if (mapMas[xPos - 1, zPos] > 0)
            {
                rotMas[0] = 1;
                freeCell++;
            }
            else
            {
                rotMas[0] = -1;
                lockCell++;
            }
        }
        if ((xPos + 1) <= portalX)
        {
            if (mapMas[xPos + 1, zPos] == -1)
            {
                rotMas[1] = -1;
                lockCell++;
            }
            else if (mapMas[xPos + 1, zPos] == 5)
            {
                rotMas[1] = 5;
                freeCell++;
                maxBossIslandGeneration+=2;
            }
            else if (mapMas[xPos + 1, zPos] > 0)
            {
                rotMas[1] = 1;
                freeCell++;
            }else rotMas[1] = 0;
        } else
        {
            if (mapMas[xPos + 1, zPos] == 0)
            {
                rotMas[1] = -2;
                lockCell++;
            }
            else if (mapMas[xPos + 1, zPos] == 5)
            {
                rotMas[1] = 5;
                freeCell++;
                maxBossIslandGeneration += 2;
            }
            else if (mapMas[xPos + 1, zPos] > 0)
            {
                rotMas[1] = 1;
                freeCell++;
            }
            else
            {
                rotMas[1] = -1;
                lockCell++;
            }
        }
        if ((zPos + 1) <= portalZ)
        {
            if (mapMas[xPos, zPos + 1] == -1)
            {
                rotMas[2] = -1;
                lockCell++;
            }
            else if (mapMas[xPos, zPos + 1] == 5)
            {
                rotMas[2] = 5;
                freeCell++;
                maxBossIslandGeneration += 2;
            }
            else if (mapMas[xPos, zPos + 1] > 0)
            {
                rotMas[2] = 1;
                freeCell++;
            } else rotMas[2] = 0;
        } else
        {
            if (mapMas[xPos, zPos + 1] == 0)
            {
                rotMas[2] = -2;
                lockCell++;
            }
            else if (mapMas[xPos, zPos + 1] == 5)
            {
                rotMas[2] = 5;
                freeCell++;
                maxBossIslandGeneration += 2;
            }
            else if (mapMas[xPos, zPos + 1] > 0)
            {
                rotMas[2] = 1;
                freeCell++;
            }
            else
            {
                rotMas[2] = -1;
                lockCell++;
            }
        }
        if ((zPos - 1) >= portalZ)
        {
            if (mapMas[xPos, zPos - 1] == -1)
            {
                rotMas[3] = -1;
                lockCell++;
            }
            else if (mapMas[xPos, zPos - 1] == 5)
            {
                rotMas[3] = 5;
                freeCell++;
                maxBossIslandGeneration += 2;
            }
            else if (mapMas[xPos, zPos - 1] > 0)
            {
                rotMas[3] = 1;
                freeCell++;
            } else rotMas[3] = 0;
        } else
        {
            if (mapMas[xPos, zPos - 1] == 0)
            {
                rotMas[3] = -2;
                lockCell++;
            }
            else if (mapMas[xPos, zPos - 1] == 5)
            {
                rotMas[3] = 5;
                freeCell++;
                maxBossIslandGeneration += 2;
            }
            else if (mapMas[xPos, zPos - 1] > 0)
            {
                rotMas[3] = 1;
                freeCell++;
            }
            else
            {
                rotMas[3] = -1;
                lockCell++;
            }
        }
        debugMat += " " + rotMas[0] + " " + rotMas[1] + " " + rotMas[2] + " " + rotMas[3] + "\n";
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
                } else if (mapMas[xPos - 1, zPos] == 5)
                {
                    rotMas[i] = 5;
                    freeCell++;
                } else if (mapMas[xPos - 1, zPos] > 0)
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
                } else if (mapMas[xPos + 1, zPos] == 5)
                {
                    rotMas[i] = 5;
                    freeCell++;
                } else if (mapMas[xPos + 1, zPos] > 0)
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
                } else if (mapMas[xPos, zPos + 1] == 5)
                {
                    rotMas[i] = 5;
                    freeCell++;
                } else if (mapMas[xPos, zPos + 1] > 0)
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
                } else if (mapMas[xPos, zPos - 1] == 5)
                {
                    rotMas[i] = 5;
                    freeCell++;
                } else if (mapMas[xPos, zPos - 1] > 0)
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
                if(mapMas[i, j] == 5)
                {
                    GameObject portal = Instantiate(additPrefabs[0], new Vector3(27 * i, -5, 27 * j), Quaternion.identity);
                    if (nextLevelPortal == true) portal.GetComponentInChildren<PortalController>().portalID = 1;
                    nextLevelPortal = true;
                    if(i==0) portal.transform.rotation = Quaternion.AngleAxis(masAngle[0], Vector3.up);
                    else if (i == 4) portal.transform.rotation = Quaternion.AngleAxis(masAngle[2], Vector3.up);
                    else if (j == 0) portal.transform.rotation = Quaternion.AngleAxis(masAngle[3], Vector3.up);
                    else if (j == 4) portal.transform.rotation = Quaternion.AngleAxis(masAngle[1], Vector3.up);
                }
                if (mapMas[i, j] == 6)
                {
                    GameObject portal = Instantiate(additPrefabs[0], new Vector3(27 * i, -5, 27 * j), Quaternion.identity);
                    portal.GetComponentInChildren<PortalController>().portalID = 1;
                    if (i == 0) portal.transform.rotation = Quaternion.AngleAxis(masAngle[0], Vector3.up);
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
                        mapMas[i, j] = 5;
                        startConnect = true;
                        SpawnPlayer(i, j);
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
                        mapMas[i, j] = 6;
                        endConnect = true;
                        
                            portalZ = j;

                            portalX = i;

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

    void SpawnPlayer(int x, int z)
    {
        if (!GameObject.FindWithTag("GUI"))
        {
            GameObject myMenu = Instantiate(GUI);
            DontDestroyOnLoad(myMenu);
            GameObject.Find("Pause").GetComponent<Button>().onClick.AddListener(delegate { GameObject.FindWithTag("Manager").GetComponent<PlayerManager>().Pause(); });
            //GameObject.Find("Restart").GetComponent<Button>().onClick.AddListener(delegate { GameObject.FindWithTag("Manager").GetComponent<PlayerManager>().ReloadLevel(); });
            GameObject.Find("Exet").GetComponent<Button>().onClick.AddListener(delegate { GameObject.FindWithTag("Manager").GetComponent<PlayerManager>().MainMenu(); });
        } 
        if (!GameObject.FindWithTag("Player"))
        {
            if (x == 0)
            {
                player = Instantiate(playerPrefabs[PlayerPrefs.GetInt("PalyerCharackter")], new Vector3((x * 27)+7.5f, 1, z * 27), Quaternion.identity);
                player.transform.rotation = Quaternion.AngleAxis(90, Vector3.up);
            } else if (z == 0)
            {
                player = Instantiate(playerPrefabs[PlayerPrefs.GetInt("PalyerCharackter")], new Vector3(x * 27, 1, (z * 27) + 7.5f), Quaternion.identity);
            }
            player.GetComponent<BaffController>().baffsImages = GameObject.Find("BaffBar").transform.GetComponentsInChildren<Image>();
            player.GetComponent<BaffController>().AllBaffsDisable();
            player.GetComponent<HeartSystem>().heartImages = GameObject.Find("HealthBar").transform.GetComponentsInChildren<Image>();
            player.GetComponent<HeartSystem>().CheckHealthAmount();
            player.GetComponent<Move_Controller>().joystickMove = GameObject.Find("MovePlayer").GetComponent<Joystick>();
            player.GetComponent<Move_Controller>().joystickFire = GameObject.Find("FirePlayer").GetComponent<Joystick>();
        }
        else
        {
            player = GameObject.FindWithTag("Player");
        }
        
        player.GetComponent<Move_Controller>().manager = gameObject;
        GetComponent<PlayerManager>().GUIspawn();
        player.GetComponent<Move_Controller>().joystickMove.enabled = true;
        player.GetComponent<Move_Controller>().joystickFire.enabled = true;

        if (x == 0)
        {
            player.transform.position = new Vector3((x * 27) + 7.5f, 1, z * 27);
            player.transform.rotation = Quaternion.identity;
            player.transform.rotation = Quaternion.AngleAxis(90, Vector3.up);
        }
        else if (z == 0)
        {
            player.transform.position = new Vector3(x * 27, 1, (z * 27) + 7.5f);
            player.transform.rotation = Quaternion.identity;
        }
        playerSpawn = true;
        mainCamera.GetComponent<Camera_Controller>().Player = player;
    }
}
