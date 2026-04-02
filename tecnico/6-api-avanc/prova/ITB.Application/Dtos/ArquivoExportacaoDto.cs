using System;

namespace ITB.Application.Dtos;

public class ArquivoExportacaoDto 
{ 
    public byte[] Conteudo { get; set; } = Array.Empty<byte>(); 
    public string NomeArquivo { get; set; } = string.Empty; 
    public string ContentType { get; set; } = string.Empty; 
}