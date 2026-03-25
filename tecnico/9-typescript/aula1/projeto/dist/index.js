"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
const curso = [
    {
        nome: 'inglês',
        duracao: 4,
    },
    {
        nome: 'matemática',
        duracao: 40,
    }
];
const alunos = [
    {
        nome: "aluno1",
        idade: 20,
        matriculado: true,
        curso: curso[0],
    },
    {
        nome: "aluno2",
        idade: 22,
        matriculado: false,
        curso: curso[1],
    }
];
console.table(alunos);
alunos.forEach((aluno) => {
    console.log("Nome: ", aluno.nome);
    console.log("Idade: ", aluno.idade);
    console.log("Curso: ", aluno.curso.nome);
    console.log("Matriculado: ", aluno.matriculado);
    console.log('=============================');
});
alunos.filter((item) => item.curso.nome.toLocaleLowerCase() === 'inglês').forEach((aluno) => {
    console.log("*-Apenas inglês-*");
    console.log("Nome: ", aluno.nome);
    console.log("Idade: ", aluno.idade);
    console.log("Curso: ", aluno.curso.nome);
    console.log("Matriculado: ", aluno.matriculado);
});
// Para compilar
// npm i
// npx tsc
// node ./dist/index.js
