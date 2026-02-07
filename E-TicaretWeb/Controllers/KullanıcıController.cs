using Dapper;
using E_TicaretWeb.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace E_TicaretWeb.Controllers
{
    [Authorize]
    public class KullanıcıController : Controller
    {
        string baglanti = "Server=-;Database=E-Ticaret;Trusted_Connection=True;TrustServerCertificate=True;";



        public IActionResult List()
        {
            using var sqlbaglantı = new SqlConnection(baglanti);
            List<Kullanicı> Kullanıcılar = sqlbaglantı.Query<Kullanicı>("SELECT * FROM Kullanicilar").ToList();

            return View(Kullanıcılar);
      
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Kullanicı  kullanicı)
        {
            using var sqlbaglantı = new SqlConnection(baglanti);
            string query = "INSERT INTO Kullanicilar (AdSoyad, Eposta, Sifre) VALUES (@AdSoyad, @Eposta, @Sifre)";
            sqlbaglantı.Execute(query, kullanicı);

            return RedirectToAction("List");
        }
        public IActionResult Delete(int id)
        {
            using var sqlbaglantı = new SqlConnection(baglanti);
            string query = "DELETE FROM Kullanicilar WHERE ID = @id";
            sqlbaglantı.Execute(query, new { id });


            return RedirectToAction("List");
        }

        [HttpGet]
        public IActionResult Update(int id)
        {
            using var sqlbaglantı = new SqlConnection(baglanti);
            string query = "SELECT * FROM Kullanicilar WHERE Id = @id";
            var oyuncu = sqlbaglantı.QuerySingleOrDefault<Kullanicı>(query, new { id });
            return View(oyuncu);
        }

        [HttpPost]
        public IActionResult Update(Kullanicı kullanicı)
        {
            using var sqlbaglantı = new SqlConnection(baglanti);
            string query = "UPDATE Kullanicilar SET AdSoyad = @AdSoyad, Eposta = @Eposta, Sifre = @Sifre WHERE ID = @Id";
            sqlbaglantı.Execute(query, kullanicı);
            return RedirectToAction("List");
        }
    }
}
