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

window.sweetAlertInterop = {
    showSuccessDialog: function (title, text) {
        return new Promise((resolve) => {
            Swal.fire({
                title: title,
                text: text,
                icon: "success"
            }).then(() => {
                resolve(true);
            });
        });
    }
};


// SweetAlertInterop.js
window.showSweetAlert = function (title, showDenyButton, showCancelButton, confirmButtonText, denyButtonText) {
    return Swal.fire({
        title: title,
        showDenyButton: showDenyButton,
        showCancelButton: showCancelButton,
        confirmButtonText: confirmButtonText,
        denyButtonText: denyButtonText
    }).then((result) => {
        return result;
    });
};

window.handleSwalResult = (result) => {
    return new Promise((resolve) => {
        Swal.fire(result).then((swalResult) => {
            if (swalResult.isConfirmed) {
                resolve(true);
            } else if (swalResult.isDenied || swalResult.dismiss === Swal.DismissReason.cancel) {
                resolve(false);
            }
        });
    });
};


//function CustomSave(titulo, mensaje, tipo) {
//    return new Promise((resolve) => {
//        Swal.fire({
//            title: titulo,
//            text: mensaje,
//            icon: tipo,
//            showDenyButton: true,
//            showCancelButton: true,
//            confirmButtonText: "Guardar",
//            denyButtonText: `No guardar`
//        }).then((result) => {
//            if (result.isConfirmed) {
//                Swal.fire("Guarado", " ", "success");
//                resolve(true); // Resuelve la promesa con true si se confirma
//            } else if (result.isDenied) {
//                Swal.fire("No guardado", " ", "info");
//                resolve(false); // Resuelve la promesa con false si se deniega
//            }
//        });
//    });
//}

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


