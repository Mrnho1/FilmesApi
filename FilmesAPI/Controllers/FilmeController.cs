using AutoMapper;
using FilmesAPI.Data;
using FilmesAPI.Data.Dtos;
using FilmesAPI.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using RouteAttribute = Microsoft.AspNetCore.Mvc.RouteAttribute;

namespace FilmesAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class FilmeController : ControllerBase
{

    private FilmeContext _context;
    private IMapper _mapper;

    public FilmeController(FilmeContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    /*recebe um filme como parametro
     * O filme vem através do corpo da requisição [FromBody]*/
    [HttpPost]
    public IActionResult AdicionaFilme( [FromBody] CreateFilmeDto filmeDto)
    {

        Filme filme = _mapper.Map<Filme>(filmeDto);
        _context.Filmes.Add(filme);
        _context.SaveChanges();
        return CreatedAtAction(nameof(RecuperaFilmePorId),new {id = filme.Id}, filme);
    }

    /* Método que irá listar os filmes armazenados pelo método AdicionaFilme */
    [HttpGet]
    public IEnumerable<ReadFilmeDto> RecuperaFilmes([FromQuery] int skip = 0 , [FromQuery] int take = 50)
    {
        return _mapper.Map<List<ReadFilmeDto>>(_context.Filmes.Skip(skip).Take(take));
    }

    [HttpGet("{id}")]
    public IActionResult RecuperaFilmePorId(int id)

    {
        /* Esse filme que estou buscando tenha o id igual o id que eu recebi via parametro
         * filme => filme.id == id*/
       var filme =  _context.Filmes.FirstOrDefault(filme => filme.Id == id);
        if(filme == null) return NotFound();
        var filmeDto = _mapper.Map<ReadFilmeDto>(filme);
        return Ok(filmeDto);
    }


    [HttpPut("{id}")]
    public IActionResult AtualizarFilme(int id, [FromBody] UpdateFilmeDto filmeDto)
    {
        var filme = _context.Filmes.FirstOrDefault(filme => filme.Id == id);
        if(filme == null) return NotFound();

        _mapper.Map(filmeDto, filme);
        _context.SaveChanges();
        return NoContent();
    }

    [HttpPatch("{id}")]
    public IActionResult AtualizarFilmePath(int id, JsonPatchDocument<UpdateFilmeDto> path)
    {
        var filme = _context.Filmes.FirstOrDefault(filme => filme.Id == id);
        if (filme == null) return NotFound();

        var movieToUpdate = _mapper.Map<UpdateFilmeDto>(filme);

        path.ApplyTo(movieToUpdate, ModelState);

        if (!TryValidateModel(movieToUpdate))
        {
            return ValidationProblem(ModelState);
        }


        _mapper.Map(movieToUpdate, filme);
        _context.SaveChanges();
        return NoContent();
    }


    [HttpDelete("{id}")]
    public IActionResult DeleteFilme(int id)
    {
        var filme = _context.Filmes.FirstOrDefault(filme => filme.Id == id);
        if (filme == null) return NotFound();
        _context.Remove(filme);
        _context.SaveChanges();
        return NoContent();
    }

}
