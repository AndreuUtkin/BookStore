//Файл для бизнес процессов
//**************
//***********
//*******
//*****
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using System.Windows.Forms;
using System.Data.SqlClient;
using static System.Net.Mime.MediaTypeNames;
using System.Threading.Tasks;
using static System.Reflection.Metadata.BlobBuilder;
using System.Net;
public class Visit
{
    public Book_short book { get; set; }
    public DateTime date { get; set; }
    public Visit(Book_short book, DateTime date)
    {
        this.book = book; this.date=date;
    }

}
public class User
{
    public int id { get; set; }
    public string name { get; set; }
    public List<Visit> visits { get; set; }
    public User(int id, string name, List<Visit> visit)
    {
        this.id = id;
       this.name = name; this.visits = visit;   
    }
}
public class Tag
{
    public int id { get; set; }
    public string name { get; set; }
    public Tag(int id, string name)
    {
        this.id = id;
        this.name = name;
    }

}
/// <summary>
/// Книга
/// </summary>
public class Book
{
    /// <summary>
    /// Книга
    /// </summary>
   [System.ComponentModel.DataAnnotations.Required]
    public int id { get; set; }
    /// <summary>
    /// Книга
    /// </summary>
    [System.ComponentModel.DataAnnotations.Required]
    public string name { get; set; }
    /// <summary>
    /// Книга
    /// </summary>
    [System.ComponentModel.DataAnnotations.Required]
    public List<Author_short> Authors { get; set; }
    /// <summary>
    /// Книга
    /// </summary>
    [System.ComponentModel.DataAnnotations.Required]
    public List<Tag> Tags { get; set; }
    /// <summary>
    /// Книга
    /// </summary>
    [System.ComponentModel.DataAnnotations.Required]
    public string description { get; set; }
    /// <summary>
    /// Книга
    /// </summary>
    public string Location { get; set; }
    public Book(int id, string name, List<Author_short> authors, List<Tag> tags, string description,string location)
    {
        this.id = id;
        this.name = name;
        Authors = authors;
        Tags = tags;
        this.description = description;
        this.Location = location;
    }
}
public class Book_short
{
    public int id { get; set; }
    public string name { get; set; }

