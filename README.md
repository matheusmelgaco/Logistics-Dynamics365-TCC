# Implementação do Dynamics 365 para Logistics

Este repositório contém todos os artefatos desenvolvidos para a implementação do Dynamics 365 CRM na empresa Logistics, uma líder global no ramo de transporte e importação de alimentos. Este projeto faz parte do Trabalho de Conclusão de Curso (TCC) oferecido pela AlfaPeople, focado em customizações e integrações avançadas no Dynamics 365.

## Participantes

- Matheus Melgaço Barroso
- Cesar Augusto Slesaczek
- Ana Costa
- Elissandro Teófilo

## Sobre o Projeto

O projeto visa atender a uma série de requisitos específicos da Logistics, incluindo a gestão de dois ambientes Dynamics, automação de cotações, integração com o ViaCEP para preenchimento automático de endereços, entre outros. As customizações e integrações são projetadas para otimizar os processos de vendas e CRM da Logistics.

## Requisitos Principais

1. **Integração de tabelas de produtos entre Dynamics 1 e Dynamics 2:** Utilização de Plugin para sincronizar o cadastro de produtos entre os dois ambientes Dynamics.
2. **Proibição de cadastro direto no Dynamics 2:** Garantir que produtos não sejam cadastrados diretamente no Dynamics 2, utilizando Plugin para esta regra de negócio.
3. **Envio automático de cotações com Power Automate:** Automatizar o envio de cotações para clientes quando uma cotação for ativada.
4. **Preenchimento automático de endereço via ViaCEP:** Implementar preenchimento automático dos campos de endereço ao digitar um CEP, usando JavaScript e uma Action que consome o webservice ViaCEP.
5. **Desenvolvimento de um aplicativo Canvas:** Criar um app que contenha FAQ, lista de clientes e oportunidades para uso dos vendedores em campo.
6. **Registro da última data de visita a clientes com Power Automate:** Automatizar o registro da última visita realizada a um cliente.
7. **Clonagem de Propostas:** Permitir a clonagem de oportunidades antigas para reutilização de produtos em novas oportunidades, utilizando JavaScript, Action, e Ribbon.
8. **Identificador único para oportunidades:** Garantir um identificador único alfanumérico para cada oportunidade, evitando duplicidade com uso de Plugin.
9. **Validação de CNPJ:** Formatar e validar os campos de CNPJ usando JavaScript.
10. **Padronização do nome da conta:** Assegurar que o nome da conta seja cadastrado com capitalização adequada usando JavaScript.
11. **Integração e bloqueio de edição de oportunidades integradas:** Integrar oportunidades do Dynamics 1 para o Dynamics 2, bloqueando a edição de oportunidades que vieram de integração, usando JavaScript e Plugin.
12. **Validação de CPF em contatos:** Validar o cadastro de CPF em contatos para garantir a inserção de dados válidos usando JavaScript.
13. **Unicidade de CPF/CNPJ:** Impedir duplicidade no cadastro de contatos com o mesmo CPF e contas com o mesmo CNPJ usando Plugin.

## Desafio Bônus

- **Integração com sistema de controle de estoque "My Warehouse":** Implementar uma integração com o sistema externo "My Warehouse" para sincronizar alterações de contas, utilizando Azure Function.

## Tecnologias Utilizadas

- Dynamics 365
- Power Automate
- JavaScript
- Azure Functions
- Canvas Apps
- C#
