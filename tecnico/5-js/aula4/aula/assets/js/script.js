const input = document.getElementById("item");
const botao = document.getElementById("adicionar");
const lista = document.getElementById("lista");

botao.addEventListener("click",(e) =>{
    const produto = input.value.trim();

    if(produto === ""){
        alert("Digite um nome de um produto");
        input.focus();
        return;
    }

    const botaoRemover = document.createElement("button");
    botaoRemover.style = "margin-left: 10px;";
    botaoRemover.textContent = "Remover";
    botaoRemover.addEventListener("click", (e) => {
        e.target.parentElement.remove();
    });

    const li = document.createElement("li");
    li.textContent = produto;
    lista.appendChild(li);

    input.value= "";
    input.focus();
});