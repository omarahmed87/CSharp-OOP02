using System;

#region DeliveryAddress Class
// Implements ICloneable to support deep copying
public class DeliveryAddress : ICloneable
{
    public string City { get; set; }
    public string Street { get; set; }
    public int BuildingNumber { get; set; }

    public DeliveryAddress(string city, string street, int buildingNumber)
    {
        City = city;
        Street = street;
        BuildingNumber = buildingNumber;
    }

    public DeliveryAddress(DeliveryAddress other)
    {
        if (other != null)
        {
            City = other.City;
            Street = other.Street;
            BuildingNumber = other.BuildingNumber;
        }
    }

    public object Clone()
    {
        return new DeliveryAddress(this.City, this.Street, this.BuildingNumber);
    }

    public string GetFullAddress()
    {
        return $"{BuildingNumber} {Street}, {City}";
    }
}
#endregion

#region Driver Class
public class Driver
{
    public string Name { get; set; }

    public Driver(string name)
    {
        Name = name;
    }
}
#endregion

#region Derived Shipment Classes
public class StandardShipment : Shipment
{
    public override decimal EstimatedCost => DeliveryFee + (Weight * 5);

    public StandardShipment(string trackingCode, string description, decimal weight, decimal deliveryFee, DeliveryAddress destination)
        : base(trackingCode, description, weight, deliveryFee, destination)
    {
    }

    public override Shipment DeepCopy()
    {
        DeliveryAddress newAddress = (DeliveryAddress)this.Destination?.Clone();
        StandardShipment copy = new StandardShipment(this.TrackingCode, this.Description, this.Weight, this.DeliveryFee, newAddress);
        copy.TrackingStatus = this.TrackingStatus;
        return copy;
    }

    public override void PrintShipment()
    {
        Console.WriteLine("Standard Shipment:");
        base.PrintShipment();
    }
}

public class ExpressShipment : Shipment
{
    private decimal extraFee;

    public decimal ExtraFee
    {
        get => extraFee;
        set
        {
            if (value >= 0)
                extraFee = value;
        }
    }

    public override decimal EstimatedCost => DeliveryFee + (Weight * 5) + ExtraFee;

    public ExpressShipment(string trackingCode, string description, decimal weight, decimal deliveryFee, DeliveryAddress destination, decimal extraFee)
        : base(trackingCode, description, weight, deliveryFee, destination)
    {
        ExtraFee = extraFee;
    }

    public override Shipment DeepCopy()
    {
        DeliveryAddress newAddress = (DeliveryAddress)this.Destination?.Clone();
        ExpressShipment copy = new ExpressShipment(this.TrackingCode, this.Description, this.Weight, this.DeliveryFee, newAddress, this.ExtraFee);
        copy.TrackingStatus = this.TrackingStatus;
        return copy;
    }

    public override void PrintShipment()
    {
        Console.WriteLine("Express Shipment:");
        base.PrintShipment();
        Console.WriteLine($"Extra Fee     : {ExtraFee} EGP");
    }
}

public class InternationalShipment : Shipment
{
    private decimal customsFee;

    public string DestinationCountry { get; set; }

    public decimal CustomsFee
    {
        get => customsFee;
        set
        {
            if (value >= 0)
                customsFee = value;
        }
    }

    public override decimal EstimatedCost => DeliveryFee + (Weight * 5) + CustomsFee;

    public InternationalShipment(string trackingCode, string description, decimal weight, decimal deliveryFee, DeliveryAddress destination, string destinationCountry, decimal customsFee)
        : base(trackingCode, description, weight, deliveryFee, destination)
    {
        DestinationCountry = destinationCountry;
        CustomsFee = customsFee;
    }

    public override Shipment DeepCopy()
    {
        DeliveryAddress newAddress = (DeliveryAddress)this.Destination?.Clone();
        InternationalShipment copy = new InternationalShipment(this.TrackingCode, this.Description, this.Weight, this.DeliveryFee, newAddress, this.DestinationCountry, this.CustomsFee);
        copy.TrackingStatus = this.TrackingStatus;
        return copy;
    }

