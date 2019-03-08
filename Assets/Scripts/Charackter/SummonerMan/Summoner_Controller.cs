using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Summoner_Controller : MonoBehaviour {

    public GameObject weapon;
    public GameObject body;
    [Tooltip("Вызываемые питомцы")]
    public GameObject[] pets;
    [Tooltip("Объект шапки")]
    public GameObject upgradeHat;
    [Tooltip("Объект плаща")]
    public GameObject upgradeCloak;
    [Tooltip("Объект насадки на посох")]
    public GameObject upgradeCrystalBall;
    [Tooltip("Объект левого украшения")]
    public GameObject upgradeLeftDecoration;
    [Tooltip("Объект правого украшения")]
    public GameObject upgradeRightDecoration;
    [Tooltip("Призванные питомцы")]
    public List<GameObject> calledPets;

    [Tooltip("Увеличение к урону питомцев")]
    public float petDamageBoost = 0f;
    [Tooltip("Скорость питомцев")]
    public float petSpeed = 5f;
    [Tooltip("Расстояние на которе питомцы могут отлететь от персонажа так же радиус атаки питомцев")]
    public float petRange = 4f;
    [Tooltip("Максимально кол-во целей феи")]
    public int petMaximumTargets = 1;
    [Tooltip("Перезарядка драконов")]
    public float petTimeReloadDragon = 0.5f;
    [Tooltip("Перезарядка гаргулий")]
    public float petTimeReloadGargoyle = 1f;

    private Move_Controller mc;
    private HeartSystem hs;

    // Use this for initialization
    void Start () {
        mc = GetComponent<Move_Controller>();
        hs = GetComponent<HeartSystem>();
        if (calledPets.Count == 0)
        {
            calledPets.Add(Instantiate(pets[0],new Vector3(transform.position.x,3f,transform.position.z),Quaternion.identity));
            calledPets.Add(Instantiate(pets[1], new Vector3(transform.position.x, 3f, transform.position.z), Quaternion.identity));
        }
	}

    private void FixedUpdate()
    {
        
    }

    // Update is called once per frame
    void Update () {
        if (hs.isDead == false && (mc.joystickFire.Horizontal != 0 || mc.joystickFire.Vertical != 0))
        {
            for (int i = 0; i < calledPets.Count; i++)
            {
                Vector3 plusPos = Vector3.zero;
                if (i == 0) plusPos = Vector3.left;
                else if (i == 1) plusPos = Vector3.right;
                Vector3 movVec = transform.position + mc.rotVector*petRange + plusPos * 2f - calledPets[i].transform.position;
                Vector3 rotVec = Vector3.RotateTowards(calledPets[i].transform.forward / 10f, movVec, movVec.magnitude * Time.deltaTime, 0);
                rotVec.y = 0f;
                Debug.Log(movVec.magnitude);
                calledPets[i].transform.Translate(calledPets[i].transform.forward * petSpeed * (movVec.magnitude/4) * Time.deltaTime, Space.World);
                calledPets[i].transform.rotation = Quaternion.LookRotation(rotVec);

            }
        } else
        {
            for(int i = 0; i < calledPets.Count; i++)
            {
                Vector3 plusPos = Vector3.zero;
                if (i == 0) plusPos = Vector3.left;
                else if (i == 1) plusPos = Vector3.right;
                Vector3 movVec = transform.position + plusPos * 2f - calledPets[i].transform.position;
                Vector3 rotVec = Vector3.RotateTowards(calledPets[i].transform.forward / 10f, movVec, 7 * Time.deltaTime, 0);
                rotVec.y = 0f;
                Debug.Log(movVec.magnitude);
                if (movVec.magnitude > 3.05f)
                {
                    calledPets[i].transform.Translate(calledPets[i].transform.forward * movVec.magnitude * Time.deltaTime, Space.World);
                    calledPets[i].transform.rotation = Quaternion.LookRotation(rotVec);
                }
                else calledPets[i].transform.rotation = Quaternion.RotateTowards(calledPets[i].transform.rotation, gameObject.transform.rotation, 2);

            }
        }
    }
}
