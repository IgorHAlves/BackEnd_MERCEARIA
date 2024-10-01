# Mercearia CRUD API

Este projeto é uma API desenvolvida para gerenciar uma mercearia fictícia, permitindo operações CRUD (Create, Read, Update, Delete) para **Clientes**, **Produtos** e **Pedidos**. A API utiliza o **SQL Server** como banco de dados, 
configurado no Docker para facilitar a execução e manutenção do ambiente.

## Tecnologias Utilizadas

- **Linguagem**: C#
- **Framework**: .NET | Entity Framework Core | AspNetCore
- - **Banco de Dados**: SQL Server
- **Contêinerização**: Docker

## Funcionalidades

- **Gerenciamento de Clientes**: Adicionar, visualizar, editar e excluir informações de clientes.
- **Gerenciamento de Produtos**: Adicionar, visualizar, editar e excluir produtos.
- **Gerenciamento de Pedidos**: Criar, visualizar e deletar, vinculando clientes e itens.

## Inicialização

Certifique-se de ter o Docker instalado em sua máquina.

1. [Docker](https://www.docker.com/) instalado e em execução.

## Executando o Projeto

### 1. Clone o Repositório

### 2. Faça o download da imagem do SQL Server no Docker
 - Faça o download da imagem do SQL Server no Docker e crie um container com o nome de usuario e senha conforme a conection string do DbContext.cs
### 3. Execute o docker e rode a API
 - Assim que rodar a API o swagger será iniciado, onde é possível consultar os endpoints e visualizar como são os retornos json de uma forma mais clara.

## Desafios

As regras de negocio foram feitas dentro do controller, onde os maiores desafios foram na parte do Pedido, onde é consultado se o cliente está cadastrado, se o produto está cadastrado e se a quantidade disponível no estoque é
suficiente para o pedido, e também e descontado do estoque assim que o pedido é confirmado. Destaco também que as tabelas relacionadas possuem o OnDelete(DeleteBehavior.Cascade) no mapeamento, garantindo a segurança ao deletar informações.
Destaco também que o banco foi criado com as migrations do EF Core.
