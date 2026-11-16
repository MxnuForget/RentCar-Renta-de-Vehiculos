using System;
using System.Security.Cryptography;

// Las contraseñas NO se encriptan (eso es reversible): se guarda un HASH con sal.
// PBKDF2 (Rfc2898) + SHA-256 + sal aleatoria por usuario + 100.000 iteraciones.
// Formato guardado: iteraciones.sal.hash (Base64)  ->  cabe en VARCHAR(255)
public static class Seguridad
{
    private const int Iteraciones = 100000;
    private const int TamSal = 16;
    private const int TamHash = 32;

    public static string Hash(string clave)
    {
        byte[] sal = new byte[TamSal];
        using (RandomNumberGenerator rng = RandomNumberGenerator.Create()) { rng.GetBytes(sal); }
        byte[] h = Derivar(clave, sal, Iteraciones);
        return Iteraciones + "." + Convert.ToBase64String(sal) + "." + Convert.ToBase64String(h);
    }

    public static bool Verificar(string clave, string almacenado)
    {
        if (string.IsNullOrEmpty(almacenado)) return false;
        string[] p = almacenado.Split('.');
        if (p.Length != 3) return false;              // por ejemplo, una clave vieja en texto plano
        int it;
        if (!int.TryParse(p[0], out it)) return false;
        byte[] sal, esperado;
        try { sal = Convert.FromBase64String(p[1]); esperado = Convert.FromBase64String(p[2]); }
        catch (FormatException) { return false; }

        byte[] calculado = Derivar(clave, sal, it);
        int diff = calculado.Length ^ esperado.Length;        // comparación en tiempo constante
        for (int i = 0; i < calculado.Length && i < esperado.Length; i++)
            diff |= calculado[i] ^ esperado[i];
        return diff == 0;
    }

    // Política de contraseñas: mínimo 8 caracteres, con al menos una letra y un número
    public static void ValidarClave(string clave)
    {
        if (string.IsNullOrEmpty(clave) || clave.Length < 8)
            throw new Exception("La contraseña debe tener al menos 8 caracteres.");
        bool letra = false, numero = false;
        foreach (char c in clave)
        {
            if (char.IsLetter(c)) letra = true;
            else if (char.IsDigit(c)) numero = true;
        }
        if (!letra || !numero)
            throw new Exception("La contraseña debe incluir al menos una letra y un número.");
    }

    private static byte[] Derivar(string clave, byte[] sal, int iteraciones)
    {
        using (Rfc2898DeriveBytes kdf = new Rfc2898DeriveBytes(clave, sal, iteraciones, HashAlgorithmName.SHA256))
            return kdf.GetBytes(TamHash);
    }
}
