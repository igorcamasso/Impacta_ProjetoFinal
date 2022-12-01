using Microsoft.AspNetCore.Mvc;

namespace ProjetoFinal.Controllers
{
    public class BaseController : Controller
    {

        protected void SetSucessoMessagem(string mensagem)
        {
            TempData["SucessoMensagem"] = mensagem;
        }

        protected void SetErroMessagem(string mensagem)
        {
            TempData["ErroMensagem"] = mensagem;
        }
    }
}
