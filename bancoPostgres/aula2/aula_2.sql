CREATE TABLE clientes (
    id SERIAL PRIMARY KEY,
    nome VARCHAR(100),
    idade INT,
    cidade VARCHAR(100),
    estado VARCHAR(2)
);

CREATE TABLE produtos (
    id SERIAL PRIMARY KEY,
    nome VARCHAR(100),
    preco NUMERIC(10,2),
    categoria VARCHAR(50)
);

CREATE TABLE vendas (
    id SERIAL PRIMARY KEY,
    cliente_id INT REFERENCES clientes(id),
    valor NUMERIC(10,2),
    data_venda DATE
);

CREATE TABLE pedidos (
    id SERIAL PRIMARY KEY,
    cliente_id INT REFERENCES clientes(id),
    valor NUMERIC(10,2),
    status VARCHAR(20)
);

INSERT INTO clientes (nome, idade, cidade, estado) VALUES 
('João Silva', 35, 'São Paulo', 'SP'),
('Maria Souza', 28, 'Campinas', 'SP'),
('José Oliveira', 42, 'Rio de Janeiro', 'RJ'),
('Ana Paula', 33, 'São Paulo', 'SP'),
('Carlos Mendes', 19, 'Belo Horizonte', 'MG'),
('Julia Ferreira', 38, 'Curitiba', 'PR'),
('Bruno Lima', 25, 'São Paulo', 'SP'),
('Juliana Costa', 40, 'Fortaleza', 'CE'),
('Fernanda Gomes', 31, 'Recife', 'PE');

-- PRODUTOS
INSERT INTO produtos (nome, preco, categoria) VALUES 
('Notebook Dell', 4500.00, 'Eletrônicos'),
('Mouse Logitech', 150.00, 'Eletrônicos'),
('Camiseta Branca', 49.90, 'Roupas'),
('Smartphone Samsung', 3200.00, 'Eletrônicos'),
('Calça Jeans', 120.00, 'Roupas'),
('Fone JBL', 399.00, 'Eletrônicos'),
('Ventilador', 200.00, 'Eletrodomésticos');

-- VENDAS
INSERT INTO vendas (cliente_id, valor, data_venda) VALUES
(1, 4500.00, '2024-10-01'),
(2, 150.00, '2024-10-02'),
(3, 3200.00, '2024-10-05'),
(4, 200.00, '2024-10-06'),
(1, 399.00, '2024-10-07'),
(5, 120.00, '2024-10-08'),
(6, 250.00, '2024-10-10');

-- PEDIDOS
INSERT INTO pedidos (cliente_id, valor, status) VALUES
(1, 300.00, 'finalizado'),
(1, 200.00, 'finalizado'),
(1, 100.00, 'pendente'),
(2, 500.00, 'finalizado'),
(2, 120.00, 'finalizado'),
(3, 600.00, 'finalizado'),
(3, 200.00, 'finalizado'),
(3, 100.00, 'finalizado'),
(3, 150.00, 'finalizado'),
(4, 400.00, 'cancelado'),
(5, 800.00, 'finalizado');

select id, nome, categoria
from produtos
where categoria = 'Eletrônicos'

begin 
	delete 
		from produt
