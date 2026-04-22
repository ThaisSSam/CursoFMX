import { useState } from "react";
import { useNavigate } from "react-router-dom";

export const Navegacao= () =>{
  const navigate = useNavigate();
  const [check, setCheck] = useState(false);
  const ChangeCheckbox = () =>{
    if(check ==false){
      setCheck(true);
    } else{
      setCheck(false);
    }
    
  }

  const NavegacaoParaHome =() =>{  
    if(check==true){
      navigate("/usuarios-fetch");  
    } else{
      alert("Usuário não permitido");
    }
  }

  return(
    <div>
      <h1>Navegação</h1>
      <div>
        <div style={{marginBottom:"10px"}}>
          <label>Check se o usuário é permitido</label>
          <input type="checkbox" name="check" onChange={ChangeCheckbox} />
        </div>
      <button onClick={NavegacaoParaHome}>Para usuário fetch</button>
      </div>
    </div>
  )
}