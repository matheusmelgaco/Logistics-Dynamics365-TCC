## Sobre o Projeto

O projeto visa atender a uma série de requisitos específicos da Logistics, incluindo a gestão de dois ambientes Dynamics, automação de cotações, integração com o ViaCEP para preenchimento automático de endereços, entre outros. As customizações e integrações são projetadas para otimizar os processos de vendas e CRM da Logistics.

### Requisitos Principais

- Integração de tabelas de produtos entre Dynamics 1 e Dynamics 2.
- Envio automático de cotações utilizando Power Automate.
- Preenchimento automático de campos de endereço via ViaCEP.
- Desenvolvimento de um aplicativo Canvas para uso em campo por vendedores.
- Implementação de validações para CNPJ e CPF.
- Clonagem de propostas e gestão de oportunidades únicas.
A implementação foca em otimizar os processos de vendas e CRM da Logistics por meio de customizações específicas no Dynamics 365, abordando desde a gestão de produtos entre diferentes ambientes Dynamics até a automação de processos e integrações com sistemas externos.

### Requisitos do Projeto

- **Integração entre Ambientes Dynamics**: Sincronização de cadastro de produtos entre Dynamics 1 (fonte) e Dynamics 2 (destino) através de Plugin.
- **Automação de Cotações**: Utilização de Power Automate para enviar automaticamente cotações ativadas para clientes.
- **Preenchimento Automático de Endereço via ViaCEP**: Implementação de JavaScript e Actions para preenchimento de campos de endereço com base no CEP fornecido.
- **Desenvolvimento de Aplicativo Canvas**: Criação de um app com FAQ, lista de clientes, e oportunidades para uso em campo por vendedores.
- **Gestão de Visitas a Clientes**: Registro da última data de visita a um cliente específico via Power Automate.
- **Clonagem de Propostas**: Possibilidade de reutilizar oportunidades antigas, clonando propostas através de JavaScript, Actions, e modificação do Ribbon.
- **Identificador Único para Oportunidades**: Garantia de unicidade de identificadores alfanuméricos para oportunidades através de Plugin.
- **Validação e Formatação de CNPJ/CPF**: Implementação de JavaScript para validar e formatar corretamente campos de CNPJ e CPF.
- **Padronização do Nome da Conta**: Customização via JavaScript para garantir a padronização do nome da conta em um formato específico.
- **Integração e Restrição de Edição de Oportunidades**: Integração de oportunidades entre Dynamics 1 e Dynamics 2 com restrição de edição para itens integrados, utilizando JavaScript e Plugin.
- **Validação de CPF em Contatos e CNPJ em Contas**: Verificação da unicidade de CPFs e CNPJs no sistema via Plugin para evitar duplicidades.

### Desafio Bônus

Integração com um sistema de controle de estoque externo "My Warehouse", refletindo alterações de contas no Dynamics 365 através de Azure Function.
- **Integração com Sistema Externo de Estoque "My Warehouse"**: Reflexão de criação, modificação ou exclusão de contas no Dynamics CE a partir de alterações no sistema de estoque My Warehouse, utilizando Azure Function.

## Tecnologias Utilizadas

@@ -33,3 +38,4 @@ Integração com um sistema de controle de estoque externo "My Warehouse", refle
- JavaScript
- Azure Functions
- Canvas Apps
- Plugin 
