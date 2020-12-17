using System;
using System.Collections.Generic;
using System.Linq;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Diagnostics;

namespace Datos.Clases.SmartChat
{
   public class ctlChat
    {
        private SMLUtilidades.clsChat miclsChat;

        public ctlChat()
        {
            miclsChat = new SMLUtilidades.clsChat();
        }


        public JObject  verusuarioypass(JObject js)
        {
            string telefono = js.SelectToken("telefono").ToString();
            string pass = js.SelectToken("password").ToString();

           SMLUtilidades.CHAT.USUARIOSCHAT usuariologado= miclsChat.grabarUsuarioYPass(telefono, SMLUtilidades.Encrypt.SHA512(pass));

            JObject salida = new JObject();
            salida.Add("TELEFONO", usuariologado.TELEFONO);
            salida.Add("ID", usuariologado.ID);

            return salida;


        }


        public JObject salirgrupo(JObject js)
        {
            string telefono = js.SelectToken("telefono").ToString();
            string grupo = js.SelectToken("grupo").ToString();

            String nombregrupo=miclsChat.SalirGrupo(telefono, grupo);

            JObject salida = new JObject();
           
            salida.Add("GRUPO", nombregrupo);

            return salida;
        }




        public JObject hacerlogin(JObject js)
        {
            String telefono=js.SelectToken("telefono").ToString();
            string pass = js.SelectToken("password").ToString();

            JObject salida = new JObject();


           SMLUtilidades.clsLogin  milogin= new SMLUtilidades.clsLogin(SMLUtilidades.Principal.GetConexion());

           SMLUtilidades.USUARIOS usuariologado=milogin.Login(telefono, pass, "SCHAT");


           SMLUtilidades.CHAT.USUARIOSCHAT usuario = miclsChat.BuscarUsuario(usuariologado.EMAIL);


            if (usuario == null)
            {

                /*    salida.Add("TELEFONO", usuariologado.EMAIL);
                    salida.Add("ID", usuariologado.IDUSUARIO);
                    salida.Add("NOMBRE", usuariologado.NOMBRE);
                    salida.Add("TOKEN", "");*/



                SMLUtilidades.CHAT.USUARIOSCHAT usuariograbado = miclsChat.GrabarUsuario(usuariologado.EMAIL, usuariologado.NOMBRE, usuariologado.EMAIL);


                salida.Add("TELEFONO", usuariograbado.TELEFONO);
                salida.Add("NOMBRE", usuariograbado.NOMBRE);
                salida.Add("TOKEN", usuariograbado.TOKEN);
                salida.Add("ID", usuariograbado.ID);
                salida.Add("RUTA", "");

            }
            else
            {
                salida.Add("TELEFONO", usuario.TELEFONO);
                salida.Add("TOKEN", usuario.TOKEN);
                salida.Add("NOMBRE", usuario.NOMBRE);

                salida.Add("ID", usuario.ID);

                SMLUtilidades.CHAT.ARCHIVOS archivo = miclsChat.buscarfoto(usuario.ID);

                if (archivo != null)
                {
                    salida.Add("RUTA", archivo.RUTA);
                }
                else
                {
                    salida.Add("RUTA", "");
                }
            }

            return salida;

        }



        public JObject buscarcontactosweb(JObject js)
        {
            SMLUtilidades.clsLogin milogin = new SMLUtilidades.clsLogin(SMLUtilidades.Principal.GetConexion());

         //   List<SMLUtilidades.USUARIOS> listado= milogin.BuscarUsuariosChat();

            JArray miembros = new JArray();
            miembros = JArray.Parse(JsonConvert.SerializeObject(milogin.BuscarUsuariosChat().Select(m => new
            {
                m.NOMBRE,
                m.EMAIL,
            
            })));

            JObject json = new JObject();

            json.Add("MIEMBROS", miembros);

            return json;


        }


