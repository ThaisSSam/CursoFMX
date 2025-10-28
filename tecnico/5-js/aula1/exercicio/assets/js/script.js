alert("Bem-vindo ao sistema!");
let usuario = prompt("Digite seu nome:");
let idade;
if(usuario){
    let idade = prompt("Digite sua idade:");
    console.log(`Usuário: ${usuario}, ${idade} anos.`);
    alert(`Olá ${usuario}, seja bem-vindo(a)!`);
}else{
    console.warn('Usuário não identificado');
}
console.log(idade);

// Desafio: Criar um prompt que só o usuário joão tem acesso caso contrário exiba um ERRO no console: Você não é o João