# EliteCar API

API REST desenvolvida em **C# com .NET 10** para o controle de clientes,
estoque de veículos e vendas de uma concessionária.

Projeto desenvolvido para fins educacionais, com o objetivo de aplicar
conceitos de desenvolvimento de **APIs REST, orientação a objetos,
Entity Framework Core e persistência de dados**.

## Tecnologias Utilizadas

-   C#
-   .NET 10
-   ASP.NET Core Web API
-   Entity Framework Core
-   PostgreSQL

------------------------------------------------------------------------

# Diagrama de Classes

O sistema é composto pelas seguintes entidades principais:

-   `Cliente`
-   `Carro`
-   `PedidoVenda`

Também são utilizados DTOs para entrada e saída de dados:

-   `ClienteDTO`
-   `CarroDTO`
-   `PedidoVendaDTO`

As principais relações são:

-   Um cliente pode possuir **zero ou várias vendas**.
-   Uma venda pertence a **um único cliente**.
-   Um veículo pode estar associado a **zero ou uma venda ativa**.
-   Uma venda está associada a **um único veículo**.

## Modelo conceitual

``` mermaid
classDiagram
    class Cliente {
        +number idCliente
        +string nome
        +string cpf
        +string telefone
        +string email
        +string cidade
        +string uf
        +boolean ativo
    }

    class Carro {
        +number idCarro
        +string marca
        +string modelo
        +number ano
        +string cor
        +string placa
        +number quilometragem
        +number preco
        +string status
        +boolean ativo
    }

    class PedidoVenda {
        +number idPedido
        +number idCliente
        +number idCarro
        +Date dataPedido
        +number valorPedido
        +string formaDePagamento
        +string observacoes
        +boolean ativo
    }

    Cliente "1" --> "0..*" PedidoVenda : realiza
    Carro "1" --> "0..1" PedidoVenda : associado
```

------------------------------------------------------------------------

# Requisitos Funcionais

## RF01 -- Cadastro de Clientes

O usuário deve ser capaz de cadastrar um cliente informando os seguintes
dados:

-   Nome --- obrigatório
-   CPF --- obrigatório e deve ser único
-   E-mail --- obrigatório e deve ser único
-   Telefone --- opcional
-   Cidade --- obrigatória
-   UF --- obrigatória

O campo `ativo` deverá ser controlado pelo sistema e não deverá ser
informado pelo usuário durante o cadastro.

------------------------------------------------------------------------

## RF02 -- Listagem de Clientes

O usuário deve ser capaz de listar todos os clientes ativos cadastrados
no sistema.

------------------------------------------------------------------------

## RF03 -- Busca de Cliente por CPF

O usuário deve ser capaz de buscar um cliente pelo CPF.

A busca deverá considerar somente clientes ativos.

------------------------------------------------------------------------

## RF04 -- Atualização de Cliente

O usuário deve ser capaz de atualizar os dados de um cliente.

Durante a atualização, o sistema deverá garantir que o CPF e o e-mail
continuem sendo únicos no sistema.

------------------------------------------------------------------------

## RF05 -- Exclusão de Cliente

O usuário deve ser capaz de remover um cliente.

A remoção deverá ser realizada de forma lógica, mantendo o registro no
banco de dados e alterando seu status para inativo.

------------------------------------------------------------------------

## RF06 -- Cadastro de Veículos

O usuário deve ser capaz de cadastrar um veículo informando os seguintes
dados:

-   Marca --- obrigatória
-   Modelo --- obrigatório
-   Ano --- obrigatório
-   Cor --- obrigatória
-   Placa --- obrigatória e deve ser única
-   Quilometragem --- obrigatória
-   Preço --- obrigatório

O sistema deverá definir automaticamente:

-   `ativo = true`
-   `status = "Disponível"`

O usuário não deverá definir o status do veículo durante o cadastro.

------------------------------------------------------------------------

## RF07 -- Listagem de Veículos

O usuário deve ser capaz de listar todos os veículos ativos cadastrados
no sistema.

------------------------------------------------------------------------

## RF08 -- Busca de Veículo por Placa

O usuário deve ser capaz de buscar um veículo pela placa.

A busca deverá considerar somente veículos ativos.

------------------------------------------------------------------------

## RF09 -- Atualização de Veículo

O usuário deve ser capaz de atualizar os dados de um veículo.

Durante a atualização, o sistema deverá garantir que a placa continue
sendo única no sistema.

O status comercial do veículo deverá ser controlado pelas regras de
negócio do sistema.

