# Implementação do Dynamics 365 para Logistics

Este repositório contém todos os artefatos desenvolvidos para a implementação do Dynamics 365 na empresa Logistics, uma líder global no ramo de transporte e importação de alimentos. Este projeto faz parte do Trabalho de Conclusão de Curso (TCC) oferecido pela AlfaPeople, focado em customizações e integrações avançadas no Dynamics 365.

## Participantes

- Matheus Melgaço Barroso
- Cesar Augusto Slesaczek
- Ana Costa
- Elissandro Teófilo

## Sobre o Projeto

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

- **Integração com Sistema Externo de Estoque "My Warehouse"**: Reflexão de criação, modificação ou exclusão de contas no Dynamics CE a partir de alterações no sistema de estoque My Warehouse, utilizando Azure Function.

## Tecnologias Utilizadas

- Dynamics 365 
- Power Automate
- JavaScript
- Azure Functions
- Canvas Apps
- Plugin 