            public JObject crearUsuario(JObject js)
        {


            JObject salida = new JObject();

            string telefono= js.SelectToken("telefono").ToString();
            string nombre = js.SelectToken("nombre").ToString();
            string token = js.SelectToken("token").ToString();


            SMLUtilidades.CHAT.USUARIOSCHAT usuariograbado = miclsChat.GrabarUsuario(telefono, nombre, token);


            salida.Add("telefono", usuariograbado.TELEFONO);
            salida.Add("nombre", usuariograbado.NOMBRE);
            salida.Add("token", usuariograbado.TOKEN);
            salida.Add("id", usuariograbado.ID);


            return salida;

        }


        public JObject anadiramigo(JObject js)
        {

            JObject salida = new JObject();

            string emailamigo = js.SelectToken("emailamigo").ToString();
            string idpropietario = js.SelectToken("idpropietario").ToString();


            miclsChat.AnadirAmigo(emailamigo, idpropietario);

            salida.Add("AÑADIDO", "SI");

            return salida;

        }



        public JObject mostraramigos(JObject js)
        {

            JObject salida = new JObject();

            string idpropietario = js.SelectToken("idpropietario").ToString();


            JArray miembros = new JArray();
            miembros = JArray.Parse(JsonConvert.SerializeObject(miclsChat.MostrarAmigos(idpropietario).Select(m => new
            {
                m.NOMBRE,
                m.TELEFONO,
                m.EMAIL,
                m.TOKEN,
                m.ID,
                RUTA = miclsChat.buscarfotoPorID(m.ID),
                MENSAJESSINLEER = miclsChat.MensajesNoLeidos(idpropietario, m.ID.ToString()).Count().ToString(),
                CODIGO = miclsChat.SaberCodigoChat(idpropietario, m.ID.ToString())!=null ? miclsChat.SaberCodigoChat(idpropietario, m.ID.ToString()).CHATSCHAT.CODIGO : "",
                INICIO = miclsChat.SaberCodigoChat(idpropietario, m.ID.ToString()) != null ? miclsChat.SaberCodigoChat(idpropietario, m.ID.ToString()).CHATSCHAT.INICIO.ToString() : "Chat No Iniciado"



            })));



            salida.Add("MIEMBROS", miembros);

            return salida;

        }

        public JObject ponercomoleidos(JObject js)
        {
            JObject salida = new JObject();

            string idpropietario = js.SelectToken("idpropietario").ToString();
            string codigochat = js.SelectToken("codigochat").ToString();

            miclsChat.ponercomoleidos(idpropietario, codigochat);


            salida.Add("Leidos", "Si");

            return salida;

        }



        public JObject ponercomoleidosUsuario(JObject js)
        {
            JObject salida = new JObject();

            string idpropietario = js.SelectToken("idpropietario").ToString();
   

            miclsChat.PonerComoLeidosUsuario(idpropietario);


            salida.Add("Leidos", "Si");

            return salida;

        }






        public JObject crearUsuarioconemail(JObject js)
        {


            JObject salida = new JObject();

            string email = js.SelectToken("email").ToString();
            string nombre = js.SelectToken("nombre").ToString();
            string token=js.SelectToken("token").ToString();


            SMLUtilidades.CHAT.USUARIOSCHAT usuariograbado = miclsChat.GrabarUsuario(email, nombre, token);


            salida.Add("telefono", usuariograbado.EMAIL);
            salida.Add("nombre", usuariograbado.NOMBRE);
            salida.Add("token", usuariograbado.TOKEN);
            salida.Add("id", usuariograbado.ID);


            return salida;

        }


        //buscarfoto

        public JObject buscarfoto(JObject js)
        {

            string id = js.SelectToken("ID").ToString();
            JObject salida = new JObject();

            SMLUtilidades.CHAT.ARCHIVOS archivo = miclsChat.buscarfoto(id);

            if (archivo !=null)
            {
                salida.Add("RUTA", archivo.RUTA);
            }else
            {
                salida.Add("RUTA", "");
            }

           



      /*      foreach (var archivo in miclsChat.buscarfoto(id))
            {
                if (archivo.MENSAJEID == null)
                {
                    salida.Add("RUTA", archivo.RUTA);
                    break;
                }
            }*/


            return salida;
        }


