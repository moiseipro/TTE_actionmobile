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

    private int[] baffEnabled = new int[9];

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
            if(baffsImages[i].enabled == true) baffsImages[i].enabled = false;
            baffEnabled[i] = -1;
        }
    }

    public void CheckBaff(int id)
    {
        bool isBaff = false;
        for(int i = 0; i < baffEnabled.Length; i++)
        {
            if(baffEnabled[i] == id)
            {
                isBaff = true;
            }
        }
        if (isBaff == false)
        {
            baffEffects[id].Stop();
        }
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
        baffEnabled[idBaffImage] = id;

        for (int i = 0; i < time; i++)
        {
            hs.TakeDamage(-damage);
            yield return new WaitForSeconds(1f);
        }

        baffEnabled[idBaffImage] = -1;
        baffsImages[idBaffImage].enabled = false;
        curBaff--;
        CheckBaff(id);
    }

    IEnumerator tikHeal(int time, int heal, int id)
    {
        curBaff++;
        int idBaffImage = curBaff;
        baffsImages[idBaffImage].enabled = true;
        baffsImages[idBaffImage].sprite = baffsSprite[id];
        baffEffects[id].Play();
        baffEnabled[idBaffImage] = id;

        for (int i = 0; i < time; i++)
        {
            hs.TakeDamage(heal);
            yield return new WaitForSeconds(1f);
        }

        baffEnabled[idBaffImage] = -1;
        curBaff--;
        CheckBaff(id);
    }

    IEnumerator tikSpeedDown(int time, int speedDown, int id)
    {
        curBaff++;
        int idBaffImage = curBaff;
        baffsImages[idBaffImage].enabled = true;
        baffsImages[idBaffImage].sprite = baffsSprite[id];
        baffEffects[id].Play();
        baffEnabled[idBaffImage] = id;

        float speedSub = mc.speed * speedDown / 100;
        speedSub = (float)System.Math.Round((decimal)speedSub, 2);
        Debug.Log("Дебаф скорости: "+ speedSub);
        mc.speedDebaf += speedSub;
        yield return new WaitForSeconds(time);
        mc.speedDebaf -= speedSub;

        baffEnabled[idBaffImage] = -1;
        baffsImages[idBaffImage].enabled = false;
        curBaff--;
        CheckBaff(id);
    }

}
