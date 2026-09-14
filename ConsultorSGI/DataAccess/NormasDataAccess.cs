using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class NormasDataAccess
    {
        public async Task<List<Normas>> ObtenerTodaslasNormas()
        {
            try
            {
                using (var bd = new gestioni_consultorNetEntities())
                {
                   
                    var modelo = bd.Normas.Include("Numerales").ToList();
                    return modelo; ;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

    }
}
