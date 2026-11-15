using System;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Forms_c_
{
    // Validações usadas pelo servidor HTTP e pelas telas do CRUD
    public static class Validacao
    {
        public static string SomenteDigitos(string texto)
        {
            if (string.IsNullOrEmpty(texto)) return "";
            var sb = new StringBuilder();
            foreach (char c in texto)
            {
                if (c >= '0' && c <= '9') sb.Append(c);
            }
            return sb.ToString();
        }

        public static bool EmailValido(string email)
        {
            if (string.IsNullOrWhiteSpace(email) || email.Trim().Length > 100) return false;
            return Regex.IsMatch(email.Trim(), @"^[^@\s]+@[^@\s]+\.[^@\s]{2,}$");
        }

        // 10 dígitos (fixo) ou 11 (celular), com DDD
        public static bool TelefoneValido(string telefone)
        {
            string d = SomenteDigitos(telefone);
            if (d.Length != 10 && d.Length != 11) return false;
            return d[0] != '0';
        }

        public static bool SenhaValida(string senha)
        {
            return senha != null && senha.Length >= 6 && senha.Length <= 128;
        }

        public static bool CepValido(string cep)
        {
            return SomenteDigitos(cep).Length == 8;
        }

        // Confere os dois dígitos verificadores
        public static bool CpfValido(string cpf)
        {
            string d = SomenteDigitos(cpf);
            if (d.Length != 11 || d.Distinct().Count() == 1) return false;
            return DigitoCpf(d, 9) == d[9] - '0' && DigitoCpf(d, 10) == d[10] - '0';
        }

        private static int DigitoCpf(string d, int quantidade)
        {
            int soma = 0;
            for (int i = 0; i < quantidade; i++)
            {
                soma += (d[i] - '0') * (quantidade + 1 - i);
            }
            int resto = (soma * 10) % 11;
            return resto == 10 ? 0 : resto;
        }

        // Algoritmo de Luhn para número de cartão
        public static bool LuhnValido(string numero)
        {
            string d = SomenteDigitos(numero);
            if (d.Length < 13 || d.Length > 19) return false;

            int soma = 0;
            bool dobrar = false;
            for (int i = d.Length - 1; i >= 0; i--)
            {
                int n = d[i] - '0';
                if (dobrar)
                {
                    n *= 2;
                    if (n > 9) n -= 9;
                }
                soma += n;
                dobrar = !dobrar;
            }
            return soma % 10 == 0;
        }

        public static string Bandeira(string numero)
        {
            string d = SomenteDigitos(numero);
            if (Regex.IsMatch(d, @"^(4011|4312|4389|4514|4576|5041|5066|5067|509|6277|6362|6363|650|6516|6550)")) return "Elo";
            if (Regex.IsMatch(d, @"^(606282|3841)")) return "Hipercard";
            if (d.StartsWith("4")) return "Visa";
            if (Regex.IsMatch(d, @"^(5[1-5]|222[1-9]|22[3-9]\d|2[3-6]\d\d|27[01]\d|2720)")) return "Mastercard";
            if (Regex.IsMatch(d, @"^3[47]")) return "Amex";
            if (Regex.IsMatch(d, @"^(30[0-5]|36|38)")) return "Diners";
            return "Outro";
        }

        // Validade no formato MM/AA e ainda não vencida
        public static bool ValidadeCartaoValida(string validade)
        {
            if (string.IsNullOrWhiteSpace(validade)) return false;
            Match m = Regex.Match(validade.Trim(), @"^(0[1-9]|1[0-2])/(\d{2})$");
            if (!m.Success) return false;

            int mes = int.Parse(m.Groups[1].Value);
            int ano = 2000 + int.Parse(m.Groups[2].Value);
            return new DateTime(ano, mes, 1).AddMonths(1) > DateTime.Today;
        }
    }
}
