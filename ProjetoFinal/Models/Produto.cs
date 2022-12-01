using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ProjetoFinal.Models
{
    public class Produto
    {
        public int Id { get; set; }

        /// <summary>
        /// Código EAN (internacional) do produto.
        /// </summary>
        [StringLength(30), DisplayName("Código de Barras")]
        public string? Ean { get; set; }

        [StringLength(250)]
        public string Nome { get; set; } = null!;

        [DisplayName("Descrição")]
        public string? Descricao { get; set; }

        [DisplayName("Preço")]
        public float Preco { get; set; }

        [DisplayName("Estoque Atual")]
        public float EstoqueAtual { get; set; }
    }
}
