using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BaffController : MonoBehaviour {

    HeartSystem hs;
    Move_Controller mc;

    public Image[] baffsImages;
    public Sprite[] baffsSprite;
    public ParticleSystem[] baffEffects;

    private int maxBaff = 9;
    private int curBaff = -1;


	// Use this for initialization
	void Start () {
        hs = GetComponent<HeartSystem>();
        mc = GetComponent<Move_Controller>();
    }

    public void AllBaffsDisable()
    {
        for(int i = 0; i < baffsImages.Length; i++)
        {
            baffsImages[i].enabled = false;
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void CreateBaff(int time, int value, int idBaff) {
        if (curBaff < maxBaff)
        {
            if (idBaff == 0 || idBaff == 3) StartCoroutine(tikDamage(time, value, idBaff));
            else if (idBaff == 1) StartCoroutine(tikHeal(time, value, idBaff));
            else if (idBaff == 2) StartCoroutine(tikSpeedDown(time, value, idBaff));
        }
        else Debug.Log("Наложено максимальное кол-во бафов");
    }

    public void StaticBaff(int value, int idBaff)
    {
        if (idBaff == 2) mc.staticSpeedDebaf = mc.speed * value / 100;
    }

    IEnumerator tikDamage(int time, int damage, int id)
    {
        curBaff++;
        int idBaffImage = curBaff;
        baffsImages[idBaffImage].enabled = true;
        baffsImages[idBaffImage].sprite = baffsSprite[id];
        baffEffects[id].Play();
        for (int i = 0; i < time; i++)
        {
            hs.TakeDamage(-damage);
            yield return new WaitForSeconds(1f);
        }

        baffsImages[idBaffImage].enabled = false;
        baffEffects[id].Stop();
        curBaff--;
    }

    IEnumerator tikHeal(int time, int heal, int id)
    {
        curBaff++;
        int idBaffImage = curBaff;
        baffsImages[idBaffImage].enabled = true;
        baffsImages[idBaffImage].sprite = baffsSprite[id];
        baffEffects[id].Play();

        for (int i = 0; i < time; i++)
        {
            hs.TakeDamage(heal);
            yield return new WaitForSeconds(1f);
        }

        baffEffects[id].Stop();
        curBaff--;
    }

    IEnumerator tikSpeedDown(int time, int speedDown, int id)
    {
        curBaff++;
        int idBaffImage = curBaff;
        baffsImages[idBaffImage].enabled = true;
        baffsImages[idBaffImage].sprite = baffsSprite[id];
        baffEffects[id].Play();

        float speedSub = mc.speed * speedDown / 100;
        speedSub = (float)System.Math.Round((decimal)speedSub, 2);
        Debug.Log("Дебаф скорости: "+ speedSub);
        mc.speedDebaf += speedSub;
        yield return new WaitForSeconds(time);
        mc.speedDebaf -= speedSub;

        baffsImages[idBaffImage].enabled = false;
        baffEffects[id].Stop();
        curBaff--;
    }

}
