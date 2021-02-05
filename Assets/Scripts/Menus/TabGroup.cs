using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TabGroup : MonoBehaviour
{
    // List of all tab buttons part of this tab group
    public List<TabButton> tabButtons;

    // Different sprites for the different states the tab buttons can be in
    // Idle, Hover, and Active
    public Color tabIdle;
    public Color tabHover;
    public Color tabActive;

    // The currently selected tab
    public TabButton selectedTab;

    // List of pages to displayed depending on the tab
    public List<GameObject> shopPages;

    // Subscribe function, adds all tab buttons in the group to a single list
    public void Subscribe(TabButton button) {
        if(tabButtons == null) {
            tabButtons = new List<TabButton>();
        }

        tabButtons.Add(button);
    }

    // Cursor hovering over a tab button
    public void OnTabEnter(TabButton button) {
        ResetTabs();
        if(selectedTab == null || button != selectedTab) {
            button.background.color = tabHover;
        }
    }

    // Moving cursor off of a hovered tab button
    public void OnTabExit(TabButton button) {
        ResetTabs();
    }

    // Selecting a tab button
    public void OnTabSelected(TabButton button) {
        selectedTab = button;
        ResetTabs();
        button.background.color = tabActive;

        // Find and set activethe shop page with the corresponding index to the tab button 
        // selected. Set all other pages to not active
        int index = button.transform.GetSiblingIndex();
        for(int i=0; i < shopPages.Count; i++) {
            if(i == index) {
                shopPages[i].SetActive(true);
            }
            else {
                shopPages[i].SetActive(false);
            }
        }
    }

    // Set all tabs not selected to be idle
    public void ResetTabs() {
        foreach(TabButton button in tabButtons) {
            if(selectedTab != null && button == selectedTab) {
                continue;
            }
            button.background.color = tabIdle;
        }
    }
}
