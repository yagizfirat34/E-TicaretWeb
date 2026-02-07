namespace E_TicaretWeb.Models
{
    public class SepetIcerigiViewModel
    {
        public int ID { get; set; }
        public int SepetID { get; set; }
        public int UrunID { get; set; }
        public string UrunAdi { get; set; }
        public string ResimUrl { get; set; }
        public decimal Fiyat { get; set; }
        public int Adet { get; set; }
        public string KullaniciAdi { get; set; }
    }
}
