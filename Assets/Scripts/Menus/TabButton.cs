using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TabButton : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler, IPointerExitHandler
{
    // Tab group holding all of the tab buttons
    public TabGroup tabGroup;

    // Sprite that changes on idle, hover, and click
    public Image background;

    public void OnPointerClick(PointerEventData eventData) {
        tabGroup.OnTabSelected(this);
    }

    public void OnPointerEnter(PointerEventData eventData) {
        tabGroup.OnTabEnter(this);
    }

    public void OnPointerExit(PointerEventData eventData) {
        tabGroup.OnTabExit(this);
    }

    // Start is called before the first frame update
    void Start() {
        background = GetComponent<Image>();
        tabGroup.Subscribe(this);
    }
}
