using System.Collections.Generic;

namespace TCCBruno.Model
{
    public class Aluno
    {
        public int aluno_id { get; set; }
        public int pessoa_id { get; set; }
        public int instrutor_id { get; set; }
        public string data_nascimento { get; set; }

        public virtual Pessoa Pessoa { get; set; }
        public virtual List<Avaliacao_Fisica> Avaliacao_Fisica { get; set; }

    }

}