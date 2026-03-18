import {Aluno} from "./types/aluno";
import {Curso} from "./types/curso";

const curso: Array<Curso> =[ 
    {
        nome: 'inglês',
        duracao: 4,
    },
    {
        nome: 'matemática',
        duracao: 40,
    }
]
const alunos: Array<Aluno> = [
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
alunos.forEach((aluno:Aluno) =>{
    console.log("Nome: ", aluno.nome);
    console.log("Idade: ", aluno.idade);
    console.log("Curso: ", aluno.curso.nome);
    console.log("Matriculado: ", aluno.matriculado);
    console.log('=============================');
})

alunos.filter((item) => item.curso.nome.toLocaleLowerCase()==='inglês').forEach((aluno: Aluno)=>{    
    console.log("*-Apenas inglês-*");
    console.log("Nome: ", aluno.nome);
    console.log("Idade: ", aluno.idade);
    console.log("Curso: ", aluno.curso.nome);
    console.log("Matriculado: ", aluno.matriculado);
})

// Para compilar
// npm i
// npx tsc
// node ./dist/index.js