        public JObject grabarTipoArchivo(JObject js)
        {
            String tipo = js.SelectToken("TIPO").ToString();

            SMLUtilidades.CHAT.TIPOS_ARCHIVO tipoarchivo = miclsChat.GrabarTipoArchivo(tipo);

            JObject salida = new JObject();

            salida.Add("ID", tipoarchivo.ID);
            salida.Add("TIPO", tipoarchivo.TIPO);
            return salida;

        }



        
        public JObject almacenarimagen(JObject js)
        {
            String id = js.SelectToken("ID").ToString();

            String imagen = js.SelectToken("IMAGEN").ToString();

            String extension = js.SelectToken("EXTENSION").ToString();


            String dia = js.SelectToken("DIA").ToString();


            String chat_id = js.SelectToken("CHAT_ID").ToString();

            String usuarioemisor = js.SelectToken("EMISOR").ToString();

            String receptor = js.SelectToken("RECEPTOR").ToString();

            int idusuario=Int32.Parse(id);

        //    JObject grabartipo = new JObject();
        //    grabartipo.Add("TIPO", extension);

         //   grabarTipoArchivo(grabartipo);



         //   SMLUtilidades.Rutas.getRutaImagenesSmartChat(idusuario);

            SMLUtilidades.clsArchivos migestionarchivo = new SMLUtilidades.clsArchivos();

      
            String ruta = SMLUtilidades.Rutas.getRutaImagenesSmartChat(idusuario).ToString();
            String nombrearchivo = "";

            if (chat_id.Length>0 && dia.Length>0)
            {

                nombrearchivo = chat_id + "_" + id + "_" + dia.Replace(":", "").Replace(" ", "-") + "." + extension;
            }else
            {
                nombrearchivo = id + "." + extension;
            }

            

            migestionarchivo.subirArchivo(ruta, imagen, nombrearchivo);


            String codificado = imagen.Split(',')[1];


            Byte[] bytes = Convert.FromBase64String(codificado);

            String hex = BitConverter.ToString(bytes);

            miclsChat.GrabarArchivoBBDD(extension, dia, chat_id, usuarioemisor, receptor, null, ruta+nombrearchivo);


            JObject salida = new JObject();
            salida.Add("GRABADO", id);

            salida.Add("RUTA", ruta + nombrearchivo);

            return salida;
        }



        public JObject anadirusuarioagrupo(JObject js)
        {

            string telefono = js.SelectToken("telefono").ToString();
            string grupo= js.SelectToken("grupo").ToString();

           if (miclsChat.AnadirUsuarioAGrupo(telefono, grupo))
            {
                return js;
            }else
            {
                return null;
            }


            
        }



        public JObject crearChat(JObject js)
        {
         
            string codigo = js.SelectToken("codigo").ToString();
            string inicio = js.SelectToken("inicio").ToString();

            miclsChat.CrearChat(codigo, inicio);

       
            return js;

        }



        public JObject crearGrupo(JObject js)
        {
 
            string nombre = js.SelectToken("nombre").ToString();



            SMLUtilidades.CHAT.GRUPOSCHAT grupochat=miclsChat.CrearGrupo(nombre);

            JObject salida = new JObject();
            salida.Add("ID", grupochat.ID);
            salida.Add("NOMBRE", grupochat.NOMBRE);

            return salida;

        }



        public JObject buscarGrupoPorID(JObject js)
        {
            JObject salida = new JObject();

            String id = js.SelectToken("ID").ToString();

            SMLUtilidades.CHAT.GRUPOSCHAT migrupo = miclsChat.BuscarGrupoPorID(id);


            JArray miembros = new JArray();
            miembros = JArray.Parse(JsonConvert.SerializeObject(miclsChat.BuscarUsuariosGrupo(id).Select(m => new
            {
                m.NOMBRE,
                m.TELEFONO,
                m.TOKEN,
                m.ID,
                RUTA =miclsChat.buscarfotoPorID(m.ID)

            })));



        //miclsChat.buscarfoto(m.ID).GetType()!=null ? miclsChat.buscarfoto(m.ID).RUTA : ""

            salida.Add("ID", migrupo.ID);
            salida.Add("NOMBRE", migrupo.NOMBRE);
            salida.Add("MIEMBROS", miembros);

            return salida;

        }


