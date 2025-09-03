select -- colunas específicas
where --linhas específicas

select id,nome, idade
from clientes
where id <5


insert into cliente (coluna1,coluna2)
	values (1,2), (2,3), (3,4)...;


update clientes 
set campo = 'nova informação'
where filtro = 'informação' --se não tiver where att tudo

delete
from clientes
where nome = 'Ana Paula'

truncate table cliente --não tem where, commit implicito

create 
-- tabela, view, procedures, functions, triggers

create or alter procedure -- o msm que:
-- drop procedure 
-- create procedure

drop --apaga objetos

where 
= --igual
<> --diferente
> --maior
< --menor
<= >= --menor ou maior igual
like '%itb%' --case sensiive, % é o resto da frase
ilike '%itb' -- não case sensitive
idade between 10 and 20 -- idade entre 10 e 20
idade in(1,2,3,4,5) --conjunto 
idade not in (1,2,3,4,5) --not negação antes de qualquer um desses


select *
from clientes
where cidade is null

select *
from clientes
where cidade is not null

select *
from clientes
order by nome --normal crescente

select *
from clientes
order by idade asc, nome desc-- decrescente

select count(1) from clientes -- mais performático
select count(*) from clientes --carrega mais dados 
select count(id) from clientes --carrega os dados de uma coluna específica
select count(cidade) from clientes --não conta null

select id, sum(valor) --soma
 	, avg(valor) --média
	 ,min(valor) --valor mínimo
	 ,max(valor) --valor máximo
from vendas
group by id --precisa com outra coluna
having sum(valor) < 1300 --'where' de agrupamento


select *
from clientes cli
inner join pedidos ped on ped.cliente_id = cli.id
-- comparação direta 1 = 1


select *
from clientes cli
left join pedidos ped on ped.cliente_id = cli.id
--  lado esquerdo (clientes) mais importante 1=0

select *
from clientes cli
left join pedidos ped on ped.cliente_id = cli.id
--  lado direito (pedidos) mais importante 0=1

select *
from clientes cli
natural join pedidos ped
--identifica o vínculo mais rápido pra fazer o join


select *
from clientes cli
full join pedidos ped on ped.cliente_id = cli.id
-- traz todo mundo 0=0

select *
from clientes cli
cross join pedidos ped on ped.cliente_id = cli.id
-- plano cartesiano,traz tudo pra tudo multiplicando 


select *
from funcionarios
where salario > (select min(salario)
				from funcionarios)
-- oq modificar em um modifica em todos


with total_por_clientes as(
	select cliente_id, sum(valor) as total
	from vendas
	group by cliente_id
)
select *
from total_por_clientes
where total>1000


select v.id, v.cliente_id, v.valor,
	sum(v.valor) over (partition by v.cliente_id) as total_cliente,
	rank() over(partition by v.cliente_id order by sum(valor)) as posicao,
	dense_rank() over(partition by v.cliente_id order by sum(valor)) as posicao,
	row_number() over (partition by v.cliente_id, v.data_venda order by v.data_venda),
	lag(valor) over (partition by v.cliente_id order by data_venda),
	lead(valor) over (partition by v.cliente_id order by data_venda)
from vendas v
group by v.id, v.cliente_id, v.valor
order by v.cliente_id

create materialized view vw_  as
select *
from -------

refresh 








create or replace function fn_cliente() 
return trigger
LANGUAGE 'plpgsql'
as $BODY$
 begin 
    insert into CLIENTES (cliente_id, nome, data_hr)
      values(new.cliente_id, new.nome, now())
return new
end;
$BODY$

create trigger trg_cliente 
before insert on CLIENTES
for each row
execute function fn_cliente();


create or replace view vw_total_vendas as 
        select cliente_id, sum(valor) as total
         from vendas 
         group by cliente_id



