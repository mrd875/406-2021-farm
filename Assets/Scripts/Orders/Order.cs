using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Order
{
    public List<string> orderNames;
    public List<Image> orderSprites;
    public List<int> orderAmounts;

    public Order(List<string> initNames, List<Image> initSprites, List<int> initAmounts) {
        // Initialize orderNames
        foreach(string name in initNames) {
            orderNames.Add(name);
        }
        // Initialize orderSprites
        foreach(Image sprite in initSprites) {
            orderSprites.Add(sprite);
        }
        // Initialize orderAmounts
        foreach(int amount in initAmounts) {
            orderAmounts.Add(amount);
        }
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
