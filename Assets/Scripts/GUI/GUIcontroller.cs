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

    bool ObjectIsSee = false;
    public bool ObjectEquipt = false;

    //Размеры панели
    float WidthPanel = 180f;
    float HeightPanel = 100f;
    float ItemsValue;

    public Material mat;

    // Use this for initialization
    void Start () {
        PredItem = GetComponent<Transform>();
        ThisObject = GetComponent<Transform>();
        customButton = GameObject.FindWithTag("Manager").GetComponent<GUICustomStyle>().customButton;
    }
	
	// Update is called once per frame
	void Update () {
        ObjectIsSee = false;
        //GameObject.Find("Image").transform.position = screenPosition;
    }

    public void ObjectSee(Transform posItem, float avs) {
        ObjectIsSee = true;
        PredItem = posItem;
        ItemsValue = avs;
    }

    void OnGUI()
    {
        screenPosition = Camera.main.WorldToScreenPoint(gameObject.transform.position);
        cameraRelative = Camera.main.transform.InverseTransformPoint(transform.position);
        predItemScreenPos = Camera.main.WorldToScreenPoint(PredItem.transform.position);
        predItemScreenPos.x += 180f;
        if (cameraRelative.z > 0 && ObjectIsSee == true && ObjectEquipt == false)
        {
            positionItem = new Rect((predItemScreenPos.x - WidthPanel / 2f) - (WidthPanel+10) * ItemsValue, (Screen.height - predItemScreenPos.y - HeightPanel - 20f), WidthPanel, HeightPanel);
            positionLine = new Rect((predItemScreenPos.x - WidthPanel / 2f) - 140f * ItemsValue, (predItemScreenPos.y + 20f), WidthPanel, HeightPanel);
            if(GUI.Button(positionItem, name, customButton))
            {
                GetComponent<Upgrade_Item>().TakeItem();
            }
        }
        /*if (cameraRelative.z > 0 && ObjectIsSee == true && ObjectEquipt == false)
        {
                position = new Rect((screenPosition.x - WidthPanel / 2f), (Screen.height - screenPosition.y - HeightPanel), WidthPanel, HeightPanel);
                GUI.Box(position, name);
        }*/
        
        
    }

    private void OnDrawGizmos()
    {
        if (ObjectIsSee == true && ObjectEquipt == false)
            RenderLines(new Vector3[0]);
    }

    void OnRenderObject()
    {
        if (ObjectIsSee == true && ObjectEquipt == false)
            RenderLines(new Vector3[0]);
    }

    private void OnPostRender()
    {
        if (ObjectIsSee == true && ObjectEquipt == false)
            RenderLines(new Vector3[0]);
    }

    void RenderLines(Vector3[] points)
    {
        GL.Begin(GL.LINES);
        mat.SetPass(0);
        GL.Color(Color.red);
        GL.Color(mat.color);
        GL.Vertex(ThisObject.position);
        GL.Vertex(Camera.main.ScreenToWorldPoint(positionLine.position));
        GL.End();
    }
}
