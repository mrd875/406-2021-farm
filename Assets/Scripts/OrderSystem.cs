using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OrderSystem : MonoBehaviour
{
    // Variables to display Player One's orders
    public Text pOneProduceText;
    public Image pOneOrderImage;
    public Text pOneOrderAmountText;

    // Variables to display Player Two's orders
    public Text pTwoProduceText;
    public Image pTwoOrderImage;
    public Text pTwoOrderAmountText;

    // List of strings that contains the names of all produce
    public List<string> produce;

    // Minimum and maximum order sizes
    public int minOrderSize;
    public int maxOrderSize;

    // Player One's list of orders
    public List<Order> pOneOrders = new List<Order>();

    // Player Two's list of orders
    public List<Order> pTwoOrders = new List<Order>();


    // Start is called before the first frame update
    void Start()
    {
        // Create two identical lists of orders, one for each player
        for(int x = 0; x < 10; x++) {
            string orderProduce = produce[Random.Range(0, produce.Count)];
            int orderSize = Random.Range(minOrderSize, maxOrderSize+1);

            pOneOrders.Add(new Order(orderProduce, orderSize));
            pTwoOrders.Add(new Order(orderProduce, orderSize));
        }

        // Display each players current order (index 0 of each list is the current order)
        DisplayOrder(pOneOrders, pOneProduceText, pOneOrderAmountText);

        DisplayOrder(pTwoOrders, pTwoProduceText, pTwoOrderAmountText);
    }

    // Update is called once per frame
    void Update()
    {
        // If "." is pressed, update Player One
        if(Input.GetKeyDown(KeyCode.Period)) {
            UpdatePlayer(pOneOrders, pOneProduceText, pOneOrderAmountText);
        }
        // If "/" is pressed, update Player Two
        if(Input.GetKeyDown(KeyCode.Slash)) {
            UpdatePlayer(pTwoOrders, pTwoProduceText, pTwoOrderAmountText);
        }
    }

    // Decrease the total amount for the current order, if the order is fulfilled, load a new order
    private void UpdatePlayer(List<Order> playerOrders, Text produceName, Text orderAmount) {
        int currentAmount = playerOrders[0].GetOrderAmount();
        currentAmount--;

        // Remove current order
        if(currentAmount == 0) {
            playerOrders.RemoveAt(0);
        }
        // Set new current order total
        else {
            playerOrders[0].SetOrderAmount(currentAmount);
        }
        DisplayOrder(playerOrders, produceName, orderAmount);
    }

    // Display the current order to the screen
    private void DisplayOrder(List<Order> playerOrders, Text produceName, Text orderAmount) {
        produceName.text = playerOrders[0].GetProduceName();
        orderAmount.text = playerOrders[0].GetOrderAmount().ToString();
    }
}
