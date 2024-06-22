using ModelLayer.Models;
using RepositoryLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Interfaces
{
    public interface IBookRepository
    {
        Book AddBook(BookModel bookModel);

        List<Book> GetAllBooks();

        Book GetBookById(int bookId);

        List<Book> GetBookByName(string title, string author);

        Book UpdateBook(int bookId, BookModel bookModel);

        bool DeleteBook(int bookId);

        // Review Task
        List<Book> GetBook_ByTitleAndPrice(string title, int price);
        Book Insert_Update_Book(int bookId, BookModel bookModel);
    }
}
