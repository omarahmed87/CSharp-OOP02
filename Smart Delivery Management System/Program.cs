using System;
#region Part 01 - Theoretical Questions

/*
Q1: Object Copying
a) What happens when you assign one object variable to another object variable?
   - When assigning one reference-type variable to another, only the memory address (reference) 
     pointing to the object on the heap is copied. No new object is created on the heap.

b) Does assigning one object to another create a new object? Explain.
   - No, it does not create a new object. Both reference variables now point to the exact same memory location on the heap. Any changes made through one variable will reflect in the other.

c) What is the difference between copying an object and copying its reference?
   - Copying a reference means copying the pointer/address so both point to the same instance. 
   - Copying an object creates a separate instance in memory with its own values or properties.

Q2: Shallow Copy vs Deep Copy
a) What is a Shallow Copy?
   - A shallow copy duplicates the top-level object. Value-type fields are copied bit-by-bit, but reference-type fields only have their references copied, pointing to the same referenced objects.

b) What is a Deep Copy?
   - A deep copy duplicates the object and recursively duplicates all objects referenced by it, creating completely independent copies.

c) What happens to reference-type members when a Shallow Copy is created?
   - The reference is copied, so both original and copied objects point to and share the exact same referenced object in heap memory.

d) What happens to reference-type members when a Deep Copy is created?
   - A new instance of the referenced object is created, so changes in the referenced object do not affect the original.

e) Give one situation where Deep Copy would be safer than Shallow Copy.
   - When an object contains sensitive or mutable child objects (like a user profile with an address or shopping cart items) where modifications to the cloned data should never corrupt the original object.

Q3: Static Members
a) What is a static field, and how is it different from an instance field?
   - A static field belongs to the class itself and has a single shared copy across all instances, whereas an instance field belongs to each individual object.

b) What is a static method? Can a static method directly access instance members?
   - A static method belongs to the class and is called using the class name. It cannot directly access instance members because it doesn't have an instance (`this`) context.

c) What is a static constructor, and when is it executed?
   - A static constructor initializes static data or runs one-time setup for the class. It is executed automatically before the first instance is created or any static member is referenced.

d) What is a static class? Can you create an object from a static class?
   - A static class is a container for static members only. You cannot instantiate an object from a static class using `new`, and it cannot be inherited.

Q4: Extension Methods
a) What is an Extension Method?
   - An extension method allows developers to add new methods to existing types without modifying the original source code, inheriting, or recompiling it.

b) What keyword must be used in the first parameter of an extension method?
   - The `this` keyword.

c) Where must an extension method be declared?
   - Inside a non-generic, static class as a static method.

d) Can an extension method access private members of the class it extends?
   - No, it cannot access private or protected members; it can only access public and internal accessible members like any external method.

Q5: Partial Classes and Partial Methods
a) What is a Partial Class?
   - A partial class allows splitting a single class definition across multiple `.cs` files, which the compiler later merges into a single class.

b) Why would a developer split one class into multiple files?
   - To organize large classes by feature/responsibility, or to separate auto-generated code (like designer files) from custom business logic.

c) What is a Partial Method?
   - A method declared in one part of a partial class and optionally implemented in another part. It must return `void` and is implicitly private.

d) What happens if a declared partial method has no implementation?
   - The compiler removes the method signature and all calls to it at compile time, causing zero runtime overhead.
*/

#endregion

