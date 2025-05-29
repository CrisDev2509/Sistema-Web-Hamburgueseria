document.addEventListener("DOMContentLoaded", function () {
    //Shopping
    const deleteAll = document.getElementById('delete-all-shop');
    const deleteById = document.querySelectorAll('.delete-item-shop');
    const addQuantity = document.querySelectorAll('.add-quantity');
    const subsQuantity = document.querySelectorAll('.subs-quantity');

    if (deleteAll !== null &&
        deleteById !== null &&
        addQuantity !== null &&
        subsQuantity !== null) {
        deleteAll.addEventListener('click', () => {
            fetch(`/Sale/deleteAll`, {
                method: 'POST',
                headers: {
                    'content-tipe': 'application/json'
                }
            })
                .then(response => {
                    if (response.ok) {
                        location.reload();
                    }
                    else {

                    }
                })
                .catch(error => {

                });
        });

        for (let i = 0; i < deleteById.length; i++) {
            const d = deleteById[i];
            d.addEventListener('click', (e) => {
                let itemShop = e.target.getAttribute('data-id-sale');

                if (itemShop) {
                    fetch(`/Sale/deleteById/${itemShop}`, {
                        method: 'POST',
                        headers: {
                            'content-tipe': 'application/json'
                        }
                    })
                        .then(response => {
                            if (response.ok) {
                                location.reload();
                            }
                            else {

                            }
                        })
                        .catch(error => {

                        });
                }
            });
        }

        for (let i = 0; i < addQuantity.length; i++) {
            const a = addQuantity[i];
            a.addEventListener('click', (e) => {
                let itemShop = e.target.getAttribute('data-id-item');

                if (itemShop) {
                    fetch(`/Sale/add/${itemShop}`, {
                        method: 'POST',
                        headers: {
                            'content-tipe': 'application/json'
                        }
                    })
                    .then(response => {
                        if (response.ok) {
                            location.reload();
                        }
                        else {

                        }
                    })
                    .catch(error => {

                    });
                }
            });
        }

        for (let i = 0; i < subsQuantity.length; i++) {
            const a = subsQuantity[i];
            a.addEventListener('click', (e) => {
                let itemShop = e.target.getAttribute('data-id-item');

                if (itemShop) {
                    fetch(`/Sale/subs/${itemShop}`, {
                        method: 'POST',
                        headers: {
                            'content-tipe': 'application/json'
                        }
                    })
                    .then(response => {
                        if (response.ok) {
                            location.reload();
                        }
                        else {

                        }
                    })
                    .catch(error => {

                    });
                }
            });
        }
    }


    //Select category
    const selectCat = document.getElementById('select-category');
    const inputCat = document.getElementById('input-category');
    if (selectCat !== null && inputCat !== null) {
        selectCat.addEventListener('change', (e) => {
            if (e.target.value == "0") {
                inputCat.hidden = false;
                selectCat.hidden = true;
                inputCat.focus();
            }
            else {
                inputCat.value = e.target.value;
            }
        });
    }

    //Modal
    const btnDelete = document.querySelectorAll('.delete');
    const btnModalInfo = document.getElementById('btn-modal-info');
    const btnProcess = document.getElementById('btn-process-sale');
    const btnCloseModal = document.querySelectorAll('.btn-close-modal');
    const btnCancelModal = document.getElementById('.btn-cancel-modal');
    const btnConfirmDelete = document.getElementById('btn-confirm-delete');
    const modal = document.getElementById('modal');
    const modalInfo = document.getElementById('modal-info');


    let currentId = null;
    let path = null;

    //Open dialgon delete
    if (btnDelete.length > 0 &&
        btnConfirmDelete !== null) {

        for (let i = 0; i < btnDelete.length; i++) {
            const btn = btnDelete[i];
            btn.addEventListener('click', (e) => {
                currentId = e.target.getAttribute('data-id');
                path = e.target.getAttribute('data-path');
                modal.classList.add('show-modal');
            });
        }

        btnConfirmDelete.addEventListener('click', () => {
            if (currentId) {
                fetch(`${path}${currentId}`, {
                    method: 'DELETE',
                    headers: {
                        'content-tipe': 'application/json'
                    }
                })
                .then(response => {
                    if (response.ok) {
                        modal.classList.remove('show-modal');
                        location.reload();
                    }
                    else {
                        console.log("Error al eliminar");
                    }
                })
                .catch(error => {

                });
            }
        });
    }

    //Open dialog shop
    if (btnProcess) {
        btnProcess.addEventListener('click', () => {
            const productList = document.querySelectorAll('.card-sale');
            if (productList.length > 0) {
                modal.classList.add('show-modal')
            }
        });
    }

    //Open model info
    if (btnModalInfo !== null && modalInfo !== null) { 
        btnModalInfo.addEventListener('click', () => {
            modalInfo.classList.add('show-modal');
        });    
    }

    //Close modal
    if (btnCloseModal !== null) {
        for (let i = 0; i < btnCloseModal.length; i++) {
            btn = btnCloseModal[i];

            btn.addEventListener('click', () => {
                if (modal && modal.classList.contains('show-modal')) {
                    modal.classList.remove('show-modal');
                }
                if (modalInfo && modalInfo.classList.contains('show-modal')) {
                    modalInfo.classList.remove('show-modal');
                }
            });
        }
    }

    if (btnCancelModal !== null) {
        btnCancelModal.addEventListener('click', () => {
            modal.classList.remove('show-modal');
        });
    }

    //Select tipe sale
    const checkDelivery = document.getElementById('delivery');
    const checkStore = document.getElementById('store');
    if (checkDelivery !== null && checkStore !== null) {
        const titleClient = document.getElementById('title-client');
        const infoClient = document.getElementById('info-delivery');

        checkDelivery.addEventListener('change', () => {
            if (checkDelivery.checked) {
                titleClient.hidden = false;
                infoClient.style.display = 'flex';
            }
        });

        checkStore.addEventListener('change', () => {
            if (checkStore.checked) {
                titleClient.hidden = true;
                infoClient.style.display = 'none';
            }
        });
    }

    //Cambiar contraseña
    const cbxChangePassword = document.getElementById('change-password');
    const pnlPassword = document.getElementById('content-password');

    if (cbxChangePassword && pnlPassword) {

        pnlPassword.style.display = 'none';

        cbxChangePassword.addEventListener('change', () => {
            if (cbxChangePassword.checked) {
                pnlPassword.style.display = 'block';
            }
            else {
                pnlPassword.style.display = 'none';
            }
        });
    }
});