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
    [Tooltip("Максимально кол-во целей дракона")]
    public int petMaximumTargetsDragon = 1;
    [Tooltip("Перезарядка драконов")]
    public float petTimeReloadDragon = 0.5f;
    [Tooltip("Перезарядка гаргулий")]
    public float petTimeReloadGargoyle = 1f;

    bool fullComplect = false;
    private const string achiv1 = ""; // ID ачивки по сбору всех предметов персонажа

    [HideInInspector] public PlayerManager pm;
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
            calledPets.Add(Instantiate(pets[2], new Vector3(transform.position.x, 3f, transform.position.z), Quaternion.identity));
        }
	}

    public void UpgradePets()
    {
        for (int i = 0; i < calledPets.Count; i++)
        {
            PetController pc = calledPets[i].GetComponent<PetController>();
            pc.damage = petDamageBoost;
            if (pc.type == 0)
            {
                pc.maxTarget = petMaximumTargetsDragon;
                pc.timeReload = petTimeReloadDragon;
            } else if (pc.type == 1)
            {
                pc.maxTarget = petMaximumTargets;
            } else if (pc.type == 2)
            {
                pc.timeReload = petTimeReloadGargoyle;
            }
        }
    }

    public void AddPet(int num)
    {
        calledPets.Add(Instantiate(pets[num], new Vector3(transform.position.x, 3f, transform.position.z), Quaternion.identity));
    }

    void Update () {
        UpgradeConnect();
        if (hs.isDead == false && (mc.joystickFire.Horizontal != 0 || mc.joystickFire.Vertical != 0))
        {
            for (int i = 0; i < calledPets.Count; i++)
            {
                Vector3 plusPos = Vector3.zero;
                if (i == 0) plusPos = Vector3.left;
                else if (i == 1) plusPos = Vector3.right;
                else if (i == 2) plusPos = Vector3.back;
                else if (i == 3) plusPos = Vector3.forward;
                Vector3 movVec = transform.position + mc.rotVector*petRange + plusPos * 2f - calledPets[i].transform.position;
                Vector3 rotVec = Vector3.RotateTowards(calledPets[i].transform.forward / 10f, movVec, movVec.magnitude * Time.deltaTime, 0);
                rotVec.y = 0f;
                //Debug.Log(movVec.magnitude);
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
                else if(i == 2) plusPos = Vector3.back;
                else if (i == 3) plusPos = Vector3.forward;
                Vector3 movVec = transform.position + plusPos * 2f - calledPets[i].transform.position;
                Vector3 rotVec = Vector3.RotateTowards(calledPets[i].transform.forward / 10f, movVec, 7 * Time.deltaTime, 0);
                rotVec.y = 0f;
                //Debug.Log(movVec.magnitude);
                if (movVec.magnitude > 3.05f)
                {
                    calledPets[i].transform.Translate(calledPets[i].transform.forward * movVec.magnitude * Time.deltaTime, Space.World);
                    calledPets[i].transform.rotation = Quaternion.LookRotation(rotVec);
                }
                else calledPets[i].transform.rotation = Quaternion.RotateTowards(calledPets[i].transform.rotation, gameObject.transform.rotation, 2);

            }
        }
    }


    void UpgradeConnect()
    {
        if (fullComplect == false && upgradeHat != null && upgradeCloak != null && upgradeCrystalBall != null && upgradeLeftDecoration != null && upgradeRightDecoration != null)
        {
            fullComplect = true;
            GameObject.FindWithTag("Manager").GetComponent<PlayerManager>().GetAchivement(achiv1);
            Debug.Log("Ачивка по сбору предметов персонажа");
        }
        if (upgradeHat != null)
        {
            Vector3 pos = new Vector3(0, 0.01f, 0f);
            upgradeHat.transform.localPosition = Vector3.Lerp(upgradeHat.transform.localPosition, pos, 5f * Time.deltaTime);
            upgradeHat.transform.rotation = Quaternion.Lerp(upgradeHat.transform.rotation, body.transform.rotation, 5f * Time.deltaTime);
        }
        if (upgradeCloak != null)
        {
            Vector3 pos = new Vector3(0, 0.007f, -0.0025f);
            upgradeCloak.transform.localPosition = Vector3.Lerp(upgradeCloak.transform.localPosition, pos, 5f * Time.deltaTime);
            upgradeCloak.transform.rotation = Quaternion.Lerp(upgradeCloak.transform.rotation, body.transform.rotation, 5f * Time.deltaTime);
        }
        if (upgradeCrystalBall != null)
        {
            Vector3 pos = new Vector3(0, 0.00633f, 0);
            upgradeCrystalBall.transform.localPosition = Vector3.Lerp(upgradeCrystalBall.transform.localPosition, pos, 5f * Time.deltaTime);
            upgradeCrystalBall.transform.rotation = Quaternion.Lerp(upgradeCrystalBall.transform.rotation, weapon.transform.rotation, 5f * Time.deltaTime);
        }
        if (upgradeLeftDecoration != null)
        {
            Vector3 pos = new Vector3(-0.0035f, 0.0015f, 0);
            upgradeLeftDecoration.transform.localPosition = Vector3.Lerp(upgradeLeftDecoration.transform.localPosition, pos, 5f * Time.deltaTime);
            upgradeLeftDecoration.transform.rotation = Quaternion.Lerp(upgradeLeftDecoration.transform.rotation, body.transform.rotation, 5f * Time.deltaTime);
        }
        if (upgradeRightDecoration != null)
        {
            Vector3 pos = new Vector3(0.0035f, 0.0015f, 0);
            upgradeRightDecoration.transform.localPosition = Vector3.Lerp(upgradeRightDecoration.transform.localPosition, pos, 5f * Time.deltaTime);
            upgradeRightDecoration.transform.rotation = Quaternion.Lerp(upgradeRightDecoration.transform.rotation, body.transform.rotation, 5f * Time.deltaTime);
        }
    }
}
