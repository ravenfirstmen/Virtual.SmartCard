using System;
using System.Collections.Generic;

namespace Virtual.SmartCard.PDU
{
    public static class PDUStatusCondition
    {
        public static SW1Qualification GetSW1Qualification(byte sw1)
        {
            switch (sw1)
            {
                case 0x90:
                case 0x61:
                    return SW1Qualification.Normal;
                case 0x62:
                case 0x63:
                    return SW1Qualification.Warning;
                case 0x64:
                case 0x65:
                case 0x66:
                    return SW1Qualification.ExecutionError;
                case 0x67:
                case 0x68:
                case 0x69:
                case 0x6A:
                case 0x6B:
                case 0x6C:
                case 0x6D:
                case 0x6E:
                case 0x6F:
                    return SW1Qualification.ChecksumError;
                default:
                    return SW1Qualification.Unknown;
            }
        }

        // ver iso 7816-4: 5.3.5
        public static string GetStatusConditionMessage(byte sw1, byte sw2)
        {
            if (sw1 == 0x90 && sw2 == 0x00)
            {
                return String.Format("{0}: Comando executado com sucesso", GetSW1Qualification(sw1));
            }
            if (sw1 == 0x61)
            {
                return String.Format("{0}: Estão disponíveis {1} bytes", GetSW1Qualification(sw1), sw2);
            }
            if (sw1 == 0x62 && (sw2 >= 0x02 && sw2 <= 0x80))
            {
                return String.Format("{0}: Faltam ler {1} bytess", GetSW1Qualification(sw1), sw2);
            }
            if (sw1 == 0x63 && ((sw2 & 0XC0) == 0xC0))
            {
                return String.Format("{0}: Contador {1}", GetSW1Qualification(sw1), sw2 >> 8 & 0x0F);
            }
            if (sw1 == 0x64 && (sw2 >= 0x02 && sw2 <= 0x80))
            {
                return String.Format("{0}: Comando rejeitado. Faltam ler {1} bytess", GetSW1Qualification(sw1), sw2);
            }

            IDictionary<byte, string> messages = null;
            if (_statusConditionMessages.TryGetValue(sw1, out messages))
            {
                return String.Format("{0}: {1}", GetSW1Qualification(sw1),
                                     messages.ContainsKey(sw2) ? messages[sw2] : "Estado desconhecido");
            }

            return String.Format("{0}: Estado desconhecido", GetSW1Qualification(sw1));
        }

        public enum SW1Qualification : byte
        {
            Unknown,
            Normal,
            Warning,
            ExecutionError,
            ChecksumError
        }

        // caso especiais 90xx, 61xx, 63Cx..., 62[02-80]
        private static IDictionary<byte, IDictionary<byte, string>> _statusConditionMessages
            = new Dictionary<byte, IDictionary<byte, string>>
                {
                    {
                        0x62, new Dictionary<byte, string>
                            {
                                {0x00, "Sem informação"},
                                //  02-80 -> iso 7816-4: 9.1.1
                                {0x81, "Parte dos dados podem estar corrompidos"},
                                {0x82, "Fim do ficheiro ou registo antes de ler os Le bytes"},
                                {0x83, "Selecção do ficheiro inválida"},
                                {0x84, "Ficheiro de controlo não formatado com as regras ISO"},
                            }
                    },

                    {
                        0x63, new Dictionary<byte, string>
                            {
                                {0x00, "Sem informação"},
                                {0x81, "Ficheiro cheio"},
                                //  CX -> iso 7816-4: 5.3.5
                            }
                    },

                    {
                        0x64, new Dictionary<byte, string>
                            {
                                {0x00, "Erro na execução do comando"},
                                {0x01, "Resposta imediata retornada pelo cartão"},
                                //  02-80 -> iso 7816-4: 9.1.1
                            }
                    },

                    {
                        0x65, new Dictionary<byte, string>
                            {
                                {0x00, "Sem informação"},
                                {0x81, "Erro de memória"},
                            }
                    },

                    {
                        0x68, new Dictionary<byte, string>
                            {
                                {0x00, "Sem informação"},
                                {0x81, "Canal lógico não suportado"},
                                {0x82, "Mensagens seguras não suportadas"},
                                {0x83, "Esperado o último comando na cadeia de comandos"},
                                {0x84, "Encadeamento de comandos não suportado"},
                            }
                    },

                    {
                        0x69, new Dictionary<byte, string>
                            {
                                {0x00, "Sem informação"},
                                {0x81, "Comando incompatível com a estrutura do ficheiro"},
                                {0x82, "Controlo de segurança não satizfeito"},
                                {0x83, "Autenticação bloqueada"},
                                {0x84, "Dados referenciados inválidos"},
                                {0x85, "Condições de uso não satizfeitas"},
                                {0x86, "Comando inválido ou não permitido (não existe EF)"},
                                {0x87, "Esperados objectos de segurança"},
                                {0x88, "Objectos de segurança inválidos"},
                            }
                    },

                    {
                        0x6A, new Dictionary<byte, string>
                            {
                                {0x00, "Sem informação"},
                                {0x80, "Parâmetros incorretos"},
                                {0x81, "Função não suportada"},
                                {0x82, "Ficheiro não encontrado"},
                                {0x83, "Registo não encontrado"},
                                {0x84, "Não existe suficiente espaço no ficheiro"},
                                {0x85, "Lc inconsistente com a estrutura TLV"},
                                {0x86, "Parâmetros incorretos em P1-P2"},
                                {0x87, "Lc inconsistente com os parâmetros P1-P2"},
                                {0x88, "Dados referenciados não encontrados"},
                                {0x89, "Ficheiro já existe"},
                                {0x8A, "DF já existe"},
                            }
                    },
                };
    }
}