using System;

public class Program
{
    #region ReadInput function
    static void ReadInput(string prompt, out string value)
    {
        Console.Write(prompt);
        value = Console.ReadLine()!;
    }
    #endregion
    #region Theoretical Q1
    /*
     Answer the following questions:
a) What is the difference between a class and a struct?

Class:
1. Class is a Reference Type; it stores the reference in the Stack and the actual data in the Heap.
2. Supports Inheritance.
3. Good for large and complex data.
4. When passed, it passes by reference (does not copy all data).

Struct:
1. Struct is a Value Type; it stores the actual value in the Stack.
2. Does NOT support Inheritance.
3. Best for small data (usually 16 bytes or less).
4. When passed, it copies the entire data.

b) Why are classes more suitable than structs for large applications?
1. Classes support OOP concepts like Inheritance, which makes the code reusable and easy to scale.
2. Classes pass references instead of copying the whole object, which gives better performance when dealing with large data.
*/
    #endregion
    #region Theoretical Q2
    /*
    Consider the following code:
    public class Shipment
    {
        public string TrackingCode { get; set; }
    }
    public class ExpressShipment : Shipment
    {
        public decimal ExtraFee { get; set; }
    }

    a) Which class is the parent class?
      :Shipment
    b) Which class is the child class?
      :ExpressShipment
    c) What members are inherited by ExpressShipment?
      :TrackingCode
    d) Why is inheritance better than duplicating the same code in multiple classes?
       1. Code Reusability: Write shared code once instead of repeating it.
       2. Easy Maintenance: Fix bugs or make updates in one place only.
     */


    #endregion



    public static void Main()
    {
        #region assignment 01
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
        #endregion

}

#region DeliveryAddress Struct
public struct DeliveryAddress
{
    #region Fields
    public string City;
    public string Street;
    public int BuildingNumber;
    #endregion

    #region Constructors
    public DeliveryAddress(string city, string street, int buildingNumber)
    {
        City = city;
        Street = street;
        BuildingNumber = buildingNumber;
    }
    #endregion

    #region Methods
    public string GetFullAddress()
    {
        return $"{BuildingNumber} {Street}, {City}";
    }
    #endregion
}
#endregion

#region Shipment Class
public class Shipment
{
    #region Private Fields
    private string trackingCode;
    private string description;
    private decimal weight;
    private decimal deliveryFee;
    #endregion

    #region Properties
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

    public decimal Weight
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

    // virtual to allow child classes to override calculation
    public virtual decimal EstimatedCost
    {
        get { return DeliveryFee + (Weight * 5); }
    }
    #endregion

    #region Constructors
    public Shipment(string trackingCode)
    {
        this.trackingCode = string.IsNullOrWhiteSpace(trackingCode) ? "UNKNOWN" : trackingCode;
        this.description = "Unknown";
        this.weight = 1;
        this.deliveryFee = 50;
        this.Destination = default;
    }

    public Shipment(string trackingCode, string description, decimal weight, decimal deliveryFee, DeliveryAddress destination)
    {
        this.trackingCode = string.IsNullOrWhiteSpace(trackingCode) ? "UNKNOWN" : trackingCode;
        this.description = !string.IsNullOrWhiteSpace(description) ? description : "Unknown";
        this.weight = weight > 0 ? weight : 1;
        this.deliveryFee = deliveryFee > 0 ? deliveryFee : 50;
        this.Destination = destination;
    }
    #endregion

    #region Methods
    public void UpdateDeliveryFee(decimal newFee)
    {
        if (newFee > 0)
            deliveryFee = newFee;
    }

    public virtual void PrintShipment()
    {
        Console.WriteLine($"Tracking Code : {TrackingCode}");
        Console.WriteLine($"Description   : {Description}");
        Console.WriteLine($"Weight        : {Weight} KG");
        Console.WriteLine($"Delivery Fee  : {DeliveryFee} EGP");
        Console.WriteLine($"Estimated Cost: {EstimatedCost} EGP");
    }
    #endregion
}
#endregion

#region Shipment Types (Inheritance)

#region StandardShipment Class
public class StandardShipment : Shipment
{
    public StandardShipment(string trackingCode, string description, decimal weight, decimal deliveryFee, DeliveryAddress destination)
        : base(trackingCode, description, weight, deliveryFee, destination)
    {
    }
}
#endregion

#region ExpressShipment Class
public class ExpressShipment : Shipment
{
    private decimal extraFee;