        public JObject buscarUsuarioConEmail(JObject js)
        {
            JObject salida = new JObject();

            String telefono = js.SelectToken("telefono").ToString();

            SMLUtilidades.CHAT.USUARIOSCHAT usuario = miclsChat.BuscarUsuario(telefono);

            salida.Add("TELEFONO", usuario.TELEFONO);
            salida.Add("TOKEN", usuario.TOKEN);
            salida.Add("NOMBRE", usuario.NOMBRE);

            salida.Add("ID", usuario.ID);

            SMLUtilidades.CHAT.ARCHIVOS archivo = miclsChat.buscarfoto(usuario.ID);

            if (archivo != null)
            {
                salida.Add("RUTA", archivo.RUTA);
            }
            else
            {
                salida.Add("RUTA", "");
            }


            return salida;

        }






            public JObject buscarUsuario(JObject js)
        {
            JObject salida = new JObject();

            String telefono = js.SelectToken("telefono").ToString();


            String idpropietario = js.SelectToken("id").ToString();

        
            SMLUtilidades.CHAT.USUARIOSCHAT usuario = miclsChat.BuscarUsuario(telefono);

            salida.Add("TELEFONO", usuario.TELEFONO);
            salida.Add("TOKEN", usuario.TOKEN);
            salida.Add("NOMBRE" , usuario.NOMBRE);

            salida.Add("ID", usuario.ID);



            SMLUtilidades.CHAT.ARCHIVOS archivo = miclsChat.buscarfoto(usuario.ID);

            if (archivo != null)
            {
                salida.Add("RUTA", archivo.RUTA);
            }
            else
            {
                salida.Add("RUTA", "");
            }





            if (Int32.Parse(idpropietario) == usuario.ID)
            {
                salida.Add("MENSAJES", 0);
                salida.Add("ULTIMOCHAT", "");

            }else
            {






                List<SMLUtilidades.CHAT.MENSAJES> mensajesnoleidos = miclsChat.BuscarMensajesNoLeidos(idpropietario, usuario.ID);


                List<SMLUtilidades.CHAT.MENSAJES> mensajesnoleidosfiltrados = new List<SMLUtilidades.CHAT.MENSAJES>();






                mensajesnoleidos.ForEach(mensaje =>
                {

                    SMLUtilidades.CHAT.GRUPOSCHAT gruposupuesto = miclsChat.BuscarGrupoPorID(mensaje.CHATSCHAT.CODIGO);



                    if (gruposupuesto == null)
                    {
                        mensajesnoleidosfiltrados.Add(mensaje);
                    }


                });


                //DameChatBuscarMensajesLeidos(Idpropietario, Idcompañero)




                salida.Add("MENSAJES", mensajesnoleidosfiltrados.Count);

                if (mensajesnoleidosfiltrados.Count > 0)
                {
                    salida.Add("ULTIMOCHAT", mensajesnoleidosfiltrados.Last().CHATSCHAT.CODIGO);

                }
                else if (mensajesnoleidosfiltrados.Count == 0 && miclsChat.DameChatBuscarMensajesLeidos(idpropietario, usuario.ID).Length > 0)
                {
                    salida.Add("ULTIMOCHAT", miclsChat.DameChatBuscarMensajesLeidos(idpropietario, usuario.ID));
                }
                else
                {
                    salida.Add("ULTIMOCHAT", "");
                }
            }



     


            /*    var jsonconvertido = JsonConvert.SerializeObject(miclsChat.BuscarUsuario(telefono), Formatting.Indented,
                          new JsonSerializerSettings
                          {
                              ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                          });

                    JObject mijobject= JsonConvert.DeserializeObject<dynamic>(jsonconvertido);

                String Telefono= mijobject.SelectToken("TELEFONO").ToString();

                String Token = mijobject.SelectToken("TOKEN").ToString();

                String Nombre = mijobject.SelectToken("NOMBRE").ToString();*/

            /*    salida.Add("TELEFONO", Telefono);
                salida.Add("TOKEN", Token);
                salida.Add("NOMBRE", Nombre);*/

            //   return JsonConvert.DeserializeObject<dynamic>(jsonconvertido);

            return salida;



            /*    miusuario.SelectToken("TELEFONO");

                JObject json = new JObject();
                json.Add("telefono", miusuario.SelectToken("TELEFONO"));
                return json;*/

        }


