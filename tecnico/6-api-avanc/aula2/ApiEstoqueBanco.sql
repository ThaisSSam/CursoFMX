-- Criar a tabela de Fabricantes
CREATE TABLE fabricante (
    id SERIAL PRIMARY KEY,
    nome VARCHAR(100) NOT NULL
);

-- Criar a tabela de Produtos
CREATE TABLE produto (
    id SERIAL PRIMARY KEY,
    nome VARCHAR(100) NOT NULL,
    preco DECIMAL(10, 2) NOT NULL,
    fabricante_id INT NOT NULL,
    
    -- Chave Estrangeira
    CONSTRAINT fk_fabricante 
        FOREIGN KEY (fabricante_id) 
        REFERENCES fabricante(id) 
        ON DELETE CASCADE
);

-- Inserindo Fabricantes
INSERT INTO fabricante (nome) VALUES ('TechMaster');
INSERT INTO fabricante (nome) VALUES ('Global Eletrônicos');

-- Inserindo Produtos (referenciando os IDs 1 e 2)
INSERT INTO produto (nome, preco, fabricante_id) VALUES ('Teclado Mecânico', 250.00, 1);
INSERT INTO produto (nome, preco, fabricante_id) VALUES ('Monitor 24"', 899.90, 2);

SELECT *
FROM produtos p
JOIN fabricantes f ON p.fabricante_id = f.id;

ALTER TABLE produto RENAME TO produtos;
ALTER TABLE fabricante RENAME TO fabricantes;

ALTER TABLE produtos RENAME COLUMN fabricante_id TO fabricanteid;