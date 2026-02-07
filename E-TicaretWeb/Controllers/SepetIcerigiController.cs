using Dapper;
using E_TicaretWeb.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace E_TicaretWeb.Controllers
{
    [Authorize]
    public class SepetIcerigiController : Controller
    {
        string baglanti = "Server=-;Database=E-Ticaret;Trusted_Connection=True;TrustServerCertificate=True;";
        public IActionResult List(int? sepetId)
        {
            using var sqlbaglantı = new SqlConnection(baglanti);
            string query = @"SELECT se.ID, se.SepetID, se.UrunID, u.UrunAdi, u.ResimUrl, u.Fiyat, se.Adet, k.AdSoyad as KullaniciAdi
                           FROM SepetIcerigi se
                           INNER JOIN Sepetler s ON s.ID = se.SepetID
                           INNER JOIN Urunler u ON u.ID = se.UrunID
                           INNER JOIN Kullanicilar k ON k.ID = s.KullaniciID";

            if (sepetId.HasValue)
            {
                query += " WHERE se.SepetID = @sepetId";
                var sepetIcerikleri = sqlbaglantı.Query<SepetIcerigiViewModel>(query, new { sepetId }).ToList();
                ViewBag.SepetID = sepetId.Value;
                return View(sepetIcerikleri);
            }
            else
            {
                var sepetIcerikleri = sqlbaglantı.Query<SepetIcerigiViewModel>(query).ToList();
                return View(sepetIcerikleri);
            }
        }

        [HttpGet]
        public IActionResult Create(int? sepetId)
        {
            using var sqlbaglantı = new SqlConnection(baglanti);
            ViewBag.Sepetler = sqlbaglantı.Query<Sepet>("SELECT * FROM Sepetler").ToList();
            ViewBag.Urunler = sqlbaglantı.Query<Urun>("SELECT * FROM Urunler").ToList();
            
            if (sepetId.HasValue)
            {
                ViewBag.SelectedSepetID = sepetId.Value;
            }
            
            return View();
        }

        [HttpPost]
        public IActionResult Create(SepetIcerigi sepetIcerigi)
        {
            using var sqlbaglantı = new SqlConnection(baglanti);
            string query = "INSERT INTO SepetIcerigi (SepetID, UrunID, Adet) VALUES (@SepetID, @UrunID, @Adet)";
            sqlbaglantı.Execute(query, sepetIcerigi);

            return RedirectToAction("List", new { sepetId = sepetIcerigi.SepetID });
        }

        [HttpGet]
        public IActionResult Update(int id)
        {
            using var sqlbaglantı = new SqlConnection(baglanti);
            SepetIcerigi sepetIcerigi = sqlbaglantı.QueryFirstOrDefault<SepetIcerigi>("SELECT * FROM SepetIcerigi WHERE ID = @id", new { id });

            if (sepetIcerigi == null)
            {
                return RedirectToAction("List");
            }

            ViewBag.Sepetler = sqlbaglantı.Query<Sepet>("SELECT * FROM Sepetler").ToList();
            ViewBag.Urunler = sqlbaglantı.Query<Urun>("SELECT * FROM Urunler").ToList();
            return View(sepetIcerigi);
        }

        [HttpPost]
        public IActionResult Update(SepetIcerigi sepetIcerigi)
        {
            using var sqlbaglantı = new SqlConnection(baglanti);
            string query = "UPDATE SepetIcerigi SET SepetID = @SepetID, UrunID = @UrunID, Adet = @Adet WHERE ID = @ID";
            sqlbaglantı.Execute(query, sepetIcerigi);

            return RedirectToAction("List", new { sepetId = sepetIcerigi.SepetID });
        }

        public IActionResult Delete(int id)
        {
            using var sqlbaglantı = new SqlConnection(baglanti);
            
            var sepetIcerigi = sqlbaglantı.QueryFirstOrDefault<SepetIcerigi>("SELECT * FROM SepetIcerigi WHERE ID = @id", new { id });
            int? sepetId = sepetIcerigi?.SepetID;
            
            string query = "DELETE FROM SepetIcerigi WHERE ID = @id";
            sqlbaglantı.Execute(query, new { id });

            if (sepetId.HasValue)
            {
                return RedirectToAction("List", new { sepetId });
            }
            
            return RedirectToAction("List");
        }
    }
}
