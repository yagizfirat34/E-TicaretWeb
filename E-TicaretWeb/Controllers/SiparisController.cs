using Dapper;
using E_TicaretWeb.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace E_TicaretWeb.Controllers
{
    [Authorize]
    public class SiparisController : Controller
    {
        string baglanti = "Server=-;Database=E-Ticaret;Trusted_Connection=True;TrustServerCertificate=True;";
        public IActionResult List()
        {
            using var sqlbaglantı = new SqlConnection(baglanti);
            string query = @"SELECT s.ID, k.AdSoyad, s.SepetID, s.ToplamTutar, s.Tarih 
                           FROM Siparisler s
                           INNER JOIN Kullanicilar k ON k.ID = s.KullaniciID
                           INNER JOIN Sepetler se ON se.ID = s.SepetID";
            List<SiparisViewModel> siparisler = sqlbaglantı.Query<SiparisViewModel>(query).ToList();

            return View(siparisler);
        }

        [HttpGet]
        public IActionResult Create()
        {
            using var sqlbaglantı = new SqlConnection(baglanti);
            ViewBag.Kullanicilar = sqlbaglantı.Query<Kullanicı>("SELECT * FROM Kullanicilar").ToList();
            ViewBag.Sepetler = sqlbaglantı.Query<Sepet>("SELECT * FROM Sepetler").ToList();
            return View();
        }

        [HttpPost]
        public IActionResult Create(Siparis siparis)
        {
            using var sqlbaglantı = new SqlConnection(baglanti);
            string query = "INSERT INTO Siparisler (KullaniciID, SepetID, ToplamTutar, Tarih) VALUES (@KullaniciID, @SepetID, @ToplamTutar, @Tarih)";
            sqlbaglantı.Execute(query, siparis);

            return RedirectToAction("List");
        }

        [HttpGet]
        public IActionResult Update(int id)
        {
            using var sqlbaglantı = new SqlConnection(baglanti);
            Siparis siparis = sqlbaglantı.QueryFirstOrDefault<Siparis>("SELECT * FROM Siparisler WHERE ID = @id", new { id });

            if (siparis == null)
            {
                return RedirectToAction("List");
            }

            ViewBag.Kullanicilar = sqlbaglantı.Query<Kullanicı>("SELECT * FROM Kullanicilar").ToList();
            ViewBag.Sepetler = sqlbaglantı.Query<Sepet>("SELECT * FROM Sepetler").ToList();
            return View(siparis);
        }

        [HttpPost]
        public IActionResult Update(Siparis siparis)
        {
            using var sqlbaglantı = new SqlConnection(baglanti);
            string query = "UPDATE Siparisler SET KullaniciID = @KullaniciID, SepetID = @SepetID, ToplamTutar = @ToplamTutar, Tarih = @Tarih WHERE ID = @ID";
            sqlbaglantı.Execute(query, siparis);

            return RedirectToAction("List");
        }

        public IActionResult Delete(int id)
        {
            using var sqlbaglantı = new SqlConnection(baglanti);
            string query = "DELETE FROM Siparisler WHERE ID = @id";
            sqlbaglantı.Execute(query, new { id });

            return RedirectToAction("List");
        }
    }
}