        public JObject ponerMensajesComoLeidos(JObject js)
        {
            JObject salida = new JObject();

            String chatid = js.SelectToken("chatid").ToString();

            String idowner = js.SelectToken("id").ToString();

            miclsChat.marcarComoLeidos(chatid, idowner);

            salida.Add("leidos", "si");

            return salida;


        }



        public JObject listadochats(JObject js)
        {


            String telefono = js.SelectToken("telefono").ToString();

            JObject json = new JObject();

            JArray jarray = new JArray();


            var jsonconvertido = JsonConvert.SerializeObject(miclsChat.MostrarChatsUsuario(telefono), Formatting.Indented,
                        new JsonSerializerSettings
                {
                     ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                    });


            jarray = JsonConvert.DeserializeObject<dynamic>(jsonconvertido);

           int numero= jarray.GroupBy(i => i["CHATID"]).Count();


            //  json.Add("chats", jarray);

            json.Add("numero", jarray);

            return json;

        }



        public JObject noleidosresumen(JObject js)
        {
            String idpropietario = js.SelectToken("idpropietario").ToString();

            JObject json = new JObject();


            JArray mensajesnoleidos = new JArray();
            mensajesnoleidos = JArray.Parse(JsonConvert.SerializeObject(miclsChat.MensajesNoLeidosResumen(idpropietario).Select(m => new
            {
                m.CONTENIDO,
                m.CHATSCHAT.CODIGO,
                m.DIA,
                m.USUARIOSCHAT2.ID,
                m.USUARIOSCHAT2.TELEFONO,
                m.USUARIOSCHAT2.NOMBRE,
                m.USUARIOSCHAT2.EMAIL,
                FOTO = miclsChat.buscarfoto(m.USUARIOSCHAT2.ID) == null ? "" : JsonConvert.SerializeObject(miclsChat.buscarfoto(m.USUARIOSCHAT2.ID).RUTA),

                ARCHIVOS = JArray.Parse(JsonConvert.SerializeObject(miclsChat.BuscarArchivosEnElMensaje(m.ID).Select(n => new
                {
                    n.RUTA
                }))),

               AMIGO = miclsChat.esamigo(idpropietario, m.USUARIOSCHAT2.ID.ToString())

        })));

            json.Add("mensajesnoleidos", mensajesnoleidos);



      

            //     AMIGO=miclsChat.esamigo(idpropietario, m.USUARIOSCHAT2.ID.ToString())


            return json;

        }



        public JObject misGrupos(JObject js)
        {
            String telefono = js.SelectToken("telefono").ToString();

            List<SMLUtilidades.CHAT.GRUPOSCHAT> grupos = new List<SMLUtilidades.CHAT.GRUPOSCHAT>();

            JObject json = new JObject();

            JArray jarray = new JArray();



            miclsChat.MostrarGruposUsuario(telefono).ForEach(item =>
            {
                if (!grupos.Contains(item.GRUPOSCHAT))
                {
                    grupos.Add(item.GRUPOSCHAT);
                    
                }

            });


         

            JArray MIEMBROS = new JArray();




            jarray = JArray.Parse(JsonConvert.SerializeObject(grupos.Select(m => new
            {
                m.ID,
                m.NOMBRE,
                MENSAJESSINLEER = miclsChat.BuscarMensajesNoLeidosGrupo(telefono, m.ID),

                MIEMBROS = JArray.Parse(JsonConvert.SerializeObject(miclsChat.BuscarUsuariosGrupo(m.ID).Select(n => new
                {
                    n.TELEFONO,
                    n.TOKEN,
                    n.NOMBRE,
                    n.ID,
                    RUTA = miclsChat.buscarfoto(n.ID) == null ? "" : JsonConvert.SerializeObject(miclsChat.buscarfoto(n.ID).RUTA)


                })))

            }))); ;

            //miclsChat.buscarfoto(mensajes.First().USUARIOSCHAT1.ID)==null ? "": JsonConvert.SerializeObject(miclsChat.buscarfoto(mensajes.First().USUARIOSCHAT1.ID))

            json.Add("GRUPOS", jarray);
      

            return json;

        }



