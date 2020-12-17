using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using System.Web.Http.Cors;

namespace SmartAPI.Controllers.SmartChat
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [RoutePrefix("api/smartchat")]
    public class SmartChatController : BaseController
    {

        Datos.Clases.SmartChat.ctlChat miclschat = new Datos.Clases.SmartChat.ctlChat();



           [AcceptVerbs("POST")]
          [Route("crearusuario")]
          [HttpPost]
          public JObject nuevoUsuario(HttpRequestMessage request)
           {
               Console.WriteLine(request);
         
               return getResponse(request, miclschat.crearUsuario);
           }

        [AcceptVerbs("POST")]
        [Route("crearusuarioconemail")]
        [HttpPost]
        public JObject nuevoUsuarioconemail(HttpRequestMessage request)
        {
            Console.WriteLine(request);

            return getResponse(request, miclschat.crearUsuarioconemail);
        }


        //noleidosresumen

        [AcceptVerbs("POST")]
        [Route("noleidosresumen")]
        [HttpPost]
        public JObject noleidosresumen(HttpRequestMessage request)
        {
            Console.WriteLine(request);

            return getResponse(request, miclschat.noleidosresumen);
        }





        [AcceptVerbs("POST")]
        [Route("ponercomoleidos")]
        [HttpPost]
        public JObject ponercomoleidos(HttpRequestMessage request)
        {
            Console.WriteLine(request);

            return getResponse(request, miclschat.ponercomoleidos);
        }




        [AcceptVerbs("POST")]
        [Route("ponercomoleidosusuario")]
        [HttpPost]
        public JObject ponercomoleidosusuario(HttpRequestMessage request)
        {
            Console.WriteLine(request);

            return getResponse(request, miclschat.ponercomoleidosUsuario);
        }




        [AcceptVerbs("POST")]
        [Route("anadiramigo")]
        [HttpPost]
        public JObject anadiramigo(HttpRequestMessage request)
        {
            Console.WriteLine(request);

            return getResponse(request, miclschat.anadiramigo);
        }



        [AcceptVerbs("POST")]
        [Route("mostraramigos")]
        [HttpPost]
        public JObject mostaramigos(HttpRequestMessage request)
        {
            Console.WriteLine(request);

            return getResponse(request, miclschat.mostraramigos);
        }






        //verusuarioypass

        [AcceptVerbs("POST")]
        [Route("buscarcontactosweb")]
        [HttpPost]
        public JObject buscarcontactosweb(HttpRequestMessage request)
        {
            Console.WriteLine(request);

            return getResponse(request, miclschat.buscarcontactosweb);
        }














        [AcceptVerbs("POST")]
        [Route("verusuarioypass")]
        [HttpPost]
        public JObject verusuarioypass(HttpRequestMessage request)
        {
            Console.WriteLine(request);

            return getResponse(request, miclschat.verusuarioypass);
        }




        [AcceptVerbs("POST")]
        [Route("hacerlogin")]
        [HttpPost]
        public JObject hacerlogin(HttpRequestMessage request)
        {
            Console.WriteLine(request);

            return getResponse(request, miclschat.hacerlogin);
        }



        [AcceptVerbs("POST")]
        [Route("salirgrupo")]
        [HttpPost]
        public JObject salirgrupo(HttpRequestMessage request)
        {
            Console.WriteLine(request);

            return getResponse(request, miclschat.salirgrupo);
        }



        [AcceptVerbs("POST")]
        [Route("creargrupo")]
        [HttpPost]
        public JObject crearGrupo(HttpRequestMessage request)
        {
            Console.WriteLine(request);
            return getResponse(request, miclschat.crearGrupo);
        }


        [AcceptVerbs("POST")]
        [Route("buscarfoto")]
        [HttpPost]
        public JObject buscarfoto(HttpRequestMessage request)
        {
            Console.WriteLine(request);
            return getResponse(request, miclschat.buscarfoto);
        }





        [AcceptVerbs("POST")]
        [Route("anadirusuarioagrupo")]
        [HttpPost]
        public JObject anadirusuarioagrupo(HttpRequestMessage request)
        {
          
            return getResponse(request, miclschat.anadirusuarioagrupo);
        }


        // buscarGrupoPorID
        [AcceptVerbs("POST")]
        [Route("buscarGrupoPorID")]
        [HttpPost]
        public JObject buscarGrupoPorID(HttpRequestMessage request)
        {

            return getResponse(request, miclschat.buscarGrupoPorID);
        }


        [AcceptVerbs("POST")]
        [Route("miembrosgrupo")]
        [HttpPost]
        public JObject miembrosgrupo(HttpRequestMessage request)
        {

            return getResponse(request, miclschat.miembrosGrupo);
        }



        [AcceptVerbs("POST")]
        [Route("crearchat")]
        [HttpPost]
        public JObject crearChat(HttpRequestMessage request)
        {
            Console.WriteLine(request);
            return getResponse(request, miclschat.crearChat);
        }




        [AcceptVerbs("POST")]
        [Route("buscarusuario")]
        [HttpPost]
        public JObject buscarUsuario(HttpRequestMessage request)
        {
            Console.WriteLine(request);
            return getResponse(request, miclschat.buscarUsuario);
        }



        [AcceptVerbs("POST")]
        [Route("buscarUsuarioConEmail")]
        [HttpPost]
        public JObject buscarUsuarioConEmail(HttpRequestMessage request)
        {
            Console.WriteLine(request);
            return getResponse(request, miclschat.buscarUsuarioConEmail);
        }




        [AcceptVerbs("POST")]
        [Route("crearmensaje")]
        [HttpPost]
        public JObject crearMensaje(HttpRequestMessage request)
        {
            Console.WriteLine(request);
            return getResponse(request, miclschat.crearMensaje);
        }



        [AcceptVerbs("POST")]
        [Route("buscarmensajeschat")]
        [HttpPost]
        public JObject buscarMensajes(HttpRequestMessage request)
        {
            Console.WriteLine(request);
            return getResponse(request, miclschat.buscarMensajes);
        }



        [AcceptVerbs("POST")]
        [Route("crearSMS")]
        [HttpPost]
        public JObject crearSMS(HttpRequestMessage request)
        {
            Console.WriteLine(request);
            return getResponse(request, miclschat.crearSMS);
        }



        [AcceptVerbs("POST")]
        [Route("crearLocalizacion")]
        [HttpPost]
        public JObject crearLocalizacion(HttpRequestMessage request)
        {
            Console.WriteLine(request);
            return getResponse(request, miclschat.crearLocalizacion);
        }






        [AcceptVerbs("POST")]
        [Route("listadochats")]
        [HttpPost]
        public JObject listadochats(HttpRequestMessage request)
        {
            Console.WriteLine(request);
            return getResponse(request, miclschat.listadochats);
        }



        [AcceptVerbs("POST")]
        [Route("detallesmischats")]
        [HttpPost]
        public JObject detallesmischats(HttpRequestMessage request)
        {
            Console.WriteLine(request);
            return getResponse(request, miclschat.detallesmischats);
        }



        [AcceptVerbs("POST")]
        [Route("almacenarimagen")]
        [HttpPost]
        public JObject almacenarimagen(HttpRequestMessage request)
        {
            Console.WriteLine(request);
            return getResponse(request, miclschat.almacenarimagen);
        }



        [AcceptVerbs("POST")]
        [Route("misgrupos")]
        [HttpPost]
        public JObject misgrupos(HttpRequestMessage request)
        {
            Console.WriteLine(request);
            return getResponse(request, miclschat.misGrupos);
        }



        [AcceptVerbs("POST")]
        [Route("crearTipoArchivo")]
        [HttpPost]
        public JObject crearTipoArchivo(HttpRequestMessage request)
        {
       
                Console.WriteLine(request);
                return getResponse(request, miclschat.grabarTipoArchivo);

        }



        [AcceptVerbs("POST")]
        [Route("ponerMensajesComoLeidos")]
        [HttpPost]
        public JObject ponerMensajesComoLeidos(HttpRequestMessage request)
        {

            Console.WriteLine(request);
            return getResponse(request, miclschat.ponerMensajesComoLeidos);

        }


    }
}
