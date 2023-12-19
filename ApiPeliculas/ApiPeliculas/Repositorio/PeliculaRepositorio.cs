using ApiPeliculas.Data;
using ApiPeliculas.Modelos;
using ApiPeliculas.Repositorio.IRepositorio;
using Microsoft.EntityFrameworkCore;
using System;

namespace ApiPeliculas.Repositorio
{
    public class PeliculaRepositorio : IPeliculaRepositorio
    {
        private readonly ApplicationDbContext _bd;

        public PeliculaRepositorio(ApplicationDbContext bd)
        {
            _bd = bd;
        }
        public bool ActualizarPelicula(Pelicula pelicula)
        {
            pelicula.FechaCreacion = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);

            _bd.Peliculas.Update(pelicula);
            return Guardar();
        }

        public bool BorrarPelicula(Pelicula pelicula)
        {
            _bd.Peliculas.Remove(pelicula);
            return Guardar();
        }

        public ICollection<Pelicula> BuscarPelicula(string nombre)
        {
            IQueryable<Pelicula> query = _bd.Peliculas;
            if (!string.IsNullOrEmpty(nombre))
            {
                query = query.Where(p => p.Nombre.Contains(nombre) || p.Descripcion.Contains(nombre));
            }
            return query.ToList();
        }

        public bool CrearPelicula(Pelicula pelicula)
        {
            pelicula.FechaCreacion = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc); ;
            _bd.Peliculas.Add(pelicula);
            return Guardar();
        }

        public bool ExistePelicula(string nombre)
        {
            bool valor = _bd.Peliculas.Any(c => c.Nombre.ToLower().Trim() == nombre.ToLower().Trim());
            return valor;
        }

        public bool ExistePelicula(int id)
        {
            return _bd.Peliculas.Any(c => c.Id == id);
        }

        public Pelicula GetPelicula(int peliculaId)
        {
            return _bd.Peliculas.FirstOrDefault(c => c.Id == peliculaId);
        }

        public ICollection<Pelicula> GetPeliculas()
        {
            return _bd.Peliculas.OrderBy(c => c.Nombre).ToList();
        }

        public ICollection<Pelicula> GetPeliculasEnCategoria(int categoriaId)
        {
            return _bd.Peliculas.Include(ca => ca.Categoria).Where(ca => ca.categoriaId == categoriaId).ToList();
        }

        public bool Guardar()
        {
            return _bd.SaveChanges() >= 0 ? true : false;
        }
    }
}
