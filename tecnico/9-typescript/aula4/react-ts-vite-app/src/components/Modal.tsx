import { useEffect, type HTMLAttributes } from "react"

type ModalProps = HTMLAttributes<HTMLDivElement>&{
  isOpen: boolean;
  title: string;
  children: React.ReactNode;
  onClose: () => void;
}

function Modal({isOpen, title, children, onClose, ...divProps}: ModalProps){
  useEffect(() =>{
    document.addEventListener("keydown", (event)=>{
      if(event.key === "Escape"){
        onClose();
      }
    });
  },[onClose]);
  
  if(!isOpen){
    return null;
  }
  
  return (
    <div className="modal-overlay">
      <div className="modal-box" {...divProps}>
        <div className="modal-header">
          <h2>{title}</h2>
          <button className="modal-close-button" onClick={onClose}>
            X
          </button>
        </div>

        <div className="modal-content">
          {children}
        </div>
      </div>
    </div>
  );
}

export default Modal;