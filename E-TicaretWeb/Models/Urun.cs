namespace E_TicaretWeb.Models
{
    public class Urun
    {
        public int ID { get; set; }
        public string UrunAdi { get; set; }
        public string ResimUrl { get; set; }
        public decimal Fiyat { get; set; }
        public int Stok { get; set; }
    }
}
