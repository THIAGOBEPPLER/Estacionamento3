using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Estacionamento3.Entities
{
    public class Patio
    {
        public int id { get; set; }
        public DateTime dataInicio { get; set; }
        public Nullable<DateTime> dataFim { get; set; }
        public Nullable<double> tempo { get; set; }
        public Nullable<float> valor { get; set; }

        public string veiculoPlaca { get; set; }
        public Veiculo veiculo { get; set; }
    }
}
