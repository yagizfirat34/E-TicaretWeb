using Dapper;
using E_TicaretWeb.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace E_TicaretWeb.Controllers
{
    [Authorize]
    public class UrunController : Controller
    {
        string baglanti = "Server=-;Database=E-Ticaret;Trusted_Connection=True;TrustServerCertificate=True;";


        public IActionResult List()
        {
            using var sqlbaglantı = new SqlConnection(baglanti);
            List<Urun> Urunler = sqlbaglantı.Query<Urun>("SELECT * FROM Urunler").ToList();

            return View(Urunler);
        }


        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Urun urun, IFormFile ResimDosyasi)
        {
            if (ResimDosyasi != null && ResimDosyasi.Length > 0)
            {
                var dosyaAdi = Guid.NewGuid().ToString() + Path.GetExtension(ResimDosyasi.FileName);
                var dosyaYolu = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Images", dosyaAdi);

                using (var stream = new FileStream(dosyaYolu, FileMode.Create))
                {
                    await ResimDosyasi.CopyToAsync(stream);
                }

                urun.ResimUrl = "/Images/" + dosyaAdi;
            }

            using var sqlbaglantı = new SqlConnection(baglanti);
            string query = "INSERT INTO Urunler (UrunAdi, ResimUrl, Fiyat,Stok) VALUES (@UrunAdi, @ResimUrl, @Fiyat,@Stok)";
            sqlbaglantı.Execute(query, urun);

            return RedirectToAction("List");
        }

        [HttpGet]
        public IActionResult Update(int id)
        {
            using var sqlbaglantı = new SqlConnection(baglanti);
            Urun urun = sqlbaglantı.QueryFirstOrDefault<Urun>("SELECT * FROM Urunler WHERE ID = @id", new { id });

            if (urun == null)
            {
                return RedirectToAction("List");
            }

            return View(urun);
        }

        [HttpPost]
        public async Task<IActionResult> Update(Urun urun, IFormFile ResimDosyasi)
        {
            if (ResimDosyasi != null && ResimDosyasi.Length > 0)
            {
                var dosyaAdi = Guid.NewGuid().ToString() + Path.GetExtension(ResimDosyasi.FileName);
                var dosyaYolu = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Images", dosyaAdi);

                using (var stream = new FileStream(dosyaYolu, FileMode.Create))
                {
                    await ResimDosyasi.CopyToAsync(stream);
                }

                urun.ResimUrl = "/Images/" + dosyaAdi;
            }

            using var sqlbaglantı = new SqlConnection(baglanti);
            string query = "UPDATE Urunler SET UrunAdi = @UrunAdi, ResimUrl = @ResimUrl, Fiyat = @Fiyat, Stok = @Stok WHERE ID = @ID";
            sqlbaglantı.Execute(query, urun);

            return RedirectToAction("List");
        }

        public IActionResult Delete(int id)
        {
            using var sqlbaglantı = new SqlConnection(baglanti);
            string query = "DELETE FROM Urunler WHERE ID = @id";
            sqlbaglantı.Execute(query, new { id });


            return RedirectToAction("List");
        }


    }
}
