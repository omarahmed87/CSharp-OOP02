

#region practical Q2
/*
a) a) Identify at least three problems with this design from an encapsulation perspective. 
b) b) How can private fields and public properties improve this design? 

 public struct Shipment
{
public string description;
public double weight;
public decimal DeliveryFee;
}
}*/
#endregion
/* response
 a) Three problems with this design:
   1.No Data Hiding: Fields are public, meaning anyone can change them directly without control.
   2.No Validation: There is no check to prevent bad data (like negative weight or negative delivery fee).
   3.Mutable Struct: Leaving struct fields public makes it mutable, which can lead to bugs when copied.

 b)How private fields and public properties improve it:
   1.Data Validation: Properties let us use logic (get/set) to reject invalid values (e.g., weight <= 0).
   2.Read-Only Protection: We can make properties read-only ({ get; }) so values can't be changed by mistake after creation.
   3.Control: Keeps the internal data safe and follows OOP encapsulation rules.
 
 */
