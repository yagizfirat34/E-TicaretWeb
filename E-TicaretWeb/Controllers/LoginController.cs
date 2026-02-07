using Dapper;
using E_TicaretWeb.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Security.Claims;

namespace E_TicaretWeb.Controllers
{
    public class LoginController : Controller
    {
        string baglanti = "Server=-;Database=E-Ticaret;Trusted_Connection=True;TrustServerCertificate=True;";



        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(Kullanicı kullanicı)
        {
            using var sqlbaglanti = new SqlConnection(baglanti);
            string query = "SELECT * FROM Kullanicilar WHERE Eposta = @Eposta AND Sifre = @Sifre";

            var sonuc = sqlbaglanti.QueryFirstOrDefault<Kullanicı>(query, kullanicı);

            if (sonuc != null)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, sonuc.AdSoyad),
                    new Claim(ClaimTypes.Email, sonuc.Eposta)
                };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal);

                return RedirectToAction("List", "Kullanıcı");
            }
            else
            {
                ViewBag.Hata = "E-posta veya şifre hatalı!";
                return View();
            }
        }
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Login");
        }
    }
}
