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
    float WidthPanel = 380f;
    float HeightPanel = 180f;
    float ItemsValue;

    public Material mat;

    // Use this for initialization
    void Start () {
        PredItem = GetComponent<Transform>();
        ThisObject = GetComponent<Transform>();
        customButton = GameObject.FindWithTag("Manager").GetComponent<GUICustomStyle>().customButton;
    }
	
	// Update is called once per frame
	void FixedUpdate() {
        //ObjectIsSee = false;
    }

    public void ObjectSee(Transform posItem) {
        ObjectIsSee = true;
        PredItem = posItem;
    }

    void OnGUI()
    {
        screenPosition = Camera.main.WorldToScreenPoint(gameObject.transform.position);
        cameraRelative = Camera.main.transform.InverseTransformPoint(transform.position);
        predItemScreenPos = Camera.main.WorldToScreenPoint(PredItem.transform.position);
        predItemScreenPos.x += 180f;
        if (cameraRelative.z > 0 && ObjectIsSee == true && ObjectEquipt == false)
        {
            positionItem = new Rect((predItemScreenPos.x + WidthPanel) - (WidthPanel+50), (Screen.height - predItemScreenPos.y - HeightPanel - 50f), WidthPanel, HeightPanel);
            positionLine = new Rect((predItemScreenPos.x + WidthPanel) - 180f, (predItemScreenPos.y + 50f), WidthPanel, HeightPanel);
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

    void OnDrawGizmos()
    {
        if (ObjectIsSee == true && ObjectEquipt == false)
            RenderLines(new Vector3[0]);
    }

    void OnRenderObject()
    {
        if (ObjectIsSee == true && ObjectEquipt == false)
            RenderLines(new Vector3[0]);
    }

    void OnPostRender()
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