    public Book_short(int id, string name)
    {
        this.id = id;
        this.name = name;

    }
}
public class Author
{
    public int id { get; set; }
    public string name { get; set; }
    public List<Book_short> Books { get; set; }
    public string description { get; set; }
    public Author(int id, string name, List<Book_short> Books, string description)
    {
        this.id = id;
        this.name = name;
        this.Books = Books;
        this.description = description;
    }
}
public class Author_short
{
    public int id { get; set; }
    public string name { get; set; }
    public Author_short(int id, string name)
    {
        this.id = id;
        this.name = name;
    }
}
public class Business
{
    private string connectionString = @"Data Source=.\ANDREU;Initial Catalog=Books;Integrated Security=false;User Id=AUE; Password =777;";
    public User? checkUsr(string name, string pwd)
    {
        DataSet ds_DataSet_Edit = new DataSet();// создаем объект DataSet
        string commandString = "SELECT id FROM Users Where name='"+name+"' AND password='"+pwd+"';";// создаем запрос
        SqlDataAdapter Adapter_EDIT = new SqlDataAdapter(commandString, connectionString);// создаем SqlDataAdapter adapter
        Adapter_EDIT.Fill(ds_DataSet_Edit); // заполнение DataSet данными с помощью DataAdapter
        int n = ds_DataSet_Edit.Tables[0].Rows.Count;
        if(n==0)
            return null;
        int id=ds_DataSet_Edit.Tables[0].Rows[0].Field<int>("id");
        List<Visit> visits = History(id);
        return new User(id, name, visits);
    }
    public List<Visit> History(int UsrId)
    {
        
        int id = UsrId;
        int n = countRows("History", "UserId", id.ToString());
        string commandString = "SELECT BookID, Date FROM History WHERE UserId = '" + id.ToString() + "';";// создаем запрос
        List<int> BookId = new List<int>();
        List<DateTime> Dates = new List<DateTime>();
        DataSet ds_DataSet_Edit = new DataSet();
        SqlDataAdapter Adapter_EDIT = new SqlDataAdapter(commandString, connectionString);// создаем SqlDataAdapter adapter
        Adapter_EDIT.Fill(ds_DataSet_Edit); // заполнение DataSet данными с помощью DataAdapter
        for (int i = 0; i < n; i++)
        {
            BookId.Add(ds_DataSet_Edit.Tables[0].Rows[i].Field<int>("BookID"));
            Dates.Add((ds_DataSet_Edit.Tables[0].Rows[i].Field<DateTime>("Date")));
        }
        //ds_DataSet_Edit = new DataSet();
        List<Book_short> books = new List<Book_short>();
        for (int i = 0; i < n; i++)
        {
            ds_DataSet_Edit = new DataSet();
            commandString = "SELECT Name FROM Books WHERE id = '" + BookId[i] + "';";// создаем запрос
            Adapter_EDIT = new SqlDataAdapter(commandString, connectionString);// создаем SqlDataAdapter adapter
            Adapter_EDIT.Fill(ds_DataSet_Edit); // заполнение DataSet данными с помощью DataAdapter
            books.Add(new Book_short(BookId[i], ds_DataSet_Edit.Tables[0].Rows[0].Field<string>("Name")));
        }
        List<Visit> vis=new List<Visit>();
        for (int i = 0; i < n; i++)
        {
            vis.Add(new Visit(books[i], Dates[i]));
        }
        return vis;
    }
    public int countRows(string table, string row, string token)
    {
        DataSet ds_DataSet_Edit = new DataSet();// создаем объект DataSet
        //string connectionString = @"Data Source=.\ANDREU;Initial Catalog=Books;Integrated Security=false;User Id=AUE; Password =777;";
        string commandString = "SELECT 1 FROM " + table + " WHERE " + row + " = '" + token + "';";// создаем запрос
        SqlDataAdapter Adapter_EDIT = new SqlDataAdapter(commandString, connectionString);// создаем SqlDataAdapter adapter

        Adapter_EDIT.Fill(ds_DataSet_Edit); // заполнение DataSet данными с помощью DataAdapter
                                            //int rows_ = 0;
        int rows_ = ds_DataSet_Edit.Tables[0].Rows.Count;
        return rows_;
    }
    public List<Author_short> AllAuthors()
    {
        DataSet ds_DataSet_Edit = new DataSet();// создаем объект DataSet
        string commandString = "SELECT id FROM Authors;";// создаем запрос
        SqlDataAdapter Adapter_EDIT = new SqlDataAdapter(commandString, connectionString);// создаем SqlDataAdapter adapter
        Adapter_EDIT.Fill(ds_DataSet_Edit); // заполнение DataSet данными с помощью DataAdapter
        int n = ds_DataSet_Edit.Tables[0].Rows.Count;
        List<int> books = new List<int>();
        for (int i = 0; i < n; i++)
        {
            books.Add(ds_DataSet_Edit.Tables[0].Rows[i].Field<int>("id"));
        }
        List<Author_short> authors = new List<Author_short>();
        foreach (int book in books)
        {
            authors.Add(Author_short(book));
        }
        return authors;
    }
    public List<Tag> Tags()
    {
        DataSet ds_DataSet_Edit = new DataSet();// создаем объект DataSet
        string commandString = "SELECT id FROM Tags;";// создаем запрос
        SqlDataAdapter Adapter_EDIT = new SqlDataAdapter(commandString, connectionString);// создаем SqlDataAdapter adapter
        Adapter_EDIT.Fill(ds_DataSet_Edit); // заполнение DataSet данными с помощью DataAdapter
        int n = ds_DataSet_Edit.Tables[0].Rows.Count;
        List<int> books = new List<int>();
        for (int i = 0; i < n; i++)
        {
            books.Add(ds_DataSet_Edit.Tables[0].Rows[i].Field<int>("id"));
        }
        List<Tag> authors = new List<Tag>();
        foreach (int book in books)
        {
            authors.Add(Tag(book));
        }
        return authors;
    }
    public Author_short Author_short(int id)
    {
        DataSet ds_DataSet_Edit = new DataSet();// создаем объект DataSet
                                                //string connectionString = @"Data Source=.\ANDREU;Initial Catalog=Books;Integrated Security=false;User Id=AUE; Password =777;";
        string commandString = "SELECT Name FROM Authors WHERE id = '" + id + "';";// создаем запрос
        SqlDataAdapter Adapter_EDIT = new SqlDataAdapter(commandString, connectionString);// создаем SqlDataAdapter adapter
        Adapter_EDIT.Fill(ds_DataSet_Edit); // заполнение DataSet данными с помощью DataAdapter
        string name = ds_DataSet_Edit.Tables[0].Rows[0].Field<string>("Name");
        return new Author_short(id, name);
    }
    public Book_short Book_short(int id)
    {
        DataSet ds_DataSet_Edit = new DataSet();// создаем объект DataSet
                                                //string connectionString = @"Data Source=.\ANDREU;Initial Catalog=Books;Integrated Security=false;User Id=AUE; Password =777;";
        string commandString = "SELECT Name FROM Books WHERE id = '" + id + "';";// создаем запрос
        SqlDataAdapter Adapter_EDIT = new SqlDataAdapter(commandString, connectionString);// создаем SqlDataAdapter adapter
        Adapter_EDIT.Fill(ds_DataSet_Edit); // заполнение DataSet данными с помощью DataAdapter
        string name = ds_DataSet_Edit.Tables[0].Rows[0].Field<string>("Name");
        return new Book_short(id, name);
    }
    public Tag Tag(int id)
    {
        DataSet ds_DataSet_Edit = new DataSet();// создаем объект DataSet
                                                //string connectionString = @"Data Source=.\ANDREU;Initial Catalog=Books;Integrated Security=false;User Id=AUE; Password =777;";
        string commandString = "SELECT Name FROM Tags WHERE id = '" + id + "';";// создаем запрос
        SqlDataAdapter Adapter_EDIT = new SqlDataAdapter(commandString, connectionString);// создаем SqlDataAdapter adapter
        Adapter_EDIT.Fill(ds_DataSet_Edit); // заполнение DataSet данными с помощью DataAdapter
        string name = ds_DataSet_Edit.Tables[0].Rows[0].Field<string>("Name");
        return new Tag(id, name);
    }
    public Author AuthorInf(int id)
    {
        DataSet ds_DataSet_Edit = new DataSet();// создаем объект DataSet
                                                //string connectionString = @"Data Source=.\ANDREU;Initial Catalog=Books;Integrated Security=false;User Id=AUE; Password =777;";
        string commandString = "SELECT Name FROM Authors WHERE id = '" + id + "';";// создаем запрос
        SqlDataAdapter Adapter_EDIT = new SqlDataAdapter(commandString, connectionString);// создаем SqlDataAdapter adapter
        Adapter_EDIT.Fill(ds_DataSet_Edit); // заполнение DataSet данными с помощью DataAdapter
        string name = ds_DataSet_Edit.Tables[0].Rows[0].Field<string>("Name");
        ds_DataSet_Edit = new DataSet();
        commandString = "SELECT Description FROM Authors WHERE id = '" + id + "';";// создаем запрос
        Adapter_EDIT = new SqlDataAdapter(commandString, connectionString);// создаем SqlDataAdapter adapter
        Adapter_EDIT.Fill(ds_DataSet_Edit); // заполнение DataSet данными с помощью DataAdapter
        string descr = ds_DataSet_Edit.Tables[0].Rows[0].Field<string>("Description");
        List<int> books = BooksOf(id);
        List<Book_short> books_Shorts = new List<Book_short>();
        foreach (int book in books)
        {
            books_Shorts.Add(Book_short(book));
        }

        return new Author(id, name, books_Shorts, descr);

    }
    public Book BookInf(int id)
    {
        DataSet ds_DataSet_Edit = new DataSet();// создаем объект DataSet
                                                //string connectionString = @"Data Source=.\ANDREU;Initial Catalog=Books;Integrated Security=false;User Id=AUE; Password =777;";
        string commandString = "SELECT Name FROM Books WHERE id = '" + id + "';";// создаем запрос
        SqlDataAdapter Adapter_EDIT = new SqlDataAdapter(commandString, connectionString);// создаем SqlDataAdapter adapter
        Adapter_EDIT.Fill(ds_DataSet_Edit); // заполнение DataSet данными с помощью DataAdapter
        string name = ds_DataSet_Edit.Tables[0].Rows[0].Field<string>("Name");
        ds_DataSet_Edit = new DataSet();
        commandString = "SELECT Description FROM Books WHERE id = '" + id + "';";// создаем запрос
        Adapter_EDIT = new SqlDataAdapter(commandString, connectionString);// создаем SqlDataAdapter adapter
        Adapter_EDIT.Fill(ds_DataSet_Edit); // заполнение DataSet данными с помощью DataAdapter
        string descr = ds_DataSet_Edit.Tables[0].Rows[0].Field<string>("Description");
        ds_DataSet_Edit = new DataSet();
        commandString = "SELECT Location FROM Books WHERE id = '" + id + "';";// создаем запрос
        Adapter_EDIT = new SqlDataAdapter(commandString, connectionString);// создаем SqlDataAdapter adapter
        Adapter_EDIT.Fill(ds_DataSet_Edit); // заполнение DataSet данными с помощью DataAdapter
        string loc = ds_DataSet_Edit.Tables[0].Rows[0].Field<string>("Location");
        List<int> auth = GetAuthors(id);
        List<Author_short> books_Shorts = new List<Author_short>();
        foreach (int book in auth)
        {
            books_Shorts.Add(Author_short(book));
        }
        List<int> tags = GetTags(id);
        List<Tag> tag = new List<Tag>();
        foreach (int book in tags)
        {
            tag.Add(Tag(book));
        }
        return new Book(id, name, books_Shorts, tag, descr,loc);
    }
    public List<int> AllTags()
    {
        DataSet ds_DataSet_Edit = new DataSet();// создаем объект DataSet
        string commandString = "SELECT id FROM Tags;";// создаем запрос
        SqlDataAdapter Adapter_EDIT = new SqlDataAdapter(commandString, connectionString);// создаем SqlDataAdapter adapter
        Adapter_EDIT.Fill(ds_DataSet_Edit); // заполнение DataSet данными с помощью DataAdapter
        int n = ds_DataSet_Edit.Tables[0].Rows.Count;
        List<int> books = new List<int>();
        for (int i = 0; i < n; i++)
        {
            books.Add(ds_DataSet_Edit.Tables[0].Rows[i].Field<int>("id"));
        }
        return books;
    }
    public List<Book> Catalog()
    {
        List<int> ints = new List<int>();
        ints = AllBooks();
        List<Book> books = new List<Book>();
        foreach (int book in ints)
        {
            books.Add(BookInf(book));
        }
        return books;
    }
    public List<int> AllBooks()
    {
        DataSet ds_DataSet_Edit = new DataSet();// создаем объект DataSet
        string commandString = "SELECT id FROM Books;";// создаем запрос
        SqlDataAdapter Adapter_EDIT = new SqlDataAdapter(commandString, connectionString);// создаем SqlDataAdapter adapter
        Adapter_EDIT.Fill(ds_DataSet_Edit); // заполнение DataSet данными с помощью DataAdapter
        int n = ds_DataSet_Edit.Tables[0].Rows.Count;
        List<int> books = new List<int>();
        for (int i = 0; i < n; i++)
        {
            books.Add(ds_DataSet_Edit.Tables[0].Rows[i].Field<int>("id"));
        }
        return books;
    }
    public List<int> GetTags(int id)
    {
        int n = countRows("BookTag", "BookID", id.ToString());
        string commandString = "SELECT TagID FROM BookTag WHERE BookID = '" + id.ToString() + "';";// создаем запрос
        List<int> authorID = new List<int>();
        DataSet ds_DataSet_Edit = new DataSet();
        SqlDataAdapter Adapter_EDIT = new SqlDataAdapter(commandString, connectionString);// создаем SqlDataAdapter adapter
        Adapter_EDIT.Fill(ds_DataSet_Edit); // заполнение DataSet данными с помощью DataAdapter
        for (int i = 0; i < n; i++)
        {
            authorID.Add(ds_DataSet_Edit.Tables[0].Rows[i].Field<int>("TagId"));
        }

        return authorID;
    }
    public List<int> GetAuthors(int id)
    {

        int n = countRows("BookAuthor", "BookID", id.ToString());
        string commandString = "SELECT AuthorID FROM BookAuthor WHERE BookID = '" + id.ToString() + "';";// создаем запрос
        List<int> authorID = new List<int>();
        DataSet ds_DataSet_Edit = new DataSet();
        SqlDataAdapter Adapter_EDIT = new SqlDataAdapter(commandString, connectionString);// создаем SqlDataAdapter adapter
        Adapter_EDIT.Fill(ds_DataSet_Edit); // заполнение DataSet данными с помощью DataAdapter
        for (int i = 0; i < n; i++)
        {
            authorID.Add(ds_DataSet_Edit.Tables[0].Rows[i].Field<int>("AuthorId"));
        }

        return authorID;
    }
    public List<int> BooksOf(int id)
    {

        int n = countRows("BookAuthor", "AuthorId", id.ToString());
        string commandString = "SELECT BookID FROM BookAuthor WHERE AuthorId = '" + id.ToString() + "';";// создаем запрос
        List<int> authorID = new List<int>();
        DataSet ds_DataSet_Edit = new DataSet();
        SqlDataAdapter Adapter_EDIT = new SqlDataAdapter(commandString, connectionString);// создаем SqlDataAdapter adapter
        Adapter_EDIT.Fill(ds_DataSet_Edit); // заполнение DataSet данными с помощью DataAdapter
        for (int i = 0; i < n; i++)
        {
            authorID.Add(ds_DataSet_Edit.Tables[0].Rows[i].Field<int>("BookID"));
        }
        return authorID;
    }
    public string GetPwd(string Name)
    {
        DataSet ds_DataSet_Edit = new DataSet();// создаем объект DataSet
        string commandString = "SELECT Password FROM Users WHERE Name = '" + Name + "';";// создаем запрос
        SqlDataAdapter Adapter_EDIT = new SqlDataAdapter(commandString, connectionString);// создаем SqlDataAdapter adapter
        Adapter_EDIT.Fill(ds_DataSet_Edit); // заполнение DataSet данными с помощью DataAdapter
        string pwd = ds_DataSet_Edit.Tables[0].Rows[0].Field<string>("Password");

        return pwd;
    }
    public async Task BookUser(int UserId, int BookId)
    {

        DataSet ds_DataSet_Edit = new DataSet();// создаем объект DataSet
        string sqlExpression = "INSERT INTO History (BookID, UserID, Date) VALUES (" + BookId + ", " + UserId + ", '" + curDateTime() + "')";

        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();
            SqlCommand command = new SqlCommand(sqlExpression, connection);
            int number = command.ExecuteNonQuery();
            //Console.WriteLine("Добавлено объектов: {0}", number);
        }
        //Console.Read();
    }
    bool check(string table, string row, string token)
    {
        DataSet ds_DataSet_Edit = new DataSet();// создаем объект DataSet
        string connectionString = @"Data Source=.\ANDREU;Initial Catalog=Books;Integrated Security=false;User Id=AUE; Password =777;";
        string commandString = "SELECT " + row + " FROM " + table + " WHERE " + row + " = '" + token + "';";// создаем запрос
        SqlDataAdapter Adapter_EDIT = new SqlDataAdapter(commandString, connectionString);// создаем SqlDataAdapter adapter

        Adapter_EDIT.Fill(ds_DataSet_Edit); // заполнение DataSet данными с помощью DataAdapter
                                            //int rows_ = 0;
        int rows_ = ds_DataSet_Edit.Tables[0].Rows.Count;
        if (rows_ == 0)
        { return false; }
        else { return true; };
    }
    bool check2(string table, string row1, string token, string row2, string token2)
    {
        DataSet ds_DataSet_Edit = new DataSet();// создаем объект DataSet
        string connectionString = @"Data Source=.\ANDREU;Initial Catalog=Books;Integrated Security=false;User Id=AUE; Password =777;";
        string commandString = "SELECT " + row1 + " FROM " + table + " WHERE " + row1 + " = '" + token + "' AND " +row2 + " = '" + token2+ "';";// создаем запрос
        SqlDataAdapter Adapter_EDIT = new SqlDataAdapter(commandString, connectionString);// создаем SqlDataAdapter adapter

        Adapter_EDIT.Fill(ds_DataSet_Edit); // заполнение DataSet данными с помощью DataAdapter
                                            //int rows_ = 0;
        int rows_ = ds_DataSet_Edit.Tables[0].Rows.Count;
        if (rows_ == 0)
        { return false; }
        else { return true; };
    }

    public async Task AddBook(string Name, string Description)
    {
        DataSet ds_DataSet_Edit = new DataSet();// создаем объект DataSet
        string commandString = "SELECT id FROM Books;";// создаем запрос
        SqlDataAdapter Adapter_EDIT = new SqlDataAdapter(commandString, connectionString);// создаем SqlDataAdapter adapter
        Adapter_EDIT.Fill(ds_DataSet_Edit); // заполнение DataSet данными с помощью DataAdapter
        List<int> ids = new List<int>();
        int n = ds_DataSet_Edit.Tables[0].Rows.Count;
        for (int i = 0; i < n; i++)
        {
            ids.Add(ds_DataSet_Edit.Tables[0].Rows[i].Field<int>("id"));
        }
        int newId;
        bool flag = false;
        int lastid = ids[0];
        for (int i = 1; i < n; i++)
        {
            if (ids[i] > lastid + 1)
            {
                flag = true;
                newId = lastid;
                break;
            }
            lastid = ids[i];
        }
        if (!flag) { newId = n; }


        string sqlExpression = "INSERT INTO Books (id, Name, Description, Location) VALUES (" + (lastid + 1).ToString() + ", '" + Name + "', '" + Description + "',NULL)";

        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();
            SqlCommand command = new SqlCommand(sqlExpression, connection);
            int number = command.ExecuteNonQuery();
            //Console.WriteLine("Добавлено объектов: {0}", number);
        }
        // Console.Read();
    }
    public async Task AddTag(string Name)
    {
        DataSet ds_DataSet_Edit = new DataSet();// создаем объект DataSet
        string commandString = "SELECT id FROM Tags;";// создаем запрос
        SqlDataAdapter Adapter_EDIT = new SqlDataAdapter(commandString, connectionString);// создаем SqlDataAdapter adapter
        Adapter_EDIT.Fill(ds_DataSet_Edit); // заполнение DataSet данными с помощью DataAdapter
        List<int> ids = new List<int>();
        int n = ds_DataSet_Edit.Tables[0].Rows.Count;
        for (int i = 0; i < n; i++)
        {
            ids.Add(ds_DataSet_Edit.Tables[0].Rows[i].Field<int>("id"));
        }
        int newId;
        bool flag = false;
        int lastid = ids[0];
        for (int i = 1; i < n; i++)
        {
            if (ids[i] > lastid + 1)
            {
                flag = true;
                newId = lastid;
                break;
            }
            lastid = ids[i];
        }
        if (!flag) { newId = n; }


        string sqlExpression = "INSERT INTO Tags (id, Name) VALUES (" + (lastid + 1).ToString() + ", '" + Name + "')";

        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();
            SqlCommand command = new SqlCommand(sqlExpression, connection);
            int number = command.ExecuteNonQuery();
            //Console.WriteLine("Добавлено объектов: {0}", number);
        }
        // Console.Read();
    }
    public async Task AddAuthor(string Name, string Description)
    {
        DataSet ds_DataSet_Edit = new DataSet();// создаем объект DataSet
        string commandString = "SELECT id FROM Authors;";// создаем запрос
        SqlDataAdapter Adapter_EDIT = new SqlDataAdapter(commandString, connectionString);// создаем SqlDataAdapter adapter
        Adapter_EDIT.Fill(ds_DataSet_Edit); // заполнение DataSet данными с помощью DataAdapter
        List<int> ids = new List<int>();
        int n = ds_DataSet_Edit.Tables[0].Rows.Count;
        for (int i = 0; i < n; i++)
        {
            ids.Add(ds_DataSet_Edit.Tables[0].Rows[i].Field<int>("id"));
        }
        int newId;
        bool flag = false;
        int lastid = ids[0];
        for (int i = 1; i < n; i++)
        {
            if (ids[i] > lastid + 1)
            {
                flag = true;
                newId = lastid;
                break;
            }
            lastid = ids[i];
        }
        if (!flag) { newId = n; }


        string sqlExpression = "INSERT INTO Authors (id, Name, Description) VALUES (" + (lastid + 1).ToString() + ", '" + Name + "', '" + Description + "')"; ;

        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();
            SqlCommand command = new SqlCommand(sqlExpression, connection);
            int number = command.ExecuteNonQuery();
            //Console.WriteLine("Добавлено объектов: {0}", number);
        }
        // Console.Read();
    }
    public async Task BookAuthor(int BookId, string author)
    {
        int n=countRows("Authors","Name",author);
        if(n==0)
        {
            AddAuthor(author,"");
        }
        
        DataSet ds_DataSet_Edit = new DataSet();// создаем объект DataSet
                                                //string connectionString = @"Data Source=.\ANDREU;Initial Catalog=Books;Integrated Security=false;User Id=AUE; Password =777;";
        string commandString = "SELECT id FROM Authors WHERE Name = '" + author + "';";// создаем запрос
        SqlDataAdapter Adapter_EDIT = new SqlDataAdapter(commandString, connectionString);// создаем SqlDataAdapter adapter
        Adapter_EDIT.Fill(ds_DataSet_Edit); // заполнение DataSet данными с помощью DataAdapter
        int AuthId = ds_DataSet_Edit.Tables[0].Rows[0].Field<int>("id");
       
            if(!check2("BookAuthor", "AuthorId", AuthId.ToString(),"BookId",BookId.ToString()))
            {
                ds_DataSet_Edit = new DataSet();

        ds_DataSet_Edit = new DataSet();// создаем объект DataSet
        string sqlExpression = "INSERT INTO BookAuthor (BookID, AuthorID) VALUES (" + BookId + ", " + AuthId + ")";

        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();
            SqlCommand command = new SqlCommand(sqlExpression, connection);
            int number = command.ExecuteNonQuery();
            //Console.WriteLine("Добавлено объектов: {0}", number);
        }
            }
        
        
        //Console.Read();
    }
    public async Task BookTag(int bookId, string tag)
    {
        int n = countRows("Tags", "Name", tag);
        if (n == 0)
        {
            AddTag(tag);
        }

        DataSet ds_DataSet_Edit = new DataSet();// создаем объект DataSet
                                                //string connectionString = @"Data Source=.\ANDREU;Initial Catalog=Books;Integrated Security=false;User Id=AUE; Password =777;";
        string commandString = "SELECT id FROM Tags WHERE Name = '" + tag + "';";// создаем запрос
        SqlDataAdapter Adapter_EDIT = new SqlDataAdapter(commandString, connectionString);// создаем SqlDataAdapter adapter
        Adapter_EDIT.Fill(ds_DataSet_Edit); // заполнение DataSet данными с помощью DataAdapter
        int TagId = ds_DataSet_Edit.Tables[0].Rows[0].Field<int>("id");
        
            if (!(check2("BookTag", "TagId", TagId.ToString(),"BookId",bookId.ToString())))
            {
                ds_DataSet_Edit = new DataSet();

                ds_DataSet_Edit = new DataSet();// создаем объект DataSet
                string sqlExpression = "INSERT INTO BookTag (BookID, TagID) VALUES (" + bookId + ", " + TagId + ")";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(sqlExpression, connection);
                    int number = command.ExecuteNonQuery();
                    //Console.WriteLine("Добавлено объектов: {0}", number);
                }
            }
        

    }
    public void DeliteBook(int bookId)
    {
        string sqlExpression = "DELETE  FROM BookAuthor WHERE BookId=" + bookId;
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();
            SqlCommand command = new SqlCommand(sqlExpression, connection);
            int number = command.ExecuteNonQuery();
            //Console.WriteLine("Удалено объектов: {0}", number);
        }
        sqlExpression = "DELETE  FROM BookTag WHERE BookId=" + bookId;
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();
            SqlCommand command = new SqlCommand(sqlExpression, connection);
            int number = command.ExecuteNonQuery();
            //Console.WriteLine("Удалено объектов: {0}", number);
        }
        sqlExpression = "DELETE  FROM Books WHERE id=" + bookId;
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();
            SqlCommand command = new SqlCommand(sqlExpression, connection);
            int number = command.ExecuteNonQuery();
            //Console.WriteLine("Удалено объектов: {0}", number);
        }
    }
    public void DeliteBookAuthor(int id, string author)
    {
        DataSet ds_DataSet_Edit = new DataSet();// создаем объект DataSet
                                                //string connectionString = @"Data Source=.\ANDREU;Initial Catalog=Books;Integrated Security=false;User Id=AUE; Password =777;";
        string commandString = "SELECT id FROM Authors WHERE Name = '" + author + "';";// создаем запрос
        SqlDataAdapter Adapter_EDIT = new SqlDataAdapter(commandString, connectionString);// создаем SqlDataAdapter adapter
        Adapter_EDIT.Fill(ds_DataSet_Edit); // заполнение DataSet данными с помощью DataAdapter
        int AuthId = ds_DataSet_Edit.Tables[0].Rows[0].Field<int>("id");
        
        string sqlExpression = "DELETE  FROM BookAuthor WHERE BookId=" + id + " AND AuthorId=" + AuthId;
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();
            SqlCommand command = new SqlCommand(sqlExpression, connection);
            int number = command.ExecuteNonQuery();
            //Console.WriteLine("Удалено объектов: {0}", number);
        }
    }
    public void renameBook(string book, string newName)
    {
        string sqlExpression = "UPDATE Books SET Name='" + newName + "' WHERE Name='" + book + "'";
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();
            SqlCommand command = new SqlCommand(sqlExpression, connection);
            int number = command.ExecuteNonQuery();
            //Console.WriteLine("Обновлено объектов: {0}", number);
        }
    }
    public void AddLok(int id, string newLoc)
    {
        string sqlExpression = "UPDATE Books SET Location='" + newLoc + "' WHERE id='" + id + "'";
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();
            SqlCommand command = new SqlCommand(sqlExpression, connection);
            int number = command.ExecuteNonQuery();
            //Console.WriteLine("Обновлено объектов: {0}", number);
        }
    }
    //public Book AllAbout(string book)
    //{
    //    List<string> Authors = this.AllAuthors();
    //    List<string> Tags = this.AllTags();
    //    return new Book(book, Authors, Tags);
    //}
    public string curDateTime()
    {
        DateTime myDateTime = DateTime.Now;
        return myDateTime.ToString("yyyy-MM-dd HH:mm:ss.fff");
    }
    public List<int> Tagged(int id)
    {
        int n = countRows("BookTag", "TagId", id.ToString());
        string commandString = "SELECT BookID FROM BookTag WHERE TagID = '" + id.ToString() + "';";// создаем запрос
        List<int> authorID = new List<int>();
        DataSet ds_DataSet_Edit = new DataSet();
        SqlDataAdapter Adapter_EDIT = new SqlDataAdapter(commandString, connectionString);// создаем SqlDataAdapter adapter
        Adapter_EDIT.Fill(ds_DataSet_Edit); // заполнение DataSet данными с помощью DataAdapter
        for (int i = 0; i < n; i++)
        {
            authorID.Add(ds_DataSet_Edit.Tables[0].Rows[i].Field<int>("BookId"));
        }

        return authorID;
    }
    public async Task AddUser(string Name, string PWD)
    {
        DataSet ds_DataSet_Edit = new DataSet();// создаем объект DataSet
        string commandString = "SELECT id FROM Users;";// создаем запрос
        SqlDataAdapter Adapter_EDIT = new SqlDataAdapter(commandString, connectionString);// создаем SqlDataAdapter adapter
        Adapter_EDIT.Fill(ds_DataSet_Edit); // заполнение DataSet данными с помощью DataAdapter
        List<int> ids = new List<int>();
        int n = ds_DataSet_Edit.Tables[0].Rows.Count;
        for (int i = 0; i < n; i++)
        {
            ids.Add(ds_DataSet_Edit.Tables[0].Rows[i].Field<int>("id"));
        }
        int newId;
        bool flag = false;
        int lastid = ids[0];
        for (int i = 1; i < n; i++)
        {
            if (ids[i] > lastid + 1)
            {
                flag = true;
                newId = lastid;
                break;
            }
            lastid = ids[i];
        }
        if (!flag) { newId = n; }


        string sqlExpression = "INSERT INTO Users (id, Name, Password) VALUES (" + (lastid + 1).ToString() + ", '" + Name + "', '" + PWD + "')";

        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();
            SqlCommand command = new SqlCommand(sqlExpression, connection);
            int number = command.ExecuteNonQuery();
            //Console.WriteLine("Добавлено объектов: {0}", number);
        }
        // Console.Read();
    }

}




