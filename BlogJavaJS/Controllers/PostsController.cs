using BlogJavaJS.Data;
using BlogJavaJS.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlogJavaJS.Controllers
{
    public class PostsController : Controller
    {
        private readonly BlogDbContext _db;
        private readonly long _maxFileSize = 5 * 1024 * 1024; // 5MB
        private static readonly string[] _allowedExt = [".jpg", ".jpeg", ".png", ".gif", ".webp"];

        public PostsController(BlogDbContext db) => _db = db;

        // GET: /Posts
       public async Task<IActionResult> Index()
{
    var posts = await _db.Posts
        .Include(p => p.Images) // Load nhiều ảnh
        .OrderByDescending(p => p.CreatedAt)
        .ToListAsync();

    return View(posts);
}

        // GET: /Posts/Details/5  (+1 view)
        public async Task<IActionResult> Details(int id)
        {
            var post = await _db.Posts.FirstOrDefaultAsync(p => p.Id == id);
            if (post == null) return NotFound();

            post.Views += 1;
            await _db.SaveChangesAsync();

            return View(post);
        }

        // GET: /Posts/Create
        public IActionResult Create() => View(new Post());

        // POST: /Posts/Create (upload ảnh)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Post post)
        {
            if (!ModelState.IsValid) return View(post);

            post.ImageUrl = await SaveImage(post.ImageFile);
            post.CreatedAt = DateTime.UtcNow;

            _db.Posts.Add(post);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: /Posts/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var post = await _db.Posts.FindAsync(id);
            if (post == null) return NotFound();
            return View(post);
        }

        // POST: /Posts/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Post post)
        {
            if (id != post.Id) return NotFound();
            if (!ModelState.IsValid) return View(post);

            var existing = await _db.Posts.FindAsync(id);
            if (existing == null) return NotFound();

            existing.Title = post.Title;
            existing.Content = post.Content;
            existing.Tag = post.Tag;

            // Nếu có upload ảnh mới thì thay thế
            var newImageUrl = await SaveImage(post.ImageFile);
            if (!string.IsNullOrEmpty(newImageUrl))
            {
                existing.ImageUrl = newImageUrl;
            }

            _db.Update(existing);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: /Posts/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var post = await _db.Posts.FirstOrDefaultAsync(p => p.Id == id);
            if (post == null) return NotFound();
            return View(post);
        }

        // POST: /Posts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var post = await _db.Posts.FindAsync(id);
            if (post == null) return NotFound();

            _db.Posts.Remove(post);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // === Helper lưu ảnh ===
        private async Task<string?> SaveImage(IFormFile? file)
        {
            if (file is null || file.Length == 0) return null;

            if (file.Length > _maxFileSize)
            {
                ModelState.AddModelError("ImageFile", "Ảnh quá lớn (tối đa 5MB).");
                return null;
            }

            var ext = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (!_allowedExt.Contains(ext) || !file.ContentType.StartsWith("image/"))
            {
                ModelState.AddModelError("ImageFile", "Định dạng ảnh không hợp lệ (jpg, jpeg, png, gif, webp).");
                return null;
            }

            var uploads = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
            Directory.CreateDirectory(uploads);

            var fileName = $"{Guid.NewGuid():N}{ext}";
            var savePath = Path.Combine(uploads, fileName);

            using (var stream = new FileStream(savePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return $"/uploads/{fileName}";
        }
    }
}
