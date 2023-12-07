namespace HotelApp.Client.Servicios
{
    public interface IHTTPServicio
    {
        Task<HTTPRespuesta<object>> Delete(string url);
        Task<HTTPRespuesta<T>> Get<T>(string url);
        Task<HTTPRespuesta<T>> GetCod<T>(string url);
        Task<HTTPRespuesta<object>> Post<T>(string url, T enviar);
        Task<HTTPRespuesta<object>> Put<T>(string url, T enviar);
    }
}