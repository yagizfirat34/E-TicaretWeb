using Dapper;
using E_TicaretWeb.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace E_TicaretWeb.Controllers
{
    [Authorize]
    public class SepetController : Controller
    {
        string baglanti = "Server=-;Database=E-Ticaret;Trusted_Connection=True;TrustServerCertificate=True;";

        public IActionResult List()
        {
            using var sqlbaglantı = new SqlConnection(baglanti);
            string query = @"SELECT s.ID, s.KullaniciID, k.AdSoyad, s.Durum 
                           FROM Sepetler s
                           INNER JOIN Kullanicilar k ON k.ID = s.KullaniciID";
            List<SepetViewModel> sepetler = sqlbaglantı.Query<SepetViewModel>(query).ToList();

            return View(sepetler);
        }

        [HttpGet]
        public IActionResult Create()
        {
            using var sqlbaglantı = new SqlConnection(baglanti);
            ViewBag.Kullanicilar = sqlbaglantı.Query<Kullanicı>("SELECT * FROM Kullanicilar").ToList();
            return View();
        }

        [HttpPost]
        public IActionResult Create(Sepet sepet)
        {
            using var sqlbaglantı = new SqlConnection(baglanti);
            string query = "INSERT INTO Sepetler (KullaniciID, Durum) VALUES (@KullaniciID, @Durum)";
            sqlbaglantı.Execute(query, sepet);

            return RedirectToAction("List");
        }

        [HttpGet]
        public IActionResult Update(int id)
        {
            using var sqlbaglantı = new SqlConnection(baglanti);
            Sepet sepet = sqlbaglantı.QueryFirstOrDefault<Sepet>("SELECT * FROM Sepetler WHERE ID = @id", new { id });

            if (sepet == null)
            {
                return RedirectToAction("List");
            }

            ViewBag.Kullanicilar = sqlbaglantı.Query<Kullanicı>("SELECT * FROM Kullanicilar").ToList();
            return View(sepet);
        }

        [HttpPost]
        public IActionResult Update(Sepet sepet)
        {
            using var sqlbaglantı = new SqlConnection(baglanti);
            string query = "UPDATE Sepetler SET KullaniciID = @KullaniciID, Durum = @Durum WHERE ID = @ID";
            sqlbaglantı.Execute(query, sepet);

            return RedirectToAction("List");
        }

        public IActionResult Delete(int id)
        {
            using var sqlbaglantı = new SqlConnection(baglanti);
            string query = "DELETE FROM Sepetler WHERE ID = @id";
            sqlbaglantı.Execute(query, new { id });

            return RedirectToAction("List");
        }
    }
}
