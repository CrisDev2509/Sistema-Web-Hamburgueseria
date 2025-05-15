describe('Pruebas de sistema web Bigtoria', () => {
    beforeEach(() => {
        cy.visit('http://localhost:5139/')

        cy.get('#Email').type('admin@gmail.com')

        cy.get('#Password').type('Admin123*{enter}')
    })

    it('verificar carta', () => {
        cy.get('.menu-item > :nth-child(2) > a').click()
        cy.get('.product-content').children('div').should('have.length.greaterThan', 0)
    })

    it('verificar que se puede hacer una compra', () => {
        cy.get('.menu-item > :nth-child(2) > a').click()
        cy.get('.product-content').children('div').eq(1).trigger('mouseenter');
        cy.get('[data-id-product="2"] > #product-info').click()
        cy.get('.btn-payment').click()
        cy.get('button.btn-payment').should('have.text', 'REALIZAR PEDIDO')
        cy.get(':nth-child(1) > :nth-child(1) > .input-form').type('Prueba')
        cy.get(':nth-child(2) > :nth-child(1) > .input-form').type('cypress')
        cy.get(':nth-child(1) > :nth-child(2) > .input-form').type('prueba@cypress.com')
        cy.get(':nth-child(2) > :nth-child(2) > .input-form').type('987654321')
        cy.get('button.btn-payment').click()
        cy.get('b').should('have.text', 'VENTA EXITOSA')
    })

    it('verificar mensaje de error cuando no se ingresan datos en la venta', () => {
        cy.get('.menu-item > :nth-child(2) > a').click()
        cy.get('.product-content').children('div').eq(1).trigger('mouseenter');
        cy.get('[data-id-product="2"] > #product-info').click()
        cy.get('.btn-payment').click()
        cy.get('button.btn-payment').should('have.text', 'REALIZAR PEDIDO')
        cy.get('button.btn-payment').click()
        cy.get('h3').contains('Error:')
    })

    it('Verificar que se muestran las ventas y comprobantes', () => {
        cy.get(':nth-child(6) > a').click()
        cy.get('b').should('have.text', 'REPORTES')
        cy.get('tbody').find('tr').first().click()
        cy.get('b').should('have.text', 'REPORTE - FACTURA')
    })

    it('Verificar que se puede registrar un producto', () => {
        cy.get(':nth-child(4) > a').click()
        cy.get('b').should('have.text', 'INVENTORIO')
        cy.get('a').contains('Nuevo').click()
        cy.get('b').should('have.text', 'INVENTORIO - INSERTAR NUEVO')
        cy.get('#Product_Name').type('Hamburguesa Test cypress')
        cy.get('#Product_Description').type('Descripcion hamburguesa cypress')
        cy.get('#select-category').select('HAMBURGUESA')
        cy.get('#Product_ShowStore').select('Si')
        cy.get('#Product_Price').clear().type(5.50)
        cy.get('#Product_Stock').clear().type(10.00)
        cy.get('#Product_Discount').clear().type(0)
        cy.get('.btn-fill').click()
        cy.get('#mensaje').should('have.text', 'Producto guardado con exito')
    })

    it('Verificar que campos se validen antes de registrar un producto', () => {
        cy.get(':nth-child(4) > a').click()
        cy.get('b').should('have.text', 'INVENTORIO')
        cy.get('a').contains('Nuevo').click()
        cy.get('b').should('have.text', 'INVENTORIO - INSERTAR NUEVO')
        cy.get('.btn-fill').click()
        cy.get('input[required]:invalid').should('exist');
    })
    it('Verificar que se puede registrar un proveedor', () => {
        cy.get(':nth-child(5) > a').click()
        cy.get('b').should('have.text', 'PROVEEDORES')
        cy.get('a').contains('Nuevo').click()
        cy.get('b').should('have.text', 'PROVEEDOR - INSERTAR NUEVO')
        cy.get('#Name').type('Proveedor Prueba 3')
        cy.get('#RUC').type(10203040501)
        cy.get('#Email').type('proveedor3@prueba.com')
        cy.get('.btn-fill').click()
        cy.get('#mensaje').should('have.text', 'Proveedor registrado')
    })

    it('Verificar que se validan los campos al registrar un proveedor', () => {
        cy.get(':nth-child(5) > a').click()
        cy.get('b').should('have.text', 'PROVEEDORES')
        cy.get('a').contains('Nuevo').click()
        cy.get('b').should('have.text', 'PROVEEDOR - INSERTAR NUEVO')
        cy.get('.btn-fill').click()
        cy.get('input[required]:invalid').should('exist');
    })

    it('Verificar que se puede ver detalle de un proveedor', () => {
        cy.get(':nth-child(5) > a').click()
        cy.get('b').should('have.text', 'PROVEEDORES')
        cy.get('tbody').find('tr').last().find('.btn-action').children('a').eq(0).click()
        cy.get('b').should('have.text', 'PROVEEDOR - DETALLE')
    })

    it('Verificar que se puede editar un proveedor', () => {
        cy.get(':nth-child(5) > a').click()
        cy.get('b').should('have.text', 'PROVEEDORES')
        cy.get('tbody').find('tr').last().find('.btn-action').children('a').eq(1).click()
        cy.get('b').should('have.text', 'PROVEEDOR - EDITAR')
        cy.get('#Name').type('actualizado')
        cy.get('.btn-fill').click()
        cy.get('#mensaje').should('have.text', 'Proveedor actualizado')
    })

    it('Verificar que se puede eliminar un proveedor', () => {
        cy.get(':nth-child(5) > a').click()
        cy.get('b').should('have.text', 'PROVEEDORES')
        cy.get('tbody').find('tr').last().find('.btn-action').children('a').eq(2).click()
        cy.get('b').should('have.text', 'PROVEEDOR - ELIMINAR')
        cy.get('.btn-fill').click()
        cy.get('#mensaje').should('have.text', 'Proveedor eliminado')
    })

    it('Verificar que se puede buscar un proveedor por RUC', () => {
        cy.get(':nth-child(5) > a').click()
        cy.get('b').should('have.text', 'PROVEEDORES')
        cy.get('#ruc').type(20987654321)
        cy.get('.btn-fill').click()
        cy.get('tbody').children('tr').should('have.length', 1)
    })
})