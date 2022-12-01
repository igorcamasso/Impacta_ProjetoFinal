using ProjetoFinal.ENum;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjetoFinal.Models
{
    public class Estoque
    {
        public Estoque()
        {
            Data = DateTimeOffset.Now;
        }

        public int Id { get; set; }

        public int ProdutoId { get; set; }

        public EEstoqueOperacao Operacao { get; set; }

        public EEstoqueTipoLancamento TipoLancamento { get; set; }

        public DateTimeOffset Data { get; set; }

        public float Quantidade { get; set; }

        public float SaldoAnterior { get; set; }

        public float SaldoNovo { get; set; }


        /// <summary>
        /// Id da venda que gerou a saída.
        /// </summary>
        public int? VendaId { get; set; }

        #region Virtual

        /// <summary>
        /// Produto ao qual esse lançamento de estoque se refere.
        /// </summary>
        [ForeignKey(nameof(ProdutoId))]
        public virtual Produto Produto { get; set; }

        /// <summary>
        /// Quando a saída do estoque for por motivo de venda, registra a venda aqui.
        /// </summary>
        [ForeignKey(nameof(VendaId))]
        public virtual Venda? Venda { get; set; }

        #endregion

    }
}
