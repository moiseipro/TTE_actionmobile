using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandController : MonoBehaviour {

    public float maxHealth = 60;
    Vector3 dam = new Vector3(0.3f, 0.3f, 0.3f);

    public GameObject boss;
    ElementhalController elementhalController;

    private void Start()
    {
        elementhalController = boss.GetComponent<ElementhalController>();
        if(gameObject.name == "Legs") maxHealth = elementhalController.maxHealth * 0.8f;
        else maxHealth = elementhalController.maxHealth / 3f;
    }

    public void AddDamage(float dam)
    {
        StartCoroutine(DamageVisual());
        elementhalController.AddDamage(dam);
        if (maxHealth > 0) maxHealth -= dam;
        else Destroy(gameObject);
    }

    IEnumerator DamageVisual()
    {

        gameObject.transform.localScale += dam;
        yield return new WaitForSeconds(0.1f);
        gameObject.transform.localScale -= dam;
    }

}
