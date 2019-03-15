using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SnapScroll : MonoBehaviour {

    public int chCount;
    public int panelOffset;
    public float timeSnap;
    public float alphaOffset;
    public GameObject chPanelPrefab;

    public int selectPanelID;
    public bool isDrag = false;
    public bool startMenu = false;

    private GameObject[] instPanels;
    private Vector2[] panelsPos;
    private Image[] panelImage;

    private RectTransform contRect;
    private Vector2 contVect;
    private Color panelColor;

    MainMenu mainMenu;


	// Use this for initialization
	void Start () {
        mainMenu = GameObject.FindWithTag("GUI").GetComponent<MainMenu>();
        panelsPos = new Vector2[chCount];
        instPanels = new GameObject[chCount];
        panelImage = new Image[chCount];
        contRect = GetComponent<RectTransform>();
        for (int i = 0; i< chCount; i++)
        {
            instPanels[i] = Instantiate(chPanelPrefab, transform, false);
            panelImage[i] = instPanels[i].GetComponent<Image>();
            if (i == 0) continue;
            instPanels[i].transform.localPosition = new Vector2(instPanels[i-1].transform.localPosition.x + chPanelPrefab.GetComponent<RectTransform>().sizeDelta.x + panelOffset, 
                instPanels[i].transform.localPosition.y);
            panelsPos[i] = -instPanels[i].transform.localPosition;
        }
        selectPanelID = PlayerPrefs.GetInt("PalyerCharackter");   
    }

    private void FixedUpdate()
    {
        if (startMenu == true)
        {
            float nearPos = float.MaxValue;
            for (int i = 0; i < chCount; i++)
            {
                float distance = Mathf.Abs(contRect.anchoredPosition.x - panelsPos[i].x);
                if (distance < nearPos)
                {
                    nearPos = distance;
                    selectPanelID = i;
                    PlayerPrefs.SetInt("PalyerCharackter", selectPanelID);
                    mainMenu.LoadFile();
                }
                float opacity = Mathf.Clamp(1 / (distance / panelOffset) * alphaOffset, 0, 1);
                panelColor = panelImage[i].color;
                panelColor.a = opacity;
                panelImage[i].color = panelColor;

            }
        }
        if (isDrag) return;
        contVect.x = Mathf.SmoothStep(contRect.anchoredPosition.x, panelsPos[selectPanelID].x, timeSnap * Time.fixedDeltaTime);
        contRect.anchoredPosition = contVect;
        if (contRect.anchoredPosition.x-5f <= panelsPos[selectPanelID].x) startMenu = true;
    }

    public void Dragging(bool drag)
    {
        isDrag = drag;
    }
}
