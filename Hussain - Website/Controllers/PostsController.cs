using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using Hussain___Website.Models;
using System.IO;
using PagedList;
using PagedList.Mvc;

namespace Hussain___Website.Controllers
{
    [RequireHttps]
    public class PostsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Posts
        public ActionResult Index(int? page)
        {
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            IList<Post> p = db.Posts.ToList();
            return View(p.ToPagedList(pageNumber, pageSize));
        }
        //POST: Posts
        [HttpPost]
        public ActionResult Index(string searchStr, int? page)
        {
            var result = db.Posts.Where(p => p.Body.Contains(searchStr)).Union(db.Posts.Where(p => p.Title.Contains(searchStr)).Union(db.Posts.Where(p => p.Comments.Any(c => c.Body.Contains(searchStr)))));
            int itemsPerPage = 10;
            int pageNumber = page ?? 1;
            IList<Post> plist = result.ToList();
            return View(plist.ToPagedList(pageNumber, itemsPerPage));
        }

        // GET: Posts/Details/5
        public ActionResult Details(int? id, int? page)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Post post = db.Posts.Find(id);
            if (post == null)
            {
                return HttpNotFound();
            }
            var comments = db.Comments.Where(c => c.PostId == id).ToList();
            post.Comments = comments;
            return View(post);
        }
        

        // GET: Posts/Create
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Posts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Title,Slug,Body,MediaURL")] Post post, HttpPostedFileBase fileUpload)
        {
            if (ModelState.IsValid)
            {
                //Restricting the valid file formats to images only
                if (fileUpload != null && fileUpload.ContentLength > 0)
                {
                    if (!fileUpload.ContentType.Contains("image"))
                    {
                        return new HttpStatusCodeResult(HttpStatusCode.UnsupportedMediaType);
                    }
                    //var fileName = Path.GetFileName(fileUpload.FileName);
                    WebImage fileName = new WebImage(fileUpload.InputStream);
                    if (fileName.Width > 500)
                        fileName.Resize(500, 500);
                    //fileUpload.SaveAs(Path.Combine(Server.MapPath("~/assets/img/blog/"), fileName));
                    fileName.Save("~/assets/img/blog/"+fileUpload.FileName);
                    post.MediaURL = "~/assets/img/blog/" + fileUpload.FileName;
                }
                var cDate = DateTimeOffset.UtcNow;
                post.CreationDate = cDate.ToLocalTime();
                db.Posts.Add(post);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(post);
        }

        // GET: Posts/Edit/5
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Post post = db.Posts.Find(id);
            if (post == null)
            {
                return HttpNotFound();
            }
            return View(post);
        }

        // POST: Posts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,CreationDate,UpdatedDate,Title,Slug,Body,MediaURL")] Post post, HttpPostedFileBase fileUpload)
        {
            if (ModelState.IsValid)
            {
                if (fileUpload != null && fileUpload.ContentLength > 0)
                {
                    if (!fileUpload.ContentType.Contains("image"))
                    {
                        return new HttpStatusCodeResult(HttpStatusCode.UnsupportedMediaType);
                    }
                    var fileName = Path.GetFileName(fileUpload.FileName);
                    fileUpload.SaveAs(Path.Combine(Server.MapPath("~/assets/img/blog/"), fileName));
                    post.MediaURL = "~/assets/img/blog/" + fileName;

                }
                post.UpdatedDate = DateTime.Now;
                db.Entry(post).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(post);
        }

        // GET: Posts/Delete/5
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Post post = db.Posts.Find(id);
            if (post == null)
            {
                return HttpNotFound();
            }
            return View(post);
        }

        // POST: Posts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Post post = db.Posts.Find(id);
            db.Posts.Remove(post);
            db.SaveChanges();
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


