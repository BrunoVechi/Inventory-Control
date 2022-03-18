
namespace Spareparts
{
    public class Requisitar
    {
        public string Compras { get; set; } = "N";
        public string YG { get; set; }
        public string Descricao { get; set; }
        public string local;
        public int Min { get; set; }
        public int Atual { get; set; }
        public string Data { get; set; }
        public int ConsumoUTL { get; set; } = 0;
        public int ConsumoEXT { get; set; } = 0;


        public Requisitar(string DATA)
        {
            Data = DATA;
        }
    }
}
