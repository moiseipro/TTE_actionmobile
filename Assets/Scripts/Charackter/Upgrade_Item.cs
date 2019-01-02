using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Upgrade_Item : MonoBehaviour {

    GameObject Player;


    [Header("Тип предмета")]
    public string upgradeType;
    [Header("Редкость предмета")]
    [Range(0.5f, 2f)]
    public float Rang;
    [Header("Характеристики предмета")]
    [Tooltip("Кол-во выпускаемых одновременно патронов")]
    public int turnValue;
    [Tooltip("Угол разброса(равномерное распределение)")]
    public int turnAngleValue;
    [Tooltip("Значение уменьшения перезарядки")]
    public float reloadResist;
    [Tooltip("Значение скорости полета пули")]
    public float bulletSpeed;
    [Tooltip("Значение скорости падения пули")]
    public float bulletFallSpeed;
    [Tooltip("Сколько врагов пуля может пронзить")]
    public int bulletHP;
    [Tooltip("Урон пули")]
    public float bulletDamage;

    public void TakeItem()
    {
        if(upgradeType == "Aim")
        {
            if (Player.GetComponent<Weapon_Controller>().UpgradeAim == null)
            {
                Player.GetComponent<Weapon_Controller>().UpgradeAim = gameObject;
                NewUpgradeSum();
            }
            else
            {
                var oldUpgradeAim = Player.GetComponent<Weapon_Controller>().UpgradeAim.GetComponent<Upgrade_Item>();
                OldUpgradeSub(oldUpgradeAim);
                Player.GetComponent<Weapon_Controller>().UpgradeAim = gameObject;
                NewUpgradeSum();
            }
        }
        if (upgradeType == "Stand")
        {
            if (Player.GetComponent<Weapon_Controller>().UpgradeStand == null)
            {
                Player.GetComponent<Weapon_Controller>().UpgradeStand = gameObject;
                NewUpgradeSum();
            }
            else
            {
                var oldUpgradeStand = Player.GetComponent<Weapon_Controller>().UpgradeStand.GetComponent<Upgrade_Item>();
                OldUpgradeSub(oldUpgradeStand);
                Player.GetComponent<Weapon_Controller>().UpgradeStand = gameObject;
                NewUpgradeSum();
            }
        }
        if (upgradeType == "Left")
        {
            if (Player.GetComponent<Weapon_Controller>().UpgradeLeft == null)
            {
                Player.GetComponent<Weapon_Controller>().UpgradeLeft = gameObject;
                NewUpgradeSum();
            }
            else
            {
                var oldUpgradeLeft = Player.GetComponent<Weapon_Controller>().UpgradeLeft.GetComponent<Upgrade_Item>();
                OldUpgradeSub(oldUpgradeLeft);
                Player.GetComponent<Weapon_Controller>().UpgradeLeft = gameObject;
                NewUpgradeSum();
            }
        }
        if (upgradeType == "Right")
        {
            if (Player.GetComponent<Weapon_Controller>().UpgradeRight == null)
            {
                Player.GetComponent<Weapon_Controller>().UpgradeRight = gameObject;
                NewUpgradeSum();
            }
            else
            {
                var oldUpgradeRight = Player.GetComponent<Weapon_Controller>().UpgradeRight.GetComponent<Upgrade_Item>();
                OldUpgradeSub(oldUpgradeRight);
                Player.GetComponent<Weapon_Controller>().UpgradeRight = gameObject;
                NewUpgradeSum();
            }
        }
        if (upgradeType == "Nozzle")
        {
            if (Player.GetComponent<Weapon_Controller>().UpgradeNozzle == null)
            {
                Player.GetComponent<Weapon_Controller>().UpgradeNozzle = gameObject;
                NewUpgradeSum();
            } else {
                var oldUpgradeNozzle = Player.GetComponent<Weapon_Controller>().UpgradeNozzle.GetComponent<Upgrade_Item>();
                OldUpgradeSub(oldUpgradeNozzle);
                Player.GetComponent<Weapon_Controller>().UpgradeNozzle = gameObject;
                NewUpgradeSum();
            }
        }
        if (upgradeType == "BAim")
        {
            if (Player.GetComponent<Weapon_Controller>().UpgradeBottomAim == null)
            {
                Player.GetComponent<Weapon_Controller>().UpgradeBottomAim = gameObject;
                NewUpgradeSum();
            }
            else
            {
                var oldUpgradeBottomAim = Player.GetComponent<Weapon_Controller>().UpgradeBottomAim.GetComponent<Upgrade_Item>();
                OldUpgradeSub(oldUpgradeBottomAim);
                Player.GetComponent<Weapon_Controller>().UpgradeBottomAim = gameObject;
                NewUpgradeSum();
            }
        }
    }

    void NewUpgradeSum()
    {
        transform.SetParent(Player.GetComponent<Weapon_Controller>().Weapon.transform);
        GetComponent<Outline>().enabled = false;
        GetComponent<GUIcontroller>().ObjectEquipt = true;
        GetComponent<Rigidbody>().isKinematic = true;
        GetComponent<BoxCollider>().isTrigger = true;
        Player.GetComponent<Weapon_Controller>().turnType += turnValue;
        Player.GetComponent<Weapon_Controller>().bulletSpeed += bulletSpeed * Rang;
        Player.GetComponent<Weapon_Controller>().bulletFallSpeed += bulletFallSpeed * Rang;
        Player.GetComponent<Weapon_Controller>().bulletHP += bulletHP;
        Player.GetComponent<Weapon_Controller>().timeReload += reloadResist * Rang;
        Player.GetComponent<Weapon_Controller>().turnAngle += turnAngleValue;
        Player.GetComponent<Weapon_Controller>().bulletDamage += bulletDamage * Rang;
    }

    void OldUpgradeSub(Upgrade_Item oldUpgradeItem)
    {
        oldUpgradeItem.GetComponent<Outline>().enabled = true;
        oldUpgradeItem.GetComponent<GUIcontroller>().ObjectEquipt = false;
        oldUpgradeItem.GetComponent<Rigidbody>().isKinematic = false;
        oldUpgradeItem.GetComponent<BoxCollider>().isTrigger = false;
        Player.GetComponent<Weapon_Controller>().turnType -= oldUpgradeItem.turnValue;
        Player.GetComponent<Weapon_Controller>().bulletSpeed -= oldUpgradeItem.bulletSpeed * oldUpgradeItem.Rang;
        Player.GetComponent<Weapon_Controller>().bulletFallSpeed -= oldUpgradeItem.bulletFallSpeed * oldUpgradeItem.Rang;
        Player.GetComponent<Weapon_Controller>().bulletHP -= oldUpgradeItem.bulletHP;
        Player.GetComponent<Weapon_Controller>().timeReload -= oldUpgradeItem.reloadResist * oldUpgradeItem.Rang;
        Player.GetComponent<Weapon_Controller>().turnAngle -= oldUpgradeItem.turnAngleValue;
        Player.GetComponent<Weapon_Controller>().bulletDamage -= oldUpgradeItem.bulletDamage * oldUpgradeItem.Rang;
        oldUpgradeItem.transform.SetParent(null);
    }

    // Use this for initialization
    void Start () {
        Player = GameObject.FindWithTag("Player");

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Object")
        {
            gameObject.GetComponent<Rigidbody>().isKinematic = true;
            gameObject.GetComponent<BoxCollider>().isTrigger = true;
            if (Rang > 1.5){
                GetComponent<Outline>().OutlineColor = Color.yellow;
            } else if (Rang > 1.1) {
                GetComponent<Outline>().OutlineColor = Color.blue;
            } else if (Rang > 0.7) {
                GetComponent<Outline>().OutlineColor = Color.green;
            } else if (Rang >= 0.5) {
                GetComponent<Outline>().OutlineColor = Color.white;
            }
        }
    }
}
