namespace BlazorCrud.Shared
{
    public class ResponseAPI<T>
    {
        public bool EsCorrecto { get; set; }
        public T? Valor { get; set; }
        public String? Mensaje { get; set; }

    }
}
