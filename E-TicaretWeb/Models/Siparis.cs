namespace E_TicaretWeb.Models
{
    public class Siparis
    {
        public int ID { get; set; }
        public int KullaniciID { get; set; }
        public int SepetID { get; set; }
        public decimal ToplamTutar { get; set; }
        public DateTime Tarih { get; set; }
    }
}
