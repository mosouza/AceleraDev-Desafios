using System;

namespace Codenation.Challenge
{
    public class CesarCypher : ICrypt, IDecrypt
    {
        private const short numeroCasas = 3;
        private const short valorMinAsciiCode = 97;
        private const short valorMaxAsciiCode = 122;

        public string Crypt(string message)
        {
            // verifica valores incorretos na mensagem
            message = VerificaExcessoes(message);

            // retorna vazio quando nao tem mensagem
            if (message == string.Empty)
                return string.Empty;
            
            // cifra o texto
            int asciiCode;
            string codigoCifrado = string.Empty;

            foreach (char letra in message.ToCharArray())
            {
                if (char.IsLetter(letra))
                {
                    asciiCode = letra;

                    if (valorMaxAsciiCode < asciiCode + numeroCasas)
                    {
                        asciiCode = valorMinAsciiCode + Math.Abs(valorMaxAsciiCode - (asciiCode + numeroCasas));
                        asciiCode -= 1;
                    }
                    else
                    {
                        asciiCode += numeroCasas;
                    }

                    codigoCifrado += (char)asciiCode;
                }
                else
                {
                    codigoCifrado += letra;
                }
            }

            return codigoCifrado;
        }

        public string Decrypt(string cryptedMessage)
        {
            // verifica valores incorretos na mensagem
            cryptedMessage = VerificaExcessoes(cryptedMessage);

            // retorna vazio quando nao tem mensagem
            if (cryptedMessage == string.Empty)
            {
                return string.Empty;
            }

            // decifra o texto
            int asciiCode;
            string codigoDecifrado = string.Empty;

            foreach (char letra in cryptedMessage.ToCharArray())
            {
                if (char.IsLetter(letra))
                {
                    asciiCode = letra;

                    if (valorMinAsciiCode > asciiCode - numeroCasas)
                    {
                        asciiCode = valorMaxAsciiCode - (valorMinAsciiCode - (asciiCode - numeroCasas));
                        asciiCode += 1;
                    }
                    else
                    {
                        asciiCode -= numeroCasas;
                    }

                    codigoDecifrado += (char)asciiCode;
                }
                else
                {
                    codigoDecifrado += letra;
                }
            }

            return codigoDecifrado;
        }

        private string VerificaExcessoes(string message)
        {
            // retorna excessao para valor null
            if (message == null)
            {
                throw new ArgumentNullException();
            }

            // retorna vazio quando nao tem mensagem
            if (message.Trim() == string.Empty)
            {
                return string.Empty;
            }

            // retorna excessao para cacteres especiais
            foreach (char letra in message.ToLower().ToCharArray())
            {
                if (char.IsWhiteSpace(letra) || char.IsDigit(letra))
                {
                    continue;
                }

                if (letra <= 96 || letra >= 123)
                {
                    throw new ArgumentOutOfRangeException();
                }
            }

            return message.ToLower();
        }
    }
}