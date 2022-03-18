
namespace Spareparts
{
    public class Historico
    {
        public string Data { get; set; }
        public string YG { get; set; }
        public string Descricao { get; set; }
        public int Atual { get; set; }
        public string local;
        public int min;        

        public string consumo;

        public Historico(string DATA)
        {
           Data = DATA;
        }
    }
}
