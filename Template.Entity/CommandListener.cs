using Common.Helper.Core.CSharpExpand;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;

namespace Template.Entity
{
    public sealed class CommandListener : IObserver<DiagnosticListener>
    {
        private readonly DbCommandInterceptor _dbCommandInterceptor = null;
        
        public CommandListener(params string[] slaveConnectionString)
        {
            _dbCommandInterceptor = new DbCommandInterceptor(slaveConnectionString);
        }
        public void OnCompleted()
        {
            throw new NotImplementedException();
        }

        public void OnError(Exception error)
        {
            throw new NotImplementedException();
        }

        public void OnNext(DiagnosticListener listener)
        {
            if (listener.Name == DbLoggerCategory.Name)
            {
                listener.Subscribe(_dbCommandInterceptor);
            }
        }
    }
}
