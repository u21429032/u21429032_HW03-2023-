using LibraryInc.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Net;
using System.Threading.Tasks;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;

namespace LibraryInc.Controllers
{
    public class StudentBookController : Controller
    {
        private LibraryEntities1 db = new LibraryEntities1();
        // GET: StudentBook

        public async Task<ActionResult> StudentBook(int? page)
        {
            int pagesize = 6, pageindex = 1;
            pageindex = page.HasValue ? Convert.ToInt32(page) : 1;

            var list = db.students.OrderByDescending(x => x.studentId).ToList();
            IPagedList<students> students = list.ToPagedList(pageindex, pagesize);

            var booksList = db.books.Include(b => b.authors).Include(b => b.types).ToList();
            IPagedList<books> books = booksList.ToPagedList(pageindex, pagesize);

            var list3 = db.borrows.Include(b => b.books).Include(b => b.students).ToList();
            IPagedList<borrows> borrows = list3.ToPagedList(pageindex, pagesize);


            var studentbook = new StudentBookViewModel
            {

                Students = await db.students.ToListAsync(),
                Borrowz = await db.borrows.Include(b => b.books).Include(b => b.students).ToListAsync(),
                Books = books,
                PageStudent = students
            };
            return View(studentbook);
        }

        //public ActionResult Index()
        //{
        //    return View();
        //}

        // GET: students
        public async Task<ActionResult> StudentIndex()
        {
            return View(await db.students.ToListAsync());
        }

        // GET: students/Details/5
        public async Task<ActionResult> StudentDetails(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            students students = await db.students.FindAsync(id);
            if (students == null)
            {
                return HttpNotFound();
            }
            return View(students);
        }

        // GET: students/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: students/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> StudentCreate([Bind(Include = "studentId,name,surname,birthdate,gender,class,point")] students students)
        {
            if (ModelState.IsValid)
            {
                db.students.Add(students);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(students);
        }

        // GET: students/Edit/5
        public async Task<ActionResult> StudentEdit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            students students = await db.students.FindAsync(id);
            if (students == null)
            {
                return HttpNotFound();
            }
            return View(students);
        }

        // POST: students/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> StudentEdit([Bind(Include = "studentId,name,surname,birthdate,gender,class,point")] students students)
        {
            if (ModelState.IsValid)
            {
                db.Entry(students).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(students);
        }

        // GET: students/Delete/5
        public async Task<ActionResult> StudentDelete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            students students = await db.students.FindAsync(id);
            if (students == null)
            {
                return HttpNotFound();
            }
            return View(students);
        }

        // POST: students/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> StudentDeleteConfirmed(int id)
        {
            students students = await db.students.FindAsync(id);
            db.students.Remove(students);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

      

        public async Task<ActionResult> BookIndex()
        {
            var books = db.books.Include(b => b.authors).Include(b => b.types);
            return View(await books.ToListAsync());
        }

        // GET: books/Details/5
        public async Task<ActionResult> BookDetails(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            books books = await db.books.FindAsync(id);
            if (books == null)
            {
                return HttpNotFound();
            }
            return View(books);
        }

        // GET: books/Create
        public ActionResult BookCreate()
        {
            ViewBag.authorId = new SelectList(db.authors, "authorId", "name");
            ViewBag.typeId = new SelectList(db.types, "typeId", "name");
            return View();
        }

        // POST: books/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> BookCreate([Bind(Include = "bookId,name,pagecount,point,authorId,typeId")] books books)
        {
            if (ModelState.IsValid)
            {
                db.books.Add(books);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.authorId = new SelectList(db.authors, "authorId", "name", books.authorId);
            ViewBag.typeId = new SelectList(db.types, "typeId", "name", books.typeId);
            return View(books);
        }

        // GET: books/Edit/5
        public async Task<ActionResult> BookEdit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            books books = await db.books.FindAsync(id);
            if (books == null)
            {
                return HttpNotFound();
            }
            ViewBag.authorId = new SelectList(db.authors, "authorId", "name", books.authorId);
            ViewBag.typeId = new SelectList(db.types, "typeId", "name", books.typeId);
            return View(books);
        }

        // POST: books/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> BookEdit([Bind(Include = "bookId,name,pagecount,point,authorId,typeId")] books books)
        {
            if (ModelState.IsValid)
            {
                db.Entry(books).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.authorId = new SelectList(db.authors, "authorId", "name", books.authorId);
            ViewBag.typeId = new SelectList(db.types, "typeId", "name", books.typeId);
            return View(books);
        }

        // GET: books/Delete/5
        public async Task<ActionResult> BookDelete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            books books = await db.books.FindAsync(id);
            if (books == null)
            {
                return HttpNotFound();
            }
            return View(books);
        }

        // POST: books/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> BookDeleteConfirmed(int id)
        {
            books books = await db.books.FindAsync(id);
            db.books.Remove(books);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

    }

}