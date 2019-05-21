using System;

namespace ThinkerThings.Servicos.Usuarios.Conta.Domain.AggregateModel.UsuarioModel
{
    public class Usuario
    {
        public int UsuarioId { get; set; }
        public string Nome { get; set; }
        public string CPF { get; set; }
        public string Email { get; set; }
        public string Telefone { get; set; }
        public DateTimeOffset DataCadastro { get; } = DateTimeOffset.Now;
        public DateTimeOffset DataUltimaAlteracao { get; }
    }
}