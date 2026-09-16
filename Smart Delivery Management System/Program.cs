#region Q1 from assignment Basics05
/*Declare an enum Genre { Fiction, NonFiction, Science }. Add a Genre property to Book, assign it Genre.
  Science, and print it.*/
using System;

public enum Genre
{
    Fiction,
    NonFiction,
    Science
}

public class Book
{
    public Genre BookGenre { get; set; }
}

public class Program
{
    public static void Main()
    {
        Book myBook = new Book();
        myBook.BookGenre = Genre.Science;
        Console.WriteLine(myBook.BookGenre);
    }
}
#endregion