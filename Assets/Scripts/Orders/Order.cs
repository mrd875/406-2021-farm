using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Order
{
    public List<string> orderNames = new List<string>();
    public List<Sprite> orderSprites = new List<Sprite>();
    public List<int> orderAmounts = new List<int>();
    public int items = 0;

    public void AddItem(string name, Sprite sprite, int amount) {
        orderNames.Add(name);
        orderSprites.Add(sprite);
        orderAmounts.Add(amount);
        items++;
    }

    public bool CheckOrder() {
        foreach(int amount in orderAmounts) {
            if(amount > 0) {
                return false;
            }
        }
        return true;
    }
    // public void SetProduceName(string newName) {
    //     produceName = newName;
    // }

    // public string GetProduceName() {
    //     return produceName;
    // }

    // public void SetOrderAmount(int newAmount) {
    //     orderAmount = newAmount;
    // }

    // public int GetOrderAmount() {
    //     return orderAmount;
    // }
}
