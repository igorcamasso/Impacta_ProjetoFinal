using ProjetoFinal.Data;
using ProjetoFinal.Models;

namespace ProjetoFinal.Repositorio
{
    public class VendaRepositorio : RepositorioBase<Venda, int>
    {
        private readonly EstoqueRepositorio _estoqueRepositorio;
        private readonly ProdutoRepositorio produtoRepositorio;

        public VendaRepositorio(ApplicationDbContext context, 
            EstoqueRepositorio estoqueRepositorio,
            ProdutoRepositorio produtoRepositorio) : base(context)
        {
            this._estoqueRepositorio = estoqueRepositorio;
            this.produtoRepositorio = produtoRepositorio;
        }

        public void Adicionar(Venda venda)
        {
            venda.ValorTotalProdutos = venda.Produtos.Sum(a => a.Preco);

			//Limpa produto
			var produtos = venda.Produtos;
			venda.Produtos = new List<Produto>();

            base.Adicionar(venda);

            foreach (var produto in produtos.GroupBy(a=>a.Id))
            {
                _estoqueRepositorio.SaidaEstoque(produto.Key, ENum.EEstoqueTipoLancamento.Venda, produto.Count(), venda.Id);
            }
        }
    }
}
