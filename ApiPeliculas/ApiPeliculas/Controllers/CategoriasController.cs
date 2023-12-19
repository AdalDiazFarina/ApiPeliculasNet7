using ApiPeliculas.Modelos;
using ApiPeliculas.Modelos.Dtos;
using ApiPeliculas.Repositorio.IRepositorio;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApiPeliculas.Controllers
{
    [Authorize]
    [ApiController]
    /*[Route("api/[controller]")]*/
    [Route("api/categorias")]
    public class CategoriasController : ControllerBase
    {
        private readonly ICategoriaRepositorio _ctRepo;
        private readonly IMapper _mapper;

        public CategoriasController(ICategoriaRepositorio ctRepo, IMapper mapper)
        {
            _ctRepo = ctRepo;
            _mapper = mapper;
        }

        [AllowAnonymous]
        [HttpGet]
        /*[ResponseCache(Duration = 20)]*/
        /*[ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]*/
        [ResponseCache(CacheProfileName = "SegundosCachePorDefecto")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetCategorias() {
            var listaCategorias = _ctRepo.GetCategorias();
            var listaCategoriasDto = new List<CategoriaDto>();

            foreach (var item in listaCategorias) {
                listaCategoriasDto.Add(_mapper.Map<CategoriaDto>(item));    
            }

            return Ok(listaCategoriasDto);
        }

        [AllowAnonymous]
        [HttpGet("{categoriaId:int}", Name = "GetCategoria")]
        // [ResponseCache(Duration = 30)]
        [ResponseCache(CacheProfileName = "SegundosCachePorDefecto")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetCategoria(int categoriaId)
        {
            var itemCategoria = _ctRepo.GetCategoria(categoriaId);
            if (itemCategoria == null) return NotFound();
            var itemCategoriaDto = _mapper.Map<CategoriaDto>(itemCategoria);
            return Ok(itemCategoriaDto);
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        [ProducesResponseType(201, Type = typeof(CategoriaDto))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult CrearCategoria([FromBody] CrearCategoriaDto crearCategoriaDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            if (crearCategoriaDto == null) return BadRequest(ModelState);

            if (_ctRepo.ExisteCategoria(crearCategoriaDto.Nombre))
            {
                ModelState.AddModelError("", "La categoría ya existe");
                return StatusCode(404, ModelState);
            }

            var categoria = _mapper.Map<Categoria>(crearCategoriaDto);
            if (!_ctRepo.CrearCategoria(categoria))
            {
                ModelState.AddModelError("", "Algo salio mal guardando el registro");
                return StatusCode(500, ModelState);
            }

            return CreatedAtRoute("GetCategoria", new { categoriaId = categoria.Id }, categoria);
        }

        [Authorize(Roles = "admin")]
        [HttpPatch("{categoriaId:int}", Name = "ActualizarPatchCategoria")]
        [ProducesResponseType(201, Type = typeof(CategoriaDto))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult ActualizarPatchCategoria(int categoriaId, [FromBody] CategoriaDto categoriaDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            if (categoriaDto == null) return BadRequest(ModelState);

            var categoria = _mapper.Map<Categoria>(categoriaDto);
            if (!_ctRepo.ActualizarCategoria(categoria))
            {
                ModelState.AddModelError("", "Algo salio mal actualizando el registro");
                return StatusCode(500, ModelState);
            }

            return CreatedAtRoute("GetCategoria", new { categoriaId = categoria.Id }, categoria);
        }

        [Authorize(Roles = "admin")]
        [HttpDelete("{categoriaId:int}", Name = "BorrarCategoria")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult BorrarCategoria(int categoriaId)
        {
            if (!_ctRepo.ExisteCategoria(categoriaId)) return NotFound();

            var categoria = _ctRepo.GetCategoria(categoriaId);

            if (!_ctRepo.BorrarCategoria(categoria))
            {
                ModelState.AddModelError("", "Algo salio mal borrando el registro");
                return StatusCode(500, ModelState);
            }

            return NoContent();

        }
    }
}
