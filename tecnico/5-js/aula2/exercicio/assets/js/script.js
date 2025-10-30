// const pessoas = [];
// let cont= 0;
// do{
//     nome = prompt("Qual seu nome?");
//     idade = Number(prompt("Qual sua idade?"));    
//     pessoas.push({
//         nome: nome,
//         idade: idade
//     });
//     cont = cont+1;
//     console.log(pessoas);  
// }while(cont<3);

//     const maiores = pessoas.filter(pessoa => pessoa.idade >= 18); 
//     console.table(maiores);   


const pessoas = []
for (let i = 0; i < 3; i++) {
    nome = prompt("Qual seu nome?");
    idade = Number(prompt("Qual sua idade?"));
    pessoas.push({
        nome: nome,
        idade:idade
    });
}
const maiores = pessoas.filter(pessoa => pessoa.idade >= 18); 
console.table(maiores);