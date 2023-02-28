﻿using DataAccess.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace DataAccess
{
    public class BaseModel<TEntity> where TEntity : class, new()
    {
        /// <summary>
        /// Contexto
        /// </summary>
        JujuTestContext _context;
        /// <summary>
        /// Entidad
        /// </summary>
        protected DbSet<TEntity> _dbSet;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="context"></param>
        public BaseModel(JujuTestContext context)
        {
            _context = context;
            _dbSet = _context.Set<TEntity>();
        }


        /// <summary>
        /// Consulta todas las entidades
        /// </summary>
        public virtual IQueryable<TEntity> GetAll
        {
            get { return _dbSet; }
        }

        /// <summary>
        /// Consulta una entidad por id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual TEntity FindById(object id)
        {
            return _dbSet.Find(id);
        }

        /// <summary>
        /// Consulta si el usuario asociado al post existe
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual Customer FindCustomerById(int id)
        {
            return (from customer in _context.Customer
                    where customer.CustomerId == id
                    select customer).FirstOrDefault();
        }

        /// <summary>
        /// Consulta posts asociados a un customer
        /// </summary>
        /// 
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual IQueryable<Post> GetPostsByCustomer(int id)
        {
            return (from post in _context.Post
                    where post.CustomerId == id
                    select post).ToList().AsQueryable();
        }

        /// <summary>
        /// Elimina posts asociados a un customer
        /// </summary>
        /// 
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual void DeletePostsByCustomer(IQueryable posts)
        {
            foreach (var post in posts)
            {
                _context.Post.Remove((Post)post);
                _context.SaveChanges();
            }
        }

        /// <summary>
        /// Crea un entidad (Guarda)
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public virtual TEntity Create(TEntity entity)
        {
            _dbSet.Add(entity);
            _context.SaveChanges();

            return entity;
        }

        /// <summary>
        /// Crea varias entidades (Guarda)
        /// </summary>
        /// <param name="entityList"></param>
        /// <returns></returns>
        public virtual IQueryable<TEntity> CreateMany(IQueryable<TEntity> entityList)
        {
            foreach (var entity in entityList)
            {
                _dbSet.Add(entity);
                _context.SaveChanges();
            }

            return entityList;
        }


        /// <summary>
        /// Actualiza la entidad (GUARDA)
        /// </summary>
        /// <param name="editedEntity">Entidad editada</param>
        /// <param name="originalEntity">Entidad Original sin cambios</param>
        /// <param name="changed">Indica si se modifico la entidad</param>
        /// <returns></returns>
        public virtual TEntity Update(TEntity editedEntity, TEntity originalEntity, out bool changed)
        {

            _context.Entry(originalEntity).CurrentValues.SetValues(editedEntity);

            changed = _context.Entry(originalEntity).State == EntityState.Modified;

            _context.SaveChanges();

            return originalEntity;
        }



        /// <summary>
        /// Elimina una entidad (Guarda)
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public virtual TEntity Delete(TEntity entity)
        {
            _dbSet.Remove(entity);

            _context.SaveChanges();

            return entity;
        }


        /// <summary>
        /// Guardar cambios
        /// </summary>
        public virtual void SaveChanges()
        {
            _context.SaveChanges();
        }
    }
}
