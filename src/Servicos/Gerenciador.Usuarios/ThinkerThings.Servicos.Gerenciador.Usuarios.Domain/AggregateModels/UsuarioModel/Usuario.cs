using System;

namespace ThinkerThings.Servicos.Gerenciador.Usuarios.Domain.AggregateModels.UsuarioModel
{
    public class Usuario
    {
        protected Usuario()
        {
        }

        public int UsuarioId { get; set; }
        public string Nome { get; set; }
        public string CPF { get; set; }
        public string Email { get; set; }
        public string Telefone { get; set; }
        public DateTimeOffset DataCadastro { get; } = DateTimeOffset.Now;
        public DateTimeOffset DataUltimaAlteracao { get; }
    }
}