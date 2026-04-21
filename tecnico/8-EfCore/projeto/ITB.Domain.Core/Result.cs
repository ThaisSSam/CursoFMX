using System;

namespace ITB.Domain.Core;

// 1. O RESULTADO BASE (Usado para ações que não devolvem objetos, como AlterarSenha) 
public class Result
{
  public List<string> Errors { get; } = [];
  public bool IsSuccess => !Errors.Any();
  protected Result(List<string> errors)
  {
    Errors = errors ?? [];
  }

  public static Result Success() => new Result([]);
  public static Result Failure(string error) => new Result([error]);
  public static Result Failure(List<string> errors) => new Result(errors);
}

// 2. O RESULTADO COM VALOR (Usado para a Fábrica que precisa devolver o Usuário criado) 
public class Result<T> : Result
{
  public T? Value { get; }

  private Result(T? value, List<string> errors) : base(errors)
  {
    Value = value;
  }

  public static Result<T> Success(T value) => new Result<T>(value, []);
  public new static Result<T> Failure(string error) => new Result<T>(default,
[error]);
  public new static Result<T> Failure(List<string> errors) => new Result<T>(default,
errors);
}