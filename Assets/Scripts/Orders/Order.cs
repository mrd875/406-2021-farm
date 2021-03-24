using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class Order
{
    public class OrderItem
    {
        public string Name { get; }

        public Sprite Sprite { get; }

        public int Amount { get; set; }

        public int StartAmount { get; }

        public OrderItem(string name, Sprite sprite, int amount)
        {
            Name = name;
            Sprite = sprite;
            Amount = amount;
            StartAmount = amount;
        }
    }

    public List<OrderItem> OrderItems { get; } = new List<OrderItem>();

    public int Points
    {
        get { return OrderItems.Sum(ele => ele.StartAmount); }
    }

    // Add an item to the order
    public void AddItem(string name, Sprite sprite, int amount)
    {
        OrderItems.Add(new OrderItem(name, sprite, amount));
    }

    // Check if the order is complete
    // True if complete, false otherwise
    public bool CheckOrder()
    {
        return OrderItems.Any(ele => ele.Amount > 0);
    }

    // Check if the order contains the given produce and it's value is not 0
    public bool OrderContains(string name)
    {
        return OrderItems.Any(ele => ele.Name.Equals(name) && ele.Amount > 0);
    }

    // Updates the total on the order and return the index of the item that was updated
    public int UpdateOrder(string name)
    {
        for (var x = 0; x < OrderItems.Count; x++)
        {
            var item = OrderItems[x];
            if (!name.Equals(item.Name) || item.Amount <= 0) continue;
            item.Amount--;
            return x;
        }

        return 0;
    }
}
