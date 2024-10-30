namespace ApiGenerosMusica.Models
{
    public class Generos
{
    public int Id { get; set; }
    public string NombreGenero { get; set; }
    public byte[] Imagen { get; set; } // Imagen en bytes
    public string ImagenBase64 { get; set; } // String de la imagen en base64
}

}
