-- Script para criar as tabelas com relacionamentos 
CREATE TABLE "TB_CATEGORIAS" ( 
    "id_categoria" SERIAL PRIMARY KEY, 
    "nome_categoria" VARCHAR(100) NOT NULL UNIQUE 
); 
CREATE TABLE "TB_PRODUTOS" ( 
    "id_produto" SERIAL PRIMARY KEY, 
    "nome_produto" VARCHAR(150) NOT NULL, 
    "preco_produto" DECIMAL(10, 2) NOT NULL, 
    "estoque_produto" INT NOT NULL, 
    "id_categoria" INT NOT NULL, 
    CONSTRAINT "fk_categoria" FOREIGN KEY ("id_categoria") REFERENCES 
"TB_CATEGORIAS"("id_categoria") 
); 
INSERT INTO "TB_CATEGORIAS" ("nome_categoria") VALUES ('Eletrônicos'), ('Livros'), 
('Vestuário'); 
INSERT INTO "TB_PRODUTOS" ("nome_produto", "preco_produto", "estoque_produto", 
"id_categoria") VALUES 
('Smartphone XYZ', 1999.99, 50, 1), 
('Notebook Gamer ABC', 7500.00, 15, 1), 
('O Senhor dos Anéis', 120.50, 100, 2), 
('Camiseta Básica', 49.90, 200, 3); 

-- Table: public.TB_CLIENTES

-- DROP TABLE IF EXISTS public."TB_CLIENTES";


-- Table: public.TB_CLIENTES

-- DROP TABLE IF EXISTS public."TB_CLIENTES";

CREATE TABLE IF NOT EXISTS public."TB_CLIENTES"
(
    id_cliente SERIAL PRIMARY KEY,
    nome_cliente character varying(150) NOT NULL,
    email_cliente character varying(150) NOT NULL,
    ativo boolean NOT NULL DEFAULT true,
    data_cadastro timestamp with time zone NOT NULL
)

-- Script do "DBA" 
CREATE TABLE "TB_CLIENTES" ( 
    "id_cliente" SERIAL PRIMARY KEY, 
    "nome_cliente" VARCHAR(150) NOT NULL, 
    "email_cliente" VARCHAR(150) NOT NULL UNIQUE, 
    "ativo" BOOLEAN NOT NULL DEFAULT true  
); 
 -- Inserindo alguns dados iniciais 
INSERT INTO "TB_CLIENTES" ("nome_cliente", "email_cliente", "ativo", "data_cadastro") 
VALUES 
('ALICE SILVA', 'alice@mail.com', true, NOW()), 
('BRUNO COSTA', 'bruno@mail.com', true, NOW()), 
('CARLOS SANTOS', 'carlos@mail.com', false, NOW()); 


SELECT *
	FROM public."TB_CLIENTES";

-- Relacionamento 1:1 com Clientes
CREATE TABLE "TB_ENDERECOS" (
    "id_cliente" INT PRIMARY KEY, -- Mesma chave primária de Clientes
    "rua" VARCHAR(200) NOT NULL,
    "cidade" VARCHAR(100) NOT NULL,
    "estado" VARCHAR(50) NOT NULL,
    "cep" VARCHAR(10) NOT NULL,
    CONSTRAINT "fk_cliente_endereco" FOREIGN KEY ("id_cliente") REFERENCES "TB_CLIENTES"("id_cliente")
);
 