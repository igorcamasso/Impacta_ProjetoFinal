using FluentResults;
using ProjetoFinal.Data;
using ProjetoFinal.ENum;
using ProjetoFinal.Models;

namespace ProjetoFinal.Repositorio
{
    public class EstoqueRepositorio : RepositorioBase<Estoque, int>
    {
		private readonly ApplicationDbContext context;
		private readonly ProdutoRepositorio _produtoRepositorio;

		public EstoqueRepositorio(ApplicationDbContext context) : base(context)
        {
			this.context = context;
		}

        public new void Adicionar(Estoque estoque)
        {
            var estoqueAtual = EstoqueAtual(estoque.ProdutoId);
            estoque.SaldoAnterior = estoqueAtual;
            if (estoque.Operacao == ENum.EEstoqueOperacao.Entrada)
                estoque.SaldoNovo = estoqueAtual + estoque.Quantidade;
            else
                estoque.SaldoNovo = estoqueAtual - estoque.Quantidade;
            base.Adicionar(estoque);
        }

        public Result Atualizar(int id, Estoque estoque)
        {
            estoque.Id = id;
            estoque.TipoLancamento = EEstoqueTipoLancamento.Ajuste;
            Adicionar(estoque);
            return Result.Ok();
        }

        public float EstoqueAtual(int ProdutoId)
        {
            return Listar()
                .Where(a => a.ProdutoId == ProdutoId)
                .OrderByDescending(a => a.Data)
                .FirstOrDefault()?.SaldoNovo ?? 0;
        }

        public void AjusteEstoque(int produtoId, EEstoqueTipoLancamento tipo, float novoSaldo)
        {
            var estoqueAnterior = EstoqueAtual(produtoId);
            if (estoqueAnterior > novoSaldo)
                SaidaEstoque(produtoId, tipo, estoqueAnterior - novoSaldo);
            if (estoqueAnterior < novoSaldo)
                EntradaEstoque(produtoId, tipo, novoSaldo - estoqueAnterior);
        }

        public void EntradaEstoque(int produtoId, EEstoqueTipoLancamento tipo, float QtdEntrada)
        {
            var estoqueAtual = EstoqueAtual(produtoId);
            var estoque = new Estoque
            {
                Operacao = EEstoqueOperacao.Entrada,
                TipoLancamento = tipo,
                Quantidade = QtdEntrada,
                SaldoAnterior = estoqueAtual,
                SaldoNovo = estoqueAtual + QtdEntrada,
                ProdutoId = produtoId,
            };
			base.Adicionar(estoque);
			AtualizaEstoqueProduto(produtoId, estoque.SaldoNovo);
		}

        public void SaidaEstoque(int produtoId, EEstoqueTipoLancamento tipo, float QtdSaida, int? VendaId = null)
        {
            var estoqueAtual = EstoqueAtual(produtoId);
            var estoque = new Estoque
            {
                Operacao = EEstoqueOperacao.Saida,
                TipoLancamento = tipo,
                Quantidade = QtdSaida,
                SaldoAnterior = estoqueAtual,
                SaldoNovo = estoqueAtual - QtdSaida,
                ProdutoId = produtoId,
                VendaId = VendaId
            };
			base.Adicionar(estoque);
            AtualizaEstoqueProduto(produtoId, estoque.SaldoNovo);
		}

        private void AtualizaEstoqueProduto(int produtoId, float estoque)
        {
            var produto = context.Produto.Find(produtoId);
            if(produto != null)
            {
                produto.EstoqueAtual = estoque;
                context.SaveChanges();
			}
		}
    }
}
