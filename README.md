# Fiap-TechChallenge01

Este é o repositório do projeto Fiap-TechChallenge01. Para executar o projeto, siga as instruções abaixo.

## Desafio

Desenvolvimento de uma aplicação web com acesso ao banco de dados e ao storage. O projeto deve conter uma Aplicação Web, uma Web API, um banco de dados e um storage hospedados no Azure. O acesso ao banco de dados deverá ser realizado pela API, a aplicação web servirá como Front end.

## Pré-requisitos

Certifique-se de ter realizado o deploy dos seguintes recursos no Azure:

- Uma conta de armazenamento (Storage Account)
- Um banco de dados CosmosDB

Além disso, preencha o arquivo `appsettings.json` com os dados necessários para acessar os recursos.

## Clonar o projeto

Clone este repositório em sua máquina local usando o comando:

`git clone https://github.com/guigsgbm/Fiap-TechChallenge01.git`

## Executar o projeto

1. Acesse as pastas `src/webAPI` e `src/WebApp` do projeto.
2. Certifique-se de ter o SDK do dotnet 6.0+ instalado em sua máquina.
3. Execute o seguinte comando em ambas as pastas:

`dotnet run`

4. Após executar os comandos acima, as aplicações estarão disponíveis nos seguintes endereços:

- WebApp: [https://localhost:7151](http://localhost:7151)
- WebAPI: [https://localhost:7009](http://localhost:7009)

Certifique-se de que as portas 7151 e 7009 estão disponíveis em sua máquina para acessar as aplicações localmente.