public class Program
{
    public static void Main()
    {
        // 1. Create driver and delivery center
        Driver driver = new Driver("Ahmed Mohamed");
        DeliveryCenter center = new DeliveryCenter("Main Logistics Hub");
        center.CenterDriver = driver; 

        // 2. Create addresses
        DeliveryAddress addr1 = new DeliveryAddress("Cairo", "Tahrir St", 10);
        DeliveryAddress addr2 = new DeliveryAddress("Giza", "Haram St", 25);
        DeliveryAddress addr3 = new DeliveryAddress("Alexandria", "Corniche St", 5);

        // 3. Create shipments
        StandardShipment standard = new StandardShipment("SH001", "Laptop", 3m, 80m, addr1);
        ExpressShipment express = new ExpressShipment("SH002", "Mobile Phone", 2m, 60m, addr2, 30m);
        InternationalShipment international = new InternationalShipment("SH003", "Television", 8m, 120m, addr3, "Germany", 100m);

        // 4. Add shipments to center
        center.AddShipment(standard);
        center.AddShipment(express);
        center.AddShipment(international);

        // 5. Print all shipments from center
        center.PrintAllShipments();

        // 6. Test DeliveryHelper
        Console.WriteLine("\n==========================================");
        Console.WriteLine("Printing Using DeliveryHelper...\n");
        DeliveryHelper.PrintShipmentDetails(standard);
        Console.WriteLine();
        DeliveryHelper.PrintShipmentDetails(express);
        Console.WriteLine();
        DeliveryHelper.PrintShipmentDetails(international);

        // 7. Test weight updates
        Console.WriteLine("\n==========================================");
        Console.WriteLine("Updating Weight...\n");
        Console.WriteLine($"Original Weight : {standard.Weight} KG\n");

        standard.UpdateWeight(5m);
        Console.WriteLine($"Updated Weight : {standard.Weight} KG\n");

        standard.UpdateWeight(5m, 0.5m);
        Console.WriteLine($"Updated Weight After Packing : {standard.Weight} KG");

        // 8. Loop through array using polymorphism
        Console.WriteLine("\n==========================================");
        Console.WriteLine("Printing Using Shipment[] Array...\n");
        Shipment[] shipmentArray = new Shipment[] { standard, express, international };

        for (int i = 0; i < shipmentArray.Length; i++)
        {
            shipmentArray[i].PrintShipment();
            Console.WriteLine("------------------------------------------");
        }
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
    public abstract class Shipment
    {
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

        // Each child class calculates its own cost
        public abstract decimal EstimatedCost { get; }

        public Shipment(string trackingCode, string description, decimal weight, decimal deliveryFee, DeliveryAddress destination)
        {
            this.trackingCode = string.IsNullOrWhiteSpace(trackingCode) ? "UNKNOWN" : trackingCode;
            this.description = !string.IsNullOrWhiteSpace(description) ? description : "Unknown";
            this.weight = weight > 0 ? weight : 1;
            this.deliveryFee = deliveryFee > 0 ? deliveryFee : 50;
            this.Destination = destination;
        }

        // Overload 1: simple weight update
        public void UpdateWeight(decimal newWeight)
        {
            if (newWeight > 0)
                Weight = newWeight;
        }

        // Overload 2: update with extra packing
        public void UpdateWeight(decimal baseWeight, decimal extraPackingWeight)
        {
            if (baseWeight > 0 && extraPackingWeight >= 0)
                Weight = baseWeight + extraPackingWeight;
        }

        // Print common shipment data
        public virtual void PrintShipment()
        {
            Console.WriteLine($"Tracking Code : {TrackingCode}");
            Console.WriteLine($"Description   : {Description}");
            Console.WriteLine($"Destination   : {Destination.GetFullAddress()}");
            Console.WriteLine($"Weight        : {Weight} KG");
            Console.WriteLine($"Delivery Fee  : {DeliveryFee} EGP");
            Console.WriteLine($"Estimated Cost: {EstimatedCost} EGP");
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

        public override void PrintShipment()
        {
            Console.WriteLine("Express Shipment:");
            base.PrintShipment();
            Console.WriteLine($"Extra Fee     : {ExtraFee} EGP");
        }
    }

    public class InternationalShipment : Shipment
    {
        private string destinationCountry;
        private decimal customsFee;

        public string DestinationCountry
        {
            get => destinationCountry;
            set
            {
                if (!string.IsNullOrWhiteSpace(value))
                    destinationCountry = value;
            }
        }

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

        // Indexer by index position
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
                // Do not allow null values to replace items
                if (shipments != null && index >= 0 && index < count && value != null)
                    shipments[index] = value;
            }
        }

        // Indexer by tracking code
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

            // Shift items left
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

    #region DeliveryHelper Static Class
    public static class DeliveryHelper
    {
        // Call PrintShipment directly using polymorphism
        public static void PrintShipmentDetails(Shipment shipment)
        {
            if (shipment != null)
            {
                shipment.PrintShipment();
            }
        }
    }
    #endregion
}
