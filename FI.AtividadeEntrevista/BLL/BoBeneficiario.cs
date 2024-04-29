using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FI.AtividadeEntrevista.BLL
{
    public class BoBeneficiario
    {
        /// <summary>
        /// Inclui um novo beneficiario
        /// </summary>
        /// <param name="beneficiario">Objeto de beneficiario</param>
        public long Incluir(DML.Beneficiario beneficiario)
        {
            DAL.DaoBeneficiario ben = new DAL.DaoBeneficiario();
            return ben.Incluir(beneficiario);
        }

        /// <summary>
        /// Altera um beneficiario
        /// </summary>
        /// <param name="beneficiario">Objeto de beneficiario</param>
        public void Alterar(DML.Beneficiario beneficiario)
        {
            DAL.DaoBeneficiario ben = new DAL.DaoBeneficiario();
            ben.Alterar(beneficiario);
        }

        /// <summary>
        /// Consulta o beneficiario pelo id
        /// </summary>
        /// <param name="id">id do beneficiario</param>
        /// <returns></returns>
        public DML.Beneficiario Consultar(long id)
        {
            DAL.DaoBeneficiario ben = new DAL.DaoBeneficiario();
            return ben.Consultar(id);
        }

        /// <summary>
        /// Exclui o beneficiario pelo id
        /// </summary>
        /// <param name="id">id de beneficiario</param>
        public void Excluir(long id)
        {
            DAL.DaoBeneficiario ben = new DAL.DaoBeneficiario();
            ben.Excluir(id);
        }

        /// <summary>
        /// Lista os beneficiarios
        /// </summary>
        public List<DML.Beneficiario> Listar()
        {
            DAL.DaoBeneficiario ben = new DAL.DaoBeneficiario();
            return ben.Listar();
        }

        /// <summary>
        /// Consulta os beneficiarios pelo id do cliente
        /// </summary>
        /// <param name="id">id do cliente</param>
        /// <returns></returns>
        public List<DML.Beneficiario> Pesquisar(long id)
        {
            DAL.DaoBeneficiario ben = new DAL.DaoBeneficiario();
            return ben.Pesquisar(id);
        }

        /// <summary>
        /// VerificaExistencia
        /// </summary>
        /// <param name="CPF"></param>
        /// <returns></returns>
        public bool VerificarExistencia(string CPF, long idCliente)
        {
            DAL.DaoBeneficiario ben = new DAL.DaoBeneficiario();
            return ben.VerificarExistencia(CPF, idCliente);
        }

        /// <summary>
        /// ValidarPadraoCPF
        /// </summary>
        /// <param name="CPF"></param>
        /// <returns></returns>
        public bool ValidarPadraoCPF(string CPF)
        {
            DAL.DaoBeneficiario ben = new DAL.DaoBeneficiario();
            return ben.ValidarPadraoCPF(CPF);
        }
    }
}
