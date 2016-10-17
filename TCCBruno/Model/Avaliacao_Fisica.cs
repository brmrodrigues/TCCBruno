using System;

namespace TCCBruno.Model
{
    public class Avaliacao_Fisica
    {
        public int avaliacao_fisica_id { get; set; }
        public int aluno_id { get; set; }
        public decimal? peso { get; set; }
        public decimal? estatura { get; set; }
        public decimal? pressao_arterial { get; set; }
        public decimal? torax { get; set; }
        public decimal? cintura { get; set; }
        public decimal? abdome { get; set; }
        public decimal? braco_dir { get; set; }
        public decimal? braco_esq { get; set; }
        public decimal? antebr_dir { get; set; }
        public decimal? antebr_esq { get; set; }
        public decimal? quadril { get; set; }
        public string coxa_dir { get; set; }
        public string coxa_esq { get; set; }
        public decimal? perna_dir { get; set; }
        public decimal? perna_esq { get; set; }
        public decimal? ombro { get; set; }
        public decimal? tricipital { get; set; }
        public decimal? subescapular { get; set; }
        public decimal? suprailicia { get; set; }
        public decimal? abdominal { get; set; }
        public decimal? peitoral { get; set; }
        public decimal? axilar_media { get; set; }
        public decimal? coxa_medial { get; set; }
        public decimal? radio_u { get; set; }
        public decimal? femural { get; set; }
        public int? flex_geral1 { get; set; }
        public int? flex_geral2 { get; set; }
        public int? flex_geral3 { get; set; }
        public int? flex_braco { get; set; }
        public int? abdominais_rep { get; set; }

    }
}