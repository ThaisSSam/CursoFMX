import {
  useEffect,
  type HTMLAttributes,
  type MouseEvent
} from "react";

//Props do Modal
type ModalProps = HTMLAttributes<HTMLDivElement> & {
  isOpen: boolean;
  title: string;
  children: React.ReactNode;
  onClose: () => void; //Função de callback quando o modal é fechado.
};

function Modal({ isOpen, title, children, onClose, ...divProps }: ModalProps) {

  //Gerenciando click no overlay do Modal
  const handleOverlayClick = (e: MouseEvent<any>) => {
    e.preventDefault();
    e.stopPropagation();
    if (e.target === e.currentTarget) onClose?.();
  };

  //Definimos evento na janela do navegador para que quando o usuário apertar "ESC" no teclado, o modal será fechado
  useEffect(() => {

    //Função que verifica qual tecla foi pressionada e se for ESC, fecha o modal
    const handleClick = (event: KeyboardEvent) => {
      if (event.key === "Escape") {
        onClose();
      }
    };

    window.addEventListener("keydown", handleClick); //Evento de tecla

    return () => {
      window.removeEventListener("keydown", handleClick); //É sempre importante remover os listeners
    };
  }, [onClose]);

  //Garante que o modal só será renderizado se estiver aberto
  if (!isOpen) {
    return null;
  }

  return (
    <div className="modal">
      <div className="modal-box" {...divProps}>
        <div className="modal-header">
          <h2>{title}</h2>
          <button className="modal-close-button" onClick={onClose}>
            X
          </button>
        </div>

        <div className="modal-content">{children}</div>
      </div>
      <div className="modal-overlay" onClick={handleOverlayClick}></div>
    </div>
  );
}

export default Modal;