    public override void PrintShipment()
    {
        Console.WriteLine("International Shipment:");
        base.PrintShipment();
        Console.WriteLine($"Country       : {DestinationCountry}");
        Console.WriteLine($"Customs Fee   : {CustomsFee} EGP");
    }
}
#endregion

#region DeliveryCenter Class
public class DeliveryCenter
{
    public string CenterName { get; set; }
    public Driver CenterDriver { get; set; }
    private Shipment[] shipments;
    private int count;

    public DeliveryCenter(string centerName = "Main Center")
    {
        CenterName = centerName;
        shipments = new Shipment[20];
        count = 0;
    }

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
            if (shipments != null && index >= 0 && index < count && value != null)
                shipments[index] = value;
        }
    }

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
        Console.WriteLine("==========================================");
        Console.WriteLine(CenterName);
        Console.WriteLine("==========================================\n");

        if (CenterDriver != null)
        {
            Console.WriteLine($"Driver : {CenterDriver.Name}\n");
        }

        for (int i = 0; i < count; i++)
        {
            Console.WriteLine("------------------------------------------");
            shipments[i].PrintShipment();
            Console.WriteLine();
        }
    }
}
#endregion

#region Question 07 - DeliveryUtilities Static Class
public static class DeliveryUtilities
{
    public static void PrintSeparator()
    {
        Console.WriteLine("==========================================");
    }

    public static void PrintSystemTitle(string title)
    {
        Console.WriteLine("------------------------------------------");
        Console.WriteLine(title);
        Console.WriteLine("------------------------------------------");
    }
}
#endregion

#region Question 08 - ShipmentExtensions Static Class
public static class ShipmentExtensions
{
    public static string GetSummary(this Shipment shipment)
    {
        if (shipment == null)
            return string.Empty;

        string shipmentType = "Shipment";
        if (shipment is StandardShipment)
            shipmentType = "Standard";
        else if (shipment is ExpressShipment)
            shipmentType = "Express";
        else if (shipment is InternationalShipment)
            shipmentType = "International";

        return $"{shipment.TrackingCode} | {shipmentType} | {shipment.Weight} KG | {shipment.GetTrackingStatus()}";
    }

    public static bool IsDelivered(this Shipment shipment)
    {
        return shipment != null && shipment.GetTrackingStatus().Equals("Delivered", StringComparison.OrdinalIgnoreCase);
    }
}
#endregion

#region DeliveryHelper Static Class
public static class DeliveryHelper
{
    public static void PrintShipmentDetails(Shipment shipment)
    {
        if (shipment != null)
        {
            shipment.PrintShipment();
        }
    }
}
#endregion

