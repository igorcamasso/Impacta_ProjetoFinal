using ProjetoFinal.ENum;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
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

        [DisplayName("Operação")]
        public EEstoqueOperacao Operacao { get; set; }

        [DisplayName("Tipo do Lançamento")]
        public EEstoqueTipoLancamento TipoLancamento { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm:ss}")]
        public DateTimeOffset Data { get; set; }

        public float Quantidade { get; set; }

        [DisplayName("Saldo Anterior")]
        public float SaldoAnterior { get; set; }

        [DisplayName("Saldo")]
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
