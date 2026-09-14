using Across.ArchivosDeRecurso;
using CoreBusiness.Interfaces;
using DataAccess;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace CoreBusiness
{
    public class ElementosComunesCoreBusiness : CRUDGenerico<ElementosComunes>, IElementosComunesCoreBusiness
    {
        private IntPtr nativeResource = Marshal.AllocHGlobal(100);
        private INormasCoreBusiness _iNormasCoreBusiness;
        private IElementosComunes_NormasCoreBusiness _iElementosComunes_NormasCoreBusiness;
        public ElementosComunesCoreBusiness() : base(new gestioni_consultorNetEntities())
        {
            this._iNormasCoreBusiness = new NormasCoreBusiness();
            this._iElementosComunes_NormasCoreBusiness = new ElementosComunes_NormasCoreBusiness();
        }

        public new async Task<string> SaveEntityAsync(ElementosComunes entity)
        {
            try
            {
                await base.SaveEntityAsync(entity);
                return string.Empty;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public new async Task<string> CreateAsync(ElementosComunes entity)
        {
            try
            {
                entity.StrDescripcion = entity.StrDescripcion;
                entity.BitActivo = entity.BitActivo;
                await base.CreateAsync(entity);

                return string.Empty;

            }
            catch (Exception)
            {

                throw;
            }
        }

        public new async Task DeleteAsync(ElementosComunes entity)
        {
            try
            {
                await base.DeleteAsync(entity);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public new async Task DeleteRangeAsync(IEnumerable<ElementosComunes> entity)
        {
            try
            {
                await base.DeleteRangeAsync(entity);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public new async Task<bool> ExistAsync(Expression<Func<ElementosComunes, bool>> match)
        {
            try
            {
                return await base.ExistAsync(match);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public new async Task<ElementosComunes> FindAsync(Expression<Func<ElementosComunes, bool>> match)
        {
            try
            {
                return await base.FindAsync(match);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public new List<ElementosComunes> GetAll()
        {
            try
            {
                return base.GetAll().ToList();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public new async Task<List<ElementosComunes>> GetAllAsync()
        {
            try
            {
                var listaEntidad = await base.GetAllAsync();
                return listaEntidad.ToList();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public new async Task<string> UpdateAsync(ElementosComunes entity)
        {
            try
            {
                var ElementosComunes = await this.FindAsync(x => x.IntElementoComunID == entity.IntElementoComunID);

                var listaNormas = ElementosComunes.ElementosComunes_Normas.ToList();

                ElementosComunes.StrDescripcion = entity.StrDescripcion;
                ElementosComunes.BitActivo = entity.BitActivo;
                await base.UpdateAsync(ElementosComunes);

                await this.AsociarNormasAsync(listaNormas, entity);

                return string.Empty;

            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<string> SaveAllAsync(ElementosComunes model)
        {
            try
            {
                if (model.IntElementoComunID != 0)
                    return await this.UpdateAsync(model);

                return await this.CreateAsync(model);
            }
            catch (Exception)
            {

                throw;
            }
        }

        private async Task EliminarNormasAsociadasAsync(ElementosComunes requisitosYSoportes)
        {
            try
            {
                var listaNormas = requisitosYSoportes.ElementosComunes_Normas.ToList();

                if (listaNormas.Count() != 0)
                {
                    var listaNormasEnBD = await _iElementosComunes_NormasCoreBusiness.FindWhereAsync(x => x.IntElementoComunID == requisitosYSoportes.IntElementoComunID);
                    await _iElementosComunes_NormasCoreBusiness.DeleteRangeAsync(listaNormasEnBD);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        private async Task AsociarNormasAsync(List<ElementosComunes_Normas> listaNormasEnBaseDeDatos, ElementosComunes requisitosYSoportes)
        {
            try
            {
                var listaNormasSeleccionadas = requisitosYSoportes.ElementosComunes_Normas.ToList();
                var listaNormasParaEliminar = listaNormasEnBaseDeDatos.Where(x => !listaNormasSeleccionadas.Any(y => y.IntNormaID == x.IntNormaID)).ToList();

                foreach (var item in listaNormasParaEliminar)
                {
                    var modelo = await _iElementosComunes_NormasCoreBusiness.FindAsync(x => x.IntRegistroID == item.IntRegistroID);
                    if (modelo != null)
                        await _iElementosComunes_NormasCoreBusiness.DeleteAsync(modelo);
                }

                var listaNormasParaCrear = listaNormasSeleccionadas.Where(x => !listaNormasEnBaseDeDatos.Any(y => y.IntNormaID == x.IntNormaID)).ToList();

                foreach (var item in listaNormasParaCrear)
                {
                    await _iElementosComunes_NormasCoreBusiness.SaveEntityAsync(new ElementosComunes_Normas()
                    {
                        IntElementoComunID = requisitosYSoportes.IntElementoComunID,
                        IntNormaID = item.IntNormaID
                    });
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<SelectList> DropDownListElementosComunesAsync(string valorSeleccionado = null)
        {
            try
            {
                var entidad = (from c in await this.GetAllAsync()
                               orderby c.IntElementoComunID
                               select new { CodigoID = c.IntElementoComunID, Descripcion = $"{c.StrDescripcion}" });

                if (string.IsNullOrEmpty(valorSeleccionado))
                    return new SelectList(entidad, "CodigoID", "Descripcion");

                return new SelectList(entidad, "CodigoID", "Descripcion", valorSeleccionado);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<MultiSelectList> DropDownListMultipleNormasAsync(ElementosComunes ElementosComunes = null)
        {
            try
            {
                var listaNormas = await _iNormasCoreBusiness.FindWhereAsync(x => x.BitActivo == true);

                var listaNormasText = new List<string>();

                if (ElementosComunes != null)
                {
                    foreach (var item in ElementosComunes.ElementosComunes_Normas.ToList())
                        listaNormasText.Add(item.IntNormaID.ToString());
                }

                return new MultiSelectList(listaNormas, "IntNormaID", "StrDescripcion", listaNormasText);
            }
            catch (Exception)
            {

                throw;
            }
        }

        #region Dispose

        public new void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~ElementosComunesCoreBusiness()
        {
            Dispose(false);
        }

        protected new virtual void Dispose(bool disposing)
        {
            if (disposing)
            {


            }

            if (nativeResource != IntPtr.Zero)
            {
                Marshal.FreeHGlobal(nativeResource);
                nativeResource = IntPtr.Zero;
            }
        }


        #endregion Dispose
    }
}
