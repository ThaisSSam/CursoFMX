import { useEffect, useState } from "react";
import Button from "./components/Button";
import Modal from "./components/Modal";
import Card from "./components/Card";
import { create } from "zustand";

function App() {
  const [isModalOpen, setIsModalOpen] = useState<boolean>(false);
  const [isLoadingModal, setIsloadingModal] = useState<boolean>(false);
  const [isLoading, setIsloading] = useState<boolean>(false);
  const [count, setIsCount] = useState<number>(0);  
  const [valorAnterior, setValorAnterior] = useState<number>(0);  
  const [cards, setCards] = useState<Array<{ title: string; content: string }>>(
    [
      {
        title: "Teste MockUp 1",
        content:
          "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.",
      },
      {
        title: "Teste MockUp 2",
        content:
          "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.",
      },
      {
        title: "Teste MockUp 3",
        content:
          "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.",
      },
    ],
  );
  useEffect(() => {
    if(valorAnterior>count){
      var calculo = "Decrementou";
    }else{
      var calculo = "Incrementou";
    }
    console.log(`${calculo} - Count: ${count}`);
  },[count, setIsCount]);

  const abrirModal = () => {
    setIsModalOpen(true);
  };

  const fecharModal = () => {
    setIsModalOpen(false);
  };

  const abrirModalComLoad = () => {
    setIsloadingModal(true);
    setTimeout(() => {
      setIsModalOpen(true);
      setIsloadingModal(false);
    }, 2000);
  };

  const loadingButton = () => {
    setIsloading(true);
    setTimeout(() => {
      setIsloading(false);
    }, 2000);
  };

  const incrementaContador = () =>{
    setIsCount (valorAnterior => valorAnterior + 1);
    setValorAnterior(count);
  }
  const decrementaContador = () =>{
    setIsCount (valorAnterior => valorAnterior - 1);
    setValorAnterior(count);
  }

  

  return (
    <>
      <section>
        <h1>Componentes com React + TypeScript</h1>

        <div
          style={{
            display: "flex",
            gap: "8px",
            flexWrap: "nowrap",
            alignContent: "flex-start",
            alignItems: "flex-start",
          }}
        >
          <Button onClick={abrirModal}>Abrir modal</Button>
          <Button
            isLoading={isLoadingModal}
            isLoadingText="Aguarde..."
            onClick={abrirModalComLoad}
          >
            Abrir modal (Carregamento)
          </Button>
          <Button
            isLoading={isLoading}
            isLoadingText="Aguarde..."
            onClick={loadingButton}
          >
            Carregamento simples
          </Button>
          <Button isLoading={true} isLoadingText="Aguarde...">
            Carregado!
          </Button>
          <Button disabled>Desabilitado</Button>
          <Button onClick={incrementaContador}>Incrementar</Button> 
          <Button onClick={decrementaContador}>Decrementar</Button> 
          <p>Contador: {count}</p>
        </div>
      </section>
      <br />
      <section>
        <h1>Bônus (Card)</h1>
        <div
          style={{
            display: "flex",
            gap: "8px",
            flexWrap: "nowrap",
            alignContent: "flex-start",
            alignItems: "flex-start",
          }}
        >
          {cards.map((item) => {
            return (
              <Card title={item.title} rounded="sm">
                {item.content}
              </Card>
            );
          })}
        </div>
      </section>
      <Modal
        isOpen={isModalOpen}
        title="Meu primeiro modal"
        onClose={fecharModal}
      >
        <p>Este é um modal criado com React, props, useState e CSS puro.</p>
      </Modal>
    </>
  );
}

export default App;
