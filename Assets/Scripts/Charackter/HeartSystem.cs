using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeartSystem : MonoBehaviour {

    private int maxHeart = 18;
    public int startHearts = 7;
    public int curHealth;
    private int maxHealth;
    private int healthPerHeart = 2;
    [HideInInspector] public bool isDead = false;

    public Image[] heartImages;
    public Sprite[] heartSprite;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    void Start () {
        HealAll();
    }

    public bool CheckFull()
    {
        if (curHealth < startHearts*healthPerHeart) return true;
        else return false;
    }

    //Метод проверки количества здоровья
    public void CheckHealthAmount()
    {
        for (int i = 0; i < maxHeart; i++)
        {
            if (startHearts <= i)
            {
                heartImages[i].enabled = false;
            } else
            {
                heartImages[i].enabled = true;
            }
        }
        UpdateHearts();
    }
    
    //Метод обновления спрайтов на контейнерах здоровья
    void UpdateHearts()
    {
        bool empty = false;
        int i = 0;

        foreach (Image image in heartImages){
            if (empty)
            {
                image.sprite = heartSprite[0];
            }
            else
            {
                i++;
                if (curHealth >= i * healthPerHeart)
                {
                    image.sprite = heartSprite[heartSprite.Length - 1];
                }
                else {
                    int currentHeartHelth = (int)(healthPerHeart - (healthPerHeart * i - curHealth));
                    int healthPerImage = healthPerHeart / (heartSprite.Length - 1);
                    int imageIndex = currentHeartHelth / healthPerImage;
                    image.sprite = heartSprite[imageIndex];
                    empty = true;
                }
            }
        }
    }

    public void TakeDamage(int amount)
    {
        curHealth += amount;
        curHealth = Mathf.Clamp(curHealth, 0, startHearts * healthPerHeart);
        Debug.Log("Damage: " + amount + " HP: " + curHealth);
        UpdateHearts();
        if (curHealth < 1)
        {
            Debug.Log("Персонаж СМЭРТЬ");
            //GetComponent<Animator>().SetTrigger("Dead");
            //isDead = true;
        }
    }

    public void HealAll()
    {
        curHealth = startHearts * healthPerHeart;
        maxHealth = maxHeart * healthPerHeart;
        if (isDead == true && curHealth > 0)
        {
            isDead = false;
            GetComponent<Animator>().SetTrigger("Alive");
        }
        CheckHealthAmount();
    }

    public void AddHeartContainer(int amount)
    {
        startHearts += amount;
        startHearts = Mathf.Clamp(startHearts, 0, maxHeart);

        CheckHealthAmount();
    }
}
