select * 
from funcionarios
where salario > (select avg(salario)
					from funcionarios)

-- O código de cima fica igual esse código
-- select avg(salario)
-- from funcionarios

-- select * 
-- from funcionarios
-- where salario >2125

-- Não controla como ele arredonda
select round(avg(salario))
from funcionarios

-- Controla como ele arredonda 
select ceil(4.1), floor(4.9)

-- Transforma o negativo (*-1)
select abs(-4.1)

-- Sempre entre 0 - 1
select random()
-- De 1 - 11
select (random() * 10 + 1)::int

-- Horário de brasília
select now()
-- Horário local
select now() at time zone current_setting('timezone');

-- Conta o tempo que passou da data até agr(now())
select age(now(),'2025-01-01')

-- Isola parte da data
select extract(day from now()),
		extract(month from now()),
		extract(year from now()),
		extract(hour from now()),
		extract(minute from now()),
		extract(second from now()),
		now()


-- Mais ou menos do tempo atual
select now() + interval '1 year'
select now() - interval '1 year'

-- Agregação em linha (Window function), adiciona uma coluna com o cálculo
select ve.id, ve.cliente_id, cli.nome, ve.valor, 
sum(ve.valor) over(partition by ve.cliente_id) as total_cliente,
count(ve.valor) over(partition by ve.cliente_id) as qtde_vendas
from vendas ve
inner join clientes cli on cli.id = ve.cliente_id

-- Ranking (posição) das consultas
select *
from (select ve.cliente_id, cli.nome,
	sum(ve.valor), 
	rank() over (order by sum(ve.valor) desc) as ranking
from vendas ve
inner join clientes cli on cli.id = ve.cliente_id
group by ve.cliente_id, cli.nome) a
where ranking between 1 and 3


-- 	Ex 1
select id, valor, (select round(valor,2)
					from vendas v2
					where v2.id = v1.id )as valor_arredondado
from vendas v1

-- Ex 2
select id, valor, extract(year from data_venda) as ano_venda
from vendas v1

-- Ex 3
select id, valor, age(now(), data_venda)
from vendas v1

-- Ex 4
select ve.id, ve.cliente_id, ve.valor, 
	sum(ve.valor) over(partition by ve.cliente_id) as total_cliente
from vendas ve

-- Ex 5
select cli.nome,
	sum(ve.valor) over(partition by ve.cliente_id) as total_cliente,
	rank() over (order by sum(ve.valor) desc) as ranking
from vendas ve
inner join clientes cli on cli.id = ve.cliente_id
group by ve.cliente_id, cli.nome, ve.valor
order by ranking

