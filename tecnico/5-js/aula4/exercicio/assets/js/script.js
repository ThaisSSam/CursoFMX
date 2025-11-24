let tarefaInput = document.getElementById('tarefa');

let btnAdicionar = document.getElementById('adicionar');

let btnLimpar = document.getElementById('limpar');

let lista = document.getElementById('lista')

let contador = document.getElementById('cont');
let cont = 0;

function atualizarContador() {
    cont = lista.children.length; 
    
    // Atualiza o valor na interface
    if (contador.tagName === 'INPUT') {
        contador.value = cont;
    } else {
        contador.textContent = cont;
    }
}

function removerTarefa(event) {
    const listItem = event.target.parentNode;
    lista.removeChild(listItem);
    atualizarContador(); 
    tarefaInput.focus();
}


document.addEventListener("DOMContentLoaded", function () {
    atualizarContador();
    
    btnAdicionar.addEventListener('click', (e)=>{
        e.preventDefault();
        const tarefaTexto = tarefaInput.value.trim();

        console.log(tarefaInput.value);
        if(tarefaTexto){
            let li = document.createElement("li");   
            li.textContent = tarefaTexto;
            li.classList.add(`tarefa-item`);

            const btnRemover = document.createElement("button");
            btnRemover.classList.add("btn-remover");
            
            btnRemover.addEventListener('click', removerTarefa);

            li.appendChild(btnRemover);
            lista.appendChild(li);

            tarefaInput.value = "";
            tarefaInput.focus();
            atualizarContador();
        }
    });

    btnLimpar.addEventListener('click', (e) =>{
        e.preventDefault();

        lista.innerHTML= "";

        atualizarContador();
        tarefaInput.focus();
    })

    document.querySelectorAll(".btn-remover").forEach(btn=>{
        btn.addEventListener("click", removerTarefa);
    });
    
});