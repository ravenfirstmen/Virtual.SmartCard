using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Text;

namespace Virtual.SmartCard
{
    public static class SmartCardErrors
    {
        public static bool InSystemFacility(Int32 errorCode)
        {
            return ((errorCode >> 16) & 0x07FF) == 0x00;
        }

        public static bool InSmartCardFacility(Int32 errorCode)
        {
            return (((errorCode >> 16) & 0x07FF) & 0x10) == 0x10;
        }

        public static Severity GetSeverityLevelFrom(Int32 errorCode)
        {
            switch (errorCode >> 30)
            {
                case 0x00:
                    return Severity.None;
                case 0x01:
                    return Severity.Informal;
                case 0x02:
                    return Severity.Warning;
                default: // 0x03
                    return Severity.Error;
            }
        }

        public static string GetSeverityMessageFrom(Int32 errorCode)
        {
            switch (errorCode >> 30)
            {
                case 0x00:
                    return "Nenhuma";
                case 0x01:
                    return "Informal";
                case 0x02:
                    return "Aviso";
                default: // 0x03
                    return "Erro";
            }
        }


        public static string GetMessageFrom(Int32 errorCode)
        {
            var errorCodeMessage = new StringBuilder();

            // severidade
            errorCodeMessage.AppendFormat("Severidade: {0}{1}", GetSeverityMessageFrom(errorCode), Environment.NewLine);

            if (ErrorMessages.ContainsKey((uint) errorCode))
            {
                errorCodeMessage.Append(ErrorMessages[(uint) errorCode]);
            }
            else
            {
                int lastW32Error = Marshal.GetLastWin32Error();
                errorCodeMessage.Append(lastW32Error != 0
                                            ? new Win32Exception(Marshal.GetLastWin32Error()).Message
                                            : "Erro desconhecido. Verifique se está a usar correctamente a libraria!");
            }

            return errorCodeMessage.ToString();
        }

        public enum Severity
        {
            None = 0x00,
            Warning = 0x02,
            Informal = 0x01,
            Error = 0x03
        }

        public static Codes GetCodeFromErrorCode(Int32 errorCode)
        {
            return ErrorMessages.ContainsKey((uint)errorCode) ? (Codes)errorCode : Codes.UnknownErrorCode;
        }

        public enum Codes : uint
        {
            NoError = 0x00,

            // Facility
            InternalError = 0x80100001,
            WaitedToLong = 0x80100007,
            CommunicationError = 0x80100013,
            UnknownError = 0x80100014,

            // Errors
            Canceled = 0x80100002,
            InvalidHandle = 0x80100003,
            InvalidParameter = 0x80100004,
            InvalidTarget = 0x80100005,
            NoMemory = 0x80100006,
            InsuficientBuffer = 0x80100008,
            UnknownReader = 0x80100009,
            TimeOut = 0x8010000A,
            SharingViolation = 0x8010000B,
            NoSmartCard = 0x8010000C,
            UnknownCard = 0x8010000D,
            CantDispose = 0x8010000E,
            ProtocolMismatch = 0x8010000F,
            NotReady = 0x80100010,
            InvalidValue = 0x80100011,
            SystemCanceled = 0x80100012,
            InvalidATR = 0x80100015,
            NotInTransaction = 0x80100016,
            ReaderUnavailable = 0x80100017,
            Shutdown = 0x80100018,
            PCIToSmall = 0x80100019,
            ReaderUnsupported = 0x8010001A,
            DuplicateReader = 0x8010001B,
            CardUnsupported = 0x8010001C,
            NoService = 0x8010001D,
            ServiceStopped = 0x8010001E,
            UnexpectedCardError = 0x8010001F,
            ICCInstalation = 0x80100020,
            ICCCreateOrder = 0x80100021,
            UnsupportedFeature = 0x80100022,
            DirectoryNotFound = 0x80100023,
            FileNotFound = 0x80100024,
            NoDirectory = 0x80100025,
            NoFile = 0x80100026,
            AccessDenied = 0x80100027,
            NotEnoughMemoryInCard = 0x80100028,
            BadSeek = 0x80100029,
            InvalidPIN = 0x8010002A,
            UnknownErrorCode = 0x8010002B,
            NoSuchCertificate = 0x8010002C,
            CertificateUnavailable = 0x8010002D,
            CannotFindCardReader = 0x8010002E,
            CommunicationDataLost = 0x8010002F,
            NoKeyContainer = 0x80100030,
            ServerToBusy = 0x80100031,
            PINCacheExpired = 0x80100032,
            NoPINCache = 0x80100033,
            ReadOnlyCard = 0x80100034,
            UnsupportedCard = 0x80100065,
            UnresponsiveCard = 0x80100066,
            UnPoweredCard = 0x80100067,
            ResetCard = 0x80100068,
            RemovedCard = 0x80100069,
            SecurityViolation = 0x8010006A,
            WrongPIN = 0x8010006B,
            PINBlocked = 0x8010006C,
            EOF = 0x8010006D,
            CanceledByUser = 0x8010006E,
            NotAuthenticatedCard = 0x8010006F,
            CacheItemNotFound = 0x80100070,
            CacheItemStale = 0x80100071,
            CacheItemToBig = 0x80100072
        }

