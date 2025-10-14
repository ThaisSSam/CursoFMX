CREATE TABLE "TB_PRODUTOS" ( 
    "id_produto" SERIAL PRIMARY KEY, 
    "nome_produto" VARCHAR(150) NOT NULL, 
    "valor_produto" NUMERIC(10,2) NOT NULL, 
	"descricao_produto" VARCHAR(500) NOT NULL, 
    "estoque_produto" INTEGER NOT NULL, 
    "deletado" BOOLEAN NOT NULL DEFAULT false
); 

-- Inserindo alguns dados iniciais 
INSERT INTO "TB_PRODUTOS" ("nome_produto", "valor_produto", "descricao_produto", "estoque_produto","deletado") 
VALUES 
('MOUSE', '59.99', 'Mouse com fio', '20', false), 
('TECLADO', '89.99', 'Teclado sem fio', '30', false), 
('MONITOR', '179.99', 'Monitor', '10', true)
