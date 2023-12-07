using Microsoft.JSInterop;

namespace HotelApp.Client.Helpers
{
    public static class IJSExtensions
    {
        public static async Task MostrarMensaje(this IJSRuntime js, string mensaje)
        {
             await js.InvokeAsync<object>("Swal.fire", mensaje);
        }

        public static async Task MostrarMensaje(this IJSRuntime js, string titulo, string mensaje, TipoMensajeSweetAlert tipoMensajeSweetAlert)
        {
            await js.InvokeAsync<object>("Swal.fire",titulo, mensaje, tipoMensajeSweetAlert.ToString());
        }

        public async static Task<bool> Confirmar(this IJSRuntime js,string titulo, string mensaje,TipoMensajeSweetAlert tipoMensajeSweetAlert)
        {
             return await js.InvokeAsync<bool>("CustomConfirm",titulo, mensaje, tipoMensajeSweetAlert.ToString());
        }

        public async static Task<string> Error(this IJSRuntime js, string titulo, string mensaje, TipoMensajeSweetAlert tipoMensajeSweetAlert)
        {
            return await js.InvokeAsync<string>("CustomError", titulo, mensaje, tipoMensajeSweetAlert.ToString());
        }

        public async static Task<bool> Guardar(this IJSRuntime js, string titulo, string mensaje, TipoMensajeSweetAlert tipoMensajeSweetAlert)
        {
            return await js.InvokeAsync<bool>("sweetAlertInterop.showCustomSaveDialog", titulo, mensaje, tipoMensajeSweetAlert.ToString());
        }
        public static async Task<bool> MostrarExito(this IJSRuntime js, string titulo, string mensaje)
        {
            return await js.InvokeAsync<bool>("sweetAlertInterop.showSuccessDialog", titulo, mensaje);
        }
    }
    public enum TipoMensajeSweetAlert
    {
        question = 1,
        error    = 2,
    }
}