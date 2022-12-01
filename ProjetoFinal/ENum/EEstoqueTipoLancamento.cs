namespace ProjetoFinal.ENum
{
    public enum EEstoqueTipoLancamento
    {
        /// <summary>
        /// Indica que o tipo de lançamento do estoque é inicial.
        /// </summary>
        Inicial = 0,
        /// <summary>
        /// Indica que é um ajuste de estoque.
        /// </summary>
        Ajuste = 1,
        /// <summary>
        /// Indica que é uma venda.
        /// </summary>
        Venda = 2,
        /// <summary>
        /// Indica que baixa proveniente de um roubo.
        /// </summary>
        Roubo = 3,
    }
}
