// document.getElementById("buscar").addEventListener("click", async () => {
//     const nome = document.getElementById("user").value;
//     const resultado = document.getElementById("resultado");

// try {
//     resultado.textContent = "Carregando...";

//     const resposta = await fetch(`https://api.github.com/users/${nome}`);

//     if (!resposta.ok) {
//         resultado.textContent = "Usuário não encontrado!";
//         return;
//     }

//     const dados = await resposta.json();

//     resultado.innerHTML = `
//         <img src="${dados.avatar_url}" width="120">
//         <h2>${dados.name}</h2>
//         <p>Seguidores: ${dados.followers}</p>
//     `;
//     } catch {
//         resultado.textContent = "Erro ao conectar com a API.";
//     }
// });

const botao = document.getElementById("buscar");

// // usando then
// botao.addEventListener("click", (e) =>{
//     const usuario = document.getElementById("user").value.trim;
//     const resultado = document.getElementById("resultado");
//     e.preventDefault();

//     fetch(`https://api.github.com/users/${usuario}`)
//     .then(res => res.json())
//     .then((dados)=>{
//         if(!!dados?.mesage && dados?.status !== "200"){
//             resultado.textContent= dados?.message;
//         }else{
//             resultado.innerHTML = `
//             <img src="${dados.avatar_url}" width="120">
//             <h2>${dados.name}</h2>
//             <p>Seguidores: ${dados.followers}</p>`;
//         }
//     });
// });

botao.addEventListener("click", async (e) => {
    e.preventDefault();
    
    const usuario = document.getElementById("user");
    const resultado = document.getElementById("resultado");

    if(!usuario || usuario.length <=0 ){
        resultado.textContent = "Digite um nome de usuário";
        return;
    }   

    resultado.textContent = "Carregando...";

    try {
        const resposta = await fetch(`https://api.github.com/users/${usuario}`);
        if (!resposta.ok) {
            if (resposta.status === 404) {
                resultado.textContent = "Usuário não encontrado";
            } else if (resposta.status === 401) {
                resultado.textContent = "Acesso negado.";
            } else if (resposta.status === 403) {
                resultado.textContent = "Limite de requisições atingido";
            } else {
                const dados = await resposta.json();
                resultado.textContent = dados?.message || "Algo está errado";
            }
            return;
        }
        const dados = await resposta.json();

        resultado.innerHTML = `
            <img src="${dados.avatar_url}" width="120">
            <h2>${dados.name}</h2>
            <p>Seguidores: ${dados.followers}</p>
        `;
    } catch (error) {
        resultado.textContent ="erro de conexão com a API";
        console.error(error);
    }
});




document.getElementById("btn2").addEventListener("click", async () => {
    const pesquisa = document.getElementById("pesquisa").value.trim();
    const area = document.getElementById("resultado2");

    if(!pesquisa|| pesquisa.length <=0 ){
        area.textContent = "Digite algo";
        return;
    }

    
    area.textContent = "Carregando...";

    try {
    const resposta = await fetch(
        `https://api.adviceslip.com/advice/search/${pesquisa}`
    );
    const dados = await resposta.json();

    // Limpa antes de renderizar
    area.textContent = "";

    // Se a API não encontrar nada
    if (!dados.slips) {
        const p = document.createElement("p");
        p.textContent = "Nenhum conselho encontrado.";
        p.style.color = "red";
        area.appendChild(p);
        return;
    }

    // Título
    const titulo = document.createElement("h3");
    titulo.textContent = `Foram encontrados ${dados.slips.length} conselhos:`;
    area.appendChild(titulo);

    // Lista de conselhos
    const lista = document.createElement("ul");

    dados.slips.forEach((item) => {
        const li = document.createElement("li");
        li.textContent = item.advice;
        li.style.marginBottom = "6px";
        lista.appendChild(li);
    });

    area.appendChild(lista);
    } catch (erro) {
        area.textContent = "Erro ao buscar conselhos.";
    }
});



// EXECÍCIO CEP
const botaoCep =document.getElementById("buscarCep");
botaoCep.addEventListener("click", async () => {
    const cep = document.getElementById("cep").value.trim();
    const resultado = document.getElementById("resultadoCep");

    if (!cep || cep.length !== 8 || isNaN(cep)) {
        resultado.textContent = "Digite um CEP válido com 8 números.";
        return;
    }

    resultado.textContent = "Carregando...";

    try {
        const resposta = await fetch(`https://viacep.com.br/ws/${cep}/json/`);

        if (!resposta.ok) {
            resultado.textContent = "Erro ao buscar o CEP.";
            return;
        }

        const dados = await resposta.json();

        if (dados.erro) {
            resultado.textContent = "CEP não encontrado!";
            return;
        }

        resultado.innerHTML = `
            <p><strong>Logradouro:</strong> ${dados.logradouro}</p>
            <p><strong>Bairro:</strong> ${dados.bairro}</p>
            <p><strong>Cidade:</strong> ${dados.localidade}</p>
            <p><strong>UF:</strong> ${dados.uf}</p>
        `;
    } catch (erro) {
        // 5. Tratamento de erro
        resultado.textContent = "Erro ao conectar com a API.";
        console.error(erro);
    }
});
