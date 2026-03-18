import { Curso } from "./curso";
export interface Aluno{
    nome: string;
    idade: number;
    curso: Curso;
    matriculado: boolean;
}

