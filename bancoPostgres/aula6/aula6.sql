select cliente_id, valor, data_venda,
	row_number() over (partition by cliente_id order by data_venda) as numero_venda
from vendas ve

-- busca o anterior e o posterior
select cliente_id, valor, data_venda,
	lag(valor) over (order by cliente_id, data_venda) as venda_anterior,
	lead(valor) over (order by cliente_id, data_venda) as proxima_venda
from vendas
order by data_venda

-- busca o anterior e posterior por cada cliente(partition)
select cliente_id, valor, data_venda,
	lag(valor) over (partition by cliente_id order by cliente_id, data_venda) as venda_anterior,
	lead(valor) over (partition by cliente_id  order by cliente_id, data_venda) as proxima_venda
from vendas
order by cliente_id, data_venda

-- pula ou não o ordenamento em posições de empate
select cliente_id, 
	sum(valor) as total,
	rank() over(order by sum(valor) desc)as pos_rank,
	dense_rank() over(order by sum(valor) desc)as pos_rank,
	rank() over(partition by cliente_id order by sum(valor) desc)as pos_rank,
	dense_rank() over(partition by sum(valor) order by sum(valor) desc)as pos_rank
from vendas
group by cliente_id
order by total desc

with total_cliente as (
	select cliente_id,
		sum(valor) as total,
		rank() over (order by sum (valor) desc)as pos_rank
	from vendas
	group by cliente_id)
select *
from total_cliente
where pos_rank =2

-- acumula as funções sem contar ele mesmo
select cliente_id, data_venda, valor,
	sum(valor) over (partition by cliente_id order by data_venda),
	avg(valor) over (partition by cliente_id order by data_venda),
	count(valor) over (partition by cliente_id order by data_venda),
	min(valor) over (partition by cliente_id order by data_venda),
	max(valor) over (partition by cliente_id order by data_venda)
from vendas
order by cliente_id, data_venda

-- acumula contando ele mesmo
SELECT
  ve.id,
  ve.cliente_id,
  ve.data_venda,
  ve.valor,
  SUM(ve.valor) OVER (
    ORDER BY ve.data_venda
    ROWS BETWEEN UNBOUNDED PRECEDING AND CURRENT ROW
  ) AS soma_acumuladas_anterior,
  SUM(ve.valor) OVER (
    ORDER BY ve.data_venda
    ROWS BETWEEN CURRENT ROW AND UNBOUNDED FOLLOWING
  ) AS soma_acumuladas_proximas,
  LAG(ve.valor) OVER (ORDER BY ve.data_venda) AS valor_anterior,
  LEAD(ve.valor) OVER (ORDER BY ve.data_venda) AS valor_proximo
FROM vendas ve
ORDER BY ve.data_venda;

-- view: cria um objeto com as informações || Evitar o where(filtro)
create or replace view vw_somas_acumuladas as
SELECT
  ve.id,
  ve.cliente_id,
  ve.data_venda,
  ve.valor,
  SUM(ve.valor) OVER (
    ORDER BY ve.data_venda
    ROWS BETWEEN UNBOUNDED PRECEDING AND CURRENT ROW
  ) AS soma_acumuladas_anterior,
  SUM(ve.valor) OVER (
    ORDER BY ve.data_venda
    ROWS BETWEEN CURRENT ROW AND UNBOUNDED FOLLOWING
  ) AS soma_acumuladas_proximas,
  LAG(ve.valor) OVER (ORDER BY ve.data_venda) AS valor_anterior,
  LEAD(ve.valor) OVER (ORDER BY ve.data_venda) AS valor_proximo
FROM vendas ve 
ORDER BY ve.data_venda;

-- busca a view
select *
	from vw_somas_acumuladas
	where cliente_id =1

-- exclui view
drop view vw_somas_acumuladas


-- Ex1 
with vendas_cliente as (
	select cliente_id,data_venda,
		row_number() over (partition by cliente_id  order by data_venda) as compras_cliente
	from vendas
	group by cliente_id, data_venda)
select *
from vendas_cliente
where compras_cliente =1

-- Ex2
with total_cliente as (
	select cliente_id,
		sum(valor) as total,
		rank() over (order by sum (valor) asc)as valor_rank,
		dense_rank() over(partition by sum(valor) order by sum(valor) asc)as valor_rank
	from vendas
	group by cliente_id)
select *
from total_cliente

-- Ex3
with venda_anterior as (
	select cliente_id, valor,
		lag(valor) over (order by cliente_id, data_venda) as valor_anterior
	from vendas)
select *, (valor-valor_anterior)as diferenca_vendas
from venda_anterior

-- Ex4
create or replace view total_vendas_mes as
	select extract(month from data_venda)as mes,
		sum(valor)as total_vendas
	from vendas
	group by mes
	order by mes

-- Ex5
	select *
	from total_vendas_mes
	where total_vendas>1000