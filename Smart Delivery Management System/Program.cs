using System;

public class Program
{
    static void ReadInput(string prompt, out string value)
    {
        Console.Write(prompt);
        value = Console.ReadLine()!;
    }

    public static void Main()
    {
        DeliveryCenter center = new DeliveryCenter();

        // 1. read and add 3 shipments[cite: 1]
        for (int i = 0; i < 3; i++)
        {
            Console.WriteLine($"Enter Shipment{i + 1} Data");

            ReadInput("Tracking Code: ", out string code);
            ReadInput("Description: ", out string desc);

            ReadInput("Weight: ", out string weightStr);
            double.TryParse(weightStr, out double weight);

            ReadInput("Delivery Fee: ", out string feeStr);
            decimal.TryParse(feeStr, out decimal fee);

            ReadInput("City: ", out string city);
            ReadInput("Street: ", out string street);

            ReadInput("Building Number: ", out string bldStr);
            int.TryParse(bldStr, out int building);

            DeliveryAddress address = new DeliveryAddress(city, street, building);
            Shipment shipment = new Shipment(code, desc, weight, fee, address);

            if (center.AddShipment(shipment))
            {
                Console.WriteLine("Shipment added successfully.");
            }
            else
            {
                Console.WriteLine("Delivery Center is full.");
            }

            Console.WriteLine();
        }

        // 2. print all shipments using int indexer[cite: 1]
        Console.WriteLine("--- All Shipments ---");
        for (int i = 0; i < 3; i++)
        {
            center[i].PrintShipment();
            Console.WriteLine();
        }

        // 3. search shipment by code using string indexer[cite: 1]
        Console.Write("Enter a tracking code to search: ");
        string searchCode = Console.ReadLine();

        Shipment foundShipment = center[searchCode];
        if (!string.IsNullOrEmpty(foundShipment.TrackingCode))
        {
            Console.WriteLine($"Shipment found:{foundShipment.TrackingCode}\t{foundShipment.Description}");
        }
        else
        {
            Console.WriteLine("Shipment not found.");
        }

        Console.WriteLine();

        // 4. test struct copy behavior[cite: 1]
        Console.WriteLine("Struct Copy Test");
        DeliveryAddress originalAddress = new DeliveryAddress("Cairo", "Tahrir Street", 15);
        DeliveryAddress copiedAddress = originalAddress;

        copiedAddress.City = "Cairo";
        copiedAddress.Street = "Makram Ebeid Street";
        copiedAddress.BuildingNumber = 20;

        Console.WriteLine($"Original Address:{originalAddress.GetFullAddress()}");
        Console.WriteLine($"Copied Address:{copiedAddress.GetFullAddress()}");
    }
}

// DeliveryAddress struct
public struct DeliveryAddress
{
    // fields
    public string City;
    public string Street;
    public int BuildingNumber;

    // constructor
    public DeliveryAddress(string city, string street, int buildingNumber)
    {
        City = city;
        Street = street;
        BuildingNumber = buildingNumber;
    }

    // get full address string
    public string GetFullAddress()
    {
        return $"{BuildingNumber}{Street},{City}";
    }
}

// Shipment struct
public struct Shipment
{
    // private fields
    private string trackingCode;
    private string description;
    private double weight;
    private decimal deliveryFee;

    // properties with validation
    public string TrackingCode
    {
        get { return trackingCode; }
    }

    public string Description
    {
        get { return description; }
        set
        {
            if (!string.IsNullOrWhiteSpace(value))
                description = value;
        }
    }

    public double Weight
    {
        get { return weight; }
        set
        {
            if (value > 0)
                weight = value;
        }
    }

    public decimal DeliveryFee
    {
        get { return deliveryFee; }
        private set
        {
            if (value > 0)
                deliveryFee = value;
        }
    }

    public DeliveryAddress Destination { get; set; }

    // calculated cost property
    public decimal EstimatedCost
    {
        get { return DeliveryFee + ((decimal)Weight * 5); }
    }

    // constructor with default values[cite: 1]
    public Shipment(string trackingCode)
    {
        this.trackingCode = string.IsNullOrWhiteSpace(trackingCode) ? "UNKNOWN" : trackingCode;
        this.description = "Unknown";
        this.weight = 1;
        this.deliveryFee = 50;
        this.Destination = default;
    }

    // full constructor
    public Shipment(string trackingCode, string description, double weight, decimal deliveryFee, DeliveryAddress destination)
    {
        this.trackingCode = string.IsNullOrWhiteSpace(trackingCode) ? "UNKNOWN" : trackingCode;
        this.description = !string.IsNullOrWhiteSpace(description) ? description : "Unknown";
        this.weight = weight > 0 ? weight : 1;
        this.deliveryFee = deliveryFee > 0 ? deliveryFee : 50;
        this.Destination = destination;
    }

    // update fee method
    public void UpdateDeliveryFee(decimal newFee)
    {
        if (newFee > 0)
            deliveryFee = newFee;
    }

    // print method
    public void PrintShipment()
    {
        Console.WriteLine($"Tracking Code:{TrackingCode}");
        Console.WriteLine($"Description:{Description}");
        Console.WriteLine($"Weight:{Weight} KG");
        Console.WriteLine($"Delivery Fee:{DeliveryFee} EGP");
        Console.WriteLine($"Destination:{Destination.GetFullAddress()}");
        Console.WriteLine($"Estimated Cost:{EstimatedCost} EGP");
    }
}

// DeliveryCenter struct
public struct DeliveryCenter
{
    private Shipment[] shipments;
    private int count;

    // add shipment to array
    public bool AddShipment(Shipment shipment)
    {
        if (shipments == null)
        {
            shipments = new Shipment[10];
            count = 0;
        }

        if (count < shipments.Length)
        {
            shipments[count] = shipment;
            count++;
            return true;
        }

        return false;
    }

    // int indexer: access by position[cite: 1]
    public Shipment this[int index]
    {
        get
        {
            if (shipments != null && index >= 0 && index < count)
                return shipments[index];

            return default;
        }
        set
        {
            if (shipments != null && index >= 0 && index < count)
                shipments[index] = value;
        }
    }

    // string indexer: find by tracking code[cite: 1]
    public Shipment this[string searchCode]
    {
        get
        {
            if (shipments != null && !string.IsNullOrWhiteSpace(searchCode))
            {
                for (int i = 0; i < count; i++)
                {
                    if (shipments[i].TrackingCode == searchCode)
                        return shipments[i];
                }
            }
            return default;
        }
    }
}