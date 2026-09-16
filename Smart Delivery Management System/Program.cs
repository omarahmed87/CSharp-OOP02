

#region practical Q1
/*
 * a)  What happens when a DeliveryAddress variable is copied into another variable 
  and the copy is modified? 
b)  What happens when a Customer variable is copied into another variable and one variable modifies 
the object
public struct DeliveryAddress
{
public string City;
public string Street;
}
public class Customer
{
public string Name;
}*/
#endregion
/*response
a) DeliveryAddress is a struct (Value Type). When it is copied, a completely independent,
new copy of the data is allocated on the Stack.
Result: Modifying the copy will NOT affect the original variable.
--------------------------------------------------------------
b) Customer is a class (Reference Type): When it is copied, only the memory reference is copied, while the actual object 
remains in the Heap. Both variables now point to the exact same object in memory.
Result: Modifying the object through either variable WILL affect both variables.
*/
