using FluentResults;
using ProjetoFinal.Data;
using ProjetoFinal.Models;

namespace ProjetoFinal.Repositorio
{
    public class ProdutoRepositorio : RepositorioBase<Produto, int>
    {
        private readonly EstoqueRepositorio _estoqueRepositorio;

        public ProdutoRepositorio(ApplicationDbContext context, EstoqueRepositorio estoqueRepositorio) : base(context)
        {
            this._estoqueRepositorio = estoqueRepositorio;
        }

        public new void Adicionar(Produto produto)
        {
            base.Adicionar(produto);

            //Lanca uma nova entrada no estoque.
            _estoqueRepositorio.EntradaEstoque(produto.Id, ENum.EEstoqueTipoLancamento.Inicial, produto.EstoqueAtual);
        }

        public new Result Atualizar(int id, Produto novoProduto)
        {
            //Atualiza o produto.
            var result = base.Atualizar(id, novoProduto);
            if (result.IsFailed) return result;

            //Ajusta o estoque.
            _estoqueRepositorio.AjusteEstoque(id, ENum.EEstoqueTipoLancamento.Ajuste, novoProduto.EstoqueAtual);

            return Result.Ok();
        }

        internal Produto? Buscar(string codigo)
        {
            return Listar().FirstOrDefault(a => a.Ean == codigo);
        }
    }
}
