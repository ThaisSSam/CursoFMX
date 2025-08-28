select *
from public.vw_somas_acumuladas a
where a.cliente_id =1

create materialized view if not exists public.mv_total_vendas_cliente
tablespace pg_default
as 
select cliente_id, sum(valor) as total
from vendas
group by cliente_id
with data;

alter table if exists public.mv_total_vendas_cliente
	owner to postgres;

select * from public.mv_total_vendas_cliente

refresh materialized view mv_total_vendas_cliente


-- Função
-- Para ver a função clica com o botão direito, script e CREATE script

create or replace function fn_somar_numeros (p_valor1 int, p_valor2 int, p_valor3 int)
	r







