using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUIcontroller : MonoBehaviour {

    Vector3 screenPosition;
    Vector3 cameraRelative;
    Vector3 predItemScreenPos;
    Rect positionItem;
    Rect positionLine;
    Transform PredItem, ThisObject;
    GUIStyle customButton;

    public bool ObjectIsSee = false;
    public bool ObjectEquipt = false;

    //Размеры панели
    float WidthPanel;
    float HeightPanel = 300f;
    float ItemsValue;

    // Use this for initialization
    void Start () {
        PredItem = GetComponent<Transform>();
        ThisObject = GetComponent<Transform>();
        WidthPanel = HeightPanel * 1.5f;
    }
	
	// Update is called once per frame
	void FixedUpdate() {
        //ObjectIsSee = false;
    }

    public void ObjectSee(Transform posItem) {
        ObjectIsSee = true;
        PredItem = posItem;
        customButton = GameObject.FindWithTag("Manager").GetComponent<GUICustomStyle>().customButton;
        if (GetComponent<CharackterItem>().Rang > 1.5){
            customButton.normal.background = Resources.Load("Sprites/UI/ItemPanel/UI_Item_3") as Texture2D;
            customButton.hover.background = Resources.Load("Sprites/UI/ItemPanel/UI_Item_3") as Texture2D;
            customButton.active.background = Resources.Load("Sprites/UI/ItemPanel/UI_Item_3") as Texture2D;
        } else if (GetComponent<CharackterItem>().Rang > 1.1) {
            customButton.normal.background = Resources.Load("Sprites/UI/ItemPanel/UI_Item_2") as Texture2D;
            customButton.hover.background = Resources.Load("Sprites/UI/ItemPanel/UI_Item_2") as Texture2D;
            customButton.active.background = Resources.Load("Sprites/UI/ItemPanel/UI_Item_2") as Texture2D;
        }
        else if (GetComponent<CharackterItem>().Rang > 0.7) {
            customButton.normal.background = Resources.Load("Sprites/UI/ItemPanel/UI_Item_1") as Texture2D;
            customButton.hover.background = Resources.Load("Sprites/UI/ItemPanel/UI_Item_1") as Texture2D;
            customButton.active.background = Resources.Load("Sprites/UI/ItemPanel/UI_Item_1") as Texture2D;
        }
        else if (GetComponent<CharackterItem>().Rang >= 0.5) {
            customButton.normal.background = Resources.Load("Sprites/UI/ItemPanel/UI_Item_0") as Texture2D;
            customButton.hover.background = Resources.Load("Sprites/UI/ItemPanel/UI_Item_0") as Texture2D;
            customButton.active.background = Resources.Load("Sprites/UI/ItemPanel/UI_Item_0") as Texture2D;
        }
    }

    void OnGUI()
    {
        screenPosition = Camera.main.WorldToScreenPoint(gameObject.transform.position);
        cameraRelative = Camera.main.transform.InverseTransformPoint(transform.position);
        predItemScreenPos = Camera.main.WorldToScreenPoint(PredItem.transform.position);
        predItemScreenPos.x += 180f;
        if (cameraRelative.z > 0 && ObjectIsSee == true && ObjectEquipt == false)
        {
            positionItem = new Rect(predItemScreenPos.x - WidthPanel, (Screen.height - predItemScreenPos.y - HeightPanel -50f), WidthPanel, HeightPanel);
            positionLine = new Rect(predItemScreenPos.x - WidthPanel/2f, predItemScreenPos.y +50f, WidthPanel, HeightPanel);
            if(GUI.Button(positionItem, GetComponent<CharackterItem>().upgradeName, customButton))
            {
                if(GetComponent<Upgrade_Item>()) GetComponent<Upgrade_Item>().TakeItem(); // После добавления нового персонажа нужно пересмотреть
                else if (GetComponent<Summoner_Item>()) GetComponent<Summoner_Item>().TakeItem();
                ObjectIsSee = false;
            }
        }
        
        /*if (cameraRelative.z > 0 && ObjectIsSee == true && ObjectEquipt == false)
        {
                position = new Rect((screenPosition.x - WidthPanel / 2f), (Screen.height - screenPosition.y - HeightPanel), WidthPanel, HeightPanel);
                GUI.Box(position, name);
        }*/


    }
}
