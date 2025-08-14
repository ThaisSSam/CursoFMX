select *
	from clientes
	where estado <> 'SP'


select *
	from clientes
	where idade between 20 and 40


select cli.*, ped.status, pro.*
	from clientes cli
	inner join pedidos ped 				on ped.cliente_id = cli.id
	inner join pedido_produto pedpro 	on pedpro.id_pedidos = ped.id
	inner join produtos pro 			on pro.id = pedpro.id_produtos


select *
	from clientes cli1
	inner join clientes cli2 on cli1.id = cli2.id
	
UPDATE funcionarios
SET salario = 4000
WHERE id_funcionario = 4;

select *
	from funcionarios

-- transformar em um 'objeto' compilado dentro do banco pra rodar bem mais rápido 
-- armazena cada select em memória
select nome, (select salario
				from funcionarios func3
				where func3.id_funcionario= funcionarios.id_funcionario) salario
	from funcionarios
	where salario in (select salario
						from funcionarios func1)

-- CTE transformar em uma 'tabela temporária'
-- armazena tudo em memória 
with cliente_vendas as(
select ven.id, cli.nome, ven.valor, ven.data_venda
	from vendas ven
	inner join clientes cli on cli.id = ven.cliente_id)
select *
	from cliente_vendas
	where valor > 1000
	
with vendas as(
	select ven.*
	from vendas ven
	where valor > 1000
	), 	
	clientes as(
		select cli.*
		from clientes cli
	)
select vendas.id, vendas.valor, vendas.data_venda, clientes.nome
	from vendas
	inner join clientes on clientes.id= vendas.cliente_id

-- colocar o insert em uma tabela temporária
create table tabela_teste(
	id int,
	valor numeric(10,2),
	data_venda date,
	nome character varying(100))
	
with vendas as(
	select ven.*
	from vendas ven
	where valor > 1000
	), 	
	clientes as(
		select cli.*
		from clientes cli
	),
	insert_teste as(
		insert into tabela_teste values(10,10,'2025-08-14', 'itb')
		returning *
	)
	select * from tabela_teste



-- Ex 1
select cli.id, ven.valor
	from vendas ven
	inner join clientes cli on cli.id = ven.cliente_id
	where valor > (select avg(valor)
						from vendas)
-- Ex 2
select ven.*, (select sum(ven.valor)
				from vendas ven
				where cli.id = ven.cliente_id
				) total
	from vendas ven
	inner join clientes cli on cli.id = ven.cliente_id
	where valor in (select ven.valor
					from vendas ven
					inner join clientes cli on cli.id = ven.cliente_id
					)
	order by ven.cliente_id
	
-- Ex 3
with valor as(
	select cli.id, sum(ven.valor)
	from vendas ven
	inner join clientes cli on cli.id = ven.cliente_id
	where cli.id = ven.cliente_id
	group by cli.id 
	)
select *
	from valor
	

-- Ex 4
select upper(nome)
from clientes

-- Ex 5
select substring(nome from 1 for 3)
from clientes



	