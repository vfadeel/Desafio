using PaschoalottoApi.Bll;
using PaschoalottoApi.Dal;
using PaschoalottoApi.Dto;
using PaschoalottoApi.Helpers;
using PaschoalottoApi.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Results;

namespace PaschoalottoApi.Controllers
{
    [RoutePrefix("titulo")]
    public class TituloController : ApiController
    {
        [Route("gettitulos")]
        public List<TitulosViewModel> GetTitulos()
        {

            List<tpTitulo> _lstTitulos = (new dbTitulo()).SelectMany<tpTitulo>("", true);

            return (new TitulosViewModel()).Mount(_lstTitulos);

        }

        [Route("adicionar")]
        [HttpPost]
        public IHttpActionResult Adicionar([FromBody]tpTitulo _tpTitulo)
        {

            IHttpActionResult _Out = Ok();

            try
            {

                (new Titulo()).Insert(_tpTitulo);

            }
            catch (Exception e)
            {
                _Out = Content(HttpStatusCode.BadRequest, e.Message);
            }

            return _Out;
        }

        [Route("gettitulo/{IdTitulo}")]
        [HttpGet]
        public tpTitulo GetTitulo(int IdTitulo)
        {


            try
            {

                return (tpTitulo)(new dbTitulo()).SelectById(IdTitulo, true, null);


            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

    }
}