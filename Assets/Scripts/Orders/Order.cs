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

    // Add an item to the order
    public void AddItem(string name, Sprite sprite, int amount) {
        orderNames.Add(name);
        orderSprites.Add(sprite);
        orderAmounts.Add(amount);
        items++;
    }


    // Check if the order is complete
    // True if complete, false otherwise
    public bool CheckOrder() {
        foreach(int amount in orderAmounts) {
            if(amount > 0) {
                return false;
            }
        }
        return true;
    }


    // Check if the order contains the given produce and it's value is not 0
    public bool OrderContains(string name) {
        for(int x = 0; x < items; x++) {
            if((name.Equals(orderNames[x])) && (orderAmounts[x] > 0)) {
                return true;
            }
        }
        return false;
    }


    // Updates the total on the order and return the index of the item that was updated
    public int UpdateOrder(string name) {
        for(int x = 0; x < items; x++) {
            if(name.Equals(orderNames[x]) && (orderAmounts[x] > 0)) {
                orderAmounts[x]--;
                return x;
            }
        }
        return 0;
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
