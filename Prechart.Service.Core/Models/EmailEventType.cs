using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prechart.Service.Core.Models;

public enum EmailEventType
{
    None = 0,
    CsvDocumentUpload,
    CsvDocumentProcess,
    XmlOnBoardingUpload,
    XmlOnBoardingProcess,
    UserLogin,
    UserRegister,
    Normal,
    Others,
}
