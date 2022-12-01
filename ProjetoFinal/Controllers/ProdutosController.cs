using Microsoft.AspNetCore.Mvc;
using ProjetoFinal.Models;
using ProjetoFinal.Repositorio;

namespace ProjetoFinal.Controllers
{
    public class ProdutosController : BaseController
    {
        private readonly ProdutoRepositorio _produtoRepositorio;

        public ProdutosController(ProdutoRepositorio produtoRepositorio)
        {
            this._produtoRepositorio = produtoRepositorio;
        }

        // GET: Produtos
        public IActionResult Index()
        {
              return View(_produtoRepositorio.Listar().ToList());
        }

        // GET: Produtos/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                SetErroMessagem("Produto não econtrado");
                return RedirectToAction(nameof(Index));
            }

            var produto = _produtoRepositorio.Buscar(id.Value);
            if (produto == null)
            {
                SetErroMessagem("Produto não econtrado");
                return RedirectToAction(nameof(Index));
            }

            return View(produto);
        }

        // GET: Produtos/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Produtos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Ean,Nome,Descricao,Preco,EstoqueAtual")] Produto produto)
        {
            if (ModelState.IsValid)
            {
                _produtoRepositorio.Adicionar(produto);
                return RedirectToAction(nameof(Index));
            }
            return View(produto);
        }

        // GET: Produtos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                SetErroMessagem("Produto não econtrado");
                return RedirectToAction(nameof(Index));
            }

            var produto = _produtoRepositorio.Buscar(id.Value);
            if (produto == null)
            {
                SetErroMessagem("Produto não econtrado");
                return RedirectToAction(nameof(Index));
            }
            return View(produto);
        }

        // POST: Produtos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Ean,Nome,Descricao,Preco,EstoqueAtual")] Produto produto)
        {
            if (id != produto.Id)
            {
                RedirectToAction(nameof(Index));
            }

            if (ModelState.IsValid)
            {
                var result = _produtoRepositorio.Atualizar(id, produto);
                if (result.IsFailed)
                {
                    SetErroMessagem(result.Errors.First().Message);
                    return RedirectToAction(nameof(Edit), new { id });
                }
                return RedirectToAction(nameof(Index));
            }

            return View(produto);
        }

        // GET: Produtos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if(id == null)
            {
                SetErroMessagem("Produto não encontrado.");
                return RedirectToAction(nameof(Index));
            }

            var produto = _produtoRepositorio.Buscar(id.Value);
            return View(produto);
        }

        // POST: Produtos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var result = _produtoRepositorio.Deletar(id);

            if (result.IsFailed)
                SetErroMessagem(result.Errors.First().Message);
            else
                SetSucessoMessagem("Produto removido com sucesso.");

            return RedirectToAction(nameof(Index));
        }
    }
}
