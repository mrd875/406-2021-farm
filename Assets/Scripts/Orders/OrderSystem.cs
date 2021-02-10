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
    public int minOrderSize;
    public int maxOrderSize;

    // Player One's list of total orders, active orders, and number of orders completed
    private List<Order> pOneOrders = new List<Order>();
    private List<Order> pOneActiveOrders = new List<Order>();
    private int pOneOrdersComplete = 0;

    private List<GameObject> pOneOrderTickets = new List<GameObject>();

    // Player Two's list of orders
    private List<Order> pTwoOrders = new List<Order>();


    // Start is called before the first frame update
    void Start()
    {
        // Create two identical lists of orders, one for each player
        for(int x = 0; x < 10; x++) {
            Order newOrder = new Order();

            // Determine the number of items in each order ticket, each ticket can have 3
            // different items
            int itemsPerTicket = Random.Range(1, 4); 

            for(int y = 0; y < itemsPerTicket; y++) {
                int randProduce = Random.Range(0, produceNames.Count);
                string orderProduce = produceNames[randProduce];
                Sprite orderSprite = produceSprites[randProduce];
                int orderSize = Random.Range(minOrderSize, maxOrderSize+1);

                newOrder.AddItem(orderProduce, orderSprite, orderSize);
            }

            pOneOrders.Add(newOrder);
            pTwoOrders.Add(newOrder);
        }

        StartCoroutine(LateStart());
    }

    IEnumerator LateStart() {
        yield return new WaitForSeconds(1f);
        
        pOneActiveOrders.Add(pOneOrders[pOneOrdersComplete]);

        newTicket(pOneOrders[pOneOrdersComplete]);
    }

    private void newTicket(Order order) {
        GameObject newTicket = (GameObject)Instantiate(orderTicketPrefab, Vector3.zero, Quaternion.identity);
        newTicket.transform.SetParent(this.transform);

        for(int x = 0; x < order.items; x++) {
            GameObject newItem = (GameObject)Instantiate(orderItemPrefab, Vector3.zero, Quaternion.identity);
            newItem.transform.SetParent(newTicket.transform.GetChild(1));

            newItem.transform.GetChild(0).GetComponent<Image>().sprite = order.orderSprites[x];
            newItem.transform.GetChild(1).GetComponent<TextMeshProUGUI>().SetText("X " + order.orderAmounts[x]);
        }

    }





    // // Update is called once per frame
    // void Update()
    // {
    //     // If "." is pressed, update Player One
    //     if(Input.GetKeyDown(KeyCode.Period)) {
    //         UpdatePlayer(pOneOrders, pOneProduceText, pOneOrderAmountText);
    //     }
    //     // If "/" is pressed, update Player Two
    //     if(Input.GetKeyDown(KeyCode.Slash)) {
    //         UpdatePlayer(pTwoOrders, pTwoProduceText, pTwoOrderAmountText);
    //     }
    // }

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
