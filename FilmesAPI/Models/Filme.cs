using System.ComponentModel.DataAnnotations;

namespace FilmesAPI.Models;

public class Filme
{
    [Key]
    [Required]
    public int Id { get; set; }

    [Required (ErrorMessage = "O título do filme é obrigatório!")]
    public string Titulo { get; set; }


    [Required (ErrorMessage = "O gênero do filme é obrigatório!")]
    [MaxLength(50 , ErrorMessage = "O tamanho do gênero não pode exceder 50 caracteres")]
    public string Genero { get; set; }

    [Required]
    [Range(70, 600, ErrorMessage = "O filme dever ter no mínimo 70 minutos de duração e no máximo 600 minutos de duração")]
    public int Duracao { get; set; }
    
}
