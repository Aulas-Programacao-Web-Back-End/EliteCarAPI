-- =============================================================
-- EliteCar API — Script de Inicialização do Banco de Dados
-- Banco  : PostgreSQL (Supabase)
-- Tabelas: clientes | carros | pedidos_venda
--
-- ATENÇÃO: Execute este script APÓS aplicar as migrations do
-- Entity Framework Core (dotnet ef database update).
--
-- O script pode ser executado mais de uma vez com segurança,
-- pois trunca as tabelas antes de inserir.
-- =============================================================

-- -------------------------------------------------------------
-- Limpeza (ordem respeitando FK)
-- -------------------------------------------------------------
TRUNCATE TABLE pedidos_venda RESTART IDENTITY CASCADE;
TRUNCATE TABLE carros         RESTART IDENTITY CASCADE;
TRUNCATE TABLE clientes       RESTART IDENTITY CASCADE;


-- =============================================================
-- CLIENTES (8 registros — 7 ativos, 1 inativo)
-- Regras: CPF único (RN01.1), e-mail único (RN01.2)
-- =============================================================
INSERT INTO clientes (nome, cpf, telefone, email, cidade, uf, ativo) VALUES
    ('Ana Silva',        '111.222.333-44', '(11) 99999-1111', 'ana.silva@elitecar.com',      'São Paulo',       'SP', TRUE),
    ('Bruno Oliveira',   '222.333.444-55', '(21) 98888-2222', 'bruno.oliveira@elitecar.com', 'Rio de Janeiro',  'RJ', TRUE),
    ('Carla Mendes',     '333.444.555-66', '(31) 97777-3333', 'carla.mendes@elitecar.com',   'Belo Horizonte',  'MG', TRUE),
    ('Diego Santos',     '444.555.666-77', '(41) 96666-4444', 'diego.santos@elitecar.com',   'Curitiba',        'PR', TRUE),
    ('Elisa Ferreira',   '555.666.777-88', NULL,              'elisa.ferreira@elitecar.com', 'Porto Alegre',    'RS', TRUE),
    ('Fábio Costa',      '666.777.888-99', '(71) 95555-6666', 'fabio.costa@elitecar.com',    'Salvador',        'BA', TRUE),
    ('Gabriela Lima',    '777.888.999-00', '(85) 94444-7777', 'gabriela.lima@elitecar.com',  'Fortaleza',       'CE', TRUE),
    ('Henrique Rocha',   '888.999.000-11', '(62) 93333-8888', 'henrique.rocha@elitecar.com', 'Goiânia',         'GO', FALSE); -- cliente inativo (exclusão lógica)


-- =============================================================
-- CARROS (11 registros — 10 ativos, 1 inativo)
-- Regras: placa única (RN06.1), status inicial = 'Disponível' (RN06.2)
--
-- status 'Vendido'    → veículo com venda ativa (RN11.5)
-- status 'Disponível' → veículo livre para venda
-- ativo  = FALSE      → removido logicamente (RN10.1)
-- =============================================================
INSERT INTO carros (marca, modelo, ano, cor, placa, quilometragem, preco, status, ativo) VALUES
    -- Carros VENDIDOS (possuem pedido de venda ativo)
    ('Toyota',          'Corolla',   2022, 'Prata',    'ABC1D23',  25000,  95000.00, 'Vendido',    TRUE),
    ('Honda',           'Civic',     2023, 'Preto',    'DEF2E34',   8000, 120000.00, 'Vendido',    TRUE),
    ('Volkswagen',      'T-Cross',   2022, 'Branco',   'GHI3F45',  32000, 105000.00, 'Vendido',    TRUE),
    ('BMW',             '320i',      2022, 'Azul',     'JKL4G56',  15000, 230000.00, 'Vendido',    TRUE),

    -- Carro com venda CANCELADA → voltou a 'Disponível' (RN15.2)
    ('Nissan',          'Kicks',     2023, 'Azul',     'MNO5H67',  12000,  98000.00, 'Disponível', TRUE),

    -- Carros DISPONÍVEIS para venda
    ('Jeep',            'Renegade',  2021, 'Vermelho', 'PQR6I78',  45000,  89000.00, 'Disponível', TRUE),
    ('Ford',            'Ranger',    2023, 'Prata',    'STU7J89',   5000, 195000.00, 'Disponível', TRUE),
    ('Chevrolet',       'Onix',      2022, 'Cinza',    'VWX8K90',  38000,  75000.00, 'Disponível', TRUE),
    ('Hyundai',         'HB20',      2021, 'Branco',   'YZA9L01',  52000,  68000.00, 'Disponível', TRUE),
    ('Mercedes-Benz',   'C180',      2021, 'Preto',    'BCD0M12',  28000, 210000.00, 'Disponível', TRUE),

    -- Carro INATIVO (removido logicamente — RN10.1)
    ('Renault',         'Kwid',      2022, 'Laranja',  'EFG1N23',  61000,  45000.00, 'Disponível', FALSE);


