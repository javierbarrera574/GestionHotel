namespace HotelApp.Client.Servicios
{
    public class HTTPRespuesta<T>
    {
        public T? Respuesta { get; set; }
        public bool Error { get; set; }
        public HttpResponseMessage HttpResponseMessage { get; set; }
        public HTTPRespuesta(T? response, bool error, HttpResponseMessage httpResponseMessage)
        {
            this.Respuesta = response;
            this.Error = error;
            this.HttpResponseMessage = httpResponseMessage; //viene del error de los controllers
        }       
        public async Task<string> ObtenerError()
        {
            if (!Error)
            {
                return "";
            }
            var statuscode = HttpResponseMessage.StatusCode;

            switch (statuscode)
            {

                case System.Net.HttpStatusCode.BadRequest:
                    //return HttpResponseMessage.ToString();
                    return await HttpResponseMessage.Content.ReadAsStringAsync();
                case System.Net.HttpStatusCode.Unauthorized:
                    //return "Error, no está logueado";
                    return await HttpResponseMessage.Content.ReadAsStringAsync();
                case System.Net.HttpStatusCode.Forbidden:
                    //return "Error, no tiene autorizacion para ejecutar este proceso";
                    return await HttpResponseMessage.Content.ReadAsStringAsync();
                case System.Net.HttpStatusCode.NotFound:
                //return "Error, direccion no encontrada";
                case System.Net.HttpStatusCode.OK:
                    return await HttpResponseMessage.Content.ReadAsStringAsync();
                default:
                    return HttpResponseMessage.Content.ReadAsStreamAsync().ToString();
            }
        }
    }
}