public class Program
{
    public static void Main()
    {
        #region Question 07 - Static Utilities Header
        DeliveryUtilities.PrintSystemTitle("Smart Delivery Management System");
        #endregion

        #region Question 04, 05, 06 - Static Members & Creating Shipments
        DeliveryUtilities.PrintSeparator();
        Console.WriteLine("Creating Shipments...");
        DeliveryUtilities.PrintSeparator();

        DeliveryAddress addr1 = new DeliveryAddress("Cairo", "Tahrir St", 10);
        DeliveryAddress addr2 = new DeliveryAddress("Giza", "Haram St", 25);
        DeliveryAddress addr3 = new DeliveryAddress("Alexandria", "Corniche St", 5);

        StandardShipment standard = new StandardShipment("SH001", "Laptop", 3m, 80m, addr1);
        Console.WriteLine("Standard Shipment Created");

        ExpressShipment express = new ExpressShipment("SH002", "Mobile Phone", 2m, 60m, addr2, 30m);
        Console.WriteLine("Express Shipment Created");

        InternationalShipment international = new InternationalShipment("SH003", "Television", 8m, 120m, addr3, "Germany", 100m);
        Console.WriteLine("International Shipment Created");

        Console.WriteLine($"Total Shipments Created : {Shipment.GetTotalShipmentsCreated()}");
        #endregion

        #region Question 01 - Object Copying
        DeliveryUtilities.PrintSeparator();
        Console.WriteLine("Object Copying");
        DeliveryUtilities.PrintSeparator();

        Shipment shipment1 = standard;
        Shipment shipment2 = shipment1;

        Console.WriteLine($"Original Shipment  : {shipment1.TrackingCode}");
        Console.WriteLine($"Assigned Shipment  : {shipment2.TrackingCode}");
        Console.WriteLine($"Same Object : {ReferenceEquals(shipment1, shipment2)}");
        #endregion

        #region Question 02 - Shallow Copy
        Console.WriteLine("------------------------------------------");
        Console.WriteLine("Shallow Copy");
        Console.WriteLine("------------------------------------------");

        StandardShipment shallowOriginal = new StandardShipment("SH004", "Keyboard", 1m, 40m, new DeliveryAddress("Cairo", "Tahrir St", 10));
        StandardShipment shallowCopied = (StandardShipment)shallowOriginal.ShallowCopy();

        Console.WriteLine($"Original Shipment Address : {shallowOriginal.Destination.City}");
        Console.WriteLine($"Copied Shipment Address   : {shallowCopied.Destination.City}");
        Console.WriteLine("Changing copied shipment address...");

        shallowCopied.Destination.City = "Giza";

        Console.WriteLine($"Original Shipment Address : {shallowOriginal.Destination.City}");
        Console.WriteLine($"Copied Shipment Address   : {shallowCopied.Destination.City}");
        Console.WriteLine($"Same DeliveryAddress Object : {ReferenceEquals(shallowOriginal.Destination, shallowCopied.Destination)}");
        #endregion

        #region Question 03 - Deep Copy
        Console.WriteLine("------------------------------------------");
        Console.WriteLine("Deep Copy");
        Console.WriteLine("------------------------------------------");

        StandardShipment deepOriginal = new StandardShipment("SH005", "Mouse", 1m, 30m, new DeliveryAddress("Cairo", "Tahrir St", 10));
        StandardShipment deepCopied = (StandardShipment)deepOriginal.DeepCopy();

        Console.WriteLine($"Original Shipment Address : {deepOriginal.Destination.City}");
        Console.WriteLine($"Copied Shipment Address   : {deepCopied.Destination.City}");
        Console.WriteLine("Changing copied shipment address...");

        deepCopied.Destination.City = "Giza";

        Console.WriteLine($"Original Shipment Address : {deepOriginal.Destination.City}");
        Console.WriteLine($"Copied Shipment Address   : {deepCopied.Destination.City}");
        Console.WriteLine($"Same DeliveryAddress Object : {ReferenceEquals(deepOriginal.Destination, deepCopied.Destination)}");
        #endregion

        #region Question 08 - Extension Methods
        DeliveryUtilities.PrintSeparator();
        Console.WriteLine("Extension Methods");
        DeliveryUtilities.PrintSeparator();

        standard.UpdateTrackingStatus("In Transit");
        express.UpdateTrackingStatus("Out For Delivery");
        international.UpdateTrackingStatus("Delivered");

        Console.WriteLine(standard.GetSummary());
        Console.WriteLine(express.GetSummary());
        Console.WriteLine(international.GetSummary());

        Console.WriteLine($"SH001 Is Delivered : {standard.IsDelivered()}");
        Console.WriteLine($"SH003 Is Delivered : {international.IsDelivered()}");
        #endregion

        #region Question 09 & 10 - Tracking Status & Partial Method
        DeliveryUtilities.PrintSeparator();
        Console.WriteLine("Tracking Status");
        DeliveryUtilities.PrintSeparator();

        standard.UpdateTrackingStatus("Out For Delivery");
        #endregion

        #region Question 07 - Static Utilities Demonstration
        DeliveryUtilities.PrintSeparator();
        Console.WriteLine("Static Utilities");
        DeliveryUtilities.PrintSeparator();

        DeliveryUtilities.PrintSystemTitle("Delivery Center");
        Console.WriteLine($"Total Shipments Created : {Shipment.GetTotalShipmentsCreated()}");
        #endregion

        #region Partial Method Demonstration
        DeliveryUtilities.PrintSeparator();
        Console.WriteLine("Partial Method");
        DeliveryUtilities.PrintSeparator();

        standard.UpdateTrackingStatus("Delivered");
        #endregion

        DeliveryUtilities.PrintSeparator();
        Console.WriteLine("Assignment Completed");
        DeliveryUtilities.PrintSeparator();
    }
}