using FI.AtividadeEntrevista.BLL;
using WebAtividadeEntrevista.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FI.AtividadeEntrevista.DML;
using System.Data.SqlClient;
using Microsoft.Ajax.Utilities;
using System.Reflection;

namespace WebAtividadeEntrevista.Controllers
{
    public class BeneficiarioController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult CarregarIdCliente(long idCliente)
        {
            List<BeneficiarioModel> listaBeneficiarios = (List<BeneficiarioModel>)Session["Beneficiarios"];
            foreach (var beneficiario in listaBeneficiarios)
            {
                beneficiario.IdCliente = idCliente;
            }

            return RedirectToAction("Incluir");
        }

        public ActionResult Incluir()
        {
            BoBeneficiario bo = new BoBeneficiario();

            List<BeneficiarioModel> listaModel = (List<BeneficiarioModel>)Session["Beneficiarios"];
            foreach (var beneficiario in listaModel)
            {
                beneficiario.Id = bo.Incluir(new Beneficiario()
                {
                    Nome = beneficiario.Nome,
                    CPF = new string(beneficiario.CPF.Where(char.IsDigit).ToArray()),
                    IdCliente = beneficiario.IdCliente
                });
            }

            return Json("Cadastro efetuado com sucesso", JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult Alterar(long idCliente)
        {
            List<BeneficiarioModel> models = Session["Beneficiarios"] as List<BeneficiarioModel>;

            if (models == null)
            {
                models = new List<BeneficiarioModel>();
            }

            BoBeneficiario bo = new BoBeneficiario();
            List<Beneficiario> beneficiario = bo.Pesquisar(idCliente);
            Models.BeneficiarioModel model = null;

            if (beneficiario != null)
            {
                foreach (var b in beneficiario)
                {
                    model = new BeneficiarioModel()
                    {
                        Id = b.Id,
                        Nome = b.Nome,
                        CPF = Convert.ToUInt64(b.CPF).ToString(@"000\.000\.000\-00"),
                        IdCliente = b.IdCliente
                    };
                    models.Add(model);
                    models = models.GroupBy(mod => mod.CPF).Select(g => g.First()).ToList();

                }
            }

            Session["Beneficiarios"] = models;
            return Json(new { models = models }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Armazenar(BeneficiarioModel model)
        {
            List<BeneficiarioModel> beneficiarios = Session["Beneficiarios"] as List<BeneficiarioModel>;

            if (beneficiarios == null)
            {
                beneficiarios = new List<BeneficiarioModel>();
            }

            BoBeneficiario bo = new BoBeneficiario();

            if (!this.ModelState.IsValid)
            {
                List<string> erros = (from item in ModelState.Values
                                      from error in item.Errors
                                      select error.ErrorMessage).ToList();

                Response.StatusCode = 400;
                return Json(string.Join(Environment.NewLine, erros));
            }
            else
            {
                if (VerificarExistenciaCPF(model.CPF, beneficiarios))
                {
                    Response.StatusCode = 400;
                    return Json(string.Join(Environment.NewLine, "CPF já cadastrado"));
                }
                else if (!bo.ValidarPadraoCPF(model.CPF))
                {
                    Response.StatusCode = 400;
                    return Json(string.Join(Environment.NewLine, "CPF inválido"));
                }
                else
                {
                    beneficiarios.Add(model);
                    Session["Beneficiarios"] = beneficiarios;
                    return Json(new { beneficiarios = beneficiarios }, JsonRequestBehavior.AllowGet);
                }
            }
        }

        [HttpPost]
        public ActionResult Remover(string CPF)
        {
            List<BeneficiarioModel> beneficiarios = Session["Beneficiarios"] as List<BeneficiarioModel>;

            if (beneficiarios == null)
            {
                beneficiarios = new List<BeneficiarioModel>();
            }

            var beneficiarioParaRemover = beneficiarios.Find(be => be.CPF == CPF);
            if (!(beneficiarioParaRemover == null))
            {
                beneficiarios.Remove(beneficiarioParaRemover);
            }

            Session["Beneficiarios"] = beneficiarios;
            return Json(new { beneficiarios = beneficiarios }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Excluir(long idCliente)
        {
            BoBeneficiario bo = new BoBeneficiario();

            List<Beneficiario> beneficiariosBD = new List<Beneficiario>();

            List<BeneficiarioModel> beneficiarios = Session["Beneficiarios"] as List<BeneficiarioModel>;

            foreach (var beneficiario in beneficiarios)
            {
                if (beneficiario.Id == 0)
                {
                    beneficiario.Id = bo.Incluir(new Beneficiario()
                    {
                        Nome = beneficiario.Nome,
                        CPF = new string(beneficiario.CPF.Where(char.IsDigit).ToArray()),
                        IdCliente = beneficiario.IdCliente
                    });
                }
                else
                {
                    bo.Alterar(new Beneficiario()
                    {
                        Nome = beneficiario.Nome,
                        CPF = new string(beneficiario.CPF.Where(char.IsDigit).ToArray()),
                        Id = beneficiario.Id
                    });
                }
            }

            beneficiariosBD = bo.Pesquisar(idCliente);

            foreach (var objeto in beneficiariosBD)
            {
                if (!beneficiarios.Any(o => o.Id == objeto.Id))
                {
                    bo.Excluir(objeto.Id);
                }
            }
            beneficiariosBD = bo.Pesquisar(idCliente);
            Session["Beneficiarios"] = beneficiariosBD;
            return Json(new { beneficiarios = beneficiarios }, JsonRequestBehavior.AllowGet);
        }

        public bool VerificarExistenciaCPF(string CPF, List<BeneficiarioModel> listaBeneficiarios)
        {
            return listaBeneficiarios.Any(ben => ben.CPF == CPF);
        }
    }
}