        public JObject miembrosGrupo(JObject js)
        {

            String id = js.SelectToken("ID").ToString();
            JArray miembros = new JArray();
            miembros = JArray.Parse(JsonConvert.SerializeObject(miclsChat.BuscarUsuariosGrupo(id).Select(m => new
            {
                m.NOMBRE,
                m.TELEFONO,
                m.TOKEN

            })));

            JObject json = new JObject();

            json.Add("MIEMBROS", miembros);

            return json;

        }




        public JObject detallesmischats(JObject js)
        {

            String telefono = js.SelectToken("telefono").ToString();

            JObject json = new JObject();

            JArray jarray = new JArray();

            List<SMLUtilidades.CHAT.MENSAJES> mensajes = miclsChat.MostrarChatsUsuario(telefono);

            List<SMLUtilidades.CHAT.MENSAJES> mensajesfiltrados = new List<SMLUtilidades.CHAT.MENSAJES>();


            foreach (var mensaje in mensajes)
            {
                if (!(miclsChat.BuscarGrupoPorID(mensaje.CHATSCHAT.CODIGO) is SMLUtilidades.CHAT.GRUPOSCHAT))
                {
                    mensajesfiltrados.Add(mensaje);
                }
            }

            List<SMLUtilidades.CHAT.CHATSCHAT> chats = new List<SMLUtilidades.CHAT.CHATSCHAT>();


            mensajesfiltrados.ForEach(mensaje =>
            {
                if (!chats.Contains(mensaje.CHATSCHAT))
                {
                    chats.Add(mensaje.CHATSCHAT);

                }

            });



            //   JObject usuarios = new JObject();

            //  usuarios.Add("usuario1", mensajes.First().USUARIOSCHAT1.TELEFONO);
            //  usuarios.Add("usuario2", mensajes.First().USUARIOSCHAT2.TELEFONO);


            List<SMLUtilidades.CHAT.MENSAJES> mensajesr = miclsChat.MostrarChatsUsuarioRecibiendo(telefono);
            List<SMLUtilidades.CHAT.MENSAJES> mensajesfiltradosr = new List<SMLUtilidades.CHAT.MENSAJES>();
            foreach (var mensajerr in mensajesr)
            {
                if (!(miclsChat.BuscarGrupoPorID(mensajerr.CHATSCHAT.CODIGO) is SMLUtilidades.CHAT.GRUPOSCHAT))
                {
                    mensajesfiltradosr.Add(mensajerr);
                }
            }

            mensajesfiltradosr.ForEach(mensajef =>
            {
                if (!chats.Contains(mensajef.CHATSCHAT))
                {
                    chats.Add(mensajef.CHATSCHAT);

                }

            });



                jarray = JArray.Parse(JsonConvert.SerializeObject(chats.Select(m => new
                {
                    m.CODIGO,
                    m.INICIO,
                    mensajes.First().USUARIOSCHAT1.TELEFONO,
                    mensajes.First().USUARIOSCHAT1.NOMBRE,
                    mensajes.First().USUARIOSCHAT1.TOKEN,
                    mensajes.First().USUARIOSCHAT1.ID,
                    RUTA = miclsChat.buscarfoto(mensajes.First().USUARIOSCHAT1.ID) == null ?  "" :  
                    JsonConvert.SerializeObject(miclsChat.buscarfoto(mensajes.First().USUARIOSCHAT1.ID).RUTA),
              

                })));





         /*   jarray = JArray.Parse(JsonConvert.SerializeObject(chats.Select(m => new
            {
                m.CODIGO,
                m.INICIO,
                TELEFONO = mensajes.First().USUARIOSCHAT1.TELEFONO == null ? miclsChat.MostrarMensajesChat(m.CODIGO).First().USUARIOSCHAT2.TELEFONO : mensajes.First().USUARIOSCHAT1.TELEFONO,
                NOMBRE = mensajes.First().USUARIOSCHAT1.NOMBRE == null ? miclsChat.MostrarMensajesChat(m.CODIGO).First().USUARIOSCHAT2.NOMBRE : mensajes.First().USUARIOSCHAT1.NOMBRE,
                TOKEN = mensajes.First().USUARIOSCHAT1.TOKEN == null ? miclsChat.MostrarMensajesChat(m.CODIGO).First().USUARIOSCHAT2.TOKEN : mensajes.First().USUARIOSCHAT1.TOKEN,
                ID = mensajes.First().USUARIOSCHAT1.ID == null ? miclsChat.MostrarMensajesChat(m.CODIGO).First().USUARIOSCHAT2.ID : mensajes.First().USUARIOSCHAT1.ID,
                RUTA = miclsChat.buscarfoto(mensajes.First().USUARIOSCHAT1.ID) == null ? "" :
                JsonConvert.SerializeObject(miclsChat.buscarfoto(mensajes.First().USUARIOSCHAT1.ID).RUTA),

            })));*/

            json.Add("chats", jarray);


      

            return json;

        }


