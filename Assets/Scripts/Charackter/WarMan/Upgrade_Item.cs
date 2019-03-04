using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Upgrade_Item : MonoBehaviour {

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

    [HideInInspector]
    public CharackterItem chi;

    void Start()
    {
        chi = GetComponent<CharackterItem>();
    }

    public void TakeItem()
    {
        if(chi.upgradeType == "Aim")
        {
            if (chi.Player.GetComponent<Weapon_Controller>().UpgradeAim == null)
            {
                chi.Player.GetComponent<Weapon_Controller>().UpgradeAim = gameObject;
                NewUpgradeSum();
            }
            else
            {
                var oldUpgradeAim = chi.Player.GetComponent<Weapon_Controller>().UpgradeAim.GetComponent<Upgrade_Item>();
                OldUpgradeSub(oldUpgradeAim);
                chi.Player.GetComponent<Weapon_Controller>().UpgradeAim = gameObject;
                NewUpgradeSum();
            }
        }
        if (chi.upgradeType == "Stand")
        {
            if (chi.Player.GetComponent<Weapon_Controller>().UpgradeStand == null)
            {
                chi.Player.GetComponent<Weapon_Controller>().UpgradeStand = gameObject;
                NewUpgradeSum();
            }
            else
            {
                var oldUpgradeStand = chi.Player.GetComponent<Weapon_Controller>().UpgradeStand.GetComponent<Upgrade_Item>();
                OldUpgradeSub(oldUpgradeStand);
                chi.Player.GetComponent<Weapon_Controller>().UpgradeStand = gameObject;
                NewUpgradeSum();
            }
        }
        if (chi.upgradeType == "Left")
        {
            if (chi.Player.GetComponent<Weapon_Controller>().UpgradeLeft == null)
            {
                chi.Player.GetComponent<Weapon_Controller>().UpgradeLeft = gameObject;
                NewUpgradeSum();
            }
            else
            {
                var oldUpgradeLeft = chi.Player.GetComponent<Weapon_Controller>().UpgradeLeft.GetComponent<Upgrade_Item>();
                OldUpgradeSub(oldUpgradeLeft);
                chi.Player.GetComponent<Weapon_Controller>().UpgradeLeft = gameObject;
                NewUpgradeSum();
            }
        }
        if (chi.upgradeType == "Right")
        {
            if (chi.Player.GetComponent<Weapon_Controller>().UpgradeRight == null)
            {
                chi.Player.GetComponent<Weapon_Controller>().UpgradeRight = gameObject;
                NewUpgradeSum();
            }
            else
            {
                var oldUpgradeRight = chi.Player.GetComponent<Weapon_Controller>().UpgradeRight.GetComponent<Upgrade_Item>();
                OldUpgradeSub(oldUpgradeRight);
                chi.Player.GetComponent<Weapon_Controller>().UpgradeRight = gameObject;
                NewUpgradeSum();
            }
        }
        if (chi.upgradeType == "Nozzle" || chi.upgradeType.Contains("Nozzle"))
        {
            if (chi.Player.GetComponent<Weapon_Controller>().UpgradeNozzle == null)
            {
                chi.Player.GetComponent<Weapon_Controller>().UpgradeNozzle = gameObject;
                NewUpgradeSum();
            } else {
                var oldUpgradeNozzle = chi.Player.GetComponent<Weapon_Controller>().UpgradeNozzle.GetComponent<Upgrade_Item>();
                OldUpgradeSub(oldUpgradeNozzle);
                chi.Player.GetComponent<Weapon_Controller>().UpgradeNozzle = gameObject;
                NewUpgradeSum();
            }
        }
        if (chi.upgradeType == "BAim")
        {
            if (chi.Player.GetComponent<Weapon_Controller>().UpgradeBottomAim == null)
            {
                chi.Player.GetComponent<Weapon_Controller>().UpgradeBottomAim = gameObject;
                NewUpgradeSum();
            }
            else
            {
                var oldUpgradeBottomAim = chi.Player.GetComponent<Weapon_Controller>().UpgradeBottomAim.GetComponent<Upgrade_Item>();
                OldUpgradeSub(oldUpgradeBottomAim);
                chi.Player.GetComponent<Weapon_Controller>().UpgradeBottomAim = gameObject;
                NewUpgradeSum();
            }
        }

        if (chi.upgradeType == "Artifact")
        {
            GameObject panelItems = GameObject.Find("ArtifactItems");
            GameObject newItem = Instantiate(Resources.Load("Prefabs/UI/Item") as GameObject, panelItems.transform);
            newItem.GetComponentInChildren<Image>().sprite = Resources.Load("Sprites/UI/ArtifactIcons/"+chi.upgradeId) as Sprite;
            Text[] nameAndDescr = newItem.GetComponentsInChildren<Text>();
            nameAndDescr[0].text = chi.upgradeName;
            nameAndDescr[1].text = chi.upgradeDescr;
            NewUpgradeSum();
            Destroy(gameObject);
        }
    }

    void NewUpgradeSum()
    {
        transform.SetParent(chi.Player.GetComponent<Weapon_Controller>().Weapon.transform);
        GetComponent<Outline>().enabled = false;
        GetComponent<GUIcontroller>().ObjectEquipt = true;
        GetComponent<Rigidbody>().isKinematic = true;
        GetComponent<BoxCollider>().isTrigger = true;
        chi.Player.GetComponent<Weapon_Controller>().turnType += turnValue;
        chi.Player.GetComponent<Weapon_Controller>().bulletSpeed += bulletSpeed * chi.Rang;
        chi.Player.GetComponent<Weapon_Controller>().bulletFallSpeed += bulletFallSpeed * chi.Rang;
        chi.Player.GetComponent<Weapon_Controller>().bulletHP += bulletHP;
        chi.Player.GetComponent<Weapon_Controller>().timeReload += reloadResist * chi.Rang;
        chi.Player.GetComponent<Weapon_Controller>().turnAngle += turnAngleValue;
        chi.Player.GetComponent<Weapon_Controller>().bulletDamage += bulletDamage * chi.Rang;
    }

    void OldUpgradeSub(Upgrade_Item oldUpgradeItem)
    {
        oldUpgradeItem.GetComponent<Outline>().enabled = true;
        oldUpgradeItem.GetComponent<GUIcontroller>().ObjectEquipt = false;
        oldUpgradeItem.GetComponent<Rigidbody>().isKinematic = false;
        oldUpgradeItem.GetComponent<BoxCollider>().isTrigger = false;
        chi.Player.GetComponent<Weapon_Controller>().turnType -= oldUpgradeItem.turnValue;
        chi.Player.GetComponent<Weapon_Controller>().bulletSpeed -= oldUpgradeItem.bulletSpeed * oldUpgradeItem.chi.Rang;
        chi.Player.GetComponent<Weapon_Controller>().bulletFallSpeed -= oldUpgradeItem.bulletFallSpeed * oldUpgradeItem.chi.Rang;
        chi.Player.GetComponent<Weapon_Controller>().bulletHP -= oldUpgradeItem.bulletHP;
        chi.Player.GetComponent<Weapon_Controller>().timeReload -= oldUpgradeItem.reloadResist * oldUpgradeItem.chi.Rang;
        chi.Player.GetComponent<Weapon_Controller>().turnAngle -= oldUpgradeItem.turnAngleValue;
        chi.Player.GetComponent<Weapon_Controller>().bulletDamage -= oldUpgradeItem.bulletDamage * oldUpgradeItem.chi.Rang;
        oldUpgradeItem.transform.SetParent(null);
    }

}
