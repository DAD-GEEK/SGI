namespace Across
{
    public static class Enumeraciones
    {
        public enum ResponseType
        {
            success,
            error
        }

        public enum EnumMesesDelAño
        {
            Enero = 1,
            Febrero = 2,
            Marzo = 3,
            Abril = 4,
            Mayo = 5,
            Junio = 6,
            Julio = 7,
            Agosto = 8,
            Septiembre = 9,
            Octubre = 10,
            Noviembre = 11,
            Diciembre = 12

        }

        public enum enumModalidades
        {
            Presencial,
            Virtual
        }

        public enum enumTipoDeCriterio
        {
            Norma,
            Resolución,
            Decreto
        }

        public enum enumRolesGestionIntegral
        {
            Administrador = 1,
            Asesor = 2
        }

        public enum enumTiposDeIncapacidad
        {
            Accidente_Trabajo = 1,
            Accidente_Comun = 2,
            Enfermedad_Laboral = 3,
            Enfermedad_General = 4
        }

        public enum enumTiposGlobalesDeIncapacidad
        {
            EG_AC,
            AT_EL
        }

        public enum enumTiposFondos
        {
            EPS,
            AFP,
            ARL
        }

        public enum enumMacroProcesos
        {
            Visionales = 1,
            Misionales = 2,
            Apoyo = 3
        }

        public enum enumExportacionPDF
        {
            PlanDeAuditoria
        }

        public enum enumOpcionesPDF
        {
            Horizontal,
            Vertical,
        }

        public enum enumTipoDeAccion
        {
            Crear,
            Editar,
            Eliminar
        }

        public enum enumSistemasDeGestion
        {
            PESV
        }

        public enum enumGenericos
        {
            General = 0
        }

        public enum enumEstadosAuditoriaProcesos
        {
            Pendiente,
            En_Proceso,
            Auditado
        }

        public enum enumCodigosAusentismoIndicadores
        {
            ausentismoPorCausaMedica,
            indiceDeFrecuenciaPorCausaMedica,
            indiceDeSeveridadPorCausaMedica,
            tasaDeAusentismoPorCausaMedica,
            incidenciaYPrevalenciaDeAusentismoPorCausaMedica,

            ausentismoPor_AT_EL,
            reporteEInvestigacionAT_EL,
            indiceDeFrecuenciaAT,
            indiceDeFrecuenciaEL,
            mortalidad,
            indiceDeSeveridadAT_EL,
            tasaDeAccidentalidad,
            incidenciaYPrevalenciaDeAccidentes,
            lesionesIncapacitantes,
        }

        public enum enumTiposDeGrafico
        {
            bar,
            line
        }

        public enum enumFrecuenciaMedicion
        {
            diario,
            mensual,
            anual
        }

        public static class CacheNames
        {
            public const string ObtenerSistemasDeGestion = "ObtenerSistemasDeGestion";
            public const string ObtenerSistemasDeGestionPorTercero = "ObtenerSistemasDeGestionPorTercero";
            public const string ObtenerTodasLasNormas = "ObtenerTodasLasNormas";
            public const string ObtenerTodosLosNumerales = "ObtenerTodosLosNumerales";
            public const string ObtenerTodosLosIndicadoresGenerales = "ObtenerTodosLosIndicadoresGenerales";
            public const string ObtenerIndicadoresPorEmpresa = "ObtenerIndicadoresPorEmpresa";
        }


    }
}
