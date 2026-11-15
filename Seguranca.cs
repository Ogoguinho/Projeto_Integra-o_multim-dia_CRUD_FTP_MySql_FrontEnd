using System;
using System.Security.Cryptography;
using System.Text;

namespace Forms_c_
{
    // Hash de senha (PBKDF2 com sal) e tokens de sessão
    internal static class Seguranca
    {
        private const int Iteracoes = 100000;
        private const int TamanhoSal = 16;
        private const int TamanhoHash = 32;

        private static string _hashFalso;

        // Hash usado quando o e-mail não existe, para o tempo de resposta não revelar se a conta existe
        public static string HashFalso
        {
            get
            {
                if (_hashFalso == null) _hashFalso = HashSenha("senha-falsa-mogus");
                return _hashFalso;
            }
        }

        // Formato gravado no banco: pbkdf2$iteracoes$sal(base64)$hash(base64)
        public static string HashSenha(string senha)
        {
            byte[] sal = new byte[TamanhoSal];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(sal);
            }

            byte[] hash = Derivar(senha, sal, Iteracoes, TamanhoHash);
            return "pbkdf2$" + Iteracoes + "$" + Convert.ToBase64String(sal) + "$" + Convert.ToBase64String(hash);
        }

        public static bool VerificarSenha(string senha, string armazenado)
        {
            if (string.IsNullOrEmpty(armazenado)) return false;

            string[] partes = armazenado.Split('$');
            if (partes.Length != 4 || partes[0] != "pbkdf2") return false;

            int iteracoes;
            if (!int.TryParse(partes[1], out iteracoes) || iteracoes < 1) return false;

            byte[] sal;
            byte[] esperado;
            try
            {
                sal = Convert.FromBase64String(partes[2]);
                esperado = Convert.FromBase64String(partes[3]);
            }
            catch (FormatException)
            {
                return false;
            }

            byte[] calculado = Derivar(senha ?? "", sal, iteracoes, esperado.Length);
            return IgualTempoConstante(calculado, esperado);
        }

        // Token enviado ao navegador (64 caracteres hexadecimais)
        public static string NovoToken()
        {
            byte[] bytes = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(bytes);
            }
            return ParaHex(bytes);
        }

        // No banco só fica o hash do token: se a tabela vazar, os tokens não servem
        public static string HashToken(string token)
        {
            using (var sha = SHA256.Create())
            {
                return ParaHex(sha.ComputeHash(Encoding.UTF8.GetBytes(token ?? "")));
            }
        }

        private static byte[] Derivar(string senha, byte[] sal, int iteracoes, int tamanho)
        {
            using (var kdf = new Rfc2898DeriveBytes(senha, sal, iteracoes, HashAlgorithmName.SHA256))
            {
                return kdf.GetBytes(tamanho);
            }
        }

        private static bool IgualTempoConstante(byte[] a, byte[] b)
        {
            if (a.Length != b.Length) return false;
            int diferenca = 0;
            for (int i = 0; i < a.Length; i++) diferenca |= a[i] ^ b[i];
            return diferenca == 0;
        }

        private static string ParaHex(byte[] bytes)
        {
            var sb = new StringBuilder(bytes.Length * 2);
            foreach (byte b in bytes) sb.Append(b.ToString("x2"));
            return sb.ToString();
        }
    }
}
