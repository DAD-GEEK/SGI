using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models;

namespace DataAccess
{
    public class UnitOfWorkh
    {
        public readonly gestioni_consultorNetEntities context = new gestioni_consultorNetEntities();

        private CRUDGenerico<Empleados> _CRUDGenericoEmpleados;
        private CRUDGenerico<Terceros> _CRUDGenericoTerceros;
        private CRUDGenerico<AspNetTerceroRoles> _CRUDGenericoAspNetTerceroRoles;

        #region Constructores
        public CRUDGenerico<Empleados> EmpleadoCRUD
        {
            get
            {
                if (this._CRUDGenericoEmpleados == null)
                    this._CRUDGenericoEmpleados = new CRUDGenerico<Empleados>(context);
                return _CRUDGenericoEmpleados;

            }

        }

        public CRUDGenerico<Terceros> TercerosCRUD
        {
            get
            {
                if (this._CRUDGenericoTerceros == null)
                    this._CRUDGenericoTerceros = new CRUDGenerico<Terceros>(context);
                return _CRUDGenericoTerceros;
            }

        }

        public CRUDGenerico<AspNetTerceroRoles> AspNetTerceroRoles
        {
            get
            {
                if (this._CRUDGenericoAspNetTerceroRoles == null)
                    this._CRUDGenericoAspNetTerceroRoles = new CRUDGenerico<AspNetTerceroRoles>(context);
                return _CRUDGenericoAspNetTerceroRoles;
            }

        }

        #endregion
    }
}