------------------------------------------------------------------------

## RF10 -- Exclusão de Veículo

O usuário deve ser capaz de remover um veículo.

A remoção deverá ser realizada de forma lógica, mantendo o registro no
banco de dados e alterando seu status para inativo.

------------------------------------------------------------------------

## RF11 -- Cadastro de Vendas

O usuário deve ser capaz de cadastrar uma venda informando os seguintes
dados:

-   Cliente --- obrigatório
-   Veículo --- obrigatório
-   Data --- obrigatória
-   Valor da venda --- obrigatório
-   Forma de pagamento --- obrigatória
-   Observações --- opcional

As formas de pagamento permitidas são:

-   À Vista
-   Financiado
-   Consórcio

O sistema deverá atualizar automaticamente o status do veículo após uma
venda válida.

------------------------------------------------------------------------

## RF12 -- Listagem de Vendas

O usuário deve ser capaz de listar todas as vendas ativas cadastradas no
sistema.

------------------------------------------------------------------------

## RF13 -- Consulta de Vendas por Data

O usuário deve ser capaz de consultar as vendas realizadas em uma
determinada data.

Como podem existir várias vendas na mesma data, o resultado deverá
permitir o retorno de múltiplas vendas.

------------------------------------------------------------------------

## RF14 -- Atualização de Venda

O usuário deve ser capaz de atualizar os dados de uma venda ativa.

As alterações deverão respeitar as regras de negócio relacionadas ao
cliente, veículo, forma de pagamento e valor da venda.

------------------------------------------------------------------------

## RF15 -- Exclusão de Venda

O usuário deve ser capaz de remover uma venda.

A remoção deverá ser realizada de forma lógica, mantendo o registro no
banco de dados e alterando seu status para inativo.

Quando uma venda for removida, o veículo associado deverá voltar a ficar
disponível para venda, desde que permaneça ativo.

------------------------------------------------------------------------

# Requisitos Não Funcionais

## RNF01 -- Plataforma

A API deverá ser desenvolvida utilizando **C# e .NET 10**.

## RNF02 -- Persistência

Os dados da aplicação deverão ser armazenados em um banco de dados
**PostgreSQL**.

## RNF03 -- ORM

A aplicação deverá utilizar o **Entity Framework Core** para comunicação
com o banco de dados.

## RNF04 -- Arquitetura

A aplicação deverá disponibilizar seus recursos por meio de uma **API
REST**.

## RNF05 -- Formato de Dados

As requisições e respostas da API deverão utilizar o formato **JSON**.

## RNF06 -- Métodos HTTP

A API deverá utilizar adequadamente os métodos HTTP de acordo com a
operação realizada:

-   `GET` para consultas
-   `POST` para criação
-   `PUT` ou `PATCH` para atualização
-   `DELETE` para remoção lógica

## RNF07 -- Códigos HTTP

A API deverá utilizar códigos de status HTTP apropriados para
representar o resultado das operações.

## RNF08 -- Validação

A API deverá validar os dados recebidos antes de realizar operações no
banco de dados.

------------------------------------------------------------------------

# Regras de Negócio

## Clientes

### RN01.1 -- CPF único

Não poderá haver no sistema mais de um cliente com o mesmo CPF.

Essa regra deverá ser aplicada tanto no **cadastro quanto na
atualização** de clientes.

### RN01.2 -- E-mail único

Não poderá haver no sistema mais de um cliente com o mesmo e-mail.

Essa regra deverá ser aplicada tanto no **cadastro quanto na
atualização** de clientes.

### RN02.1 -- Clientes ativos

O sistema deverá retornar somente clientes ativos nas listagens e
buscas.

### RN05.1 -- Exclusão lógica de cliente

Ao remover um cliente, o registro não deverá ser excluído fisicamente do
banco de dados.

O cliente deverá ser marcado como inativo.

### RN05.2 -- Preservação do histórico

A inativação de um cliente não deverá remover ou alterar suas vendas já
registradas.

------------------------------------------------------------------------

## Veículos

### RN06.1 -- Placa única

Não poderá haver no sistema mais de um veículo com a mesma placa.

Essa regra deverá ser aplicada tanto no **cadastro quanto na
atualização** de veículos.

### RN06.2 -- Status inicial do veículo

Todo veículo cadastrado deverá iniciar com:

``` text
ativo = true
status = "Disponível"
```

### RN07.1 -- Veículos ativos

O sistema deverá retornar somente veículos ativos nas listagens e
buscas.

