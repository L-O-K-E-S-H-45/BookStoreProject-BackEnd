﻿using BusinessLayer.Interfaces;
using ModelLayer.Models;
using RepositoryLayer.Entities;
using RepositoryLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Services
{
    public class BookBusiness : IBookBusiness
    {
        private readonly IBookRepository bookRepository;
        public BookBusiness(IBookRepository bookRepository)
        {
            this.bookRepository = bookRepository;
        }
        public Book AddBook(BookModel bookModel)
        {
            return bookRepository.AddBook(bookModel);
        }

        public List<Book> GetAllBooks()
        {
            return bookRepository.GetAllBooks();
        }

        public Book GetBookById(int bookId)
        {
            return bookRepository.GetBookById(bookId);
        }

        public List<Book> GetBookByName(string title, string author)
        {
            return bookRepository.GetBookByName(title, author);
        }

        public Book UpdateBook(int bookId, BookModel bookModel)
        {
            return bookRepository.UpdateBook(bookId, bookModel);
        }

        public bool DeleteBook(int bookId)
        {
            return bookRepository.DeleteBook(bookId);
        }

        // review
        public List<Book> GetBook_ByTitleAndPrice(string title, int price)
        {
            return bookRepository.GetBook_ByTitleAndPrice(title, price);
        }

        public Book Insert_Update_Book(int bookId, BookModel bookModel)
        {
            return bookRepository.Insert_Update_Book(bookId, bookModel);
        }

    }
}
