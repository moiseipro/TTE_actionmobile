using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_Controller : MonoBehaviour {

    public GameObject[] Bullets;
    public GameObject Weapon;
    [Tooltip("Объект прицела надетый на оружие")]
    public GameObject UpgradeAim;
    [Tooltip("Объект нижней части оружия")]
    public GameObject UpgradeBottomAim;
    [Tooltip("Объект насадки надетый на оружие")]
    public GameObject UpgradeNozzle;
    [Tooltip("Объект подщечки надетый на оружие")]
    public GameObject UpgradeStand;
    [Tooltip("Объект левой части оружия надетый на оружие")]
    public GameObject UpgradeLeft;
    [Tooltip("Объект правой части надетый на оружие")]
    public GameObject UpgradeRight;

    //Уровень оружия
    public int WeaponLevel = 1;
    //Скорость стрельбы
    public float timeReload = 0.8f; 
    //Кол-во пуль
    public int turnType = 4;
    //Угол мужду пулями
    public float turnAngle = 80f;
    //Скорость пули
    public float bulletSpeed = 6;
    private float bulletSpeedMultiplier;
    //Скорость падения пули
    public float bulletFallSpeed = 6;
    //Скорость падения пули
    public int bulletHP = 0;
    //Урон пули
    public float bulletDamage = 1;
    

    int bulletID; // Вид пули
    private bool reload = false; //Перезарядка

    void Start () {
		
	}

    void FixedUpdate()
    {
        if ((GetComponent<Move_Controller>().joystickFire.Horizontal != 0 || GetComponent<Move_Controller>().joystickFire.Vertical != 0) && !reload)
        {
            
            float currentTurnAngle = turnAngle/2f;
            if (turnType == 1){
                GameObject Bull = Instantiate(Bullets[bulletID], Weapon.transform.position + Weapon.transform.forward * 0.9f, transform.rotation);
                Bull.transform.Rotate(0, Random.Range(-currentTurnAngle,currentTurnAngle), 0, Space.Self);
                Bull.GetComponent<Bullet_Options>().speed = bulletSpeed;
                Bull.GetComponent<Bullet_Options>().rotationSpeed = bulletFallSpeed;
                Bull.GetComponent<Bullet_Options>().hpBullet = bulletHP;
                Bull.GetComponent<Bullet_Options>().damage += bulletDamage;
                Destroy(Bull, 5f);
            } else {
                for (int i = 0; i < turnType; i++)
                {
                    GameObject Bull = Instantiate(Bullets[bulletID], Weapon.transform.position + Weapon.transform.forward * 0.9f, transform.rotation);
                    Bull.transform.Rotate(0, Random.Range(-currentTurnAngle, currentTurnAngle), 0, Space.Self);
                    Bull.GetComponent<Bullet_Options>().speed = bulletSpeed;
                    Bull.GetComponent<Bullet_Options>().rotationSpeed = bulletFallSpeed;
                    Bull.GetComponent<Bullet_Options>().hpBullet = bulletHP;
                    Bull.GetComponent<Bullet_Options>().damage = bulletDamage;
                    currentTurnAngle -= turnAngle / (turnType - 1);
                    Destroy(Bull, 5f);
                }
            }
            reload = true;
            StartCoroutine(ReloadWeapon());
        }
    }

    void Update () {
        UpgradeConnect();
    }

    void UpgradeConnect() {
        if (UpgradeAim != null)
        {
            Vector3 pos = new Vector3(0, 0.004f, 0.00294f);
            UpgradeAim.transform.localPosition = Vector3.Lerp(UpgradeAim.transform.localPosition, pos, 5f * Time.deltaTime);
            UpgradeAim.transform.rotation = Quaternion.Lerp(UpgradeAim.transform.rotation, Weapon.transform.rotation, 5f * Time.deltaTime);
        }
        if (UpgradeStand != null)
        {
            Vector3 pos = new Vector3(0f, 0.0025f, -0.0025f);
            UpgradeStand.transform.localPosition = Vector3.Lerp(UpgradeStand.transform.localPosition, pos, 5f * Time.deltaTime);
            UpgradeStand.transform.rotation = Quaternion.Lerp(UpgradeStand.transform.rotation, Weapon.transform.rotation, 5f * Time.deltaTime);
        }
        if (UpgradeLeft != null)
        {
            Vector3 pos = new Vector3(-0.0015f, 0.0025f, 0.0035f);
            UpgradeLeft.transform.localPosition = Vector3.Lerp(UpgradeLeft.transform.localPosition, pos, 5f * Time.deltaTime);
            UpgradeLeft.transform.rotation = Quaternion.Lerp(UpgradeLeft.transform.rotation, Weapon.transform.rotation, 5f * Time.deltaTime);
        }
        if (UpgradeRight != null)
        {
            Vector3 pos = new Vector3(0.0015f, 0.00258f, 0.003f);
            UpgradeRight.transform.localPosition = Vector3.Lerp(UpgradeRight.transform.localPosition, pos, 5f * Time.deltaTime);
            UpgradeRight.transform.rotation = Quaternion.Lerp(UpgradeRight.transform.rotation, Weapon.transform.rotation, 5f * Time.deltaTime);
        }
        if (UpgradeNozzle != null)
        {
            Vector3 pos = new Vector3(0f, 0.0025f, 0.0065f);
            UpgradeNozzle.transform.localPosition = Vector3.Lerp(UpgradeNozzle.transform.localPosition, pos, 5f * Time.deltaTime);
            UpgradeNozzle.transform.rotation = Quaternion.Lerp(UpgradeNozzle.transform.rotation, Weapon.transform.rotation, 5f * Time.deltaTime);
            if (UpgradeNozzle.GetComponent<Upgrade_Item>().upgradeType.Contains("4")) bulletID = 3;
            else if (UpgradeNozzle.GetComponent<Upgrade_Item>().upgradeType.Contains("5")) bulletID = 2;
            else if (UpgradeNozzle.GetComponent<Upgrade_Item>().upgradeType.Contains("6")) bulletID = 1;
            else bulletID = 0;
        }
        if (UpgradeBottomAim != null)
        {
            Vector3 pos = new Vector3(0f, 0.001f, 0.004f);
            UpgradeBottomAim.transform.localPosition = Vector3.Lerp(UpgradeBottomAim.transform.localPosition, pos, 5f * Time.deltaTime);
            UpgradeBottomAim.transform.rotation = Quaternion.Lerp(UpgradeBottomAim.transform.rotation, Weapon.transform.rotation, 5f * Time.deltaTime);
        }
    }

    IEnumerator ReloadWeapon()
    {
        yield return new WaitForSeconds(timeReload);
        reload = false;
    }
}