    public decimal ExtraFee
    {
        get { return extraFee; }
        set
        {
            if (value >= 0)
                extraFee = value;
        }
    }

    public override decimal EstimatedCost
    {
        get { return base.EstimatedCost + ExtraFee; }
    }

    public ExpressShipment(string trackingCode, string description, decimal weight, decimal deliveryFee, DeliveryAddress destination, decimal extraFee)
        : base(trackingCode, description, weight, deliveryFee, destination)
    {
        ExtraFee = extraFee;
    }

    public override void PrintShipment()
    {
        base.PrintShipment();
        Console.WriteLine($"Extra Fee     : {ExtraFee} EGP");
    }
}
#endregion

#region InternationalShipment Class
public class InternationalShipment : Shipment
{
    private string destinationCountry;
    private decimal customsFee;

    public string DestinationCountry
    {
        get { return destinationCountry; }
        set
        {
            if (!string.IsNullOrWhiteSpace(value))
                destinationCountry = value;
        }
    }

    public decimal CustomsFee
    {
        get { return customsFee; }
        set
        {
            if (value >= 0)
                customsFee = value;
        }
    }

    public override decimal EstimatedCost
    {
        get { return base.EstimatedCost + CustomsFee; }
    }

    public InternationalShipment(string trackingCode, string description, decimal weight, decimal deliveryFee, DeliveryAddress destination, string destinationCountry, decimal customsFee)
        : base(trackingCode, description, weight, deliveryFee, destination)
    {
        DestinationCountry = destinationCountry;
        CustomsFee = customsFee;
    }

    public override void PrintShipment()
    {
        base.PrintShipment();
        Console.WriteLine($"Country       : {DestinationCountry}");
        Console.WriteLine($"Customs Fee   : {CustomsFee} EGP");
    }
}
#endregion

#endregion

#region DeliveryCenter Class
public class DeliveryCenter
{
    #region Properties & Fields
    public string CenterName { get; set; }
    private Shipment[] shipments;
    private int count;
    #endregion

    #region Constructors
    public DeliveryCenter(string centerName = "Main Center")
    {
        CenterName = centerName;
        shipments = new Shipment[20];
        count = 0;
    }
    #endregion

    #region Indexers
    // int indexer: access by position
    public Shipment this[int index]
    {
        get
        {
            if (shipments != null && index >= 0 && index < count)
                return shipments[index];

            return null;
        }
        set
        {
            if (shipments != null && index >= 0 && index < count)
                shipments[index] = value;
        }
    }

    // string indexer: find by tracking code
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
            return null;
        }
    }
    #endregion

    #region Methods
    public bool AddShipment(Shipment shipment)
    {
        if (shipment == null)
            return false;

        if (count < shipments.Length)
        {
            shipments[count] = shipment;
            count++;
            return true;
        }

        return false;
    }

    public bool RemoveShipment(string trackingCode)
    {
        if (string.IsNullOrWhiteSpace(trackingCode) || count == 0)
            return false;

        int foundIndex = -1;
        for (int i = 0; i < count; i++)
        {
            if (shipments[i].TrackingCode == trackingCode)
            {
                foundIndex = i;
                break;
            }
        }

        if (foundIndex == -1)
            return false;

        // Shift elements to the left to remove item
        for (int i = foundIndex; i < count - 1; i++)
        {
            shipments[i] = shipments[i + 1];
        }

        shipments[count - 1] = null;
        count--;
        return true;
    }

    public void PrintAllShipments()
    {
        Console.WriteLine("==================================================");
        Console.WriteLine($"Delivery Center : {CenterName}");
        Console.WriteLine("==================================================");

        for (int i = 0; i < count; i++)
        {
            if (shipments[i] is StandardShipment)
                Console.WriteLine("Standard Shipment\n");
            else if (shipments[i] is ExpressShipment)
                Console.WriteLine("Express Shipment\n");
            else if (shipments[i] is InternationalShipment)
                Console.WriteLine("International Shipment\n");

            shipments[i].PrintShipment();
            Console.WriteLine("--------------------------------------------------");
        }
    }
    #endregion
}
#endregion