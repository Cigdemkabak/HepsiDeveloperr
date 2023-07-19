using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HepsiDeveloper.Data;
using HepsiDeveloper.Models;
using System.IO;
using Microsoft.AspNetCore.Authorization;
using HepsiDeveloper.Data.Entities;

namespace HepsiDeveloper.Controllers
{
    public class UrunsController : Controller
    {
        
            private readonly ApplicationDbContext _context;
            private readonly IWebHostEnvironment _hostEnvironment;
            private string _dosyaYolu;

            public UrunsController(ApplicationDbContext context, IWebHostEnvironment hostEnvironment)
            {
                _context = context;
                _hostEnvironment = hostEnvironment;

                _dosyaYolu = Path.Combine(_hostEnvironment.WebRootPath, "resimler");

                if (Directory.Exists(_dosyaYolu))
                {
                    Directory.CreateDirectory(_dosyaYolu);
                }
            }


        // GET: Uruns
        public async Task<IActionResult> Index()
        {
            return _context.Urunler != null ?
                        View(await _context.Urunler.Include(x => x.Resimler).ToListAsync()) :
                        Problem("Entity set 'ApplicationDbContext.Urun'  is null.");
        }

        // GET: Uruns/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var urun = await _context.Set<Urun>()
                .Include(x => x.Resimler)
                .SingleOrDefaultAsync(m => m.Id == id);

            if (urun == null)
            {
                return NotFound();
            }

            return View(urun);
        }


        // GET: Uruns/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Uruns/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public async Task<IActionResult> Create([Bind("Id,Ad,Aciklama,Fiyat,Dosyalar")] Urun urun)
        {

     
            if (ModelState.IsValid)
            {
               


                foreach (var item in urun.Dosyalar)
                {
                    var tamDosyaAdi = Path.Combine(_dosyaYolu, item.FileName);

                    var dosyaDizini = Path.GetDirectoryName(tamDosyaAdi);
                    if (!Directory.Exists(dosyaDizini))
                    {
                        Directory.CreateDirectory(dosyaDizini);
                    }

                    using (var dosyaAkisi = new FileStream(tamDosyaAdi, FileMode.Create))
                    {
                        await item.CopyToAsync(dosyaAkisi);
                    }
                    urun.Resimler.Add(new Resim { DosyaAdi = item.FileName });


                }

                _context.Add(urun);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));



            }
            return View(urun);
        }
   

        // GET: Uruns/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Urunler == null)
            {
                return NotFound();
            }

            var urun = await _context.Urunler.Include(x => x.Resimler).SingleOrDefaultAsync(x => x.Id == id);

            if (urun == null)
            {
                return NotFound();
            }
            return View(urun);
        }


      

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Ad,Aciklama,Fiyat")] Urun urun)
        {
            if (id != urun.Id)
            {
                return NotFound();
            }

           

                try
                {
                    _context.Update(urun);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UrunExists(urun.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            
        }

        // POST: Uruns/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Urunler == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Urun'  is null.");
            }
            var urun = await _context.Urunler.Include(x => x.Resimler).SingleOrDefaultAsync(x => x.Id == id);
            if (urun != null)
            {
                _context.Urunler.Remove(urun);
            }

            await _context.SaveChangesAsync();

            foreach (var item in urun.Resimler)
            {
                System.IO.File.Delete(Path.Combine(_dosyaYolu, item.DosyaAdi));
            }

            return RedirectToAction(nameof(Index));
        }



        public async Task<IActionResult> ResimSil(int id)
        {

            var resim = await _context.Resimler.FindAsync(id);
            if (resim != null)
            {
                _context.Resimler.Remove(resim);
            }

            await _context.SaveChangesAsync();
            System.IO.File.Delete(Path.Combine(_dosyaYolu, resim.DosyaAdi));

            return RedirectToAction(nameof(Edit), new { id = resim.UrunuId });
        }

        private bool UrunExists(int id)
        {
            return (_context.Urunler?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}




