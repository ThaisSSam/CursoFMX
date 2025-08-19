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
	),
	insert_teste as(
		insert into tabel
	)
select vendas.id, vendas.valor, vendas.data_venda, clientes.nome
	from vendas
	inner join clientes on clientes.id= vendas.cliente_id



create table tabela_teste(
	id int,
	valor numeric(10,2),
	data_venda date,
	nome character vaying(100)
	)

	