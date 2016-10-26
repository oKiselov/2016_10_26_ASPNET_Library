using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Net.NetworkInformation;
using System.Text;
using System.Web.Mvc;
using LibraryApp.Models.Enum;
using LibraryApp.Models.Factory;
using LibraryApp.Models.Settings;

namespace LibraryApp.Models
{
    public class DataService
    {
        /// <summary>
        /// Method puts data about current author to Author Table 
        /// </summary>
        /// <param name="currAuthor"></param>
        public static void AuthorToDataBase(Author currAuthor)
        {
            SqlParameter paramId = new SqlParameter("@id", currAuthor.AuthorId);
            SqlParameter paramFName = new SqlParameter("@FName", currAuthor.FirstName);
            SqlParameter paramLName = new SqlParameter("@LName", currAuthor.LastName);

            using (SqlConnection connection = new SqlConnection(ConnectionSettings.connectionString))
            {
                connection.Open();
                using (SqlCommand cmd = connection.CreateCommand())
                {
                    cmd.CommandText =
                        "INSERT INTO [dbo].[Author]([AuthorId], [FirstName], [LastName]) VALUES (@id, @FName, @LName)";
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.Add(paramId);
                    cmd.Parameters.Add(paramFName);
                    cmd.Parameters.Add(paramLName);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// Method puts data about current book to IPrint Table 
        /// </summary>
        /// <param name="currBook"></param>
        public static void BookToDataBase(Book currBook)
        {
            SqlParameter paramId = new SqlParameter("@id", currBook.BookId);
            SqlParameter paramTitle = new SqlParameter("@Title", currBook.Title);
            SqlParameter paramAvail = new SqlParameter("@Keeper", currBook.Keeper);

            using (SqlConnection connection = new SqlConnection(ConnectionSettings.connectionString))
            {
                connection.Open();
                using (SqlCommand cmd = connection.CreateCommand())
                {
                    cmd.CommandText =
                        "INSERT INTO [dbo].[IPrint]([BookId], [Title], [Keeper]) VALUES (@id, @Title, @Keeper)";
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.Add(paramId);
                    cmd.Parameters.Add(paramTitle);
                    cmd.Parameters.Add(paramAvail);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// Method puts data about relation book-author(-s) to AuthorBook Table 
        /// </summary>
        /// <param name="currBookId"></param>
        /// <param name="currAuthorId"></param>
        public static void BookAuthorToDataBase(Guid currBookId, Guid currAuthorId)
        {
            SqlParameter paramBookId = new SqlParameter("@BookId", currBookId);
            SqlParameter paramAuthorId = new SqlParameter("@AuthorId", currAuthorId);

            using (SqlConnection connection = new SqlConnection(ConnectionSettings.connectionString))
            {
                connection.Open();
                using (SqlCommand cmd = connection.CreateCommand())
                {
                    cmd.CommandText =
                        "INSERT INTO [dbo].[AuthorBook]([BookId], [AuthorId]) VALUES (@Bookid, @AuthorId)";
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.Add(paramBookId);
                    cmd.Parameters.Add(paramAuthorId);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// Method returns data about all books, ordered by Title 
        /// Paging : 10 titles on one page  
        /// </summary>
        /// <param name="iBegin"></param>
        /// <returns></returns>
        public static List<Book> AllBooksFromDataBase(int iBegin)
        {
            List<Book> lstBooks = new List<Book>();
            SqlParameter paramPageBegin = new SqlParameter("@RowBegin", iBegin);
            // Проверка на выход за пределы количества строк в базе 
            SqlParameter paramPageEnd = new SqlParameter("@RowEnd", iBegin + 10);

            using (SqlConnection connection = new SqlConnection(ConnectionSettings.connectionString))
            {
                string query =
                    "SELECT * FROM (SELECT ROW_NUMBER() OVER (ORDER BY Title) AS RowNum, * FROM IPrint) AS RowConstrainedResult WHERE RowNum >= @RowBegin AND RowNum < @RowEnd ORDER BY Title";
                //string query = "SELECT * FROM IPrint"; 
                SqlCommand cmdRead = new SqlCommand(query, connection);
                connection.Open();
                // надо ли то, что внизу??? 
                cmdRead.Parameters.Add(paramPageBegin);
                cmdRead.Parameters.Add(paramPageEnd);
                using (SqlDataReader reader = cmdRead.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        lstBooks.Add(new Book(reader.GetGuid(1), reader.GetString(2), reader.GetGuid(3)));
                    }
                }
            }
            return lstBooks;
        }

        /// <summary>
        /// Method returns data about all Available books, ordered by Title 
        /// Paging : 10 titles on one page  
        /// </summary>
        /// <param name="iBegin"></param>
        /// <returns></returns>
        public static List<Book> AvailableBooksFromDataBase(int iBegin)
        {
            List<Book> lstBooks = new List<Book>();
            SqlParameter paramPageBegin = new SqlParameter("@RowBegin", iBegin);
            // Проверка на выход за пределы количества строк в базе 
            SqlParameter paramPageEnd = new SqlParameter("@RowEnd", iBegin + 10);
            SqlParameter paramAvail = new SqlParameter("@Keeper", Guid.Empty);

            using (SqlConnection connection = new SqlConnection(ConnectionSettings.connectionString))
            {
                string query =
                    "SELECT * FROM (SELECT ROW_NUMBER() OVER (ORDER BY Title) AS RowNum, * FROM IPrint) AS RowConstrainedResult WHERE RowNum >= @RowBegin AND RowNum < @RowEnd AND Keeper = @Keeper ORDER BY Title";
                //string query = "SELECT * FROM IPrint"; 
                SqlCommand cmdRead = new SqlCommand(query, connection);
                connection.Open();
                // надо ли то, что внизу??? 
                cmdRead.Parameters.Add(paramPageBegin);
                cmdRead.Parameters.Add(paramPageEnd);
                cmdRead.Parameters.Add(paramAvail);
                using (SqlDataReader reader = cmdRead.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        lstBooks.Add(new Book(reader.GetGuid(1), reader.GetString(2), reader.GetGuid(3)));
                    }
                }
            }
            return lstBooks;
        }

        /// <summary>
        /// Method puts data about user to IAccount Table
        /// </summary>
        /// <param name="currUser"></param>
        public static void UserToDataBase(User currUser)
        {
            SqlParameter paramId = new SqlParameter("@id", currUser.UserId);
            SqlParameter paramMail = new SqlParameter("@Mail", currUser.eMail);
            SqlParameter paramFName = new SqlParameter("@FName", currUser.FirstName);
            SqlParameter paramLName = new SqlParameter("@LName", currUser.LastName);
            SqlParameter paramDate = new SqlParameter("@Date", currUser.RegDate);
            SqlParameter paramRole = new SqlParameter("@Role", currUser.Role);

            using (SqlConnection connection = new SqlConnection(ConnectionSettings.connectionString))
            {
                connection.Open();
                using (SqlCommand cmd = connection.CreateCommand())
                {
                    cmd.CommandText =
                        "INSERT INTO [dbo].[IAccount]([UserId], [eMail], [FirstName], [LastName], [RegDate], [Role]) VALUES (@id, @Mail, @FName, @LName, @Date, @Role)";
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.Add(paramId);
                    cmd.Parameters.Add(paramMail);
                    cmd.Parameters.Add(paramFName);
                    cmd.Parameters.Add(paramLName);
                    cmd.Parameters.Add(paramDate);
                    cmd.Parameters.Add(paramRole);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// Method returns User object, searched by its e-mail 
        /// </summary>
        /// <param name="strMail"></param>
        /// <returns></returns>
        public static User SearchUser(string strMail)
        {
            User currUser = new User();
            SqlParameter paramMail = new SqlParameter("@Mail", strMail);

            using (SqlConnection connection = new SqlConnection(ConnectionSettings.connectionString))
            {
                string query = "SELECT * FROM IAccount WHERE eMail = @Mail";
                SqlCommand cmdRead = new SqlCommand(query, connection);
                connection.Open();
                cmdRead.Parameters.Add(paramMail);
                using (SqlDataReader reader = cmdRead.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        currUser.UserId = reader.GetGuid(0);
                        currUser.eMail = reader.GetString(1);
                        currUser.FirstName = reader.GetString(2);
                        currUser.LastName = reader.GetString(3);
                        currUser.RegDate = reader.GetString(4);
                        currUser.Role = reader.GetInt32(5);
                    }
                }
            }
            return currUser;
        }

        /// <summary>
        /// Method returns User object, searched by its User's ID  
        /// </summary>
        /// <param name="currGuid"></param>
        /// <returns></returns>
        public static User SearchUser(Guid currGuid)
        {
            User currUser = new User();
            SqlParameter paramGuid = new SqlParameter("@Guid", currGuid);

            using (SqlConnection connection = new SqlConnection(ConnectionSettings.connectionString))
            {
                string query = "SELECT * FROM IAccount WHERE UserId = @Guid";
                SqlCommand cmdRead = new SqlCommand(query, connection);
                connection.Open();
                cmdRead.Parameters.Add(paramGuid);
                using (SqlDataReader reader = cmdRead.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        currUser.UserId = reader.GetGuid(0);
                        currUser.eMail = reader.GetString(1);
                        currUser.FirstName = reader.GetString(2);
                        currUser.LastName = reader.GetString(3);
                        currUser.RegDate = reader.GetString(4);
                        currUser.Role = reader.GetInt32(5);
                    }
                }
            }
            return currUser;
        }

        /// <summary>
        /// Checks users existing 
        /// Returns true - if exists, false - if not 
        /// </summary>
        /// <param name="strMail"></param>
        /// <returns></returns>
        public static bool IsUser(string strMail)
        {
            User currUser = new User();
            SqlParameter paramMail = new SqlParameter("@Mail", strMail);

            using (SqlConnection connection = new SqlConnection(ConnectionSettings.connectionString))
            {
                string query = "SELECT * FROM IAccount WHERE eMail = @Mail";
                SqlCommand cmdRead = new SqlCommand(query, connection);
                connection.Open();
                cmdRead.Parameters.Add(paramMail);
                using (SqlDataReader reader = cmdRead.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        currUser.UserId = reader.GetGuid(0);
                        currUser.eMail = reader.GetString(1);
                        currUser.FirstName = reader.GetString(2);
                        currUser.LastName = reader.GetString(3);
                        currUser.RegDate = reader.GetString(4);
                        currUser.Role = reader.GetInt32(5);
                    }
                }
            }
            bool bRet = true;
            if (currUser.eMail == null)
            {
                bRet = false;
            }
            return bRet;
        }

        /// <summary>
        /// Checks users existing 
        /// Returns true - if exists, false - if not 
        /// </summary>
        /// <param name="currGuid"></param>
        /// <returns></returns>
        public static bool IsUser(Guid currGuid)
        {
            User currUser = new User();
            SqlParameter paramGuid = new SqlParameter("@Guid", currGuid);

            using (SqlConnection connection = new SqlConnection(ConnectionSettings.connectionString))
            {
                string query = "SELECT * FROM IAccount WHERE UserId = @Guid";
                SqlCommand cmdRead = new SqlCommand(query, connection);
                connection.Open();
                cmdRead.Parameters.Add(paramGuid);
                using (SqlDataReader reader = cmdRead.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        currUser.UserId = reader.GetGuid(0);
                        currUser.eMail = reader.GetString(1);
                        currUser.FirstName = reader.GetString(2);
                        currUser.LastName = reader.GetString(3);
                        currUser.RegDate = reader.GetString(4);
                        currUser.Role = reader.GetInt32(5);
                    }
                }
            }
            bool bRet = true;
            if (currUser.eMail == null)
            {
                bRet = false;
            }
            return bRet;
        }

        /// <summary>
        /// Method deletes user from IAccount table  
        /// </summary>
        /// <param name="strMail"></param>
        public static void DeleteUser(string strMail)
        {
            SqlParameter paramMail = new SqlParameter("@Mail", strMail);

            using (SqlConnection connection = new SqlConnection(ConnectionSettings.connectionString))
            {
                connection.Open();

                using (SqlCommand cmd = connection.CreateCommand())
                {
                    cmd.CommandText =
                        "DELETE FROM IAccount WHERE eMail = @Mail";
                    cmd.Parameters.Add(paramMail);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// Method returns collection of books with the same title 
        /// </summary>
        /// <param name="strTitle"></param>
        /// <returns></returns>
        public static List<Book> SearchBookByName(string strTitle)
        {
            List<Book> lstBooks = new List<Book>();
            SqlParameter paramTitle = new SqlParameter("@strTitle", strTitle);

            using (SqlConnection connection = new SqlConnection(ConnectionSettings.connectionString))
            {
                string query = "SELECT * FROM IPrint WHERE Title = @strTitle";
                SqlCommand cmdRead = new SqlCommand(query, connection);
                connection.Open();
                cmdRead.Parameters.Add(paramTitle);
                using (SqlDataReader reader = cmdRead.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        lstBooks.Add(new Book(reader.GetGuid(0), reader.GetString(1), reader.GetGuid(2)));
                    }
                }
            }
            return lstBooks;
        }

        /// <summary>
        /// Method returns book by its Book ID 
        /// </summary>
        /// <param name="bookId"></param>
        /// <returns></returns>
        public static Book SearchBookById(Guid bookId)
        {
            Book currBook = new Book();
            SqlParameter paramGuid = new SqlParameter("@BookId", bookId);

            using (SqlConnection connection = new SqlConnection(ConnectionSettings.connectionString))
            {
                string query = "SELECT * FROM IPrint WHERE BookId = @BookId";
                SqlCommand cmdRead = new SqlCommand(query, connection);
                connection.Open();
                cmdRead.Parameters.Add(paramGuid);
                using (SqlDataReader reader = cmdRead.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        currBook = new Book(reader.GetGuid(0), reader.GetString(1), reader.GetGuid(2));

                    }
                }
            }
            return currBook;
        }

        /// <summary>
        /// Checks books existing 
        /// Returns true - if exists, false - if not 
        /// </summary>
        /// <param name="bookId"></param>
        /// <returns></returns>
        public static bool IsBook(Guid bookId)
        {
            Book currBook = new Book();
            SqlParameter paramGuid = new SqlParameter("@BookId", bookId);

            using (SqlConnection connection = new SqlConnection(ConnectionSettings.connectionString))
            {
                string query = "SELECT * FROM IPrint WHERE BookId = @BookId";
                SqlCommand cmdRead = new SqlCommand(query, connection);
                connection.Open();
                cmdRead.Parameters.Add(paramGuid);
                using (SqlDataReader reader = cmdRead.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        currBook = new Book(reader.GetGuid(0), reader.GetString(1), reader.GetGuid(2));
                    }
                }
            }
            bool bRet = true;
            if (currBook.Title == null)
            {
                bRet = false;
            }
            return bRet;
        }

        /// <summary>
        /// Method returns AuthorID of all book's authors 
        /// </summary>
        /// <param name="bookId"></param>
        /// <returns></returns>
        public static List<Guid> GetAuthorsOfBookGuids(Guid bookId)
        {
            List<Guid> currAuthorGuid = new List<Guid>();
            SqlParameter paramBookId = new SqlParameter("@BookId", bookId);
            using (SqlConnection connection = new SqlConnection(ConnectionSettings.connectionString))
            {
                connection.Open();
                using (SqlCommand cmd = connection.CreateCommand())
                {
                    cmd.CommandText = "SELECT * FROM AuthorBook WHERE BookId = @BookId";
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.Add(paramBookId);
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        currAuthorGuid.Add(reader.GetGuid(1));
                    }
                }
            }
            return currAuthorGuid;
        }

        /// <summary>
        /// Method returns Author object by its Author ID 
        /// </summary>
        /// <param name="currAuthorGuid"></param>
        /// <returns></returns>
        public static Author GetAuthorsByGuid(Guid currAuthorGuid)
        {
            Author currAuthor = new Author();
            SqlParameter paramGuid = new SqlParameter("@AuthorId", currAuthorGuid);
            using (SqlConnection connection = new SqlConnection(ConnectionSettings.connectionString))
            {
                connection.Open();
                using (SqlCommand cmd = connection.CreateCommand())
                {
                    cmd.CommandText = "SELECT * FROM Author WHERE AuthorId = @AuthorId";
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.Add(paramGuid);
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        currAuthor = new Author(reader.GetGuid(0), reader.GetString(1), reader.GetString(2));
                    }
                }
            }
            return currAuthor;
        }

        /// <summary>
        /// Method deletes book from all tables by its BookID 
        /// </summary>
        /// <param name="bookId"></param>
        public static void DeleteBookById(Guid bookId)
        {
            SqlParameter paramGuid = new SqlParameter("@BookId", bookId);

            using (SqlConnection connection = new SqlConnection(ConnectionSettings.connectionString))
            {
                connection.Open();

                using (SqlCommand cmd = connection.CreateCommand())
                {
                    cmd.CommandText =
                        "DELETE FROM AuthorBook WHERE BookId = @BookId";
                    cmd.Parameters.Add(paramGuid);
                    cmd.ExecuteNonQuery();

                    cmd.CommandText =
                        "DELETE FROM IPrint WHERE BookId = @BookId";
                    cmd.ExecuteNonQuery();

                }
            }
        }

        /// <summary>
        /// Method makes changes about book issue/return to BookUsageHistory Table 
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="bookId"></param>
        /// <param name="iAction"></param>
        public static void UpdateBookAction(Guid userId, Guid bookId, int iAction)
        {
            SqlParameter paramBookId = new SqlParameter("@BookId", bookId);
            SqlParameter paramUserId = new SqlParameter("@UserId", userId);
            SqlParameter paramAction = new SqlParameter("@Action", iAction);
            SqlParameter paramKeeper; 

            if (iAction == (int) IsAvailable.Yes)
            {
                  paramKeeper = new SqlParameter("@Keeper", Guid.Empty);
            }
            else
            {
                paramKeeper = new SqlParameter("@Keeper", userId);
            }
            SqlParameter paramDate = new SqlParameter("@Date", DateTime.UtcNow.ToString());

            using (SqlConnection connection = new SqlConnection(ConnectionSettings.connectionString))
            {
                connection.Open();

                using (SqlCommand cmd = connection.CreateCommand())
                {
                    cmd.CommandText =
                        "UPDATE IPrint SET Keeper = @Keeper WHERE BookId=@BookId";
                    cmd.Parameters.Add(paramBookId);
                    cmd.Parameters.Add(paramAction);
                    cmd.Parameters.Add(paramUserId);
                    cmd.Parameters.Add(paramKeeper); 

                    cmd.ExecuteNonQuery();

                    cmd.CommandText =
                        "INSERT INTO BookUsageHistory (UserId, BookId, Date, Action) VALUES (@UserId, @BookId, @Date, @Action)";
                    cmd.Parameters.Add(paramDate);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// Returns information about current book and its authors 
        /// </summary>
        /// <param name="currGuid"></param>
        /// <returns></returns>
        public static KeyValuePair<Book, List<Author>> GetBookDetails(Guid currGuid)
        {
            // returns book by its BookID
            Book currBook = DataService.SearchBookById(currGuid);

            // returns amoont of authors ID
            List<Guid> lstAuthorsGuids = DataService.GetAuthorsOfBookGuids(currGuid);

            List<Author> lstAuthors = new List<Author>();
            for (int i = 0; i < lstAuthorsGuids.Count; i++)
            {
                lstAuthors.Add(DataService.GetAuthorsByGuid(lstAuthorsGuids[i]));
            }
            return new KeyValuePair<Book, List<Author>>(currBook, lstAuthors);
        }

        /// <summary>
        /// Method returns info about book usage history
        /// </summary>
        /// <param name="currBookGuid"></param>
        /// <returns></returns>
        public static List<KeyValuePair<Guid, string>> GetBookHistory(Guid currBookGuid)
        {
            List<KeyValuePair<Guid, string>> lstPair = new List<KeyValuePair<Guid, string>>();
            SqlParameter paramGuid = new SqlParameter("@BookId", currBookGuid);
            SqlParameter paramAction = new SqlParameter("@Action", (int)IsAvailable.No);
            using (SqlConnection connection = new SqlConnection(ConnectionSettings.connectionString))
            {
                connection.Open();
                using (SqlCommand cmd = connection.CreateCommand())
                {
                    cmd.CommandText = "SELECT * FROM BookUsageHistory WHERE BookId = @BookId AND Action = @Action";
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.Add(paramGuid);
                    cmd.Parameters.Add(paramAction);
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        lstPair.Add(new KeyValuePair<Guid, string>(reader.GetGuid(0), reader.GetString(2)));
                    }
                }
            }
            return lstPair;
        }

        /// <summary>
        /// Checks users existing 
        /// Returns true - if exists, false - if not 
        /// </summary>
        /// <param name="currGuid"></param>
        /// <returns></returns>
        public static List<Book> GetBooksTakenByUser(Guid currGuid)
        {
            List<Book> lstBooks = new List<Book>();
            SqlParameter paramGuid = new SqlParameter("@Guid", currGuid);

            using (SqlConnection connection = new SqlConnection(ConnectionSettings.connectionString))
            {
                string query = "SELECT * FROM IPrint WHERE Keeper = @Guid";
                SqlCommand cmdRead = new SqlCommand(query, connection);
                connection.Open();
                cmdRead.Parameters.Add(paramGuid);
                using (SqlDataReader reader = cmdRead.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        lstBooks.Add(new Book(reader.GetGuid(0), reader.GetString(1), reader.GetGuid(2)));

                    }
                }
            }
            return lstBooks; 
        }
    }
}
