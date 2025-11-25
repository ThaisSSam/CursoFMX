async function carregaGithub(username){
  const response = await fetch(`https://api.github.com/users/${username}`);
  return await response.json();
}

async function carregaFrase(){
  try {
    const response = await fetch(`https://api.adviceslip.com/advice`);
    const frases = await response.json();
    return frases.slip.advice;
  } catch (error) {
    return "Não foi possível carregar os dados";
  }
}

async function carregarDados() {
  
  try {
    const [github, frases] = await Promise.all(
      [
        carregaGithub("pedro-amadio"),
        carregaFrase("live")
      ]
    );
    console.log('github',github);
    console.log('frases',frases);
  } catch (error) {
    console.error("Error ao chamar as APIs", error);
  }

}


document.addEventListener("DOMContentLoaded", async ()=>{
  await carregarDados();

  navigator.geolocation.getCurrentPosition(
    (pos) => {
      const { latitude, longitude } = pos.coords;
      console.log(latitude, longitude);
    },
    (erro) => {
      console.log("Não foi possível obter a localização.");
    }
  );


});