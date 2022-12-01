using FluentResults;

namespace ProjetoFinal.Repositorio
{
    public interface IRepositorio<Model, PrimaryKey>
    {
        /// <summary>
        /// Lista os itens do modelo.
        /// </summary>
        /// <returns>Lista não executada (query) dos itens do modelo.</returns>
        IQueryable<Model> Listar();

        /// <summary>
        /// Busca um item.
        /// </summary>
        /// <param name="Id">Id do item</param>
        /// <returns></returns>
        Model? Buscar(PrimaryKey Id);

        /// <summary>
        /// Adiciona um item ao banco de dados.
        /// </summary>
        /// <param name="Item">Item a ser adicionado.</param>
        void Adicionar(Model Item);

        /// <summary>
        /// Atualiza um item ao banco de dados.
        /// </summary>
        /// <param name="Item">Item a ser atualizado.</param>
        Result Atualizar(PrimaryKey Id, Model Item);

        /// <summary>
        /// Deleta um item do banco de dados.
        /// </summary>
        /// <param name="Id">Id do item.</param>
        /// <returns>Verdadeiro se deletado ou falso em caso de erro.</returns>
        Result Deletar(PrimaryKey Id);
    }
}