        public JObject crearLocalizacion(JObject js)
        {
            String latitud = js.SelectToken("latitud").ToString();
            String longitud = js.SelectToken("longitud").ToString();


            String mensajeid = js.SelectToken("mensajeid").ToString();

           SMLUtilidades.CHAT.LOCALIZACIONES milocalizacion= miclsChat.grabarlocalizacion(latitud, longitud, mensajeid);


            JObject json = new JObject();

            json.Add("ID", milocalizacion.ID);

            json.Add("LATITUD", milocalizacion.LATITUD);

            json.Add("LONGITUD", milocalizacion.LONGITUD);


            json.Add("MENSAJEID", milocalizacion.MENSAJEID);


            return json;
        }




        public JObject crearSMS(JObject js)
        {

            JObject json = new JObject();

            JArray jarray = new JArray();

            String destinatario = js.SelectToken("destinatario").ToString();
            String texto= js.SelectToken("texto").ToString();

            var misms = SMLUtilidades.SMS.EnviarSMS(destinatario, texto, "SmartChat");

            Console.WriteLine("sms "+misms);



            json.Add("sms", misms.ToString());



            return json;

        }



        public JObject buscarMensajes(JObject js)
        {
        

            String codigo = js.SelectToken("codigo").ToString();

            JObject json = new JObject();

            JArray jarray = new JArray();

            var datos = miclsChat.MostrarMensajesChat(codigo);

         
            jarray =JArray.Parse(JsonConvert.SerializeObject(datos.Select(m=> new
            {
                
                m.CHATSCHAT.CODIGO,
                m.DIA,
                m.CONTENIDO,
                m.USUARIOSCHAT2.TELEFONO,
                m.USUARIOSCHAT2.NOMBRE,
                FOTO= miclsChat.buscarfoto(m.USUARIOSCHAT2.ID)==null ? "" : miclsChat.buscarfoto(m.USUARIOSCHAT2.ID).RUTA,



                ARCHIVOS = JArray.Parse(JsonConvert.SerializeObject(miclsChat.BuscarArchivosEnElMensaje(m.ID).Select(n => new
                {
                    n.RUTA
                }))),
                LOCALIZACION= JArray.Parse(JsonConvert.SerializeObject(miclsChat.BuscaLocalizacionEnElMensaje(m.ID).Select(l=> new
                {
                    l.LATITUD, l.LONGITUD

                })))

                //   ARCHIVOS = JArray.Parse(JsonConvert.SerializeObject(miclsChat.BuscarArchivosEnElMensaje(m.ID))

            })));

            json.Add("mensajes", jarray);

            return json;

        }






        public JObject crearMensaje(JObject js)
        {
           
            string contenido = js.SelectToken("contenido").ToString();
            string dia = js.SelectToken("dia").ToString();
            string usuarioid = js.SelectToken("usuarioid").ToString();
            string chatid = js.SelectToken("chatid").ToString();
            string usuarioreceptor = js.SelectToken("idusuariorecepcion").ToString();

            SMLUtilidades.CHAT.MENSAJES mensajecreado=miclsChat.CrearMensaje(contenido, dia, usuarioid, chatid, usuarioreceptor);


            js.Add("ID", mensajecreado.ID);


         
            return js;

        }





    }
}
