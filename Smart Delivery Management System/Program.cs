using System;
using System.Diagnostics.Contracts;

public class Program
{
    #region Part 01 - Theoretical Questions
    #region Theoretical Q1

    /*
    Q1: Abstraction
    a) What is Abstraction in Object-Oriented Programming?
    - Answer:
    Abstraction means hiding the complex internal details and showing only the essential features to the user. It focuses on what an object does rather than how it does it.

    b) Why is abstraction considered one of the four pillars of OOP?
    - Answer:
    Because it reduces code complexity and keeps different parts of the program independent (loose coupling). It allows you to change internal implementation details without breaking the code that depends on it.

    */

    #endregion

    public static void Main()
    {
        // 1. Create Driver and DeliveryCenter
        Driver driver = new Driver("Ahmed Mohamed");
        DeliveryCenter center = new DeliveryCenter("Delivery Center");
        center.CenterDriver = driver;

        // 2. Addresses
        DeliveryAddress addr1 = new DeliveryAddress("Cairo", "Tahrir St", 10);
        DeliveryAddress addr2 = new DeliveryAddress("Giza", "Haram St", 25);
        DeliveryAddress addr3 = new DeliveryAddress("Alexandria", "Corniche St", 5);

        // 3. Create Shipments
        StandardShipment standard = new StandardShipment("SH001", "Laptop", 3m, 80m, addr1);
        ExpressShipment express = new ExpressShipment("SH002", "Mobile Phone", 2m, 60m, addr2, 30m);
        InternationalShipment international = new InternationalShipment("SH003", "Television", 8m, 120m, addr3, "Germany", 100m);

        // 4. Add Shipments
        center.AddShipment(standard);
        center.AddShipment(express);
        center.AddShipment(international);

        // 5. Print All Shipments using center
        center.PrintAllShipments();

        // 6. Print using DeliveryHelper
        Console.WriteLine("\n==========================================");
        Console.WriteLine("Printing Using DeliveryHelper...\n");
        DeliveryHelper.PrintShipmentDetails(standard);
        Console.WriteLine();
        DeliveryHelper.PrintShipmentDetails(express);
        Console.WriteLine();
        DeliveryHelper.PrintShipmentDetails(international);

        // 7. Update Weight Demonstration
        Console.WriteLine("\n==========================================");
        Console.WriteLine("Updating Weight...\n");
        Console.WriteLine($"Original Weight : {standard.Weight} KG\n");

        standard.UpdateWeight(5m);
        Console.WriteLine($"Updated Weight : {standard.Weight} KG\n");

        standard.UpdateWeight(5m, 0.5m);
        Console.WriteLine($"Updated Weight After Packing : {standard.Weight} KG");

        // 8. Print using Shipment[] array
        Console.WriteLine("\n==========================================");
        Console.WriteLine("Printing Using Shipment[]...\n");
        Shipment[] shipmentArray = new Shipment[] { standard, express, international };

        for (int i = 0; i < shipmentArray.Length; i++)
        {
            if (shipmentArray[i] is StandardShipment)
                Console.WriteLine("Standard Shipment...\n");
            else if (shipmentArray[i] is ExpressShipment)
                Console.WriteLine("Express Shipment...\n");
            else if (shipmentArray[i] is InternationalShipment)
                Console.WriteLine("International Shipment...\n");
        }
        Console.WriteLine("==========================================");
    }

    #region DeliveryAddress Struct
    public struct DeliveryAddress
    {
        public string City;
        public string Street;
        public int BuildingNumber;

