using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Summoner_Item : MonoBehaviour {

    [Header("Характеристики предмета")]
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

    [HideInInspector] public CharackterItem chi;


    void Start () {
        chi = GetComponent<CharackterItem>();
    }

    public void TakeItem()
    {
        if (chi.upgradeType == "Hat")
        {
            if (chi.Player.GetComponent<Summoner_Controller>().upgradeHat == null)
            {
                chi.Player.GetComponent<Summoner_Controller>().upgradeHat = gameObject;
                NewUpgradeSum();
            }
            else
            {
                var oldUpgradeHat = chi.Player.GetComponent<Summoner_Controller>().upgradeHat.GetComponent<Summoner_Item>();
                OldUpgradeSub(oldUpgradeHat);
                chi.Player.GetComponent<Summoner_Controller>().upgradeHat = gameObject;
                NewUpgradeSum();
            }
        } else if (chi.upgradeType == "Cloak")
        {
            if (chi.Player.GetComponent<Summoner_Controller>().upgradeCloak == null)
            {
                chi.Player.GetComponent<Summoner_Controller>().upgradeCloak = gameObject;
                NewUpgradeSum();
            }
            else
            {
                var oldUpgradeCloak = chi.Player.GetComponent<Summoner_Controller>().upgradeCloak.GetComponent<Summoner_Item>();
                OldUpgradeSub(oldUpgradeCloak);
                chi.Player.GetComponent<Summoner_Controller>().upgradeCloak = gameObject;
                NewUpgradeSum();
            }
        } else if (chi.upgradeType == "CrystalBall")
        {
            if (chi.Player.GetComponent<Summoner_Controller>().upgradeCrystalBall == null)
            {
                chi.Player.GetComponent<Summoner_Controller>().upgradeCrystalBall = gameObject;
                NewUpgradeSum();
            }
            else
            {
                var oldUpgradeCrystalBall = chi.Player.GetComponent<Summoner_Controller>().upgradeCrystalBall.GetComponent<Summoner_Item>();
                OldUpgradeSub(oldUpgradeCrystalBall);
                chi.Player.GetComponent<Summoner_Controller>().upgradeCrystalBall = gameObject;
                NewUpgradeSum();
            }
        } else if (chi.upgradeType == "LeftDecoration")
        {
            if (chi.Player.GetComponent<Summoner_Controller>().upgradeLeftDecoration == null)
            {
                chi.Player.GetComponent<Summoner_Controller>().upgradeLeftDecoration = gameObject;
                NewUpgradeSum();
            }
            else
            {
                var oldUpgradeLeftDecoration = chi.Player.GetComponent<Summoner_Controller>().upgradeLeftDecoration.GetComponent<Summoner_Item>();
                OldUpgradeSub(oldUpgradeLeftDecoration);
                chi.Player.GetComponent<Summoner_Controller>().upgradeLeftDecoration = gameObject;
                NewUpgradeSum();
            }
        } else if (chi.upgradeType == "RightDecoration")
        {
            if (chi.Player.GetComponent<Summoner_Controller>().upgradeRightDecoration == null)
            {
                chi.Player.GetComponent<Summoner_Controller>().upgradeRightDecoration = gameObject;
                NewUpgradeSum();
            }
            else
            {
                var oldUpgradeRightDecoration = chi.Player.GetComponent<Summoner_Controller>().upgradeRightDecoration.GetComponent<Summoner_Item>();
                OldUpgradeSub(oldUpgradeRightDecoration);
                chi.Player.GetComponent<Summoner_Controller>().upgradeRightDecoration = gameObject;
                NewUpgradeSum();
            }
        }

        if (chi.upgradeType == "Artifact")
        {
            GameObject panelItems = GameObject.Find("ArtifactItems");
            GameObject newItem = Instantiate(Resources.Load<GameObject>("Prefabs/UI/Item"), panelItems.transform);
            //newItem.GetComponentInChildren<Image>().sprite = Resources.Load<Sprite>("Sprites/UI/ArtifactIcons/" + chi.upgradeId);
            Text[] nameAndDescr = newItem.GetComponentsInChildren<Text>();
            nameAndDescr[0].text = chi.upgradeName;
            nameAndDescr[1].text = chi.upgradeDescr;
            NewUpgradeSum();
            Destroy(gameObject);
        }
        chi.Player.GetComponent<Summoner_Controller>().UpgradePets();
    }

    void NewUpgradeSum()
    {
        if(chi.upgradeType == "CrystalBall") transform.SetParent(chi.Player.GetComponent<Summoner_Controller>().weapon.transform);
        else transform.SetParent(chi.Player.GetComponent<Summoner_Controller>().body.transform);
        GetComponent<Outline>().enabled = false;
        GetComponent<GUIcontroller>().ObjectEquipt = true;
        GetComponent<Rigidbody>().isKinematic = true;
        GetComponent<BoxCollider>().isTrigger = true;
        chi.Player.GetComponent<Summoner_Controller>().petDamageBoost += petDamageBoost * chi.Rang;
        chi.Player.GetComponent<Summoner_Controller>().petSpeed += petSpeed * chi.Rang;
        chi.Player.GetComponent<Summoner_Controller>().petRange += petRange * chi.Rang;
        chi.Player.GetComponent<Summoner_Controller>().petMaximumTargets += petMaximumTargets;
        chi.Player.GetComponent<Summoner_Controller>().petMaximumTargetsDragon += petMaximumTargetsDragon;
        chi.Player.GetComponent<Summoner_Controller>().petTimeReloadDragon += petTimeReloadDragon * chi.Rang;
        chi.Player.GetComponent<Summoner_Controller>().petTimeReloadGargoyle += petTimeReloadGargoyle * chi.Rang;
    }

    void OldUpgradeSub(Summoner_Item oldUpgradeItem)
    {
        oldUpgradeItem.GetComponent<Outline>().enabled = true;
        oldUpgradeItem.GetComponent<GUIcontroller>().ObjectEquipt = false;
        oldUpgradeItem.GetComponent<Rigidbody>().isKinematic = false;
        oldUpgradeItem.GetComponent<BoxCollider>().isTrigger = false;
        chi.Player.GetComponent<Summoner_Controller>().petDamageBoost -= oldUpgradeItem.petDamageBoost * oldUpgradeItem.chi.Rang;
        chi.Player.GetComponent<Summoner_Controller>().petSpeed -= oldUpgradeItem.petSpeed * oldUpgradeItem.chi.Rang;
        chi.Player.GetComponent<Summoner_Controller>().petRange -= oldUpgradeItem.petRange * oldUpgradeItem.chi.Rang;
        chi.Player.GetComponent<Summoner_Controller>().petMaximumTargets -= oldUpgradeItem.petMaximumTargets;
        chi.Player.GetComponent<Summoner_Controller>().petMaximumTargetsDragon -= oldUpgradeItem.petMaximumTargetsDragon;
        chi.Player.GetComponent<Summoner_Controller>().petTimeReloadDragon -= oldUpgradeItem.petTimeReloadDragon * oldUpgradeItem.chi.Rang;
        chi.Player.GetComponent<Summoner_Controller>().petTimeReloadGargoyle -= oldUpgradeItem.petTimeReloadGargoyle * oldUpgradeItem.chi.Rang;
        oldUpgradeItem.transform.SetParent(null);
        oldUpgradeItem.transform.position += oldUpgradeItem.gameObject.transform.forward + Vector3.up;
    }
}
