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

create or replace function fn_somar_numeros (p_valor1 int, p_valor2 bigint, p_valor3 int)
returns int
language plpgsql
as $$ 
declare r_valor int;
sum_vendas numeric;
begin
	-- return(p_valor1 + p_valor2) * p_valor3;
	-- r_valor := (p_valor1 + p_valor2) * p_valor3;
	r_valor := (p_valor1+ p_valor2);
	r_valor := r_valor * p_valor3;

	select sum (valor)
	into sum_vendas
	from vendas;

	r_valor:= r_valor * sum_vendas;
	return r_valor;
end;
$$


select fn_somar_numeros(1,10,2)

select fn_somar_numeros( 1, (select count(1) from vendas), 2)


create or replace procedure pr_inserir_cliente(p_nome varchar, p_idade int)
language plpgsql
as $$
begin
	if p_idade <1 then
		insert into clientes (nome, idade)
		values (p_nome, p_idade);
	end if;
end;
$$

create or replace procedure pr_inserir_cliente(p_nome varchar, p_idade int)
language plpgsql
as $$
begin
	if p_idade <10 then
		insert into clientes (nome, idade)
		values (p_nome, (select fn_somar_numeros(1,1,2)));
	end if;

exception 	
	when unique_violation then
		raise notice 'chave unica violada';
	
	when others then
		raise notice 'erro generico';
end;
$$


call pr_inserir_cliente('teste', 1);


select *
from clientes

create or replace function fnt_log_insercao_clientes()
returns trigger
language plpgsql
as $$
begin
	insert into log_clientes (cliente_id, nome)
		values(new.id, new.nome);
		
	return new;
end
$$

create trigger trg_log_insercao_clientes
after insert on clientes
for each statement
execute function fnt_log_insercao_clientes()

insert into clientes (nome, idade) values ('Anakin', 36)

select *
from clientes

select *
from log_clientes
