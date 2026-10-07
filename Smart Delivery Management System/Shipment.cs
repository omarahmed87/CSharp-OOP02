using System;
using static Program;

// Implements ICloneable interface
public abstract partial class Shipment : ICloneable
{
    // Static counter shared across all shipments
    private static int totalShipmentsCreated;

    public static int TotalShipmentsCreated => totalShipmentsCreated;

    // Static constructor runs automatically before first use
    static Shipment()
    {
        totalShipmentsCreated = 0;
        Console.WriteLine("Shipment System Initialized");
    }

    // Static method to get total shipments created
    public static int GetTotalShipmentsCreated()
    {
        return totalShipmentsCreated;
    }

    // Instance fields
    private string trackingCode;
    private string description;
    private decimal weight;
    private decimal deliveryFee;

    public string TrackingCode => trackingCode;

    public string Description
    {
        get => description;
        set
        {
            if (!string.IsNullOrWhiteSpace(value))
                description = value;
        }
    }

    public decimal Weight
    {
        get => weight;
        set
        {
            if (value > 0)
                weight = value;
        }
    }

    public decimal DeliveryFee
    {
        get => deliveryFee;
        private set
        {
            if (value > 0)
                deliveryFee = value;
        }
    }

    public DeliveryAddress Destination { get; set; }

    public abstract decimal EstimatedCost { get; }

    // Base constructor increments the shared counter
    public Shipment(string trackingCode, string description, decimal weight, decimal deliveryFee, DeliveryAddress destination)
    {
        this.trackingCode = string.IsNullOrWhiteSpace(trackingCode) ? "UNKNOWN" : trackingCode;
        this.description = !string.IsNullOrWhiteSpace(description) ? description : "Unknown";
        this.weight = weight > 0 ? weight : 1;
        this.deliveryFee = deliveryFee > 0 ? deliveryFee : 50;
        this.Destination = destination;

        totalShipmentsCreated++;
    }

    // Q1: Method to copy current shipment
    public virtual Shipment CopyShipment()
    {
        return (Shipment)this.Clone();
    }

    // Q2: Shallow Copy using MemberwiseClone
    public Shipment ShallowCopy()
    {
        return (Shipment)this.MemberwiseClone();
    }

    // ICloneable Implementation
    public object Clone()
    {
        return this.MemberwiseClone();
    }

    // Q3: Deep Copy method overridden in child classes
    public abstract Shipment DeepCopy();

    public void UpdateWeight(decimal newWeight)
    {
        if (newWeight > 0)
            Weight = newWeight;
    }

    public void UpdateWeight(decimal baseWeight, decimal extraPackingWeight)
    {
        if (baseWeight > 0 && extraPackingWeight >= 0)
            Weight = baseWeight + extraPackingWeight;
    }

    public virtual void PrintShipment()
    {
        Console.WriteLine($"Tracking Code : {TrackingCode}");
        Console.WriteLine($"Description   : {Description}");
        Console.WriteLine($"Destination   : {Destination?.GetFullAddress()}");
        Console.WriteLine($"Weight        : {Weight} KG");
        Console.WriteLine($"Delivery Fee  : {DeliveryFee} EGP");
        Console.WriteLine($"Estimated Cost: {EstimatedCost} EGP");
    }
}