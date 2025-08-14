select *
 from clientes
 where idade > 32
 and nome ilike '%a%'
 and id in (1,2,4,5,6,8,9)

 select *
 from produtos
 where categoria is not null
 order by nome desc

inne
 select * 
 	 from clientes cli
	 inner join pedidos ped
	 on cli.id = ped.cliente_id

select * 
 	from pedidos


