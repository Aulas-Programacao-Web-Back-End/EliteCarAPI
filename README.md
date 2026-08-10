# EliteCar API
API REST desenvolvida em C# com .NET 10 para o controle de estoque de veiculos.  
Um projeto estudantil para aprendizado de desenvolvimento de APIs REST no curso técnico em Desenvolvimento de Sistemas.

## Tecnologias Utilizadas
- C#
- .NET 10
- Entity Framework Core
- PostgreSQL

## Diagrama de Classes
![Diagrama de Classes](/assets/EliteCar-ClassDiagram.png)

## Requisitos Funcionais e Não Funcionais

### RF01: Cadastro de Clientes
O usuário deve ser capaz de cadastrar um cliente informando os seguintes dados:
- Nome (obrigatório)
- Email (obrigatório e deve ser único)
- Telefone (opcional)
- CPF (obrigatório e deve ser único)
    
### RF02: Listagem de Clientes
O usuário deve ser capaz de listar todos os clientes cadastrados.

### RF03: Busca de Cliente por CPF
O usuário deve ser capaz de buscar um cliente pelo CPF.

### RF04: Atualização de Cliente
O usuário deve ser capaz de atualizar os dados de um cliente.

### RF05: Exclusão de Cliente
O usuário deve ser capaz de excluir um cliente.

### RF06: Cadastro de Veículos
O usuário deve ser capaz de cadastrar um veículo informando os seguintes dados:
- Marca (obrigatório)
- Modelo (obrigatório)
- Ano (obrigatório)
- Placa (obrigatório e deve ser única)
- Preço (obrigatório)
    
### RF07: Listagem de Veículos
O usuário deve ser capaz de listar todos os veículos cadastrados.

### RF08: Busca de Veículo por Placa
O usuário deve ser capaz de buscar um veículo pela placa.

### RF09: Atualização de Veículo
O usuário deve ser capaz de atualizar os dados de um veículo.

### RF10: Exclusão de Veículo
O usuário deve ser capaz de excluir um veículo.

### RF11: Cadastro de Vendas
O usuário deve ser capaz de cadastrar uma venda informando os seguintes dados:
- Cliente (obrigatório)
- Veículo (obrigatório)
- Data (obrigatório)
- Preço (obrigatório)
    
### RF12: Listagem de Vendas
O usuário deve ser capaz de listar todas as vendas cadastradas.

### RF13: Busca de Venda por Data
O usuário deve ser capaz de buscar uma venda por data.

### RF14: Atualização de Venda
O usuário deve ser capaz de atualizar os dados de uma venda.

### RF15: Exclusão de Venda
O usuário deve ser capaz de excluir uma venda.

## Regras de negócios
**RN01.1**: Não poderá haver no sistema mais de um cliente com o mesmo CPF.  
**RN02.1**: O sistema só deve retornar os clientes ativos.  
**RN05.1**: Ao remover um cliente, o mesmo não deve ser excluído do banco de dados, mas sim marcado como inativo.  
**RN06.1**: Não poderá haver no sistema mais de um veículo com a mesma placa.  
**RN07.1**: O sistema só deve retornar os veículos ativos.    
**RN10.1**: Ao remover um veículo, o mesmo não deve ser excluído do banco de dados, mas sim marcado como inativo.  
**RN15.1**: Ao remover uma venda, o mesmo não deve ser excluído do banco de dados, mas sim marcado como inativo.  
**RN11.1**: A venda só pode ser realizado caso o cliente e o veículo estejam ativos.  
**RN11.2**: A forma de pagamento deverá ser apenas, "Á Vista", "Financiado" ou "Consórcio".  
**RN11.3**: O valor da venda não pode ser menor que o valor do veículo.  
**RN11.4**: Caso a venda seja "Á Vista", um desconto de 5% deve ser aplicado sobre o valor do veículo.  
**RF15.1**: Caso uma venda seja excluída, o carro deve estar disponível para venda novamente.  
**RF11.5**: O status do veículo deve ser "Vendido" caso a venda seja realizada, e "Disponível" caso a venda seja excluída.