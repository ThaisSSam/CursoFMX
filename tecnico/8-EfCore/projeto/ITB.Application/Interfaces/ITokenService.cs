using ITB.Domain.Entities;

namespace ITB.Application.Interfaces;

public interface ITokenService
{
    string GerarToken(Usuario usuario);
}
