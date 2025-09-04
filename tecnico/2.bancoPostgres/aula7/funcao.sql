-- FUNCTION: public.fnt_log_insercao_clientes()

-- DROP FUNCTION IF EXISTS public.fnt_log_insercao_clientes();

CREATE OR REPLACE FUNCTION public.fnt_log_insercao_clientes()
    RETURNS trigger
    LANGUAGE 'plpgsql'
    COST 100
    VOLATILE NOT LEAKPROOF
AS $BODY$
begin
	insert into log_clientes (cliente_id, nome)
		values(new.id, new.nome);

	-- sempre usar return new, se não a trigger dispara e não retorna nd
	return new;
end
$BODY$;

ALTER FUNCTION public.fnt_log_insercao_clientes()
    OWNER TO postgres;
