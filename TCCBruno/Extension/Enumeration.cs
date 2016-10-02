using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvFisica
{
    public class Enumeration
    {
        public enum TipoClassificacao
        {
            ABDOMINAL,
            BRACO,
            GERAL
        }

        public enum ResultadoClassificacao
        {
            RUIM,
            ABAIXO_DA_MEDIA, 
            NA_MEDIA,
            ACIMA_DA_MEDIA,
            EXCELENTE
        }
    }
}