### RN10.1 -- Exclusão lógica de veículo

Ao remover um veículo, o registro não deverá ser excluído fisicamente do
banco de dados.

O veículo deverá ser marcado como inativo.

------------------------------------------------------------------------

## Vendas

### RN11.1 -- Cliente e veículo válidos

Uma venda somente poderá ser realizada caso:

-   o cliente esteja ativo;
-   o veículo esteja ativo;
-   o veículo esteja com status `Disponível`.

### RN11.2 -- Formas de pagamento

A forma de pagamento deverá ser obrigatoriamente uma das seguintes
opções:

``` text
À Vista
Financiado
Consórcio
```

Qualquer outra forma de pagamento deverá ser rejeitada pela API.

### RN11.3 -- Valor mínimo da venda

Para vendas **financiadas ou por consórcio**, o valor da venda não
poderá ser inferior ao preço do veículo.

### RN11.4 -- Desconto à vista

Para vendas realizadas **à vista**, deverá ser aplicado automaticamente
um desconto de **5% sobre o preço do veículo**.

### RN11.5 -- Veículo vendido

Após o cadastramento de uma venda válida, o sistema deverá alterar
automaticamente o status do veículo para:

``` text
Vendido
```

### RN11.6 -- Uma venda ativa por veículo

Um veículo não poderá possuir mais de uma venda ativa.

### RN15.1 -- Exclusão lógica de venda

Ao remover uma venda, o registro não deverá ser excluído fisicamente do
banco de dados.

A venda deverá ser marcada como inativa.

### RN15.2 -- Disponibilidade do veículo

Ao remover uma venda ativa, o sistema deverá alterar automaticamente o
status do veículo associado para:

``` text
Disponível
```

O veículo somente deverá voltar a ser disponibilizado caso esteja ativo.

------------------------------------------------------------------------

# Matriz de Rastreabilidade

A relação entre os requisitos funcionais e as regras de negócio fica
definida da seguinte maneira:

  -----------------------------------------------------------------------
  Requisito               Funcionalidade          Regras de negócio
  ----------------------- ----------------------- -----------------------
  **RF01**                Cadastro de clientes    RN01.1, RN01.2

  **RF02**                Listagem de clientes    RN02.1

  **RF03**                Busca de cliente por    RN02.1
                          CPF                     

  **RF04**                Atualização de cliente  RN01.1, RN01.2

  **RF05**                Exclusão de cliente     RN05.1, RN05.2

  **RF06**                Cadastro de veículos    RN06.1, RN06.2

  **RF07**                Listagem de veículos    RN07.1

  **RF08**                Busca de veículo por    RN07.1
                          placa                   

  **RF09**                Atualização de veículo  RN06.1

  **RF10**                Exclusão de veículo     RN10.1

  **RF11**                Cadastro de vendas      RN11.1, RN11.2, RN11.3,
                                                  RN11.4, RN11.5, RN11.6

  **RF12**                Listagem de vendas      ---

  **RF13**                Consulta de vendas por  ---
                          data                    

  **RF14**                Atualização de venda    RN11.1, RN11.2, RN11.3,
                                                  RN11.4

  **RF15**                Exclusão de venda       RN15.1, RN15.2
  -----------------------------------------------------------------------

------------------------------------------------------------------------

# Estrutura Esperada das Entidades

## Cliente

``` text
Cliente
--------------------------------
idCliente: number
nome: string
cpf: string
telefone: string
email: string
cidade: string
uf: string
ativo: boolean
```

## Carro

``` text
Carro
--------------------------------
idCarro: number
marca: string
modelo: string
ano: number
cor: string
placa: string
quilometragem: number
preco: number
status: string
ativo: boolean
```

## PedidoVenda

``` text
PedidoVenda
--------------------------------
idPedido: number
idCliente: number
idCarro: number
dataPedido: Date
valorPedido: number
formaDePagamento: string
observacoes: string
ativo: boolean
```

------------------------------------------------------------------------

# Observação sobre `ativo` e `status`

Os campos `ativo` e `status` possuem finalidades diferentes.

-   **`ativo`** indica se o registro está ativo no sistema ou se foi
    removido logicamente.
-   **`status`** do veículo indica sua situação comercial.

Exemplo de veículo vendido:

``` text
ativo = true
status = "Vendido"
```

Exemplo de veículo removido logicamente:

``` text
ativo = false
status = "Disponível"
```

Essa separação permite preservar o histórico do sistema sem confundir
**remoção do cadastro** com **venda do veículo**.
