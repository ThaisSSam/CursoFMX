using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Treinamento.Domain.Commands;

public class JsonConversorStringOuInt : JsonConverter<int>
{
    public override int Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.Number)
        {
            return reader.GetInt32();
        }

        if (reader.TokenType == JsonTokenType.String)
        {
            string texto = reader.GetString()?.ToLower().Trim();

            return texto switch
            {
                // Prioridades
                "baixa" => 1,
                "media" => 2,
                "alta" => 3,
                "critica" => 3,

                // Situações
                "pendente" => 1,
                "afazer" => 1,
                "a fazer" => 1,
                "emandamento" => 2,
                "em andamento" => 2,
                "concluido" => 3,
                "concluida" => 3,
                "concluída" => 3,

                _ => int.TryParse(texto, out var numero) ? numero : 1
            };
        }

        return 1;
    }

    public override void Write(Utf8JsonWriter writer, int value, JsonSerializerOptions options)
    {
        writer.WriteNumberValue(value);
    }
}