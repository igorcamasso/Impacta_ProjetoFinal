using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjetoFinal.Data;
using ProjetoFinal.Repositorio;

namespace ProjetoFinal.Controllers
{
    public class EstoqueController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ProdutoRepositorio produtoRepositorio;

        public EstoqueController(ApplicationDbContext context, ProdutoRepositorio produtoRepositorio)
        {
            _context = context;
            this.produtoRepositorio = produtoRepositorio;
        }

        // GET: Estoque
        public async Task<IActionResult> Index(int id)
        {
            var produto = produtoRepositorio.Buscar(id);
            ViewBag.Produto = produto;
            var applicationDbContext = _context.Estoque.Where(a=>a.ProdutoId == id).Include(e => e.Venda);
            return View(await applicationDbContext.ToListAsync());
        }

    }
}
