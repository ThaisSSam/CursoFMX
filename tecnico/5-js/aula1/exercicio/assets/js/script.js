// alert("Bem-vindo ao sistema!");
// let usuario = prompt("Digite seu nome:");
// let idade;
// if(usuario){
//     let idade = prompt("Digite sua idade:");
//     console.log(`Usuário: ${usuario}, ${idade} anos.`);
//     alert(`Olá ${usuario}, seja bem-vindo(a)!`);
// }else{
//     console.warn('Usuário não identificado');
// }
// console.log(idade);

// Desafio: Criar um prompt que só o usuário joão tem acesso caso contrário exiba um ERRO no console: Você não é o João

const perguntas = [
    {
        title: "Qual o seu nome?",
        default: ""
    },
    {
        title: "Qual a sua Idade?",
        default: ""
    },
    {
        title: "Onde você mora?",
        default: ""
    }
]
const respostas = [];
for (let i = 0; i < perguntas.length; i++) {
    const resposta = prompt(perguntas[i].title, perguntas[i].default);
    respostas.push({
        title: perguntas[i].title,
        data: resposta
    });
}
console.table(respostas);