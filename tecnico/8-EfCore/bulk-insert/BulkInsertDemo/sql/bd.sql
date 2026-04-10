BEGIN;
CREATE TABLE IF NOT EXISTS veiculo (
    id uuid PRIMARY KEY,
    placa varchar(50) NOT NULL,
    modelo varchar(200) NOT NULL,
    preco numeric(18,2) NOT NULL
);

CREATE UNIQUE INDEX IF NOT EXISTS ux_veiculo_placa ON veiculo (placa);


INSERT INTO veiculo (id, placa, modelo, preco) VALUES
('00000000-0000-0000-0000-000000000001', 'ABC-1234', 'Gol G6', 25000.00),
('00000000-0000-0000-0000-000000000002', 'XYZ-9876', 'Civic 2015', 45000.00)
ON CONFLICT (placa) DO NOTHING;


INSERT INTO veiculo (id, placa, modelo, preco) VALUES
(gen_random_uuid(), 'ABC-1234', 'Gol G7', 27000.00),
(gen_random_uuid(), 'NEW-2026', 'Fiat Pulse', 82000.00)
ON CONFLICT (placa) DO UPDATE SET
    modelo = EXCLUDED.modelo,
    preco = EXCLUDED.preco;

-- Table: public.operacao_financeira

-- DROP TABLE IF EXISTS public.operacao_financeira;

CREATE TABLE IF NOT EXISTS public.operacao_financeira
(
    id uuid NOT NULL DEFAULT gen_random_uuid(),
    numero_contrato character varying(100) COLLATE pg_catalog."default" NOT NULL,
    valor_total numeric(18,2) NOT NULL,
    data_processamento timestamp with time zone NOT NULL,
    CONSTRAINT operacao_financeira_pkey PRIMARY KEY (id)
);

COMMIT;
