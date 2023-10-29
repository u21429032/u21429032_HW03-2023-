using LibraryInc.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static LibraryInc.Models.ChartData;

namespace LibraryInc.Controllers
{
    public class ChartsController : Controller
    {
        private LibraryEntities1 db = new LibraryEntities1();
        // GET: Charts
        public ActionResult Index()
        {
            

            var bookCountsByType = db.books
    .GroupBy(b => b.types.name) // Use the TypeName property of the related Type entity
    .Select(group => new
    {
        TypeName = group.Key,
        Count = group.Count()
    })
    .ToList();
            var chartData = new ChartData
            {
                Labels = bookCountsByType.Select(x => x.TypeName).ToList(),
                Values = bookCountsByType.Select(x => x.Count).ToList(),

                TotalStudents = db.students.Count(),
                TotalBooks = db.books.Count(),
                TotalBorrows = db.borrows.Count(),
                MostPopularBooks = GetMostPopularBooks(),
                StudentsByClass = GetStudentsByClass()

            };

         

            return View(chartData);

            
        }

        private List<MostPopularBookViewModel> GetMostPopularBooks()
        {
            // Implement your logic to get the most popular books here
            // Example: Return a list of books with the highest borrows
            return db.books.OrderByDescending(b => b.borrows.Count)
                          .Select(book => new MostPopularBookViewModel
                          {
                              BookName = book.name,
                              TotalBorrows = book.borrows.Count
                          })
                          .Take(5) // Limit to the top 5 books
                          .ToList();
        }

        private List<StudentsByClassViewModel> GetStudentsByClass()
        {
            // Implement your logic to get the count of students by class
            // Example: Return the count of students in each class
            return db.students.GroupBy(s => s.@class)
                             .Select(group => new StudentsByClassViewModel
                             {
                                 ClassName = group.Key,
                                 TotalStudents = group.Count()
                             })
                             .ToList();
        }

        public ActionResult Gender()
        {
            var genderCounts = db.students
                .GroupBy(s => s.gender)
                .Select(group => new
                {
                    Gender = group.Key,
                    Count = group.Count()
                })
                .ToList();

            var chartData = new ChartData
            {
                Labels = genderCounts.Select(x => x.Gender).ToList(),
                Values = genderCounts.Select(x => x.Count).ToList(),
            };

            return View(chartData);
        }

        public ActionResult Classes()
        {
            // Retrieve class information and student counts
            var classInfo = db.students
                .GroupBy(s => s.@class)
                .Select(group => new
                {
                    ClassName = group.Key,
                    StudentCount = group.Count()
                })
                .ToList();

            // Create the ChartData object
            var chartData = new ChartData
            {
                ClassNames = classInfo.Select(item => item.ClassName).ToList(),
                StudentCounts = classInfo.Select(item => item.StudentCount).ToList(),
                TotalStudents = db.students.Count(),
                TotalBooks = db.books.Count(),
                TotalBorrows = db.borrows.Count(),
                MostPopularBooks = GetMostPopularBooks(),
                StudentsByClass = GetStudentsByClass()
            };

            return View( chartData);
        }

    }
}
