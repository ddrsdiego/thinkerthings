namespace ThinkerThings.GerenciamentoProtocolo.Infra.Statements
{
    internal static class ProtocoloRepositorioStatemets
    {
        public const string Registrar = "";
        public const string ObterProximoNumeroProtocolo = @"SELECT NEXT VALUE FOR SEQ_PROTOCOLO";
    }
}