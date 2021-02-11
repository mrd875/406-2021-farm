using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class OrderSystem : MonoBehaviour
{
    // Order UI Prefabs
    public GameObject orderTicketPrefab;
    public GameObject orderItemPrefab;

    // List of strings that contains the names of all produce
    public List<string> produceNames;

    // List of all sprites of all produce
    public List<Sprite> produceSprites;

    // Minimum and maximum order sizes
    public int minItemAmount;
    public int maxItemAmount;

    // Player One's list of total orders, active orders, and number of orders completed
    private List<Order> oneOrders = new List<Order>();
    private List<Order> oneActiveOrders = new List<Order>();
    private int oneOrdersComplete = 0;

    // List of currently active ticket objects
    private List<GameObject> oneActiveTicketObjects = new List<GameObject>();

    // Player Two's list of orders
    // private List<Order> pTwoOrders = new List<Order>();

    // Timer used to automatically add orders
    private float timer = 1.0f;
    public float timeBetweenOrders;


    void Start()
    {

        timer = timeBetweenOrders;

        // Create two identical lists of orders, one for each player
        for(int x = 0; x < 10; x++) {
            Order newOrder = new Order();

            // Determine the number of items in each order ticket, each ticket can have 3 different items
            int itemsPerTicket = Random.Range(1, 4); 

            for(int y = 0; y < itemsPerTicket; y++) {
                int randProduce = Random.Range(0, produceNames.Count); // rand index to be used

                // Use the randProduce as the index to get the random produce name and corresponding sprite
                string orderProduce = produceNames[randProduce];
                Sprite orderSprite = produceSprites[randProduce];
                int orderSize = Random.Range(minItemAmount, maxItemAmount + 1);

                newOrder.AddItem(orderProduce, orderSprite, orderSize);
            }
            // Add the newly created order to each players total list of orders
            oneOrders.Add(newOrder);
            // pTwoOrders.Add(newOrder);
        }
    }


    // Update is called once per frame
    void Update()
    {
        // Add orders until the order queue is full
        if(oneActiveOrders.Count < 3) {
            timer -= Time.deltaTime;
            if(timer <= 0.0f) {
                newTicket(oneOrders[0]);
                timer = timeBetweenOrders;
            }
        }
        if(Input.GetKeyDown(KeyCode.Space)) {
            CompleteTicket(0);
        }
    }


    // Create a new order ticket to display on screen
    private void newTicket(Order order) {
        // Add order to active list, remove from total list of orders
        oneActiveOrders.Add(order);
        oneOrders.Remove(order);

        // Create ticket object and add to list of tickets
        GameObject newTicket = (GameObject)Instantiate(orderTicketPrefab, Vector3.zero, Quaternion.identity);
        oneActiveTicketObjects.Add(newTicket);
        newTicket.transform.SetParent(this.transform);

        // Set sprites and amounts for each item in the order
        for(int x = 0; x < order.items; x++) {
            GameObject newItem = (GameObject)Instantiate(orderItemPrefab, Vector3.zero, Quaternion.identity);
            newItem.transform.SetParent(newTicket.transform.GetChild(1));

            newItem.transform.GetChild(0).GetComponent<Image>().sprite = order.orderSprites[x];
            newItem.transform.GetChild(1).GetComponent<TextMeshProUGUI>().SetText("X " + order.orderAmounts[x]);
        }
    }


    // Remove an order ticket from the screen
    private void CompleteTicket(int orderIndex) {
        // Find the ticket object at the given index, remove it from the list, and destroy it
        GameObject ticketToRemove = oneActiveTicketObjects[orderIndex];
        oneActiveTicketObjects.Remove(ticketToRemove);
        Destroy(ticketToRemove);

        // Remove the order from the active list
        oneActiveOrders.RemoveAt(orderIndex);
    }


    public void UpdateTicket(string name) {



    }

    // // Decrease the total amount for the current order, if the order is fulfilled, load a new order
    // private void UpdatePlayer(List<Order> playerOrders, Text produceName, Text orderAmount) {
    //     int currentAmount = playerOrders[0].GetOrderAmount();
    //     currentAmount--;

    //     // Remove current order
    //     if(currentAmount == 0) {
    //         playerOrders.RemoveAt(0);
    //     }
    //     // Set new current order total
    //     else {
    //         playerOrders[0].SetOrderAmount(currentAmount);
    //     }
    //     DisplayOrder(playerOrders, produceName, orderAmount);
    // }

    // // Display the current order to the screen
    // private void DisplayOrder(List<Order> playerOrders) {
    //     GameObject
    // }
}
