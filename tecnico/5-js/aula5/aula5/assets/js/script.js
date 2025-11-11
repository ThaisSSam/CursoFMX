const form = document.getElementById("cadastro");
// const msg =document.getElementById("mensagem");

function exibirMensagemGeral(texto, cor="red"){
    const msg = document.getElementById("mensagem");
    msg.textContent =texto;
    msg.style.color=cor;
}

function exibirMensagem(id, texto, tipo="error"){
    const input = document.getElementById(id);
    const msg = document.querySelector(`[data-error-for="${id}"]`);

    // reset visual
    if(tipo === "reset"){
        msg.textContent = "";
        input.style.borderColor = "";
        return;
    }

    msg.textContent = texto.trim();
    msg.style.fontSize = "0.9rem";
    msg.style.marginTop = "2px";

    if(tipo === "error"){
        msg.style.color= "light-red";
        input.style.borderColor= "red";
    }else if(tipo === "success"){
        msg.style.color = "green";
        input.style.borderColor = "green";
    }
}

// function mostrarMensagem(texto, cor = "red"){
//     msg.textContent = texto;
//     msg.style.color = cor;
// }


form.addEventListener("submit", (e) =>{

    e.preventDefault();

    let isOk= true;

    exibirMensagemGeral("", "black");
//     e.preventDefault();
//     // console.log("Formulário enviado")


//     const nome =document.getElementById("nome").value.trim();
//     const idade =document.getElementById("idade").value.trim();
//     const email =document.getElementById("email").value.trim();
//     const senha =document.getElementById("senha").value.trim();
//     const repSenha =document.getElementById("rep-senha").value.trim();

    const formData = new FormData(e.target);

    const nome =formData.get("nome");
    const idade =formData.get("idade");
    const email =formData.get("email");
    const senha =formData.get("senha");
    const repSenha =formData.get("rep-senha");
    
    if(nome.length < 3){
        exibirMensagem("nome","Nome deve ser maior que 3 caracteres");
        isOk = false;
    } else if(nome.length > 20){
        exibirMensagem("nome","Nome deve ser menor que 20 caracteres");   
        isOk = false;     
    }else{
        exibirMensagem("nome","", "reset");
    }

    const regexEmail = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    if(!regexEmail.test(email)){
        mostrarMensagem("email","E-mail inválido");
        isOk = false;
    }else{
        mostrarMensagem("email", "E-mail válido", "success");
    }

    if(isNaN(idade) || idade < 18){
        exibirMensagem("idade","Idade deve ser maior de 18");
        isOk = false;
    } else{
        exibirMensagem("idade", "", "reset");
    }

    const regexSenha= /^(?=.*[0-9]).{6,}$/;
    if(!regexSenha.test(senha)){
        exibirMensagem("senha","A senha deve conter pelo menos 1 número e 6 caracteres");      
        isOk = false;  
    }

    if(repSenha == !senha){
        exibirMensagem("repSenha","A senhas tem que ser iguais");
        isOk=false;
    }else{
        exibirMensagem("repSenha", "", "reset");
    }

    if(isOk){
        exibirMensagemGeral("sucesso", "green");
        const resultado = {
            nome,
            email,
            idade,
            senha,
            repSenha
        };
        console.table(resultado);
    }

//     // console.log(formData);
    
//     // console.log(formData.get("nome"));
//     // formData.append("CustomInput","teste");
//     // console.log(formData.forEach(item => {
//     //     console.log(item);
//     // }));

//     // Nome maior que 4 e menor que 20 caracteres
//     if(nome.length < 3){
//         mostrarMensagem("Nome deve ser maior que 3 caracteres", "red");
//         return;
//     } else if(nome.length > 20){
//         mostrarMensagem("Nome deve ser menor que 20 caracteres", "red" );
//         return;
//     }

//     const regexEmail = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
//     if(!regexEmail.test(email)){
//         mostrarMensagem("E-mail inválido", "red");
//         return;
//     }

//     // isNaN = is not a number
//     // Deve ser maior de 18
//     if(isNaN(idade) || idade < 18){
//         mostrarMensagem("Idade deve ser maior de 18", "red");
//         return;
//     }

//     // A senha deve conter pelo menos um número e 6 caracteres
//     const regexSenha= /^(?=.*[0-9]).{6,}$/;
//     if(!regexSenha.test(senha)){
//         mostrarMensagem("A senha deve conter pelo menos um número e 6 caracteres", "red");
//         return;
//     }

//     // senha repetida == senha original
//     if(repSenha == !senha){
//         mostrarMensagem("A senhas tem que ser iguais", "red");
//         return;
//     }

//     mostrarMensagem("Cadastrado", "green");
});

