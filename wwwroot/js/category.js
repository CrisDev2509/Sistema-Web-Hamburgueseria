document.addEventListener("DOMContentLoaded", function () {
    const formCategoria = document.getElementById("formCategoria");

    if (formCategoria) {
        // Manejo del envío del formulario (Botón Guardar)
        formCategoria.addEventListener("submit", function (e) {
            e.preventDefault(); // Evitar que se recargue la página

            // Obtener los valores del formulario
            const categoryId = document.getElementById("categoryId").value;
            const name = document.getElementById("name").value;
            const status = document.getElementById("status").value; // Estado como texto ("Activo" o "Inactivo")
            const showFilter = document.getElementById("showFilter").value === "true";  // Convertir a booleano, debería ser "ShowFilter"

            // Crear un objeto con los datos del formulario
            const categoryData = {
                id: categoryId ? categoryId : null, // Si es una nueva categoría, el ID será nulo
                name: name,
                state: status, // "Activo" o "Inactivo"
                showFilter: showFilter // Asegúrate de que este campo se llame "showFilter"
            };

            // Determinar si estamos creando o actualizando
            const url = categoryId ? `/Category/${categoryId}` : `/Category`; // Si estamos editando, la URL será /Category/{id}

            // Enviar los datos al servidor usando Fetch (POST o PUT según el caso)
            fetch(url, {
                method: categoryId ? 'PUT' : 'POST', // 'PUT' si estamos actualizando, 'POST' si estamos creando
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify(categoryData) // Enviar el objeto con los datos como JSON
            })
                .then(response => response.json())
                .then(data => {
                    if (data.success) {
                        // Si la operación fue exitosa, puedes actualizar la tabla o mostrar el mensaje adecuado
                        alert("Categoría guardada correctamente");

                        // Si estás editando, actualizas la fila correspondiente en la tabla
                        if (categoryId) {
                            actualizarFilaEnTabla(categoryData);
                        } else {
                            // Si estás creando, agregas la nueva fila a la tabla
                            agregarFilaATabla(categoryData);
                        }

                        // Limpiar el formulario (opcional)
                        formCategoria.reset();
                        document.getElementById("categoryId").value = ""; // Limpiar el campo oculto
                    } else {
                        alert("Hubo un error al guardar la categoría.");
                    }
                })
                .catch(error => {
                    console.error('Error al guardar:', error);
                });
        });

        // Función para agregar una fila nueva a la tabla
        function agregarFilaATabla(category) {
            const table = document.getElementById('tb-categoria').getElementsByTagName('tbody')[0];
            const fila = table.insertRow();
            const c1 = fila.insertCell(0);
            const c2 = fila.insertCell(1);
            const c3 = fila.insertCell(2);
            const c4 = fila.insertCell(3);
            const c5 = fila.insertCell(4);

            c1.innerHTML = category.id;
            c2.innerHTML = category.name;
            c3.innerHTML = category.state === "Activo" ? "Activo" : "Inactivo";
            c4.innerHTML = category.showFilter ? "Visible" : "No visible";
            c5.innerHTML = "<a href='#' class='btn-table edit-view edit-cat' data-id='" + category.id + "'><i class='fa-solid fa-pen'></i></a>" +
                "<a href='#' class='btn-table delete delete-cat' data-id='" + category.id + "'><i class='fa-solid fa-trash'></i></a>";

            c1.style.textAlign = "center";
            c3.style.textAlign = "center";
            c4.style.textAlign = "center";
            c5.style.textAlign = "center";
            c5.style.gap = "10";
        }

        // Función para actualizar una fila existente en la tabla
        function actualizarFilaEnTabla(category) {
            const rows = document.getElementById('tb-categoria').getElementsByTagName('tbody')[0].rows;
            for (let i = 0; i < rows.length; i++) {
                const row = rows[i];
                const cellId = row.cells[0].innerText;

                // Si encontramos la fila con el mismo ID, actualizamos sus valores
                if (cellId == category.id) {
                    row.cells[1].innerText = category.name;
                    row.cells[2].innerText = category.state === "Activo" ? "Activo" : "Inactivo";
                    row.cells[3].innerText = category.showFilter ? "Visible" : "No visible";
                    break;
                }
            }
        }

        // Función para eliminar una fila de la tabla
        function eliminarFilaDeTabla(id) {
            const rows = document.getElementById('tb-categoria').getElementsByTagName('tbody')[0].rows;
            for (let i = 0; i < rows.length; i++) {
                const row = rows[i];
                const cellId = row.cells[0].innerText;
                if (cellId == id) {
                    row.remove();
                    break;
                }
            }
        }

        // Evento de clic para editar
        document.body.addEventListener('click', function (e) {
            if (e.target && e.target.classList.contains('edit-cat')) {
                const id = e.target.getAttribute('data-id');
                // Realiza una solicitud GET o carga los datos para editar
                fetch(`/Category/${id}`)
                    .then(response => response.json())
                    .then(data => {
                        if (data.success) {
                            const category = data.category;
                            document.getElementById("categoryId").value = category.id;
                            document.getElementById("name").value = category.name;
                            document.getElementById("status").value = category.status;
                            document.getElementById("showFilter").value = category.showFilter ? "true" : "false";
                        } else {
                            alert("No se encontraron los datos para editar.");
                        }
                    })
                    .catch(error => console.error("Error al cargar los datos de la categoría:", error));
            }
        });

        // Evento de clic para eliminar
        document.body.addEventListener('click', function (e) {
            if (e.target && e.target.classList.contains('delete-cat')) {
                const id = e.target.getAttribute('data-id');
                if (confirm("¿Estás seguro de que deseas eliminar esta categoría?")) {
                    fetch(`/Category/${id}`, {
                        method: 'DELETE'
                    })
                        .then(response => response.json())
                        .then(data => {
                            if (data.success) {
                                alert("Categoría eliminada correctamente");
                                eliminarFilaDeTabla(id); // Eliminar la fila de la tabla
                            } else {
                                alert("Hubo un error al eliminar la categoría.");
                            }
                        })
                        .catch(error => console.error("Error al eliminar la categoría:", error));
                }
            }
        });

        // Cargar las categorías al inicio
        function cargarCategorias() {
            fetch('/Category')
                .then(response => response.json())
                .then(data => {
                    if (data.success) {
                        data.categories.forEach(category => agregarFilaATabla(category));
                    }
                })
                .catch(error => console.error("Error al cargar categorías:", error));
        }

        // Cargar las categorías al inicio
        cargarCategorias();
    }
});
