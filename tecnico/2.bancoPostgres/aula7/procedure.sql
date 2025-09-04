-- PROCEDURE: public.pr_inserir_cliente(character varying, integer)

-- DROP PROCEDURE IF EXISTS public.pr_inserir_cliente(character varying, integer);

CREATE OR REPLACE PROCEDURE public.pr_inserir_cliente(
	IN p_nome character varying,
	IN p_idade integer)
LANGUAGE 'plpgsql'
AS $BODY$
declare
	r_funcao int;
begin
	if p_idade <10 then
		insert into clientes (nome, idade)
		values (p_nome, p_idade);
	end if;
	
	-- implicitamente commit
exception 	
	when division_by_zero then
		raise notice 'divisão por zero';
		
	when unique_violation then
		raise notice 'chave unica violada';
	
	when others then
		raise notice 'erro generico';

	-- implicitamente rollback
end;
$BODY$;
ALTER PROCEDURE public.pr_inserir_cliente(character varying, integer)
    OWNER TO postgres;
