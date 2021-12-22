using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OneHealth.Data;
using OneHealth.Models;
using OneHealth.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace OneHealth.Areas.admin.Controllers
{
    [Area("admin")]
    public class NewsController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;


        public NewsController(AppDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {

            VmNews news = new VmNews()
            {
                News = _context.News.ToList(),
                NewsCategories = _context.NewsCategories.ToList(),
                Tag = _context.Tags.FirstOrDefault()
            };
            //List<News> doctor = _context.News.OrderByDescending(o => o.CreatedDate)
            //                                 .Include(nc => nc.NewsCategory)
            //                                 .Include(u => u.User)
            //                                 .Include(tb => tb.TagToNews).ThenInclude(t => t.Tag)
            //                                 .ToList();
            return View(news);
        }

        public IActionResult Create()
        {
            ViewBag.Category = _context.NewsCategories.ToList();
            ViewBag.Tags = _context.Tags.ToList();
            return View();
        }

        [HttpPost]
        public IActionResult Create(News model)
        {
            if (ModelState.IsValid)
            {
                if (model.NewsImageFile.ContentType == "image/jpeg" || model.NewsImageFile.ContentType == "image/png")
                {
                    if (model.NewsImageFile.Length <= 2097152)
                    {

                        //Create Blog
                        string fileName = Guid.NewGuid() + "-" + DateTime.Now.ToString("yyyyMMddHHmmss") + "-" + model.NewsImageFile.FileName;
                        string filePath = Path.Combine(_webHostEnvironment.WebRootPath, "Uploads", fileName);
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            model.NewsImageFile.CopyTo(stream);
                        }

                        model.NewsImage = fileName;
                        model.CreatedDate = DateTime.Now;
                        model.UserId =1;

                        _context.News.Add(model);
                        _context.SaveChanges();


                        //Create Tag to blog
                        if (model.TagToNewsId != null && model.TagToNewsId.Count > 0)
                        {
                            foreach (var item in model.TagToNewsId)
                            {
                                TagToNews tagToBlog = new TagToNews();
                                tagToBlog.TagId = item;
                                tagToBlog.NewsId = model.Id;
                                _context.TagToNews.Add(tagToBlog);
                                _context.SaveChanges();
                            }
                        }

                        return RedirectToAction("Index");

                    }
                    else
                    {
                        ModelState.AddModelError("", "You can upload only less than 2 mb.");
                        ViewBag.Category = _context.NewsCategories.ToList();
                        ViewBag.Tags = _context.Tags.ToList();
                        return View(model);
                    }
                }
                else
                {
                    ModelState.AddModelError("", "You can upload only .jpeg, .jpg and .png");
                    ViewBag.Category = _context.NewsCategories.ToList();
                    ViewBag.Tags = _context.Tags.ToList();
                    return View(model);
                }
            }

            ViewBag.Category = _context.NewsCategories.ToList();
            ViewBag.Tags = _context.Tags.ToList();
            return View(model);
        }

        public IActionResult Update(int? id)
        {
            News model = _context.News.Include(tb => tb.TagToNews).ThenInclude(t => t.Tag).FirstOrDefault(b => b.Id == id);
            model.TagToNewsId = _context.TagToNews.Where(tb => tb.NewsId == id).Select(a => a.TagId).ToList();
            ViewBag.Category = _context.NewsCategories.ToList();
            ViewBag.Tags = _context.Tags.ToList();
            return View(model);
        }

        [HttpPost]
        public IActionResult Update(News model)
        {
            if (ModelState.IsValid)
            {
                if (model.NewsImageFile != null)
                {
                    if (model.NewsImageFile.ContentType == "image/jpeg" || model.NewsImageFile.ContentType == "image/png")
                    {
                        if (model.NewsImageFile.Length <= 2097152)
                        {
                            //Delete old image
                            if (!string.IsNullOrEmpty(model.NewsImage))
                            {
                                string oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, "Uploads", model.NewsImage);
                                if (System.IO.File.Exists(oldImagePath))
                                {
                                    System.IO.File.Delete(oldImagePath);
                                }
                            }

                            //Create new image
                            string fileName = Guid.NewGuid() + "-" + DateTime.Now.ToString("yyyyMMddHHmmss") + "-" + model.NewsImageFile.FileName;
                            string filePath = Path.Combine(_webHostEnvironment.WebRootPath, "Uploads", fileName);
                            using (var stream = new FileStream(filePath, FileMode.Create))
                            {
                                model.NewsImageFile.CopyTo(stream);
                            }

                            model.NewsImage = fileName;
                        }
                        else
                        {
                            ModelState.AddModelError("", "You can upload only less than 2 mb.");
                            ViewBag.Category = _context.NewsCategories.ToList();
                            ViewBag.Tags = _context.Tags.ToList();
                            return View(model);
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "You can upload only .jpeg, .jpg and .png");
                        ViewBag.Category = _context.NewsCategories.ToList();
                        ViewBag.Tags = _context.Tags.ToList();
                        return View(model);
                    }
                }


                _context.News.Update(model);
                _context.SaveChanges();

                //Delete old data
                List<TagToNews> tagToNews = _context.TagToNews.Where(tb => tb.TagId == model.Id).ToList();
                foreach (var item in tagToNews)
                {
                    _context.TagToNews.Remove(item);
                }
                _context.SaveChanges();

                //Create new Tag to blog
                if (model.TagToNewsId != null && model.TagToNewsId.Count > 0)
                {
                    foreach (var item in model.TagToNewsId)
                    {
                        TagToNews tagTonews = new TagToNews();
                        tagTonews.TagId = item;
                        tagTonews.NewsId = model.Id;
                        _context.TagToNews.Add(tagTonews);
                    }

                    _context.SaveChanges();
                }
                return RedirectToAction("Index");

            }

            ViewBag.Category = _context.NewsCategories.ToList();
            ViewBag.Tags = _context.Tags.ToList();
            return View(model);
        }

        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                HttpContext.Session.SetString("NullIdError", "Id can not be null");
                return RedirectToAction("Index");
            }

            News news = _context.News.Find(id);
            if (news == null)
            {
                HttpContext.Session.SetString("NullDataError", "Can not found the data");
                return RedirectToAction("Index");
            }




            if (!string.IsNullOrEmpty(news.NewsImage))
            {
                string oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, "Uploads", news.NewsImage);
                if (System.IO.File.Exists(oldImagePath))
                {
                    System.IO.File.Delete(oldImagePath);
                }
            }

            //List<TagToBlog> tagToBlogs = _context.TagToBlogs.Where(t=>t.BlogId==id).ToList();
            //foreach (var item in tagToBlogs)
            //{
            //    _context.TagToBlogs.Remove(item);
            //}
            //_context.SaveChanges();

            _context.News.Remove(news);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
