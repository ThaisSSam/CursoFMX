select *
	from clientes
	where nome = 'Fernanda Gomes' 
	
-- nome único para o banco inteiro
create index idx_vendas_cliente on clientes(nome);

-- null atrapalha o índice
create unique index idx_vendas_cliente1 on clientes (email);

create index idx_venda_nome_cidade on clientes(nome,cidade)

select *
from clientes
where nome = '' and
cidade = ''


create index nome 
on vendas(valor) 
where valor >1000

select* from vendas
order by valor

-- usar apenas o select no que tem o índice ou o que não tem, não usar o meio a meio
-- ex: não usar valor between 500 and 9999, já que no índice está valor>1000
select * 
from vendas
where valor between 1000 and 9999
order by valor

-- o indice assim não funciona sozinho
select * 
from clientes
where nome = lower(nome)

-- precisa fzr isso(índice por expressão)
create index idx_cliente_nome
on clientes(lower(nome))

-- consultar quantos indice tem dentro das tabelas, indexes ou:
select *
	from pg_indexes
	where schemaname = 'public'

-- descobrir se ta usando o índice
explain 
select *
from clientes
where id = 1;

-- executa para saber os detalhes
explain analyze
select *
from clientes
where id = 1;


select * from PG_STATISTIC

-- atualiza a estatistica, fzr com cada indice.
analyse vendas

-- atualiza e remove tuplas mortas, trunca todas as informações das tabelas 
vacuum analyse vendas


