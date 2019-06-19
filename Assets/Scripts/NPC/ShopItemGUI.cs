using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopItemGUI : MonoBehaviour {

    Vector3 screenPosition;
    Vector3 cameraRelative;
    Vector3 predItemScreenPos;
    Rect positionItem;
    Rect positionLine;
    Transform PredItem, ThisObject;
    GUIStyle customButton;
    GUIStyle customText;
    PlayerManager pm;
    DropItemController dic;

    public bool ObjectIsSee = false;
    public bool ObjectEquipt = false;

    private int id;
    private int price;

    private string name;

    //Размеры панели
    float WidthPanel;
    float HeightPanel = 300f;
    float ItemsValue;

    // Use this for initialization
    void Start()
    {
        customText = GameObject.FindWithTag("Manager").GetComponent<GUICustomStyle>().customText;
        pm = GameObject.FindWithTag("Manager").GetComponent<PlayerManager>();
        dic = GameObject.FindWithTag("Manager").GetComponent<DropItemController>();
        PredItem = GetComponent<Transform>();
        ThisObject = GetComponent<Transform>();
        WidthPanel = HeightPanel * 1.5f;
        if(pm.levelGame<5)id = Random.Range(0,6);
        else id = Random.Range(0, 9);
        price = pm.levelGame * 12 + id * Random.Range(10,20);
        switch (id)
        {
            case 0:
                name = "Посылка с продуктового";
                break;
            case 1:
                name = "Посылка из сундука";
                break;
            case 2:
                name = "Посылка с обычным снаряжением";
                break;
            case 3:
                name = "Посылка с снаряжением случайной редкости";
                break;
            case 4:
                name = "Посылка с необычным снаряжением";
                break;
            case 5:
                name = "Посылка с артефактом";
                break;
            case 6:
                name = "Посылка с редким снаряжением";
                break;
            case 7:
                name = "Посылка с легендарным снаряжением";
                break;
            case 8:
                name = "Посылка с артефактным снаряжением";
                price = id * Random.Range(20, 50) + pm.levelGame * 10;
                break;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //ObjectIsSee = false;
    }

    public void ObjectSee(Transform posItem)
    {
        ObjectIsSee = true;
        PredItem = posItem;
        customButton = GameObject.FindWithTag("Manager").GetComponent<GUICustomStyle>().customButton;
            customButton.normal.background = Resources.Load("Sprites/UI/ItemPanel/UI_Item_0") as Texture2D;
            customButton.hover.background = Resources.Load("Sprites/UI/ItemPanel/UI_Item_0") as Texture2D;
            customButton.active.background = Resources.Load("Sprites/UI/ItemPanel/UI_Item_0") as Texture2D;
    }

    void OnGUI()
    {
        screenPosition = Camera.main.WorldToScreenPoint(gameObject.transform.position);
        cameraRelative = Camera.main.transform.InverseTransformPoint(transform.position);
        predItemScreenPos = Camera.main.WorldToScreenPoint(PredItem.transform.position);
        predItemScreenPos.x += 180f;
        if (cameraRelative.z > 0 && ObjectIsSee == true && ObjectEquipt == false)
        {
            positionItem = new Rect(predItemScreenPos.x - WidthPanel, (Screen.height - predItemScreenPos.y - HeightPanel - 50f), WidthPanel, HeightPanel);
            positionLine = new Rect(predItemScreenPos.x - WidthPanel / 2f, predItemScreenPos.y + 50f, WidthPanel, HeightPanel);

            if (GUI.Button(positionItem, name, customButton))
            {
                if (pm.moneyValue >= price)
                {
                    DropShop();
                    ObjectIsSee = false;
                    pm.AddMoney(-price);
                }
            }
            GUI.Label(positionItem, price.ToString(), customText);
        }
    }

    void DropShop()
    {
        GameObject Item = new GameObject();
        switch (id)
        {
            case 0:
                Item = Instantiate(dic.DropItemBox(2), gameObject.transform.position, transform.rotation);
                break;
            case 1:
                Item = Instantiate(dic.DropItemChest(1), gameObject.transform.position, transform.rotation);
                break;
            case 2:
                Item = Instantiate(dic.DropCertainItem(Random.Range(0.5f,0.7f)), gameObject.transform.position, transform.rotation);
                break;
            case 3:
                Item = Instantiate(dic.DropItem(2), gameObject.transform.position, transform.rotation);
                break;
            case 4:
                Item = Instantiate(dic.DropCertainItem(Random.Range(0.7f, 1.1f)), gameObject.transform.position, transform.rotation);
                break;
            case 5:
                Item = Instantiate(dic.DropOnlyArtifact(), gameObject.transform.position, transform.rotation);
                break;
            case 6:
                Item = Instantiate(dic.DropCertainItem(Random.Range(1.1f, 1.5f)), gameObject.transform.position, transform.rotation);
                break;
            case 7:
                Item = Instantiate(dic.DropCertainItem(Random.Range(1.5f, 2f)), gameObject.transform.position, transform.rotation);
                break;
            case 8:
                Item = Instantiate(dic.DropCertainItem(2f), gameObject.transform.position, transform.rotation);
                break;
        }
        Item.GetComponent<Rigidbody>().AddForce(new Vector3(0, Random.Range(1, 2), 0), ForceMode.Impulse);
        Destroy(gameObject);
    }
}