        private static readonly IDictionary<uint, string> ErrorMessages = new Dictionary<uint, string>()
            {
                {(uint) Codes.NoError, "Sem nenhum erro associado. Tudo foi executado conforme o esperado."},
                {(uint) Codes.InternalError, "Um erro interno ocorreu. Foi encontrado um estado inconsistente."},
                {(uint) Codes.WaitedToLong, "Decorreu demasiado tempo de espera por um estado consistente."},
                {(uint) Codes.CommunicationError, "Foi detetado um erro interno de comunicação."},
                {(uint) Codes.UnknownError, "Ocorreu um erro interno mas de fonte desconhecida."},
                {(uint) Codes.Canceled, "A acão ou operação foi explicitamente interrompida."},
                {(uint) Codes.InvalidHandle, "A referência do recurso é inválida."},
                {(uint) Codes.InvalidParameter, "Um ou mais parâmetros indicados são inválidos e não é possível a sua interpretação."},
                {(uint) Codes.InvalidTarget, "A informação de configuração (Registry) é inválida."},
                {(uint) Codes.NoMemory, "Não existe memória suficiente para completar o comando."},
                {(uint) Codes.InsuficientBuffer, "O buffer configurado para recepção de dados é demasiado pequeno para os adas recebidos."},
                {(uint) Codes.UnknownReader, "O nome do leitor de cartões é desconhecido."},
                {(uint) Codes.TimeOut, "O timeou configurado para a operação foi alcançado."},
                {(uint) Codes.SharingViolation, "Não é possível aceder cartão porque existem outras coneções a aceder ao mesmo."},
                {(uint) Codes.NoSmartCard, "A operação requer um cartão que não se encontra disponível no dispositivo."},
                {(uint) Codes.UnknownCard, "O nome especificado para o cartão é desconhecido."},
                {(uint) Codes.CantDispose, "Não é possível dispor do cartão da forma pretendida."},
                {(uint) Codes.ProtocolMismatch, "Os protocolos configurados são incompatíveis com o protocolo configurado no cartão."},
                {(uint) Codes.NotReady, "O leitor ou o cartão não estão prontos a aceitar comandos."},
                {(uint) Codes.InvalidValue, "Um ou mais parâmetros indicados são inválidos e não podem ser interpretados."},
                {(uint) Codes.SystemCanceled, "A ação foi interrompida pelo sistema. Provavelmente através de um evento de shutdon ou logoff."},
                {(uint) Codes.InvalidATR, "O ATR obtido ou confugurado não é válido."},
                {(uint) Codes.NotInTransaction, "Não existe nehuma transação ativa para processar o comando."},
                {(uint) Codes.ReaderUnavailable, "O leitor especificado não está disponível para uso."},
                {(uint) Codes.Shutdown, "A operação foi abortada para permitir à aplicação sair."},
                {(uint) Codes.PCIToSmall, "O buffer PCI é demasiado pequeno."},
                {(uint) Codes.ReaderUnsupported, "O driver do leitor não cumpre os requisitos minimos para ser usado."},
                {(uint) Codes.DuplicateReader, "O driver do leitor não reproduz um nome único para o leitor."},
                {(uint) Codes.CardUnsupported, "O cartão não cumpre os requisitos minimos para ser suportado."},
                {(uint) Codes.NoService, "O gestor de cartões não se encontra em execução."},
                {(uint) Codes.ServiceStopped, "O gestor de cartões foi desligado."},
                {(uint) Codes.UnexpectedCardError, "Ocorreu um erro inesperado do cartão."},
                {(uint) Codes.ICCInstalation, "O circuito integrado do cartão não foi encontrado."},
                {(uint) Codes.ICCCreateOrder, "A ordem de criação dos objetos não é suportada."},
                {(uint) Codes.UnsupportedFeature, "O cartão não suporta a funcionalidade solicitada."},
                {(uint) Codes.DirectoryNotFound, "A diretoria pretendida não existe no cartão."},
                {(uint) Codes.FileNotFound, "O ficheiro pretendido não existe no cartão."},
                {(uint) Codes.NoDirectory, "O caminho especificado não represente uma directoria do cartão."},
                {(uint) Codes.NoFile, "O caminho especificado não represente uma ficheiro do cartão."},
                {(uint) Codes.AccessDenied, "O acesso ao ficheiro foi negado."},
                {(uint) Codes.NotEnoughMemoryInCard, "O cartão não dispõe de memória suficiente para armazenar a informação pretendida."},
                {(uint) Codes.BadSeek, "Ocorreu um erro ao definir o apontador para o ficheiro do cartão."},
                {(uint) Codes.InvalidPIN, "O PIN indicado é inválido."},
                {(uint) Codes.UnknownErrorCode, "Ocorreu um erro desconhecido originado por um componente da solução."},
                {(uint) Codes.NoSuchCertificate, "O certificado solicitado não existe."},
                {(uint) Codes.CertificateUnavailable, "O certificado solicitado não se encontra disponível."},
                {(uint) Codes.CannotFindCardReader, "Não foi possível encontrar nenhum leitor de cartões."},
                {(uint) Codes.CommunicationDataLost, "Ocorreu um erro de comunicações com o cartão. Reinicie a operação."},
                {(uint) Codes.NoKeyContainer, "O local indicado não existe no cartão."},
                {(uint) Codes.ServerToBusy, "O gestor do cartão está demasiado ocupado para completar a operação solicitada."},
                {(uint) Codes.PINCacheExpired, "A cache do PIN do cartão expirou."},
                {(uint) Codes.NoPINCache, "Não existe cache para o PIN do cartão."},
                {(uint) Codes.ReadOnlyCard, "O cartão é apenas de leitura."},
                {(uint) Codes.UnsupportedCard, "O cartão colocado no leitor não é suportado."},
                {(uint) Codes.UnresponsiveCard, "O cartão não responde a comandos de reiniciar."},
                {(uint) Codes.UnPoweredCard, "Não existe energia no cartão. Não é possível mais comunicação com o mesmo."},
                {(uint) Codes.ResetCard, "O cartão foi reiniciado. Qualquer estado é inválido."},
                {(uint) Codes.RemovedCard, "O cartão foi removido. Não é possível mais comunicação com o mesmo."},
                {(uint) Codes.SecurityViolation, "O acesso foi negado por existir uma violação dos requisitos de segurança."},
                {(uint) Codes.WrongPIN, "O PIN indicado é errado."},
                {(uint) Codes.PINBlocked, "O PIN do cartão encontra-se bloqueado. O número máximo de tentativas foi alcançado."},
                {(uint) Codes.EOF, "Foi encontrado o fim do ficheiro do cartão."},
                {(uint) Codes.CanceledByUser, "A operação foi cancelada pelo operador."},
                {(uint) Codes.NotAuthenticatedCard, "Não existe qualquer PIN definido no cartão."},
                {(uint) Codes.CacheItemNotFound, "O item solicitado não se encontra em cache."},
                {(uint) Codes.CacheItemStale, "O item da cache expirou."},
                {(uint) Codes.CacheItemToBig, "O item para colocar em cache é demasiado grande."}
            };
    }
}