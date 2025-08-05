-- Tem o commit implícito
create table tabela_modelo (
  codigo int,
  descricao char(50)) 

insert into tabela_modelo (codigo, descricao)
 	values (1, 'MODELO 1')

insert into tabela_modelo (codigo, descricao)
	values (2, 'MODELO 2')

insert into tabela_modelo (codigo, descricao)
	values (3, 'MODELO 3')

select * 
	from tabela_modelo

select descricao
	from tabela_modelo

select * 
	from tabela_modelo
	where codigo != 2

select * 
	from tabela_modelo
	where codigo >= 1
	and codigo <= 3

select * 
	from tabela_modelo
	where codigo between 1 and 3

insert into tabela_modelo (codigo, descricao)
	values (4, 'MODELO 4')

insert into tabela_modelo 
	values (5, 'MODELO 5')

insert into tabela_modelo 
	values (6)

select * 
	from tabela_modelo

UPDATE tabela_modelo
SET descricao = 'MODELO 6'
WHERE codigo = 6

insert into tabela_modelo 
	values (7, 'MODELO ANTIGO 7')

DELETE 
FROM tabela_modelo
WHERE codigo = 7

-- Tem o commit implícito
-- TRUNCATE 
-- table tabela_modelo
-- WHERE codigo = 7

create table nome_tabela(
	codigo int, 
	descricao character varying
)
-- Utiliza 50 de espaço com o char
-- create table tabela_modelo1(
-- 	codigo int, 
-- 	descricao char(50)
-- )

alter table nome_tabela
add column tipo char(1)

select * 
	from nome_tabela

alter table nome_tabela
drop column tipo

drop table nome_tabela