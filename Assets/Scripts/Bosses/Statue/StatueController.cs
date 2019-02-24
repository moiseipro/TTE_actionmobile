using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatueController : BossHeartController {

    public GameObject[] totems;
    private List<GameObject> calledTotems = new List<GameObject>();

    public int maxTotems; // Максимальное количество тотемов
    int maxCallTotems = 12; //Вызвано тотемов

    float reloadTimeTotems = 20f;

    //Проверка на вызов тотемов
    bool atackTotemReload = false,
    guardTotemReload = false,
    facesTotemReload = false,
    poisonTotemReload = false,
    skullTotemReload = false,
    tunderTotemReload = false,
    healTotemReload = false,
    lampTotemReload = false;

    void Start () {
        StartScript();
        reloadTimeTotems -= bossLevel;
        maxTotems += bossLevel;
        maxTotems = Mathf.Clamp(maxTotems,3,maxCallTotems);
        reloadTimeTotems = Mathf.Clamp(reloadTimeTotems, 5, 20);
    }
	
	void FixedUpdate () {
        BossFightStartRadius();
        UpdateHpContainers();
        foreach (GameObject totem in calledTotems)
        {
            if (totem != null)
            {
                if (totem.GetComponent<TitemController>().totemName == "guard")
                {
                    immortality = true;
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Bullet" && other.GetComponent<Bullet_Options>().type != -1)
        {
            
            foreach (GameObject totem in calledTotems)
            {
                if (totem != null)
                {
                    if (totem.GetComponent<TitemController>().totemName == "atack")
                    {
                        totem.GetComponent<TitemController>().atackTotem();
                    }
                }
            }
            if (dead == true && calledTotems != null)
            {
                maxTotems = 0;
                for (int i = calledTotems.Count-1; i >=0; i--)
                {
                    Destroy(calledTotems[i]);
                    DeliteTotem(i);
                }
            }
            int whatCall = Random.Range(0, 3);
            if (guardTotemReload == false && health < maxHealth / 1.1f) CallTotem(1);
            else if (healTotemReload == false && health < maxHealth / 1.5f) CallTotem(6);
            else if (whatCall == 0 && atackTotemReload == false) CallTotem(0);
            else if (whatCall == 0 && facesTotemReload == false) CallTotem(2);
            else if (whatCall == 1 && poisonTotemReload == false) CallTotem(3);
            else if (whatCall == 1 && tunderTotemReload == false) CallTotem(5);
            else if (whatCall == 2 && skullTotemReload == false) CallTotem(4);
            else if (whatCall == 2 && lampTotemReload == false) CallTotem(7);
        }
    }

    IEnumerator ReloadTotem(int totemID)
    {
        yield return new WaitForSeconds(reloadTimeTotems);
        if (totemID == 0) atackTotemReload = false;
        else if (totemID == 1) guardTotemReload = false;
        else if (totemID == 2) facesTotemReload = false;
        else if (totemID == 3) poisonTotemReload = false;
        else if (totemID == 4) skullTotemReload = false;
        else if (totemID == 5) tunderTotemReload = false;
        else if (totemID == 6) healTotemReload = false;
        else if (totemID == 7) lampTotemReload = false;
    }

    public void CallTotem(int totemID)
    {
        if (calledTotems.Count < maxTotems)
        {
            int totemCount = 0;
            foreach(GameObject tot in calledTotems)
            {
                if(tot.GetComponent<TitemController>().idTotem == totemID) totemCount++;
            }
            if (totemID == 0 && totemCount >= 3) return;
            else if (totemID == 1 && totemCount >= 1) return;
            else if (totemID == 2 && totemCount >= 2) return;
            else if (totemID == 3 && totemCount >= 3) return;
            else if (totemID == 4 && totemCount >= 2) return;
            else if (totemID == 5 && totemCount >= 1) return;
            else if (totemID == 6 && totemCount >= 1) return;
            else if (totemID == 7 && totemCount >= 2) return;
            GameObject totem = Instantiate(totems[totemID], GenerateTotemSpawn(), Quaternion.identity);
            calledTotems.Add(totem);
            for (int i = 0; i < calledTotems.Count; i++)
            {
                calledTotems[i].GetComponent<TitemController>().id = i;
            }
            if (totemID == 0) atackTotemReload = true;
            else if (totemID == 1) guardTotemReload = true;
            else if (totemID == 2) facesTotemReload = true;
            else if (totemID == 3) poisonTotemReload = true;
            else if (totemID == 4) skullTotemReload = true;
            else if (totemID == 5) tunderTotemReload = true;
            else if (totemID == 6) healTotemReload = true;
            else if (totemID == 7) lampTotemReload = true;
            StartCoroutine(ReloadTotem(totemID));
        }
    }

    public void DeliteTotem(int idtotem)
    {
        //calledTotems[idtotem] = null;
        calledTotems.RemoveAt(idtotem);
        for(int i=0; i< calledTotems.Count; i++)
        {
            calledTotems[i].GetComponent<TitemController>().id = i;
            if (dead == false && calledTotems[i].GetComponent<TitemController>().totemName == "faces") StartCoroutine(calledTotems[i].GetComponent<TitemController>().facesTotemLaser());
        }
    }

    Vector3 GenerateTotemSpawn()
    {
        Vector3 spawnVec;
        bool cold;
        do
        {
            cold = false;
            spawnVec = new Vector3(Random.Range(gameObject.transform.position.x - 6.5f, gameObject.transform.position.x + 6.5f), gameObject.transform.position.y, Random.Range(gameObject.transform.position.z - 6.5f, gameObject.transform.position.z + 6.5f));
            foreach (Collider col in Physics.OverlapSphere(spawnVec, 3f))
            {
                if (col.tag == "Enemy")
                {
                    cold = true;
                    //Debug.Log("SPAWN TOTEM ERROR");
                    break;
                }
            }
        } while (cold == true || spawnVec.magnitude < 2.5f);

        return spawnVec;
    }
}
