﻿using ModelLayer.Models;
using RepositoryLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Interfaces
{
    public interface IBookBusiness
    {
        Book AddBook(BookModel bookModel);

        Book GetBookById(int bookId);

        List<Book> GetAllBooks();

        Book UpdateBook(int bookId, BookModel bookModel);

        bool DeleteBook(int bookId);
    }
}