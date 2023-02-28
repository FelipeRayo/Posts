using DataAccess;
using DataAccess.Data;
using System;
using System.Linq;

namespace Business
{
    public class BaseService<TEntity> where TEntity : class, new()
    {
        protected BaseModel<TEntity> _BaseModel;

        public BaseService(BaseModel<TEntity> baseModel)
        {
            _BaseModel = baseModel;
        }

        #region Repository


        /// <summary>
        /// Consulta todas las entidades
        /// </summary>
        public virtual IQueryable<TEntity> GetAll()
        {
            return _BaseModel.GetAll;
        }


        /// <summary>
        /// Consulta una entidad
        /// </summary>
        public virtual TEntity Get(object entityId)
        {
            return _BaseModel.FindById(entityId);
        }

        /// <summary>
        /// Crea un entidad (Guarda)
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public virtual TEntity Create(TEntity entity)
        {
            ValidateCustomer(entity);
            ValidatePost(entity);

            return _BaseModel.Create(entity);
        }

        /// <summary>
        /// Crea un entidad (Guarda)
        /// </summary>
        /// <param name="entityList"></param>
        /// <returns></returns>
        public virtual IQueryable<TEntity> CreateMany(IQueryable<TEntity> entityList)
        {

            return _BaseModel.CreateMany(entityList);
        }


        /// <summary>
        /// Actualiza la entidad (GUARDA)
        /// </summary>
        /// <param name="editedEntity">Entidad editada</param>
        /// <param name="originalEntity">Entidad Original sin cambios</param>
        /// <param name="changed">Indica si se modifico la entidad</param>
        /// <returns></returns>
        public virtual TEntity Update(object id, TEntity editedEntity, out bool changed)
        {
            TEntity originalEntity = _BaseModel.FindById(id);
            return _BaseModel.Update(editedEntity, originalEntity, out changed);
        }


        /// <summary>
        /// Elimina una entidad (Guarda)
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public virtual TEntity Delete(TEntity entity)
        {
            if (entity.GetType() == typeof(Customer)){
                var posts = _BaseModel.GetPostsByCustomer((int)GetPropertyValue(entity, "CustomerId"));
                _BaseModel.DeletePostsByCustomer(posts);
            }

            return _BaseModel.Delete(entity);
        }

        /// <summary>
        /// Guardar cambios
        /// </summary>
        public virtual void SaveChanges()
        {
            _BaseModel.SaveChanges();
        }

        private void ValidateCustomer(TEntity entity)
        {
            if (entity.GetType() == typeof(Customer))
            {
                var customerId = GetPropertyValue(entity, "CustomerId");
                var nameCustomer = GetPropertyValue(entity, "Name");

                var customer = _BaseModel.FindById(customerId);

                if (customer != null && customer.Equals(nameCustomer))
                {
                    throw new Exception("Usuario ya registrado");
                }
            }
        }

        private void ValidatePost(TEntity entity)
        {
            if (entity.GetType() == typeof(Post))
            {
                var customerId = GetPropertyValue(entity, "CustomerId");
                _ = _BaseModel.FindCustomerById((int)customerId) ?? throw new Exception("Usuario no existe");
                var body = (string)GetPropertyValue(entity, "Body");

                if (body != null && body.Length > 97)
                {
                    body = body.Substring(0, 97) + "...";
                }

                var typePost = (int)GetPropertyValue(entity, "Type");
                var type = entity.GetType();
                var category = type.GetProperty("Category");


                switch (typePost)
                {
                    case 1:
                        category.SetValue(type, "Farándula");

                        break;

                    case 2:
                        category.SetValue(type, "Política");

                        break;

                    case 3:
                        category.SetValue(type, "Futbol");
                        break;
                }
            }
        }


        private object GetPropertyValue(TEntity entity, string propertyName)
        {
            var type = entity.GetType();
            var propertyInfo = type.GetProperty(propertyName);

            return propertyInfo.GetValue(entity);
        }
        #endregion


    }
}
