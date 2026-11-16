using System.Data;
public class PersonaDatos
{
    public void Insertar(Persona p)
    {
        Db.Ejecutar(@"INSERT INTO Persona (nombres, ap_paterno, ap_materno, ci, fecha_nacimiento, email)
                      VALUES (@nom, @pat, @mat, @ci, @nac, @mail)",
            Db.P("@nom", p.Nombres), Db.P("@pat", p.ApPaterno),
            Db.P("@mat", string.IsNullOrWhiteSpace(p.ApMaterno) ? null : p.ApMaterno),
            Db.P("@ci", p.Ci), Db.P("@nac", p.FechaNacimiento),
            Db.P("@mail", string.IsNullOrWhiteSpace(p.Email) ? null : p.Email));
    }
    public void Modificar(Persona p)
    {
        Db.Ejecutar(@"UPDATE Persona SET nombres=@nom, ap_paterno=@pat, ap_materno=@mat, ci=@ci,
                      fecha_nacimiento=@nac, email=@mail WHERE id_persona=@id",
            Db.P("@nom", p.Nombres), Db.P("@pat", p.ApPaterno),
            Db.P("@mat", string.IsNullOrWhiteSpace(p.ApMaterno) ? null : p.ApMaterno),
            Db.P("@ci", p.Ci), Db.P("@nac", p.FechaNacimiento),
            Db.P("@mail", string.IsNullOrWhiteSpace(p.Email) ? null : p.Email), Db.P("@id", p.IdPersona));
    }
    public void Eliminar(int id)
    {
        Db.Ejecutar("DELETE FROM Persona WHERE id_persona=@id", Db.P("@id", id));
    }
    public DataTable Listar()
    {
        return Db.Consultar(@"SELECT id_persona, nombres, ap_paterno, ap_materno, ci, fecha_nacimiento, email
                              FROM Persona WHERE ci <> '0000000' ORDER BY id_persona DESC");
    }
}
