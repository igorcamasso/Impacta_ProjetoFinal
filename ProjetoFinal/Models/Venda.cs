using ProjetoFinal.ENum;

namespace ProjetoFinal.Models
{
    public class Venda
    {
        public Venda()
        {
            Produtos = new List<Produto>();
            Data = DateTimeOffset.Now;
        }

        public int Id { get; set; }

        public DateTimeOffset Data { get; set; }

        /// <summary>
        /// Valor total da soma dos produtos.
        /// </summary>
        public float ValorTotalProdutos
        {
            get => _valorTotalProdutos;
            set
            {
                _valorTotalProdutos = value;
                CalcularValorTotal();
            }
        }

        /// <summary>
        /// Valor de desconto aplicado a venda.
        /// </summary>
        public float ValorDesconto { get => _valorDesconto; 
            set { 
                _valorDesconto = value;
                CalcularValorTotal();
            } 
        }

        /// <summary>
        /// Valor total da venda
        /// </summary>
        public float ValorTotal
        {
            get
            {
                if (_valorTotal == null) CalcularValorTotal();
                return _valorTotal!.Value;
            }
            private set { return; }
        }

        /// <summary>
        /// Forma de pagamento da venda.
        /// </summary>
        public EFormaPagamento FormaPagamento { get; set; }

        /// <summary>
        /// Lista de produtos vendidos.
        /// </summary>
        public virtual ICollection<Produto> Produtos { get; set; }

        #region Virtual

        /// <summary>
        /// Lançamento da saída do estoque.
        /// </summary>
        public virtual Estoque? SaidaEstoque { get; set; }

        #endregion

        #region Full Properties

        private float? _valorTotal;
        private float _valorTotalProdutos;
        private float _valorDesconto;

        #endregion

        #region Funções Auxiliares Internas

        /// <summary>
        /// Calcula a propriedade <see cref="ValorTotal"/>
        /// </summary>
        private void CalcularValorTotal()
        {
            _valorTotal = ValorTotalProdutos - ValorDesconto;
        }

        #endregion
    }
}
