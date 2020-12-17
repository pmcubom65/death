Imports System.Text
Imports Newtonsoft.Json
Imports SMLUtilidades.CHAT

Public Class clsChat

    Dim ctx As CHATDataContext = New CHATDataContext(Principal.GetConexionChat())



    Public Function AnadirUsuarioAGrupo(telefono, grupo) As Boolean

        Dim miusuario As CHAT.USUARIOSCHAT
        miusuario = BuscarUsuario(telefono)

        Dim migrupo As CHAT.GRUPOSCHAT
        migrupo = BuscarGrupo(grupo)

        If (miusuario Is Nothing Or migrupo Is Nothing) Then
            Return False
        Else
            Dim grupo_usuario As GRUPOSCHAT_USUARIO = New GRUPOSCHAT_USUARIO

            grupo_usuario.GRUPOSCHAT = migrupo
            grupo_usuario.USUARIOSCHAT = miusuario


            '   (From x In ctx. Where x.USUARIOID = idusuario Where x.MENSAJEID Is Nothing).FirstOrDefault()


            Dim relacion As CHAT.GRUPOSCHAT_USUARIO = (From x In ctx.GRUPOSCHAT_USUARIO Where x.USUARIOID = miusuario.ID Where x.GRUPOID = migrupo.ID).FirstOrDefault()


            If relacion Is Nothing Then
                ctx.GRUPOSCHAT_USUARIO.InsertOnSubmit(grupo_usuario)

                ctx.SubmitChanges()


            End If

            Return True


        End If


    End Function


    Public Function SalirGrupo(identificacion As String, idgrupo As String) As String

        Dim miusuario As CHAT.USUARIOSCHAT = BuscarUsuario(identificacion)

        Dim mirelacion As CHAT.GRUPOSCHAT_USUARIO = (From x In ctx.GRUPOSCHAT_USUARIO Where x.GRUPOID = idgrupo Where x.USUARIOID = miusuario.ID Select x).FirstOrDefault()

        Dim nombre As String = mirelacion.GRUPOSCHAT.NOMBRE

        ctx.GRUPOSCHAT_USUARIO.DeleteOnSubmit(mirelacion)

        ctx.SubmitChanges()

        Return nombre


    End Function







    Public Function MensajesNoLeidos(idpropietario As String, idamigo As String) As List(Of MENSAJES)



        Return (From x In ctx.MENSAJES Where x.USUARIOID = idamigo Where x.IDUSUARIORECEPCION = idpropietario Where x.LEIDO Is Nothing
                Where x.CHATSCHAT.CODIGO.Length > 10
                Select x).ToList()


    End Function



    Public Function PonerComoLeidosUsuario(idpropietario As String)



        Dim noleidosdelchat As List(Of CHAT.MENSAJES) = (From x In ctx.MENSAJES Where x.IDUSUARIORECEPCION = idpropietario Where x.LEIDO Is Nothing
                                                         Where x.CHATSCHAT.CODIGO.Length > 10 Select x).ToList()


        For Each mensaje As CHAT.MENSAJES In noleidosdelchat
            mensaje.LEIDO = 1

        Next

        ctx.SubmitChanges()

    End Function







    Public Function MensajesNoLeidosResumen(idpropietario As String) As List(Of MENSAJES)



        Return (From x In ctx.MENSAJES Where x.IDUSUARIORECEPCION = idpropietario Where x.LEIDO Is Nothing
                Where x.CHATSCHAT.CODIGO.Length > 10
                Select x).ToList()


    End Function


    Public Function SaberCodigoChat(idpropietario As String, idamigo As String) As CHAT.MENSAJES

        Dim ultimomensaje As CHAT.MENSAJES = (From x In ctx.MENSAJES Where x.USUARIOID = idamigo Where x.IDUSUARIORECEPCION = idpropietario
                                              Where x.CHATSCHAT.CODIGO.Length > 10 Select x).FirstOrDefault()

        If ultimomensaje Is Nothing Then

            Dim noultimomensaje As CHAT.MENSAJES = (From x In ctx.MENSAJES Where x.USUARIOID = idpropietario Where x.IDUSUARIORECEPCION = idamigo
                                                    Where x.CHATSCHAT.CODIGO.Length > 10 Select x).FirstOrDefault()

            Return noultimomensaje

        Else

            Return ultimomensaje

        End If


    End Function


    Public Function ponercomoleidos(idpropietario As String, codigochat As String) As List(Of CHAT.MENSAJES)

        Dim noleidosdelchat As List(Of CHAT.MENSAJES) = (From x In ctx.MENSAJES Where x.IDUSUARIORECEPCION = idpropietario Where x.CHATSCHAT.CODIGO = codigochat Where x.LEIDO Is Nothing Select x).ToList()

        For Each mensaje As CHAT.MENSAJES In noleidosdelchat
            mensaje.LEIDO = 1

        Next

        ctx.SubmitChanges()
    End Function



    Public Function AnadirAmigo(email As String, id As String)

        Dim Amigo As CHAT.USUARIOSCHAT = BuscarUsuario(email)

        Dim relacion As CHAT.AMIGOSCHAT = New CHAT.AMIGOSCHAT

        relacion.AMIGO = Amigo.ID

        relacion.PROPIETARIO = id

        Dim yaesamigo As CHAT.AMIGOSCHAT = (From x In ctx.AMIGOSCHAT Where x.PROPIETARIO = id Where x.AMIGO = Amigo.ID Select x).FirstOrDefault()

        If (yaesamigo Is Nothing And id <> Amigo.ID) Then

            ctx.AMIGOSCHAT.InsertOnSubmit(relacion)

            ctx.SubmitChanges()

        End If


    End Function




    Public Function MostrarAmigos(id As String) As List(Of CHAT.USUARIOSCHAT)


        Return (From x In ctx.AMIGOSCHAT Where x.PROPIETARIO = id Select x.USUARIOSCHAT1).ToList()


    End Function



    Public Function esamigo(idpropietario As String, idmiamigo As String) As String


        Dim miamigo As CHAT.AMIGOSCHAT = (From x In ctx.AMIGOSCHAT Where x.PROPIETARIO = idpropietario Where x.AMIGO = idmiamigo Select x).FirstOrDefault()

        If (miamigo Is Nothing) Then
            Return "NULL"
        Else
        Return "HAY AMIGO"
        End If


    End Function





    Public Function GrabarUsuario(telefono As String, nombre As String, token As String) As USUARIOSCHAT




        Dim usuarioestaono As CHAT.USUARIOSCHAT = BuscarUsuario(telefono)

        If (usuarioestaono IsNot Nothing) Then
            usuarioestaono.TOKEN = token
            usuarioestaono.NOMBRE = nombre

        Else

            usuarioestaono = New USUARIOSCHAT
            usuarioestaono.NOMBRE = nombre
            usuarioestaono.TELEFONO = telefono
            usuarioestaono.TOKEN = token

            If (telefono.Contains("@")) Then
                usuarioestaono.EMAIL = telefono
            Else
                usuarioestaono.EMAIL = ""
            End If

            ctx.USUARIOSCHAT.InsertOnSubmit(usuarioestaono)
            End If


            ctx.SubmitChanges()

        Return usuarioestaono



    End Function


    Public Function buscarfoto(id) As CHAT.ARCHIVOS
        'As List(Of CHAT.ARCHIVOS)


        Dim idusuario As String = id


        '  Return (From x In ctx.ARCHIVOS Where x.USUARIOID = idusuario Where x.MENSAJEID Is Nothing Select x).ToList()

        Return (From x In ctx.ARCHIVOS Where x.USUARIOID = idusuario Where x.MENSAJEID Is Nothing).FirstOrDefault()


    End Function



    Public Function buscarfotoPorID(id) As String
        'As List(Of CHAT.ARCHIVOS)


        Dim idusuario As String = id


        '  Return (From x In ctx.ARCHIVOS Where x.USUARIOID = idusuario Where x.MENSAJEID Is Nothing Select x).ToList()

        Dim misalida As CHAT.ARCHIVOS = (From x In ctx.ARCHIVOS Where x.USUARIOID = idusuario Where x.MENSAJEID Is Nothing Select x).FirstOrDefault()

        If (misalida Is Nothing) Then
            Return ""
        Else
            Return misalida.RUTA
        End If



    End Function



    Public Function comprobarUsuarioYPass(telefono As String, pass As String) As CHAT.USUARIOSCHAT

        Dim miusuarioypass = New CHAT.USUARIOS_PASSWORD
        Dim usuariopass As CHAT.USUARIOS_PASSWORD = New CHAT.USUARIOS_PASSWORD

        Dim miusuario As USUARIOSCHAT = BuscarUsuario(telefono)

        If (miusuario Is Nothing) Then
            Return Nothing

        Else
            usuariopass = (From x In ctx.USUARIOS_PASSWORD Where x.PASSWORDS = pass Where x.USUARIOID = miusuario.ID).FirstOrDefault()

            If (usuariopass Is Nothing) Then
                Return Nothing
            Else
                Return miusuario
            End If

        End If




    End Function






    Public Function grabarUsuarioYPass(telefono As String, pass As String) As CHAT.USUARIOSCHAT

        Dim miusuario As CHAT.USUARIOSCHAT = BuscarUsuario(telefono)

        Dim miusuarioypass = New CHAT.USUARIOS_PASSWORD

        miusuarioypass.USUARIOID = miusuario.ID

        miusuarioypass.PASSWORDS = pass

        ctx.USUARIOS_PASSWORD.InsertOnSubmit(miusuarioypass)

        ctx.SubmitChanges()

        Return miusuario



    End Function






    Public Function GrabarArchivoBBDD(tipoarchivo As String, dia As String, chat_id As String, usuarioemisor As String, usuarioreceptor As String, imagen As String, ruta As String) As CHAT.ARCHIVOS

        Dim mitipoarchivo As CHAT.TIPOS_ARCHIVO = GrabarTipoArchivo(tipoarchivo)

        Dim usuarioquerecive As CHAT.USUARIOSCHAT = BuscarUsuario(usuarioreceptor)

        Dim mimensaje As CHAT.MENSAJES = New CHAT.MENSAJES

        If usuarioquerecive IsNot Nothing Then

            mimensaje = CrearMensaje(Nothing, dia, usuarioemisor, chat_id, usuarioreceptor)

        Else
            mimensaje = Nothing

        End If


        Dim archivoagrabar As CHAT.ARCHIVOS = New CHAT.ARCHIVOS

        archivoagrabar.TIPOID = mitipoarchivo.ID

        archivoagrabar.MENSAJES = mimensaje

        Dim usuarioenvia As CHAT.USUARIOSCHAT = BuscarUsuario(usuarioemisor)

        archivoagrabar.USUARIOID = usuarioenvia.ID


        archivoagrabar.RUTA = ruta


        'archivoagrabar.CONTENIDO = toBinary(imagen)


        ctx.ARCHIVOS.InsertOnSubmit(archivoagrabar)

        ctx.SubmitChanges()

        Return archivoagrabar



    End Function


    Public Function toBinary(texto)
        Dim Text As String = texto
        Dim oReturn As New StringBuilder
        Dim Separator As String = ("")
        For Each Character As Byte In ASCIIEncoding.ASCII.GetBytes(Text)
            oReturn.Append(Convert.ToString(Character, 2).PadLeft(8, "1"))
            oReturn.Append(Separator)
        Next
        Return (oReturn.ToString)


    End Function





    Public Function GrabarTipoArchivo(tipo) As CHAT.TIPOS_ARCHIVO

        Dim mitipo As String = tipo

        Dim mitipoarchivo As CHAT.TIPOS_ARCHIVO = New CHAT.TIPOS_ARCHIVO

        mitipoarchivo.TIPO = mitipo.ToUpper()

        '    Try
        'ctx.TIPOS_ARCHIVO.InsertOnSubmit(mitipoarchivo)

        'ctx.SubmitChanges()




        '  Catch exception As System.Data.SqlClient.SqlException

        'mitipoarchivo = (From x In ctx.TIPOS_ARCHIVO Where x.TIPO = mitipo).FirstOrDefault()

        '  End Try

        Dim busqueda As CHAT.TIPOS_ARCHIVO = New CHAT.TIPOS_ARCHIVO


        busqueda = (From x In ctx.TIPOS_ARCHIVO Where x.TIPO = mitipo).FirstOrDefault()


            If busqueda Is Nothing Then
            ctx.TIPOS_ARCHIVO.InsertOnSubmit(mitipoarchivo)
            ctx.SubmitChanges()
            Return mitipoarchivo
        Else
            Return busqueda

        End If

        Return mitipoarchivo



    End Function





    Public Function BuscarUsuario(telefono As String) As CHAT.USUARIOSCHAT

        Dim mitelefono As String = telefono

        If (mitelefono.Contains("@")) Then

            Return (From x In ctx.USUARIOSCHAT Where x.EMAIL = mitelefono).FirstOrDefault()

        Else

            Return (From x In ctx.USUARIOSCHAT Where x.TELEFONO = mitelefono).FirstOrDefault()

        End If

    End Function



    Public Function BuscarUsuarioPorId(id) As CHAT.USUARIOSCHAT

        Dim miid As String = id

        Return (From x In ctx.USUARIOSCHAT Where x.ID = miid).FirstOrDefault()


    End Function


    Public Function BuscarArchivosEnElMensaje(id) As List(Of CHAT.ARCHIVOS)

        Dim idmensaje As String = id

        Return (From x In ctx.ARCHIVOS Where x.MENSAJEID = idmensaje Select x).ToList()


    End Function



    Public Function BuscaLocalizacionEnElMensaje(id) As List(Of CHAT.LOCALIZACIONES)
        Dim idmensaje As String = id
        Return (From x In ctx.LOCALIZACIONES Where x.MENSAJEID = idmensaje Select x).ToList()
    End Function


    Public Function BuscarUsuariosGrupo(id) As List(Of CHAT.USUARIOSCHAT)
        Dim idgrupo As String = id
        Return (From x In ctx.GRUPOSCHAT_USUARIO Where x.GRUPOID = idgrupo Select x.USUARIOSCHAT).ToList()
    End Function

    Public Function BuscarGrupoPorID(id) As CHAT.GRUPOSCHAT
        Dim migrupo As String = id
        Return (From x In ctx.GRUPOSCHAT Where x.ID = migrupo).FirstOrDefault()
    End Function

    Public Function BuscarGrupo(nombre) As CHAT.GRUPOSCHAT
        Dim migrupo As String = nombre
        Return (From x In ctx.GRUPOSCHAT Where x.NOMBRE = migrupo).FirstOrDefault()
    End Function

    Public Function BuscarChat(codigo) As CHAT.CHATSCHAT
        Dim micodigo As String = codigo
        Return (From x In ctx.CHATSCHAT Where x.CODIGO = micodigo).FirstOrDefault()
    End Function


    Public Sub CrearChat(codigo, inicio)

        Dim michat As CHAT.CHATSCHAT = New CHAT.CHATSCHAT
        michat.CODIGO = codigo
        michat.INICIO = Convert.ToDateTime(inicio)

        ctx.CHATSCHAT.InsertOnSubmit(michat)
        ctx.SubmitChanges()
    End Sub

    Public Function CrearGrupo(nombre) As CHAT.GRUPOSCHAT

        Dim migrupo As CHAT.GRUPOSCHAT = New CHAT.GRUPOSCHAT
        migrupo.NOMBRE = nombre

        ctx.GRUPOSCHAT.InsertOnSubmit(migrupo)
        ctx.SubmitChanges()

        Return migrupo
    End Function

    Public Function CrearMensaje(contenido, dia, telefono, chat_id, usuariorecepcion) As CHAT.MENSAJES

        Dim mimensaje As CHAT.MENSAJES = New CHAT.MENSAJES

        mimensaje.CONTENIDO = contenido


        mimensaje.DIA = Convert.ToDateTime(dia)


        Dim miusuario As CHAT.USUARIOSCHAT
        Dim miusuarioreceptor As CHAT.USUARIOSCHAT
        Dim michat As CHAT.CHATSCHAT

        miusuario = BuscarUsuario(telefono)
        miusuarioreceptor = BuscarUsuario(usuariorecepcion)
        michat = BuscarChat(chat_id)
        mimensaje.CHATSCHAT = michat
        mimensaje.USUARIOSCHAT2 = miusuario
        mimensaje.USUARIOSCHAT = miusuarioreceptor

        ctx.MENSAJES.InsertOnSubmit(mimensaje)
        ctx.SubmitChanges()

        Return mimensaje
    End Function



    Public Sub marcarComoLeidos(chatid, idpropietario)

        Dim recibe As String = idpropietario

        Dim michat As String = chatid

        Dim mismensajes As List(Of CHAT.MENSAJES) = (From x In ctx.MENSAJES Where x.IDUSUARIORECEPCION = recibe Where x.LEIDO Is Nothing Where x.CHATSCHAT.CODIGO = michat Select x).ToList()

        For Each item As CHAT.MENSAJES In mismensajes

            item.LEIDO = 1

        Next

        ctx.SubmitChanges()
    End Sub



    Public Function BuscarMensajesNoLeidosGrupo(telefono, IdGrupo) As Integer

        Dim mitelefono As String = telefono
        Dim migrupo As String = IdGrupo
        Dim mismensajes As List(Of CHAT.MENSAJES)

        Dim miusuario As CHAT.USUARIOSCHAT = BuscarUsuario(telefono)

        mismensajes = (From x In ctx.MENSAJES Where x.IDUSUARIORECEPCION = miusuario.ID Where x.CHATSCHAT.CODIGO = migrupo Where x.LEIDO Is Nothing Select x).ToList()


        Return mismensajes.Count


    End Function







    Public Function BuscarMensajesNoLeidos(Idpropietario, Idcompañero) As List(Of CHAT.MENSAJES)

        Dim mismensajes As List(Of CHAT.MENSAJES)

        Dim mIdpropietario As String = Idpropietario
        Dim mIdcompañero As String = Idcompañero

        mismensajes = (From x In ctx.MENSAJES Where x.IDUSUARIORECEPCION = mIdpropietario Where x.USUARIOID = mIdcompañero Where x.LEIDO Is Nothing Select x).ToList()

        Return mismensajes


    End Function


    Public Function grabarlocalizacion(latitud, longitud, mensajeid) As CHAT.LOCALIZACIONES

        Dim lat As String = latitud
        Dim longt As String = longitud

        Dim mid As String = mensajeid

        Dim localizacion As CHAT.LOCALIZACIONES = New CHAT.LOCALIZACIONES

        localizacion.LATITUD = lat
        localizacion.LONGITUD = longt

        Dim mimensaje As CHAT.MENSAJES = (From x In ctx.MENSAJES Where x.ID = mid Select x).FirstOrDefault()


        localizacion.MENSAJES = mimensaje


        ctx.LOCALIZACIONES.InsertOnSubmit(localizacion)

        ctx.SubmitChanges()

        Return localizacion

    End Function





    Public Function DameChatBuscarMensajesLeidos(Idpropietario, Idcompañero) As String

        Dim mismensajes As List(Of CHAT.MENSAJES)
        Dim mismensajes2 As List(Of CHAT.MENSAJES)

        Dim mIdpropietario As String = Idpropietario
        Dim mIdcompañero As String = Idcompañero

        mismensajes = (From x In ctx.MENSAJES Where x.IDUSUARIORECEPCION = mIdpropietario Where x.USUARIOID = mIdcompañero Select x).ToList()

        mismensajes2 = (From x In ctx.MENSAJES Where x.IDUSUARIORECEPCION = mIdcompañero Where x.USUARIOID = mIdpropietario Select x).ToList()


        Dim micodigo As String = ""

        If mismensajes.Count > 0 Then



            For Each mensaje In mismensajes



                If BuscarGrupoPorID(mensaje.CHATSCHAT.CODIGO) Is Nothing Then

                    micodigo = mensaje.CHATSCHAT.CODIGO

                End If


            Next

            Return micodigo

        ElseIf mismensajes2.Count > 0 Then



            For Each mensaje In mismensajes2


                If BuscarGrupoPorID(mensaje.CHATSCHAT.CODIGO) Is Nothing Then

                    micodigo = mensaje.CHATSCHAT.CODIGO

                End If


            Next

            Return micodigo

        Else
            Return ""

        End If
    End Function



    Public Function MostrarMensajesChat(codigo) As List(Of CHAT.MENSAJES)

        Dim michat As CHAT.CHATSCHAT
        michat = BuscarChat(codigo)

        Dim mensajes As New List(Of CHAT.MENSAJES)
        mensajes = (From x In ctx.MENSAJES Where x.CHATSCHAT.CODIGO = michat.CODIGO Select x).ToList()


        Return mensajes




    End Function


    Public Function BuscarChatPorID(id) As CHAT.CHATSCHAT

        Dim miid As String = id

        Return (From x In ctx.CHATSCHAT Where x.ID = miid).FirstOrDefault()

    End Function



    Public Function MostrarChatsUsuario(telefono) As List(Of MENSAJES)

        Dim miusuario = BuscarUsuario(telefono)

        ' Dim idgrupos As List(Of Integer) = (From x In ctx.GRUPOSCHAT Select x.ID).ToList()

        Dim listado As List(Of MENSAJES) = (From x In ctx.MENSAJES Where x.USUARIOID = miusuario.ID Select x).ToList()

        ' Dim listadoRECIBIENDO As List(Of MENSAJES) = (From x In ctx.MENSAJES Where x.IDUSUARIORECEPCION = miusuario.ID Select x).ToList()

        ' Dim listado As List(Of MENSAJES) = listadoENVIANDO.Union(listadoRECIBIENDO)

        'Dim res As List(Of MENSAJES) = (From m In ctx.MENSAJES Group Join so In ctx.GRUPOSCHAT On so.ID Equals m.CHATID Into MensajesGrupos = Group From mg In MensajesGrupos Where Is Nothing Select m).ToList()


        Return listado


    End Function


    Public Function MostrarChatsUsuarioRecibiendo(telefono) As List(Of MENSAJES)

        Dim miusuario = BuscarUsuario(telefono)

        ' Dim idgrupos As List(Of Integer) = (From x In ctx.GRUPOSCHAT Select x.ID).ToList()

        Dim listado As List(Of MENSAJES) = (From x In ctx.MENSAJES Where x.IDUSUARIORECEPCION = miusuario.ID Select x).ToList()




        'Dim res As List(Of MENSAJES) = (From m In ctx.MENSAJES Group Join so In ctx.GRUPOSCHAT On so.ID Equals m.CHATID Into MensajesGrupos = Group From mg In MensajesGrupos Where Is Nothing Select m).ToList()


        Return listado


    End Function






    Public Function MostrarGruposUsuario(telefono) As List(Of GRUPOSCHAT_USUARIO)

        Dim miusuario = BuscarUsuario(telefono)

        Dim listado As List(Of GRUPOSCHAT_USUARIO) = (From x In ctx.GRUPOSCHAT_USUARIO Where x.USUARIOSCHAT.ID = miusuario.ID Select x).ToList()


        Return listado


    End Function












End Class
