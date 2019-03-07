using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Summoner_Controller : MonoBehaviour {

    public GameObject weapon;
    public GameObject body;
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
    [Tooltip("Питомцы")]
    public List<GameObject> pets;

    [Tooltip("Урон питомцев")]
    public float petDamage = 1f;
    [Tooltip("Скорость питомцев")]
    public float petSpeed = 6f;
    [Tooltip("Расстояние на которе питомцы могут отлететь от персонажа")]
    public float petRange = 4f;
    [Tooltip("Максимально кол-во целей феи")]
    public int petMaximumTargets = 1;
    [Tooltip("Перезарядка драконов")]
    public float petTimeReloadDragon = 0.5f;
    [Tooltip("Перезарядка гаргулий")]
    public float petTimeReloadGargoyle = 1f;


    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
