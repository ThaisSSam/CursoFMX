import { useState } from 'react';
import Button from './components/button';
import Modal from './components/Modal';

function App() {
  const [isModalOpen, setIsModalOpen] = useState<boolean>(false);

  const abrirModal = () =>{
    setIsModalOpen(true);
  };

  const fecharModal = () =>{
    setIsModalOpen(false);
  }

  return (
    <div>
      <h1>Componentes com React + TypeScript</h1>

      <Button onClick={abrirModal} >Abrir modal</Button>

      <Modal
        isOpen={isModalOpen}
        title="Meu primeiro modal"
        onClose={fecharModal}
      >
        <p>Este é um modal criado com React, props, useState e CSS puro.</p>
      </Modal>
    </div>
  );
}

export default App;