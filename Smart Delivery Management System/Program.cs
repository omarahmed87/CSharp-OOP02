#region Q1 from assignment Basics05
///*Declare an enum Genre { Fiction, NonFiction, Science }. Add a Genre property to Book, assign it Genre.
//  Science, and print it.*/
//using System;

//public enum Genre
//{
//    Fiction,
//    NonFiction,
//    Science
//}

//public class Book
//{
//    public Genre BookGenre { get; set; }
//}

//public class Program
//{
//    public static void Main()
//    {
//        Book myBook = new Book();
//        myBook.BookGenre = Genre.Science;
//        Console.WriteLine(myBook.BookGenre);
//    }
//}
#endregion
#region Q2 from assignment Basics05
/*
 Using the Genre enum above, print the underlying int value of Genre.Fiction, Genre.NonFiction, 
and Genre.Science by casting each to int.
 */

//using System;

//public enum Genre
//{
//    Fiction,
//    NonFiction,
//    Science
//}

//public class Program
//{
//    public static void Main()
//    {
//        int fictionVal = (int)Genre.Fiction;
//        int nonFictionVal = (int)Genre.NonFiction;
//        int scienceVal = (int)Genre.Science;

//        Console.WriteLine(fictionVal);
//        Console.WriteLine(nonFictionVal);
//        Console.WriteLine(scienceVal);
//    }
//}
#endregion
#region Q3 from assignment Basics05
/*
 Given int genreNumber = 1;, cast it into a Genre value and print the result.
 */
using System;

public enum Genre
{
    Fiction,
    NonFiction,
    Science
}

public class Program
{
    public static void Main()
    {
        int genreNumber = 1;
        Genre genre = (Genre)genreNumber;
        Console.WriteLine(genre);
    }
}
#endregion