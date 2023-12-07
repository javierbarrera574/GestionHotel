function CustomConfirm(titulo, mensaje, tipo)
{
	return new Promise((resolve) =>
	{
        Swal.fire({
            title: titulo,
            text: mensaje,
            type: tipo,
            showCancelButton: true,
            confirmButtonColor: "#3085d6",
            cancelButtonColor: "#d33",
            confirmButtonText: "Confirmar",
            cancelButtonText: "Cancelar"
        }).then((result) =>
        {
            if (result.value)
            {
                resolve(true);
            }
            else
            {
                resolve(false);
			}
        });
	});
}

function CustomError(titulo, mensaje, tipo) {
    return new Promise((resolve) => {
        Swal.fire({
            icon: "error",
            title: titulo,
            text: mensaje,
            type: tipo,
            confirmButtonColor: "#3085d6",
            confirmButtonText: "OK"
        }).then(() => {
            resolve("OK"); // Devolver un mensaje simple
        });
    });
}


