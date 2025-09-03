select *
	from clientes
	where nome = 'Fernanda Gomes' 
	
-- nome único para o banco inteiro
create index idx_vendas_cliente on clientes(nome);

-- null atrapalha o índice
create unique index idx_vendas_cliente1 on clientes (email);

create index idx_venda_nome_cidade on clientes(nome,cidade)

select*
dormf