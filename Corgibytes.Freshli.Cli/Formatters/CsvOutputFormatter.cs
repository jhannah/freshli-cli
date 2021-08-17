﻿using ServiceStack.Text;
using System.Collections.Generic;

namespace Corgibytes.Freshli.Cli.Formatters
{
    public class CsvOutputFormatter : OutputFormatter
    {
        public override FormatType Type => FormatType.Csv;

        protected override string Build<T>( T entity )
        {
            return this.Build<T>(new List<T>() { entity });
        }

        protected override string Build<T>( IList<T> entities )
        {
            return CsvSerializer.SerializeToCsv(entities);
        }
    }
}
