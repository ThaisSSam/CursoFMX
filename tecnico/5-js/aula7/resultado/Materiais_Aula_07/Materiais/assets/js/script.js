/* Definição de variáveis constantes */
const retornoFrase = document.getElementById("frase");
const retornoGithub = document.getElementById("github");
const retornoClima = document.getElementById("clima");
const retornoCotacao = document.getElementById("cotacao");

const GITHUB_USER = "ThaisSSam";
const WEATHER_API_KEY = "1fd31ebe8ff3c003f5e210f3c833d58a";


/* Definindo funções de chamada API */
async function getFrases(){
  return new Promise( async (resolve, reject) => {
    const response = await fetch(`https://api.adviceslip.com/advice`);
    if (!response.ok){
      reject("Falha ao carregar a frase do dia.");
      return;
    }

    const dados = await response.json();

    //{"slip": { "id": 157, "advice": "When something goes wrong in life, just shout \"plot twist!\" and carry on."}}
    return resolve(dados.slip.advice);
  })
}

async function getGithubUser(username){
  return new Promise( async (resolve, reject) => {
    try {
      const response = await fetch(`https://api.github.com/users/${username}`);
      
      if (!response.ok){
        throw new Error("Falha ao obter os dados do GitHub");
      }
      const dados = await response.json();
      resolve(dados);
    } catch (error) {
      const erro = {
        ok: false,
        message: "Falha ao obter os dados do usuário."
      };
      reject(erro);
    }
  })
}

async function getWeather(lat, lon){
  return new Promise( async (resolve, reject) => {
    try {
      const response = await fetch(`https://api.openweathermap.org/data/2.5/weather?lat=${lat}&lon=${lon}&units=metric&appid=${WEATHER_API_KEY}`);
      
      if (!response.ok){
        throw new Error("Falha ao obter os dados de clima.");
      }
      const dados = await response.json();
      resolve(dados);
    } catch (error) {
      const erro = {
        ok: false,
        message: error.message
      };
      reject(erro);
    }
  })
}

async function getCotacaoDolar(){
  return new Promise( async (resolve, reject) => {
    try {
      const response = await fetch(`https://economia.awesomeapi.com.br/json/last/USD-BRL`);
      
      if (response.status !== 200){
        throw new Error("Falha ao obter os dados de cotacao.");
      }
      const dados = await response.json();
      resolve(dados);
    } catch (error) {
      const erro = {
        ok: false,
        message: error.message
      };
      reject(erro);
    }
  })
}

function obterLocalizacao() {
  return new Promise( (resolve, reject) => {
    if (!navigator.geolocation) {
      reject(new Error("Geolocalização não suportada."));
      return;
    }

    navigator.geolocation.getCurrentPosition(resolve, reject);
  });
}

/* Funções de renderização */
function renderizaFrase(frase){
  retornoFrase.innerHTML = `
    <p>${frase}</p>
  `;
}

function renderizaGithub(github){
  
  if(!github?.ok && !!github?.message){
    retornoGithub.innerHTML = `
      <p>${github.message}</p>
    `;
    return;
  }

  retornoGithub.innerHTML = `
    <h2>${github.name || github.login}</h2>
    <img src='${github.avatar_url}' class='github-image' width='100px' height='100px' />
  `;
}

function renderizaClima(clima){

  if(!clima?.ok && !!clima?.message){
    retornoClima.innerHTML = `
      <p>${clima.message}</p>
    `;
    return;
  }

  retornoClima.innerHTML = `
    <h2>Clima</h2>
    <p>${clima.name}</p>
    <p>${clima.weather[0].main}</p>
    <p>${clima.main.temp} ºC</p>
  `;
}

function renderizaCotacao(cotacao){

  const dolar = cotacao.USDBRL;

  if(!cotacao?.ok && !!cotacao?.message){
    retornoCotacao.innerHTML = `
      <p>${cotacao.message}</p>
    `;
    return;
  }

  retornoCotacao.innerHTML = `
    <h2>Cotacao Dolar</h2>
    <p>$ ${dolar.bid}</p>
  `;
}

document.addEventListener("DOMContentLoaded", async () =>{

  let lat;
  let lon;

  try {
    const {coords} = await obterLocalizacao();
    lat = coords.latitude;
    lon = coords.longitude;
  } catch (error) {
    console.warn('Falha ao obter a localizacao');
  }

  const [frase, github, weather, cotacao] = await Promise.all(
    [
      getFrases(),
      getGithubUser(GITHUB_USER),
      getWeather(lat, lon),
      getCotacaoDolar()
    ]
  );

  renderizaFrase(frase);
  renderizaGithub(github);
  renderizaClima(weather);
  renderizaCotacao(cotacao);

});