-- =============================================================
-- PEDIDOS DE VENDA (5 registros — 4 ativos, 1 inativo)
--
-- Regras de negócio aplicadas nos valores:
--   RN11.3 → Financiado/Consórcio: valor_pedido >= preco do carro
--   RN11.4 → À Vista: valor_pedido = preco * 0.95 (desconto de 5%)
--   RN11.5 → Carro associado passa para status 'Vendido'
--   RN15.2 → Venda inativa → carro volta para 'Disponível'
-- =============================================================
INSERT INTO pedidos_venda (id_cliente, id_carro, data_pedido, valor_pedido, forma_de_pagamento, observacoes, ativo) VALUES

    -- Pedido 1 — Financiado | Toyota Corolla R$ 95.000
    -- RN11.3: valor (95.000) >= preço (95.000) ✓
    (1, 1, '2026-02-10',  95000.00, 'Financiado',  'Financiamento em 48 parcelas. Entrada de 20%.', TRUE),

    -- Pedido 2 — À Vista | Honda Civic R$ 120.000
    -- RN11.4: valor = 120.000 × 0,95 = 114.000 ✓
    (2, 2, '2026-03-15', 114000.00, 'À Vista',     NULL, TRUE),

    -- Pedido 3 — Consórcio | Volkswagen T-Cross R$ 105.000
    -- RN11.3: valor (108.000) >= preço (105.000) ✓
    (3, 3, '2026-04-22', 108000.00, 'Consórcio',   'Consórcio com prazo de 60 meses.', TRUE),

    -- Pedido 4 — À Vista | BMW 320i R$ 230.000
    -- RN11.4: valor = 230.000 × 0,95 = 218.500 ✓
    (4, 4, '2026-06-01', 218500.00, 'À Vista',     'Cliente VIP — desconto automático de 5% aplicado.', TRUE),

    -- Pedido 5 — INATIVO (venda cancelada) | Nissan Kicks R$ 98.000
    -- RN15.1: exclusão lógica; RN15.2: carro (id=5) voltou para 'Disponível'
    -- RN11.4: valor = 98.000 × 0,95 = 93.100 ✓
    (5, 5, '2026-05-03',  93100.00, 'À Vista',     'Venda cancelada a pedido do cliente.', FALSE);


-- =============================================================
-- Atualiza as sequências para evitar conflitos após os INSERTs
-- =============================================================
SELECT setval(pg_get_serial_sequence('clientes',     'id_cliente'), MAX(id_cliente))     FROM clientes;
SELECT setval(pg_get_serial_sequence('carros',       'id_carro'),   MAX(id_carro))       FROM carros;
SELECT setval(pg_get_serial_sequence('pedidos_venda','id_pedido'),   MAX(id_pedido))      FROM pedidos_venda;
