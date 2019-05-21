using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using ThinkerThings.Servicos.Usuarios.Conta.Domain.AggregateModel.SenhaModel;
using ThinkerThings.Servicos.Usuarios.Conta.Domain.AggregateModel.UsuarioModel;
using ThinkerThings.Servicos.Usuarios.Conta.Infra.Options;

namespace ThinkerThings.Servicos.Usuarios.Conta.Infra.DataContext
{
    public class ServicosUsuariosContaDataContext : DbContext
    {
        private readonly IOptions<ConnectionStringOptions> _connectionStringOptions;

        public ServicosUsuariosContaDataContext(DbContextOptions options, IOptions<ConnectionStringOptions> connectionStringOptions)
            : base(options)
        {
            _connectionStringOptions = connectionStringOptions;
        }

        public DbSet<Usuario> Usuario { get; set; }
        public DbSet<RequisicaoSenhaUsuario> RequisicoesSenhaUsuarios { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseSqlServer(_connectionStringOptions.Value.ConnectionString);
    }
}