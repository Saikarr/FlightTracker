using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab1;
public interface IReportable
{
    string Accept(IVisitor visitor);
}

