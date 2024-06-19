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

        Book UpdateBook(int bookId, BookModel bookModel);

        bool DeleteBook(int bookId);
    }
}
