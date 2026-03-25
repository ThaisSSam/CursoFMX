import { useState, type ChangeEvent } from "react";
import "./App.css";
import {type Tarefa} from "./types/tarefa";

function App() {
  const [novaTarefa, setNovaTarefa] = useState<string>("");
  const [tarefas, setTarefas] = useState<Array<string>>([]);

  const handleChange = (event: ChangeEvent<HTMLInputElement>) => {
    setNovaTarefa(event.target.value);
  }

  const adicionarItem = (): void=> {
    if(novaTarefa.trim() === "") {
      return
    }
    setTarefas([...tarefas, novaTarefa]);
    setNovaTarefa("");
  };

  return (
    <>
      <div>
        <h1>Lista de Tarefas</h1>
        <p>Minha primeira aplicação com React + TypeScript</p>
      </div>
      <div>
        <p>{novaTarefa}</p>
        <input type="text" onChange={handleChange} value={novaTarefa}/>
        <button>Adicionar</button>
      </div>
      <div>
        <h2>Lista</h2>
        <div>
          {tarefas.map((item) => (
            <p>{item.title}</p>
          ))}
        </div>
      </div>
    </>
  );
}

export default App;

// rodar 
// npm install
// npm run dev