using Dapper;
using E_TicaretWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace E_TicaretWeb.Controllers
{
    public class RegisterController : Controller
    {
        string baglanti = "Server=-;Database=E-Ticaret;Trusted_Connection=True;TrustServerCertificate=True;";


        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Index(Kullanicı kullanici)
        {
           


            using var sqlbaglanti = new SqlConnection(baglanti);
            string query = "INSERT INTO Kullanicilar (AdSoyad, Eposta, Sifre) VALUES (@AdSoyad, @Eposta, @Sifre)";
            sqlbaglanti.Execute(query, kullanici);

            return RedirectToAction("Index", "Login");




           
        }

    }
}
