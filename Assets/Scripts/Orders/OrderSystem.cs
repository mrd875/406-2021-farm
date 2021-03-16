using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Mirror;

public class OrderSystem : NetworkBehaviour
{
    // RNG synced from the server
    public ServRandom servRandom; 

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
    private List<Order> orders = new List<Order>();
    private List<Order> activeOrders = new List<Order>();

    // List of currently active ticket objects
    private List<GameObject> activeTicketObjects = new List<GameObject>();

    // Timer used to automatically add orders
    private float timer;
    public float timeBetweenOrders;

    void Start()
    {
        gameObject.GetComponent<RectTransform>().anchoredPosition= new Vector2(0,0);

        // Game seed to synchronize player orders
        Random.seed = servRandom.rand;

        // Initialize the timer
        timer = timeBetweenOrders;

        CreateOrderList();
    }

    private void CreateOrderList() {
        // Create list of 20 orders 
        for(int x = 0; x < 20; x++) {
            Order newOrder = new Order();

            // Determine the number of items in each order ticket, each ticket can have up to 3 different items
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
            orders.Add(newOrder);
        }
    }


    // Update is called once per frame
    void Update()
    {
        // Add orders every x seconds(timeBetweenOrders) until the order queue is full
        if(activeOrders.Count < 3) {
            timer -= Time.deltaTime;
            if(timer <= 0.0f) {
                newTicket(orders[0]);
                timer = timeBetweenOrders;
            }
        }
        // For testing, quick way to add points
        // if(Input.GetKeyDown(KeyCode.Period)) {
        //     GameObject localplayer = ClientScene.localPlayer.gameObject;
        //     localplayer.GetComponent<PlayerScore>().UpdateScore();
        // }
    }


    // Create a new order ticket to display on screen
    private void newTicket(Order order) {
        // Add order to active list, remove from total list of orders
        activeOrders.Add(order);
        orders.Remove(order);

        // Create ticket object and add to list of tickets
        GameObject newTicket = (GameObject)Instantiate(orderTicketPrefab, Vector3.zero, Quaternion.identity);
        activeTicketObjects.Add(newTicket);
        newTicket.transform.SetParent(this.transform);

        // Set sprites and amounts for each item in the order
        for(int x = 0; x < order.items; x++) {
            GameObject newItem = (GameObject)Instantiate(orderItemPrefab, Vector3.zero, Quaternion.identity);
            newItem.transform.SetParent(newTicket.transform.GetChild(1));

            newItem.transform.GetChild(0).GetComponent<Image>().sprite = order.orderSprites[x];
            newItem.transform.GetChild(1).GetComponent<TextMeshProUGUI>().SetText("X " + order.orderAmounts[x]);
        }
        newTicket.transform.GetChild(3).GetComponent<TextMeshProUGUI>().SetText("$" + order.points * 5 + ".00");
    }


    // Remove an order ticket from the screen
    private void CompleteTicket(int index) {
        // Find the ticket object at the given index, remove it from the list, and destroy it
        GameObject ticketToRemove = activeTicketObjects[index];
        activeTicketObjects.Remove(ticketToRemove);
        Destroy(ticketToRemove);

        // Remove the order from the active list
        int points = activeOrders[index].points;
        activeOrders.RemoveAt(index);

        // Add money to the players total
        PlayerData.AddMoney(points * 5);
        
        // Increase the players score
        GameObject localplayer = ClientScene.localPlayer.gameObject;
        localplayer.GetComponent<PlayerScore>().UpdateScore();
    }


    // Check if any of the active orders contain the given produce
    // If they do, return true AND the index of the first order to contain that produce
    public (bool, int) CheckTickets(string name) {
        for(int x = 0; x < activeOrders.Count; x++) {
            if(activeOrders[x].OrderContains(name)) {
                return (true, x);
            }
        }
        return (false, 0);
    }


    // Update the item amounts on the given ticket
    // CheckTickets should have been called and returned true before calling UpdateTicket
    public void UpdateTicket(string name, int index) {
        int itemIndex = activeOrders[index].UpdateOrder(name);

        // Get the ticket object at the given index, get it's child "Order List", get the "Order Item" at the itemIndex within "Order List"
        // Get the "Quantity object of "Order Item" and it's TMP component, update the text to relfect the new total
        activeTicketObjects[index].transform.GetChild(1).GetChild(itemIndex).GetChild(1).GetComponent<TextMeshProUGUI>().SetText("X " + activeOrders[index].orderAmounts[itemIndex]);

        if(activeOrders[index].CheckOrder()) {
            CompleteTicket(index);
        }
    }
}
