using BusinessLayer.Interfaces;
using BusinessLayer.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ModelLayer.Models;
using RepositoryLayer.Entities;

namespace BookStore_WebAPI_Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly IBookBusiness bookBusiness;
        public BookController(IBookBusiness bookBusiness)
        {
            this.bookBusiness = bookBusiness;
        }

        [HttpPost("add")]
        public IActionResult AddBook(BookModel bookModel)
        {
            try
            {
                var book = bookBusiness.AddBook(bookModel);
                return Ok(new ResponseModel<Book> { IsSuccess = true, Message = "Book added successfully", Data = book });
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseModel<string> { IsSuccess = false, Message = "Failed to add book", Data = ex.Message });
            }
        }

        [HttpGet("books")]
        public IActionResult GetAllBooks()
        {
            try
            {
                var books = bookBusiness.GetAllBooks().ToList();
                return Ok(new ResponseModel<List<Book>> { IsSuccess = true, Message = "All Books fetched successfully", Data = books });
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseModel<string> { IsSuccess = false, Message = "Failed to fetch books", Data = ex.Message });
            }
        }

        [HttpGet("getById")]
        public IActionResult GetBookById(int bookId)
        {
            try
            {
                var book = bookBusiness.GetBookById(bookId);
                return Ok(new ResponseModel<Book> { IsSuccess = true, Message = "Book fetched successfully", Data = book });
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseModel<string> { IsSuccess = false, Message = "Failed to fetch book", Data = ex.Message });
            }
        }

        [HttpGet("getByName")]
        public IActionResult GetBookByName(string? title, string? author)
        {
            try
            {
                var books = bookBusiness.GetBookByName(title, author).ToList();
                return Ok(new ResponseModel<List<Book>> { IsSuccess = true, Message = "Books fetched successfully", Data = books });
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseModel<string> { IsSuccess = false, Message = "Failed to fetch books", Data = ex.Message });
            }
        }

        [HttpPut("update")]
        public IActionResult UpdateBook(int bookId, BookModel bookModel)
        {
            try
            {
                var book = bookBusiness.UpdateBook(bookId, bookModel);
                return Ok(new ResponseModel<Book> { IsSuccess = true, Message = "Book updated successfully", Data = book });
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseModel<string> { IsSuccess = false, Message = "Failed to update book", Data = ex.Message });
            }
        }

        [HttpDelete("delete")]
        public IActionResult DeleteBook(int bookId)
        {
            try
            {
                var result = bookBusiness.DeleteBook(bookId);
                return Ok(new ResponseModel<bool> { IsSuccess = true, Message = "Book deleted successfully", Data = result });
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseModel<string> { IsSuccess = false, Message = "Failed to delete book", Data = ex.Message });
            }
        }

    }
}