        public DeliveryAddress(string city, string street, int buildingNumber)
        {
            City = city;
            Street = street;
            BuildingNumber = buildingNumber;
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

    #region Shipment Base Class
    public class Shipment
    {
        private string trackingCode;
        private string description;
        private decimal weight;
        private decimal deliveryFee;

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

        public virtual decimal EstimatedCost
        {
            get { return DeliveryFee + (Weight * 5); }
        }

        public Shipment(string trackingCode, string description, decimal weight, decimal deliveryFee, DeliveryAddress destination)
        {
            this.trackingCode = string.IsNullOrWhiteSpace(trackingCode) ? "UNKNOWN" : trackingCode;
            this.description = !string.IsNullOrWhiteSpace(description) ? description : "Unknown";
            this.weight = weight > 0 ? weight : 1;
            this.deliveryFee = deliveryFee > 0 ? deliveryFee : 50;
            this.Destination = destination;
        }

        // Method Overloading: Version 1
        public void UpdateWeight(decimal newWeight)
        {
            if (newWeight > 0)
                Weight = newWeight;
        }

        // Method Overloading: Version 2
        public void UpdateWeight(decimal baseWeight, decimal extraPackingWeight)
        {
            if (baseWeight > 0 && extraPackingWeight >= 0)
                Weight = baseWeight + extraPackingWeight;
        }

        public virtual void PrintShipment()
        {
            Console.WriteLine($"Tracking Code : {TrackingCode}");
            Console.WriteLine($"Description   : {Description}");
            Console.WriteLine($"Weight        : {Weight} KG");
            Console.WriteLine($"Delivery Fee  : {DeliveryFee} EGP");
            Console.WriteLine($"Estimated Cost: {EstimatedCost} EGP");
        }
    }
    #endregion

    #region Derived Shipment Classes

    public class StandardShipment : Shipment
    {
        public StandardShipment(string trackingCode, string description, decimal weight, decimal deliveryFee, DeliveryAddress destination)
            : base(trackingCode, description, weight, deliveryFee, destination)
        {
        }

        public override void PrintShipment()
        {
            Console.WriteLine("Standard Shipment\n");
            base.PrintShipment();
        }
    }

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
            get { return DeliveryFee + (Weight * 5) + ExtraFee; }
        }

        public ExpressShipment(string trackingCode, string description, decimal weight, decimal deliveryFee, DeliveryAddress destination, decimal extraFee)
            : base(trackingCode, description, weight, deliveryFee, destination)
        {
            ExtraFee = extraFee;
        }

        public override void PrintShipment()
        {
            Console.WriteLine("Express Shipment\n");
            Console.WriteLine($"Tracking Code : {TrackingCode}");
            Console.WriteLine($"Description   : {Description}");
            Console.WriteLine($"Weight        : {Weight} KG");
            Console.WriteLine($"Delivery Fee  : {DeliveryFee} EGP");
            Console.WriteLine($"Extra Fee     : {ExtraFee} EGP");
            Console.WriteLine($"Estimated Cost: {EstimatedCost} EGP");
        }
    }

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
            get { return DeliveryFee + (Weight * 5) + CustomsFee; }
        }

        public InternationalShipment(string trackingCode, string description, decimal weight, decimal deliveryFee, DeliveryAddress destination, string destinationCountry, decimal customsFee)
            : base(trackingCode, description, weight, deliveryFee, destination)
        {
            DestinationCountry = destinationCountry;
            CustomsFee = customsFee;
        }

        public override void PrintShipment()
        {
            Console.WriteLine("International Shipment\n");
            Console.WriteLine($"Tracking Code        : {TrackingCode}");
            Console.WriteLine($"Description           : {Description}");
            Console.WriteLine($"Weight                : {Weight} KG");
            Console.WriteLine($"Delivery Fee          : {DeliveryFee} EGP");
            Console.WriteLine($"Destination Country   : {DestinationCountry}");
            Console.WriteLine($"Customs Fee           : {CustomsFee} EGP");
            Console.WriteLine($"Estimated Cost        : {EstimatedCost} EGP");
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
                if (shipments != null && index >= 0 && index < count)
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
                Console.WriteLine("------------------------------------------\n");
                shipments[i].PrintShipment();
                Console.WriteLine();
            }
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
                if (shipment is StandardShipment)
                    Console.WriteLine("Standard Shipment Printed Successfully.");
                else if (shipment is ExpressShipment)
                    Console.WriteLine("Express Shipment Printed Successfully.");
                else if (shipment is InternationalShipment)
                    Console.WriteLine("International Shipment Printed Successfully.");
            }
        }
    }
    #endregion
}
