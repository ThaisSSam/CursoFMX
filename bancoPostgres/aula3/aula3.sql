select *
 from clientes
 where idade > 32
 and nome ilike '%a%'
 and id in (1,2,4,5,6,8,9)

 select *
 from produtos
 where categoria is not null
 order by nome desc

-- inner -> 1=1, oq tem nos dois lados
 select cli.id,
 		cli.nome,
		cli.cidade, 
		ped.valor
 	 from clientes cli
	 inner join pedidos ped
	 on cli.id = ped.cliente_id
	 where cli.id in (1,2,3)
	 and ped.valor = 300.00

-- traz os dados do lado esquerdo e trazer os dados corespondentes a eles 
 select cli.id,
 		cli.nome,
		cli.cidade, 
		ped.valor
 	 from clientes cli
	 left join pedidos ped
	 on cli.id = ped.cliente_id
	 where ped.valor is not null 

-- traz os dados do lado direito e trazer os dados corespondentes a eles 
 select cli.id,
 		cli.nome,
		cli.cidade, 
		ped.valor
 	 from clientes cli
	 right join pedidos ped
	 on cli.id = ped.cliente_id
	 where ped.valor is not null 

 -- traz os dados dos dois por inteiro
 select cli.id,
 		cli.nome,
		cli.cidade, 
		ped.valor
 	 from clientes cli
	 full join pedidos ped
	 on cli.id = ped.cliente_id
	 where ped.valor is not null 

-- multiplica as duas tabelas 
 select *
 	 from clientes cli
	 cross join pedidos ped


select *
 	 from clientes cli1
	 left join clientes cli2
	 on  cli1.id = cli2.id

-- Ex 1
select cli.nome,
	ped.status
	from clientes cli
	right join pedidos ped 
	on cli.id = ped.cliente_id

-- Ex 2
select cli.nome,
	ped.valor
	from clientes cli
	left join pedidos ped
	on cli.id = ped.cliente_id

-- Ex 3
select ped.id,
	ped.valor,
	ped.status,
	cli.nome
	from clientes cli
	left join pedidos ped
	on cli.id = ped.cliente_id

-- Ex 4
select cli.nome,
	ped.id,
	ped.valor
	from pedidos ped
	full join clientes cli
	on ped.cliente_id = cli.id

-- Ex 5
select *
	from funcionarios func
	left join gerentes ger
	on func.id_gerente = ger.id_gerente
