describe('Pruebas de sistema web Bigtoria', () => {
    beforeEach(() => {
        cy.visit('http://localhost:5139/')

        cy.get('#Email').type('admin@gmail.com')

        cy.get('#Password').type('Admin123*{enter}')
    })

    it('Verificar datos home', () => {
        cy.get('.message > p').should('have.text', 'Bienvenido ADMINISTRADOR GENERAL')
        cy.get(':nth-child(1) > .info-body > h2').should('have.text', 'S/. 0')
        cy.get(':nth-child(2) > .info-body > h2').should('have.text', '15629.00')
        cy.get(':nth-child(3) > .info-body > h2').should('have.text', '0')
        cy.get(':nth-child(4) > .info-body > h2').should('have.text', '0')
    })
});