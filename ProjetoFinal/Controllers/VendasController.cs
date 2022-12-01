using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using ProjetoFinal.Data;
using ProjetoFinal.Models;
using ProjetoFinal.Repositorio;

namespace ProjetoFinal.Controllers
{
    public class VendasController : BaseController
    {
        private readonly ApplicationDbContext _context;
        private readonly ProdutoRepositorio _produtoRepositorio;
        private readonly VendaRepositorio _vendaRepositorio;

        public VendasController(ApplicationDbContext context, ProdutoRepositorio produtoRepositorio, VendaRepositorio vendaRepositorio)
        {
            _context = context;
            this._produtoRepositorio = produtoRepositorio;
            this._vendaRepositorio = vendaRepositorio;
        }

        // GET: Vendas
        [Authorize]
        public async Task<IActionResult> Index()
        {
            if (User.Identity.IsAuthenticated)
                return View(await _context.Venda.ToListAsync());
            return RedirectToAction(nameof(Create));
        }

        // GET: Vendas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Venda == null)
            {
                return NotFound();
            }

            var venda = await _context.Venda
                .FirstOrDefaultAsync(m => m.Id == id);
            if (venda == null)
            {
                return NotFound();
            }

            return View(venda);
        }

        // GET: Vendas/Create
        public IActionResult Create()
        {
            var produtosId = GetProdutoSession();
            return View(produtosId);
        }

        public IActionResult FormaPagamento()
        {
            var produtosId = GetProdutoSession();
            var valorTotal = produtosId.Select(a => a.Qtd * a.ProdutoInfo?.Preco).Sum() ?? 0;
            var venda = new Venda
            {
                ValorTotalProdutos = valorTotal
            };

            return View(venda);
        }

        // POST: Vendas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ValorDesconto,FormaPagamento")] Venda venda)
        {
            if (ModelState.IsValid)
            {
                var produtos = GetProdutoSession();

                produtos.ForEach(a =>
                {
                    for (int i = 0; i < a.Qtd; i++)
                    {
                        venda.Produtos.Add(a.ProdutoInfo ?? new Produto { Id = a.Id });
                    }
                });

                _vendaRepositorio.Adicionar(venda);
                LimpaProdutosSession();
                SetSucessoMessagem("Venda efetuada");
                return RedirectToAction(nameof(Index));
            }
            SetErroMessagem("Verifique os campos e tente novamente");
            return RedirectToAction(nameof(FormaPagamento));
        }

        [HttpPost]
        public IActionResult AddProduto(string codigo, int qtd = 1)
        {
            var produto = _produtoRepositorio.Buscar(codigo);
            if (produto == null)
                SetErroMessagem("Produto não encontrado");
            else if(produto.EstoqueAtual <= qtd)
				SetErroMessagem("Produto sem estoque disponível");
			else
                AddProdutoSession(produto.Id, qtd);
            return RedirectToAction(nameof(Create));
        }

		[HttpPost]
		public IActionResult RemoverProduto(int produtoId)
		{
            RemoveProdutoSession(produtoId);
			return RedirectToAction(nameof(Create));
		}

		// GET: Vendas/Edit/5
		public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Venda == null)
            {
                return NotFound();
            }

            var venda = await _context.Venda.FindAsync(id);
            if (venda == null)
            {
                return NotFound();
            }
            return View(venda);
        }

        // POST: Vendas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Data,ValorTotalProdutos,ValorDesconto,ValorTotal,FormaPagamento")] Venda venda)
        {
            if (id != venda.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(venda);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VendaExists(venda.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(venda);
        }

        // GET: Vendas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Venda == null)
            {
                return NotFound();
            }

            var venda = await _context.Venda
                .FirstOrDefaultAsync(m => m.Id == id);
            if (venda == null)
            {
                return NotFound();
            }

            return View(venda);
        }

        // POST: Vendas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Venda == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Venda'  is null.");
            }
            var venda = await _context.Venda.FindAsync(id);
            if (venda != null)
            {
                _context.Venda.Remove(venda);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VendaExists(int id)
        {
            return _context.Venda.Any(e => e.Id == id);
        }

        private void AddProdutoSession(int produtoId, int qtd = 1)
        {
            List<ProdutoQtd> listaProdutoId = GetProdutoSession();
            var qtdPrd = listaProdutoId.FirstOrDefault(a => a.Id == produtoId);
            if (qtdPrd != null) qtdPrd.Qtd += qtd;
            else
            {
                var produto = _produtoRepositorio.Buscar(produtoId);
                listaProdutoId.Add(new ProdutoQtd { Id = produtoId, Qtd = qtd, ProdutoInfo = produto });
            }
            HttpContext.Session.SetString("produtoList", JsonConvert.SerializeObject(listaProdutoId));
        }

		private void RemoveProdutoSession(int produtoId)
		{
			List<ProdutoQtd> listaProdutoId = GetProdutoSession();
            listaProdutoId.RemoveAll(a => a.Id == produtoId);
			HttpContext.Session.SetString("produtoList", JsonConvert.SerializeObject(listaProdutoId));
		}

		private List<ProdutoQtd> GetProdutoSession()
        {
            var prdStr = HttpContext.Session.GetString("produtoList");
            return JsonConvert.DeserializeObject<List<ProdutoQtd>>(prdStr ?? "") ?? new();
        }

        private void LimpaProdutosSession()
        {
            HttpContext.Session.Remove("produtoList");
        }

        public class ProdutoQtd
        {
            public int Id { get; set; }
            public float Qtd { get; set; }
            public Produto? ProdutoInfo { get; set; }
        }
    }
}
