using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Order : MonoBehaviour
{
    public string produceName = "";
    public int orderAmount = 0;

    public Order(string initName, int initAmount) {
        produceName = initName;
        orderAmount = initAmount;
    }

    public void SetProduceName(string newName) {
        produceName = newName;
    }

    public string GetProduceName() {
        return produceName;
    }

    public void SetOrderAmount(int newAmount) {
        orderAmount = newAmount;
    }

    public int GetOrderAmount() {
        return orderAmount;
    }
}
