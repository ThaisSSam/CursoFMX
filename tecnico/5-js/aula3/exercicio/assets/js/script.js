

function calcularIMC(peso, altura){
    // peso = Number(prompt("Qual seu peso?"));
    // altura = Number(prompt("Qual sua altura?"));
    // dimensoes.push({
    //     peso: peso,
    //     altura: altura
    // });
    // let peso = peso.toFixed(2);
    // let altura = altura.toFixed(2);
    let calculo;
    return calculo = (peso/ (altura*altura)).toFixed(2);
}

function classificarIMC(imc){
    let txt; 
    console.log(imc);
    if(imc<18.5){
        txt="Abaixo do peso"
    }else{
        if(imc<24.9){
            txt="Peso normal"
        }else{
            if(imc<29.9){
                txt="Sobrepeso"
            }else{
                txt="Obesidade"
            }
        }
    }
    console.log(imc);
    return txt;
}


const peso = 70; 
const altura = 1.75; 
const imc = calcularIMC(peso, altura); 
const classificacao = classificarIMC(imc);
console.log(classificarIMC(imc)); 
console.log(`IMC: ${imc} - Classificação: ${classificacao}`); 