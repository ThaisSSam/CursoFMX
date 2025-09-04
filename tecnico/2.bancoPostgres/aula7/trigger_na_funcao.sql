-- Trigger: trg_log_insercao_clientes

-- DROP TRIGGER IF EXISTS trg_log_insercao_clientes ON public.clientes;

CREATE OR REPLACE TRIGGER trg_log_insercao_clientes
    AFTER INSERT
    ON public.clientes
    FOR EACH ROW --para cada linha
    -- FOR EACH STATEMENT --para cada bloco
    EXECUTE FUNCTION public.fnt_log_insercao_clientes();--tem que ser criado antes